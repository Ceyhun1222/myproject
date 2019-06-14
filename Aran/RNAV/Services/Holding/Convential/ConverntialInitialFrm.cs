using System;
using System.Windows.Forms;

using Delib.Classes.Codes;
using ARAN.Common;

namespace Holding.Convential
{
    public partial class ConverntialInitialFrm : Form
    {
        private VOR_DME vordme = new VOR_DME();
        private PointType pntType;
        private NavaidServiceType[] navaidServices;
        private int _organisationIndex =-1, _airportIndex = -1, _navaidIndex = -1, _designatedPointIndex = -1;

        public ConverntialInitialFrm ( ) 
        {
            InitializeComponent ();
            //vordme.Output += TimeChanged;
        }

        private void TimeChanged ( object sender, EventArgTmp argTmp )
        {
            TxtBxTime.Text = argTmp.Value;
        }

        private void ConverntialInitialFrm_Load ( object sender, EventArgs e ) 
        {

            #region For Model

            NmrcUpDownRadiusNavaids.Value = 50;

            InitHolding.ConvRacetrackModel.AddOrgListCreatedEvent ( OnOrgListCreated );
            InitHolding.ConvRacetrackModel.AddAirdromeListChangedEvent ( OnAirportHeliportListChanged );
            InitHolding.ConvRacetrackModel.AddNavListChangedEvent ( OnNavaidListChanged );
            InitHolding.ConvRacetrackModel.AddDsgPointListChangedEvent ( OnDesignatePointListChanged );
            InitHolding.ConvRacetrackModel.AddDirectionChangedEvent ( OnDirectionChanged );
            InitHolding.ConvRacetrackModel.AddNominalDistanceChangedEvent ( OnDistanceChanged );
            InitHolding.ConvRacetrackModel.AddIASChangedEvent ( OnIASChanged );
            InitHolding.ConvRacetrackModel.AddCategoryListChangedEvent ( OnAircraftCategoryListChanged );
            InitHolding.ConvRacetrackModel.AddTimeChangedEvent ( OnTimeChanged );
            InitHolding.ConvRacetrackModel.AddLimitingDistanceChangedEvent ( OnLimitingDistanceChanged );
            InitHolding.ConvRacetrackModel.GetOrgList ();
            InitHolding.ConvRacetrackModel.GetCategoryList ();

            RadBtnVorDme.Checked = true;

            RadBtnSelectDsgnPnt.Checked = true;
            NmrcUpDownRadiusDsgntdPnts.Value = 150;
            NmrcUpDownAltitude.Value = 3050;
            #endregion 


            //RadBtnVorDme.Checked = true;
            //rdbRight.Checked = true;
            //NmrcUpDownAltitude.Value = 3050;
            //NmrcUpDownIAS.Value = 405;


            //vordme.ReadOrganisation ( CmbBxOrganisation );
            //NmrcUpDownRadiusNavaids.Value = 50;
            //RadBtnSelectDsgnPnt.Checked = true;
            
            

            //NmrcUpDownLimDist.Value = ( decimal ) vordme.LimitingDistanceInPrj;

            SetLanguage ();
        }

        public void OnOrgListCreated(object sender, EventArgOrgList arg)
        {
            CmbBxOrganisation.DisplayMember = "designator";
            CmbBxOrganisation.DataSource = arg.OrganisationList;
            if (arg.OrganisationList.Count == 0)
            {
                CmbBxAirport.Enabled = false;
                LierTxtBxRadiusForNav.BringToFront ();
                NmrcUpDownRadiusNavaids.Enabled = false;
                CmbBxNavaids.Enabled = false;
                LierTxtBxRadiusForDsgPnt.BringToFront ();
                NmrcUpDownRadiusDsgntdPnts.Enabled = false;
                LierTxtBxDsgPnt.BringToFront ();
                CmbBxDsgntdPnts.Enabled = false;
                LierTxtBxNomDist.BringToFront ();
                //NmrcUpDownDistance.Enabled = false;
                LierTxtBxRadial.BringToFront ();
                //NmrcUpDownRadial.Enabled = false;
                BtnOK.Enabled = false;
                MessageBox.Show ( "There is not organisation !!!" );
            }            
        }

