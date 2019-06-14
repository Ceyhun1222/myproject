using System;
using System.Windows.Forms;
using Aran.PANDA.Common;
using Aran.AranEnvironment;
using Aran.Geometries;
using System.Collections.Generic;
using System.Globalization;
using Aran.PANDA.Conventional.Racetrack.Properties;

namespace Aran.PANDA.Conventional.Racetrack
{
	public partial class FormMain : Form
	{
        public FormMain ( )
		{
			InitializeComponent ( );

			_hasToBeCalculate = true;
			_pointPickerClicked = false;
			_valueChangedFromOtherComponent = false;
			_calledByPointPickerControl = true;
			_calculatedIntersectingRadial = false;

			int length = GlobalParams.Settings.AnglePrecision.ToString (CultureInfo.InvariantCulture).Length;
			int round = 0;
			if ( length > 2 )
				round = length - 2;
			nmrcUpDownRadial.DecimalPlaces = round;

			length = GlobalParams.Settings.DistancePrecision.ToString (CultureInfo.InvariantCulture).Length;
			round = 0;
			if ( length > 2 )
				round = length - 2;
			nmrcUpDownDistance.DecimalPlaces = round;
			nmrcUpDownLimDist.DecimalPlaces = round;

			length = GlobalParams.Settings.SpeedPrecision.ToString (CultureInfo.InvariantCulture).Length;
			round = 0;
			if ( length > 2 )
				round = length - 2;
			nmrcUpDownIAS.DecimalPlaces = round;
		}

		#region Event handlers of Main Controller

		internal void OnIasIntervalChanged ( object sender, SpeedInterval iasInterval )
		{
            if (double.IsNaN(iasInterval.Min) && double.IsNaN(iasInterval.Max))
            {
                ChangeSpeedToMach(true);
                if (_controller.AltitudeAboveNavaid > _controller.Speed.Height10350)
                {
                    txtBxAllowedSpeedMach.Width = txtBxAllowableSpeed.Width;
                    _controller.Speed.Ias = _controller.Speed.CalculateMach() * 0.83;
                }
                else 
                {
                    txtBxAllowedSpeedMach.Text = _controller.GetSpeedIntervalString();
                    toolTipMach.SetToolTip(txtBxAllowedSpeedMach, "0.8M = " + _controller.CalculateMach(0.8));
                    txtBxIas.Text = _controller.CalculateMinimumSpeed();

                    txtBxAllowedSpeedMach.Width = 90;
                    lblIASUnit.Text = "";
                    lblPermSpeedUnit.Text = "";
                }
            }
            else
            {
                txtBxAllowedSpeedMach.Width = txtBxAllowableSpeed.Width;
                ChangeSpeedToMach(false);
                nmrcUpDownIAS.Minimum = (decimal)iasInterval.Min;
                nmrcUpDownIAS.Maximum = (decimal)iasInterval.Max;
                nmrcUpDownIAS.Value = nmrcUpDownIAS.Maximum;
                txtBxAllowableSpeed.Text = iasInterval.Min.ToString(CultureInfo.InvariantCulture) + "/" + iasInterval.Max.ToString(CultureInfo.InvariantCulture);
            }
            //SetCategoryList((List<string>)sender);
		}

		internal void OnTimeChanged ( object sender, CommonEventArg argTime )
		{
			_hasToBeCalculate = false;
			nmrcUpDownTime.Value = ( decimal ) argTime.Value;
			_hasToBeCalculate = true;
		}

		internal void OnLimitDistIntervalChanged ( object sender, SpeedInterval interval )
		{
			if ( double.IsNaN ( interval.Min ) && double.IsNaN ( interval.Max) )
			{
				//txtBxAllowableLimDist.Text = "";
				//btnConstruct.Enabled = false;
				EnableConstructBtn ( false, Resources.Define_Pnt_Far );
			}
			else
			{
				_hasToBeCalculate = false;
				nmrcUpDownLimDist.Minimum = ( decimal ) interval.Min;
				nmrcUpDownLimDist.Maximum = ( decimal ) interval.Max;
				_hasToBeCalculate = true;

				txtBxAllowableLimDist.Text = nmrcUpDownLimDist.Minimum + "/" + nmrcUpDownLimDist.Maximum;

				if ( sender != null )
				{
					double time = ( double ) sender;
					if ( _controller.SelectedModul == ModulType.Holding && _controller.AltitudeAboveNavaid < _controller.Altitude4Holding 
						&& time == 1.5)
						return;					
					nmrcUpDownTime.Minimum = ( decimal ) time;
					_controller.SetTime ( time, _calledByPointPickerControl );
				}
				else
				{
					nmrcUpDownTime.Minimum = 1;
					_controller.SetTime ( ( double ) nmrcUpDownTime.Value, _calledByPointPickerControl );
				}

			}
		}

		internal void OnLimDistChanged ( object sender, CommonEventArg arg )
		{
			_hasToBeCalculate = false;
			nmrcUpDownLimDist.Value = ( decimal ) arg.Value;
			_hasToBeCalculate = true;
		}

		public void OnRE_AngleChanged ( object sender, CommonEventArg arg )
		{
			txtBxAngRE.Text = double.IsNaN ( arg.Value ) ? "" : arg.Value.ToString (CultureInfo.InvariantCulture);
		}

		internal void OnDirectionChanged ( object sender, DirectionEventArg argDir )
		{
			_valueChangedFromOtherComponent = true;
			nmrcUpDownRadial.Value = ( decimal ) ARANMath.Modulus ( argDir.DirectionForGui, 360 );
            _controller.DirectionInDeg = nmrcUpDownRadial.Value;
			_valueChangedFromOtherComponent = false;
		}

		internal void OnIntersectionDirectionIntervalChanged ( object sender, SpeedInterval interval )
		{
			_intersectDircInterval = interval;
			if ( radBtnViaDsgPnt.Checked )
				return;

			SetFixToleranceComps ( sender == null );
			txtBxAllowableIntersRadials.Text = interval.Min + "/" + interval.Max;
			if ( interval.Min < interval.Max )
			{
				_calculatedIntersectingRadial = true;
				nmrcUpDownIntersectignVorRadial.Minimum = ( decimal ) interval.Min;
				nmrcUpDownIntersectignVorRadial.Maximum = ( decimal ) interval.Max;
				_calculatedIntersectingRadial = false;

				nmrcUpDwnIntersectignVorRadial_ValueChanged ( null, null );
			}
			else
			{
				SetDefaults4IntersectignVorRadial ( );

				double fixToleranceDist;
				double value = _controller.CalculateIntersectRadial ( out fixToleranceDist );
					if(double.IsNaN(value))
						return;
				
				_calculatedIntersectingRadial = true;
				nmrcUpDownIntersectignVorRadial.Value = ( decimal ) value;
				txtBxFixToleranceDist.Text = fixToleranceDist.ToString (CultureInfo.InvariantCulture);
				_calculatedIntersectingRadial = false;
			}
		}

		private void SetFixToleranceComps ( bool enable )
		{
			if ( !enable)
			{
				txtBxFixToleranceDist.ForeColor = System.Drawing.Color.Red;
				txtBxAllowableIntersRadials.ForeColor = radBtnViaVorRadials.Checked ? System.Drawing.Color.Red : System.Drawing.SystemColors.WindowText;
				btnConstruct.Enabled = false;
			}
			else
			{
				if ( radBtnViaVorRadials.Checked )
					txtBxAllowableIntersRadials.ForeColor = System.Drawing.SystemColors.WindowText;
				txtBxFixToleranceDist.ForeColor = System.Drawing.SystemColors.WindowText;
				if ( !btnConstruct.Enabled )
					btnConstruct.Enabled = true;
			}
		}

		internal void OnIntersectionDirectionChanged ( object sender, DirectionEventArg arg )
		{
			double value = ARANMath.Modulus ( Math.Round ( arg.DirectionForGui ), 360 );
			if ( value > ( double ) nmrcUpDownIntersectignVorRadial.Minimum && value < ( double ) nmrcUpDownIntersectignVorRadial.Maximum )
			{
				_calculatedIntersectingRadial = true;
				nmrcUpDownIntersectignVorRadial.Value = ( decimal ) value;
				_calculatedIntersectingRadial = false;
			}
		}

