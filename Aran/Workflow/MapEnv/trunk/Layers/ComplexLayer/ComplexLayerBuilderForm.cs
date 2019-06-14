using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Aran.Aim;
using System.IO;
using Aran.Package;
using MapEnv.Layers;

namespace MapEnv.ComplexLayer
{
    public partial class ComplexLayerBuilderForm : Form
    {
        private int _pageIndex;
        private int _pageCount;
        private Dictionary<FeatureType, List<TableShapeInfo>> _featShapeInfoDict;
        private Dictionary<FeatureType, QueryInfo> _featQueryInfoDict;

        public ComplexLayerBuilderForm ()
        {
            InitializeComponent ();

            _pageIndex = -1;
            _pageCount = ui_tabControl.TabPages.Count;
            _featShapeInfoDict = new Dictionary<FeatureType, List<TableShapeInfo>> ();
            _featQueryInfoDict = new Dictionary<FeatureType, QueryInfo> ();

            ui_layerTreeCont = ui_layerTreeContElementHos.Child as CompLayerTreeControl;
            ui_layerTreeCont.AddRefClicked += LayerTreeCont_AddRefClicked;
            ui_layerTreeCont.SetSymbolClicked += LayerTreeCont_SetSymbolClicked;
            ui_layerTreeCont.SetFilterClicked += LayerTreeCont_SetFilterClicked;
            ui_layerTreeCont.RemoveItemClicked += LayerTreeCont_RemoveItemClicked;
        }

        public QueryInfo GetQueryInfo ()
        {
            if (ui_layerTreeCont.Items.Count == 0)
                return null;

            var qi = ui_layerTreeCont.Items[0].QueryInfo;

            if (!string.IsNullOrWhiteSpace(ui_layerNameTB.Text))
                qi.Name = ui_layerNameTB.Text;

            return qi;
        }

        public void SetQueryInfo (QueryInfo queryInfo)
        {
            ui_layerTreeCont.Items.Clear ();

            var item = new MyTreeItem (queryInfo);
            FillQueryInfo (item);
            ui_layerTreeCont.Items.Add (item);

            ui_layerNameTB.Text = queryInfo.Name;
        }


        private void FillQueryInfo (MyTreeItem item)
        {
            foreach (var refItem in item.QueryInfo.RefQueries)
            {
                var childItem = new MyTreeItem (refItem.QueryInfo);
                childItem.Type = MyTreeItemType.Ref;
                childItem.PropPath = refItem.PropertyPath;
                FillQueryInfo (childItem);
                item.Items.Add (childItem);
            }

            foreach (var refItem in item.QueryInfo.SubQueries)
            {
                var childItem = new MyTreeItem (refItem.QueryInfo);
                childItem.Type = MyTreeItemType.Sub;
                childItem.PropPath = refItem.PropertyPath;
                FillQueryInfo (childItem);
                item.Items.Add (childItem);
            }
        }


        private void Form_Load (object sender, EventArgs e)
        {
            foreach (TabPage tabPage in ui_tabControl.TabPages)
            {
                if (tabPage.Controls.Count == 0)
                    continue;
                var cont = tabPage.Controls [0];
                cont.Visible = false;
                cont.Parent = ui_tabControl.Parent;
                cont.Dock = DockStyle.Fill;
            }

            if (ui_layerTreeCont.Items.Count == 0)
            {
                SetPageIndex (0);
                ui_featureTypeSelector.Init ();

                Text = "Complex Layer Builder";
            }
            else
            {
                ui_layerTreeContElementHos.Visible = true;
                ui_prevButton.Visible = false;
                ui_nextButton.Text = "OK";
                ui_nextButton.Enabled = true;
                ui_nextButton.Tag = true;

                Text = "Complex Layer Property";
            }
        }

        private void SetPageIndex (int pageIndex)
        {
            _pageIndex = pageIndex;

            ui_featureTypeSelector.Visible = (pageIndex == 0);
            ui_layerTreeContElementHos.Visible = (pageIndex == 1);
            ui_selectBaseOnFeatTSBI.Visible = (pageIndex == 1);
            ui_selectBaseOnFeatTSBI.Tag = (pageIndex == 1);
            ui_prevButton.Visible = (pageIndex > 0);

            if (pageIndex == _pageCount - 1)
            {
                ui_nextButton.Text = "OK";
                ui_nextButton.Tag = true;

                if (string.IsNullOrWhiteSpace(ui_layerNameTB.Text))
                    ui_layerNameTB.Text = ui_featureTypeSelector.SelectedType.ToString();
            }
            else
            {
                ui_nextButton.Text = "Next >";
                ui_nextButton.Tag = null;
            }

            if (pageIndex == 1)
                OnTreeControlVisible ();
            else if (pageIndex == 2)
                OnFinish ();
        }

