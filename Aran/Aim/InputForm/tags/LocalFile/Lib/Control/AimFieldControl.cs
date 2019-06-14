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
    public partial class AimFieldControl : UserControl, IAimFieldControl
    {
        private AimField _aimField;
        private AimPropInfo _propInfo;
        private UIPropInfo _uiPropInfo;

        public AimFieldControl ()
        {
            InitializeComponent ();
        }

        public void SetAimField (AimField aimField, AimPropInfo propInfo)
        {
            _aimField = aimField;
            _propInfo = propInfo;
            _uiPropInfo = propInfo.UiPropInfo ();

            ui_propNameLabel.Text = _uiPropInfo.Caption;

            bool changeWidth;
            Control editControl = CreateEditControl (out changeWidth);
            editControl.Location = new Point (0, 15);

            if (changeWidth)
            {
                editControl.Width = ui_midPanel.Width;
                editControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            }

            ui_midPanel.Controls.Add (editControl);
        }

        private Control CreateEditControl (out bool changeWidth)
        {
            Control cont;

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

                if (_aimField != null)
                    cb.SelectedItem = (_aimField as IEditAimField).FieldValue;

                cont = cb;
                changeWidth = true;
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

                            cont = geoPanel;
                            changeWidth = true;
                            break;
                        }
                    case AimFieldType.SysBool:
                        {
                            CheckBox chb = new CheckBox ();
                            chb.Width = chb.Height;

                            cont = chb;
                            changeWidth = false;
                            break;
                        }
                    case AimFieldType.SysDateTime:
                        {
                            NDateTimePicker ndtp = new NDateTimePicker ();
                            ndtp.Value = DateTime.MinValue;

                            cont = ndtp;
                            changeWidth = true;
                            break;
                        }
                    case AimFieldType.SysDouble:
                    case AimFieldType.SysInt32:
                    case AimFieldType.SysInt64:
                    case AimFieldType.SysUInt32:
                        {
                            NumericUpDown nud = new NumericUpDown ();

                            cont = nud;
                            changeWidth = true;
                            break;
                        }
                    case AimFieldType.SysString:
                        {
                            TextBox tb = new TextBox ();

                            cont = tb;
                            changeWidth = true;
                            break;
                        }
                    default:
                        {
                            Panel panel = new Panel ();
                            panel.BackColor = Color.Gray;

                            cont = panel;
                            changeWidth = true;
                            break;
                        }
                }
            }

            return cont;
        }

        public AimField AimField
        {
            get { return _aimField; }
        }
    }
}