        public void OnAirportHeliportListChanged ( object sender, EventArgAirportList arg ) 
        {
            CmbBxAirport.DisplayMember = "designator";
            CmbBxAirport.DataSource = arg.AirportList;
            if ( arg.AirportList.Count == 0 )
            {
                LierTxtBxRadiusForNav.BringToFront ();
                NmrcUpDownRadiusNavaids.Enabled = false;
                CmbBxNavaids.Enabled = false;
                LierTxtBxRadiusForDsgPnt.BringToFront ();
                NmrcUpDownRadiusDsgntdPnts.Enabled = false;
                LierTxtBxDsgPnt.BringToFront ();
                CmbBxDsgntdPnts.Enabled = false;
                LierTxtBxNomDist.BringToFront ();
                //NmrcUpDownDistance.Enabled = false;
                LierTxtBxRadial.BringToFront ();
                //NmrcUpDownRadial.Enabled = false;
                BtnOK.Enabled = false;
            }
            else if (! NmrcUpDownRadiusNavaids.Enabled )
            {
                LierTxtBxRadiusForNav.SendToBack ();
                NmrcUpDownRadiusNavaids.Enabled = true;
                CmbBxNavaids.Enabled = true;
                LierTxtBxRadiusForDsgPnt.SendToBack ();
                NmrcUpDownRadiusDsgntdPnts.Enabled = true;
                LierTxtBxDsgPnt.SendToBack ();
                CmbBxDsgntdPnts.Enabled = true;
                LierTxtBxNomDist.SendToBack ();
                //NmrcUpDownDistance.Enabled = true;
                LierTxtBxRadial.SendToBack ();
                //NmrcUpDownRadial.Enabled = true;
                BtnOK.Enabled = true;
            }
        }

        public void OnNavaidListChanged ( object sender, EventArgNavaidList arg ) 
        {
            if ( RadBtnVorDme.Checked)
            {
                CmbBxNavaids.DisplayMember = "designator";
                CmbBxNavaids.DataSource  = arg.NavaidList;
                if (arg.NavaidList.Count == 0)
                {
                    LierTxtBxRadiusForDsgPnt.BringToFront ();
                    NmrcUpDownRadiusDsgntdPnts.Enabled = false;
                    LierTxtBxDsgPnt.BringToFront ();
                    CmbBxDsgntdPnts.Enabled = false;
                    LierTxtBxNomDist.BringToFront ();
                    //NmrcUpDownDistance.Enabled = false;
                    LierTxtBxRadial.BringToFront ();
                    //NmrcUpDownRadial.Enabled = false;
                    BtnOK.Enabled = false;
                }
                else if ( !NmrcUpDownRadiusDsgntdPnts.Enabled )
                {
                    LierTxtBxRadiusForDsgPnt.SendToBack ();
                    NmrcUpDownRadiusDsgntdPnts.Enabled = true;
                    LierTxtBxDsgPnt.SendToBack ();
                    CmbBxDsgntdPnts.Enabled = true;
                    LierTxtBxNomDist.SendToBack ();
                    //NmrcUpDownDistance.Enabled = true;
                    LierTxtBxRadial.SendToBack ();
                    //NmrcUpDownRadial.Enabled = true;
                    BtnOK.Enabled = true;
                }
            }
            else if ( RadBtnVorNdb.Checked)
            {
            }
            else
            {
            }
        }

