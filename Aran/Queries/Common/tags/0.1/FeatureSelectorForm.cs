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

namespace Aran.Queries.Common
{
    public partial class FeatureSelectorForm : Form
    {
        private IEnumerable<Feature> _featureList;
        private AimPropInfo _propInfo;
        public FillDataGridColumnsHandler DataGridColumnsFilled;
        public SetDataGridRowHandler DataGridRowSetted;
        public GetFeatureListHandler GetFeatureList;
        public bool _isAbstractFeature;
        
        public FeatureSelectorForm (AimPropInfo propInfo)
        {
            InitializeComponent ();

            _propInfo = propInfo;
        }

        public FeatureRef SelectedFeatureRef { get; private set; }

        public AimPropInfo PropInfo
        {
            get { return _propInfo; }
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

        private void FillRows (FeatureType featureType)
        {
            if (GetFeatureList == null || DataGridRowSetted == null)
                return;

            _featureList = GetFeatureList (featureType);

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

        private void okButton_Click (object sender, EventArgs e)
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

            SelectedFeatureRef.Identifier = ((Feature) ui_dgv.CurrentRow.Tag).Identifier;

            DialogResult = DialogResult.OK;
        }

        private void FeatureSelectorForm_Load (object sender, EventArgs e)
        {
            if (DataGridColumnsFilled == null)
                return;

            if (_propInfo.ReferenceFeature != 0)
            {
                _isAbstractFeature = false;
                ui_topPanel.Visible = false;
                AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex ((int) _propInfo.ReferenceFeature);
                ui_featureTypeComboBox.Items.Add (new ComboBoxItem (classInfo));
                
                Text = "Select " + classInfo.AixmName;
            }
            else
            {
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
            ui_okButton.Enabled = (ui_dgv.CurrentRow != null);
        }

        private void ui_featureTypeComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            if (ui_featureTypeComboBox.SelectedItem == null)
                return;

            AimClassInfo classInfo = ((ComboBoxItem) ui_featureTypeComboBox.SelectedItem).ClassInfo;
            DataGridColumnsFilled (classInfo, ui_dgv);
            FillRows ((FeatureType) classInfo.Index);
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