		internal void OnDistanceChanged ( object sender, DistanceEventArg argDist )
		{
			_valueChangedFromOtherComponent = true;
			nmrcUpDownDistance.Value = ( decimal ) argDist.DistanceForGui;
			_valueChangedFromOtherComponent = false;
		}

		internal void OnDesignatedPointChanged ( object sender, DsgPntEventArg argDsgPnt )
		{
			if ( argDsgPnt.DsgPntPrj != null )
			{
				ChangeLierTxtBxsAppearence ( false, cmbBxNavaids.Items.Count == 0 );

				if ( !_calledByPointPickerControl )
					SetPointPickerCoords ( argDsgPnt.DsgPntPrj );
			}
			else
			{
				ChangeLierTxtBxsAppearence ( true, cmbBxNavaids.Items.Count == 0 );
			}
		}

		internal void OnMouseClickedOnMap ( object sender, MapMouseEventArg e )
		{
			Point pntPrj = new Point ( e.X, e.Y );

			bool tmp = _calledByPointPickerControl;
			_calledByPointPickerControl = false;
			Point pntGeo = GlobalParams.SpatialRefOperation.ToGeo<Point> ( pntPrj );
			_controller.SetDesignatedPoint ( pntPrj );

			pointPicker1.Latitude = pntGeo.Y;
			pointPicker1.Longitude = pntGeo.X;
			_calledByPointPickerControl = tmp;
		}

		internal void OnDeacitvatedPointPickerTool ( )
		{
			pointPicker_ByClickChanged ( null, null );
		}

		internal void OnAppliedValueChanged ( bool isEqual )
		{
			if ( _controller.ProcType == ProcedureTypeConv.Vorvor )
			{
				if ( radBtnViaVorRadials.Checked )
				{
					if ( txtBxAllowableIntersRadials.ForeColor != System.Drawing.Color.Red )
						btnConstruct.Enabled = !isEqual;
				}
				else
				{
					if ( txtBxFixToleranceDist.ForeColor != System.Drawing.Color.Red )
						btnConstruct.Enabled = !isEqual;
				}
			}
			else
				btnConstruct.Enabled = !isEqual;
			btnReport.Enabled = isEqual;
			if ( btnSave.Enabled )
				btnSave.Enabled = isEqual;
		}

		#endregion

		#region Event handlers of Form

