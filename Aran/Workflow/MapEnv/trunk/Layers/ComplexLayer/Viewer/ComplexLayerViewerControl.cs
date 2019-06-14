using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Aran.Aim.Metadata.UI;
using MapEnv.Layers;

namespace MapEnv.ComplexLayer
{
    public partial class ComplexLayerViewerControl : UserControl, ILeftWindow
    {
        private ESRI.ArcGIS.Carto.ILayer _esriLayer;
        private int _refreshHandleCount;



        public ComplexLayerViewerControl()
        {
            InitializeComponent();

            _refreshHandleCount = 0;
        }

        public void SetComplexLayer(AimFeatureLayer aimFL)
        {
            _esriLayer = aimFL;

            BaseOn = aimFL;
            Title = aimFL.Name;

            var compTable = aimFL.ComplexTable;
            var root = ui_complexLayerViewerW.Root;

            if (compTable == null)
                return;

            root.FeatureType = compTable.FeatureType.ToString();

            FillCompTable(compTable, root.Items, string.Empty);
        }

        public Control AreaControl
        {
            get { return this; }
        }

        public object BaseOn { get; private set; }

        public string Title { get; private set; }


        private void FillCompTable(AimComplexTable compTable, IList items, string propName)
        {
            foreach (var compRow in compTable.Rows) {
                var newItem = CreateItem(compRow.Row.IsVisible, compRow.Row.IsSelected);
                newItem.Name = UIUtilities.GetFeatureDescription(compRow.Row.AimFeature);
                newItem.FeatureType = compTable.FeatureType.ToString();
                newItem.PropName = propName;
                newItem.ComplexRow = compRow;

                foreach (var subItem in compRow.SubQueryList) {
                    FillCompTable(subItem.ComplexTable, newItem.Items, subItem.PropertyPath);
                }

                foreach (var refItem in compRow.RefQueryList) {
                    FillCompTable(refItem.ComplexTable, newItem.Items, refItem.PropertyPath);
                }

                items.Add(newItem);
            }
        }

        public CLVTreeItem CreateItem(bool isVisible = true, bool isSelected = false)
        {
            var item = new CLVTreeItem();
            item.IsVisible = isVisible;
            item.IsSelected = isSelected;
            item.PropertyChanged += TreeItem_PropertyChanged;
            return item;
        }

        private void TreeItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible") {
                BeginRefresh();

                var treeItem = sender as CLVTreeItem;
                treeItem.ComplexRow.Row.IsVisible = treeItem.IsVisible;

                if (!ComplexLayerViewerW.IsControlKeyDown) {
                    foreach (var item in treeItem.Items)
                        item.IsVisible = treeItem.IsVisible;
                }

                EndRefresh();
            }
            else if (e.PropertyName == "IsSelected") {
                BeginRefresh();

                var treeItem = sender as CLVTreeItem;
                treeItem.ComplexRow.Row.IsSelected = treeItem.IsSelected;

                if (!ComplexLayerViewerW.IsControlKeyDown) {
                    foreach (var item in treeItem.Items)
                        item.IsSelected = treeItem.IsSelected;
                }

                EndRefresh();
            }
        }



        private void BeginRefresh()
        {
            _refreshHandleCount++;
        }

        private void EndRefresh()
        {
            _refreshHandleCount--;

            if (_refreshHandleCount == 0) {
                if (_esriLayer is AimFeatureLayer) {
                    var aimFL = _esriLayer as AimFeatureLayer;
                    aimFL.RefreshComplex();
                    aimFL.RefreshLayers();
                }
                else {
                    Globals.MainForm.RefreshLayer(_esriLayer);
                }
            }
        }
    }
}
