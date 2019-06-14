using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Queries.Viewer;
using Aran.Aim.DataTypes;
using Aran.Aim.ValidationInfo;

namespace Aran.Queries.Common
{
    public partial class PropControl : UserControl
    {
        private string _propertyName;
        private PropControlTag _propertyTag;
        private Control _curControl;
        private int _controlLeft;
        private int _controlWidth;
        private TextBox _nullDateTimeControl;
        private bool _valueIsNull;
        private bool _readOnly;
        private bool _isNullable;

        public PropControl ()
        {
            InitializeComponent ();

            Height = 40;
            _curControl = stringTb;
            _controlLeft = _curControl.Left;
            _controlWidth = _curControl.Width;
            _valueIsNull = true;
            _isNullable = true;
            //_readOnly = false;
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public PropControlTag PropertyTag
        {
            get
            { 
                return _propertyTag;
            }
            set
            {
                _propertyTag = value;

                if (_propertyTag == null)
                    return;

                bool hideClearLabel = false;
                _propertyName = _propertyTag.PropInfo.AixmName;

                AimPropInfo aimPropInfo = _propertyTag.PropInfo;
                IAimProperty aimPropValue = _propertyTag.PropValue;

                switch (aimPropInfo.TypeIndex)
                {
                    case (int) AimFieldType.SysString:
                        {
                            _curControl = stringTb;

                            if (aimPropValue != null)
                                stringTb.Text = (aimPropValue as AimField<string>).Value;

                            stringTb.Validated += new EventHandler (stringTb_Validated);
                        }
                        break;
                    case (int) AimFieldType.SysBool:
                        {
                            _curControl = boolChB;

                            if (aimPropValue != null)
                                boolChB.Checked = (aimPropValue as AimField<bool>).Value;

                            boolChB.CheckStateChanged += new EventHandler (boolChB_CheckStateChanged);
                        }
                        break;
                    case (int) AimFieldType.SysDouble:
                        {
                            _curControl = doubleNud;

                            if (aimPropValue != null)
                            {
                                doubleNud.Value = Convert.ToDecimal ((aimPropValue as IEditAimField).FieldValue);
                                doubleNud.IsNull = false;
                            }

                            doubleNud.ValueChanged += new EventHandler (doubleNud_ValueChanged);
                        }
                        break;
                    case (int) AimFieldType.SysInt32:
                    case (int) AimFieldType.SysInt64:
                    case (int) AimFieldType.SysUInt32:
                        {
                            _curControl = intTb;

                            if (aimPropValue != null)
                            {
                                intTb.IsNull = false;
                                intTb.Value = Convert.ToDecimal ((aimPropValue as IEditAimField).FieldValue);
                            }

                            intTb.ValueChanged += new EventHandler (intTb_ValueChanged);
                        }
                        break;
                    case (int) AimFieldType.SysDateTime:
                        {
                            _curControl = dateTimePicker;

                            if (aimPropValue != null)
                                dateTimePicker.Value = (aimPropValue as AimField<DateTime>).Value;

                            dateTimePicker.ValueChanged += new EventHandler (dateTimePicker_ValueChanged);
                        }
                        break;
                    case (int) AimFieldType.GeoPoint:
                        {
                            _curControl = coordPanel;
                            hideClearLabel = true;
                            _propertyName = "coordinate";

                            if (aimPropValue != null)
                                SetCoordinateText ((aimPropValue as AimField<Geometries.Point>).Value);
                        }
                        break;
                    case (int) DataType.TextNote:
                        {
                            _curControl = notePanel;

                            TextNote nextNode = (TextNote) aimPropValue;


                            noteCB.Items.Clear ();
                            Array array = Enum.GetValues (typeof (Aran.Aim.Enums.language));
                            foreach (object enumItem in array)
                            {
                                noteCB.Items.Add (enumItem);
                            }

                            if (nextNode != null)
                            {
                                noteTB.Text = nextNode.Value;
                                noteCB.SelectedItem = nextNode.Lang;
                            }

                            noteTB.Validated += new EventHandler (noteTB_Validated);
                            noteCB.SelectedIndexChanged += new EventHandler (noteCB_SelectedIndexChanged);

                            lineGrb.Top += 40;
                            Height += 50;
                        }
                        break;
                    default:
                        {
                            if (AimMetadata.IsEnum (aimPropInfo.TypeIndex))
                            {
                                _curControl = enumCb;
                                EnumType enumType = (EnumType) aimPropInfo.TypeIndex;

                                string typeName = "Aran.Aim.Enums." + enumType;

                                Type en = typeof (AimField).Assembly.GetType (typeName);
                                Array array = Enum.GetValues (en);

                                foreach (object enumItem in array)
                                    enumCb.Items.Add (enumItem);

                                if (aimPropValue != null)
                                {
                                    object itemValue = (aimPropValue as IEditAimField).FieldValue;

                                    if (itemValue is Int32)
                                    {
                                    }
                                    else
                                    {
                                        enumCb.SelectedItem = itemValue;
                                    }
                                }

                                enumCb.SelectedIndexChanged += new EventHandler (enumCb_SelectedIndexChanged);
                            }
                            else if (AimMetadata.IsValClass (aimPropInfo.TypeIndex))
                            {
                                _curControl = uomPanel;

                                AimPropInfo [] valClassPropIndoArr = AimMetadata.GetAimPropInfos (aimPropInfo.TypeIndex);
                                AimPropInfo uomPropInfo = valClassPropIndoArr [valClassPropIndoArr.Length - 1];
                                EnumType uomEnumType = (EnumType) uomPropInfo.TypeIndex;

                                string typeName = "Aran.Aim.Enums." + uomEnumType;
                                Type en = typeof (AimField).Assembly.GetType (typeName);
                                Array array = Enum.GetValues (en);

                                foreach (object enumItem in array)
                                    uomCb.Items.Add (enumItem);

                                if (aimPropValue != null)
                                {
                                    IEditValClass editValClass = aimPropValue as IEditValClass;

                                    for (int i = 0; i < uomCb.Items.Count; i++)
                                    {
                                        if ((int) uomCb.Items [i] == editValClass.Uom)
                                        {
                                            uomCb.SelectedIndex = i;
                                            break;
                                        }
                                    }
                                    uomValueNud.Value = Convert.ToDecimal (editValClass.Value);
                                    uomValueNud.IsNull = false;
                                }

                                uomValueNud.ValueChanged += new EventHandler (uomValueNud_ValueChanged);
                                uomCb.Validated += new EventHandler (uomCb_Validated);
                            }
                            else
                            {
                                throw new Exception ("Property not supported");
                            }
                        }
                        break;
                }

                _curControl.Top = 8;
                _curControl.Visible = true;
                _curControl.Left = _controlLeft;
                _curControl.Width = _controlWidth;

                string propName = MakeSentence (_propertyName);

                propNameLabel.Text = propName + ":";
                toolTip1.SetToolTip (propNameLabel, _propertyName);

                if (_isNullable && aimPropValue != null && !hideClearLabel)
                    clearButton.Visible = true;

                PropControl_SizeChanged (this, null);
            }
        }

        public bool ShowSeperateLine
        {
            get { return lineGrb.Visible; }
            set { lineGrb.Visible = value; }
        }

        public int ControlLeft
        {
            get { return _curControl.Left; }
            set 
            {
                _curControl.Left = value;
                _controlLeft = value;
                propNameLabel.Width = _controlLeft - propNameLabel.Left - 5;
            }
        }

        public int ControlWidth
        {
            get { return _curControl.Width; }
            set
            {
                _curControl.Width = value;
                _controlWidth = value;
            }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;

                if (_propertyTag == null)
                    return;

                AimPropInfo aimPropInfo = _propertyTag.PropInfo;

                if (aimPropInfo == null)
                    return;

                switch (aimPropInfo.TypeIndex)
                {
                    case (int) AimFieldType.SysString:
                        stringTb.ReadOnly = value;
                        break;
                    case (int) AimFieldType.SysBool:
                        boolChB.Enabled = !value;
                        break;
                    case (int) AimFieldType.SysDouble:
                        doubleNud.Enabled = !value;
                        break;
                    case (int) AimFieldType.SysInt32:
                    case (int) AimFieldType.SysInt64:
                    case (int) AimFieldType.SysUInt32:
                        intTb.Enabled = !value;
                        break;
                    case (int) AimFieldType.SysDateTime:
                        {
                            dateTimePicker.Enabled = !value;
                            if (_nullDateTimeControl != null)
                                _nullDateTimeControl.BackColor = (_readOnly ? SystemColors.Control : SystemColors.Window);
                        }
                        break;
                    case (int) AimFieldType.GeoPoint:
                        coordPanel.Enabled = !value;
                        coordXTB.Validated += new EventHandler (coordXYTB_Validated);
                        coordYTB.Validated += new EventHandler (coordXYTB_Validated);
                        break;
                    default:
                        {
                            if (AimMetadata.IsEnum (aimPropInfo.TypeIndex))
                            {
                                enumCb.Enabled = !value;
                            }
                            else if (AimMetadata.IsValClass (aimPropInfo.TypeIndex))
                            {
                                uomValueNud.Enabled = !value;
                                uomCb.Enabled = !value;
                            }
                        }
                        break;
                }

                if (_readOnly)
                    clearButton.Visible = false;
            }
        }

