using System;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Panda.Common;
using Aran.Panda.Conventional.Racetrack.Properties;
using System.Threading;
using Aran.AranEnvironment;
using Aran.Geometries;
using System.Collections.Generic;

namespace Aran.Panda.Conventional.Racetrack
{
	public partial class FormMain : Form
	{
		public FormMain ( )
		{
			InitializeComponent ( );

			_hasToBeCalculate = true;
			_firstTimeProcTypeChanged = true;
			_pointPickerClicked = false;
			_valueChangedFromOtherComponent = false;
			_calledByPointPickerControl = true;
			_calculatedIntersectingRadial = false;
			toolTip.SetToolTip ( nmrcUpDownLimDist, "Time should be in 1-3 minutes");
		}

		#region Event handlers of business Logic

		internal void OnAircraftCategoryListChanged ( object sender, CategoryListEventArg argCtgList )
		{
			cmbBxAircraftCategory.DataSource = argCtgList.CategoryList;
		}

		internal void OnIASIntervalChanged ( object sender, Interval iasInterval )
		{
			nmrcUpDownIAS.Minimum = ( decimal ) iasInterval.Left;
			nmrcUpDownIAS.Maximum = ( decimal ) iasInterval.Right;
			txtBxAllowableSpeed.Text = iasInterval.Left.ToString ( ) + "/" + iasInterval.Right.ToString ( );
		}

		internal void OnTimeChanged ( object sender, CommonEventArg argTime )
		{
			_hasToBeCalculate = false;
			nmrcUpDownTime.Value = ( decimal ) argTime.Value;
			_hasToBeCalculate = true;
		}

		internal void OnLimitDistIntervalChanged ( object sender, Interval interval )
		{
			nmrcUpDownLimDist.Minimum = ( decimal ) interval.Left;
			nmrcUpDownLimDist.Maximum = ( decimal ) interval.Right;
		}

		internal void OnLimDistChanged ( object sender, CommonEventArg arg )
		{
			_hasToBeCalculate = false;
			nmrcUpDownLimDist.Value = ( decimal ) arg.Value;
			_hasToBeCalculate = true;
		}

		internal void OnOrganisationChanged ( object sender, OrganisationEventArg arg )
		{
			txtBxOrganisation.Text = arg.Organisation.Designator;
		}

		public void OnRE_AngleChanged ( object sender, CommonEventArg arg )
		{
			if ( double.IsNaN ( arg.Value ) )
				txtBxAngRE.Text = "";
			else
				txtBxAngRE.Text = arg.Value.ToString ( );
		}

		internal void OnAirportHeliportChanged ( object sender, AirportEventArg arg )
		{
			txtBxAirport.Text = arg.Airport.Designator;
		}

		internal void OnNavaidListChanged ( object sender, NavaidListEventArg arg )
		{
			NavaidList = arg.NavaidList;
			AddNavaids ( NavaidList, cmbBxNavaids );
		}

		internal void OnIntersectingNavaidListChanged ( object sender, NavaidListEventArg arg )
		{
			if ( arg.NavaidList.Count > 0 )
			{
				btnNext.Enabled = true;
				AddNavaids ( arg.NavaidList, cmbBxIntersectingVor );
			}
			else
				btnNext.Enabled = false;
		}

		private void AddNavaids ( List<Navaid> navaidList, ComboBox cmbBxNavaids )
		{
			string text = cmbBxNavaids.Text;
			cmbBxNavaids.Items.Clear ( );
			if ( navaidList.Count == 0 )
				return;

			foreach ( Navaid navaid in navaidList )
			{
				cmbBxNavaids.Items.Add ( navaid.Designator );
			}
			int index = navaidList.FindIndex ( nav => nav.Designator == text );
			if ( index == -1 )
				index = 0;
			cmbBxNavaids.SelectedIndex = index;
		}

		internal void OnDesignatePointListChanged ( object sender, DsgPntListEventArg arg )
		{
			DesignatePointList = arg.DsgPntList;

			//cmbBxDsgntdPnts.DisplayMember = "designator";
			//if ( index != 0 )
			//{
			//    _calculatedDsgPnt = true;
			//    cmbBxDsgntdPnts.DataSource = arg.DsgPntList;
			//    AddNavaids(arg.DsgPntList
			//    _calculatedDsgPnt = false;
			//    cmbBxDsgntdPnts.SelectedIndex = index;
			//}
		}

