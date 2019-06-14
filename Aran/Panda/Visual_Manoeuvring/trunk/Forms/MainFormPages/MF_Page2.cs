using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Geometries.Operators;
using Aran.Panda.Constants;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class MF_Page2 : UserControl
    {
        #region Variables
        public int prevPageIndex { get; set; }
        private FormHelpers.MF_Page2_Helper pageHelper;

        private MultiPoint RWYCollection;
        private Aran.Geometries.Point m_pCentroid;
        private double CntX;
        private double CntY;
        private GeometryOperators geomOper;
        private MultiPolygon pRWYsPolygon;
        private NavaidType[] ArNavList;
        private bool OnAero;
        public LowHigh[] Solutions { get; set; }
        private Aran.Geometries.Point RWYTHRPrj;
        private int[] RWYIndex;
        private InSectorNav[] InSectList;
        private double m_ArDir;
        #endregion

        public MF_Page2()
        {
            InitializeComponent();
            pageHelper = new FormHelpers.MF_Page2_Helper();
            geomOper = new GeometryOperators();
            lbl_ShortestFacilityDist.Text = GlobalVars.unitConverter.DistanceUnit;
        }

        private void cmbBox_GuidanceFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int K = -1;
            //int Side1;

            //bool bSolution;

            //GeometryOperators TopoOper = new GeometryOperators();

            //LineString pRWYLine;

            
            //double Rad1 = 0;
            //double Rad2 = 0;
            //double Rad3 = 0;
            //double Rad4 = 0;
            //double fTmp1;
            //double fTmp;

            ////===========Constants==================
            //double MinDistStraightInApproach;
            //double MaxDistStraightInApproach;
            //double OnAeroRange;
            //double TolerDist;
            //double Theta1;
            //double Theta2;

            //double dRad;
            //double dRad1;
            //double dRad2;
            //double dRad3;
            //double dRad4;

            //double Dist;
            //double Dist1;

            //double RadToAirport;

            
            //System.Windows.Forms.ListViewItem itmX;
            //string ItemStr;


            //K = cmbBox_GuidanceFacility.SelectedIndex;
            //if (K < 0)
            //    return;

            //VMManager.Instance.FinalNavaid = ArNavList[K];


            //lbl_NavaidType.Text = GlobalVars.constants.NavaidConstants.GetNavTypeName(VMManager.Instance.FinalNavaid.TypeCode);

            //if (VMManager.Instance.FinalNavaid.TypeCode != eNavaidType.LLZ)
            //    VMManager.Instance.FinalNavaid.GP_RDH = GlobalVars.constants.Pansops[ePANSOPSData.arAbv_Treshold].Value;

            ////===========================================================================

            //OnAeroRange = GlobalVars.constants.Pansops[ePANSOPSData.arCirclAprShift].Value;
            //TolerDist = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value;
            //Theta2 = GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value;
            //Theta1 = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arMaxInterAngle].Value[VMManager.Instance.Category];

            //MinDistStraightInApproach = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value;
            //MaxDistStraightInApproach = GlobalVars.constants.Pansops[ePANSOPSData.arMinInterDist].Value + 
            //    GlobalVars.constants.Pansops[ePANSOPSData.arMinInterToler].Value / 
            //        System.Math.Tan(GlobalVars.constants.Pansops[ePANSOPSData.arStrInAlignment].Value); //arFAFLenght.Value

            //lstVw_Solutions.Items.Clear();

            //pRWYLine = new LineString();


            ////========== Проверка приаэродромного средства =====

            //if (RWYCollection.Count < 3) //Расстояние РНС от порога ВПП :
            //{
            //    Dist = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, RWYTHRPrj);

            //    txtBox_ShortestFacARPDist.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(Dist, eRoundMode.NERAEST).ToString();
            //    lbl_ShortestFacARPDist.Text = Properties.Resources.str10204;  // "Ближайшее расстояние от РНС до порога ВПП :"

            //    pRWYLine.Add(RWYCollection[0]);
            //    pRWYLine.Add(RWYCollection[1]);
            //    TopoOper.CurrentGeometry = pRWYLine;
            //}
            //else
            //{
            //    Dist = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, m_pCentroid);

            //    txtBox_ShortestFacARPDist.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, GlobalVars.CurrADHP.pPtPrj), eRoundMode.NERAEST).ToString();
            //    lbl_ShortestFacARPDist.Text = Properties.Resources.str10209;  //"Ближайшее расстояние от РНС до КТА :"

            //    TopoOper.CurrentGeometry = TopoOper.ConvexHull(RWYCollection);

            //    //pRWYsPolygon = ;
            //    //TopoOper = pRWYsPolygon;
            //    //TopoOper.IsKnownSimple_2 = false;
            //    //TopoOper.Simplify();
            //    //pRoxi = pRWYsPolygon;
            //}

            //if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.VOR)         //        bSolution = VOR.FA_Range > Dist
            //{
            //    if (GlobalVars.constants.NavaidConstants.VOR.FA_Range <= Dist)
            //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(255, 0, 0);
            //    else
            //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0);
            //}
            //else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.NDB)         //        bSolution = NDB.FA_Range > Dist
            //{
            //    if (GlobalVars.constants.NavaidConstants.NDB.FA_Range <= Dist)
            //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(255, 0, 0);
            //    else
            //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0);
            //}
            //else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)         //        bSolution = (LLZ.Range > Dist) 'And (abs(Azt2Dir(vmManager.FinalNavaid.pPtGeo, _FinalNav.Course)-)
            //{
            //    if (GlobalVars.constants.NavaidConstants.LLZ.Range <= Dist)
            //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(255, 0, 0);
            //    else
            //        txtBox_ShortestFacARPDist.ForeColor = Color.FromArgb(0);
            //}

            //bSolution = GlobalVars.ArPANSOPS_MaxNavDist > Dist;
            //Dist = TopoOper.GetDistance(VMManager.Instance.FinalNavaid.pPtPrj);
            //OnAero = Dist < OnAeroRange;

            //if (OnAero)
            //    lbl_OnOffARDFac.Text = Properties.Resources.str10210;  //"Аэродромный"
            //else
            //    lbl_OnOffARDFac.Text = Properties.Resources.str10211;  //"Не аэродромный"

            //Solutions = null;

            //if (!bSolution)
            //{
            //    ItemStr = Properties.Resources.str303;
            //    ItemStr.Replace(((char) 10).ToString(), " ");
            //    //Replace(ItemStr, Chr(10), " ");
            //    itmX = lstVw_Solutions.Items.Add(ItemStr);
            //    itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, ItemStr));
            //    itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, ItemStr));
            //}
            //else if (VMManager.Instance.FinalNavaid.TypeCode == eNavaidType.LLZ)
            //{
            //    Solutions = new LowHigh[1];
            //    Solutions[0].Low = VMManager.Instance.FinalNavaid.Course;
            //    Solutions[0].High = VMManager.Instance.FinalNavaid.Course;
            //    Solutions[0].Tag = 0;

            //    itmX = lstVw_Solutions.Items.Add("LLZ");
            //    itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[0].Low + "°"));
            //    itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[0].High + "°"));
            //}
            //else //=========По кругу
            //{
            //    if (OnAero)
            //    {
            //        itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10221);

            //        Solutions = new LowHigh[1];
            //        Solutions[0].Low = 0.0;
            //        Solutions[0].High = 359.0;
            //        Solutions[0].Tag = 0;
            //    }
            //    else
            //    {
            //        Solutions = new LowHigh[6];
            //        RadToAirport = ARANMath.DegToRad(ARANMath.Modulus(ARANFunctions.ReturnAngleInDegrees(VMManager.Instance.FinalNavaid.pPtPrj, m_pCentroid), 360));

            //        Solutions[0].Low = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, RadToAirport) + 0.4999);
            //        Solutions[0].High = Solutions[0].Low;
            //        Solutions[0].Tag = 0;

            //        Solutions[1].Low = ARANMath.Modulus(Solutions[0].Low + 180.0, 360.0);
            //        Solutions[1].High = Solutions[1].Low;
            //        Solutions[1].Tag = 0;

            //        itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10218);
            //        itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[0].Low + "°"));

            //        itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10218);
            //        itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[1].Low + "°"));

            //        dRad1 = 0.0;
            //        dRad2 = 0.0;
            //        dRad3 = 0.0;
            //        dRad4 = 0.0;

            //        for (int i = 0; i < RWYCollection.Count; i++)
            //        {
            //            Side1 = (int)ARANMath.SideDef(VMManager.Instance.FinalNavaid.pPtPrj, RadToAirport, RWYCollection[i]);
            //            Dist1 = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, RWYCollection[i]);

            //            fTmp1 = OnAeroRange / Dist1;
            //            fTmp = ARANMath.Modulus(System.Math.Atan(fTmp1 / System.Math.Sqrt(-fTmp1 * fTmp1 + 1)), 2 * ARANMath.C_PI);
            //            fTmp1 = ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(VMManager.Instance.FinalNavaid.pPtPrj, RWYCollection[i]), 2 * ARANMath.C_PI);

            //            if (Side1 == (int) SideDirection.sideLeft)
            //            {
            //                dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1);
            //                if (dRad > dRad1)
            //                {
            //                    dRad1 = dRad;
            //                    Rad1 = fTmp1;
            //                }
            //            }
            //            else
            //            {
            //                dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1);
            //                if (dRad > dRad2)
            //                {
            //                    dRad2 = dRad;
            //                    Rad2 = fTmp1;
            //                }
            //            }

            //            dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1 + fTmp);
            //            if (dRad > dRad3)
            //            {
            //                dRad3 = dRad;
            //                Rad3 = fTmp1 + fTmp;
            //            }

            //            dRad = ARANMath.SubtractAngles(RadToAirport, fTmp1 - fTmp);
            //            if (dRad > dRad4)
            //            {
            //                dRad4 = dRad;
            //                Rad4 = fTmp1 - fTmp;
            //            }
            //        }

            //        Solutions[2].Low = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad1) + 0.4999);
            //        Solutions[2].High = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad2) - 0.4999);
            //        if (Solutions[2].Low > Solutions[2].High)
            //        {
            //            Solutions[2].Low = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad1));
            //            Solutions[2].High = System.Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad2));
            //        }
            //        Solutions[2].Tag = 1;

            //        Solutions[3].Low = ARANMath.Modulus(Solutions[2].Low + 180.0, 360.0);
            //        Solutions[3].High = ARANMath.Modulus(Solutions[2].High + 180.0, 360.0);
            //        Solutions[3].Tag = 1;

            //        itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10219);
            //        itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[2].Low + "°"));
            //        itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[2].High + "°"));

            //        itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10219);
            //        itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[3].Low + "°"));
            //        itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[3].High + "°"));


            //        Rad1 = GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad3);
            //        Rad2 = GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, Rad4);

            //        Rad3 = ARANMath.Modulus(Rad1 + 180.0, 360.0);
            //        Rad4 = ARANMath.Modulus(Rad2 + 180.0, 360.0);

            //        Solutions[4].Low = System.Math.Round(Rad1 + 0.4999);
            //        Solutions[4].High = System.Math.Round(Rad2 - 0.4999);
            //        Solutions[4].Tag = 2;

            //        Solutions[5].Low = System.Math.Round(Rad3 + 0.4999);
            //        Solutions[5].High = System.Math.Round(Rad4 - 0.4999);
            //        Solutions[5].Tag = 2;

            //        itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10220);
            //        itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[4].Low + "°"));
            //        itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[4].High + "°"));

            //        itmX = lstVw_Solutions.Items.Add(Properties.Resources.str10220);
            //        itmX.SubItems.Insert(1, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[5].Low + "°"));
            //        itmX.SubItems.Insert(2, new System.Windows.Forms.ListViewItem.ListViewSubItem(null, Solutions[5].High + "°"));
            //    }
            //}

            //cmbBox_TrueBRGRange.Items.Clear();
            //int N = Solutions.Length;
            //for (int i = 0; i < N; i++)
            //{
            //    if (Solutions[i].Low == Solutions[i].High)
            //        cmbBox_TrueBRGRange.Items.Add(Solutions[i].Low);
            //    else
            //        cmbBox_TrueBRGRange.Items.Add(Solutions[i].Low + "-" + Solutions[i].High);
            //}
            //cmbBox_TrueBRGRange.SelectedIndex = 0;
        }

        private void cmbBox_RWYTHR_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////If Not bFormInitialised Then Return

            //int K;
            //K = cmbBox_RWYTHR.SelectedIndex;
            //if (K < 0)
            //    return;

            //VMManager.Instance.SelectedRWY = GlobalVars.RWYList[RWYIndex[K]];
            //RWYTHRPrj = VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR];

            //txtBox_TrueBRG.Text = System.Math.Round(ARANMath.Modulus(VMManager.Instance.SelectedRWY.TrueBearing, 360.0), 2).ToString(); //ik
            //txtBox_MAGBRG.Text = System.Math.Round(ARANMath.Modulus(VMManager.Instance.SelectedRWY.TrueBearing - GlobalVars.CurrADHP.MagVar, 360.0), 2).ToString(); //mk


            ////_fRefHeight = GlobalVars.CurrADHP.pPtGeo.Z;
            ////_Label0101_8.Text = My.Resources.str10212  '"OCH определяется относительно превышения аэродрома"

            //if (cmbBox_GuidanceFacility.Items.Count > 0)
            //    cmbBox_GuidanceFacility.SelectedIndex = 0;
        }

        private void cmbBox_TrueBRGRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////If Not bFormInitialised Then Return

            //int k;
            //int N;
            //int Optima;
            //double fTmp;
            //double fLow;
            //double fHigh;
            //double RWYAzt;

            ////Dim itmX As System.Windows.Forms.ListViewItem
            //GeometryOperators pTopo = new GeometryOperators();
            //MultiPoint pSector;
            ////Dim pSector As ESRI.ArcGIS.Geometry.IPointCollection

            //k = cmbBox_TrueBRGRange.SelectedIndex;

            //if ((k < 0) || (cmbBox_TrueBRGRange.Tag == "A")) 
            //    return;

            //cmbBox_TrueBRGRange.Tag = "A";

            //N = Solutions.Length;

            ////for (int i = 0; i < N; i++)
            ////{
            ////    if (Solutions[i].Low == Solutions[i].High)
            ////        cmbBox_TrueBRGRange.Items[i] = Solutions[i].Low;
            ////    else
            ////        cmbBox_TrueBRGRange.Items[i] = Solutions[i].Low + "-" + Solutions[i].High;
            ////}

            //cmbBox_TrueBRGRange.Tag = "";

            //RWYAzt = GlobalVars.pspatialReferenceOperation.DirToAztPrj(VMManager.Instance.FinalNavaid.pPtPrj, RWYTHRPrj.M);

            //Optima =  (int) Math.Round(Solutions[k].Low);
            //fLow = ARANMath.SubtractAnglesInDegs(RWYAzt, Optima);

            //for (int i = (int) Math.Round(Solutions[k].Low); i <= (int) Math.Round(Solutions[k].High); i++)
            //{
            //    fTmp = ARANMath.SubtractAnglesInDegs(RWYAzt, i);
            //    if (fTmp < fLow)
            //    {
            //        fLow = fTmp;
            //        Optima = i;
            //    }
            //}

            //nmrcUpDown_TrueBRGRange.Tag = false;

            //nmrcUpDown_TrueBRGRange.Minimum = (decimal) Math.Round(Solutions[k].Low);

            //if (Solutions[k].Low <= Solutions[k].High)
            //    nmrcUpDown_TrueBRGRange.Maximum = (decimal) Math.Round(Solutions[k].High);
            //else
            //    nmrcUpDown_TrueBRGRange.Maximum = (decimal) Math.Round(Solutions[k].High) + 360;

            //nmrcUpDown_TrueBRGRange.Tag = true;

            //if (nmrcUpDown_TrueBRGRange.Value == Optima)
            //    nmrcUpDown_TrueBRGRange_ValueChanged(nmrcUpDown_TrueBRGRange, new System.EventArgs());
            //else
            //    nmrcUpDown_TrueBRGRange.Value = Optima;

            //if (Solutions[k].Low == Solutions[k].High)
            //{
            //    txtBox_TrueBRGRange.ReadOnly = true;
            //    txtBox_TrueBRGRange.BackColor = System.Drawing.SystemColors.Control;
            //    nmrcUpDown_TrueBRGRange.Enabled = false;
            //}
            //else{
            //    txtBox_TrueBRGRange.ReadOnly = false;
            //    txtBox_TrueBRGRange.BackColor = System.Drawing.SystemColors.Window;
            //    nmrcUpDown_TrueBRGRange.Enabled = true;
            //}

            //if (ARANMath.SubtractAnglesInDegs(Solutions[k].Low, Solutions[k].High) < 1.0)
            //{
            //    fHigh = GlobalVars.pspatialReferenceOperation.AztToDirGeo(VMManager.Instance.FinalNavaid.pPtGeo, Solutions[k].High + 0.5 * GlobalVars.constants.NavaidConstants.LLZ.TrackingTolerance);
            //    fLow = GlobalVars.pspatialReferenceOperation.AztToDirGeo(VMManager.Instance.FinalNavaid.pPtGeo, Solutions[k].Low - 0.5 * GlobalVars.constants.NavaidConstants.LLZ.TrackingTolerance);
            //}
            //else
            //{
            //    fHigh = GlobalVars.pspatialReferenceOperation.AztToDirGeo(VMManager.Instance.FinalNavaid.pPtGeo, Solutions[k].High);
            //    fLow = GlobalVars.pspatialReferenceOperation.AztToDirGeo(VMManager.Instance.FinalNavaid.pPtGeo, Solutions[k].Low);
            //}

            //if (OnAero)
            //    pSector = ARANFunctions.CreateCircleAsMultiPolyPrj(VMManager.Instance.FinalNavaid.pPtPrj, 10.0 * GlobalVars.RModel).ToMultiPoint();
            //else
            //{
            //    pSector = new MultiPoint();
            //    pSector.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, fHigh, 10.0 * GlobalVars.RModel));
            //    pSector.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, fLow, 10.0 * GlobalVars.RModel));
            //    pSector.Add(VMManager.Instance.FinalNavaid.pPtPrj);
            //    pSector.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, fLow + 180.0, 10.0 * GlobalVars.RModel));
            //    pSector.Add(ARANFunctions.PointAlongPlane(VMManager.Instance.FinalNavaid.pPtPrj, fHigh + 180.0, 10.0 * GlobalVars.RModel));
            //    pSector.Add(VMManager.Instance.FinalNavaid.pPtPrj);

            //}

            //pageHelper.NavInSector(VMManager.Instance.FinalNavaid, out InSectList, ref pSector, fLow, RWYTHRPrj, m_ArDir);
        }

        private void nmrcUpDown_TrueBRGRange_ValueChanged(object sender, EventArgs e)
        {
            //if (!(bool) nmrcUpDown_TrueBRGRange.Tag) return;
            //string tmp = "";
            //pageHelper.AzimuthChanged((double) nmrcUpDown_TrueBRGRange.Value, RWYTHRPrj, ref m_ArDir, double.Parse(txtBox_ARCSemiSpan.Text.ToString()), double.Parse(txtBox_ARCVeticalDimension.Text.ToString()), ref tmp);
            //txtBox_TrueBRGRange.Text = tmp;
        }

        private void txtBox_TrueBRGRange_TextChanged(object sender, EventArgs e)
        {            
            //VMManager.Instance.TrueBRGAngle = ARANMath.DegToRad(Double.Parse(txtBox_TrueBRGRange.Text.ToString()));
        }

        private void MF_Page2_VisibleChanged(object sender, EventArgs e)
        {
            //if (this.Visible)
            //{
            //    if (prevPageIndex == 0)
            //    {
            //        txtBox_ARCSemiSpan.Text = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arSemiSpan].Value[VMManager.Instance.Category].ToString();
            //        txtBox_ARCVeticalDimension.Text = GlobalVars.constants.AircraftCategory[aircraftCategoryData.arVerticalSize].Value[VMManager.Instance.Category].ToString();
            //        cmbBox_RWYTHR.Items.Clear();
            //        int j = -1;
            //        for (int i = 0; i < GlobalVars.RWYList.Length; i++)
            //        {
            //            if (GlobalVars.RWYList[i].Selected)
            //            {
            //                j++;
            //                RWYIndex[j] = i;
            //                RWYCollection.Add(GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR]);
            //                cmbBox_RWYTHR.Items.Add(GlobalVars.RWYList[i].Name);
            //            }
            //        }
            //        cmbBox_RWYTHR.SelectedIndex = 0;

            //        if (j < 0)
            //        {
            //            MessageBox.Show(Properties.Resources.str300);
            //            return;
            //        }
            //        else
            //        {
            //            if (j < 2)
            //            {
            //                CntX = 0.0;
            //                CntY = 0.0;
            //                for (int i = 0; i <= 1; i++)
            //                {
            //                    CntX = CntX + RWYCollection[i].X;
            //                    CntY = CntY + RWYCollection[i].Y;
            //                }

            //                CntX = 0.5 * CntX;
            //                CntY = 0.5 * CntY;

            //                m_pCentroid = new Aran.Geometries.Point();
            //                m_pCentroid.SetCoords(CntX, CntY);
            //            }
            //            else
            //            {
            //                pRWYsPolygon = (MultiPolygon)geomOper.ConvexHull(RWYCollection);
            //                m_pCentroid = pRWYsPolygon.Centroid;
            //            }
            //        }


            //        cmbBox_GuidanceFacility.Items.Clear();

            //        int N = pageHelper.FillArNavListForCircling(m_pCentroid, out ArNavList);

            //        if (N > 0)
            //        {
            //            for (int i = 0; i < N; i++)
            //            {
            //                cmbBox_GuidanceFacility.Items.Add(ArNavList[i].CallSign);
            //            }
            //            cmbBox_GuidanceFacility.SelectedIndex = 0;
            //        }
            //        else
            //        {
            //            MessageBox.Show("There is no suitable facility for guidance.");
            //            return;
            //        }

            //        txtBox_ShortestFacARPDist.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.FinalNavaid.pPtPrj, GlobalVars.CurrADHP.pPtPrj), eRoundMode.NERAEST).ToString();
            //    }
            //} 
        }

        private void MF_Page2_Load(object sender, EventArgs e)
        {
            //RWYIndex = new int[GlobalVars.RWYList.Length];
            //if (RWYCollection == null)
            //    RWYCollection = new MultiPoint();
            //else
            //    for (int i = 0; i < RWYCollection.Count; i++)
            //        RWYCollection.Remove(i);
        }

        private void txtBox_TrueBRGRange_KeyPress(object sender, KeyPressEventArgs e)
        {
            //int isNumber = 0;
            //e.Handled = !(int.TryParse(e.KeyChar.ToString(), out isNumber) || e.KeyChar == '\b');
        }
    }
}
