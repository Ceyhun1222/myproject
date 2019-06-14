using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using Aran.Package;

namespace Aran.Aim.Data.TerrainReader
{
    public partial class TerrainSettingsPage : UserControl, ISettingsPage
    {
        public TerrainSettingsPage()
        {
            InitializeComponent();
        }

        public IAranEnvironment AranEnv { get; set; }

        public string Title
        {
            get { return "Terrain Data"; }
        }

        public Control Page
        {
            get { return this; }
        }

        public void OnLoad()
        {
            ui_terrainLayerCB.Items.Clear();

            var layerName = "";
            var slp = new SelectedLayerPackable();
            if (AranEnv.GetExtData("selectedTerrainData", slp))
                layerName = slp.LayerName;

            var esriMapControl = AranEnv.MapControl as AxMapControl;
            var terrainLayers = new List<IFeatureLayer>();
            var enumValues = Enum.GetValues(typeof(VerticalStructureField));

            for (int i = 0; i < esriMapControl.LayerCount; i++) {
                var featLayer = esriMapControl.get_Layer(i) as IFeatureLayer;
                if (featLayer != null && featLayer.FeatureClass != null)
                {
                    var fields = featLayer.FeatureClass.Fields;
                    var hasAllNeededFields = true;

                    foreach (var enumVal in enumValues) {
                        var fieldName = enumVal.ToString().ToLower();
                        var fieldIndex = fields.FindField(fieldName);
                        if (fieldIndex == -1) {
                            hasAllNeededFields = false;
                            break;
                        }
                    }

                    if (hasAllNeededFields)
                        terrainLayers.Add(featLayer);
                }
            }

            var selIndex = -1;
            foreach (var terrainFL in terrainLayers) {
                var item = new CBItem(terrainFL);
                var index = ui_terrainLayerCB.Items.Add(item);

                if (!string.IsNullOrWhiteSpace(layerName) && item.ToString() == layerName)
                    selIndex = index;
            }

            if (selIndex != -1)
                ui_terrainLayerCB.SelectedIndex = selIndex;
        }

        public bool OnSave()
        {
            var slp = new SelectedLayerPackable();
            slp.LayerName = "";

            var selectedLayer = ui_terrainLayerCB.SelectedItem as CBItem;
            if (selectedLayer != null)
                slp.LayerName = selectedLayer.ToString();

            AranEnv.PutExtData("selectedTerrainData", slp);
            return true;
        }


        private class CBItem
        {
            public CBItem(IFeatureLayer featureLayer)
            {
                FeatureLayer = featureLayer;
            }

            public IFeatureLayer FeatureLayer { get; private set; }

            public override string ToString()
            {
                return FeatureLayer.Name;
            }
        }

    }

    public class SettingPlugin : ISettingsPlugin
    {
        public void Startup(IAranEnvironment aranEnv)
        {
            var control = new TerrainSettingsPage();
            control.AranEnv = aranEnv;

            aranEnv.AranUI.AddSettingsPage(new Guid[] {
                    new Guid("f3d11f81-9c38-4ce0-ab80-10477af5f0d4")},
                    control);
        }
    }

    public class SelectedLayerPackable : IPackable
    {
        public string LayerName { get; set; }

        public void Pack(PackageWriter writer)
        {
            writer.PutString(LayerName);
        }

        public void Unpack(PackageReader reader)
        {
            LayerName = reader.GetString();
        }
    }
}