using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Metadata.UI;

namespace Aran.Aim.InputFormLib
{
    public partial class AimPropertyControl : UserControl
    {
        private IAimProperty _aimProperty;
        private AimPropInfo _propInfo;
        private UIPropInfo _uiPropInfo;
        private Control _editControl;
        private Color? _backColor;

        public AimPropertyControl ()
        {
            InitializeComponent ();
        }

        public void SetProperty (IAimProperty aimProperty, AimPropInfo propInfo)
        {
            _aimProperty = aimProperty;
            _propInfo = propInfo;
            _uiPropInfo = propInfo.UiPropInfo ();

            ui_propNameLabel.Text = _uiPropInfo.Caption;
            if (aimProperty == null)
                ui_propValueLabel.Text = Globals.NullFieldText;
            else
            {
                ui_propValueLabel.Text = GetPropertyText (aimProperty);
            }
        }


        private Control EditControl
        {
            get
            {
                if (_editControl == null)
                {
                    if (_aimProperty is AimField)
                    {
                        _editControl = CreateEditControl (_aimProperty as AimField);
                        _editControl.Visible = false;
                        _editControl.Location = ui_propValueLabel.Location - new Size (1, 2);
                        _editControl.Size = new Size (160, 24);

                        Controls.Add (_editControl);
                    }
                }
                return _editControl;
            }
        }
        
        private string GetPropertyText (IAimProperty aimProperty)
        {
            switch (aimProperty.PropertyType)
            {
                case AimPropertyType.AranField:
                    return GetAimFieldText ((AimField) aimProperty);
            }
            return string.Empty;
        }

        private string GetAimFieldText (AimField aimField)
        {
            IEditAimField editAimField = aimField;

#warning Implement
            /// *** Implement ***
            /// 1. Geo Fields.
            /// 2. DateTime, Double and GUID Format. (from settings)
            
            switch (aimField.FieldType)
            {
                case AimFieldType.GeoPoint:
                    return "Point";             
                case AimFieldType.GeoPolygon:
                    return "Polygon";
                case AimFieldType.GeoPolyline:
                    return "Polyline";
                case AimFieldType.SysBool:
                    return ((bool) editAimField.FieldValue ? "YES" : "NO");
                case AimFieldType.SysDateTime:
                    return ((DateTime) editAimField.FieldValue).ToString ("yyyy-MM-dd");
                case AimFieldType.SysDouble:
                    return ((double) editAimField.FieldValue).ToString ();
                case AimFieldType.SysEnum:
                    return editAimField.FieldValue.ToString ();
                case AimFieldType.SysGuid:
                    return ((Guid) editAimField.FieldValue).ToString ("B");
                case AimFieldType.SysInt32:
                case AimFieldType.SysInt64:
                case AimFieldType.SysUInt32:
                    return editAimField.FieldValue.ToString ();
                case AimFieldType.SysString:
                    return (string) editAimField.FieldValue;
            }

            return string.Empty;
        }

        private void AimPropertyControl_MouseEnter (object sender, EventArgs e)
        {
            //ui_propValueLabel.ForeColor = Color.Red;

            if (_backColor == null)
                _backColor = BackColor;

            BackColor = ControlPaint.Light (SystemColors.Highlight, 1.5F);
            
            if (_editControl == null)
                return;

            _editControl.Visible = true;
            ui_propValueLabel.Visible = false;
        }

        private void AimPropertyControl_MouseLeave (object sender, EventArgs e)
        {
            Control childControl = GetChildAtPoint (PointToClient (MousePosition));
            if (childControl != null)
                return;

            BackColor = _backColor.Value;

            ui_propValueLabel.ForeColor = SystemColors.ControlText;

            if (_editControl == null)
                return;

            _editControl.Visible = false;
            ui_propValueLabel.Visible = true;
        }

        private Control CreateEditControl (AimField aimField)
        {
            Control cont;
            IEditAimField editAimField = aimField;

            AimFieldType aimFieldType = (AimFieldType) _propInfo.TypeIndex;

            if (AimMetadata.IsEnum (_propInfo.TypeIndex))
            {
                ComboBox cb = new ComboBox ();
                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                EnumType enumType = (EnumType) _propInfo.TypeIndex;

                string typeName = "Aran.Aim.Enums." + enumType;
                Type en = typeof (AimField).Assembly.GetType (typeName);
                Array array = Enum.GetValues (en);

                foreach (object enumItem in array)
                    cb.Items.Add (enumItem);

                if (editAimField != null)
                    cb.SelectedItem = editAimField.FieldValue;

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

                            cont = geoPanel;
                            break;
                        }
                    case AimFieldType.SysBool:
                        {
                            ComboBox cb = new ComboBox ();
                            cb.DropDownStyle = ComboBoxStyle.DropDownList;
                            cb.Items.Add ("YES");
                            cb.Items.Add ("NO");

                            if ( editAimField != null)
                                cb.SelectedIndex = ((bool) editAimField.FieldValue ? 0 : 1);

                            cont = cb;
                            break;
                        }
                    case AimFieldType.SysDateTime:
                        {
                            NDateTimePicker ndtp = new NDateTimePicker ();

                            if (editAimField != null)
                                ndtp.Value = (DateTime) editAimField.FieldValue;
                            
                            cont = ndtp;
                            break;
                        }
                    case AimFieldType.SysDouble:
                    case AimFieldType.SysInt32:
                    case AimFieldType.SysInt64:
                    case AimFieldType.SysUInt32:
                        {
                            NumericUpDown nud = new NumericUpDown ();

                            if (editAimField != null)
                                nud.Value = Convert.ToDecimal (editAimField.FieldValue);

                            cont = nud;
                            break;
                        }
                    case AimFieldType.SysString:
                        {
                            TextBox tb = new TextBox ();

                            if (editAimField != null)
                                tb.Text = editAimField.FieldValue.ToString ();

                            cont = tb;
                            break;
                        }
                    default:
                        {
                            Panel panel = new Panel ();
                            panel.BackColor = Color.Gray;

                            cont = panel;
                            break;
                        }
                }
            }

            return cont;
        }
    }
}