		internal void OnDirectionChanged ( object sender, DirectionEventArg argDir )
		{
			_valueChangedFromOtherComponent = true;
			nmrcUpDownRadial.Value = ( decimal ) ARANMath.Modulus ( Math.Round ( argDir.DirectionForGUI ), 360 );
			_valueChangedFromOtherComponent = false;
		}

		private void OnIntersectionDirectionChanged ( object sender, DirectionEventArg arg )
		{
			_calculatedIntersectingRadial = true;
			nmrcUpDwnIntersectignVorRadial.Value = ( decimal ) ARANMath.Modulus ( Math.Round ( arg.DirectionForGUI ), 360 );
			_calculatedIntersectingRadial = false;
		}

		internal void OnDistanceChanged ( object sender, NomDistanceEventArg argDist )
		{
			_valueChangedFromOtherComponent = true;
			nmrcUpDownDistance.Value = ( decimal ) argDist.NomDistanceForGUI;
			_valueChangedFromOtherComponent = false;
		}

		internal void OnDesignatedPointChanged ( object sender, DsgPntEventArg argDsgPnt )
		{
			if ( argDsgPnt.DsgPntPrj != null )
			{
				ChangeLierTxtBxsAppearence ( false, cmbBxNavaids.Items.Count == 0 );

				//bool tmp = _valueChangedFromOtherComponent;
				//_valueChangedFromOtherComponent = true;
				if ( !_calledByPointPickerControl )
					SetPointPickerCoords ( argDsgPnt.DsgPntPrj );
				//_valueChangedFromOtherComponent = tmp;

				//InitHolding.ConvRacetrackModel.DrawDesigntdPoint(  argDsgPnt.DsgPntPrj );
				btnNext.Enabled = true;
			}
			else
			{
				ChangeLierTxtBxsAppearence ( true, cmbBxNavaids.Items.Count == 0 );
				//InitHolding.ConvRacetrackModel.DeleteDesignatedPoint ( );
			}
		}

		internal void OnMouseClickedOnMap ( object sender, MapMouseEventArg e )
		{
			Point pnt = new Point ( e.X, e.Y );
			SetPointPickerCoords ( pnt );
		}

		internal void OnDeacitvatedPointPickerTool ( )
		{
			pointPicker_ByClickChanged ( null, null );
		}

		internal void OnMapMouseUp ( object sender, MapMouseEventArg mapMouseMoveEventArg )
		{
			if ( _pointPickerClicked && radBtnCreateDsgnPnt.Checked && mapMouseMoveEventArg.Button == System.Windows.Forms.MouseButtons.Left )
			{
				InitHolding.ConvRacetrackModel.SetDesignatedPoint ( mapMouseMoveEventArg.X, mapMouseMoveEventArg.Y );
			}
		}

		private void OnAppliedValueChanged ( bool isEqual )
		{
			btnApply.Enabled = !isEqual;
			btnReport.Enabled = isEqual;
			btnSave.Enabled = isEqual;
		}

		#endregion

		#region Event handlers of form control

