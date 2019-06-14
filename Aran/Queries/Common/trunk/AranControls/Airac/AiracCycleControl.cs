using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using Aran.AranEnvironment;

namespace Aran.Controls.Airac
{
    public partial class AiracCycleControl : UserControl
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


        public AiracCycleControl()
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
				_value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
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
                ui_airacOrCustomLabel.Text = (value == AiracSelectionMode.Airac ? "AIRAC" : "Custom") + ":";
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
            var k = list.Where(ac => ac.RadCutOff.Year == dt.Year && ac.RadCutOff.Month == dt.Month).FirstOrDefault();
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
                new CultureInfo("en-IE"),
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
