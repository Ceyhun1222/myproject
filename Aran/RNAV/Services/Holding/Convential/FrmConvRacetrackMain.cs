using System;
using System.Windows.Forms;

using Delib.Classes.Codes;
using ARAN.Common;
using ARAN.GeometryClasses;
using ARAN.Contracts.UI;
using Delib.Classes.Features.Organisation;
using Delib.Classes.Features.Navaid;
using Delib.Classes.Features.AirportHeliport;
using System.Collections.Generic;

namespace Holding.Convential
{
    public partial class FrmConvRacetrackMain : Form
    {
        public FrmConvRacetrackMain ( ) 
        {
            InitializeComponent ();
            //vordme.Output += TimeChanged;
        }

        private void FrmConvRacetrackMain_Load ( object sender, EventArgs e ) 
        {
            tabControl1.Visible = false;
            PnlPntSelection.Parent = this;
            PnlParams.Parent = this;
            PnlPntSelection.Location = new System.Drawing.Point ( 0, 0 );
            PnlParams.Location = new System.Drawing.Point ( 0, 0 );
            PnlParams.Visible = false;
            RadBtnVorDme.Tag = ProcedureTypeConv.ProcType_VORDME;
            RadBtnVorNdb.Tag = ProcedureTypeConv.ProcType_VOR_NDB;
            RadBtnVorVor.Tag = ProcedureTypeConv.ProcType_VORVOR;

            _radBtnsProcType [0] = RadBtnVorDme;
            _radBtnsProcType [1] = RadBtnVorNdb;
            _radBtnsProcType [2] = RadBtnVorVor;


            #region For Model

            NmrcUpDownRadiusNavaids.Value = 50;

            InitHolding.ConvRacetrackModel.AddOrgListCreatedEvent ( OnOrgListCreated );
            InitHolding.ConvRacetrackModel.AddAirdromeListChangedEvent ( OnAirportHeliportListChanged );
            InitHolding.ConvRacetrackModel.AddNavListChangedEvent ( OnNavaidListChanged );
            InitHolding.ConvRacetrackModel.AddDsgPointListChangedEvent ( OnDesignatePointListChanged );
            InitHolding.ConvRacetrackModel.AddDesignatedPointChangedEvent ( OnDesignatedPointChanged );
            InitHolding.ConvRacetrackModel.AddDirectionChangedEvent ( OnDirectionChanged );
            InitHolding.ConvRacetrackModel.AddNominalDistanceChangedEvent ( OnDistanceChanged );
            InitHolding.ConvRacetrackModel.GetOrgList ();

            RadBtnVorDme.Checked = true;
            NmrcUpDownAltitude.Value = 3050;
            RadBtnSelectDsgnPnt.Checked = true;
            NmrcUpDownRadiusDsgntdPnts.Value = 150;
            #endregion 

            SetLanguage ();
        }

        public void OnAircraftCategoryListChanged ( object sender, EventArgCategoryList argCtgList )
        {
            CmbBxAircraftCategory.DataSource = argCtgList.CategoryList;
        }

        public void OnIASIntervalChanged ( object sender, EventArgIASInterval argIAS )
        {
            NmrcUpDownIAS.Minimum = ( decimal ) argIAS.Min;
            NmrcUpDownIAS.Maximum = ( decimal ) argIAS.Max;
            TxtBxAllowableSpeed.Text = argIAS.Min.ToString () + "/"+argIAS.Max.ToString ();
        }

        public void OnTimeChanged ( object sender, EventArgTime argTime )
        {
            NmrcUpDownTime.Value = (decimal) argTime.Time;
        }

        public void OnLimitingDistanceChanged ( object sender, EventArgLimitingDistance argLimDist )
        {
            NmrcUpDownLimDist.Minimum = ( decimal ) argLimDist.Min;
            NmrcUpDownLimDist.Maximum = ( decimal ) argLimDist.Max;
        }

        public void OnOrgListCreated(object sender, EventArgOrgList arg)
        {
            CmbBxOrganisation.DisplayMember = "designator";
            CmbBxOrganisation.DataSource = arg.OrganisationList;
            if (arg.OrganisationList == null)
            {
                MessageBox.Show ( "There is not organisation !!!" );
            }
        }