        public bool IsNullable
        {
            get { return _isNullable; }
            set
            {
                _isNullable = value;
                if (!_isNullable)
                {
                    clearButton.Visible = _isNullable;
                }
            }
        }


        public static bool IsSupported (AimPropInfo aimPropInfo)
        {
            switch (aimPropInfo.TypeIndex)
            {
                case (int) AimFieldType.SysString:
                case (int) AimFieldType.SysBool:
                case (int) AimFieldType.SysDouble:
                case (int) AimFieldType.SysInt32:
                case (int) AimFieldType.SysInt64:
                case (int) AimFieldType.SysUInt32:
                case (int) AimFieldType.SysDateTime:
                case (int) AimFieldType.GeoPoint:
                case (int) DataType.TextNote:
                    return true;
                default:
                    {
                        return (AimMetadata.IsEnum (aimPropInfo.TypeIndex) ||
                            AimMetadata.IsValClass (aimPropInfo.TypeIndex));
                    }
            }
        }

        public static string MakeSentence (string propName)
        {
            if (propName.Length > 0 && char.IsLower (propName [0]))
            {
                propName = char.ToUpper (propName [0]) + propName.Substring (1);
            }

            for (int i = 1; i < propName.Length - 1; i++)
            {
                if (char.IsUpper (propName [i]) && (char.IsLower (propName [i - 1]) || char.IsLower (propName [i + 1])))
                {
                    propName = propName.Insert (i, " ");
                    i++;
                }
            }

            return propName;
        }


