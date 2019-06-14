using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Package;
using System.IO;
using MapEnv.Layers;

namespace MapEnv
{
    public partial class DefaultFeatureStyleForm : Form
    {
        private DefaultStyleLoader _styleLoader;
        

        public DefaultFeatureStyleForm ()
        {
            InitializeComponent ();

            _styleLoader = DefaultStyleLoader.Instance;
        }

        
        private void DefaultFeatureStyleForm_Load (object sender, EventArgs e)
        {
            _styleLoader.Load ();

            foreach (var featType in _styleLoader.Dict.Keys) {
                var shapeInfos = _styleLoader.Dict[featType];
                ui_featTypesStyleControl.AddFeatureType(featType, shapeInfos);
            }
        }

        private void Save_Click (object sender, EventArgs e)
        {
            ui_featTypesStyleControl.SaveChanges();

            _styleLoader.Dict.Clear ();

            for (int i = 0; i < ui_featTypesStyleControl.ItemsCount; i++) {
                List<TableShapeInfo> shapeInfos = null;
                FeatureType featType;
                if (ui_featTypesStyleControl.GetItem(i, out featType, out shapeInfos))
                    _styleLoader.Dict.Add(featType, shapeInfos);
            }

            _styleLoader.Save ();

            Close();
        }

        private void Close_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void AddFeatureType_Click(object sender, EventArgs e)
        {
            var ef = new EmptyForm();
            ef.Text = "Select Feature Type";
            var fts = new Controls.FeatureTypeSelector();
            fts.HideLayerType(LayerType.Geography);
            ef.WorkControl = fts;
            ef.OKClicked += FeatureSelector_OKClicked;
            ef.ShowDialog(this);
        }

        private void FeatureSelector_OKClicked(object sender, EventArgs e)
        {
            var ef = sender as EmptyForm;
            var fts = ef.WorkControl as Controls.FeatureTypeSelector;
            var featureType = fts.SelectedType;

            if (featureType == 0)
                return;

            if (!ui_featTypesStyleControl.AddFeatureType(featureType)) {
                MessageBox.Show("FeatureType: " + featureType + " already added.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ef.Close();
        }

        private void RemoveFeatureType_Click (object sender, EventArgs e)
        {
            if (ui_featTypesStyleControl.SelectedFeatureType == null)
                return;

            var dr = MessageBox.Show("Do you want to remove the selected FeatureType Default Style?", Text,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dr != DialogResult.Yes)
                return;

            ui_featTypesStyleControl.RemoveFeatureType(ui_featTypesStyleControl.SelectedFeatureType.Value);
        }

        private void FeatTypesStyleControl_SelectedFeatureTypeChanged(object sender, EventArgs e)
        {
            ui_removeTSB.Enabled = (ui_featTypesStyleControl.SelectedFeatureType != null);
        }

        private void RestoreSettings_Click(object sender, EventArgs e)
        {
            if (ui_featTypesStyleControl.ItemsCount > 0)
            {
                if (MessageBox.Show("Do you want to restore default setting?" +
                    "\nCurrent settings will be lost", "Restore", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;
            }

            ui_featTypesStyleControl.ClearFeatureTypes();

            _styleLoader.Reload(Application.StartupPath + DefaultStyleLoader.c_fileName);

            foreach (var featType in _styleLoader.Dict.Keys)
            {
                var shapeInfos = _styleLoader.Dict[featType];
                ui_featTypesStyleControl.AddFeatureType(featType, shapeInfos);
            }
        }

        private class ListBoxItem
        {
            public ListBoxItem (FeatureType featureType, List<TableShapeInfo> shapeInfos)
            {
                FeatureType = featureType;
                ShapeInfos = shapeInfos;
            }

            public FeatureType FeatureType { get; private set; }

            public List<TableShapeInfo> ShapeInfos { get; set; }

            public override string ToString ()
            {
                return FeatureType.ToString ();
            }
        }
    }

    public class DefaultStyleLoader
    {
        public static readonly string c_fileName = "\\DefaultFeatureTypeStyle.dat";

        public DefaultStyleLoader ()
        {
            Dict = new Dictionary<FeatureType, List<TableShapeInfo>> ();
            _isLoaded = false;
        }

        public void Save ()
        {
            string dir = Aran.Aim.Metadata.UI.UIMetadata.GetGeoViewerApplicationDataDir ();
            string fileName = dir + DefaultStyleLoader.c_fileName;
            Save (fileName);
        }

        public void Save (string fileName)
        {
            var fs = new FileStream (fileName, FileMode.Create);
            var writer = new BinaryPackageWriter (fs);

            writer.PutInt32 (Dict.Count);

            foreach (FeatureType featType in Dict.Keys)
            {
                writer.PutEnum<FeatureType> (featType);
                List<TableShapeInfo> shapeInfoList = Dict [featType];
                writer.PutInt32 (shapeInfoList.Count);

                foreach (var shapeInfo in shapeInfoList)
                    (shapeInfo as IPackable).Pack (writer);
            }

            fs.Close ();
        }

        public void Load ()
        {
            try
            {
                string dir = Aran.Aim.Metadata.UI.UIMetadata.GetGeoViewerApplicationDataDir();
                string fileName = dir + c_fileName;

                if (!File.Exists(fileName))
                    fileName = Application.StartupPath + c_fileName;

                Load(fileName);
            }
            catch { }
        }

        public void Load (string fileName)
        {
            if (_isLoaded)
                return;

            _isLoaded = true;
            
            if (!File.Exists (fileName))
                return;

            Dict.Clear ();

            var fs = new FileStream (fileName, FileMode.Open);
            var reader = new BinaryPackageReader (fs);

            int count = reader.GetInt32 ();

            for (int i = 0; i < count; i++)
            {
                var featType = reader.GetEnum<FeatureType> ();
                var shapeInfoCount = reader.GetInt32 ();
                var shapeInfoList = new List<TableShapeInfo> (shapeInfoCount);

                for (int j = 0; j < shapeInfoCount; j++)
                {
                    var shapeInfo = new TableShapeInfo ();
                    (shapeInfo as IPackable).Unpack (reader);
                    shapeInfoList.Add (shapeInfo);
                }

                Dict.Add (featType, shapeInfoList);
            }

            fs.Close ();
        }

        public void Reload(string fileName)
        {
            _isLoaded = false;
            Load(fileName);
        }

        public List<TableShapeInfo> GetShapeInfo (FeatureType featureType)
        {
            List<TableShapeInfo> list;
            Dict.TryGetValue (featureType, out list);
            return list;
        }

        public void SetShapeInfo (FeatureType featureType, List<TableShapeInfo> shapeInfos)
        {
            if (Dict.ContainsKey (featureType))
                Dict [featureType] = shapeInfos;
            else
                Dict.Add (featureType, shapeInfos);
        }

        public Dictionary<FeatureType, List<TableShapeInfo>> Dict { get; private set; }


        public static DefaultStyleLoader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DefaultStyleLoader ();
                return _instance;
            }
        }

        public static List<TableShapeInfo> GetDefaultStyle (FeatureType featType)
        {
            List<TableShapeInfo> list;
            Instance.Dict.TryGetValue (featType, out list);
            return list;
        }

        public static void GetDefaultStyle (FeatureType featType, List<TableShapeInfo> destList)
        {
            var list = GetDefaultStyle (featType);
            if (list != null)
                destList.AddRange (list);
        }

        private bool _isLoaded;
        private static DefaultStyleLoader _instance;
    }
}
