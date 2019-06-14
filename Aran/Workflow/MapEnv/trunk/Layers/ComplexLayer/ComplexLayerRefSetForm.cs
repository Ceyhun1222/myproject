using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Metadata.UI;
using MapEnv.Layers;

namespace MapEnv.ComplexLayer
{
    public partial class ComplexLayerRefSetForm : Form
    {
        private FeatureType _featureType;
        private AimClassInfo _aimClassInfo;
        private Dictionary<UIReferenceInfo, int> _uiClassInfoIndexOrder;

        public event EventHandler OKClicked;

        public ComplexLayerRefSetForm ()
        {
            InitializeComponent ();

            _uiClassInfoIndexOrder = new Dictionary<UIReferenceInfo, int> ();

            ui_treeViewImageList.Images.Add (MapEnv.Properties.Resources.ref_left_32);
            ui_treeViewImageList.Images.Add (MapEnv.Properties.Resources.ref_right_32);

            SetTreeViewSize ();
        }

        public FeatureType FeatureType
        {
            get
            {
                return _featureType;
            }
            set
            {
                SetFeatureType (value);
            }
        }

        public void GetQueryInfo (List <RefQueryInfo> refList, List <SubQueryInfo> subList)
        {
            foreach (TreeNode treeNode in ui_rightTreeView.Nodes)
            {
                var uiRefInfo = treeNode.Tag as UIReferenceInfo;

                foreach (TreeNode subNode in treeNode.Nodes)
                {
                    var prPathInfo = subNode.Tag as PropertyPathInfo;

                    if (uiRefInfo.Direction == PropertyDirection.Ref)
                    {
                        var refQI = new RefQueryInfo ();
                        refQI.PropertyPath = prPathInfo.Name;
                        refQI.QueryInfo = new QueryInfo ((FeatureType) uiRefInfo.ClassInfo.Index);

                        refList.Add (refQI);
                    }
                    else
                    {
                        var subQI = new SubQueryInfo ();
                        subQI.PropertyPath = prPathInfo.Name;
                        subQI.QueryInfo = new QueryInfo ((FeatureType) uiRefInfo.ClassInfo.Index);

                        subList.Add (subQI);
                    }
                }
            }
        }


        private void SetFeatureType (FeatureType value)
        {
            _featureType = value;
            ui_titleLabel.Text = value.ToString ();

            _aimClassInfo = AimMetadata.GetClassInfoByIndex ((int) value);
            var uiClassInfo = _aimClassInfo.UiInfo ();

            foreach (UIReferenceInfo uiRefInfo in uiClassInfo.RefInfoList)
            {
                var treeNode = new TreeNode ();
                ToTreeNode (uiRefInfo, treeNode);

                foreach (PropertyPathInfo prPathInfo in uiRefInfo.PropertyPath)
                {
                    var propTreeNode = new TreeNode ();
                    propTreeNode.Tag = prPathInfo;
                    propTreeNode.Text = prPathInfo.Name;
                    propTreeNode.ImageIndex = treeNode.ImageIndex;
                    propTreeNode.SelectedImageIndex = treeNode.SelectedImageIndex;

                    treeNode.Nodes.Add (propTreeNode);
                }

                ui_leftTreeView.Nodes.Add (treeNode);
                _uiClassInfoIndexOrder.Add (uiRefInfo, treeNode.Index);
            }
        }

        private void ToTreeNode (UIReferenceInfo uiRefInfo, TreeNode treeNode)
        {
            treeNode.Tag = uiRefInfo;
            treeNode.Text = uiRefInfo.ClassInfo.AixmName;
            var imgInd = (uiRefInfo.Direction == PropertyDirection.Sub ? 1 : 0);
            treeNode.ImageIndex = imgInd;
            treeNode.SelectedImageIndex = imgInd;
        }