        private void Next_Click (object sender, EventArgs e)
        {
            if (true.Equals (ui_nextButton.Tag))
                OnFinish ();
            else
                SetPageIndex (_pageIndex + 1);
        }

        private void Prev_Click (object sender, EventArgs e)
        {
            SetPageIndex (_pageIndex - 1);
        }

        private void FeatureTypeSelector_TypeSelected (object sender, EventArgs e)
        {
            ui_nextButton.Enabled = (ui_featureTypeSelector.SelectedType != 0);
        }

        private void LayerTreeCont_AddRefClicked (object sender, CompLayerTreeItemEventArgs e)
        {
            var form = new ComplexLayerRefSetForm ();
            form.FeatureType = e.TreeItem.FeatureType;
            form.OKClicked += LayerRefSet_OKClicked;
            form.Tag = e.TreeItem;
            form.ShowDialog (this);
        }

        private void LayerTreeCont_SetFilterClicked (object sender, CompLayerTreeItemEventArgs e)
        {
			var ff = new Forms.FilterForm ();
            ff.SetFilter(e.TreeItem.FeatureType, e.TreeItem.QueryInfo.Filter, Globals.GetFeatureListByDepend);

			if (ff.ShowDialog () != DialogResult.OK)
				return;

			var filter = ff.GetFilter ();
			e.TreeItem.QueryInfo.Filter = filter;
			e.TreeItem.HasFilterChanged ();
        }

        private void LayerTreeCont_SetSymbolClicked (object sender, CompLayerTreeItemEventArgs e)
        {
            EmptyForm emptyForm = new EmptyForm ();
            emptyForm.OKClicked += EmptyForm_OKClicked;

            FeatureStyleControl fsc = new FeatureStyleControl ();
            fsc.Tag = e.TreeItem;

            var sil = e.TreeItem.QueryInfo.ShapeInfoList;

            if (sil.Count == 0)
            {
                List<TableShapeInfo> tmpSil;
                if (DefaultStyleLoader.Instance.Dict.TryGetValue (e.TreeItem.FeatureType, out tmpSil))
                {
                    sil = tmpSil;
                }
                else
                {
                    if (_featShapeInfoDict.TryGetValue (e.TreeItem.FeatureType, out tmpSil))
                        sil = tmpSil;
                }
            }

            fsc.SetShapeInfos (e.TreeItem.FeatureType, sil);
            emptyForm.WorkControl = fsc;

            emptyForm.ShowDialog (this);
        }

