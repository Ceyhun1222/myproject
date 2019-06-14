using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AIP.BaseLib.Airac
{

    public partial class AiracCycleDateTime : UserControl
    {
        #region Win32

        public const int EM_SETCUEBANNER = 0x1501;
        public const int EM_SETMARGINS = 0xd3;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SendMessage(
            IntPtr hWnd, Int32 msg, IntPtr wParam, IntPtr lParam);

        #endregion

        private DateTime _value;
        private string _dateTimeFormat;
        private AiracSelectionMode _selectionMode;


        public AiracCycleDateTime()
        {
            InitializeComponent();

            ui_selButton.Cursor = Cursors.Default;
            DateTimeFormat = "yyyy - MM - dd  HH:mm";

            var rightMarginWidth = ui_airacOrCustomLabel.Width + 3;
            SendMessage(ui_maskedTB.Handle, EM_SETMARGINS, (IntPtr)1, (IntPtr)(rightMarginWidth));

            SetNextCycle();

            ui_selButton.CausesValidation = true;
            ui_maskedTB.LostFocus += MaskedTB_LostFocus;
              
        }


        public event EventHandler ValueChanged;


        public DateTime Value
        {
            get { return _value; }
            set
            {
                //_value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
                _value = new DateTime(value.Year, value.Month, value.Day, 0,0,0);
                ui_maskedTB.Text = _value.ToString(DateTimeFormat);

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }

        public String DTValue
        {
            get { return _value.ToShortDateString(); }
            set
            {
                DateTime dt = DateTime.Parse(value);
                _value = new DateTime(dt.Year, dt.Month, dt.Day, 0,0,0);
                //_value = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                ui_maskedTB.Text = _value.ToString(DateTimeFormat);

                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
        }


        public AiracSelectionMode SelectionMode
        {
            get { return _selectionMode; }
            set
            {
                _selectionMode = value;
                //ui_airacOrCustomLabel.Text = (value == AiracSelectionMode.Airac ? "AIRAC" : "Custom") + ":";
                ui_airacOrCustomLabel.Text = (value == AiracSelectionMode.Airac ? "AIRAC" : "Non-AIRAC");
                ui_maskedTB.ReadOnly = (value == AiracSelectionMode.Airac);
                ui_utcLabel.Visible = (value == AiracSelectionMode.Custom);
            }
        }

        public string DateTimeFormat
        {
            get { return _dateTimeFormat; }
            set
            {
                _dateTimeFormat = value;
                ui_maskedTB.Mask = value.
                    Replace('y', '0').
                    Replace('M', '0').
                    Replace('d', '0').
                    Replace('H', '0').
                    Replace('m', '0');
            }
        }

        public AiracDateTime AiracDateTime
        {
            get
            {
                return new AiracDateTime() { Mode = SelectionMode, Value = this.Value };
            }
            set
            {
                Value = value.Value;
                SelectionMode = value.Mode;
            }
        }

        public void SetNextCycle()
        {
            var dt = DateTime.UtcNow;
            var list = AiracCycle.AiracCycleList;
            var k = list.FirstOrDefault(ac => ac.RadCutOff.Year == dt.Year && ac.RadCutOff.Month == dt.Month);
            if (k != null) {
                int index = list.IndexOf(k);
                var ni = (index == list.Count - 1 ? index : index + 1);
                Value = list[ni].RadCutOff;
            }
        }

        
        private void SelectButton_Click(object sender, EventArgs e)
        {
            var form = new ACSelectionForm();
            form.ValueChanged += SelectionForm_ValueChanged;
            form.StartPosition = FormStartPosition.Manual;
            var scrPt = ui_selButton.PointToScreen(new Point(ui_selButton.Width - form.Width, ui_selButton.Height));
            form.Location = scrPt;
            form.Value = _value;
            form.AiracSelectionMode = SelectionMode;
            form.Show(this);
        }

        private void SelectionForm_ValueChanged(object sender, EventArgs e)
        {
            var form = sender as ACSelectionForm;
            Value = form.Value;
            SelectionMode = form.AiracSelectionMode;
        }

        private void MaskedTB_Validating(object sender, CancelEventArgs e)
        {
            DateTime dt;

            if (!DateTime.TryParseExact(
                this.ui_maskedTB.Text,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dt)) {

                ui_maskedTB.Text = Value.ToString(DateTimeFormat);
            }
            else {
                Value = dt;
            }
        }

        private void MaskedTB_LostFocus(object sender, EventArgs e)
        {
            MaskedTB_Validating(sender, null);
        }
    }

}
