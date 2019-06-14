using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Controls;
using EsriWorkEnvironment;
using ARENA.Environment;

namespace ARENA
{
    public partial class FormLayerProperties : Form
    {
        public IMap mapControl;

        public FormLayerProperties()
        {
            InitializeComponent();

           
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            try
            {

                TreeNode[] SelRootNodeArray = new TreeNode[treeView1.SelectedNode.Nodes.Count];
                TreeNode[] PrevRootNodeArray = new TreeNode[treeView1.SelectedNode.PrevNode.Nodes.Count];

                treeView1.SelectedNode.Nodes.CopyTo(SelRootNodeArray, 0);
                treeView1.SelectedNode.PrevNode.Nodes.CopyTo(PrevRootNodeArray, 0);

                treeView1.SelectedNode.Nodes.Clear();
                treeView1.SelectedNode.PrevNode.Nodes.Clear();

                MoveNode_UP(treeView1.SelectedNode.PrevNode, treeView1.SelectedNode);

                treeView1.SelectedNode.PrevNode.Nodes.AddRange(SelRootNodeArray);
                treeView1.SelectedNode.Nodes.AddRange(PrevRootNodeArray);


                treeView1.SelectedNode = treeView1.SelectedNode.PrevNode;

                treeView1.Select();

                ChangeLayerPosition(treeView1.SelectedNode.Tag as ILayer, true);

            }
            catch { }
            

        }

        private void buttonDn_Click(object sender, EventArgs e)
        {
            try
            {

                TreeNode[] SelRootNodeArray = new TreeNode[treeView1.SelectedNode.Nodes.Count];
                TreeNode[] NextRootNodeArray = new TreeNode[treeView1.SelectedNode.NextNode.Nodes.Count];

                treeView1.SelectedNode.Nodes.CopyTo(SelRootNodeArray, 0);
                treeView1.SelectedNode.NextNode.Nodes.CopyTo(NextRootNodeArray, 0);

                treeView1.SelectedNode.Nodes.Clear();
                treeView1.SelectedNode.NextNode.Nodes.Clear();

                MoveNode_DN(treeView1.SelectedNode.NextNode, treeView1.SelectedNode);

                treeView1.SelectedNode.NextNode.Nodes.AddRange(SelRootNodeArray);
                treeView1.SelectedNode.Nodes.AddRange(NextRootNodeArray);


                treeView1.SelectedNode = treeView1.SelectedNode.NextNode;

                treeView1.Select();

                ChangeLayerPosition(treeView1.SelectedNode.Tag as ILayer, false);

            }
            catch { }
        }

        private void MoveNode_UP(TreeNode prevNode, TreeNode selectedNode)
        {
            if (prevNode == null || selectedNode == null) return;

            string prevNd = selectedNode.PrevNode.Text;
            object prevNdTag = selectedNode.PrevNode.Tag;

            selectedNode.PrevNode.Text = selectedNode.Text;
            selectedNode.PrevNode.Tag = selectedNode.Tag;


            selectedNode.Text = prevNd;
            selectedNode.Tag = prevNdTag;
        }

        private void MoveNode_DN(TreeNode nextNode, TreeNode selectedNode)
        {
            if (nextNode == null || selectedNode == null) return;

            string nextNd = selectedNode.NextNode.Text;
            object nextNdTag = selectedNode.NextNode.Tag;

            selectedNode.NextNode.Text = selectedNode.Text;
            selectedNode.NextNode.Tag = selectedNode.Tag;


            selectedNode.Text = nextNd;
            selectedNode.Tag = nextNdTag;

        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Nodes.Count == 0)
                LayerPropertiesEdit();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            (mapControl as IActiveView).Refresh();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LayerPropertiesEdit();
        }

