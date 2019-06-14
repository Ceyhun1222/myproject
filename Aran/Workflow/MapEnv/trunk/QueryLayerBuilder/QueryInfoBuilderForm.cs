using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapEnv.QueryLayer;
using Aran.Aim;
using System.IO;
using Aran.Aim.Package;
using Aran.Aim.Env2.Layers;

namespace MapEnv
{
    public partial class QueryInfoBuilderForm : Form
    {
        private bool _saveQueryInfoSymbol;
        private bool _saveQueryInfoFilter;
        private QueryInfoNodeTag _currentQueryTag;
        private QueryInfoNodeTag _prevQueryTag;

        public event EventHandler OKClicked;
        private Aran.Aim.Data.User SelectedUser;
        private Aran.Aim.Data.User _user;

        public QueryInfoBuilderForm ()
        {
            InitializeComponent ();

            ui_addQIButton.Visible = true;

            ui_filterControl.FeatureDescription += Globals.AimPropertyControl_FeatureDescription;
            ui_filterControl.FillDataGridColumnsHandler = Globals.AimPropertyControl_FillDataGridColumn;
            ui_filterControl.SetDataGridRowHandler = Globals.AimPropertyControl_SetRow;
            ui_filterControl.LoadFeatureListByDependHandler = Globals.GetFeatureListByDepend;
        }

        public QueryInfoBuilderForm (Aran.Aim.Data.User SelectedUser)
            : this ()
        {
            // TODO: Complete member initialization
            _user = SelectedUser;
        }

        //public AimFeatureLayerGroup BasedOnLayer { get; set; }

        public QueryInfo_OLD QueryInfo
        {
            get
            {
                if (ui_queryInfosTreeView.Nodes.Count == 0)
                    return null;

                var nodeTag = ui_queryInfosTreeView.Nodes [0].Tag as QueryInfoNodeTag;
                return nodeTag.QueryInfo;
            }
            set
            {
                ui_queryInfosTreeView.Nodes.Clear ();

                TreeNode tn = ToTreeNode (value);
                ui_queryInfosTreeView.Nodes.Add (tn);
                tn.ExpandAll ();

                ui_addQIButton.Visible = false;
            }
        }


        #region Private Classes

        private class QueryInfoNodeTag
        {
            public bool IsSubQuery
            {
                get { return (SubQueryInfo != null); }
            }

            public QueryInfo_OLD QueryInfo { get; set; }

            public SubQueryInfo_OLD SubQueryInfo { get; set; }

            public QueryInfo_OLD GetQueryInfo ()
            {
                if (QueryInfo != null)
                    return QueryInfo;

                return SubQueryInfo.QueryInfo;
            }
        }

        #endregion


        private void AddQI_Click (object sender, EventArgs e)
        {
            SubQuerySelectorForm sf = new SubQuerySelectorForm (false, _user);
            if (sf.ShowDialog (this) != DialogResult.OK)
                return;

            FeatureType ft = sf.FeatureType;
            if (ft == 0)
                return;

            QueryInfo_OLD qi = new QueryInfo_OLD ();
            qi.FeatureType = ft;

            var tn = ToTreeNode (qi);
            ui_queryInfosTreeView.Nodes.Add (tn);
            ui_queryInfosTreeView.SelectedNode = tn;

            EnableOkButton ();

            ui_addQIButton.Visible = false;
        }