        private void SetCoordinateText (Geometries.Point point)
        {
			coordXTB.Text = point.X.ToString("#.########");
			coordYTB.Text = point.Y.ToString("#.########");
        }

        private void SetValue (IAimProperty value)
        {
            if (_propertyTag == null)
                return;

            _valueIsNull = (value == null);

            if (!_valueIsNull)
            {
                if (value.PropertyType == AimPropertyType.AranField)
                {
                    if (_propertyTag.AimObject.AimObjectType == AimObjectType.Feature)
                    {
                        CommonValidationErrorType errorType;
                        if (!CommonValidation.CheckValue (AimMetadata.GetAimTypeIndex (_propertyTag.AimObject),
                            _propertyTag.PropInfo.Index, ((IEditAimField)value).FieldValue, out errorType))
                        {
                            SetValidationError (errorType);
                            return;
                        }
                    }
                }
            }

            ui_warningPicBox.Visible = false;

            _propertyTag.AimObject.SetValue (_propertyTag.PropInfo.Index, value);
            clearButton.Visible = _isNullable && !_valueIsNull;

            switch (_propertyTag.PropInfo.TypeIndex)
            {
                case (int) AimFieldType.SysDateTime:
                    if (_valueIsNull)
                        _nullDateTimeControl.Visible = true;
                    break;
                case (int) AimFieldType.SysDouble:
                    doubleNud.IsNull = _valueIsNull;
                    break;
                case (int) AimFieldType.SysUInt32:
                case (int) AimFieldType.SysInt32:
                case (int) AimFieldType.SysInt64:
                    intTb.IsNull = _valueIsNull;
                    break;
                default:
                    {
                        if (AimMetadata.IsValClass (_propertyTag.PropInfo.TypeIndex))
                        {
                            uomValueNud.IsNull = _valueIsNull;
                        }
                    }
                    break;
            }
        }

