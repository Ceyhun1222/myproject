using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Utilities;
using Aran.Aim.DataTypes;
using Aran.Converters;
using Aran.Controls;
using Aran.Aim.Features;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Env.Layers;
using Aran.Aim.Metadata.Geo;

namespace MapEnv
{
    public partial class FilterControl : UserControl
    {
        private Control [] _operPanels;
        private FeatureType _featureType;
        private AimClassInfo _classInfo;
        public event EventHandler ValueChanged;


        public FilterControl ()
        {
            InitializeComponent ();
            CustomInitComponent ();
        }

        public void SetFilter (FeatureType featureType, Filter filter = null)
        {
            _featureType = featureType;
            _classInfo = UIMetadata.Instance.GetClassInfo ((int) featureType);

            GeoClassInfo geoClassInfo = GeoMetadata.GetGeoInfoByAimInfo (_classInfo);


            ui_compPropSelControl.ClassInfo = _classInfo;
            ui_layerFilterPropSelCont.ClassInfo = _classInfo;


            if (filter != null && filter.Operation.Choice == OperationChoiceType.Comparison)
            {
                ComparisonOps compOps = filter.Operation.ComparisonOps;

                if (!string.IsNullOrWhiteSpace (compOps.PropertyName))
                {
                    if (compOps.PropertyName.StartsWith ("<layerFilter>"))
                    {
                        ui_layerFilterPropSelCont.Text = compOps.PropertyName.Substring ("<layerFilter>".Length);
                        ui_operChoiceComboBox.SelectedIndex = ui_operChoiceComboBox.Items.Count - 1;
                        ui_featureLayersCB.Text = compOps.Value.ToString ();
                    }
                    else
                    {
                        ui_compPropSelControl.Text = compOps.PropertyName;

                        AimPropInfo propInfo = _classInfo.Properties [compOps.PropertyName];
                        ui_aimPropertyControl.PropInfo = propInfo;
                        ui_aimPropertyControl.SetValue (compOps.Value);

                        foreach (RadioButton rb in ui_compOperTypeFlowPanel.Controls)
                        {
                            if (compOps.OperationType.Equals (rb.Tag))
                            {
                                rb.Checked = true;
                                break;
                            }
                        }
                    }
                }

                if (compOps.Value is Guid &&
                    ui_aimPropertyControl.PropInfo != null &&
                    ui_aimPropertyControl.PropInfo.ReferenceFeature != 0 &&
                    ui_aimPropertyControl.PropInfo.PropType.Index == (int) DataType.FeatureRef )
                {
                    Guid identifier = (Guid) compOps.Value;

                    IDbProvider dbProvider = Globals.MainForm.DbProvider;

                    GettingResult result = dbProvider.GetVersionsOf (
                        ui_aimPropertyControl.PropInfo.ReferenceFeature,
                        TimeSliceInterpretationType.BASELINE,
                        identifier);

                    if (result.IsSucceed && result.List.Count > 0)
                    {
                        var feature = result.List [0] as Feature;
                        var featRef = new FeatureRef (feature.Identifier);

                        ui_aimPropertyControl.SetFeature (featRef, feature);
                    }
                }
            }
        }

        public Filter GetFilter ()
        {
            OperationChoice operChoice = null;

            object operChoiceItem = ui_operChoiceComboBox.SelectedItem;

            if (operChoiceItem is OperationChoiceType)
            {
                OperationChoiceType operChoiceType = (OperationChoiceType) operChoiceItem;

                if (operChoiceType == OperationChoiceType.Comparison)
                {
                    string propName = ui_compPropSelControl.Text;

                    if (string.IsNullOrWhiteSpace (propName))
                        return null;

                    object propValue = GetValue ();
                    if (propValue == null)
                        return null;

                    ComparisonOps compOp = new ComparisonOps (
                        GetCheckedComparisonOperType (),
                        propName);

                    compOp.Value = propValue;
                    operChoice = new OperationChoice (compOp);
                }
            }
            else if ("LayerFilter".Equals (operChoiceItem) && ui_featureLayersCB.SelectedItem != null)
            {
                string propName = "<layerFilter>" + ui_layerFilterPropSelCont.Text;
                string layerName = ui_featureLayersCB.SelectedItem.ToString ();

                ComparisonOps compOp = new ComparisonOps (ComparisonOpType.EqualTo, propName, layerName);

                operChoice = new OperationChoice (compOp);
            }

            return new Filter (operChoice);
        }