		private void frmConvRacetrackMain_Load ( object sender, EventArgs e )
		{
			pnlPntsSelection.Parent = this;
			pnlParams.Parent = this;

			pnlPntsSelection.Location = new System.Drawing.Point ( 0, 0 );
			pnlPntsSelection.Visible = true;

			pnlParams.Location = new System.Drawing.Point ( 0, 0 );
			pnlParams.Visible = false;

			tabControl1.Visible = false;

			radBtnVorDme.Tag = ProcedureTypeConv.VORDME;
			radBtnOverhead.Tag = ProcedureTypeConv.VOR_NDB;
			radBtnVorVor.Tag = ProcedureTypeConv.VORVOR;

			//_radBtnsProcType [ 0 ] = radBtnVorDme;
			//_radBtnsProcType [ 1 ] = radBtnVorNdb;
			//_radBtnsProcType [ 2 ] = radBtnVorVor;

			#region For Model

			txtBxRadius.Text = GlobalParams.UnitConverter.DistanceToDisplayUnits ( GlobalParams.Settings.Radius, eRoundMode.NERAEST ).ToString ( );

			InitHolding.ConvRacetrackModel.AddOrganisationChangedEvent ( OnOrganisationChanged );
			InitHolding.ConvRacetrackModel.AddAirdromeChangedEvent ( OnAirportHeliportChanged );
			InitHolding.ConvRacetrackModel.AddNavListChangedEvent ( OnNavaidListChanged );
			InitHolding.ConvRacetrackModel.AddIntersectingNavListChangedHandler ( OnIntersectingNavaidListChanged );
			InitHolding.ConvRacetrackModel.AddDsgPointListChangedEvent ( OnDesignatePointListChanged );
			InitHolding.ConvRacetrackModel.AddDesignatedPointChangedEvent ( OnDesignatedPointChanged );
			InitHolding.ConvRacetrackModel.AddDirectionChangedEvent ( OnDirectionChanged );
			InitHolding.ConvRacetrackModel.AddRE_AngleChangedEvent ( OnRE_AngleChanged );
			InitHolding.ConvRacetrackModel.AddIntersectionDirectionChangedEvent ( OnIntersectionDirectionChanged );
			InitHolding.ConvRacetrackModel.AddNominalDistanceChangedEvent ( OnDistanceChanged );
			InitHolding.ConvRacetrackModel.AddAppliedValueChangedEvent ( OnAppliedValueChanged );

			try
			{
				InitHolding.ConvRacetrackModel.GetOrganisation ( );
			}
			catch ( Exception except )
			{
				MessageBox.Show ( except.Message, "Exception occured ", MessageBoxButtons.OK, MessageBoxIcon.Error );
				Close ( );
			}

			radBtnVorDme.Checked = true;
			nmrcUpDownAltitude.Value = 3050;
			radBtnSelectDsgnPnt.Checked = true;
			nmrcUpDownRadiusDsgntdPnts.Value = 150;
			#endregion

			ApplyInterfaceLanguage ( );

			this.Size = tabControl1.Size + new System.Drawing.Size ( 5, 5 );
			this.MaximumSize = this.Size;
			this.MinimumSize = this.Size;
		}