        public void OnAirportHeliportListChanged ( object sender, EventArgAirportList arg ) 
        {
            CmbBxAirport.DisplayMember = "designator";
            CmbBxAirport.DataSource = arg.AirportList;
            if ( arg.AirportList == null)
            {
                CmbBxAirport.Enabled = false;
                LierTxtBxRadiusForNav.BringToFront ();
                NmrcUpDownRadiusNavaids.Enabled = false;
            }
            else if (! CmbBxAirport.Enabled )
            {
                CmbBxAirport.Enabled = true;
                LierTxtBxRadiusForNav.SendToBack ();
                NmrcUpDownRadiusNavaids.Enabled = true;
            }
        }

        public void OnNavaidListChanged ( object sender, EventArgNavaidList arg ) 
        {
            if ( RadBtnVorDme.Checked)
            {
                CmbBxNavaids.DisplayMember = "designator";
                CmbBxNavaids.DataSource  = arg.NavaidList;
                if (arg.NavaidList == null)
                {
                    CmbBxNavaids.Enabled = false;
                    GrpBxSetDsgPnt.Enabled = false;
                    LierTxtBxRadiusForDsgPnt.BringToFront ();
                    NmrcUpDownRadiusDsgntdPnts.Enabled = false;
                    if ( RadBtnCreateDsgnPnt.Checked )
                    {
                        LierTxtBxDsgPnt.BringToFront ();
                        CmbBxDsgntdPnts.Enabled = false;
                    }
                }
                else if (! CmbBxNavaids.Enabled)
                {
                    CmbBxNavaids.Enabled = true;
                    GrpBxSetDsgPnt.Enabled = true;
                    LierTxtBxRadiusForDsgPnt.SendToBack ();
                    NmrcUpDownRadiusDsgntdPnts.Enabled = true;
                    if ( RadBtnCreateDsgnPnt.Checked && CmbBxDsgntdPnts.Items.Count > 0)
                    {
                        LierTxtBxDsgPnt.SendToBack ();
                        CmbBxDsgntdPnts.Enabled = true;
                    }
                }
            }
            else if ( RadBtnVorNdb.Checked)
            {
                CmbBxNavaids.DisplayMember = "designator";
                CmbBxNavaids.DataSource  = arg.NavaidList;
                if ( arg.NavaidList == null )
                {
                    CmbBxNavaids.Enabled = false;
                }
                else if ( !CmbBxNavaids.Enabled )
                {
                    CmbBxNavaids.Enabled = true;
                }
            }
            else
            {

            }
        }

        public void DrawNavaid ( List<NavaidPntPrj> navaidPnts )
        {
            if ( RadBtnVorDme.Checked )
            {
                GlobalParams.UI.SafeDeleteGraphic ( ref _handleForNav1 );
                GlobalParams.UI.SafeDeleteGraphic ( ref _handleForNav2 );
                if ( navaidPnts[0].Value != null )
                {
                    _handleForNav1 = GlobalParams.UI.DrawPoint ( navaidPnts[0].Value, 0);
                }
                if ( navaidPnts[1].Value != null )
                {
                    _handleForNav2 = GlobalParams.UI.DrawPoint ( navaidPnts[1].Value, 0);
                }
                if ( _hasShownPnlParams && _applyBtnClicked )
                {
                    if ( _appliedNavaidPnts[0] == navaidPnts[0] || _appliedNavaidPnts[1]== navaidPnts[1] )                       
                    {
                        AppliedValueChanged ( true );
                    }
                    else
                    {
                        AppliedValueChanged ( false );
                    }
                }
                else
                {
                    _appliedNavaidPnts = new List<NavaidPntPrj> ();
                    foreach ( NavaidPntPrj navPntPrj in navaidPnts)
                    {
                        _appliedNavaidPnts.Add(navPntPrj.Clone());
                        
                    }
                }
            }
            else if ( RadBtnVorNdb.Checked )
            {
                GlobalParams.UI.SafeDeleteGraphic ( ref _handleForNav1 );
                if ( navaidPnts [0].Value != null )
                {
                    _handleForNav1 = GlobalParams.UI.DrawPoint ( navaidPnts [0].Value, 0 );
                }
                if ( _hasShownPnlParams && _applyBtnClicked )
                {
                    if ( _appliedNavaidPnts [0] == navaidPnts [0])
                    {
                        AppliedValueChanged ( true );
                    }
                    else
                    {
                        AppliedValueChanged ( false );
                    }
                }
                else
                {
                    _appliedNavaidPnts = new List<NavaidPntPrj> ();
                    foreach ( NavaidPntPrj navPntPrj in navaidPnts)
                    {
                        _appliedNavaidPnts.Add ( navPntPrj.Clone () );
                    }
                }
            }
            else
            {

            }
        }

