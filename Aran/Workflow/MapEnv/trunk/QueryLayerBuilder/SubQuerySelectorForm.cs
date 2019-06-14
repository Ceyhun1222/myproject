using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data;

namespace MapEnv
{
    public partial class SubQuerySelectorForm : Form
    {
        private FeatureType _featureType;
        private bool _isSubQuery;
        private bool _abstractSelected;
        private User _user;

        public SubQuerySelectorForm (bool isSubQuery, User user)
        {
            InitializeComponent ();
            _isSubQuery = isSubQuery;
            _user = user;

            if (_user == null)
            {
                throw new Exception ("User not defined");
            }

            ui_featureTypePanel.Visible = !isSubQuery;
            ui_propertyPanel.Visible = isSubQuery;

            if (!isSubQuery)
            {
                var values = Enum.GetValues (typeof (FeatureType));
                foreach (FeatureType ft in values)
                {
                    if (_user.ContainsFeatType (ft))
                        ui_featureTypeCB.Items.Add (ft);
                }
            }
        }

        public FeatureType FeatureType
        {
            get
            {
                return _featureType;
            }
            set
            {
                _featureType = value;

                if (!_isSubQuery)
                {
                    ui_featureTypeCB.SelectedItem = value;
                }
                else
                {
                    var classInfo = AimMetadata.GetClassInfoByIndex ((int) _featureType);
                    ui_compPropSelControl.ClassInfo = classInfo;
                }
            }
        }

        public string PropertyPath
        {
            get { return ui_compPropSelControl.Text; }
        }

        public bool IsAbstractFeature (out List<FeatureType> checkedFeatures)
        {
            checkedFeatures = null;
            if (!_abstractSelected)
                return false;

            checkedFeatures = new List<FeatureType> ();
            foreach (FeatureType ft in ui_absFeaturesCLB.CheckedItems)
            {
                checkedFeatures.Add (ft);
            }
            return true;
        }


        private void SubQuerySelectorForm_Load (object sender, EventArgs e)
        {
            ResetHeight ();
        }

        private void Ok_Click (object sender, EventArgs e)
        {
            if (_abstractSelected && ui_absFeaturesCLB.CheckedItems.Count > 0)
            {
                _featureType = (FeatureType) ui_absFeaturesCLB.CheckedItems [0];
            }

            if (_featureType == 0)
                throw new Exception ("FeatureType not defined");

            DialogResult = DialogResult.OK;
        }

        private void FeatureType_TextChanged (object sender, EventArgs e)
        {
            FeatureType ft;
            bool res = Enum.TryParse<FeatureType> (ui_featureTypeCB.Text, true, out ft);
            bool contains = _user.ContainsFeatType (ft);
            ui_okButton.Enabled = res && contains;

            if (res && contains)
            {
                _featureType = ft;
                //ui_featureTypeCB.SelectedItem = ft;
            }
        }

        private void CompPropSel_AfterSelect (object sender, PropertySelectedEventArgs e)
        {
            AimPropInfo propInfo = e.SelectedProp.LastOrDefault ();

            //--- Edited by Abuzer...

            //var a = AimObjectFactory.Create ( propInfo.PropType.Index );
            //if (propInfo == null || !propInfo.IsFeatureReference || 
            //    ! _user.ContainsFeatType ( propInfo.ReferenceFeature ) || 
            //    propInfo.PropType.SubClassType != AimSubClassType.AbstractFeatureRef)
            //{
            //    e.Cancel = true;
            //    return;
            //}

            if (propInfo == null || !propInfo.IsFeatureReference)
            {
                e.Cancel = true;
                return;
            }
        }

        private void CompPropSel_ValueChanged (object sender, EventArgs e)
        {
            bool enableOk = false;
            AimPropInfo propInfo = null;
            _abstractSelected = false;

            if (ui_compPropSelControl.Value != null)
            {
                propInfo = ui_compPropSelControl.Value.LastOrDefault ();
                if (propInfo != null && propInfo.IsFeatureReference)
                {
                    if (propInfo.PropType.SubClassType == AimSubClassType.AbstractFeatureRef)
                    {
                        _abstractSelected = true;
                    }
                    else
                    {
                        _featureType = propInfo.ReferenceFeature;
                        enableOk = true;
                    }
                }
            }

            ui_okButton.Enabled = enableOk;

            if (_abstractSelected)
            {
                var classInfo = AimMetadata.GetClassInfoByIndex (propInfo.TypeIndex);
                var list = Aran.Aim.Utilities.AimMetadataUtility.GetAbstractChilds (classInfo);
                ui_absFeaturesCLB.Items.Clear ();
                foreach (var ci in list)
                {
                    FeatureType ft = (FeatureType) ci.Index;
                    ui_absFeaturesCLB.Items.Add (ft);
                }
            }

            ui_absFeaturePanel.Visible = _abstractSelected;

            ResetHeight ();
        }

        private void ResetHeight ()
        {
            Height = flowLayoutPanel1.Height + 85;
        }

        private void AbsFeatures_ItemCheck (object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                ui_okButton.Enabled = true;
            else
                ui_okButton.Enabled = (ui_absFeaturesCLB.CheckedItems.Count > 1);
        }
    }
}
