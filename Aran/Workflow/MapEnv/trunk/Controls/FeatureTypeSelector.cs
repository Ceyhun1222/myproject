using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Metadata.Geo;
using System.Collections;

namespace MapEnv.Controls
{
    public partial class FeatureTypeSelector : UserControl
    {
        private User _user;
        private ListViewItem _prevSelectedLVI;

        public FeatureTypeSelector()
        {
            InitializeComponent();

            ui_layerTypeCB.Items.Add(LayerType.Table);
            ui_layerTypeCB.Items.Add(LayerType.Geography);

            WindowsAPI.SetTextBoxPlaseHolder(ui_quickSearchTB, "Quick Search");

        }

        public event EventHandler TypeSelected;

        public void SetUser (User value)
        {
            _user = value;
        }

        public void Init (LayerType layerType = MapEnv.LayerType.Table)
        {
            ui_layerTypeCB.SelectedItem = layerType;
            LayerType_SelectedIndexChanged (null, null);
        }

        public FeatureType SelectedType
        {
            get
            {
                if (ui_listView.SelectedItems.Count == 0)
                    return 0;

                return (FeatureType) ui_listView.SelectedItems [0].Tag;
            }
        }

        public LayerType LayerType
        {
            get { return (LayerType)ui_layerTypeCB.SelectedItem; }
        }

        public void HideLayerType (LayerType selectedLayerType)
        {
            ui_layerTypeCB.SelectedItem = selectedLayerType;

            ui_layerTypeLabel.Visible = false;
            ui_layerTypeCB.Visible = false;
        }


        private void LayerTypeChanged(LayerType layerType)
        {
            if (layerType == LayerType.Geography)
            {
                var featureTypeList = new List<FeatureType>();
                foreach (var item in GeoMetadata.GeoClassInfoList)
                {
                    if (!item.AimClassInfo.IsAbstract)
                        featureTypeList.Add((FeatureType)item.AimClassInfo.Index);
                }
                FillListViews(featureTypeList);
            }
            else
            {
                var featTypeArr = (FeatureType[])typeof(FeatureType).GetEnumValues();
                FillListViews(featTypeArr);
            }
        }

        private void FillListViews (IEnumerable<FeatureType> featureTypes)
        {
            var lv = ui_listView;
            lv.Items.Clear ();
            lv.Groups.Clear ();

            foreach (FeatureType featType in featureTypes)
            {
                if (_user == null || _user.ContainsFeatType (featType))
                {
                    var lvi = new ListViewItem ();
                    lvi.Tag = featType;
                    lvi.Text = featType.ToString ();
                    lv.Items.Add (lvi);
                }
            }

            if (lv.Items.Count > 0)
            {
                char c = lv.Items [0].Text [0];
                string s = c.ToString ();
                lv.Groups.Add (s, s);

                for (int i = 1; i < lv.Items.Count; i++)
                {
                    if (c != lv.Items [i].Text [0])
                    {
                        c = lv.Items [i].Text [0];
                        s = c.ToString ();
                        lv.Groups.Add (s, s);
                    }
                }
            }

            foreach (ListViewItem lvi in lv.Items)
            {
                lvi.Group = lv.Groups [lvi.Text [0].ToString ()];
            }

            lv.Sorting = SortOrder.Descending;
            lv.ListViewItemSorter = new ListViewItemComparer2();
            lv.View = View.Details;
            lv.Sort();
        }

        private void LayerType_SelectedIndexChanged (object sender, EventArgs e)
        {
            LayerTypeChanged ((LayerType) ui_layerTypeCB.SelectedItem);
        }

        private void ListView_SelectedIndexChanged (object sender, EventArgs e)
        {
            if (TypeSelected != null)
                TypeSelected (this, e);
        }


        private class ListViewItemComparer2 : IComparer
        {
            public int Compare (object x, object y)
            {
                var lvi1 = x as ListViewItem;
                var lvi2 = y as ListViewItem;
                return string.Compare (lvi1.Tag.ToString (), lvi2.Tag.ToString ());
            }
        }

        private void QuickSearch_TextChanged(object sender, EventArgs e)
        {
            var text = ui_quickSearchTB.Text.ToLower();

            //if (_prevSelectedLVI != null)
            //    _prevSelectedLVI.Selected = false;

            for(int i = 0; i < ui_listView.Items.Count; i++)
            {
                var lvi = ui_listView.Items[i];
                if (lvi.Tag.ToString().ToLower().StartsWith(text))
                {
                    lvi.Selected = true;
                    ui_listView.EnsureVisible(i);
                    _prevSelectedLVI = lvi;
                    break;
                }
            }
        }
    }
}
