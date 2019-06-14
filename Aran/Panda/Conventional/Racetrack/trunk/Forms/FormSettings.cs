using System;
using System.Globalization;
using System.Windows.Forms;

namespace Aran.PANDA.Conventional.Racetrack.Forms
{
	public partial class FormSettings : Form
	{
		private readonly MainController _controller;

		private FormSettings ( )
		{
			InitializeComponent ( );
		}

		public FormSettings ( bool drawNominalTraject, bool drawToleranceArea, bool drawShablon, bool drawSectors, bool drawBuffers, MainController controller, bool isTestVersion = false )
			: this ( )
		{
			// TODO: Complete member initialization
			chckBxNominalTrack.Checked = drawNominalTraject;
			chckBxToleranceArea.Checked = drawToleranceArea;
			chckBxShablon.Checked = drawShablon;
			chckBxSector.Checked = drawSectors;
			chckBxBuffers.Checked = drawBuffers;
			chckBxTest.Checked = isTestVersion;
			_controller = controller;
			lblLowLimUnit.Text = GlobalParams.UnitConverter.HeightUnit;
		}

		private void btnOk_Click ( object sender, EventArgs e )
		{
			DialogResult = DialogResult.OK;
			Hide ( );
		}

		private void chckBxNominalTrack_CheckedChanged ( object sender, EventArgs e )
		{
			_controller?.SetNominalTrajectVisibility ( chckBxNominalTrack.Checked );
		}

		private void chckBxToleranceArea_CheckedChanged ( object sender, EventArgs e )
		{
			_controller?.SetToleranceAreaVisibility ( chckBxToleranceArea.Checked );
		}

		private void chckBxSector_CheckedChanged ( object sender, EventArgs e )
		{

		}

		private void chckBxShablon_CheckedChanged ( object sender, EventArgs e )
		{
			_controller?.SetTemplateVisibility ( chckBxShablon.Checked );
		}

		private void chckBxBuffers_CheckedChanged ( object sender, EventArgs e )
		{
			_controller?.SetBufferVisibility ( chckBxBuffers.Checked );
		}

		public bool IsVisibleNominalTrajectory => chckBxNominalTrack.Checked;

		public bool IsVisibleToleranceArea => chckBxToleranceArea.Checked;

		public bool IsVisibleShablon => chckBxShablon.Checked;

		public bool IsVisibleSectors => chckBxSector.Checked;

		public bool IsVisibleBuffer => chckBxBuffers.Checked;

		private void FormSettings_FormClosing ( object sender, FormClosingEventArgs e )
		{
			Hide ( );
			e.Cancel = true;
		}

		private void chckBxTest_CheckedChanged ( object sender, EventArgs e )
		{
			GlobalParams.IsTestVersion = chckBxTest.Checked;
		}

		private void nmrcUpDownFixTolDist_ValueChanged ( object sender, EventArgs e )
		{
			_controller.SetFixToleranceDist ( nmrcUpDownFixTolDist.Value );
		}

		internal void SetFixToleranceDist (bool visible, double value = double.NaN)
		{
			if ( visible )
			{

				int length = GlobalParams.Settings.DistancePrecision.ToString (CultureInfo.InvariantCulture).Length;
				int round = 0;
				if ( length > 2 )
					round = length - 2;
				nmrcUpDownFixTolDist.DecimalPlaces = round;

				lblFixToleranceDist.Visible = true;
				nmrcUpDownFixTolDist.Visible = true;
				nmrcUpDownFixTolDist.Value = ( decimal ) value;
				lblFixTolDistUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
				lblFixTolDistUnit.Visible = true;
			}
			else
			{
				lblFixToleranceDist.Visible = false;
				nmrcUpDownFixTolDist.Visible = false;
				lblFixTolDistUnit.Visible = false;
			}
		}

		private void nmrcUpDwnLowLimHldngPattern_ValueChanged (object sender, EventArgs e)
		{
			_controller.SetLowerLimitHoldingPattern (nmrcUpDwnLowLimHldngPattern.Value);
		}

		internal void SetMinLowLimitHoldingPattern (double assessedAltitude)
		{
			nmrcUpDwnLowLimHldngPattern.Minimum = (decimal) assessedAltitude;
		}

        private void chckBxSaveSecndPnt_CheckedChanged(object sender, EventArgs e)
        {
            _controller.SaveSecondaryPoint = chckBxSaveSecndPnt.Checked;
        }
    }
}