		private void frmMain_Load ( object sender, EventArgs e )
		{
			cmbBxMoc.DataSource = _controller.MocList;
			//cmbBxMoc.SelectedIndex = 0;
			_controller.GetNavaidList ( );
			if ( _controller.ProcType == ProcedureTypeConv.Vorvor )
				_controller.SetDirection ( ( double ) nmrcUpDownRadial.Value );
			_hasToBeCalculate = true;


			RestrictTimeInterval();

			#region Designs GUI components


			//pointPicker1.Parent = splitCntPntSet.Panel2;
			//pointPicker1.Location = new System.Drawing.Point ( 5, 0 );
			//pointPicker1.Visible = false;

			pnlRadial_Dist.Parent = splitCntPntSet.Panel2;
			pnlRadial_Dist.Location = new System.Drawing.Point ( 0, pnlSelectDsgPnt.Height );
			//pnlRadial_Dist.Location = new System.Drawing.Point ( 5, 0 );
			pnlRadial_Dist.Visible = true;

			pnlSelectDsgPnt.Parent = splitCntPntSet.Panel2;
			pnlSelectDsgPnt.Location = new System.Drawing.Point ( 0, 0 );

			ApplyInterfaceLanguage ( );

			tabCntrlSetDsgPnt.Visible = false;

			bool hasNavaid = ( cmbBxNavaids.SelectedItem != null );
			//grpBxSpeed.Width -= 50;
			grpBxSetDsgPnt.Width = grpBxSpeed.Width;
			chckBxProtectSector1.Text = "Protect Sector 1";

			if ( _controller.ProcType == ProcedureTypeConv.Vordme )
			{
                chckBxOmniDirectionalEntry.Visible = false;
                chckBxIntersectRadialEntry.Visible = false;

				grpBxEntryDir.Visible = true;
				grpBxSlctFacility.Height = nmrcUpDownAltitude.Location.Y + 30;
				grpBxOutboundLegParams.Parent = this;
				grpBxOutboundLegParams.Location = new System.Drawing.Point ( grpBxSlctFacility.Location.X, grpBxSlctFacility.Location.Y + grpBxSlctFacility.Height + 10 );
			    grpBxOutboundLegParams.Height = grpBxOutboundLegParams.Height - pnlTime.Height;

				lblNavaids.Text = "Navaids";
				grpBxSetDsgPnt.Visible = hasNavaid;
				chckBxIntersectRadialEntry.Visible = false;
				chckBxProtectSector1.Enabled = true;

				nmrcUpDownLimDist.Visible = true;
				SetOverheadRadialVisibility ( false );
				SetIntersectVorCntrlsVisibility ( false );

				lblRadial.Location = new System.Drawing.Point ( lblDist.Location.X, lblDist.Location.X + 25 );
				nmrcUpDownRadial.Location = new System.Drawing.Point ( nmrcUpDownDistance.Location.X, nmrcUpDownDistance.Location.Y + 30 );
				lierTxtBxRadial.Location = nmrcUpDownRadial.Location;
				lblRadialUnit.Location = new System.Drawing.Point ( lblDistanceUnit.Location.X, lblDistanceUnit.Location.Y + 30 );

				chckBxProtectSector2.Visible = true;
				grpBxPrtctInbound.Text = "Protection of entries to FIX on the inbound leg";
				grpBxPrtctInbound.Width = grpBxOutboundLegParams.Width + 3;
				grpBxPrtctInbound.Location = new System.Drawing.Point ( grpBxOutboundLegParams.Location.X, grpBxOutboundLegParams.Location.Y + grpBxOutboundLegParams.Height + 7 );
                
                Size = new System.Drawing.Size(Width - 10, grpBxOutbndLegProtection.Location.Y + grpBxOutbndLegProtection.Height + 80);
			    radBtnViaTime.Checked = true;
			}
			else if ( _controller.ProcType == ProcedureTypeConv.VorNdb )
			{
				grpBxTurnDir.Height += 8;
				radBtnLeftDir.Location = new System.Drawing.Point ( radBtnLeftDir.Location.X, radBtnLeftDir.Location.Y + 3 );
				radBtnRightDir.Location = new System.Drawing.Point ( radBtnRightDir.Location.X, radBtnRightDir.Location.Y + 3 );

				chckBxIntersectRadialEntry.Visible = false;
				chckBxOmniDirectionalEntry.Enabled = true;
				chckBxOmniDirectionalEntry.BringToFront ( );
				chckBxProtectSector1.Text = "Sector 1 is not\n permitted";
				chckBxProtectSector1.Visible = true;
				chckBxProtectSector1.Enabled = false;
				chckBxProtectSector1.BringToFront();
				chckBxProtectSector1.Location = new System.Drawing.Point(chckBxOmniDirectionalEntry.Location.X + chckBxOmniDirectionalEntry.Width + 5,
					chckBxOmniDirectionalEntry.Location.Y);

				lblMoc.Location = new System.Drawing.Point ( lblMoc.Location.X, grpBxTurnDir.Location.Y + grpBxTurnDir.Height + 18 );
				cmbBxMoc.Location = new System.Drawing.Point ( cmbBxMoc.Location.X, lblMoc.Location.Y - 3 );
				lblMocUnit.Location = new System.Drawing.Point ( lblMocUnit.Location.X, lblMoc.Location.Y );

				grpBxSlctFacility.Location = new System.Drawing.Point ( grpBxTurnDir.Location.X, cmbBxMoc.Location.Y + cmbBxMoc.Height + 5 );
				lblRadial.Location = new System.Drawing.Point ( lblAltitude.Location.X, lblAltitude.Location.X + 25 );
				nmrcUpDownRadial.Location = new System.Drawing.Point ( nmrcUpDownAltitude.Location.X, nmrcUpDownAltitude.Location.Y + 30 );
				lierTxtBxRadial.Location = nmrcUpDownRadial.Location;
				lblRadialUnit.Location = new System.Drawing.Point ( lblAltitudeUnit.Location.X, lblAltitude.Location.Y + 30 );

				grpBxSlctFacility.Height = nmrcUpDownRadial.Location.Y + nmrcUpDownRadial.Height + 10;

				grpBxEntryDir.Visible = false;

				lblNavaids.Text = "Navaids";
				grpBxSetDsgPnt.Visible = false;

				if ( hasNavaid )
					nmrcUpDownOverheadRadial_ValueChanged ( nmrcUpDownOverheadRadial, null );

				nmrcUpDownLimDist.Visible = false;
				SetOverheadRadialVisibility ( true );
				lblOverheadRadial.Text = lblRadial.Text;
				SetIntersectVorCntrlsVisibility ( false );

				chckBxProtectSector2.Visible = false;

			    pnlTime.Parent = this;
			    pnlTime.Dock = DockStyle.None;
                pnlTime.Location = new System.Drawing.Point(grpBxSpeed.Location.X, grpBxSpeed.Location.Y + grpBxSpeed.Height + 3);

    //            grpBxOutboundLegParams.Parent = this;
				//grpBxOutboundLegParams.Location = new System.Drawing.Point ( grpBxSpeed.Location.X, grpBxSpeed.Location.Y + grpBxSpeed.Height + 3 );
				//grpBxOutboundLegParams.Height = nmrcUpDownTime.Location.Y + nmrcUpDownTime.Height + 5;
				//grpBxOutboundLegParams.Width = grpBxSpeed.Width;


				grpBxPrtctInbound.Text = "Protection of entries";
				//grpBxPrtctInbound.Width = grpBxSpeed.Width;
				grpBxPrtctInbound.Height = chckBxOmniDirectionalEntry.Location.Y + chckBxOmniDirectionalEntry.Height + 5;
                grpBxPrtctInbound.Location = new System.Drawing.Point(grpBxSlctFacility.Location.X, grpBxSlctFacility.Location.Y + grpBxSlctFacility.Height + 3);

				Size = new System.Drawing.Size ( Width - 10, grpBxPrtctInbound.Location.Y + grpBxPrtctInbound.Height + 80 );
			}
			else
			{
				pnlRadial_Dist.Visible = false;
				radBtnSelectDsgnPnt.Checked = true;

				grpBxTurnDir.Height -= 5;
				radBtnLeftDir.Location = new System.Drawing.Point ( radBtnLeftDir.Location.X, radBtnLeftDir.Location.Y - 5 );
				radBtnRightDir.Location = new System.Drawing.Point ( radBtnRightDir.Location.X, radBtnRightDir.Location.Y - 5 );

			    #region Time components

			    pnlTime.Dock = DockStyle.None;
			    pnlTime.Parent = this;
			    pnlTime.Visible = true;
			    pnlTime.Location = new System.Drawing.Point(grpBxEntryDir.Location.X + 12, grpBxEntryDir.Location.Y + grpBxEntryDir.Height + 8);

			    //            lblTime.Parent = this;
			    //nmrcUpDownTime.Parent = this;
			    //lblTimeUnit.Parent = this;
			    //lblTime.Location = new System.Drawing.Point ( lblAltitude.Location.X, nmrcUpDownAltitude.Location.Y + nmrcUpDownAltitude.Height + 8 );
			    //nmrcUpDownTime.Location = new System.Drawing.Point ( nmrcUpDownAltitude.Location.X, lblTime.Location.Y );
			    //lblTimeUnit.Location = new System.Drawing.Point ( lblAltitudeUnit.Location.X, lblTime.Location.Y );

			    #endregion

                #region Altitude components

                lblAltitude.Parent = this;
				nmrcUpDownAltitude.Parent = this;
				lblAltitudeUnit.Parent = this;
				lblAltitude.Location = new System.Drawing.Point ( lblAltitude.Location.X + 5, pnlTime.Location.Y + pnlTime.Height + 8 );
				nmrcUpDownAltitude.Location = new System.Drawing.Point ( nmrcUpDownAltitude.Location.X + 5, lblAltitude.Location.Y );
				lblAltitudeUnit.Location = new System.Drawing.Point ( lblAltitudeUnit.Location.X + 5, lblAltitude.Location.Y );

                #endregion

			    #region Moc components

			    lblMoc.Location = new System.Drawing.Point(lblAltitude.Location.X, lblAltitude.Location.Y + lblAltitude.Height + 20);
			    cmbBxMoc.Location = new System.Drawing.Point(nmrcUpDownAltitude.Location.X, nmrcUpDownAltitude.Location.Y + nmrcUpDownAltitude.Height + 8);
			    lblMocUnit.Location = new System.Drawing.Point(lblAltitudeUnit.Location.X, lblAltitudeUnit.Location.Y + lblAltitudeUnit.Height + 20);

			    #endregion
				
			    


				#region Homing VOR components

				grpBxSlctFacility.Parent = grpBxEntryPointDef;
				grpBxSlctFacility.Location = new System.Drawing.Point ( grpBxEntryPntSelection.Location.X, grpBxEntryPntSelection.Location.Y + grpBxEntryPntSelection.Height + 5 );
				grpBxSlctFacility.Text = "Homing VOR";
				lblNavaids.Text = "VOR";
				lblRadial.Parent = grpBxSlctFacility;
				nmrcUpDownRadial.Parent = grpBxSlctFacility;
				lblRadialUnit.Parent = grpBxSlctFacility;

				lblRadial.Location = new System.Drawing.Point ( lblNavaids.Location.X, cmbBxNavaids.Location.Y + cmbBxNavaids.Height + 10 );
				nmrcUpDownRadial.Location = new System.Drawing.Point ( cmbBxNavaids.Location.X, lblRadial.Location.Y - 2 );
				lblRadialUnit.Location = new System.Drawing.Point ( nmrcUpDownRadial.Location.X + nmrcUpDownRadial.Width + 10, lblRadial.Location.Y - 2 );

				//lblDist.Parent = grpBxSlctFacility;
				//txtBxHomingDist.Parent = grpBxSlctFacility;
				//lblDistanceUnit.Parent = grpBxSlctFacility;
				//lblDist.Location = new System.Drawing.Point ( lblRadial.Location.X, nmrcUpDownRadial.Location.Y + nmrcUpDownRadial.Height + 10 );
				//txtBxHomingDist.Location = new System.Drawing.Point ( nmrcUpDownRadial.Location.X, lblDist.Location.Y - 2 );
				//lblDistanceUnit.Location = new System.Drawing.Point ( lblRadialUnit.Location.X, lblDist.Location.Y - 2 );

				grpBxSlctFacility.Height = nmrcUpDownRadial.Location.Y + nmrcUpDownRadial.Height + 10;


				#endregion

				#region Intersecting VOR

				grpBxIntersectingVOR.Parent = grpBxEntryPointDef;
				grpBxIntersectingVOR.Visible = true;
				grpBxIntersectingVOR.Location = new System.Drawing.Point ( grpBxSlctFacility.Location.X, grpBxSlctFacility.Location.Y + grpBxSlctFacility.Height + 9 );
				//grpBxIntersectingVOR.Size = grpBxSlctFacility.Size;
				grpBxIntersectingVOR.Width = grpBxSlctFacility.Width;


				lblIntersectVor.Parent = grpBxIntersectingVOR;
				cmbBxIntersectingVor.Parent = grpBxIntersectingVOR;

				lblIntersectVORRadial.Parent = grpBxIntersectingVOR;
				nmrcUpDownIntersectignVorRadial.Parent = grpBxIntersectingVOR;
				lblIntersectingVorRadialUnit.Parent = grpBxIntersectingVOR;

				lblIntersectVor.Text = lblNavaids.Text;
				lblIntersectVor.Location = new System.Drawing.Point ( lblNavaids.Location.X, lblNavaids.Location.Y );
				cmbBxIntersectingVor.Location = new System.Drawing.Point ( cmbBxNavaids.Location.X, cmbBxNavaids.Location.Y );

				lblIntersectVORRadial.Text = lblRadial.Text;
				lblIntersectVORRadial.Location = new System.Drawing.Point ( lblRadial.Location.X, lblRadial.Location.Y );
				nmrcUpDownIntersectignVorRadial.Location = new System.Drawing.Point ( nmrcUpDownRadial.Location.X, nmrcUpDownRadial.Location.Y );
				lblIntersectingVorRadialUnit.Location = new System.Drawing.Point ( lblRadialUnit.Location.X, lblRadialUnit.Location.Y );

				lblAllowableIntersRadials.Parent = grpBxIntersectingVOR;
				txtBxAllowableIntersRadials.Parent = grpBxIntersectingVOR;
				lblAllowableIntersRadialsUnit.Parent = grpBxIntersectingVOR;
				lblAllowableIntersRadials.Location = new System.Drawing.Point ( lblIntersectVORRadial.Location.X, nmrcUpDownIntersectignVorRadial.Location.Y + nmrcUpDownIntersectignVorRadial.Height + 5 );
				txtBxAllowableIntersRadials.Location = new System.Drawing.Point ( nmrcUpDownIntersectignVorRadial.Location.X, nmrcUpDownIntersectignVorRadial.Location.Y + nmrcUpDownIntersectignVorRadial.Height + 7 );
				lblAllowableIntersRadialsUnit.Location = new System.Drawing.Point ( txtBxAllowableIntersRadials.Location.X + txtBxAllowableIntersRadials.Width + 5, txtBxAllowableIntersRadials.Location.Y );

				//lblIntersectDist.Parent = grpBxIntersectingVOR;
				//txtBxIntersectDist.Parent = grpBxIntersectingVOR;
				//lblIntersectDistUnit.Parent = grpBxIntersectingVOR;
				//lblIntersectDist.Location = new System.Drawing.Point ( lblAllowableIntersRadials.Location.X, txtBxAllowableIntersRadials.Location.Y + txtBxAllowableIntersRadials.Height + 10 );
				//txtBxIntersectDist.Location = new System.Drawing.Point ( txtBxAllowableIntersRadials.Location.X, lblIntersectDist.Location.Y - 2 );
				//lblIntersectDistUnit.Location = new System.Drawing.Point ( txtBxAllowableIntersRadials.Location.X, lblIntersectDist.Location.Y - 2 );

				grpBxIntersectingVOR.Height = txtBxAllowableIntersRadials.Location.Y + txtBxAllowableIntersRadials.Height + 10;
				#endregion

				#region Set Designated Point components

				pnlSelectDsgPnt.Parent = grpBxEntryPointDef;
				pnlSelectDsgPnt.Width = grpBxSpeed.Width - 10;
				pnlSelectDsgPnt.Location = new System.Drawing.Point ( grpBxSlctFacility.Location.X + grpBxSlctFacility.Width + 5, grpBxSlctFacility.Location.Y );
				lblRadiusDsgntdPnt.Text += "\n(around Homing VOR)";
				lblRadiusDsgntdPnt.Location = new System.Drawing.Point ( lblRadiusDsgntdPnt.Location.X, lblRadiusDsgntdPnt.Location.Y - 10 );


				//grpBxSetDsgPnt.Parent = grpBxEntryPointDef;
				//grpBxSetDsgPnt.Width = grpBxSpeed.Width - 10;
				//grpBxSetDsgPnt.Location = new System.Drawing.Point ( grpBxSlctFacility.Location.X + grpBxSlctFacility.Width + 5, grpBxSlctFacility.Location.Y );
	
				grpBxSetDsgPnt.Visible = false;
				lblAllowableRadius4DsgPntUnit.Visible = false;
				lblAllowableRadius4DsgPnt.Visible = false;
				txtBxAllowableRadius4DsgPnt.Visible = false;

				//grpBxSetDsgPnt.Height = splitCntPntSet.Height - pnlRadial_Dist.Height + 20;
				//lblRadiusDsgntdPnt.Text += "\n(around Homing VOR)";
				//lblRadiusDsgntdPnt.Location = new System.Drawing.Point ( lblRadiusDsgntdPnt.Location.X, lblRadiusDsgntdPnt.Location.Y - 10 );

				lblDsgntdPnt.Location = lblAllowableRadius4DsgPnt.Location;
				cmbBxDsgntdPnts.Location = txtBxAllowableRadius4DsgPnt.Location;
				pnlSelectDsgPnt.Height = cmbBxDsgntdPnts.Location.Y + cmbBxDsgntdPnts.Height + 5;
				
				#endregion

				#region Fix Tolerance Distance 
				lblFixToleranceDist.Parent =grpBxEntryPointDef;
				txtBxFixToleranceDist.Parent =grpBxEntryPointDef;
				lblFixToleranceDistUnit.Parent = grpBxEntryPointDef;
				lblFixToleranceDist.Location = new System.Drawing.Point ( pnlSelectDsgPnt.Location.X + 10, pnlSelectDsgPnt.Location.Y + pnlSelectDsgPnt.Height + 5 );
				txtBxFixToleranceDist.Location = new System.Drawing.Point ( pnlSelectDsgPnt.Location.X + cmbBxDsgntdPnts.Location.X, pnlSelectDsgPnt.Location.Y + pnlSelectDsgPnt.Height + 5 );
				lblFixToleranceDistUnit.Location = new System.Drawing.Point ( txtBxFixToleranceDist.Location.X + txtBxFixToleranceDist.Width + 10, txtBxFixToleranceDist.Location.Y + 2 );
				#endregion

				grpBxEntryPointDef.Visible = true;
				grpBxEntryPointDef.Location = new System.Drawing.Point ( grpBxEntryPointDef.Location.X, cmbBxMoc.Location.Y + cmbBxMoc.Height + 15 );
				grpBxEntryPointDef.Height = grpBxSlctFacility.Height + grpBxIntersectingVOR.Height + grpBxEntryPntSelection.Height + 40;

				#region Protection 
				grpBxPrtctInbound.Parent = grpBxEntryPointDef;
				grpBxPrtctInbound.Width = grpBxSpeed.Width - 25;
				grpBxPrtctInbound.Location = new System.Drawing.Point ( pnlSelectDsgPnt.Location.X, grpBxIntersectingVOR.Location.Y + 15);
				//grpBxPrtctInbound.Width = grpBxEntryPointDef.Width;

				chckBxOmniDirectionalEntry.Visible = false;
				chckBxProtectSector1.Visible = false;
				chckBxProtectSector2.Visible = false;

				chckBxRecipDirEntSecondary.Parent = grpBxPrtctInbound;
				chckBxRecipDirEntSecondary.Location = new System.Drawing.Point ( chckBxProtectSector1.Location.X, chckBxProtectSector1.Location.Y + 5 );

				chckBxIntersectRadialEntry.Visible = true;
				chckBxIntersectRadialEntry.Location = new System.Drawing.Point ( chckBxRecipDirEntSecondary.Location.X, chckBxRecipDirEntSecondary.Location.Y + chckBxRecipDirEntSecondary.Height + 5);
				grpBxPrtctInbound.Height = chckBxIntersectRadialEntry.Location.Y + chckBxIntersectRadialEntry.Height + 5;
				grpBxOutbndLegProtection.Visible = false;

				#endregion

				SetOverheadRadialVisibility ( false );
				SetIntersectVorCntrlsVisibility ( true );

				//btnConstruct.Enabled = ( hasNavaid && ( cmbBxIntersectingVor.SelectedItem != null ) );
				Size = new System.Drawing.Size ( Width - 10, grpBxEntryPointDef.Location.Y + grpBxEntryPointDef.Height + 80 );
			}

			//MaximumSize = Size;
			//MinimumSize = Size;
			#endregion
		}