        private ComparisonOpType GetCheckedComparisonOperType ()
        {
            foreach (RadioButton rb in ui_compOperTypeFlowPanel.Controls)
            {
                if (rb.Checked)
                    return (ComparisonOpType) rb.Tag;
            }

            throw new Exception ("No Comparison Operation Type Checked");
        }

        public FeatureType FeatureType
        {
            get { return _featureType; }
        }

        
        private void CustomInitComponent ()
        {
            ui_operChoiceComboBox.Items.Add (OperationChoiceType.Comparison);
            ui_operChoiceComboBox.Items.Add (OperationChoiceType.Logic);
            ui_operChoiceComboBox.Items.Add (OperationChoiceType.Spatial);
            ui_operChoiceComboBox.Items.Add ("LayerFilter");

            _operPanels = new Control [ui_opersTabControl.TabPages.Count];
            for (int i = 0; i < _operPanels.Length; i++)
            {
                Control control = ui_opersTabControl.TabPages [i].Controls [0];
                control.Parent = ui_opersTabControl.Parent;
                control.Location = ui_opersTabControl.Location;
                _operPanels [i] = control;
            }
            
            ui_aimPropertyControl.FeatureDescription += Globals.AimPropertyControl_FeatureDescription;
            ui_aimPropertyControl.LoadFeatureListByDependHandler = Globals.GetFeatureListByDepend;
            ui_aimPropertyControl.FillDataGridColumnsHandler = Globals.AimPropertyControl_FillDataGridColumn;
            ui_aimPropertyControl.SetDataGridRowHandler = Globals.AimPropertyControl_SetRow;
        }

        private void AddComparisonOpers (IEnumerable<ComparisonOpType> compOpTypeItems)
        {
            ui_compOperTypeFlowPanel.Controls.Clear ();
            if (compOpTypeItems == null)
                return;

            foreach (ComparisonOpType compOperType in compOpTypeItems)
            {
                RadioButton rb = new RadioButton ();
                rb.AutoSize = true;
                rb.Appearance = Appearance.Button;
                rb.Text = GetCompOperTypeText (compOperType);
                rb.Tag = compOperType;

                if (compOperType == ComparisonOpType.NotEqualTo)
                {
                    rb.AutoSize = false;
                    rb.Font = new Font ("Arial", 10, FontStyle.Regular);
                    rb.Size = new Size (22, 22);
                }
                ui_toolTip.SetToolTip (rb, compOperType.ToString ());
                ui_compOperTypeFlowPanel.Controls.Add (rb);
            }

            if (ui_compOperTypeFlowPanel.Controls.Count > 0)
                (ui_compOperTypeFlowPanel.Controls [0] as RadioButton).Checked = true;
        }

        private void FilterControl_Load (object sender, EventArgs e)
        {
            if (ui_operChoiceComboBox.SelectedIndex == -1)
                ui_operChoiceComboBox.SelectedIndex = 0;
        }

        private void OperChoice_SelectedIndexChanged (object sender, EventArgs e)
        {
            for (int i = 0; i < _operPanels.Length; i++)
                _operPanels [i].Visible = (i == ui_operChoiceComboBox.SelectedIndex);

            DoValueChanged ();
        }

        private string GetCompOperTypeText (ComparisonOpType compOperType)
        {
            switch (compOperType)
            {
                case ComparisonOpType.EqualTo: return "=";
                case ComparisonOpType.GreaterThan: return ">";
                case ComparisonOpType.GreaterThanOrEqualTo: return ">=";
                case ComparisonOpType.LessThan: return "<";
                case ComparisonOpType.LessThanOrEqualTo: return "<=";
                case ComparisonOpType.NotEqualTo: return "≠";
                default: return compOperType.ToString ();
            }
        }

        private object GetValue ()
        {
            IAimProperty aimProp = ui_aimPropertyControl.Value;

