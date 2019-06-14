using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Queries.Common;

namespace Aran.Controls
{
    public partial class AimPropertyControl : UserControl
    {
        private AimPropInfo _propInfo;
        private AimObject _aimObject;

        #region Event Handlers

        public event EventHandler ValueChanged;

        public event FeatureDescriptionEventHandler FeatureDescription;

        public FeatureListByDependEventHandler LoadFeatureListByDependHandler { get; set; }

        public SetDataGridRowHandler SetDataGridRowHandler { get; set; }

        public FillDataGridColumnsHandler FillDataGridColumnsHandler { get; set; }

        #endregion

        public AimPropertyControl ()
        {
            InitializeComponent ();
        }


        public AimPropInfo PropInfo
        {
            get
            {
                return _propInfo;
            }
            set
            {

                _propInfo = value;

                if (_propInfo == null)
                    return;

                Controls.Clear ();

                bool defaultWidth;
                Control cont = GetPropInfoControl (out defaultWidth);

                if (cont == null)
                    return;
                
                BorderStyle = BorderStyle.None;

                if (defaultWidth)
                {
                    cont.Dock = DockStyle.Fill;
                }

                Controls.Add (cont);
            }
        }

        public IAimProperty Value
        {
            get
            {
                return _aimObject as IAimProperty;
            }
        }