		private void frmConvInitial_FormClosing ( object sender, FormClosingEventArgs e )
		{
            if (!InitHolding.ConvRacetrackModel.IsDisplayClear)
            {
                if (MessageBox.Show("Clean graphics ?", "Graphics info", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    InitHolding.ConvRacetrackModel.CleanDisplay();
			}
            GlobalParams.MenuItem.Checked = false;
		}

		private void txtBxRadius_TextChanged ( object sender, EventArgs e )
		{
			double tmp;
			double.TryParse ( txtBxRadius.Text, out tmp );
			InitHolding.ConvRacetrackModel.SetRadiusForNavaids ( tmp );
		}

		private void radBtnSelectDsgnPnt_CheckedChanged ( object sender, EventArgs e )
		{
			EnableComponontsVisible ( radBtnSelectDsgnPnt.Checked );
			if ( radBtnCreateDsgnPnt.Checked )
				InitHolding.ConvRacetrackModel.SetPointChoice ( PointChoice.Create );
			else
			{
				InitHolding.ConvRacetrackModel.SetPointChoice ( PointChoice.Select );

				if ( pointPicker1.ByClick )
					pointPicker_ByClickChanged ( null, null );
			}
			nmrcUpDwnIntersectignVorRadial.Enabled = radBtnVorVor.Checked && radBtnCreateDsgnPnt.Checked;
		}

		private void radBtnVorDme_CheckedChanged ( object sender, EventArgs e )
		{
			if ( ( ( RadioButton ) sender ).Checked )
			{
				InitHolding.ConvRacetrackModel.SetProcType ( ( ProcedureTypeConv ) ( ( RadioButton ) sender ).Tag );
				if ( radBtnVorDme.Checked )
				{
					grpBxSetDsgPnt.Visible = true;
					grpBxEntryDir.Enabled = true;
					chckBxIntersectRadialEntry.Visible = false;
					chckBxProtectSector1.Enabled = true;

					SetIntersectDistVisibility ( false );
					SetLimDistCntrlsVisibility ( true );
					SetOverheadRadialVisibility ( false );
					SetIntersectVorCntrlsVisibility ( false );
				}
				else if ( radBtnOverhead.Checked )
				{
					grpBxSetDsgPnt.Visible = false;
					grpBxEntryDir.Enabled = false;
					chckBxIntersectRadialEntry.Visible = false;
					chckBxProtectSector1.Enabled = true;
					nmrcUpDownOverheadRadial_ValueChanged ( nmrcUpDownOverheadRadial, null );

					SetIntersectDistVisibility ( false );
					SetLimDistCntrlsVisibility ( false );
					SetOverheadRadialVisibility ( true );
					SetIntersectVorCntrlsVisibility ( false );
				}
				else
				{
					grpBxSetDsgPnt.Visible = true;
					grpBxEntryDir.Enabled = true;
					chckBxWithLimRadial.Enabled = false;
					chckBxIntersectRadialEntry.Visible = true;
					chckBxProtectSector1.Enabled = false;

					SetIntersectDistVisibility ( true );
					SetLimDistCntrlsVisibility ( false );
					SetOverheadRadialVisibility ( false );
					SetIntersectVorCntrlsVisibility ( true );
				}
				if ( !_firstTimeProcTypeChanged )
				{
					_hasToBeCalculate = true;
				}
				_firstTimeProcTypeChanged = false;
			}
		}

		private void SetIntersectVorCntrlsVisibility ( bool visible )
		{
			lblIntersectVor.Visible = visible;
			cmbBxIntersectingVor.Visible = visible;

			lblVorRadial.Visible = visible;
			lblVorRadialUnit.Visible = visible;
			nmrcUpDwnIntersectignVorRadial.Visible = visible;
			nmrcUpDwnIntersectignVorRadial.Enabled = radBtnCreateDsgnPnt.Checked;
		}

		private void SetOverheadRadialVisibility ( bool visible )
		{
			lblOverheadRadial.Visible = visible;
			lblOverheadRadialUnit.Visible = visible;
			nmrcUpDownOverheadRadial.Visible = visible;
		}

		private void SetIntersectDistVisibility ( bool visible )
		{
			lblCloseDist.Visible = visible;
			txtBxCloseDist.Visible = visible;
			lblCloseDistUnit.Visible = visible;

			lblFurtherDist.Visible = visible;
			txtBxFurtherDist.Visible = visible;
			lblFurtherDistUnit.Visible = visible;
		}

		private void btnApply_Click ( object sender, EventArgs e )
		{
			txtBxAngRE.Text = "";
			InitHolding.ConvRacetrackModel.ConstructBasicArea ( );
			InitHolding.ConvRacetrackModel.ConstructProtectionSector1 ( !chckBxProtectSector1.Checked );
			InitHolding.ConvRacetrackModel.ConstructProtectionSector2 ( !chckBxProtectSector2.Checked );
			InitHolding.ConvRacetrackModel.ConstructReciprocalEntryArea ( !chckBxRecipDirEntSecondary.Checked );
			btnReport.Enabled = true;            
			//chckBxProtectSector1.Enabled = true;
			//chckBxProtectSector2.Enabled = true;
			//chckBxRecipDirEntSecondary.Enabled = true;
		}

		private void btnClear4Debug_Click ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.CleanDisplay ( );
		}

		private void btnNext_Click ( object sender, EventArgs e )
		{
			pnlPntsSelection.Visible = false;
			pnlParams.Visible = true;
			InitializeParamsFor2ndTab ( );
		}

		private void prevBtn_Click ( object sender, EventArgs e )
		{
			pnlPntsSelection.Visible = true;
			pnlParams.Visible = false;
		}

		private void radBtnRightDir_CheckedChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetSideDirection ( radBtnRightDir.Checked );
		}

		private void radBtnToward_CheckedChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetEntryDirection ( radBtnToward.Checked );
			if ( radBtnVorDme.Checked )
			{
				chckBxWithLimRadial.Enabled = !radBtnToward.Checked;
				if ( !chckBxWithLimRadial.Enabled )
					chckBxWithLimRadial.Checked = false;
			}
			//if ( _applyBtnClicked )
			//    AppliedValueChanged ( _appliedIsToward == RadBtnToward.Checked );
		}

		private void cmbBxDsgntdPnts_SelectedIndexChanged ( object sender, EventArgs e )
		{
			bool tmp = _calledByPointPickerControl;
			_calledByPointPickerControl = false;

			var dsgPnt = DesignatePointList.Find ( arg => arg.Designator == cmbBxDsgntdPnts.Text );
			InitHolding.ConvRacetrackModel.SetDesignatedPoint ( dsgPnt );

			if ( cmbBxDsgntdPnts.SelectedItem == null )
			{
				_designatedPointID = default ( Guid );
				ChangeLierTxtBxsAppearence ( true, cmbBxNavaids.Items.Count == 0 );
			}
			else if ( _designatedPointID != dsgPnt.Identifier )
			{
				_designatedPointID = dsgPnt.Identifier;
				ChangeLierTxtBxsAppearence ( false, cmbBxNavaids.Items.Count == 0 );
			}
			_calledByPointPickerControl = tmp;
		}