            if (aimProp.PropertyType == AimPropertyType.AranField)
            {
                IEditAimField editAimField = aimProp as IEditAimField;
                object value = editAimField.FieldValue;

                Type [] genTypeArr = aimProp.GetType ().GetGenericArguments ();
                if (genTypeArr.Length > 0)
                {
                    value = Convert.ChangeType (value, genTypeArr [0]);
                }

                return value;
            }
            else if (aimProp.PropertyType == AimPropertyType.DataType)
            {
                if (aimProp is IEditValClass)
                {
                    IEditValClass editValClass = aimProp as IEditValClass;
                    double valueAsMetre = ConverterToSI.Convert (editValClass, 0);
                    return valueAsMetre;
                }
                else if (aimProp is FeatureRef)
                {
                    return (aimProp as FeatureRef).Identifier;
                }
            }

            return null;
        }

        private void CompPropSel_AfterSelect (object sender, PropertySelectedEventArgs e)
        {
            AimPropInfo propInfo = e.SelectedProp.LastOrDefault ();
            if (propInfo == null)
            {
                e.Cancel = true;
                return;
            }

            if (propInfo.PropType.AimObjectType == AimObjectType.Object)
            {
                e.Cancel = true;
                return;
            }

            ui_aimPropertyControl.PropInfo = propInfo;

            var compOpTypeItems = GetComparisonOpersByPropInfo (propInfo);
            AddComparisonOpers (compOpTypeItems);
        }

        private IEnumerable<ComparisonOpType> GetComparisonOpersByPropInfo (AimPropInfo propInfo)
        {
            List<ComparisonOpType> list = new List<ComparisonOpType> ();

            if (propInfo.PropType.AimObjectType == AimObjectType.Field)
            {
                AimFieldType type = (AimFieldType) propInfo.TypeIndex;

                list.Add (ComparisonOpType.EqualTo);
                list.Add (ComparisonOpType.NotEqualTo);
                list.Add (ComparisonOpType.NotNull);
                list.Add (ComparisonOpType.Null);

                switch (type)
                {
                    case AimFieldType.SysString:
                        list.Add (ComparisonOpType.Like);
                        list.Add (ComparisonOpType.NotLike);
                        break;
                    case AimFieldType.SysDateTime:
                    case AimFieldType.SysDouble:
                    case AimFieldType.SysInt32:
                    case AimFieldType.SysInt64:
                    case AimFieldType.SysUInt32:
                        list.Add (ComparisonOpType.GreaterThan);
                        list.Add (ComparisonOpType.GreaterThanOrEqualTo);
                        list.Add (ComparisonOpType.LessThan);
                        list.Add (ComparisonOpType.LessThanOrEqualTo);
                        break;
                }
            }
            else if (propInfo.PropType.AimObjectType == AimObjectType.DataType &&
                propInfo.PropType.SubClassType == AimSubClassType.ValClass)
            {
                list.Add (ComparisonOpType.EqualTo);
                list.Add (ComparisonOpType.NotEqualTo);
                list.Add (ComparisonOpType.NotNull);
                list.Add (ComparisonOpType.Null);
                list.Add (ComparisonOpType.GreaterThan);
                list.Add (ComparisonOpType.GreaterThanOrEqualTo);
                list.Add (ComparisonOpType.LessThan);
                list.Add (ComparisonOpType.LessThanOrEqualTo);
            }

            return list;
        }

        private void LayerFilterPropSel_AfterSelect (object sender, PropertySelectedEventArgs e)
        {
            
        }

        private void LayerFilterPropSel_ValueChanged (object sender, EventArgs e)
        {
            ui_featureLayersCB.Items.Clear ();


            AimPropInfo [] propInfoArr = ui_layerFilterPropSelCont.Value;

            if (propInfoArr.Length == 0)
                return;

            AimPropInfo propInfo = propInfoArr [propInfoArr.Length - 1];

            if (propInfo.ReferenceFeature != 0)
            {
                for (int i = 0; i < Globals.MainForm.Map.LayerCount; i++)
                {
                    if (Globals.MainForm.Map.get_Layer (i) is AimFeatureLayer)
                    {
                        AimFeatureLayer aimFeatureLayer = Globals.MainForm.Map.get_Layer (i) as AimFeatureLayer;
                        if (aimFeatureLayer.AimTable.FeatureType == propInfo.ReferenceFeature)
                        {
                            ui_featureLayersCB.Items.Add (aimFeatureLayer.Name);
                        }
                    }
                }
            }
        }

        private void DoValueChanged ()
        {
            if (ValueChanged != null)
                ValueChanged (this, null);
        }

        private void CompPropSel_ValueChanged (object sender, EventArgs e)
        {
            DoValueChanged ();
        }
    }
}
