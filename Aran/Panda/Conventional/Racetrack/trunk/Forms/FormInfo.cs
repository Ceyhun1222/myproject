using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack.Forms
{
	public partial class FormInfo : Form
	{
		private FormInfo ( )
		{
			InitializeComponent ( );
		}

		public FormInfo ( string caption, ProcedureTypeConv procType )
			: this ( )
		{
			//toolTipInfo.SetToolTip(txtBxR,"");
			Text += " - " + caption;
			if ( procType == ProcedureTypeConv.Vordme )
			{
				lblFarDist.Visible = false;
				txtBxWidthToleranceArea.Visible = false;

				//lblLimRadial.Visible = false;
				//txtBxLimRadial.Visible = false;

				lblDistToHomVOR.Visible = false;
				txtBxHomingVorDist.Visible = false;

				lblDistToIntersectVOR.Visible = false;
				txtBxIntersectVorDist.Visible = false;

				ClientSize = new Size ( ClientSize.Width, txtBxMagVar.Location.Y + txtBxMagVar.Height + 40 );
			}
			else if ( procType == ProcedureTypeConv.VorNdb )
			{
				lblMagVar.Visible = false;
				txtBxMagVar.Visible = false;

				lblFarDist.Visible = false;
				txtBxWidthToleranceArea.Visible = false;

				lblLimRadial.Visible = false;
				txtBxLimRadial.Visible = false;

				lblDistToHomVOR.Visible = false;
				txtBxHomingVorDist.Visible = false;

				lblDistToIntersectVOR.Visible = false;
				txtBxIntersectVorDist.Visible = false;

				ClientSize = new Size ( ClientSize.Width, txtBxTas.Location.Y + txtBxTas.Height + 40 );
			}
			else if ( procType == ProcedureTypeConv.Vorvor )
			{
				lblLimRadial.Visible = false;
				txtBxLimRadial.Visible = false;

				lblFarDist.Visible = true;
				txtBxWidthToleranceArea.Visible = true;
				var upLocationLbl = lblLimRadial.Location;
				var upLocationTxtBx = txtBxLimRadial.Location;
				var downLocationLbl = lblFarDist.Location;
				var downLocationTxtBx = txtBxWidthToleranceArea.Location;
				lblFarDist.Location = upLocationLbl;
				txtBxWidthToleranceArea.Location = upLocationTxtBx;
				upLocationLbl = downLocationLbl;
				upLocationTxtBx = downLocationTxtBx;

				lblDistToHomVOR.Visible = true;
				txtBxHomingVorDist.Visible = true;
				downLocationLbl = lblDistToHomVOR.Location;
				downLocationTxtBx = txtBxHomingVorDist.Location;
				lblDistToHomVOR.Location = upLocationLbl;
				txtBxHomingVorDist.Location = upLocationTxtBx;
				upLocationLbl = downLocationLbl;
				upLocationTxtBx = downLocationTxtBx;


				lblDistToIntersectVOR.Visible = true;
				txtBxIntersectVorDist.Visible = true;
				lblDistToIntersectVOR.Location = upLocationLbl;
				txtBxIntersectVorDist.Location = upLocationTxtBx;

				ClientSize = new Size ( ClientSize.Width, txtBxIntersectVorDist.Location.Y + txtBxIntersectVorDist.Height + 40 );
			}
			MinimumSize = Size;
			MaximumSize = Size;
		}

		internal void SetElevNavaid ( string elev )
		{
			txtBxElevNavaid.Text = elev + " " + GlobalParams.UnitConverter.HeightUnit;
		}

		internal void SetNavType ( string navType )
		{
			txtBxTypeNavaid.Text = !string.IsNullOrEmpty(navType) ? navType : "-----";
		}

		internal void SetLegLength ( string legLength )
		{
			txtBxLegLength.Text = legLength + " " + GlobalParams.UnitConverter.DistanceUnit;
		}

		internal void SetTypeDsgPnt ( string typeDsgPnt )
		{
			//if ( typeDsgPnt != "" )
			//    txtBxTypeDsgPnt.Text = typeDsgPnt;
			//else
			//    txtBxTypeDsgPnt.Text = "-----";
		}

		private void FormInfo_FormClosing ( object sender, FormClosingEventArgs e )
		{
			Hide ( );
			e.Cancel = true;
		}

		internal void SetTas ( double tas )
		{
			txtBxTas.Text = tas + " " + GlobalParams.UnitConverter.SpeedUnit;
		}

		internal void SetLimitingRadial ( double angle )
		{
			txtBxLimRadial.Text = double.IsNaN ( angle ) ? "" : angle.ToString (CultureInfo.InvariantCulture);
		}

		internal void SetWidthToleranceArea ( double value )
		{
			txtBxWidthToleranceArea.Text = value + " " + GlobalParams.UnitConverter.DistanceUnit;
		}

		internal void SetMagVar ( double value )
		{
			txtBxMagVar.Text = value.ToString (CultureInfo.InvariantCulture);
		}

		internal void SetHomingVorDist ( double p )
		{
			txtBxHomingVorDist.Text = p + " " + GlobalParams.UnitConverter.DistanceUnit;
		}

		internal void SetIntersectingVorDist ( double p )
		{
			txtBxIntersectVorDist.Text = p + " " + GlobalParams.UnitConverter.DistanceUnit;
		}

		public void SetSpiralParameters()
		{

			txtBxR.Text = Math.Round(GlobalParams.SpiralParameterR, 3) + @"°/s";
			txtBxE45.Text =
				GlobalParams.UnitConverter.DistanceToDisplayUnits(GlobalParams.SpiralParameterE45, eRoundMode.NEAREST).ToString() +
				" " + GlobalParams.UnitConverter.DistanceUnit;
		}
	}
}