		private void cmbBxNavaids_SelectedIndexChanged ( object sender, EventArgs e )
		{
			var navaid = NavaidList.Find ( nav => nav.Designator == cmbBxNavaids.Text );
			InitHolding.ConvRacetrackModel.SetNavaid ( navaid );

			if ( cmbBxNavaids.SelectedItem == null )
			{
				_navaidID = default ( Guid );

				cmbBxNavaids.Enabled = false;
				nmrcUpDownAltitude.Enabled = false;
				radBtnCreateDsgnPnt.Enabled = false;
				radBtnSelectDsgnPnt.Enabled = false;
			}
			else if ( _navaidID != navaid.Identifier )
			{
				_navaidID = navaid.Identifier;

				cmbBxNavaids.Enabled = true;
				nmrcUpDownAltitude.Enabled = true;
				radBtnCreateDsgnPnt.Enabled = true;
				radBtnSelectDsgnPnt.Enabled = true;
			}
		}

		private void cmbBxIntersectingVor_SelectedIndexChanged ( object sender, EventArgs e )
		{
			var navaid = NavaidList.Find ( nav => nav.Designator == cmbBxIntersectingVor.Text );
			double [] distances = InitHolding.ConvRacetrackModel.SetIntersectingVor ( navaid );

			if ( distances != null )
			{
				txtBxCloseDist.Text = Math.Round ( distances [ 0 ] ).ToString ( );
				txtBxFurtherDist.Text = Math.Round ( distances [ 1 ] ).ToString ( );
			}

			Guid _intersectingVorId = Guid.Empty;
			if ( cmbBxNavaids.SelectedItem == null )
			{
				_intersectingVorId = Guid.Empty;

				cmbBxNavaids.Enabled = false;
				nmrcUpDownAltitude.Enabled = false;
				radBtnCreateDsgnPnt.Enabled = false;
				radBtnSelectDsgnPnt.Enabled = false;
			}
			else if ( _navaidID != navaid.Identifier )
			{
				_intersectingVorId = navaid.Identifier;

				cmbBxNavaids.Enabled = true;
				nmrcUpDownAltitude.Enabled = true;
				radBtnCreateDsgnPnt.Enabled = true;
				radBtnSelectDsgnPnt.Enabled = true;
			}

			//AddNavaids ( NavaidList, cmbBxNavaids, _intersectingVorId );
		}