	    private void RestrictTimeInterval()
	    {
	        var tmp = nmrcUpDownTime.Maximum;
	        if (_controller.SelectedModul == ModulType.Holding)
	        {
	            if (_controller.ProcType == ProcedureTypeConv.Vordme && pnlDistance.Visible)
	            {

	                MaximizeTimeInterval();
	            }
	            else
	            {
	                toolTipTime.SetToolTip(nmrcUpDownTime, Resources.Time_Tooltip_Holding);
	                if (_controller.AltitudeAboveNavaid < _controller.Altitude4Holding)
	                    nmrcUpDownTime.Maximum = 1;
	                else
	                    nmrcUpDownTime.Maximum = (decimal)(1.5);
	            }
            }
	        else
	        {
	            MaximizeTimeInterval();
	        }

	        if (tmp != nmrcUpDownTime.Maximum)
	            _controller.SetMaximumTime((double) nmrcUpDownTime.Maximum);
	    }
	    private void MaximizeTimeInterval()
	    {
	        toolTipTime.SetToolTip(nmrcUpDownTime, Resources.Time_Tooltip_Racetrack);
	        nmrcUpDownTime.Maximum = 3;
	    }

        private void frmMain_Closing ( object sender, FormClosingEventArgs e )
		{
			if ( _pointPickerClicked )
			{
				GlobalParams.AranEnvironment.AranUI.SetPanTool ( );
				pointPicker1.ByClick = false;
			}
			_controller.MainFormClosing ( );
		}

