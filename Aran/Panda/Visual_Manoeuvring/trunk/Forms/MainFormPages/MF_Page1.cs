using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Aran.Geometries;
using Aran.Panda.Common;
using Aran.Geometries.Operators;
using Aran.Panda.Constants;
using Aran.Aim.Features;
using Aran.Queries;
using Aran.Aim.Enums;
using Aran.Converters;
using Aran.Aim.DataTypes;

namespace Aran.Panda.VisualManoeuvring.Forms
{
    public partial class MF_Page1 : UserControl
    {
        bool isInitialization = true;
        bool exclude = false;
        public FormHelpers.MF_Page1_Helper pageHelper;
        enum Cat { A = 0, B = 1, C = 2, D = 3, E = 5 };
        private int[] RWYIndex;
        private MultiPoint RWYCollection;
        private Aran.Geometries.Point m_pCentroid;
        private double CntX;
        private double CntY;
        private GeometryOperators geomOper;
        private MultiPolygon pRWYsPolygon;
        private NavaidType[] ArNavList;
        public LowHigh[] Solutions { get; set; }
        List<NavaidType> usableNavaids;

        MultiLineString finalSegmentLegsTrajectoryPrjMerged;
        Point segmentLegStartPoint;
        Point segmentLegEndPoint;
        double segmentLegUpperAltitude;
        double segmentLegLowerAltitude;
        List<SegmentLeg> finalSegmentLegsList;

        Point firstPoint;
        Point lastPoint;
        Point BaseTrackInitialPosition;

        String[] MAG_TRUE = { "MAG BRG", "TRUE BRG"};
        double legCourse;

        String[] OCH_OCA = { "OCH", "OCA" };

        MainForm mainForm;
        public MF_Page1(MainForm mf)
        {
            InitializeComponent();
            cmbBox_MAG_TRUE.DataSource = MAG_TRUE;
            cmbBox_OCH_OCA.DataSource = OCH_OCA;
            pageHelper = new FormHelpers.MF_Page1_Helper();
            mainForm = mf;
            int k = 0;
            double temp = GlobalVars.unitConverter.DistancePrecision;
            while (temp != 1)
            {
                temp *= 10;
                k++;
            }

            nmrcUpDown_initialPositionAdjust.DecimalPlaces = k;
            nmrcUpDown_initialPositionAdjust.Increment = (decimal) GlobalVars.unitConverter.DistancePrecision;

            
            lbl_distanceSign.Text = GlobalVars.unitConverter.DistanceUnit;
            lbl_heightSign.Text = GlobalVars.unitConverter.HeightUnit;
            lbl_heightSign2.Text = GlobalVars.unitConverter.HeightUnit;
        }

        private void MF_Page1_Load(object sender, EventArgs e)
        {
            geomOper = new GeometryOperators();

            VMManager.Instance.GeomOper = new JtsGeometryOperators();
            

            RWYIndex = new int[GlobalVars.RWYList.Length];
            if (RWYCollection == null)
                RWYCollection = new MultiPoint();
            else
                for (int i = 0; i < RWYCollection.Count; i++)
                    RWYCollection.Remove(i);

            lstVw_RWY.Items.Clear();
            for (int i = 0; i < GlobalVars.RWYList.Length; i++)
            {
                GlobalVars.RWYList[i].Selected = true;
                if ((i % 2) == 0)
                {
                    lstVw_RWY.Items.Add(GlobalVars.RWYList[i].Name + " / " + GlobalVars.RWYList[i].PairName);
                    lstVw_RWY.Items[i / 2].Checked = true;
                }
            }

            

            cmbBox_AircraftCat.DataSource = Cat.GetValues(typeof(Cat));
            isInitialization = false;
            if (cmbBox_AircraftCat.SelectedIndex >= 0)
                cmbBox_AircraftCat_SelectedIndexChanged(cmbBox_AircraftCat, new System.EventArgs());
            else
                cmbBox_AircraftCat.SelectedIndex = 0;

            isInitialization = true;
            lstVw_RWY_ItemChecked(this, null);
            isInitialization = false;


            //VMManager.Instance.ArrivalRadiusMin = 55560;
            //VMManager.Instance.ArrivalRadiusMax = 74080;

            //nmrcUpDown_ArrivalRadius.Minimum = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.ArrivalRadiusMin, Common.eRoundMode.CEIL);
            //nmrcUpDown_ArrivalRadius.Maximum = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.ArrivalRadiusMax, Common.eRoundMode.CEIL);
            //nmrcUpDown_ArrivalRadius.Value = nmrcUpDown_ArrivalRadius.Minimum;

            lbl_distanceSign2.Text = GlobalVars.unitConverter.DistanceUnit;
            lbl_kmSign2.Text = GlobalVars.unitConverter.DistanceUnit;
            if (GlobalVars.settings.DistanceUnit == HorizantalDistanceType.KM)
            {
                nmrcUpDown_visibilityDistance.DecimalPlaces = 1;
                nmrcUpDown_visibilityDistance.Increment = (decimal)0.1;
            }
            else
            {
                nmrcUpDown_visibilityDistance.DecimalPlaces = 2;
                nmrcUpDown_visibilityDistance.Increment = (decimal)0.01;
            }

            exclude = true;

            VMManager.Instance.IAPList = new BindingList<InstrumentApproachProcedure>(DBModule.pObjectDir.GetIAPList(GlobalVars.CurrADHP.Identifier));

            for (int i = 0; i < VMManager.Instance.IAPList.Count; i++)
			{
			    if(VMManager.Instance.IAPList[i].Landing == null || VMManager.Instance.IAPList[i].Landing.Runway.Count < 2)
                {
                    VMManager.Instance.IAPList.RemoveAt(i);
                    i--;
                }
			}

            if (VMManager.Instance.IAPList.Count == 0)
            {
                MessageBox.Show("There is no circling IAP for this airport in the database.");
                mainForm.btn_Next.Enabled = false;
                return;
            }

            cmbBox_IAP.DataSource = VMManager.Instance.IAPList;
            VMManager.Instance.TrackInitialPosition = null;
            cmbBox_IAP.DisplayMember = "Name";
            if (cmbBox_IAP.Items.Count > 0)
                cmbBox_IAP.SelectedIndex = 0;


            cmbBox_DivergenceVF.DataSource = VMManager.Instance.DivergenceVFList;
            cmbBox_DivergenceVF.DisplayMember = "Name";
            if(cmbBox_DivergenceVF.Items.Count > 0)
                cmbBox_DivergenceVF.SelectedIndex = 0;
        }