        public void OnDesignatePointListChanged ( object sender, EventArgDsgPntList arg )
        {
            CmbBxDsgntdPnts.DisplayMember = "designator";
            CmbBxDsgntdPnts.DataSource = arg.DsgPntList;
            if ( arg.DsgPntList.Count == 0 )
            {
                LierTxtBxDsgPnt.BringToFront ();
                CmbBxDsgntdPnts.Enabled = false;
                LierTxtBxNomDist.BringToFront ();
                LierTxtBxRadial.BringToFront ();
                BtnOK.Enabled = false;
            }
            else if ( !CmbBxDsgntdPnts.Enabled )
            {
                LierTxtBxDsgPnt.SendToBack ();
                CmbBxDsgntdPnts.Enabled = true;
                LierTxtBxNomDist.SendToBack ();
                LierTxtBxRadial.SendToBack ();
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

        public void OnAircraftCategoryListChanged ( object sender, EventArgCategoryList argCtgList )
        {
            CmbBxAircraftCategory.DataSource = argCtgList.CategoryList;
        }

        public void OnIASChanged ( object sender, EventArgIAS argIAS )
        {
            //NmrcUpDownIAS.Minimum = ( decimal ) argIAS.Min;
            //NmrcUpDownIAS.Maximum = ( decimal ) argIAS.Max;
            //TxtBxAllowableSpeed.Text = argIAS.Min.ToString () + "/"+argIAS.Max.ToString ();
            NmrcUpDownIAS.Value = ( decimal ) argIAS.IAS;
        }

        public void OnTimeChanged ( object sender, EventArgTime argTime )
        {
            TxtBxTime.Text = argTime.Time.ToString ();
        }

        public void OnLimitingDistanceChanged ( object sender, EventArgLimitingDistance argLimDist )
        {
            NmrcUpDownLimDist.Minimum = ( decimal ) argLimDist.Min;
            NmrcUpDownLimDist.Maximum = ( decimal ) argLimDist.Max;
            NmrcUpDownLimDist.Value = ( decimal ) argLimDist.LimitingDistance;
        }

        private void SetLanguage ( )
        {
            LblDistanceUnit.Text = InitHolding.DistanceConverter [InitHolding.DistanceUnit].Unit;
            LblRadiusDsgntdPntUnit.Text = InitHolding.DistanceConverter [InitHolding.DistanceUnit].Unit;
            LblRadiusUnit.Text = InitHolding.DistanceConverter [InitHolding.DistanceUnit].Unit;
        }

        private void RadBtnVorDme_CheckedChanged ( object sender, EventArgs e )
        {
            if ( RadBtnVorDme.Checked )
            {
                // For Model
                InitHolding.ConvRacetrackModel.SetType ( ProcedureTypeConv.ProcType_VORDME );
                //navaidServices = new NavaidServiceType [] { NavaidServiceType.VOR_DME };
            }
            else if ( RadBtnVorNdb.Checked )
            {
                // For Model
                //InitHolding.ConvRacetrackModel.Proc.SetType ( ProcedureTypeConv.ProcType_VOR_NDB );
                navaidServices = new NavaidServiceType [] { NavaidServiceType.VOR, NavaidServiceType.VOR_DME, NavaidServiceType.VORTAC, 
                                                            NavaidServiceType.NDB, NavaidServiceType.NDB_DME,NavaidServiceType.NDB_MKR};
            }
            else
            {
                // For Model
                //InitHolding.ConvRacetrackModel.Proc.SetType ( ProcedureTypeConv.ProcType_VORVOR );
                
                navaidServices = new NavaidServiceType [] { NavaidServiceType.VOR, NavaidServiceType.VOR_DME, NavaidServiceType.VORTAC };
            }
        }

        private void CmbBxOrganisation_SelectedIndexChanged ( object sender, EventArgs e )
        {
            // For Model 
            InitHolding.ConvRacetrackModel.SetOrganisation ( CmbBxOrganisation.SelectedIndex );

            //if ( CmbBxOrganisation.SelectedIndex != _organisationIndex )
            //{
            //    _organisationIndex = CmbBxOrganisation.SelectedIndex;
            //    vordme.ReadAirports ( CmbBxAirport, CmbBxOrganisation.Items [_organisationIndex] as OrganisationAuthority );
            //}
        }

        private void CmbBxAirport_SelectedIndexChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetAirport ( CmbBxAirport.SelectedIndex );

            //if ( CmbBxAirport.SelectedIndex != _airportIndex )
            //{
            //    _airportIndex = CmbBxAirport.SelectedIndex;
            //    vordme.ReadNavaids ( CmbBxNavaids, CmbBxAirport.SelectedItem as AirportHeliport, ( double ) NmrcUpDownRadiusNavaids.Value, navaidServices );
            //}
        }

        private void NmrcUpDownRadiusNavaids_ValueChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetRadiusForNavaids ( ( double ) NmrcUpDownRadiusNavaids.Value );
            
            //vordme.ReadNavaids ( CmbBxNavaids, CmbBxAirport.SelectedItem as AirportHeliport, ( double ) NmrcUpDownRadiusNavaids.Value, navaidServices );
        }

