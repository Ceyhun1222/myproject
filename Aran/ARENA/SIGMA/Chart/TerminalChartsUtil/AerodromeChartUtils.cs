using ANCOR.MapCore;
using ANCOR.MapElements;
using AranSupport;
using ARENA;
using ArenaStatic;
using ChartDataTabulation;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
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
    public static class AerodromeChartUtils
    {
        public static void CreateRunwayCartography(List<Runway> runwayList, IMap FocusMap, double? magVarArp ,ISpatialReference pSpatialReference,int _chartType, IFeatureClass anno_RunwayElementCartography_featClass,
            IFeatureClass anno_RunwayProtectedAreaCartography_featClass, IFeatureClass anno_RunwayDirectionCenterLinePointCartography_featClass, IFeatureClass anno_DecorLineCartography_featClass,
            IFeatureClass anno_LightElementCartography_featClass, IFeatureClass Anno_TaxiHoldingPositionCartography_featClass, IFeatureClass anno_RunwayVisualRangeCartography_featClass, UOM_DIST_HORZ DistUom, UOM_DIST_VERT VertUom, ref List<GuidanceLine> GdnLnLst,
            ref List<MarkingElement> MrkLst)
        {
            if (runwayList == null) return;

            double rotationValue = Double.Parse(FocusMap.Description);

            
            foreach (Runway rwy in runwayList)
            {
                try
                {

                    if (rwy.RunwayMarkingList!=null && rwy.RunwayMarkingList.Count > 0)
                    {
                        foreach (var item in rwy.RunwayMarkingList)
                        {
                            if (item.MarkingElementList != null && item.MarkingElementList.Count > 0) MrkLst.AddRange(item.MarkingElementList);
                        }
                    }

                    if (rwy.RunwayElementsList == null) continue;

                    
                    foreach (var rwyEl in rwy.RunwayElementsList)
                    {
                        IGeometry rwyElGeometry = Save_ChartGeo(anno_RunwayElementCartography_featClass, FocusMap, rwyEl, "RunwayElement", pSpatialReference);

                        if (rwyEl.RunwayElementType != CodeRunwayElementType.NORMAL) continue;

                        ChartElement_TextArrow chrtEl_rwy = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RunwayElement");

                       if (_chartType == 9)
                            chrtEl_rwy = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ParkingDockingRunwayElement");
                        if (rwyElGeometry != null)
                        {
                            if (_chartType != 9)
                            {
                                if (rwyEl.Width.HasValue && rwyEl.Length.HasValue && !Double.IsNaN(rwyEl.Width.Value) && !Double.IsNaN(rwyEl.Length.Value))
                                {
                                    chrtEl_rwy.TextContents[0][0].TextValue = ChartsHelperClass.MakeText_UOM(rwyEl, chrtEl_rwy.TextContents[0][0].DataSource, DistUom.ToString());
                                    chrtEl_rwy.TextContents[0][1].TextValue = DistUom.ToString().ToLower();

                                    chrtEl_rwy.TextContents[0][2].TextValue = ChartsHelperClass.MakeText_UOM(rwyEl, chrtEl_rwy.TextContents[0][2].DataSource, DistUom.ToString());
                                    chrtEl_rwy.TextContents[0][3].TextValue = DistUom.ToString().ToLower();
                                    chrtEl_rwy.TextContents[0][4].TextValue = ChartsHelperClass.MakeText(rwyEl, chrtEl_rwy.TextContents[0][4].DataSource);

                                }
                            }
                            else
                            {
                                chrtEl_rwy.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(rwyEl, chrtEl_rwy.TextContents[0][0].DataSource);
                            }

                            chrtEl_rwy.LinckedGeoId = rwyEl.ID;

                            chrtEl_rwy.Slope = rotationValue * -1;

                            IPoint cntr = (rwyElGeometry as IArea).Centroid;
                            chrtEl_rwy.Anchor = new AncorPoint(cntr.X, cntr.Y);

                            IElement el_anno = (IElement)chrtEl_rwy.ConvertToIElement();

                            el_anno.Geometry = cntr;
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_rwy.Name, rwyEl.ID, el_anno, ref chrtEl_rwy, chrtEl_rwy.Id, FocusMap.MapScale);
                        }

                        #region save RWY STRIP

                        if (rwy.StripProperties != null && rwy.StripProperties.WidthStrip.HasValue && rwy.StripProperties.LengthStrip.HasValue)
                        {
                            foreach (var rdn in rwy.RunwayDirectionList)
                            {

                                if (rdn.RwyProtectArea != null)
                                {
                                    var RWY_STRIP = (from element in rdn.RwyProtectArea
                                                   where (element != null) && (element.Length == rwy.StripProperties.LengthStrip.Value &&
                element.Width == rwy.StripProperties.WidthStrip.Value && element.Width_UOM == rwy.StripProperties.Strip_UOM && element.Length_UOM == rwy.StripProperties.Strip_UOM)
                                                   select element).FirstOrDefault();

                                    if (RWY_STRIP != null)
                                    {
                                        string protectedAreaId = "";
                                        
                                            IGeometry StripGeo = Save_RwyStripGeo(anno_RunwayProtectedAreaCartography_featClass, FocusMap, rdn.ID, RWY_STRIP.Width.Value, RWY_STRIP.Length.Value, RWY_STRIP.Length_UOM.ToString(), ref protectedAreaId);

                                        if (rwyElGeometry != null)
                                        {
                                            ChartElement_TextArrow chrtEl_StripRwy = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RunwayStripElement");

                                            chrtEl_StripRwy.TextContents[0][0].TextValue = ChartsHelperClass.MakeText_UOM(RWY_STRIP, chrtEl_StripRwy.TextContents[0][0].DataSource, DistUom.ToString(), 0);
                                            chrtEl_StripRwy.TextContents[0][1].TextValue = DistUom.ToString();

                                            chrtEl_StripRwy.TextContents[0][2].TextValue = ChartsHelperClass.MakeText_UOM(RWY_STRIP, chrtEl_StripRwy.TextContents[0][2].DataSource, DistUom.ToString(), 0);
                                            chrtEl_StripRwy.TextContents[0][3].TextValue = DistUom.ToString();

                                            chrtEl_StripRwy.LinckedGeoId = protectedAreaId;

                                            chrtEl_StripRwy.Slope = rotationValue * -1;//((IActiveView)FocusMap).ScreenDisplay.DisplayTransformation.Rotation * -1;

                                            IPoint cntr = (StripGeo as IPolygon).Envelope.UpperLeft;
                                            chrtEl_StripRwy.Anchor = new AncorPoint(cntr.X, cntr.Y);

                                            IElement el_anno = (IElement)chrtEl_StripRwy.ConvertToIElement();



                                            el_anno.Geometry = cntr;
                                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_StripRwy.Name, protectedAreaId, el_anno, ref chrtEl_StripRwy, chrtEl_StripRwy.Id, FocusMap.MapScale);
                                        }




                                        break;
                                    }

                                }

                            }
                        }

                        #endregion

                        if (rwy.TaxiHoldingPositionList!=null && rwy.TaxiHoldingPositionList.Count > 0)
                        {
                            foreach (var taxiHlpdPos in rwy.TaxiHoldingPositionList)
                            {
                                Save_ChartGeo(Anno_TaxiHoldingPositionCartography_featClass, FocusMap, taxiHlpdPos, "TaxiHoldingPosition", pSpatialReference);

                                if(taxiHlpdPos.TaxiHoldingMarkingList !=null && taxiHlpdPos.TaxiHoldingMarkingList.Count >0)
                                {
                                    foreach (var item in taxiHlpdPos.TaxiHoldingMarkingList)
                                    {
                                        if (item.MarkingElementList != null && item.MarkingElementList.Count > 0)
                                            MrkLst.AddRange(item.MarkingElementList);
                                    }
                                }

                            }
                        }

                    }


                    if (rwy.RunwayDirectionList == null) continue;

                    #region CenterLinePointList (THR, TDZ)

                    foreach (var thr in rwy.RunwayDirectionList)
                    {
                        
                        if (thr.CenterLinePoints != null)
                        {
                            var RWY_THR_TDZ = (from element in thr.CenterLinePoints
                                               where (element != null && (element.Role == CodeRunwayCenterLinePointRoleType.THR || element.Role == CodeRunwayCenterLinePointRoleType.TDZ))
                                               select element).ToList();

                            if (RWY_THR_TDZ != null)
                            {
                                foreach (var cpl in RWY_THR_TDZ)
                                {

                                    IGeometry EL_Geometry = Save_ChartGeo(anno_RunwayDirectionCenterLinePointCartography_featClass, FocusMap, cpl, "RunwayDirectionCenterLinePoint", pSpatialReference);

                                    ChartElement_SigmaCollout_Navaid chrtEl_thr = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RunwayTHRElement");
                                    if (cpl.Role == CodeRunwayCenterLinePointRoleType.TDZ)
                                        chrtEl_thr = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RunwayTDZElement");
                                    
                                    if (EL_Geometry != null)
                                    {
                                        IPoint TDZ_PNT = new PointClass();
                                        TDZ_PNT.PutCoords(((IPoint)EL_Geometry).X - chrtEl_thr.Anchor.X, ((IPoint)EL_Geometry).Y - chrtEl_thr.Anchor.Y);

                                        chrtEl_thr.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(cpl, chrtEl_thr.TextContents[0][0].DataSource);
                                        chrtEl_thr.TextContents[0][1].TextValue = ChartsHelperClass.MakeText_UOM(cpl, chrtEl_thr.TextContents[0][1].DataSource, VertUom.ToString(), 0);

                                        chrtEl_thr.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(cpl, chrtEl_thr.TextContents[1][0].DataSource, coordtype.DDMMSS_SS_2,10);
                                        chrtEl_thr.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(cpl, chrtEl_thr.TextContents[1][1].DataSource, coordtype.DDMMSS_SS_2,10);

                                        chrtEl_thr.LinckedGeoId = cpl.ID;

                                        chrtEl_thr.Slope = rotationValue * -1;

                                        chrtEl_thr.Anchor = new AncorPoint(((IPoint)EL_Geometry).X, ((IPoint)EL_Geometry).Y);
                                        //chrtEl_thr.Anchor = new AncorPoint(0,0);

                                        #region Caption Text

                                        chrtEl_thr.HasHeader = false;
                                        chrtEl_thr.CaptionTextLine.Remove(chrtEl_thr.CaptionTextLine[0]);

                                        #endregion


                                        #region BottomText

                                        chrtEl_thr.BottomTextLine.Remove(chrtEl_thr.BottomTextLine[0]);
                                        chrtEl_thr.HasFooter = false;

                                        #endregion


                                        chrtEl_thr.MorseTextLine = null;

                                        IElement el_anno = (IElement)chrtEl_thr.ConvertToIElement();

                                        if (el_anno is IGroupElement)
                                        {
                                            IGroupElement GrEl = el_anno as IGroupElement;
                                            for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                                            {
                                                GrEl.get_Element(i).Geometry = TDZ_PNT;
                                            }
                                        }
                                        else el_anno.Geometry = TDZ_PNT;

                                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_thr.Name, cpl.ID, el_anno, ref chrtEl_thr, chrtEl_thr.Id, FocusMap.MapScale);

                                        
                                    }

                                    if (cpl.Role == CodeRunwayCenterLinePointRoleType.THR && EL_Geometry !=null)
                                    {
                                        ChartElement_SimpleText chrtEl_rdn = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RDNElement");

                                        IPoint RDN_PNT = new PointClass();
                                        RDN_PNT.PutCoords(((IPoint)EL_Geometry).X - chrtEl_rdn.Anchor.X, ((IPoint)EL_Geometry).Y - chrtEl_rdn.Anchor.Y);

                                        chrtEl_rdn.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(thr, chrtEl_rdn.TextContents[0][1].DataSource, Rounder: 0);
                                        if (chrtEl_rdn.TextContents[0][1].TextValue.CompareTo("NaN") == 0 && thr.TrueBearing.HasValue)
                                            chrtEl_rdn.TextContents[0][1].TextValue = magVarArp.HasValue ? (Math.Round( thr.TrueBearing.Value + magVarArp.Value,0)).ToString() : (thr.TrueBearing.Value).ToString();

                                        if (thr.TrueBearing.HasValue)
                                        {
                                            if (thr.TrueBearing.Value > 0 && thr.TrueBearing.Value <= 180)
                                                chrtEl_rdn.TextContents[0][2].Visible = true;
                                            else
                                                chrtEl_rdn.TextContents[0][0].Visible = true;
                                        }

                                        chrtEl_rdn.LinckedGeoId = cpl.ID;

                                        chrtEl_rdn.Slope = rotationValue * -1;

                                        IElement el_anno = (IElement)chrtEl_rdn.ConvertToIElement();

                                        el_anno.Geometry = RDN_PNT;

                                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_rdn.Name, cpl.ID, el_anno, ref chrtEl_rdn, chrtEl_rdn.Id, FocusMap.MapScale);

                                    }

                                    

                                }
                            }

                            var clp_gdn = (from element in thr.CenterLinePoints
                                               where (element != null && element.GuidanceLineList !=null)
                                               select element).ToList();

                            foreach (var clp in clp_gdn)
                            {
                                if (clp.GuidanceLineList != null)
                                    GdnLnLst.AddRange(clp.GuidanceLineList);
                            }

                            var clp_tora = (from element in thr.CenterLinePoints
                                            where (element != null && element.DeclDist != null && element.Role == CodeRunwayCenterLinePointRoleType.START_RUN && 
                                            (element.DeclDist.Select(d => d.DistanceType == CodeDeclaredDistance.TORA).ToList().Count > 0))
                                            select element).ToList();

                            if (clp_tora != null && clp_tora.Count > 0)
                            {
                                foreach (RunwayCenterLinePoint item in clp_tora)
                                {
                                    if (item.DeclDist != null)
                                    {
                                        DeclaredDistance toraDeclDist = item.DeclDist.Where(t => t.DistanceType == CodeDeclaredDistance.TORA && t.DistanceValue > 0).FirstOrDefault();
                                        if (toraDeclDist != null)
                                        {
                                            CreateTORA_Anno(toraDeclDist, thr, anno_RunwayDirectionCenterLinePointCartography_featClass, anno_DecorLineCartography_featClass, FocusMap, pSpatialReference, DistUom);

                                        }
                                    }
                                }
                            }
                        }

                        //if (thr.RdnDeclaredDistance != null)
                        //{
                        //    DeclaredDistance toraDeclDist = (from element in thr.RdnDeclaredDistance
                        //                                     where (element != null  && element.DistanceType  == CodeDeclaredDistance.TORA)
                        //                                     select element).FirstOrDefault();
                        //    if (toraDeclDist != null)
                        //    {
                        //        CreateTORA_Anno(toraDeclDist,thr, anno_RunwayDirectionCenterLinePointCartography_featClass, anno_DecorLineCartography_featClass, FocusMap, pSpatialReference, DistUom);

                        //    }
                        //}

                        if (thr.VisualGlideSlope !=null && thr.VisualGlideSlope.Elements !=null)
                        {
                            int indx = 0;
                            foreach (LightElement lghtEl in thr.VisualGlideSlope.Elements)
                            {

                                IGeometry EL_Geometry = Save_ChartGeo(anno_LightElementCartography_featClass, FocusMap, lghtEl, "LightElement", pSpatialReference);

                                if (indx == 0 || indx == thr.VisualGlideSlope.Elements.Count - 1)
                                {

                                    ChartElement_SimpleText chrtEl_lgt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RunwayVisulRangeElement");

                                    chrtEl_lgt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(thr.VisualGlideSlope, chrtEl_lgt.TextContents[0][0].DataSource);

                                    chrtEl_lgt.LinckedGeoId = lghtEl.ID;

                                    chrtEl_lgt.Slope = rotationValue * -1;

                                    IPoint cntr = (IPoint)EL_Geometry;

                                    IElement el_anno = (IElement)chrtEl_lgt.ConvertToIElement();

                                    el_anno.Geometry = cntr;
                                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_lgt.Name, lghtEl.ID, el_anno, ref chrtEl_lgt, chrtEl_lgt.Id, FocusMap.MapScale);
                                }
                                indx++;
                            }
                        }

                        if (thr.RwyVisualRange !=null && thr.RwyVisualRange.Count>0)
                        {
                            foreach (var vRange in thr.RwyVisualRange)
                            {
                                Save_ChartGeo(anno_RunwayVisualRangeCartography_featClass, FocusMap, vRange, "RunwayVisualRange", pSpatialReference);

                            }
                        }
                    }

                    #endregion



                }
                catch (Exception ex)
                {
                    
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    continue;
                }
            }
            


            
        }

        private static void CreateTORA_Anno(DeclaredDistance ToraDeclDist,RunwayDirection THR, IFeatureClass Anno_RunwayDirectionCenterLinePointCartography_featClass, IFeatureClass Anno_DecorLineCartography_featClass,
            IMap focusMap, ISpatialReference pSpatialReference, UOM_DIST_HORZ distUom)
        {
            string _geoId = "";
            bool rDirection = false;
            IGeometry ToraGeo = Save_TORAGeo(Anno_RunwayDirectionCenterLinePointCartography_featClass, Anno_DecorLineCartography_featClass, focusMap, pSpatialReference, THR.ID, THR.TrueBearing,
                ToraDeclDist.DistanceValue, ToraDeclDist.DistanceUOM.ToString(), ref _geoId, ref rDirection);

            ChartElement_SigmaCollout_AccentBar chrtEl_Tora = (ChartElement_SigmaCollout_AccentBar)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "RunwayTORAElement");

            if (ToraGeo != null)
            {


                chrtEl_Tora.TextContents[0][0].TextValue = ToraDeclDist.DistanceType.ToString();
                chrtEl_Tora.TextContents[1][0].TextValue = distUom == UOM_DIST_HORZ.M ?
                    Math.Round(THR.ConvertValueToMeter(ToraDeclDist.DistanceValue, ToraDeclDist.DistanceUOM.ToString()), 0).ToString() :
                    Math.Round(THR.ConvertValueToFeet(ToraDeclDist.DistanceValue, ToraDeclDist.DistanceUOM.ToString()), 0).ToString();
                chrtEl_Tora.TextContents[1][1].TextValue = distUom.ToString().ToLower();

                chrtEl_Tora.LinckedGeoId = _geoId;



                IPoint cntr = ((IPolyline)ToraGeo).FromPoint;
                cntr.PutCoords(cntr.X - chrtEl_Tora.Anchor.X, cntr.Y - chrtEl_Tora.Anchor.Y);

                IPolyline prjGeom = (IPolyline)EsriUtils.ToProject(ToraGeo, focusMap, pSpatialReference);
                ILine ln = new LineClass();
                ln.FromPoint = prjGeom.FromPoint;
                ln.ToPoint = prjGeom.ToPoint;


                double angle = ln.Angle * 180 / Math.PI;
                bool flag = false;
                chrtEl_Tora.Slope = ChartsHelperClass.CheckedAngle(angle - 90, ref flag);

                chrtEl_Tora.Anchor = new AncorPoint(cntr.X, cntr.Y);
                if (rDirection)
                {
                    chrtEl_Tora.HorizontalAlignment = horizontalAlignment.Left;
                    chrtEl_Tora.AccentBarPosition = sigmaCalloutAccentbarPosition.left;
                    chrtEl_Tora.TextContents[2][0].TextValue = "s";
                }

                IElement el_anno = (IElement)chrtEl_Tora.ConvertToIElement();



                el_anno.Geometry = cntr;
                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Tora.Name, _geoId, el_anno, ref chrtEl_Tora, chrtEl_Tora.Id, focusMap.MapScale);
            }
        }

        public static void CreateRadioFreqArea(RadioFrequencyArea Rfa, IFeatureClass anno_RadioFrequencyAreaCartography_featClass, IMap focusMap, ISpatialReference pSpatialReference)
        {

            double rotationValue = Double.Parse(focusMap.Description);
            IGeometry _Geometry = Save_ChartGeo(anno_RadioFrequencyAreaCartography_featClass, focusMap, Rfa, "RadioFrequencyArea", pSpatialReference);

            if (_Geometry == null) return;

            ChartElement_SimpleText chrtEl_pnt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "FreqAreaElement");

            chrtEl_pnt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Rfa, chrtEl_pnt.TextContents[0][0].DataSource);
            chrtEl_pnt.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Rfa, chrtEl_pnt.TextContents[0][1].DataSource);
            chrtEl_pnt.TextContents[0][2].TextValue = ChartsHelperClass.MakeText(Rfa, chrtEl_pnt.TextContents[0][2].DataSource);

            chrtEl_pnt.LinckedGeoId = Rfa.ID;

            chrtEl_pnt.Slope = rotationValue * -1;

            IPoint cntr = ((IArea)_Geometry).Centroid;
            chrtEl_pnt.Anchor = new AncorPoint(chrtEl_pnt.Anchor.X, chrtEl_pnt.Anchor.Y);

            IElement el_anno = (IElement)chrtEl_pnt.ConvertToIElement();

            el_anno.Geometry = cntr;
            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_pnt.Name, Rfa.ID, el_anno, ref chrtEl_pnt, chrtEl_pnt.Id, focusMap.MapScale);
        }

        public static void UpdateNorthArrow(AirportHeliport selARP, IMap focusMap)
        {
            throw new NotImplementedException();
        }

        public static void CreateBirdPolygon(List<PDMObject> BirdAirspace, IFeatureClass anno_BirdAirspaceCartography_featClass, IMap focusMap, ISpatialReference pSpatialReference)
        {
            double rotationValue = Double.Parse(focusMap.Description);

            foreach (Airspace arsps in BirdAirspace)
            {

                foreach (var vol in ((Airspace)arsps).AirspaceVolumeList)
                {

                    IGeometry _Geometry = Save_ChartGeo(anno_BirdAirspaceCartography_featClass, focusMap, vol, "AirspaceVolume", pSpatialReference);

                    if (_Geometry == null) continue;

                    ChartElement_SimpleText chrtEl_pnt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "BirdElement");

                    chrtEl_pnt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(arsps, chrtEl_pnt.TextContents[0][0].DataSource);

                    chrtEl_pnt.LinckedGeoId = vol.ID;

                    chrtEl_pnt.Slope = rotationValue * -1;

                    IPoint cntr = ((IArea)_Geometry).Centroid;
                    chrtEl_pnt.Anchor = new AncorPoint(chrtEl_pnt.Anchor.X, chrtEl_pnt.Anchor.Y);

                    IElement el_anno = (IElement)chrtEl_pnt.ConvertToIElement();

                    el_anno.Geometry = cntr;
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_pnt.Name, vol.ID, el_anno, ref chrtEl_pnt, chrtEl_pnt.Id, focusMap.MapScale);
                }
                
            }
        }

        public static void StoreDecorPoly(IFeatureClass anno_DecorPolygonCartography_featClass, string iD, string polyType, IGeometry gm)
        {
            try
            {
                int fID = -1;
                IFeature pFeat = anno_DecorPolygonCartography_featClass.CreateFeature();


                IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)gm;
                zAware.ZAware = false;
                IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)gm;
                mAware.MAware = false;

                pFeat.Shape = gm;

                fID = pFeat.Fields.FindField("type");
                if (fID >= 0) pFeat.set_Value(fID, polyType);

                fID = pFeat.Fields.FindField("FeatureGuid");
                if (fID >= 0) pFeat.set_Value(fID, iD);

                pFeat.Store();

            }
            catch { }
        }

        public static void CreateVerticalStructureCartography(List<PDMObject> vertStructureList, IMap focusMap, ISpatialReference pSpatialReference, IFeatureClass anno_VertStructureSurfaceCartography_featClass, IFeatureClass anno_VerticalStructurePointCartography_featClass)
        {
            double rotationValue = Double.Parse(focusMap.Description);

            foreach (VerticalStructure vertStruct in vertStructureList)
            {

                foreach (var part in vertStruct.Parts)
                {

                    
                    IGeometry _Geometry = Save_ChartGeo(anno_VertStructureSurfaceCartography_featClass, focusMap, part, "VerticalStructure_Surface", pSpatialReference, pdmObj: part);
                    if (_Geometry == null)
                        _Geometry = Save_ChartGeo(anno_VerticalStructurePointCartography_featClass, focusMap, part, "VerticalStructure_Point", pSpatialReference, pdmObj: part);
                    if (_Geometry == null) continue;

                    ChartElement_SimpleText chrtEl_pnt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "VerticalStructureElement");
                    if (_Geometry.GeometryType == esriGeometryType.esriGeometryPoint)
                        chrtEl_pnt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "VerticalStructureElementPoint");

                    chrtEl_pnt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(vertStruct, chrtEl_pnt.TextContents[0][0].DataSource);

                    chrtEl_pnt.LinckedGeoId = part.ID;

                    chrtEl_pnt.Slope = rotationValue * -1;

                    IPoint cntr = _Geometry.GeometryType != esriGeometryType.esriGeometryPoint? ((IArea)_Geometry).Centroid : (IPoint)_Geometry;
                    chrtEl_pnt.Anchor = new AncorPoint(chrtEl_pnt.Anchor.X, chrtEl_pnt.Anchor.Y);

                    IElement el_anno = (IElement)chrtEl_pnt.ConvertToIElement();

                    el_anno.Geometry = cntr;
                    if (_Geometry.GeometryType == esriGeometryType.esriGeometryPolygon)
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_pnt.Name, part.ID, el_anno, ref chrtEl_pnt, chrtEl_pnt.Id, focusMap.MapScale);


                    if (_Geometry.GeometryType == esriGeometryType.esriGeometryPoint)
                    {

                        #region elev VS Point

                        chrtEl_pnt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "VerticalStructureElementElev");

                        chrtEl_pnt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(part, chrtEl_pnt.TextContents[0][0].DataSource);

                        chrtEl_pnt.LinckedGeoId = part.ID;

                        chrtEl_pnt.Slope = rotationValue * -1;

                        chrtEl_pnt.Anchor = new AncorPoint(chrtEl_pnt.Anchor.X, chrtEl_pnt.Anchor.Y);

                        el_anno = (IElement)chrtEl_pnt.ConvertToIElement();

                        el_anno.Geometry = cntr;
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_pnt.Name, part.ID, el_anno, ref chrtEl_pnt, chrtEl_pnt.Id, focusMap.MapScale);

                        #endregion

                        #region height VS Point

                        chrtEl_pnt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "VerticalStructureElementHeight");

                        chrtEl_pnt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(part, chrtEl_pnt.TextContents[0][0].DataSource);

                        chrtEl_pnt.LinckedGeoId = part.ID;

                        chrtEl_pnt.Slope = rotationValue * -1;

                        chrtEl_pnt.Anchor = new AncorPoint(chrtEl_pnt.Anchor.X, chrtEl_pnt.Anchor.Y);

                        el_anno = (IElement)chrtEl_pnt.ConvertToIElement();

                        el_anno.Geometry = cntr;
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_pnt.Name, part.ID, el_anno, ref chrtEl_pnt, chrtEl_pnt.Id, focusMap.MapScale);

                        #endregion

                    }

                }
            }
        }

        public static void RotateMap(IMap FocusMap, double rotateAngl)
        {
            ((IActiveView)FocusMap).ScreenDisplay.DisplayTransformation.Rotation = rotateAngl;

            for (int i = 0; i <= FocusMap.LayerCount - 1; i++)
            {
                ILayer _layer = FocusMap.get_Layer(i);
                if (_layer.Name.StartsWith("Annotations")) continue;


                //if (_layer is IGeoFeatureLayer)
                //{
                //    ESRI.ArcGIS.Display.ISymbol symb = null;
                //    IGeoFeatureLayer geoFeatLayer = _layer as IGeoFeatureLayer;

                    //if ((geoFeatLayer.FeatureClass != null) && (geoFeatLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint))
                    //{

                    //    if (geoFeatLayer.Renderer is ISimpleRenderer)
                    //    {
                    //        symb = (geoFeatLayer.Renderer as ISimpleRenderer).Symbol;
                    //        if (symb != null)
                    //            ((ESRI.ArcGIS.Display.IMarkerSymbol)symb).Angle = -rotateAngl;
                    //    }
                    //    else if (geoFeatLayer.Renderer is IUniqueValueRenderer)
                    //    {
                    //        symb = (geoFeatLayer.Renderer as IUniqueValueRenderer).DefaultSymbol;
                    //        if (symb != null)
                    //            ((ESRI.ArcGIS.Display.IMarkerSymbol)symb).Angle = -rotateAngl;

                    //        for (int j = 0; j < (geoFeatLayer.Renderer as IUniqueValueRenderer).ValueCount; j++)
                    //        {
                    //            string vs = (geoFeatLayer.Renderer as IUniqueValueRenderer).Value[j];
                    //            symb = (geoFeatLayer.Renderer as IUniqueValueRenderer).Symbol[vs];
                    //            if (symb != null)
                    //                ((ESRI.ArcGIS.Display.IMarkerSymbol)symb).Angle = -rotateAngl;
                    //        }
                            
                    //    }


                    //}

                    
                //}


                else
                {
                    //System.Diagnostics.Debug.WriteLine((lr as FeatureLayer).FeatureClass.ShapeType.ToString());
                }
            }


        }

        public static void CreateNavaids_ChartElements(List<PDMObject> navaidsList, IMap FocusMap,  ref List<string> NavaidListID, IFeatureClass Anno_NavaidGeo_featClass, string VertUom)
        {
            if (navaidsList == null) return;
            double rotationValue = Double.Parse(FocusMap.Description);

            foreach (NavaidSystem navSystem in navaidsList)
            {
                try
                {

                    if (NavaidListID.IndexOf(navSystem.ID) < 0)
                    {
                        NavaidListID.Add(navSystem.ID);

                   
                        CreateNavaidAnno((PDM.NavaidSystem)navSystem, Anno_NavaidGeo_featClass, VertUom, rotationValue, FocusMap.MapScale);

                       

                    }

                }
                catch { }
            }
        }

        private static List<IElement> CreateNavaidAnno(PDM.NavaidSystem navaidSystem, IFeatureClass Anno_NavaidGeo_featClass, string vertUom, double mapRotation, double MapScale)
        {
            IElement el_Navaid = null;
            PDM.NavaidComponent Component = null;
            List<PDM.NavaidComponent> ComponentsList = new List<NavaidComponent>();
            List<IElement> res = new List<IElement>();
            PDM.NavaidComponent freq = null;

            switch (navaidSystem.CodeNavaidSystemType)
            {
                case NavaidSystemType.VOR_DME:
                case NavaidSystemType.VORTAC:
                    Component = (from element in navaidSystem.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.VOR) select element).FirstOrDefault();
                    ComponentsList.Add(Component);
                    break;

                case NavaidSystemType.VOR:
                case NavaidSystemType.DME:
                case NavaidSystemType.NDB:
                case NavaidSystemType.TACAN:
                case NavaidSystemType.LOC:
                case NavaidSystemType.MKR:
                    Component = navaidSystem.Components!=null && navaidSystem.Components.Count > 0 ? navaidSystem.Components[0] : null;
                    ComponentsList.Add(Component);
                    break;

                case NavaidSystemType.ILS:
                    Component = (from element in navaidSystem.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Localizer) select element).FirstOrDefault();
                    ComponentsList.Add(Component);
                    break;

                case NavaidSystemType.ILS_DME:
                case NavaidSystemType.NDB_DME:
                case NavaidSystemType.LOC_DME:
                case NavaidSystemType.NDB_MKR:
                    foreach (var cmp in navaidSystem.Components)
                    {
                        ComponentsList.Add(cmp);

                    }
                    break;
               
                default:
                    break;
            }

            if (ComponentsList == null || ComponentsList.Count <=0) return null;


            foreach (var navComp in ComponentsList)
            {
                IPoint pnt = new PointClass();
                
                string chnl = "";
                if (navComp.PDM_Type != PDM_ENUM.GlidePath)
                {
                   
                    if (navaidSystem.CodeNavaidSystemType == NavaidSystemType.ILS_DME)
                    {
                      Component = (from element in navaidSystem.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.Localizer) select element).FirstOrDefault();
                       freq = Component;

                        Component = (from element in navaidSystem.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.DME) select element).FirstOrDefault();
                        chnl = Component != null && ((DME)Component).Channel.ToString().Length > 0 ? ((DME)Component).Channel : "NaN";

                    }
                    else if (navaidSystem.CodeNavaidSystemType == NavaidSystemType.VOR_DME)
                    {
                        Component = (from element in navaidSystem.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.DME) select element).FirstOrDefault();
                        chnl = Component != null && ((DME)Component).Channel.ToString().Length > 0 ? ((DME)Component).Channel : "NaN";

                    }
                    else if (navaidSystem.CodeNavaidSystemType == NavaidSystemType.VORTAC)
                    {
                        Component = (from element in navaidSystem.Components where (element != null) && (element.PDM_Type == PDM.PDM_ENUM.TACAN) select element).FirstOrDefault();
                        chnl = Component != null && ((TACAN)Component).Channel.ToString().Length > 0 ? ((DME)Component).Channel : "NaN";

                    }
                    ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                    chrtEl_Navaid.Slope = mapRotation * -1;

                    ChartElement_SigmaCollout_Navaid chrtEl_Sign = (ChartElement_SigmaCollout_Navaid)chrtEl_Navaid;

                    if (navComp.Geo == null) navComp.RebuildGeo();
                    pnt.PutCoords(((IPoint)navComp.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)navComp.Geo).Y - chrtEl_Sign.Anchor.Y);


                    #region Caption

                    chrtEl_Sign.CaptionTextLine[0][0].TextValue = navaidSystem.CodeNavaidSystemType.ToString();
                    if (navComp.PDM_Type == PDM_ENUM.Localizer) chrtEl_Sign.CaptionTextLine[0][0].TextValue = "LOC";

                    #endregion

                    #region Inner Text

                    chrtEl_Sign.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(navComp, chrtEl_Sign.TextContents[0][0].DataSource).Trim();
                    chrtEl_Sign.MorseTextLine.MorseText = chrtEl_Sign.TextContents[0][0].TextValue;

                    chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(navComp, chrtEl_Sign.TextContents[0][1].DataSource,  Rounder: 2);
                    if (navComp.PDM_Type == PDM_ENUM.DME && freq !=null) chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(freq, chrtEl_Sign.TextContents[0][1].DataSource, Rounder: 2);

                    chrtEl_Sign.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(navComp, chrtEl_Sign.TextContents[1][0].DataSource);
                    if (navComp.PDM_Type == PDM_ENUM.Localizer || navComp.PDM_Type == PDM_ENUM.VOR) chrtEl_Sign.TextContents[1][0].TextValue = chnl;

                    chrtEl_Sign.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(navComp, chrtEl_Sign.TextContents[2][0].DataSource).Trim();
                    
                    chrtEl_Sign.TextContents[3][0].TextValue = ChartsHelperClass.MakeText(navComp, chrtEl_Sign.TextContents[3][0].DataSource, coordtype.DDMMSS_2, 10);
                    chrtEl_Sign.TextContents[3][1].TextValue = ChartsHelperClass.MakeText(navComp, chrtEl_Sign.TextContents[3][1].DataSource, coordtype.DDMMSS_2, 10);

                    #endregion

                    #region Bottom

                    if (navComp.PDM_Type != PDM_ENUM.Localizer)
                    {

                        int el = 0;
                        string ElevStr = "";
                        string ElevStrUOM = "";

                        if (navComp.Elev != null && navComp.Elev.HasValue && !navComp.Elev.ToString().StartsWith("NaN")) el = (int)Math.Round((double)navComp.Elev, 0);


                        if (el != 0)
                        {

                            //string uom = navComp.Elev_UOM.ToString();
                            //double elevDme = ArenaStaticProc.UomTransformation(uom, uom, (double)el);
                            //var ff = Math.DivRem((int)elevDme, 100, out el);
                            //el = 100 * (ff + 1);

                            ElevStr = el.ToString();
                            ElevStrUOM = "FT";
                            chrtEl_Sign.BottomTextLine[0][0].TextValue = ElevStr;
                            chrtEl_Sign.BottomTextLine[0][1].TextValue = ElevStrUOM;

                        }


                        else
                        {
                            chrtEl_Sign.BottomTextLine[0][0].TextValue = "";
                            chrtEl_Sign.BottomTextLine[0][1].TextValue = "";
                            chrtEl_Sign.BottomTextLine.Remove(chrtEl_Sign.BottomTextLine[0]);
                            chrtEl_Sign.HasFooter = false;

                        }


                    }
                    else
                    {
                        chrtEl_Sign.BottomTextLine.Remove(chrtEl_Sign.BottomTextLine[0]);
                        chrtEl_Sign.Frame.FrameMargins.BottomMargin = 5;
                        chrtEl_Sign.Frame.FrameMargins.TopMargin = 0;
                        chrtEl_Sign.Frame.FrameMargins.FooterHorizontalMargin = 0;
                        chrtEl_Sign.Frame.FrameMargins.HeaderHorizontalMargin = 0;
                        //chrtEl_Sign.TextContents.RemoveAt(chrtEl_Sign.TextContents.Count - 1);
                        chrtEl_Sign.HasFooter = false;

                    }

                    #endregion


                    chrtEl_Sign.MorseTextLine  = null;


                    chrtEl_Sign.Anchor = new AncorPoint(((IPoint)navComp.Geo).X, ((IPoint)navComp.Geo).Y);


                    el_Navaid = (IElement)chrtEl_Sign.ConvertToIElement();

                    if (el_Navaid is IGroupElement)
                    {
                        IGroupElement GrEl = el_Navaid as IGroupElement;
                        for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                        {
                            GrEl.get_Element(i).Geometry = pnt as IGeometry;
                        }
                    }
                    else el_Navaid.Geometry = pnt;

                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navComp.ID, el_Navaid, ref chrtEl_Navaid, chrtEl_Navaid.Id, MapScale);
                    ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, navComp);


                    
                }
                else
                {
                    ChartElement_TextArrow chrtEl_Navaid = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "GlidePath_Navaid");
                    chrtEl_Navaid.Slope = mapRotation * -1;

                    ChartElement_TextArrow chrtEl_Sign = (ChartElement_TextArrow)chrtEl_Navaid;


                    if (navComp.Geo == null) navComp.RebuildGeo();
                    pnt.PutCoords(((IPoint)navComp.Geo).X - chrtEl_Sign.Anchor.X, ((IPoint)navComp.Geo).Y - chrtEl_Sign.Anchor.Y);

                    chrtEl_Sign.Anchor = new AncorPoint(((IPoint)navComp.Geo).X, ((IPoint)navComp.Geo).Y);

                    chrtEl_Sign.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(navComp, chrtEl_Sign.TextContents[0][1].DataSource);

                    el_Navaid = (IElement)chrtEl_Sign.ConvertToIElement();

                    if (el_Navaid is IGroupElement)
                    {
                        IGroupElement GrEl = el_Navaid as IGroupElement;
                        for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                        {
                            GrEl.get_Element(i).Geometry = pnt as IGeometry;
                        }
                    }
                    else el_Navaid.Geometry = pnt;

                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navComp.ID, el_Navaid, ref chrtEl_Navaid, chrtEl_Navaid.Id, MapScale);
                    ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, navComp);


                }











                res.Add(el_Navaid);
            }

            return res;

           

        }

        public static void CreateCheckPoints(List<NavigationSystemCheckpoint> navSystemCheckpoints, IMap focusMap, ISpatialReference pSpatialReference, IFeatureClass anno_CheckpointCartography_featClass)
        {
            if (navSystemCheckpoints == null) return;
            double rotationValue = Double.Parse(focusMap.Description);

            foreach (NavigationSystemCheckpoint checkPnt in navSystemCheckpoints)
            {
                if (checkPnt is CheckpointVOR)
                {
                    IGeometry checkPntGeometry = Save_ChartGeo(anno_CheckpointCartography_featClass, focusMap, checkPnt, "CheckpointVOR", pSpatialReference);

                    if (checkPntGeometry == null) continue;

                    ChartElement_TextArrow chrtEl_pnt = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "CheckpointElement");



                            chrtEl_pnt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(checkPnt, chrtEl_pnt.TextContents[0][0].DataSource);
                            chrtEl_pnt.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(checkPnt, chrtEl_pnt.TextContents[1][0].DataSource, Rounder: 2);

                         

                            chrtEl_pnt.LinckedGeoId = checkPnt.ID;



                            chrtEl_pnt.Slope = rotationValue * -1;

                            IPoint cntr = (IPoint)checkPntGeometry;
                            chrtEl_pnt.Anchor = new AncorPoint(cntr.X, cntr.Y);

                            IElement el_anno = (IElement)chrtEl_pnt.ConvertToIElement();



                            el_anno.Geometry = cntr;
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_pnt.Name, checkPnt.ID, el_anno, ref chrtEl_pnt, chrtEl_pnt.Id, focusMap.MapScale);

                }
            }

        }

        public static void CreateTaxiwayCartography(List<Taxiway> taxiwayList, IMap FocusMap, ISpatialReference pSpatialReference,
            IFeatureClass TaxiwayElementCartography_featClass, UOM_DIST_HORZ distUom, ref List<GuidanceLine> GdnLnLst, ref List<DeicingArea> DcngArea, ref List<MarkingElement> MrkLst)
        {
            if (taxiwayList == null) return;

            foreach (Taxiway twy in taxiwayList)
            {
                try
                {

                    if (twy.TaxiWayElementsList != null)
                    {
                        foreach (var twyEl in twy.TaxiWayElementsList)
                        {
                            Save_ChartGeo(TaxiwayElementCartography_featClass, FocusMap, twyEl, "TaxiwayElement", pSpatialReference);

                        }
                    }

                    if (twy.GuidanceLineList != null && twy.GuidanceLineList.Count > 0) GdnLnLst.AddRange(twy.GuidanceLineList);
                    if (twy.DeicingAreaList != null && twy.DeicingAreaList.Count > 0) DcngArea.AddRange(twy.DeicingAreaList);
                    if (twy.TaxiwayMarkingList != null && twy.TaxiwayMarkingList.Count > 0)
                    {
                        foreach (var item in twy.TaxiwayMarkingList)
                        {
                            if (item.MarkingElementList != null && item.MarkingElementList.Count > 0)
                                MrkLst.AddRange(item.MarkingElementList);
                        }
                    }
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    continue;
                }
            }




        }
   
        public static void CreateApronCartography(List<Apron> apronList, IMap FocusMap, ISpatialReference pSpatialReference, IFeatureClass anno_ApronElementCartography_featClass, 
            IFeatureClass anno_AircraftStandCartography_featClass, IFeatureClass anno_AircraftStandExtentCartography_featClass,
            IFeatureClass anno_LightElementCartography_featClass, IFeatureClass anno_GuidanceLineCartography_featClass, UOM_DIST_HORZ distUom, ref List<GuidanceLine> GdnLnLst, ref List<DeicingArea> DcngArea, ref List<MarkingElement> MrkLst)
        {
            if (apronList == null) return;
            double rotationValue = Double.Parse(FocusMap.Description);

            foreach (Apron aprn in apronList)
            {
                try
                {

                    if (aprn.ApronElementList != null)
                    {
                        foreach (var aprnEl in aprn.ApronElementList)
                        {
                            Save_ChartGeo(anno_ApronElementCartography_featClass, FocusMap, aprnEl, "ApronElement", pSpatialReference);


                            if (aprnEl.AircrafrStandList != null)
                            {
                                foreach (var arcrftStnd in aprnEl.AircrafrStandList)
                                {

                                    double? angl = null;

                                    if (arcrftStnd.GuidanceLineList != null && arcrftStnd.GuidanceLineList.Count > 0)
                                    {
                                        foreach (var gdln in arcrftStnd.GuidanceLineList)
                                        {
                                            //gdln.SourceDetail = "AircraftStand";
                                            GdnLnLst.Add(gdln);
                                            angl = GetGuidanceLineAngle(anno_GuidanceLineCartography_featClass, FocusMap, gdln, "GuidanceLine", pSpatialReference) + rotationValue;

                                        }
                                    }

                                    IGeometry _Geo = Save_ChartGeo(anno_AircraftStandCartography_featClass, FocusMap, arcrftStnd, "AircraftStand", pSpatialReference, angl);


                                    if (_Geo != null)
                                    {
                                        ChartElement_SimpleText chrtEl_Stnd = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "AircraftStandElement");

                                        chrtEl_Stnd.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(arcrftStnd, chrtEl_Stnd.TextContents[0][0].DataSource);
                                        if (chrtEl_Stnd.TextContents[0][0].TextValue.StartsWith("NaN")) continue;
                                        chrtEl_Stnd.LinckedGeoId = arcrftStnd.ID;
                                        chrtEl_Stnd.Slope = rotationValue * -1;

                                        IPoint cntr = (IPoint)_Geo;
                                        chrtEl_Stnd.Anchor = new AncorPoint(chrtEl_Stnd.Anchor.X, chrtEl_Stnd.Anchor.Y);

                                        IElement el_anno = (IElement)chrtEl_Stnd.ConvertToIElement();
                                        el_anno.Geometry = _Geo;
                                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Stnd.Name, arcrftStnd.ID, el_anno, ref chrtEl_Stnd, chrtEl_Stnd.Id, FocusMap.MapScale);
                                    }

                                    Save_ChartGeo(anno_AircraftStandExtentCartography_featClass, FocusMap, arcrftStnd, "AircraftStandExtent", pSpatialReference);



                                    if (arcrftStnd.DeicingAreaList != null && arcrftStnd.DeicingAreaList.Count > 0) DcngArea.AddRange(arcrftStnd.DeicingAreaList);
                                    if (arcrftStnd.StandMarkingList != null && arcrftStnd.StandMarkingList.Count > 0)
                                    {
                                        foreach (var item in arcrftStnd.StandMarkingList)
                                        {
                                            if (item.MarkingElementList != null && item.MarkingElementList.Count > 0)
                                                MrkLst.AddRange(item.MarkingElementList);
                                        }
                                    }

                                }
                            }


                        }
                    }
                    if (aprn.GuidanceLineList != null)
                        GdnLnLst.AddRange(aprn.GuidanceLineList);
                    if (aprn.DeicingAreaList != null)
                        DcngArea.AddRange(aprn.DeicingAreaList);

                    if (aprn.LightSystem != null)
                    {
                        foreach (var lghtEl in ((LightSystem)aprn.LightSystem).Elements)
                        {
                            Save_ChartGeo(anno_LightElementCartography_featClass, FocusMap, lghtEl, "LightElement", pSpatialReference);

                        }
                    }

                    if (aprn.ApronMarkingList!=null && aprn.ApronMarkingList.Count > 0)
                    {
                        foreach (var item in aprn.ApronMarkingList)
                        {
                            MrkLst.AddRange(item.MarkingElementList);
                        }
                    }

                }
                catch { }
            }
        }

        public static void CreateGuidanceLineCartography(List<GuidanceLine> GdnLnLst, IMap FocusMap, ISpatialReference pSpatialReference,
           IFeatureClass GuidanceLineCartography_featClass, UOM_DIST_HORZ distUom, ref List<MarkingElement> MrkLst)
        {

            double rotationValue = Double.Parse(FocusMap.Description);

            List<string> uids = new List<string>();
            try
            {

                foreach (var gdnLineEl in GdnLnLst)
                {
                    if (uids.IndexOf(gdnLineEl.ID) >= 0) continue;

                    IGeometry gdnLineElGeometry = Save_ChartGeo(GuidanceLineCartography_featClass, FocusMap, gdnLineEl, "GuidanceLine", pSpatialReference);

                    uids.Add(gdnLineEl.ID);

                    if (gdnLineEl.TaxiwayName == null || gdnLineEl.TaxiwayName.Trim().Length <= 0) continue;

                    
                    IPoint StPnt = new PointClass();
                    IPoint CntrPnt = new PointClass();
                    IPoint EndPnt = new PointClass();
                    double angl = 0;

                    if (gdnLineElGeometry != null)
                        TerminalChartsUtil.GetInterestPoints(FocusMap, pSpatialReference, out StPnt, out CntrPnt, out EndPnt, gdnLineElGeometry as IPolyline, out angl);

                    if (CntrPnt != null)
                    {
                        ChartElement_TextArrow chrtEl_gdnLn = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "GuidanceLineElement");


                        chrtEl_gdnLn.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(gdnLineEl, chrtEl_gdnLn.TextContents[0][0].DataSource);

                        if (chrtEl_gdnLn.TextContents[0][0].TextValue.StartsWith("NaN")) continue;

                        chrtEl_gdnLn.LinckedGeoId = gdnLineEl.ID;

                        chrtEl_gdnLn.Slope = angl;

                        chrtEl_gdnLn.Anchor = new AncorPoint(CntrPnt.X, CntrPnt.Y);

                        chrtEl_gdnLn.Placed = gdnLineEl.Y <= 3 && gdnLineEl.X >= 100;
                        gdnLineEl.X = null;
                        gdnLineEl.Y = null;

                        IElement el_anno = (IElement)chrtEl_gdnLn.ConvertToIElement();


                        el_anno.Geometry = CntrPnt;
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_gdnLn.Name, gdnLineEl.ID, el_anno, ref chrtEl_gdnLn, chrtEl_gdnLn.Id, FocusMap.MapScale);

                    }

                    if (gdnLineEl.GuidanceLineMarkingList != null && gdnLineEl.GuidanceLineMarkingList.Count > 0)
                    {
                        foreach (var item in gdnLineEl.GuidanceLineMarkingList)
                        {
                            if (item.MarkingElementList != null && item.MarkingElementList.Count > 0) MrkLst.AddRange(item.MarkingElementList);
                        }
                    }
                }


            }
            catch { }
        }


        public static void CreateAirportHotSpotAnno(List<AirportHotSpot> airportHotSpotList, IFeatureClass anno_AirportHotSpotCartography_featClass, IMap focusMap, ISpatialReference pSpatialReference)
        {
            
            double rotationValue = Double.Parse(focusMap.Description);

            foreach (AirportHotSpot checkPnt in airportHotSpotList)
            {

                IGeometry hotSpotGeometry = Save_ChartGeo(anno_AirportHotSpotCartography_featClass, focusMap, checkPnt, "AirportHotSpot", pSpatialReference);

                if (hotSpotGeometry == null) continue;

                ChartElement_SimpleText chrtEl_pnt = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "AirportHotSpotElement");



                chrtEl_pnt.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(checkPnt, chrtEl_pnt.TextContents[0][0].DataSource);

                chrtEl_pnt.LinckedGeoId = checkPnt.ID;


                chrtEl_pnt.Slope = rotationValue * -1;

                IPoint cntr = ((IArea)hotSpotGeometry).Centroid;
                //chrtEl_pnt.Anchor = new AncorPoint(cntr.X, cntr.Y);

                IElement el_anno = (IElement)chrtEl_pnt.ConvertToIElement();



                el_anno.Geometry = cntr;
                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_pnt.Name, checkPnt.ID, el_anno, ref chrtEl_pnt, chrtEl_pnt.Id, focusMap.MapScale);


            }
        }


        public static IGeometry Save_ChartGeo(IFeatureClass cartography_featClass, IMap _map, PDMObject PDMObj, string FCName, ISpatialReference pSpatialReference, double? AdditionalVaue = null, PDMObject pdmObj = null)
        {
            var workspaceEdit = (IWorkspaceEdit)cartography_featClass.FeatureDataset.Workspace;

            ITable tbl = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, FCName);
            if (tbl == null) return null;


            //double rotationValue = Double.Parse(_map.Description);

            var fc = (IFeatureClass)tbl;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "FeatureGUID = '" + PDMObj.ID + "'";
            IFeatureCursor featureCursor = fc.Search(queryFilter, false);


            IFeature feature;
            IGeometry gm = null;
            while ((feature = featureCursor.NextFeature()) != null)
            {
                if (feature.Shape == null)
                {
                    if (PDMObj.Geo == null) PDMObj.RebuildGeo();
                    if (PDMObj.Geo != null) gm = PDMObj.Geo;
                    else break;
                }
                else
                    gm = feature.Shape;

                if (gm == null) break;

                IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)gm;
                zAware.ZAware = false;
                IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)gm;
                mAware.MAware = false;


                int fID = -1;
                IFeature pFeat = cartography_featClass.CreateFeature();

                try
                {
                   
                    pFeat.Shape = gm;

                    for (int i = 1; i <= pFeat.Fields.FieldCount-1; i++)
                    {
                     
                        if (pFeat.Fields.Field[i].Name.ToUpper().StartsWith("SHAPE")) continue;

                        fID = fc.Fields.FindField(pFeat.Fields.Field[i].Name);
                        if (fID >= 0) pFeat.set_Value(i, feature.Value[fID]);

                    }

                    if (FCName.CompareTo("GuidanceLine") ==0)
                    {
                        IGeometry glPrjGeo = EsriUtils.ToProject(gm, _map, pSpatialReference);
                        double l = ((IPolyline)glPrjGeo).Length;
                        fID = pFeat.Fields.FindField("GLLength");
                        if (fID >= 0) pFeat.set_Value(fID, l);
                        PDMObj.X = l;

                        l = ((IPointCollection)gm).PointCount;
                        fID = pFeat.Fields.FindField("GLPointCount");
                        if (fID >= 0) pFeat.set_Value(fID, l);
                        PDMObj.Y = l;

                        //if (PDMObj.SourceDetail!=null &&  PDMObj.SourceDetail.StartsWith("AircraftStand"))
                        //{
                        //    PDMObj.SourceDetail = "";
                        //    ILine ln = new LineClass { FromPoint = ((IPolyline)glPrjGeo).FromPoint, ToPoint = ((IPolyline)glPrjGeo).ToPoint };
                        //    l = ln.Angle * (180 / Math.PI);

                        //    fID = pFeat.Fields.FindField("GLAngle");
                        //    if (fID >= 0) pFeat.set_Value(fID, l);
                            
                        //}
                    }

                    if (FCName.CompareTo("AircraftStand") == 0 && AdditionalVaue.HasValue)
                    {
                        fID = pFeat.Fields.FindField("SymbolAngle");
                        if (fID >= 0) pFeat.set_Value(fID, AdditionalVaue.Value);
                    }

                    if (FCName.StartsWith("VerticalStructure") && pdmObj!=null && pdmObj.PDM_Type == PDM_ENUM.VerticalStructurePart)
                    {
                        VerticalStructurePart vsPart = (VerticalStructurePart)pdmObj;

                        //fID = pFeat.Fields.FindField("SHAPE"); if (fID > 0) pFeat.set_Value(fID, vsPart.Geo);
                        //fID = pFeat.Fields.FindField("FeatureGUID"); if (fID > 0) pFeat.set_Value(fID, vsPart.ID);
                        //fID = pFeat.Fields.FindField("NAME"); if (fID > 0) pFeat.set_Value(fID, vsPart.Designator);
                        fID = pFeat.Fields.FindField("type"); if (fID > 0) pFeat.set_Value(fID, vsPart.Type.ToString());

                        //fID = pFeat.Fields.FindField("Lighted");
                        //if (fID > 0)
                        //{
                        //    int fl = VStruct.Lighted ? 1 : 0;
                        //    pFeat.set_Value(fID, fl);
                        //}

                        //fID = pFeat.Fields.FindField("GroupFlag");
                        //if (fID > 0)
                        //{
                        //    if (VStruct.Group && vsPart.Height.HasValue && vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Height_UOM.ToString()) < 300)
                        //        pFeat.set_Value(fID, 1);
                        //    else if (VStruct.Group && vsPart.Height.HasValue && vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Height_UOM.ToString()) >= 300)
                        //        pFeat.set_Value(fID, 1);
                        //    else
                        //        pFeat.set_Value(fID, 0);

                        //}


                        //fID = pFeat.Fields.FindField("High");
                        //if (fID > 0 && vsPart.Height.HasValue)
                        //{
                        //    int fl = vsPart.ConvertValueToMeter(vsPart.Height.Value, vsPart.Elev_UOM.ToString()) >= 300 ? 1 : 0;
                        //    pFeat.set_Value(fID, fl);
                        //}


                        //fID = pFeat.Fields.FindField("type2"); if (fID > 0) pFeat.set_Value(fID, vsPart.Type.ToString());
                        fID = pFeat.Fields.FindField("verticalExtent"); if (fID > 0) pFeat.set_Value(fID, vsPart.VerticalExtent);
                        fID = pFeat.Fields.FindField("verticalExtentUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.VerticalExtent.ToString());
                        fID = pFeat.Fields.FindField("designator"); if (fID > 0) pFeat.set_Value(fID, vsPart.Designator);
                        fID = pFeat.Fields.FindField("Lighting"); if (fID > 0) pFeat.set_Value(fID, vsPart.MarkingFirstColour.ToString());
                        //fID = pFeat.Fields.FindField("Elevation"); if (fID > 0) pFeat.set_Value(fID, vsPart.Elev);
                        //fID = pFeat.Fields.FindField("ElevationUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.Elev_UOM.ToString());
                        fID = pFeat.Fields.FindField("Height"); if (fID > 0) pFeat.set_Value(fID, vsPart.Height);
                        fID = pFeat.Fields.FindField("HeightUom"); if (fID > 0) pFeat.set_Value(fID, vsPart.Height_UOM.ToString());
                    }

                    pFeat.Store();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                Application.DoEvents();


            }

            Marshal.ReleaseComObject(featureCursor);

            return gm;

        }


        public static double GetGuidanceLineAngle(IFeatureClass cartography_featClass, IMap _map, PDMObject PDMObj, string FCName, ISpatialReference pSpatialReference)
        {
            double res = 0;

            var workspaceEdit = (IWorkspaceEdit)cartography_featClass.FeatureDataset.Workspace;

            ITable tbl = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, FCName);
            if (tbl == null) return 0;


            var fc = (IFeatureClass)tbl;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "FeatureGUID = '" + PDMObj.ID + "'";
            IFeatureCursor featureCursor = fc.Search(queryFilter, false);


            IFeature feature;
            IGeometry gm = null;
            while ((feature = featureCursor.NextFeature()) != null)
            {
                if (feature.Shape == null)
                {
                    if (PDMObj.Geo == null) PDMObj.RebuildGeo();
                    if (PDMObj.Geo != null) gm = PDMObj.Geo;
                    else break;
                }
                else
                    gm = feature.Shape;

                if (gm == null) break;



                try
                {

                    IGeometry glPrjGeo = EsriUtils.ToProject(gm, _map, pSpatialReference);

                    PDMObj.SourceDetail = "";
                    ILine ln = new LineClass { FromPoint = ((IPolyline)glPrjGeo).FromPoint, ToPoint = ((IPolyline)glPrjGeo).ToPoint };
                    res = ln.Angle * (180 / Math.PI);

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                Application.DoEvents();


            }

            Marshal.ReleaseComObject(featureCursor);

            return res;

        }

        private static IGeometry Save_RwyStripGeo(IFeatureClass cartography_featClass, IMap _map, string RunwayDirectionID, double width, double length, string uom, ref string GeoID)
        {
            var workspaceEdit = (IWorkspaceEdit)cartography_featClass.FeatureDataset.Workspace;

            ITable tbl = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "RunwayProtectArea");
            if (tbl == null) return null;


            var fc = (IFeatureClass)tbl;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "ID_RunwayDirection = '" + RunwayDirectionID + "' and Width = "+ width.ToString() + " and Length = "+ length.ToString() + " and Length_UOM = '" + uom +"'";
            IFeatureCursor featureCursor = fc.Search(queryFilter, false);


            IFeature feature;
            IGeometry gm = null;
            while ((feature = featureCursor.NextFeature()) != null)
            {
                gm = feature.Shape;

                if (gm == null) break;

                IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)gm;
                zAware.ZAware = false;
                IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)gm;
                mAware.MAware = false;


                int fID = -1;
                IFeature pFeat = cartography_featClass.CreateFeature();

                try
                {


                    pFeat.Shape = gm;

                    for (int i = 1; i < fc.Fields.FieldCount; i++)
                    {

                        if (fc.Fields.Field[i].Name.ToUpper().StartsWith("SHAPE")) continue;


                        fID = cartography_featClass.FindField(fc.Fields.Field[i].Name);
                        if (fID >= 0) pFeat.set_Value(fID, feature.Value[fID]);


                        if (fc.Fields.Field[i].Name.StartsWith("FeatureGUID"))
                            GeoID = feature.Value[fID].ToString();



                    }

                    pFeat.Store();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                Application.DoEvents();


            }

            Marshal.ReleaseComObject(featureCursor);

            return gm;

        }

        private static IGeometry Save_TORAGeo(IFeatureClass cartography_featClass, IFeatureClass DecorLineCartography_featClass, IMap _map, ISpatialReference pSpatialReference,
            string RunwayDirectionID, double? theBearing, double length, string uom, ref string GeoID, ref bool RightDirection)
        {
            var workspaceEdit = (IWorkspaceEdit)cartography_featClass.FeatureDataset.Workspace;

            ITable tbl = EsriUtils.getTableByname((IFeatureWorkspace)workspaceEdit, "RunwayDirectionCenterLinePoint");
            if (tbl == null) return null;


            var fc = (IFeatureClass)tbl;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "ID_RunwayDirection = '" + RunwayDirectionID + "' and TORA = " +  length.ToString() + " and DeclaredDistance_UOM = '" + uom + "'";
            IFeatureCursor featureCursor = fc.Search(queryFilter, false);


            IFeature feature;
            IGeometry gm = null;
            while ((feature = featureCursor.NextFeature()) != null)
            {
                gm = feature.Shape;

                if (gm == null) break;

                IZAware zAware = (ESRI.ArcGIS.Geometry.IZAware)gm;
                zAware.ZAware = false;
                IMAware mAware = (ESRI.ArcGIS.Geometry.IMAware)gm;
                mAware.MAware = false;


                int fID = -1;
                IFeature pFeat = cartography_featClass.CreateFeature();

                try
                {


                    pFeat.Shape = gm;

                    for (int i = 1; i < fc.Fields.FieldCount; i++)
                    {

                        if (fc.Fields.Field[i].Name.ToUpper().StartsWith("SHAPE")) continue;


                        fID = cartography_featClass.FindField(fc.Fields.Field[i].Name);
                        if (fID >= 0) pFeat.set_Value(fID, feature.Value[fID]);


                        if (fc.Fields.Field[i].Name.StartsWith("FeatureGUID"))
                            GeoID = feature.Value[fID].ToString();
                        if (fc.Fields.Field[i].Name.StartsWith("Direction"))
                            RightDirection = feature.Value[fID].ToString().CompareTo("RIGHT") == 0 ;




                    }

                    pFeat.Store();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                Application.DoEvents();


            }

            Marshal.ReleaseComObject(featureCursor);


            if (gm != null && theBearing.HasValue)
            {
                TapFunctions util = new TapFunctions();
                IPoint cp = (IPoint)gm;

                double rotationValue = Double.Parse(_map.Description) -90;
                double angle = theBearing.Value;


                Utilitys _aranSupportUtil = new Utilitys();
                


                IPoint start = (IPoint)EsriUtils.ToProject(cp, _map, pSpatialReference);
                angle = _aranSupportUtil.Azt2Direction(cp, angle, _map, pSpatialReference);

                var tmpPt1 = util.PointAlongPlane(start, angle +  90, 500);
                var tmpPt2 = util.PointAlongPlane(start, angle - 90, 500);

                IPolyline ln = new PolylineClass();

                ln.FromPoint = tmpPt1;
                ln.ToPoint = tmpPt2;

                IGeometry l1 = EsriUtils.ToGeo(ln, _map, pSpatialReference);

                IFeature pFeat = DecorLineCartography_featClass.CreateFeature();
                pFeat.Shape = l1;

                int fID = DecorLineCartography_featClass.FindField("type");
                if (fID >= 0) pFeat.set_Value(fID, "TORA");

                fID = DecorLineCartography_featClass.FindField("FeatureGuid");
                if (fID >= 0) pFeat.set_Value(fID, GeoID);


                pFeat.Store();

                gm = l1;

                util = null;
            }

            return gm;

        }



    }
}