		private void radBtnSelectDsgnPnt_CheckedChanged ( object sender, EventArgs e )
		{
			EnableComponontsVisible ( radBtnSelectDsgnPnt.Checked );
			if ( radBtnCreateDsgnPnt.Checked )
			{
				_controller.SetPointChoice ( PointChoice.Create );
				//pnlRadial_Dist.Location = new System.Drawing.Point ( 0, pointPicker1.Height + 5 );
				pnlRadial_Dist.Location = new System.Drawing.Point ( 5, 0 );
			}
			else
			{
				_controller.SetPointChoice ( PointChoice.Select );
				if ( pointPicker1.ByClick )
					pointPicker_ByClickChanged ( null, null );
				pnlRadial_Dist.Location = new System.Drawing.Point ( 0, pnlSelectDsgPnt.Height );
			}
			//nmrcUpDownDistance.Enabled = radBtnCreateDsgnPnt.Checked;
			//nmrcUpDownRadial.Enabled = radBtnCreateDsgnPnt.Checked;
			SetBtnConstructEnabled ( );
		}

		private void btnApply_Click ( object sender, EventArgs e )
		{
            txtBxAngRE.Text = "";
			_holdingDefined = true;
            Cursor = Cursors.WaitCursor;
            _controller.ConstructBasicArea ( );
			btnConstruct.Enabled = false;
			if ( !_holdingDefined )
				return;

			if ( _controller.ProcType == ProcedureTypeConv.Vordme )
			{
				_controller.ConstructProtectionSector1 ( !chckBxProtectSector1.Checked );
				_controller.ConstructProtectionSector2 ( !chckBxProtectSector2.Checked );
				_controller.ConstructReciprocalEntryArea ( !chckBxRecipDirEntSecondary.Checked );
			}
			else if ( _controller.ProcType == ProcedureTypeConv.VorNdb )
			{
				_controller.ConstructOmnidirectionalEntry ( !chckBxOmniDirectionalEntry.Checked );
				_controller.ConstructProtectionSector1(!chckBxProtectSector1.Checked);
			}
			else
			{
				_controller.ConstructReciprocalEntryArea ( !chckBxRecipDirEntSecondary.Checked );
				_controller.ConstructIntersectingRadialEntry ( !chckBxIntersectRadialEntry.Checked );
			}
			btnReport.Enabled = true;
			btnSave.Enabled = false;
			_controller.CreateAreas();
			btnSave.Enabled = _controller.CreateReport();
			//if (_controller.FrmReport != null && _controller.FrmReport.Visible)
			//	_controller.FrmReport.Close();			
			Cursor = Cursors.Default;			
		}

		private void btnInfo_Click ( object sender, EventArgs e )
		{
			_controller.InfoBtnClicked ( );
		}

		private void radBtnViaVorRadials_CheckedChanged ( object sender, EventArgs e )
		{
			pnlSelectDsgPnt.Enabled = ( !radBtnViaVorRadials.Checked );
			nmrcUpDownRadial.Enabled = radBtnViaVorRadials.Checked;
			nmrcUpDownDistance.Enabled = radBtnViaVorRadials.Checked;
			nmrcUpDownIntersectignVorRadial.Enabled = radBtnViaVorRadials.Checked;
			cmbBxNavaids.Enabled = radBtnViaVorRadials.Checked;
			cmbBxIntersectingVor.Enabled = radBtnViaVorRadials.Checked;
			txtBxAllowableIntersRadials.Enabled = radBtnViaVorRadials.Checked;

			if ( radBtnViaDsgPnt.Checked )
			{
				_intersectDircInterval.Min = ( double ) nmrcUpDownIntersectignVorRadial.Minimum;
				_intersectDircInterval.Max = ( double ) nmrcUpDownIntersectignVorRadial.Maximum;

				SetDefaults4IntersectignVorRadial ( );
				_controller.SetDsgPntDefinitionVia ( DsgPntDefinition.ViaSelectionFromDb );
				txtBxAllowableIntersRadials.Text = "";

				if ( nmrcUpDownRadiusDsgntdPnts.Value == 0 )
					nmrcUpDownRadiusDsgntdPnts.Value = ( decimal ) GlobalParams.UnitConverter.DistanceToDisplayUnits ( 150000, eRoundMode.NEAREST );

				if ( cmbBxDsgntdPnts.SelectedIndex != -1 )
					cmbBxDsgntdPnts_SelectedIndexChanged ( cmbBxDsgntdPnts, null );
			}
			else
			{
				if ( _intersectDircInterval.Min > _intersectDircInterval.Max )
				{
					SetDefaults4IntersectignVorRadial ( );
				}
				else
				{
					nmrcUpDownIntersectignVorRadial.Minimum = ( decimal ) _intersectDircInterval.Min;
					nmrcUpDownIntersectignVorRadial.Maximum = ( decimal ) _intersectDircInterval.Max;

				}
				txtBxAllowableIntersRadials.Text = _intersectDircInterval.Min + "/" + _intersectDircInterval.Max;
				_controller.SetDsgPntDefinitionVia ( DsgPntDefinition.ViaVorVorRadial );
			}
		}

		private void SetDefaults4IntersectignVorRadial ( )
		{
			nmrcUpDownIntersectignVorRadial.Minimum = 0;
			nmrcUpDownIntersectignVorRadial.Maximum = 361;
		}

		private void cmbBxMoc_SelectedIndexChanged ( object sender, EventArgs e )
		{
			double moc;
			double.TryParse ( cmbBxMoc.SelectedItem.ToString ( ), out moc );
			_controller.SetMoc ( moc );
		}

		//private void btnNext_Click ( object sender, EventArgs e )
		//{
		//    pnlPntsSelection.Visible = true;
		//    pnlParams.Visible = false;

		//    //btnApply.Enabled = true;

		//    btnNext.Enabled = false;
		//    btnBack.Enabled = true;

		//    InitializeParamsFor2ndTab ( );

		//    //SetBtnNextEnabled ( );
		//    if ( radBtnVorDme.Checked )
		//    {
		//        SetVisibleGrpBxEntryDir ( true );
		//        grpBxSlctFacility.Height = nmrcUpDownAltitude.Location.Y + 25;
		//        grpBxOutboundLegParams.Parent = pnlPntsSelection;
		//        grpBxOutboundLegParams.Location = new System.Drawing.Point ( grpBxSlctFacility.Location.X, grpBxSlctFacility.Location.Y + grpBxSlctFacility.Height + 5 );
		//    }
		//    else if ( radBtnOverhead.Checked )
		//        SetVisibleGrpBxEntryDir ( false );
		//    else
		//        SetVisibleGrpBxEntryDir ( true );

		//}

		private void radBtnRightDir_CheckedChanged ( object sender, EventArgs e )
		{
			_controller.SetSideDirection ( radBtnRightDir.Checked );
		}

		private void radBtnToward_CheckedChanged ( object sender, EventArgs e )
		{
			_controller.SetEntryDirection ( radBtnToward.Checked );
			//if ( _controller.ProcType == ProcedureTypeConv.VORDME)
			//{
			// Uncomment this part for "With limiting radial" implementing
			//chckBxWithLimRadial.Visible = !radBtnToward.Checked;
			//if ( !chckBxWithLimRadial.Visible )
			//    chckBxWithLimRadial.Checked = false;
			//}
		}

