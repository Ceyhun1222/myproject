using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using MapEnv.Layers;

namespace MapEnv
{
    public partial class FeatureTypesStyleControl : UserControl
    {
        private FeatureType? _selectedFeatureType;
        private ListBoxItem _prevItem;

        public event EventHandler SelectedFeatureTypeChanged;


        public FeatureTypesStyleControl()
        {
            InitializeComponent();
        }


        public bool AddFeatureType(FeatureType featureType)
        {
            foreach (ListBoxItem lbi in ui_featTypesLB.Items) {
                if (lbi.FeatureType == featureType)
                    return false;
            }

            var newLbi = new ListBoxItem(featureType, null);
            ui_featTypesLB.Items.Add(newLbi);
            ui_featTypesLB.SelectedItem = newLbi;
            return true;
        }

        public void AddFeatureType(FeatureType featureType, List<TableShapeInfo> shapeInfos)
        {
            foreach (ListBoxItem lbi in ui_featTypesLB.Items) {
                if (lbi.FeatureType == featureType)
                    return;
            }

            var newLbi = new ListBoxItem(featureType, shapeInfos);
            ui_featTypesLB.Items.Add(newLbi);
            ui_featTypesLB.SelectedItem = newLbi;
        }

        public void RemoveFeatureType(FeatureType featureType)
        {
            for (int i = 0; i < ui_featTypesLB.Items.Count; i++) {
                if ((ui_featTypesLB.Items[i] as ListBoxItem).FeatureType == featureType) {
                    ui_featTypesLB.Items.RemoveAt(i);
                    return;
                }
            }
        }

        public void ClearFeatureTypes()
        {
            ui_featTypesLB.SelectedItem = null;
            FeatureTypes_SelectedIndexChanged(null, null);

            ui_featTypesLB.Items.Clear();
        }

        public FeatureType? SelectedFeatureType
        {
            get
            {
                return _selectedFeatureType;
            }
            set
            {
                if (_selectedFeatureType == value)
                    return;

                _selectedFeatureType = value;
                if (SelectedFeatureTypeChanged != null)
                    SelectedFeatureTypeChanged(this, null);
            }
        }

        public void SaveChanges()
        {
            FeatureTypes_SelectedIndexChanged(null, null);
        }

        public int ItemsCount
        {
            get { return ui_featTypesLB.Items.Count; }
        }

        public bool GetItem(int index, out FeatureType featType, out List<TableShapeInfo> shapeInfos)
        {
            featType = 0;
            shapeInfos = null;

            if (index < 0 || index > ui_featTypesLB.Items.Count - 1)
                return false;

            var lbi = ui_featTypesLB.Items[index] as ListBoxItem;
            if (lbi.ShapeInfos == null || lbi.ShapeInfos.Count == 0)
                return false;

            featType = lbi.FeatureType;
            shapeInfos = lbi.ShapeInfos;
            return true;
        }

        public List<TableShapeInfo> GetShapeInfo(FeatureType featureType)
        {
            foreach (ListBoxItem lbi in ui_featTypesLB.Items) {
                if (lbi.FeatureType == featureType)
                    return lbi.ShapeInfos;
            }
            return null;
        }


        private void FeatureTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lbi = ui_featTypesLB.SelectedItem as ListBoxItem;

            if (lbi == null) {
                SelectedFeatureType = null;
                return;
            }

            var featType = lbi.FeatureType;

            if (_prevItem != null) {
                var shapeInfoArr = ui_featureStyleControl.GetShapeInfos();
                if (shapeInfoArr.Length > 0)
                    _prevItem.ShapeInfos = new List<TableShapeInfo>(shapeInfoArr);
            }

            ui_featureStyleControl.SetShapeInfos(lbi.FeatureType, lbi.ShapeInfos);

            _prevItem = lbi;
            SelectedFeatureType = featType;
        }


        private class ListBoxItem
        {
            public ListBoxItem(FeatureType featureType, List<TableShapeInfo> shapeInfos)
            {
                FeatureType = featureType;
                ShapeInfos = shapeInfos;
            }

            public FeatureType FeatureType { get; private set; }

            public List<TableShapeInfo> ShapeInfos { get; set; }

            public override string ToString()
            {
                return FeatureType.ToString();
            }
        }
    }
}
