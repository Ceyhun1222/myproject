using Aran.AranEnvironment;
using Europe_ICAO015;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICAO015
{
    public partial class ObstacleReport : Form
    {
        private AranTool _aranToolItem;
        //private bool _pointPickerClicked;
        private int _handleNavaid;
        public ObstacleReport()
        {
            //InitializeComponent();

            UseWaitCursor = true;

            InitializeComponent();

            PopulateTree(triStateTreeView1.Nodes, "");

            UseWaitCursor = false;

            foreach (TreeNode nodeone in triStateTreeView1.Nodes)
            {
                TreeViewClass.HideCheckBox(triStateTreeView1, nodeone);

                if (nodeone.Text == "VOR" || nodeone.Text == "DME" || nodeone.Text == "ILS")
                {
                    foreach (TreeNode node2 in nodeone.Nodes)
                    {
                        TreeViewClass.HideCheckBox(triStateTreeView1, node2);
                    }
                }
            }
        }
        public ObstacleReport(AranTool arantoolitem) : this()
        {
            _aranToolItem = arantoolitem;
            //_aranToolItem.MouseClickedOnMap += OnMouseClickedOnMap;
        }

        //DMEN {
        public static List<ReportForDme300r> ListForDMEN300r = new List<ReportForDme300r>();
        public static List<ReportForDme3000r> ListForDMEN3000r = new List<ReportForDme3000r>();
        //DMEN }
        //DVOR {
        public static bool windturbine;
        public static List<ReportForDVOR600R> ListforDvor600 = new List<ReportForDVOR600R>();
        public static List<ReportForDVOR3000R> ListforDvor3000 = new List<ReportForDVOR3000R>();
        public static List<ReportForDVOR10000R> ListforDvor10000 = new List<ReportForDVOR10000R>();
        public static List<ReportDvorForWindTurbine> listforDvorWindTurbine = new List<ReportDvorForWindTurbine>();
        //DVOR }
        //CVOR {
        public static List<ReportForCVOR600r> ListforCvor600 = new List<ReportForCVOR600r>();
        public static List<ReportForCVOR3000r> ListforCvor3000 = new List<ReportForCVOR3000r>();
        public static List<ReportForCVOR15000r> ListforCvor15000 = new List<ReportForCVOR15000r>();
        public static List<ReportCvorForWindTurbine> listforCvorWindTurbine = new List<ReportCvorForWindTurbine>();
        //CVOR }
        //Markers {
        public static List<ReportForMarkers50r> ListForMarkers50r = new List<ReportForMarkers50r>();
        public static List<ReportForMarkers200r> ListForMarker200r = new List<ReportForMarkers200r>();
        //Markers }
        //NDB {
        public static List<ReportForNDB200r> ListForNDB200r = new List<ReportForNDB200r>();
        public static List<ReportForNDB1000r> ListForNDB1000r = new List<ReportForNDB1000r>();
        //NDB }
        //HarmonisedGuidanceReport {
        public static List<Lists_FOR_2DGraphics> List_2DGrpahics_Calculation = new List<Lists_FOR_2DGraphics>();
        //HarmonisedGuidanceReport }
        DataTable tableforILS = new DataTable();
        DataTable tableofDME = new DataTable();
        public static List<AddCheckedNavaids> AddcheckedChildParent = new List<AddCheckedNavaids>(); //TreeView add Navaid
        private void triStateTreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // A node in the tree has been selected
            TreeView tv = sender as TreeView;
            tv.UseWaitCursor = true;

            if ((e.Node.Nodes.Count == 1) && (e.Node.Nodes[0].Text == ""))
            {
                // This is a 'dummy' node.  Replace it with actual data
                e.Node.Nodes.RemoveAt(0);
                PopulateTree(e.Node.Nodes, e.Node.Text);
            }

            tv.UseWaitCursor = false;
        }

        private void PopulateTree(TreeNodeCollection ParentNodes, string PreText)
        {
            //string parentname = "";
            // TreeNodeCollection col = triStateTreeView1.Nodes;

            //List<AddCheckedNavaids> ListSort = AddcheckedChildParent.OrderBy(name => name.SecondParentName).ToList();

            int dme = 0;
            int ils = 0;
            for (int i = 0; i < AddcheckedChildParent.Count; i++)
            {
                if (AddcheckedChildParent[i].FirstParentName != null)
                {
                    if (AddcheckedChildParent[i].FirstParentName == "ILS")
                    {

                        int r = 0;
                        //TreeNode tn = new TreeNode();

                        foreach (TreeNode node in triStateTreeView1.Nodes)
                        {
                            if (node.Text == AddcheckedChildParent[i].FirstParentName.ToString())
                            {
                                r = 1;
                            }
                        }

                        if (r == 0)
                        {
                            triStateTreeView1.Nodes.Add(AddcheckedChildParent[i].FirstParentName.ToString(), AddcheckedChildParent[i].FirstParentName.ToString());
                        }


                        int second = 0;

                        foreach (TreeNode node in triStateTreeView1.Nodes)
                        {
                            foreach (TreeNode node2 in node.Nodes)
                            {
                                if (node2.Text == AddcheckedChildParent[i].SecondParentName.ToString())
                                {
                                    second = 1;
                                }
                            }
                        }

                        if (second == 0)
                        {
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName.ToString()].Nodes.Add(AddcheckedChildParent[i].SecondParentName.ToString(), AddcheckedChildParent[i].SecondParentName.ToString());
                        }

                        ils++;
                        if (ils == 1)
                        {
                            tableforILS.Columns.Add("TypeOFNAvigation");
                            tableforILS.Columns.Add("NavaidName");
                            tableforILS.Columns.Add("ID");
                            tableforILS.Columns.Add("Obstacle");
                            tableforILS.Columns.Add("Elevation");
                            tableforILS.Columns.Add("Distance(meter)");
                            tableforILS.Columns.Add("Penetrate");
                            tableforILS.Columns.Add("GeoType");
                            tableforILS.Columns.Add("PolygonType");
                        }

                        //TreeNode tn = new TreeNode(AddcheckedChildParent[i].FirstParentName.ToString());
                        //triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes.Add(AddcheckedChildParent[i].SecondParentName.ToString(), AddcheckedChildParent[i].SecondParentName.ToString());
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes.Add(AddcheckedChildParent[i].ChildName.ToString(), AddcheckedChildParent[i].ChildName.ToString());
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Polygon Segment", "Polygon Segment").ForeColor = Color.SaddleBrown;
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("First Corner Polygon", "First Corner Polygon").ForeColor = Color.Red;
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Second Corner Polygon", "Second Corner Polygon").ForeColor = Color.MediumAquamarine;
                        //ParentNodes.Add(tn);
                    }
                    if (AddcheckedChildParent[i].FirstParentName != "ILS")
                    {
                        int r = 0;
                        //TreeNode tn = new TreeNode();

                        foreach (TreeNode node in triStateTreeView1.Nodes)
                        {
                            if (node.Text == AddcheckedChildParent[i].FirstParentName.ToString())
                            {
                                r = 1;
                            }
                        }

                        if (r == 0)
                        {
                            triStateTreeView1.Nodes.Add(AddcheckedChildParent[i].FirstParentName.ToString(), AddcheckedChildParent[i].FirstParentName.ToString());
                        }

                        int second = 0;

                        foreach (TreeNode node in triStateTreeView1.Nodes)
                        {
                            foreach (TreeNode node2 in node.Nodes)
                            {
                                if (node2.Text == AddcheckedChildParent[i].SecondParentName.ToString())
                                {
                                    second = 1;
                                }
                            }
                        }

                        if (second == 0)
                        {
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName.ToString()].Nodes.Add(AddcheckedChildParent[i].SecondParentName.ToString(), AddcheckedChildParent[i].SecondParentName.ToString());
                        }

                        dme++;
                        if (dme == 1)
                        {
                            tableofDME.Columns.Add("TypeOfNavigation");
                            tableofDME.Columns.Add("NavaidName");
                            tableofDME.Columns.Add("ID");
                            tableofDME.Columns.Add("Radius");
                            tableofDME.Columns.Add("Obstacle");
                            tableofDME.Columns.Add("Elevation");
                            tableofDME.Columns.Add("Distance(meter)");
                            tableofDME.Columns.Add("Penetrate");
                            tableofDME.Columns.Add("GeoType");
                        }

                        //TreeNode tn = new TreeNode(AddcheckedChildParent[i].FirstParentName.ToString());
                        //triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes.Add(AddcheckedChildParent[i].SecondParentName.ToString(), AddcheckedChildParent[i].SecondParentName.ToString());
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes.Add(AddcheckedChildParent[i].ChildName.ToString(), AddcheckedChildParent[i].ChildName.ToString());

                        if (AddcheckedChildParent[i].SecondParentName.ToString() == "DVOR")
                        {
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 600", "Radius for 600");
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 3000", "Radius for 3000");
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 10000", "Radius for 10000");
                            if (windturbine == true)
                            {
                                triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Wind Turbine", "Wind Turbine");
                            }
                        }
                        if (AddcheckedChildParent[i].SecondParentName.ToString() == "DMEN")
                        {
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 300", "Radius for 300");
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 3000", "Radius for 3000");
                        }
                        if (AddcheckedChildParent[i].SecondParentName.ToString() == "CVOR")
                        {
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 600", "Radius for 600");
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 3000", "Radius for 3000");
                            triStateTreeView1.Nodes[AddcheckedChildParent[i].FirstParentName].Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 15000", "Radius for 15000");
                        }
                        //ParentNodes.Add(tn);

                    }
                }
                if (AddcheckedChildParent[i].FirstParentName == null)
                {
                    int r = 0;
                    //TreeNode tn = new TreeNode();

                    foreach (TreeNode node in triStateTreeView1.Nodes)
                    {
                        if (node.Text == AddcheckedChildParent[i].SecondParentName.ToString())
                        {
                            r = 1;
                        }
                    }

                    if (r == 0)
                    {
                        triStateTreeView1.Nodes.Add(AddcheckedChildParent[i].SecondParentName.ToString(), AddcheckedChildParent[i].SecondParentName.ToString());
                    }

                    dme++;
                    if (dme == 1)
                    {
                        tableofDME.Columns.Add("TypeOfNavigation");
                        tableofDME.Columns.Add("NavaidName");
                        tableofDME.Columns.Add("ID");
                        tableofDME.Columns.Add("Radius");
                        tableofDME.Columns.Add("Obstacle");
                        tableofDME.Columns.Add("Elevation");
                        tableofDME.Columns.Add("Distance(meter)");
                        tableofDME.Columns.Add("Penetrate");
                        tableofDME.Columns.Add("GeoType");
                        //tn.Nodes.Add(AddcheckedChildParent[i].SecondParentName.ToString(), AddcheckedChildParent[i].SecondParentName.ToString());
                        //ParentNodes = triStateTreeView1.Nodes;
                    }





                    //parentname = AddcheckedChildParent[i].SecondParentName.ToString();



                    triStateTreeView1.Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes.Add(AddcheckedChildParent[i].ChildName.ToString(), AddcheckedChildParent[i].ChildName.ToString());

                    if (AddcheckedChildParent[i].SecondParentName.ToString() == "NDB")
                    {
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 200", "Radius for 200");
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 1000", "Radius for 1000");
                    }
                    if (AddcheckedChildParent[i].SecondParentName.ToString() == "Markers")
                    {
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 50", "Radius for 50");
                        triStateTreeView1.Nodes[AddcheckedChildParent[i].SecondParentName.ToString()].Nodes[AddcheckedChildParent[i].ChildName.ToString()].Nodes.Add("Radius for 200", "Radius for 200");
                    }


                }
            }

        }
        ObstacleReport_Add_Or_Remove_From_Grid ObtsacleReportAddOrRemovefromGrid = new ObstacleReport_Add_Or_Remove_From_Grid();

        private void triStateTreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode ENODE = e.Node;

            if (ENODE.Checked == true)
            {
                if (ENODE.Level == 3)
                {

                    //VOR or DME or ILS StepbyStep Check
                    TreeNode thirdparent = ENODE.Parent;
                    TreeNode secondparent = thirdparent.Parent;
                    TreeNode firstparent = secondparent.Parent;
                    if (firstparent.Text == "ILS")
                    {
                        TabPage tab = tabControl1.TabPages[1];
                        tabControl1.SelectTab(tab);
                        ObtsacleReportAddOrRemovefromGrid.ILS_StepbyStep_Check_Show_ObstacleReportAdd(GridObstclDataForILS, tableforILS, ENODE, secondparent, thirdparent, List_2DGrpahics_Calculation);
                    }
                    else
                    {
                        //TabPage tab = tabControl1.TabPages[0];
                        //tabControl1.SelectTab(tab);

                        ObtsacleReportAddOrRemovefromGrid.VORORDME_StepbyStep_Check_Show_ObstacleReportAdd(GridObstclDataForDME, tableofDME, ENODE, secondparent, ListforDvor600, ListforDvor3000, ListforDvor10000, listforDvorWindTurbine,
                            ListforCvor600, ListforCvor3000, ListforCvor15000, listforCvorWindTurbine, ListForDMEN300r, ListForDMEN3000r);

                    }
                }
                if (ENODE.Level == 2)
                {
                    TreeNode secondparent = ENODE.Parent;
                    TreeNode firstparent = secondparent.Parent;

                    if (firstparent.Text == "VOR" || firstparent.Text == "DME")
                    {
                        //VOR Or DME or ILS All Check for Radius
                        //tableofDME.Clear();
                        //ObtsacleReportAddOrRemovefromGrid.VORORDME_AllSteps_Check_Show_ObstacleReport(GridObstclDataForDME, ENODE, tableofDME);
                    }
                    if (firstparent.Text == "ILS")
                    {
                        //ObtsacleReportAddOrRemovefromGrid.VORORDME_AllSteps_Check_Show_ObstacleReport(GridObstclDataForDME, ENODE);

                    }
                    else
                    {
                        if (firstparent.Text != "ILS")
                        {
                            TabPage tab = tabControl1.TabPages[0];
                            tabControl1.SelectTab(tab);
                        }
                        //NDB or Markers StepbyStep Check
                        ObtsacleReportAddOrRemovefromGrid.NDB_OR_Markers_Check_Show_ObstacleReportADD(GridObstclDataForDME, tableofDME, ENODE, firstparent, secondparent, ListForMarkers50r, ListForMarker200r, ListForNDB200r, ListForNDB1000r);
                    }

                }
                if (ENODE.Level == 1)
                {
                    //All Check NDB or Markers
                }
            }
            if (ENODE.Checked == false)
            {
                if (ENODE.Level == 3)
                {

                    //VOR or DME or ILS StepbyStep UnCheck
                    TreeNode thirdparent = ENODE.Parent;
                    TreeNode secondparent = thirdparent.Parent;
                    TreeNode firstparent = secondparent.Parent;

                    if (firstparent.Text == "ILS")
                    {
                        TabPage tab = tabControl1.TabPages[1];
                        tabControl1.SelectTab(tab);
                        ObtsacleReportAddOrRemovefromGrid.ILS_StepbyStep_Check_Remove_From_Grid(GridObstclDataForILS, ENODE, secondparent, thirdparent);
                    }
                    else
                    {
                        TabPage tab = tabControl1.TabPages[0];
                        tabControl1.SelectTab(tab);
                        ObtsacleReportAddOrRemovefromGrid.VORORDME_StepbyStep_Check_Show_ObstacleReportRemove(GridObstclDataForDME, ENODE, secondparent);

                    }
                }
                if (ENODE.Level == 2)
                {
                    //NDB OR MArkers step by step uncheck
                    TreeNode secondparent = ENODE.Parent;
                    TreeNode firstparent = secondparent.Parent;

                    if (firstparent.Text != "ILS")
                    {
                        TabPage tab = tabControl1.TabPages[0];
                        tabControl1.SelectTab(tab);

                    }
                    ObtsacleReportAddOrRemovefromGrid.NDB_OR_Markers_Check_Remove_FromGrid(GridObstclDataForDME, ENODE, secondparent, firstparent);

                }
            }

            LblDmeRowCount.Text = GridObstclDataForDME.Rows.Count.ToString();
            LblIlsRowCount.Text = GridObstclDataForILS.Rows.Count.ToString();

            if (GridObstclDataForDME.Rows.Count > 0)
            {

                foreach (DataGridViewRow row in GridObstclDataForDME.Rows)
                {
                    if (Convert.ToDouble(row.Cells[7].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            if (GridObstclDataForILS.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GridObstclDataForILS.Rows)
                {
                    if (Convert.ToDouble(row.Cells[6].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }

            }
        }

        private void triStateTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            triStateTreeView1.SelectedNode = null;
            foreach (TreeNode nodeone in triStateTreeView1.Nodes)
            {
                TreeViewClass.HideCheckBox(triStateTreeView1, nodeone);

                if (nodeone.Text == "VOR" || nodeone.Text == "DME" || nodeone.Text == "ILS")
                {
                    foreach (TreeNode node2 in nodeone.Nodes)
                    {
                        TreeViewClass.HideCheckBox(triStateTreeView1, node2);
                    }

                }

            }
        }

        private void triStateTreeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void ObstacleReport_Load(object sender, EventArgs e)
        {
            MethodTreeViewAddIconAndHideCheckbox();

            //triStateTreeView1.SelectedNode = null;
            foreach (TreeNode nodeone in triStateTreeView1.Nodes)
            {
                TreeViewClass.HideCheckBox(triStateTreeView1, nodeone);

                if (nodeone.Text == "VOR" || nodeone.Text == "DME" || nodeone.Text == "ILS")
                {
                    foreach (TreeNode node2 in nodeone.Nodes)
                    {
                        TreeViewClass.HideCheckBox(triStateTreeView1, node2);
                    }

                }

            }

        }
        public void MethodTreeViewAddIconAndHideCheckbox()
        {
            triStateTreeView1.ImageList = imageList1;

            foreach (TreeNode node in triStateTreeView1.Nodes)
            {
                //TreeViewClass.HideCheckBox(triStateTreeView1, node);

                if (node.Text == "NDB" || node.Text == "Markers")
                {
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        node2.ImageIndex = 100;
                        node2.SelectedImageIndex = 100;
                        node2.ForeColor = Color.Blue;
                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            node3.ImageIndex = 100;
                            node3.SelectedImageIndex = 100;
                            node3.ForeColor = Color.Blue;
                        }
                    }
                }
                else if (node.Text != "NDB" || node.Text != "Markers")
                {
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            node3.ImageIndex = 100;
                            node3.SelectedImageIndex = 100;

                            node3.ForeColor = Color.Blue;


                            foreach (TreeNode node4 in node3.Nodes)
                            {
                                node4.ImageIndex = 100;
                                node4.SelectedImageIndex = 100;

                                node4.ForeColor = Color.Blue;
                            }


                        }
                    }
                }
            }



        }

        private void GridObstclDataForILS_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GridObstclDataForILS.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GridObstclDataForILS.Rows)
                {
                    if (Convert.ToDouble(row.Cells[6].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void GridObstclDataForDME_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GridObstclDataForDME.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in GridObstclDataForDME.Rows)
                {
                    if (Convert.ToDouble(row.Cells[7].Value) < 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
}
