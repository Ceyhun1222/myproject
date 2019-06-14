using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.DataTypes;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Enums;

namespace Aran.Queries.Common
{
    public partial class FeatureSelectorForm : Form
    {
        private IEnumerable<Feature> _featureList;
        private AimPropInfo _propInfo;
        private UIMetadata _uiMetaData;
        private AimClassInfo _mainClassInfo;
        private int _indexCurrRoot;
        private bool _isAbstractFeature;

        //public FillDataGridColumnsHandler DataGridColumnsFilled;
        public SetDataGridRowHandler DataGridRowSetted;
        public FeatureListByDependEventHandler GetFeatListByDepend;

        public FeatureSelectorForm (AimPropInfo propInfo)
        {
            InitializeComponent ();

            _propInfo = propInfo;

            ui_dgv.SortCompare += UIUtilities.DGV_SortCompare;
        }

        public FeatureRef SelectedFeatureRef { get; set; }

        public Feature SelectedFeature { get; private set; }

        public AimPropInfo PropInfo
        {
            get { return _propInfo; }
        }

        public Dictionary<AimClassInfo, Feature> Features
        {
            get;
            set;
        }

        public IEnumerable<Feature> FeatureList
        {
            get { return _featureList; }
            //set
            //{
            //    _featureList = value;

            //    if (Visible)
            //    {
            //        FillRows ();
            //    }
            //}
        }

        public DateTime? EffectiveDate { get; set; }

        
        private void FillRows (FeatureType featureType)
        {
            if (GetFeatListByDepend == null || DataGridRowSetted == null)
                return;

            Feature previousFeat;
            if (IndexCurrRoot == 0)
                previousFeat = null;
            else
                previousFeat = Features.ElementAt (IndexCurrRoot - 1).Value;

            FeatureListByDependEventArgs fldtArg = new FeatureListByDependEventArgs (
                featureType,
                TimeSliceInterpretationType.BASELINE,
                EffectiveDate ?? Global.DefaultEffectiveDate,
                previousFeat);

            GetFeatListByDepend (this, fldtArg);

            _featureList = fldtArg.FeatureList;

            foreach (Feature feature in _featureList)
                DataGridRowSetted (ui_dgv, feature);
        }

        private DataGridViewColumn ToDataGridViewColumn (AimPropInfo propInfo)
        {
            AimObjectType aimObjectType = AimMetadata.GetAimObjectType (propInfo.TypeIndex);

            DataGridViewColumn col = null;

            if (aimObjectType == AimObjectType.Field)
            {
                AimFieldType aimFieldType = (AimFieldType) propInfo.TypeIndex;

                switch (aimFieldType)
                {
                    case AimFieldType.SysBool:
                        DataGridViewCheckBoxColumn chbCol = new DataGridViewCheckBoxColumn ();
                        col = chbCol;
                        break;
                    case AimFieldType.SysString:
                        DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                        col = tbCol;
                        break;
                }
            }

            if (col != null)
            {
                col.Name = propInfo.Name;
                col.HeaderText = propInfo.Name;
                col.Tag = propInfo;
            }

            return col;

        }

        private Dictionary<AimClassInfo, Feature> GetFeatures (AimClassInfo classInfo)
        {
            if (_uiMetaData == null)
            {
                _uiMetaData = UIMetadata.Instance;
            }
            string dependFeature = classInfo.UiInfo ().DependsFeature;
            Dictionary<AimClassInfo, Feature> rootFeats = new Dictionary<AimClassInfo, Feature> ();
            //FeatureType featType;
            AimClassInfo aimClassInfo = classInfo;
            List<AimClassInfo> aimClassInfoList = new List<AimClassInfo> ();
            
            //while (dependFeature != null)
            //{
            //    //Enum.TryParse<FeatureType> ( dependFeature, out featType );
            //    aimClassInfo = _uiMetaData.ClassInfoList.Where (cInfo => cInfo.Name == dependFeature).First ();
            //    aimClassInfoList.Add (aimClassInfo);
            //    dependFeature = aimClassInfo.UiInfo ().DependsFeature;
            //}

            while (!string.IsNullOrEmpty (dependFeature))
            {
                //Enum.TryParse<FeatureType> ( dependFeature, out featType );
                aimClassInfo = _uiMetaData.ClassInfoList.Where (cInfo => cInfo.Name == dependFeature).First ();
                aimClassInfoList.Add (aimClassInfo);
                dependFeature = aimClassInfo.UiInfo ().DependsFeature;
            }
            
            aimClassInfoList.Reverse ();
            foreach (var item in aimClassInfoList)
            {
                rootFeats.Add (item, AimObjectFactory.CreateFeature ((FeatureType) item.Index));
            }
            rootFeats.Add (classInfo, AimObjectFactory.CreateFeature ((FeatureType) classInfo.Index));
            return rootFeats;
        }

