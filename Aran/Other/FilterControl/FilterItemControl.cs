using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data.Filters;
using Aran.Aim.Metadata.UI;
using Aran.Aim.Utilities;
using Aran.Queries.Common;

namespace Aran.Controls
{
    public partial class FilterItemControl : UserControl
    {
        public const string GeoDWithinText = "Distance Within";
        public const string GeoWithinText = "Within";

        private AimPropInfo[] _propInfoArr;

        public event EventHandler ValueChanged;


        public FilterItemControl()
        {
            InitializeComponent();

            ui_dwithinGroupBox.Top = ui_aimPropertyControl.Top;
            ui_aimPropertyControl.ValueChanged += new EventHandler(OnValueChanged);
        }


        public AimClassInfo ClassInfo { get; set; }

        public AimPropInfo[] PropInfoArr
        {
            get { return _propInfoArr; }
            set
            {
                _propInfoArr = value;
                ui_propNameTB.Text = string.Join<string>(" > ", value.Select(pi => pi.Name));
                ui_propNameTB.Tag = value;

                var psea = new PropertySelectedEventArgs(value);
                PropSel_AfterSelect(null, psea);
            }
        }

        public void SetOperation(OperationChoice oper)
        {
            if (oper.Choice == OperationChoiceType.Spatial) {
                var dw = oper.SpatialOps as DWithin;
                if (dw != null && !string.IsNullOrEmpty(dw.PropertyName)) {
                    PropInfoArr = AimMetadataUtility.GetInnerProps(ClassInfo.Index, dw.PropertyName);
                    ui_dwithinControl.SetValue(dw);
                }
            }
            else if (oper.Choice == Aim.Data.Filters.OperationChoiceType.Comparison) {
                var co = oper.ComparisonOps;

                if (!string.IsNullOrEmpty(co.PropertyName)) {
                    PropInfoArr = AimMetadataUtility.GetInnerProps(ClassInfo.Index, co.PropertyName);
                    ui_aimPropertyControl.SetValue(co.Value);

                    for (int i = 0; i < ui_operTypeCB.Items.Count; i++) {
                        if ((ui_operTypeCB.Items[i] as OperTypeItem).OpType == co.OperationType) {
                            ui_operTypeCB.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        public OperationChoice GetOperation()
        {
            var oti = ui_operTypeCB.SelectedItem as OperTypeItem;

            if (oti == null)
                return null;

            string propName = GetPropertyName();

            if (oti.Text == GeoDWithinText) {
                var dw = ui_dwithinControl.GetValue(propName);
                return new OperationChoice(dw);
            }
            else {
                object val = ui_aimPropertyControl.Value;
                if (val is IEditAimField)
                    val = (val as IEditAimField).FieldValue;
                else if (val is Aim.DataTypes.FeatureRef)
                    val = (val as Aim.DataTypes.FeatureRef).Identifier;

                var co = new ComparisonOps();
                co.PropertyName = propName;
                co.OperationType = oti.OpType;
                co.Value = val;

                return new OperationChoice(co);
            }
        }

        public Control AndTextControl { get; set; }


        protected override void OnLoad(EventArgs eventArgs)
        {
            ui_aimPropertyControl.FeatureDescription += (s, e) => { return UIUtilities.GetFeatureDescription(e.Feature); };
            ui_aimPropertyControl.LoadFeatureListByDependHandler = InternalGlobal.LoadFeatureListByDependHandler;
            ui_aimPropertyControl.FillDataGridColumnsHandler = (classInfo, dgv) => { UIUtilities.FillColumns(classInfo, dgv); };
            ui_aimPropertyControl.SetDataGridRowHandler = (dgv, feature, rowIndex) => { UIUtilities.SetRow(dgv, feature, rowIndex); };

            base.OnLoad(eventArgs);
        }

        private void PropSel_AfterSelect(object sender, PropertySelectedEventArgs e)
        {
            var propInfo = e.SelectedProp.LastOrDefault();

            if (propInfo == null) {
                e.Cancel = true;
                return;
            }

            if (propInfo.PropType.AimObjectType == AimObjectType.Object) {
                if (propInfo.PropType.Index == (int)ObjectType.FeatureRefObject) {
                    var tmp = propInfo.PropType.Properties["Feature"].Clone();
                    tmp.ReferenceFeature = propInfo.ReferenceFeature;
                    propInfo = tmp;
                }
                else {
                    e.Cancel = true;
                    return;
                }
            }

            bool isGeo;
            var compOpTypeItems = GetComparisonOpersByPropInfo(propInfo, out isGeo);

            ui_operTypeCB.Items.Clear();

            if (isGeo) {
                ui_operTypeCB.Items.Add(new OperTypeItem(GeoDWithinText));
                ui_operTypeCB.Items.Add(new OperTypeItem(GeoWithinText));
            }
            else {
                foreach (ComparisonOpType compOperType in compOpTypeItems)
                    ui_operTypeCB.Items.Add(new OperTypeItem(compOperType));
            }

            ui_aimPropertyControl.PropInfo = propInfo;

            if (ui_operTypeCB.Items.Count > 0)
                ui_operTypeCB.SelectedIndex = 0;

            ui_operValuePanel.Visible = (ui_operTypeCB.Items.Count > 0);
        }

        private IEnumerable<ComparisonOpType> GetComparisonOpersByPropInfo(AimPropInfo propInfo, out bool isGeo)
        {
            isGeo = false;
            List<ComparisonOpType> list = new List<ComparisonOpType>();

            list.Add(ComparisonOpType.EqualTo);
            list.Add(ComparisonOpType.NotEqualTo);
            list.Add(ComparisonOpType.NotNull);
            list.Add(ComparisonOpType.Null);

            if (propInfo.PropType.AimObjectType == AimObjectType.Field) {
                AimFieldType type = (AimFieldType)propInfo.TypeIndex;

                switch (type) {
                    case AimFieldType.SysString:
                        list.Add(ComparisonOpType.Like);
                        list.Add(ComparisonOpType.NotLike);
                        break;
                    case AimFieldType.SysDateTime:
                    case AimFieldType.SysDouble:
                    case AimFieldType.SysInt32:
                    case AimFieldType.SysInt64:
                    case AimFieldType.SysUInt32:
                        list.Add(ComparisonOpType.GreaterThan);
                        list.Add(ComparisonOpType.GreaterThanOrEqualTo);
                        list.Add(ComparisonOpType.LessThan);
                        list.Add(ComparisonOpType.LessThanOrEqualTo);
                        break;
                    case AimFieldType.GeoPoint:
                    case AimFieldType.GeoPolyline:
                    case AimFieldType.GeoPolygon:
                        isGeo = true;
                        break;
                }
            }
            else if (propInfo.PropType.AimObjectType == AimObjectType.DataType &&
                propInfo.PropType.SubClassType == AimSubClassType.ValClass) {
                list.Add(ComparisonOpType.GreaterThan);
                list.Add(ComparisonOpType.GreaterThanOrEqualTo);
                list.Add(ComparisonOpType.LessThan);
                list.Add(ComparisonOpType.LessThanOrEqualTo);
            }

            return list;
        }

        private void OperType_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperTypeItem oti;
            bool b = ((oti = ui_operTypeCB.SelectedItem as OperTypeItem) != null &&
                oti.Text == GeoDWithinText);

            ui_aimPropertyControl.Visible = !b;
            ui_dwithinGroupBox.Visible = b;

            DoValueChanged();
        }

        private string GetPropertyName()
        {
            var propInfoArr = ui_propNameTB.Tag as AimPropInfo[];
            return string.Join<string>(".", propInfoArr.Select(pi => pi.AixmName));
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            DoValueChanged();
        }

        private void DoValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, null);
        }

        private void SettingsPicBox_Click(object sender, EventArgs e)
        {
            ui_settingsMenuStrip.Show(ui_settingsPicBox,
                new Point(ui_settingsPicBox.Width, ui_settingsPicBox.Height),
                ToolStripDropDownDirection.BelowLeft);
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
        }
    }

    internal class OperTypeItem
    {
        public OperTypeItem(ComparisonOpType opType)
        {
            OpType = opType;
        }

        public OperTypeItem(string text)
        {
            Text = text;
        }

        public ComparisonOpType OpType
        {
            get { return _opType; }
            set
            {
                _opType = value;
                Text = GetText(value);
            }
        }

        public string Text { get; private set; }

        public static string GetText(ComparisonOpType opType)
        {
            switch (opType) {
                case ComparisonOpType.EqualTo: return "=";
                case ComparisonOpType.GreaterThan: return ">";
                case ComparisonOpType.GreaterThanOrEqualTo: return ">=";
                case ComparisonOpType.LessThan: return "<";
                case ComparisonOpType.LessThanOrEqualTo: return "<=";
                case ComparisonOpType.NotEqualTo: return "≠";
                case ComparisonOpType.Null: return "Is NULL";
                case ComparisonOpType.NotNull: return "Is Not NULL";
                default: return opType.ToString();
            }
        }

        public override string ToString()
        {
            return Text;
        }

        private ComparisonOpType _opType;
    }
}