        private void SetValidationError (CommonValidationErrorType errorType)
        {
            ui_warningPicBox.Visible = true;
            string errorText = errorType + " condition not satisfied.";
            toolTip1.SetToolTip (ui_warningPicBox, errorText);
        }

        private void stringTb_Validated (object sender, EventArgs e)
        {
            if (ReadOnly)
                return;

            ui_warningPicBox.Visible = false;

            TextBox tb = (TextBox) sender;
            IAimProperty aimPropVal = null;

            if (!string.IsNullOrEmpty (tb.Text))
            {
                AimField<string> propVal = new AimField<string> (tb.Text);
                aimPropVal = propVal;
            }
            
            SetValue (aimPropVal);
        }

        private void boolChB_CheckStateChanged (object sender, EventArgs e)
        {
            IAimProperty aimPropVal = null;

            if (boolChB.CheckState != CheckState.Indeterminate)
            {
                AimField<bool> propVal = new AimField<bool> (boolChB.Checked);
                aimPropVal = propVal;
            }

            SetValue (aimPropVal);
        }

        private void doubleNud_ValueChanged (object sender, EventArgs e)
        {
            IAimProperty aimPropVal = null;

            AimField<double> propVal = new AimField<double> (Convert.ToDouble (doubleNud.Value));
            aimPropVal = propVal;

            SetValue (aimPropVal);
        }

        private void intTb_ValueChanged (object sender, EventArgs e)
        {
            IAimProperty aimPropVal = null;

            try
            {
                double val = Convert.ToDouble (intTb.Value);

                switch (_propertyTag.PropInfo.TypeIndex)
                {
                    case (int) AimFieldType.SysInt32:
                        AimField<Int32> int32Val = new AimField<Int32> (Convert.ToInt32 (val));
                        aimPropVal = int32Val;
                        break;
                    case (int) AimFieldType.SysInt64:
                        AimField<Int64> int64Val = new AimField<Int64> (Convert.ToInt64 (val));
                        aimPropVal = int64Val;
                        break;
                    case (int) AimFieldType.SysUInt32:
                        AimField<UInt32> uint32Val = new AimField<UInt32> (Convert.ToUInt32 (val));
                        aimPropVal = uint32Val;
                        break;
                }
            }
            catch
            {
            }

            SetValue (aimPropVal);
        }

        private void enumCb_SelectedIndexChanged (object sender, EventArgs e)
        {
            IAimProperty aimPropVal = null;

            if (enumCb.SelectedItem != null)
            {
                IEditAimField editAimField = AimObjectFactory.CreateEnumType ((EnumType) _propertyTag.PropInfo.TypeIndex);
                editAimField.FieldValue = enumCb.SelectedItem;
                aimPropVal = editAimField as IAimProperty;
            }
            
            SetValue (aimPropVal);
        }

        private void uomValueNud_ValueChanged (object sender, EventArgs e)
        {
            //if (uomCb.Focused)
            //    return;
            
            UomValidated ();
        }

        private void uomCb_Validated (object sender, EventArgs e)
        {
            //if (uomValueNud.Focused)
            //    return;

            UomValidated ();
        }

        private void UomValidated ()
        {
            if (uomCb.SelectedItem == null)
                return;

            AimObject aimObject = AimObjectFactory.Create (_propertyTag.PropInfo.TypeIndex);
            IEditValClass valClass = aimObject as IEditValClass;

            valClass.Value = Convert.ToDouble (uomValueNud.Value);
            //uomValueNud.IsNull = false;
            valClass.Uom = Convert.ToInt32 (uomCb.SelectedItem);

            SetValue (aimObject as IAimProperty);
        }

        private void dateTimePicker_ValueChanged (object sender, EventArgs e)
        {
            AimField<DateTime> aimField = new AimField<DateTime> (dateTimePicker.Value);
            SetValue (aimField);
        }

