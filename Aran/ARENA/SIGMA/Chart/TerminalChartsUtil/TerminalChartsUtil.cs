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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public static class TerminalChartsUtil
    {
        enum tableBuildDestination
        {
            LeftDoun =0,
            RightUp = 1
        }


        public static void UpdateDinamicText(List<PDMObject> selectedProc, IHookHelper SigmaHookHelper, AirportHeliport selARP, List<RunwayDirection> selRwyDir, string vertUom, string distUom,int Rnp = -1)
        {
            try
            {
            SigmaHookHelper.ActiveView.GraphicsContainer.Reset();
            IElementProperties docElementProperties2;
            IElement dinamic_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();
            while (dinamic_el != null)
            {
                docElementProperties2 = dinamic_el as IElementProperties;
                string target_prop = "";
                if (docElementProperties2.Name.StartsWith("Sigma"))
                {
                    if (dinamic_el is IGroupElement)
                    {
                        IGroupElement dinamic_group_el = (IGroupElement)dinamic_el;
                        for (int i = 0; i < dinamic_group_el.ElementCount; i++)
                        {
                            IElement _el = dinamic_group_el.get_Element(i);

                            docElementProperties2 = (IElementProperties)_el;
                            target_prop = docElementProperties2.Name;
                            if (!target_prop.StartsWith("Sigma")) continue;
                            if (!(_el is ITextElement)) continue;
                            ITextElement txtEl = (ITextElement)_el;
                            string newtxt = ChartsHelperClass.GetTargetText(target_prop, selARP, selRwyDir, selectedProc,vertUom,distUom);
                            txtEl.Text = newtxt.Length > 0 ? newtxt : txtEl.Text;
                        }
                    }
                    else
                    {
                        target_prop = docElementProperties2.Name;
                        if (dinamic_el is ITextElement)
                        {
                            ITextElement txtEl = (ITextElement)dinamic_el;
                            string newtxt = ChartsHelperClass.GetTargetText(target_prop, selARP, selRwyDir, selectedProc,vertUom,distUom,Rnp);
                            
                            txtEl.Text = newtxt.Length > 0 ? newtxt : txtEl.Text;
                        }
                    }
                }
                dinamic_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();

            }

            }
            catch 
            {

            }

        }
 
        public static void UpdateDinamicText(IHookHelper SigmaHookHelper, string textName, string newTextValue, bool keepOld = true, double rotateAngle = 0)
        {
            try
            {
                IElement dinamic_el = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, textName);
                if (dinamic_el != null)
                {
                    if (dinamic_el is IGroupElement3)
                    {
                        IGroupElement3 grpEl = (IGroupElement3)dinamic_el;
                        for (int i = 0; i < grpEl.ElementCount; i++)
                        {
                            IElement elval = grpEl.get_Element(i);
                            IElementProperties3 prp = (IElementProperties3)elval;
                            if (!prp.Name.ToLower().StartsWith("value")) continue;
                            ITextElement txtEl = (ITextElement)elval;
                            txtEl.Text = keepOld ? txtEl.Text + newTextValue : newTextValue;
                            break;
                        }
                    }
                    else
                    {
                        ITextElement txtEl = (ITextElement)dinamic_el;
                        txtEl.Text = keepOld ? txtEl.Text + newTextValue : newTextValue;
                    }

                    if (rotateAngle != 0)
                    {
                        ITransform2D transformScaleTMP = dinamic_el as ITransform2D;
                        transformScaleTMP.Rotate((dinamic_el.Geometry as IPoint), rotateAngle);
                    }


                }
                else
                {
                    dinamic_el = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "Sigma");
                    if (dinamic_el is IGroupElement3)
                    {
                        IGroupElement3 grpEl = (IGroupElement3)dinamic_el;
                        for (int i = 0; i < grpEl.ElementCount; i++)
                        {
                            IElement elval = grpEl.get_Element(i);
                            IElementProperties3 prp = (IElementProperties3)elval;
                            if (!prp.Name.ToLower().StartsWith(textName.ToLower())) continue;
                            ITextElement txtEl = (ITextElement)elval;
                            txtEl.Text = keepOld ? txtEl.Text + newTextValue : newTextValue;
                            break;
                        }
                    }
                }
            }
            catch { }

        }

        public static void UpdateNorthArrowText(AirportHeliport selARP, double mapAngle, IHookHelper SigmaHookHelper)
        {
            if (selARP.MagneticVariation.HasValue)
            {
                string magvar = selARP.MagneticVariation.Value.ToString();

                if (selARP.MagneticVariation.Value > 0)
                {
                    magvar = magvar + "°E" + " - " + selARP.DateMagneticVariation;
                }
                else
                {
                    magvar = magvar + "°W" + " - " + selARP.DateMagneticVariation;
                }

                bool Flag = false;
                double angl = mapAngle + 90;
                angl = ChartsHelperClass.CheckedAngle(angl, ref Flag);
                angl = angl * Math.PI / 180;

                //IElement dinamic_el = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "Sigma_NorthArrow");
                //if (dinamic_el !=null)
                //{
                //    ITransform2D transformScaleTMP = dinamic_el as ITransform2D;
                //    transformScaleTMP.Rotate((dinamic_el.Geometry as IPoint), angl);
                //}


                //angl = angl - 90;
                //angl = ChartsHelperClass.CheckedAngle(angl, ref Flag);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_MagVariation", magvar, true, angl);


            }

            if (selARP.MagneticVariationChange != null && selARP.MagneticVariationChange.HasValue && !selARP.MagneticVariationChange.Value.ToString().StartsWith("NaN"))
            {
                string magvarStr = selARP.MagneticVariationChange.Value.ToString();


                if (selARP.MagneticVariationChange.Value > 0)
                {
                    magvarStr = magvarStr + " °E";
                }
                else
                {
                    magvarStr = magvarStr + " °W";
                }


                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AnnualRate", magvarStr, false);

            }

        }

        public static bool CheckPdmProjectType(SigmaChartTypes _Chart_Type, string mapDirectoryName)
        {
            bool res = true;
            string ConString = ArenaStaticProc.GetTargetDB();

            bool ItIsPdm = ConString.Contains("pdm.mdb");

            if (File.Exists(mapDirectoryName + @"\CEFID.txt"))
            {
                switch (_Chart_Type)
                {

                    case SigmaChartTypes.EnrouteChart_Type:
                    case SigmaChartTypes.SIDChart_Type:
                    case SigmaChartTypes.STARChart_Type:
                    case SigmaChartTypes.IAPChart_Type:
                    case SigmaChartTypes.PATChart_Type:
                    case SigmaChartTypes.AreaChart:
                    case SigmaChartTypes.MinimumAltitudeChart:
                        if (ItIsPdm)
                        {
                            MessageBox.Show("Since yo created 'Area and Terminal' project type, you are not able to create such a type of chart.", "SIGMA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            res = false;
                        }
                        break;
                    case SigmaChartTypes.AerodromeElectronicChart:
                    case SigmaChartTypes.AerodromeParkingDockingChart:
                    case SigmaChartTypes.AerodromeGroundMovementChart:
                    case SigmaChartTypes.AerodromeBirdChart:
                    case SigmaChartTypes.AerodromeChart:
                        if (ItIsPdm)
                        {
                            MessageBox.Show("Since yo created 'Area and Terminal' project type, you are not able to create such a type of chart.", "SIGMA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            res = false;
                        }
                        break;
                    case SigmaChartTypes.None:
                    default:
                        res = false;
                        break;
                }
            }
            return res;
        }

        public static void UpdateDinamicText(IHookHelper SigmaHookHelper, string textName, List<string> _arp_chanels)
        {
            if (_arp_chanels == null) return;
             IElement dinamic_el = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, textName);
             if (dinamic_el != null)
             {
                 ITextElement txtEl = (ITextElement)dinamic_el;
                 txtEl.Text = "";
                 if (_arp_chanels == null && _arp_chanels.Count <= 0) return;

                foreach (var chanel in _arp_chanels)
                 {
                    string str = "";

                    string[] words = chanel.Split(' ');
                    foreach (var st in words)
                    {
                        if (st.Trim().StartsWith("(") && st.Trim().EndsWith(")")) continue;
                        str = str + " " + st;
                    }

                    str = str.Length > 1 ? str : chanel;



                     txtEl.Text = txtEl.Text + "<BOL>"+ str + "</BOL>" + (char)13 + (char)10;
                 }
             }
        }

        public static IElement Get_GraphicsContainerElemnt_ByName(IGraphicsContainer _GraphicsContainer, string textName, bool IgnoreCase = true)
        {
            _GraphicsContainer.Reset();
            IElementProperties docElementProperties2;
            IElement dinamic_el = _GraphicsContainer.Next();
            bool flag = false;
            string elementname = IgnoreCase ? textName.ToUpper() : textName;

            while (dinamic_el != null)
            {
                docElementProperties2 = dinamic_el as IElementProperties;
                string curName = IgnoreCase ? docElementProperties2.Name.ToUpper() : docElementProperties2.Name;
                if (curName.CompareTo(elementname) == 0)
                {
                    flag = true;
                    break;

                }
                dinamic_el = _GraphicsContainer.Next();

            }

            if (flag) return dinamic_el;
            else return null;
        }

        public static IElement Get_GraphicsContainerElemnt_BeginWithName(IGraphicsContainer _GraphicsContainer, string textName, bool IgnoreCase = true)
        {
            _GraphicsContainer.Reset();
            IElementProperties docElementProperties2;
            IElement dinamic_el = _GraphicsContainer.Next();
            bool flag = false;
            string elementname = IgnoreCase ? textName.ToUpper() : textName;

            while (dinamic_el != null)
            {
                docElementProperties2 = dinamic_el as IElementProperties;
                string curName = IgnoreCase ? docElementProperties2.Name.ToUpper() : docElementProperties2.Name;
                if (curName.StartsWith(elementname))
                {
                    flag = true;
                    break;

                }
                dinamic_el = _GraphicsContainer.Next();

            }

            if (flag) return dinamic_el;
            else return null;
        }

        public static string GetCountryName(string ADHP_Designator)
        {
            string res = "";
            string PathToRegionsFile = ArenaStaticProc.GetPathToRegionsFile();
            var d = AreaManager.AreaUtils.GetCountryICAOCodes(PathToRegionsFile);

            string cntrIacaoCode = ADHP_Designator.Remove(2);
            foreach (KeyValuePair<string, List<string>> pair in d)
            {
                foreach (var item in pair.Value)
                {
                    if (item.CompareTo(cntrIacaoCode) == 0)
                    {
                        res = pair.Key;
                        break;
                    }
                }
            }

            return res;
        }

        private static void Set_TableCell_ByName(IGroupElement3 dinamic_el, string _text, string cellValue)
        {

            IGroupElement3 grpEl = (IGroupElement3)dinamic_el;
            for (int i = 0; i < grpEl.ElementCount; i++)
            {
                IElement elval = grpEl.get_Element(i);
                IElementProperties3 prp = (IElementProperties3)elval;
                if (!prp.Name.StartsWith(_text)) continue;
                ITextElement txtEl = (ITextElement)elval;
                txtEl.Text = cellValue;
                break;
            }
        }


        private static string Get_TableCell_ByName(IGroupElement3 dinamic_el, string textName)
        {
            string res = "";
            IGroupElement3 grpEl = (IGroupElement3)dinamic_el;
            for (int i = 0; i < grpEl.ElementCount; i++)
            {
                IElement elval = grpEl.get_Element(i);
                IElementProperties3 prp = (IElementProperties3)elval;
                if (!prp.Name.StartsWith(textName)) continue;
                ITextElement txtEl = (ITextElement)elval;
                res = txtEl.Text;
                break;
            }

            return res;
        }

        private static void Delete_GraphicsContainerElemnt_ByName(IHookHelper SigmaHookHelper, string textName, bool IgnoreCase = true, bool Begin_With = false)
        {
            IElement el = null;
            if (Begin_With)
                el = Get_GraphicsContainerElemnt_BeginWithName(SigmaHookHelper.ActiveView.GraphicsContainer, textName, IgnoreCase);
            else
                el = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, textName, IgnoreCase);
                

            if (el != null )
                SigmaHookHelper.ActiveView.GraphicsContainer.DeleteElement(el);

        }

        public static void CreateSectorAirspace_ChartElements(List<AirspaceVolume> selectedArspVol, IGeometry CenterPnt, IMap pMap,IHookHelper sigmaHookHelper,ISpatialReference pSpatialReference,
            IFeatureClass AnnoAirspaceGeo_featClass, IFeatureClass Anno_DecorPolyCartography_featClass, IFeatureClass Anno_DecorPointCartography_featClass, UOM_DIST_HORZ dUom, int circleStep, double maxCircleDist)
        {

            double mapScale = pMap.MapScale;
            TapFunctions util = new TapFunctions();


            #region Circle

            IPoint centerPoint = new PointClass { X = ((IPoint)CenterPnt).X, Y = ((IPoint)CenterPnt).Y };
            IPoint start = (IPoint)EsriUtils.ToProject(centerPoint, pMap, pSpatialReference);

            PDMObject tmpObj = new PDMObject();
            //double maxCircleDist = dUom == UOM_DIST_HORZ.NM ? 20 : 40;
            double radius = tmpObj.ConvertValueToMeter(circleStep, dUom.ToString()) / 111144.0;
            maxCircleDist = tmpObj.ConvertValueToMeter(maxCircleDist, dUom.ToString()) / 111144.0;
            int stepval = circleStep;
            AirspaceBuffer buf = new AirspaceBuffer(sigmaHookHelper);
            while (radius <= maxCircleDist)
            {
                ICircularArc circularArc = new CircularArcClass();
                IConstructCircularArc construtionCircularArc = circularArc as IConstructCircularArc;

                construtionCircularArc.ConstructCircle(centerPoint, radius, true);
                circularArc.SpatialReference = pMap.SpatialReference;


                object missing = Type.Missing;
                PolygonClass plgn = new PolygonClass();
                ISegmentCollection segcol = (ISegmentCollection)plgn;

                segcol.AddSegment((ISegment)circularArc, ref missing, ref missing);
                string GeoID = Guid.NewGuid().ToString();

                IFeature pFeat = Anno_DecorPolyCartography_featClass.CreateFeature();

                pFeat.Shape = plgn;

                int fID = Anno_DecorPolyCartography_featClass.FindField("type");
                if (fID >= 0) pFeat.set_Value(fID, "SECTOR");

                fID = Anno_DecorPolyCartography_featClass.FindField("FeatureGuid");
                if (fID >= 0) pFeat.set_Value(fID, GeoID);

                pFeat.Store();

                radius = radius + tmpObj.ConvertValueToMeter(circleStep, dUom.ToString()) / 111144.0;


                #region Distance 
                
                var tmpPt1 = util.PointAlongPlane(start, 90, tmpObj.ConvertValueToMeter(stepval, dUom.ToString()));
                tmpPt1 = (IPoint)EsriUtils.ToGeo(tmpPt1, pMap, pSpatialReference);

                ChartElement_SimpleText chrtEl_radius_SECTOR = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "CircleDistace");
                chrtEl_radius_SECTOR.TextContents[0][0].TextValue = stepval.ToString();

                chrtEl_radius_SECTOR.Anchor = new AncorPoint(0, 0);

                IElement el_radius = (IElement)chrtEl_radius_SECTOR.ConvertToIElement();

                IPoint pnt = new PointClass();
                pnt.PutCoords(tmpPt1.X, tmpPt1.Y);
                el_radius.Geometry = pnt;

                //GeoID = Guid.NewGuid().ToString();

                pFeat = Anno_DecorPointCartography_featClass.CreateFeature();
                pFeat.Shape = pnt;

                fID = Anno_DecorPointCartography_featClass.FindField("type");
                if (fID >= 0) pFeat.set_Value(fID, "CircleDistace");

                fID = Anno_DecorPointCartography_featClass.FindField("FeatureGuid");
                if (fID >= 0) pFeat.set_Value(fID, GeoID);

                pFeat.Store();

                ChartElementsManipulator.StoreSingleElementToDataSet("CircleDistace", GeoID, el_radius, ref chrtEl_radius_SECTOR, chrtEl_radius_SECTOR.Id, pMap.MapScale);

                #endregion

                stepval = stepval + circleStep;

            }

            #endregion

            #region Sector

            foreach (PDM.AirspaceVolume arspsVol in selectedArspVol)
            {
                try
                {


                    if (arspsVol.Geo == null) arspsVol.RebuildGeo2();
                    if (arspsVol.Geo == null || arspsVol.Geo.IsEmpty) continue;
                    if (((IArea)arspsVol.Geo).Area == 0) continue;


                    ChartElement_BorderedText_Collout_CaptionBottom chrtEl_Arspc_SECTOR = (ChartElement_BorderedText_Collout_CaptionBottom)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SectorAirspace");

                    chrtEl_Arspc_SECTOR.CaptionTextLine[0][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_SECTOR.CaptionTextLine[0][0].DataSource);
                    chrtEl_Arspc_SECTOR.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_SECTOR.TextContents[0][0].DataSource);

                    #region saveElement



                    IPoint cntr = ((IArea)arspsVol.Geo).Centroid;


                    chrtEl_Arspc_SECTOR.Anchor = new AncorPoint(cntr.X, cntr.Y);

                    IElement el_arspc = (IElement)chrtEl_Arspc_SECTOR.ConvertToIElement();


                    IPoint pnt = new PointClass();
                    pnt.PutCoords(cntr.X, cntr.Y);
                    el_arspc.Geometry = pnt;

                    ChartsHelperClass.SaveAirspace_ChartGeo(AnnoAirspaceGeo_featClass, arspsVol);


                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Arspc_SECTOR.Name, arspsVol.ID, el_arspc, ref chrtEl_Arspc_SECTOR, chrtEl_Arspc_SECTOR.Id, (int)mapScale);
                    Application.DoEvents();

                    #endregion


                }
                catch
                {
                    //MessageBox.Show(arsps.CodeID);
                    continue;
                }
            }


            #endregion


        }

        public static IPolygon Difference(IPolygon geom1, IPolygon geom2)
        {
            try
            {
                var topoOperator2 = geom1 as ITopologicalOperator2;
                if (topoOperator2 != null)
                {
                    IGeometry differenceGeometry = topoOperator2.Difference(geom2);
                    if (differenceGeometry == null || differenceGeometry.IsEmpty)
                        return null;

                    SimplifyGeometry(differenceGeometry);
                    return differenceGeometry as IPolygon;
                }
                return geom1;
            }
            catch (Exception)
            {
                return geom1;
            }
        }

        public static IPolygon Bugle(IPolygon geom1, double distance)
        {
            try
            {
                var topoOperator2 = geom1 as ITopologicalOperator2;
                if (topoOperator2 != null)
                {
                    IGeometry bufferGeometry = topoOperator2.Buffer(distance);
                    SimplifyGeometry(bufferGeometry);
                    var poly = bufferGeometry as IPolygon;
                    if (poly != null)
                    {
                        poly.Generalize(0.1);
                        SimplifyGeometry(poly);
                    }
                    return bufferGeometry as IPolygon;
                }
                return geom1;
            }
            catch (Exception)
            {
                return geom1;
            }
        }

        public static void UpdateDinamicLabels(IHookHelper SigmaHookHelper, DateTime effectiveDate, int AiracCircle, List<PDMObject> selectedProc, AirportHeliport selARP, List<RunwayDirection> selRwyDir, string vertUomSTR, string distUomSTR, int RNPFlag, List<string> arp_chanels)
        {


            if (AiracCircle >0)
            {
                string num = (AiracCircle % 100).ToString();
                int y = AiracCircle / 100;

                while (num.Length < 2) num = "0" + num;

                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_Airac", " " + num + @"/" + y.ToString(), false);
            }
            TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_EffectiveDate", (effectiveDate.Day.ToString() + " " + effectiveDate.ToString("MMM", CultureInfo.CreateSpecificCulture("en")) + " " + effectiveDate.Year.ToString()).ToUpper(), false);
            //TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_dateHeader", (effectiveDate.Day.ToString() + " " + effectiveDate.ToString("MMM", CultureInfo.CreateSpecificCulture("en")) + " " + effectiveDate.Year.ToString()).ToUpper(), false);
            TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "sigma_data", (effectiveDate.Day.ToString() + " " + effectiveDate.ToString("MMM", CultureInfo.CreateSpecificCulture("en")) + " " + effectiveDate.Year.ToString()).ToUpper(), false);
            
            if (selARP != null) 
            {
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_Authority", selARP.OrganisationAuthority, false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_ARP_HEADER", selARP.Designator,false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AirportICAOCode", selARP.Designator, false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AirportIATA", selARP.DesignatorIATA);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_ADHPNAME", selARP.Name, false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_country", TerminalChartsUtil.GetCountryName(selARP.Designator), false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_arpCoordLat", ArenaStaticProc.LatToDDMMSS(selARP.Y.Value.ToString(), coordtype.DDMMSS_SS_2), keepOld: false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_arpCoordLon", ArenaStaticProc.LatToDDMMSS(selARP.X.Value.ToString(), coordtype.DDMMSS_SS_2), keepOld: false);
                if (selARP.Elev.HasValue)
                {
                    double val = ArenaStaticProc.UomTransformation(selARP.Elev_UOM.ToString(), vertUomSTR, selARP.Elev.Value, 0);
                    TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AirportElev", val.ToString(), false);

                    string elevStr = "";
                    if (vertUomSTR.Contains("FT"))
                        elevStr = Math.Round(selARP.ConvertValueToFeet(selARP.Elev.Value, selARP.Elev_UOM.ToString()), 0).ToString();
                    else
                        elevStr = Math.Round(selARP.ConvertValueToMeter(selARP.Elev.Value, selARP.Elev_UOM.ToString()), 2).ToString();
                    TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_arpELEV", elevStr + vertUomSTR);

                }

               if( selARP.NavSystemCheckpoints!=null)
                {

                    var chpntVOR = selARP.NavSystemCheckpoints.FindAll(pnVr => pnVr.PDM_Type == PDM_ENUM.CheckpointVOR).FirstOrDefault();
                    if (chpntVOR !=null)
                        TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_CHECKPOINT_VOR", ((CheckpointVOR)chpntVOR).DesignatorVOR + "\r\n" + ((CheckpointVOR)chpntVOR).Frequency.Value.ToString(), false);

                }
            }

            if (selARP!=null && selARP.TransitionAltitude.HasValue)
            {
                string elevStr = "";

                if (vertUomSTR.Contains("FT"))
                    elevStr = Math.Round(selARP.ConvertValueToFeet(selARP.TransitionAltitude.Value, selARP.TransitionAltitudeUOM.ToString()), 0).ToString();
                else
                    elevStr = Math.Round(selARP.ConvertValueToMeter(selARP.TransitionAltitude.Value, selARP.TransitionAltitudeUOM.ToString()), 2).ToString();

                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_TransitionAlt",elevStr + " " + vertUomSTR, false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AirportHeliport_TransitionAltitude", elevStr + " " + vertUomSTR, false);
            }


            if (RNPFlag > 0) TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_RNP", Convert.ToInt16(RNPFlag).ToString());

            if (arp_chanels !=null) TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_chanels", arp_chanels);

            if (selectedProc != null)
            {
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AircraftCategories", "AIRCRAFT CATEGORY " + TerminalChartsUtil.GetAircfatCategoriesLine(selectedProc), false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_Instruction", TerminalChartsUtil.GetProcIntcructionLine(selectedProc), false);
                TerminalChartsUtil.UpdateDinamicText(selectedProc, SigmaHookHelper, selARP, selRwyDir, vertUomSTR, distUomSTR, RNPFlag);

            }


            IElement dinamic_el = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "Sigma_uomDist");
            if (dinamic_el != null)
            {
                ITextElement txtEl = (ITextElement)dinamic_el;
                string txt = txtEl.Text;
                if (txt.StartsWith("DIST IN ")) distUomSTR = "DIST IN " + distUomSTR;
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_uomDist", distUomSTR, false);

            }

            dinamic_el = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "Sigma_uomVert");
            if (dinamic_el != null)
            {
                ITextElement txtEl = (ITextElement)dinamic_el;
                string txt = txtEl.Text;
                if (txt.StartsWith("ALTITUDES AND ELEV IN") || txt.StartsWith("ELEV, ALT IN")) vertUomSTR = "ALTITUDES AND ELEV IN  " + vertUomSTR;
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_uomVert", vertUomSTR, false);

            }
           
        }


        public static void SimplifyGeometry(IGeometry geom)
        {
            var topOper2 = geom as ITopologicalOperator2;
            if (topOper2 != null)
            {
                topOper2.IsKnownSimple_2 = false;
                topOper2.Simplify();
            }
        }

        public static void CreateNorthArrow(IHookHelper SigmaHookHelper, AirportHeliport selARP)
        {
            IElementProperties3 docElementProperties;
            IElement sigma_el;

            GraphicsChartElement_NorthArrow chrtEl_NA = (GraphicsChartElement_NorthArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "NorthArrow");
            string magvar = "";

            chrtEl_NA.Name = "Sigma_NorthArrow_" + chrtEl_NA.Id;
            chrtEl_NA.GraphicsChartElementName = "Sigma_NorthArrow_" + selARP.Designator;
           // chrtEl_NA.Angle = 68.7;

            if (selARP.MagneticVariation != null && selARP.MagneticVariation.HasValue)
            {
                chrtEl_NA.SubTextContents.TextValue = Math.Round(selARP.MagneticVariation.Value,0).ToString();
                magvar = chrtEl_NA.SubTextContents.TextValue;


            }

            if (selARP.MagneticVariation.Value > 0)
            {
                chrtEl_NA.SubTextContents.EndSymbol.Text = "°E" + " - " + selARP.DateMagneticVariation;
                chrtEl_NA.hemiSphere = ANCOR.MapCore.hemiSphere.EasternHemisphere;

                magvar = magvar + "°E" + " - " + selARP.DateMagneticVariation;
                //: "W";
            }
            else
            {
                chrtEl_NA.SubTextContents.EndSymbol.Text = "°W" + " - " + selARP.DateMagneticVariation; ;
                chrtEl_NA.hemiSphere = ANCOR.MapCore.hemiSphere.WesternHemisphere;
                chrtEl_NA.TextShift = new ANCOR.MapCore.AncorPoint(0.5, 1.3);

                magvar = magvar + "°W" + " - " + selARP.DateMagneticVariation;
            }

            chrtEl_NA.TextContents[0][0].TextValue = "";
            chrtEl_NA.TextContents[0][0].EndSymbol.Text = "";
            chrtEl_NA.TextContents[1][0].TextValue = "";
            chrtEl_NA.TextContents[1][0].EndSymbol.Text = "";

            if (selARP.MagneticVariationChange != null && selARP.MagneticVariationChange.HasValue && !selARP.MagneticVariationChange.Value.ToString().StartsWith("NaN"))
            {
                string magRate = selARP.MagneticVariationChange.Value.ToString();
                //chrtEl_NA.TextContents[1][0].TextValue = chrtEl_NA.TextContents[1][0].TextValue + " " + selARP.MagneticVariationChange.Value.ToString();

                if (selARP.MagneticVariationChange.Value > 0)
                {
                    //chrtEl_NA.TextContents[1][0].EndSymbol.Text = "°E";
                    magRate = magRate + " °E";
                }
                else
                {
                    //chrtEl_NA.TextContents[1][0].EndSymbol.Text = "°W";
                    magRate = magRate + " °W";
                }
                
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_AnnualRate", magRate,false);
                TerminalChartsUtil.UpdateDinamicText(SigmaHookHelper, "Sigma_MagVAR", magvar, false); // если Sigma_AnnualRate нет
            }
            else
            {
                //    chrtEl_NA.TextContents[1][0].TextValue = "";
                //    chrtEl_NA.TextContents[1][0].EndSymbol.Text = "";
            }



            SigmaHookHelper.ActiveView.GraphicsContainer.Reset();

            sigma_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();
            while (sigma_el != null)
            {
                IPoint pos = null;

                docElementProperties = sigma_el as IElementProperties3;
                if (docElementProperties.Name.StartsWith("Sigma_NorthArrow"))
                {
                    if (sigma_el is IGroupElement)
                    {
                        for (int i = 0; i <= ((IGroupElement)sigma_el).ElementCount - 1; i++)
                        {
                            IElement tt = ((IGroupElement)sigma_el).get_Element(i);
                            if (((ITextElement)tt).Symbol.Font.Name.StartsWith("Arial")) continue;

                            pos = (IPoint)tt.Geometry;
                        }

                    }

                    else pos = (IPoint)sigma_el.Geometry;
                    chrtEl_NA.Position = new ANCOR.MapCore.AncorPoint(pos.X, pos.Y);
                    SigmaHookHelper.ActiveView.GraphicsContainer.DeleteElement(sigma_el);
                    break;
                }
                sigma_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();

            }


            IElement nortArrowEl = (IElement)chrtEl_NA.ConvertToIElement();

            docElementProperties = nortArrowEl as IElementProperties3;
            docElementProperties.Name = chrtEl_NA.Name;
            docElementProperties.AnchorPoint = esriAnchorPointEnum.esriCenterPoint;

            SigmaHookHelper.ActiveView.GraphicsContainer.AddElement(nortArrowEl, 0);
            
            ChartElementsManipulator.StoreGraphicsElementToDataSet(chrtEl_NA.Name, "NorthArrow", nortArrowEl, ref chrtEl_NA, chrtEl_NA.Id);

           

        }



        public static void CreateMsa(List<PDMObject> msa_list, IHookHelper SigmaHookHelper, AirportHeliport selARP)
        {

            IElementProperties3 docElementProperties;
            IElement sigma_el;

            foreach (SafeAltitudeArea msa_item in msa_list)
            {
                if (msa_item.SafeAltitudeAreaSector.Count <= 0) continue;

                GraphicsChartElement_SafeArea chrtEl_MSA = (GraphicsChartElement_SafeArea)ChartsHelperClass.getPrototypeChartElement( SigmaDataCash.prototype_anno_lst,"SafeArea");
                chrtEl_MSA.Name = "Sigma_MSA_" + chrtEl_MSA.Id;
                //chrtEl_MSA.MsaName = "Sigma_MSA_"+selARP.Designator;
                chrtEl_MSA.GraphicsChartElementName = "Sigma_MSA_" + selARP.Designator;
                chrtEl_MSA.ValDistOuter = 0;
                chrtEl_MSA.ValDistOuterUOM = "NM";

                chrtEl_MSA.Sectors.Clear();


                foreach (SafeAltitudeAreaSector msa_sector in msa_item.SafeAltitudeAreaSector)
                {
                    SafeArea_SectorDescription sec = new SafeArea_SectorDescription(msa_sector.FromAngle.Value, msa_sector.ToAngle.Value, msa_sector.LowerLimitVal.Value, msa_sector.LowerLimitUOM.ToString());
                    if (sec != null) chrtEl_MSA.Sectors.Add(sec);
                    if (msa_sector.OuterDistance.HasValue && chrtEl_MSA.ValDistOuter < msa_sector.OuterDistance.Value) chrtEl_MSA.ValDistOuter = msa_sector.OuterDistance.Value;
                }
                

                chrtEl_MSA.TextContents[0][0].TextValue = chrtEl_MSA.ValDistOuter.ToString();
                chrtEl_MSA.TextContents[0][1].TextValue = chrtEl_MSA.ValDistOuterUOM;
                chrtEl_MSA.TextContents[0][2].TextValue = GetPointChoice(msa_item.CentrePoint);// SegmentPointDesignator;

                SigmaHookHelper.ActiveView.GraphicsContainer.Reset();

                sigma_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();
                while (sigma_el != null)
                {
                    docElementProperties = sigma_el as IElementProperties3;
                    if (docElementProperties.Name.StartsWith("Sigma_MSA"))
                    {
                        IPoint pos = ((IArea)sigma_el.Geometry).Centroid;
                        chrtEl_MSA.Position = new ANCOR.MapCore.AncorPoint(pos.X, pos.Y);
                        SigmaHookHelper.ActiveView.GraphicsContainer.DeleteElement(sigma_el);
                        break;
                    }
                    sigma_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();

                }

                IElement graphicsEl = (IElement)chrtEl_MSA.ConvertToIElement();

                docElementProperties = graphicsEl as IElementProperties3;
                docElementProperties.Name = chrtEl_MSA.Name;
                docElementProperties.AnchorPoint = esriAnchorPointEnum.esriCenterPoint;

                SigmaHookHelper.ActiveView.GraphicsContainer.AddElement(graphicsEl, 0);

                ChartElementsManipulator.StoreGraphicsElementToDataSet(chrtEl_MSA.Name, msa_item.ID, graphicsEl, ref chrtEl_MSA, chrtEl_MSA.Id);

                break;
            }
        }



        public static void CreateMsa(SafeAltitudeArea msa_item, IHookHelper SigmaHookHelper, AirportHeliport selARP, UOM_DIST_VERT vertUom, UOM_DIST_HORZ distUom)
        {

            IElementProperties3 docElementProperties;
            IElement sigma_el;


            if (msa_item == null) return;

            GraphicsChartElement_SafeArea chrtEl_MSA = (GraphicsChartElement_SafeArea)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Sigma_MSA");
            chrtEl_MSA.Name = "Sigma_MSA_" + chrtEl_MSA.Id;
            //chrtEl_MSA.MsaName = "Sigma_MSA_"+selARP.Designator;
            chrtEl_MSA.GraphicsChartElementName = "Sigma_MSA_" + selARP.Designator;
            chrtEl_MSA.ValDistOuter = 0;
            chrtEl_MSA.ValDistOuterUOM = distUom.ToString();

            chrtEl_MSA.Sectors.Clear();


            foreach (SafeAltitudeAreaSector msa_sector in msa_item.SafeAltitudeAreaSector)
            {
                #region Округлить значение высот

                if (msa_sector.LowerLimitVal.HasValue && !Double.IsNaN(msa_sector.LowerLimitVal.Value) && msa_sector.LowerLimitUOM == UOM_DIST_VERT.M)
                {
                    var obj = ArenaStaticProc.UomTransformation(msa_sector.LowerLimitUOM.ToString(), vertUom.ToString(), (Double)msa_sector.LowerLimitVal.Value, 10);
                    int el = 0;
                    var ff = Math.DivRem((int)obj, 100, out el);
                    el = 100 * (ff + 1);
                    msa_sector.LowerLimitVal = el;
                    msa_sector.LowerLimitUOM = vertUom;
                }

                #endregion

                SafeArea_SectorDescription sec = new SafeArea_SectorDescription(msa_sector.FromAngle.Value, msa_sector.ToAngle.Value, msa_sector.LowerLimitVal.Value, msa_sector.LowerLimitUOM.ToString());
                if (sec != null) chrtEl_MSA.Sectors.Add(sec);
                if (msa_sector.OuterDistance.HasValue && chrtEl_MSA.ValDistOuter < msa_sector.OuterDistance.Value) chrtEl_MSA.ValDistOuter = msa_sector.OuterDistance.Value;
            }
            

            chrtEl_MSA.TextContents[0][0].TextValue = chrtEl_MSA.ValDistOuter.ToString();
            chrtEl_MSA.TextContents[0][1].TextValue = chrtEl_MSA.ValDistOuterUOM;
            chrtEl_MSA.TextContents[0][1].EndSymbol.Text = " ";
            chrtEl_MSA.TextContents[1][0].TextValue = GetPointChoice(msa_item.CentrePoint);// SegmentPointDesignator;

            SigmaHookHelper.ActiveView.GraphicsContainer.Reset();

            sigma_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();
            while (sigma_el != null)
            {
                docElementProperties = sigma_el as IElementProperties3;
                if (docElementProperties.Name.StartsWith("Sigma_MSA"))
                {
                    IPoint pos = ((IArea)sigma_el.Geometry).Centroid;
                    chrtEl_MSA.Position = new ANCOR.MapCore.AncorPoint(pos.X, pos.Y);
                    SigmaHookHelper.ActiveView.GraphicsContainer.DeleteElement(sigma_el);
                    break;
                }
                sigma_el = SigmaHookHelper.ActiveView.GraphicsContainer.Next();

            }

            IElement graphicsEl = (IElement)chrtEl_MSA.ConvertToIElement();

            docElementProperties = graphicsEl as IElementProperties3;
            docElementProperties.Name = chrtEl_MSA.Name;
            docElementProperties.AnchorPoint = esriAnchorPointEnum.esriCenterPoint;

            SigmaHookHelper.ActiveView.GraphicsContainer.AddElement(graphicsEl, 0);

            ChartElementsManipulator.StoreGraphicsElementToDataSet(chrtEl_MSA.Name, msa_item.ID, graphicsEl, ref chrtEl_MSA, chrtEl_MSA.Id);


        }


        private static string GetPointChoice(RouteSegmentPoint pointChoiceObj)
        {
            string res = "";
            switch (pointChoiceObj.PointChoice)
            {
                case PointChoice.DesignatedPoint:
                    res = pointChoiceObj.SegmentPointDesignator;
                    break;
                case PointChoice.Navaid:
                    res = pointChoiceObj.SegmentPointDesignator;
                    break;
                case PointChoice.TouchDownLiftOff:
                    res = "Tloff";
                    break;
                case PointChoice.RunwayCentrelinePoint:
                    res = "Rcp";
                    break;
                case PointChoice.AirportHeliport:
                    res = "ARP";
                    break;
                case PointChoice.NONE:
                case PointChoice.OTHER:
                    res = "";
                    break;
            }

            return res;
        }

        public static void CreateNavaids_ChartElements(List<PDMObject> navaidsList, double MapScale,  ref List<string> NavaidListID, IFeatureClass Anno_NavaidGeo_featClass, string VertUom)
        {
            foreach (NavaidSystem navSystem in navaidsList)
            {
                try
                {

                    if (NavaidListID.IndexOf(navSystem.ID) < 0)
                    {
                        NavaidListID.Add(navSystem.ID);

                        ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                        IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)navSystem, chrtEl_Navaid, VertUom);
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navSystem.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, MapScale);
                        ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)navSystem);

                    }

                }
                catch (Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(navSystem.GetObjectLabel() + (char)9 + navSystem.ID + (char)9 + ex.Message + (char)9 + ex.StackTrace);
                }
            }
        }

        public static void CreateProcedureLegs_ChartElements(List<ProcedureLeg> selectedLegs, bool rnavFlag, IMap FocusMap, ISpatialReference pSpatialReference, double ARP_MagneticVariation,
                                                        IFeatureClass Anno_LegGeo_featClass, IFeatureClass Anno_DesignatedGeo_featClass, IFeatureClass Anno_NavaidGeo_featClass,
                                                        IFeatureClass AnnoFacilitymakeUpGeo_featClass, IFeatureClass Anno_HoldingGeo_featClass, IFeatureClass Anno_HoldingpathGeo_featClass, UOM_DIST_VERT vertUom,
                                                        UOM_DIST_HORZ distUom, ref List<string> NavaidListID, ref List<string> DpnListID, bool IgnorFirstLeg = true, bool ShowOnlyFirstProcDesignator = false)
        {
           
            string curProcID = "";
            string curAdhp_ID = "";
            Utilitys _aranSupportUtil = new Utilitys();
            if (selectedLegs != null && selectedLegs.Count > 0)
            {
                curProcID = "";//selectedLegs[0].ProcedureIdentifier;
            }

            List<string> lengtElementsDictionary = new List<string>();
            List<string> speedElementsDictionary = new List<string>();
            List<string> courseElementsDictionary = new List<string>();
            List<string> nameElementsDictionary = new List<string>();
            List<string> angleIndicationDictionary = new List<string>();
            List<string> distanceIndicationDictionary = new List<string>();


            bool IF_flag = false;


            bool PrecisionFlag = selectedLegs.Where(finalLeg => finalLeg.PDM_Type == PDM_ENUM.FinalLeg && ((FinalLeg)finalLeg).LandingSystemCategory != CodeApproachGuidance.NON_PRECISION && ((FinalLeg)finalLeg).LandingSystemCategory != CodeApproachGuidance.OTHER).ToList() != null;


            var MisLeg = selectedLegs.FindAll(lg => lg.LegSpecialization == SegmentLegSpecialization.MissedApproachLeg).OrderByDescending(l => l.SeqNumberARINC).FirstOrDefault();
            if (MisLeg != null) MisLeg.PositionFlag = -1;



            //foreach (ProcedureLeg Legs in selectedLegs)
            for (int legIndx = selectedLegs.Count -1 ; legIndx >= 0; legIndx--)
            {
                ProcedureLeg Legs = selectedLegs[legIndx];
                try
                {

                    PROC_TYPE_code prcType = PROC_TYPE_code.Approach;
                    if (Legs.LegSpecialization == SegmentLegSpecialization.DepartureLeg) prcType = PROC_TYPE_code.SID;
                    if (Legs.LegSpecialization == SegmentLegSpecialization.ArrivalFeederLeg || Legs.LegSpecialization == SegmentLegSpecialization.ArrivalLeg) prcType = PROC_TYPE_code.STAR;


                    if (Legs.Lat.CompareTo(curAdhp_ID) != 0)
                    {
                        if (Legs.Lat.CompareTo("FINAL") != 0 && Legs.Lat.CompareTo("MISSED") != 0)
                        {
                            curAdhp_ID = Legs.Lat;
                            ARP_MagneticVariation = ARP_MagneticVariation != 0 ? ARP_MagneticVariation : 0;
                        }

                    }

                    if (Legs.Geo == null) Legs.RebuildGeo2();
                    if (Legs.Geo == null || Legs.Geo.IsEmpty) continue;

                    if (ARP_MagneticVariation == 0 && Legs.Lat != null && Legs.Lat.Length > 0) // признак того, что MagneticVariation фиктивный, и его надо получить из Airport, к которому относится процедура
                    {
                        try
                        {

                            if (IsNumeric(Legs.Lat))
                            {
                                var adhp = DataCash.GetAirport(Legs.Lat);
                                ARP_MagneticVariation = adhp != null && adhp.MagneticVariation.HasValue ? adhp.MagneticVariation.Value : 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.StackTrace);
                        }
                    }

                    IF_flag = ((IPolyline)Legs.Geo).Length == 0 || Legs.LegTypeARINC == CodeSegmentPath.IF;

                    #region LEGs Length, Speed, Course, Name


                    ChartElement_MarkerSymbol chrtEl_legLength = (ChartElement_MarkerSymbol)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ProcedureLegLength");
                    //chrtEl_legLength.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legLength.TextContents[0][0].DataSource, 1);
                    chrtEl_legLength.TextContents[0][0].TextValue = ChartsHelperClass.MakeText_UOM(Legs, chrtEl_legLength.TextContents[0][0].DataSource, distUom.ToString(), 1);

                    if (prcType == PROC_TYPE_code.SID && chrtEl_legLength.TextContents.Count >1)
                    {
                        chrtEl_legLength.TextContents[1][0].TextValue = ChartsHelperClass.MakeText_UOM(Legs, chrtEl_legLength.TextContents[1][0].DataSource, distUom.ToString(), 0);
                    }

                    ChartElement_SimpleText chrtEl_legSpeed = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ProcedureLegSpeed");
                    chrtEl_legSpeed.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legSpeed.TextContents[0][0].DataSource);
                    chrtEl_legSpeed.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legSpeed.TextContents[0][1].DataSource);
                    chrtEl_legSpeed.TextContents[0][2].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legSpeed.TextContents[0][2].DataSource);

                    if (chrtEl_legSpeed.TextContents[0][1].TextValue.Trim().CompareTo("KT") == 0)
                    {
                        chrtEl_legSpeed.TextContents[0][1].Visible = false;
                        chrtEl_legSpeed.TextContents[0][2].StartSymbol.Text = "K";
                    }
                    if (chrtEl_legSpeed.TextContents[0][1].TextValue.CompareTo("KM_H") == 0) chrtEl_legSpeed.TextContents[0][1].TextValue = "KM/H";



                    switch (Legs.SpeedInterpritation)
                    {
                        case AltitudeUseType.OTHER:
                        case AltitudeUseType.AS_ASSIGNED:
                        case AltitudeUseType.BETWEEN:
                        case AltitudeUseType.RECOMMENDED:

                            break;
                        case AltitudeUseType.ABOVE_LOWER:
                        case AltitudeUseType.AT_LOWER:
                        case AltitudeUseType.EXPECT_LOWER:
                            if (chrtEl_legSpeed.TextContents[0][0].StartSymbol.Text.Length > 0)
                            {
                                chrtEl_legSpeed.TextContents[0][0].StartSymbol.Text = "MIN ";
                                chrtEl_legSpeed.TextContents[0][0].StartSymbol.TextFont.Bold = chrtEl_legSpeed.TextContents[0][0].Font.Bold;
                            }
                            break;
                        case AltitudeUseType.BELOW_UPPER:
                            if (chrtEl_legSpeed.TextContents[0][0].StartSymbol.Text.Length > 0)
                            {
                                chrtEl_legSpeed.TextContents[0][0].StartSymbol.Text = "MAX ";
                                chrtEl_legSpeed.TextContents[0][0].StartSymbol.TextFont.Bold = chrtEl_legSpeed.TextContents[0][0].Font.Bold;
                            }
                            break;
                        default:
                            break;
                    }

                    ChartElement_SimpleText chrtEl_legCourse = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ProcedureLegCourse");

                    chrtEl_legSpeed.TextContents.ToString();

                    #region chrtEl_legCourse formating

                    //double crs = (double)ArenaStaticProc.GetObjectValue(Legs, "Course", false);
                    string crsType = ArenaStaticProc.GetObjectValueAsString(Legs, "CourseType");

                    if (!crsType.StartsWith("TRUE_TRACK") && !crsType.StartsWith("TRUE_BRG"))
                    {
                        chrtEl_legCourse.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legCourse.TextContents[0][0].DataSource);
                    }
                    else
                    {
                        chrtEl_legCourse.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legCourse.TextContents[0][0].DataSource, 0, 3, ARP_MagneticVariation, "NaN", true);
                    }

                    if (chrtEl_legCourse.TextContents[0][0].TextValue.Trim().Length < 3) chrtEl_legCourse.TextContents[0][0].StartSymbol.Text = "0";
                    //if (chrtEl_legCourse.TextContents[0][0].TextValue.Trim().Length < 3) chrtEl_legCourse.TextContents[0][0].StartSymbol.Text = "0";

                    if (!rnavFlag)
                    {
                        chrtEl_legCourse.TextContents.RemoveRange(1, 1);
                        chrtEl_legCourse.Anchor.Y = -10;
                    }
                    else
                    {
                        if (!crsType.StartsWith("TRUE_TRACK") && !crsType.StartsWith("TRUE_BRG"))
                        {
                            chrtEl_legCourse.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legCourse.TextContents[1][0].DataSource, 1, 3, ARP_MagneticVariation, "NaN", true);
                            //if (chrtEl_legCourse.TextContents[1][0].TextValue.Trim().Length < 5) chrtEl_legCourse.TextContents[1][0].StartSymbol.Text = "0";
                            //if (chrtEl_legCourse.TextContents[1][0].TextValue.Trim().Length < 5) chrtEl_legCourse.TextContents[1][0].StartSymbol.Text = "0";
                            chrtEl_legCourse.TextContents[1][0].StartSymbol.Text = "(";
                            chrtEl_legCourse.TextContents[1][0].EndSymbol.Text = chrtEl_legCourse.TextContents[1][0].EndSymbol.Text + "°T)";
                        }
                        else
                        {
                            chrtEl_legCourse.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legCourse.TextContents[1][0].DataSource, 1);
                            if (chrtEl_legCourse.TextContents[1][0].TextValue.Trim().Length < 5) chrtEl_legCourse.TextContents[1][0].StartSymbol.Text = "(0";
                            else chrtEl_legCourse.TextContents[1][0].StartSymbol.Text = "(";
                            chrtEl_legCourse.TextContents[1][0].EndSymbol.Text = chrtEl_legCourse.TextContents[1][0].EndSymbol.Text + "°T)";
                        }
                    }

                    #endregion

                    ChartElement_SimpleText chrtEl_legName = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ProcedureLegName");

                    if (Legs.LegTypeARINC != CodeSegmentPath.AF)
                    {
                        chrtEl_legName.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_legName.TextContents[0][0].DataSource);
                    }
                    else
                    {
                        if (Legs.DistanceIndication != null)
                        {

                            PDMObject navSystem = null;
                            string Fix = Legs.DistanceIndication.FixID;
                            string Nav = Legs.DistanceIndication.SignificantPointID;
                            IGeometry gm = Get_Angle_Distance_Line(Nav, Fix, ref navSystem);

                            string uom = Legs.DistanceIndication.DistanceUOM.ToString();
                            double dist = ArenaStaticProc.UomTransformation(uom, distUom.ToString(), Legs.DistanceIndication.Distance.Value, 1);

                            chrtEl_legName.TextContents[0][0].TextValue = ((NavaidSystem)navSystem).Designator + " " + dist.ToString();
                            chrtEl_legName.TextContents[0][0].EndSymbol.Text = " DME Arc";
                            chrtEl_legName.TextContents[0][0].EndSymbol.TextFont.Bold = true;
                        }
                    }


                    #region ShowOnlyFirstProcDesignator option

                    if (ShowOnlyFirstProcDesignator && (curProcID.CompareTo(Legs.ProcedureIdentifier) != 0))
                    {
                        chrtEl_legName.Placed = true;

                    }
                    else if (ShowOnlyFirstProcDesignator && (curProcID.CompareTo(Legs.ProcedureIdentifier) == 0))
                    {
                        chrtEl_legName.Placed = false;
                    }
                    else
                    {
                        chrtEl_legName.Placed = true;

                    }
                    curProcID = Legs.ProcedureIdentifier;

                    #endregion


                    if (chrtEl_legName.TextContents[0][0].TextValue.Contains("/"))
                        ChartsHelperClass.WrapText(ref chrtEl_legName);

                    ////////////////////

                    IPoint StPnt = new PointClass();
                    IPoint CntrPnt = new PointClass();
                    IPoint EndPnt = new PointClass();
                    double st; double en; double cn;


                    Polyline PolyLn = new PolylineClass();
                    IPointCollection RealShape = Legs.Geo as IPointCollection;

                    IPoint startPoint = RealShape.get_Point(0);
                    IPoint endPoint = RealShape.get_Point(RealShape.PointCount - 1);
                    {
                        (PolyLn as IPointCollection).AddPointCollection(RealShape);
                    }

                    bool reverse = GetInterestPoints(FocusMap, pSpatialReference, out StPnt, out CntrPnt, out EndPnt, (IPolyline)PolyLn, out cn);

                    chrtEl_legCourse.Slope = cn;
                    chrtEl_legSpeed.Slope = chrtEl_legSpeed.Anchor.X == 0 && chrtEl_legSpeed.Anchor.Y == 0 ? cn : 0;
                    chrtEl_legLength.Slope = cn > 180 ? cn - 360 : cn;
                    chrtEl_legName.Slope = cn;


                    //if (reverse) { chrtEl_legLength.Slope += 180; } // 94 95
                    if (reverse) { chrtEl_legLength.MarkerBackGround.CharacterIndex = 118; chrtEl_legLength.MarkerBackGround.InnerCharacterIndex = 119; } // 94 95

                    IElement el_legSpeed = (IElement)chrtEl_legSpeed.ConvertToIElement();
                    IElement el_legCourse = (IElement)chrtEl_legCourse.ConvertToIElement();

                    IElement el_legLength = (IElement)chrtEl_legLength.ConvertToIElement();
                    IElement el_legName = (IElement)chrtEl_legName.ConvertToIElement();

                    el_legSpeed.Geometry = new PointClass { X = endPoint.X + chrtEl_legSpeed.Anchor.X, Y = endPoint.Y + chrtEl_legSpeed.Anchor.Y };//EndPnt as IGeometry;
                    el_legCourse.Geometry = CntrPnt as IGeometry;//StPnt as IGeometry;
                    el_legName.Geometry = CntrPnt as IGeometry;//StPnt as IGeometry;

                    if (Legs.LegTypeARINC == CodeSegmentPath.AF)
                    {
                        chrtEl_legName.HorizontalAlignment = horizontalAlignment.Left;
                        chrtEl_legName.Anchor.X = 0;


                        if (((IPolyline)Legs.Geo).FromPoint.X > ((IPolyline)Legs.Geo).ToPoint.X)
                        {
                            ((IPolyline)Legs.Geo).ReverseOrientation();
                        }
                        else
                            chrtEl_legName.HorizontalAlignment = horizontalAlignment.Right;

                        el_legName = (IElement)chrtEl_legName.ConvertToIElement();
                        el_legName.Geometry = Legs.Geo;

                    }


                    IGroupElement GrEl = el_legLength as IGroupElement;
                    for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                    {

                        GrEl.get_Element(i).Geometry = CntrPnt as IGeometry;
                    }


                    ChartsHelperClass.SavePocedureLeg_ChartGeo(Anno_LegGeo_featClass, Legs);

                    if (Legs.LegTypeARINC != CodeSegmentPath.IF)
                    {
                        #region save chrtEl_legSpeed

                        if (speedElementsDictionary.IndexOf(chrtEl_legSpeed.GetTextContensAsString() + Math.Round(EndPnt.X, 6).ToString() + Math.Round(EndPnt.Y, 6).ToString()) < 0 && !IF_flag)
                        {
                            speedElementsDictionary.Add(chrtEl_legSpeed.GetTextContensAsString() + Math.Round(EndPnt.X, 6).ToString() + Math.Round(EndPnt.Y, 6).ToString());
                            if (chrtEl_legName.TextContents[0][0].TextValue.StartsWith("ignore") || chrtEl_legSpeed.TextContents[0][0].TextValue.ToUpper().StartsWith("NAN"))
                                chrtEl_legSpeed.Placed = false;
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legSpeed.Name, Legs.ID, el_legSpeed, ref chrtEl_legSpeed, chrtEl_legSpeed.Id, FocusMap.MapScale);

                        }

                        #endregion

                        #region save chrtEl_legCourse

                        //if (!Double.IsNaN(crs) && (Legs.LegTypeARINC != CodeSegmentPath.DF) && (Legs.LegTypeARINC != CodeSegmentPath.RF) && !IF_flag)
                        {
                            if (courseElementsDictionary.IndexOf(chrtEl_legCourse.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString()) < 0)
                            {
                                courseElementsDictionary.Add(chrtEl_legCourse.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString());
                                if (chrtEl_legName.TextContents[0][0].TextValue.StartsWith("ignore"))
                                    chrtEl_legCourse.Placed = false;
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legCourse.Name, Legs.ID, el_legCourse, ref chrtEl_legCourse, chrtEl_legCourse.Id, FocusMap.MapScale);

                            }

                        }

                        #endregion

                        #region save chrtEl_legLength

                        if (lengtElementsDictionary.IndexOf(chrtEl_legLength.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString()) < 0 && !IF_flag)
                        {
                            lengtElementsDictionary.Add(chrtEl_legLength.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString());
                            if (chrtEl_legName.TextContents[0][0].TextValue.StartsWith("ignore"))
                                chrtEl_legLength.Placed = false;
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legLength.Name, Legs.ID, el_legLength, ref chrtEl_legLength, chrtEl_legLength.Id, FocusMap.MapScale);

                        }

                        #endregion

                        #region save chrtEl_legName
                        if (IgnorFirstLeg)
                        {
                            if (Legs.SeqNumberARINC != 1)//if (step > 0)
                            {
                                if (nameElementsDictionary.IndexOf(chrtEl_legName.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString()) < 0)
                                {
                                    nameElementsDictionary.Add(chrtEl_legName.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString());
                                    if (chrtEl_legName.TextContents[0][0].TextValue.StartsWith("ignore"))
                                    {
                                        chrtEl_legName.TextContents[0][0].TextValue = chrtEl_legName.TextContents[0][0].TextValue.Remove(0, 6);
                                        chrtEl_legName.Placed = false;
                                    }
                                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legName.Name, Legs.ID, el_legName, ref chrtEl_legName, chrtEl_legName.Id, FocusMap.MapScale);
                                }

                            }
                        }
                        else
                        {
                            if (nameElementsDictionary.IndexOf(chrtEl_legName.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString()) < 0)
                            {
                                nameElementsDictionary.Add(chrtEl_legName.GetTextContensAsString() + Math.Round(CntrPnt.X, 6).ToString() + Math.Round(CntrPnt.Y, 6).ToString());
                                if (chrtEl_legName.TextContents[0][0].TextValue.StartsWith("ignore"))
                                {
                                    chrtEl_legName.TextContents[0][0].TextValue = chrtEl_legName.TextContents[0][0].TextValue.Remove(0, 6);
                                    chrtEl_legName.Placed = false;
                                }
                                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legName.Name, Legs.ID, el_legName, ref chrtEl_legName, chrtEl_legName.Id, FocusMap.MapScale);
                            }
                        }
                        #endregion

                    }

                    Application.DoEvents();

                    #endregion

                    #region Segment point

                    if (Legs.EndPoint != null)
                    {
                        if (Legs.EndPoint.Geo == null) Legs.EndPoint.RebuildGeo();

                        if (Legs.EndPoint.Geo != null)
                        {
                            NavaidSystem _NAVAID = null;

                            if (Legs.EndPoint.PointChoice == PDM.PointChoice.DesignatedPoint || (Legs.EndPoint.PointChoice == PDM.PointChoice.RunwayCentrelinePoint))
                            {
                                string _idSgmtPnt = GetUniversalId(Legs.EndPoint);

                                if (DpnListID.IndexOf(_idSgmtPnt) < 0)
                                {
                                    DpnListID.Add(_idSgmtPnt);

                                    bool FafFlag = (Legs.EndPoint.PointRole == ProcedureFixRoleType.FAF) && PrecisionFlag;

                                    if (Legs.EndPoint.PointChoice == PDM.PointChoice.DesignatedPoint || (Legs.EndPoint.PointChoice == PointChoice.RunwayCentrelinePoint && prcType == PROC_TYPE_code.Approach))
                                    {
                                        //if (Legs.EndPoint.PointRole != ProcedureFixRoleType.TP && !Legs.LegTypeARINC.ToString().EndsWith("A"))
                                        if (!Legs.LegTypeARINC.ToString().EndsWith("A"))

                                        {
                                            ChartElement_SigmaCollout_Designatedpoint chrtEl_DesigPoint = (ChartElement_SigmaCollout_Designatedpoint)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Designatedpoint");
                                            IElement el = null;

                                            if (prcType != PROC_TYPE_code.STAR)
                                                el = ChartsHelperClass.CreateSegmentPointAnno(Legs, chrtEl_DesigPoint, false, distUom.ToString(), rnavFlag);
                                            else
                                                el = ChartsHelperClass.CreateSegmentPointAnno_STAR(Legs, chrtEl_DesigPoint, false, distUom.ToString(), rnavFlag);


                                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_DesigPoint.Name, Legs.EndPoint.ID, el, ref chrtEl_DesigPoint, chrtEl_DesigPoint.Id, FocusMap.MapScale);
                                        }
                                        ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(Anno_DesignatedGeo_featClass, Legs.EndPoint, Legs.EndPoint.Geo, FafFlag);
                                    }
                                }
                            }
                            else if (Legs.EndPoint.PointChoice == PDM.PointChoice.Navaid)
                            {
                                _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                         where (element != null)
                                                             && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                             && (((PDM.NavaidSystem)element).Designator != null)
                                                             && (element.ID.StartsWith(Legs.EndPoint.PointChoiceID) || ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(Legs.EndPoint.PointChoiceID.Trim()))
                                                         select element).FirstOrDefault();

                                if (_NAVAID == null) _NAVAID = (NavaidSystem)DataCash.GetNavaidByID(Legs.StartPoint.PointChoiceID);


                            }


                            if (Legs.EndPoint.PointFacilityMakeUp != null && Legs.EndPoint.PointFacilityMakeUp.AngleIndication != null && Legs.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointType != null && Legs.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointType.StartsWith("Navaid"))
                            {
                                _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                         where (element != null)
                                                             && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                             && (((PDM.NavaidSystem)element).Designator != null)
                                                             && (element.ID.StartsWith(Legs.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID) ||
                                                                ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(Legs.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID.Trim()))
                                                         select element).FirstOrDefault();

                                if (_NAVAID == null) _NAVAID = (NavaidSystem)DataCash.GetNavaidByID(Legs.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID);

                            }
                            if (_NAVAID == null && Legs.EndPoint.PointFacilityMakeUp != null && Legs.EndPoint.PointFacilityMakeUp.DistanceIndication != null && Legs.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointType != null && Legs.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointType.StartsWith("Navaid"))
                            {
                                _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                         where (element != null)
                                                             && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                             && (((PDM.NavaidSystem)element).Designator != null)
                                                             && (element.ID.StartsWith(Legs.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID) ||
                                                                ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(Legs.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID.Trim()))
                                                         select element).FirstOrDefault();

                                if (_NAVAID == null) _NAVAID = (NavaidSystem)DataCash.GetNavaidByID(Legs.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);


                            }

                            if (_NAVAID != null && (NavaidListID.IndexOf(_NAVAID.ID) < 0))
                            {
                                NavaidListID.Add(_NAVAID.ID);

                                ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                                //IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, vertUom.ToString());
                                IElement el = null;

                                if (prcType != PROC_TYPE_code.STAR)
                                    el = el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, vertUom.ToString());
                                else
                                    el = ChartsHelperClass.CreateSegmentPointAnno_STAR((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, vertUom.ToString(), Legs.EndPoint.PointRole.HasValue ? Legs.EndPoint.PointRole.Value : ProcedureFixRoleType.OTHER);

                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, _NAVAID.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                                ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)_NAVAID);

                            }

                            if (Legs.EndPoint.PointFacilityMakeUp != null)
                            {
                                #region AngleIndication

                                if (Legs.EndPoint.PointFacilityMakeUp.AngleIndication != null)
                                {
                                    CreateStoreAngleIndication(Legs.EndPoint.PointFacilityMakeUp.AngleIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                                         Legs.EndPoint.PointFacilityMakeUp.ID, Legs.EndPoint, distUom.ToString(), vertUom.ToString(), ref angleIndicationDictionary, ref NavaidListID);


                                }

                                #endregion

                                #region DistanceIndication

                                if (Legs.EndPoint.PointFacilityMakeUp.DistanceIndication != null)
                                {

                                    CreateStoreDistanceIndication(Legs.EndPoint.PointFacilityMakeUp.DistanceIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                                        Legs.EndPoint, vertUom.ToString(), distUom.ToString(), ref angleIndicationDictionary, ref NavaidListID);

                                }

                                #endregion

                            }
                        }

                    }


                    if (Legs.StartPoint != null)
                    {
                        if (Legs.StartPoint.Geo == null) Legs.StartPoint.RebuildGeo();

                        if (Legs.StartPoint.Geo != null)
                        {
                            NavaidSystem _NAVAID = null;

                            if (Legs.StartPoint.PointChoice == PDM.PointChoice.DesignatedPoint || (Legs.StartPoint.PointChoice == PointChoice.RunwayCentrelinePoint))
                            {
                                string _idSgmtPnt = GetUniversalId(Legs.StartPoint);

                                //if (DpnListID.IndexOf(Legs.StartPoint.PointChoiceID) < 0)
                                if (DpnListID.IndexOf(_idSgmtPnt) < 0)
                                {
                                    //DpnListID.Add(Legs.StartPoint.PointChoiceID);
                                    DpnListID.Add(_idSgmtPnt);

                                    bool FafFlag = (Legs.StartPoint.PointRole == ProcedureFixRoleType.FAF) && PrecisionFlag;
                                    if (Legs.StartPoint.PointChoice == PDM.PointChoice.DesignatedPoint || (Legs.StartPoint.PointChoice == PointChoice.RunwayCentrelinePoint && prcType == PROC_TYPE_code.Approach))
                                    {
                                        //if (Legs.StartPoint.PointRole != ProcedureFixRoleType.TP && ! Legs.LegTypeARINC.ToString().EndsWith("A"))
                                        {
                                            ChartElement_SigmaCollout_Designatedpoint chrtEl_DesigPoint = (ChartElement_SigmaCollout_Designatedpoint)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Designatedpoint");
                                            IElement el = null;

                                            if (prcType != PROC_TYPE_code.STAR)
                                                el = ChartsHelperClass.CreateSegmentPointAnno(Legs, chrtEl_DesigPoint, true, distUom.ToString(), rnavFlag);
                                            else
                                                el = ChartsHelperClass.CreateSegmentPointAnno_STAR(Legs, chrtEl_DesigPoint, true, distUom.ToString(), rnavFlag);

                                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_DesigPoint.Name, Legs.StartPoint.ID, el, ref chrtEl_DesigPoint, chrtEl_DesigPoint.Id, FocusMap.MapScale);
                                        }
                                        ChartsHelperClass.SaveRouteSegmentPoint_ChartSegmentPointGeo(Anno_DesignatedGeo_featClass, Legs.StartPoint, Legs.StartPoint.Geo, FafFlag);
                                    }
                                }
                            }
                            else if (Legs.StartPoint.PointChoice == PDM.PointChoice.Navaid)
                            {


                                _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                         where (element != null)
                                                             && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                             && (((PDM.NavaidSystem)element).Designator != null)
                                                             && (element.ID.StartsWith(Legs.StartPoint.PointChoiceID) || ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(Legs.StartPoint.PointChoiceID.Trim()))
                                                         select element).FirstOrDefault();

                                if (_NAVAID == null) _NAVAID = (NavaidSystem)DataCash.GetNavaidByID(Legs.StartPoint.PointChoiceID);


                            }

                            if (Legs.StartPoint.PointFacilityMakeUp != null && Legs.StartPoint.PointFacilityMakeUp.AngleIndication != null && Legs.StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointType != null && Legs.StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointType.StartsWith("Navaid"))
                            {
                                _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                         where (element != null)
                                                             && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                             && (((PDM.NavaidSystem)element).Designator != null)
                                                             && (element.ID.StartsWith(Legs.StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID) ||
                                                                ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(Legs.StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID.Trim()))
                                                         select element).FirstOrDefault();

                                if (_NAVAID == null) _NAVAID = (NavaidSystem)DataCash.GetNavaidByID(Legs.StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID);

                            }
                            if (_NAVAID == null && Legs.StartPoint.PointFacilityMakeUp != null && Legs.StartPoint.PointFacilityMakeUp.DistanceIndication != null && Legs.StartPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointType != null && Legs.StartPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointType.StartsWith("Navaid"))
                            {
                                _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                         where (element != null)
                                                             && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                             && (element.ID.StartsWith(Legs.StartPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID) ||
                                                                ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(Legs.StartPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID.Trim()))
                                                         select element).FirstOrDefault();

                                if (_NAVAID == null) _NAVAID = (NavaidSystem)DataCash.GetNavaidByID(Legs.StartPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);

                            }

                            if (_NAVAID != null && (NavaidListID.IndexOf(_NAVAID.ID) < 0))
                            {
                                NavaidListID.Add(_NAVAID.ID);

                                ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                                //IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, vertUom.ToString());
                                IElement el = null;

                                if (prcType != PROC_TYPE_code.STAR)
                                    el = el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, vertUom.ToString());
                                else
                                    el = ChartsHelperClass.CreateSegmentPointAnno_STAR((PDM.NavaidSystem)_NAVAID, chrtEl_Navaid, vertUom.ToString(), Legs.StartPoint.PointRole.HasValue ? Legs.StartPoint.PointRole.Value : ProcedureFixRoleType.OTHER);


                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, _NAVAID.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                                ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)_NAVAID);

                            }

                            if (Legs.StartPoint.PointFacilityMakeUp != null)
                            {
                                #region AngleIndication

                                if (Legs.StartPoint.PointFacilityMakeUp.AngleIndication != null)
                                {
                                    CreateStoreAngleIndication(Legs.StartPoint.PointFacilityMakeUp.AngleIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                                        Legs.StartPoint.PointFacilityMakeUp.ID, Legs.StartPoint, distUom.ToString(), vertUom.ToString(), ref angleIndicationDictionary, ref NavaidListID);


                                }

                                #endregion

                                #region DistanceIndication

                                if (Legs.StartPoint.PointFacilityMakeUp.DistanceIndication != null)
                                {

                                    CreateStoreDistanceIndication(Legs.StartPoint.PointFacilityMakeUp.DistanceIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                                       Legs.StartPoint, vertUom.ToString(), distUom.ToString(),  ref angleIndicationDictionary, ref NavaidListID);


                                }

                                #endregion

                            }
                        }

                       
                    }
  
                    #endregion

                    if (Legs.HoldingUse != null && Legs.HoldingUse.HoldingPoint != null && Legs.HoldingUse.HoldingPoint.PointRole.HasValue)
                    {

                        CreateHoldingPatterns_ChartElement(Legs.HoldingUse, Legs, rnavFlag, FocusMap, ARP_MagneticVariation,  Anno_HoldingGeo_featClass, vertUom, distUom, Anno_HoldingpathGeo_featClass);


                        if (Legs.HoldingUse.EndPoint !=null)
                        {
                            #region AngleIndication

                            if (Legs.HoldingUse.EndPoint.PointFacilityMakeUp.AngleIndication != null)
                            {
                                CreateStoreAngleIndication(Legs.HoldingUse.EndPoint.PointFacilityMakeUp.AngleIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                                     Legs.HoldingUse.EndPoint.PointFacilityMakeUp.ID, Legs.HoldingUse.EndPoint, distUom.ToString(), vertUom.ToString(), ref angleIndicationDictionary, ref NavaidListID);


                            }

                            #endregion

                            #region DistanceIndication

                            if (Legs.HoldingUse.EndPoint.PointFacilityMakeUp.DistanceIndication != null)
                            {

                                CreateStoreDistanceIndication(Legs.HoldingUse.EndPoint.PointFacilityMakeUp.DistanceIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                                    Legs.HoldingUse.EndPoint, vertUom.ToString(), distUom.ToString(),  ref angleIndicationDictionary, ref NavaidListID);

                            }

                            #endregion

                        }

                    }

                    if (Legs.AngleIndication != null && Legs.EndPoint != null)
                    {
                        CreateStoreAngleIndication(Legs.AngleIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                        Legs.ID, Legs.EndPoint, distUom.ToString(), vertUom.ToString(), ref angleIndicationDictionary, ref NavaidListID, true);
                    }

                    if (Legs.DistanceIndication != null && Legs.LegTypeARINC != CodeSegmentPath.AF && Legs.EndPoint != null)
                    {
                        CreateStoreDistanceIndication(Legs.DistanceIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                        Legs.EndPoint, vertUom.ToString(), distUom.ToString(),  ref distanceIndicationDictionary, ref NavaidListID, true);
                    }
                    else if (Legs.DistanceIndication != null && Legs.LegTypeARINC == CodeSegmentPath.AF && Legs.EndPoint != null)
                    {
                        CreateStoreDistanceIndication(Legs.DistanceIndication, AnnoFacilitymakeUpGeo_featClass, Anno_NavaidGeo_featClass, FocusMap, pSpatialReference, ARP_MagneticVariation,
                       Legs.EndPoint, vertUom.ToString(), distUom.ToString(),  ref distanceIndicationDictionary, ref NavaidListID, false, Legs.Geo);
                    }


                    if (Legs.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                    {

                        PDMObject loc = null;
                        PDMObject gp = null;
                        if (((FinalLeg)Legs).StartPoint != null && ((FinalLeg)Legs).StartPoint.PointRole == ProcedureFixRoleType.FAF && ((FinalLeg)Legs).StartPoint.PointFacilityMakeUp != null && ((FinalLeg)Legs).StartPoint.PointFacilityMakeUp.AngleIndication != null)
                        {
                            string navId = ((FinalLeg)Legs).StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID;

                            var nav = DataCash.GetNavaidByID(navId);
                            if (nav != null)
                            {
                                loc = ((NavaidSystem)nav).Components.FindAll(eq => eq.PDM_Type == PDM_ENUM.Localizer).FirstOrDefault();
                                gp = ((NavaidSystem)nav).Components.FindAll(eq => eq.PDM_Type == PDM_ENUM.GlidePath).FirstOrDefault();


                            }

                            bool ItsPrecision = (((FinalLeg)Legs).LandingSystemCategory != CodeApproachGuidance.NON_PRECISION && ((FinalLeg)Legs).LandingSystemCategory != CodeApproachGuidance.OTHER) || loc != null;


                            if (ItsPrecision && loc != null)
                            {
                                ChartElement_ILSCollout chrtEl_ils = (ChartElement_ILSCollout)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ILSGlidePath");

                                if (loc != null)
                                {
                                    if (loc.Geo == null) loc.RebuildGeo();
                                    chrtEl_ils.locX = ((IPoint)loc.Geo).X; chrtEl_ils.locY = ((IPoint)loc.Geo).Y;
                                }
                                if (gp != null)
                                {
                                    if (gp.Geo == null) gp.RebuildGeo();
                                    chrtEl_ils.gpX = ((IPoint)gp.Geo).X; chrtEl_ils.gpY = ((IPoint)gp.Geo).Y;
                                }


                                if (((Localizer)loc).trueBearing.HasValue)
                                {

                                    double angle = _aranSupportUtil.Azt2Direction((IPoint)Legs.EndPoint.Geo, ((Localizer)loc).trueBearing.Value, FocusMap, pSpatialReference) - 180;
                                    chrtEl_ils.Slope = angle;
                                }


                                IPoint stPoint = null;
                                if (((FinalLeg)Legs).StartPoint.Geo == null) ((FinalLeg)Legs).StartPoint.RebuildGeo();
                                if (((FinalLeg)Legs).StartPoint.Geo != null)
                                {
                                    stPoint = new PointClass { X = ((IPoint)((FinalLeg)Legs).StartPoint.Geo).X, Y = ((IPoint)((FinalLeg)Legs).StartPoint.Geo).Y };


                                    #region LOC

                                    IPoint aPt = new PointClass();
                                    if (loc.Geo == null) loc.RebuildGeo();
                                    aPt.PutCoords(((IPoint)loc.Geo).X, ((IPoint)loc.Geo).Y);
                                    if (chrtEl_ils.IlsAnchorPoint == SigmaIlsAnchorPoint.LOC)
                                        chrtEl_ils.Anchor = new AncorPoint(aPt.X, aPt.Y);

                                    double _length = _aranSupportUtil.GetDistanceBetweenPoints_Elips(stPoint, aPt);
                                    IDisplayTransformation DT = (FocusMap as IActiveView).ScreenDisplay.DisplayTransformation;
                                    chrtEl_ils.DistToLOC = DT.ToPoints(_length);

                                    #endregion

                                    #region GP

                                    if (gp.Geo == null) gp.RebuildGeo();
                                    aPt.PutCoords(((IPoint)gp.Geo).X, ((IPoint)gp.Geo).Y);
                                    if (chrtEl_ils.IlsAnchorPoint == SigmaIlsAnchorPoint.GP)
                                        chrtEl_ils.Anchor = new AncorPoint(aPt.X, aPt.Y);

                                    _length = _aranSupportUtil.GetDistanceBetweenPoints_Elips(stPoint, aPt);
                                    DT = (FocusMap as IActiveView).ScreenDisplay.DisplayTransformation;
                                    chrtEl_ils.DistToGP = DT.ToPoints(_length);

                                    #endregion


                                    if (chrtEl_ils.IlsAnchorPoint == SigmaIlsAnchorPoint.LOC)
                                        chrtEl_ils.Length = Convert.ToInt32(chrtEl_ils.DistToLOC);
                                    else
                                        chrtEl_ils.Length = Convert.ToInt32(chrtEl_ils.DistToGP);



                                    IElement ils_El = (IElement)chrtEl_ils.ConvertToIElement();
                                    ils_El.Geometry = aPt;

                                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_ils.Name, Legs.ID, ils_El, ref chrtEl_ils, chrtEl_ils.Id, FocusMap.MapScale);

                                }
                            }
                        }
                    }


                    #region CodeSegmentPath.FC

                    if (Legs.LegTypeARINC == CodeSegmentPath.FC)
                    {
                        IPoint FC_Point = ((IPointCollection)Legs.Geo).Point[((IPointCollection)Legs.Geo).PointCount - 1];

                        NavaidSystem _NAVAID = (NavaidSystem)(from element in DataCash.ProjectEnvironment.Data.PdmObjectList
                                                              where (element != null)
                                                                  && (element.PDM_Type == PDM.PDM_ENUM.NavaidSystem)
                                                                  && (((PDM.NavaidSystem)element).Designator != null)
                                                                  && (element.ID.StartsWith(Legs.StartPoint.PointChoiceID) || ((PDM.NavaidSystem)element).Designator.Trim().StartsWith(Legs.StartPoint.SegmentPointDesignator.Trim()))
                                                              select element).FirstOrDefault();

                        if (_NAVAID == null) _NAVAID = (NavaidSystem)DataCash.GetNavaidByID(Legs.StartPoint.PointChoiceID);


                        #region FC_RadialDistance

                        
                        ChartElement_TextArrow chrtEl_FC_RadialDistance = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "FC_RadialDistance");


                        if (_NAVAID != null)
                        {

                            Legs.Course = _NAVAID.CodeNavaidSystemType.ToString().Contains("NDB") ? Legs.Course + 180 : Legs.Course;

                            //chrtEl_FC_RadialDistance.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_FC_RadialDistance.TextContents[0][0].DataSource, 0, 3, 0, "NaN", true);//"R046";
                            crsType = ArenaStaticProc.GetObjectValueAsString(Legs, "CourseType");

                            if (!crsType.StartsWith("TRUE_TRACK"))
                            {
                                chrtEl_FC_RadialDistance.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_FC_RadialDistance.TextContents[0][0].DataSource);
                            }
                            else
                            {
                                chrtEl_FC_RadialDistance.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Legs, chrtEl_FC_RadialDistance.TextContents[0][0].DataSource, 0, 3, ARP_MagneticVariation, "NaN", true);
                            }


                            chrtEl_FC_RadialDistance.TextContents[1][0].TextValue = ChartsHelperClass.MakeText_UOM(Legs, chrtEl_FC_RadialDistance.TextContents[1][0].DataSource, distUom.ToString(), 0);//"D14.6";
                            chrtEl_FC_RadialDistance.TextContents[1][1].TextValue = distUom.ToString();
                            chrtEl_FC_RadialDistance.TextContents[1][2].TextValue = ChartsHelperClass.MakeText(Legs.StartPoint, chrtEl_FC_RadialDistance.TextContents[1][2].DataSource);//"/PVL";

                            //chrtEl_FC_RadialDistance.Name = "SigmaCollout_Designatedpoint";


                            chrtEl_FC_RadialDistance.Anchor = new AncorPoint(FC_Point.X, FC_Point.Y);

                            IElement el = (IElement)chrtEl_FC_RadialDistance.ConvertToIElement();
                            el.Geometry = FC_Point;


                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_FC_RadialDistance.Name, Legs.StartPoint.ID, el, ref chrtEl_FC_RadialDistance, chrtEl_FC_RadialDistance.Id, FocusMap.MapScale);
                        }

                        #endregion

                        #region FC_Height


                        ChartElement_TextArrow chrtEl_FC_Height = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "FC_Height");
                        chrtEl_FC_Height.TextContents[0][0].TextValue = ChartsHelperClass.MakeText_UOM(Legs, chrtEl_FC_Height.TextContents[0][0].DataSource, vertUom.ToString(), 0);//"D14.6";

                        //chrtEl_FC_Height.Name = "SigmaCollout_Designatedpoint";


                        chrtEl_FC_Height.Anchor = new AncorPoint(FC_Point.X, FC_Point.Y);

                        IElement elH = (IElement)chrtEl_FC_Height.ConvertToIElement();
                        elH.Geometry = FC_Point;


                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_FC_Height.Name, Legs.StartPoint.ID, elH, ref chrtEl_FC_Height, chrtEl_FC_Height.Id, FocusMap.MapScale);

                        #endregion

                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(Legs.ProcedureIdentifier + " " + Legs.GetObjectLabel() + (char)9 + Legs.ID + (char)9 + ex.Message + (char)9 + ex.StackTrace);
                    continue;
                }
            }


            _aranSupportUtil = null;
        }


        public static string GetUniversalId(SegmentPoint segPoint)
        {
            string res = segPoint.PointChoiceID;

            if (segPoint.PointFacilityMakeUp != null)
            {
                if(segPoint.PointFacilityMakeUp.AngleIndication!=null && segPoint.PointFacilityMakeUp.AngleIndication.Angle.HasValue)
                {
                    res += Math.Round(segPoint.PointFacilityMakeUp.AngleIndication.Angle.Value,1).ToString();
                }
                if (segPoint.PointFacilityMakeUp.DistanceIndication != null && segPoint.PointFacilityMakeUp.DistanceIndication.Distance.HasValue)
                {
                    res += Math.Round(segPoint.PointFacilityMakeUp.DistanceIndication.Distance.Value,1).ToString();
                }
            }

            return res;
        }

        public static bool IsNumeric(string anyString)
        {
            if (anyString == null)
            {
                anyString = "";
            }
            if (anyString.Length > 0)
            {
                double dummyOut = new double();
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
                bool res =Double.TryParse(anyString, out dummyOut) && !Double.IsNaN(dummyOut);
                return res;
            }
            else
            {
                return false;
            }
        }
        

        public static void CreateHoldingPatterns_ChartElement(HoldingPattern Hldng, ProcedureLeg prcLeg, bool rnavFlag, IMap FocusMap, double ARP_MagneticVariation, 
                                                                IFeatureClass Anno_HoldingGeo_featClass, UOM_DIST_VERT vertUom, UOM_DIST_HORZ distUom,
                                                                IFeatureClass Anno_HoldingPathGeo_featClass, bool CreateHoldingPointAnno = true)
        {
            try
            {

                #region 

                // рассчет магнитного склонения

                double magVar;
                try
                {
                    double? altitude = Hldng.ConvertValueToMeter(Hldng.LowerLimit.Value, Hldng.LowerLimitUOM.ToString()) / 1000;

                    
                    magVar = IsNumeric(Hldng.HoldingPoint.Lat) ? ChartValidator.ExternalMagVariation.MagVar(Convert.ToDouble(Hldng.HoldingPoint.Lat), Convert.ToDouble(Hldng.HoldingPoint.Lon), altitude.Value,
                                        Hldng.ActualDate.Day, Hldng.ActualDate.Month, Hldng.ActualDate.Year, 1) :
                                         ChartValidator.ExternalMagVariation.MagVar(Convert.ToDouble(Hldng.HoldingPoint.Y), Convert.ToDouble(Hldng.HoldingPoint.X), altitude.Value,
                                        Hldng.ActualDate.Day, Hldng.ActualDate.Month, Hldng.ActualDate.Year, 1);
                }
                catch { magVar = ARP_MagneticVariation; }



                #region Округлить значение высот

                if (Hldng.LowerLimit.HasValue && !Double.IsNaN(Hldng.LowerLimit.Value) && Hldng.LowerLimitUOM == UOM_DIST_VERT.M)
                {
                    var obj = ArenaStaticProc.UomTransformation(Hldng.LowerLimitUOM.ToString(), vertUom.ToString(), (Double)Hldng.LowerLimit.Value, 10);
                    int el = 0;
                    var ff = Math.DivRem((int)obj, 100, out el);
                    el = 100 * (ff + 1);
                    Hldng.LowerLimit = el;
                    Hldng.LowerLimitUOM = vertUom;
                }
                if (Hldng.UpperLimit.HasValue && !Double.IsNaN(Hldng.UpperLimit.Value) && Hldng.UpperLimitUOM == UOM_DIST_VERT.M)
                {
                    var obj = ArenaStaticProc.UomTransformation(Hldng.UpperLimitUOM.ToString(), vertUom.ToString(), (Double)Hldng.UpperLimit.Value, 10);
                    int el = 0;
                    var ff = Math.DivRem((int)obj, 100, out el);
                    el = 100 * (ff + 1);
                    Hldng.UpperLimit= el;
                    Hldng.UpperLimitUOM = vertUom;

                }

                #endregion

                ChartsHelperClass.SaveHoldingPath(Anno_HoldingPathGeo_featClass, Hldng);
                ChartsHelperClass.SaveHolding_PointChartGeo(Anno_HoldingGeo_featClass, Hldng, ARP_MagneticVariation);

                if (CreateHoldingPointAnno)
                {
                    ChartsHelperClass.SaveHolding_PointChartGeo(Anno_HoldingGeo_featClass, Hldng);

                    ChartElement_SimpleText chrtEl_Hldng = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPattern");

                    chrtEl_Hldng.TextContents[1][0].TextValue = ChartsHelperClass.MakeText_UOM(Hldng, chrtEl_Hldng.TextContents[1][0].DataSource, vertUom.ToString()); //upperLimit
                    chrtEl_Hldng.TextContents[1][1].TextValue = vertUom.ToString();

                    if (chrtEl_Hldng.TextContents[1][1].TextValue.StartsWith("FL"))
                    {
                        AncorDataSource dsVal = (AncorDataSource)chrtEl_Hldng.TextContents[1][0].DataSource.Clone();
                        AncorDataSource dsUom = (AncorDataSource)chrtEl_Hldng.TextContents[1][1].DataSource.Clone();

                        chrtEl_Hldng.TextContents[1][0].DataSource = dsUom;
                        chrtEl_Hldng.TextContents[1][1].DataSource = dsVal;

                        chrtEl_Hldng.TextContents[1][0].TextValue = ChartsHelperClass.MakeText_UOM(Hldng, chrtEl_Hldng.TextContents[1][0].DataSource, vertUom.ToString()); //upperLimit
                        chrtEl_Hldng.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_Hldng.TextContents[1][1].DataSource);
                    }

                    chrtEl_Hldng.TextContents[2][0].TextValue = ChartsHelperClass.MakeText_UOM(Hldng, chrtEl_Hldng.TextContents[2][0].DataSource, vertUom.ToString()); //lowerLimit
                    chrtEl_Hldng.TextContents[2][1].TextValue = vertUom.ToString();

                    chrtEl_Hldng.TextContents[3][0].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_Hldng.TextContents[3][0].DataSource, Rounder: -1);
                    chrtEl_Hldng.TextContents[3][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_Hldng.TextContents[3][1].DataSource);


                    chrtEl_Hldng.Slope = Hldng.InboundCourse.HasValue ? 90 - Hldng.InboundCourse.Value : 0;
                    if (Hldng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_Hldng.Slope = Hldng.OutboundCourse.HasValue ? 90 - Hldng.OutboundCourse.Value : 0;

                    chrtEl_Hldng.Slope = Hldng.OutboundCourseType == CodeCourse.TRUE_BRG ? ChartsHelperClass.NormalizeSlope(chrtEl_Hldng.Slope) : ChartsHelperClass.NormalizeSlope(chrtEl_Hldng.Slope - magVar);

                    IElement hldng_lim_el = (IElement)chrtEl_Hldng.ConvertToIElement();
                    if (Hldng.HoldingPoint != null && Hldng.HoldingPoint.Geo == null)
                    {
                        Hldng.HoldingPoint.RebuildGeo();
                        if (Hldng.HoldingPoint.Geo == null) Hldng.HoldingPoint.Geo = ((IPointCollection)Hldng.Geo).get_Point(0);
                        hldng_lim_el.Geometry = Hldng.HoldingPoint.Geo;
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Hldng.Name, Hldng.HoldingPoint.ID, hldng_lim_el, ref chrtEl_Hldng, chrtEl_Hldng.Id, FocusMap.MapScale);

                    }

                }


                ///// InboundCourse
                ChartElement_SimpleText chrtEl_HldngInboundCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternInboundCource");
                /// OutboundCourse
                ChartElement_SimpleText chrtEl_HldngOutboundCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternOutboundCource");

               if (Hldng.InboundCourse != null && Hldng.InboundCourse.HasValue &&
               Hldng.OutboundCourse != null && Hldng.OutboundCourse.HasValue &&
               Hldng.InboundCourse.Value < Hldng.OutboundCourse.Value &&
               0 < Hldng.InboundCourse.Value && Hldng.InboundCourse.Value <= 179.9)
                {

                    AncorDataSource ds = (AncorDataSource)chrtEl_HldngInboundCource.TextContents[0][0].DataSource.Clone();
                    chrtEl_HldngInboundCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternOutboundCource");
                    chrtEl_HldngInboundCource.TextContents[0][0].DataSource = ds;
                    chrtEl_HldngInboundCource.TextContents[0][1].DataSource = ds;

                    ds = (AncorDataSource)chrtEl_HldngOutboundCource.TextContents[0][0].DataSource.Clone();
                    chrtEl_HldngOutboundCource = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "HoldingPatternInboundCource");
                    chrtEl_HldngOutboundCource.TextContents[0][0].DataSource = ds;
                    chrtEl_HldngOutboundCource.TextContents[0][1].DataSource = ds;
                }

                ////////////////////////////////////
                
                string crsType = ArenaStaticProc.GetObjectValueAsString(Hldng, "OutboundCourseType");

                #region Inbound
                if (!crsType.StartsWith("TRUE_TRACK") && !crsType.StartsWith("TRUE_BRG"))
                {
                    chrtEl_HldngInboundCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngInboundCource.TextContents[0][0].DataSource, 0, 3, 0, "NaN", true);
                }
                else
                {
                    chrtEl_HldngInboundCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngInboundCource.TextContents[0][0].DataSource, 0, 3, magVar, "NaN", true);
                }

                if (rnavFlag)
                {
                    string endS = "";//chrtEl_HldngInboundCource.TextContents[0][1].EndSymbol.Text.StartsWith(")") ? "T" : "T)";
                    if (!crsType.StartsWith("TRUE_TRACK") && !crsType.StartsWith("TRUE_BRG"))
                    {
                        chrtEl_HldngInboundCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngInboundCource.TextContents[0][1].DataSource, 1, 3, magVar, "NaN", true, endS);
                    }
                    else
                    {
                        chrtEl_HldngInboundCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngInboundCource.TextContents[0][1].DataSource, 1, 3, 0, "NaN", true, endS);
                    }
                }
                else
                {
                    chrtEl_HldngInboundCource.TextContents[0][0].EndSymbol = chrtEl_HldngInboundCource.TextContents[0][1].EndSymbol;
                    chrtEl_HldngInboundCource.TextContents[0].RemoveRange(1, 1);
                }
                //if (chrtEl_HldngInboundCource.TextContents.Count > 1) chrtEl_HldngInboundCource.TextContents.RemoveRange(1, 1);

                chrtEl_HldngInboundCource.Slope = Hldng.InboundCourse != null ? 90 - Hldng.InboundCourse.Value : 0;
                if (Hldng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngInboundCource.Slope = Hldng.OutboundCourse != null ? 90 - Hldng.OutboundCourse.Value : 0;

                //chrtEl_HldngInboundCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngInboundCource.Slope);
                chrtEl_HldngInboundCource.Slope = Hldng.OutboundCourseType == CodeCourse.TRUE_BRG ? ChartsHelperClass.NormalizeSlope(chrtEl_HldngInboundCource.Slope) : ChartsHelperClass.NormalizeSlope(chrtEl_HldngInboundCource.Slope - magVar);

                IElement hldng_cource_el = (IElement)chrtEl_HldngInboundCource.ConvertToIElement();
                if (Hldng.Geo == null) Hldng.RebuildGeo();

                hldng_cource_el.Geometry = ((IPointCollection)Hldng.Geo).get_Point(0);
                ChartElementsManipulator.StoreSingleElementToDataSet("ProcedureLegCourse", Hldng.HoldingPoint.ID, hldng_cource_el, ref chrtEl_HldngInboundCource, chrtEl_HldngInboundCource.Id, FocusMap.MapScale);

                #endregion
                ////////////////////////////////////

                ////////////////////////////////////
                #region Outboand
                if (!crsType.StartsWith("TRUE_TRACK") && !crsType.StartsWith("TRUE_BRG"))
                {
                    chrtEl_HldngOutboundCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngOutboundCource.TextContents[0][0].DataSource, 0, 3, 0, "NaN", true);
                }
                else
                {
                    chrtEl_HldngOutboundCource.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngOutboundCource.TextContents[0][0].DataSource, 0, 3, magVar, "NaN", true);
                }

                if (rnavFlag)
                {
                    string endS = "";//chrtEl_HldngOutboundCource.TextContents[0][1].EndSymbol.Text.StartsWith(")") ? "T" : "T)";
                    if (!crsType.StartsWith("TRUE_TRACK") && !crsType.StartsWith("TRUE_BRG"))
                    {
                        chrtEl_HldngOutboundCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngOutboundCource.TextContents[0][1].DataSource, 1, 3, magVar, "NaN", true, endS);
                    }
                    else
                    {
                        chrtEl_HldngOutboundCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngOutboundCource.TextContents[0][1].DataSource, 1, 3, 0, "NaN", true, endS);
                    }
                    //chrtEl_HldngOutboundCource.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_HldngOutboundCource.TextContents[0][1].DataSource, 0, 3, magVar, "NaN", true, endS);
                    //if (chrtEl_HldngOutboundCource.TextContents.Count > 1) chrtEl_HldngOutboundCource.TextContents.RemoveRange(1, 1);
                }
                else
                {
                    chrtEl_HldngOutboundCource.TextContents[0][0].EndSymbol = chrtEl_HldngOutboundCource.TextContents[0][1].EndSymbol;
                    chrtEl_HldngOutboundCource.TextContents[0].RemoveRange(1, 1);
                }

                chrtEl_HldngOutboundCource.Slope = Hldng.InboundCourse != null ? 90 - Hldng.InboundCourse.Value : 0;
                if (Hldng.TurnDirection == DirectionTurnType.RIGHT) chrtEl_HldngOutboundCource.Slope = Hldng.OutboundCourse != null ? 90 - Hldng.OutboundCourse.Value : 0;

                //chrtEl_HldngOutboundCource.Slope = ChartsHelperClass.NormalizeSlope(chrtEl_HldngOutboundCource.Slope);
                chrtEl_HldngOutboundCource.Slope = Hldng.OutboundCourseType == CodeCourse.TRUE_BRG ? ChartsHelperClass.NormalizeSlope(chrtEl_HldngOutboundCource.Slope) : ChartsHelperClass.NormalizeSlope(chrtEl_HldngOutboundCource.Slope - magVar);


                hldng_cource_el = (IElement)chrtEl_HldngOutboundCource.ConvertToIElement();
                hldng_cource_el.Geometry = ((IPointCollection)Hldng.Geo).get_Point(((IPointCollection)Hldng.Geo).PointCount / 2);

                ChartElementsManipulator.StoreSingleElementToDataSet("ProcedureLegCourse", Hldng.HoldingPoint.ID, hldng_cource_el, ref chrtEl_HldngOutboundCource, chrtEl_HldngOutboundCource.Id, FocusMap.MapScale);

                #endregion
                ////////////////////////////////////
                ///
                #region Holding Speed

                if (Hldng.SpeedLimit.HasValue && Hldng.SpeedLimit.Value >=0)
                {
                    ChartElement_SimpleText chrtEl_legSpeed = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ProcedureLegSpeed");
                    chrtEl_legSpeed.TextContents[0][0].Font.Bold = false;
                    chrtEl_legSpeed.TextContents[0][1].Font.Bold = false;
                    chrtEl_legSpeed.TextContents[0][0].StartSymbol.TextFont.Bold = false;

                    chrtEl_legSpeed.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_legSpeed.TextContents[0][0].DataSource);
                    chrtEl_legSpeed.TextContents[0][1].DataSource.Value = "SpeedLimitUOM";
                    chrtEl_legSpeed.TextContents[0][1].TextValue = ChartsHelperClass.MakeText(Hldng, chrtEl_legSpeed.TextContents[0][1].DataSource);
                    chrtEl_legSpeed.TextContents[0][2].Visible = false;

                    if (chrtEl_legSpeed.TextContents[0][1].TextValue.Trim().CompareTo("KT") == 0)
                    {
                        chrtEl_legSpeed.TextContents[0][1].Visible = false;
                        chrtEl_legSpeed.TextContents[0][2].StartSymbol.Text = "K";
                    }
                    if (chrtEl_legSpeed.TextContents[0][1].TextValue.CompareTo("KM_H") == 0) chrtEl_legSpeed.TextContents[0][1].TextValue = "KM/H";

                    chrtEl_legSpeed.TextContents[0][0].StartSymbol.Text = "MAX ";
                    chrtEl_legSpeed.TextContents[0][0].StartSymbol.TextFont.Bold = chrtEl_legSpeed.TextContents[0][0].Font.Bold;

                    

                    chrtEl_legSpeed.Slope = Hldng.OutboundCourseType == CodeCourse.TRUE_BRG ? ChartsHelperClass.NormalizeSlope(chrtEl_HldngInboundCource.Slope) : ChartsHelperClass.NormalizeSlope(chrtEl_HldngInboundCource.Slope - magVar);
                    IElement el_legSpeed = (IElement)chrtEl_legSpeed.ConvertToIElement();
                    el_legSpeed.Geometry = ((IPointCollection)Hldng.Geo).get_Point(0);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legSpeed.Name, Hldng.HoldingPoint.ID, el_legSpeed, ref chrtEl_legSpeed, chrtEl_legSpeed.Id, FocusMap.MapScale);



                }

                #endregion

                #endregion

            }
            catch (Exception ex)
            {
                if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                SigmaDataCash.Report.Add(Hldng.GetObjectLabel() + (char)9 + Hldng.ID + (char)9 + ex.Message + (char)9 + ex.StackTrace);
                
            }
        }

        public static FinalProfile _UpdateProfile(RunwayDirection selRwyDir, InstrumentApproachProcedure _iapProc, double OCA_Value_InFT, int FinalLegsCount, IMap focusMap, ISpatialReference pSpatialReference, UOM_DIST_VERT vertUom, UOM_DIST_HORZ distUom)
        {
            try
            {



                FinalProfile fProfile = new FinalProfile { ApproachAltitudeTable = new List<ApproachAltitude>(), ApproachDistancetable = new List<ApproachDistance>(), ApproachMinimaTable = new List<ApproachMinima>() };
                ProcedureFixRoleType[] arrayProcedureFixRoleTypeArray = { ProcedureFixRoleType.FAF, ProcedureFixRoleType.SDF, ProcedureFixRoleType.MAPT };

                List<ProcedureFixRoleType> arrayProcedureFixRoleTypelist = arrayProcedureFixRoleTypeArray.ToList();

                double Bearing = selRwyDir.TrueBearing.HasValue ? selRwyDir.TrueBearing.Value : selRwyDir.MagBearing.Value;

                foreach (ProcedureTransitions _tran in _iapProc.Transitions)
                {
                    
                    foreach (ProcedureLeg _leg in _tran.Legs)
                    {

                        //var _legPnt = _leg.StartPoint != null ? _leg.StartPoint : _leg.EndPoint;

                        //if (_leg.EndPoint.PointRole.HasValue && _leg.EndPoint.PointRole.Value == ProcedureFixRoleType.MAPT)
                        //    _legPnt = _leg.EndPoint;

                        var _legPnt = _leg.StartPoint;

                        if (_legPnt != null)
                        {
                            if (_legPnt.PointRole.HasValue && arrayProcedureFixRoleTypelist.Contains(_legPnt.PointRole.Value))
                            {
                                ApproachAltitude hpAlt = new ApproachAltitude { Altitude = _leg.UpperLimitAltitude.Value, AltitudeReference = CODE_DIST_VER.MSL, AltitudeUOM = _leg.UpperLimitAltitudeUOM };
                                ApproachDistance hpDist = new ApproachDistance { DistanceUOM = UOM_DIST_HORZ.M, ValueHAT = null, ValueHATUOM = UOM_DIST_VERT.SM, EndingMeasurementPoint = CodeProcedureDistance.THLD };

                                switch (_legPnt.PointRole.Value)
                                {
                                    case ProcedureFixRoleType.IAF:
                                    case ProcedureFixRoleType.IF:
                                    case ProcedureFixRoleType.IF_IAF:
                                    case ProcedureFixRoleType.VDP:
                                    case ProcedureFixRoleType.ENRT:
                                    case ProcedureFixRoleType.OTHER_WPT:
                                    case ProcedureFixRoleType.ENRT_HLDNG:
                                    case ProcedureFixRoleType.MAHF:
                                    case ProcedureFixRoleType.FPAP:
                                    case ProcedureFixRoleType.FTP:
                                    case ProcedureFixRoleType.FROP:
                                    case ProcedureFixRoleType.TP:
                                    case ProcedureFixRoleType.OTHER:
                                        break;
                                    case ProcedureFixRoleType.FAF:
                                        hpDist.StartingMeasurementPoint = CodeProcedureDistance.FAF;
                                        hpAlt.MeasurementPoint = CodeProcedureDistance.FAF;
                                        break;
                                    case ProcedureFixRoleType.SDF:
                                        if (FinalLegsCount > 1)
                                        {
                                            hpDist.StartingMeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                                            hpAlt.MeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                                        }
                                        else
                                        {
                                            hpDist = null;
                                            hpAlt = null;
                                        }
                                        break;
                                    case ProcedureFixRoleType.MAPT:
                                        hpDist.StartingMeasurementPoint = CodeProcedureDistance.MAP;
                                        hpAlt.MeasurementPoint = CodeProcedureDistance.MAP;
                                        hpAlt.Altitude = OCA_Value_InFT!=-100? OCA_Value_InFT : 100;
                                        hpAlt.AltitudeUOM = UOM_DIST_VERT.FT;


                                        break;
                                    default:
                                        break;
                                }

                                if (hpDist != null)
                                    hpDist.Distance = calcDistFromRwy(_legPnt, focusMap, pSpatialReference, selRwyDir, Bearing);

                                if (hpDist != null) fProfile.ApproachDistancetable.Add(hpDist);
                                if (hpAlt != null) fProfile.ApproachAltitudeTable.Add(hpAlt);

                                arrayProcedureFixRoleTypelist.Remove(_legPnt.PointRole.Value);

                            }
                        }

                        if (_tran.RouteType == ProcedurePhaseType.FINAL)
                        {
                            try
                            {

                                if (_leg.AssessmentArea != null && _leg.AssessmentArea.Count > 0 && _leg.AssessmentArea[0].AssessedAltitude.HasValue)
                                {
                                    ApproachMinima appMin = new ApproachMinima { Minima = _leg.AssessmentArea[0].AssessedAltitude.Value, MinimaUom = _leg.AssessmentArea[0].AssessedAltitudeUOM };
                                    appMin.ProfileSegmnetDesignator = _legPnt.PointRole.ToString() + ":" + _legPnt.PointRole.ToString();

                                    fProfile.ApproachMinimaTable.Add(appMin);
                                }

                            }
                            catch { continue; }

                        }


                        #region End

                        _legPnt = _leg.EndPoint;

                        if (_legPnt != null)
                        {
                            if (_legPnt.PointRole.HasValue && arrayProcedureFixRoleTypelist.Contains(_legPnt.PointRole.Value))
                            {
                                ApproachAltitude hpAlt = new ApproachAltitude { Altitude = _leg.UpperLimitAltitude.Value, AltitudeReference = CODE_DIST_VER.MSL, AltitudeUOM = _leg.UpperLimitAltitudeUOM };
                                ApproachDistance hpDist = new ApproachDistance { DistanceUOM = UOM_DIST_HORZ.M, ValueHAT = null, ValueHATUOM = UOM_DIST_VERT.SM, EndingMeasurementPoint = CodeProcedureDistance.THLD };

                                switch (_legPnt.PointRole.Value)
                                {
                                    case ProcedureFixRoleType.IAF:
                                    case ProcedureFixRoleType.IF:
                                    case ProcedureFixRoleType.IF_IAF:
                                    case ProcedureFixRoleType.VDP:
                                    case ProcedureFixRoleType.ENRT:
                                    case ProcedureFixRoleType.OTHER_WPT:
                                    case ProcedureFixRoleType.ENRT_HLDNG:
                                    case ProcedureFixRoleType.MAHF:
                                    case ProcedureFixRoleType.FPAP:
                                    case ProcedureFixRoleType.FTP:
                                    case ProcedureFixRoleType.FROP:
                                    case ProcedureFixRoleType.TP:
                                    case ProcedureFixRoleType.OTHER:
                                        break;
                                    case ProcedureFixRoleType.FAF:
                                        hpDist.StartingMeasurementPoint = CodeProcedureDistance.FAF;
                                        hpAlt.MeasurementPoint = CodeProcedureDistance.FAF;
                                        break;
                                    case ProcedureFixRoleType.SDF:
                                        if (FinalLegsCount > 1)
                                        {
                                            hpDist.StartingMeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                                            hpAlt.MeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                                        }
                                        else
                                        {
                                            hpDist = null;
                                            hpAlt = null;
                                        }
                                        break;
                                    case ProcedureFixRoleType.MAPT:
                                        hpDist.StartingMeasurementPoint = CodeProcedureDistance.MAP;
                                        hpAlt.MeasurementPoint = CodeProcedureDistance.MAP;
                                        hpAlt.Altitude = OCA_Value_InFT != -100 ? OCA_Value_InFT : 100;
                                        hpAlt.AltitudeUOM = UOM_DIST_VERT.FT;


                                        break;
                                    default:
                                        break;
                                }

                                if (hpDist != null)
                                    hpDist.Distance = calcDistFromRwy(_legPnt, focusMap, pSpatialReference, selRwyDir, Bearing);

                                if (hpDist != null) fProfile.ApproachDistancetable.Add(hpDist);
                                if (hpAlt != null) fProfile.ApproachAltitudeTable.Add(hpAlt);

                                arrayProcedureFixRoleTypelist.Remove(_legPnt.PointRole.Value);

                            }
                        }

                        if (_tran.RouteType == ProcedurePhaseType.FINAL)
                        {
                            try
                            {

                                if (_leg.AssessmentArea != null && _leg.AssessmentArea.Count > 0 && _leg.AssessmentArea[0].AssessedAltitude.HasValue)
                                {
                                    ApproachMinima appMin = new ApproachMinima { Minima = _leg.AssessmentArea[0].AssessedAltitude.Value, MinimaUom = _leg.AssessmentArea[0].AssessedAltitudeUOM };
                                    appMin.ProfileSegmnetDesignator = _legPnt.PointRole.ToString() + ":" + _legPnt.PointRole.ToString();

                                    fProfile.ApproachMinimaTable.Add(appMin);
                                }

                            }
                            catch { continue; }

                        }

                        #endregion

                    }
                }


                if (fProfile.ApproachAltitudeTable.Count > 0)
                {
                    ApproachAltitude hpAlt = new ApproachAltitude { Altitude = 15, AltitudeReference = CODE_DIST_VER.MSL, AltitudeUOM = UOM_DIST_VERT.M, MeasurementPoint = CodeProcedureDistance.THLD };
                    fProfile.ApproachAltitudeTable.Insert(fProfile.ApproachAltitudeTable.Count - 1, hpAlt);
                }

                return fProfile;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static FinalProfile UpdateProfile(RunwayDirection selRwyDir, InstrumentApproachProcedure _iapProc, double OCA_Value_InFT, int FinalLegsCount, IMap focusMap, ISpatialReference pSpatialReference, UOM_DIST_VERT vertUom, UOM_DIST_HORZ distUom)
        {
            try
            {



                FinalProfile fProfile = new FinalProfile { ApproachAltitudeTable = new List<ApproachAltitude>(), ApproachDistancetable = new List<ApproachDistance>(), ApproachMinimaTable = new List<ApproachMinima>() };
                ProcedureFixRoleType[] arrayProcedureFixRoleTypeArray = { ProcedureFixRoleType.FAF, ProcedureFixRoleType.SDF, ProcedureFixRoleType.MAPT };

                List<ProcedureFixRoleType> arrayProcedureFixRoleTypelist = arrayProcedureFixRoleTypeArray.ToList();

                double Bearing = selRwyDir.TrueBearing.HasValue ? selRwyDir.TrueBearing.Value : selRwyDir.MagBearing.Value;

                foreach (ProcedureTransitions _tran in _iapProc.Transitions)
                {

                    foreach (ProcedureLeg _leg in _tran.Legs)
                    {

                        var _legPnt = _leg.StartPoint != null ? _leg.StartPoint : _leg.EndPoint;


                        if (_legPnt != null)
                        {
                            if (_legPnt.PointRole.HasValue && arrayProcedureFixRoleTypelist.Contains(_legPnt.PointRole.Value))
                            {
                                ApproachAltitude hpAlt = new ApproachAltitude { Altitude = _leg.UpperLimitAltitude.Value, AltitudeReference = CODE_DIST_VER.MSL, AltitudeUOM = _leg.UpperLimitAltitudeUOM };
                                ApproachDistance hpDist = new ApproachDistance { DistanceUOM = UOM_DIST_HORZ.M, ValueHAT = null, ValueHATUOM = UOM_DIST_VERT.SM, EndingMeasurementPoint = CodeProcedureDistance.THLD };

                                switch (_legPnt.PointRole.Value)
                                {
                                    case ProcedureFixRoleType.IAF:
                                    case ProcedureFixRoleType.IF:
                                    case ProcedureFixRoleType.IF_IAF:
                                    case ProcedureFixRoleType.VDP:
                                    case ProcedureFixRoleType.ENRT:
                                    case ProcedureFixRoleType.OTHER_WPT:
                                    case ProcedureFixRoleType.ENRT_HLDNG:
                                    case ProcedureFixRoleType.MAHF:
                                    case ProcedureFixRoleType.FPAP:
                                    case ProcedureFixRoleType.FTP:
                                    case ProcedureFixRoleType.FROP:
                                    case ProcedureFixRoleType.TP:
                                    case ProcedureFixRoleType.OTHER:
                                        break;
                                    case ProcedureFixRoleType.FAF:
                                        hpDist.StartingMeasurementPoint = CodeProcedureDistance.FAF;
                                        hpAlt.MeasurementPoint = CodeProcedureDistance.FAF;
                                        break;
                                    case ProcedureFixRoleType.SDF:
                                        if (FinalLegsCount > 1)
                                        {
                                            hpDist.StartingMeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                                            hpAlt.MeasurementPoint = CodeProcedureDistance.OTHER_SDF;
                                        }
                                        else
                                        {
                                            hpDist = null;
                                            hpAlt = null;
                                        }
                                        break;
                                    case ProcedureFixRoleType.MAPT:
                                        hpDist.StartingMeasurementPoint = CodeProcedureDistance.MAP;
                                        hpAlt.MeasurementPoint = CodeProcedureDistance.MAP;
                                        hpAlt.Altitude = OCA_Value_InFT != -100 ? OCA_Value_InFT : 100;
                                        hpAlt.AltitudeUOM = UOM_DIST_VERT.FT;


                                        break;
                                    default:
                                        break;
                                }

                                if (hpDist != null)
                                    hpDist.Distance = calcDistFromRwy(_legPnt, focusMap, pSpatialReference, selRwyDir, Bearing);

                                if (hpDist != null) fProfile.ApproachDistancetable.Add(hpDist);
                                if (hpAlt != null) fProfile.ApproachAltitudeTable.Add(hpAlt);

                                arrayProcedureFixRoleTypelist.Remove(_legPnt.PointRole.Value);

                            }
                        }


                        if (_tran.RouteType == ProcedurePhaseType.FINAL)
                        {
                            try
                            {

                                if (_leg.AssessmentArea != null && _leg.AssessmentArea.Count > 0 && _leg.AssessmentArea[0].AssessedAltitude.HasValue)
                                {
                                    ApproachMinima appMin = new ApproachMinima { Minima = _leg.AssessmentArea[0].AssessedAltitude.Value, MinimaUom = _leg.AssessmentArea[0].AssessedAltitudeUOM };
                                    appMin.ProfileSegmnetDesignator = _legPnt.PointRole.ToString() + ":" + _legPnt.PointRole.ToString();

                                    fProfile.ApproachMinimaTable.Add(appMin);
                                }

                            }
                            catch { continue; }

                        }
                    }
                }


                if (fProfile.ApproachAltitudeTable.Count > 0)
                {
                    ApproachAltitude hpAlt = new ApproachAltitude { Altitude = 15, AltitudeReference = CODE_DIST_VER.MSL, AltitudeUOM = UOM_DIST_VERT.M, MeasurementPoint = CodeProcedureDistance.THLD };
                    fProfile.ApproachAltitudeTable.Insert(fProfile.ApproachAltitudeTable.Count - 1, hpAlt);
                }

                return fProfile;

            }
            catch (Exception)
            {
                return null;
            }
        }


        public static bool ProfiletableAnalizator(InstrumentApproachProcedure instrumentApproachProcedure)
        {
            bool res = false;
            try
            {
                if (instrumentApproachProcedure.Profile == null) return true;
                if (instrumentApproachProcedure.Profile.ApproachDistancetable == null) return true;
                if (instrumentApproachProcedure.Profile.ApproachAltitudeTable == null) return true;

                foreach (ApproachDistance dist in instrumentApproachProcedure.Profile.ApproachDistancetable)
                {
                    if (dist.StartingMeasurementPoint != CodeProcedureDistance.THLD || dist.EndingMeasurementPoint != CodeProcedureDistance.THLD)
                    {
                        res = true;
                        break;
                    }
                }
            }
            catch
            {
                return true;
            }



            return res;
        }

        public static void CreateStoreDistanceIndication(DistanceIndication distanceIndication, IFeatureClass AnnoFacilitymakeUpGeo_featClass, IFeatureClass Anno_NavaidGeo_featClass,
                                                  IMap FocusMap, ISpatialReference pSpatialReference, double ARP_MagneticVariation, SegmentPoint LegstPoint, string resultVERT_Uom, string resultDIST_Uom,
                                                  ref List<string> distanceIndicationDictionary, ref List<string> NavaidListID, bool GetAngleFromLeg = false, IGeometry DistAnGeometry = null)
        {
            string Fix = distanceIndication.FixID;
            string Nav = distanceIndication.SignificantPointID;
            if (Fix != null && Nav != null && distanceIndicationDictionary.IndexOf(Fix + Nav) < 0)
            {
                distanceIndicationDictionary.Add(Fix + Nav);
                PDMObject navSystem = null;

                IGeometry gm = Get_Angle_Distance_Line(Nav, Fix, ref navSystem);
                if (DistAnGeometry != null) gm = DistAnGeometry;

                if (gm != null && AnnoFacilitymakeUpGeo_featClass != null && navSystem != null)
                {
                    ChartElement_SimpleText chrtEl_Dist = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "DistanceIndication");

                    IPolyline prjGeom = (IPolyline)EsriUtils.ToProject(gm, FocusMap, pSpatialReference);
                    ILine ln = new LineClass();
                    ln.FromPoint = prjGeom.FromPoint;
                    ln.ToPoint = prjGeom.ToPoint;

                    double angle = ln.Angle * 180 / Math.PI + 90;
                    bool flag = false;
                    chrtEl_Dist.Slope = ChartsHelperClass.CheckedAngle(angle, ref flag);

                     IElement el;
                    if (GetAngleFromLeg)
                        el = ChartsHelperClass.CreateDistanceIdicationAnno(((IPolyline)gm).ToPoint, distanceIndication, ((PDM.NavaidSystem)navSystem).Designator, chrtEl_Dist, resultDIST_Uom);
                    else
                        el = ChartsHelperClass.CreateDistanceIdicationAnno(((IPolyline)gm).ToPoint, LegstPoint, ((PDM.NavaidSystem)navSystem).Designator, chrtEl_Dist);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Dist.Name, chrtEl_Dist.Id.ToString(), el, ref chrtEl_Dist, chrtEl_Dist.Id, FocusMap.MapScale);

                    gm = GetDisatnceLine(Fix, ln.Angle, FocusMap, pSpatialReference);
                    if (DistAnGeometry != null) gm = DistAnGeometry;
                    ChartsHelperClass.SaveFacilityMakeUp_Geo(AnnoFacilitymakeUpGeo_featClass, chrtEl_Dist.Id.ToString(), gm);
                }

                if (navSystem != null && (NavaidListID.IndexOf(navSystem.ID) < 0))
                {
                    NavaidListID.Add(navSystem.ID);

                    ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                    IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)navSystem, chrtEl_Navaid, resultVERT_Uom);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navSystem.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                    ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)navSystem);

                }


            }
        }


        public static void CreateStoreAngleIndication(AngleIndication angleIndication, IFeatureClass AnnoFacilitymakeUpGeo_featClass, IFeatureClass Anno_NavaidGeo_featClass,
                                                        IMap FocusMap, ISpatialReference pSpatialReference, double ARP_MagneticVariation, string masterPDM_ID, SegmentPoint LegstPoint,
                                                        string DistUom, string VerUom, ref List<string> angleIndicationDictionary, ref List<string> NavaidListID, bool GetAngleFromLeg = false)
        {
            string Fix = angleIndication.FixID != null ? angleIndication.FixID : LegstPoint.PointChoiceID;
            string Nav = angleIndication.SignificantPointID;

            

            if (Fix != null && Nav != null)
            {
                PDMObject navSystem = null;
                IGeometry gm = Get_Angle_Distance_Line(Nav ,Fix, ref navSystem);
                if (gm != null && AnnoFacilitymakeUpGeo_featClass != null && navSystem != null)
                {
                    ChartElement_Radial chrtEl_Radial = (ChartElement_Radial)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "AngleIndication");

                    IPolyline prjGeom = (IPolyline)EsriUtils.ToProject(gm, FocusMap, pSpatialReference);
                    ILine ln = new LineClass();

                    if (prjGeom.IsEmpty) return; ///////////////////////// ПРОВЕРИТЬ ПОЧЕМУ
                    ln.FromPoint = prjGeom.FromPoint;
                    ln.ToPoint = prjGeom.ToPoint;

                    double angl = ln.Angle * 180 / Math.PI;
                    bool flag = false;
                    angl = ChartsHelperClass.CheckedAngle(angl, ref flag);
                    if (flag) chrtEl_Radial.ArrowPosition = arrowPosition.Start;


                    if (((PDM.NavaidSystem)navSystem).CodeNavaidSystemType.ToString().StartsWith("NDB"))
                    {
                        if (chrtEl_Radial.ArrowPosition == arrowPosition.End) chrtEl_Radial.ArrowPosition = arrowPosition.Start;
                        else chrtEl_Radial.ArrowPosition = arrowPosition.End;

                    }

                    chrtEl_Radial.Slope = angl;

                    IElement el;
                    if (GetAngleFromLeg)
                        el = ChartsHelperClass.CreateAngleIdicationAnno(((IPolyline)gm).ToPoint, angleIndication, ((PDM.NavaidSystem)navSystem).Designator, ARP_MagneticVariation, chrtEl_Radial, ((PDM.NavaidSystem)navSystem).CodeNavaidSystemType);
                    else
                        el = ChartsHelperClass.CreateAngleIdicationAnno(((IPolyline)gm).ToPoint, LegstPoint, ((PDM.NavaidSystem)navSystem).Designator, ARP_MagneticVariation, chrtEl_Radial, ((PDM.NavaidSystem)navSystem).CodeNavaidSystemType);


                    if ( angleIndicationDictionary.IndexOf(((ITextElement)el).Text + ln.Length.ToString()) < 0)
                    {
                        angleIndicationDictionary.Add(((ITextElement)el).Text + ln.Length.ToString());
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Radial.Name, masterPDM_ID, el, ref chrtEl_Radial, chrtEl_Radial.Id, FocusMap.MapScale);
                        ChartsHelperClass.SaveFacilityMakeUp_Geo(AnnoFacilitymakeUpGeo_featClass, masterPDM_ID, gm);
                    }
                }

                if (navSystem != null && (NavaidListID.IndexOf(navSystem.ID) < 0))
                {
                    NavaidListID.Add(navSystem.ID);

                    ChartElement_SigmaCollout_Navaid chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                    IElement el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)navSystem, chrtEl_Navaid, VerUom);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navSystem.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                    ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)navSystem);

                    if (((PDM.NavaidSystem)navSystem).CodeNavaidSystemType == NavaidSystemType.ILS_DME)
                    {
                        chrtEl_Navaid = (ChartElement_SigmaCollout_Navaid)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Navaid");
                        el = ChartsHelperClass.CreateSegmentPointAnno((PDM.NavaidSystem)navSystem, chrtEl_Navaid, VerUom,true);
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Navaid.Name, navSystem.ID, el, ref chrtEl_Navaid, chrtEl_Navaid.Id, FocusMap.MapScale);
                        ((PDM.NavaidSystem)navSystem).CodeNavaidSystemType = NavaidSystemType.DME;
                        ChartsHelperClass.SaveNavaidSegmentPoint_ChartSegmentPointGeo(Anno_NavaidGeo_featClass, (PDM.NavaidSystem)navSystem);
                    }

                }


            }
        }

 
        public static void CreateHeightAnnotation(List<PDMObject> SelecteDProc, IMap FocusMap, string vertUom, string distUom)
        {
            Dictionary<string, ProcedureLeg> segPntList = new Dictionary<string, ProcedureLeg>();
            Dictionary<string, ProcedureLeg> allLegList = new Dictionary<string, ProcedureLeg>();

            foreach (var item in SelecteDProc)
            {
                Procedure proc = (Procedure)item;

                foreach (ProcedureTransitions trans in proc.Transitions)
                {
                    foreach (ProcedureLeg leg in trans.Legs)
                    {
                        allLegList.Add(leg.ID, leg);
                        if (leg.Notes == null) leg.Notes = new List<string>();
                        leg.Notes.Clear();
                        leg.Notes.Add(proc.ProcedureIdentifier + ":" + leg.ID);


                        if (leg.EndPoint == null &&  leg.StartPoint != null && (leg.LegTypeARINC == CodeSegmentPath.IF || leg.LegTypeARINC == CodeSegmentPath.CA))
                            leg.EndPoint = leg.StartPoint;
                        if (leg.EndPoint == null ) continue;

                       
                        if (segPntList.ContainsKey(leg.EndPoint.ID))
                        {
                            segPntList[leg.EndPoint.ID].Notes.Add(proc.ProcedureIdentifier + ":" + leg.ID);
                        }
                        else
                        {
                            segPntList.Add(leg.EndPoint.ID, leg);
                        }


                    }
                }

            }


            var lst_LOWER = (from keyValuePair in allLegList
                             where keyValuePair.Value.AltitudeInterpretation == AltitudeUseType.ABOVE_LOWER || keyValuePair.Value.AltitudeInterpretation == AltitudeUseType.AT_LOWER ||
                        keyValuePair.Value.AltitudeInterpretation == AltitudeUseType.RECOMMENDED || keyValuePair.Value.AltitudeInterpretation == AltitudeUseType.EXPECT_LOWER
                             select keyValuePair.Value).ToList();

            var lst_BELOW_UPPER = (from keyValuePair in allLegList
                                   where keyValuePair.Value.AltitudeInterpretation == AltitudeUseType.BELOW_UPPER
                                   select keyValuePair.Value).ToList();

            var lst_BETWEEN = (from keyValuePair in allLegList
                               where keyValuePair.Value.AltitudeInterpretation == AltitudeUseType.BETWEEN
                               select keyValuePair.Value).ToList();


            foreach (var pair in segPntList)
            {

                Dictionary<double, ProcedureLeg> lower_Legs = new Dictionary<double, ProcedureLeg>();
                Dictionary<double, ProcedureLeg> upper_Legs = new Dictionary<double, ProcedureLeg>();
                Dictionary<double, ProcedureLeg> upper_lower_Legs = new Dictionary<double, ProcedureLeg>();

                ProcedureLeg Leg = pair.Value;

                foreach (var item in pair.Value.Notes)
                {

                    #region


                    //if (pair.Value.Notes.Count > 1 || SelecteDProc.Count ==1)
                    {
                        string[] info = item.Split(':');
                        string PrcName = info[0];
                        Leg = allLegList[info[1]];

                        Leg.ProcedureIdentifier = PrcName;

                        if (lst_LOWER.IndexOf(Leg) >= 0)
                        {
                            if (Leg.LowerLimitAltitude.HasValue && !lower_Legs.ContainsKey(Leg.ConvertValueToMeter(Leg.LowerLimitAltitude.Value, Leg.LowerLimitAltitudeUOM.ToString())))
                                lower_Legs.Add(Leg.ConvertValueToMeter(Leg.LowerLimitAltitude.Value, Leg.LowerLimitAltitudeUOM.ToString()), Leg);
                            else
                                lower_Legs[Leg.ConvertValueToMeter(Leg.LowerLimitAltitude.Value, Leg.LowerLimitAltitudeUOM.ToString())].ProcedureIdentifier =
                                    lower_Legs[Leg.ConvertValueToMeter(Leg.LowerLimitAltitude.Value, Leg.LowerLimitAltitudeUOM.ToString())].ProcedureIdentifier + "/" + PrcName;
                        }

                        if (lst_BELOW_UPPER.IndexOf(Leg) >= 0)
                        {
                            if (!Leg.LowerLimitAltitude.HasValue || !Leg.UpperLimitAltitude.HasValue) continue;
                            double lim = Leg.ConvertValueToMeter(Leg.UpperLimitAltitude.Value, Leg.UpperLimitAltitudeUOM.ToString()) + Leg.ConvertValueToMeter(Leg.LowerLimitAltitude.Value, Leg.LowerLimitAltitudeUOM.ToString());
                            if (!upper_Legs.ContainsKey(lim))
                                upper_Legs.Add(lim, Leg);
                            else
                                upper_Legs[lim].ProcedureIdentifier = upper_Legs[lim].ProcedureIdentifier + "/" + PrcName;
                        }

                        if (lst_BETWEEN.IndexOf(Leg) >= 0)
                        {
                           

                            if (Leg.UpperLimitAltitude.HasValue && Leg.LowerLimitAltitude.HasValue)
                            {
                                double l = Leg.ConvertValueToMeter(Leg.UpperLimitAltitude.Value, Leg.UpperLimitAltitudeUOM.ToString()) +
                               Leg.ConvertValueToMeter(Leg.LowerLimitAltitude.Value, Leg.LowerLimitAltitudeUOM.ToString());
                                if ( !upper_lower_Legs.ContainsKey(l))
                                    upper_lower_Legs.Add(l, Leg);
                                else
                                    upper_lower_Legs[l].ProcedureIdentifier =
                                        upper_lower_Legs[l].ProcedureIdentifier + "/" + PrcName;
                            }
                                
                        }
                    }


                    #endregion

                }

                if (lower_Legs != null && lower_Legs.Count > 0) CreateAnnoHeightFromList(lower_Legs, FocusMap, vertUom, distUom);
                if (upper_Legs != null && upper_Legs.Count > 0) CreateAnnoHeightFromList(upper_Legs, FocusMap, vertUom, distUom);
                if (upper_lower_Legs != null && upper_lower_Legs.Count > 0) CreateAnnoHeightFromList(upper_lower_Legs, FocusMap, vertUom, distUom);

            }


        }

        private static void CreateAnnoHeightFromList(Dictionary<double, ProcedureLeg> Legs_List, IMap _Map, string vertUom, string distUom)
        {
            foreach (var pair1 in Legs_List)
            {
                ProcedureLeg Leg1 = pair1.Value;

                ChartElement_TextArrow chrtEl_legHeightArrow = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ProcedureLegHeight");

                var endPnt = Legs_List.Select(x => x.Value.EndPoint != null && x.Value.EndPoint.ID.StartsWith(Leg1.EndPoint.ID)).Count();

                string prcname = endPnt == 1 ? null : Leg1.ProcedureIdentifier;

                //IElement arrowHeighy_el = ChartsHelperClass.CreateSegmentPointLegHeightAnno(Leg1, chrtEl_legHeightArrow,vertUom, distUom, prcname);
                IElement arrowHeighy_el = ChartsHelperClass.CreateSegmentPointLegHeightAnno(Leg1, chrtEl_legHeightArrow, vertUom, distUom, _Map, _Map.SpatialReference, prcname);
                if (Leg1.EndPoint != null)
                {
                    //ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legHeightArrow.Name, Leg1.EndPoint.ID, arrowHeighy_el, ref chrtEl_legHeightArrow, chrtEl_legHeightArrow.Id, _Map.MapScale);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legHeightArrow.Name, Leg1.ID, arrowHeighy_el, ref chrtEl_legHeightArrow, chrtEl_legHeightArrow.Id, _Map.MapScale);
                }
                else if (Leg1.StartPoint != null)
                {
                    //ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legHeightArrow.Name, Leg1.StartPoint.ID, arrowHeighy_el, ref chrtEl_legHeightArrow, chrtEl_legHeightArrow.Id, _Map.MapScale);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_legHeightArrow.Name, Leg1.ID, arrowHeighy_el, ref chrtEl_legHeightArrow, chrtEl_legHeightArrow.Id, _Map.MapScale);
                }


            }
        }

        private static bool CompareLegs(ProcedureLeg leg, ProcedureLeg procedureLegFromList)
        {
            bool res = true;
            res = res && leg.AltitudeInterpretation == procedureLegFromList.AltitudeInterpretation;

            switch (leg.AltitudeInterpretation)
            {
                case AltitudeUseType.ABOVE_LOWER:
                case AltitudeUseType.AT_LOWER:
                case AltitudeUseType.RECOMMENDED:
                case AltitudeUseType.EXPECT_LOWER:
                    res = res && leg.LowerLimitAltitude.HasValue && leg.LowerLimitAltitude.Value == procedureLegFromList.LowerLimitAltitude && leg.LowerLimitAltitudeUOM == procedureLegFromList.LowerLimitAltitudeUOM;
                    break;
                case AltitudeUseType.BELOW_UPPER:
                    res = res && leg.UpperLimitAltitude.HasValue && leg.UpperLimitAltitude.Value == procedureLegFromList.UpperLimitAltitude && leg.UpperLimitAltitudeUOM == procedureLegFromList.UpperLimitAltitudeUOM;
                    break;
                case AltitudeUseType.BETWEEN:
                    res = res && leg.LowerLimitAltitude.HasValue && leg.LowerLimitAltitude.Value == procedureLegFromList.LowerLimitAltitude && leg.LowerLimitAltitudeUOM == procedureLegFromList.LowerLimitAltitudeUOM;
                    res = res && leg.UpperLimitAltitude.HasValue && leg.UpperLimitAltitude.Value == procedureLegFromList.UpperLimitAltitude && leg.UpperLimitAltitudeUOM == procedureLegFromList.UpperLimitAltitudeUOM;
                    break;
                case AltitudeUseType.AS_ASSIGNED:
                case AltitudeUseType.OTHER:
                    res = false;
                    break;
                default:
                    break;
            }


            return res;
        }

        private static IGeometry GetDisatnceLine(string Fix, double angle, IMap FocusMap, ISpatialReference pSpatialReference)
        {
            IGeometry res = null;
            IPoint DpnPnt = new PointClass();

            var fix = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(Fix) == 0) select element).FirstOrDefault();
            if (fix != null)
            {
                if (fix.Geo == null) fix.RebuildGeo();
                DpnPnt.PutCoords(((IPoint)fix.Geo).X, ((IPoint)fix.Geo).Y);

                TapFunctions util = new TapFunctions();

                angle = util.RadToDeg(angle);

                IPoint start = (IPoint)EsriUtils.ToProject(DpnPnt, FocusMap, pSpatialReference);
                var tmpPt1 = util.PointAlongPlane(start, angle + 90, 4000);
                var tmpPt2 = util.PointAlongPlane(start, angle - 90, 8000);

                IPolyline ln = new PolylineClass();

                ln.FromPoint = tmpPt1;
                ln.ToPoint = tmpPt2;

                res = EsriUtils.ToGeo(ln, FocusMap, pSpatialReference);

                util = null;
            }


            return res;
        }

        public static IGeometry Get_Angle_Distance_Line(string SignificantPointID_start , string FixID_end, ref PDMObject NavComponent)
        {

            IGeometry res = null;
            IPoint NavPnt = new PointClass();
            IPoint DpnPnt = new PointClass();

            var fix = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(FixID_end) == 0) select element).FirstOrDefault();
            if (fix == null)
                fix = DataCash.GetNavaidByID(FixID_end);
            if (fix != null)
            {
                if (fix.Geo == null) fix.RebuildGeo();
                if (fix.PDM_Type == PDM_ENUM.NavaidSystem)
                {
                    DpnPnt.PutCoords(((IPoint)((NavaidSystem)fix).Components[0].Geo).X, ((IPoint)((NavaidSystem)fix).Components[0].Geo).Y);
                }
                else 
                    DpnPnt.PutCoords(((IPoint)fix.Geo).X, ((IPoint)fix.Geo).Y);
            }



                var nav = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) 
                           && ((((NavaidSystem)element).ID.CompareTo(SignificantPointID_start) == 0)  || (((NavaidSystem)element).ID_Feature.CompareTo(SignificantPointID_start) == 0)) select element).FirstOrDefault();
                if (nav != null)
                {

                    if (((NavaidSystem)nav).Components != null && ((NavaidSystem)nav).Components.Count > 0)
                    {
                        if (((NavaidSystem)nav).Components[0].Geo == null) ((NavaidSystem)nav).Components[0].RebuildGeo();
                        NavPnt.PutCoords(((IPoint)((NavaidSystem)nav).Components[0].Geo).X, ((IPoint)((NavaidSystem)nav).Components[0].Geo).Y);
                        NavComponent = nav;
                    }
                }
                else
                {
                    bool flag = false;
                    var arpList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) select element).ToList();
                    foreach (AirportHeliport arp in arpList)
                    {
                        if (flag) break;
                        foreach (Runway rwy in arp.RunwayList)
                        {
                            if (rwy.RunwayDirectionList == null) continue;
                            foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                            {
                                if (rdn.Related_NavaidSystem == null) continue;
                                var ILS_NAV = (from element in rdn.Related_NavaidSystem where (element != null) && (element is NavaidSystem) && (((NavaidSystem)element).ID!=null) && 
                                               (((NavaidSystem)element).ID.CompareTo(SignificantPointID_start) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(SignificantPointID_start) == 0) select element).FirstOrDefault();
                                if (ILS_NAV != null)
                                {
                                    if (((NavaidSystem)ILS_NAV).Components != null && ((NavaidSystem)ILS_NAV).Components.Count > 0)
                                    {

                                        if (((NavaidSystem)ILS_NAV).CodeNavaidSystemType == NavaidSystemType.ILS_DME)
                                        {
                                            foreach (NavaidComponent cmpnt in ((NavaidSystem)ILS_NAV).Components)
                                            {
                                                if (cmpnt.PDM_Type == PDM_ENUM.Localizer)
                                                {
                                                    NavComponent = ILS_NAV;

                                                    if (cmpnt.Geo == null) cmpnt.RebuildGeo();
                                                    NavPnt.PutCoords(((IPoint)cmpnt.Geo).X, ((IPoint)cmpnt.Geo).Y);

                                                    break;
                                                }
                                            }

                                           
                                        }

                                        
                                    }
                                    flag = true;
                                    break;
                                }

                            }
                        }
                    }
                }



                if (NavPnt != null && DpnPnt != null && !NavPnt.IsEmpty && !DpnPnt.IsEmpty)
                {
                    IPolyline ln = new PolylineClass();

                    ln.FromPoint = NavPnt;
                    ln.ToPoint = DpnPnt;

                    //ln.FromPoint = DpnPnt;
                    //ln.ToPoint = NavPnt;


                    res = ln;
                }
            

            return res;
        

        }

          public static void CreateAirspace_ChartElements(List<PDMObject> arspc_featureList, IHookHelper SigmaHookHelper, IFeatureClass AnnoAirspaceGeo_featClass, 
              double MapScale,string vertUom, string distUom,int airspaceBuferWidth, bool CreateAirspaceBufferSign,int PrototypeIndex = 0, 
              int UpperSeparation_FL = 999, int LowerSeparation_FL = 0, IGeometry _extent = null)
        {

            AirspaceBuffer buf = new AirspaceBuffer(SigmaHookHelper);
            var bufferList = new List<Bagel>();

            AirspaceType[] arspcTypes = {AirspaceType.TMA, AirspaceType.TMA_P, AirspaceType.ATZ, AirspaceType.ATZ_P,AirspaceType.FIR, AirspaceType.FIR_P, AirspaceType.CTR, AirspaceType.CTR_P, AirspaceType.D,
                                            AirspaceType.P,AirspaceType.R, AirspaceType.UIR, AirspaceType.UIR_P, AirspaceType.CTA, AirspaceType.CTA_P };

            int gg = 0;
            string s = "";

            DateTime start = DateTime.Now;
            foreach (PDM.Airspace arsps in arspc_featureList)
            {
                try
                {
                    //if (arsps.ID.StartsWith("32364732-00a6-461c-8f72-2b0790b45357"))
                    //    System.Diagnostics.Debug.WriteLine("");
                    //else
                    //    continue;

                    foreach (PDM.AirspaceVolume arspsVol in arsps.AirspaceVolumeList)
                    {
                        s = arspsVol.GetObjectLabel();

                        if (arspsVol.CodeType == AirspaceType.OTHER)
                        {
                            if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                            SigmaDataCash.Report.Add(arsps.GetObjectLabel() + (char)9 + arsps.ID + (char)9 + s + (char)9 + "CodeType == AirspaceType.OTHER");
                            continue;
                        }
                        if (arspsVol.CodeType == AirspaceType.AMA)
                        {
                            if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                            SigmaDataCash.Report.Add(arsps.GetObjectLabel() + (char)9 + arsps.ID + (char)9 + s + (char)9 + "CodeType == AirspaceType.AMA");
                            continue;
                        }
                        if (arspsVol.Geo == null) arspsVol.RebuildGeo2();
                        if (arspsVol.Geo == null || arspsVol.Geo.IsEmpty)
                        {
                            if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                            SigmaDataCash.Report.Add(arsps.GetObjectLabel() + (char)9 + arsps.ID + (char)9 + s + (char)9 + "Geo.IsEmpty");
                            continue;
                        }
                        if (((IArea)arspsVol.Geo).Area == 0)
                        {
                            if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                            SigmaDataCash.Report.Add(arsps.GetObjectLabel() + (char)9 + arsps.ID + (char)9 + s + (char)9 + "Geo.Area = 0");
                            continue;
                        }


                        if (_extent != null)
                        {
                            ESRI.ArcGIS.Geometry.IRelationalOperator rel1 = arspsVol.Geo as ESRI.ArcGIS.Geometry.IRelationalOperator;
                            //if (rel1.Crosses(arspsVol.Geo)) System.Diagnostics.Debug.WriteLine("Crosses " + arsps.GetObjectLabel());
                            //if (rel1.Contains(arspsVol.Geo)) System.Diagnostics.Debug.WriteLine("Contains " + arsps.GetObjectLabel());
                            //if (rel1.Disjoint(arspsVol.Geo)) System.Diagnostics.Debug.WriteLine("Disjoint " + arsps.GetObjectLabel());
                            //if (rel1.Overlaps(arspsVol.Geo)) System.Diagnostics.Debug.WriteLine("Overlaps " + arsps.GetObjectLabel());
                            //if (rel1.Touches(arspsVol.Geo)) System.Diagnostics.Debug.WriteLine("Touches " + arsps.GetObjectLabel());
                            //if (rel1.Within(arspsVol.Geo)) System.Diagnostics.Debug.WriteLine("Within " + arsps.GetObjectLabel());

                            //double r1 = IntersectRectangles(arspsVol.VolumeEnvilope, _extent);
                            //bool r2 = ;

                            //if (r1 == 1 && !r2)
                            //    System.Diagnostics.Debug.WriteLine("");
                            
                            if (rel1.Disjoint(_extent)) continue;
                        }


                        if (arsps.ClassAirspace != null && arsps.ClassAirspace.Count > 0 && (UpperSeparation_FL < 999 || LowerSeparation_FL > 0) && arspsVol.ValDistVerLower.HasValue && arspsVol.ValDistVerUpper.HasValue)
                        {
                            

                            foreach (var cls in arsps.ClassAirspace)
                            {
                                if (cls.ClassAssociatedLevels == null || cls.ClassAssociatedLevels.Count == 0) continue;

                                
                                foreach (var lev in cls.ClassAssociatedLevels)
                                {
                                    double vallowerFT = arsps.ConvertValueToFeet(lev.lowerLimit, lev.lowerLimitUOM.ToString());
                                    double valupperFT = arsps.ConvertValueToFeet(lev.upperLimit, lev.upperLimitUOM.ToString());

                                    double VOL_vallowerFT = arspsVol.ConvertValueToFeet(LowerSeparation_FL, "FL");
                                    double VOL_valupperFT = arspsVol.ConvertValueToFeet(UpperSeparation_FL, "FL");

                                    if (VOL_vallowerFT >= vallowerFT && VOL_valupperFT <= valupperFT)
                                    {
                                        arspsVol.ValDistVerUpper = lev.upperLimit;
                                        arspsVol.UomValDistVerUpper = lev.upperLimitUOM;
                                        arspsVol.CodeDistVerUpper = lev.upperLimitReference;
                                        if (!arsps.Class.Contains(cls.Classification)) arsps.Class.Add(cls.Classification);
                                    }
                                    else
                                        if (arsps.Class.Contains(cls.Classification)) arsps.Class.Remove(cls.Classification);




                                }
                            }
                        }

                        

                        if (UpperSeparation_FL < 999 || LowerSeparation_FL > 0)
                        {

                            if (arspsVol.ValDistVerUpper.HasValue)
                            {
                                double FL_Val_UP = arspsVol.UomValDistVerUpper == UOM_DIST_VERT.FL? arspsVol.ValDistVerUpper.Value : arspsVol.ConvertValueToFeet(arspsVol.ValDistVerUpper.Value, arspsVol.UomValDistVerUpper.ToString()) / 100;
                                double FL_Val_LW = arspsVol.UomValDistVerLower == UOM_DIST_VERT.FL? arspsVol.ValDistVerLower.Value : arspsVol.ConvertValueToFeet(arspsVol.ValDistVerLower.Value, arspsVol.UomValDistVerLower.ToString()) / 100;

                                if (FL_Val_UP > UpperSeparation_FL || FL_Val_LW < LowerSeparation_FL) continue;
                            }

                        }

                        if ((!(arspsVol.CodeType == AirspaceType.TMA || arspsVol.CodeType == AirspaceType.TMA_P ||
                                arspsVol.CodeType == AirspaceType.CTR || arspsVol.CodeType == AirspaceType.CTR_P ||
                                arspsVol.CodeType == AirspaceType.SECTOR || arspsVol.CodeType == AirspaceType.SECTOR_C ||
                                arspsVol.CodeType == AirspaceType.ATZ || arspsVol.CodeType == AirspaceType.ATZ_P)))
                        {

                            #region not CalloutStyle

                            ChartElement_TextArrow chrtEl_Arspc_Simple = (ChartElement_TextArrow)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Airspace_Simple");
                            string GND_SFC = chrtEl_Arspc_Simple.TextContents[2][1].TextValue;


                            chrtEl_Arspc_Simple.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[0][0].DataSource);

                            #region Limits

                            if (arspsVol.UomValDistVerUpper != PDM.UOM_DIST_VERT.FL)
                            {
                                //uom
                                chrtEl_Arspc_Simple.TextContents[1][0].TextValue = ChartsHelperClass.MakeText_UOM(arspsVol, chrtEl_Arspc_Simple.TextContents[1][0].DataSource, vertUom);

                                string uom = ArenaStaticProc.GetObjectUomString(arspsVol, chrtEl_Arspc_Simple.TextContents[1][0].DataSource.Value);
                                if (uom.CompareTo("FT") != 0 && vertUom.CompareTo("FT") == 0 && IsNumeric(chrtEl_Arspc_Simple.TextContents[1][0].TextValue))
                                {
                                    int VL = Convert.ToInt32(chrtEl_Arspc_Simple.TextContents[1][0].TextValue);
                                    int p1 = VL - VL % 100;
                                    int p2 = VL % 100 > 0 ? 100 : 0;
                                    VL = p1 + p2;
                                    chrtEl_Arspc_Simple.TextContents[1][0].TextValue = VL.ToString() +" ";
                                }

                                chrtEl_Arspc_Simple.TextContents[1][1].TextValue = vertUom + " ";
                                chrtEl_Arspc_Simple.TextContents[1][0].Font.UnderLine = true;
                                chrtEl_Arspc_Simple.TextContents[1][1].Font.UnderLine = true;
                                chrtEl_Arspc_Simple.TextContents[1][2].Font.UnderLine = true;

                                chrtEl_Arspc_Simple.TextContents[1][2].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[1][2].DataSource);

                                if (chrtEl_Arspc_Simple.TextContents[1][2].Visible)
                                    chrtEl_Arspc_Simple.TextContents[1][2].Visible = chrtEl_Arspc_Simple.TextContents[1][2].TextValue.StartsWith("OTHER") ? false : true;
                                //chrtEl_Arspc_Simple.TextContents[1][1].Visible = false; //латвийское требование - выводить вертикальный UOM в аннотации airspaces


                            }
                            else
                            {
                                chrtEl_Arspc_Simple.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[1][1].DataSource);
                                chrtEl_Arspc_Simple.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[1][0].DataSource);

                                //if (chrtEl_Arspc_Simple.TextContents[1][1].TextValue.CompareTo("999") == 0)
                                //{
                                //    chrtEl_Arspc_Simple.TextContents[1][0].Visible = false;
                                //    chrtEl_Arspc_Simple.TextContents[1][1].TextValue = "UNL";
                                //}

                                chrtEl_Arspc_Simple.TextContents[1][0].Font.UnderLine = true;
                                chrtEl_Arspc_Simple.TextContents[1][1].Font.UnderLine = true;
                                chrtEl_Arspc_Simple.TextContents[1][1].Font.UnderLine = true;

                                chrtEl_Arspc_Simple.TextContents[1][2].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[1][2].DataSource);
                                chrtEl_Arspc_Simple.TextContents[1][1].Visible = true;
                                chrtEl_Arspc_Simple.TextContents[1][2].Visible = false;

                            }




                            if (arspsVol.UomValDistVerUpper == PDM.UOM_DIST_VERT.FL && arspsVol.ValDistVerUpper.HasValue && arspsVol.ValDistVerUpper.Value == 999)
                            {
                                chrtEl_Arspc_Simple.TextContents[1][0].TextValue = " ";
                                chrtEl_Arspc_Simple.TextContents[1][1].TextValue = "UNL";
                                chrtEl_Arspc_Simple.TextContents[1][1].Font.UnderLine = true;
                                chrtEl_Arspc_Simple.TextContents[1][2].Visible = false;

                            }


                            if (arspsVol.UomValDistVerLower != PDM.UOM_DIST_VERT.FL)
                            {
                                //uom

                                chrtEl_Arspc_Simple.TextContents[2][0].TextValue = ChartsHelperClass.MakeText_UOM(arspsVol, chrtEl_Arspc_Simple.TextContents[2][0].DataSource, vertUom);
                                chrtEl_Arspc_Simple.TextContents[2][1].TextValue = chrtEl_Arspc_Simple.TextContents[2][0].TextValue.StartsWith("NaN") ? "" : vertUom +" ";
                                chrtEl_Arspc_Simple.TextContents[2][2].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[2][2].DataSource);//
                                //chrtEl_Arspc_Simple.TextContents[2][1].Visible = false; //латвийское требование - выводить вертикальный UOM в аннотации airspaces
                                string uom = ArenaStaticProc.GetObjectUomString(arspsVol, chrtEl_Arspc_Simple.TextContents[2][0].DataSource.Value);
                                if (uom.CompareTo("FT") != 0 && vertUom.CompareTo("FT") == 0 && IsNumeric(chrtEl_Arspc_Simple.TextContents[2][0].TextValue))
                                {
                                    int VL = Convert.ToInt32(chrtEl_Arspc_Simple.TextContents[2][0].TextValue);
                                    int p1 = VL - VL % 100;
                                    
                                    chrtEl_Arspc_Simple.TextContents[2][0].TextValue = p1.ToString() +" ";
                                }


                            }
                            else
                            {
                                chrtEl_Arspc_Simple.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[2][1].DataSource);
                                chrtEl_Arspc_Simple.TextContents[2][1].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[2][0].DataSource);
                                chrtEl_Arspc_Simple.TextContents[2][2].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[2][2].DataSource); //
                                chrtEl_Arspc_Simple.TextContents[2][2].Visible = false; 
                            }



                            if (arspsVol.ValDistVerLower.HasValue && arspsVol.ValDistVerLower.Value == 0)
                            {
                                chrtEl_Arspc_Simple.TextContents[2][0].TextValue = "";
                                chrtEl_Arspc_Simple.TextContents[2][1].TextValue = arspsVol.CodeDistVerLower == CODE_DIST_VER.SFC || arspsVol.CodeDistVerLower == CODE_DIST_VER.MSL || arspsVol.CodeDistVerLower == CODE_DIST_VER.OTHER ? GND_SFC : arspsVol.CodeDistVerLower.ToString();
                                chrtEl_Arspc_Simple.TextContents[2][2].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[2][2].DataSource); //
                                //chrtEl_Arspc_Simple.TextContents[2][1].Visible = true;
                                chrtEl_Arspc_Simple.TextContents[2][2].Visible = (!chrtEl_Arspc_Simple.TextContents[2][1].TextValue.Trim().StartsWith("GND") && !chrtEl_Arspc_Simple.TextContents[2][1].TextValue.Trim().StartsWith("SFC") && !chrtEl_Arspc_Simple.TextContents[2][1].TextValue.Trim().Contains(chrtEl_Arspc_Simple.TextContents[2][1].DataSource.Condition));
                                //chrtEl_Arspc_Simple.TextContents[2][2].Visible = !chrtEl_Arspc_Simple.TextContents[2][1].TextValue.Trim().Contains(chrtEl_Arspc_Simple.TextContents[2][1].DataSource.Condition);

                                chrtEl_Arspc_Simple.TextContents[2][1].Visible = true;
                            }
                            else
                                System.Diagnostics.Debug.WriteLine("");

                            #endregion

                            #region Check Style

                            ChartElement_SigmaCollout_Airspace chrtEl_Arspc = null;
                            if (arsps.LocalType != null && arsps.LocalType.Length > 0 && (arsps.LocalType.Trim().CompareTo("TIA") == 0 || arsps.LocalType.Trim().CompareTo("TIZ") == 0))
                                chrtEl_Arspc = (ChartElement_SigmaCollout_Airspace)CheckAirspaceType(arspsVol.CodeType, chrtEl_Arspc_Simple, arsps.LocalType, arsps.Class, arsps);
                            else
                            chrtEl_Arspc_Simple = (ChartElement_TextArrow)CheckAirspaceType(arsps.LocalType, arspsVol.CodeType, chrtEl_Arspc_Simple);


                            if (arsps.CodeType == AirspaceType.PROTECT)
                            {
                                chrtEl_Arspc_Simple.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Simple.TextContents[0][0].DataSource);

                            }

                            #endregion

                            #region AMSL для белорусии

                            foreach (var Ln in chrtEl_Arspc_Simple.TextContents)
                            {
                                foreach (var wrd in Ln)
                                {
                                    if (wrd.DataSource.Value.StartsWith("CodeDistVerUpper") || wrd.DataSource.Value.StartsWith("CodeDistVerLower"))
                                    {
                                        if (!wrd.TextValue.StartsWith("MSL"))
                                            wrd.StartSymbol.Text = "";
                                    }
                                }
                            }

                            #endregion


                            #region saveElement

                            IPoint cntr = ((IArea)arspsVol.Geo).Centroid;


                            chrtEl_Arspc_Simple.Anchor = new AncorPoint(cntr.X, cntr.Y);
                            if (chrtEl_Arspc !=null) chrtEl_Arspc.Anchor = new AncorPoint(cntr.X, cntr.Y);

                            IElement el_arspc = (chrtEl_Arspc != null)? (IElement)chrtEl_Arspc.ConvertToIElement() : (IElement)chrtEl_Arspc_Simple.ConvertToIElement();


                            IPoint pnt = new PointClass();
                            pnt.PutCoords(cntr.X, cntr.Y);
                            el_arspc.Geometry = pnt;

                            ChartsHelperClass.SaveAirspace_ChartGeo(AnnoAirspaceGeo_featClass, arspsVol);

                            if (chrtEl_Arspc != null)
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Arspc.Name, arspsVol.ID, el_arspc, ref chrtEl_Arspc, chrtEl_Arspc.Id, (int)MapScale);
                            else
                                ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Arspc_Simple.Name, arspsVol.ID, el_arspc, ref chrtEl_Arspc_Simple, chrtEl_Arspc_Simple.Id, (int)MapScale);

                            Application.DoEvents();

                            #endregion

                            #endregion
                        }

                        else
                        {
                            #region CalloutStyle

                            #region Callout

                            ChartElement_SigmaCollout_Airspace chrtEl_Arspc_Callout = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SigmaCollout_Airspace");
                            string GND_SFC = chrtEl_Arspc_Callout.TextContents[2][1].TextValue;

                            chrtEl_Arspc_Callout.CaptionTextLine[0][0].TextValue = ChartsHelperClass.MakeText(arsps, chrtEl_Arspc_Callout.CaptionTextLine[0][0].DataSource).Trim();

                            chrtEl_Arspc_Callout.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(arsps, chrtEl_Arspc_Callout.TextContents[0][0].DataSource);

                            #region Limits

                            if (arspsVol.UomValDistVerUpper != PDM.UOM_DIST_VERT.FL)
                            {
                                //uom
                                chrtEl_Arspc_Callout.TextContents[1][0].TextValue = ChartsHelperClass.MakeText_UOM(arspsVol, chrtEl_Arspc_Callout.TextContents[1][0].DataSource, vertUom);
                                chrtEl_Arspc_Callout.TextContents[1][1].TextValue = vertUom +" ";
                                chrtEl_Arspc_Callout.TextContents[1][0].Font.UnderLine = true;
                                chrtEl_Arspc_Callout.TextContents[1][1].Font.UnderLine = true;
                                chrtEl_Arspc_Callout.TextContents[1][2].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Callout.TextContents[1][2].DataSource);
                                chrtEl_Arspc_Callout.TextContents[1][2].Font.UnderLine = true;

                            }
                            else
                            {
                                chrtEl_Arspc_Callout.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Callout.TextContents[1][1].DataSource);
                                chrtEl_Arspc_Callout.TextContents[1][1].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Callout.TextContents[1][0].DataSource);
                                chrtEl_Arspc_Callout.TextContents[1][0].Font.UnderLine = true;
                                chrtEl_Arspc_Callout.TextContents[1][1].Font.UnderLine = true;
                                chrtEl_Arspc_Callout.TextContents[1][2].TextValue = " ";
                            }
                           



                            if (arspsVol.UomValDistVerUpper == PDM.UOM_DIST_VERT.FL && arspsVol.ValDistVerUpper.HasValue && arspsVol.ValDistVerUpper.Value == 999)
                            {
                                chrtEl_Arspc_Callout.TextContents[1][0].TextValue = " ";
                                chrtEl_Arspc_Callout.TextContents[1][1].TextValue = "UNL";
                                chrtEl_Arspc_Callout.TextContents[1][1].Font.UnderLine = true;
                                chrtEl_Arspc_Callout.TextContents[1][2].TextValue = " ";

                            }


                            if (arspsVol.UomValDistVerLower != PDM.UOM_DIST_VERT.FL)
                            {
                                //uom
                                chrtEl_Arspc_Callout.TextContents[2][0].TextValue = ChartsHelperClass.MakeText_UOM(arspsVol, chrtEl_Arspc_Callout.TextContents[2][0].DataSource, vertUom);
                                chrtEl_Arspc_Callout.TextContents[2][1].TextValue = chrtEl_Arspc_Callout.TextContents[2][0].TextValue.StartsWith("NaN") ? "" : vertUom +" ";//ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Callout.TextContents[2][1].DataSource);
                                chrtEl_Arspc_Callout.TextContents[2][2].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Callout.TextContents[2][2].DataSource); 
                                

                            }
                            else
                            {
                                chrtEl_Arspc_Callout.TextContents[2][0].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Callout.TextContents[2][1].DataSource);
                                chrtEl_Arspc_Callout.TextContents[2][1].TextValue = ChartsHelperClass.MakeText(arspsVol, chrtEl_Arspc_Callout.TextContents[2][0].DataSource);
                                chrtEl_Arspc_Callout.TextContents[2][2].TextValue = "";
                            }



                            if (arspsVol.ValDistVerLower.HasValue && arspsVol.ValDistVerLower.Value == 0)
                            {
                                chrtEl_Arspc_Callout.TextContents[2][0].TextValue = "";
                                chrtEl_Arspc_Callout.TextContents[2][1].TextValue = "";
                                chrtEl_Arspc_Callout.TextContents[2][2].TextValue = arspsVol.CodeDistVerLower == CODE_DIST_VER.SFC ? GND_SFC : arspsVol.CodeDistVerLower.ToString(); ;
                            }

                            #endregion

                            if (arsps.CommunicationChanels != null && arsps.CommunicationChanels.Count > 0)
                            {
                                chrtEl_Arspc_Callout.TextContents[3][0].DataSource.Value = "CallSign";
                                chrtEl_Arspc_Callout.TextContents[3][0].TextValue = ChartsHelperClass.MakeText(arsps.CommunicationChanels[0], chrtEl_Arspc_Callout.TextContents[3][0].DataSource);

                                chrtEl_Arspc_Callout.TextContents[4][0].DataSource.Value = "FrequencyTransmission";
                                chrtEl_Arspc_Callout.TextContents[4][0].TextValue = ChartsHelperClass.MakeText(arsps.CommunicationChanels[0], chrtEl_Arspc_Callout.TextContents[4][0].DataSource, 3);

                                chrtEl_Arspc_Callout.BottomTextLine[0][0].DataSource.Value = "FrequencyReception";
                                chrtEl_Arspc_Callout.BottomTextLine[0][0].TextValue = ChartsHelperClass.MakeText(arsps.CommunicationChanels[0], chrtEl_Arspc_Callout.BottomTextLine[0][0].DataSource, 3);

                            }
                            else
                            {
                                chrtEl_Arspc_Callout.TextContents[3][0].TextValue = ChartsHelperClass.MakeText(arsps, chrtEl_Arspc_Callout.TextContents[3][0].DataSource);
                                chrtEl_Arspc_Callout.TextContents[4][0].TextValue = ChartsHelperClass.MakeText(arsps, chrtEl_Arspc_Callout.TextContents[4][0].DataSource, 3);
                                chrtEl_Arspc_Callout.BottomTextLine[0][0].TextValue = ChartsHelperClass.MakeText(arsps, chrtEl_Arspc_Callout.BottomTextLine[0][0].DataSource, 3);

                            }




                            if (chrtEl_Arspc_Callout.TextContents[4][0].TextValue.CompareTo(chrtEl_Arspc_Callout.BottomTextLine[0][0].TextValue) == 0)
                                chrtEl_Arspc_Callout.TextContents.Remove((chrtEl_Arspc_Callout.TextContents[4]));


                            if (arsps.Class != null && arsps.Class.Count > 0) chrtEl_Arspc_Callout.AirspaceSign.AirspaceSymbols = ChartsHelperClass.ToString(arsps.Class);


                            #endregion


                            #region Check Style
                            if (arsps.LocalType != null && arsps.LocalType.Length > 0)
                            {
                                var _chrtEl_Arspc = (ChartElement_SigmaCollout_Airspace)CheckAirspaceType(arspsVol.CodeType, chrtEl_Arspc_Callout, arsps.LocalType, arsps.Class, arsps);
                                if (_chrtEl_Arspc != null) chrtEl_Arspc_Callout = _chrtEl_Arspc;
                            }

                            #endregion

                            #region AMSL для белорусии

                            foreach (var Ln in chrtEl_Arspc_Callout.TextContents)
                            {
                                foreach (var wrd in Ln)
                                {
                                    if (wrd.DataSource.Value.StartsWith("CodeDistVerUpper") || wrd.DataSource.Value.StartsWith("CodeDistVerLower"))
                                    {
                                        if (!wrd.TextValue.StartsWith("MSL"))
                                            wrd.StartSymbol.Text = "";
                                    }
                                }
                            }

                            #endregion

                            #region saveElement


                            if (arspsVol.Geo == null) arspsVol.RebuildGeo2();
                            if (arspsVol.Geo == null || arspsVol.Geo.IsEmpty) continue;

                            IPoint cntr = ((IArea)arspsVol.Geo).Centroid;
                            chrtEl_Arspc_Callout.Anchor = new AncorPoint(cntr.X, cntr.Y);

                            IElement el_arspc = (IElement)chrtEl_Arspc_Callout.ConvertToIElement();


                            IPoint pnt = new PointClass();
                            pnt.PutCoords(cntr.X, cntr.Y);
                            el_arspc.Geometry = pnt;


                            ChartsHelperClass.SaveAirspace_ChartGeo(AnnoAirspaceGeo_featClass, arspsVol);


                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Arspc_Callout.Name, arspsVol.ID, el_arspc, ref chrtEl_Arspc_Callout, chrtEl_Arspc_Callout.Id, (int)MapScale);
                            Application.DoEvents();

                            #endregion


                            #region Airspace class

                            if (CreateAirspaceBufferSign && arspcTypes.Contains( arsps.CodeType))
                            {
                                ChartElement_MarkerSymbol chrtEl_Arspc_Class = (ChartElement_MarkerSymbol)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Airspace_Class");

                                if (arsps.Class != null && arsps.Class.Count > 0)
                                {

                                    chrtEl_Arspc_Class.TextContents[0][0].TextValue = ChartsHelperClass.ToString(arsps.Class);

                                    if (arspsVol.Geo == null) arspsVol.RebuildGeo2();
                                    if (arspsVol.Geo == null || arspsVol.Geo.IsEmpty) continue;

                                    cntr = ((IPointCollection)arspsVol.Geo).get_Point(0);

                                    el_arspc = (IElement)chrtEl_Arspc_Class.ConvertToIElement();

                                    pnt = new PointClass();
                                    pnt.PutCoords(cntr.X, cntr.Y);

                                    IGroupElement grp = (IGroupElement)el_arspc;
                                    for (int i = 0; i < grp.ElementCount; i++)
                                    {
                                        grp.get_Element(i).Geometry = pnt;

                                    }

                                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_Arspc_Class.Name, arspsVol.ID, el_arspc, ref chrtEl_Arspc_Class, chrtEl_Arspc_Class.Id, (int)MapScale);
                                    Application.DoEvents();
                                }
                            }

                            #endregion

                            #endregion
                        }

                        //bufer

                        //if (arspcTypes.Contains(arspsVol.CodeType) && airspaceBuferWidth > 0)
                        if (airspaceBuferWidth > 0)
                        {
                            Bagel bgl = buf.Buffer((IPolygon)arspsVol.Geo, airspaceBuferWidth);

                            bgl.BagelCodeId = arspsVol.CodeType.ToString();
                            bgl.BagelCodeClass = arspsVol.CodeClass;
                            bgl.BagelTxtName = arspsVol.TxtName;
                            bgl.MasterID = arspsVol.ID;
                            bgl.BagelLocalType = arspsVol.TxtLocalType;
                            bufferList.Add(bgl);
                        }
                    }

                    

                }
                catch(Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(arsps.GetObjectLabel() + (char)9 + arsps.ID + (char)9  + s + (char)9 + ex.Message + (char)9 + ex.StackTrace);
                    continue;
                }

                
            }

            ChartsHelperClass.SaveBuffer(bufferList, SigmaDataCash.AnnotationLinkedGeometryList["AirspaceB"]);


            //MessageBox.Show((DateTime.Now - start).Milliseconds.ToString());

        }

//        private static double IntersectRectangles(AirspaceVolumeEnvelope vol, IGeometry extent)
//        {

//            /*
//    x1, y1 - левая нижняя точка первого прямоугольника
//    x2, y2 - правая верхняя точка первого прямоугольника
//    x3, y3 - левая нижняя точка второго прямоугольника
//    x4, y4 - правая верхняя точка второго прямоугольника
//*/

//            AirspaceVolumeEnvelope extEnv = new AirspaceVolumeEnvelope { LowerLeft_X = 0 };


//            extEnv.LowerLeft_X = (float)((IEnvelope)extent).LowerLeft.X;
//            extEnv.LowerLeft_Y = (float)((IEnvelope)extent).LowerLeft.Y;

//            extEnv.LowerRight_X = (float)((IEnvelope)extent).LowerRight.X;
//            extEnv.LowerRight_Y = (float)((IEnvelope)extent).LowerRight.Y;

//            extEnv.TopLeft_X = (float)((IEnvelope)extent).UpperLeft.X;
//            extEnv.TopLeft_Y = (float)((IEnvelope)extent).UpperLeft.Y;

//            extEnv.TopRight_X = (float)((IEnvelope)extent).UpperRight.X;
//            extEnv.TopRight_Y = (float)((IEnvelope)extent).UpperRight.Y;

            

//            double left = Math.Max(vol.LowerLeft_X, extEnv.LowerLeft_X);
//            double top = Math.Min(vol.TopRight_Y, extEnv.TopRight_Y);
//            double right = Math.Min(vol.TopRight_X, extEnv.TopRight_Y);
//            double bottom = Math.Max(vol.LowerLeft_Y, extEnv.LowerLeft_Y);

//            double width = right - left;
//            double height = top - bottom;

//                if ((width < 0) || (height < 0))
//                    return 0;

//                return width * height;
            


//        }

        private static ChartElement_SigmaCollout_Airspace CheckAirspaceType(AirspaceType arspCodeType, ChartElement_SimpleText chrtEl_Arspc, string localType, List<string> _arspClases, PDMObject _obj)
        {
            ChartElement_SimpleText chrtEl_Arspc_Callout = (ChartElement_SimpleText)chrtEl_Arspc;
            ChartElement_SigmaCollout_Airspace _localStyle = null;

            if ((arspCodeType == AirspaceType.ATZ || arspCodeType == AirspaceType.ATZ_P || arspCodeType == AirspaceType.CTR || arspCodeType == AirspaceType.CTR_P || arspCodeType == AirspaceType.TMA
                || arspCodeType == AirspaceType.TMA_P || arspCodeType == AirspaceType.SECTOR || arspCodeType == AirspaceType.SECTOR_C) || localType.CompareTo("TIA") == 0 || localType.CompareTo("TIZ") == 0)
            {
                _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ATZ_ATZP_Airspace");

                switch (arspCodeType)
                {
                    case AirspaceType.TMA:
                    case AirspaceType.TMA_P:
                        _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "TMA_TMAP_Airspace");
                        break;

                    case AirspaceType.ATZ:
                    case AirspaceType.ATZ_P:
                        _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "ATZ_ATZP_Airspace");
                        break;

                    case AirspaceType.CTR:
                    case AirspaceType.CTR_P:
                        _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "CTR_CTRP_Airspace");
                        break;

                    case AirspaceType.SECTOR:
                    case AirspaceType.SECTOR_C:
                        _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "SECTOR_SECTORC_Airspace");
                        break;

                    default:
                        break;
                }
                
                if (localType.CompareTo("TIZ") == 0)
                {
                    _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "TIZ_Airspace");

                }
                else if (localType.CompareTo("TIA") == 0)
                {
                    _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "TIA_Airspace");

                }
                else if (localType.CompareTo("AOR") == 0)
                {
                    _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "AOR_Airspace");

                }
                else if (localType.CompareTo("FIS") == 0)
                {
                    _localStyle = (ChartElement_SigmaCollout_Airspace)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "FIS_Airspace");

                }


                if (_localStyle != null)
                {
                    int lnCntr = 0;
                    int LineCount = _localStyle.CaptionTextLine.Count;


                     if (_arspClases != null && _arspClases.Count > 0) _localStyle.AirspaceSign.AirspaceSymbols = ChartsHelperClass.ToString(_arspClases);


                    #region CaptionTextLine

                    //foreach (var line in _localStyle.CaptionTextLine)
                    //{
                    //    int wrdCntr = 0;
                    //    if (lnCntr > chrtEl_Arspc_Callout.CaptionTextLine.Count - 1) break;

                    //    foreach (var wrd in line)
                    //    {
                    //        if (wrd.DataSource.Value.Length > 0 && wrd.Visible)
                    //        {
                    //            wrd.DataSource = chrtEl_Arspc_Callout.CaptionTextLine[lnCntr][wrdCntr].DataSource;
                    //            wrd.TextValue = chrtEl_Arspc_Callout.CaptionTextLine[lnCntr][wrdCntr].TextValue;
                    //            if (wrd.TextValue.Length > 0)
                    //            {
                    //                wrd.Font = (AncorFont)chrtEl_Arspc_Callout.CaptionTextLine[lnCntr][wrdCntr].Font.Clone();
                    //            }
                    //        }
                    //        wrdCntr++;
                    //    }
                    //    lnCntr++;
                    //}

                    #endregion


                    #region InnerText

                    LineCount = _localStyle.TextContents.Count;
                    lnCntr = 0;
                    foreach (var line in _localStyle.TextContents)
                    {
                        int wrdCntr = 0;
                        if (lnCntr > chrtEl_Arspc_Callout.TextContents.Count - 1) break;

                        foreach (var wrd in line)
                        {
                            if (wrd.DataSource.Value.Length > 0 && wrd.Visible)
                            {
                                wrd.DataSource = chrtEl_Arspc_Callout.TextContents[lnCntr][wrdCntr].DataSource;
                                wrd.TextValue = chrtEl_Arspc_Callout.TextContents[lnCntr][wrdCntr].TextValue;
                                if (wrd.TextValue.Length > 0)
                                {
                                    AncorColor clr = (AncorColor)wrd.Font.FontColor.Clone();
                                    wrd.Font = (AncorFont)chrtEl_Arspc_Callout.TextContents[lnCntr][wrdCntr].Font.Clone();
                                    wrd.Font.FontColor = clr;
                                }
                                wrd.Visible = chrtEl_Arspc_Callout.TextContents[lnCntr][wrdCntr].Visible;
                            }
                            wrdCntr++;
                        }
                        lnCntr++;
                    }


                    #endregion


                    #region BottomTextLine

                    if (chrtEl_Arspc is ChartElement_SigmaCollout_Airspace)
                    {
                        LineCount = _localStyle.BottomTextLine.Count;
                        lnCntr = 0;

                        foreach (var line in _localStyle.BottomTextLine)
                        {
                            int wrdCntr = 0;
                            if (lnCntr > ((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine.Count - 1) break;

                            foreach (var wrd in line)
                            {
                                if (wrd.DataSource.Value.Length > 0 && wrd.Visible)
                                {


                                    if (((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine.Count <= lnCntr &&
                                        ((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr].Count <= wrdCntr &&
                                        wrd.DataSource.Value.CompareTo(((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr][wrdCntr].DataSource.Value) == 0)
                                    {
                                        wrd.TextValue = ((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr][wrdCntr].TextValue;
                                        wrd.DataSource = ((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr][wrdCntr].DataSource;
                                    }
                                    else
                                    {
                                        if (_obj != null)
                                        {
                                            if (((Airspace)_obj).CommunicationChanels != null && ((Airspace)_obj).CommunicationChanels.Count > 0)
                                            {
                                                wrd.TextValue = ChartsHelperClass.MakeText(((Airspace)_obj).CommunicationChanels[0], wrd.DataSource, Rounder: 3);

                                                if (wrd.TextValue.StartsWith("NaN")) { wrd.TextValue = ChartsHelperClass.MakeText(_obj, wrd.DataSource, Rounder: 3); }
                                            }
                                        }
                                    }


                                    if (wrd.TextValue.Length > 0)
                                    {
                                        AncorColor clr = (AncorColor)wrd.Font.FontColor.Clone();
                                        if (((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine.Count <= lnCntr &&
                                        ((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr].Count <= wrdCntr)
                                        {
                                            wrd.Font = (AncorFont)((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr][wrdCntr].Font.Clone();
                                            wrd.Font.FontColor = clr;
                                        }
                                    }

                                    if (((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine.Count <= lnCntr &&
                                        ((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr].Count <= wrdCntr)
                                        wrd.Visible = ((ChartElement_SigmaCollout_Airspace)chrtEl_Arspc_Callout).BottomTextLine[lnCntr][wrdCntr].Visible;
                                }
                                wrdCntr++;
                            }
                            lnCntr++;
                        }
                    }

                    #endregion

                    //chrtEl_Arspc_Callout = _localStyle;
                }

            }

            if (_localStyle != null) return _localStyle;
            else return null;
        }

        private static ChartElement_SimpleText CheckAirspaceType(string localType, AirspaceType arspCodeType, ChartElement_SimpleText chrtEl_Arspc)
        {
            ChartElement_SimpleText _chrtEl_Arspc = (ChartElement_SimpleText)chrtEl_Arspc;
            ChartElement_SimpleText _localStyle = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "TRA_TSA_Airspace");


            switch (arspCodeType)
            {
                case AirspaceType.TRA:
                case AirspaceType.TSA:
                    _localStyle = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "TRA_TSA_Airspace");
                    break;

                case AirspaceType.R:
                case AirspaceType.D:
                    if (localType.CompareTo("AMC") != 0)
                        _localStyle = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "R_D_P_Airspace");
                    else
                        _localStyle = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "R_D_P_AMC_Airspace");
                    break;

                case AirspaceType.P:
                        _localStyle = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "R_D_P_AMC_Airspace");
                    break;

                case AirspaceType.PROTECT:
                    _localStyle = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "PROTECT_Airspace");
                    break;

                default:
                    break;
            }
            



            if (_localStyle != null)
            {
                int lnCntr = 0;
                int LineCount = _localStyle.TextContents.Count;


                #region InnerText

                LineCount = _localStyle.TextContents.Count;
                lnCntr = 0;
                foreach (var line in _localStyle.TextContents)
                {
                    int wrdCntr = 0;
                    if (lnCntr > _chrtEl_Arspc.TextContents.Count - 1) break;
                    bool FLFlag = false;
                    foreach (var wrd in line)
                    {
                        if (_chrtEl_Arspc.TextContents[lnCntr][wrdCntr].TextValue.StartsWith("UNL")) wrd.Visible = true;
                        //if (_chrtEl_Arspc.TextContents[lnCntr][wrdCntr].TextValue.StartsWith("SFC")) wrd.Visible = true;
                        if (_chrtEl_Arspc.TextContents[lnCntr][wrdCntr].TextValue.StartsWith("GND")) wrd.Visible = true;

                        if ((wrd.DataSource.Value.Length > 0 && wrd.Visible) || FLFlag)
                        {

                            //wrd.DataSource = _chrtEl_Arspc.TextContents[lnCntr][wrdCntr].DataSource;
                            wrd.TextValue = _chrtEl_Arspc.TextContents[lnCntr][wrdCntr].TextValue;
                            if (wrd.TextValue.Length > 0)
                            {
                                AncorColor clr = (AncorColor)wrd.Font.FontColor.Clone();

                                wrd.Font = (AncorFont)_chrtEl_Arspc.TextContents[lnCntr][wrdCntr].Font.Clone();
                                wrd.Font.FontColor = clr;

                            }
                            wrd.Visible = _chrtEl_Arspc.TextContents[lnCntr][wrdCntr].Visible || FLFlag;
                            if (wrd.TextValue.StartsWith("UNL")) wrd.Visible = true;
                            //if (wrd.TextValue.StartsWith("SFC")) wrd.Visible = true;
                            if (wrd.TextValue.StartsWith("GND")) wrd.Visible = true;
                            if (wrd.TextValue.StartsWith("FL"))
                            {
                                wrd.Visible = true;
                                FLFlag = true;
                            }
                            else
                                FLFlag = false;
                        }
                        wrdCntr++;
                    }
                    lnCntr++;
                }




                #endregion

                _chrtEl_Arspc = _localStyle;
            }



            return _chrtEl_Arspc;
        }


        public static List<RunwayDirection> GetSelectedRunwayDirectionList(List<PDMObject> selectedProcedures)
        {
            List<RunwayDirection> selRwyDir = new List<RunwayDirection>();
            List<string> ids = new List<string>();
            foreach (Procedure Proc in selectedProcedures)
            {
                if (Proc.LandingArea != null)
                {
                    foreach (var rwy in Proc.LandingArea)
                    {
                        RunwayDirection rwyDir = (RunwayDirection)rwy;
                        if (rwyDir != null && ids.IndexOf(rwyDir.ID) < 0)
                        {
                            selRwyDir.Add(rwyDir);
                            ids.Add(rwyDir.ID);
                        }
                    }

                }

            }
            return selRwyDir;
        }

        public static void CreateOIS_ChartElements(List<PDMObject> selectedProcedures, List<PDMObject> obstacleList, RunwayDirection selRwyDir, IMap FocusMap, IFeatureClass Anno_ObstacleGeo_featClass, string vertUom)
        {
            try
            {

                if (selRwyDir != null)
                {
                    OISCreater _ois = new OISCreater();
                    string errorMessage;
                    AranSupport.Utilitys util = new AranSupport.Utilitys();

                    double k = ((Procedure)selectedProcedures[0]).Transitions[0].Legs[0].VerticalAngle.Value;
                    double tan = Math.Tan((Math.PI * k / 180));
                    k = Math.Round(tan, 3);

                    double h = util.ConvertValueToMeter(((Procedure)selectedProcedures[0]).Transitions[0].Legs[0].LowerLimitAltitude.Value.ToString(), ((Procedure)selectedProcedures[0]).Transitions[0].Legs[0].LowerLimitAltitudeUOM.ToString());
                    if (selRwyDir.CenterLinePoints != null)
                    {
                        RunwayCenterLinePoint endRwyCntPnt = selRwyDir.CenterLinePoints.FirstOrDefault(pdm => pdm.Role == PDM.CodeRunwayCenterLinePointRoleType.END) as PDM.RunwayCenterLinePoint;
                        if (endRwyCntPnt != null)
                        {
                            double h_endRwyCntPnt = util.ConvertValueToMeter(endRwyCntPnt.Elev.Value.ToString(), endRwyCntPnt.Elev_UOM.ToString());
                            h = h - h_endRwyCntPnt - 5;

                            double area1Dist = h / tan;

                            List<PDM.VerticalStructure> OIStList = _ois.Check(FocusMap, selRwyDir, obstacleList, h, area1Dist, k, out errorMessage);
                            if (errorMessage == "")
                            {
                                string uom = "";
                                UOM_DIST_VERT vertDistUom;

                                foreach (VerticalStructure vstr in OIStList)
                                {
                                    foreach (VerticalStructurePart item in vstr.Parts)
                                    {
                                        //item.Height = util.ConvertValueToMeter(item.Elev.Value.ToString(), item.Elev_UOM.ToString()) - util.ConvertValueToMeter(endRwyCntPnt.Elev.Value.ToString(), endRwyCntPnt.Elev_UOM.ToString());
                                        if (item.Elev.HasValue)
                                        {
                                            uom = item.Elev_UOM.ToString();
                                            item.Elev = ArenaStaticProc.UomTransformation(uom, vertUom, item.Elev.Value);

                                            Enum.TryParse<UOM_DIST_VERT>(item.Elev_UOM.ToString(), out vertDistUom);
                                            item.Elev_UOM = vertDistUom;
                                        }

                                        if (item.VerticalExtent.HasValue)
                                        {
                                            uom = item.VerticalExtent_UOM.ToString();
                                            item.VerticalExtent = ArenaStaticProc.UomTransformation(uom, vertUom, item.VerticalExtent.Value);

                                            Enum.TryParse<UOM_DIST_VERT>(item.VerticalExtent_UOM.ToString(), out vertDistUom);
                                            item.VerticalExtent_UOM = vertDistUom;
                                        }

                                        if (item.Height.HasValue)
                                        {
                                            uom = item.Height_UOM.ToString();
                                            item.Height = ArenaStaticProc.UomTransformation(uom, vertUom, item.Height.Value);

                                            Enum.TryParse<UOM_DIST_VERT>(item.Height_UOM.ToString(), out vertDistUom);
                                            item.Height_UOM = vertDistUom;
                                        }
                                    }
                                    ChartsHelperClass.SaveVerticalStructPoint_ChartGeo(Anno_ObstacleGeo_featClass, vstr);
                                }

                            }
                            else
                            {
                                //MessageBox.Show("OISCreater " + errorMessage);

                            }
                        }
                    }

                }
            }
            catch
            {
            }
        }

        public static void SaveVerticalStructureGeo(List<PDMObject> obstacleList, IFeatureClass Anno_ObstacleGeo_featClass, double VerticalLimit_M, string vertUom, string distUom, double FocusMapScale  = 10000)
        {
            try
            {
                if (obstacleList != null)
                {
                    AranSupport.Utilitys util = new AranSupport.Utilitys();
                    string uom = "";
                    UOM_DIST_VERT vertDistUom;
                    VerticalStructurePart vertStructPart = null;
                    foreach (VerticalStructure vstr in obstacleList)
                    {
                        if (vstr.Parts == null || vstr.Parts.Count == 0) continue;
                        var _partsList = (from element in vstr.Parts
                                          where (element != null)
                                                 && ( ((VerticalStructurePart)element).Elev.HasValue)
                                             orderby ((VerticalStructurePart)element).Elev
                                          select element).ToList();

                        //foreach (VerticalStructurePart item in _partsList)

                        if (_partsList != null) vertStructPart = _partsList[0];
                        {
                            if (vertStructPart.Geo == null) vertStructPart.RebuildGeo();
                            if (vertStructPart.Geo == null) continue;
                            if (vertStructPart.Geo.GeometryType == esriGeometryType.esriGeometryPolygon)
                            {
                                vertStructPart.Geo = ((IArea)vertStructPart.Geo).Centroid;
                                vstr.Group = true;
                            }

                            

                            if (vertStructPart.Elev.HasValue)
                            {
                                uom = vertStructPart.Elev_UOM.ToString();
                                vertStructPart.Elev = ArenaStaticProc.UomTransformation(uom, vertUom, vertStructPart.Elev.Value,3);

                                Enum.TryParse<UOM_DIST_VERT>(vertUom.ToString(), out vertDistUom);
                                vertStructPart.Elev_UOM = vertDistUom;
                            }

                            if (vertStructPart.VerticalExtent.HasValue)
                            {
                                uom = vertStructPart.VerticalExtent_UOM.ToString();
                                vertStructPart.VerticalExtent = ArenaStaticProc.UomTransformation(uom, vertUom, vertStructPart.VerticalExtent.Value,3);

                                Enum.TryParse<UOM_DIST_VERT>(vertUom.ToString(), out vertDistUom);
                                vertStructPart.VerticalExtent_UOM = vertDistUom;
                            }

                            if (vertStructPart.Height.HasValue)
                            {
                                uom = vertStructPart.Height_UOM.ToString();
                                vertStructPart.Height = ArenaStaticProc.UomTransformation(uom, vertUom, vertStructPart.Height.Value, 3);

                                Enum.TryParse<UOM_DIST_VERT>(vertUom.ToString(), out vertDistUom);
                                vertStructPart.Height_UOM = vertDistUom;
                            }
                            else
                            {
                                vertStructPart.Height = vertStructPart.Elev;
                                vertStructPart.Height_UOM = vertStructPart.Elev_UOM;
                            }

                           
                        }


                        ChartsHelperClass.SaveVerticalStructPoint_ChartGeo(Anno_ObstacleGeo_featClass, vstr, vertStructPart, VerticalLimit_M);
                        CreateVerticalStructureAnno(vertStructPart, FocusMapScale);

                        //break;
                    }

                }


            }
            catch
            {
            }

        }


        public static void CreateVerticalStructureAnno(PDM.VerticalStructurePart vsPart, double MapScale)
        {

            //foreach (PDM.VerticalStructurePart vsPart in vertStruct.Parts)
            {
                try
                {
                    if (vsPart.Elev.HasValue)
                    {
                        ChartElement_SimpleText chrtEl_vertpart_elev = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "VerticalStructurePartElev");
                        chrtEl_vertpart_elev.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(vsPart, chrtEl_vertpart_elev.TextContents[0][0].DataSource);
                        chrtEl_vertpart_elev.Slope = 0;//slope;
                        chrtEl_vertpart_elev.LinckedGeoId = vsPart.ID;
                        IElement el_Vert_Elev = (IElement)chrtEl_vertpart_elev.ConvertToIElement();
                        el_Vert_Elev.Geometry = ChartElementsManipulator.GetLinkedGeometry(chrtEl_vertpart_elev.Name, chrtEl_vertpart_elev.LinckedGeoId);
                        ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_vertpart_elev.Name, vsPart.ID, el_Vert_Elev, ref chrtEl_vertpart_elev, chrtEl_vertpart_elev.Id, MapScale);


                        if (vsPart.Height.HasValue)
                        {
                            ChartElement_SimpleText chrtEl_vertpart_height = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "VerticalStructurePartHeight");
                            chrtEl_vertpart_height.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(vsPart, chrtEl_vertpart_height.TextContents[0][0].DataSource, Rounder: 1);
                            chrtEl_vertpart_height.Slope = 0;//slope;
                            chrtEl_vertpart_height.LinckedGeoId = vsPart.ID;
                            IElement el_Vert_height = (IElement)chrtEl_vertpart_height.ConvertToIElement();
                            el_Vert_height.Geometry = ChartElementsManipulator.GetLinkedGeometry(chrtEl_vertpart_height.Name, chrtEl_vertpart_elev.LinckedGeoId);
                            ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_vertpart_height.Name, vsPart.ID, el_Vert_height, ref chrtEl_vertpart_height, chrtEl_vertpart_height.Id, MapScale);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    //continue;
                }
            }



        }


        public static bool GetInterestPoints(IMap FocusMap, ISpatialReference pSpatialReference, out IPoint StPnt, out IPoint CntrPnt, out IPoint EndPnt, IPolyline polyLine, out double anglCnt)
        {
            double angl;
            ILine ln = new LineClass();
            bool reverseFlag = false;

            StPnt = ((IPointCollection)polyLine).get_Point(0);
            EndPnt = ((IPointCollection)polyLine).get_Point(((IPointCollection)polyLine).PointCount - 1);
            if (((IPointCollection)polyLine).PointCount == 2)
            {
                //CntrPnt = (polyLine.Envelope as IArea).Centroid;

                CntrPnt = (polyLine as IPointCollection).PointCount > 2 ? getPointOnLine(polyLine) : getPointOnLine(StPnt, EndPnt, 40, FocusMap, pSpatialReference);

                ln.FromPoint = StPnt;
                ln.ToPoint = EndPnt;

                angl = ChartElementsManipulator.GetLineSlopE(ln, FocusMap, pSpatialReference) * 180 / Math.PI;
                angl = angl % 360;
                angl = angl < 0 ? angl + 360 : angl;


                if (angl > 90 && angl <= 270)
                {
                    ln.FromPoint = EndPnt;
                    ln.ToPoint = StPnt;

                    angl = ChartElementsManipulator.GetLineSlopE(ln, FocusMap, pSpatialReference) * 180 / Math.PI;
                    reverseFlag = true;
                }
                anglCnt = angl;
            }
            else
            {
                int indx = (int)((IPointCollection)polyLine).PointCount - 1;
                CntrPnt = ((IPointCollection)polyLine).get_Point(indx);
                double maxlen = -1;
                int MaxI = -1;
                for (int i = 0; i < indx; i++)
                {

                    double lenCur = Math.Sqrt(Math.Pow(((ESRI.ArcGIS.Geometry.IPointCollection2)polyLine).Point[i].X - ((ESRI.ArcGIS.Geometry.IPointCollection2)polyLine).Point[i + 1].X, 2) +
                                              Math.Pow(((ESRI.ArcGIS.Geometry.IPointCollection2)polyLine).Point[i].Y - ((ESRI.ArcGIS.Geometry.IPointCollection2)polyLine).Point[i + 1].Y, 2));
                    if (maxlen < lenCur)
                    {
                        maxlen = lenCur;
                        MaxI = i;
                        EndPnt = ((IPointCollection)polyLine).get_Point(MaxI + 1);
                        StPnt = ((IPointCollection)polyLine).get_Point(MaxI);
                        CntrPnt = i > 0 ? ((IPointCollection)polyLine).get_Point(i - 1) : ((IPointCollection)polyLine).get_Point(i);

                    }
                }
                ln.FromPoint = ((IPointCollection)polyLine).get_Point(MaxI);
                ln.ToPoint = ((IPointCollection)polyLine).get_Point(MaxI + 1);
                
                CntrPnt = getPointOnLine(ln.FromPoint, ln.ToPoint, 40, FocusMap, pSpatialReference);


                angl = ChartElementsManipulator.GetLineSlopE(ln, FocusMap, pSpatialReference) * 180 / Math.PI;
                angl = angl % 360;
                angl = angl < 0 ? angl + 360 : angl;


                if (angl > 90 && angl <= 270)
                {
                    ln.FromPoint = EndPnt;
                    ln.ToPoint = StPnt;

                    angl = ChartElementsManipulator.GetLineSlopE(ln, FocusMap, pSpatialReference) * 180 / Math.PI;
                    reverseFlag = true;
                }

                anglCnt = angl;


            }

            return reverseFlag;

        }

        private static IPoint getPointOnLine(IPolyline polyLine)
        {
            IPointCollection lnPnts = polyLine as IPointCollection;
            IPoint res = new PointClass();
            res.PutCoords(lnPnts.Point[0].X, lnPnts.Point[0].Y);
            double maxD = -10000;

            for (int i = 0; i < lnPnts.PointCount-1; i++)
            {
                double curD = Math.Pow(lnPnts.Point[i].X - lnPnts.Point[i + 1].X, 2) + Math.Pow(lnPnts.Point[i].Y - lnPnts.Point[i + 1].Y, 2);
                if (curD > maxD)
                {
                    maxD = curD;
                    res.PutCoords(lnPnts.Point[i].X, lnPnts.Point[i].Y);
                }
            }

            return res;
        }

        public static IPoint getPointOnLine(IPoint LineStartGeo, IPoint LineEndGeo, Double OffsetPercent, IMap FocusMap, ISpatialReference pSpatialReference)
        {
            GeometryFunctions.TapFunctions GF = new GeometryFunctions.TapFunctions();
            IPoint pRes = new PointClass();
            pRes.PutCoords(LineStartGeo.X, LineStartGeo.Y);
            pRes = (IPoint)EsriUtils.ToProject(pRes, FocusMap, pSpatialReference);

            ILine ll = new LineClass();

            IPoint pp1 = new PointClass();
            pp1.PutCoords(LineStartGeo.X, LineStartGeo.Y);
            ll.FromPoint = (IPoint)EsriUtils.ToProject(pp1, FocusMap, pSpatialReference);

            IPoint pp2 = new PointClass();
            pp2.PutCoords(LineEndGeo.X, LineEndGeo.Y);
            ll.ToPoint = (IPoint)EsriUtils.ToProject(pp2, FocusMap, pSpatialReference);


            double a = 180 * ll.Angle / Math.PI;
            IPoint res = GF.PointAlongPlane(pRes, a, ll.Length * OffsetPercent / 100);
            pRes = (IPoint)EsriUtils.ToGeo(res, FocusMap, pSpatialReference);

            GF = null;

            return pRes;
        }

        public static void SetMapScale(IMap pMap, List<PDMObject> selectedProcedures, IApplication SigmaApplication)
        {
            try
            {


                pMap.ClearSelection();
                ILayer layer = EsriUtils.getLayerByName(pMap, "ProcedureLegs");
                var pSelect = layer as IFeatureSelection;


                if (pSelect != null)
                {
                    pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultAdd;
                    var s = "( ";
                    if (selectedProcedures != null && selectedProcedures.Count > 0)
                    {


                        foreach (Procedure Proc in selectedProcedures)
                        {
                            foreach (ProcedureTransitions Trans in Proc.Transitions)
                            {
                                foreach (ProcedureLeg Legs in Trans.Legs)
                                {
                                    { s = s + " '" + Legs.ID + "', "; }

                                }
                            }
                        }


                        s = s + "'a'" + ")";

                    }

                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "FeatureGUID in " + s;

                    pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);

                    UID menuID = new UIDClass();

                    menuID.Value = "{AB073B49-DE5E-11D1-AA80-00C04FA37860}"; //zoomToSelected

                    ICommandItem pCmdItem = SigmaApplication.Document.CommandBars.Find(menuID);
                    pCmdItem.Execute();
                    Marshal.ReleaseComObject(pCmdItem);
                    Marshal.ReleaseComObject(menuID);
                    pMap.ClearSelection();

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace + " ERROR " + ex.Message);
                throw;
            }

           System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


        }


        public static IGeometry CreayteRwyStrip(Runway rwy, IMap FocusMap, ISpatialReference pSpatialReference)
        {
            try
            {
                IGeometry res = null;
                if (rwy.RunwayElementsList != null && rwy.RunwayElementsList.Count > 0)
                {
                    foreach (var rwyEl in rwy.RunwayElementsList)
                    {
                        if (rwyEl.Geo == null) rwyEl.RebuildGeo();
                    }

                    ITopologicalOperator2 topoOper2 = (ITopologicalOperator2)rwy.RunwayElementsList[0].Geo;

                    for (int i = 0; i < rwy.RunwayElementsList.Count; i++)
                    {
                        res = topoOper2.Union(rwy.RunwayElementsList[i].Geo);

                    }

                    IZAware zAware = (IZAware)res;
                    zAware.ZAware = false;
                    IMAware mAware = (IMAware)res;
                    mAware.MAware = false;

                    return res;
                }

                double runwayElementWidth = 300;
                //if (rwy.Width.HasValue) runwayElementWidth = rwy.Width.Value;

                

                double dir = rwy.RunwayDirectionList[0].MagBearing.HasValue? rwy.RunwayDirectionList[0].MagBearing.Value : -99;
                dir = rwy.RunwayDirectionList[0].TrueBearing.HasValue? rwy.RunwayDirectionList[0].TrueBearing.Value : -99;

                if (dir == -99) return null;
                
                if (rwy.RunwayDirectionList[0].Geo == null) rwy.RunwayDirectionList[0].RebuildGeo();
                if (rwy.RunwayDirectionList[1].Geo == null) rwy.RunwayDirectionList[1].RebuildGeo();

                IPoint start = new Point { X = ((IPoint)rwy.RunwayDirectionList[0].Geo).X, Y = ((IPoint)rwy.RunwayDirectionList[0].Geo).Y };
                IPoint end = new Point { X = ((IPoint)rwy.RunwayDirectionList[1].Geo).X, Y = ((IPoint)rwy.RunwayDirectionList[1].Geo).Y };

                start = (IPoint)EsriUtils.ToProject(start, FocusMap, pSpatialReference);
                end = (IPoint)EsriUtils.ToProject(end, FocusMap, pSpatialReference);

                dir = 90.0 - dir;

                TapFunctions util = new TapFunctions();


                var tmpPt1 = util.PointAlongPlane(start, dir + 90, runwayElementWidth / 2);
                var tmpPt2 = util.PointAlongPlane(start, dir - 90, runwayElementWidth / 2);
                var tmpPt3 = util.PointAlongPlane(end, dir - 90, runwayElementWidth / 2);
                var tmpPt4 = util.PointAlongPlane(end, dir + 90, runwayElementWidth / 2);


                IPointCollection ring = (IPointCollection)new ESRI.ArcGIS.Geometry.Ring();
                ring.AddPoint(tmpPt1);
                ring.AddPoint(tmpPt2);
                ring.AddPoint(tmpPt3);
                ring.AddPoint(tmpPt4);
                ring.AddPoint(tmpPt1);

                var geoPrj = (IGeometryCollection)new ESRI.ArcGIS.Geometry.Polygon();
                geoPrj.AddGeometry((IGeometry)ring);

                ITopologicalOperator2 topoOper = geoPrj as ITopologicalOperator2;
                topoOper.IsKnownSimple_2 = false;
                topoOper.Simplify();

                res = EsriUtils.ToGeo((IGeometry)geoPrj, FocusMap, pSpatialReference);

                util = null;
                return res;

            }
            catch
            {
                return null;
            }
        }

        private static List<PDMObject> CreateNavaidsList(List<ProcedureLeg> selectedLegs, IMap FocusMap, ISpatialReference pSpatialReference, RunwayDirection selRwyDir, double Bearing)
        {
            List<PDMObject> resList = new List<PDMObject>();

            foreach (var leg in selectedLegs)
            {
                double distToRwyLine = 0;
                if (leg.LegSpecialization == SegmentLegSpecialization.MissedApproachLeg) continue;

                if (leg.StartPoint!=null)
                {

                    List<PDMObject> lst = GetNavaidPnt(leg, true);

                    foreach (var PntObj in lst)
                    {
                        if (PntObj != null)
                        {
                            if (resList.IndexOf(PntObj) < 0)
                            {
                                distToRwyLine = calcDistFromRwy(PntObj, FocusMap, pSpatialReference, selRwyDir, Bearing);
                                PntObj.X = distToRwyLine;
                                if (distToRwyLine < 8000)
                                    resList.Add(PntObj);

                            }

                        }
                    }


                }

                if (leg.EndPoint!=null)
                {

                    List<PDMObject> lst = GetNavaidPnt(leg, false);

                    foreach (var PntObj in lst)
                    {
                        if (PntObj != null)
                        {
                            if (resList.IndexOf(PntObj) < 0)
                            {
                                distToRwyLine = calcDistFromRwy(PntObj, FocusMap, pSpatialReference, selRwyDir, Bearing);
                                PntObj.X = distToRwyLine;
                                if (distToRwyLine < 8000)
                                    resList.Add(PntObj);

                            }

                        }
                    }


                }
            }

            return resList;
        }

        private static List<PDMObject> GetNavaidPnt(ProcedureLeg leg, bool StartEnd)
        {
            List<PDMObject> resList = new List<PDMObject>();

            SegmentPoint _PointChoice = StartEnd ? leg.StartPoint : leg.EndPoint;
            PDMObject nav = DataCash.GetPDMObject(_PointChoice.Route_LEG_ID, PDM_ENUM.NavaidSystem);//DataCash.GetNavaidByID(_PointChoice.PointChoiceID);

            if (nav != null)
            {
                nav.Elev = leg.LowerLimitAltitude;
                nav.Elev_UOM = leg.LowerLimitAltitudeUOM;
                resList.Add(nav);
            }

            if (_PointChoice.PointFacilityMakeUp != null && _PointChoice.PointFacilityMakeUp.AngleIndication != null)
            {

                nav = DataCash.GetPDMObject(_PointChoice.PointFacilityMakeUp.AngleIndication.SignificantPointID,PDM_ENUM.NavaidSystem);

                if (nav != null && resList.IndexOf(nav) < 0)
                {
                    nav.Elev = leg.LowerLimitAltitude;
                    nav.Elev_UOM = leg.LowerLimitAltitudeUOM;

                }

            }

            if (_PointChoice.PointFacilityMakeUp != null && _PointChoice.PointFacilityMakeUp.DistanceIndication != null)
            {
                nav =  DataCash.GetPDMObject(_PointChoice.PointFacilityMakeUp.DistanceIndication.SignificantPointID,PDM_ENUM.NavaidSystem);
                if (nav != null && resList.IndexOf(nav) < 0)
                {
                    nav.Elev = leg.LowerLimitAltitude;
                    nav.Elev_UOM = leg.LowerLimitAltitudeUOM;
                }
            }

            return resList;
        }

        private static List<PDMObject> CreatePointsList(List<ProcedureLeg> selectedLegs, IMap FocusMap,ISpatialReference pSpatialReference,RunwayDirection selRwyDir,double Bearing)
        {
            List<PDMObject> resList = new List<PDMObject>();

            ProcedureFixRoleType[] FixRoleTypeArray = { ProcedureFixRoleType.IF, ProcedureFixRoleType.IF_IAF, ProcedureFixRoleType.FAF, ProcedureFixRoleType.FPAP, ProcedureFixRoleType.MAPT, ProcedureFixRoleType.SDF, ProcedureFixRoleType.IAF };


            foreach (var leg in selectedLegs)
            {
                PDMObject PntObj = null;
                double distToRwyLine = 0;
                if (leg.LegSpecialization == SegmentLegSpecialization.MissedApproachLeg) continue;

                if (leg.StartPoint != null && leg.StartPoint.PointRole.HasValue && FixRoleTypeArray.Contains(leg.StartPoint.PointRole.Value))
                {
                    PntObj = GetProfilePoint(leg,true);

                    if (PntObj != null)
                    {
                        if (resList.IndexOf(PntObj) < 0 )
                        {
                            distToRwyLine = calcDistFromRwy(PntObj, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            PntObj.X = distToRwyLine;
                            resList.Add(PntObj);

                        }
                       
                    }
                }

                if (leg.EndPoint != null && leg.EndPoint.PointRole.HasValue && FixRoleTypeArray.Contains(leg.EndPoint.PointRole.Value))
                {
                    PntObj = GetProfilePoint(leg, false);

                    if (PntObj != null)
                    {
                        if (resList.IndexOf(PntObj) < 0 )
                        {
                            distToRwyLine = calcDistFromRwy(PntObj, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            PntObj.X = distToRwyLine;
                            resList.Add(PntObj);

                        }

                    }
                }



            }

            return resList;
        }

        private static PDMObject GetProfilePoint(ProcedureLeg leg, bool StartEnd)
        {

            SegmentPoint _PointChoice = StartEnd ? leg.StartPoint : leg.EndPoint;
            PDMObject PntObj = DataCash.GetPDMObject(_PointChoice.PointChoiceID, PDM_ENUM.WayPoint);
            if (PntObj == null) PntObj = DataCash.GetPDMObject(_PointChoice.PointChoiceID, PDM_ENUM.NavaidSystem);
            //if (PntObj == null) PntObj = DataCash.GetNavaidByID(PointChoiceID);
            if (PntObj != null)
            {

                PntObj.ID = leg.ID;
                PntObj = _PointChoice;
                PntObj.Elev = leg.LowerLimitAltitude;
                PntObj.Elev_UOM = leg.LowerLimitAltitudeUOM;
                PntObj.Lat = _PointChoice.PointRole.Value.ToString();
                //PntObj.SourceDetail = "PointInTrack";
            }
            

            return PntObj;
        }


        public static void CreateProfile(IHookHelper SigmaHookHelper, string _selectedProcID ,FinalProfile _profile, string _selectedProcInstruction, AirportHeliport Arp, RunwayDirection selRwyDir, Runway rwy, 
            List<ProcedureLeg> selectedLegs, IMap FocusMap, ISpatialReference pSpatialReference, UOM_DIST_VERT VertUom, UOM_DIST_HORZ DistUom, bool rnav)
        {
            try
            {
               

                double distanceMultiplier = DistUom == UOM_DIST_HORZ.KM ? 0.001 : 0.0005399568;
                double elevationMultiplier = VertUom == UOM_DIST_VERT.M ? 1 : 3.28083; 
             
                double maxDistfromRwy = 0;
                double minDistfromRwy = 10000000;
                double maxElevAboveRwy = -10000000;
                double minElevAboveRwy = 10000000;
                //double distToRwyLine = 1000000;
                List<IPoint> profilePointsArray = new List<IPoint>();

                IElement profeliGroup = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "Sigma_ProfileCanvas");
                if (profeliGroup == null) return;

                IPoint profeliGroupAncor = profeliGroup.Geometry.Envelope.UpperLeft;

                IGroupElement3 resultGrpEl = new GroupElementClass();
                IElementProperties docElementProperties2;

                #region направление захода на посадку (слева-направо или справа-налево)

                int profileDirection = 1;

                string ss = selRwyDir.Designator.Length > 2 ? selRwyDir.Designator.Remove(2) : selRwyDir.Designator;

                Int32.TryParse(ss, out profileDirection);

                if (profileDirection == 36 || (profileDirection >= 1 && profileDirection <= 17)) profileDirection = -1; //слева-направо
                else profileDirection = 1; //справо-налево

                #endregion

                Utilitys _aranSupportUtil = new Utilitys();
                TapFunctions TapUtil = new TapFunctions();
                if (selRwyDir.Geo == null) selRwyDir.RebuildGeo();


                double OCA = getOCA(selectedLegs);
                double OCH = getOCH(selectedLegs);

                double? ThresholdCrossingHeightValue = null;

                double Bearing = selRwyDir.TrueBearing.HasValue ? selRwyDir.TrueBearing.Value : selRwyDir.MagBearing.Value;

                if (selRwyDir.Geo == null) selRwyDir.RebuildGeo();
                double dirBearing = _aranSupportUtil.Azt2Direction((IPoint)selRwyDir.Geo, Bearing, FocusMap, pSpatialReference);
                double rdnElevInFt = selRwyDir.Elev.HasValue ? Math.Round(selRwyDir.ConvertValueToFeet(selRwyDir.Elev.Value, selRwyDir.Elev_UOM.ToString()), 0) : 0;
                double rdnElevInMeter = selRwyDir.Elev.HasValue ? selRwyDir.ConvertValueToMeter(selRwyDir.Elev, selRwyDir.Elev_UOM.ToString()).Value : 0;
                double rwylen_M = rwy != null ? rwy.ConvertValueToMeter(rwy.Length.Value, rwy.Uom.ToString()) : 3000;


                

                // подготовка
                ProcedureLeg finalLeg = null;
                NavaidSystem _mainNavaid = null;
                NavaidSystem arpNav = null;

                double GP = getGP(selectedLegs, ref finalLeg, ref _mainNavaid, ref ThresholdCrossingHeightValue);
                double VA = GP;
                bool ItsPrecision = (finalLeg != null && ((FinalLeg)finalLeg).LandingSystemCategory != CodeApproachGuidance.NON_PRECISION && ((FinalLeg)finalLeg).LandingSystemCategory != CodeApproachGuidance.OTHER);
                bool NoFAF = finalLeg.StartPoint!=null ? finalLeg.StartPoint.PointFacilityMakeUp == null : finalLeg.EndPoint.PointFacilityMakeUp == null;
                bool IAFflag = false;
                MissaedApproachLeg missedAppLeg = GetMissedApproachLeg(selectedLegs);

                IPoint HorPartPointAfterSDF = ItsPrecision ? null : HorPart_pointAfterSDF(selectedLegs, _profile);
                IPoint HorPartPointAfterFAF = ItsPrecision ? null : HorPart_pointAfterFAF(selectedLegs, _profile);

                if (_mainNavaid != null)
                {
                    _mainNavaid.X = calcDistFromRwy(_mainNavaid, FocusMap, pSpatialReference, selRwyDir, Bearing);
                }
                double shiftnavaid = 0;//ItsPrecision && _mainNavaid != null  ? 0 : _mainNavaid!=null? _mainNavaid.X.Value : 0;

                if (_mainNavaid!=null &&  _mainNavaid.X.Value > 5556) shiftnavaid = 0; //если _mainNavaid находится ближе чем 3 NM от аэропорта, т.е является аэродромным средством

                if (ItsPrecision)
                {
                    arpNav = (NavaidSystem)DataCash.GetAirportNavaidByAirportID(Arp.ID, NavaidSystemType.VOR_DME);

                    if (arpNav == null && _mainNavaid != null) arpNav = _mainNavaid;
                    if (arpNav != null)
                        arpNav.X = calcDistFromRwy(arpNav, FocusMap, pSpatialReference, selRwyDir, Bearing);
                   
                }

                if (HorPartPointAfterSDF != null)
                {
                    ApproachAltitude hpAlt = new ApproachAltitude { Altitude = HorPartPointAfterSDF.Y, AltitudeReference = CODE_DIST_VER.MSL, AltitudeUOM = UOM_DIST_VERT.M, MeasurementPoint = CodeProcedureDistance.MM };
                    ApproachDistance hpDist = new ApproachDistance { Distance = HorPartPointAfterSDF.X , DistanceUOM = UOM_DIST_HORZ.M, ValueHAT = null, ValueHATUOM = UOM_DIST_VERT.SM, EndingMeasurementPoint = CodeProcedureDistance.THLD, StartingMeasurementPoint = CodeProcedureDistance.MM };

                    _profile.ApproachAltitudeTable.Insert(_profile.ApproachAltitudeTable.Count - 1, hpAlt);
                    _profile.ApproachDistancetable.Insert(_profile.ApproachDistancetable.Count - 1, hpDist);
                }

                if (HorPartPointAfterFAF != null)
                {
                    ApproachAltitude hpAlt = new ApproachAltitude { Altitude = HorPartPointAfterFAF.Y, AltitudeReference = CODE_DIST_VER.MSL, AltitudeUOM = UOM_DIST_VERT.M, MeasurementPoint = CodeProcedureDistance.OM };
                    ApproachDistance hpDist = new ApproachDistance { Distance = HorPartPointAfterFAF.X, DistanceUOM = UOM_DIST_HORZ.M, ValueHAT = null, ValueHATUOM = UOM_DIST_VERT.SM, EndingMeasurementPoint = CodeProcedureDistance.THLD, StartingMeasurementPoint = CodeProcedureDistance.OM };

                    _profile.ApproachAltitudeTable.Insert(1, hpAlt);
                    _profile.ApproachDistancetable.Insert(1, hpDist);
                }


                List<RouteSegment> ProfileSegmentList = GetPointsFromProfile(_profile);

                if (ProfileSegmentList == null) return;

    
                RouteSegment firstSeg = GetFirstSegment_IF(selectedLegs, ProfileSegmentList[0], FocusMap, pSpatialReference, selRwyDir, Bearing);
                if (firstSeg != null)
                {
                    ProfileSegmentList.Insert(0, firstSeg);


                }

                #region IAF

                RouteSegment iafSeg = GetIAF_First(selectedLegs, ProfileSegmentList[0], FocusMap, pSpatialReference, selRwyDir, Bearing);
                if (iafSeg != null)
                {
                    ApproachDistance apDist = _profile.ApproachDistancetable[0]; // FAF - THR

                    ProfileSegmentList.Insert(0, iafSeg);


                    IAFflag = true;

                }

                #endregion


                List<PDMObject> NavSystemsList = GetNavaidsList(selectedLegs, selRwyDir, Bearing, FocusMap, pSpatialReference);
                if (arpNav != null && !NavSystemsList.Contains(arpNav)) NavSystemsList.Add(arpNav);

                SetMax_Dist_Elev(ProfileSegmentList, rdnElevInMeter, ref maxDistfromRwy, ref minDistfromRwy, ref maxElevAboveRwy, ref minElevAboveRwy);


                if (iafSeg != null)
                {
                    var pegPnt = iafSeg.StartPoint != null ? iafSeg.StartPoint : iafSeg.EndPoint;

                    if (pegPnt != null && pegPnt.Elev.HasValue && pegPnt.Elev.Value > maxElevAboveRwy)
                    maxElevAboveRwy = iafSeg.StartPoint.Elev.Value;
                }



                    /////////////////////////////// Рисование
                IPoint pp = new PointClass { X = 0, Y = 0 };
                double dX = profileDirection < 0 ? ((IPolygon)profeliGroup.Geometry).Envelope.LowerRight.X : ((IPolygon)profeliGroup.Geometry).Envelope.LowerLeft.X;
                double dY = profileDirection < 0 ? ((IPolygon)profeliGroup.Geometry).Envelope.LowerRight.Y : ((IPolygon)profeliGroup.Geometry).Envelope.LowerLeft.Y;

                double widthScale = (maxDistfromRwy + rwylen_M + 3704) / (((IPolygon)profeliGroup.Geometry).Envelope.Width);

                double heightScale = (maxElevAboveRwy  - rdnElevInMeter) / (((IPolygon)profeliGroup.Geometry).Envelope.Height);

                if (heightScale == 0) heightScale = maxElevAboveRwy / (((IPolygon)profeliGroup.Geometry).Envelope.Height);

                double rulerShift = 0.0;
                double textShift = 0.5;
                double ticsHeight = 0.1;
                double navHeight = 1.5;
                double navWidth = 0.4;

                // бумажные координаты торца (сдвинуть от края на 2 NM = 3704 meter)
                double thrX = dX + (3704 / widthScale) * profileDirection;
                double thrY = dY;
                double k = 0;
                double dispValue = 0;
                #region вспомогательные линии

                /// create Rwy line
                /// 

                double vertLineShift = 0;

                IGroupElement3 rwy_linesGr = new GroupElementClass();

              

                profilePointsArray.Add(new PointClass { X = thrX, Y = thrY });
                profilePointsArray.Add(new PointClass { X = thrX - (rwylen_M / widthScale) * profileDirection, Y = thrY });
                var lineEl = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(profilePointsArray, 0, 4);
                rwy_linesGr.AddElement(lineEl);
                int ticCount = (int)(((IPolygon)profeliGroup.Geometry).Envelope.Width / 3704 * widthScale);


                #region рисование navaids

                if (NavSystemsList != null && NavSystemsList.Count > 0)
                {
                    foreach (var item in NavSystemsList)
                    {
                        NavaidSystem nvd = (NavaidSystem)item;

                        if ((((NavaidSystem)nvd).CodeNavaidSystemType == NavaidSystemType.ILS || ((NavaidSystem)nvd).CodeNavaidSystemType == NavaidSystemType.ILS_DME)) continue;


                        foreach (var component in ((NavaidSystem)nvd).Components)
                        {
                            if (component.PDM_Type != PDM_ENUM.DME) continue;

                            if (((DME)component).Displace.HasValue && !Double.IsNaN(((DME)component).Displace.Value))
                            {
                                component.X = component.X - ((DME)component).Displace.Value;
                                dispValue = ((DME)component).Displace.Value;
                            }
                            break;
                        }

                        if (dispValue == 0)
                        {

                            IPointCollection4 pointCollection = new PolygonClass();
                            pointCollection.AddPoint(new PointClass { X = thrX + (nvd.X.Value / widthScale) * profileDirection + navWidth / 2 * profileDirection, Y = thrY });
                            pointCollection.AddPoint(new PointClass { X = thrX + (nvd.X.Value / widthScale) * profileDirection + navWidth / 2 * profileDirection, Y = thrY + navHeight + k });
                            pointCollection.AddPoint(new PointClass { X = thrX + (nvd.X.Value / widthScale) * profileDirection - navWidth / 2 * profileDirection, Y = thrY + navHeight + k });
                            pointCollection.AddPoint(new PointClass { X = thrX + (nvd.X.Value / widthScale) * profileDirection - navWidth / 2 * profileDirection, Y = thrY });

                            var plgnEl = AranSupport.AnnotationUtil.GetPolygonElement((IPolygon3)pointCollection);
                            resultGrpEl.AddElement(plgnEl);

                            pp = new PointClass { X = dX + (3704 / widthScale) * profileDirection + (nvd.X.Value / widthScale) * profileDirection, Y = thrY + navHeight + textShift - 0.1 + k };
                            resultGrpEl.AddElement(AranSupport.AnnotationUtil.CreateFreeTextElement(pp, nvd.GetObjectLabel(), false, true, 6, horizontalAlignment.Center, verticalAlignment.Center, fillStyle.fSNull, true));

                            k += 0.2;
                        }

                    }

                }

                #endregion


                #region horizontal ruler

                IGroupElement3 hor_linesGr = new GroupElementClass();
                profilePointsArray = new List<IPoint>();

                profilePointsArray.Add(new PointClass { X = dX + vertLineShift, Y = dY - rulerShift });
                profilePointsArray.Add(new PointClass { X = dX + (((IPolygon)profeliGroup.Geometry).Envelope.Width) * profileDirection, Y = dY - rulerShift });
                lineEl = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(profilePointsArray, 0, 2);
                hor_linesGr.AddElement(lineEl);

                //tics
                int i = 0;
                int ticsLabel = 0;
                double step = DistUom == UOM_DIST_HORZ.NM ? 1852 : 1000;

                double maxStep = 0;
                if(shiftnavaid < 0)
                    maxStep = (maxDistfromRwy + shiftnavaid * -1) * distanceMultiplier;
                else if (shiftnavaid > maxDistfromRwy)
                    maxStep = (maxDistfromRwy + (shiftnavaid - maxDistfromRwy)) * distanceMultiplier;
                else
                    maxStep = maxDistfromRwy * distanceMultiplier;



                double curPosition = thrX + i * step / widthScale;

                double st = thrX;
                if (!ItsPrecision && _mainNavaid != null && _mainNavaid.X.Value <= 5556) //если _mainNavaid находится ближе чем 3 NM от аэропорта, т.е является аэродромным средством
                {
                    //st = dX + (3704 / widthScale) * profileDirection + (_mainNavaid.X.Value / widthScale) * profileDirection;
                    //curPosition = st + i * step / widthScale * profileDirection;
                    //ticsLabel =  i - Convert.ToInt32(Math.Round(Math.Abs(_mainNavaid.X.Value / widthScale)) + 1);
                }

                while (i < maxStep +1)
                {
                    profilePointsArray = new List<IPoint>();
                    profilePointsArray.Add(new PointClass { X = st + i * step / widthScale * profileDirection, Y = dY - rulerShift });
                    profilePointsArray.Add(new PointClass { X = st + i * step / widthScale * profileDirection, Y = dY - rulerShift - ticsHeight });
                    lineEl = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(profilePointsArray, 0, 1);
                    hor_linesGr.AddElement(lineEl);

                    //text
                    pp = new PointClass { X = st + i * (step / widthScale) * profileDirection, Y = dY - textShift };
                    var freeText = AranSupport.AnnotationUtil.CreateFreeTextElement(pp, ticsLabel.ToString(), false, false, 6);
                    hor_linesGr.AddElement(freeText);


                    i++;
                    ticsLabel++;
                    curPosition = st + i * step / widthScale * profileDirection;

                    
                }

                #endregion

                #region vertical ruler

                IGroupElement3 ver_linesGr = new GroupElementClass();

                profilePointsArray = new List<IPoint>();

                double vertLinePos = ((IPolygon)profeliGroup.Geometry).Envelope.LowerRight.X + vertLineShift; // dX
                double topPosY = 0.2 + dY + (maxElevAboveRwy - rdnElevInMeter) / heightScale;

                profilePointsArray.Add(new PointClass { X = vertLinePos, Y = dY - rulerShift });
                profilePointsArray.Add(new PointClass { X = vertLinePos, Y = topPosY });
                lineEl = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(profilePointsArray, 0, 2);
                ver_linesGr.AddElement(lineEl);

                //tics
                i = 0;//;
                curPosition = dY;

                while (((IPolygon)profeliGroup.Geometry).Envelope.LowerLeft.Y / heightScale <= curPosition && topPosY >= curPosition)
                {
                    profilePointsArray = new List<IPoint>();
                    profilePointsArray.Add(new PointClass { X = vertLinePos, Y = curPosition });
                    profilePointsArray.Add(new PointClass { X = vertLinePos - ticsHeight * profileDirection, Y = curPosition });
                    lineEl = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(profilePointsArray, 0, 1);
                    ver_linesGr.AddElement(lineEl);

                    //text
                    pp = new PointClass { X = vertLinePos - textShift * -1, Y = curPosition };

                    var freeText = AranSupport.AnnotationUtil.CreateFreeTextElement(pp, (i + Convert.ToInt32(Arp.ConvertValueToFeet(rdnElevInMeter, "M"))).ToString(), false, false, 6);
                    ver_linesGr.AddElement(freeText);

                    i = Convert.ToInt32(i + 1000);
                    curPosition = thrY + Arp.ConvertValueToMeter(i, VertUom.ToString()) / heightScale;
                }



                #endregion


                IGroupElement3 _linesGr = new GroupElementClass();

                _linesGr.AddElement((IElement)rwy_linesGr);
                _linesGr.AddElement((IElement)hor_linesGr);
                //_linesGr.AddElement((IElement)ver_linesGr); //убрать вертикальную шкалу

                resultGrpEl.AddElement((IElement)_linesGr);


                #endregion

                #region рисование профиля

                #region профиль

                IGroupElement3 profile_lines_Gr = new GroupElementClass();
                profilePointsArray = new List<IPoint>();
                IPointCollection polyLine = new ESRI.ArcGIS.Geometry.Polyline();
                int segCounter = 0; 
                //bool FAF_OCH_Flag  = false;

                foreach (var ProfileSegment in ProfileSegmentList)
                {
                    if (ProfileSegment.GetObjectLabel().StartsWith("MAP : THLD") && (ProfileSegment.EndPoint.Elev == null || !ProfileSegment.EndPoint.Elev.HasValue)) continue;
                    //if (ProfileSegment.GetObjectLabel().StartsWith("FAF : OM")) continue;
                    double curX = thrX + (ProfileSegment.ValLen.Value / widthScale) * profileDirection;
                    double curY = thrY + (ProfileSegment.ConvertValueToMeter(ProfileSegment.StartPoint.Elev.Value, ProfileSegment.Elev_UOM.ToString()) - rdnElevInMeter) / heightScale;
                    //double curY = thrY + (ProfileSegment.ConvertValueToMeter(ProfileSegment.StartPoint.Elev.Value, ProfileSegment.Elev_UOM.ToString())) / heightScale;

                    // точка линии профиля
                    IPoint profilPP = new PointClass { X = curX, Y = curY };

                    
                    ProcedureFixRoleType fr = ProfileSegment.StartPoint.PointRole.HasValue? ProfileSegment.StartPoint.PointRole.Value : ProcedureFixRoleType.OTHER;
                    profilPP.ID = (int)fr;
                    profilePointsArray.Add(profilPP);

                    // точка в линию проекции на масштабную линейку
                    List<IPoint> projectLinePointArray = new List<IPoint>();
                    projectLinePointArray.Add(profilPP);
                    pp = new PointClass { X = curX, Y = thrY };
                    projectLinePointArray.Add(pp);

                    //линия проекции на масштабную линейку
                    var lineProj = AranSupport.AnnotationUtil.GetPolylineElement_Simle(projectLinePointArray, 0, esriSimpleLineStyle.esriSLSDot, 1);
                    if (!ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("FAF") && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("OM") && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("MM"))
                        resultGrpEl.AddElement(lineProj);
                    else if (ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("FAF") && !NoFAF)
                        resultGrpEl.AddElement(lineProj);

                    //подпись к линии проекции на масштабную линейку PointRole
                    string lbl = ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("OTHER_SDF") ? "SDF" : ProfileSegment.StartPoint.SegmentPointDesignator;
                    if (ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("PFAF")) lbl = "FAP";
                    if (ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("MAP")) lbl = "MAPt";

                    lbl = (lbl.StartsWith("FAF") || lbl.StartsWith("FAP")) && NoFAF ? "" : lbl;
                    lbl = (lbl.StartsWith("OM")) ? "" : lbl;
                    lbl = (lbl.StartsWith("MM")) ? "" : lbl;

                    verticalAlignment va = lbl.StartsWith("IAF") || lbl.StartsWith("FAF") || lbl.StartsWith("SDF") ? verticalAlignment.Bottom : verticalAlignment.Center;
                    pp = new PointClass { X = curX, Y = thrY + ProfileSegment.ConvertValueToMeter((ProfileSegment.StartPoint.Elev.Value) / 2, ProfileSegment.Elev_UOM.ToString()) / heightScale };
                    if (lbl.StartsWith("IAF") || lbl.StartsWith("FAF") || lbl.StartsWith("SDF"))
                    {
                        pp = new PointClass { X = curX, Y = curY };
                        lbl = lbl + ":";
                        lbl = ProfileSegment.StartPoint.Elev_UOM != VertUom?  lbl + " " + ChartsHelperClass.Round_Hundred(ProfileSegment.StartPoint.Elev.Value * elevationMultiplier).ToString() :
                            lbl + " " + Math.Round(ProfileSegment.StartPoint.Elev.Value, 0).ToString();
                    }
                    var freeText = AranSupport.AnnotationUtil.CreateFreeTextElement(pp, lbl, false, profileDirection > 0, 8, horizontalAlignment.Center, va, fillStyle.fSNull,true,"Arial",0,':');
                    resultGrpEl.AddElement(freeText);



                    lbl = "";
                    if (ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("IF"))
                    {
                        lbl = ProfileSegment.StartPoint.Route_LEG_ID != null && ProfileSegment.StartPoint.Route_LEG_ID.Length > 0 ? ProfileSegment.StartPoint.Route_LEG_ID : "";
                        if (lbl.Length > 0) lbl = lbl + ":";
                        lbl = ProfileSegment.StartPoint.Lon != null && ProfileSegment.StartPoint.Lon.Length > 0 ? lbl + ProfileSegment.StartPoint.Lon : lbl;
                        lbl = ProfileSegment.StartPoint.X.HasValue ? "IF " + lbl + " " + Math.Round(ProfileSegment.StartPoint.X.Value * distanceMultiplier, 2).ToString() + "D" : "IF " + lbl;
                        pp = new PointClass { X = curX, Y = curY };
                        freeText = AranSupport.AnnotationUtil.CreateFreeTextElement(pp, lbl, false, profileDirection > 0, 8, horizontalAlignment.Center, verticalAlignment.Bottom, fillStyle.fSSolid, true, "Arial", 0, ':');
                        resultGrpEl.AddElement(freeText);
                    }

                    //звездочка в случае неточный заход FAF
                    if (ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("FAF") && !NoFAF)
                    {
                        pp = new PointClass { X = curX, Y = curY };
                        freeText = AranSupport.AnnotationUtil.CreateFreeTextElement(pp, "I", false, profileDirection > 0, 12, horizontalAlignment.Center, verticalAlignment.Center, fillStyle.fSSolid, false, "AeroSigma");
                        resultGrpEl.AddElement(freeText);

                    }


                    //подпись к линии проекции на масштабную линейку - значение dist

                    lbl = Math.Round((ProfileSegment.ValLen.Value - shiftnavaid) * distanceMultiplier, 1).ToString();
                    pp = new PointClass { X = curX, Y = thrY - ticsHeight - 0.1 };
                    var distText = AranSupport.AnnotationUtil.CreateFreeTextElement(pp, lbl, profileDirection > 0, false, 6);

                    if (!ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("FAF") && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("OM") && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("IAF")
                        && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("IAF") && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("MM"))
                        resultGrpEl.AddElement(distText);
                    else if (ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("FAF") && !NoFAF)
                        resultGrpEl.AddElement(distText);

                    //значение высоты (если участок прямолинейный)
                    polyLine.AddPoint(new Point { X = curX, Y = curY });

                    if (polyLine.PointCount == 2 && ProfileSegment.ID_Route != null && !ProfileSegment.ID_Route.StartsWith("ignore") && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("OM") && !ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("MM"))
                    {
                        double legHeightinMeter = ProfileSegment.ConvertValueToMeter(ProfileSegment.StartPoint.Elev.Value, ProfileSegment.StartPoint.Elev_UOM.ToString());
                        double legHeight = Math.Round(legHeightinMeter * elevationMultiplier, 0);
                        var txt = AranSupport.AnnotationUtil.CreateFreeTextElement((IPolyline)polyLine, Math.Round(legHeight, 0).ToString(), profileDirection > 0, false, 8, horizontalAlignment.Center, verticalAlignment.Bottom);
                        if (((IPolyline)polyLine).FromPoint.Y == ((IPolyline)polyLine).ToPoint.Y) resultGrpEl.AddElement(txt);
                        legHeight = (legHeightinMeter - rdnElevInMeter) * elevationMultiplier;
                        txt = AranSupport.AnnotationUtil.CreateFreeTextElement((IPolyline)polyLine, "(" + Math.Round(legHeight, 0).ToString() + ")", profileDirection > 0, false, 8, horizontalAlignment.Center, verticalAlignment.Top);
                        if (((IPolyline)polyLine).FromPoint.Y == ((IPolyline)polyLine).ToPoint.Y) resultGrpEl.AddElement(txt);

                        polyLine = new ESRI.ArcGIS.Geometry.Polyline();
                        polyLine.AddPoint(new Point { X = curX, Y = curY });
                    }

                    //рисование OCH 
                    if (!NoFAF && segCounter < ProfileSegmentList.Count-1)
                    {
                        /// найти соответсвующий сегменту минимум
                        /// 
                        string mP_designator = ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("OTHER_SDF") ? "SDF" : ProfileSegment.StartPoint.SegmentPointDesignator;
                        ApproachMinima apMin = _profile.ApproachMinimaTable.FindAll(r => r.ProfileSegmnetDesignator.StartsWith(mP_designator)).FirstOrDefault();
                        double nextX = thrX + (ProfileSegmentList[segCounter + 1].ValLen.Value / widthScale) * profileDirection;

                        //FAF_OCH_Flag = ProfileSegment.StartPoint.SegmentPointDesignator.StartsWith("FA");

                        if (apMin!=null)
                        {
                            double valArp = 0;

                            if (Arp.Elev.HasValue)
                            {

                                valArp = Arp.ConvertValueToMeter(Arp.Elev.Value, Arp.Elev_UOM.ToString());
                                double d = valArp > rdnElevInMeter ? valArp - rdnElevInMeter : rdnElevInMeter - valArp;
                                if ((d * elevationMultiplier) >= 2)
                                {
                                    rdnElevInMeter = valArp;
                                    rdnElevInFt = Arp.ConvertValueToFeet(Arp.Elev.Value, Arp.Elev_UOM.ToString());
                                }

                            }

                            //double apOCA = ProfileSegment.ConvertValueToMeter(apMin.Minima, apMin.MinimaUom.ToString()) - valArp;
                            double apOCA = ProfileSegment.ConvertValueToMeter(apMin.Minima, apMin.MinimaUom.ToString());


                            IPoint lowerL = new PointClass { X = curX , Y = thrY }; 
                            IPoint upperL = new PointClass { X = curX , Y = thrY + (apOCA- valArp) / heightScale};
                            IPoint lowerR = new PointClass { X = nextX, Y = thrY };
                            IPoint upperR = new PointClass { X = nextX, Y = thrY + (apOCA - valArp) / heightScale };

                            IPointCollection4 pointCollection = new PolygonClass();
                            pointCollection.AddPoint(lowerL);
                            pointCollection.AddPoint(upperL);
                            pointCollection.AddPoint(upperR);
                            pointCollection.AddPoint(lowerR);

                            var plgnEl = AranSupport.AnnotationUtil.GetPolygonElement((IPolygon3)pointCollection,markerFillStyle: esriSimpleMarkerStyle.esriSMSX);
                            resultGrpEl.AddElement((IElement)plgnEl);

                            
                            
                            double oca = Math.Round(apOCA * elevationMultiplier);
                            double och = VertUom == UOM_DIST_VERT.M ? Math.Round(apOCA * elevationMultiplier) - rdnElevInMeter : Math.Round(apOCA * elevationMultiplier) - rdnElevInFt;
                            lbl = oca.ToString() + "(" + Math.Round(och).ToString() + ")";
                            if (((IArea)plgnEl.Geometry).Area > 0)
                            {
                                var ocaText = AranSupport.AnnotationUtil.CreateFreeTextElement(((IArea)plgnEl.Geometry).Centroid, lbl, profileDirection > 0, false, 6, UseHalo: 1);
                                resultGrpEl.AddElement(ocaText);
                            }

                        }

                    }

                    segCounter++;
                }

                #endregion


                #region finall leg

                if (finalLeg != null)
                {
                    IPointCollection finalLegPointsArray = new ESRI.ArcGIS.Geometry.Polyline();
                    int offset = IAFflag ? 1 : 0;
                    if (!NoFAF)
                    {
                        finalLegPointsArray.AddPoint(profilePointsArray[1 + offset]);
                        finalLegPointsArray.AddPoint(profilePointsArray[2 + offset]);
                    }
                    else
                    {
                        finalLegPointsArray.AddPoint(profilePointsArray[0 + offset]);
                        finalLegPointsArray.AddPoint(profilePointsArray[1 + offset]);
                    }


                    double legCourse = Math.Round(finalLeg.Course.Value, 0);
                    string legCourseTxt = "";
                    if (finalLeg.CourseType != CodeCourse.MAG_TRACK)
                    {
                        legCourse = Arp.MagneticVariation.HasValue ? Math.Round(legCourse - Arp.MagneticVariation.Value) : legCourse;
                        if (legCourse < 0) legCourse += 360;
                    }

                    legCourseTxt = legCourse.ToString();
                    if (legCourseTxt.Length < 3) legCourseTxt = "0" + legCourseTxt;
                    if (legCourseTxt.Length < 3) legCourseTxt = "0" + legCourseTxt;
                    legCourseTxt = legCourseTxt + "°";

                    var txt = AranSupport.AnnotationUtil.CreateFreeTextElement((IPolyline)finalLegPointsArray, legCourseTxt , profileDirection > 0, false, 8, horizontalAlignment.Center, ItsPrecision ? verticalAlignment.Center : verticalAlignment.Top, fillStyle.fSSolid, false, "Arial", ItsPrecision ? -5 : 0);
                    resultGrpEl.AddElement(txt);

                    IGroupElement3 finalLegGr = new GroupElementClass();

                    if (ItsPrecision || rnav)
                    {
                        #region glide Path

                        ILine ln = new LineClass();
                        ln.FromPoint = profilePointsArray[2];
                        ln.ToPoint = profilePointsArray[1];
                        IPoint ptSt = profilePointsArray[2];

                        double dist = ln.Length - 1;
                        //double a = 0.05;

                        /////////////////////////////////////////////////////////////

                        IPoint ptL = new PointClass { X = ln.ToPoint.X - 0.8, Y = ln.ToPoint.Y };
                        IPoint ptR = new PointClass { X = ln.ToPoint.X + 0.8, Y = ln.ToPoint.Y };


                        IPointCollection4 pointCollection = new PolygonClass();
                        pointCollection.AddPoint(ptSt);
                        pointCollection.AddPoint(ptL);
                        pointCollection.AddPoint(ptR);
                        pointCollection.AddPoint(ptSt);

                        var plgnEl = AranSupport.AnnotationUtil.GetPolygonElement((IPolygon3)pointCollection);
                        finalLegGr.AddElement((IElement)plgnEl);


                        List<IPoint> pointArr2 = new List<IPoint>();

                        pointArr2.Add(profilePointsArray[1 + offset]);
                        pointArr2.Add(profilePointsArray[2 + offset]);

                        var tmp = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(pointArr2, 0, 1.5, true, false);
                        txt = AranSupport.AnnotationUtil.CreateFreeTextElement(tmp.Geometry, "GP " + Math.Round(GP, 1).ToString() + "°", profileDirection < 0, false, 8, horizontalAlignment.Center, verticalAlignment.Bottom, fillStyle.fSSolid);
                        finalLegGr.AddElement(txt);

                        if (ThresholdCrossingHeightValue.HasValue)
                        {
                            UpdateDinamicText(SigmaHookHelper, "Sigma_IlsRdh", Math.Round(ThresholdCrossingHeightValue.Value).ToString(), false);
                        }
                        else
                        {
                            Delete_GraphicsContainerElemnt_ByName(SigmaHookHelper, "Sigma_IlsRdh");
                        }



                        #endregion
                    }
                    else
                    {
                        List<IPoint> pointArrGP = new List<IPoint>();

                        if (!NoFAF)
                        {
                            pointArrGP.Add(profilePointsArray[1 + offset]);
                            pointArrGP.Add(profilePointsArray[2 + offset]);
                        }
                        else
                        {
                            pointArrGP.Add(profilePointsArray[0 + offset]);
                            pointArrGP.Add(profilePointsArray[1 + offset]);
                        }

                        var tmpGP = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(pointArrGP, 0, 1.5, true, false);
                        GP = Math.Atan(TapUtil.DegToRad(GP)) * 100;
                        txt = AranSupport.AnnotationUtil.CreateFreeTextElement(tmpGP.Geometry, Math.Round(GP, 1).ToString() + "%", profileDirection < 0, false, 8, horizontalAlignment.Center, verticalAlignment.Bottom, fillStyle.fSSolid, false, "Arial", 0);
                        if (!NoFAF)  finalLegGr.AddElement(txt);

                        Delete_GraphicsContainerElemnt_ByName(SigmaHookHelper, "Sigma_IlsRdh");
                    }

                    resultGrpEl.AddElement((IElement)finalLegGr);
                }

                #endregion


                #region missed approach leg

                if (missedAppLeg != null)
                {

                    List<IPoint> missedLegPointsArray = new List<IPoint>();

                    pp = profilePointsArray[profilePointsArray.Count - 1];
                    missedLegPointsArray = new List<IPoint>();
                    missedLegPointsArray.Add(pp);
                    pp = new PointClass { X = pp.X - 2.5 * profileDirection, Y = pp.Y + 0.5 };
                    missedLegPointsArray.Add(pp);

                    var lineprof_MissedLeg = AranSupport.AnnotationUtil.GetPolylineElement_Cartographic(missedLegPointsArray, 0, 1.5, true, false);

                    double legMissedCourse = finalLeg != null ? Math.Round(finalLeg.Course.Value, 0) : Math.Round(missedAppLeg.Course.Value, 0);
                    string legCourseTxt = "";

                    if (finalLeg != null && finalLeg.CourseType != CodeCourse.MAG_TRACK)
                    {
                        legMissedCourse = Arp.MagneticVariation.HasValue ? Math.Round(legMissedCourse - Arp.MagneticVariation.Value) : legMissedCourse;
                        if (legMissedCourse < 0) legMissedCourse += 360;
                    }

                    legCourseTxt = legMissedCourse.ToString();
                    if (legCourseTxt.Length < 3) legCourseTxt = "0" + legCourseTxt;
                    if (legCourseTxt.Length < 3) legCourseTxt = "0" + legCourseTxt;
                    legCourseTxt = legCourseTxt + "°";

                    var txtMissedCourse = AranSupport.AnnotationUtil.CreateFreeTextElement(lineprof_MissedLeg.Geometry, legCourseTxt, profileDirection > 0, false, 8, horizontalAlignment.Center, verticalAlignment.Bottom, fillStyle.fSSolid);

                    resultGrpEl.AddElement(txtMissedCourse);
                    resultGrpEl.AddElement((IElement)lineprof_MissedLeg);
                }

                #endregion


                var line = AranSupport.AnnotationUtil.GetPolylineElement_Simle(profilePointsArray, 0, esriSimpleLineStyle.esriSLSSolid, 1);
                resultGrpEl.AddElement(line);

                if (IAFflag)
                {
                    IPoint fafPp = profilePointsArray[1];

                    IPoint omPp = new Point { X = fafPp.X + 1, Y = fafPp.Y };

                    

                    List<IPoint> omList = new List<IPoint>();
                    omList.Add(fafPp);
                    omList.Add(omPp);
                    var lineOM = AranSupport.AnnotationUtil.GetPolylineElement_Simle(omList, 0, esriSimpleLineStyle.esriSLSSolid, 1);
                    resultGrpEl.AddElement(lineOM);

                }

                #endregion


                #region Заполнение статического текста

                IGroupElement instructionGroup = (IGroupElement)Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "Sigma_MissedInstruction");
                if (instructionGroup != null)
                {
                    for (int elIndx = 0; elIndx < instructionGroup.ElementCount; elIndx++)
                    {
                        IElement instrEl = instructionGroup.get_Element(elIndx);
                        IElementProperties docEl2 = (IElementProperties)instrEl;
                        if (docEl2.Name.StartsWith("Instruction"))
                        {
                            ((ITextElement)instrEl).Text = _selectedProcInstruction;
                            break;
                        }
                    }
                }
                #endregion

                

                docElementProperties2 = (IElementProperties)((IElement)resultGrpEl);
                docElementProperties2.Name = "Sigma_Profile";
                SigmaHookHelper.ActiveView.GraphicsContainer.AddElement((IElement)resultGrpEl, 0);

                IPoint resultGrpElAncor = ((IElement)resultGrpEl).Geometry.Envelope.UpperLeft;
                ITransform2D mv = (ITransform2D)resultGrpEl;
                mv.Move(profeliGroupAncor.X - resultGrpElAncor.X, 0);

                #region заполнение таблиц

                IPolygon _PrevTableEnvelope = new PolygonClass();
                int TimeRow = _mainNavaid != null && (_mainNavaid.CodeNavaidSystemType == NavaidSystemType.ILS || _mainNavaid.CodeNavaidSystemType == NavaidSystemType.ILS_DME) ? 3 : 2;

                #region создать линию профиля для заполнения distance table
                
                Polyline ProfilePolyLn = new PolylineClass();

                /////

                //добавляем первую точку на высоте finalLeg, чтобы можно было гарантировано заполнять таблицу дистанций
                IPoint d1 = new PointClass();
                d1.X = finalLeg.ConvertValueToMeter(40000, "M");
                d1.Y = finalLeg.ConvertValueToMeter(finalLeg.UpperLimitAltitude.Value, finalLeg.UpperLimitAltitudeUOM.ToString());

                (ProfilePolyLn as IPointCollection).AddPoint(d1);

                ////


                foreach (ApproachDistance appDistance in _profile.ApproachDistancetable)
                {
                    string MP = appDistance.StartingMeasurementPoint.ToString();
                    if (MP.StartsWith("THLD")) MP = appDistance.EndingMeasurementPoint.ToString();

                    IPoint profPnt = new PointClass();
                    profPnt.X = finalLeg.ConvertValueToMeter(appDistance.Distance.Value, appDistance.DistanceUOM.ToString());
                    ApproachAltitude prAlt = (from element in _profile.ApproachAltitudeTable where element.MeasurementPoint.ToString().StartsWith(MP) select element).FirstOrDefault();
                    profPnt.Y = finalLeg.ConvertValueToMeter(prAlt.Altitude.Value, prAlt.AltitudeUOM.ToString());

                    (ProfilePolyLn as IPointCollection).AddPoint(profPnt);

                }

                #endregion

                #region SpeedTable

                IElement tbl = CreateSigmaTable(SigmaHookHelper, "Sigma_SpeedTable", "SigmaTable_Speed",8, TimeRow, 0, tableBuildDestination.RightUp, ref _PrevTableEnvelope);
                if (tbl != null)
                {
                    double FafMaptDist = 0;
                    string fafmap = "FAF-MAPT";


                    RouteSegment rsFaf = ProfileSegmentList.FindAll(r => r.StartPoint.SegmentPointDesignator.StartsWith("FAF")).FirstOrDefault();
                    if (rsFaf == null)
                    {
                        fafmap = "PFAF-MAPT";
                        rsFaf = ProfileSegmentList.FindAll(r => r.StartPoint.SegmentPointDesignator.StartsWith("PFAF")).FirstOrDefault();
                    }
                    RouteSegment rsMap = ProfileSegmentList.FindAll(r => r.StartPoint.SegmentPointDesignator.StartsWith("MAP")).FirstOrDefault();

                    if (rsFaf != null && rsMap != null)
                    {
                        FafMaptDist = rsFaf.ValLen.Value - rsMap.ValLen.Value;
                    }


                    if (FafMaptDist > 0)
                    {
                        FillSpeedTable(tbl, FafMaptDist, VA, TimeRow == 3, fafmap);
                        SigmaHookHelper.ActiveView.GraphicsContainer.AddElement(tbl, 0);
                    }


                }

                #endregion

                #region Distance table

                // рассчитать кол-во столбцов в таблице

                ApproachDistance faf_thr = _profile.ApproachDistancetable[0]; // первая колонка расстояние FAF - THR
                ApproachDistance map_thr = _profile.ApproachDistancetable[_profile.ApproachDistancetable.Count - 1]; // последняя колонка расстояние MAP - THR
                ApproachAltitude faf_alt = _profile.ApproachAltitudeTable[0];

                double faf_thrDist = ArenaStaticProc.UomTransformation(faf_thr.DistanceUOM.ToString(), DistUom.ToString(), faf_thr.Distance.Value, 1);
                double map_thrDist = ArenaStaticProc.UomTransformation(map_thr.DistanceUOM.ToString(), DistUom.ToString(), map_thr.Distance.Value, 1);

                double FAFelev = ArenaStaticProc.UomTransformation(faf_alt.AltitudeUOM.ToString(), VertUom.ToString(), faf_alt.Altitude.Value, 1);

                int c = (int)(faf_thrDist) - ((int)map_thrDist);
               
                int distTableColCount = DistUom == UOM_DIST_HORZ.NM ? c : ((int)(c / 2)) + 1;
                int dist_step = DistUom == UOM_DIST_HORZ.NM ? 1 : 2;
                double navX = _mainNavaid!=null ? ArenaStaticProc.UomTransformation("M", DistUom.ToString(), _mainNavaid.X.Value,10) : 0;
                double arpNavX = ItsPrecision && arpNav!=null? ArenaStaticProc.UomTransformation("M", DistUom.ToString(), arpNav.X.Value, 10) : 0;


                SigmaIACProfileTable distTable = new SigmaIACProfileTable(distTableColCount + 1, 5); //неточный заход
                List<object> tblR_Nav = new List<object>();
                List<object> tblR_DistTHR = new List<object>();
                List<object> tblR_DistArpNav = new List<object>();
                List<object> tblR_Height = new List<object>();
                List<object> tblR_alt = new List<object>();


                DME dmeNav = _mainNavaid != null ? (PDM.DME)(from element in _mainNavaid.Components where element.PDM_Type == PDM_ENUM.DME select element).FirstOrDefault() : null;
                

                tblR_Nav.Add(dmeNav!=null ? dmeNav.GetObjectLabel() :  _mainNavaid.GetObjectLabel());
                if (ItsPrecision && arpNav!=null)
                {
                    dmeNav = (PDM.DME)(from element in arpNav.Components where element.PDM_Type == PDM_ENUM.DME select element).FirstOrDefault();
                    tblR_DistArpNav.Add(dmeNav != null ? dmeNav.GetObjectLabel() : arpNav.GetObjectLabel());
                }
                else
                    tblR_DistArpNav.Add("-");

                tblR_DistTHR.Add("DIST THR");
                tblR_Height.Add("HEIGHT");
                tblR_alt.Add("ALTITUDE");



                for (int cellIndx = 0; cellIndx < distTableColCount; cellIndx++)
                {
                    double curDist = (faf_thrDist - dist_step * cellIndx);
                    double curDistToThr = Math.Round(Math.Truncate(curDist - navX) + navX, 1);


                    #region первая строка DIST Navaid

                    double dNav = Math.Truncate(curDist - navX - ArenaStaticProc.UomTransformation("M", DistUom.ToString(), dispValue));
                    if (dNav < 0) dNav = dNav * -1;
                    tblR_Nav.Add(dNav);

                    #endregion

                    #region вторая строка DIST ARP NAV

                    if (ItsPrecision && arpNav!=null)
                    {
                        tblR_DistArpNav.Add(Math.Round(curDistToThr - arpNavX,1));
                    }
                    else
                        tblR_DistArpNav.Add("-");

                    #endregion

                    #region третья строка DIST THR

                    tblR_DistTHR.Add(curDistToThr);
                    
                    #endregion

                    #region четвертая - пятая строка Altitude - Height

                    Polyline altLine = new PolylineClass();
                    altLine.AddPoint(new PointClass { X = finalLeg.ConvertValueToMeter(curDistToThr, DistUom.ToString()), Y = -400 });
                    altLine.AddPoint(new PointClass { X = finalLeg.ConvertValueToMeter(curDistToThr, DistUom.ToString()), Y = 800000 });

                    ITopologicalOperator3 topOperator = (ITopologicalOperator3)ProfilePolyLn;
                    topOperator.IsKnownSimple_2 = false;
                    topOperator.Simplify();

                    IGeometry resultGeom = (IGeometry)topOperator.Intersect((IGeometry)altLine, esriGeometryDimension.esriGeometry0Dimension);

                    if (resultGeom != null && ((IPointCollection)resultGeom).PointCount > 0)
                    {
                        double _alt = ArenaStaticProc.UomTransformation("M", VertUom.ToString(), ((IPointCollection)resultGeom).get_Point(0).Y, 1);
                        double _height = VertUom == UOM_DIST_VERT.M ? _alt - rdnElevInMeter : _alt - rdnElevInFt;

                        tblR_Height.Add("(" + Math.Truncate(_height).ToString()+")");
                        tblR_alt.Add(Math.Truncate(_alt));
                    }

                    #endregion
                }

                distTable.fillRow(0, tblR_Nav);
                distTable.fillRow(1, tblR_DistArpNav);
                distTable.fillRow(2, tblR_DistTHR);
                distTable.fillRow(3, tblR_alt);
                distTable.fillRow(4, tblR_Height);
                

                if (!ItsPrecision) distTable.deleteRow(1);

                if (ItsPrecision && _mainNavaid.CodeNavaidSystemType == NavaidSystemType.ILS)
                {
                    distTable.deleteRow(0);
                    distTable.deleteRow(0);
                }

                IElement tblDistance = CreateSigmaTable(SigmaHookHelper, "Sigma_DistTable", "SigmaTable_Distance", distTable.TableColCount, distTable.TableRowCount, TimeRow, tableBuildDestination.RightUp, ref _PrevTableEnvelope);
                if (tblDistance != null)
                {
                    Fill_Table(tblDistance, distTable);

                    SigmaHookHelper.ActiveView.GraphicsContainer.AddElement(tblDistance, 0);

                    
                }

              
                #endregion

                #region OCH/CIRCLING TABLE

                IGraphicsContainer graphics = (IGraphicsContainer)SigmaHookHelper.PageLayout;
                IFrameElement frameElement = graphics.FindFrame(SigmaHookHelper.ActiveView.FocusMap);
                IMapFrame mapFrame = (IMapFrame)frameElement;
                IElement mapElement = (IElement)mapFrame;
                IEnvelope frameEnv = mapElement.Geometry.Envelope;

             
                IPolygon _newTableEnvelope = new PolygonClass();
                ((IPointCollection)_PrevTableEnvelope).AddPoint(frameEnv.LowerLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(_PrevTableEnvelope.Envelope.LowerLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(_PrevTableEnvelope.Envelope.UpperLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(frameEnv.UpperLeft);


                IElement HeaderTable = CreateSigmaTable(SigmaHookHelper, "Sigma_OCHHeader", "SigmaTable_OCHHeader", 5, 1, 0, tableBuildDestination.LeftDoun, ref _newTableEnvelope);
                Set_TableCell_ByName((IGroupElement3)HeaderTable, "Cell_1_0", "A");
                Set_TableCell_ByName((IGroupElement3)HeaderTable, "Cell_2_0", "B");
                Set_TableCell_ByName((IGroupElement3)HeaderTable, "Cell_3_0", "C");
                Set_TableCell_ByName((IGroupElement3)HeaderTable, "Cell_4_0", "D");

                _newTableEnvelope = new PolygonClass();
                ((IPointCollection)_PrevTableEnvelope).AddPoint(frameEnv.LowerLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(_PrevTableEnvelope.Envelope.LowerLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(_PrevTableEnvelope.Envelope.UpperLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(frameEnv.UpperLeft);

                tbl = CreateSigmaTable(SigmaHookHelper, "Sigma_OCHTable", "SigmaTable_OCH", 5, 3, 0, tableBuildDestination.LeftDoun, ref _newTableEnvelope, 1, 1);




                InstrumentApproachProcedure pr = (InstrumentApproachProcedure)DataCash.GetPDMObject(_selectedProcID, PDM_ENUM.InstrumentApproachProcedure);
                List<ProcedureLeg> _FinalLegList = new List<ProcedureLeg>();
                foreach (var trans in pr.Transitions)
                {
                    if (trans.Legs != null && trans.Legs.Count > 0)
                    {
                        var flList = trans.Legs.Where(l => l.PDM_Type == PDM_ENUM.FinalLeg).ToList();
                        if (flList != null && flList.Count > 0) _FinalLegList.AddRange(flList);
                    }
                }

                int rIndx = 1;
                foreach (var _finalLegOCH in _FinalLegList)
                {

                    string pres = ((FinalLeg)_finalLegOCH).LandingSystemCategory.ToString();
                    pres = pres.Replace("_PRECISION_", " ");
                    pres = pres.Replace("_", " ");
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_" + rIndx.ToString(), pres);

                    if (((FinalLeg)_finalLegOCH).Condition_Minima != null && ((FinalLeg)_finalLegOCH).Condition_Minima.Count > 0)
                    {
                        
                        foreach (ApproachCondition _minima in ((FinalLeg)_finalLegOCH).Condition_Minima)
                        {
                           
                            if (_minima.FinalApproachPath == CodeMinimaFinalApproachPath.STRAIGHT_IN)
                            {
                                if (((FinalLeg)_finalLegOCH).Condition_Minima[0].AircraftCategory != null)
                                {

                                    foreach (var airCat in _minima.AircraftCategory)
                                    {

                                        string oca = ArenaStaticProc.UomTransformation(_minima.MinAltitudeUOM.ToString(), VertUom.ToString(), _minima.MinAltitude.Value).ToString();
                                        string och = ArenaStaticProc.UomTransformation(_minima.MinHeightUOM.ToString(), VertUom.ToString(), _minima.MinHeight.Value).ToString();

                                        if (_minima.MinAltitudeCode != CodeMinimumAltitude.OCA) oca = "";
                                        if (_minima.MinHeightCode != CodeMinimumHeight.OCH) och = "";

                                        switch (airCat)
                                        {
                                            case AircraftCategoryType.A:
                                                Set_TableCell_ByName((IGroupElement3)tbl, "Cell_2_" + rIndx.ToString(), oca + "(" + och + ")");
                                                break;
                                            case AircraftCategoryType.B:
                                                Set_TableCell_ByName((IGroupElement3)tbl, "Cell_3_" + rIndx.ToString(), oca + "(" + och + ")");
                                                break;
                                            case AircraftCategoryType.C:
                                                Set_TableCell_ByName((IGroupElement3)tbl, "Cell_4_" + rIndx.ToString(), oca + "(" + och + ")");
                                                break;
                                            case AircraftCategoryType.D:
                                                Set_TableCell_ByName((IGroupElement3)tbl, "Cell_5_" + rIndx.ToString(), oca + "(" + och + ")");
                                                break;
                                            case AircraftCategoryType.E:
                                                break;
                                            case AircraftCategoryType.H:
                                                break;
                                            case AircraftCategoryType.ALL:
                                                break;
                                            case AircraftCategoryType.OTHER:
                                                break;
                                            default:
                                                break;
                                        }
                                    }



                                }
                            }


                        }
                    }

                    rIndx++;
                    if (rIndx > 3) break;
                }

                _newTableEnvelope = new PolygonClass();
                ((IPointCollection)_PrevTableEnvelope).AddPoint(frameEnv.LowerLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(_PrevTableEnvelope.Envelope.LowerLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(_PrevTableEnvelope.Envelope.UpperLeft);
                ((IPointCollection)_PrevTableEnvelope).AddPoint(frameEnv.UpperLeft);

                IElement CircleTable = CreateSigmaTable(SigmaHookHelper, "Sigma_CirclingTable", "SigmaTable_Circling", 5, 1, 0, tableBuildDestination.LeftDoun, ref _newTableEnvelope);


                FinalLeg _finalLegCircle = (FinalLeg)_FinalLegList[_FinalLegList.Count - 1];
                if (((FinalLeg)_finalLegCircle).Condition_Minima != null && ((FinalLeg)_finalLegCircle).Condition_Minima.Count > 0)
                {
                    foreach (ApproachCondition _minima in ((FinalLeg)_finalLegCircle).Condition_Minima)
                    {
                        if (_minima.FinalApproachPath != CodeMinimaFinalApproachPath.CIRCLING) continue;

                        if (((FinalLeg)_finalLegCircle).Condition_Minima[0].AircraftCategory != null)
                        {

                            foreach (var airCat in _minima.AircraftCategory)
                            {

                                string oca = ArenaStaticProc.UomTransformation(_minima.MinAltitudeUOM.ToString(), VertUom.ToString(), _minima.MinAltitude.Value).ToString();
                                string och = ArenaStaticProc.UomTransformation(_minima.MinHeightUOM.ToString(), VertUom.ToString(), _minima.MinHeight.Value).ToString();

                                if (_minima.MinAltitudeCode != CodeMinimumAltitude.OCA) oca = "";
                                if (_minima.MinHeightCode != CodeMinimumHeight.OCH) och = "";


                                switch (airCat)
                                {
                                    case AircraftCategoryType.A:
                                        Set_TableCell_ByName((IGroupElement3)CircleTable, "Cell_1_0", oca + "(" + och + ")");
                                        break;
                                    case AircraftCategoryType.B:
                                        Set_TableCell_ByName((IGroupElement3)CircleTable, "Cell_2_0", oca + "(" + och + ")");
                                        break;
                                    case AircraftCategoryType.C:
                                        Set_TableCell_ByName((IGroupElement3)CircleTable, "Cell_3_0", oca + "(" + och + ")");
                                        break;
                                    case AircraftCategoryType.D:
                                        Set_TableCell_ByName((IGroupElement3)CircleTable, "Cell_4_0", oca + "(" + och + ")");
                                        break;
                                    case AircraftCategoryType.E:
                                        break;
                                    case AircraftCategoryType.H:
                                        break;
                                    case AircraftCategoryType.ALL:
                                        break;
                                    case AircraftCategoryType.OTHER:
                                        break;
                                    default:
                                        break;
                                }
                            }



                        }
                    }
                }


                ((IGroupElement)tbl).AddElement(HeaderTable);
                ((IGroupElement)tbl).AddElement(Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "Sigma_OCHTable_Column_0_1"));
                ((IGroupElement)tbl).AddElement(CircleTable);
                SigmaHookHelper.ActiveView.GraphicsContainer.AddElement(tbl, 0);


                #endregion

                #region Aerodrome Operating Minima

                _PrevTableEnvelope = new PolygonClass();

                tbl = CreateSigmaTable(SigmaHookHelper, "Sigma_ArptMINIMA_Table", "SigmaArptMINIMA", 5, 4, 0, tableBuildDestination.RightUp, ref _PrevTableEnvelope);
                if (tbl != null && _FinalLegList!=null)
                {
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_0_0", "DA (DH)");
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_0", "A");
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_2_0", "B");
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_3_0", "C");
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_4_0", "D");


                    rIndx = 1;
                    foreach (var _finalLegDA in _FinalLegList)
                    {

                        string pres = ((FinalLeg)_finalLegDA).LandingSystemCategory.ToString();
                        pres = pres.Replace("_PRECISION_", " ");
                        pres = pres.Replace("_", " ");
                        Set_TableCell_ByName((IGroupElement3)tbl, "Cell_0_" + rIndx.ToString(), pres);

                        if (((FinalLeg)_finalLegDA).Condition_Minima != null && ((FinalLeg)_finalLegDA).Condition_Minima.Count > 0)
                        {

                            foreach (ApproachCondition _minima in ((FinalLeg)_finalLegDA).Condition_Minima)
                            {

                                if (_minima.FinalApproachPath == CodeMinimaFinalApproachPath.STRAIGHT_IN && _minima.MinAltitudeCode == CodeMinimumAltitude.DA)
                                {
                                    if (((FinalLeg)_finalLegDA).Condition_Minima[0].AircraftCategory != null)
                                    {

                                        foreach (var airCat in _minima.AircraftCategory)
                                        {

                                            string oca = ArenaStaticProc.UomTransformation(_minima.MinAltitudeUOM.ToString(), VertUom.ToString(), _minima.MinAltitude.Value).ToString();
                                            string och = ArenaStaticProc.UomTransformation(_minima.MinHeightUOM.ToString(), VertUom.ToString(), _minima.MinHeight.Value).ToString();
                                            string rvr = _minima.MinMandatoryRVR ? "RVR " : "";
                                            rvr = rvr + _minima.MinVisibility.Value.ToString() + _minima.MinVisibilityUOM.ToString().ToLower();
                                            switch (airCat)
                                            {
                                                case AircraftCategoryType.A:
                                                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_" + rIndx.ToString(), oca + "(" + och + ") \r\n" + rvr);
                                                    break;
                                                case AircraftCategoryType.B:
                                                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_2_" + rIndx.ToString(), oca + "(" + och + ") \r\n" + rvr);
                                                    break;
                                                case AircraftCategoryType.C:
                                                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_3_" + rIndx.ToString(), oca + "(" + och + ") \r\n" + rvr);
                                                    break;
                                                case AircraftCategoryType.D:
                                                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_4_" + rIndx.ToString(), oca + "(" + och + ") \r\n" + rvr);
                                                    break;
                                                case AircraftCategoryType.E:
                                                    break;
                                                case AircraftCategoryType.H:
                                                    break;
                                                case AircraftCategoryType.ALL:
                                                    break;
                                                case AircraftCategoryType.OTHER:
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }



                                    }
                                }


                            }
                        }

                        rIndx++;
                        if (rIndx > 3) break;
                    }


                    SigmaHookHelper.ActiveView.GraphicsContainer.AddElement(tbl, 0);
                }
                #endregion


                #endregion


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace.ToString());
            }

        }

        public static void CreateEOSIdtable(IHookHelper SigmaHookHelper, List<PDMObject> selectedProc, IMap FocusMap, ISpatialReference pSpatialReference, UOM_DIST_VERT VertUom, UOM_DIST_HORZ DistUom)
        {
            IElement tbl = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, "SIGMA_EOSIDTable");

            if (tbl == null) return;

            ITransform2D _move = tbl as ITransform2D;

            IPoint _TopLeftCorner = new PointClass { X = tbl.Geometry.Envelope.UpperLeft.X, Y = tbl.Geometry.Envelope.UpperLeft.Y };
            IPoint _BottomRightCorner = new PointClass { X = tbl.Geometry.Envelope.LowerRight.X, Y = tbl.Geometry.Envelope.LowerRight.Y };
            double w = tbl.Geometry.Envelope.Width;
            double h = tbl.Geometry.Envelope.Height;

            int Indx = 0;
            IGroupElement tblGrp = new GroupElementClass();
            foreach (var item in selectedProc) 
            {
                
                IClone tbl_clone = (tbl as IClone).Clone();
                Set_TableCell_ByName((IGroupElement3)tbl_clone, "Cell_Header", ((Procedure)item).ProcedureIdentifier);

                IElementProperties3 elProp = (IElementProperties3)tbl_clone;
                elProp.Name = "SIGMA_"+ ((Procedure)item).ProcedureIdentifier;


                _move = tbl_clone as ITransform2D;
                _move.Move(0, (_TopLeftCorner.Y + h * Indx) - _BottomRightCorner.Y);


                tblGrp.AddElement((IElement)tbl_clone);

                Indx++;
            }


            //_move = tblGrp as ITransform2D;
            //_move.Move(0, _Anchor.Y);

            SigmaHookHelper.ActiveView.GraphicsContainer.AddElement((IElement)tblGrp, 0);
            SigmaHookHelper.ActiveView.GraphicsContainer.DeleteElement(tbl);


        }

        private static void Fill_Table(IElement IElementTable, SigmaIACProfileTable SigmaTable)
        {
            for (int rowIndx = 0; rowIndx < SigmaTable.TableRowCount; rowIndx++) 
            {
                SigmaProfileTableRow row = SigmaTable.Content[rowIndx];
                for (int colIndx = 0; colIndx < SigmaTable.TableColCount; colIndx++)
                {
                    string cellAdress = "Cell_" + colIndx.ToString() + "_" + rowIndx.ToString();
                    Set_TableCell_ByName((IGroupElement3)IElementTable, cellAdress, row.ProfileRowCell[colIndx]);
                }
            }
        }

        private static RouteSegment GetIAF_First(List<ProcedureLeg> selectedLegs, RouteSegment IF_Segment, IMap focusMap, ISpatialReference spRef, RunwayDirection selRwyDir, double Bearing)
        {
            

            RouteSegment res =null;

            RouteSegmentPoint IAF_PNT = null;
            var _firstLeg = (from element in selectedLegs
                             where (element != null)
                                    && (element is ProcedureLeg)
                                    && (((ProcedureLeg)element).LegSpecialization != SegmentLegSpecialization.FinalLeg)
                                    && (((ProcedureLeg)element).LegSpecialization != SegmentLegSpecialization.MissedApproachLeg)
                                 && (element.StartPoint != null)
                                 && (element.StartPoint.PointRole.HasValue)
                                 && (element.StartPoint.PointRole.Value == ProcedureFixRoleType.IAF)
                             select element).FirstOrDefault();

            if (_firstLeg != null)
            {
                IAF_PNT= new RouteSegmentPoint();
                //IAF_PNT.Elev = _firstLeg.LowerLimitAltitude.HasValue ? _firstLeg.ConvertValueToMeter(_firstLeg.LowerLimitAltitude, _firstLeg.LowerLimitAltitudeUOM.ToString()) : _firstLeg.ConvertValueToMeter(_firstLeg.UpperLimitAltitude, _firstLeg.UpperLimitAltitudeUOM.ToString());
                IAF_PNT.Elev = _firstLeg.UpperLimitAltitude.HasValue ? _firstLeg.ConvertValueToMeter(_firstLeg.UpperLimitAltitude, _firstLeg.UpperLimitAltitudeUOM.ToString()) : _firstLeg.ConvertValueToMeter(_firstLeg.LowerLimitAltitude, _firstLeg.LowerLimitAltitudeUOM.ToString());
                IAF_PNT.SegmentPointDesignator = "IAF";

                double IAFdist = calcDistFromRwy(_firstLeg.StartPoint, focusMap, spRef, selRwyDir, Bearing);

                res = new RouteSegment { StartPoint = IAF_PNT, EndPoint = IF_Segment.StartPoint, ValLen = IAFdist, Elev_UOM = UOM_DIST_VERT.M };
                
                res.ID_Route = "ignore";
            }



            return res;
        }


        private static RouteSegment GetIAF_Second(RouteSegment IF_Segment, double HorDistValue = 9000)
        {
            RouteSegment res = null;
            RouteSegmentPoint FAF_PNT = null;

            FAF_PNT = new RouteSegmentPoint();
            FAF_PNT.Elev = IF_Segment.ConvertValueToMeter(IF_Segment.StartPoint.Elev, IF_Segment.StartPoint.Elev_UOM.ToString());
            FAF_PNT.SegmentPointDesignator = "OM";

            res = new RouteSegment { StartPoint = IF_Segment.StartPoint, EndPoint = FAF_PNT, ValLen = IF_Segment.ValLen - HorDistValue, Elev_UOM = UOM_DIST_VERT.M };
            //res = new RouteSegment { StartPoint = IAF_PNT, EndPoint = IF_Segment.StartPoint, ValLen = IF_Segment.ValLen + HorDistValue, Elev_UOM = UOM_DIST_VERT.M };

            res.ID_Route = "ignore";


            return res;
        }

        private static IPoint HorPart_pointAfterSDF(List<ProcedureLeg> selectedLegs, FinalProfile _profile)
        {
            try
            {
                var _FinalLegList = (from element in selectedLegs
                                     where (element != null)
                                            && (element is FinalLeg)
                                         && (element.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                                     orderby element.SeqNumberARINC
                                     select element).ToList();

                FinalLeg _finalLeg = _FinalLegList.Count > 1 ? (FinalLeg)_FinalLegList[1] : (FinalLeg)_FinalLegList[0];


                IPoint pointAfterSDF = null;

                double Dist = 0;
                double H_FAF_SDF = 0;
                double H_MAPT = 0;
                double DIST_THR = 0;
                ApproachAltitude apAlt;
                ApproachDistance apDist;
                double tgVA = Math.Tan(_finalLeg.VerticalAngle.Value * Math.PI /180) * -1;

                apAlt = _profile.ApproachAltitudeTable.FindAll(r => r.MeasurementPoint == CodeProcedureDistance.MAP).FirstOrDefault(); // Mapt
                H_MAPT = _finalLeg.ConvertValueToMeter(apAlt.Altitude.Value, apAlt.AltitudeUOM.ToString()); //высота Mapt

                if (_FinalLegList.Count > 1)  // with SDF
                {
                    apAlt = _profile.ApproachAltitudeTable.FindAll(r => r.MeasurementPoint == CodeProcedureDistance.OTHER_SDF).FirstOrDefault();
                    apDist = _profile.ApproachDistancetable[1];
                    var apDist2 = _profile.ApproachDistancetable[_profile.ApproachDistancetable.Count - 1];
                    Dist = apDist.Distance.Value - apDist2.Distance.Value;
                    DIST_THR = _finalLeg.ConvertValueToMeter(apDist.Distance.Value, apDist.DistanceUOM.ToString());

                }
                else
                {
                    apAlt = _profile.ApproachAltitudeTable.FindAll(r => r.MeasurementPoint == CodeProcedureDistance.FAF).FirstOrDefault();
                    apDist = _profile.ApproachDistancetable[0];
                    var apDist2 = _profile.ApproachDistancetable[_profile.ApproachDistancetable.Count - 1];
                    Dist = apDist.Distance.Value - apDist2.Distance.Value;
                    DIST_THR = _finalLeg.ConvertValueToMeter(apDist.Distance.Value, apDist.DistanceUOM.ToString());

                }

                H_FAF_SDF = _finalLeg.ConvertValueToMeter(apAlt.Altitude.Value, apAlt.AltitudeUOM.ToString());
                Dist = _finalLeg.ConvertValueToMeter(Dist, apDist.DistanceUOM.ToString());

                if (tgVA > (H_FAF_SDF - H_MAPT) / Dist)
                {
                    pointAfterSDF = new Point { X = DIST_THR - (H_FAF_SDF - H_MAPT) / tgVA , Y = H_MAPT };
                    
                }

               
                return pointAfterSDF;
            }
            catch
            {
                return null;
            }
        }

        private static IPoint HorPart_pointAfterFAF(List<ProcedureLeg> selectedLegs, FinalProfile _profile)
        {
            try
            {
                var _FinalLegList = (from element in selectedLegs
                                     where (element != null)
                                            && (element is FinalLeg)
                                         && (element.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                                     orderby element.SeqNumberARINC
                                     select element).ToList();

                FinalLeg _finalLeg = (FinalLeg)_FinalLegList[0];


                IPoint pointAfterFAF = null;

                double Dist_FAF_SDF = 0;
                
                double tgVA = Math.Tan(_finalLeg.VerticalAngle.Value * Math.PI / 180);
                if (tgVA < 0) tgVA = tgVA * -1;

                ApproachAltitude apAlt = _profile.ApproachAltitudeTable.FindAll(r => r.MeasurementPoint == CodeProcedureDistance.FAF).FirstOrDefault(); // FAF
                double H_FAF = _finalLeg.ConvertValueToMeter(apAlt.Altitude.Value, apAlt.AltitudeUOM.ToString()); //

                apAlt = _profile.ApproachAltitudeTable.FindAll(r => r.MeasurementPoint == CodeProcedureDistance.OTHER_SDF).FirstOrDefault(); // SDF
                

                if (_FinalLegList.Count > 1 && apAlt!=null)  // with SDF
                {
                    double H_SDF = _finalLeg.ConvertValueToMeter(apAlt.Altitude.Value, apAlt.AltitudeUOM.ToString()); //
                    ApproachDistance apDistFAF_THR = _profile.ApproachDistancetable[0]; // DIST FAF - THR
                    ApproachDistance apDistSDF_THR = _profile.ApproachDistancetable[1]; // DIST SDF - THR
                    double Dist_FAF_THR = _finalLeg.ConvertValueToMeter(apDistFAF_THR.Distance.Value, apDistFAF_THR.DistanceUOM.ToString());

                    Dist_FAF_SDF = apDistFAF_THR.Distance.Value - apDistSDF_THR.Distance.Value;
                    Dist_FAF_SDF = _finalLeg.ConvertValueToMeter(Dist_FAF_SDF, apDistFAF_THR.DistanceUOM.ToString());

                    if (tgVA > (H_FAF - H_SDF) / Dist_FAF_SDF)
                    {
                        pointAfterFAF = new Point { X = Dist_FAF_THR - (H_FAF - H_SDF) / tgVA, Y = H_SDF };

                    }

                }
               

                return pointAfterFAF;
            }
            catch
            {
                return null;
            }
        }


        private static List<PDMObject> GetNavaidsList(List<ProcedureLeg> selectedLegs, RunwayDirection selRwyDir, double Bearing, IMap FocusMap, ISpatialReference pSpatialReference)
        {

            List<PDMObject> resList = new List<PDMObject>();
            ProcedureFixRoleType[] FixRoleTypeArray = { ProcedureFixRoleType.IF, ProcedureFixRoleType.IF_IAF, ProcedureFixRoleType.FAF, ProcedureFixRoleType.MAPT, ProcedureFixRoleType.SDF, ProcedureFixRoleType.IAF };


            foreach (ProcedureLeg leg in selectedLegs)
            {
                
                /// Перебираем леги. 
                /// надо получить список всех Navaids, которые используются в Angle/Distance Indication
                /// включаем в список сами точки start - end легов, если они Navaids
                /// надо получить список всех StartPoint - EndPoint, которые являются IF,IF_IAF,FAF,FAP,MAPT и IAF (последний если указан флаг об использовании начального участка)
                /// переберая леги  - пропускаем MissedApproach 

                //nav = null;
                PDMObject nav = null;
                double distToRwyLine = 0;
                double maxDist_M = 55560; // 30 NM

                if (leg.LegSpecialization == SegmentLegSpecialization.InitialLeg && leg.LegTypeARINC.ToString().StartsWith("H"))
                {
                    

                    continue;
                }

                if (leg.StartPoint != null && leg.StartPoint.PointRole.HasValue   && (leg.StartPoint.PointRole == ProcedureFixRoleType.IF || leg.StartPoint.PointRole == ProcedureFixRoleType.FAF || leg.StartPoint.PointRole == ProcedureFixRoleType.SDF))
                {

                    #region StartPoint


                    nav = leg.StartPoint;


                    if (leg.StartPoint.PointChoice == PointChoice.Navaid )
                    {
                        nav = DataCash.GetNavaidByID(leg.StartPoint.PointChoiceID);

                        if (nav != null && resList.IndexOf(nav) <0)
                        {
                            nav.Elev = leg.LowerLimitAltitude;
                            nav.Elev_UOM = leg.LowerLimitAltitudeUOM;

                            distToRwyLine = calcDistFromRwy(nav, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            nav.X = distToRwyLine;
                            if (distToRwyLine < maxDist_M)
                            {
                                resList.Add(nav);
                            }

                            nav.SourceDetail = "PointInTrack";
                        }
                    }

                    if (leg.StartPoint.PointFacilityMakeUp != null && leg.StartPoint.PointFacilityMakeUp.AngleIndication != null)
                    {

                        nav = DataCash.GetNavaidByID(leg.StartPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID);

                        if (nav != null && resList.IndexOf(nav)<0)
                        {
                            nav.Elev = leg.LowerLimitAltitude;
                            nav.Elev_UOM = leg.LowerLimitAltitudeUOM;

                            distToRwyLine = calcDistFromRwy(nav, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            nav.X = distToRwyLine;
                            if (distToRwyLine < maxDist_M)
                            {
                                resList.Add(nav);
                            }
                        }

                    }

                    if (leg.StartPoint.PointFacilityMakeUp != null && leg.StartPoint.PointFacilityMakeUp.DistanceIndication != null)
                    {
                        nav = DataCash.GetNavaidByID(leg.StartPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);
                        if (nav != null && resList.IndexOf(nav) < 0)
                        {
                            nav.Elev = leg.LowerLimitAltitude;
                            nav.Elev_UOM = leg.LowerLimitAltitudeUOM;

                            distToRwyLine = calcDistFromRwy(nav, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            nav.X = distToRwyLine;
                            if (distToRwyLine < maxDist_M)
                            {
                                resList.Add(nav);
                            }
                        }
                    }

                    #endregion
                }

                if (leg.EndPoint != null && leg.EndPoint.PointRole.HasValue && (leg.EndPoint.PointRole == ProcedureFixRoleType.IF || leg.EndPoint.PointRole == ProcedureFixRoleType.FAF || leg.EndPoint.PointRole == ProcedureFixRoleType.SDF))
                {
                    #region EndPoint

                    nav = leg.EndPoint;

                    if (leg.EndPoint.PointChoice == PointChoice.Navaid )
                    {

                        nav = DataCash.GetNavaidByID(leg.EndPoint.PointChoiceID);

                        if (nav != null && resList.IndexOf(nav)<0 && resList.IndexOf(leg.EndPoint) <0)
                        {
                            nav.Elev = leg.LowerLimitAltitude;
                            nav.Elev_UOM = leg.LowerLimitAltitudeUOM;

                            distToRwyLine = calcDistFromRwy(nav, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            nav.X = distToRwyLine;
                            if (distToRwyLine < maxDist_M) resList.Add(nav);


                        }


                    }

                    if (leg.EndPoint.PointFacilityMakeUp != null && leg.EndPoint.PointFacilityMakeUp.AngleIndication != null)
                    {
                        nav = DataCash.GetNavaidByID(leg.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID);



                        if (nav != null && resList.IndexOf(nav)<0 && resList.IndexOf(leg.EndPoint)<0)
                        {
                            nav.Elev = leg.LowerLimitAltitude;
                            nav.Elev_UOM = leg.LowerLimitAltitudeUOM;

                            distToRwyLine = calcDistFromRwy(nav, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            nav.X = distToRwyLine;
                            if (distToRwyLine < maxDist_M) resList.Add(nav);
                        }

                    }

                    if (leg.EndPoint.PointFacilityMakeUp != null && leg.EndPoint.PointFacilityMakeUp.DistanceIndication != null)
                    {
                        nav = DataCash.GetNavaidByID(leg.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);

                        if (nav != null && resList.IndexOf(nav) <0)
                        {
                            nav.Elev = leg.LowerLimitAltitude;
                            nav.Elev_UOM = leg.LowerLimitAltitudeUOM;

                            distToRwyLine = calcDistFromRwy(nav, FocusMap, pSpatialReference, selRwyDir, Bearing);
                            nav.X = distToRwyLine;
                            if (distToRwyLine < maxDist_M) resList.Add(nav);
                        }
                    }

                   

                    

                    #endregion
                }

               

            }

            return resList;
        }

        private static MissaedApproachLeg GetMissedApproachLeg(List<ProcedureLeg> selectedLegs)
        {
            var _MissedApproachLegLeg = (from element in selectedLegs
                             where (element != null)
                                    && (element is MissaedApproachLeg)
                                 && (element.LegSpecialization == SegmentLegSpecialization.MissedApproachLeg)
                             select element).FirstOrDefault();

            return (MissaedApproachLeg)_MissedApproachLegLeg;
        }

        private static RouteSegment GetFirstSegment_IF(List<ProcedureLeg> selectedLegs, RouteSegment fafSegmnen, IMap focusMap, ISpatialReference spRef, RunwayDirection selRwyDir, double Bearing)
        {
           

            RouteSegment res =null;

            RouteSegmentPoint IF_PNT = null;
            var _firstLeg = (from element in selectedLegs
                             where (element != null)
                                    && (element is ProcedureLeg)
                                 && (element.StartPoint != null)
                                 && (element.StartPoint.PointRole.HasValue)
                                 && (element.StartPoint.PointRole.Value == ProcedureFixRoleType.IF)
                             select element).FirstOrDefault();

            if (_firstLeg != null)
            {
                IF_PNT= new RouteSegmentPoint();
                IF_PNT.Elev = _firstLeg.ConvertValueToMeter(_firstLeg.UpperLimitAltitude, _firstLeg.UpperLimitAltitudeUOM.ToString());
                IF_PNT.SegmentPointDesignator = "IF";

                
                if (_firstLeg.StartPoint.PointFacilityMakeUp != null && _firstLeg.StartPoint.PointFacilityMakeUp.DistanceIndication != null && _firstLeg.StartPoint.PointFacilityMakeUp.DistanceIndication.Distance.HasValue)
                {
                    IF_PNT.X = IF_PNT.ConvertValueToMeter(_firstLeg.StartPoint.PointFacilityMakeUp.DistanceIndication.Distance.Value, _firstLeg.StartPoint.PointFacilityMakeUp.DistanceIndication.DistanceUOM.ToString());
                    IF_PNT.Route_LEG_ID = GetFixName( _firstLeg.StartPoint.PointFacilityMakeUp.DistanceIndication.FixID);
                    IF_PNT.Lon = GetNavName(_firstLeg.StartPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);
                }

                double IFdist = calcDistFromRwy(_firstLeg.StartPoint, focusMap, spRef, selRwyDir, Bearing);



                res = new RouteSegment { StartPoint = IF_PNT, EndPoint = fafSegmnen.StartPoint, ValLen = IFdist, Elev_UOM = UOM_DIST_VERT.M };
                res.ID_Route = "ignore";

            }



                return res;
        }

        private static string GetNavName(string SignificantPointID)
        {
            string res = "";
            if ((SignificantPointID != null) && (SignificantPointID.Length > 0))
            {
                var nav = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is NavaidSystem) && 
                           (((NavaidSystem)element).ID.CompareTo(SignificantPointID) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(SignificantPointID) == 0) select element).FirstOrDefault();
                if (nav != null) res = res + ((NavaidSystem)nav).Designator;
                else
                {
                    bool flag = false;
                    var arpList = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is AirportHeliport) && (((AirportHeliport)element).RunwayList != null) select element).ToList();
                    foreach (AirportHeliport arp in arpList)
                    {
                        if (flag) break;
                        foreach (Runway rwy in arp.RunwayList)
                        {
                            if (rwy.RunwayDirectionList == null) continue;
                            foreach (RunwayDirection rdn in rwy.RunwayDirectionList)
                            {
                                if (rdn.Related_NavaidSystem == null) continue;
                                var ILS_NAV = (from element in rdn.Related_NavaidSystem where (element != null) && (element is NavaidSystem) && 
                                               (((NavaidSystem)element).ID.CompareTo(SignificantPointID) == 0 || ((NavaidSystem)element).ID_Feature.CompareTo(SignificantPointID) == 0) select element).FirstOrDefault();
                                if (ILS_NAV != null)
                                {
                                    res = res + ((NavaidSystem)ILS_NAV).Designator;
                                    flag = true;
                                    break;
                                }

                            }
                        }
                    }
                }
            }

            return res;
        }

        private static string GetFixName(string FixID)
        {

            string res = "";
            if ((FixID != null) && (FixID.Length > 0))
            {
                var fix = (from element in DataCash.ProjectEnvironment.Data.PdmObjectList where (element != null) && (element is WayPoint) && (((WayPoint)element).ID.CompareTo(FixID) == 0) select element).FirstOrDefault();
                if (fix != null) res = ((WayPoint)fix).Designator;
            }

            return res;
        

        }

        private static double getGP(List<ProcedureLeg> selectedLegs, ref ProcedureLeg _finalLeg, ref NavaidSystem ils_nav, ref double? ThresholdCrossingHeight)
        {
            double res = 0;

            var _FinalLegList = (from element in selectedLegs
                             where (element != null)
                                    && (element is FinalLeg)
                                 && (element.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                                 orderby element.SeqNumberARINC select element).ToList();

            _finalLeg = _FinalLegList[0];

            double va = ((FinalLeg)_FinalLegList[0]).VerticalAngle.HasValue ? ((FinalLeg)_FinalLegList[0]).VerticalAngle.Value * -1 : 3;
            res = va;

            _finalLeg.VerticalAngle = _finalLeg != null && double.IsNaN(_finalLeg.VerticalAngle.Value) ? va : _finalLeg.VerticalAngle.Value * -1;

            var legPnt = _finalLeg.StartPoint != null ? _finalLeg.StartPoint : _finalLeg.EndPoint;

            if (((FinalLeg)_finalLeg).LandingSystemCategory != CodeApproachGuidance.NON_PRECISION)
            {
                if (legPnt != null && legPnt.PointFacilityMakeUp != null && legPnt.PointFacilityMakeUp.AngleIndication != null && legPnt.PointFacilityMakeUp.AngleIndication.SignificantPointID != null)
                {
                    ils_nav = (NavaidSystem)DataCash.GetNavaidByID(legPnt.PointFacilityMakeUp.AngleIndication.SignificantPointID);
                    if (ils_nav.CodeNavaidSystemType == NavaidSystemType.ILS_DME || ils_nav.CodeNavaidSystemType == NavaidSystemType.ILS || ils_nav.CodeNavaidSystemType == NavaidSystemType.DME)
                    {
                        foreach (var component in ((NavaidSystem)ils_nav).Components)
                        {
                            if (component.PDM_Type != PDM_ENUM.GlidePath) continue;

                            if (((GlidePath)component).Angle.HasValue)
                                res = ((GlidePath)component).Angle.Value;
                            if (((GlidePath)component).ThresholdCrossingHeight.HasValue)
                                ThresholdCrossingHeight = ((GlidePath)component).ThresholdCrossingHeight;
                            break;
                        }

                    }
                }
            }
            else if (((FinalLeg)_finalLeg).LandingSystemCategory == CodeApproachGuidance.NON_PRECISION)
            {
                if (legPnt != null && legPnt.PointFacilityMakeUp != null && legPnt.PointFacilityMakeUp.DistanceIndication != null && legPnt.PointFacilityMakeUp.DistanceIndication.SignificantPointID != null)
                {
                    ils_nav = (NavaidSystem)DataCash.GetNavaidByID(legPnt.PointFacilityMakeUp.DistanceIndication.SignificantPointID);

                }
                else if (_finalLeg.EndPoint != null && _finalLeg.EndPoint.PointFacilityMakeUp != null && _finalLeg.EndPoint.PointFacilityMakeUp.DistanceIndication != null && _finalLeg.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID != null)
                {
                    ils_nav = (NavaidSystem)DataCash.GetNavaidByID(_finalLeg.EndPoint.PointFacilityMakeUp.DistanceIndication.SignificantPointID);

                }
                else if (_finalLeg.EndPoint != null && _finalLeg.EndPoint.PointFacilityMakeUp != null && _finalLeg.EndPoint.PointFacilityMakeUp.AngleIndication != null && _finalLeg.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID != null)
                {
                    ils_nav = (NavaidSystem)DataCash.GetNavaidByID(_finalLeg.EndPoint.PointFacilityMakeUp.AngleIndication.SignificantPointID);

                }

                if (ils_nav == null && _finalLeg.AngleIndication !=null)
                {
                    ils_nav = (NavaidSystem)DataCash.GetNavaidByID(_finalLeg.AngleIndication.SignificantPointID);

                }
                else if (ils_nav == null && _finalLeg.AngleIndication == null && _finalLeg.DistanceIndication !=null )
                {
                    ils_nav = (NavaidSystem)DataCash.GetNavaidByID(_finalLeg.DistanceIndication.SignificantPointID);

                }
                res = _finalLeg.VerticalAngle!=null && _finalLeg.VerticalAngle.HasValue?  _finalLeg.VerticalAngle.Value : 0;
            }



            return res;
        }

        private static double getOCA(List<ProcedureLeg> selectedLegs)
        {
            double res = 0;

            var _FinalLeg = (from element in selectedLegs
                             where (element != null)
                                    && (element is FinalLeg)
                                 && (element.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                                 && (((FinalLeg)element).Condition_Minima !=null)
                                 && (((FinalLeg)element).Condition_Minima.Count >0 )
                             select element).FirstOrDefault();

            if (_FinalLeg != null)
            {
                res =((FinalLeg)_FinalLeg).Condition_Minima[0].MinAltitude.HasValue?
                    _FinalLeg.ConvertValueToMeter(((FinalLeg)_FinalLeg).Condition_Minima[0].MinAltitude.Value, ((FinalLeg)_FinalLeg).Condition_Minima[0].MinAltitudeUOM.ToString()) : 0;
            }

            return res;
        }

        private static double getOCH(List<ProcedureLeg> selectedLegs)
        {
            double res = 0;

            var _FinalLeg = (from element in selectedLegs
                             where (element != null)
                                    && (element is FinalLeg)
                                 && (element.LegSpecialization == SegmentLegSpecialization.FinalLeg)
                                 && (((FinalLeg)element).Condition_Minima != null)
                                 && (((FinalLeg)element).Condition_Minima.Count > 0)
                             select element).FirstOrDefault();

            if (_FinalLeg != null)
            {
                res = ((FinalLeg)_FinalLeg).Condition_Minima[0].MinHeight.HasValue ?
                    _FinalLeg.ConvertValueToMeter(((FinalLeg)_FinalLeg).Condition_Minima[0].MinHeight.Value, ((FinalLeg)_FinalLeg).Condition_Minima[0].MinHeightUOM.ToString()) : 0;
            }

            return res;
        }

        private static List<RouteSegment> GetPointsFromProfile(FinalProfile _profile)
        {
            List<RouteSegment> res = new List<RouteSegment>();

            if (_profile.ApproachAltitudeTable == null || _profile.ApproachAltitudeTable.Count <= 0) return null;
            if (_profile.ApproachDistancetable == null || _profile.ApproachDistancetable.Count <= 0) return null;

            for (int i = 0; i < _profile.ApproachAltitudeTable.Count -1; i++)
            {
              if ( _profile.ApproachAltitudeTable[i].MeasurementPoint.ToString().StartsWith("THLD")) continue;

                RouteSegment seg = new RouteSegment();

                seg.StartPoint = new RouteSegmentPoint();
                seg.StartPoint.Elev = seg.StartPoint.ConvertValueToMeter(_profile.ApproachAltitudeTable[i].Altitude, _profile.ApproachAltitudeTable[i].AltitudeUOM.ToString());
                seg.StartPoint.Elev_UOM = UOM_DIST_VERT.M;
                seg.StartPoint.SegmentPointDesignator = _profile.ApproachAltitudeTable[i].MeasurementPoint.ToString();

                seg.EndPoint = new RouteSegmentPoint();
                seg.EndPoint.Elev = seg.EndPoint.ConvertValueToMeter(_profile.ApproachAltitudeTable[i +1].Altitude, _profile.ApproachAltitudeTable[i +1].AltitudeUOM.ToString());
                seg.EndPoint.Elev_UOM = UOM_DIST_VERT.M;
                seg.EndPoint.SegmentPointDesignator = _profile.ApproachAltitudeTable[i + 1].MeasurementPoint.ToString();

                seg.Elev_UOM = UOM_DIST_VERT.M;

                seg.ID_Route = "";
                
                res.Add(seg);

            }

            if (res.Count <= 0) return null;


            RouteSegment segEnd = new RouteSegment();
            int endI = _profile.ApproachAltitudeTable.Count - 1;
            segEnd.StartPoint = new RouteSegmentPoint();
            segEnd.StartPoint.Elev = segEnd.StartPoint.ConvertValueToMeter(_profile.ApproachAltitudeTable[endI].Altitude, _profile.ApproachAltitudeTable[endI].AltitudeUOM.ToString());
            segEnd.StartPoint.Elev_UOM = UOM_DIST_VERT.M;
            segEnd.StartPoint.SegmentPointDesignator = _profile.ApproachAltitudeTable[endI].MeasurementPoint.ToString();

            segEnd.EndPoint = new RouteSegmentPoint();
            segEnd.EndPoint.Elev = segEnd.EndPoint.ConvertValueToMeter(_profile.ApproachAltitudeTable[endI].Altitude, _profile.ApproachAltitudeTable[endI].AltitudeUOM.ToString());
            segEnd.EndPoint.Elev_UOM = UOM_DIST_VERT.M;
            segEnd.EndPoint.SegmentPointDesignator = "THLD";

            segEnd.Elev_UOM = UOM_DIST_VERT.M;

            res.Add(segEnd);
           

            for (int i = 0; i <= _profile.ApproachDistancetable.Count - 1; i++)
            {
                res[i].ValLen = res[i].ConvertValueToMeter(_profile.ApproachDistancetable[i].Distance, _profile.ApproachDistancetable[i].DistanceUOM.ToString()).Value;
            }


            //res.Remove(res[3]);
            return res;
        }

        private static void SetMax_Dist_Elev(List<RouteSegment> profileSegmentList, double RdnElev_M, ref double maxDistfromRwy, ref double minDistfromRwy, ref double maxElevAboveRwy, ref double minElevAboveRwy)
        {
            double delta = 0;
            foreach (var _seg in profileSegmentList)
            {
                if (_seg.ValLen != null && _seg.ValLen.HasValue && _seg.ValLen.Value < 0) { delta = _seg.ValLen.Value; continue; }
                if (_seg.EndPoint.SegmentPointDesignator.StartsWith("THLD") && _seg.ValLen!=null && _seg.ValLen.HasValue) minDistfromRwy = _seg.ValLen.Value;
                if (_seg.ValLen != null && _seg.ValLen.HasValue && maxDistfromRwy < _seg.ValLen.Value) maxDistfromRwy = _seg.ValLen.Value;

                //maxDistfromRwy = maxDistfromRwy + _seg.ValLen.Value;

                if (maxElevAboveRwy < _seg.StartPoint.Elev.Value) maxElevAboveRwy = _seg.StartPoint.Elev.Value;
                if (maxElevAboveRwy < _seg.EndPoint.Elev.Value) maxElevAboveRwy = _seg.EndPoint.Elev.Value;

                if (minElevAboveRwy > _seg.StartPoint.Elev) minElevAboveRwy = _seg.StartPoint.Elev.Value;
                if (minElevAboveRwy > _seg.EndPoint.Elev) minElevAboveRwy = _seg.EndPoint.Elev.Value;

            }

            minDistfromRwy = minDistfromRwy + delta;
        }

   
        private static void FillSpeedTable(IElement tbl, double DistFaf_Mapt, double GlidePathAngle,bool timeRow = true, string FAF_Dist = "FAF-MAPT", SpeedType uom = SpeedType.KT)
        {

            if (tbl is IGroupElement3)
            {
                DistFaf_Mapt = uom == SpeedType.KT ? DistFaf_Mapt * 0.0005399568 : DistFaf_Mapt / 1000;

                Set_TableCell_ByName((IGroupElement3)tbl, "Cell_0_0", "GS");
                Set_TableCell_ByName((IGroupElement3)tbl, "Cell_0_1", "Rate of descent");
                

                if (uom == SpeedType.KT) Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_0", "Kt"); else Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_0", "km/h");
                if (uom == SpeedType.KT) Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_1", "ft/min"); else Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_1", "m/min");
                if (uom == SpeedType.KT)
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_0_2", FAF_Dist + " " + Math.Round(DistFaf_Mapt, 1).ToString() + " NM"); 
                else 
                    Set_TableCell_ByName((IGroupElement3)tbl, "Cell_0_2", FAF_Dist+" (" + Math.Round(DistFaf_Mapt, 1).ToString() + ") km");

                for (int i = 2; i < 9; i++)
                {
                    //speed row  
                    int val =  80 + 20 * (i - 2);
                    if (uom == SpeedType.KM_H) val = ((int)Math.Round(val * 1.852 / 10.0)) * 10;

                    string adress = "Cell_" + i.ToString() + "_0";
                    Set_TableCell_ByName((IGroupElement3)tbl, adress, val.ToString());


                    //rate of descent row
                    double Rd = val * Math.Atan(GlidePathAngle * Math.PI / 180);
                    Rd = uom == SpeedType.KT ? Rd * 6076 / 60 : Rd * 1000 / 60;
                    double val2 = Math.Round(Rd /10, 0);
                    val2 = val2 * 10;
                    val2 = Math.Round(Math.Round(Rd) - val2) >4 ? val2+10 : val2;
                    adress = "Cell_" + i.ToString() + "_1";
                    Set_TableCell_ByName((IGroupElement3)tbl, adress, Int32.Parse(val2.ToString()).ToString());

                    if (timeRow) // time row
                    {
                        double t = DistFaf_Mapt * 60 / val;
                        var t_min = Math.Truncate(t);
                        var t_sec = (t % 1) * 60;
                        adress = "Cell_" + i.ToString() + "_2";

                        string t_min_txt = Math.Round(t_min)<=9 ? "0" + Math.Round(t_min).ToString() : Math.Round(t_min).ToString();
                        string t_sec_txt = Math.Round(t_sec) <= 9 ? "0" + Math.Round(t_sec).ToString() : Math.Round(t_sec).ToString();
                        Set_TableCell_ByName((IGroupElement3)tbl, adress, t_min_txt + ":" + t_sec_txt);
                       

                        Set_TableCell_ByName((IGroupElement3)tbl, "Cell_1_2", "min:sec");

                    }
                }


            }
        }

        private static double calcDistFromRwy(PDMObject nav, IMap FocusMap, ISpatialReference pSpatialReference, RunwayDirection selRwyDir, double Bearing)
        {
            Utilitys util = new Utilitys();

            IPoint gm = null;


            if (nav.Geo == null) nav.RebuildGeo();
            if (selRwyDir.Geo == null) selRwyDir.RebuildGeo();
            double dirBearing = util.Azt2Direction((IPoint)selRwyDir.Geo, Bearing, FocusMap, pSpatialReference);

            IPoint thrPrjGeo = (IPoint)EsriUtils.ToProject(selRwyDir.Geo, FocusMap, pSpatialReference);

            if (nav is SegmentPoint && ((SegmentPoint)nav).PointChoice == PointChoice.Navaid)
            {
                nav = DataCash.GetNavaidByID(((SegmentPoint)nav).PointChoiceID);
            }

            if (nav is NavaidSystem)
            {

                foreach (NavaidComponent item in ((NavaidSystem)nav).Components)
                {
                    if (((NavaidSystem)nav).CodeNavaidSystemType.ToString().Contains("DME") &&  item.PDM_Type == PDM_ENUM.DME)
                    {
                        gm = (IPoint)EsriUtils.ToProject(item.Geo, FocusMap, pSpatialReference);
                        break;
                    }
                    else if (((NavaidSystem)nav).CodeNavaidSystemType.ToString().Contains("NDB") && item.PDM_Type == PDM_ENUM.NDB)
                    {
                        gm = (IPoint)EsriUtils.ToProject(item.Geo, FocusMap, pSpatialReference);
                        break;
                    }
                }
  
            }
            
            else
            {
                gm = (IPoint)EsriUtils.ToProject(nav.Geo, FocusMap, pSpatialReference);
            }

            double distToRwyLine = 10000;
            if (gm != null)
            {
                 distToRwyLine = util.PointToLineDistance(thrPrjGeo, gm, dirBearing);
                 if (Math.Abs(distToRwyLine) < 8000 )
                 {
                     distToRwyLine = util.PointToLineDistance(thrPrjGeo, gm, dirBearing - 90);
                 }
                 else
                     distToRwyLine = 10000; 
               
                    
            }

            util = null;


            return distToRwyLine;

        }

        private static double calcDistFromRwy(PDMObject nav, RunwayDirection selRwyDir)
        {
            Utilitys util = new Utilitys();

            IPoint gm = null;


            if (nav.Geo == null) nav.RebuildGeo();
 
            if (nav is NavaidSystem)
            {

                foreach (NavaidComponent item in ((NavaidSystem)nav).Components)
                {
                    if (item.PDM_Type == PDM_ENUM.DME)
                    {
                        gm = (IPoint)item.Geo;
                        break;
                    }
                }
             }
            else
            {
                gm = (IPoint)nav.Geo;
            }

            double distToRwyLine = util.GetDistanceBetweenPoints_Elips((IPoint)selRwyDir.Geo, (IPoint)gm);



            util = null;


            return distToRwyLine;

        }

        private static double calcDistBettweenObjects(PDMObject Obj1, PDMObject Obj2, IMap FocusMap, ISpatialReference pSpatialReference)
        {

            IPoint gm1 = null;
            IPoint gm2 = null;

            if (Obj1.Geo == null) Obj1.RebuildGeo();
            if (Obj2.Geo == null) Obj2.RebuildGeo();

            gm1 = (IPoint)Obj1.Geo;
            gm2 = (IPoint)Obj2.Geo;

            gm1 = (IPoint)EsriUtils.ToProject(gm1, FocusMap, pSpatialReference);
            gm2 = (IPoint)EsriUtils.ToProject(gm2, FocusMap, pSpatialReference);

            Utilitys util = new Utilitys();

            return util.GetDistanceBetweenPoints_Proj(gm1,gm2);

        }

        private static void SetMax_Dist_Elev(Dictionary<PDMObject, ProcedureLeg> resList,double RdnElev_M, ref double maxDistfromRwy, ref double minDistfromRwy, ref double maxElevAboveRwy, ref double minElevAboveRwy)
        {
            foreach (var item in resList)
            {
                PDMObject nav = item.Key;
                double distToRwyLine = nav.X.Value;


                if (maxDistfromRwy < distToRwyLine) maxDistfromRwy = distToRwyLine;
                if (minDistfromRwy > distToRwyLine) minDistfromRwy = distToRwyLine;

                if (nav.Elev.HasValue)
                {
                    //double lla = nav.ConvertValueToMeter(nav.Elev.Value - RdnElev_M, nav.Elev_UOM.ToString());
                    double lla = nav.ConvertValueToMeter(nav.Elev.Value, nav.Elev_UOM.ToString()) - RdnElev_M;

                    if (maxElevAboveRwy < lla) maxElevAboveRwy = lla;
                    if (minElevAboveRwy > lla) minElevAboveRwy = lla;
                }
            }


        }

        private static IElement CreateSigmaTable(IHookHelper SigmaHookHelper, string ProtoTypeName, string NewTableName, int colCount, int rowCount,int prevTblRowCount, tableBuildDestination BuildDestination,
                                                ref IPolygon tableEnvelope, int colInxShift = 0, int rowInxShift = 0)
        {

            IElement _cell = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, ProtoTypeName + "_cell_0_0");
            IElement _col = Get_GraphicsContainerElemnt_ByName(SigmaHookHelper.ActiveView.GraphicsContainer, ProtoTypeName + "_Column_0_0");

            if (_cell == null) return null;

            if (!tableEnvelope.IsEmpty)
            {
                double oldW = _cell.Geometry.Envelope.Width;
                double newW = (tableEnvelope.Envelope.Width - _col.Geometry.Envelope.Width) / (colCount-1);
                double wScale =newW/ _cell.Geometry.Envelope.Width;
                ITransform2D cell_resize = _cell as ITransform2D;
                cell_resize.Scale(_cell.Geometry.Envelope.LowerLeft, wScale, 1);

                double dW = newW > oldW? oldW - newW : newW - oldW;
                cell_resize.Move(dW, 0);

                cell_resize = _col as ITransform2D;
                cell_resize.Move(dW, 0);

                
            }


            Delete_GraphicsContainerElemnt_ByName(SigmaHookHelper, NewTableName,true,true);


            IGroupElement3 resultTable = new GroupElementClass();

            double w = _cell.Geometry.Envelope.Width;
            double h = _cell.Geometry.Envelope.Height;

            IPoint _TopLeftCorner = new PointClass { X = _cell.Geometry.Envelope.UpperLeft.X, Y = _cell.Geometry.Envelope.UpperLeft.Y };
            IPoint _BottomRightCorner = new PointClass { X = _cell.Geometry.Envelope.LowerRight.X, Y = _cell.Geometry.Envelope.LowerRight.Y };
            IClone cell_clone = _cell as IClone;
            IClone col_clone = _col as IClone;

            for (int rowIndx = 0; rowIndx < rowCount; rowIndx++)
            {
                for (int ColIndx = 0; ColIndx < colCount; ColIndx++)
                {
                    var cl = cell_clone.Clone();
                    string cIndx_txt = (ColIndx + colInxShift).ToString();
                    string rIndx_txt = (rowIndx + rowInxShift).ToString();
                    if (BuildDestination == tableBuildDestination.RightUp)
                    {
                        cIndx_txt = (colCount - ColIndx -1 + colInxShift).ToString();
                        rIndx_txt = (rowCount - rowIndx -1 + rowInxShift).ToString();
                    }

                    if ((BuildDestination == tableBuildDestination.RightUp && ColIndx == colCount - 1) || (BuildDestination == tableBuildDestination.LeftDoun && ColIndx == 0))
                    {

                        cl = col_clone.Clone();

                        ITransform2D cell_move = cl as ITransform2D;
                        if (BuildDestination == tableBuildDestination.LeftDoun)
                        {
                            cell_move.Move(0, (_BottomRightCorner.Y - h * rowIndx) - _BottomRightCorner.Y);
                        }
                        else
                        {
                            cell_move.Move(-(ColIndx-1) * w, h * rowIndx);

                        }

                       
                    }
                    else
                    {
                        ITransform2D cell_move = cl as ITransform2D;
                        if (BuildDestination == tableBuildDestination.LeftDoun)
                        {
                            cell_move.Move((_TopLeftCorner.X + (ColIndx -1) * w) - _TopLeftCorner.X, (_BottomRightCorner.Y - h * rowIndx) - _BottomRightCorner.Y);
                        }
                        else
                        {
                            cell_move.Move((_TopLeftCorner.X - ColIndx * w) - _TopLeftCorner.X, h * rowIndx);

                        }

                        
                    }

                    IElementProperties3 docElementProperties2 = (IElementProperties3)((IElement)cl);
                    docElementProperties2.Name = "Cell_" + cIndx_txt + "_" + rIndx_txt.ToString();


                    resultTable.AddElement((IElement)cl);
                }
            }



            if (!tableEnvelope.IsEmpty && prevTblRowCount >0)
            {
                ITransform2D cell_move = resultTable as ITransform2D;
                if (BuildDestination == tableBuildDestination.RightUp)
                    cell_move.Move(0, (prevTblRowCount-1) * _cell.Geometry.Envelope.Height);
                else
                    cell_move.Move(0, 0);  // ??????????????
            }

            #region Расчет "площади" таблицы

            IPointCollection pointArr2 = (IPointCollection)tableEnvelope;
            
            if (BuildDestination == tableBuildDestination.RightUp)
            {
                IPoint p1 = new PointClass { X = _BottomRightCorner.X - (colCount - 1) * _cell.Geometry.Envelope.Width - _col.Geometry.Envelope.Width, Y = _BottomRightCorner.Y + rowCount * _cell.Geometry.Envelope.Height };
                pointArr2.AddPoint(p1);
                pointArr2.AddPoint(new PointClass { X = _BottomRightCorner.X, Y = p1.Y });
                pointArr2.AddPoint(_BottomRightCorner);
                pointArr2.AddPoint(new PointClass { X = p1.X, Y = _BottomRightCorner.Y });
            }

            else //(BuildDestination == tableBuildDestination.LeftDoun)
            {
                IPoint p1 = new PointClass { X = (_TopLeftCorner.X - _col.Geometry.Envelope.Width), Y = _TopLeftCorner.Y };
                IPoint p2 = new PointClass { X = _TopLeftCorner.X + (colCount - 1) * _cell.Geometry.Envelope.Width, Y = _TopLeftCorner.Y - rowCount * _cell.Geometry.Envelope.Height };

                pointArr2.AddPoint(p1);
                pointArr2.AddPoint(new PointClass { X = p2.X, Y = p1.Y });
                pointArr2.AddPoint(p2);
                pointArr2.AddPoint(new PointClass { X = p1.X, Y = p2.Y });

            }

            #endregion


            Delete_GraphicsContainerElemnt_ByName(SigmaHookHelper, ProtoTypeName + "_cell_0_0");
            Delete_GraphicsContainerElemnt_ByName(SigmaHookHelper, ProtoTypeName + "_column_0_0");

            IElementProperties3 docElementProperties = (IElementProperties3)resultTable;
            docElementProperties.Name = NewTableName + "|" + colCount.ToString() + "|" + rowCount.ToString();

            return (IElement)resultTable;

        }

        public static void CreateGeoBorderAnno(List<PDMObject> GeoBorderList, IMap FocusMap)
        {


            foreach (PDM.GeoBorder geoBord in GeoBorderList)
            {
                try
                {
                    ChartElement_SimpleText chrtEl_GeoBorder = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "GeoBorder_name");

                    chrtEl_GeoBorder.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(geoBord, chrtEl_GeoBorder.TextContents[0][0].DataSource);
                    //chrtEl_GeoBorder.TextContents[1][0].TextValue = ChartsHelperClass.MakeText(geoBord, chrtEl_GeoBorder.TextContents[1][0].DataSource);
                    chrtEl_GeoBorder.Slope = 0;//slope;
                    chrtEl_GeoBorder.LinckedGeoId = geoBord.ID;
                    IElement el_GeoBorder = (IElement)chrtEl_GeoBorder.ConvertToIElement();

                    el_GeoBorder.Geometry = ChartElementsManipulator.GetLinkedGeometry(chrtEl_GeoBorder.Name, chrtEl_GeoBorder.LinckedGeoId);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_GeoBorder.Name, geoBord.ID, el_GeoBorder, ref chrtEl_GeoBorder, chrtEl_GeoBorder.Id, FocusMap.MapScale);


                    //NeighborName
                    chrtEl_GeoBorder = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "GeoBorder_name");
                    chrtEl_GeoBorder.TextContents[0][0].DataSource.Value = "NeighborName";
                    chrtEl_GeoBorder.VerticalAlignment = verticalAlignment.Bottom;
                    chrtEl_GeoBorder.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(geoBord, chrtEl_GeoBorder.TextContents[0][0].DataSource );
                    chrtEl_GeoBorder.Slope = 0;//slope;
                    chrtEl_GeoBorder.LinckedGeoId = geoBord.ID;
                    el_GeoBorder = (IElement)chrtEl_GeoBorder.ConvertToIElement();

                    el_GeoBorder.Geometry = ChartElementsManipulator.GetLinkedGeometry(chrtEl_GeoBorder.Name, chrtEl_GeoBorder.LinckedGeoId);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_GeoBorder.Name, geoBord.ID, el_GeoBorder, ref chrtEl_GeoBorder, chrtEl_GeoBorder.Id, FocusMap.MapScale);
                }
                catch(Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(geoBord.GetObjectLabel() + (char)9 + geoBord.ID + (char)9 + ex.Message + (char)9 + ex.StackTrace);
                    continue;
                }
            }


        }

        public static void CreateAirportAnno(List<AirportHeliport> AirportList, string SelArpId , IFeatureClass Anno_AirportGeo_featClass, IFeatureClass AnnoRWYGeo_featClass , IMap FocusMap, 
                                                            ISpatialReference pSpatialReference, bool StoreAllRwy = false, string selArpDesignator = "")
        {
            
            foreach (PDM.AirportHeliport adhp in AirportList)
            {
                try
                {

                    if (!adhp.CertifiedICAO && adhp.Designator.CompareTo(selArpDesignator) !=0) continue;

                    ChartElement_SimpleText chrtEl_airport = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Airport");

                    chrtEl_airport.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(adhp, chrtEl_airport.TextContents[0][0].DataSource);
                    //chrtEl_airport.Slope = slope;
                    chrtEl_airport.LinckedGeoId = adhp.ID;
                    IElement el_Adhp = (IElement)chrtEl_airport.ConvertToIElement();

                    ChartsHelperClass.saveAirportHeliport_ChartGeo(Anno_AirportGeo_featClass, adhp);


                    el_Adhp.Geometry = ChartElementsManipulator.GetLinkedGeometry(chrtEl_airport.Name, chrtEl_airport.LinckedGeoId);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_airport.Name, adhp.ID, el_Adhp, ref chrtEl_airport, chrtEl_airport.Id, FocusMap.MapScale);


                    #region save RWY geometry

                    if ((adhp.ID.StartsWith(SelArpId) || StoreAllRwy) &&  adhp.RunwayList != null)
                    {
                        foreach (var rwy in adhp.RunwayList)
                        {

                            IGeometry rwyGeo = TerminalChartsUtil.CreayteRwyStrip(rwy, FocusMap, pSpatialReference);

                            if (rwyGeo != null && AnnoRWYGeo_featClass != null)
                                ChartsHelperClass.SaveRWY_ChartGeo(AnnoRWYGeo_featClass, rwyGeo, rwy.Designator);

                        }


                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(adhp.GetObjectLabel() + (char)9 + adhp.ID + (char)9 + ex.Message + (char)9 + ex.StackTrace);
                    continue;
                }

            }


        }

        public static void CreateAirportAnno(List<AirportHeliport> AirportList, IFeatureClass Anno_AirportGeo_featClass,IMap FocusMap,double slope = 0)
        {


            foreach (PDM.AirportHeliport adhp in AirportList)
            {
                try
                {
                    ChartElement_SimpleText chrtEl_airport = (ChartElement_SimpleText)ChartsHelperClass.getPrototypeChartElement(SigmaDataCash.prototype_anno_lst, "Airport");

                    chrtEl_airport.TextContents[0][0].TextValue = ChartsHelperClass.MakeText(adhp, chrtEl_airport.TextContents[0][0].DataSource);
                    chrtEl_airport.Slope = slope;
                    chrtEl_airport.LinckedGeoId = adhp.ID;
                    IElement el_Adhp = (IElement)chrtEl_airport.ConvertToIElement();

                    ChartsHelperClass.saveAirportHeliport_ChartGeo(Anno_AirportGeo_featClass, adhp);


                    el_Adhp.Geometry = ChartElementsManipulator.GetLinkedGeometry(chrtEl_airport.Name, chrtEl_airport.LinckedGeoId);
                    ChartElementsManipulator.StoreSingleElementToDataSet(chrtEl_airport.Name, adhp.ID, el_Adhp, ref chrtEl_airport, chrtEl_airport.Id, FocusMap.MapScale);


                   
                }
                catch (Exception ex)
                {
                    if (SigmaDataCash.Report == null) SigmaDataCash.Report = new List<string>();
                    SigmaDataCash.Report.Add(adhp.GetObjectLabel() +(char)9 + adhp.ID + (char)9 + ex.Message + (char)9 + ex.StackTrace);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    continue;
                }

            }


        }


        public static Dictionary<string, ProcedureLeg> LegFilter(List<PDMObject> selectedProc)
        {
            Dictionary<string, ProcedureLeg> dicLeg = new Dictionary<string, ProcedureLeg>();
            string uId = "";
            foreach (Procedure proc in selectedProc)
            {
                foreach (ProcedureTransitions trans in proc.Transitions)
                {
                    int legIndx = 0;
                   
                    foreach (ProcedureLeg leg in trans.Legs)
                    {
                        string startID = "";
                        string endID = "";

                        if (leg.StartPoint != null) startID = leg.StartPoint.PointChoiceID;// != null ? leg.StartPoint.PointChoiceID : Guid.NewGuid().ToString();
                        else
                        {
                            if (leg.LegTypeARINC != CodeSegmentPath.IF && legIndx > 0 && trans.Legs[legIndx - 1].EndPoint !=null)
                                startID = trans.Legs[legIndx - 1].EndPoint.PointChoiceID;
                        }
                        if (leg.EndPoint != null) endID = leg.EndPoint.PointChoiceID;// != null ? leg.EndPoint.PointChoiceID : Guid.NewGuid().ToString();
                        else
                        {
                            if (legIndx != trans.Legs.Count && leg.LegTypeARINC != CodeSegmentPath.IF && trans.Legs.Count > 1 && trans.Legs[legIndx + 1].StartPoint != null)
                                endID = trans.Legs[legIndx + 1].StartPoint.PointChoiceID;
                        }


                        uId = startID + ":" + endID;
                        leg.ProcedureIdentifier = proc.ProcedureIdentifier;
                        leg.Lat = proc.AirportIdentifier;

                        

                        if (!dicLeg.ContainsKey(uId))
                        {
                            dicLeg.Add(uId, leg);
                        }

                        else
                        {
                            if (dicLeg[uId].Geo == null) dicLeg[uId].RebuildGeo();
                            if (leg.Geo == null) leg.RebuildGeo();
                            if (leg.Geo == null)
                            {
                                System.Diagnostics.Debug.WriteLine(proc.GetObjectLabel() + " "+ trans.GetObjectLabel() + " " + leg.GetObjectLabel() + " don't have geometry");
                                continue;
                            }

                            ESRI.ArcGIS.Geometry.IRelationalOperator rel1 = dicLeg[uId].Geo as ESRI.ArcGIS.Geometry.IRelationalOperator;

                            if (rel1.Equals(leg.Geo)) //((dicLeg[uId].Geo as IPolyline).Length == (leg.Geo as IPolyline).Length)
                            {
                                dicLeg[uId].ProcedureIdentifier = dicLeg[uId].ProcedureIdentifier + "/" + proc.ProcedureIdentifier;
                            }
                            else
                            {
                                dicLeg[uId].ProcedureIdentifier = proc.ProcedureIdentifier;
                                leg.ProcedureIdentifier = "ignore"+ leg.ProcedureIdentifier;
                                dicLeg.Add(Guid.NewGuid().ToString(), leg);
                            }
                        }

                        legIndx++;
                    }
                }

            }

            return dicLeg;
        }

        public static bool _CheckFileExisting(string _ProjectName, string _FolderName)
        {
            var projectName = _ProjectName.EndsWith(".mxd") ? _ProjectName : _ProjectName + ".mxd";
            var destPath2 = System.IO.Directory.CreateDirectory(_FolderName + @"\" + _ProjectName).FullName;
            string fn = System.IO.Path.Combine(destPath2, projectName);

            if (File.Exists(fn))
            {
                if (MessageBox.Show("There is already a file with same name in this location. \n Do you want to overwrite it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No) return false;
            }
            return true;
        }

        public static bool CheckFileExisting(string _ProjectName, string _FolderName)
        {
            var projectName = _ProjectName.EndsWith(".mxd") ? _ProjectName : _ProjectName + ".mxd";
            var destPath2 = System.IO.Directory.CreateDirectory(_FolderName + @"\" + _ProjectName).FullName;
            string fn = System.IO.Path.Combine(destPath2, projectName);

            if (File.Exists(fn) && !System.Diagnostics.Debugger.IsAttached)
            {
                if (MessageBox.Show("There is already a file with same name in this location. \n Please select another place to save the project", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK) return false;
            }
            else if (File.Exists(fn) && System.Diagnostics.Debugger.IsAttached)
            {
                    if (MessageBox.Show("There is already a file with same name in this location. \n Do you want to overwrite it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No) return false;

            }
            return true;
        }

        public static List<string> getChanelsList(List<RadioCommunicationChanel> _ChanelsList)
        {
            if (_ChanelsList == null || _ChanelsList.Count == 0) return null;

            List<string> res = new List<string>();

            foreach (RadioCommunicationChanel rcnl in _ChanelsList)
            {
                string rchStr = (rcnl.CallSign.Length > 0 ? rcnl.CallSign : rcnl.ChanelType);
                string prim = rcnl.FrequencyReception.HasValue? rcnl.FrequencyReception.ToString() : "" ;
                string sec = rcnl.FrequencyTransmission.HasValue ? rcnl.FrequencyTransmission.ToString() : "";
                string rank = rcnl.Rank.ToString();

                if (prim.Length > 0)
                {
                    string[] pnts = prim.Split('.');
                    if (pnts.Length > 1 && pnts[1].Length < 2) prim = prim + "0";
                    else if (pnts.Length == 1) prim = prim + ".00";
                }
                if (sec.Length > 0)
                {
                    string[] pnts = sec.Split('.');
                    if (pnts.Length > 1 && pnts[1].Length < 2) sec = sec + "0";
                    else if (pnts.Length == 1) sec = sec + ".00";
                }

                if (prim.CompareTo(sec) ==0) sec = "";

                rchStr = rchStr + " " + prim + "/" + sec;
                if (rchStr.EndsWith("/")) rchStr = rchStr.Remove(rchStr.Length - 1, 1);

                rchStr = rchStr + " (" + rank + ")";
                if (rchStr.EndsWith("()")) rchStr = rchStr.Remove(rchStr.Length - 2, 2);
                res.Add(rchStr);
            }

            

            res.Sort();
            return res;
        }

        public static string getChanelsString(RadioCommunicationChanel rcnl)
        {
            if (rcnl == null) return null;

           string res = "";

            
                string rchStr = (rcnl.CallSign.Length > 0 ? rcnl.CallSign : rcnl.ChanelType);
                string prim = rcnl.FrequencyReception.HasValue ? rcnl.FrequencyReception.ToString() : "";
                string sec = rcnl.FrequencyTransmission.HasValue ? rcnl.FrequencyTransmission.ToString() : "";
                string rank = rcnl.Rank.ToString();

                if (prim.Length > 0)
                {
                    string[] pnts = prim.Split('.');
                    if (pnts.Length > 1 && pnts[1].Length < 2) prim = prim + "0";
                    else if (pnts.Length == 1) prim = prim + ".00";
                }
                if (sec.Length > 0)
                {
                    string[] pnts = sec.Split('.');
                    if (pnts.Length > 1 && pnts[1].Length < 2) sec = sec + "0";
                    else if (pnts.Length == 1) sec = sec + ".00";
                }

                if (prim.CompareTo(sec) == 0) sec = "";

                rchStr = rchStr + " " + prim + "/" + sec;
                if (rchStr.EndsWith("/")) rchStr = rchStr.Remove(rchStr.Length - 1, 1);

                rchStr = rchStr + " (" + rank + ")";
                if (rchStr.EndsWith("()")) rchStr = rchStr.Remove(rchStr.Length - 2, 2);
                res  = rchStr;

            return res;
        }


        public static void UnselectFeatureClasses(IMap pMap)
        {
            foreach (KeyValuePair<string, object> item in SigmaDataCash.AnnotationFeatureClassList)
            {
                if (!(item.Value is IFeatureClass)) continue;


                IFeatureClass Anno_featClass = (IFeatureClass)item.Value;
                ILayer _Layer = EsriUtils.getLayerByName(pMap, Anno_featClass.AliasName);

                if (_Layer == null || !_Layer.Visible) continue;

                /////////////
                IFeatureSelection pSelect = (IFeatureSelection)_Layer;
                if (pSelect != null) pSelect.Clear();

                ////////////

            }
        }

        public static IGeometry AnnotationsPosition(ChartElement_SimpleText chartEl, IMap pMap)
        {
            ILayer _Layer = EsriUtils.getLayerByName2(pMap, ChartElementsManipulator.DefinelayerName(chartEl.Name));
            IGeometry res = null;
            try
            {
                pMap.ClearSelection();
                _Layer.Visible = true;

                IFeatureClass _fc = ((IFeatureLayer)_Layer).FeatureClass;
                IFeature feat = ChartElementsManipulator.SearchBySpatialFilter(_fc, "AncorUID =  " + "'" + chartEl.Id.ToString() + "'");
                if (feat != null)
                {
                    
                    res = feat.Shape;
                }
                

                return res;



            }
            catch { return null; }
        }
       
        public static string GetAircfatCategoriesLine(List<PDMObject> selectedProc)
        {

            string aircraftCat = "";
            foreach (var prc in selectedProc)
            {
                if (((Procedure)prc).AircraftCharacteristic != null)
                {
                    foreach (var airChar in ((Procedure)prc).AircraftCharacteristic)
                    {
                        if (aircraftCat.Contains(airChar.AircraftLandingCategory.ToString())) continue;
                        aircraftCat = aircraftCat + airChar.AircraftLandingCategory.ToString() + ",";
                    }
                }
            }


            if (aircraftCat.EndsWith(","))
                aircraftCat = " " + aircraftCat.Remove(aircraftCat.Length - 1, 1);

            return aircraftCat;
        }

        public static string GetProcIntcructionLine(List<PDMObject> selectedProc)
        {
            string procInstruct = "";
            foreach (var prc in selectedProc)
            {
                if (((Procedure)prc).Instruction != null && ((Procedure)prc).Instruction.Length > 0)
                {

                    procInstruct = procInstruct + prc.GetObjectLabel() + ": " + ((Procedure)prc).Instruction + '\n';

                }
            }


            return procInstruct;
        }

        public static string GetTemplateForChart(SigmaChartTypes mDesc)
        {
            var _FolderName = ArenaStaticProc.GetMainFolder() + @"\Model\SIGMA\Templates";


            switch ((SigmaChartTypes)mDesc)
            {

                case SigmaChartTypes.EnrouteChart_Type:
                    _FolderName = _FolderName + @"\Enroute\";
                    break;
                case SigmaChartTypes.SIDChart_Type:
                    _FolderName = _FolderName + @"\SID\";
                    break;
                case SigmaChartTypes.ChartTypeA:
                    _FolderName = _FolderName + @"\ChartTypeA\";
                    break;
                case SigmaChartTypes.STARChart_Type:
                    _FolderName = _FolderName + @"\STAR\";
                    break;
                case SigmaChartTypes.IAPChart_Type:
                    _FolderName = _FolderName + @"\IAP\";
                    break;
                case SigmaChartTypes.PATChart_Type:
                    _FolderName = _FolderName + @"\PATC\";
                    break;
                case SigmaChartTypes.AreaChart:
                    _FolderName = _FolderName + @"\AreaChart\";
                    break;
                case SigmaChartTypes.AerodromeElectronicChart:
                    _FolderName = _FolderName + @"\AerodromeElectronicChart\";
                    break;
                case SigmaChartTypes.AerodromeParkingDockingChart:
                    _FolderName = _FolderName + @"\AerodromeParkingDockingChart\";
                    break;
                case SigmaChartTypes.AerodromeGroundMovementChart:
                    _FolderName = _FolderName + @"\AerodromeGroundMovementChart\";
                    break;
                case SigmaChartTypes.AerodromeBirdChart:
                    _FolderName = _FolderName + @"\AerodromeBirdChart\";
                    break;
                case SigmaChartTypes.AerodromeChart:
                    _FolderName = _FolderName + @"\AerodromeChart\";
                    break;
                case SigmaChartTypes.MinimumAltitudeChart:
                    _FolderName = _FolderName + @"\MinimumAltitudeChart\";
                    break;
                case SigmaChartTypes.None:
                default:
                    break;
            }

            return _FolderName;

        }



    }
}