		private void cmbBxDsgntdPnts_SelectedIndexChanged ( object sender, EventArgs e )
		{
			if ( cmbBxDsgntdPnts.SelectedIndex == -1 )
				return;

			bool tmp = _calledByPointPickerControl;
			_calledByPointPickerControl = false;

			string typeDsgPnt;
			Guid selectedDsgPntIdentifier = _controller.SetDesignatedPoint ( ( KeyValuePair<Guid, string> ) cmbBxDsgntdPnts.SelectedItem, out typeDsgPnt );
			//bool hasType = ( typeDsgPnt != "" );
			//lblTypeDsgPnt.Visible = hasType;
			//txtBxTypeDsgPnt.Visible = hasType;
			//txtBxTypeDsgPnt.Text = typeDsgPnt;
			if ( cmbBxDsgntdPnts.SelectedItem == null )
			{
				_designatedPointId = default ( Guid );
				ChangeLierTxtBxsAppearence ( true, cmbBxNavaids.Items.Count == 0 );
			}
			else if ( _designatedPointId != selectedDsgPntIdentifier )
			{
				_designatedPointId = selectedDsgPntIdentifier;
				ChangeLierTxtBxsAppearence ( false, cmbBxNavaids.Items.Count == 0 );
			}
			_calledByPointPickerControl = tmp;
		}

		private void cmbBxNavaids_SelectedIndexChanged ( object sender, EventArgs e )
		{
			if ( cmbBxNavaids.SelectedIndex == -1 )
				return;

			Guid navIdentifier = _controller.SetNavaid ( ( KeyValuePair<Guid, string> ) cmbBxNavaids.SelectedItem,out _selectedNavType );
            if (navIdentifier == Guid.Empty)
                return;
			lblOverheadRadial.Text = _selectedNavType.ToLower ( ).Contains ( "vor" ) ? Resources.Radial_VOR : Resources.Radial_NDB;
			bool hasSelectedItem = ( cmbBxNavaids.SelectedItem != null );
			if ( !hasSelectedItem )
				_navaidId = Guid.Empty;
			else if ( _navaidId != navIdentifier )
				_navaidId = navIdentifier;

			cmbBxNavaids.Enabled = hasSelectedItem;
			nmrcUpDownAltitude.Enabled = hasSelectedItem;
			radBtnCreateDsgnPnt.Enabled = hasSelectedItem;
			radBtnSelectDsgnPnt.Enabled = hasSelectedItem;
		}

		private void cmbBxIntersectingVor_SelectedIndexChanged ( object sender, EventArgs e )
		{
			Guid navIdentifier = _controller.SetIntersectingVor ( cmbBxIntersectingVor.Text );

			Guid intersectingVorId = Guid.Empty;
			if ( cmbBxNavaids.SelectedItem == null )
			{
				intersectingVorId = Guid.Empty;

				cmbBxNavaids.Enabled = false;
				nmrcUpDownAltitude.Enabled = false;
				radBtnCreateDsgnPnt.Enabled = false;
				radBtnSelectDsgnPnt.Enabled = false;
			}
			else if ( _navaidId != navIdentifier )
			{
				intersectingVorId = navIdentifier;

				cmbBxNavaids.Enabled = true;
				nmrcUpDownAltitude.Enabled = true;
				radBtnCreateDsgnPnt.Enabled = true;
				radBtnSelectDsgnPnt.Enabled = true;
			}
		}

		private void chckBxWithLimRadial_CheckedChanged ( object sender, EventArgs e )
		{
			_controller.SetWithLimitingRadial ( chckBxWithLimRadial.Checked );
		}

		private void chckBxProtectSector1_CheckedChanged ( object sender, EventArgs e )
		{
			// Unchecking it should erase Protection Sector 1 Area
			_controller.ConstructProtectionSector1 ( !( sender as CheckBox ).Checked, true );
		}

		private void chckBxOmniDirectionalEntry_CheckedChanged ( object sender, EventArgs e )
		{
			// Unchecking it should erase Omnidirectional entry
			_controller.ConstructOmnidirectionalEntry ( !( sender as CheckBox ).Checked, true );
			if (_controller.ProcType == ProcedureTypeConv.VorNdb)
				chckBxProtectSector1.Enabled = (sender as CheckBox).Checked;
		}

		private void chckBxProtectSector2_CheckedChanged ( object sender, EventArgs e )
		{
			// Unchecking it should erase Protection Sector 1 Area
			_controller.ConstructProtectionSector2 ( !( sender as CheckBox ).Checked, true );
		}

		private void chckBxRecipDirEntSecondary_CheckedChanged ( object sender, EventArgs e )
		{
			// Unchecking it should erase Protection reciprocal direction area area
			_controller.ConstructReciprocalEntryArea ( !( sender as CheckBox ).Checked, true);
		}

		private void chckBxIntersectRadialEntry_CheckedChanged ( object sender, EventArgs e )
		{
			_controller.ConstructIntersectingRadialEntry ( !( sender as CheckBox ).Checked, true );
		}

		private void cmbBxAircraftCategory_SelectedIndexChanged ( object sender, EventArgs e )
		{
			_controller.SetAircraftCategory ( cmbBxAircraftCategory.SelectedIndex );
		}

		private void nmrcUpDownIAS_ValueChanged ( object sender, EventArgs e )
		{
			_controller.SetIas ( ( double ) nmrcUpDownIAS.Value );
		}

		private void nmrcUpDownAltitude_ValueChanged ( object sender, EventArgs e )
		{
			_controller.SetAltitude ( ( double ) nmrcUpDownAltitude.Value );
		    RestrictTimeInterval();
		}

		private void nmrcUpDownLimDist_ValueChanged ( object sender, EventArgs e )
		{
			if ( _hasToBeCalculate )
				_controller.SetLimitingDistance ( ( double ) nmrcUpDownLimDist.Value );
		}

		private void nmrcUpDownDistance_ValueChanged ( object sender, EventArgs e )
		{
			if ( !_valueChangedFromOtherComponent )
			{
				bool tmp = _calledByPointPickerControl;
				_calledByPointPickerControl = false;
				_controller.SetNominalDistance ( ( double ) nmrcUpDownDistance.Value );
				_calledByPointPickerControl = tmp;
			}
		}

		private void nmrcUpDownRadial_ValueChanged ( object sender, EventArgs e )
		{
			bool tmp = _calledByPointPickerControl;
			_calledByPointPickerControl = false;
			if ( _hasToBeCalculate && !_valueChangedFromOtherComponent )
			{
				double value = ( double ) nmrcUpDownRadial.Value;
				_controller.SetDirection ( value );
			}
			_calledByPointPickerControl = tmp;
		}

		private void nmrcUpDownRadiusDsgntdPnts_ValueChanged ( object sender, EventArgs e )
		{
			_controller.SetRadiusForDsgPnts ( ( double ) nmrcUpDownRadiusDsgntdPnts.Value );
		}

		private void nmrcUpDownTime_ValueChanged ( object sender, EventArgs e )
		{
			if ( _hasToBeCalculate )
				_controller.SetTime ( ( double ) nmrcUpDownTime.Value );
		}

		private void nmrcUpDownOverheadRadial_ValueChanged ( object sender, EventArgs e )
		{
			double value = ( double ) nmrcUpDownOverheadRadial.Value;
            _controller.DirectionInDeg = (decimal)value;
			if ( _selectedNavType.ToLower ( ).Contains ( "vor" ) )
				value += 180;
			_controller.SetOverheadDirection ( value );
		}

		private void pointPicker_ByClickChanged ( object sender, EventArgs e )
		{
			_pointPickerClicked = !_pointPickerClicked;
			if ( _pointPickerClicked )
			{
				GlobalParams.AranEnvironment.AranUI.SetCurrentTool ( GlobalParams.AranMapToolMenuItem );
			}
			else
			{
				GlobalParams.AranEnvironment.AranUI.SetPanTool ( );
				pointPicker1.ByClick = false;
			}
		}

		private void pointPicker_LatitudeChanged ( object sender, EventArgs e )
		{
			ChangePointPickerCoords ( );
		}

		private void pointPicker_LongitudeChanged ( object sender, EventArgs e )
		{
			ChangePointPickerCoords ( );
		}