        public void OnDesignatePointListChanged ( object sender, EventArgDsgPntList arg )
        {
            CmbBxDsgntdPnts.DisplayMember = "designator";
            CmbBxDsgntdPnts.DataSource = arg.DsgPntList;
            if ( arg.DsgPntList == null )
            {
                LierTxtBxDsgPnt.BringToFront ();
                CmbBxDsgntdPnts.Enabled = false;
            }
            else if ( !CmbBxDsgntdPnts.Enabled )
            {
                LierTxtBxRadiusForDsgPnt.SendToBack ();
                NmrcUpDownRadiusDsgntdPnts.Enabled = true;
                LierTxtBxDsgPnt.SendToBack ();
                CmbBxDsgntdPnts.Enabled = true;
                BtnOK.Enabled = true;
            }
        }

        public void OnDirectionChanged ( object sender, EventArgDirection argDir )
        {
            NmrcUpDownRadial.Value = ( decimal ) argDir.DirectionForGUI;
        }

        public void OnDistanceChanged ( object sender, EventArgNomDistance argDist )
        {
            NmrcUpDownDistance.Value = ( decimal ) argDist.NomDistanceForGUI;
        }

        public void OnDesignatedPointChanged ( object sender, EventArgDsgPnt argDsgPnt )
        {
            if ( argDsgPnt.DsgPntPrj != null )
            {
                _dsgPnt = argDsgPnt.DsgPntPrj;
                LierTxtBxNomDist.SendToBack();
                //NmrcUpDownDistance.Enabled = true;
                LierTxtBxRadial.SendToBack ();
                //NmrcUpDownRadial.Enabled = true;
                GlobalParams.UI.SafeDeleteGraphic ( ref _handleForDsgPnt );
                _handleForDsgPnt = GlobalParams.UI.DrawPointWithText ( _dsgPnt, 255, "DsgPnt" );
                if ( _hasShownPnlParams && _applyBtnClicked)
                {
                        AppliedValueChanged ( ( _appliedDsgPnt.X == _dsgPnt.X && _appliedDsgPnt.Y == _dsgPnt.Y ) );
                }
                else
                {
                    _appliedDsgPnt.Assign ( _dsgPnt );
                }
                BtnOK.Enabled =true;
            }
            else
            {

                LierTxtBxNomDist.BringToFront ();
                NmrcUpDownDistance.Enabled = false;
                LierTxtBxRadial.BringToFront ();
                NmrcUpDownRadial.Enabled = false;
                GlobalParams.UI.SafeDeleteGraphic ( ref _handleForDsgPnt );
                BtnOK.Enabled = false;
            }
        }

        private void SetLanguage ( ) 
        {
            LblDistanceUnit.Text = InitHolding.DistanceConverter.Unit;
            LblRadiusDsgntdPntUnit.Text = InitHolding.DistanceConverter.Unit;
            LblRadiusUnit.Text = InitHolding.DistanceConverter.Unit;
        }

