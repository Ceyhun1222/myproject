using System;
using System.Windows.Forms;
using System.IO;
using Aran.Geometries;

namespace Aran.PANDA.RNAV.DMECoverage
{
	public partial class AddDMEForm : Form
	{
		private double _resX;
		private double _resY;
		private double _resZ;

		public AddDMEForm()
		{
			InitializeComponent();
			label8.Text = GlobalVars.unitConverter.HeightUnit;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			// Get a handle to a copy of this form's system (window) menu
			IntPtr hSysMenu = HelperUnit.GetSystemMenu(this.Handle, false);
			// Add a separator
			HelperUnit.AppendMenu(hSysMenu, GlobalVars.MF_SEPARATOR, 0, string.Empty);
			// Add the About menu item
			HelperUnit.AppendMenu(hSysMenu, GlobalVars.MF_STRING, GlobalVars.SYSMENU_ABOUT_ID, "&About…");
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			if ((m.Msg == GlobalVars.WM_SYSCOMMAND) && ((int)m.WParam == GlobalVars.SYSMENU_ABOUT_ID))
			{
				AboutForm about = new AboutForm();
				about.ShowDialog(this);
				about = null;
			}
		}

		public DialogResult ShowForm(ref Point point)
		{
			int Deg, Min;
			double Sec;

			if (point.X >= 0.0)
				cbX.SelectedIndex = 0;
			else
				cbX.SelectedIndex = 1;

			HelperUnit.DD2DMS(Math.Abs(point.X), out Deg, out Min, out Sec, 360, 3);

			Edit4.Text = Deg.ToString();
			Edit5.Text = Min.ToString();
			Edit6.Text = Convert.ToString(Sec);


			if (point.Y >= 0.0)
				cbY.SelectedIndex = 0;
			else
				cbY.SelectedIndex = 1;

			HelperUnit.DD2DMS(Math.Abs(point.Y), out Deg, out Min, out Sec, 0, 3);

			Edit1.Text = Deg.ToString();
			Edit2.Text = Min.ToString();
			Edit3.Text = Convert.ToString(Sec);

			textBox1.Text = GlobalVars.unitConverter.HeightToDisplayUnits(point.Z).ToString();

			DialogResult result = this.ShowDialog();

			if (result == System.Windows.Forms.DialogResult.OK)
			{
				point.X = _resX;
				point.Y = _resY;
				point.Z = _resZ;
			}

			return result;
		}

		public void btnOKClick(System.Object Sender, System.EventArgs _e1)
		{
			try
			{
				_resZ = GlobalVars.unitConverter.HeightToInternalUnits(double.Parse(textBox1.Text));

				int signLat = 1;
				if (cbY.SelectedIndex == 1)
					signLat = -1;

				int signLon = 1;
				if (cbX.SelectedIndex == 1)
					signLon = -1;

				double xDeg = double.Parse(Edit4.Text);
				double xMin = double.Parse(Edit5.Text);
				double xSec = double.Parse(Edit6.Text);

				double yDeg = double.Parse(Edit1.Text);
				double yMin = double.Parse(Edit2.Text);
				double ySec = double.Parse(Edit3.Text);

				_resX = HelperUnit.DMS2DD(xDeg, xMin, xSec, signLon, 360);
				_resY = HelperUnit.DMS2DD(yDeg, yMin, ySec, signLat, 0);
			}
			catch
			{
				MessageBox.Show("Invalid coordinate value.", Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}
		}
	}
}