        private void noteTB_Validated (object sender, EventArgs e)
        {
            NoteChanged ();
        }

        private void noteCB_SelectedIndexChanged (object sender, EventArgs e)
        {
            NoteChanged ();
        }

        private void NoteChanged ()
        {
            if (noteCB.SelectedItem == null)
                return;

            TextNote textNote = new TextNote ();
            textNote.Value = noteTB.Text;
            textNote.Lang = (Aim.Enums.language) noteCB.SelectedItem;
            SetValue (textNote);
        }

        private void coordXYTB_Validated (object sender, EventArgs e)
        {
            double x;
            double y;

            if (double.TryParse (coordXTB.Text, out x) &&
                double.TryParse (coordYTB.Text, out y))
            {
                CoordinateChanged (x, y);
            }
        }

        private void CoordinateChanged (double x, double y)
        {
            if (double.IsNaN (x) || double.IsNaN (y))
                return;

            IAimProperty aimProp = _propertyTag.AimObject.GetValue (_propertyTag.PropInfo.Index);
            Aran.Aim.AimField<Aran.Geometries.Point> ptField = aimProp as Aran.Aim.AimField<Aran.Geometries.Point>;
            ptField.Value.SetCoords (x, y);
        }

        private void clearValue ()
        {
            AimPropInfo aimPropInfo = _propertyTag.PropInfo;

            switch (aimPropInfo.TypeIndex)
            {
                case (int) AimFieldType.SysString:
                    stringTb.Text = "";
                    break;
                case (int) AimFieldType.SysBool:
                    boolChB.CheckState = CheckState.Indeterminate;
                    break;
                case (int) AimFieldType.SysDouble:
                    doubleNud.Value = 0;
                    break;
                case (int) AimFieldType.SysInt32:
                case (int) AimFieldType.SysInt64:
                case (int) AimFieldType.SysUInt32:
                    intTb.Value = 0;
                    break;
                case (int) AimFieldType.SysDateTime:
                    dateTimePicker.Value = DateTime.Now;
                    break;
                default:
                    {
                        if (AimMetadata.IsEnum (aimPropInfo.TypeIndex))
                        {
                            enumCb.SelectedItem = null;
                        }
                        else if (AimMetadata.IsValClass (aimPropInfo.TypeIndex))
                        {
                            uomValueNud.Value = 0;
                            uomCb.SelectedItem = null;
                        }
                        else
                        {
                            throw new Exception ("Property not supported");
                        }
                    }
                    break;
            }

            SetValue (null);
        }

        private void PropControl_Load (object sender, EventArgs e)
        {
            if (_propertyTag == null)
                return;

            clearButton.Left = _curControl.Right + 5;
            _valueIsNull = (_propertyTag.PropValue == null);

            if (_propertyTag.PropInfo.TypeIndex == (int) AimFieldType.SysDateTime)
            {
                TextBox tb = new TextBox ();

                tb.Visible = _valueIsNull;
                tb.BorderStyle = BorderStyle.None;
                tb.ReadOnly = true;
                tb.BackColor = (_readOnly ? SystemColors.Control : SystemColors.Window);
                tb.Location = new Point (2, 2);
                tb.Size = new Size (_curControl.Width - 26, _curControl.Height - 4);
                tb.Text = "";//<null>";
                tb.Font = new Font (tb.Font, FontStyle.Italic);

                _curControl.Controls.Add (tb);
                _nullDateTimeControl = tb;
            }
        }

        private void dateTimePicker_Enter (object sender, EventArgs e)
        {
            _nullDateTimeControl.Visible = false;
        }

        private void dateTimePicker_Leave (object sender, EventArgs e)
        {
            if (_valueIsNull)
                _nullDateTimeControl.Visible = true;
        }

        private void clearButton_Click (object sender, EventArgs e)
        {
            clearValue ();
        }

        private void PropControl_SizeChanged (object sender, EventArgs e)
        {
            clearButton.Top = (Height - clearButton.Height) / 2;
            if (_curControl != null)
            {
                _curControl.Top = (Height - _curControl.Height) / 2;
                propNameLabel.Top = _curControl.Top;
            }
        }
    }
}