        private void AddSubQuery_Click (object sender, EventArgs e)
        {
            var nt = CurrentQueryTag;
            if (nt == null)
                return;

            var qi = nt.GetQueryInfo ();
            SubQuerySelectorForm sf = new SubQuerySelectorForm (true, _user);
            sf.FeatureType = qi.FeatureType;
            if (sf.ShowDialog (this) != DialogResult.OK)
                return;

            #region Check If Added
            int addedCount = 0;
            foreach (var childSQI in qi.SubQueries)
            {
                if (childSQI.PropertyPath == sf.PropertyPath)
                {
                    addedCount++;
                }
            }
            if (addedCount > 0)
            {
                var mr = MessageBox.Show (
                    "Property: [" + sf.PropertyPath + "] already added" +
                    (addedCount > 1 ?  " " + addedCount + " times" : "") +
                    ".\nDo you want to add again?",
                    Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (mr != DialogResult.Yes)
                    return;
            }
            #endregion

            List<FeatureType> featList;
            if (!sf.IsAbstractFeature (out featList))
            {
                featList = new List<FeatureType> ();
                featList.Add (sf.FeatureType);
            }

            TreeNode selNode = null;

            foreach (FeatureType ft in featList)
            {
                var qi2 = new QueryInfo_OLD ();
                qi2.FeatureType = ft;
                var sqi = new SubQueryInfo_OLD (sf.PropertyPath, qi2);
                qi.SubQueries.Add (sqi);

                TreeNode tn = ToTreeNode (sqi);
                ui_queryInfosTreeView.SelectedNode.Nodes.Add (tn);
                selNode = tn;
            }

            if (selNode != null)
                ui_queryInfosTreeView.SelectedNode = selNode;
        }

        private static TreeNode ToTreeNode (QueryInfo_OLD qi)
        {
            TreeNode tn = new TreeNode ();
            tn.Text = qi.FeatureType.ToString ();

            QueryInfoNodeTag tag = new QueryInfoNodeTag ();
            tag.QueryInfo = qi;
            tn.Tag = tag;

            foreach (var sqi in qi.SubQueries)
            {
                TreeNode stn = ToTreeNode (sqi);
                tn.Nodes.Add (stn);
            }

            return tn;
        }

        private static TreeNode ToTreeNode (SubQueryInfo_OLD sqi)
        {
            TreeNode tn = new TreeNode ();
            tn.Text = sqi.QueryInfo.FeatureType.ToString ();
            tn.ToolTipText = sqi.PropertyPath;

            QueryInfoNodeTag tag = new QueryInfoNodeTag ();
            tag.SubQueryInfo = sqi;
            tn.Tag = tag;

            if (sqi.QueryInfo != null)
            {
                foreach (SubQueryInfo_OLD subSQI in sqi.QueryInfo.SubQueries)
                {
                    var subNode = ToTreeNode (subSQI);
                    tn.Nodes.Add (subNode);
                }
            }

            return tn;
        }

        private void QueryInfos_AfterSelect (object sender, TreeViewEventArgs e)
        {
            if (ui_queryInfosTreeView.SelectedNode == null)
            {
                CurrentQueryTag = null;
                return;
            }

            CurrentQueryTag = ui_queryInfosTreeView.SelectedNode.Tag as QueryInfoNodeTag;
        }

        private void FeatureStyleControl_ValueChanged (object sender, EventArgs e)
        {
            _saveQueryInfoSymbol = true;
        }

        private QueryInfoNodeTag CurrentQueryTag
        {
            get
            {
                return _currentQueryTag;
            }
            set
            {
                SavePrev (_prevQueryTag);

                QueryInfoNodeTag nt = value;
                _currentQueryTag = nt;

                bool notNull = (nt != null);
                ui_addSubQueryButton.Enabled = notNull;
                ui_featureStyleControl.Enabled = notNull;
                ui_removeLayerButton.Enabled = notNull;

                ui_nameOrPropLabel.Text = "Query Name: ";
                ui_nameOrPropertyTB.Text = string.Empty;
                ui_nameOrPropertyTB.ReadOnly = true;

                if (nt != null)
                {
                    var qi = nt.GetQueryInfo ();

                    if (nt.IsSubQuery)
                    {
                        ui_nameOrPropLabel.Text = "Link Property: ";
                        ui_nameOrPropertyTB.Text = nt.SubQueryInfo.PropertyPath;
                    }
                    else
                    {
                        ui_nameOrPropertyTB.Text = qi.Name;
                        ui_nameOrPropertyTB.ReadOnly = false;
                    }



                    bool b = ui_featureStyleControl.SetShapeInfos (qi.FeatureType, qi.ShapeInfoList);
                    ui_filterControl.SetFilter (qi.FeatureType, qi.Filter);

                    if (b)
                    {
                        if (!ui_tabControl.TabPages.ContainsKey ("ui_styleTabPage"))
                        {
                            ui_tabControl.TabPages.Insert (0, ui_styleTabPage);
                            ui_tabControl.SelectedTab = ui_styleTabPage;
                        }
                    }
                    else
                    {
                        if (ui_tabControl.TabPages.ContainsKey ("ui_styleTabPage"))
                            ui_tabControl.TabPages.Remove (ui_styleTabPage);
                    }

                    ui_tabControl.Enabled = true;
                }
                else
                {
                    ui_featureStyleControl.SetShapeInfos ((FeatureType) 0, null);
                    ui_tabControl.Enabled = false;
                }

                _prevQueryTag = nt;
                _saveQueryInfoSymbol = false;
                _saveQueryInfoFilter = false;

                EnableOkButton ();
            }
        }

        private void SavePrev (QueryInfoNodeTag qiTag)
        {
            if (qiTag == null)
                return;

            if (_saveQueryInfoSymbol)
                SaveQueryInfoSymbol (qiTag);
            if (_saveQueryInfoFilter)
                SaveQueryInfoFilter (qiTag);

            var qi = qiTag.GetQueryInfo ();

            if (!qiTag.IsSubQuery)
                qi.Name = ui_nameOrPropertyTB.Text;
            else
                qi.Name = qiTag.SubQueryInfo.PropertyPath;


        }

        private void SaveQueryInfoSymbol (QueryInfoNodeTag qi)
        {
            var shapeInfos = ui_featureStyleControl.GetShapeInfos ();
            var qqi = qi.GetQueryInfo ();
            qqi.ShapeInfoList.Clear ();
            qqi.ShapeInfoList.AddRange (shapeInfos);
        }

        private void SaveQueryInfoFilter (QueryInfoNodeTag qi)
        {
            qi.GetQueryInfo ().Filter = ui_filterControl.GetFilter ();
        }

        private void FilterControl_ValueChanged (object sender, EventArgs e)
        {
            _saveQueryInfoFilter = true;
        }

        private void RemoveLayer_Click (object sender, EventArgs e)
        {
            if (ui_queryInfosTreeView.SelectedNode == null)
                return;

            var mr = MessageBox.Show ("Do you want to remove the selected node?",
                Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (mr != DialogResult.Yes)
                return;

            var nodeTag = CurrentQueryTag;
            var currQI = nodeTag.GetQueryInfo ();

            if (ui_queryInfosTreeView.SelectedNode.Parent != null)
            {
                var parentNodeTag = ui_queryInfosTreeView.SelectedNode.Parent.Tag as QueryInfoNodeTag;
                var parentQI = parentNodeTag.GetQueryInfo ();
                for (int i = 0; i < parentQI.SubQueries.Count; i++)
                {
                    if (parentQI.SubQueries [i].QueryInfo == currQI)
                    {
                        parentQI.SubQueries.RemoveAt (i);
                        break;
                    }
                }
            }

            ui_queryInfosTreeView.SelectedNode.Remove ();

            if (ui_queryInfosTreeView.Nodes.Count > 0)
                ui_queryInfosTreeView.SelectedNode = ui_queryInfosTreeView.Nodes [0];
            else
                CurrentQueryTag = null;

            EnableOkButton ();

            ui_addQIButton.Visible = (ui_queryInfosTreeView.Nodes.Count == 0);
        }

        private void OK_Click (object sender, EventArgs e)
        {
            SavePrev (_prevQueryTag);

            if (ui_queryInfosTreeView.Nodes.Count > 0 &&
                string.IsNullOrEmpty (
                    (ui_queryInfosTreeView.Nodes [0].Tag as QueryInfoNodeTag).QueryInfo.Name))
            {
                MessageBox.Show ("Please, set [Query] Name",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (OKClicked != null)
                OKClicked (this, e);
        }

        private void Cancel_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void EnableOkButton ()
        {
            ui_okButton.Enabled = (ui_queryInfosTreeView.Nodes.Count > 0);
        }

        private void MainMenu_Click (object sender, EventArgs e)
        {
            ui_mainContextMenu.Show (ui_mainMenuButton, new Point (0, 0),
                ToolStripDropDownDirection.AboveRight);
        }

        private void SaveSettings_Click (object sender, EventArgs e)
        {
            if (ui_queryInfosTreeView.Nodes.Count == 0)
            {
                MessageBox.Show ("No [Query Settings] added.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var qiTag = ui_queryInfosTreeView.Nodes [0].Tag as QueryInfoNodeTag;

            SaveFileDialog sfd = new SaveFileDialog ();
            sfd.Filter = "Query Layer Settings (*.qls)|*.qls";
            if (sfd.ShowDialog () != DialogResult.OK)
                return;

            using (AranPackageWriter apw = new AranPackageWriter ())
            {
                qiTag.QueryInfo.Pack (apw);
                byte [] buffer = apw.GetBytes ();

                var fileStream = File.OpenWrite (sfd.FileName);
                fileStream.Write (buffer, 0, buffer.Length);
                fileStream.Close ();
                fileStream.Dispose ();
            }
        }

        private void OpenSettings_Click (object sender, EventArgs e)
        {
            if (ui_queryInfosTreeView.Nodes.Count > 0)
            {
                var mr = MessageBox.Show ("Added [Query Settings].\nDo you want to discard current settings?",
                    Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (mr != DialogResult.Yes)
                    return;
            }

            OpenFileDialog ofd = new OpenFileDialog ();
            ofd.Filter = "Query Layer Settings (*.qls)|*.qls";
            if (ofd.ShowDialog () != DialogResult.OK)
                return;

            var fileStream = File.OpenRead (ofd.FileName);
            var buffer = new byte [fileStream.Length];
            fileStream.Read (buffer, 0, buffer.Length);
            fileStream.Close ();
            fileStream.Dispose ();

            QueryInfo_OLD qi = new QueryInfo_OLD ();

            using (AranPackageReader apr = new AranPackageReader (buffer))
            {
                qi.Unpack (apr);
            }

            ui_queryInfosTreeView.Nodes.Clear ();

            var tn = ToTreeNode (qi);
            ui_queryInfosTreeView.Nodes.Add (tn);
            ui_queryInfosTreeView.SelectedNode = tn;

            tn.ExpandAll ();

            EnableOkButton ();

            ui_addQIButton.Visible = (ui_queryInfosTreeView.Nodes.Count == 0);

        }
    }
}