        private void FeatureSelectorForm_Load (object sender, EventArgs e)
        {
            //if (DataGridColumnsFilled == null)
            //    return;

            if (_propInfo.ReferenceFeature != 0)
            {
                _isAbstractFeature = false;
                ui_topPanel.Visible = false;
                _mainClassInfo = AimMetadata.GetClassInfoByIndex ((int) _propInfo.ReferenceFeature);
                Features = GetFeatures (_mainClassInfo);
                IndexCurrRoot = 0;
                AddFeatTypeToComboBox (Features.ElementAt (IndexCurrRoot).Key);
            }
            else
            {
                IndexCurrRoot = 0;
                _isAbstractFeature = true;
                AimClassInfo classInfo = _propInfo.PropType;
                if (classInfo.SubClassType != AimSubClassType.AbstractFeatureRef)
                    return;

                string s = classInfo.Name.Substring ("Abstract".Length);
                s = s.Substring (0, s.Length - "Ref".Length);

                Text = "Select " + s;

                string absEnumTypeName = "Aran.Aim." + s + "Type";

                Type enumType = typeof (FeatureType).Assembly.GetType (absEnumTypeName);
                Array enumItemArr = Enum.GetValues (enumType);

                for (int i = 0; i < enumItemArr.Length; i++)
                {
                    int featureTypeIndex = (int) enumItemArr.GetValue (i);
                    AimClassInfo classInfoItem = AimMetadata.GetClassInfoByIndex (featureTypeIndex);
                    ui_featureTypeComboBox.Items.Add (new ComboBoxItem (classInfoItem));
                }
            }

            ui_featureTypeComboBox.SelectedIndex = 0;
            //ui_featureTypeComboBox_SelectedIndexChanged (ui_featureTypeComboBox, null);
        }

        private void dgv_CurrentCellChanged (object sender, EventArgs e)
        {
            if (Features != null && IndexCurrRoot != Features.Count - 1)
                ui_nextButton.Enabled = (ui_dgv.CurrentRow != null);
            else
                ui_okButton.Enabled = (ui_dgv.CurrentRow != null);
        }

        private void FeatureType_SelectedIndexChanged (object sender, EventArgs e)
        {
            if (ui_featureTypeComboBox.SelectedItem == null)
                return;

            AimClassInfo classInfo = ((ComboBoxItem) ui_featureTypeComboBox.SelectedItem).ClassInfo;

            UIUtilities.FillColumns (classInfo, ui_dgv);
            //DataGridColumnsFilled (classInfo, ui_dgv);
            FillRows ((FeatureType) classInfo.Index);
        }

        private void Next_Click (object sender, EventArgs e)
        {
            Features [Features.ElementAt (IndexCurrRoot).Key] = ((Feature) ui_dgv.CurrentRow.Tag);
            IndexCurrRoot++;
            AddFeatTypeToComboBox (Features.ElementAt (IndexCurrRoot).Key);
        }

        private void OK_Click (object sender, EventArgs e)
        {
            if (ui_dgv.CurrentRow == null || ui_featureTypeComboBox.SelectedItem == null)
                return;

            if (_isAbstractFeature)
            {
                IAbstractFeatureRef absFeatRef = (IAbstractFeatureRef) AimObjectFactory.CreateADataType (
                    (DataType) PropInfo.TypeIndex);
                absFeatRef.FeatureTypeIndex = ((ComboBoxItem) ui_featureTypeComboBox.SelectedItem).ClassInfo.Index;
                SelectedFeatureRef = (FeatureRef) absFeatRef;
            }
            else
            {
                SelectedFeatureRef = new FeatureRef ();
            }

            var feat = (Feature) ui_dgv.CurrentRow.Tag;

            SelectedFeatureRef.Identifier = feat.Identifier;
            SelectedFeature = feat;

            DialogResult = DialogResult.OK;
        }

        private void Back_Click (object sender, EventArgs e)
        {
            IndexCurrRoot--;
            AddFeatTypeToComboBox (Features.ElementAt (IndexCurrRoot).Key);
        }

        private void AddFeatTypeToComboBox (AimClassInfo classInfo)
        {
            ui_featureTypeComboBox.Items.Clear ();
            ui_featureTypeComboBox.Items.Add (new ComboBoxItem (classInfo));
            Text = "Select " + classInfo.AixmName;
            ui_featureTypeComboBox.SelectedIndex = 0;
        }

        private int IndexCurrRoot
        {
            get
            {
                return _indexCurrRoot;
            }
            set
            {
                _indexCurrRoot = value;
                ui_backButton.Enabled = (_indexCurrRoot > 0);
                ui_nextButton.Enabled = (Features != null && _indexCurrRoot != Features.Count - 1);
                ui_okButton.Enabled = !ui_nextButton.Visible;
            }
        }

        internal class ComboBoxItem
        {
            public ComboBoxItem (AimClassInfo classInfo)
            {
                ClassInfo = classInfo;
            }

            public override string ToString ()
            {
                return ClassInfo.Name;
            }

            public AimClassInfo ClassInfo { get; private set; }
        }
    }
}