        private void RadBtnVorDme_CheckedChanged ( object sender, EventArgs e )        
        {
            if ((sender as RadioButton).Checked)
            {
                InitHolding.ConvRacetrackModel.SetProcType ( ( ProcedureTypeConv ) Enum.Parse ( typeof ( ProcedureTypeConv ), (sender as RadioButton).Tag.ToString () ) );
                if ( RadBtnVorDme.Checked )
                {
                    GrpBxSetDsgPnt.Enabled = true;
                    GrpBxEntryDir.Enabled = true;
                    NmrcUpDownTime.Enabled = false;

                    NmrcUpDownRadial.Enabled = RadBtnCreateDsgnPnt.Checked;
                    NmrcUpDownDistance.Enabled = RadBtnCreateDsgnPnt.Checked;
                    if ( !_isFirstTimeProcTypeChanged )
                    {
                        GlobalParams.UI.DrawPoint ( _dsgPnt, 0 );
                        if (_hasShownPnlParams)
                            NmrcUpDownTime.Value = (decimal) _calculatedTime;                        
                        NmrcUpDownRadial.Value = (decimal) _calculatedRadial;
                        _hasToBeCalculate = true;
                    }
                    _isFirstTimeProcTypeChanged = false;
                }
                else if ( RadBtnVorNdb.Checked )
                {
                    GlobalParams.UI.SafeDeleteGraphic ( ref _handleForDsgPnt );
                    GrpBxSetDsgPnt.Enabled = false;
                    GrpBxEntryDir.Enabled = false;
                    if ( !_isFirstTimeProcTypeChanged )
                    {
                        if (_hasShownPnlParams)
                            _calculatedTime = ( double ) NmrcUpDownTime.Value;
                        _calculatedRadial = ( double ) NmrcUpDownRadial.Value;
                        _hasToBeCalculate = false;
                    }
                    NmrcUpDownRadial.Enabled = true;
                    NmrcUpDownTime.Enabled = true;
                    if ( NmrcUpDownDistance.Enabled )
                        NmrcUpDownDistance.Enabled = false;
                    _isFirstTimeProcTypeChanged = false;
                }
            }
        }

        private void CmbBxOrganisation_SelectedIndexChanged ( object sender, EventArgs e )
        {            
            if ( CmbBxOrganisation.SelectedItem == null )
            {
                _organisationID = "";
                InitHolding.ConvRacetrackModel.SetOrganisation ( -1 );
            }
            else if ( _organisationID  != (CmbBxOrganisation.SelectedItem as OrganisationAuthority).identifier)
            {
                _organisationID = ( CmbBxOrganisation.SelectedItem as OrganisationAuthority ).identifier;
                InitHolding.ConvRacetrackModel.SetOrganisation ( CmbBxOrganisation.SelectedIndex);
            }
        }

        private void CmbBxAirport_SelectedIndexChanged ( object sender, EventArgs e )
        {
            if ( CmbBxAirport.SelectedItem == null )
            {
                _airportID =  "";
                InitHolding.ConvRacetrackModel.SetAirport ( -1 );
            }
            else if ( _airportID  != (CmbBxAirport.SelectedItem as AirportHeliport).identifier)
            {
                _airportID =  ( CmbBxAirport.SelectedItem as AirportHeliport ).identifier;
                InitHolding.ConvRacetrackModel.SetAirport ( CmbBxAirport.SelectedIndex );
            }
        }

        private void NmrcUpDownRadiusNavaids_ValueChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetRadiusForNavaids ( ( double ) NmrcUpDownRadiusNavaids.Value );            
        }

        private void CmbBxNavaids_SelectedIndexChanged ( object sender, EventArgs e )
        {
            if ( CmbBxNavaids.SelectedItem == null )
            {
                _navaidID = "";
                DrawNavaid ( InitHolding.ConvRacetrackModel.SetNavaid ( -1 ) );
            }
            else if ( _navaidID != (CmbBxNavaids.SelectedItem as Navaid).identifier)
            {
                _navaidID = ( CmbBxNavaids.SelectedItem as Navaid ).identifier;
                DrawNavaid ( InitHolding.ConvRacetrackModel.SetNavaid ( CmbBxNavaids.SelectedIndex ) );
            }
        }

