using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Aran.AranEnvironment;
using System.Globalization;
using Aran.PANDA.Conventional.Racetrack;
using ChoosePointNS;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Aran.Aim.Features;
using System.Collections.ObjectModel;
using Aran.AranEnvironment.Symbols;
using MahApps.Metro.Controls;
using Aran.Aim.Enums;
using Aran.Queries.Common;
using Aran.Queries;
using Aran.Queries.Omega;
using Aran.Geometries.Operators;
using System.Reflection;
using System.Reflection.Emit;
using Aran.Omega.Properties;
using Aran.Omega.View;
using System.Threading;
using Aran.Aim;
using Aran.Aim.Data;
using System.Data.OleDb;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.ADF.CATIDs;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ArcMapUI;
using System.Diagnostics;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.PANDA.RNAV.Departure;
using ICAO015;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using ICAO015.TabControl;
using ICAO015.Draw_Remove_Lists;

namespace Europe_ICAO015
{


    public partial class Form1 : Form
    {
        //HarmonisedGuidanceForPar {
        public Aran.Geometries.Point PTPrjTRESHOLD;
        public static SpatialReferenceOperation pspatialReferenceOperation;
        public Aran.Geometries.Point PrjGpOrLocOrDmePoint;
        public List<CoordinateListsForGP_OR_LOC_DME> ParamListsGP_LOC_DME = new List<CoordinateListsForGP_OR_LOC_DME>();
        //HarmonisedGuidanceForPar }        
        private AranTool _aranToolItem;

        //OnlyForRadiusCylnder {
        public bool WindTurbineValue = false; //CVORorDVORorDirectionFinder

        //List<string> ListNavaids = new List<string>();
        DataTable dt = new DataTable();

        public List<ParameterForDmeN> ParamListDMEN = new List<ParameterForDmeN>();
        public List<ParameterForCVOR> ParamListCVOR = new List<ParameterForCVOR>();
        public List<ParameterForDVOR> ParamListDVOR = new List<ParameterForDVOR>();
        public List<ParameterForMarkers> ParamListMarker = new List<ParameterForMarkers>();
        public List<ParameterForNDB> ParamListNDB = new List<ParameterForNDB>();

        //OnlyForRadiusCylnder }

        List<Obstacle_ParamListPolygons> Obstcl_CalcParamList_2DGraphic = new List<Obstacle_ParamListPolygons>();//Obstacle_Input_ParamListFor_2DGraphic

        public List<AddCheckedNavaids> AddcheckParentChildList = new List<AddCheckedNavaids>();

        //<<<<------------------------------------------------------------------------------------------------------------------------------->>>>>

        public List<Runway> RwyList { get; private set; }
        public List<RunwayDirection> RwyDirList { get; private set; }
        public List<RunwayCentrelinePoint> RwyCenterlinePointList { get; private set; }
        DBModule IcaoDbM = new DBModule();