        private void LayerTreeCont_RemoveItemClicked (object sender, CompLayerTreeItemEventArgs e)
        {
            if (e.TreeItem.Parent == null)
                return;

            var mr = MessageBox.Show ("Do you want to remove Link?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (mr != DialogResult.Yes)
                return;

            var qi = e.TreeItem.QueryInfo;
            var parQi = e.TreeItem.Parent.QueryInfo;

            if (e.TreeItem.Type == MyTreeItemType.Ref)
            {
                for (int i = 0; i < parQi.RefQueries.Count; i++)
                {
                    if (parQi.RefQueries [i].QueryInfo == qi)
                    {
                        parQi.RefQueries.RemoveAt (i);
                        break;
                    }
                }
            }
            else if (e.TreeItem.Type == MyTreeItemType.Sub)
            {
                for (int i = 0; i < parQi.SubQueries.Count; i++)
                {
                    if (parQi.SubQueries [i].QueryInfo == qi)
                    {
                        parQi.SubQueries.RemoveAt (i);
                        break;
                    }
                }
            }

            e.TreeItem.Parent.Items.Remove (e.TreeItem);
        }

        private void EmptyForm_OKClicked (object sender, EventArgs e)
        {
            var emptyForm = sender as EmptyForm;

            if (emptyForm.WorkControl is FeatureStyleControl)
            {
                var fsc = emptyForm.WorkControl as FeatureStyleControl;
                var treeItem = fsc.Tag as MyTreeItem;
                treeItem.QueryInfo.ShapeInfoList.Clear ();

                var siArr = fsc.GetShapeInfos ();

                treeItem.QueryInfo.ShapeInfoList.AddRange (siArr);
                treeItem.HasSymbolChanged ();
                
                if (!_featShapeInfoDict.ContainsKey (fsc.FeatureType))
                    _featShapeInfoDict.Add (fsc.FeatureType, new List<TableShapeInfo> (siArr));
            }

            emptyForm.Close ();
        }

        private void LayerRefSet_OKClicked (object sender, EventArgs e)
        {
            var form = sender as ComplexLayerRefSetForm;
            var refList = new List<RefQueryInfo> ();
            var subList = new List<SubQueryInfo> ();
            form.GetQueryInfo (refList, subList);
            form.Close ();

            var treeItem = form.Tag as MyTreeItem;
            var queryInfo = treeItem.QueryInfo;

            foreach (var sqi in subList)
            {
                queryInfo.SubQueries.Add (sqi);

                var subTreeItem = new MyTreeItem ();
                subTreeItem.FeatureType = sqi.QueryInfo.FeatureType;
                subTreeItem.PropPath = sqi.PropertyPath;
                subTreeItem.Type = MyTreeItemType.Sub;
                subTreeItem.LinkInfo = sqi;

                DefaultStyleLoader.GetDefaultStyle (sqi.QueryInfo.FeatureType, subTreeItem.QueryInfo.ShapeInfoList);

                treeItem.Items.Add (subTreeItem);
            }

            foreach (var rqi in refList)
            {
                queryInfo.RefQueries.Add (rqi);

                var subTreeItem = new MyTreeItem ();
                subTreeItem.FeatureType = rqi.QueryInfo.FeatureType;
                subTreeItem.PropPath = rqi.PropertyPath;
                subTreeItem.Type = MyTreeItemType.Ref;
                subTreeItem.LinkInfo = rqi;

                DefaultStyleLoader.GetDefaultStyle (rqi.QueryInfo.FeatureType, subTreeItem.QueryInfo.ShapeInfoList);

                treeItem.Items.Add (subTreeItem);
            }
        }

        private void OnTreeControlVisible ()
        {
            ui_layerTreeCont.Items.Clear ();
            MyTreeItem item = new MyTreeItem (new QueryInfo (ui_featureTypeSelector.SelectedType));
            item.QueryInfo.Name = item.Name;
            ui_layerTreeCont.Items.Add (item);
        }

        private void OnFinish ()
        {
            DialogResult = DialogResult.OK;
        }

        private void SelectBaseOnFeature_Click (object sender, EventArgs e)
        {
            var fsf = new FeatureSelectorForm ();
            fsf.FeatureType = ui_featureTypeSelector.SelectedType;
            if (fsf.ShowDialog (this) != DialogResult.OK)
                return;

            ui_layerTreeCont.Items.Clear ();

            var qiGen = new QueryInfoGenerator ();
            var qi = qiGen.Load (fsf.CurrentFeature);
            SetQueryInfo (qi);
        }

        private void button1_Click (object sender, EventArgs e)
        {
            contextMenuStrip1.Show (button1, new Point (0, 0), ToolStripDropDownDirection.AboveRight);
        }

        private void SaveFeatureRelation_Click (object sender, EventArgs e)
        {
            if (ui_layerTreeCont.Items.Count == 0)
                return;

            var sfd = new SaveFileDialog ();
            sfd.Filter = "Query Layer Settings (*.qls)|*.qls";
            if (sfd.ShowDialog () != DialogResult.OK)
                return;

            var qi = ui_layerTreeCont.Items [0].QueryInfo;
            var fs = new FileStream (sfd.FileName, FileMode.Create);
            var writer = new BinaryPackageWriter (fs);
            qi.Pack (writer);
            fs.Close ();
        }

        private void LoadFeatureRelation_Click (object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog ();
            ofd.Filter = "Query Layer Settings (*.qls)|*.qls";
            if (ofd.ShowDialog () != DialogResult.OK)
                return;
            
            var fs = new FileStream (ofd.FileName, FileMode.Open);
            var reader = new BinaryPackageReader (fs);
            var qi = new QueryInfo ();
            qi.Unpack (reader);
            fs.Close ();

            SetQueryInfo (qi);

            ui_featureTypeSelector.Visible = false;
            ui_layerTreeContElementHos.Visible = true;
            ui_nextButton.Text = "OK";
            ui_nextButton.Enabled = true;
            ui_nextButton.Tag = true;
        }

        private void contextMenuStrip1_Opening (object sender, CancelEventArgs e)
        {
            ui_splitter.Visible = true.Equals (ui_selectBaseOnFeatTSBI.Tag);
        }
    }
}