        private void NmrcUpDownDistance_ValueChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetNominalDistance ( ( double ) NmrcUpDownDistance.Value );
        }

        private void NmrcUpDownRadial_ValueChanged ( object sender, EventArgs e )
        {
            if (_hasToBeCalculate)
                InitHolding.ConvRacetrackModel.SetDirection ( ( double ) NmrcUpDownRadial.Value );
        }

        private void NmrcUpDownRadiusDsgntdPnts_ValueChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetRadiusForDsgPnts ( ( double ) NmrcUpDownRadiusDsgntdPnts.Value );
        }

        private void CmbBxDsgntdPnts_SelectedIndexChanged ( object sender, EventArgs e )
        {
            if ( CmbBxDsgntdPnts.SelectedItem == null )
            {
                _designatedPointID = "";
                InitHolding.ConvRacetrackModel.SetDesignatedPoint ( -1);
            }
            else if ( _designatedPointID != (CmbBxDsgntdPnts.SelectedItem as DesignatedPoint).identifier)
            {
                _designatedPointID = ( CmbBxDsgntdPnts.SelectedItem as DesignatedPoint ).identifier;
                InitHolding.ConvRacetrackModel.SetDesignatedPoint ( CmbBxDsgntdPnts.SelectedIndex );
            }
        }

        private void RadBtnSelectDsgnPnt_CheckedChanged ( object sender, EventArgs e )
        {
            ChangeEnablesRadAndDistCompononts ( ! RadBtnSelectDsgnPnt.Checked );
            pointPicker1.Enabled = ! RadBtnSelectDsgnPnt.Checked;
            PnlDsgPntSlc.Enabled = RadBtnSelectDsgnPnt.Checked;

            if ( RadBtnCreateDsgnPnt.Checked )
            {
                InitHolding.ConvRacetrackModel.SetPointChoice ( PointChoice.PntChoice_Create );
                LierTxtBxNomDist.SendToBack ();
                LierTxtBxRadial.SendToBack ();
            }
            else
            {
                InitHolding.ConvRacetrackModel.SetPointChoice ( PointChoice.PntChoice_Select );
            }
        }

        private void ChangeEnablesRadAndDistCompononts (bool  isEnabled)
        {
            NmrcUpDownDistance.Enabled = isEnabled;
            NmrcUpDownRadial.Enabled = isEnabled;
            //LblDistance.Enabled = isEnabled;
            //LblRadial.Enabled = isEnabled;
        }

        private void BtnOK_Click ( object sender, EventArgs e )
        {
            PnlPntSelection.Visible = false;
            PnlParams.Visible = true;
            InitializeParamsForSecndPnl ();
        }

        private void InitializeParamsForSecndPnl ( )
        {
            if ( !_hasShownPnlParams )
            {
                InitHolding.ConvRacetrackModel.AddIASIntervalChangedEvent ( OnIASIntervalChanged );
                InitHolding.ConvRacetrackModel.AddCategoryListChangedEvent ( OnAircraftCategoryListChanged );
                InitHolding.ConvRacetrackModel.AddTimeChangedEvent ( OnTimeChanged );
                InitHolding.ConvRacetrackModel.AddLimitingDistanceChangedEvent ( OnLimitingDistanceChanged );

                InitHolding.ConvRacetrackModel.GetCategoryList ();

                RadBtnToward.Checked = true;
                RadBtnRightDir.Checked = true;
                _hasShownPnlParams = true;
            }
        }

        private void FrmConvInitial_FormClosing ( object sender, FormClosingEventArgs e )
        {
            GlobalParams.UI.SafeDeleteGraphic ( ref _handleForNav1 );
            GlobalParams.UI.SafeDeleteGraphic ( ref _handleForNav2 );
            GlobalParams.UI.SafeDeleteGraphic ( ref _handleForDsgPnt );
            GlobalParams.UI.SafeDeleteGraphic ( ref _handleBasicArea );
            GlobalParams.UI.SafeDeleteGraphic ( ref _handlePrtctSector1 );
            GlobalParams.UI.SafeDeleteGraphic ( ref _handlePrtctSector2 );
            GlobalParams.UI.SafeDeleteGraphic ( ref _handlePrtctRecipDir2SecPnt );
        }

        private void RadBtnRightDir_CheckedChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetSideDirection ( RadBtnRightDir.Checked );
            if ( _applyBtnClicked )
                AppliedValueChanged ( _appliedIsRight == RadBtnRightDir.Checked );
        }

        private void RadBtnToward_CheckedChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetEntryDirection ( RadBtnToward.Checked );
            if ( _applyBtnClicked )
                AppliedValueChanged ( _appliedIsToward == RadBtnToward.Checked );
        }

        private void ChckBxProtectSector1_CheckedChanged ( object sender, EventArgs e )
        {
            if ( !_applyBtnClicked )
                return;

            DrawProtectionSector1 ();
        }

        private void ChckBxProtectSector2_CheckedChanged ( object sender, EventArgs e )
        {
            if ( !_applyBtnClicked )
                return;

            DrawProtectionSector2 ();
        }

        private void ChckBxRecipDirEntSecondary_CheckedChanged ( object sender, EventArgs e )
        {
            if ( !_applyBtnClicked )
                return;
            DrawRecipDir2SecPnt ();
        }

        private void CmbBxAircraftCategory_SelectedIndexChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetAircraftCategory ( CmbBxAircraftCategory.SelectedIndex );
        }

        private void NmrcUpDownIAS_ValueChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetIAS ( ( double ) NmrcUpDownIAS.Value );
            if ( _applyBtnClicked )
                AppliedValueChanged ( _appliedIAS == ( double ) NmrcUpDownIAS.Value );
        }

        private void NmrcUpDownAltitude_ValueChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetAltitude ( ( double ) NmrcUpDownAltitude.Value );
            if ( _applyBtnClicked )
                AppliedValueChanged ( _appliedAltitude == ( double ) NmrcUpDownAltitude.Value );
        }

        private void NmrcUpDownLimDist_ValueChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetLimitingDistance ( ( double ) NmrcUpDownLimDist.Value );
            if ( _applyBtnClicked )
                AppliedValueChanged ( _appliedLimitingDistance == ( double ) NmrcUpDownLimDist.Value );
        }

        private void BtnApply_Click ( object sender, EventArgs e )
        {
            _basicArea = InitHolding.ConvRacetrackModel.ConstructBasicArea ();
             
            GlobalParams.UI.SafeDeleteGraphic ( ref _handleBasicArea );
            
            if ( _basicArea!= null )
            {
                _handleBasicArea = GlobalParams.UI.DrawPolygon ( _basicArea, 0, eFillStyle.sfsCross );
            }

            _hasCalculatedPrtctSector1 = false;
            _hasCalculatedPrtctSector2 = false;
            _hasCalculatedPrtcRecipDir2SecPnt = false;
            DrawProtectionSector1 ();
            DrawProtectionSector2 ();
            DrawRecipDir2SecPnt ();

            ChckBxProtectSector1.Enabled = true;
            ChckBxProtectSector2.Enabled = true;
            ChckBxRecipDirEntSecondary.Enabled = true;
            _appliedIAS = ( double ) NmrcUpDownIAS.Value;
            _appliedAltitude = ( double ) NmrcUpDownAltitude.Value;
            _appliedLimitingDistance = ( double ) NmrcUpDownLimDist.Value;
            _appliedIsRight = RadBtnRightDir.Checked;
            _appliedIsToward = RadBtnToward.Checked;
            _applyBtnClicked = true;
            for ( int i = 0; i <= _radBtnsProcType.Length - 1; i++ )
            {
                if ( _radBtnsProcType [i].Checked )
                {
                    _appliedProcType = ( ProcedureTypeConv ) Enum.Parse ( typeof ( ProcedureTypeConv ), _radBtnsProcType [i].Tag.ToString () );
                    break;
                }
            }
            _numberChangedAppliedValues = 0;
            BtnApply.Enabled = false;
        }

        private void PrevBtn_Click ( object sender, EventArgs e )
        {
            PnlPntSelection.Visible = true;
            PnlParams.Visible = false;
        }

        private void NmrcUpDownTime_ValueChanged ( object sender, EventArgs e )
        {
            if ( _hasToBeCalculate )
            {
                InitHolding.ConvRacetrackModel.SetTime ( (double) NmrcUpDownTime.Value );
            }
        }

        private void ChckBxWithLimRadial_CheckedChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetWithLimitingRadial ( ChckBxWithLimRadial.Checked );
        }

        private void AppliedValueChanged ( bool isEqual )
        {
            if ( ! isEqual )
            {
                BtnApply.Enabled = true;
                _numberChangedAppliedValues++;
                ChckBxProtectSector1.Enabled = false;
                ChckBxProtectSector2.Enabled = false;
                ChckBxRecipDirEntSecondary.Enabled = false;
            }
            else
            {
                _numberChangedAppliedValues--;
                BtnApply.Enabled = (_numberChangedAppliedValues != 0);
                ChckBxProtectSector1.Enabled = !BtnApply.Enabled;
                ChckBxProtectSector2.Enabled = ! BtnApply.Enabled;
                ChckBxRecipDirEntSecondary.Enabled = !BtnApply.Enabled;
            }
        }

        private void DrawProtectionSector1 ( )
        {
            GlobalParams.UI.SafeDeleteGraphic ( ref _handlePrtctSector1 );

            if ( ChckBxProtectSector1.Checked )
            {
                if ( !_hasCalculatedPrtctSector1 )
                {
                    _protectionSector1 = InitHolding.ConvRacetrackModel.ConstructProtectionSector1 ();
                    _hasCalculatedPrtctSector1 = true;
                }
                if ( _protectionSector1 != null )
                {
                    _handlePrtctSector1 = GlobalParams.UI.DrawPolygon ( _protectionSector1, 255, eFillStyle.sfsHorizontal );
                }
            }
        }

        private void DrawProtectionSector2 ( )
        {
            GlobalParams.UI.SafeDeleteGraphic ( ref _handlePrtctSector2 );

            if ( ChckBxProtectSector2.Checked )
            {
                if ( !_hasCalculatedPrtctSector2 )
                {
                    _protectionSector2 = InitHolding.ConvRacetrackModel.ConstructProtectionSector2 ();
                    _hasCalculatedPrtctSector2 = true;
                }
                if ( _protectionSector2 != null )
                {
                    _handlePrtctSector2 = GlobalParams.UI.DrawPolygon ( _protectionSector2, 255*255, eFillStyle.sfsVertical );
                }
            }
        }

        private void DrawRecipDir2SecPnt ( )
        {
            TxtBxAngRE.Text = "";
            
            if ( ChckBxRecipDirEntSecondary.Checked )
            {
                if ( !_hasCalculatedPrtcRecipDir2SecPnt )
                {
                    _protectionRecipDir2SecPnt = InitHolding.ConvRacetrackModel.ConstructRecipDir2SecPnt ( out _angleRE );
                    _hasCalculatedPrtcRecipDir2SecPnt = true;
                }                    
                TxtBxAngRE.Text = _angleRE.ToString ();
            }
        }
        
        private RadioButton[] _radBtnsProcType = new RadioButton [3];
        private string _organisationID, _airportID, _navaidID, _designatedPointID;
        private Point _dsgPnt;
        private int _handleForDsgPnt;
        private int _handleForNav1, _handleForNav2;
        private int _numberChangedAppliedValues = 0;
        //private bool _isFirstTimeForDsgPnt = true, _isFirstTimeForNavaid = true;
        private bool _hasCalculatedPrtctSector1, _hasCalculatedPrtctSector2, _hasCalculatedPrtcRecipDir2SecPnt;
        private bool _hasShownPnlParams = false;
        private double _appliedIAS = 0;
        private double _appliedAltitude = 0;
        private double _appliedLimitingDistance;
        private Point _appliedDsgPnt = new Point ();
        private List<NavaidPntPrj> _appliedNavaidPnts = new List<NavaidPntPrj> ();
        private int _handleBasicArea, _handlePrtctSector1, _handlePrtctSector2, _handlePrtctRecipDir2SecPnt;
        //private bool _isFirstTimeForBasicArea = true, _isFirstTimePrtctSector1 = true, _isFirstTimePrtctSector2 = true, _isFirstTimePrtctRecipDir2SecPnt = true;
        private bool _applyBtnClicked = false;
        private bool _appliedIsRight = false;
        private bool _appliedIsToward = false;
        private ProcedureTypeConv _appliedProcType = ProcedureTypeConv.ProcType_NONE;
        private Polygon _basicArea, _protectionSector1, _protectionSector2, _protectionRecipDir2SecPnt;
        private int _angleRE;
        private bool _hasToBeCalculate = true;
        private double _calculatedTime, _calculatedRadial;
        private bool _isFirstTimeProcTypeChanged = true;
    } 
}