		private void nmrcUpDwnIntersectignVorRadial_ValueChanged ( object sender, EventArgs e )
		{
			if ( nmrcUpDownIntersectignVorRadial.Value == 361 )
			{
				nmrcUpDownIntersectignVorRadial.Value = 1;
				return;
			}
			// Check whether is called via changing value by user 
			if ( !_calculatedIntersectingRadial )
			{
				//if ( sender == null )
				//{
				//    nmrcUpDwnIntersectignVorRadial.Value = ( decimal ) ARANMath.Modulus ( Math.Round ( _controller.CalculateIntersectRadial ( ) ), 360 );
				//    return;

				if ( _intersectDircInterval.Min > _intersectDircInterval.Max )
				{
					double value = ( double ) nmrcUpDownIntersectignVorRadial.Value;
					//double diffWithMin =  (_intersectDircInterval.Min - value)
					if ( value < _intersectDircInterval.Min && value > _intersectDircInterval.Max )
					{
						_calledByPointPickerControl = true;
						nmrcUpDownIntersectignVorRadial.Value = ( decimal ) _intersectDircInterval.Min;
						//( decimal ) ARANMath.Modulus ( Math.Round ( _controller.CalculateIntersectRadial ( ) ), 360 );
						_calledByPointPickerControl = false;
						return;
					}
				}
				//}
				bool tmp = _calledByPointPickerControl;
				_calledByPointPickerControl = false;
				_controller.SetIntersectingVorRadial ( ( double ) nmrcUpDownIntersectignVorRadial.Value );
				_calledByPointPickerControl = tmp;
			}

		}

		private void btnReport_Click ( object sender, EventArgs e )
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				_controller.ShowReportWindow ();
			}
			catch ( Exception ex )
			{
				MessageBox.Show ( ex.ToString ( ) );
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void btnSettings_Click ( object sender, EventArgs e )
		{
			_controller.SettingBtnClicked ( );
		}

		private void btnSave_Click ( object sender, EventArgs e )
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				_controller.Save ( );
				Close ( );
			}
			catch ( Exception ex )
			{
				MessageBox.Show ( ex.ToString ( ) );
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void btnAbout_Click ( object sender, EventArgs e )
		{
			_controller.AboutBtnClicked ( );
		}

		#endregion

		#region Methods

		internal void SetCategoryList ( List<string> categoryList )
		{
            if (categoryList == null || categoryList.Count == 0)
                return;

            var selectedItem = cmbBxAircraftCategory.SelectedItem;
            cmbBxAircraftCategory.Items.Clear();
            int selectedIndex = 0;
            int i = 0;
            foreach (var item in categoryList)
            {
                cmbBxAircraftCategory.Items.Add(item);
                if (item == (string) selectedItem)
                    selectedIndex = i;
                i++;
            }
            if (cmbBxAircraftCategory.SelectedIndex == -1)
            { 
                cmbBxAircraftCategory.SelectedIndex = selectedIndex;
                cmbBxAircraftCategory_SelectedIndexChanged(null, null);
            }
		}

		internal void SetMaxFixToleranceDist ( double value, bool enable )
		{
			txtBxFixToleranceDist.Text = value.ToString (CultureInfo.InvariantCulture);
			SetFixToleranceComps ( enable );
		}

		internal void AddIntersectingVorList ( List<string> intersectingVorList, int selectedIndex )
		{
			cmbBxIntersectingVor.Items.Clear ( );
			if ( intersectingVorList == null || intersectingVorList.Count == 0 )
			{
				SetFixToleranceComps ( false );
				nmrcUpDownIntersectignVorRadial.ForeColor = System.Drawing.Color.Red;
				return;
			}
			SetFixToleranceComps ( true );
			nmrcUpDownIntersectignVorRadial.ForeColor = System.Drawing.SystemColors.WindowText;

			foreach ( var navaid in intersectingVorList )
			{
				cmbBxIntersectingVor.Items.Add ( navaid );
			}
			cmbBxIntersectingVor.SelectedIndex = selectedIndex;
		}

		internal void AddNavaids ( Dictionary<Guid, string> navaidList, int selectedIndex )
		{
			//cmbBxNavaids.Items.Clear ( );
			//cmbBxNavaids.DataSource = null;
			if ( navaidList == null || navaidList.Count == 0 )
			{
				btnConstruct.Enabled = false;
				grpBxSetDsgPnt.Visible = false;
				return;
			}
			cmbBxNavaids.DataSource = new BindingSource ( navaidList, null );
			cmbBxNavaids.DisplayMember = "Value";
			cmbBxNavaids.SelectedIndex = selectedIndex;
		}

		internal void AddDesignatedPointList ( Dictionary<Guid, string> designatedPointList, int selectedIndex )
		{
			cmbBxDsgntdPnts.IntegralHeight = false;
			cmbBxDsgntdPnts.MaxDropDownItems = 8;
			cmbBxDsgntdPnts.Sorted = true;
			cmbBxDsgntdPnts.Refresh ( );

			//cmbBxDsgntdPnts.Items.Clear ( );
			//cmbBxDsgntdPnts.DataSource = null;
			if ( designatedPointList.Count == 0 )
			{
				((BindingSource) cmbBxDsgntdPnts.DataSource)?.Clear ( );
				btnConstruct.Enabled = false;
				return;
			}

			cmbBxDsgntdPnts.DataSource = new BindingSource ( designatedPointList, null );
			cmbBxDsgntdPnts.DisplayMember = "Value";
			cmbBxDsgntdPnts.SelectedIndex = selectedIndex;
		}

		internal void EnableConstructBtn ( bool isEnabled, string errorMessage = "" )
		{
			if ( isEnabled )
				SetBtnConstructEnabled ( );
			else
			{
				btnConstruct.Enabled = false;
				if ( errorMessage != "" )
					MessageBox.Show ( this, errorMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning );
			}
		}

		internal void Show ( IWin32Window iWin32Window, MainController mainController )
		{
			_controller = mainController;
			Show ( iWin32Window );
		}

		internal void InitializeParams ( decimal altitude, decimal radius4DsgPnt, decimal timeInMin )
		{
			if ( _controller.ProcType == ProcedureTypeConv.Vorvor )
				radBtnViaVorRadials.Checked = true;
			//3050;b            
			nmrcUpDownAltitude.Value = altitude;
			if ( _controller.ProcType == ProcedureTypeConv.Vordme )
			{
				radBtnSelectDsgnPnt.Checked = true;
				//150;
				nmrcUpDownRadiusDsgntdPnts.Value = radius4DsgPnt;
			}
			nmrcUpDownTime.Minimum = timeInMin;
            SetCategoryList(_controller.Speed.CategoryList);
		}

		private void SetBtnConstructEnabled ( )
		{
			if ( !_controller.HasNavaid )
				btnConstruct.Enabled = false;
			//else if ( radBtnVorDme.Checked )
			else if ( _controller.ProcType == ProcedureTypeConv.Vordme )
			{
				btnConstruct.Enabled = ( _controller.HasDsgPoint && btnConstruct.Enabled );
			}
			//else if ( radBtnVorVor.Checked )
			else if ( _controller.ProcType == ProcedureTypeConv.Vorvor )
				btnConstruct.Enabled = ( cmbBxIntersectingVor.Items.Count > 0 && _controller.HasDsgPoint && txtBxFixToleranceDist.ForeColor != System.Drawing.Color.Red );
			//else if ( radBtnOverhead.Checked )
			else if ( _controller.ProcType == ProcedureTypeConv.VorNdb )
				btnConstruct.Enabled = true;
		}

		private void SetIntersectVorCntrlsVisibility ( bool visible )
		{
			lblIntersectVor.Visible = visible;
			cmbBxIntersectingVor.Visible = visible;

			lblIntersectVORRadial.Visible = visible;
			lblIntersectingVorRadialUnit.Visible = visible;
			nmrcUpDownIntersectignVorRadial.Visible = visible;
			nmrcUpDownIntersectignVorRadial.Enabled = !radBtnViaDsgPnt.Checked;
		}

		private void SetOverheadRadialVisibility ( bool visible )
		{
			lblOverheadRadial.Visible = visible;
			lblOverheadRadialUnit.Visible = visible;
			nmrcUpDownOverheadRadial.Visible = visible;
		}

		internal void SetInterval4DsgSelection ( double minDist, double maxDist )
		{
			nmrcUpDownRadiusDsgntdPnts.Minimum = ( decimal ) minDist;
			nmrcUpDownRadiusDsgntdPnts.Maximum = ( decimal ) maxDist;

			txtBxAllowableRadius4DsgPnt.Text = minDist + "/" + maxDist;
		}

		internal void SetInterval4DsgDistance ( double p1, double p2 )
		{
			nmrcUpDownDistance.Minimum = ( decimal ) p1;
			nmrcUpDownDistance.Maximum = ( decimal ) p2;
		}

		private void ApplyInterfaceLanguage ( )
		{
			//Resources.Culture = Thread.CurrentThread.CurrentUICulture;
			lblAltitudeUnit.Text = GlobalParams.UnitConverter.HeightUnit;
			lblFixToleranceDistUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
			lblMocUnit.Text = GlobalParams.UnitConverter.HeightUnit;
			//lblNavElevUnit.Text = GlobalParams.UnitConverter.HeightUnit;
			lblDistanceUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
			//lblIntersectDist.Text = GlobalParams.UnitConverter.DistanceUnit;
			lblRadiusDsgntdPntUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
			lblAllowableRadius4DsgPntUnit.Text = GlobalParams.UnitConverter.DistanceUnit;

			lblPermSpeedUnit.Text = GlobalParams.UnitConverter.SpeedUnit;
			lblIASUnit.Text = GlobalParams.UnitConverter.SpeedUnit;
			lblLimDistUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
			//lblLegLengthUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
			lblPermisLimDistUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
		}

		private void ChangePointPickerCoords ( )
		{
			if ( !_calledByPointPickerControl )
				return;
			Point pntGeo = new Point ( pointPicker1.Longitude, pointPicker1.Latitude );
			Point pntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( pntGeo );

			_controller.SetDesignatedPoint ( pntPrj );
		}

		private void control_KeyDown ( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Enter )
				e.SuppressKeyPress = true;
		}