        private void CmbBxNavaids_SelectedIndexChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetNavaid ( CmbBxNavaids.SelectedIndex );
            
            //if ( CmbBxNavaids.SelectedIndex != _navaidIndex )
            //{
            //    _navaidIndex = CmbBxNavaids.SelectedIndex;
            //    if ( pntType == PointType.Select )
            //    {
            //        vordme.ReadDesignatedPoints ( CmbBxDsgntdPnts, CmbBxNavaids.SelectedItem as Navaid, ( double ) NmrcUpDownRadiusDsgntdPnts.Value, NmrcUpDownDistance, NmrcUpDownRadial );
            //    }
            //}
        }

        private void NmrcUpDownAltitude_ValueChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetAltitude ( ( double ) NmrcUpDownAltitude.Value );

            //vordme.Altitude = ( double ) NmrcUpDownAltitude.Value;
        }

        private void NmrcUpDownDistance_ValueChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetNominalDistance ( ( double ) NmrcUpDownDistance.Value );

            //vordme.NominalDistanceInPrj = ( double ) NmrcUpDownDistance.Value;
        }

        private void NmrcUpDownRadial_ValueChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetDirection ( ( double ) NmrcUpDownRadial.Value );

            //vordme.Direction = ( double ) NmrcUpDownRadial.Value;
        }

        private void NmrcUpDownRadiusDsgntdPnts_ValueChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetRadiusForDsgPnts ( ( double ) NmrcUpDownRadiusDsgntdPnts.Value );

            //vordme.ReadDesignatedPoints ( CmbBxDsgntdPnts, CmbBxNavaids.SelectedItem as Navaid, ( double ) NmrcUpDownRadiusDsgntdPnts.Value, NmrcUpDownDistance, NmrcUpDownRadial );
        }

        private void CmbBxDsgntdPnts_SelectedIndexChanged ( object sender, EventArgs e )
        {
            // For Model
            InitHolding.ConvRacetrackModel.SetDesignatedPoint ( CmbBxDsgntdPnts.SelectedIndex );

            //if ( CmbBxDsgntdPnts.SelectedIndex != _designatedPointIndex )
            //{
            //    _designatedPointIndex = CmbBxDsgntdPnts.SelectedIndex;
            //    vordme.CalculateDistance ( CmbBxDsgntdPnts, NmrcUpDownDistance, NmrcUpDownRadial, NmrcUpDownAltitude, NmrcUpDownLimDist );
            //}
        }

        private void BtnOK_Click ( object sender, EventArgs e )
        {
            //if ( pntType == PointType.Create )
            //    vordme.CreateDesignatedPoint ( NmrcUpDownDistance, NmrcUpDownRadial, NmrcUpDownAltitude );
            //if ( RadBtnToWard.Checked )
            //{
            //    vordme.TowardConstructBasicArea ();
            //}
            //else
            //{
            //    vordme.AwayConstructBasicArea ();
            //}
        }

        private void RadBtnToWard_CheckedChanged ( object sender, EventArgs e )
        {
            //if ( RadBtnToWard.Checked )
            //    vordme.TowardProtectSector1 ();

            InitHolding.ConvRacetrackModel.SetEntryDirection ( RadBtnToward.Checked );
        }

        private void NmrcUpDownIAS_ValueChanged ( object sender, EventArgs e )
        {
            //vordme.IAS = ( double ) NmrcUpDownIAS.Value;

            InitHolding.ConvRacetrackModel.SetIAS ( (double) NmrcUpDownIAS.Value );
        }

        private void TxtBxTime_TextChanged ( object sender, EventArgs e )
        {
            //vordme.TimeInMinute = System.Convert.ToDouble ( TxtBxTime.Text );
        }

        private void NmrcUpDownLimDist_ValueChanged ( object sender, EventArgs e )
        {
            //vordme.LimitingDistanceInPrj = ( double ) NmrcUpDownLimDist.Value;

            InitHolding.ConvRacetrackModel.SetLimitingDistance ((double) NmrcUpDownLimDist.Value );
        }

        //private void RadBtnSelectDsgnPnt_CheckedChanged ( object sender, EventArgs e )
        //{
        //    NmrcUpDownDistance.Enabled = RadBtnCreateDsgnPnt.Checked;
        //    NmrcUpDownRadial.Enabled = RadBtnCreateDsgnPnt.Checked;
        //    CmbBxDsgntdPnts.Enabled = !RadBtnCreateDsgnPnt.Checked;
        //    if ( RadBtnCreateDsgnPnt.Checked )
        //    {
        //        pntType = PointType.Create;
        //        vordme.CreateDesignatedPoint ( NmrcUpDownDistance, NmrcUpDownRadial, NmrcUpDownAltitude );
        //    }
        //    else
        //    {
        //        pntType = PointType.Select;
        //        vordme.ReadDesignatedPoints ( CmbBxDsgntdPnts, CmbBxNavaids.SelectedItem as Navaid, ( double ) NmrcUpDownRadiusDsgntdPnts.Value, NmrcUpDownDistance, NmrcUpDownRadial );
        //    }
        //}

        #region For Model 

        private void RadBtnSelectDsgnPnt_CheckedChanged ( object sender, EventArgs e )
        {
            NmrcUpDownDistance.Enabled = RadBtnCreateDsgnPnt.Checked;
            NmrcUpDownRadial.Enabled = RadBtnCreateDsgnPnt.Checked;
            pointPicker1.Visible = RadBtnCreateDsgnPnt.Checked;
            PnlDsgPntSlc.Visible = !RadBtnCreateDsgnPnt.Checked;

            if ( RadBtnCreateDsgnPnt.Checked )
            {
                GrpBxSetDsgPnt.Width = pointPicker1.Width + 5;
                GrpBxSetDsgPnt.Height = PnlPntType.Height + pointPicker1.Height + PnlDsgPntCreate.Height + 20;
                InitHolding.ConvRacetrackModel.SetPointChoice ( PointChoice.PntChoice_Create );
            }
            else
            {
                InitHolding.ConvRacetrackModel.SetPointChoice ( PointChoice.PntChoice_Select );
                GrpBxSetDsgPnt.Width = PnlDsgPntSlc.Width + 10;
                GrpBxSetDsgPnt.Height = PnlPntType.Height +PnlDsgPntSlc.Height + PnlDsgPntCreate.Height + 20;
            }
        }

        #endregion 

        private void RadBtnRightDir_CheckedChanged ( object sender, EventArgs e )
        {
            //vordme.ChangeSide ( rdbRight.Checked );

            InitHolding.ConvRacetrackModel.SetSideDirection ( RadBtnRightDir.Checked );

        }

        private void ChckBxProtectSector1_CheckedChanged ( object sender, EventArgs e )
        {
            //if ( checkBox1.Checked )
            //{
            //    vordme.TowardProtectSector1 ();
            //}
        }

        private void ChckBxProtectSector2_CheckedChanged ( object sender, EventArgs e )
        {
            //if ( checkBox2.Checked )
            //{
            //    vordme.TowardProtectSector2 ();
            //}
        }

        private void ChckBxRecipDirEntSecondary_CheckedChanged ( object sender, EventArgs e )
        {
            //if ( checkBox3.Checked )
            //{
            //    textBox1.Text = vordme.TowardRecipDirectEntryToSecondaryPnt ().ToString ();
            //}
        }

        private void CmbBxAircraftCategory_SelectedIndexChanged ( object sender, EventArgs e )
        {
            InitHolding.ConvRacetrackModel.SetAircraftCategory ( CmbBxAircraftCategory.SelectedIndex );
        }
    } 
}