        public void SetValue (object value)
        {
            IEditAimField editAimField = _aimObject as IEditAimField;
            if (editAimField != null)
            {
                editAimField.FieldValue = value;

                if (Controls.Count > 0)
                {
                    Control control = Controls [0];

                    if (control is TextBox)
                    {
                        ((TextBox) control).Text = value.ToString ();
                    }
                    else if (control is ComboBox)
                    {
                        if (editAimField.FieldValue is bool)
                            ((ComboBox)control).SelectedIndex = ((bool)editAimField.FieldValue ? 0 : 1);
                        else
                            ((ComboBox)control).SelectedItem = editAimField.FieldValue;
                    }
                }
            }
            else if (value is IEditValClass)
            {
                var valClass = value as IEditValClass;
                Control control = Controls [0];

                if (control.Controls.Count == 2)
                {
                    for (int i = 0; i < control.Controls.Count; i++)
                    {
                        if (control.Controls [i] is NumericUpDown)
                        {
                            var nud = control.Controls [i] as NumericUpDown;
                            nud.Value = (decimal) valClass.Value;
                        }
                        else if (control.Controls [i] is ComboBox)
                        {
                            var cb = control.Controls [i] as ComboBox;

                            foreach (object cbItem in cb.Items)
                            {
                                if (Convert.ToInt32 (cbItem) == valClass.Uom)
                                {
                                    cb.SelectedItem = cbItem;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetFeature (FeatureRef featureRef, Feature feature)
        {
            _aimObject = featureRef;

            string featText = featureRef.Identifier.ToString (); ;

            if (FeatureDescription != null && feature != null)
                featText = FeatureDescription (this, new FeatureEventArgs (feature));

            if (Controls.Count > 0 && Controls [0] is LinkLabel)
            {
                LinkLabel link = Controls [0] as LinkLabel;
                link.Text = featText;
            }
        }


        private Control GetPropInfoControl (out bool defaultWidth)
        {
            defaultWidth = true;
            Control cont = null;

            AimClassInfo classInfo = AimMetadata.GetClassInfoByIndex (_propInfo.TypeIndex);

            _aimObject = AimObjectFactory.Create (_propInfo.TypeIndex);

            if (classInfo.AimObjectType == AimObjectType.Field)
            {
                #region Field

                AimFieldType aimFieldType = (AimFieldType) _propInfo.TypeIndex;

                if (AimMetadata.IsEnum (_propInfo.TypeIndex))
                {
                    ComboBox cb = new ComboBox ();
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.SelectedIndexChanged += new EventHandler (events_ComboBox_SelectedIndexChanged);

                    EnumType enumType = (EnumType) _propInfo.TypeIndex;
                    string typeName = "Aran.Aim.Enums." + enumType;
                    Type en = typeof (AimField).Assembly.GetType (typeName);
                    Array array = Enum.GetValues (en);

                    foreach (object enumItem in array)
                        cb.Items.Add (enumItem);

                    if (cb.Items.Count > 0)
                    {
                        cb.SelectedIndex = 0;
                        if (ValueChanged != null)
                            ValueChanged(this, new EventArgs());
                    }

                    cont = cb;
                }
                else
                {
                    switch (aimFieldType)
                    {
                        case AimFieldType.GeoPoint:
                        case AimFieldType.GeoPolygon:
                        case AimFieldType.GeoPolyline:
                            {
                                Panel geoPanel = new Panel ();
                                geoPanel.BorderStyle = BorderStyle.FixedSingle;
                                LinkLabel linkLabel = new LinkLabel ();
                                linkLabel.Location = new System.Drawing.Point (2, 2);
                                linkLabel.Text = "Geometry";
                                geoPanel.Controls.Add (linkLabel);
                                cont = geoPanel;
                                break;
                            }
                        case AimFieldType.SysBool:
                            {
                                //CheckBox chb = new CheckBox ();
                                //chb.Width = chb.Height;
                                //chb.CheckedChanged +=new EventHandler(events_CheckBox_CheckedChanged);

                                //cont = chb;

                                ComboBox cb = new ComboBox ();
                                cb.Width = 100;
                                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                                cb.Items.Add ("YES");
                                cb.Items.Add ("NO");
                                cb.SelectedIndexChanged += new EventHandler (events_YesNoCombo_SelectedIndexChanged);

                                defaultWidth = false;
                                cont = cb;
                                break;
                            }
                        case AimFieldType.SysDateTime:
                            {
                                DateTimePicker dtp = new DateTimePicker ();
                                dtp.Value = DateTime.MinValue;
                                dtp.ValueChanged += new EventHandler (events_DateTimePicker_ValueChanged);

                                cont = dtp;
                                break;
                            }
                        case AimFieldType.SysDouble:
                        case AimFieldType.SysInt32:
                        case AimFieldType.SysInt64:
                        case AimFieldType.SysUInt32:
                            {
                                NumericUpDown nud = new NumericUpDown ();
                                nud.Minimum = decimal.MinValue;
                                nud.Maximum = decimal.MaxValue;
                                nud.DecimalPlaces = 6;
                                nud.ValueChanged += new EventHandler (events_NumericUpDown_ValueChanged);

                                cont = nud;
                                break;
                            }
                        case AimFieldType.SysGuid:
                        case AimFieldType.SysString:
                            {
                                TextBox tb = new TextBox ();
                                tb.TextChanged += new EventHandler (events_TextBox_TextChanged);

                                cont = tb;
                                break;
                            }
                        default:
                            {
                                Panel panel = new Panel ();
                                panel.BorderStyle = BorderStyle.FixedSingle;

                                cont = panel;
                                break;
                            }
                    }
                }

                #endregion
            }
            else if (classInfo.AimObjectType == AimObjectType.DataType)
            {
                if (classInfo.SubClassType == AimSubClassType.ValClass)
                {
                    Panel panel = new Panel ();
                    panel.Size = new System.Drawing.Size (320, 30);

                    NumericUpDown nud = new NumericUpDown ();
                    nud.Location = new System.Drawing.Point (1, 1);
                    nud.Size = new System.Drawing.Size (160, 28);
                    nud.Minimum = decimal.MinValue;
                    nud.Maximum = decimal.MaxValue;
                    nud.DecimalPlaces = 6;
                    nud.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                    nud.ValueChanged += new EventHandler (events_ValClass_NumericUpDown_ValueChanged);
                    panel.Controls.Add (nud);

                    ComboBox cb = new ComboBox ();
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb.Location = new System.Drawing.Point (164, 1);
                    cb.Size = new System.Drawing.Size (134, 28);
                    cb.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                    cb.SelectedIndexChanged += new EventHandler (events_ValClass_ComboBox_SelectedIndexChanged);
                    panel.Controls.Add (cb);

                    EnumType enumType = (EnumType) classInfo.Properties ["Uom"].TypeIndex;
                    string typeName = "Aran.Aim.Enums." + enumType;
                    Type en = typeof (AimField).Assembly.GetType (typeName);
                    Array array = Enum.GetValues (en);

                    foreach (object enumItem in array)
                        cb.Items.Add (enumItem);

                    if (cb.Items.Count > 0)
                        cb.SelectedIndex = 0;

                    cont = panel;
                }
                else if (classInfo.Index == (int) DataType.FeatureRef ||
                        classInfo.SubClassType == AimSubClassType.AbstractFeatureRef)
                {
                    LinkLabel link = new LinkLabel ();
                    link.TextAlign = ContentAlignment.MiddleLeft;
                    link.Text = "<Empty>";
                    link.Click += new EventHandler (events_FeatureRefLink_Click);

                    cont = link;
                }

            }

            return cont;
        }

        private void events_FeatureRefLink_Click (object sender, EventArgs e)
        {
            FeatureSelectorForm fsf = new FeatureSelectorForm (_propInfo);

            //fsf.DataGridColumnsFilled = FillDataGridColumnsHandler;
            fsf.DataGridRowSetted = SetDataGridRowHandler;
            fsf.GetFeatListByDepend = LoadFeatureListByDependHandler;

            if (_aimObject != null)
                fsf.SelectedFeatureRef = _aimObject as FeatureRef;

            if (fsf.ShowDialog (this) == DialogResult.OK)
            {
                if (fsf.SelectedFeatureRef != null)
                {
                    SetFeature (fsf.SelectedFeatureRef, fsf.SelectedFeature);
                    DoValueChanged ();
                }
            }
        }

        private void events_ValClass_ComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox) sender;

            IEditValClass editValClass = _aimObject as IEditValClass;
            editValClass.Uom = (int) cb.SelectedItem;

            DoValueChanged ();
        }

        private void events_ValClass_NumericUpDown_ValueChanged (object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown) sender;

            IEditValClass editValClass = _aimObject as IEditValClass;
            editValClass.Value = Convert.ToDouble (nud.Value);

            DoValueChanged ();
        }

        private void events_TextBox_TextChanged (object sender, EventArgs e)
        {
            AimFieldValueChanged (((TextBox) sender).Text);

            DoValueChanged ();
        }

        private void events_NumericUpDown_ValueChanged (object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown) sender;

            Type [] genericTypeArr = _aimObject.GetType ().GetGenericArguments ();

            if (genericTypeArr != null && genericTypeArr.Length > 0)
            {
                try
                {
                    object value = Convert.ChangeType (nud.Value, genericTypeArr [0]);
                    AimFieldValueChanged (value);
                }
                catch
                {
                    nud.Value = default (decimal);
                }
            }

            DoValueChanged ();
        }

        private void events_ComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            AimFieldValueChanged (((ComboBox) sender).SelectedItem);

            DoValueChanged ();
        }

        private void events_YesNoCombo_SelectedIndexChanged (object sender, EventArgs e)
        {
            var cb = (sender as ComboBox);
            AimFieldValueChanged (cb.SelectedIndex == 0);

            DoValueChanged ();
        }

        private void events_DateTimePicker_ValueChanged (object sender, EventArgs e)
        {
            AimFieldValueChanged (((DateTimePicker) sender).Value);

            DoValueChanged ();
        }

        private void AimFieldValueChanged (object value)
        {
            if ((_aimObject as IAimObject).AimObjectType == AimObjectType.Field)
            {
                AimField aimField = _aimObject as AimField;
                IEditAimField editAimField = aimField;

                if (aimField.FieldType == AimFieldType.SysGuid)
                {
                    try
                    {
                        editAimField.FieldValue = new Guid (value.ToString ());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show ("Guid format is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    editAimField.FieldValue = value;
                }
            }

            DoValueChanged ();
        }

        private void DoValueChanged ()
        {
            if (ValueChanged != null)
                ValueChanged (this, null);
        }
    }
}
