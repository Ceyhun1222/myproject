using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Utilities;

namespace Aran.Aim.Data.LocalDbLoader
{
    public partial class FeatureTypeSelectorForm : Form
    {
        public FeatureTypeSelectorForm()
        {
            InitializeComponent();

            ui_featTypeGroupCB.Items.Add("PANDA");
            ui_featTypeGroupCB.Items.Add("All");
        }

        public ReadOnlyCollection<FeatureType> FeatureTypeList { get; set; }

        private void FeatureTypeSelectorForm_Load(object sender, EventArgs e)
        {
            ui_featTypeGroupCB.SelectedIndex = 0;
        }

        private void FillListViews(IEnumerable<FeatureType> featureTypes)
        {
            var lv = ui_listView;
            lv.Items.Clear();
            lv.Groups.Clear();

            var featTypeList = FeatureTypeList ?? new ReadOnlyCollection<FeatureType>(new FeatureType[] { });

            foreach (FeatureType featType in featureTypes)
            {
                if (featTypeList.Contains(featType))
                    continue;

                var lvi = new ListViewItem();
                lvi.Tag = featType;
                lvi.Text = featType.ToString();
                lv.Items.Add(lvi);
            }

            if (lv.Items.Count > 0)
            {
                char c = lv.Items[0].Text[0];
                string s = c.ToString();
                lv.Groups.Add(s, s);

                for (int i = 1; i < lv.Items.Count; i++)
                {
                    if (c != lv.Items[i].Text[0])
                    {
                        c = lv.Items[i].Text[0];
                        s = c.ToString();
                        lv.Groups.Add(s, s);
                    }
                }
            }

            foreach (ListViewItem lvi in lv.Items)
            {
                lvi.Group = lv.Groups[lvi.Text[0].ToString()];
            }

            lv.Sorting = SortOrder.Descending;
            lv.ListViewItemSorter = new ListViewItemComparer();
            lv.View = View.Details;
            lv.Sort();
        }

        private class ListViewItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var lvi1 = x as ListViewItem;
                var lvi2 = y as ListViewItem;
                return string.Compare(lvi1.Tag.ToString(), lvi2.Tag.ToString());
            }
        }

        private void FeatureTypeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var group = ui_featTypeGroupCB.Text;
            var featTypes = GetFeatureType(group);

            FillListViews(featTypes);

            if (group == "PANDA")
            {
                foreach (ListViewItem lvi in ui_listView.Items)
                    lvi.Checked = true;
            }
        }

        private IEnumerable<FeatureType> GetFeatureType(string group)
        {
            if (group == "All")
                return Enum.GetValues(typeof(FeatureType)).Cast<FeatureType>();

            if (group == "PANDA")
            {
                var featTypeList = new List<FeatureType>(new FeatureType[] {
                    FeatureType.OrganisationAuthority,
                    FeatureType.AirportHeliport,
                    FeatureType.Runway,
                    FeatureType.RunwayDirection,
                    FeatureType.RunwayCentrelinePoint,
                    FeatureType.StandardInstrumentArrival,
                    FeatureType.StandardInstrumentDeparture,
                    FeatureType.InstrumentApproachProcedure,
                    FeatureType.AngleIndication,
                    FeatureType.DistanceIndication,
                    FeatureType.Navaid,
                    FeatureType.DesignatedPoint,
                    FeatureType.VerticalStructure,
                    FeatureType.Route,
                    FeatureType.RouteSegment
                });

                var classInfo = AimMetadata.GetClassInfoByIndex((int)AbstractFeatureType.NavaidEquipment);
                AimMetadataUtility.GetAbstractChilds(classInfo).ForEach(ci => featTypeList.Add((FeatureType)ci.Index));

                classInfo = AimMetadata.GetClassInfoByIndex((int)AbstractFeatureType.SegmentLeg);
                AimMetadataUtility.GetAbstractChilds(classInfo).ForEach(ci => featTypeList.Add((FeatureType)ci.Index));

                featTypeList.Sort((ft1, ft2) => { return string.Compare(ft1.ToString(), ft2.ToString()); });

                return featTypeList;
            }

            return null;
        }

        private void QuickSearch_TextChanged(object sender, EventArgs e)
        {
            var text = ui_quickSearchTB.Text.ToLower();

            for (int i = 0; i < ui_listView.Items.Count; i++)
            {
                var lvi = ui_listView.Items[i];
                if (lvi.Tag.ToString().ToLower().StartsWith(text))
                {
                    lvi.Selected = true;
                    ui_listView.EnsureVisible(i);
                    break;
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (ui_listView.CheckedIndices.Count == 0)
                return;

            var list = new List<FeatureType>();
            foreach(ListViewItem lvi in ui_listView.CheckedItems)
                list.Add((FeatureType)lvi.Tag);

            FeatureTypeList = new ReadOnlyCollection<FeatureType>(list);

            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
