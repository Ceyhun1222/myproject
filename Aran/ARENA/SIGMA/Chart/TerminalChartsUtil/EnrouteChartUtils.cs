using ANCOR.MapCore;
using ANCOR.MapElements;
using ARENA;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using GeometryFunctions;
using OIS;
using PDM;
using SigmaChart.CmdsMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public static class EnrouteChartUtils
    {

        public static List<string> DpnList = new List<string>();
        public static List<string> NavaidList = new List<string>();


        public static void CreateRouteSegments_ChartElements(List<PDMObject> ENRT_featureList, string VertUom, string DistUOM, IFeatureClass AnnoRouteGeoFeatClass,
            IFeatureClass AnnoNavaidGeoFeatClass, IFeatureClass AnnoDesignatedGeoFeatClass, IMap FocusMap, ISpatialReference pSpatialReference, bool MagTrack = true)
        {
            object Missing = Type.Missing;

            string s = "";
            foreach (PDM.Enroute ENRT in ENRT_featureList)
            {
                if (ENRT.ID.ToString().CompareTo("00000000-0000-0000-0000-000000000000") == 0) continue;
                try
                {
                    #region Route - SegPoint

                    foreach (var RTSeg in ENRT.Routes)
                    {

                        s = RTSeg.GetObjectLabel();

                        if (RTSeg.SourceDetail != null && RTSeg.SourceDetail.StartsWith("skip")) continue;
                        if (ENRT.Lat.Contains("inArea")) RTSeg.CodeType = "inArea"; // метка, сделанная для AreaChart, указывающая что этот Enroute  имеет общую точку с процедурой STAR (SID)

                        if (ChecrRouteSegmantGeometry(RTSeg, AnnoRouteGeoFeatClass, ENRT.TxtDesig)) continue;

                        bool reverse = false;
                        if (RTSeg.Geo == null) RTSeg.RebuildGeo();
                        if (RTSeg.Geo == null) continue;


                        if (Double.IsNaN(RTSeg.ValMagTrack.Value) && RTSeg.ValTrueTrack.HasValue)
                        {
                            if (RTSeg.StartPoint.Geo == null) RTSeg.StartPoint.RebuildGeo();

                            if (RTSeg.StartPoint.Geo == null)
                            {
                                var pp = DataCash.GetPDMObject(RTSeg.StartPoint.ID, PDM_ENUM.WayPoint);
                                if (pp.Geo == null) pp.RebuildGeo();
                                RTSeg.StartPoint.Geo = pp.Geo;
                                //if (ArenaStatic.ArenaStaticProc.checkGuid(RTSeg.StartPoint.SegmentPointDesignator))
                                //{
                                //    RTSeg.StartPoint.SegmentPointDesignator = ((WayPoint)pp).Designator != null && ((WayPoint)pp).Designator.Length > 0 ? ((WayPoint)pp).Designator : RTSeg.StartPoint.SegmentPointDesignator;
                                //}
                            }

                            if (RTSeg.StartPoint.Geo != null)
                            {
                                double lat = (RTSeg.StartPoint.Geo as IPoint).Y;
                                double lon = (RTSeg.StartPoint.Geo as IPoint).X;
                                double? altitude = RTSeg.ConvertValueToMeter(RTSeg.ValDistVerLower.Value, RTSeg.UomValDistVerLower.ToString()) / 1000;
                                double magVar = ChartValidator.ExternalMagVariation.MagVar(lat, lon, altitude.Value,
                                                    RTSeg.ActualDate.Day, RTSeg.ActualDate.Month, RTSeg.ActualDate.Year, 1);
                                RTSeg.ValMagTrack = RTSeg.ValTrueTrack - magVar;

                                RTSeg.ValMagTrack = RTSeg.ValMagTrack > 360 ? RTSeg.ValMagTrack - 360 : RTSeg.ValMagTrack;
                                RTSeg.ValMagTrack = RTSeg.ValMagTrack < 0 ? RTSeg.ValMagTrack + 360 : RTSeg.ValMagTrack;
                            }
                        }



                        if (Double.IsNaN(RTSeg.ValReversMagTrack.Value) && RTSeg.ValReversTrueTrack.HasValue)
                        {
                            if (RTSeg.EndPoint.Geo == null) RTSeg.EndPoint.RebuildGeo();

                            if (RTSeg.EndPoint.Geo == null)
                            {
                                var pp = DataCash.GetPDMObject(RTSeg.EndPoint.ID, PDM_ENUM.WayPoint);
                                if(pp.Geo == null) pp.RebuildGeo();
                                RTSeg.EndPoint.Geo = pp.Geo;
                                //if (ArenaStatic.ArenaStaticProc.checkGuid(RTSeg.EndPoint.SegmentPointDesignator))
                                //{
                                //    RTSeg.EndPoint.SegmentPointDesignator = ((WayPoint)pp).Designator != null && ((WayPoint)pp).Designator.Length > 0 ? ((WayPoint)pp).Designator : RTSeg.EndPoint.SegmentPointDesignator;
                                //}

                            }


                            if (RTSeg.EndPoint.Geo != null)
                            {
                                double lat = (RTSeg.EndPoint.Geo as IPoint).Y;
                                double lon = (RTSeg.EndPoint.Geo as IPoint).X;
                                double? altitude = RTSeg.ConvertValueToMeter(RTSeg.ValDistVerLower.Value, RTSeg.UomValDistVerLower.ToString()) / 1000;
                                double magVar = ChartValidator.ExternalMagVariation.MagVar(lat, lon, altitude.Value,
                                                    RTSeg.ActualDate.Day, RTSeg.ActualDate.Month, RTSeg.ActualDate.Year, 1);
                                RTSeg.ValReversMagTrack = RTSeg.ValReversTrueTrack - magVar;

                                RTSeg.ValReversMagTrack = RTSeg.ValReversMagTrack > 360 ? RTSeg.ValReversMagTrack - 360 : RTSeg.ValReversMagTrack;
                                RTSeg.ValReversMagTrack = RTSeg.ValReversMagTrack < 0 ? RTSeg.ValReversMagTrack + 360 : RTSeg.ValReversMagTrack;
                            }

                        }



                        #region RouteSegment

                        ChartElement_SimpleText chrtEl_MagTrack = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RouteSegment_ValMagTrack");
                        ChartElement_SimpleText chrtEl_BackMagTrack = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RouteSegment_ValReversMagTrack");

                        if (!MagTrack)
                        {
                            chrtEl_MagTrack.TextContents[0][0].DataSource.Value = "ValTrueTrack";
                            chrtEl_BackMagTrack.TextContents[0][0].DataSource.Value = "ValReversTrueTrack";
                        }

                        ChartElement_RouteDesignator chrtEl_RouteSign = (ChartElement_RouteDesignator)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RouteSegment_sign");
                        ChartElement_SimpleText chrtEl_RouteLimits = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RouteSegment_UpperLowerLimit");


                        chrtEl_RouteSign.ReverseSign = false;

                        chrtEl_MagTrack.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_MagTrack.TextContents[0][0].DataSource, 0, 3);

                        RTSeg.ValLen = RTSeg.ValLen + 0.01;
                        chrtEl_BackMagTrack.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_BackMagTrack.TextContents[0][0].DataSource, 0, 3);

                        chrtEl_RouteSign.RouteDesignatorSource[0][0].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteSign.RouteDesignatorSource[0][0].DataSource);
                        chrtEl_RouteSign.TextContents[0][0].TextValue = ChartsHelperClass.MakeText_UOM(RTSeg, chrtEl_RouteSign.TextContents[0][0].DataSource,DistUOM);
                        chrtEl_RouteSign.RouteDesignatorSource[0][0].DataSource.Condition = RTSeg.SourceDetail;


                        #region chrtEl_RouteLimits

                        if (RTSeg.UomValDistVerUpper != PDM.UOM_DIST_VERT.FL)
                        {
                            chrtEl_RouteLimits.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[0][0].DataSource);
                            chrtEl_RouteLimits.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[0][1].DataSource);
                        }
                        else
                        {
                            chrtEl_RouteLimits.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[0][1].DataSource);
                            chrtEl_RouteLimits.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[0][0].DataSource);
                            chrtEl_RouteLimits.TextContents[0][0].Font.UnderLine = true;
                            chrtEl_RouteLimits.TextContents[0][1].Font.UnderLine = true;
                        }
                        chrtEl_RouteLimits.TextContents[0][2].TextValue = "";

                        if (RTSeg.UomValDistVerUpper == PDM.UOM_DIST_VERT.FL && RTSeg.ValDistVerUpper.HasValue && RTSeg.ValDistVerUpper.Value == 999)
                        {
                            chrtEl_RouteLimits.TextContents[0][0].TextValue = "_";
                            chrtEl_RouteLimits.TextContents[0][1].TextValue = "UNL";
                            chrtEl_RouteLimits.TextContents[0][2].TextValue = "_";
                        }


                        if (RTSeg.UomValDistVerLower != PDM.UOM_DIST_VERT.FL)
                        {
                            chrtEl_RouteLimits.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[1][0].DataSource);
                            chrtEl_RouteLimits.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[1][1].DataSource);
                            chrtEl_RouteLimits.TextContents[1][2].TextValue = "";
                        }
                        else
                        {
                            chrtEl_RouteLimits.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[1][1].DataSource);
                            chrtEl_RouteLimits.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(RTSeg, chrtEl_RouteLimits.TextContents[1][0].DataSource);
                            chrtEl_RouteLimits.TextContents[1][2].TextValue = "";
                        }



                        if (RTSeg.UomValDistVerLower == PDM.UOM_DIST_VERT.FL && RTSeg.ValDistVerLower.HasValue && RTSeg.ValDistVerLower.Value == 0)
                        {
                            chrtEl_RouteLimits.TextContents[0][0].TextValue = "";
                            chrtEl_RouteLimits.TextContents[0][1].TextValue = RTSeg.CodeDistVerLower.ToString();
                            chrtEl_RouteLimits.TextContents[0][2].TextValue = "";
                        }

                        #endregion


                        switch (RTSeg.CodeDir)
                        {
                            case PDM.CODE_ROUTE_SEGMENT_DIR.BOTH:
                                chrtEl_RouteSign.RouteSegmentDirection = routeSegmentDirection.Both;
                                break;
                            case PDM.CODE_ROUTE_SEGMENT_DIR.FORWARD:
                                chrtEl_RouteSign.RouteSegmentDirection = routeSegmentDirection.Forward;
                                break;
                            case PDM.CODE_ROUTE_SEGMENT_DIR.BACKWARD:
                                chrtEl_RouteSign.RouteSegmentDirection = routeSegmentDirection.Backward;
                                break;
                            case PDM.CODE_ROUTE_SEGMENT_DIR.OTHER:
                                chrtEl_RouteSign.RouteSegmentDirection = routeSegmentDirection.Both;
                                break;
                            default:
                                break;
                        }


                        ILine ln = new LineClass();

                        IPoint startP = ((IPolyline)RTSeg.Geo).FromPoint;
                        IPoint endP = ((IPolyline)RTSeg.Geo).ToPoint;
                        IPoint cntrP = ChartElementsManipulator.getPointOnLine(startP, endP, 50, FocusMap, pSpatialReference);


                        ln.FromPoint = startP;
                        ln.ToPoint = endP;

                        if (startP.X < endP.X)
                        {
                            ln.FromPoint = startP;
                            ln.ToPoint = endP;
                            //reverse = true;
                        }
                        else
                        {
                            ln.FromPoint = endP;
                            ln.ToPoint = startP;
                            reverse = true;
                        }


                        if (!reverse)
                        {
                            //chrtEl_MagTrack.Anchor.X = chrtEl_MagTrack.Anchor.X * -1;

                            //chrtEl_BackMagTrack.Anchor.X = chrtEl_BackMagTrack.Anchor.X * -1;

                            //chrtEl_RouteSign.ReverseSign = reverse;

                        }



                        chrtEl_RouteSign.ReverseSign = reverse;


                        double slope = ChartElementsManipulator.GetLineSlopE(ln, FocusMap, pSpatialReference) * 180 / Math.PI;
                        //slope = slope > 180 ? slope - 360 : slope;


                        chrtEl_MagTrack.Slope = slope;
                        chrtEl_BackMagTrack.Slope = slope;
                        chrtEl_RouteSign.Slope = slope;
                        chrtEl_RouteLimits.Slope = slope;

                        IElement el_Track = (IElement)chrtEl_MagTrack.ConvertToIElement();
                        IElement el_backTrack = (IElement)chrtEl_BackMagTrack.ConvertToIElement();
                        IElement el_limits = (IElement)chrtEl_RouteLimits.ConvertToIElement();

                        IElement el_rtSign = (IElement)chrtEl_RouteSign.ConvertToIElement();




                        Polyline PolyLn = new PolylineClass();
                        ISegmentCollection SegCol = (ISegmentCollection)PolyLn;


                        SegCol.AddSegment((ISegment)ln, ref Missing, ref Missing);

                        ChartsHelperClass.saveRouteSegment_ChartRouteGeo(AnnoRouteGeoFeatClass, ENRT.TxtDesig, RTSeg, (IGeometry)SegCol); ///!!!!!!


                        el_Track.Geometry = ChartElementsManipulator.getPointOnLine(startP, endP, 10, FocusMap, pSpatialReference);

                        el_backTrack.Geometry = ChartElementsManipulator.getPointOnLine(endP, startP, 10, FocusMap, pSpatialReference);


                        el_limits.Geometry = cntrP;


                        IGroupElement GrEl = el_rtSign as IGroupElement;
                        for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                        {
                            GrEl.get_Element(i).Geometry = cntrP; //RTSeg.Geo;
                        }


                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_RouteSign.Name, RTSeg.ID, el_rtSign, ref chrtEl_RouteSign, chrtEl_RouteSign.Id, FocusMap.MapScale);
                        Application.DoEvents();
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_RouteLimits.Name, RTSeg.ID, el_limits, ref chrtEl_RouteLimits, chrtEl_RouteLimits.Id, FocusMap.MapScale);
                        Application.DoEvents();

                        switch (RTSeg.CodeDir)
                        {
                            case PDM.CODE_ROUTE_SEGMENT_DIR.BOTH:
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_MagTrack.Name, RTSeg.ID, el_Track, ref chrtEl_MagTrack, chrtEl_MagTrack.Id, FocusMap.MapScale);
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_BackMagTrack.Name, RTSeg.ID, el_backTrack, ref chrtEl_BackMagTrack, chrtEl_BackMagTrack.Id, FocusMap.MapScale);
                                break;
                            case PDM.CODE_ROUTE_SEGMENT_DIR.FORWARD:
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_MagTrack.Name, RTSeg.ID, el_Track, ref chrtEl_MagTrack, chrtEl_MagTrack.Id, FocusMap.MapScale);
                                break;
                            case PDM.CODE_ROUTE_SEGMENT_DIR.BACKWARD:
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_BackMagTrack.Name, RTSeg.ID, el_backTrack, ref chrtEl_BackMagTrack, chrtEl_BackMagTrack.Id, FocusMap.MapScale);
                                break;
                            case PDM.CODE_ROUTE_SEGMENT_DIR.OTHER:
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_MagTrack.Name, RTSeg.ID, el_Track, ref chrtEl_MagTrack, chrtEl_MagTrack.Id, FocusMap.MapScale);
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_BackMagTrack.Name, RTSeg.ID, el_backTrack, ref chrtEl_BackMagTrack, chrtEl_BackMagTrack.Id, FocusMap.MapScale);
                                break;
                            default:
                                break;
                        }


                        if (chrtEl_RouteSign.HideDesignatorText)
                        {
                            ChartElement_SimpleText chrtDesignatorText = ChartElementsManipulator.ConstructDesignatorElement(chrtEl_RouteSign);
                            chrtDesignatorText.Tag = chrtEl_RouteSign.Id.ToString();
                            IElement el_rtDesignatorText = (IElement)chrtDesignatorText.ConvertToIElement();
                            el_rtDesignatorText.Geometry = cntrP;
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtDesignatorText.Name, RTSeg.ID, el_rtDesignatorText, ref chrtDesignatorText, chrtDesignatorText.Id, FocusMap.MapScale);

                        }

                        Application.DoEvents();

                        #endregion



                        #region SegPoint

                        if (RTSeg.StartPoint != null)
                        {
                            #region PDM.PointChoice.DesignatedPoint

                            if (RTSeg.StartPoint.PointChoice == PDM.PointChoice.DesignatedPoint)
                            {
                                Guid guidOutput;
                                if (!Guid.TryParse(RTSeg.StartPoint.SegmentPointDesignator, out guidOutput))
                                {
                                    if (DpnList.IndexOf(RTSeg.StartPoint.PointChoiceID) < 0)
                                    {
                                        DpnList.Add(RTSeg.StartPoint.PointChoiceID);

                                        ChartElement_SigmaCollout_Designatedpoint chrtEl_DesigPoint = (ChartElement_SigmaCollout_Designatedpoint)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Designatedpoint");
                                        IElement el = ChartsHelperClass.CreateSegmentPointAnno(RTSeg.StartPoint, chrtEl_DesigPoint);
                                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_DesigPoint.Name, RTSeg.StartPoint.ID, el, ref chrtEl_DesigPoint, chrtEl_DesigPoint.Id, FocusMap.MapScale);
                                        ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(AnnoDesignatedGeoFeatClass, RTSeg.StartPoint, RTSeg.StartPoint.Geo);
                                    }
                                }
                            }

                            #endregion

                            #region PDM.PointChoice.Navaid

                            if (RTSeg.StartPoint.PointChoice == PDM.PointChoice.Navaid)
                            {

                                if (NavaidList.IndexOf(RTSeg.StartPoint.PointChoiceID) < 0)
                                {
                                    NavaidList.Add(RTSeg.StartPoint.PointChoiceID);

                                    var _NAVAID = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                   where (element != null)
                                                       && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                       && (element.ID.StartsWith(RTSeg.StartPoint.PointChoiceID) || ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(RTSeg.StartPoint.PointChoiceID.Trim()))
                                                   select element).FirstOrDefault();

                                    if (_NAVAID == null) continue;

                                    ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                                    IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, VertUom);
                                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, _NAVAID.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                                    ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(AnnoNavaidGeoFeatClass, (PDM.NavaidSystem)_NAVAID);
                                }
                            }

                            #endregion


                            Application.DoEvents();

                        }

                        if (RTSeg.EndPoint != null)
                        {
                            #region PDM.PointChoice.DesignatedPoint

                            if (RTSeg.EndPoint.PointChoice == PDM.PointChoice.DesignatedPoint)
                            {
                                Guid guidOutput;
                                if (!Guid.TryParse(RTSeg.EndPoint.SegmentPointDesignator, out guidOutput))
                                {
                                    if (DpnList.IndexOf(RTSeg.EndPoint.PointChoiceID) < 0)
                                    {
                                        DpnList.Add(RTSeg.EndPoint.PointChoiceID);

                                        ChartElement_SigmaCollout_Designatedpoint chrtEl_DesigPoint = (ChartElement_SigmaCollout_Designatedpoint)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Designatedpoint");
                                        IElement el = ChartsHelperClass.CreateSegmentPointAnno(RTSeg.EndPoint, chrtEl_DesigPoint);
                                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_DesigPoint.Name, RTSeg.EndPoint.ID, el, ref chrtEl_DesigPoint, chrtEl_DesigPoint.Id, FocusMap.MapScale);
                                        ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(AnnoDesignatedGeoFeatClass, RTSeg.EndPoint, RTSeg.EndPoint.Geo);
                                    }
                                }
                            }
                            #endregion

                            #region PDM.PointChoice.Navaid

                            if (RTSeg.EndPoint.PointChoice == PDM.PointChoice.Navaid)
                            {

                                if (NavaidList.IndexOf(RTSeg.EndPoint.PointChoiceID) < 0)
                                {
                                    NavaidList.Add(RTSeg.EndPoint.PointChoiceID);

                                    var _NAVAID = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                   where (element != null)
                                                       && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                       && (element.ID.StartsWith(RTSeg.EndPoint.PointChoiceID) || ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(RTSeg.EndPoint.PointChoiceID.Trim()))
                                                   select element).FirstOrDefault();

                                    if (_NAVAID == null) continue;

                                    ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                                    IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, VertUom);
                                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, _NAVAID.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                                    ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(AnnoNavaidGeoFeatClass, (PDM.NavaidSystem)_NAVAID);
                                }
                            }

                            #endregion

                            Application.DoEvents();

                        }



                        #endregion


                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(ENRT.GetObjectLabel() + (char)9 + ENRT.ID + (char)9  + s + (char)9 + ex.Message + (char)9 + ex.Source);
                    continue;
                }
            }




        }

        public static void CreateHolding_ChartElements(List<PDMObject> HlngFeatureList, string vertUom, string distUom, IFeatureClass AnnoHoldingGeoFeatClass, 
            IFeatureClass AnnoDesignatedGeoFeatClass, IFeatureClass AnnoNavaidGeoFeatClass, IMap FocusMap, ISpatialReference pSpatialReference)
        {

            foreach (PDM.HoldingPattern hlng in HlngFeatureList)
            {

                try
                {

                    #region

                    if (hlng.HoldingPoint == null) continue;
                    if (hlng.HoldingPoint.Geo == null) hlng.HoldingPoint.RebuildGeo();
                    PDMObject pdmObj = ChartsHelperClass.SaveHolding_PointChartGeo(AnnoHoldingGeoFeatClass, hlng);

                    ChartElement_SigmaCollout_Designatedpoint chrtEl_HldPnt = null;


                    if (hlng.HoldingPoint.PointChoice == PDM.PointChoice.DesignatedPoint)
                    {
                        if (DpnList.IndexOf(hlng.HoldingPoint.PointChoiceID) < 0)
                        {

                            DpnList.Add(hlng.HoldingPoint.PointChoiceID);
                            ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(AnnoDesignatedGeoFeatClass, hlng.HoldingPoint, hlng.HoldingPoint.Geo);

                            chrtEl_HldPnt = (ChartElement_SigmaCollout_Designatedpoint)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Designatedpoint");
                            IElement el = ChartsHelperClass.CreateSegmentPointAnno(hlng.HoldingPoint, chrtEl_HldPnt);
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldPnt.Name, hlng.HoldingPoint.PointChoiceID, el, ref chrtEl_HldPnt, chrtEl_HldPnt.Id, FocusMap.MapScale);
                            Application.DoEvents();
                        }
                    }

                    if (hlng.HoldingPoint.PointChoice == PDM.PointChoice.Navaid)
                    {


                        if (NavaidList.IndexOf(hlng.HoldingPoint.PointChoiceID) < 0 && pdmObj != null)
                        {

                            NavaidList.Add(hlng.HoldingPoint.PointChoiceID);
                            ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(AnnoNavaidGeoFeatClass, (PDM.NavaidSystem)pdmObj);

                            ChartElement_SigmaCollout_Navaid chrtEl_HldPnt_NAV = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                            IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)pdmObj, chrtEl_HldPnt_NAV, vertUom);
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldPnt_NAV.Name, hlng.HoldingPoint.PointChoiceID, el, ref chrtEl_HldPnt_NAV, chrtEl_HldPnt_NAV.Id, FocusMap.MapScale);
                            Application.DoEvents();
                        }
                    }

                    ///// limits
                    ChartElement_SimpleText chrtEl_Hldng = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPattern");

                    chrtEl_Hldng.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[0][0].DataSource);
                    chrtEl_Hldng.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[0][1].DataSource);

                    if (hlng.UpperLimitUOM == UOM_DIST_VERT.FL)
                    {
                        var ds = chrtEl_Hldng.TextContents[1][0].DataSource.Clone();
                        chrtEl_Hldng.TextContents[1][0].DataSource = chrtEl_Hldng.TextContents[1][1].DataSource;
                        chrtEl_Hldng.TextContents[1][1].DataSource = (AncorDataSource)ds;
                    }

                    chrtEl_Hldng.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[1][0].DataSource);
                    chrtEl_Hldng.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[1][1].DataSource);

                    chrtEl_Hldng.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[2][0].DataSource, 1);
                    chrtEl_Hldng.TextContents[2][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_Hldng.TextContents[2][1].DataSource);


                    chrtEl_Hldng.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
                    if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_Hldng.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;

                    chrtEl_Hldng.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_Hldng.Slope);

                    IElement hldng_lim_el = (IElement)chrtEl_Hldng.ConvertToIElement();
                    hldng_lim_el.Geometry = hlng.HoldingPoint.Geo;

                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Hldng.Name, hlng.ID, hldng_lim_el, ref chrtEl_Hldng, chrtEl_Hldng.Id, FocusMap.MapScale);



                    double magVar;
                    try
                    {
                        double? altitude = hlng.ConvertValueToMeter(hlng.LowerLimit.Value, hlng.LowerLimitUOM.ToString()) / 1000;
                        magVar = ChartValidator.ExternalMagVariation.MagVar(Convert.ToDouble(hlng.HoldingPoint.Y.Value), Convert.ToDouble(hlng.HoldingPoint.X.Value), altitude.Value,
                                            hlng.ActualDate.Day, hlng.ActualDate.Month, hlng.ActualDate.Year, 1);
                    }
                    catch { magVar = 0; }


                    ///// InboundCourse
                    ChartElement_SimpleText chrtEl_HldngCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternInboundCource");

                    chrtEl_HldngCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource, 0, 0, magVar);
                    if (hlng.OutboundCourseType != CodeCourse.MAG_BRG || hlng.OutboundCourseType != CodeCourse.MAG_TRACK)
                        chrtEl_HldngCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource);


                    chrtEl_HldngCource.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
                    if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngCource.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;
                    chrtEl_HldngCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngCource.Slope);

                    IElement hldng_cource_el = (IElement)chrtEl_HldngCource.ConvertToIElement();
                    hldng_cource_el.Geometry = ChartElementsManipulator.getPointAlongDirection(hlng.HoldingPoint.Geo as IPoint, 90 - hlng.OutboundCourse.Value, 1000, FocusMap, pSpatialReference);

                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldngCource.Name, hlng.ID, hldng_cource_el, ref chrtEl_HldngCource, chrtEl_HldngCource.Id, FocusMap.MapScale);


                    ////////////////////////////////////

                    ///// OutboundCourse
                    chrtEl_HldngCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternOutboundCource");

                    chrtEl_HldngCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource, 0, 0, magVar);
                    if (hlng.OutboundCourseType != CodeCourse.MAG_BRG || hlng.OutboundCourseType != CodeCourse.MAG_TRACK)
                        chrtEl_HldngCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(hlng, chrtEl_HldngCource.TextContents[0][0].DataSource);

                    chrtEl_HldngCource.Slope = hlng.InboundCourse != null ? 90 - hlng.InboundCourse.Value : 0;
                    if (hlng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngCource.Slope = hlng.OutboundCourse != null ? 90 - hlng.OutboundCourse.Value : 0;
                    chrtEl_HldngCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngCource.Slope);


                    hldng_cource_el = (IElement)chrtEl_HldngCource.ConvertToIElement();
                    hldng_cource_el.Geometry = ChartElementsManipulator.getPointAlongDirection(hlng.HoldingPoint.Geo as IPoint, 90 - hlng.OutboundCourse.Value, -1000, FocusMap, pSpatialReference);

                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_HldngCource.Name, hlng.ID, hldng_cource_el, ref chrtEl_HldngCource, chrtEl_HldngCource.Id, FocusMap.MapScale);
                    ////////////////////////////////////


                    #endregion
                }
                catch (Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(hlng.GetObjectLabel() + (char)9 + hlng.ID + (char)9 + ex.Message + (char)9 + ex.Source);
                    continue;
                }

            }



        }


        public static void CreateNavaids_ChartElements(List<PDMObject> nvdsList, string vertUom, IFeatureClass AnnoNavaidGeoFeatClass,  IMap FocusMap)
        {
            foreach (NavaidSystem navSystem in nvdsList)
            {
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.ILS) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.ILS_DME) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.LOC) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.LOC_DME) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.MKR) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.MLS) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.MLS_DME) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.TLS) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.SDF) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.DF) continue;
                if (navSystem.CodeNavaidSystemType == NavaidSystemType.OTHER) continue;


                try
                {
                    if (NavaidList.IndexOf(navSystem.ID) < 0)
                    {

                        NavaidList.Add(navSystem.ID);

                        ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                        IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)navSystem, chrtEl_Navaid, vertUom);
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navSystem.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                        ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(AnnoNavaidGeoFeatClass, (PDM.NavaidSystem)navSystem);
                    }
                }
                catch(Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(navSystem.GetObjectLabel() + (char)9 + navSystem.ID + (char)9 + ex.Message + (char)9 + ex.Source);
                    continue;
                }
            }
        }

        public static bool ChecrRouteSegmantGeometry(RouteSegment rtSegmant, IFeatureClass _RouteGeoFeatClass, string ENRT_TxtDesig)
        {
            bool res = false;

            string startDesig = rtSegmant.StartPoint.SegmentPointDesignator;
            string endDesig = rtSegmant.EndPoint.SegmentPointDesignator;

            if (ArenaStatic.ArenaStaticProc.checkGuid(startDesig) && ArenaStatic.ArenaStaticProc.checkGuid(endDesig))
            {
                PDMObject pdmO = DataCash.GetPDMObject(startDesig);
                if (pdmO != null) startDesig = pdmO.GetObjectLabel();
                else startDesig = "";

                pdmO = DataCash.GetPDMObject(endDesig);
                if (pdmO != null) endDesig = pdmO.GetObjectLabel();
                else endDesig = "";

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "designator = '" + startDesig + " : " + endDesig + "'";

                IFeatureCursor featCur = _RouteGeoFeatClass.Search(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    int FID = feat.Fields.FindField("RouteFormedID");
                    if (FID >= 0)
                    {
                        string RF = feat.Value[FID].ToString();
                        RF = RF + rtSegmant.ID + "/";
                        feat.set_Value(FID, RF);
                        feat.Store();
                        string FGuid = feat.get_Value(feat.Fields.FindField("FeatureGUID")).ToString();
                        string Direction = feat.get_Value(feat.Fields.FindField("codeDir")).ToString();

                        var RouteSignAnno = SigmaDataCash.ChartElementList.FindAll(sEl => sEl is ChartElement_RouteDesignator && ((ChartElement_RouteDesignator)sEl).LinckedGeoId.CompareTo(FGuid) == 0).FirstOrDefault();
                        ChartElement_RouteDesignator _routeSign = (ChartElement_RouteDesignator)RouteSignAnno;

                        AncorChartElementWord wrd = _routeSign.RouteDesignatorSource[_routeSign.RouteDesignatorSource.Count - 1][0].Clone() as AncorChartElementWord;
                        wrd.TextValue = ENRT_TxtDesig;
                        List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); 
                        txtLine.Add(wrd);
                        _routeSign.RouteDesignatorSource.Add(txtLine);

                        if (Direction.CompareTo(rtSegmant.CodeDir.ToString()) != 0)
                            _routeSign.RouteSegmentDirection = routeSegmentDirection.Both;


                        IElement el = _routeSign.ConvertToIElement() as IElement;
                        ChartElementsManipulator.UpdateSingleElementToDataSet(_routeSign.Name, _routeSign.Id.ToString(), el, ref _routeSign, false);

                        res = true;
                    }
                }

                Marshal.ReleaseComObject(featCur);
            }

            return res;

        }


    }
}
