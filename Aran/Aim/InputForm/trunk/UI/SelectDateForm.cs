using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Aim.InputForm
{
	public partial class SelectDateForm : Form
	{
        private DateTime _minDate;
        private DateTime _selDate;
		public SelectDateForm ()
		{
			InitializeComponent ();

            _minDate = DateTime.MinValue;

            ui_errorLabel.Text = string.Empty;
            ui_selDateLabel.Text = string.Empty;
		}

        private void Form_Load(object sender, EventArgs e)
        {
            ui_airacCycleControl.SetNextCycle();
            _selDate = MakeLestNight(ui_airacCycleControl.Value);
        }

		public void SetMinDateTime (DateTime dateTime)
		{
            _minDate = dateTime;
		}

		public DateTime GetDateTime ()
		{
            return _selDate;
		}


		private void OK_Click (object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

        private void ShowError(string value)
        {
            ui_errorLabel.Text = value;
            ui_okButton.Enabled = value.Length == 0;
        }

        private void AiracCycleControl_ValueChanged(object sender, EventArgs e)
        {
            SetSelDate(MakeLestNight(ui_airacCycleControl.Value));
            
            var s = string.Empty;
            if (_minDate > _selDate)
                s = "Decomission Date must be later than Begin of Valid!";
            
            ShowError(s);
        }

        private void SetSelDate(DateTime dateTime)
        {
            _selDate = dateTime;
            ui_selDateLabel.Text = "Decomission Date will be " + dateTime.ToString("yyyy-MM-dd HH:mm");
        }

        private DateTime MakeLestNight(DateTime dt)
        {
            dt = dt.AddDays(-1);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 0);
        }
	}
}