        private void cmbBox_AircraftCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitialization)
                return;
            int Idx = (int)cmbBox_AircraftCat.SelectedItem;
            pageHelper.setCategory(cmbBox_AircraftCat.SelectedIndex, Idx);
            nmrcUpDown_visibilityDistance.Minimum = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.MinVisibilityDistance, eRoundMode.NONE);
            nmrcUpDown_visibilityDistance.Maximum = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.MinVisibilityDistance + 5000, eRoundMode.NONE);
            nmrcUpDown_visibilityDistance.Value = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(VMManager.Instance.MinVisibilityDistance, eRoundMode.NONE);
            pageHelper.vsManev(Idx);
            //if (exclude)
            //{
            //    //pageHelper.ExcludeDesignatedPointsWithinCirclingArea();
            //    nmrcUpDown_ArrivalRadius_ValueChanged(this, null);
            //}
            VMManager.Instance.TrackInitialPosition = null;
            cmbBox_IAP_SelectedIndexChanged(this, null);

            if(cmbBox_OCH_OCA.SelectedIndex == 0)
                txtBox_minOCA.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MinOCH, eRoundMode.CEIL).ToString();
            else
                txtBox_minOCA.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MinOCA, eRoundMode.CEIL).ToString();
        }

        private void lstVw_RWY_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (cmbBox_AircraftCat.SelectedIndex < 0)
                return;

            bool isAnyChecked = false;
            for (int i = 0; i < lstVw_RWY.Items.Count; i++)
            {
                if (lstVw_RWY.Items[i].Checked)
                {
                    GlobalVars.RWYList[i * 2].Selected = true;
                    GlobalVars.RWYList[i * 2 + 1].Selected = true;
                    isAnyChecked = true;
                }
                else
                {
                    GlobalVars.RWYList[i * 2].Selected = false;
                    GlobalVars.RWYList[i * 2 + 1].Selected = false;
                }
            }

            if (!isAnyChecked)
            {
                e.Item.Checked = true;
                int idx = e.Item.Index;
                GlobalVars.RWYList[idx * 2].Selected = true;
                GlobalVars.RWYList[idx * 2 + 1].Selected = true;
            }
            else
                pageHelper.vsManev(cmbBox_AircraftCat.SelectedIndex);


            cmbBox_RWYTHR.Items.Clear();            
            int j = -1;
            for (int i = 0; i < GlobalVars.RWYList.Length; i++)
            {
                if (GlobalVars.RWYList[i].Selected)
                {
                    j++;
                    RWYIndex[j] = i;
                    RWYCollection.Add(GlobalVars.RWYList[i].pPtPrj[eRWY.ptTHR]);
                    cmbBox_RWYTHR.Items.Add(GlobalVars.RWYList[i].Name);
                }
            }
            

            if (j < 0)
            {
                MessageBox.Show(Properties.Resources.str300);
                return;
            }
            else
            {
                if (j < 2)
                {
                    CntX = 0.0;
                    CntY = 0.0;
                    for (int i = 0; i <= 1; i++)
                    {
                        CntX = CntX + RWYCollection[i].X;
                        CntY = CntY + RWYCollection[i].Y;
                    }

                    CntX = 0.5 * CntX;
                    CntY = 0.5 * CntY;

                    m_pCentroid = new Aran.Geometries.Point();
                    m_pCentroid.SetCoords(CntX, CntY);
                }
                else
                {
                    pRWYsPolygon = (MultiPolygon)geomOper.ConvexHull(RWYCollection);
                    m_pCentroid = pRWYsPolygon.Centroid;
                }
            }                      

            //int N = pageHelper.FillArNavListForCircling(m_pCentroid, out ArNavList);
            cmbBox_RWYTHR.SelectedIndex = 0;
        }

        private void nmrcUpDown_ArrivalRadius_ValueChanged(object sender, EventArgs e)
        {
            //VMManager.Instance.ArrivalRadius = GlobalVars.unitConverter.DistanceToInternalUnits((double)nmrcUpDown_ArrivalRadius.Value);
            //pageHelper.ConstructArrivalArea();
            //pageHelper.GetDesignatedPointsWithinArrivalArea();
            
            //if (VMManager.Instance.DesignatedPointsList.Count > 0)
            //{
            //    VMManager.Instance.DesignatedPointsList = VMManager.Instance.DesignatedPointsList.OrderBy(dp => dp.Designator).ToList<Aran.Aim.Features.DesignatedPoint>();
            //    cmbBox_DesignatedPoint.DataSource = VMManager.Instance.DesignatedPointsList;
            //    cmbBox_DesignatedPoint.DisplayMember = "Designator";
            //    cmbBox_DesignatedPoint.SelectedIndex = 0;
            //}
        }

        private void cmbBox_DesignatedPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmbBox_GuidanceFacility.Items.Clear();
            //VMManager.Instance.SelectedDesignatedPoint = VMManager.Instance.DesignatedPointsList[cmbBox_DesignatedPoint.SelectedIndex];
            //usableNavaids = pageHelper.GetUsableNavaids(VMManager.Instance.SelectedDesignatedPoint, ArNavList, RWYCollection, m_pCentroid);
            //int N = usableNavaids.Count;
            //if (N > 0)
            //{
            //    for (int i = 0; i < N; i++)
            //    {
            //        cmbBox_GuidanceFacility.Items.Add(usableNavaids[i].CallSign);
            //    }
            //    cmbBox_GuidanceFacility.SelectedIndex = 0;
                
            //}
            //else
            //{
            //    MessageBox.Show("There is no suitable facility for guidance.");
            //    return;
            //}

            //if (VMManager.Instance.DesignatedPointElement > -1)
            //{
            //    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.DesignatedPointElement);
            //}

            //VMManager.Instance.DesignatedPointElement = GlobalVars.gAranEnv.Graphics.DrawPointWithText(
            //    GlobalVars.pspatialReferenceOperation.ToPrj<Point>(VMManager.Instance.SelectedDesignatedPoint.Location.Geo), 255,
            //    VMManager.Instance.SelectedDesignatedPoint.Designator);
        }

        private void cmbBox_GuidanceFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int K = cmbBox_GuidanceFacility.SelectedIndex;            

            //VMManager.Instance.FinalNavaid = usableNavaids[K];

            //lbl_NavaidType.Text = GlobalVars.constants.NavaidConstants.GetNavTypeName(VMManager.Instance.FinalNavaid.TypeCode);

            //LineString ls = new LineString();
            //ls.Add(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(VMManager.Instance.SelectedDesignatedPoint.Location.Geo));
            //ls.Add(VMManager.Instance.FinalNavaid.pPtPrj);
            //if (VMManager.Instance.DesignatedPointToNavaidSegmentElement > -1)
            //    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.DesignatedPointToNavaidSegmentElement);

            //VMManager.Instance.DesignatedPointToNavaidSegmentElement = GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 255, 0), 1);
            //VMManager.Instance.TrueBRGAngle = ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(VMManager.Instance.SelectedDesignatedPoint.Location.Geo), VMManager.Instance.FinalNavaid.pPtPrj), ARANMath.C_2xPI);
            //pageHelper.getInitialPositionAndDirection();
            //txtBox_CinrclingAreaEntranceDirection.Text = Math.Round(GlobalVars.pspatialReferenceOperation.DirToAztPrj(GlobalVars.pspatialReferenceOperation.ToPrj<Point>(VMManager.Instance.SelectedDesignatedPoint.Location.Geo), VMManager.Instance.TrueBRGAngle), 1).ToString();

            //if (VMManager.Instance.InitialPositionElement > -1)
            //    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.InitialPositionElement);

            //VMManager.Instance.InitialPositionElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.InitialPosition, 255);

            //if (VMManager.Instance.InitialDirectionElement > -1)
            //    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.InitialDirectionElement);

            //Point pnt = ARANFunctions.PointAlongPlane(VMManager.Instance.InitialPosition, VMManager.Instance.InitialDirection, 1500);
            //MultiLineString mls = new MultiLineString();
            //mls.Add(new LineString());
            //mls[0].Add(VMManager.Instance.InitialPosition);
            //mls[0].Add(pnt);

            //mls.Add(new LineString());
            //mls[1].Add(ARANFunctions.PointAlongPlane(pnt, ARANMath.Modulus(VMManager.Instance.InitialDirection - ARANMath.DegToRad(150), ARANMath.C_2xPI), 400));
            //mls[1].Add(pnt);
            //mls[1].Add(ARANFunctions.PointAlongPlane(pnt, ARANMath.Modulus(VMManager.Instance.InitialDirection + ARANMath.DegToRad(150), ARANMath.C_2xPI), 400));

            //VMManager.Instance.InitialDirectionElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(mls, ARANFunctions.RGB(0, 0, 255), 2);
        }

        private void cmbBox_RWYTHR_SelectedIndexChanged(object sender, EventArgs e)
        {
            int K;
            K = cmbBox_RWYTHR.SelectedIndex;
            if (K < 0)
                return;

            VMManager.Instance.SelectedRWY = GlobalVars.RWYList[RWYIndex[K]];

            if(!isInitialization)
                cmbBox_DesignatedPoint_SelectedIndexChanged(this, null);

            MultiLineString mls = new MultiLineString();
            mls.Add(new LineString());
            Point tempPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], 
                ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR], 
                VMManager.Instance.SelectedRWY.pRunwayDir.TrueBearing.Value) - ARANMath.C_PI, ARANMath.C_2xPI), 1500);
            
            mls[0].Add(tempPnt);            
            mls[0].Add(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR]);

            mls.Add(new LineString());
            tempPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR],
                ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR],
                VMManager.Instance.SelectedRWY.pRunwayDir.TrueBearing.Value) - ARANMath.DegToRad(150), ARANMath.C_2xPI), 400);
            mls[1].Add(tempPnt);
            mls[1].Add(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR]);
            tempPnt = ARANFunctions.PointAlongPlane(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR],
                ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR],
                VMManager.Instance.SelectedRWY.pRunwayDir.TrueBearing.Value) + ARANMath.DegToRad(150), ARANMath.C_2xPI), 400);
            mls[1].Add(tempPnt);

            if (VMManager.Instance.RWYTHRSelectElement > -1)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RWYTHRSelectElement);
            VMManager.Instance.RWYTHRSelectElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(mls, 255, 2);


            LineString ls = new LineString();
            ls.Add(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptTHR]);
            ls.Add(VMManager.Instance.SelectedRWY.pPtPrj[eRWY.ptEnd]);
            if (VMManager.Instance.RWYElement > -1)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.RWYElement);
            VMManager.Instance.RWYElement = GlobalVars.gAranEnv.Graphics.DrawLineString(ls, ARANFunctions.RGB(0, 153, 76), 2);
        }

        private void cmbBox_IAP_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (cmbBox_IAP.SelectedIndex < 0)
                return;

            for (int i = 0; i < VMManager.Instance.FinalSegmentLegsPointsElements.Count; i++)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.FinalSegmentLegsPointsElements[i]);
            }

            VMManager.Instance.FinalSegmentLegsPointsElements = new List<int>();
            finalSegmentLegsTrajectoryPrjMerged = new MultiLineString();
            finalSegmentLegsList = new List<SegmentLeg>();
            VMManager.Instance.TrackInitialPosition = null;
            VMManager.Instance.SelectedIAP = VMManager.Instance.IAPList[cmbBox_IAP.SelectedIndex];


            for (int i = 0; i < VMManager.Instance.SelectedIAP.FlightTransition.Count; i++)
            {
                if (VMManager.Instance.SelectedIAP.FlightTransition[i].Type == Aim.Enums.CodeProcedurePhase.FINAL)
                {
                    for (int j = 0; j < VMManager.Instance.SelectedIAP.FlightTransition[i].TransitionLeg.Count; j++)
                    {
                        if (VMManager.Instance.SelectedIAP.FlightTransition[i].TransitionLeg[j].TheSegmentLeg.Type == Aim.SegmentLegType.FinalLeg)
                        {
                            SegmentLeg leg = (SegmentLeg)VMManager.Instance.SelectedIAP.FlightTransition[i].TransitionLeg[j].TheSegmentLeg.GetFeature();
                            if(leg.Course == null)
                            {
                                MessageBox.Show("Final leg course is NULL");
                                return;
                            }

                            //var x = leg.EndPoint.Role.Value;

                            MultiLineString legTrajectoryPrj = GlobalVars.pspatialReferenceOperation.ToPrj<MultiLineString>(leg.Trajectory.Geo);
                            
                            segmentLegStartPoint = legTrajectoryPrj[0][0];
                            segmentLegEndPoint = legTrajectoryPrj[0][legTrajectoryPrj[0].Count - 1];

                            
                            segmentLegUpperAltitude = ConverterToSI.Convert(new ValDistanceVertical(leg.UpperLimitAltitude.Value, leg.UpperLimitAltitude.Uom), 0);
                            segmentLegLowerAltitude = ConverterToSI.Convert(new ValDistanceVertical(leg.LowerLimitAltitude.Value, leg.LowerLimitAltitude.Uom), 0);
                            //VMManager.Instance.FinalLegsElements.Add(GlobalVars.gAranEnv.Graphics.DrawMultiLineString(legTrajectoryPrj, 0, 2));

                            LineString ls = new LineString();
                            ls.AddMultiPoint(VMManager.Instance.ConvexPoly[0].ExteriorRing);


                            if(VMManager.Instance.TrackInitialPosition == null)
                            {
                                Geometry g = geomOper.Intersect(ls, legTrajectoryPrj);
                                //Geometry g = geomOper.Intersect(VMManager.Instance.ConvexPoly, legTrajectoryPrj);
                                
                                //JtsGeometryOperators gO = new JtsGeometryOperators();
                                //Geometry g = gO.Intersect(ls, legTrajectoryPrj);
                                if (!g.IsEmpty || geomOper.Contains(VMManager.Instance.ConvexPoly, legTrajectoryPrj))
                                {
                                    finalSegmentLegsList.Add(leg);
                                    finalSegmentLegsTrajectoryPrjMerged.Add(legTrajectoryPrj);
                                    
                                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement);
                                    VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(finalSegmentLegsTrajectoryPrjMerged, ARANFunctions.RGB(0, 153, 76), 2);
                                    VMManager.Instance.FinalSegmentLegsPointsElements.Add(GlobalVars.gAranEnv.Graphics.DrawPoint(segmentLegStartPoint, ARANFunctions.RGB(76, 0, 153)));
                                    VMManager.Instance.FinalSegmentLegsPointsElements.Add(GlobalVars.gAranEnv.Graphics.DrawPoint(segmentLegEndPoint, ARANFunctions.RGB(76, 0, 153)));
                                    
                                    if (!g.IsEmpty)
                                    {
                                        VMManager.Instance.TrackInitialPosition = ((MultiPoint)g)[0];
                                        VMManager.Instance.TrackInitialDirection = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.TrackInitialPosition, (double)leg.Course), ARANMath.C_2xPI);
                                        double legStartPointAltitudeM = segmentLegUpperAltitude;
                                        VMManager.Instance.TrackInitialPositionAltitude = legStartPointAltitudeM - (ARANFunctions.ReturnDistanceInMeters(segmentLegStartPoint, VMManager.Instance.TrackInitialPosition) * Math.Tan(ARANMath.DegToRad(Math.Abs((double)leg.VerticalAngle))));
                                    }
                                    else // if (geomOper.Contains(VMManager.Instance.ConvexPoly, legTrajectoryPrj))
                                    {
                                        VMManager.Instance.TrackInitialPosition = segmentLegStartPoint;
                                        VMManager.Instance.TrackInitialDirection = ARANMath.Modulus(GlobalVars.pspatialReferenceOperation.AztToDirPrj(VMManager.Instance.TrackInitialPosition, (double)leg.Course), ARANMath.C_2xPI);
                                        VMManager.Instance.TrackInitialPositionAltitude = segmentLegUpperAltitude;
                                    }
                                    
                                    if (VMManager.Instance.TrackInitialPositionAltitude < VMManager.Instance.CA_OCA_Actual)
                                        MessageBox.Show("The part of the final segment within the circling area is below the circling area OCH.");
                                    else
                                    {
                                        nmrcUpDown_initialPositionAdjust.Minimum = (decimal) GlobalVars.unitConverter.DistanceToDisplayUnits(ARANFunctions.ReturnDistanceInMeters(segmentLegStartPoint, VMManager.Instance.TrackInitialPosition));
                                        if (segmentLegLowerAltitude >= VMManager.Instance.CA_OCA_Actual)
                                        {
                                            nmrcUpDown_initialPositionAdjust.Maximum = (decimal) GlobalVars.unitConverter.DistanceToDisplayUnits(ARANFunctions.ReturnDistanceInMeters(segmentLegStartPoint, segmentLegEndPoint));
                                            nmrcUpDown_initialPositionAdjust.Value = nmrcUpDown_initialPositionAdjust.Minimum;                                            
                                        }
                                        else
                                        {
                                            double deltaHeight = VMManager.Instance.TrackInitialPositionAltitude - VMManager.Instance.CA_OCA_Actual;
                                            double deltaDistance = deltaHeight / Math.Tan(ARANMath.DegToRad(Math.Abs((double)leg.VerticalAngle)));
                                            nmrcUpDown_initialPositionAdjust.Maximum = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(deltaDistance);
                                            nmrcUpDown_initialPositionAdjust.Value = nmrcUpDown_initialPositionAdjust.Minimum;
                                        }
                                        
                                    }
                                    legCourse = (double)leg.Course;
                                    if(cmbBox_MAG_TRUE.SelectedIndex == 0)
                                        txtBox_CirclingAreaEntranceDirection.Text = (Math.Round(legCourse - GlobalVars.CurrADHP.MagVar, 0)).ToString();                                    
                                    else
                                        txtBox_CirclingAreaEntranceDirection.Text = (Math.Round(legCourse, 1)).ToString();                                    
                                }
                            }
                            else
                            {
                                if (segmentLegUpperAltitude < VMManager.Instance.CA_OCA_Actual)
                                    break;
                                else
                                {
                                    finalSegmentLegsList.Add(leg);
                                    finalSegmentLegsTrajectoryPrjMerged.Add(legTrajectoryPrj);
                                    
                                    GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement);
                                    VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(finalSegmentLegsTrajectoryPrjMerged, ARANFunctions.RGB(0, 153, 76), 2);
                                    VMManager.Instance.FinalSegmentLegsPointsElements.Add(GlobalVars.gAranEnv.Graphics.DrawPoint(segmentLegStartPoint, ARANFunctions.RGB(76, 0, 153)));
                                    VMManager.Instance.FinalSegmentLegsPointsElements.Add(GlobalVars.gAranEnv.Graphics.DrawPoint(segmentLegEndPoint, ARANFunctions.RGB(76, 0, 153)));

                                    double deltaDistance = GlobalVars.unitConverter.DistanceToInternalUnits((double)nmrcUpDown_initialPositionAdjust.Maximum);
                                    if(segmentLegLowerAltitude >= VMManager.Instance.CA_OCA_Actual)
                                    {                                        
                                        deltaDistance += ARANFunctions.ReturnDistanceInMeters(segmentLegStartPoint, segmentLegEndPoint);                                        
                                    }
                                    else
                                    {
                                        double deltaHeight = segmentLegUpperAltitude - VMManager.Instance.CA_OCA_Actual;
                                        deltaDistance += deltaHeight / Math.Tan(ARANMath.DegToRad(Math.Abs((double)leg.VerticalAngle)));
                                    }
                                    nmrcUpDown_initialPositionAdjust.Maximum = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(deltaDistance);
                                }
                            }                            
                        }
                    }
                }
            }

            //Construct polygon to choose divergence VF from
            firstPoint = VMManager.Instance.TrackInitialPosition;
            BaseTrackInitialPosition = VMManager.Instance.TrackInitialPosition;
            lastPoint = finalSegmentLegsTrajectoryPrjMerged[finalSegmentLegsTrajectoryPrjMerged.Count - 1][finalSegmentLegsTrajectoryPrjMerged[finalSegmentLegsTrajectoryPrjMerged.Count - 1].Count - 1];
            pageHelper.constructDivergenceVFSelectionPolygon(BaseTrackInitialPosition, ARANFunctions.ReturnDistanceInMeters(firstPoint, lastPoint));
            cmbBox_DivergenceVF_SelectedIndexChanged(this, null);
            lbl_initialPositionAdjustRange.Text = "(" + nmrcUpDown_initialPositionAdjust.Minimum + " " + GlobalVars.unitConverter.DistanceUnit + " - " + nmrcUpDown_initialPositionAdjust.Maximum + " " + GlobalVars.unitConverter.DistanceUnit + ")";
            GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialDirectionElement, false);
            GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialDirectionElement, true);
            GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialPositionElement, false);
            GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.TrackInitialPositionElement, true);
        }

        private void nmrcUpDown_initialPositionAdjust_ValueChanged(object sender, EventArgs e)
        {
            double distanceTillTrackInitialPosition = GlobalVars.unitConverter.DistanceToInternalUnits((double) nmrcUpDown_initialPositionAdjust.Value);

            //VMManager.Instance.TrackInitialPosition = ARANFunctions.PointAlongPlane(finalSegmentLegsTrajectoryPrjMerged[0][0], VMManager.Instance.TrackInitialDirection, distanceTillTrackInitialPosition);            
            VMManager.Instance.TrackInitialPosition = ARANFunctions.PointAlongPlane(finalSegmentLegsTrajectoryPrjMerged[0][0], ARANFunctions.ReturnAngleInRadians(finalSegmentLegsTrajectoryPrjMerged[0][0], 
                finalSegmentLegsTrajectoryPrjMerged[finalSegmentLegsTrajectoryPrjMerged.Count - 1][finalSegmentLegsTrajectoryPrjMerged[finalSegmentLegsTrajectoryPrjMerged.Count - 1].Count - 1]), 
                distanceTillTrackInitialPosition);

            if (VMManager.Instance.TrackInitialDirectionElement > -1)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.TrackInitialDirectionElement);

            Point pnt = ARANFunctions.PointAlongPlane(VMManager.Instance.TrackInitialPosition, VMManager.Instance.TrackInitialDirection, 1500);
            MultiLineString mls = new MultiLineString();
            mls.Add(new LineString());
            mls[0].Add(VMManager.Instance.TrackInitialPosition);
            mls[0].Add(pnt);

            mls.Add(new LineString());
            mls[1].Add(ARANFunctions.PointAlongPlane(pnt, ARANMath.Modulus(VMManager.Instance.TrackInitialDirection - ARANMath.DegToRad(150), ARANMath.C_2xPI), 400));
            mls[1].Add(pnt);
            mls[1].Add(ARANFunctions.PointAlongPlane(pnt, ARANMath.Modulus(VMManager.Instance.TrackInitialDirection + ARANMath.DegToRad(150), ARANMath.C_2xPI), 400));

            VMManager.Instance.TrackInitialDirectionElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(mls, ARANFunctions.RGB(0, 0, 255), 2);

            if (VMManager.Instance.TrackInitialPositionElement > -1)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.TrackInitialPositionElement);

            VMManager.Instance.TrackInitialPositionElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.TrackInitialPosition, 255);

            double tempDist = 0;
            double tempAltitude = 0;
            for(int i = 0; i < finalSegmentLegsList.Count; i++)
            {
                for(int j = 0; j < i; j++)
                    tempDist += ConverterToSI.Convert(new ValDistance(finalSegmentLegsList[j].Length.Value, finalSegmentLegsList[j].Length.Uom), 0);
                if(distanceTillTrackInitialPosition >= tempDist && distanceTillTrackInitialPosition < tempDist + ConverterToSI.Convert(new ValDistance(finalSegmentLegsList[i].Length.Value, finalSegmentLegsList[i].Length.Uom), 0))
                {
                    tempAltitude = ConverterToSI.Convert(new ValDistanceVertical(finalSegmentLegsList[i].UpperLimitAltitude.Value, finalSegmentLegsList[i].UpperLimitAltitude.Uom), 0) -
                        (distanceTillTrackInitialPosition - tempDist) * Math.Tan(ARANMath.DegToRad(Math.Abs((double)finalSegmentLegsList[i].VerticalAngle)));
                    break;
                }
            }
            VMManager.Instance.TrackInitialPositionAltitude = tempAltitude;
            txtBox_initialPositionAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(tempAltitude, eRoundMode.CEIL).ToString();
        }

        private void MF_Page1_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedDivergenceVFElement);
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement, false);

                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement, false);
                for (int i = 0; i < VMManager.Instance.FinalSegmentLegsPointsElements.Count; i++)
                {
                    GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentLegsPointsElements[i], false);
                }
            }
            else
            {                
                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement, false);

                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement, false);
                for (int i = 0; i < VMManager.Instance.FinalSegmentLegsPointsElements.Count; i++)
                {
                    GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentLegsPointsElements[i], false);
                }

                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.DivergenceVFSelectionPolyElement, true);

                GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.finalSegmentLegsTrajectoryPrjMergedElement, true);
                for (int i = 0; i < VMManager.Instance.FinalSegmentLegsPointsElements.Count; i++)
                {
                    GlobalVars.gAranEnv.Graphics.ShowGraphic(VMManager.Instance.FinalSegmentLegsPointsElements[i], true);
                }
            }
        }

        public void cmbBox_DivergenceVF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBox_DivergenceVF.SelectedIndex == -1)
            {
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedDivergenceVFElement);
                mainForm.btn_Next.Enabled = false;
                lbl_message.Visible = true;
                return;
            }
            else
                lbl_message.Visible = false;
            mainForm.btn_Next.Enabled = true;
            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.SelectedDivergenceVFElement);
            VMManager.Instance.SelectedDivergenceVFElement = GlobalVars.gAranEnv.Graphics.DrawPointWithText(VMManager.Instance.DivergenceVFList[cmbBox_DivergenceVF.SelectedIndex].pShape, 255, VMManager.Instance.DivergenceVFList[cmbBox_DivergenceVF.SelectedIndex].Name);

            double dist = ARANFunctions.ReturnDistanceInMeters(BaseTrackInitialPosition, VMManager.Instance.DivergenceVFList[cmbBox_DivergenceVF.SelectedIndex].pShape);
            double angle = Math.Abs(ARANFunctions.ReturnAngleInRadians(BaseTrackInitialPosition, VMManager.Instance.DivergenceVFList[cmbBox_DivergenceVF.SelectedIndex].pShape) - 
                VMManager.Instance.TrackInitialDirection);
            if(angle > ARANMath.C_PI)
                angle = ARANMath.C_2xPI - angle;
            VMManager.Instance.TrackInitialPosition = ARANFunctions.PointAlongPlane(BaseTrackInitialPosition, VMManager.Instance.TrackInitialDirection, dist * Math.Cos(angle));
            //GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.initPointElement);
            //VMManager.Instance.initPointElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.TrackInitialPosition, 255);

            Point a = BaseTrackInitialPosition;


            double distanceTillTrackInitialPosition = ARANFunctions.ReturnDistanceInMeters(VMManager.Instance.TrackInitialPosition, finalSegmentLegsTrajectoryPrjMerged[0][0]);
            //nmrcUpDown_initialPositionAdjust.Value = (decimal)GlobalVars.unitConverter.DistanceToDisplayUnits(distanceTillTrackInitialPosition);
            
            Point pnt = ARANFunctions.PointAlongPlane(VMManager.Instance.TrackInitialPosition, VMManager.Instance.TrackInitialDirection, 1500);
            MultiLineString mls = new MultiLineString();
            mls.Add(new LineString());
            mls[0].Add(VMManager.Instance.TrackInitialPosition);
            mls[0].Add(pnt);

            mls.Add(new LineString());
            mls[1].Add(ARANFunctions.PointAlongPlane(pnt, ARANMath.Modulus(VMManager.Instance.TrackInitialDirection - ARANMath.DegToRad(150), ARANMath.C_2xPI), 400));
            mls[1].Add(pnt);
            mls[1].Add(ARANFunctions.PointAlongPlane(pnt, ARANMath.Modulus(VMManager.Instance.TrackInitialDirection + ARANMath.DegToRad(150), ARANMath.C_2xPI), 400));

            GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.TrackInitialDirectionElement);
            VMManager.Instance.TrackInitialDirectionElement = GlobalVars.gAranEnv.Graphics.DrawMultiLineString(mls, ARANFunctions.RGB(0, 0, 255), 2);

            if (VMManager.Instance.TrackInitialPositionElement > -1)
                GlobalVars.gAranEnv.Graphics.SafeDeleteGraphic(VMManager.Instance.TrackInitialPositionElement);

            VMManager.Instance.TrackInitialPositionElement = GlobalVars.gAranEnv.Graphics.DrawPoint(VMManager.Instance.TrackInitialPosition, 255);

            double tempDist = 0;
            double tempAltitude = 0;
            for (int i = 0; i < finalSegmentLegsList.Count; i++)
            {
                for (int j = 0; j < i; j++)
                    tempDist += ConverterToSI.Convert(new ValDistance(finalSegmentLegsList[j].Length.Value, finalSegmentLegsList[j].Length.Uom), 0);
                if (distanceTillTrackInitialPosition >= tempDist && distanceTillTrackInitialPosition < tempDist + ConverterToSI.Convert(new ValDistance(finalSegmentLegsList[i].Length.Value, finalSegmentLegsList[i].Length.Uom), 0))
                {
                    tempAltitude = ConverterToSI.Convert(new ValDistanceVertical(finalSegmentLegsList[i].UpperLimitAltitude.Value, finalSegmentLegsList[i].UpperLimitAltitude.Uom), 0) -
                        (distanceTillTrackInitialPosition - tempDist) * Math.Tan(ARANMath.DegToRad(Math.Abs((double)finalSegmentLegsList[i].VerticalAngle)));
                    break;
                }
            }
            VMManager.Instance.TrackInitialPositionAltitude = tempAltitude;
            txtBox_initialPositionAltitude.Text = GlobalVars.unitConverter.HeightToDisplayUnits(tempAltitude, eRoundMode.CEIL).ToString();

        }

        private void nmrcUpDown_visibilityDistance_ValueChanged(object sender, EventArgs e)
        {            
            VMManager.Instance.VisibilityDistance = GlobalVars.unitConverter.DistanceToInternalUnits((double)nmrcUpDown_visibilityDistance.Value);
            if (firstPoint == null)
                return;
            pageHelper.constructDivergenceVFSelectionPolygon(BaseTrackInitialPosition, ARANFunctions.ReturnDistanceInMeters(firstPoint, lastPoint));
            cmbBox_DivergenceVF_SelectedIndexChanged(this, null);
        }

        private void cmbBox_MAG_TRUE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBox_MAG_TRUE.SelectedIndex == 0)
                txtBox_CirclingAreaEntranceDirection.Text = (Math.Round(legCourse - GlobalVars.CurrADHP.MagVar, 0)).ToString();
            else
                txtBox_CirclingAreaEntranceDirection.Text = (Math.Round(legCourse, 1)).ToString();
        }

        private void cmbBox_OCH_OCA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBox_OCH_OCA.SelectedIndex == 0)
                txtBox_minOCA.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MinOCH, eRoundMode.CEIL).ToString();
            else
                txtBox_minOCA.Text = GlobalVars.unitConverter.HeightToDisplayUnits(VMManager.Instance.MinOCA, eRoundMode.CEIL).ToString();
        }
    }
}