		private void chckBxWithLimRadial_CheckedChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetWithLimitingRadial ( chckBxWithLimRadial.Checked );
		}

		private void chckBxProtectSector1_CheckedChanged ( object sender, EventArgs e )
		{
			// Unchecking it should erase Protection Sector 1 Area
			InitHolding.ConvRacetrackModel.ConstructProtectionSector1 ( !( sender as CheckBox ).Checked );
		}

		private void chckBxProtectSector2_CheckedChanged ( object sender, EventArgs e )
		{
			// Unchecking it should erase Protection Sector 1 Area
			InitHolding.ConvRacetrackModel.ConstructProtectionSector2 ( !( sender as CheckBox ).Checked );
		}

		private void chckBxRecipDirEntSecondary_CheckedChanged ( object sender, EventArgs e )
		{
			// Unchecking it should erase Protection reciprocal direction area area
			InitHolding.ConvRacetrackModel.ConstructReciprocalEntryArea ( !( sender as CheckBox ).Checked );
		}

		private void chckBxIntersectRadialEntry_CheckedChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.ConstructIntersectingRadialEntry ( !( sender as CheckBox ).Checked );
		}

		private void cmbBxAircraftCategory_SelectedIndexChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetAircraftCategory ( cmbBxAircraftCategory.SelectedIndex );
		}

		private void nmrcUpDownIAS_ValueChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetIAS ( ( double ) nmrcUpDownIAS.Value );
			//if ( _applyBtnClicked )
			//    AppliedValueChanged ( _appliedIAS == ( double ) NmrcUpDownIAS.Value );
		}

		private void nmrcUpDownAltitude_ValueChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetAltitude ( ( double ) nmrcUpDownAltitude.Value );
			//if ( _applyBtnClicked )
			//    AppliedValueChanged ( _appliedAltitude == ( double ) NmrcUpDownAltitude.Value );
		}

		private void nmrcUpDownLimDist_ValueChanged ( object sender, EventArgs e )
		{
			if ( _hasToBeCalculate )
				InitHolding.ConvRacetrackModel.SetLimitingDistance ( ( double ) nmrcUpDownLimDist.Value );
			//if ( _applyBtnClicked )
			//    AppliedValueChanged ( _appliedLimitingDistance == ( double ) NmrcUpDownLimDist.Value );
		}

		private void nmrcUpDownDistance_ValueChanged ( object sender, EventArgs e )
		{
			bool tmp = _calledByPointPickerControl;
			_calledByPointPickerControl = false;
			if ( !_valueChangedFromOtherComponent )
				InitHolding.ConvRacetrackModel.SetNominalDistance ( ( double ) nmrcUpDownDistance.Value );
			_calledByPointPickerControl = tmp;
		}

		private void nmrcUpDownRadial_ValueChanged ( object sender, EventArgs e )
		{
			bool tmp = _calledByPointPickerControl;
			_calledByPointPickerControl = false;
			if ( _hasToBeCalculate && !_valueChangedFromOtherComponent )
				InitHolding.ConvRacetrackModel.SetDirection ( ( double ) nmrcUpDownRadial.Value );
			_calledByPointPickerControl = tmp;
		}

		private void nmrcUpDownRadiusDsgntdPnts_ValueChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetRadiusForDsgPnts ( ( double ) nmrcUpDownRadiusDsgntdPnts.Value );
		}

		private void nmrcUpDownTime_ValueChanged ( object sender, EventArgs e )
		{
			if ( _hasToBeCalculate )
				InitHolding.ConvRacetrackModel.SetTime ( ( double ) nmrcUpDownTime.Value );
		}

		private void nmrcUpDownOverheadRadial_ValueChanged ( object sender, EventArgs e )
		{
			InitHolding.ConvRacetrackModel.SetOverheadDirection ( ( double ) ( ( NumericUpDown ) sender ).Value );
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
				GlobalParams.AranEnvironment.AranUI.SetPreviousTool ( );
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
			// Check whether is called via changing value by user 
			if ( !_calculatedIntersectingRadial )
			{
				bool tmp = _calledByPointPickerControl;
				_calledByPointPickerControl = false;
				InitHolding.ConvRacetrackModel.SetIntersectingVorRadial ( ( double ) nmrcUpDwnIntersectignVorRadial.Value );
				_calledByPointPickerControl = tmp;
			}
		}

		#endregion

		public List<DesignatedPoint> DesignatePointList
		{
			get
			{
				return _designatePointList;
			}

			set
			{
				_designatePointList = value;
				if ( _designatePointList == null )
					return;
				int index = _designatePointList.FindIndex ( dsgPnt => dsgPnt.Designator == cmbBxDsgntdPnts.Text );
				if ( index == -1 )
					index = 0;

				cmbBxDsgntdPnts.Items.Clear ( );
				string dsgName = "";
				foreach ( var item in _designatePointList )
				{
					if ( item.Designator != null )
						dsgName = item.Designator;
					else if ( item.Name != null )
						dsgName = item.Name;
					else
						dsgName = "";
					cmbBxDsgntdPnts.Items.Add (dsgName);
				}
				cmbBxDsgntdPnts.SelectedIndex = index;
			}
		}

		private void InitializeParamsFor2ndTab ( )
		{
			if ( cmbBxAircraftCategory.Items.Count > 0 )
				return;

			InitHolding.ConvRacetrackModel.AddIASIntervalChangedEvent ( OnIASIntervalChanged );
			InitHolding.ConvRacetrackModel.AddCategoryListChangedEvent ( OnAircraftCategoryListChanged );
			InitHolding.ConvRacetrackModel.AddTimeChangedEvent ( OnTimeChanged );
			InitHolding.ConvRacetrackModel.AddLimitDistIntervalChangedEvent ( OnLimitDistIntervalChanged );
			InitHolding.ConvRacetrackModel.AddLimDistChangedEvent ( OnLimDistChanged );

			InitHolding.ConvRacetrackModel.GetCategoryList ( );

			radBtnToward.Checked = true;
			radBtnRightDir.Checked = true;
		}

		private void SetLimDistCntrlsVisibility ( bool visible )
		{
			lblLimDist.Visible = visible;
			nmrcUpDownLimDist.Visible = visible;
			lblLimDistUnit.Visible = visible;
		}

		private void ApplyInterfaceLanguage ( )
		{
			Resources.Culture = Thread.CurrentThread.CurrentUICulture;

			lblNavRadiusUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
			lblAltitudeUnit.Text = GlobalParams.UnitConverter.HeightUnit;
			lblDistanceUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
			lblRadiusDsgntdPntUnit.Text = GlobalParams.UnitConverter.DistanceUnit;

			lblPermSpeedUnit.Text = GlobalParams.UnitConverter.SpeedUnit;
			lblIASUnit.Text = GlobalParams.UnitConverter.SpeedUnit;
			lblLimDistUnit.Text = GlobalParams.UnitConverter.DistanceUnit;
		}

		private void ChangePointPickerCoords ( )
		{
			if ( !_calledByPointPickerControl )
				return;
			Point pntGeo = new Point ( pointPicker1.Longitude, pointPicker1.Latitude );
			Point pntPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( pntGeo );

			InitHolding.ConvRacetrackModel.SetDesignatedPoint ( pntPrj.X, pntPrj.Y );
		}

		private void ChangeLierTxtBxsAppearence ( bool toFront, bool lierTxtBxRadiusForDsgPnt_BringtToFront = true )
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
					if ( lierTxtBxRadiusForDsgPnt_BringtToFront )
						lierTxtBxRadiusForDsgPnt.BringToFront ( );
					cmbBxDsgntdPnts.Enabled = false;
				}
				btnNext.Enabled = !radBtnVorDme.Checked;
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
					if ( lierTxtBxRadiusForDsgPnt_BringtToFront )
						lierTxtBxRadiusForDsgPnt.SendToBack ( );
				}
				btnNext.Enabled = true;
			}
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
			pnlCreateDsgPnt.Visible = !isChosenPntTypeSelect;

			nmrcUpDwnIntersectignVorRadial.Enabled = ( radBtnVorVor.Checked && !isChosenPntTypeSelect );
			//nmrcUpDownDistance.Enabled = !isPntTypeSelect;
			//nmrcUpDownRadial.Enabled = !isPntTypeSelect;

			//if ( isPntTypeSelect )
			//{
			//lierTxtBxRadiusForDsgPnt.SendToBack ( );
			//lierTxtBxDsgPnt.SendToBack ( );

			//pnlCreateDsgPnt.SendToBack ( );
			//pnlSelectDsgPnt.BringToFront ( );
			//}
			//else
			//{
			//pnlSelectDsgPnt.SendToBack ( );
			//pnlCreateDsgPnt.BringToFront ( );

			//lierTxtBxNomDist.SendToBack ( );
			//lierTxtBxRadial.SendToBack ( );
			//}

			//pointPicker1.Enabled = isEnabled;
			//pnlSelectDsgPnt.Enabled = !isEnabled;
			//LblDistance.Enabled = isEnabled;
			//LblRadial.Enabled = isEnabled;
		}

		private List<Navaid> NavaidList
		{
			get
			{
				return _navaidList;
			}

			set
			{
				_navaidList = value;
				btnNext.Enabled = ( _navaidList.Count > 0 );
			}
		}

		private void btnReport_Click ( object sender, EventArgs e )
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				btnSave.Enabled = InitHolding.ConvRacetrackModel.CreateReport ( );

				if ( InitHolding.FrmReport == null )
					InitHolding.FrmReport = new FormReport ( );
				InitHolding.FrmReport.UptadeReport ( InitHolding.ConvRacetrackModel.Report, InitHolding.ConvRacetrackModel.Altitude );

				if ( InitHolding.FrmReport.Closed )
					InitHolding.FrmReport.ShowDialog ( this );
			}
			catch ( Exception ex )
			{
				MessageBox.Show ( ex.ToString ( ) );
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void btnSave_Click ( object sender, EventArgs e )
		{
            //if (!InitHolding.ConvRacetrackModel.CalculatedReport) 
            //{

            //}
            InitHolding.ConvRacetrackModel.Save();
        }

		private Guid _navaidID, _designatedPointID;
		private List<Navaid> _navaidList;
		private List<DesignatedPoint> _designatePointList;
		private bool _firstTimeProcTypeChanged, _hasToBeCalculate, _pointPickerClicked, _valueChangedFromOtherComponent, _calledByPointPickerControl, _calculatedIntersectingRadial;
	}
}