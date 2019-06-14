using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;
using Aran.Aim;
using EsriGeom = ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.CatalogUI;

namespace MapEnv
{
    public partial class NewProPluginPage : UserControl
    {
        private EsriGeom.ISpatialReference _spatialReference;


        public NewProPluginPage()
        {
            InitializeComponent();

            ui_effectiveDateTimePicker.SetNextCycle();
        }


        public EsriGeom.ISpatialReference GetSpatialReference ()
        {
            if (_spatialReference == null)
                _spatialReference = Globals.CreateWGS84SR();

            return _spatialReference;
        }

        public AiracDateTime AiracDateTime
        {
            get { return ui_effectiveDateTimePicker.AiracDateTime; }
        }

        public List<AranPlugin> GetSelectedPlugins()
        {
            var list = new List<AranPlugin>();

            foreach (LBItem item in ui_pluginsCLB.CheckedItems)
                list.Add(item.Plugin);

            return list;
        }


        public void FillPlugins()
        {
            ui_pluginsCLB.Items.Clear();

            foreach (var item in Globals.AranPluginList) {
                if (!item.Plugin.IsSystemPlugin && !string.IsNullOrWhiteSpace(item.Plugin.Name)) {
                    ui_pluginsCLB.Items.Add(new LBItem() { Plugin = item.Plugin });
                }
            }
        }

        private void Plugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            ui_featLayersLB.Items.Clear();

            if (ui_pluginsCLB.SelectedItem == null)
                return;

            var lbItem = ui_pluginsCLB.SelectedItem as LBItem;

            foreach (FeatureType ft in lbItem.FeatureTypes)
                ui_featLayersLB.Items.Add(ft);
        }

        #region LBItem class

        private class LBItem
        {
            private AranPlugin _plugin;
            private string _text;


            public LBItem()
            {
                _text = string.Empty;
                FeatureTypes = new List<FeatureType>();
            }


            public AranPlugin Plugin
            {
                get { return _plugin; }
                set
                {
                    _plugin = value;
                    _text = value.Name;

                    try {
                        FeatureTypes.AddRange(_plugin.GetLayerFeatureTypes());
                    }
                    catch { }
                }
            }

            public List<FeatureType> FeatureTypes { get; private set; }

            public override string ToString()
            {
                return _text;
            }
        }

        #endregion

        private void SelectSpatialRef_Click(object sender, EventArgs e)
        {
            var SpRefDialog = new SpatialReferenceDialog() as ISpatialReferenceDialog2;

            var spatRef = SpRefDialog.DoModalEdit(GetSpatialReference(),
                true, true, true, false, false, false, false, 0);

            if (spatRef != null) {
                _spatialReference = spatRef;
                ui_spatialRefTB.Text = _spatialReference.Name;
            }
        }

        private void CheckAll_CheckedChanged(object sender, EventArgs e)
        {
            ui_pluginsCLB.Tag = true;
            
            for (int i = 0; i < ui_pluginsCLB.Items.Count; i++)
                ui_pluginsCLB.SetItemChecked(i, ui_checkAllChB.Checked);

            ui_pluginsCLB.Tag = null;
        }

        private void PluginsCLB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (true.Equals(ui_pluginsCLB.Tag))
                return;

            ui_checkAllChB.Checked = (ui_pluginsCLB.CheckedItems.Count == ui_pluginsCLB.Items.Count);
        }
    }
}