        private void LayerPropertiesEdit()
        {
            ESRI.ArcGIS.Carto.ILayer l = treeView1.SelectedNode.Tag as ESRI.ArcGIS.Carto.ILayer;

            try
            {
                ILayer L1 = treeView1.SelectedNode.Tag as ILayer;
                if (L1 is IFeatureLayer) 
                {
                    DeleteLayerButton.Enabled = !(System.IO.Path.GetFileNameWithoutExtension(((IFeatureLayer)L1).FeatureClass.FeatureDataset.Workspace.PathName).ToUpper().CompareTo("pdm") ==0);
                    
                }


                EsriUtils._LayerPropertiesEdit(L1, mapControl as IActiveView,0);
                treeView1.Select();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

 
        private void ChangeLayerPosition(ILayer L, bool Up)
        {
            try
            {
                int oper = -1;
                if (!Up) oper = 1;

                for (int i = 0; i <= mapControl.LayerCount - 1; i++)
                {
                    ILayer Ly = mapControl.get_Layer(i);

                    if (L.Name.CompareTo(Ly.Name) == 0)
                    {
                        mapControl.MoveLayer(L, i + oper);
                        break;
                    }


                    if (Ly is ICompositeLayer)
                    {

                        List<ILayer> tmpList = new List<ILayer>();
                        for (int j = 0; j <= ((ICompositeLayer)Ly).Count - 1; j++)
                        {
                            tmpList.Add( ((ICompositeLayer)Ly).get_Layer(j));
                        }

                        int oldPos = tmpList.IndexOf(L);
                        if (oldPos >= 0)
                        {
                            ILayer movedLayer = tmpList[oldPos];
                            ILayer savedLayer = tmpList[oldPos+oper];
                            tmpList[oldPos + oper] = movedLayer;
                            tmpList[oldPos] = savedLayer;

                            ((IGroupLayer)Ly).Clear();

                            foreach(ILayer Lyr in tmpList)
                                ((IGroupLayer)Ly).Add(Lyr);
                        }
                    }

                }
            }
            catch { }

        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DeleteLayerButton.Enabled = false;
            try
            {
                ILayer L1 = treeView1.SelectedNode.Tag as ILayer;
                if (L1 is IFeatureLayer)
                {
                    DeleteLayerButton.Enabled = !(System.IO.Path.GetFileNameWithoutExtension(((IFeatureLayer)L1).FeatureClass.FeatureDataset.Workspace.PathName).ToUpper().CompareTo("PDM") == 0);

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
            }
            treeView1.Refresh();

            
        }

        private void BuildLayersTreeView(IMap Focusmap, bool MarkAsChecked)
        {

            TreeNode nd0 = null;
            TreeNode nd1 = null;
            TreeNode nd2 = null;

            treeView1.Nodes.Clear();

            if (MarkAsChecked) treeView1.CheckBoxes = true;

            for (int i = 0; i <= Focusmap.LayerCount - 1; i++)
            {
                ILayer L = Focusmap.get_Layer(i);

                //if (L.Name.CompareTo("Anno&Decor") == 0) continue;

                nd0 = new TreeNode();
                nd0.Text = L.Name;

                nd0.Name = "mnuLayersProp_" + L.Name;
                nd0.Tag = L;
                nd0.Checked = true;
                treeView1.Nodes.Add(nd0);

                if (L is ICompositeLayer)
                {
                    for (int j = 0; j <= ((ICompositeLayer)L).Count - 1; j++)
                    {

                        ILayer L1 = ((ICompositeLayer)L).get_Layer(j);

                        if (L1 is ICompositeLayer)
                        {
                            for (int k = 0; k <= ((ICompositeLayer)L1).Count - 1; k++)
                            {
                                ILayer L2 = ((ICompositeLayer)L1).get_Layer(k);
                                nd2 = new TreeNode();
                                nd2.Text = L2.Name;
                                nd2.Name = "mnuLayersProp_" + L2.Name;
                                nd2.Tag = L2;
                                nd2.Checked = true;
                                if (nd1 != null) nd1.Nodes.Add(nd2);
                                else nd0.Nodes.Add(nd2);
                            }
                        }
                        else
                        {
                            nd1 = new TreeNode();
                            nd1.Text = L1.Name;
                            nd1.Name = "mnuLayersProp_" + L1.Name;
                            nd1.Tag = L1;
                            nd1.Checked = true;
                            nd0.Nodes.Add(nd1);
                        }
                    }
                }
                //else
                //{
                //    nd0 = new TreeNode();
                //    nd0.Text = L.Name;
                //    nd0.Name = "mnuLayersProp_" + L.Name;
                //    nd0.Tag = L;
                //    nd0.Checked = true;
                //    treeView1.Nodes.Add(nd0);
                //}

            }


        }

        private void FormLayerProperties_Load(object sender, EventArgs e)
        {
            BuildLayersTreeView(mapControl, false);
        }

        private void DeleteLayerButton_Click(object sender, EventArgs e)
        {
            try
            {
                ESRI.ArcGIS.Carto.ILayer l = treeView1.SelectedNode.Tag as ESRI.ArcGIS.Carto.ILayer;

                if (MessageBox.Show("Delete Layer " + l.Name + " ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;

                mapControl.DeleteLayer(l);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
            }
            treeView1.Refresh();
        }

    }
}
