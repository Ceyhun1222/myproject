using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;

namespace MapEnv
{
    public partial class PropertySelectorForm : Form
    {
        private AimClassInfo _classInfo;

        public PropertySelectedEventHandler PropertySelected;
        public SelectorAddedPropInfoEventHandler SelectorAddedPropInfo;

        public PropertySelectorForm ()
        {
            InitializeComponent ();
        }

        public AimClassInfo ClassInfo
        {
            get
            {
                return _classInfo;
            }
            set
            {
                if (value == null)
                    return;

                _classInfo = value;

                FillChilds (ui_treeView.Nodes, _classInfo);
            }
        }

        public void SetSelected (AimPropInfo [] propInfoArr)
        {
            if (propInfoArr == null || propInfoArr.Length == 0)
                return;

            FindNode (0, propInfoArr, ui_treeView.Nodes);
        }


        private void FindNode (int index, AimPropInfo [] propInfoArr, TreeNodeCollection nodeColl)
        {
            AimPropInfo propInfo = propInfoArr [index];

            foreach (TreeNode node in nodeColl)
            {
                if (propInfo.Equals (node.Tag))
                {
                    ui_treeView.SelectedNode = node;
                    node.EnsureVisible ();

                    if (index < (propInfoArr.Length - 1) && node.Nodes.Count > 0)
                    {
                        node.Expand ();
                        FindNode (index + 1, propInfoArr, node.Nodes);
                    }
                    
                    break;
                }
            }
        }

        private TreeNode PropInfoToNode (AimPropInfo propInfo)
        {
            TreeNode treeNode = new TreeNode ();
            treeNode.Tag = propInfo;
            treeNode.Text = propInfo.Name;

            if (propInfo.PropType.AimObjectType == AimObjectType.Object)
            {
                TreeNode loadingNode = new TreeNode ("Loading...");
                loadingNode.NodeFont = new Font (ui_treeView.Font, FontStyle.Italic);
                treeNode.Nodes.Add (loadingNode);
            }

            return treeNode;
        }

        private void FillChilds (TreeNodeCollection treeNodeCollection, AimClassInfo classInfo)
        {
            foreach (AimPropInfo propInfo in classInfo.Properties)
            {
                if (propInfo.Name == "Id")
                    continue;

                if (SelectorAddedPropInfo != null)
                {
                    SelectorAddedPropInfoEventArgs se = new SelectorAddedPropInfoEventArgs (propInfo);
                    SelectorAddedPropInfo (this, se);
                    if (!se.AddToList)
                        continue;
                }

                TreeNode treeNode = PropInfoToNode (propInfo);
                treeNodeCollection.Add (treeNode);
            }
        }

        private void uiEvents_treeView_BeforeExpand (object sender, TreeViewCancelEventArgs e)
        {
            AimPropInfo propInfo = e.Node.Tag as AimPropInfo;
            if (propInfo == null)
                return;

            if (e.Node.Nodes.Count == 1 && e.Node.Nodes [0].Tag == null)
            {
                e.Node.Nodes.Clear ();
                FillChilds (e.Node.Nodes, propInfo.PropType);
            }
        }

        private void uiEvents_treeView_NodeMouseDoubleClick (object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null || PropertySelected == null)
                return;

            List<AimPropInfo> propInfoList = new List<AimPropInfo> ();
            TreeNode treeNode = e.Node;

            while (treeNode != null)
            {
                propInfoList.Add ((AimPropInfo) treeNode.Tag);
                treeNode = treeNode.Parent;
            }

            propInfoList.Reverse ();

            PropertySelectedEventArgs propSelected = new PropertySelectedEventArgs (propInfoList.ToArray ());
            PropertySelected (this, propSelected);

            if (propSelected.Cancel)
                return;
            
            Close ();
        }

        private void PropertySelectorForm_Deactivate (object sender, EventArgs e)
        {
            Close ();
        }
    }
}