		private void ChangeLierTxtBxsAppearence ( bool toFront, bool lierTxtBxRadiusForDsgPntBringtToFront = true )
		{
			if ( toFront )
			{
				if ( radBtnCreateDsgnPnt.Checked )
				{
					pointPicker1.Enabled = false;
					lierTxtBxNomDist.BringToFront ( );
					lierTxtBxRadial.BringToFront ( );
				}
				else
				{
					if ( lierTxtBxRadiusForDsgPntBringtToFront )
						lierTxtBxRadiusForDsgPnt.BringToFront ( );
					cmbBxDsgntdPnts.Enabled = false;
				}
			}
			else
			{
				if ( radBtnCreateDsgnPnt.Checked )
				{
					pointPicker1.Enabled = true;
					lierTxtBxNomDist.SendToBack ( );
					lierTxtBxRadial.SendToBack ( );
				}
				else
				{
					cmbBxDsgntdPnts.Enabled = true;
					if ( lierTxtBxRadiusForDsgPntBringtToFront )
						lierTxtBxRadiusForDsgPnt.SendToBack ( );
				}
			}
			SetBtnConstructEnabled ( );
		}

		private void SetPointPickerCoords ( Point pntPrj )
		{
			Point pntGeo = GlobalParams.SpatialRefOperation.ToGeo<Point> ( pntPrj );
			pointPicker1.Latitude = pntGeo.Y;
			pointPicker1.Longitude = pntGeo.X;
		}

		private void EnableComponontsVisible ( bool isChosenPntTypeSelect )
		{
			pnlSelectDsgPnt.Visible = isChosenPntTypeSelect;
			//pointPicker1.Visible = !isChosenPntTypeSelect;
			if ( _controller.ProcType != ProcedureTypeConv.Vorvor )
			{
				nmrcUpDownDistance.Enabled = !isChosenPntTypeSelect;
				nmrcUpDownRadial.Enabled = !isChosenPntTypeSelect;
				nmrcUpDownIntersectignVorRadial.Enabled = ( ( _controller.ProcType == ProcedureTypeConv.Vorvor ) && !isChosenPntTypeSelect && radBtnViaDsgPnt.Checked );
			}
		}

		#endregion

		internal void WarnUserWrongParameters ( double dist )
		{
			_holdingDefined = false;
            MessageBox.Show(Resources.Holding_Error + "\nDist = " + dist + " " + GlobalParams.Settings.DistanceUnit);

		}

		#region Fields

		internal string Caption;

		private Guid _navaidId, _designatedPointId;
		private bool _hasToBeCalculate, _pointPickerClicked, _valueChangedFromOtherComponent, _calledByPointPickerControl, _calculatedIntersectingRadial;
		private MainController _controller;
		private SpeedInterval _intersectDircInterval;
		private bool _holdingDefined;

        private void radBtnViaTime_CheckedChanged(object sender, EventArgs e)
        {
            pnlDistance.Visible = radBtnViaDist.Checked;
            pnlDistance.Dock = pnlDistance.Visible ? DockStyle.Bottom : DockStyle.None;

            pnlTime.Visible = !pnlDistance.Visible;
            pnlTime.Dock = pnlTime.Visible ? DockStyle.Bottom : DockStyle.None;

            RestrictTimeInterval();

            _controller.SetOutboundLegDefinition(pnlTime.Visible);
        }

        private string _selectedNavType;

		#endregion

        internal void ChangeSpeedToMach(bool isMach)
        {
            if (isMach)
            {
                lblIASUnit.Text = "M";
                lblPermSpeedUnit.Text = "M";
                txtBxAllowedSpeedMach.Text = "0.83";
                txtBxIas.Text = "0.83";

                txtBxAllowedSpeedMach.Visible = true;
                txtBxIas.Visible = true;
                txtBxAllowedSpeedMach.BringToFront();
                txtBxIas.BringToFront();

                nmrcUpDownIAS.Visible = false;
                txtBxAllowableSpeed.Visible = false;
            }
            else
            {
                txtBxAllowableSpeed.Visible = true;
                lblPermSpeedUnit.Text = GlobalParams.UnitConverter.SpeedUnit;
                lblIASUnit.Text = GlobalParams.UnitConverter.SpeedUnit;

                nmrcUpDownIAS.Visible = true;
                txtBxAllowableSpeed.Visible = true;
                nmrcUpDownIAS.BringToFront();
                txtBxAllowableSpeed.BringToFront();

                txtBxAllowedSpeedMach.Visible = false;
                txtBxIas.Visible = false;

            }
        }

        private void radBtnTerminal_CheckedChanged(object sender, EventArgs e)
        {
            grpBxConditions.Visible = !chckBxEnroute.Checked;
            lblAircraftCategory.Visible = !chckBxEnroute.Checked;
            cmbBxAircraftCategory.Visible = !chckBxEnroute.Checked;

            if (chckBxEnroute.Checked)
            {
                nmrcUpDownAltitude.Minimum = (decimal)GlobalParams.UnitConverter.HeightToDisplayUnits(4250);
                _controller.SetFlightPhase(FlightPhase.Enroute);
            }
            else 
            {
                nmrcUpDownAltitude.Minimum = 1;
                if (radBtnTerminalNormal.Checked)
                    _controller.SetFlightPhase(FlightPhase.TerminalNormal);
                else if (radBtnTerminalTurbulence.Checked)
                    _controller.SetFlightPhase(FlightPhase.TerminalTurbulence);
                else
                    _controller.SetFlightPhase(FlightPhase.TerminalInitialApproach);
            }
        }

        private void radBtnTerminalNormal_CheckedChanged(object sender, EventArgs e)
        {
            nmrcUpDownAltitude.Minimum = 1;
            if (radBtnTerminalNormal.Checked)
            {
                _controller.SetFlightPhase(FlightPhase.TerminalNormal);
            }
            else if (radBtnTerminalTurbulence.Checked)
            {
                _controller.SetFlightPhase(FlightPhase.TerminalTurbulence);
            }
            else if (radBtnTerminalInitialApproach.Checked)
            {
                _controller.SetFlightPhase(FlightPhase.TerminalInitialApproach);
            }
        }

        private void radBtnTerminalTurbulence_CheckedChanged(object sender, EventArgs e)
        {
            radBtnTerminalNormal_CheckedChanged(null, null);
        }
    }
}