        public Form1()
        {
            InitializeComponent();

            PTPrjTRESHOLD = null;
            PrjGpOrLocOrDmePoint = null;
            ParamListsGP_LOC_DME.Clear();
            ParamListCVOR.Clear();
            ParamListDMEN.Clear();
            ParamListDVOR.Clear();
            ParamListMarker.Clear();
            ParamListNDB.Clear();
            Obstcl_CalcParamList_2DGraphic.Clear();
            AddcheckParentChildList.Clear();
            RwyList = null;
            RwyDirList = null;
            RwyCenterlinePointList = null;
        }
        public Form1(AranTool aranToolitem) : this()
        {
            _aranToolItem = aranToolitem;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrjGpOrLocOrDmePoint = null;

            IcaoDbM.ICAO_OnLoad();
            LoadRunwayLists();
            ObjComboRwyDirList.SelectedIndex = 0;
            this.Text = "Omega(European ICAO Doc 15) - Airport / Heliport: " + IcaoDbM.GetAirportDesignator();

            string AirportName = Aran.Omega.GlobalParams.Database.AirportHeliport.Name.ToString();

            TrwViewNavaids.Nodes.Add("DME", "DME");
            TrwViewNavaids.Nodes.Add("ILS", "ILS");
            TrwViewNavaids.Nodes.Add("VOR", "VOR");
            TrwViewNavaids.Nodes.Add("Markers", "Markers");
            TrwViewNavaids.Nodes.Add("NDB", "NDB");
            TrwViewNavaids.Nodes["DME"].Nodes.Add("DMEN", "DMEN");
            TrwViewNavaids.Nodes["ILS"].Nodes.Add("GP", "GP");
            TrwViewNavaids.Nodes["ILS"].Nodes.Add("LOC(Single Frequency)", "LOC(Single Frequency)");
            TrwViewNavaids.Nodes["ILS"].Nodes.Add("LOC(Dual Frequency)", "LOC(Dual Frequency)");
            TrwViewNavaids.Nodes["ILS"].Nodes.Add("DME", "DME");
            TrwViewNavaids.Nodes["VOR"].Nodes.Add("CVOR", "CVOR");
            TrwViewNavaids.Nodes["VOR"].Nodes.Add("DVOR", "DVOR");

            var ListForNDB = NDBList().OrderByDescending(name => name.Name).ToList();
            var ListForMarker = MarkerList().OrderByDescending(name => name.Name).ToList();

            for (int ndb = 0; ndb < ListForNDB.Count; ndb++)
            {
                TrwViewNavaids.Nodes["NDB"].Nodes.Add(ListForNDB[ndb].Designator.ToString());
            }
            for (int marker = 0; marker < ListForMarker.Count; marker++)
            {
                TrwViewNavaids.Nodes["Markers"].Nodes.Add(ListForMarker[marker].Designator.ToString());
            }
            //ForCVOR
            for (int q = 0; q < IcaoDbM.VORlistsForCvor.Count; q++)
            {
                TrwViewNavaids.Nodes["VOR"].Nodes["CVOR"].Nodes.Add(IcaoDbM.VORlistsForCvor[q].ToString());
            }
            //-------------------
            //ForDVOR
            for (int q = 0; q < IcaoDbM.VORlistsForDvor.Count; q++)
            {
                TrwViewNavaids.Nodes["VOR"].Nodes["DVOR"].Nodes.Add(IcaoDbM.VORlistsForDvor[q].ToString());
            }
            //-------------------

            dt.Columns.Add("TypeofNavigationFacilities");
            dt.Columns.Add("Radius(r - Cylnder)");
            dt.Columns.Add("Alpha(a-cone)(o)");
            dt.Columns.Add("Radius(R-cone)(m)");
            dt.Columns.Add("Radius(J-Cylnder)(m)WindTurbine(s)Only");
            dt.Columns.Add("HeightofCylnderJH(height)(m)WindTurbine(s)Only");

            var MyNavaidlist = NavaidList().OrderByDescending(srt => srt.Id).ToList();
            var MyDmeLists = DmeListsLocGeo().OrderByDescending(srt => srt.Id).ToList();

            //DME {
            int empty = 0;
            int var = 0;
            int continueval = 0;
            string designator = null;
            string type = null;

            //GP or DME or Loc add to ILS but this below algoritm if ILS_DME has in navaidlist, it has not in DME_List {

            for (int q = 0; q < MyDmeLists.Count; q++)
            {
                if (MyDmeLists[q].Name.ToString() == AirportName)
                {
                    if (MyDmeLists[q].Type.ToString() == "NARROW")
                    {
                        string setvalue = MyDmeLists[q].Designator.ToString();

                        if (var != 1 && empty == 1)
                        {
                            TrwViewNavaids.Nodes["DME"].Nodes["DMEN"].Nodes.Add(designator);
                        }

                        var = 0;
                        empty = 0;

                        for (int s = continueval; s < MyNavaidlist.Count; s++)
                        {
                            if (MyNavaidlist[s].Name != null)
                            {
                                if (MyNavaidlist[s].Name.ToString() == AirportName)
                                {
                                    if (MyNavaidlist[s].Type.Value.ToString() == "ILS_DME")
                                    {
                                        if (MyNavaidlist[s].Designator.ToString() == MyDmeLists[q].Designator.ToString())
                                        {
                                            TrwViewNavaids.Nodes["ILS"].Nodes["GP"].Nodes.Add(MyNavaidlist[s].Designator.ToString());
                                            TrwViewNavaids.Nodes["ILS"].Nodes["LOC(Single Frequency)"].Nodes.Add(MyNavaidlist[s].Designator.ToString());
                                            TrwViewNavaids.Nodes["ILS"].Nodes["LOC(Dual Frequency)"].Nodes.Add(MyNavaidlist[s].Designator.ToString());
                                            TrwViewNavaids.Nodes["ILS"].Nodes["DME"].Nodes.Add(MyNavaidlist[s].Designator.ToString());
                                            continueval = 0;
                                        }
                                        if (MyNavaidlist[s].Designator.ToString() != setvalue)
                                        {
                                            empty = 1;
                                            designator = MyDmeLists[q].Designator.ToString();
                                            type = MyDmeLists[q].Type.Value.ToString();
                                        }
                                        if (MyNavaidlist[s].Designator.ToString() == setvalue)
                                        {
                                            var = 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //GP or DME or Loc add to ILS but this below algoritm if ILS_DME has in navaidlist, it has not in DME_List }


            //DME }
            TrwViewNavaids.ImageList = imageList1;

            foreach (TreeNode node in TrwViewNavaids.Nodes)
            {
                TreeViewClass.HideCheckBox(TrwViewNavaids, node);

                if (node.Text == "NDB" || node.Text == "Markers")
                {
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        node2.ImageIndex = 100;
                        node2.SelectedImageIndex = 100;
                        node2.ForeColor = Color.Blue;
                    }
                }
                else if (node.Text != "NDB" || node.Text != "Markers")
                {


                    foreach (TreeNode node2 in node.Nodes)
                    {

                        TreeViewClass.HideCheckBox(TrwViewNavaids, node2);

                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            node3.ImageIndex = 100;
                            node3.SelectedImageIndex = 100;

                            node3.ForeColor = Color.Blue;

                            if (node3.Text == "GP")
                            {
                                TreeViewClass.HideCheckBox(TrwViewNavaids, node3);
                            }
                            if (node3.Text == "LOC")
                            {
                                TreeViewClass.HideCheckBox(TrwViewNavaids, node3);
                            }
                            if (node3.Text == "DME")
                            {
                                TreeViewClass.HideCheckBox(TrwViewNavaids, node3);
                            }
                        }
                    }
                }
            }




        }
        public List<DME> DmeListsLocGeo()
        {
            return IcaoDbM.DMELists.ToList();
        }
        public List<VOR> VorListLocGeo()
        {
            return IcaoDbM.VORlistsAll.ToList();
        }
        public List<VerticalStructure> VerticalStrList()
        {
            return DBModule.VRTsList.ToList();
        }
        public List<Navaid> NavaidList()
        {
            return DBModule.NavaidLists.ToList();
        }
        public List<NDB> NDBList()
        {
            return IcaoDbM.NDBLists.ToList();
        }
        public List<MarkerBeacon> MarkerList()
        {
            return IcaoDbM.MarkersList.ToList();
        }

        public void LoadRunwayLists()
        {

            AllRunwayLists allrunwaylists = new AllRunwayLists();
            RwyList = IcaoDbM.RunwayList;
            RwyDirList = IcaoDbM.RunwayDirList;
            RwyCenterlinePointList = IcaoDbM.RunwayCentreLinePoint;
            allrunwaylists.RunwayListFillDataGridView(RwyList, DatGridVRunwayList);
            allrunwaylists.RunwayDirectionListFillCombobx(ObjComboRwyDirList, RwyDirList, DatGridVRunwayList);
        }
        private void GrbBoxParmetrs_Enter(object sender, EventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveAll_DRAW();
        }
        private void RemoveAll_DRAW()
        {
            for (int i = 0; i < DrawREmoveListILS.Count; i++)
            {
                GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawREmoveListILS[i].Square);
                GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawREmoveListILS[i].FirstCornerPolygon);
                GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawREmoveListILS[i].SecondCornerPolygon);
                GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawREmoveListILS[i].SegmentPolygon);
                //DrawREmoveListILS.RemoveAt(i);
            }
            for (int j = 0; j < DrawRemoveListFORRADIUS.Count; j++)
            {
                if (DrawRemoveListFORRADIUS[j].MiddleRadius != 0)
                {
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawRemoveListFORRADIUS[j].SmallRadius);
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawRemoveListFORRADIUS[j].MiddleRadius);
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawRemoveListFORRADIUS[j].LargeRadius);
                    //DrawRemoveListFORRADIUS.RemoveAt(j);
                }
                if (DrawRemoveListFORRADIUS[j].MiddleRadius == 0)
                {
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawRemoveListFORRADIUS[j].SmallRadius);
                    GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(DrawRemoveListFORRADIUS[j].LargeRadius);
                    //DrawRemoveListFORRADIUS.RemoveAt(j);
                }
            }
        }
        private void GrbBoxNavaids_Enter(object sender, EventArgs e)
        {

        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            //progressBar1.Visible = true;//Displays progress bar
            //progressBar1.Show();
            //progressBar1.Maximum = 100;
            //progressBar1.Step = 1;
            Cursor.Current = Cursors.WaitCursor;
            LblLoadingTxt.Visible = true;
            LblLoadingTxt.Show();
            Calculate_Obstacle();
            LblLoadingTxt.Visible = false;
            Cursor.Current = Cursors.Default;
            //progressBar1.PerformStep();

            //progressBar1.Visible = false;
        }
        private void Calculate_Obstacle()
        {
            Calculate_MethodFor_2D_Graphic ObtsclCalcFor2DGraphicMEthod = new Calculate_MethodFor_2D_Graphic();
            Calculate_Obstcl_MethodFor_NavaidRadius ObtsclCalcForNavaidRadiusMEthod = new Calculate_Obstcl_MethodFor_NavaidRadius();

            List<VerticalStructure> VerticalStrucutrlist = VerticalStrList();

            AranTool toolitem = new AranTool();

            List<Lists_FOR_2DGraphics> List2DGraphicCalculated = ObtsclCalcFor2DGraphicMEthod.CalcObstacleNearDistanceFor2DGraphic(Obstcl_CalcParamList_2DGraphic);
            ObstacleReport.List_2DGrpahics_Calculation = List2DGraphicCalculated;
            int Calc_Count = 0;
            ObtsclCalcForNavaidRadiusMEthod.CalcLoadObstacleNearDistanceForOnlyRadius(VerticalStrucutrlist, ChkBoxWindTurbine, ParamListDMEN, ParamListCVOR, ParamListDVOR, ParamListMarker, ParamListNDB, _aranToolItem, ref Calc_Count);

            if (List2DGraphicCalculated.Count > 0 || Calc_Count > 0)
            {
                ObstacleReport obstcl = new ObstacleReport(toolitem);
                obstcl.Show(GlobalParams.AranEnvironment.Win32Window);
            }
            else
            {
                MessageBox.Show("Please select Navaid list");
            }
        }
        private void TrwViewNavaids_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TrwViewNavaids.SelectedNode = null;
        }
        public bool isProcessRunning = false;
        //public bool TreeviewCheckControl = false;
        AddToTabOr_Remove_ListCommand Add_OR_Removee_ToTablistDmeORILS = new AddToTabOr_Remove_ListCommand();//Add to TabList DME
        List<Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList> DrawRemoveListFORRADIUS = new List<Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList>();
        List<Draw_Remove_ILS_List> DrawREmoveListILS = new List<Draw_Remove_ILS_List>();
        private void TrwViewNavaids_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //TrwViewNavaids.SelectedNode = null;

            TreeNode tnChild = e.Node;

            if (e.Node.Checked == true)
            {

                if (e.Node.Level == 2)
                {
                    TreeNode secondparent = e.Node.Parent;
                    TreeNode firstparent = secondparent.Parent;

                    if (firstparent.Text == "ILS")
                    {
                        TabPage tab = TabNavaidControl.TabPages[1];
                        TabNavaidControl.SelectTab(tab);
                    }
                    else
                    {
                        TabPage tab = TabNavaidControl.TabPages[0];
                        TabNavaidControl.SelectTab(tab);
                    }
                }
                else
                {
                    TabPage tab = TabNavaidControl.TabPages[0];
                    TabNavaidControl.SelectTab(tab);
                }

                ProcessingLoadingForOnlyRadiusCalc(tnChild, tnChild.Parent);

                TreeviewParentOrChildAdd(tnChild, tnChild.Parent);
                TrwViewNavaids.BeginUpdate();
                //ListNavaids.Add(tnChild.Parent.Text);

                ProcessForHarmonisedGuidance(tnChild.Parent, e.Node);

                //TreeViewClass.OnlyOneCheckTreeNode(TrwViewNavaids, e.Node, e.Node.Parent, TreeviewCheckControl); //Select Only one Childcheckbox Method

                //TrwViewNavaids.BeginUpdate();
                //TrwViewNavaids.SelectedNode = null;
                TrwViewNavaids.Enabled = false;


                Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    isProcessRunning = true;

                    for (int n = 0; n < 100; n++)
                    {
                        Thread.Sleep(6);
                    }
                    progressBar1.BeginInvoke(
                    new Action(() =>
                    {
                        TrwViewNavaids.Enabled = true;
                    }
            ));

                    isProcessRunning = false;
                }));

                backgroundThread.Start();
                //TrwViewNavaids.SelectedNode = null;
                tnChild.ForeColor = Color.Red;
                //TrwViewNavaids.EndUpdate();

                BtnObstcInput.Enabled = true;


            }
            if (e.Node.Checked == false)
            {

                if (e.Node.Level == 2)
                {
                    TreeNode secondparent = e.Node.Parent;
                    TreeNode firstparent = secondparent.Parent;

                    if (firstparent.Text == "ILS")
                    {
                        TabPage tab = TabNavaidControl.TabPages[1];
                        TabNavaidControl.SelectTab(tab);
                    }
                    else
                    {
                        TabPage tab = TabNavaidControl.TabPages[0];
                        TabNavaidControl.SelectTab(tab);
                    }
                }
                else
                {
                    TabPage tab = TabNavaidControl.TabPages[0];
                    TabNavaidControl.SelectTab(tab);
                }

                NavaidParameterRemoveForDMEORILS(e.Node);

                Add_OR_Removee_ToTablistDmeORILS.RemoveTabList_ILSORDME(DGridVNvidsParameters, tnChild);
                Add_OR_Removee_ToTablistDmeORILS.RemoveTabList_ILSORDME(DatGridILSParameters, tnChild);
                Draw_Remove_Command.DrawRemoveILS_OR_DME(DrawRemoveListFORRADIUS, e.Node.Text, e.Node.Parent.Text);
                Draw_Remove_Command.DrawRemoveILS_OR_DME(DrawREmoveListILS, e.Node.Text, e.Node.Parent.Text);

                TreeviewParentOrChildRemoving(e.Node, tnChild.Parent);

                //TrwViewNavaids.BeginUpdate();
                TrwViewNavaids.Enabled = false;

                Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    isProcessRunning = true;

                    for (int n = 0; n < 100; n++)
                    {
                        Thread.Sleep(3);
                    }
                    progressBar1.BeginInvoke(
                    new Action(() =>
                    {
                        TrwViewNavaids.Enabled = true;
                    }
            ));

                    isProcessRunning = false;
                }));

                backgroundThread.Start();

                //TrwViewNavaids.SelectedNode = null;
                tnChild.ForeColor = Color.Blue;
                //TrwViewNavaids.EndUpdate();

                //BtnObstcInput.Enabled = false;
            }

            TrwViewNavaids.EndUpdate();
        }
        public void NavaidParameterRemoveForDMEORILS(TreeNode eNode)
        {
            if (eNode.Parent.Text == "GP" || eNode.Parent.Text == "DME" || eNode.Parent.Text == "LOC(Dual Frequency)" || eNode.Parent.Text == "LOC(Single Frequency)")
            {
                for (int i = 0; i < Obstcl_CalcParamList_2DGraphic.Count; i++)
                {
                    if (Obstcl_CalcParamList_2DGraphic[i].Parent_NodeTxt == eNode.Parent.Text && Obstcl_CalcParamList_2DGraphic[i].Child_NodeTxt == eNode.Text)
                    {
                        Obstcl_CalcParamList_2DGraphic.RemoveAt(i);
                    }
                }
            }
            if (eNode.Parent.Text == "DVOR")
            {
                for (int i = 0; i < ParamListDVOR.Count; i++)
                {
                    if (ParamListDVOR[i].NavaidName == eNode.Text && ParamListDVOR[i].TypeOfNavigation == eNode.Parent.Text)
                    {
                        ParamListDVOR.RemoveAt(i);
                    }
                }
            }
            if (eNode.Parent.Text == "CVOR")
            {
                for (int i = 0; i < ParamListCVOR.Count; i++)
                {
                    if (ParamListCVOR[i].NavaidName == eNode.Text && ParamListCVOR[i].TypeOfNavigation == eNode.Parent.Text)
                    {
                        ParamListCVOR.RemoveAt(i);
                    }
                }
            }
            if (eNode.Parent.Text == "DMEN")
            {
                for (int i = 0; i < ParamListDMEN.Count; i++)
                {
                    if (ParamListDMEN[i].NavaidName == eNode.Text && ParamListDMEN[i].TypeofNavigation == eNode.Parent.Text)
                    {
                        ParamListDMEN.RemoveAt(i);
                    }
                }
            }
            if (eNode.Parent.Text == "NDB")
            {
                for (int i = 0; i < ParamListNDB.Count; i++)
                {
                    if (ParamListNDB[i].NavaidName == eNode.Text && ParamListNDB[i].TypeofNavigation == eNode.Parent.Text)
                    {
                        ParamListNDB.RemoveAt(i);
                    }
                }
            }
            if (eNode.Parent.Text == "Markers")
            {
                for (int i = 0; i < ParamListMarker.Count; i++)
                {
                    if (ParamListMarker[i].NavaidName == eNode.Text && ParamListMarker[i].TypeOfNavigation == eNode.Parent.Text)
                    {
                        ParamListMarker.RemoveAt(i);
                    }
                }
            }
        }
        private void TreeviewParentOrChildRemoving(TreeNode noderemove, TreeNode parentnode)
        {

            for (int q = 0; q < AddcheckParentChildList.Count; q++)
            {
                if (parentnode.Text == "NDB" || parentnode.Text == "Markers")
                {
                    if (AddcheckParentChildList[q].ChildName == noderemove.Text && AddcheckParentChildList[q].SecondParentName == parentnode.Text)
                    {
                        AddcheckParentChildList.Remove(AddcheckParentChildList[q]);
                    }
                }
                else if (parentnode.Text != "NDB" || parentnode.Text != "Markers")
                {
                    if (AddcheckParentChildList[q].ChildName == noderemove.Text && AddcheckParentChildList[q].SecondParentName == noderemove.Parent.Text && AddcheckParentChildList[q].FirstParentName == parentnode.Parent.Text)
                    {
                        AddcheckParentChildList.Remove(AddcheckParentChildList[q]);
                    }
                }
            }


        }
        private void TreeviewParentOrChildAdd(TreeNode node, TreeNode ParentNode)
        {
            //AddcheckParentChildList.Clear();

            var Last_nodetxt = "";
            var Secondnodeparent = "";
            var FirstBigParent = "";

            AddCheckedNavaids adchecked = new AddCheckedNavaids();

            Last_nodetxt = node.Text;
            Secondnodeparent = node.Parent.Text;


            if (Secondnodeparent == "Markers" || Secondnodeparent == "NDB")
            {
                adchecked.ChildName = Last_nodetxt.ToString();
                adchecked.SecondParentName = Secondnodeparent.ToString();
            }
            else
            {
                FirstBigParent = ParentNode.Parent.Text;
                adchecked.ChildName = Last_nodetxt.ToString();
                adchecked.SecondParentName = Secondnodeparent.ToString();
                adchecked.FirstParentName = FirstBigParent.ToString();
            }

            AddcheckParentChildList.Add(adchecked);
            ObstacleReport.AddcheckedChildParent = AddcheckParentChildList;

        }
        public void ProcessingLoadingForOnlyRadiusCalc(TreeNode node, TreeNode ParentNode)
        {

            OleDbConnection c = null;

            try
            {
                c = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Program Files (x86)\R.I.S.K\IAIM\bin\NavaidsParameters.mdb");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection problem" + ex.Message);
            }

            ParameterForDmeN ParamDMEN = new ParameterForDmeN();
            ParameterForDVOR ParamDvor = new ParameterForDVOR();
            ParameterForCVOR ParamCvor = new ParameterForCVOR();
            ParameterForMarkers ParamMarker = new ParameterForMarkers();
            ParameterForNDB ParamNDB = new ParameterForNDB();

            List<DME> MylistDmen = DmeListsLocGeo();
            List<VOR> MylistCVOR = VorListLocGeo();
            List<VOR> MylistDVOR = VorListLocGeo();
            List<MarkerBeacon> MyListMarker = MarkerList();
            List<NDB> MyListNDB = NDBList();

            var Last_nodetxt = "";
            var Secondnodeparent = "";
            var FirstBigParent = "";

            Last_nodetxt = node.Text;
            Secondnodeparent = node.Parent.Text;

            if (Secondnodeparent == "Markers" || Secondnodeparent == "NDB")
            {
                switch (Secondnodeparent)
                {
                    case "Markers":

                        try
                        {
                            for (int r = 0; r < Convert.ToInt32(MyListMarker.Count); r++)
                            {
                                if (MyListMarker[r].Designator == node.Text)
                                {
                                    Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList DrawRemove = new Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList();

                                    double uom = Convert.ToDouble(MyListMarker[r].Location.Elevation.Value);
                                    double DmeMeter = Common.DeConvertHeight(uom);
                                    ParamMarker.HeightDistance = Convert.ToInt32(DmeMeter);
                                    //ParamMarker.ID = MylistDmen[r].Designator.ToString();
                                    ParamMarker.Coordinate = GlobalParams.SpatialRefOperation.ToPrj(MyListMarker[r].Location.Geo);
                                    ParamMarker.NavaidName = node.Text;
                                    ParamMarker.TypeOfNavigation = node.Parent.Text;
                                    var count = ParamListMarker.Count;

                                    //GlobalParams.AranEnvironment.Graphics.DrawPoint(ParamMarker.Coordinate, 255 * 255, true, false);
                                    c.Open();
                                    OleDbCommand cmd = new OleDbCommand("select TypeofNavigationFacilities,[Radius(r-Cylnder)],[Alpha(a-cone)(o)],[Radius(R-cone)(m)],[Radius(J-Cylnder)(m)WindTurbine(s)Only],[HeightofCylnderJH(height)(m)WindTurbine(s)Only],[OriginofConeandAxisofCylinders] from Navaids where TypeofNavigationFacilities='" + Secondnodeparent + "'", c);
                                    OleDbDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read() == true)
                                    {
                                        DrawRemove.SmallRadius = DrawCircle(ParamMarker.Coordinate, Convert.ToInt32(reader[1].ToString()), "small");
                                        DrawRemove.LargeRadius = DrawCircle(ParamMarker.Coordinate, Convert.ToInt32(reader[3].ToString()), "large");
                                        ParamMarker.MarkerForAlpha = Convert.ToDouble(reader[2]);
                                        ParamMarker.MarkerForLargeRadius = Convert.ToDouble(reader[3]);
                                        ParamMarker.MarkerForSmallRadius = Convert.ToDouble(reader[1]);

                                        Add_OR_Removee_ToTablistDmeORILS.AddToTabList_FOR_DME(DGridVNvidsParameters, reader[0].ToString(), node.Text.ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());

                                    }
                                    c.Close();

                                    ParamListMarker.Add(ParamMarker);

                                    DrawRemove.ChildNode = node.Text;
                                    DrawRemove.ParentNode = node.Parent.Text;
                                    DrawRemoveListFORRADIUS.Add(DrawRemove);

                                    //Draw_Remove_Command.Draw_RemoveList = DrawRemoveList;

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Problem Happened Markers for parent Markers check child node:::" + ex.Message);
                        }
                        break;
                    case "NDB":

                        try
                        {
                            for (int r = 0; r < Convert.ToInt32(MyListNDB.Count); r++)
                            {
                                if (MyListNDB[r].Designator == node.Text)
                                {
                                    Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList DrawRemove = new Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList();

                                    double uom = Convert.ToDouble(MyListNDB[r].Location.Elevation.Value);
                                    double DmeMeter = Common.DeConvertHeight(uom);
                                    ParamNDB.HeightDistance = Convert.ToInt32(DmeMeter);
                                    ParamNDB.NavaidName = node.Text;
                                    ParamNDB.TypeofNavigation = node.Parent.Text;
                                    ParamNDB.Coordinate = GlobalParams.SpatialRefOperation.ToPrj(MyListNDB[r].Location.Geo);
                                    var count = ParamListNDB.Count;

                                    //GlobalParams.AranEnvironment.Graphics.DrawPoint(ParamNDB.Coordinate, 255 * 255, true, false);
                                    c.Open();
                                    OleDbCommand cmd = new OleDbCommand("select TypeofNavigationFacilities,[Radius(r-Cylnder)],[Alpha(a-cone)(o)],[Radius(R-cone)(m)],[Radius(J-Cylnder)(m)WindTurbine(s)Only],[HeightofCylnderJH(height)(m)WindTurbine(s)Only],[OriginofConeandAxisofCylinders] from Navaids where TypeofNavigationFacilities='" + Secondnodeparent + "'", c);
                                    OleDbDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read() == true)
                                    {
                                        DrawRemove.SmallRadius = DrawCircle(ParamNDB.Coordinate, Convert.ToInt32(reader[1].ToString()), "small");
                                        DrawRemove.LargeRadius = DrawCircle(ParamNDB.Coordinate, Convert.ToInt32(reader[3].ToString()), "large");
                                        ParamNDB.NDBForAlpha = Convert.ToDouble(reader[2]);
                                        ParamNDB.NDBForLargeRadius = Convert.ToDouble(reader[3]);
                                        ParamNDB.NDBForSmallRadius = Convert.ToDouble(reader[1]);

                                        Add_OR_Removee_ToTablistDmeORILS.AddToTabList_FOR_DME(DGridVNvidsParameters, reader[0].ToString(), node.Text.ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());

                                    }
                                    c.Close();

                                    ParamListNDB.Add(ParamNDB);

                                    DrawRemove.ChildNode = node.Text;
                                    DrawRemove.ParentNode = node.Parent.Text;
                                    DrawRemoveListFORRADIUS.Add(DrawRemove);

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Problem Happened NDB" + ex.Message);
                        }
                        break;
                }

            }
            else
            {
                FirstBigParent = ParentNode.Parent.Text;

                switch (Secondnodeparent)
                {
                    case "DMEN":
                        try
                        {
                            for (int r = 0; r < Convert.ToInt32(MylistDmen.Count); r++)
                            {
                                if (MylistDmen[r].Designator == node.Text)
                                {
                                    Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList DrawRemove = new Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList();

                                    double uom = Convert.ToDouble(MylistDmen[r].Location.Elevation.Value);
                                    double DmeMeter = Common.DeConvertHeight(uom);
                                    ParamDMEN.HeightDistance = Convert.ToInt32(DmeMeter);
                                    ParamDMEN.NavaidName = node.Text;
                                    ParamDMEN.TypeofNavigation = node.Parent.Text;
                                    ParamDMEN.Coordinate = GlobalParams.SpatialRefOperation.ToPrj(MylistDmen[r].Location.Geo);
                                    var count = ParamListDMEN.Count;

                                    //GlobalParams.AranEnvironment.Graphics.DrawPoint(ParamDMEN.Coordinate, 255 * 255, true, false);
                                    //node.Text = nodetxt;
                                    //-------------------------------------

                                    c.Open();
                                    OleDbCommand cmd = new OleDbCommand("select TypeofNavigationFacilities,[Radius(r-Cylnder)],[Alpha(a-cone)(o)],[Radius(R-cone)(m)],[Radius(J-Cylnder)(m)WindTurbine(s)Only],[HeightofCylnderJH(height)(m)WindTurbine(s)Only],[OriginofConeandAxisofCylinders] from Navaids where TypeofNavigationFacilities='" + Secondnodeparent + "'", c);
                                    OleDbDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read() == true)
                                    {
                                        DrawRemove.SmallRadius = DrawCircle(ParamDMEN.Coordinate, Convert.ToInt32(reader[1].ToString()), "small");
                                        DrawRemove.LargeRadius = DrawCircle(ParamDMEN.Coordinate, Convert.ToInt32(reader[3].ToString()), "large");
                                        ParamDMEN.DmeForAlpha = Convert.ToDouble(reader[2]);
                                        ParamDMEN.DmeForLargeRadius = Convert.ToDouble(reader[3]);
                                        ParamDMEN.DmeForSmallRadius = Convert.ToDouble(reader[1]);

                                        Add_OR_Removee_ToTablistDmeORILS.AddToTabList_FOR_DME(DGridVNvidsParameters, reader[0].ToString(), node.Text.ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());

                                    }
                                    c.Close();

                                    ParamListDMEN.Add(ParamDMEN);

                                    DrawRemove.ChildNode = node.Text;
                                    DrawRemove.ParentNode = node.Parent.Text;
                                    DrawRemoveListFORRADIUS.Add(DrawRemove);

                                    //------------------------------

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Problem Happened for parent DMEN check child node" + ex.Message);
                        }
                        break;

                    case "CVOR":

                        for (int r = 0; r < Convert.ToInt32(MylistCVOR.Count); r++)
                        {
                            if (MylistCVOR[r].Type.Value.ToString() == "CVOR")
                            {

                                if (MylistCVOR[r].Designator == node.Text)
                                {
                                    Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList DrawRemove = new Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList();

                                    double uom = Convert.ToDouble(MylistCVOR[r].Location.Elevation.Value);
                                    double DmeMeter = Common.DeConvertHeight(uom);
                                    ParamCvor.Coordinate = GlobalParams.SpatialRefOperation.ToPrj(MylistCVOR[r].Location.Geo);
                                    ParamCvor.HeightDistance = Convert.ToInt32(DmeMeter);
                                    //ParamCvor.ID = MylistCVOR[r].Designator.ToString();
                                    ParamCvor.NavaidName = node.Text;
                                    ParamCvor.TypeOfNavigation = node.Parent.Text;
                                    ParamCvor.Coordinate = ParamCvor.Coordinate;
                                    var count = ParamListCVOR.Count;

                                    //GlobalParams.AranEnvironment.Graphics.DrawPoint(ParamCvor.Coordinate, 255 * 255, true, false);

                                    //node.Text = nodetxt;
                                    //-------------------------------------                                        
                                    c.Open();
                                    OleDbCommand cmd = new OleDbCommand("select TypeofNavigationFacilities,[Radius(r-Cylnder)],[Alpha(a-cone)(o)],[Radius(R-cone)(m)],[Radius(J-Cylnder)(m)WindTurbine(s)Only],[HeightofCylnderJH(height)(m)WindTurbine(s)Only],[OriginofConeandAxisofCylinders] from Navaids where TypeofNavigationFacilities='" + Secondnodeparent + "'", c);
                                    OleDbDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read() == true)
                                    {
                                        DrawRemove.SmallRadius = DrawCircle(ParamCvor.Coordinate, Convert.ToInt32(reader[1].ToString()), "small");
                                        DrawRemove.MiddleRadius = DrawCircle(ParamCvor.Coordinate, Convert.ToInt32(reader[3].ToString()), "middle");
                                        DrawRemove.LargeRadius = DrawCircle(ParamCvor.Coordinate, Convert.ToInt32(reader[4].ToString()), "large");

                                        ParamCvor.CVORForAlpha = Convert.ToDouble(reader[2]);
                                        ParamCvor.CVORForSmallRadius = Convert.ToDouble(reader[1]);
                                        ParamCvor.CVORForMiddleRadius = Convert.ToDouble(reader[3]);
                                        ParamCvor.CVORForLargeRadius = Convert.ToDouble(reader[4]);
                                        ParamCvor.WindTurbineHeight = Convert.ToDouble(reader[5]);

                                        Add_OR_Removee_ToTablistDmeORILS.AddToTabList_FOR_DME(DGridVNvidsParameters, reader[0].ToString(), node.Text.ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());

                                    }
                                    c.Close();

                                    ParamListCVOR.Add(ParamCvor);

                                    DrawRemove.ChildNode = node.Text;
                                    DrawRemove.ParentNode = node.Parent.Text;
                                    DrawRemoveListFORRADIUS.Add(DrawRemove);

                                    //------------------------------
                                }
                            }
                        }
                        break;

                    case "DVOR":

                        for (int r = 0; r < Convert.ToInt32(MylistDVOR.Count); r++)
                        {

                            if (MylistDVOR[r].Type.Value.ToString() == "DVOR")
                            {
                                if (MylistDVOR[r].Designator == node.Text)
                                {
                                    Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList DrawRemove = new Draw_RemoveMarker_NDB_DMEN_DVOR_CVORList();

                                    double uom = Convert.ToDouble(MylistDVOR[r].Location.Elevation.Value);
                                    double DmeMeter = Common.DeConvertHeight(uom);
                                    Aran.Geometries.Point ptgeo = MylistDVOR[r].Location.Geo;
                                    ParamDvor.Coordinate = GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.Point>(ptgeo);
                                    ParamDvor.HeightDistance = Convert.ToInt32(DmeMeter);
                                    ParamDvor.NavaidName = node.Text;
                                    ParamDvor.TypeOfNavigation = node.Parent.Text;
                                    //ParamDvor.Coordinate = ParamDvor.Coordinate;
                                    var count = ParamListDVOR.Count;

                                    //GlobalParams.AranEnvironment.Graphics.DrawPoint(ParamDvor.Coordinate, 255 * 255, true, false);

                                    //node.Text = nodetxt;
                                    //-------------------------------------
                                    c.Open();
                                    OleDbCommand cmd = new OleDbCommand("select TypeofNavigationFacilities,[Radius(r-Cylnder)],[Alpha(a-cone)(o)],[Radius(R-cone)(m)],[Radius(J-Cylnder)(m)WindTurbine(s)Only],[HeightofCylnderJH(height)(m)WindTurbine(s)Only],[OriginofConeandAxisofCylinders] from Navaids where TypeofNavigationFacilities='" + Secondnodeparent + "'", c);
                                    OleDbDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read() == true)
                                    {
                                        DrawRemove.SmallRadius = DrawCircle(ParamDvor.Coordinate, Convert.ToInt32(reader[1].ToString()), "small");
                                        DrawRemove.MiddleRadius = DrawCircle(ParamDvor.Coordinate, Convert.ToInt32(reader[3].ToString()), "middle");
                                        DrawRemove.LargeRadius = DrawCircle(ParamDvor.Coordinate, Convert.ToInt32(reader[4].ToString()), "large");
                                        ParamDvor.DVORForAlpha = Convert.ToDouble(reader[2]);
                                        ParamDvor.DVORForSmallRadius = Convert.ToDouble(reader[1]);
                                        ParamDvor.DVORForMiddleRadius = Convert.ToDouble(reader[3]);
                                        ParamDvor.DVORForLargeRadius = Convert.ToDouble(reader[4]);
                                        ParamDvor.WindTurbineHeight = Convert.ToDouble(reader[5]);

                                        Add_OR_Removee_ToTablistDmeORILS.AddToTabList_FOR_DME(DGridVNvidsParameters, reader[0].ToString(), node.Text.ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString());
                                    }
                                    c.Close();

                                    ParamListDVOR.Add(ParamDvor);

                                    DrawRemove.ChildNode = node.Text;
                                    DrawRemove.ParentNode = node.Parent.Text;
                                    DrawRemoveListFORRADIUS.Add(DrawRemove);

                                }
                            }
                        }
                        break;

                }
            }


        }
        private void ProcessForHarmonisedGuidance(TreeNode nodeparent, TreeNode enode)
        {
            List<Localizer> myGliPAthOrLocOrDme = new List<Localizer>();
            myGliPAthOrLocOrDme.Clear();
            myGliPAthOrLocOrDme = IcaoDbM.LocList.ToList();

            string nodetxt = nodeparent.Text;

            //HarmonisedGuidanceMethod {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Program Files (x86)\R.I.S.K\IAIM\bin\HarmonisedGuidance.mdb");
            con.Open();
            if (nodetxt == "GP")
            {
                double NavaidHeight;
                OleDbCommand cmd = new OleDbCommand("select Adis,bdis,h,r,D,BigH,L,Fderece from HarGuidance where TypOfNavigFacilit='ILSGP'", con);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    NavaidHeight = LoadHarmonisedGraphicParameters(enode);
                    for (int i = 0; i < myGliPAthOrLocOrDme.Count; i++)
                    {
                        if (myGliPAthOrLocOrDme[i].Designator == enode.Text)
                        {

                            DRAWGPOrLocORDMEHarGuidance(PrjGpOrLocOrDmePoint, NavaidHeight, Convert.ToDouble(reader[0].ToString()), Convert.ToDouble(reader[1].ToString()), Convert.ToDouble(reader[2].ToString()), reader[3].ToString(), Convert.ToDouble(reader[4].ToString()), Convert.ToDouble(reader[5].ToString())
                        , Convert.ToDouble(reader[6].ToString()), Convert.ToDouble(reader[7].ToString()), myGliPAthOrLocOrDme[i].TrueBearing.Value, enode);
                        }
                    }
                }
            }

            if (nodetxt == "LOC(Dual Frequency)")
            {
                double NavaidHeight;
                OleDbCommand cmd = new OleDbCommand("select Adis,bdis,h,r,D,BigH,L,Fderece from HarGuidance where TypOfNavigFacilit='ILSLLZDual'", con);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    NavaidHeight = LoadHarmonisedGraphicParameters(enode);

                    for (int i = 0; i < myGliPAthOrLocOrDme.Count; i++)
                    {
                        if (myGliPAthOrLocOrDme[i].Designator == enode.Text)
                        {
                            DRAWGPOrLocORDMEHarGuidance(PrjGpOrLocOrDmePoint, NavaidHeight, Convert.ToDouble(reader[0].ToString()), Convert.ToDouble(reader[1].ToString()), Convert.ToDouble(reader[2].ToString()), reader[3].ToString(), Convert.ToDouble(reader[4].ToString()), Convert.ToDouble(reader[5].ToString())
                                                    , Convert.ToDouble(reader[6].ToString()), Convert.ToDouble(reader[7].ToString()), myGliPAthOrLocOrDme[i].TrueBearing.Value, enode);
                        }
                    }
                }
            }
            if (nodetxt == "LOC(Single Frequency)")
            {
                double NavaidHeight;
                OleDbCommand cmd = new OleDbCommand("select Adis,bdis,h,r,D,BigH,L,Fderece from HarGuidance where TypOfNavigFacilit='SingleLoc'", con);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    NavaidHeight = LoadHarmonisedGraphicParameters(enode);
                    for (int i = 0; i < myGliPAthOrLocOrDme.Count; i++)
                    {
                        if (myGliPAthOrLocOrDme[i].Designator == enode.Text)
                        {
                            DRAWGPOrLocORDMEHarGuidance(PrjGpOrLocOrDmePoint, NavaidHeight, Convert.ToDouble(reader[0].ToString()), Convert.ToDouble(reader[1].ToString()), Convert.ToDouble(reader[2].ToString()), reader[3].ToString(), Convert.ToDouble(reader[4].ToString()), Convert.ToDouble(reader[5].ToString())
                        , Convert.ToDouble(reader[6].ToString()), Convert.ToDouble(reader[7].ToString()), myGliPAthOrLocOrDme[i].TrueBearing.Value, enode);
                        }
                    }
                }
            }
            if (nodetxt == "DME")
            {
                double NavaidHeight;
                OleDbCommand cmd = new OleDbCommand("select Adis,bdis,h,r,D,BigH,L,Fderece from HarGuidance where TypOfNavigFacilit='DME'", con);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read() == true)
                {
                    NavaidHeight = LoadHarmonisedGraphicParameters(enode);
                    for (int i = 0; i < myGliPAthOrLocOrDme.Count; i++)
                    {
                        if (myGliPAthOrLocOrDme[i].Designator == enode.Text)
                        {
                            DRAWGPOrLocORDMEHarGuidance(PrjGpOrLocOrDmePoint, NavaidHeight, Convert.ToDouble(reader[0].ToString()), Convert.ToDouble(reader[1].ToString()), Convert.ToDouble(reader[2].ToString()), reader[3].ToString(), Convert.ToDouble(reader[4].ToString()), Convert.ToDouble(reader[5].ToString())
                        , Convert.ToDouble(reader[6].ToString()), Convert.ToDouble(reader[7].ToString()), myGliPAthOrLocOrDme[i].TrueBearing.Value, enode);
                        }
                    }
                }
            }
            con.Close();
            //HarmonisedGuidanceMethod }
        }

        public double LoadHarmonisedGraphicParameters(TreeNode nodechild)
        {
            Double Navaid_height = new Double();

            PrjGpOrLocOrDmePoint = null;

            //List<Navaid> mylistNavaid = NavaidList();
            List<Localizer> myLocalizer = IcaoDbM.LocList.ToList();
            List<Glidepath> myglidepath = IcaoDbM.GlidePathList.ToList();
            List<DME> mydme = DmeListsLocGeo();

            var nodeparet = nodechild.Parent.Text;

            switch (nodeparet)
            {
                case "GP":

                    try
                    {

                        for (int i = 0; i < myglidepath.Count; i++)
                        {
                            if (myglidepath[i].Designator == nodechild.Text)
                            {
                                //MessageBox.Show(compnavaid.Location.Geo.X.ToString());
                                double uom = Convert.ToDouble(myglidepath[i].Location.Elevation.Value);
                                double GpMeter = Common.DeConvertHeight(uom);
                                PrjGpOrLocOrDmePoint = GlobalParams.SpatialRefOperation.ToPrj(myglidepath[i].Location.Geo);
                                //CoordinateListsForGP_OR_LOC_DME list = new CoordinateListsForGP_OR_LOC_DME();
                                //list.ID = myglidepath[i].Designator.ToString();
                                //list.Coordinate = PrjGpOrLocOrDmePoint;
                                //list.HeightDistance = GpMeter;
                                Navaid_height = GpMeter;
                                //ParamListsGP_LOC_DME.Add(list);
                                //GlobalParams.AranEnvironment.Graphics.DrawPoint(PrjGpOrLocOrDmePoint, 255 * 255, true, false);
                            }
                        }

                        //Draw(PrjAeroport);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("GP Select Error" + ex.Message);
                    }
                    break;

                case "LOC(Single Frequency)":

                    try
                    {

                        for (int i = 0; i < myLocalizer.Count; i++)
                        {
                            if (myLocalizer[i].Designator == nodechild.Text)
                            {
                                double uom = Convert.ToDouble(myLocalizer[i].Location.Elevation.Value);
                                double LocMeter = Common.DeConvertHeight(uom);
                                PrjGpOrLocOrDmePoint = GlobalParams.SpatialRefOperation.ToPrj(myLocalizer[i].Location.Geo);
                                //CoordinateListsForGP_OR_LOC_DME list = new CoordinateListsForGP_OR_LOC_DME();
                                //list.ID = myLocalizer[i].Designator.ToString();
                                //list.Coordinate = PrjGpOrLocOrDmePoint;
                                //list.HeightDistance = LocMeter;
                                Navaid_height = LocMeter;
                                //ParamListsGP_LOC_DME.Add(list);
                                //GlobalParams.AranEnvironment.Graphics.DrawPoint(PrjGpOrLocOrDmePoint, 255 * 255, true, false);
                            }
                        }


                        //Draw(PrjAeroport);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("LOC Single Frequency Select Error" + ex.Message);
                    }
                    break;

                case "LOC(Dual Frequency)":

                    try
                    {

                        for (int i = 0; i < myLocalizer.Count; i++)
                        {
                            if (myLocalizer[i].Designator == nodechild.Text)
                            {
                                double uom = Convert.ToDouble(myLocalizer[i].Location.Elevation.Value);
                                double LocMeter = Common.DeConvertHeight(uom);
                                PrjGpOrLocOrDmePoint = GlobalParams.SpatialRefOperation.ToPrj(myLocalizer[i].Location.Geo);
                                //CoordinateListsForGP_OR_LOC_DME list = new CoordinateListsForGP_OR_LOC_DME();
                                //list.ID = myLocalizer[i].Designator.ToString();
                                //list.Coordinate = PrjGpOrLocOrDmePoint;
                                //list.HeightDistance = LocMeter;
                                Navaid_height = LocMeter;
                                //ParamListsGP_LOC_DME.Add(list);
                                //GlobalParams.AranEnvironment.Graphics.DrawPoint(PrjGpOrLocOrDmePoint, 255 * 255, true, false);
                            }
                        }


                        //Draw(PrjAeroport);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("LOC Dual Frequency Select Error" + ex.Message);
                    }
                    break;


                case "DME":

                    try
                    {
                        for (int i = 0; i < mydme.Count; i++)
                        {
                            if (mydme[i].Designator == nodechild.Text)
                            {
                                double uom = Convert.ToDouble(mydme[i].Location.Elevation.Value);
                                double DmeMeter = Common.DeConvertHeight(uom);
                                PrjGpOrLocOrDmePoint = GlobalParams.SpatialRefOperation.ToPrj(mydme[i].Location.Geo);
                                //CoordinateListsForGP_OR_LOC_DME list = new CoordinateListsForGP_OR_LOC_DME();
                                //list.ID = mydme[i].Designator.ToString();
                                //list.Coordinate = PrjGpOrLocOrDmePoint;
                                //list.HeightDistance = DmeMeter;
                                Navaid_height = DmeMeter;
                                //ParamListsGP_LOC_DME.Add(list);
                                //GlobalParams.AranEnvironment.Graphics.DrawPoint(PrjGpOrLocOrDmePoint, 255 * 255, true, false);
                            }
                        }
                        //Draw(PrjAeroport);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("DME Select Error" + ex.Message);
                    }
                    break;



            }

            return Navaid_height;

        }
        public Aran.Geometries.Point IntersectPoint1 = null;
        public Aran.Geometries.Point IntersectPoint2 = null;
        public void DRAWGPOrLocORDMEHarGuidance(Aran.Geometries.Point Navaidptprj, double NavaidHeight, double A, double b, double Segment_Polygon_h, string radius, double D, double Corners_Polygon_H, double L, double f, double TrueBearing, TreeNode enode)
        {

            //polygonSegment {
            Aran.Geometries.Polygon PolygonSegment = new Aran.Geometries.Polygon();
            Aran.Geometries.Point PolygonSegmentMainPoint = new Aran.Geometries.Point();
            Aran.Geometries.MultiLineString Polygon_Segment_Line = new Aran.Geometries.MultiLineString();
            //Set_Parameters_FOR_PolygonS Set_Par_For_Poly = new Set_Parameters_FOR_PolygonS();
            //polygonSegment }
            //PolygonFirstCorner {
            Aran.Geometries.Polygon PolygonFirstCorner = new Aran.Geometries.Polygon();
            Aran.Geometries.MultiLineString Polygon_First_CornerLine = new Aran.Geometries.MultiLineString();
            //PolygonFirstCorner }
            //PolygonSecondtCorner {
            Aran.Geometries.Polygon PolygonSecondCorner = new Aran.Geometries.Polygon();
            Aran.Geometries.MultiLineString Polygon_Second_CornerLine = new Aran.Geometries.MultiLineString();
            //PolygonSecondCorner }


            Draw_Remove_ILS_List DRawILS = new Draw_Remove_ILS_List();

            DRawILS.ChildNode = enode.Text;
            DRawILS.ParentNode = enode.Parent.Text;

            //Obstcl_InputCalcParamList_2DGraphic.Clear();

            double DistanceToTreshold = Math.Sqrt(Math.Pow(PTPrjTRESHOLD.X - Navaidptprj.X, 2) + Math.Pow(PTPrjTRESHOLD.Y - Navaidptprj.Y, 2));

            Double r = new Double();
            if (radius == "a+6000")
            {
                A = DistanceToTreshold;
                r = A + 6000;

                Add_OR_Removee_ToTablistDmeORILS.AddToTabList_ILS(DatGridILSParameters, enode.Parent.Text, enode.Text, "Distance to Treshold:" + DistanceToTreshold.ToString(), b.ToString(), Segment_Polygon_h.ToString(), "A + 6000:" + r.ToString(), D.ToString(), Corners_Polygon_H.ToString(), L.ToString(), f.ToString(), TrueBearing.ToString());
            }
            else
            {
                r = 6000;
                Add_OR_Removee_ToTablistDmeORILS.AddToTabList_ILS(DatGridILSParameters, enode.Parent.Text, enode.Text, A.ToString(), b.ToString(), Segment_Polygon_h.ToString(), r.ToString(), D.ToString(), Corners_Polygon_H.ToString(), L.ToString(), f.ToString(), TrueBearing.ToString());
            }



            Obstacle_ParamListPolygons Obstcl_InputCalcParam = new Obstacle_ParamListPolygons();

            Obstcl_InputCalcParam.Navaid_Point = Navaidptprj;
            Obstcl_InputCalcParam.A = A;
            Obstcl_InputCalcParam.B = b;
            Obstcl_InputCalcParam.Child_NodeTxt = enode.Text;
            Obstcl_InputCalcParam.Parent_NodeTxt = enode.Parent.Text;
            Obstcl_InputCalcParam.F = f;
            Obstcl_InputCalcParam.Delta_L = L - D;
            Obstcl_InputCalcParam.L = L;
            Obstcl_InputCalcParam.Radius = r;
            Obstcl_InputCalcParam.Segment_Polygon_h = Segment_Polygon_h;
            Obstcl_InputCalcParam.Corners_Polygon_H = Corners_Polygon_H;
            Obstcl_InputCalcParam.Navaid_Height = NavaidHeight;

            IntersectPoint1 = null;
            IntersectPoint2 = null;

            //DRAW 2D Graphic {
            IntersectPoint1 = GlobalParams.SpatialRefOperation.ToGeo(IntersectPoint1);
            IntersectPoint2 = GlobalParams.SpatialRefOperation.ToGeo(IntersectPoint2);

            double XCenterNavaid = Navaidptprj.X;
            double YCenterNavaid = Navaidptprj.Y;

            //Square {
            List<Aran.Geometries.Point> pcolsquare = new List<Aran.Geometries.Point>();
            List<Aran.Geometries.Point> pcolCutdistance = new List<Aran.Geometries.Point>();
            List<Aran.Geometries.Point> pcolCutdistance2 = new List<Aran.Geometries.Point>();


            //first successfully
            Aran.Geometries.Point First_Point = new Aran.Geometries.Point();
            First_Point = GlobalParams.SpatialRefOperation.ToPrj(First_Point);
            Navaidptprj.X = XCenterNavaid;
            Navaidptprj.Y = YCenterNavaid;
            Navaidptprj = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);
            double Degree3 = GlobalParams.SpatialRefOperation.AztToDirGeo(Navaidptprj, TrueBearing);
            Navaidptprj = GlobalParams.SpatialRefOperation.ToPrj(Navaidptprj);
            //Degree = Degree * 180;

            double SinA3 = Math.Sin(Degree3);
            double CosA3 = Math.Cos(Degree3);

            double XFirst = Navaidptprj.X - b * CosA3 - D * SinA3;
            double YFirst = Navaidptprj.Y - b * SinA3 + D * CosA3;

            Navaidptprj.X = XFirst;
            Navaidptprj.Y = YFirst;

            First_Point.X = XFirst;
            First_Point.Y = YFirst;

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);

            pcolsquare.Add(new Aran.Geometries.Point(XFirst, YFirst));

            //second successfully
            Aran.Geometries.Point Second_Point = new Aran.Geometries.Point();
            Second_Point = GlobalParams.SpatialRefOperation.ToPrj(Second_Point);

            Navaidptprj.X = XCenterNavaid;
            Navaidptprj.Y = YCenterNavaid;
            Navaidptprj = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);
            double Degree = GlobalParams.SpatialRefOperation.AztToDirGeo(Navaidptprj, TrueBearing);
            Navaidptprj = GlobalParams.SpatialRefOperation.ToPrj(Navaidptprj);

            double SinA = Math.Sin(Degree);
            double CosA = Math.Cos(Degree);
            double XSecond = Navaidptprj.X + A * CosA - D * SinA;
            double YSecond = Navaidptprj.Y + A * SinA + D * CosA;

            Navaidptprj.X = XSecond;
            Navaidptprj.Y = YSecond;

            Second_Point.X = XSecond;
            Second_Point.Y = YSecond;

            IntersectPoint1 = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);

            pcolsquare.Add(new Aran.Geometries.Point(XSecond, YSecond));

            //third successfully
            Aran.Geometries.Point Third_Point = new Aran.Geometries.Point();
            Third_Point = GlobalParams.SpatialRefOperation.ToPrj(Third_Point);

            Navaidptprj.X = XCenterNavaid;
            Navaidptprj.Y = YCenterNavaid;

            Navaidptprj = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);
            double Degree2 = GlobalParams.SpatialRefOperation.AztToDirGeo(Navaidptprj, TrueBearing);
            Navaidptprj = GlobalParams.SpatialRefOperation.ToPrj(Navaidptprj);

            //Degree = Degree * 180;

            double SinA2 = Math.Sin(Degree2);
            double CosA2 = Math.Cos(Degree2);

            double XThird = Navaidptprj.X + A * CosA2 + D * SinA2;
            double YThird = Navaidptprj.Y + A * SinA2 - D * CosA2;

            Navaidptprj.X = XThird;
            Navaidptprj.Y = YThird;

            Third_Point.X = XThird;
            Third_Point.Y = YThird;

            IntersectPoint2 = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);

            pcolsquare.Add(new Aran.Geometries.Point(XThird, YThird));




            //forth succesfully
            Aran.Geometries.Point Forth_Point = new Aran.Geometries.Point();
            Forth_Point = GlobalParams.SpatialRefOperation.ToPrj(Forth_Point);
            Navaidptprj.X = XCenterNavaid;
            Navaidptprj.Y = YCenterNavaid;
            Navaidptprj = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);
            double Degree4 = GlobalParams.SpatialRefOperation.AztToDirGeo(Navaidptprj, TrueBearing);
            Navaidptprj = GlobalParams.SpatialRefOperation.ToPrj(Navaidptprj);

            //Degree = Degree * 180;

            double SinA4 = Math.Sin(Degree4);
            double CosA4 = Math.Cos(Degree4);

            double XForth = Navaidptprj.X - b * CosA4 + D * SinA4;
            double YForth = Navaidptprj.Y - b * SinA4 - D * CosA4;

            Navaidptprj.X = XForth;
            Navaidptprj.Y = YForth;

            Forth_Point.X = XForth;
            Forth_Point.Y = YForth;

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);
            pcolsquare.Add(new Aran.Geometries.Point(XForth, YForth));

            //Add polygon

            Aran.Geometries.Polygon p = new Aran.Geometries.Polygon();

            for (int w = 0; w < pcolsquare.Count; w++)
            {
                p.ExteriorRing.Add(pcolsquare[w]);
            }

            DRawILS.Square = DrawBlueMEthodPointCollection(p);//Polygon int number for Remove Grpahic

            //}



            // First L Distance from NavaidCenter down Truebearing
            double angle = f;
            double radians = angle * (Math.PI / 180);
            double tangensf = Math.Tan(radians);
            double DeltaL = L - D;
            double KatetUcbucaq = DeltaL / tangensf;
            double CutDistance = A + b + KatetUcbucaq;

            Navaidptprj.X = XCenterNavaid;
            Navaidptprj.Y = YCenterNavaid;
            Navaidptprj = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);
            Degree = GlobalParams.SpatialRefOperation.AztToDirGeo(Navaidptprj, TrueBearing);
            Navaidptprj = GlobalParams.SpatialRefOperation.ToPrj(Navaidptprj);

            //Degree = Degree * 180;

            SinA = Math.Sin(Degree);
            CosA = Math.Cos(Degree);

            double XL1 = Navaidptprj.X - b * CosA - L * SinA;
            double YL1 = Navaidptprj.Y - b * SinA + L * CosA;

            Navaidptprj.X = XL1;
            Navaidptprj.Y = YL1;

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);

            double XCutDistance = Navaidptprj.X + CutDistance * CosA;
            double YCutDistance = Navaidptprj.Y + CutDistance * SinA;

            Navaidptprj.X = XCutDistance;
            Navaidptprj.Y = YCutDistance;

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);

            pcolCutdistance.Add(new Aran.Geometries.Point(XFirst, YFirst));
            pcolCutdistance.Add(new Aran.Geometries.Point(XL1, YL1));
            pcolCutdistance.Add(new Aran.Geometries.Point(XCutDistance, YCutDistance));
            pcolCutdistance.Add(new Aran.Geometries.Point(XSecond, YSecond));

            Aran.Geometries.Polygon FirstCornerPolygon = new Aran.Geometries.Polygon();

            for (int w = 0; w < pcolCutdistance.Count; w++)
            {
                FirstCornerPolygon.ExteriorRing.Add(pcolCutdistance[w]);
            }


            DRawILS.FirstCornerPolygon = DrawRedMEthodPointCollection(FirstCornerPolygon);//Polygon int number for Remove Grpahic

            PolygonFirstCorner = FirstCornerPolygon;

            Obstcl_InputCalcParam.First_Corner_Polygon = FirstCornerPolygon;

            // Second L Distance from NavaidCenter down Truebearing

            Navaidptprj.X = XCenterNavaid;
            Navaidptprj.Y = YCenterNavaid;
            Navaidptprj = GlobalParams.SpatialRefOperation.ToGeo(Navaidptprj);
            Degree = GlobalParams.SpatialRefOperation.AztToDirGeo(Navaidptprj, TrueBearing);
            Navaidptprj = GlobalParams.SpatialRefOperation.ToPrj(Navaidptprj);

            //Degree = Degree * 180;

            SinA = Math.Sin(Degree);
            CosA = Math.Cos(Degree);

            double XL2 = Navaidptprj.X - b * CosA + L * SinA;
            double YL2 = Navaidptprj.Y - b * SinA - L * CosA;

            Navaidptprj.X = XL2;
            Navaidptprj.Y = YL2;

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);

            double XCutDistance2 = Navaidptprj.X + CutDistance * CosA;
            double YCutDistance2 = Navaidptprj.Y + CutDistance * SinA;

            Navaidptprj.X = XCutDistance2;
            Navaidptprj.Y = YCutDistance2;

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(Navaidptprj, 255 * 255, true, false);

            pcolCutdistance2.Add(new Aran.Geometries.Point(XL2, YL2));
            pcolCutdistance2.Add(new Aran.Geometries.Point(XForth, YForth));
            pcolCutdistance2.Add(new Aran.Geometries.Point(XThird, YThird));
            pcolCutdistance2.Add(new Aran.Geometries.Point(XCutDistance2, YCutDistance2));

            Aran.Geometries.Polygon PolygonSecCorner = new Aran.Geometries.Polygon();

            for (int w = 0; w < pcolCutdistance2.Count; w++)
            {
                PolygonSecCorner.ExteriorRing.Add(pcolCutdistance2[w]);
            }

            DRawILS.SecondCornerPolygon = DrawNearAquaMEthodPointCollection(PolygonSecCorner);//Polygon int number for Remove Grpahic

            PolygonSecondCorner = PolygonSecCorner;

            Obstcl_InputCalcParam.Second_Corner_Polygon = PolygonSecCorner;

            //Berbareyanli Trapesiya h distance

            //Intersect Circle begin

            Navaidptprj.X = XCenterNavaid;
            Navaidptprj.Y = YCenterNavaid;

            Aran.Geometries.Point pt11 = new Aran.Geometries.Point();
            Aran.Geometries.Point pt12 = new Aran.Geometries.Point();
            Aran.Geometries.Point pt21 = new Aran.Geometries.Point();
            Aran.Geometries.Point pt22 = new Aran.Geometries.Point();

            pt11.X = XCutDistance;
            pt11.Y = YCutDistance;
            pt12.X = XSecond;
            pt12.Y = YSecond;
            pt21.X = XCutDistance2;
            pt21.Y = YCutDistance2;
            pt22.X = XThird;
            pt22.Y = YThird;

            Intersection_FunctionsOROhters IntersectFunction = new Intersection_FunctionsOROhters();

            Aran.Geometries.Point PTRadiusCenter = IntersectFunction.GetLinesIntersection(pt11, pt12, pt21, pt22);

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(PTRadiusCenter, 255 * 255, true, false);

            Aran.Geometries.Point PtLineIntersectCircle1 = IntersectFunction.ClosestIntersection(Navaidptprj.X, Navaidptprj.Y, r, pt11, pt12);
            Aran.Geometries.Point PtLineIntersectCircle2 = IntersectFunction.ClosestIntersection(Navaidptprj.X, Navaidptprj.Y, r, pt21, pt22);

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(PtLineIntersectCircle1, 255 * 255, true, false);
            //GlobalParams.AranEnvironment.Graphics.DrawPoint(PtLineIntersectCircle2, 255 * 255, true, false);

            Aran.Geometries.MultiPoint multipoint = new Aran.Geometries.MultiPoint();

            multipoint.Add(pt12);

            ARANFunctions.AddArcToMultiPoint(Navaidptprj, PtLineIntersectCircle1, PtLineIntersectCircle2, TurnDirection.CW, ref multipoint);

            multipoint.Add(pt22);

            Aran.Geometries.Polygon polygon_segment = new Aran.Geometries.Polygon();

            for (int w = 0; w < multipoint.Count; w++)
            {
                polygon_segment.ExteriorRing.Add(multipoint[w]);
            }

            PolygonSegment = polygon_segment;
            Obstcl_InputCalcParam.Segment_Polygon = polygon_segment;

            DRawILS.SegmentPolygon = DrawCircle_OR_PolygonSegment(polygon_segment);

            //DRAW 2D Graphic }

            //PolygonSegment Calculation therefore Main(Basic) Point {

            PolygonSegmentMainPoint = GlobalParams.SpatialRefOperation.ToPrj(PolygonSegmentMainPoint);

            SinA = Math.Sin(Degree);
            CosA = Math.Cos(Degree);

            double Treshold_Distance = A;

            double XTreshold_A_Distance = Navaidptprj.X + Treshold_Distance * CosA;
            double YTreshold_A_Distance = Navaidptprj.Y + Treshold_Distance * SinA;

            PolygonSegmentMainPoint.X = XTreshold_A_Distance;
            PolygonSegmentMainPoint.Y = YTreshold_A_Distance;

            //GlobalParams.AranEnvironment.Graphics.DrawPoint(PolygonSegmentMainPoint, 255 * 255, true, false);

            //PolygonSegment Calculation therefore Main(Basic) Point }

            //LineFor_PolygonSegment_Calc {
            Second_Point = GlobalParams.SpatialRefOperation.ToGeo(Second_Point);
            Third_Point = GlobalParams.SpatialRefOperation.ToGeo(Third_Point);
            Aran.Geometries.MultiPoint multipt = new MultiPoint();
            multipt.Add(Second_Point);
            multipt.Add(Third_Point);
            Aran.Geometries.LineString SegmentMainlinstring = new Aran.Geometries.LineString();
            SegmentMainlinstring.AddMultiPoint(multipt);
            Polygon_Segment_Line.Add(SegmentMainlinstring);
            Obstcl_InputCalcParam.SegmentPoly_Line = Polygon_Segment_Line;//Obstacle Input Calc add Parameter SegmentPolygon line
            //LineFor_PolygonSegment_Calc }
            //LineFOR_FirstCornerPolygon {
            First_Point = GlobalParams.SpatialRefOperation.ToGeo(First_Point);
            //Second_Point = GlobalParams.SpatialRefOperation.ToGeo(Second_Point);
            Aran.Geometries.MultiPoint Multi_ptForFirstCorner = new MultiPoint();
            Multi_ptForFirstCorner.Add(First_Point);
            Multi_ptForFirstCorner.Add(Second_Point);
            Aran.Geometries.LineString linstringForFirstCorner = new Aran.Geometries.LineString();
            linstringForFirstCorner.AddMultiPoint(Multi_ptForFirstCorner);
            Polygon_First_CornerLine.Add(linstringForFirstCorner);
            Obstcl_InputCalcParam.FirstCornerPoly_Line = Polygon_First_CornerLine;//Obstacle Input Calc add Parameter FirstCornerPolygon line
            //LineFOR_FirstCornerPolygon }
            //LineFOR_SecondCornerPolygon {            
            Forth_Point = GlobalParams.SpatialRefOperation.ToGeo(Forth_Point);
            //Third_Point = GlobalParams.SpatialRefOperation.ToGeo(Third_Point);
            Aran.Geometries.MultiPoint Multi_ptForSecCorner = new MultiPoint();
            Multi_ptForSecCorner.Add(Forth_Point);
            Multi_ptForSecCorner.Add(Third_Point);
            Aran.Geometries.LineString linstringForSecCorner = new Aran.Geometries.LineString();
            linstringForSecCorner.AddMultiPoint(Multi_ptForSecCorner);
            Polygon_Second_CornerLine.Add(linstringForSecCorner);
            Obstcl_InputCalcParam.SecondCornerPoly_Line = Polygon_Second_CornerLine;//Obstacle Input Calc add Parameter SecondCornerPolygon line
                                                                                    //LineFOR_SecondtCornerPolygon }

            //Set_Parameters_FOR_Poly {
            //Set_Par_For_Poly.a_par = A;
            //Set_Par_For_Poly.h_par = Segment_Polygon_h;
            // Set_Par_For_Poly.r_par = r;
            // Set_Par_For_Poly.H_par = Corners_Polygon_H;
            // Set_Par_For_Poly.Delta_L = L - D;
            // Set_Par_For_Poly.Line_PT_Second = Second_Point;
            // Set_Par_For_Poly.Line_PT_Third = Third_Point;
            //Set_Parameters_FOR_Poly }

            Obstcl_CalcParamList_2DGraphic.Add(Obstcl_InputCalcParam);//Obstacle Input_ParamListFor_2DGraphic

            DrawREmoveListILS.Add(DRawILS);
        }
        int IlsPolygonnumber = 0;
        private int DrawNearAquaMEthodPointCollection(Aran.Geometries.Polygon Pointcoll)
        {
            IlsPolygonnumber++;
            int numberreturn = IlsPolygonnumber;
            Aran.AranEnvironment.Symbols.FillSymbol _polygonFillSymbolCutdistance2 = new Aran.AranEnvironment.Symbols.FillSymbol
            {
                Color = 242424,
                Outline = new Aran.AranEnvironment.Symbols.LineSymbol(eLineStyle.slsDash,
                    Aran.PANDA.Common.ARANFunctions.RGB(154, 168, 159), 2),
                Style = eFillStyle.sfsBackwardDiagonal
            };

            numberreturn = GlobalParams.AranEnvironment.Graphics.DrawPolygon(Pointcoll, _polygonFillSymbolCutdistance2, true, false);
            return numberreturn;
        }
        private int DrawRedMEthodPointCollection(Aran.Geometries.Polygon Pointcoll)
        {
            IlsPolygonnumber++;
            int numberreturn = IlsPolygonnumber;
            Aran.AranEnvironment.Symbols.FillSymbol _polygonFillSymbolCutdistance2 = new Aran.AranEnvironment.Symbols.FillSymbol
            {
                Color = 242424,
                Outline = new Aran.AranEnvironment.Symbols.LineSymbol(eLineStyle.slsDash,
                    Aran.PANDA.Common.ARANFunctions.RGB(247, 95, 95), 2),
                Style = eFillStyle.sfsBackwardDiagonal
            };

            numberreturn = GlobalParams.AranEnvironment.Graphics.DrawPolygon(Pointcoll, _polygonFillSymbolCutdistance2, true, false);
            return numberreturn;
        }
        private int DrawBlueMEthodPointCollection(Aran.Geometries.Polygon Pointcoll)
        {
            IlsPolygonnumber++;
            int number = IlsPolygonnumber;
            Aran.AranEnvironment.Symbols.FillSymbol _polygonFillSymbolCutdistance2 = new Aran.AranEnvironment.Symbols.FillSymbol
            {
                Color = 242424,
                Outline = new Aran.AranEnvironment.Symbols.LineSymbol(eLineStyle.slsDash,
                    Aran.PANDA.Common.ARANFunctions.RGB(0, 0, 255), 2),
                Style = eFillStyle.sfsDiagonalCross
            };

            number = GlobalParams.AranEnvironment.Graphics.DrawPolygon(Pointcoll, _polygonFillSymbolCutdistance2, true, false);
            return number;
        }
        int pointsCount = 360;
        int polygonnumberforremovedraw = 0;
        private int DrawCircle(Aran.Geometries.Point center, double radius, string size)
        {

            int removegraphic = 0;
            Aran.Geometries.Polygon p = new Aran.Geometries.Polygon();
            List<Aran.Geometries.Point> pcol = new List<Aran.Geometries.Point>();
            double slice = 2 * Math.PI / pointsCount;
            for (int i = 0; i <= pointsCount; i++)
            {
                double rad = slice * i;
                double px = center.X + radius * Math.Cos(rad);
                double py = center.Y + radius * Math.Sin(rad);
                pcol.Add(new Aran.Geometries.Point(px, py));
            }
            for (int w = 0; w < pcol.Count; w++)
            {
                p.ExteriorRing.Add(pcol[w]);
            }
            if (size == "small")
            {
                removegraphic = DrawCircle_OR_PolygonSegment(p);
            }
            if (size == "middle")
            {
                removegraphic = DrwCircleRed(p);
            }
            if (size == "large")
            {
                removegraphic = DrwCircleAqua(p);
            }
            return removegraphic;
        }
        public int DrawCircle_OR_PolygonSegment(Aran.Geometries.Polygon polygnon)
        {
            polygonnumberforremovedraw++;
            int graphicnumber = polygonnumberforremovedraw;

            Aran.AranEnvironment.Symbols.FillSymbol _polygonFillSymbol = new Aran.AranEnvironment.Symbols.FillSymbol
            {
                Color = 242424,
                Outline = new Aran.AranEnvironment.Symbols.LineSymbol(eLineStyle.slsDash,
                    Aran.PANDA.Common.ARANFunctions.RGB(68, 100, 255), 2),
                Style = eFillStyle.sfsHorizontal
            };

            graphicnumber = GlobalParams.AranEnvironment.Graphics.DrawPolygon(polygnon, _polygonFillSymbol, true, false);

            return graphicnumber;

        }
        public int DrwCircleRed(Aran.Geometries.Polygon polygnon)
        {
            polygonnumberforremovedraw++;
            int graphicnumber = polygonnumberforremovedraw;

            Aran.AranEnvironment.Symbols.FillSymbol _polygonFillSymbol = new Aran.AranEnvironment.Symbols.FillSymbol
            {
                Color = 242424,
                Outline = new Aran.AranEnvironment.Symbols.LineSymbol(eLineStyle.slsDash,
                    Aran.PANDA.Common.ARANFunctions.RGB(247, 95, 95), 2),
                Style = eFillStyle.sfsHorizontal
            };

            graphicnumber = GlobalParams.AranEnvironment.Graphics.DrawPolygon(polygnon, _polygonFillSymbol, true, false);

            return graphicnumber;

        }
        public int DrwCircleAqua(Aran.Geometries.Polygon polygnon)
        {
            polygonnumberforremovedraw++;
            int graphicnumber = polygonnumberforremovedraw;

            Aran.AranEnvironment.Symbols.FillSymbol _polygonFillSymbol = new Aran.AranEnvironment.Symbols.FillSymbol
            {
                Color = 242424,
                Outline = new Aran.AranEnvironment.Symbols.LineSymbol(eLineStyle.slsDash,
                    Aran.PANDA.Common.ARANFunctions.RGB(154, 168, 159), 2),
                Style = eFillStyle.sfsHorizontal
            };

            graphicnumber = GlobalParams.AranEnvironment.Graphics.DrawPolygon(polygnon, _polygonFillSymbol, true, false);

            return graphicnumber;

        }
        private void BtnClear_Click(object sender, EventArgs e)
        {
            RemoveAll_DRAW();

            foreach (TreeNode node1 in TrwViewNavaids.Nodes)
            {
                if (node1.Text == "Markers" || node1.Text == "NDB")
                {
                    foreach (TreeNode node2 in node1.Nodes)
                    {
                        node2.Checked = false;
                    }
                }
                else
                {
                    foreach (TreeNode node2 in node1.Nodes)
                    {
                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            node3.Checked = false;
                        }
                    }
                }
            }


            TrwViewNavaids.CollapseAll();


        }

        private void DGridVNvidsParameters_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void DGridVNvidsParameters_MouseClick(object sender, MouseEventArgs e)
        {
            int currentMouseOverRow = DGridVNvidsParameters.HitTest(e.X, e.Y).RowIndex;

            for (int x = 0; x < DGridVNvidsParameters.Rows.Count; x++)
            {
                if (DGridVNvidsParameters.Rows[x].Index == currentMouseOverRow)
                {
                    DGridVNvidsParameters.Rows[x].Selected = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show(progressBar1.Value.ToString());
            //MessageBox.Show("called");
            //if (progressBar1.Value == progressBar1.Maximum)
            //{
            //    progressBar1.Value = progressBar1.Minimum;
            //    return;
            //}
            //progressBar1.PerformStep();
        }
        //AranTool itemtools = new AranTool();
        private void button1_Click(object sender, EventArgs e)
        {
            ObtalceInputDialog ObstclDialgInput = new ObtalceInputDialog(_aranToolItem);
            if (ParamListCVOR.Count > 0 || ParamListDVOR.Count > 0 || ParamListDMEN.Count > 0 || ParamListMarker.Count > 0 || ParamListNDB.Count > 0 || Obstcl_CalcParamList_2DGraphic.Count > 0)
            {
                //ObstclDialgInput.AddListParameters(ParamListDMEN, ParamListCVOR, ParamListDVOR, ParamListMarker, ParamListNDB, Obstcl_CalcParamList_2DGraphic);
                ObstacleInputAddParameterList.AddParameter(ParamListDMEN, ParamListCVOR, ParamListDVOR, ParamListMarker, ParamListNDB, Obstcl_CalcParamList_2DGraphic);
            }
            ObstclDialgInput.Show(GlobalParams.AranEnvironment.Win32Window);
        }
        //private void BtnObsSortGroup_Click(object sender, EventArgs e)
        //{

        //    List<IlyasMclass> ListElCorOrg = new List<IlyasMclass>();
        //    ElevatedPoint pointelevated;
        //    List<VerticalStructure> myList = VerticalStrList();

        //    for (int r = 0; r < myList.Count; r++)
        //    {
        //        long ObsID = myList[r].Id;
        //        foreach (VerticalStructurePart part in myList[r].Part)
        //        {

        //            if (part.HorizontalProjection == null)
        //                continue;

        //            switch (part.HorizontalProjection.Choice)
        //            {

        //                case VerticalStructurePartGeometryChoice.ElevatedPoint:
        //                    pointelevated = part.HorizontalProjection.Location;
        //                    string geotype = pointelevated.Geo.Type.ToString();
        //                    if (pointelevated == null) continue;
        //                    if (pointelevated.Elevation == null) continue;
        //                    Aran.Geometries.Point ptrjpoint = GlobalParams.SpatialRefOperation.ToPrj(pointelevated.Geo);
        //                    IlyasMclass lst = new IlyasMclass();
        //                    lst.Point = ptrjpoint;
        //                    lst.ID = ObsID;
        //                    double uom = pointelevated.Elevation.Value;
        //                    lst.Elevation = Common.DeConvertHeight(uom);
        //                    ListElCorOrg.Add(lst);

        //                    break;
        //            }
        //        }
        //    }
        //    double R = 1750;
        //    double meter;

        //    List<IlyasMclass> listelcororg1 = new List<IlyasMclass>();
        //    listelcororg1 = ListElCorOrg.OrderByDescending(d => d.Elevation).ToList();

        //    for (int d = 0; d < listelcororg1.Count; d++)
        //    {
        //        Aran.Geometries.Point ptstart = listelcororg1[d].Point;
        //        //double Y;
        //        for (int u = 0; u < listelcororg1.Count; u++)
        //        {
        //            if (ptstart != listelcororg1[u].Point)
        //            {
        //                meter = Math.Pow(listelcororg1[u].Point.X - ptstart.X, 2) + Math.Pow(listelcororg1[u].Point.Y - ptstart.Y, 2);

        //                if (meter < Math.Pow(R, 2))
        //                {
        //                    listelcororg1.Remove(listelcororg1[u]);
        //                }
        //            }
        //        }
        //        //meter = Math.Pow(PtEnd.X - PtStart.X, 2) + Math.Pow(PtEnd.Y - PtStart.Y, 2);
        //    }

        //    for (int h = 0; h < listelcororg1.Count; h++)
        //    {
        //        //MessageBox.Show(listelcororg1[h].Elevation.ToString());
        //    }


        //}
        public void ObstacleElevationCalculate(Aran.Geometries.Point ObstaclePoint)
        {
            MessageBox.Show("X:  " + ObstaclePoint.X + "\n" + "Y:  " + ObstaclePoint.Y);
            throw new NotImplementedException();
        }

        private int HighlightedRowIndex;

        private void DatGridVRunwayList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DatGridVRunwayList.ClearSelection();

        }
        private void DatGridVRunwayList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DatGridVRunwayList.ClearSelection();
        }

        private void DatGridVRunwayList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                ClickRowColorMethod(e.RowIndex);
                RunwayDirListAddToCombo(e.RowIndex);
                TresholdFindUseRwyCenterLinePoint(ObjComboRwyDirList.SelectedItem.ToString());
            }
        }
        private void RunwayDirListAddToCombo(int Rowindex)
        {
            var value = DatGridVRunwayList.Rows[Rowindex].Cells[0].Value;
            //Guid IdenTifayr = (Guid)DatGridVRunwayList.Rows[Rowindex].Cells[0].Value;
            List<RunwayDirection> NewComboRwydirlist = RwyDirList.Where(id => id.UsedRunway.Identifier.ToString() == value.ToString()).ToList();

            ObjComboRwyDirList.Items.Clear();

            for (int i = 0; i < NewComboRwydirlist.Count; i++)
            {
                ObjComboRwyDirList.Items.Add(NewComboRwydirlist[i].Designator);
            }

            ObjComboRwyDirList.SelectedIndex = 0;

        }
        private void TresholdFindUseRwyCenterLinePoint(string designator)
        {
            for (int i = 0; i < RwyCenterlinePointList.Count; i++)
            {

                if (RwyCenterlinePointList[i].Location == null || RwyCenterlinePointList[i].Role == null)
                    continue;

                if (RwyCenterlinePointList[i].Role.Value.ToString() == "THR")
                {
                    if (RwyCenterlinePointList[i].Designator.ToString() == designator)
                    {
                        Aran.Geometries.Point PtTreshold = RwyCenterlinePointList[i].Location.Geo;
                        PtTreshold = GlobalParams.SpatialRefOperation.ToPrj(PtTreshold);
                        PTPrjTRESHOLD = PtTreshold;
                        //GlobalParams.AranEnvironment.Graphics.DrawPoint(PtTreshold, 255 * 255, true, false);
                    }
                }

            }

        }
        private void ClickRowColorMethod(int index)
        {
            foreach (DataGridViewRow row in DatGridVRunwayList.Rows)
            {
                if (row.Index == HighlightedRowIndex)
                {
                    DatGridVRunwayList.Rows[HighlightedRowIndex].DefaultCellStyle.BackColor = Color.White;
                    DatGridVRunwayList.Rows[HighlightedRowIndex].DefaultCellStyle.SelectionBackColor = Color.White;
                }
            }
            DatGridVRunwayList.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
            DatGridVRunwayList.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.Black;
            foreach (DataGridViewRow row in DatGridVRunwayList.Rows)
            {
                if (row.Index == index)
                {
                    DatGridVRunwayList.Rows[index].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                }
            }
            HighlightedRowIndex = index;
        }

        private void DatGridVRunwayList_Sorted(object sender, EventArgs e)
        {
            DatGridVRunwayList.ClearSelection();
        }

        private void ObjComboRwyDirList_SelectedIndexChanged(object sender, EventArgs e)
        {
            TresholdFindUseRwyCenterLinePoint(ObjComboRwyDirList.SelectedItem.ToString());
        }
        private void MouseHover(CheckBox checkbox, ToolTip ttpShow)
        {
            ttpShow.AutoPopDelay = 2000;
            ttpShow.InitialDelay = 1000;
            ttpShow.ReshowDelay = 500;
            ttpShow.IsBalloon = true;
            ttpShow.SetToolTip(checkbox, "This is only for VOR(CVOR and DVOR)");
            ttpShow.Show("This is only for VOR(CVOR and DVOR)", checkbox, checkbox.Width, checkbox.Height / 10, 5000);
        }
        private void MouseLeave(CheckBox checkbox, ToolTip ttpreomve)
        {
            ttpreomve.AutoPopDelay = 0;
            ttpreomve.InitialDelay = 0;
            ttpreomve.ReshowDelay = 0;
            ttpreomve.IsBalloon = false;
            ttpreomve.Hide(checkbox);
        }
        private void ChkBoxWindTurbine_MouseHover(object sender, EventArgs e)
        {
            MouseHover(ChkBoxWindTurbine, TooltipForWindTurbine);
        }

        private void ChkBoxWindTurbine_MouseLeave(object sender, EventArgs e)
        {
            MouseLeave(ChkBoxWindTurbine, TooltipForWindTurbine);
        }

        private void ChkBoxWindTurbine_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBoxWindTurbine.Checked == true)
            {
                WindTurbineValue = true;
            }
            if (ChkBoxWindTurbine.Checked == false)
            {
                WindTurbineValue = false;
            }

            ObstacleReport.windturbine = WindTurbineValue;

        }

        private void TabNavaidControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            // This event is called once for each tab button in your tab control

            // First paint the background with a color based on the current tab

            // e.Index is the index of the tab in the TabPages collection.
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(new SolidBrush(Color.Blue), e.Bounds);
                    break;
                default:
                    break;
            }

            // Then draw the current tab button text 
            Rectangle paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -2);
            e.Graphics.DrawString(TabNavaidControl.TabPages[e.Index].Text, this.Font, SystemBrushes.HighlightText, paddedBounds);
        }

        private void backgroundWorkeprogres_DoWork(object sender, DoWorkEventArgs e)
        {
            ////backgroundWorkeprogres.ReportProgress(10);
            //try
            //{
            //    Action action = () =>
            //    {


            //        //ObstacleReport obstcl = new ObstacleReport(toolitem);
            //        //obstcl.Show(GlobalParams.AranEnvironment.Win32Window);

            //    };

            //    progressBar1.BeginInvoke(action);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("ERROR:        " + ex.Message);
            //}
            ////backgroundWorkeprogres.ReportProgress(100);
        }

        private void backgroundWorkeprogres_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorkeprogres_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //timer1.Stop();
            //progressBar1.Visible = false;
        }
    }
}