        private void Cancel_Click (object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OK_Click (object sender, EventArgs e)
        {
            if (OKClicked == null)
                return;



            OKClicked (this, e);
        }

        private void SetTreeViewSize ()
        {
            var size = new Size (Width / 2 - 60,
                Height - ui_titleLabel.Height - 90);

            ui_leftTreeView.Size = size;
            ui_rightTreeView.Size = size;
        }

        private void ComplexLayerRefSetForm_SizeChanged (object sender, EventArgs e)
        {
            SetTreeViewSize ();
        }

        private void AddToRight_Click (object sender, EventArgs e)
        {
            var removedList = new List<TreeNode> ();

            foreach (TreeNode treeNode in ui_leftTreeView.Nodes)
            {
                foreach (TreeNode subTreeNode in treeNode.Nodes)
                {
                    if (subTreeNode.Checked)
                    {
                        var uiRefInfo = treeNode.Tag as UIReferenceInfo;
                        var prPathInfo = subTreeNode.Tag as PropertyPathInfo;
                        AddToRight (uiRefInfo.ClassInfo, uiRefInfo.Direction, prPathInfo);
                        removedList.Add (subTreeNode);
                        break;
                    }
                }
            }

            foreach (var tn in removedList)
            {
                var parNode = tn.Parent;
                tn.Remove ();
                if (parNode.Nodes.Count == 0)
                    parNode.Remove ();
            }
        }

        private void RemoveToLeft_Click (object sender, EventArgs e)
        {
			var selNode = ui_rightTreeView.SelectedNode;
			if (selNode == null || selNode.Parent == null)
			{
				return;
			}

			var uiRefInfo = selNode.Parent.Tag as UIReferenceInfo;
			var prPathInfo = selNode.Tag as PropertyPathInfo;

			if (selNode.Parent.Nodes.Count == 1)
				selNode.Parent.Remove ();
			else
				selNode.Remove ();
			
			AddToLeft (uiRefInfo.ClassInfo, uiRefInfo.Direction, prPathInfo);


			//var removedList = new List<TreeNode> ();

			//foreach (TreeNode treeNode in ui_rightTreeView.Nodes)
			//{
			//    foreach (TreeNode subTreeNode in treeNode.Nodes)
			//    {
			//        if (subTreeNode.Checked)
			//        {
			//            var uiRefInfo = treeNode.Tag as UIReferenceInfo;
			//            var prPathInfo = subTreeNode.Tag as PropertyPathInfo;
			//            AddToLeft (uiRefInfo.ClassInfo, uiRefInfo.Direction, prPathInfo);
			//            removedList.Add (subTreeNode);
			//            break;
			//        }
			//    }
			//}

			//foreach (var tn in removedList)
			//{
			//    var parNode = tn.Parent;
			//    tn.Remove ();
			//    if (parNode.Nodes.Count == 0)
			//        parNode.Remove ();
			//}
        }

        private void AddToRight (AimClassInfo classInfo, PropertyDirection prDir, PropertyPathInfo prPathInfo)
        {
            TreeNode sTreeNode = null;

            foreach (TreeNode treeNode in ui_rightTreeView.Nodes)
            {
                var uiRefInfo = treeNode.Tag as UIReferenceInfo;
                if (uiRefInfo.ClassInfo == classInfo)
                {
                    sTreeNode = treeNode;
                    break;
                }
            }

            if (sTreeNode == null)
            {
                var newUiRefInfo = new UIReferenceInfo ();
                newUiRefInfo.ClassInfo = classInfo;
                newUiRefInfo.Direction = prDir;

                sTreeNode = new TreeNode ();
                ToTreeNode (newUiRefInfo, sTreeNode);
                ui_rightTreeView.Nodes.Add (sTreeNode);
            }

            var sUiRefInfo = sTreeNode.Tag as UIReferenceInfo;
            sUiRefInfo.PropertyPath.Add (prPathInfo);

            var propTreeNode = new TreeNode ();
            propTreeNode.Tag = prPathInfo;
            propTreeNode.Text = prPathInfo.Name;
            propTreeNode.ImageIndex = sTreeNode.ImageIndex;
            propTreeNode.SelectedImageIndex = sTreeNode.SelectedImageIndex;

            sTreeNode.Nodes.Add (propTreeNode);
            sTreeNode.Expand ();
        }

        private void AddToLeft (AimClassInfo classInfo, PropertyDirection prDir, PropertyPathInfo prPathInfo)
        {
            TreeNode sTreeNode = null;

            foreach (TreeNode treeNode in ui_leftTreeView.Nodes)
            {
                var uiRefInfo = treeNode.Tag as UIReferenceInfo;
                if (uiRefInfo.ClassInfo == classInfo)
                {
                    sTreeNode = treeNode;
                    break;
                }
            }

            if (sTreeNode == null)
            {
                var uiClassInfo = _aimClassInfo.UiInfo ();
                UIReferenceInfo uiRefInfo = null;
                foreach (var itemUiRefInfo in uiClassInfo.RefInfoList)
                {
                    if (itemUiRefInfo.ClassInfo == classInfo)
                    {
                        uiRefInfo = itemUiRefInfo;
                        break;
                    }
                }

                sTreeNode = new TreeNode ();
                ToTreeNode (uiRefInfo, sTreeNode);

                int index = _uiClassInfoIndexOrder [uiRefInfo];
                ui_leftTreeView.Nodes.Insert (index, sTreeNode);
            }

            var propTreeNode = new TreeNode ();
            propTreeNode.Tag = prPathInfo;
            propTreeNode.Text = prPathInfo.Name;
            propTreeNode.ImageIndex = sTreeNode.ImageIndex;
            propTreeNode.SelectedImageIndex = sTreeNode.SelectedImageIndex;

            sTreeNode.Nodes.Add (propTreeNode);
        }
    }
}
