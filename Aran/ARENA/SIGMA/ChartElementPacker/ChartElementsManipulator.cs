using ANCOR.MapElements;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using EsriWorkEnvironment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using ANCOR.MapCore;
using System.Media;
using ESRI.ArcGIS.Controls;
using ArenaStatic;
using Encryptor;

namespace SigmaChart
{
    public static class ChartElementsManipulator
    {


        [DllImport("MathFunctions.dll", CharSet = CharSet.Ansi, EntryPoint = "_Modulus@16")]
        public static extern double Modulus(double X, double Y);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hDC, int nXPos, int nYPos);
        [DllImport("gdi32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);



        public static string GetActiveFrameName(IMap map, IHookHelper m_hookHelper)
        {
            /////////////////////////////
            //IMap map = m_hookHelper.ActiveView.FocusMap;
            if (IsNumeric(map.Description)) return "LAYERS";

            IGraphicsContainer graphicsContainer = (IGraphicsContainer)m_hookHelper.ActiveView;
            IFrameElement frameElement = graphicsContainer.FindFrame(map);
            if (frameElement != null)
            {


                IMapFrame mapFrame = (IMapFrame)frameElement;


                IElement pElement = (IElement)mapFrame;

                return (pElement as IElementProperties3).Name;
            }
            else return "";
            /////////////////////////////
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
                return Double.TryParse(anyString, System.Globalization.NumberStyles.Any, cultureInfo.NumberFormat, out dummyOut);
            }
            else
            {
                return false;
            }
        }

        public static System.Drawing.Color GetPixelColor(IntPtr hDC)
        {

            System.Drawing.Point cursor = new System.Drawing.Point();
            GetCursorPos(ref cursor);

            return GetColorAt(cursor);

        }


        private static System.Drawing.Color GetColorAt(System.Drawing.Point location)
        {
            System.Drawing.Bitmap screenPixel = new System.Drawing.Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (System.Drawing.Graphics gdest = System.Drawing.Graphics.FromImage(screenPixel))
            {
                using (System.Drawing.Graphics gsrc = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)System.Drawing.CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }


        public static object DeserializeObject<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader textReader = new StringReader(toDeserialize);
            return xmlSerializer.Deserialize(textReader);
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            try
            {

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                StringWriter textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return "";
            }
        }


        public static int StoreSingleElementToDataSet<T>(string ElementName, string pdmElementID, IElement mapElem, ref T chartEl, Guid chartElID, double mapScale, int flag = 0)
        {
            if (DateTime.Now > SigmaEncryptor.GetEncryptedDate()) return 0;

            int res = 0;
            if (mapElem == null) return 0;
            try
            {

                IFeatureClass featureClass = GetLinkedFeatureClass(ElementName);

                if (featureClass == null && (ElementName.StartsWith("HoldingPatternInboundCource") || ElementName.StartsWith("HoldingPatternOutboundCource")))
                    featureClass = GetLinkedFeatureClass("RouteSegment_ValMagTrack");

                int LinkedFeatureID = flag;
                if (flag == 0) LinkedFeatureID = GetLinkedFeatureID(ElementName, pdmElementID);


                IAnnoClass pAnnoClass = (IAnnoClass)featureClass.Extension;

                IFeatureClass pClass = pAnnoClass.FeatureClass;
                IFeature pFeat = pClass.CreateFeature();
                IAnnotationFeature pAnnoFeat = (IAnnotationFeature)pFeat;


                pAnnoFeat.Annotation = mapElem;
                pAnnoFeat.LinkedFeatureID = 1;
                string objSer = "";

                int fID = pFeat.Fields.FindField("FeatureID");
                if (fID > 0) pFeat.set_Value(fID, LinkedFeatureID);

                fID = pFeat.Fields.FindField("AnnotationClassID");
                if (fID > 0) pFeat.set_Value(fID, 0);

                int status = (chartEl as AbstractChartElement).Placed ? 0 : 1;
                fID = pFeat.Fields.FindField("Status");
                if (fID > 0) pFeat.set_Value(fID, status);

                fID = pFeat.Fields.FindField("SymbolID");
                if (fID > 0) pFeat.set_Value(fID, 0);

                fID = pFeat.Fields.FindField("PdmUID");
                if (fID > 0) pFeat.set_Value(fID, pdmElementID);

                (chartEl as AbstractChartElement).LinckedGeoId = pdmElementID;
                //(chartEl as AbstractChartElement).Scale = mapScale;

                fID = pFeat.Fields.FindField("ObjectType");
                if (fID > 0) pFeat.set_Value(fID, chartEl.GetType().Name.ToString());

                fID = pFeat.Fields.FindField("OBJ");
                if (fID > 0)
                {
                    objSer = SerializeObject(chartEl);
                    pFeat.set_Value(fID, objSer);
                }

                fID = pFeat.Fields.FindField("AncorUID");
                if (fID > 0) pFeat.set_Value(fID, chartElID.ToString());



                pFeat.Store();



                SigmaDataCash.ChartElementList.Add(chartEl);

                res++;

                LoadToMirror(mapElem, chartElID, ElementName, mapScale, objSer, status);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res = -1;
            }
            return res;
        }

        public static bool FindAndReplaceText(ChartElement_SimpleText cartoEl, string _oldTxt, string _newTxt, bool _MatchCase, bool _MatchWholeWord)
        {
            bool res = false;

            foreach (var ln in cartoEl.TextContents)
            {
                foreach (var wrd in ln)
                {
                    string curTxt = !_MatchCase ? wrd.TextValue.ToUpper() : wrd.TextValue;

                    if (curTxt.CompareTo(_oldTxt) == 0 || curTxt.Contains(_oldTxt))
                    {
                        if (curTxt.CompareTo(_oldTxt) == 0 && _MatchWholeWord)
                        {
                            wrd.TextValue = _newTxt;
                            res = true;
                        }
                        else if (curTxt.Contains(_oldTxt))
                        {
                            curTxt = curTxt.Replace(_oldTxt, _newTxt);
                            wrd.TextValue = curTxt;
                            res = true;
                        }
                    }
                }
            }

            if (cartoEl is ChartElement_BorderedText_Collout_CaptionBottom)
            {
                ChartElement_BorderedText_Collout_CaptionBottom CapBtnTextEl = (ChartElement_BorderedText_Collout_CaptionBottom)cartoEl;

                if (CapBtnTextEl.CaptionTextLine !=null)
                {
                    foreach (var ln in CapBtnTextEl.CaptionTextLine)
                    {
                        foreach (var wrd in ln)
                        {
                            string curTxt = !_MatchCase ? wrd.TextValue.ToUpper() : wrd.TextValue;

                            if (curTxt.CompareTo(_oldTxt) == 0 || curTxt.Contains(_oldTxt))
                            {
                                if (curTxt.CompareTo(_oldTxt) == 0 && _MatchWholeWord)
                                {
                                    wrd.TextValue = _newTxt;
                                    res = true;
                                }
                                else if (curTxt.Contains(_oldTxt))
                                {
                                    curTxt = curTxt.Replace(_oldTxt, _newTxt);
                                    wrd.TextValue = curTxt;
                                    res = true;
                                }
                            }
                        }
                    }
                }

                if (CapBtnTextEl.BottomTextLine != null)
                {
                    foreach (var ln in CapBtnTextEl.BottomTextLine)
                    {
                        foreach (var wrd in ln)
                        {
                            string curTxt = !_MatchCase ? wrd.TextValue.ToUpper() : wrd.TextValue;

                            if (curTxt.CompareTo(_oldTxt) == 0 || curTxt.Contains(_oldTxt))
                            {
                                if (curTxt.CompareTo(_oldTxt) == 0 && _MatchWholeWord)
                                {
                                    wrd.TextValue = _newTxt;
                                    res = true;
                                }
                                else if (curTxt.Contains(_oldTxt))
                                {
                                    curTxt = curTxt.Replace(_oldTxt, _newTxt);
                                    wrd.TextValue = curTxt;
                                    res = true;
                                }
                            }
                        }
                    }
                }


            }

            return res;
        }

        public static string FindAndReplaceUtil(string oldTxtArg, string newTxtArg, bool MatchCaseArg, bool MatchWholeWordArg, bool ignoreAnnoArg, bool ignoreGraphicsArg, 
            string FelFeatureArg, IGraphicsContainer _GraphicsContainer)
        {
            int ChartElementCounter = 0;
            int graphElementCounter = 0;

            SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
            SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();


            if (!MatchCaseArg)
            {
                oldTxtArg = oldTxtArg.ToUpper();
                //_newTxt = _newTxt.ToUpper();
            }

            if (!ignoreAnnoArg)
            {
                foreach (var item in SigmaDataCash.ChartElementList)
                {

                    AbstractChartElement cartoEl = (AbstractChartElement)item;
                    if (FelFeatureArg.Length > 0 && cartoEl.Name.CompareTo(FelFeatureArg) != 0) continue;

                    if (cartoEl is ChartElement_SimpleText)
                    {
                        if (ChartElementsManipulator.FindAndReplaceText((ChartElement_SimpleText)cartoEl, oldTxtArg, newTxtArg, MatchCaseArg, MatchWholeWordArg))
                        {
                            IElement el = cartoEl.ConvertToIElement() as IElement;
                            ChartElementsManipulator.UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, false);

                            ChartElementCounter++;

                        }


                    }
                }

            }

            if (!ignoreGraphicsArg)
            {
                _GraphicsContainer.Reset();

                IElement dinamic_el = _GraphicsContainer.Next();

                while (dinamic_el != null)
                {

                    IElementProperties3 elProp = (IElementProperties3)dinamic_el;
                    elProp.Name.ToString();
                    if (dinamic_el is IGroupElement3)
                    {
                        IGroupElement3 grpEl = (IGroupElement3)dinamic_el;
                        for (int i = 0; i < grpEl.ElementCount; i++)
                        {

                            IElement elval = grpEl.get_Element(i);
                            if (grpEl.get_Element(i) is ITextElement)
                            {
                                ITextElement txtEl = (ITextElement)elval;
                                string curTxt = !MatchCaseArg ? txtEl.Text.ToUpper() : txtEl.Text;

                                if ((curTxt.CompareTo(oldTxtArg) == 0) || (curTxt.Contains(oldTxtArg)))
                                {
                                    if (curTxt.CompareTo(oldTxtArg) == 0 && MatchWholeWordArg)
                                    {
                                        txtEl.Text = newTxtArg;
                                        graphElementCounter++;

                                    }
                                    else if (curTxt.Contains(oldTxtArg))
                                    {
                                        curTxt = curTxt.Replace(oldTxtArg, newTxtArg);
                                        txtEl.Text = curTxt;
                                        graphElementCounter++;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dinamic_el is ITextElement)
                        {
                            ITextElement txtEl = (ITextElement)dinamic_el;
                            string curTxt = !MatchCaseArg ? txtEl.Text.ToUpper() : txtEl.Text;

                            if ((curTxt.CompareTo(oldTxtArg) == 0) || (curTxt.Contains(oldTxtArg)))
                            {
                                if (curTxt.CompareTo(oldTxtArg) == 0 && MatchWholeWordArg)
                                {
                                    txtEl.Text = newTxtArg;
                                    graphElementCounter++;

                                }
                                else if (curTxt.Contains(oldTxtArg))
                                {
                                    curTxt = curTxt.Replace(oldTxtArg, newTxtArg);
                                    txtEl.Text = curTxt;
                                    graphElementCounter++;

                                }
                            }
                        }
                    }

                    dinamic_el = _GraphicsContainer.Next();
                }

            }

            SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
            SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

            string Message = ChartElementCounter > 0 ? ChartElementCounter.ToString() + " charts elements modified" : "";
            if (Message.Length > 0) Message = Message + Environment.NewLine;
            Message = graphElementCounter > 0 ? Message + graphElementCounter.ToString() + " graphics elements modified" : Message + "";

            //if (Message.Length > 0 && ChartElementCounter > 0)
            //{
            //    Message = Message + Environment.NewLine + "Please refresh SIGMA TOC";
            //    ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);

            //}

            //MessageBox.Show(Message, "Sigma", MessageBoxButton.OK, MessageBoxImage.Information);
            return Message;

        }

        public static int StoreGraphicsElementToDataSet<T>(string ElementName, string pdmElementID, IElement mapElem, ref T chartEl, Guid chartElID)
        {
            int res = 0;

            try
            {
                ITable graphicsTable = null;

                if (SigmaDataCash.AnnotationFeatureClassList.ContainsKey("GraphicsElementsView"))
                    graphicsTable = (ITable)SigmaDataCash.AnnotationFeatureClassList["GraphicsElementsView"];

                IRow pFeat = graphicsTable.CreateRow();
                int fID = pFeat.Fields.FindField("Status");
                if (fID > 0) pFeat.set_Value(fID, 0);

                fID = pFeat.Fields.FindField("PdmUID");
                if (fID > 0) pFeat.set_Value(fID, pdmElementID);

                fID = pFeat.Fields.FindField("ObjectType");
                if (fID > 0) pFeat.set_Value(fID, chartEl.GetType().Name.ToString());

                fID = pFeat.Fields.FindField("OBJ");
                if (fID > 0) pFeat.set_Value(fID, SerializeObject(chartEl));

                fID = pFeat.Fields.FindField("AncorUID");
                if (fID > 0) pFeat.set_Value(fID, chartElID.ToString());

                pFeat.Store();



                SigmaDataCash.ChartElementList.Add(chartEl);

                res++;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res = -1;
            }
            return res;
        }

        public static IFeatureClass GetLinkedFeatureClass(string ChartElemenName)
        {
            IFeatureClass res = null;
            string FCN = DefinelayerName(ChartElemenName);

           
            switch (ChartElemenName)
            {
                case ("RouteSegment_ValMagTrack"):
                //case ("HoldingPatternInboundCource"):
                    FCN = "RouteSegmentAnnoMagTrack";
                    break;
                case ("RouteSegment_ValReversMagTrack"):
                    FCN = "RouteSegmentAnnoReverseMagTrack";
                    break;
                case ("RouteSegment_sign"):
                    FCN = "RouteSegmentAnnoSign";
                    break;
                case ("RouteSegment_UpperLowerLimit"):
                    FCN = "RouteSegmentAnnoLimits";
                    break;
                case ("DesignatedPoint"):
                case ("HoldingPattern"):
                case ("SigmaCollout_Designatedpoint"):
                case ("FC_RadialDistance"):
                case ("FC_Height"):
                case ("DesignatedPoint_Simple"):
                    FCN = "DesignatedPointAnno";
                    break;
                case ("Navaids"):
                case ("SigmaCollout_Navaid"):
                case ("GlidePath_Navaid"):
                    FCN = "NavaidsAnno";
                    break;
                case ("Airspace"):
                case ("Airspace_Simple"):
                case ("SigmaCollout_Airspace"):
                case ("Airspace_Class"):
                case ("BirdElement"):
                case ("SectorAirspace"):
                case ("ATZ_ATZP_Airspace"):
                case ("CTR_CTRP_Airspace"):
                case ("TMA_TMAP_Airspace"):
                case ("TIZ_Airspace"):
                case ("TIA_Airspace"):
                case ("FIS_Airspace"):
                case ("R_D_P_Airspace"):
                case ("R_D_P_AMC_Airspace"):
                case ("SECTOR_SECTORC_Airspace"):
                case ("PROTECT_Airspace"):
                case ("TRA_TSA_Airspace"):
                case ("AOR_Airspace"):
                    FCN = "AirspaceAnno";
                    break;
                case ("AMA_Text"):
                    FCN = "AMAEAnno";
                    break;
                case ("Mirror"):
                    FCN = "Mirror";
                    break;
                case ("ProcedureLegLength"):
                case ("NoneScale"):
                    FCN = "ProcedureLegsAnnoLengthCartography";
                    break;
                case ("ProcedureLegSpeed"):
                    FCN = "ProcedureLegsAnnoSpeedLimitCartography";
                    break;
                case ("ProcedureLegCourse"):
                case ("HoldingPatternInboundCource"):
                case ("HoldingPatternOutboundCource"):
                    FCN = "ProcedureLegsAnnoCourseCartography";
                    break;
                case ("ILSGlidePath"):
                    FCN = "ProcedureLegsAnnoILSCartography";
                    break;
                case ("ProcedureLegName"):
                    FCN = "ProcedureLegsAnnoLegNameCartography";
                    break;
                case ("ProcedureLegHeight"):
                case ("TextArrow"):
                    FCN = "ProcedureLegsAnnoHeightCartography";
                    break;
                case ("AngleIndication"):
                case ("DistanceIndication"):
                    FCN = "FacilityMakeUpAnno";
                    break;
                case ("Airport"):
                    FCN = "AirportAnnoCartography";
                    break;
                case ("GeoBorder_name"):
                    FCN = "GeoBorderAnno";
                    break;
                case ("VerticalStructurePartElev"):
                case ("VerticalStructurePartHeight"):
                    FCN = "VerticalStructureAnnoCartography";
                    break;
                case ("IsogonalLines"):
                    FCN = "IsogonalLinesAnno";
                    break;
                case ("RunwayElement"):
                case ("ParkingDockingRunwayElement"):
                    FCN = "RunwayElementAnno";
                    break;
                case ("GuidanceLineElement"):
                    FCN = "GuidanceLineAnno";
                    break;
                case ("AircraftStandElement"):
                    FCN = "AircraftStandAnno";
                    break;
                case ("RunwayStripElement"):
                    FCN = "RunwayProtectAreaAnno";
                    break;
                case ("RunwayTHRElement"):
                case ("RunwayTDZElement"):
                case ("RunwayTORAElement"):
                case ("RDNElement"):
                    FCN = "RunwayDirectionCenterLinePointAnno";
                    break;
                case ("CheckpointElement"):
                    FCN = "CheckpointAnno";
                    break;
                case ("RunwayVisulRangeElement"):
                    FCN = "LightElementAnno";
                    break;
                case ("AirportHotSpotElement"):
                    FCN = "AirportHotSpotAnno";
                    break;
                case ("VerticalStructureElement"):
                case ("VerticalStructureElementPoint"):
                    FCN = "VertStructureSurfaceAnno";
                    break;
                case ("VerticalStructureElementElev"):
                case ("VerticalStructureElementHeight"):
                    FCN = "VerticalStructurePointAnno";
                    break;
                case ("FreqAreaElement"):
                    FCN = "RadioFrequencyAreaAnno";
                    break;
                case ("FreeAnno"):
                case ("CircleDistace"):
                    FCN = "FreeAnno";
                    break;
                default:
                    FCN = "RouteSegmentAnnoMagTrack";
                    break;
            }

            if (SigmaDataCash.SigmaChartType == 4 && (ChartElemenName.StartsWith("HoldingPatternInboundCource") || ChartElemenName.StartsWith("HoldingPatternOutboundCource")))
            {
                FCN = "ProcedureLegsAnnoCourseCartography";
            }


            if (SigmaDataCash.SigmaChartType == 1 && ChartElemenName.StartsWith("HoldingPatternInboundCource"))
            {
                if (SigmaDataCash.AnnotationFeatureClassList.ContainsKey("RouteSegmentAnnoMagTrack"))
                    FCN = "RouteSegmentAnnoMagTrack";
            }
            if (SigmaDataCash.SigmaChartType == 1 && ChartElemenName.StartsWith("HoldingPatternOutboundCource"))
            {
                if (SigmaDataCash.AnnotationFeatureClassList.ContainsKey("RouteSegmentAnnoReverseMagTrack"))
                    FCN = "RouteSegmentAnnoReverseMagTrack";
            }

            if (SigmaDataCash.AnnotationFeatureClassList.ContainsKey(FCN))
                res = (IFeatureClass)SigmaDataCash.AnnotationFeatureClassList[FCN];

            return res;
        }


        public static IGeometry GetLinkedGeometry(string ChartElemenName, string LinkedID)
        {
            IGeometry res = null;
            string FCN = "RouteSegmentCartography";

            FCN = GetLinkedGeometryFeatureClassName(ChartElemenName);

            string LinkedID2 = LinkedID;

            if (LinkedID.Length > 36)
            LinkedID2 = LinkedID.Remove(36);

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(FCN))
            {
                IFeatureClass geoClass = SigmaDataCash.AnnotationLinkedGeometryList[FCN];

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "FeatureGUID= '" + LinkedID + "' OR FeatureGUID= '" + LinkedID2 + "'";

                IFeatureCursor featCur = geoClass.Search(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat == null && ChartElemenName.CompareTo("ProcedureLegHeight") == 0) // некоторые аннотации LegHeight ассоциируются с наваидами, а не DPN (как по умолчанию). Поэтому если feat == null надо поискать в Navaids
                {
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];

                    featFilter = new QueryFilterClass();

                    featFilter.WhereClause = "FeatureGUID= '" + LinkedID + "'";

                    featCur = geoClass.Search(featFilter, false);

                    feat = featCur.NextFeature();
                }



                if (feat != null)
                {
                    res = feat.Shape;

                }


                Marshal.ReleaseComObject(featCur);
            }
            return res;
        }

        public static object GetLinkedProperty(string ChartElemenName, string LinkedID, string PropFromTable)
        {
            object res = null;
            string FCN = "RouteSegmentCartography";

            FCN = GetLinkedGeometryFeatureClassName(ChartElemenName);

            string LinkedID2 = LinkedID;

            if (LinkedID.Length > 36)
                LinkedID2 = LinkedID.Remove(36);

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(FCN))
            {
                IFeatureClass geoClass = SigmaDataCash.AnnotationLinkedGeometryList[FCN];

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "FeatureGUID= '" + LinkedID + "' OR FeatureGUID= '" + LinkedID2 + "'";

                IFeatureCursor featCur = geoClass.Search(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat == null && ChartElemenName.CompareTo("ProcedureLegHeight") == 0) // некоторые аннотации LegHeight ассоциируются с наваидами, а не DPN (как по умолчанию). Поэтому если feat == null надо поискать в Navaids
                {
                    geoClass = SigmaDataCash.AnnotationLinkedGeometryList["NavaidsCartography"];

                    featFilter = new QueryFilterClass();

                    featFilter.WhereClause = "FeatureGUID= '" + LinkedID + "'";

                    featCur = geoClass.Search(featFilter, false);

                    feat = featCur.NextFeature();
                }


                int fi = feat.Fields.FindField(PropFromTable);
                if (feat != null &&  fi >= 0)
                {
                    res = feat.Value[fi];
                }



                Marshal.ReleaseComObject(featCur);
            }
            return res;
        }

        public static string GetLegArincType(string ChartElemenName, string LinkedID)
        {
            string res = null;
            string FCN = "RouteSegmentCartography";

            FCN = GetLinkedGeometryFeatureClassName(ChartElemenName);

            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(FCN))
            {
                IFeatureClass geoClass = SigmaDataCash.AnnotationLinkedGeometryList[FCN];

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "FeatureGUID= '" + LinkedID + "'";

                IFeatureCursor featCur = geoClass.Search(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    if (FCN.StartsWith("ProcedureLegs"))
                    {
                        res = feat.get_Value(feat.Fields.FindField("legTypeARINC")).ToString();
                    }
                }

                Marshal.ReleaseComObject(featCur);
            }
            return res;
        }


        public static string GetLinkedGeometryFeatureClassName(string ChartElemenName)
        {
            string FCN = "RouteSegmentCartography";
            switch (ChartElemenName)
            {
                case ("RouteSegment_ValMagTrack"):
                case ("RouteSegment_ValReversMagTrack"):
                case ("RouteSegment_UpperLowerLimit"):
                case ("RouteSegment_sign"):
                    FCN = "RouteSegmentCartography";
                    break;
                case ("DesignatedPoint"):
                case ("TextArrow"):
                //case ("ProcedureLegHeight"):
                case ("SigmaCollout_Designatedpoint"):
                case ("DesignatedPoint_Simple"):
                case ("FC_RadialDistance"):
                case ("FC_Height"):
                    FCN = "DesignatedPointCartography";
                    break;
                case ("Navaids"):
                case ("SigmaCollout_Navaid"):
                case ("GlidePath_Navaid"):
                    FCN = "NavaidsCartography";
                    break;
                case ("Airspace"):
                case ("Airspace_Simple"):
                case ("SigmaCollout_Airspace"):
                case ("Airspace_Class"):
                case ("SectorAirspace"):
                case ("ATZ_ATZP_Airspace"):
                case ("CTR_CTRP_Airspace"):
                case ("TMA_TMAP_Airspace"):
                case ("TIZ_Airspace"):
                case ("TIA_Airspace"):
                case ("FIS_Airspace"):
                case ("SECTOR_SECTORC_Airspace"):
                case ("R_D_P_Airspace"):
                case ("R_D_P_AMC_Airspace"):
                case ("TRA_TSA_Airspace"): 
                case ("PROTECT_Airspace"): 
                case ("AOR_Airspace"):
                    FCN = "AirspaceC";
                    break;
                case ("BirdElement"):
                    FCN = "AirspaceCartography";
                    break;
                case ("AMA_Text"):
                    FCN = "AMEA";
                    break;
                case ("ProcedureLegLength"):
                case ("ProcedureLegSpeed"):
                case ("ProcedureLegCourse"):
                case ("ProcedureLegName"):
                case ("ProcedureLegHeight"):
                case ("NoneScale"):
                    FCN = "ProcedureLegsCartography";
                    break;
                case ("ILSGlidePath"):
                    FCN = "ProcedureLegsAnnoILSCartography";
                    break;
                case ("HoldingPattern"):
                case ("HoldingPatternInboundCource"):
                case ("HoldingPatternOutboundCource"):
                    FCN = "HoldingCartography";
                    break;
                case ("AngleIndication"):
                case ("DistanceIndication"):
                    FCN = "FacilityMakeUpCartography";
                    break;
                case ("GeoBorder_name"):
                    FCN = "GeoBorderCartography";
                    break;
                case ("Airport"):
                case ("AirportCartography"):
                    FCN = "AirportCartography";
                    break;
                case ("VerticalStructurePartElev"):
                case ("VerticalStructurePartHeight"):
                    FCN = "VerticalStructurePointCartography";
                    break;
                case ("IsogonalLines"):
                    FCN = "IsogonalLinesCartography";
                    break;
                case ("RunwayElement"):
                case ("ParkingDockingRunwayElement"):
                    FCN = "RunwayElementCartography";
                    break;
                case ("GuidanceLineElement"):
                    FCN = "GuidanceLineCartography";
                    break;
                case ("AircraftStandElement"):
                    FCN = "AircraftStandCartography";
                    break;
                case ("RunwayStripElement"):
                    FCN = "RunwayProtectAreaCartography";
                    break;
                case ("RunwayTHRElement"):
                case ("RunwayTDZElement"):
                case ("RDNElement"):
                    FCN = "RunwayDirectionCenterLinePointCartography";
                    break;
                case ("RunwayTORAElement"):
                    FCN = "DecorLineCartography";
                    break;
                case ("CheckpointElement"):
                    FCN = "CheckpointCartography";
                    break;
                case ("RunwayVisulRangeElement"):
                    FCN = "LightElementCartography";
                    break;
                case ("AirportHotSpotElement"):
                    FCN = "AirportHotSpotCartography";
                    break;
                case ("VerticalStructureElement"):
                    FCN = "VertStructureSurfaceCartography";
                    break;
                case ("VerticalStructureElementPoint"):
                case ("VerticalStructureElementElev"):
                case ("VerticalStructureElementHeight"):
                    FCN = "VerticalStructurePointCartography";
                    break;
                case ("FreqAreaElement"):
                    FCN = "RadioFrequencyAreaCartography";
                    break;
                case ("FreeAnno"):
                    FCN = "DecorPointCartography";
                    break;
                case ("CircleDistace"):
                    FCN = "DecorPolygonCartography";
                    break;
                default:
                    FCN = "RouteSegmentCartography";
                    break;
            }

            return FCN;
        }

        private static int GetLinkedFeatureID(string ChartElemenName, string LinkedID)
        {
            int res = -1;
            string FCN = "RouteSegmentCartography";
            switch (ChartElemenName)
            {
                case ("RouteSegment_ValMagTrack"):
                case ("RouteSegment_ValReversMagTrack"):
                case ("RouteSegment_UpperLowerLimit"):
                case ("RouteSegment_sign"):
                    FCN = "RouteSegmentCartography";
                    break;
                case ("DesignatedPoint"):
                    FCN = "DesignatedPointCartography";
                    break;
                case ("Navaids"):
                case ("SigmaCollout_Navaid"):
                case ("GlidePath_Navaid"):
                    FCN = "NavaidsCartography";
                    break;
                case ("Airspace"):
                    FCN = "AirspaceC";
                    break;
                case ("AMA"):
                    FCN = "AMEA";
                    break;
                case ("ProcedureLegLength"):
                case ("ProcedureLegSpeed"):
                case ("ProcedureLegCourse"):
                case ("ProcedureLegName"):
                case ("ProcedureLegHeight"):
                    FCN = "ProcedureLegsCartography";
                    break;
                case ("HoldingPattern"):
                    FCN = "HoldingCartography";
                    break;
                case ("Airport"):
                    FCN = "AirportCartography";
                    break;
                case ("IsogonalLines"):
                    FCN = "IsogonalLinesCartography";
                    break;
                default:
                    FCN = "RouteSegmentCartography";
                    break;
            }


            if (SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(FCN))
            {
                IFeatureClass geoClass = SigmaDataCash.AnnotationLinkedGeometryList[FCN];

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "FeatureGUID= '" + LinkedID + "'";

                IFeatureCursor featCur = geoClass.Search(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    res = feat.OID;
                }

                Marshal.ReleaseComObject(featCur);
            }
            return res;
        }

        public static string DefinelayerName(string chartEl)
        {
            string res = chartEl;
            switch (chartEl)
            {
                case ("RouteSegment_ValMagTrack"):
                case ("HoldingPatternInboundCource"):
                case ("HoldingPatternOutboundCource"):
                    res = "RouteSegmentAnnoMagTrack";
                    break;
                case ("RouteSegment_ValReversMagTrack"):
                    res = "RouteSegmentAnnoReverseMagTrack";
                    break;
                case ("RouteSegment_sign"):
                    res = "RouteSegmentAnnoSign";
                    break;
                case ("RouteSegment_UpperLowerLimit"):
                    res = "RouteSegmentAnnoLimits";
                    break;
                case ("DesignatedPoint"):
                case ("HoldingPattern"):
                case ("SigmaCollout_Designatedpoint"):
                case ("DesignatedPoint_Simple"):
                case ("FC_RadialDistance"):
                case ("FC_Height"):
                    res = "DesignatedPointAnno";
                    break;
                case ("Navaids"):
                case ("SigmaCollout_Navaid"):
                case ("GlidePath_Navaid"):
                    res = "NavaidsAnno";
                    break;
                case ("Airspace"):
                case ("Airspace_Simple"):
                case ("SigmaCollout_Airspace"):
                case ("Airspace_Class"):
                case ("BirdElement"):
                case ("SectorAirspace"):
                case ("ATZ_ATZP_Airspace"):
                case ("CTR_CTRP_Airspace"):
                case ("TMA_TMAP_Airspace"):
                case ("TIZ_Airspace"):
                case ("TIA_Airspace"):
                case ("FIS_Airspace"):
                case ("SECTOR_SECTORC_Airspace"):
                case ("R_D_P_Airspace"):
                case ("R_D_P_AMC_Airspace"):
                case ("TRA_TSA_Airspace"):
                case ("PROTECT_Airspace"): 
                case ("AOR_Airspace"): 
                    res = "AirspaceAnno";
                    break;
                case ("AMA_Text"):
                    res = "AMAEAnno";
                    break;
                case ("ProcedureLegLength"):
                case ("NoneScale"):
                    res = "ProcedureLegsAnnoLength";
                    break;
                case ("ProcedureLegCourse"):
                    res = "ProcedureLegsAnnoCourse";
                    break;
                case ("ILSGlidePath"):
                    res = "ProcedureLegsAnnoILSCartography";
                    break;
                case ("ProcedureLegHeight"):
                    res = "ProcedureLegsAnnoHeight";
                    break;
                case ("ProcedureLegName"):
                    res = "ProcedureLegsAnnoLegName";
                    break;
                case ("ProcedureLegSpeed"):
                    res = "ProcedureLegsAnnoSpeedLimit";
                    break;
                case ("AngleIndication"):
                case ("DistanceIndication"):
                    res = "FacilityMakeUpAnno";
                    break;
                case ("GeoBorder_name"):
                    res = "GeoBorderAnno";
                    break;
                case ("Airport"):
                    res = "AirportAnnoCartography";
                    break;
                case ("VerticalStructurePartElev"):
                case ("VerticalStructurePartHeight"):
                    res = "VerticalStructureAnnoCartography";
                    break;
                case ("VerticalStructureElement"):
                case ("VerticalStructureElementPoint"):
                case ("VerticalStructureElementElev"):
                case ("VerticalStructureElementHeight"):
                    res = "VertStructureSurfaceAnno";
                    break;
                case ("AirportHotSpotElement"):
                    res = "AirportHotSpot";
                    break;
                case ("RunwayElement"):
                case ("ParkingDockingRunwayElement"):
                    res = "RunwayElement";
                    break;
                case ("GuidanceLineElement"):
                    res = "GuidanceLine";
                    break;
                case ("RunwayStripElement"):
                    res = "RunwayProtectArea";
                    break;
                case ("RunwayTHRElement"):
                case ("RunwayTDZElement"):
                case ("RunwayTORAElement"):
                case ("RDNElement"):
                    res = "RunwayDirectionCenterLinePoint";
                    break;
                case ("CheckpointElement"):
                    res = "Checkpoint";
                    break;
                case ("RunwayVisulRangeElement"):
                    res = "LightElement";
                    break;
                case ("AircraftStandElement"):
                    res = "AircraftStand";
                    break;
                case ("FreqAreaElement"):
                    res = "RadioFrequencyAreaAnno";
                    break;
                case ("CircleDistace"):
                    res = "FreeAnno";
                    break;

                case ("Mirror"):
                    res = "Mirror";
                    break;//
            }

            return res;
        }


        public static int UpdateSingleElementToDataSet<T>(string ElementName, string pdmElementID, IElement mapElem, ref T chartEl, bool UpdateMirrorFlag = true)
        {
            IFeatureCursor featCur = null;
            try
            {

               

                IFeatureClass featureClass = GetLinkedFeatureClass(ElementName);

                if (featureClass == null) return -1;

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + pdmElementID + "'";

                featCur = featureClass.Update(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    /// код ниже необходим для конвертации аннотаций sigma_collaut из старого формата - в новый
                    /// при необходимости убрать коментарии и закоментировать код от А до Z
                    //IGeometry gm = (feat.Extent as IArea).Centroid;
                    //mapElem.Geometry = gm;

                    try
                    {
                        //A
                        IElement OldEl = ((IAnnotationFeature)feat).Annotation;
                        IGeometry gm = OldEl.Geometry;

                        if (gm == null || gm.IsEmpty)
                        {
                            IPoint _pp = new PointClass { X = (chartEl as ChartElement_SimpleText).Anchor.X , Y = (chartEl as ChartElement_SimpleText).Anchor.Y};
                            gm = _pp;
                        }

                        if (OldEl is IGroupElement)
                        {
                            if ((mapElem is IGroupElement) && ((IGroupElement)mapElem).ElementCount > 2)
                                gm = (((IGroupElement)OldEl).get_Element(1)).Geometry;
                            else
                            {
                                gm = (((IGroupElement)OldEl).get_Element(0)).Geometry;

                            }
                        }

                        if (mapElem is IGroupElement)
                        {
                            IGroupElement GrEl = mapElem as IGroupElement;
                            for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                            {
                                GrEl.get_Element(i).Geometry = gm;
                            }
                        }
                        else
                            mapElem.Geometry = gm;
                        //Z
                    }
                    catch
                    {
                        IGeometry gm = (feat.Extent as IArea).Centroid;
                        mapElem.Geometry = gm;
                    }
                    ((IAnnotationFeature)feat).Annotation = mapElem;



                    int fID = feat.Fields.FindField("ObjectType");
                    string ObjType = chartEl.GetType().Name;//feat.get_Value(fID).ToString();
                    ///
                    feat.set_Value(fID, ObjType);


                    /////
                    fID = feat.Fields.FindField("PdmUID");
                    feat.set_Value(fID, (chartEl as ChartElement_SimpleText).LinckedGeoId);
                    //////


                    fID = feat.Fields.FindField("OBJ");
                    string objSer = "";
                    if (fID > 0)
                    {
                        switch (ObjType)
                        {
                            case ("ChartElement_SimpleText"):
                                objSer = SerializeObject(chartEl as ChartElement_SimpleText);
                                break;
                            case ("ChartElement_RouteDesignator"):
                                objSer = SerializeObject(chartEl as ChartElement_RouteDesignator);
                                break;
                            case ("ChartElement_BorderedText"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText);
                                break;
                            case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom);
                                break;
                            case ("ChartElement_BorderedText_Collout"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText_Collout);
                                break;
                            case ("ChartElement_SigmaCollout_Navaid"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid);
                                break;
                            case ("ChartElement_MarkerSymbol"):
                                objSer = SerializeObject(chartEl as ChartElement_MarkerSymbol);
                                break;
                            case ("ChartElement_TextArrow"):
                                objSer = SerializeObject(chartEl as ChartElement_TextArrow);
                                break;
                            case ("ChartElement_Radial"):
                                objSer = SerializeObject(chartEl as ChartElement_Radial);
                                break;
                            case ("ChartElement_SigmaCollout_Designatedpoint"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint);
                                break;
                            case ("ChartElement_SigmaCollout_Airspace"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace);
                                break;
                            case ("ChartElement_SigmaCollout_AccentBar"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar);
                                break;
                            case ("ChartElement_ILSCollout"):
                                objSer = SerializeObject(chartEl as ChartElement_ILSCollout);
                                break;
                            default:
                                break;
                        }

                        feat.set_Value(fID, objSer);

                    }


                    feat.Store();

                    if (UpdateMirrorFlag) UpdateMirror(mapElem, pdmElementID, objSer);

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (featCur != null) Marshal.ReleaseComObject(featCur);

            }


            return 1;

        }

        public static int UpdateILSGlidePathElementToDataSet<T>(string ElementName, string pdmElementID, IElement mapElem, IGeometry gm, ref T chartEl, bool UpdateMirrorFlag = true)
        {
            IFeatureCursor featCur = null;
            try
            {

                IFeatureClass featureClass = GetLinkedFeatureClass(ElementName);

                if (featureClass == null) return -1;

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + pdmElementID + "'";

                featCur = featureClass.Update(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {

                    mapElem.Geometry = gm;

                    ((IAnnotationFeature)feat).Annotation = mapElem;


                    int fID = feat.Fields.FindField("ObjectType");
                    string ObjType = chartEl.GetType().Name;//feat.get_Value(fID).ToString();
                    ///
                    feat.set_Value(fID, ObjType);

                    fID = feat.Fields.FindField("OBJ");
                    string objSer = "";
                    objSer = SerializeObject(chartEl as ChartElement_ILSCollout);

                    feat.set_Value(fID, objSer);

                    feat.Store();

                    if (UpdateMirrorFlag) UpdateMirror(mapElem, pdmElementID, objSer);

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (featCur != null) Marshal.ReleaseComObject(featCur);

            }


            return 1;

        }

        public static int UpdateSingleElementToDataSet<T>(string ElementName, string pdmElementID, IElement mapElem, ref T chartEl, IGeometry NewGeometry, bool UpdateMirrorFlag = true)
        {
            IFeatureCursor featCur = null;
            try
            {



                IFeatureClass featureClass = GetLinkedFeatureClass(ElementName);
                if (featureClass == null && (ElementName.StartsWith("HoldingPatternInboundCource") || ElementName.StartsWith("HoldingPatternOutboundCource")))
                    featureClass = ChartElementsManipulator.GetLinkedFeatureClass("RouteSegment_ValMagTrack");

                if (featureClass == null) return -1;

                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + pdmElementID + "'";

                featCur = featureClass.Update(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    int pdmID = feat.Fields.FindField("pdmUID");
                    IGeometry gm = NewGeometry;

                    if (mapElem is IGroupElement)
                    {
                        IGroupElement GrEl = mapElem as IGroupElement;
                        for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                        {
                            GrEl.get_Element(i).Geometry = gm;
                        }
                    }
                    else
                        mapElem.Geometry = gm;

                    ((IAnnotationFeature)feat).Annotation = mapElem;


                    int fID = feat.Fields.FindField("ObjectType");
                    string ObjType = feat.get_Value(fID).ToString();


                    fID = feat.Fields.FindField("OBJ");
                    string objSer = "";
                    if (fID > 0)
                    {
                        switch (ObjType)
                        {
                            case ("ChartElement_SimpleText"):
                                objSer = SerializeObject(chartEl as ChartElement_SimpleText);
                                break;
                            case ("ChartElement_RouteDesignator"):
                                objSer = SerializeObject(chartEl as ChartElement_RouteDesignator);
                                break;
                            case ("ChartElement_BorderedText"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText);
                                break;
                            case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom);
                                break;
                            case ("ChartElement_BorderedText_Collout"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText_Collout);
                                break;
                            case ("ChartElement_SigmaCollout_Navaid"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid);
                                break;
                            case ("ChartElement_MarkerSymbol"):
                                objSer = SerializeObject(chartEl as ChartElement_MarkerSymbol);
                                break;
                            case ("ChartElement_TextArrow"):
                                objSer = SerializeObject(chartEl as ChartElement_TextArrow);
                                break;
                            case ("ChartElement_Radial"):
                                objSer = SerializeObject(chartEl as ChartElement_Radial);
                                break;
                            case ("ChartElement_SigmaCollout_Designatedpoint"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint);
                                break;
                            case ("ChartElement_SigmaCollout_Airspace"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace);
                                break;
                            case ("ChartElement_SigmaCollout_AccentBar"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar);
                                break;
                            case ("ChartElement_ILSCollout"):
                                objSer = SerializeObject(chartEl as ChartElement_ILSCollout);
                                break;
                            default:
                                break;
                        }
                        feat.set_Value(fID, objSer);
                    }


                    feat.Store();

                    if (UpdateMirrorFlag) UpdateMirror(mapElem, pdmElementID, objSer);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (featCur != null) Marshal.ReleaseComObject(featCur);




            }


            return 1;

        }

        public static int UpdateSingleElement_Mirror<T>(string pdmElementID, IElement mapElem, ref T chartEl, IGeometry NewGeometry)
        {
            IFeatureCursor featCur = null;
            try
            {

                IFeatureClass featureClass = GetLinkedFeatureClass("Mirror");


                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + pdmElementID.ToString() + "'";

                featCur = featureClass.Update(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    //int pdmID = feat.Fields.FindField("pdmUID");
                    IGeometry gm = NewGeometry;

                    if (mapElem is IGroupElement)
                    {
                        IGroupElement GrEl = mapElem as IGroupElement;
                        for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                        {
                            GrEl.get_Element(i).Geometry = gm;
                        }
                    }
                    else
                        mapElem.Geometry = gm;

                    ((IAnnotationFeature)feat).Annotation = mapElem;


                    //int fID = feat.Fields.FindField("ObjectType");
                    string ObjType = chartEl.GetType().Name;


                    int fID = feat.Fields.FindField("OBJ");
                    string objSer = "";
                    if (fID > 0)
                    {
                        switch (ObjType)
                        {
                            case ("ChartElement_SimpleText"):
                                objSer = SerializeObject(chartEl as ChartElement_SimpleText);
                                break;
                            case ("ChartElement_RouteDesignator"):
                                objSer = SerializeObject(chartEl as ChartElement_RouteDesignator);
                                break;
                            case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom);
                                break;
                            case ("ChartElement_BorderedText"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText);
                                break;
                            case ("ChartElement_BorderedText_Collout"):
                                objSer = SerializeObject(chartEl as ChartElement_BorderedText_Collout);
                                break;
                            case ("ChartElement_SigmaCollout_Navaid"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid);
                                break;
                            case ("ChartElement_MarkerSymbol"):
                                objSer = SerializeObject(chartEl as ChartElement_MarkerSymbol);
                                break;
                            case ("ChartElement_TextArrow"):
                                objSer = SerializeObject(chartEl as ChartElement_TextArrow);
                                break;
                            case ("ChartElement_Radial"):
                                objSer = SerializeObject(chartEl as ChartElement_Radial);
                                break;
                            case ("ChartElement_SigmaCollout_Designatedpoint"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint);
                                break;
                            case ("ChartElement_SigmaCollout_Airspace"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace);
                                break;
                            case ("ChartElement_SigmaCollout_AccentBar"):
                                objSer = SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar);
                                break;
                            default:
                                break;
                        }
                        feat.set_Value(fID, objSer);
                    }


                    feat.Store();

                    //UpdateMirror(mapElem, pdmElementID, objSer);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (featCur != null) Marshal.ReleaseComObject(featCur);




            }


            return 1;

        }


        public static int UpdateGraphicsElementToDataSet<T>(string ElementName, string pdmElementID, IElement mapElem, ref T chartEl)
        {
            ICursor rowCur = null;
            try
            {


                ITable _table = (ITable)SigmaDataCash.AnnotationFeatureClassList["GraphicsElementsView"];

                IQueryFilter _Filter = new QueryFilterClass();

                _Filter.WhereClause = "AncorUID= '" + pdmElementID + "'";

                rowCur = _table.Update(_Filter, false);

                IRow _row = rowCur.NextRow();

                if (_row != null)
                {

                    int fID = _row.Fields.FindField("ObjectType");
                    string ObjType = _row.get_Value(fID).ToString();


                    fID = _row.Fields.FindField("OBJ");
                    if (fID > 0)
                    {
                        switch (ObjType)
                        {
                            case ("GraphicsChartElement_SafeArea"):
                                _row.set_Value(fID, SerializeObject(chartEl as GraphicsChartElement_SafeArea));
                                break;
                            case ("GraphicsChartElement_NorthArrow"):
                                _row.set_Value(fID, SerializeObject(chartEl as GraphicsChartElement_NorthArrow));
                                break;
                            default:
                                break;
                        }
                    }


                    _row.Store();

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(rowCur);




            }


            return 1;

        }

        public static void LoadAnnotation_LinkedGeomFeatureClasses(int ChartType, IWorkspaceEdit workspaceEdit)
        {

            switch (ChartType)
            {
                case (1):
                    LoadAnnotation_LinkedGeomFeatureClasses_Enroute(workspaceEdit);
                    break;

                case (2):
                case (4):
                case (5):
                case (13):
                    LoadAnnotation_LinkedGeomFeatureClasses_Terminal(workspaceEdit);
                    break;

                case (6):
                    LoadAnnotation_LinkedGeomFeatureClasses_PATC(workspaceEdit);
                    break;
                case (7):
                    LoadAnnotation_LinkedGeomFeatureClasses_AreaChart(workspaceEdit);
                    break;
                case (8):
                    LoadAnnotation_LinkedGeomFeatureClasses_AerodromeChart(workspaceEdit);
                    break;
                default:
                    break;
            }


        }

        public static void LoadAnnotation_LinkedGeomFeatureClasses(int ChartType, IWorkspaceEdit workspaceEdit, int MapScale)
        {

            switch (ChartType)
            {
                case (1):
                    LoadAnnotation_LinkedGeomFeatureClasses_Enroute(workspaceEdit, MapScale);
                    break;
                case (2):
                case (4):
                case (5):
                case (13):
                    LoadAnnotation_LinkedGeomFeatureClasses_Terminal(workspaceEdit, MapScale);
                    break;

                case (6):
                    LoadAnnotation_LinkedGeomFeatureClasses_PATC(workspaceEdit, MapScale);
                    break;
                case (7):
                    LoadAnnotation_LinkedGeomFeatureClasses_AreaChart(workspaceEdit, MapScale);
                    break;
                case (8):
                    LoadAnnotation_LinkedGeomFeatureClasses_AerodromeChart(workspaceEdit, MapScale);
                    break;
                default:
                    break;
            }


        }


        #region Aerodrome Anno Loader

        private static void LoadAnnotation_LinkedGeomFeatureClasses_AerodromeChart(IWorkspaceEdit workspaceEdit, int _scale)
        {
            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;

            //List<string> tmp = new List<string>();


            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;

                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;


                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {

                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID


                        if (Anno_featClass.Extension is IAnnoClass)
                        {
                            IAnnoClass pAnnoClass = (IAnnoClass)Anno_featClass.Extension;

                            IAnnoClassAdmin3 pACAdmin = (IAnnoClassAdmin3)pAnnoClass;
                            pACAdmin.ReferenceScale = _scale;//(double)mapScale;
                            pACAdmin.UpdateProperties();


                        }


                        switch (_aliasName)
                        {
                            //annotations
                            case ("AirportAnnoCartography"):
                            case ("AircraftStandAnno"):
                            case ("AircraftStandExtentAnno"):
                            case ("AirportHeliportExtentAnno"):
                            case ("AirportHotSpotAnno"):
                            case ("ApronElement"):
                            case ("CheckpointAnno"):
                            case ("DeicingAreaAnno"):
                            case ("GuidanceLineAnno"):
                            case ("LightElementAnno"):
                            case ("MarkingCurveAnno"):
                            case ("MarkingPointAnno"):
                            case ("MarkingSurfaceAnno"):
                            case ("NavaidsAnno"):
                            case ("NonMovementAreaAnno"):
                            case ("RadioCommunicationChanelAnno"):
                            case ("RoadAnno"):
                            case ("RunwayDirectionCenterLinePointAnno"):
                            case ("RunwayElementAnno"):
                            case ("RunwayProtectAreaAnno"):
                            case ("RunwayVisualRangeAnno"):
                            case ("TaxiHoldingPositionAnno"):
                            case ("TaxiwayElementAnno"):
                            case ("TouchDownLiftOffAimingPointAnno"):
                            case ("RunwayDirectionAnno"):
                            case ("TouchDownLiftOffAnno"):
                            case ("TouchDownLiftOffSafeAreaAnno"):
                            case ("UnitCartographyAnno"):
                            case ("VerticalStructurePointAnno"):
                            case ("VertStructureCurveAnno"):
                            case ("VertStructureSurfaceAnno"):
                            case ("WorkAreaAnno"):
                            case ("AirspaceAnno"):
                            case ("RadioFrequencyAreaAnno"):
                            case ("FreeAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);
                                break;

                            //geometry
                            case ("AirportCartography"):
                            case ("AircraftStandCartography"):
                            case ("AircraftStandExtentCartography"):
                            case ("AirportHotSpotCartography"):
                            case ("ApronElementCartography"):
                            case ("DeicingAreaCartography"):
                            case ("GuidanceLineCartography"):
                            case ("NonMovementAreaCartography"):
                            case ("RadioCommunicationChanelCartography"):
                            case ("RoadCartography"):
                            case ("NavaidsCartography"):
                            case ("VertStructureSurfaceCartography"):
                            case ("VerticalStructurePointCartography"):
                            case ("VertStructureCurveCartography"):
                            case ("RunwayVisualRangeCartography"):
                            case ("TaxiHoldingPositionCartography"):
                            case ("TouchDownLiftOffCartography"):
                            case ("TouchDownLiftOffAimingPointCartography"):
                            case ("TouchDownLiftOffSafeAreaCartography"):
                            case ("UnitCartography"):
                            case ("WorkAreaCartography"):
                            case ("AirportHeliportExtentCartography"):
                            case ("LightElementCartography"):
                            case ("RunwayDirectionCartography"):
                            case ("RunwayDirectionCenterLinePointCartography"):
                            case ("RunwayElementCartography"):
                            case ("RunwayProtectAreaCartography"):
                            case ("TaxiwayElementCartography"):
                            case ("CheckpointCartography"):
                            case ("MarkingCurveCartography"):
                            case ("MarkingPointCartography"):
                            case ("MarkingSurfaceCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                            case ("AirspaceCartography"):
                            case ("RadioFrequencyAreaCartography"):
                                {
                                    if (Anno_featClass.Fields.FindField("VisibleFlag") < 0)
                                    {
                                        UpdateFeatereClass_AddField(Anno_featClass, _aliasName, "VisibleFlag", esriFieldType.esriFieldTypeSmallInteger);
                                    }

                                    if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);
                                }
                                break;



                        }

                    }

                }

                ITable graphicsTable = ((IFeatureWorkspace)workspaceEdit).OpenTable("GraphicsElementsView");
                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey("GraphicsElementsView")) SigmaDataCash.AnnotationFeatureClassList.Add("GraphicsElementsView", graphicsTable);

                #endregion

                break;

            }



        }

        private static void LoadAnnotation_LinkedGeomFeatureClasses_AerodromeChart(IWorkspaceEdit workspaceEdit)
        {
            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;

            //List<string> tmp = new List<string>();


            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;

                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;


                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {

                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID


                        switch (_aliasName)
                        {
                            //annotations
                            case ("AirportAnnoCartography"):
                            case ("AircraftStandAnno"):
                            case ("AircraftStandExtentAnno"):
                            case ("AirportHeliportExtentAnno"):
                            case ("AirportHotSpotAnno"):
                            case ("ApronElement"):
                            case ("CheckpointAnno"):
                            case ("DeicingAreaAnno"):
                            case ("GuidanceLineAnno"):
                            case ("LightElementAnno"):
                            case ("MarkingCurveAnno"):
                            case ("MarkingPointAnno"):
                            case ("MarkingSurfaceAnno"):
                            case ("NavaidsAnno"):
                            case ("NonMovementAreaAnno"):
                            case ("RadioCommunicationChanelAnno"):
                            case ("RoadAnno"):
                            case ("RunwayDirectionCenterLinePointAnno"):
                            case ("RunwayElementAnno"):
                            case ("RunwayProtectAreaAnno"):
                            case ("RunwayVisualRangeAnno"):
                            case ("TaxiHoldingPositionAnno"):
                            case ("TaxiwayElementAnno"):
                            case ("TouchDownLiftOffAimingPointAnno"):
                            case ("RunwayDirectionAnno"):
                            case ("TouchDownLiftOffAnno"):
                            case ("TouchDownLiftOffSafeAreaAnno"):
                            case ("UnitCartographyAnno"):
                            case ("VerticalStructurePointAnno"):
                            case ("VertStructureCurveAnno"):
                            case ("VertStructureSurfaceAnno"):
                            case ("WorkAreaAnno"):
                            case ("AirspaceAnno"):
                            case ("RadioFrequencyAreaAnno"):
                            case ("FreeAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);
                                break;

                            //geometry
                            case ("AirportCartography"):
                            case ("AircraftStandCartography"):
                            case ("AircraftStandExtentCartography"):
                            case ("AirportHotSpotCartography"):
                            case ("ApronElementCartography"):
                            case ("DeicingAreaCartography"):
                            case ("GuidanceLineCartography"):
                            case ("NonMovementAreaCartography"):
                            case ("RadioCommunicationChanelCartography"):
                            case ("RoadCartography"):
                            case ("NavaidsCartography"):
                            case ("VertStructureSurfaceCartography"):
                            case ("VerticalStructurePointCartography"):
                            case ("VertStructureCurveCartography"):
                            case ("RunwayVisualRangeCartography"):
                            case ("TaxiHoldingPositionCartography"):
                            case ("TouchDownLiftOffCartography"):
                            case ("TouchDownLiftOffAimingPointCartography"):
                            case ("TouchDownLiftOffSafeAreaCartography"):
                            case ("UnitCartography"):
                            case ("WorkAreaCartography"):
                            case ("AirportHeliportExtentCartography"):
                            case ("LightElementCartography"):
                            case ("RunwayDirectionCartography"):
                            case ("RunwayDirectionCenterLinePointCartography"):
                            case ("RunwayElementCartography"):
                            case ("RunwayProtectAreaCartography"):
                            case ("TaxiwayElementCartography"):
                            case ("CheckpointCartography"):
                            case ("MarkingCurveCartography"):
                            case ("MarkingPointCartography"):
                            case ("MarkingSurfaceCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                            case ("AirspaceCartography"):
                            case ("RadioFrequencyAreaCartography"):
                                {
                                    if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);

                                }
                                break;



                        }

                    }

                }

                ITable graphicsTable = ((IFeatureWorkspace)workspaceEdit).OpenTable("GraphicsElementsView");
                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey("GraphicsElementsView")) SigmaDataCash.AnnotationFeatureClassList.Add("GraphicsElementsView", graphicsTable);

                #endregion

                break;

            }



        }


        #endregion

        #region PATC Anno Loader

        private static void LoadAnnotation_LinkedGeomFeatureClasses_PATC(IWorkspaceEdit workspaceEdit, int _scale)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;

                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {

                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID

                        if (Anno_featClass.Extension is IAnnoClass)
                        {
                            IAnnoClass pAnnoClass = (IAnnoClass)Anno_featClass.Extension;

                            IAnnoClassAdmin3 pACAdmin = (IAnnoClassAdmin3)pAnnoClass;
                            pACAdmin.ReferenceScale = _scale;//(double)mapScale;
                            pACAdmin.UpdateProperties();

                        }

                        switch (_aliasName)
                        {
                            //annotations
                            case ("IsogonalLinesAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey("IsogonalLinesAnno")) SigmaDataCash.AnnotationFeatureClassList.Add("IsogonalLinesAnno", Anno_featClass);
                                break;

                            //geometry
                            case ("IsogonalLinesCartography"):
                                if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey("IsogonalLinesCartography")) SigmaDataCash.AnnotationLinkedGeometryList.Add("IsogonalLinesCartography", Anno_featClass);
                                break;


                        }

                    }

                }

                #endregion

                break;

            }

        }

        private static void LoadAnnotation_LinkedGeomFeatureClasses_PATC(IWorkspaceEdit workspaceEdit)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;


                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {
                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID


                        switch (_aliasName)
                        {
                            case ("IsogonalLinesAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);

                                break;
                        }

                        switch (_aliasName)
                        {

                            case ("IsogonalLinesCartography"):
                                if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);

                                break;

                        }

                    }
                }

                #endregion

                break;

            }

            SigmaDataCash.AnnotationLinkedGeometryList.Add("GeoBorderCartography", ((IFeatureWorkspace)workspaceEdit).OpenFeatureClass("GeoBorder"));

        }

        #endregion

        #region Enroute Anno Loader     

        private static void LoadAnnotation_LinkedGeomFeatureClasses_Enroute(IWorkspaceEdit workspaceEdit, int _scale)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;


                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {
                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID

                        if (Anno_featClass.Extension is IAnnoClass)
                        {
                            IAnnoClass pAnnoClass = (IAnnoClass)Anno_featClass.Extension;

                            IAnnoClassAdmin3 pACAdmin = (IAnnoClassAdmin3)pAnnoClass;
                            pACAdmin.ReferenceScale = _scale;//(double)mapScale;
                            pACAdmin.UpdateProperties();
                        }

                        switch (_aliasName)
                        {
                            case ("AirspaceAnno"):
                            case ("DesignatedPointAnno"):
                            case ("NavaidsAnno"):
                            case ("RouteSegmentAnnoMagTrack"):
                            case ("RouteSegmentAnnoReverseMagTrack"):
                            case ("RouteSegmentAnnoSign"):
                            case ("RouteSegmentAnnoLimits"):
                            case ("AMAEAnno"):
                            case ("Mirror"):
                            case ("GeoBorderAnno"):
                            case ("AirportAnnoCartography"):
                            case ("FreeAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);

                                break;
                        }

                        switch (_aliasName)
                        {

                            case ("AirspaceC"):
                            case ("AirspaceB"):
                            case ("DesignatedPointCartography"):
                            case ("NavaidsCartography"):
                            case ("RouteSegmentCartography"):
                            case ("HoldingCartography"):
                            case ("AMEA"):
                            case ("AirportCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                                {
                                    if (Anno_featClass.Fields.FindField("VisibleFlag") < 0)
                                    {
                                        UpdateFeatereClass_AddField(Anno_featClass, _aliasName, "VisibleFlag", esriFieldType.esriFieldTypeSmallInteger);
                                    }
                                    if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);
                                }
                                break;

                        }

                    }
                }

                #endregion

                break;

            }

            SigmaDataCash.AnnotationLinkedGeometryList.Add("GeoBorderCartography", ((IFeatureWorkspace)workspaceEdit).OpenFeatureClass("GeoBorder"));

            SigmaDataCash.AnnotationLinkedGeometryList = SigmaDataCash.AnnotationLinkedGeometryList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            SigmaDataCash.AnnotationFeatureClassList = SigmaDataCash.AnnotationFeatureClassList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        }

        public static void SynchronizeMirror(ChartElement_SimpleText sigmaEl)
        {
            #region отразить текстовую инфо в зеркальном элементе

            IFeatureCursor featCur = null;
            try
            {
                AbstractChartElement mirrorEl = null;
                IFeatureClass featureClass = GetLinkedFeatureClass("Mirror");


                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + sigmaEl.Id.ToString() + "'";

                featCur = featureClass.Search(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    string ObjType = sigmaEl.GetType().Name;
                    int fID = feat.Fields.FindField("OBJ");

                    if (fID > 0)
                    {
                        switch (ObjType)
                        {
                            case ("ChartElement_SimpleText"):
                                mirrorEl = DeserializeObject<ChartElement_SimpleText>(feat.get_Value(fID).ToString()) as ChartElement_SimpleText;
                                break;
                            case ("ChartElement_RouteDesignator"):
                                mirrorEl = DeserializeObject<ChartElement_RouteDesignator>(feat.get_Value(fID).ToString()) as ChartElement_RouteDesignator;
                                break;
                            case ("ChartElement_BorderedText"):
                                mirrorEl = DeserializeObject<ChartElement_BorderedText>(feat.get_Value(fID).ToString()) as ChartElement_BorderedText;
                                break;
                            case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                                mirrorEl = DeserializeObject<ChartElement_BorderedText_Collout_CaptionBottom>(feat.get_Value(fID).ToString()) as ChartElement_BorderedText_Collout_CaptionBottom;
                                break;
                            case ("ChartElement_BorderedText_Collout"):
                                mirrorEl = DeserializeObject<ChartElement_BorderedText_Collout>(feat.get_Value(fID).ToString()) as ChartElement_BorderedText_Collout;
                                break;
                            case ("ChartElement_SigmaCollout_Navaid"):
                                mirrorEl = DeserializeObject<ChartElement_SigmaCollout_Navaid>(feat.get_Value(fID).ToString()) as ChartElement_SigmaCollout_Navaid;
                                break;
                            case ("ChartElement_MarkerSymbol"):
                                mirrorEl = DeserializeObject<ChartElement_MarkerSymbol>(feat.get_Value(fID).ToString()) as ChartElement_MarkerSymbol;
                                break;
                            case ("ChartElement_TextArrow"):
                                mirrorEl = DeserializeObject<ChartElement_TextArrow>(feat.get_Value(fID).ToString()) as ChartElement_TextArrow;
                                break;
                            case ("ChartElement_Radial"):
                                mirrorEl = DeserializeObject<ChartElement_Radial>(feat.get_Value(fID).ToString()) as ChartElement_Radial;
                                break;
                            case ("ChartElement_SigmaCollout_Designatedpoint"):
                                mirrorEl = DeserializeObject<ChartElement_SigmaCollout_Designatedpoint>(feat.get_Value(fID).ToString()) as ChartElement_SigmaCollout_Designatedpoint;
                                break;
                            case ("ChartElement_SigmaCollout_Airspace"):
                                mirrorEl = DeserializeObject<ChartElement_SigmaCollout_Airspace>(feat.get_Value(fID).ToString()) as ChartElement_SigmaCollout_Airspace;
                                break;
                            case ("ChartElement_SigmaCollout_AccentBar"):
                                mirrorEl = DeserializeObject<ChartElement_SigmaCollout_AccentBar>(feat.get_Value(fID).ToString()) as ChartElement_SigmaCollout_AccentBar;
                                break;
                            case ("ChartElement_ILSCollout"):
                                mirrorEl = DeserializeObject<ChartElement_ILSCollout>(feat.get_Value(fID).ToString()) as ChartElement_ILSCollout;
                                break;
                            default:
                                break;
                        }

                    }


                    if (mirrorEl != null)
                    {
                        if (ArenaStaticProc.HasProperty(mirrorEl, "TextContents"))
                            CopyTestContent(sigmaEl.TextContents, ((ChartElement_SimpleText)mirrorEl).TextContents);
                        if (ArenaStaticProc.HasProperty(mirrorEl, "CaptionTextLine"))
                            CopyTestContent(sigmaEl.TextContents, ((ChartElement_BorderedText_Collout_CaptionBottom)mirrorEl).CaptionTextLine);
                        if (ArenaStaticProc.HasProperty(mirrorEl, "BottomTextLine"))
                            CopyTestContent(sigmaEl.TextContents, ((ChartElement_BorderedText_Collout_CaptionBottom)mirrorEl).BottomTextLine);
                        if (ArenaStaticProc.HasProperty(mirrorEl, "RouteDesignatorSource"))
                            CopyTestContent(sigmaEl.TextContents, ((ChartElement_RouteDesignator)mirrorEl).RouteDesignatorSource);
                    }

                    string objSer = "";
                    switch (ObjType)
                    {
                        case ("ChartElement_SimpleText"):
                            objSer = SerializeObject(mirrorEl as ChartElement_SimpleText);
                            break;
                        case ("ChartElement_RouteDesignator"):
                            objSer = SerializeObject(mirrorEl as ChartElement_RouteDesignator);
                            break;
                        case ("ChartElement_BorderedText"):
                            objSer = SerializeObject(mirrorEl as ChartElement_BorderedText);
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            objSer = SerializeObject(mirrorEl as ChartElement_BorderedText_Collout_CaptionBottom);
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            objSer = SerializeObject(mirrorEl as ChartElement_BorderedText_Collout);
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            objSer = SerializeObject(mirrorEl as ChartElement_SigmaCollout_Navaid);
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            objSer = SerializeObject(mirrorEl as ChartElement_MarkerSymbol);
                            break;
                        case ("ChartElement_TextArrow"):
                            objSer = SerializeObject(mirrorEl as ChartElement_TextArrow);
                            break;
                        case ("ChartElement_Radial"):
                            objSer = SerializeObject(mirrorEl as ChartElement_Radial);
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            objSer = SerializeObject(mirrorEl as ChartElement_SigmaCollout_Designatedpoint);
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            objSer = SerializeObject(mirrorEl as ChartElement_SigmaCollout_Airspace);
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            objSer = SerializeObject(mirrorEl as ChartElement_SigmaCollout_AccentBar);
                            break;
                        case ("ChartElement_ILSCollout"):
                            objSer = SerializeObject(mirrorEl as ChartElement_ILSCollout);
                            break;
                        default:
                            break;
                    }


                    IElement mapElem = (IElement)mirrorEl.ConvertToIElement();
                    UpdateMirror(mapElem, sigmaEl.Id.ToString(), objSer);

                }
            }
            catch { }
            finally
            {
                Marshal.ReleaseComObject(featCur);

            }

            #endregion

        }

        public static void CopyTestContent(List<List<AncorChartElementWord>> SourceTextContents, List<List<AncorChartElementWord>> MirrorTextContents)
        {
            if (SourceTextContents == null || MirrorTextContents == null) return;

            for (int i = 0; i < SourceTextContents.Count; i++)
            {
                var srcLn = SourceTextContents[i];

                if (i > MirrorTextContents.Count) break;
                var mrrLn = MirrorTextContents[i];

                for (int j = 0; j < srcLn.Count; j++)
                {
                    var srcWrd = srcLn[j];

                    if (j > mrrLn.Count) break;
                    var mrrWrd = mrrLn[j];

                    mrrWrd.TextValue = srcWrd.TextValue;

                }
            }
        }


        private static void LoadAnnotation_LinkedGeomFeatureClasses_Enroute(IWorkspaceEdit workspaceEdit)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;


                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {
                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID





                        switch (_aliasName)
                        {
                            case ("AirspaceAnno"):
                            case ("DesignatedPointAnno"):
                            case ("NavaidsAnno"):
                            case ("RouteSegmentAnnoMagTrack"):
                            case ("RouteSegmentAnnoReverseMagTrack"):
                            case ("RouteSegmentAnnoSign"):
                            case ("RouteSegmentAnnoLimits"):
                            case ("AMAEAnno"):
                            case ("Mirror"):
                            case ("GeoBorderAnno"):
                            case ("AirportAnnoCartography"):
                            case ("FreeAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);

                                break;
                        }

                        switch (_aliasName)
                        {

                            case ("AirspaceC"):
                            case ("AirspaceB"):
                            case ("DesignatedPointCartography"):
                            case ("NavaidsCartography"):
                            case ("RouteSegmentCartography"):
                            case ("HoldingCartography"):
                            case ("AMEA"):
                            case ("AirportCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                                if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);

                                break;

                        }

                    }
                }

                #endregion

                break;

            }

            SigmaDataCash.AnnotationLinkedGeometryList.Add("GeoBorderCartography", ((IFeatureWorkspace)workspaceEdit).OpenFeatureClass("GeoBorder"));


            SigmaDataCash.AnnotationLinkedGeometryList = SigmaDataCash.AnnotationLinkedGeometryList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            SigmaDataCash.AnnotationFeatureClassList = SigmaDataCash.AnnotationFeatureClassList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        }

        #endregion

        #region Terminal Anno Loader

        private static void LoadAnnotation_LinkedGeomFeatureClasses_Terminal(IWorkspaceEdit workspaceEdit, int _scale)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);

           

            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;

                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {

                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID

                        

                        if (Anno_featClass.Extension is IAnnoClass)
                        {
                            IAnnoClass pAnnoClass = (IAnnoClass)Anno_featClass.Extension;

                            IAnnoClassAdmin3 pACAdmin = (IAnnoClassAdmin3)pAnnoClass;
                            pACAdmin.ReferenceScale = _scale;//(double)mapScale;
                            pACAdmin.UpdateProperties();

                        }

                        switch (_aliasName)
                        {
                            //annotations
                            case ("AirspaceAnno"):
                            case ("ProcedureLegsAnnoCourseCartography"):
                            case ("ProcedureLegsAnnoHeightCartography"):
                            case ("ProcedureLegsAnnoLengthCartography"):
                            case ("ProcedureLegsAnnoSpeedLimitCartography"):
                            case ("ProcedureLegsAnnoLegNameCartography"):
                            case ("ProcedureLegsAnnoILSCartography"):
                            case ("Mirror"):
                            case ("DesignatedPointAnno"):
                            case ("NavaidsAnno"):
                            case ("VerticalStructurePointAnno"):
                            case ("FacilityMakeUpAnno"):
                            case ("GeoBorderAnno"):
                            case ("AirportAnnoCartography"):
                            case ("VerticalStructureAnnoCartography"):
                            case ("AMAEAnno"):
                            case ("FreeAnno"):

                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);
                                break;

                            //geometry
                            case ("AirspaceC"):
                            case ("AirspaceB"):
                            case ("DesignatedPointCartography"):
                            case ("NavaidsCartography"):
                            case ("ProcedureLegsCartography"):
                            case ("HoldingCartography"):
                            case ("HoldingPath"):
                            case ("VerticalStructurePointCartography"):
                            case ("RunwayCartography"):
                            case ("FacilityMakeUpCartography"):
                            case ("AirportCartography"):
                            case ("AMEA"):
                            case ("GlidePathCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                                {
                                    if (Anno_featClass.Fields.FindField("VisibleFlag") < 0)
                                    {
                                        UpdateFeatereClass_AddField(Anno_featClass, _aliasName, "VisibleFlag", esriFieldType.esriFieldTypeSmallInteger);
                                    }
                                    if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);
                                }
                                
                                break;



                        }

                    }

                }

                

                ITable graphicsTable = ((IFeatureWorkspace)workspaceEdit).OpenTable("GraphicsElementsView");
                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey("GraphicsElementsView")) SigmaDataCash.AnnotationFeatureClassList.Add("GraphicsElementsView", graphicsTable);

                #endregion

                break;

            }

            SigmaDataCash.AnnotationLinkedGeometryList.Add("GeoBorderCartography", ((IFeatureWorkspace)workspaceEdit).OpenFeatureClass("GeoBorder"));

            SigmaDataCash.AnnotationLinkedGeometryList = SigmaDataCash.AnnotationLinkedGeometryList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            SigmaDataCash.AnnotationFeatureClassList = SigmaDataCash.AnnotationFeatureClassList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);


        }

        public static void UpdateFeatereClass_AddField2(IWorkspace wrkspc, IFeatureClass _featClass, string _featureClassName, string FieldEditName, esriFieldType fildTp, string _datasetName = "chart_pattern")
        {
            IEnumDataset enumDataset = wrkspc.Datasets[esriDatasetType.esriDTFeatureDataset];
            IDataset _pattern_dataset = null;
            while ((_pattern_dataset = enumDataset.Next()) != null)
            {
                if (_pattern_dataset.Name.ToLower().StartsWith(_datasetName)) break;

            }


            string shName = _featClass.ShapeFieldName;

            // Clone the fields from the feature class
            IFields oldFields = _featClass.Fields;
            IClone cloneSource = (IClone)oldFields;
            IClone cloneTarget = cloneSource.Clone();
            IFields fields = (IFields)cloneTarget;
            // QI the fields collection to the IFieldsEdit interface
            IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
            IField visibleField = new FieldClass();
            IFieldEdit visibleFieldEdit = (IFieldEdit)visibleField;
            visibleFieldEdit.Name_2 = FieldEditName;
            visibleFieldEdit.Type_2 = fildTp;
            visibleFieldEdit.IsNullable_2 = true;

            fieldsEdit.AddField(visibleFieldEdit);

            (_featClass as IDataset).Delete();

            IFeatureClassDescription fcDesc = new FeatureClassDescriptionClass();
            IObjectClassDescription ocDesc = (IObjectClassDescription)fcDesc;

            _featClass = ((IFeatureDataset)_pattern_dataset).CreateFeatureClass(_featureClassName, fields,
             ocDesc.InstanceCLSID, ocDesc.ClassExtensionCLSID, esriFeatureType.esriFTSimple, shName, "");

        }

        public static void UpdateFeatereClass_AddField(IFeatureClass _featClass, string _featureClassName, string FieldEditName, esriFieldType fildTp, string _datasetName = "chart_pattern")
        {
            IField visibleField = new FieldClass();
            IFieldEdit visibleFieldEdit = (IFieldEdit)visibleField;
            visibleFieldEdit.Name_2 = FieldEditName;
            visibleFieldEdit.Type_2 = fildTp;
            visibleFieldEdit.IsNullable_2 = true;

            try
            {
                //schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);

                _featClass.AddField(visibleFieldEdit);

            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                //schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
            }


        }

        private static void LoadAnnotation_LinkedGeomFeatureClasses_Terminal(IWorkspaceEdit workspaceEdit)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;


                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {

                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID

                        switch (_aliasName)
                        {
                            //annotations
                            case ("AirspaceAnno"):
                            case ("ProcedureLegsAnnoHeightCartography"):
                            case ("ProcedureLegsAnnoCourseCartography"):
                            case ("ProcedureLegsAnnoLengthCartography"):
                            case ("ProcedureLegsAnnoSpeedLimitCartography"):
                            case ("ProcedureLegsAnnoLegNameCartography"):
                            case ("ProcedureLegsAnnoILSCartography"):
                            case ("Mirror"):
                            case ("DesignatedPointAnno"):
                            case ("NavaidsAnno"):
                            case ("VerticalStructurePointAnno"):
                            case ("FacilityMakeUpAnno"):
                            case ("GeoBorderAnno"):
                            case ("AirportAnnoCartography"):
                            case ("VerticalStructureAnnoCartography"):
                            case ("AMAEAnno"):
                            case ("FreeAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);
                                break;

                            //geometry
                            case ("AirspaceC"):
                            case ("AirspaceB"):
                            case ("DesignatedPointCartography"):
                            case ("NavaidsCartography"):
                            case ("ProcedureLegsCartography"):
                            case ("HoldingCartography"):
                            case ("HoldingPath"):
                            case ("VerticalStructurePointCartography"):
                            case ("RunwayCartography"):
                            case ("FacilityMakeUpCartography"):
                            case ("AirportCartography"):
                            case ("AMEA"):
                            case ("GlidePathCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                                if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);
                                break;



                        }

                    }

                }

                ITable graphicsTable = ((IFeatureWorkspace)workspaceEdit).OpenTable("GraphicsElementsView");
                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey("GraphicsElementsView")) SigmaDataCash.AnnotationFeatureClassList.Add("GraphicsElementsView", graphicsTable);

                #endregion

                break;

            }

            SigmaDataCash.AnnotationLinkedGeometryList.Add("GeoBorderCartography", ((IFeatureWorkspace)workspaceEdit).OpenFeatureClass("GeoBorder"));
            SigmaDataCash.AnnotationLinkedGeometryList = SigmaDataCash.AnnotationLinkedGeometryList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            SigmaDataCash.AnnotationFeatureClassList = SigmaDataCash.AnnotationFeatureClassList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);

        }

        #endregion

        #region AreaChart Anno Loader     

        private static void LoadAnnotation_LinkedGeomFeatureClasses_AreaChart(IWorkspaceEdit workspaceEdit, int _scale)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;


                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {
                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID

                        if (Anno_featClass.Extension is IAnnoClass)
                        {
                            IAnnoClass pAnnoClass = (IAnnoClass)Anno_featClass.Extension;

                            IAnnoClassAdmin3 pACAdmin = (IAnnoClassAdmin3)pAnnoClass;
                            pACAdmin.ReferenceScale = _scale;//(double)mapScale;
                            pACAdmin.UpdateProperties();
                        }

                        switch (_aliasName)
                        {
                            case ("AirspaceAnno"):
                            case ("DesignatedPointAnno"):
                            case ("NavaidsAnno"):
                            case ("RouteSegmentAnnoMagTrack"):
                            case ("RouteSegmentAnnoReverseMagTrack"):
                            case ("RouteSegmentAnnoSign"):
                            case ("RouteSegmentAnnoLimits"):
                            case ("AMAEAnno"):
                            case ("Mirror"):
                            case ("GeoBorderAnno"):
                            case ("AirportAnnoCartography"):
                            case ("ProcedureLegsAnnoHeightCartography"):
                            case ("ProcedureLegsAnnoCourseCartography"):
                            case ("ProcedureLegsAnnoLengthCartography"):
                            case ("ProcedureLegsAnnoSpeedLimitCartography"):
                            case ("ProcedureLegsAnnoLegNameCartography"):
                            case ("VerticalStructurePointAnno"):
                            case ("VerticalStructureAnnoCartography"):
                            case ("FacilityMakeUpAnno"):
                            case ("FreeAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);

                                break;
                        }

                        switch (_aliasName)
                        {

                            case ("AirspaceC"):
                            case ("AirspaceB"):
                            case ("DesignatedPointCartography"):
                            case ("NavaidsCartography"):
                            case ("RouteSegmentCartography"):
                            case ("HoldingCartography"):
                            case ("AMEA"):
                            case ("AirportCartography"):
                            case ("VerticalStructureAnnoCartography"):
                            case ("ProcedureLegsCartography"):
                            case ("VerticalStructurePointCartography"):
                            case ("RunwayCartography"):
                            case ("FacilityMakeUpCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                                {
                                    if (Anno_featClass.Fields.FindField("VisibleFlag") < 0)
                                    {
                                        UpdateFeatereClass_AddField(Anno_featClass, _aliasName, "VisibleFlag", esriFieldType.esriFieldTypeSmallInteger);
                                    }
                                    if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);
                                }
                                break;

                        }

                    }
                }

                #endregion

                break;

            }

            SigmaDataCash.AnnotationLinkedGeometryList.Add("GeoBorderCartography", ((IFeatureWorkspace)workspaceEdit).OpenFeatureClass("GeoBorder"));

            SigmaDataCash.AnnotationLinkedGeometryList = SigmaDataCash.AnnotationLinkedGeometryList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            SigmaDataCash.AnnotationFeatureClassList = SigmaDataCash.AnnotationFeatureClassList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        }

        private static void LoadAnnotation_LinkedGeomFeatureClasses_AreaChart(IWorkspaceEdit workspaceEdit)
        {

            SigmaDataCash.AnnotationFeatureClassList = new Dictionary<string, object>();
            SigmaDataCash.AnnotationLinkedGeometryList = new Dictionary<string, IFeatureClass>();
            IEnumDatasetName enumDatasetName = (workspaceEdit as IWorkspace).get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName datasetName = null;
            while ((datasetName = enumDatasetName.Next()) != null)
            {
                if (!datasetName.Name.ToLower().StartsWith("chart_pattern")) continue;


                #region

                IEnumDatasetName enumSubDatasetName = datasetName.SubsetNames;
                while ((datasetName = enumSubDatasetName.Next()) != null)
                {
                    if ((datasetName as IName).Open() is IFeatureClass)
                    {
                        string _aliasName = ((datasetName as IName).Open() as IFeatureClass).AliasName;
                        IFeatureClass Anno_featClass = (datasetName as IName).Open() as IFeatureClass; //OBJECTID

                        switch (_aliasName)
                        {
                            case ("AirspaceAnno"):
                            case ("DesignatedPointAnno"):
                            case ("NavaidsAnno"):
                            case ("RouteSegmentAnnoMagTrack"):
                            case ("RouteSegmentAnnoReverseMagTrack"):
                            case ("RouteSegmentAnnoSign"):
                            case ("RouteSegmentAnnoLimits"):
                            case ("AMAEAnno"):
                            case ("Mirror"):
                            case ("GeoBorderAnno"):
                            case ("AirportAnnoCartography"):
                            case ("ProcedureLegsAnnoHeightCartography"):
                            case ("ProcedureLegsAnnoCourseCartography"):
                            case ("ProcedureLegsAnnoLengthCartography"):
                            case ("ProcedureLegsAnnoSpeedLimitCartography"):
                            case ("ProcedureLegsAnnoLegNameCartography"):
                            case ("VerticalStructurePointAnno"):
                            case ("VerticalStructureAnnoCartography"):
                            case ("FacilityMakeUpAnno"):
                            case ("FreeAnno"):
                                if (!SigmaDataCash.AnnotationFeatureClassList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationFeatureClassList.Add(_aliasName, Anno_featClass);

                                break;
                        }

                        switch (_aliasName)
                        {

                            case ("AirspaceC"):
                            case ("AirspaceB"):
                            case ("DesignatedPointCartography"):
                            case ("NavaidsCartography"):
                            case ("RouteSegmentCartography"):
                            case ("HoldingCartography"):
                            case ("AMEA"):
                            case ("AirportCartography"):
                            case ("VerticalStructureAnnoCartography"):
                            case ("ProcedureLegsCartography"):
                            case ("VerticalStructurePointCartography"):
                            case ("RunwayCartography"):
                            case ("FacilityMakeUpCartography"):
                            case ("DecorPointCartography"):
                            case ("DecorLineCartography"):
                            case ("DecorPolygonCartography"):
                                if (!SigmaDataCash.AnnotationLinkedGeometryList.ContainsKey(_aliasName)) SigmaDataCash.AnnotationLinkedGeometryList.Add(_aliasName, Anno_featClass);

                                break;

                        }

                    }
                }

                #endregion

                break;

            }

            SigmaDataCash.AnnotationLinkedGeometryList.Add("GeoBorderCartography", ((IFeatureWorkspace)workspaceEdit).OpenFeatureClass("GeoBorder"));

            SigmaDataCash.AnnotationLinkedGeometryList = SigmaDataCash.AnnotationLinkedGeometryList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            SigmaDataCash.AnnotationFeatureClassList = SigmaDataCash.AnnotationFeatureClassList.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
        }

        #endregion


        public static void LoadChartElements(IWorkspaceEdit workspaceEdit)
        {

            if (SigmaDataCash.ChartElementList != null) SigmaDataCash.ChartElementList.Clear();


            // if (SigmaDataCash.AnnotationFeatureClassList == null || SigmaDataCash.AnnotationFeatureClassList.Count == 0)
            LoadAnnotation_LinkedGeomFeatureClasses(SigmaDataCash.SigmaChartType, workspaceEdit);

            if (SigmaDataCash.AnnotationFeatureClassList == null) return;


            foreach (KeyValuePair<string, object> pair in SigmaDataCash.AnnotationFeatureClassList)
            {
                if (pair.Key.Contains("Mirror")) continue;
                else if (!pair.Key.Contains("GraphicsElementsView"))
                {
                    IFeatureClass Anno_featClass = (IFeatureClass)pair.Value;
                    LoadChartElementFromFeatureClass(Anno_featClass);
                }
                else
                {
                    ITable grTable = (ITable)pair.Value;
                    LoadChartElementFromFeatureClass(grTable);

                }

            }


        }

        private static void LoadChartElementFromFeatureClass(IFeatureClass Anno_featClass)
        {


            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = "OBJECTID >0";

            IFeatureCursor featCur = Anno_featClass.Search(featFilter, false);

            IFeature _Feature = null;
            while ((_Feature = featCur.NextFeature()) != null)
            {

                int fID1 = _Feature.Fields.FindField("OBJ");
                int fID2 = _Feature.Fields.FindField("ObjectType");
                if (fID1 >= 0 && fID2 >= 0)
                {
                    string ObjType = _Feature.get_Value(fID2).ToString();

                    switch (ObjType) //!
                    {
                        case ("ChartElement_SimpleText"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_SimpleText>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_RouteDesignator"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_RouteDesignator>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_BorderedText"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_BorderedText>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_BorderedText_Collout_CaptionBottom>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_BorderedText_Collout"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_BorderedText_Collout>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_SigmaCollout_Navaid"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_SigmaCollout_Navaid>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_MarkerSymbol"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_MarkerSymbol>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_TextArrow"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_TextArrow>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_Radial"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_Radial>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_SigmaCollout_Designatedpoint"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_SigmaCollout_Designatedpoint>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_SigmaCollout_Airspace"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_SigmaCollout_Airspace>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_SigmaCollout_AccentBar"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_SigmaCollout_AccentBar>(_Feature.get_Value(fID1).ToString()));
                            break;
                        case ("ChartElement_ILSCollout"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<ChartElement_ILSCollout>(_Feature.get_Value(fID1).ToString()));
                            break;
                        default:
                            break;
                    }

                }
            }

            Marshal.ReleaseComObject(featCur);
        }

        private static void LoadChartElementFromFeatureClass(ITable Grahics_Table)
        {


            IQueryFilter _Filter = new QueryFilterClass();

            _Filter.WhereClause = "OBJECTID >0";

            ICursor featCur = Grahics_Table.Search(_Filter, false);

            IRow _Row = null;
            while ((_Row = featCur.NextRow()) != null)
            {

                int fID1 = _Row.Fields.FindField("OBJ");
                int fID2 = _Row.Fields.FindField("ObjectType");
                if (fID1 >= 0 && fID2 >= 0)
                {
                    string ObjType = _Row.get_Value(fID2).ToString();

                    switch (ObjType)
                    {
                        case ("GraphicsChartElement_SafeArea"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<GraphicsChartElement_SafeArea>(_Row.get_Value(fID1).ToString()));
                            break;
                        case ("GraphicsChartElement_NorthArrow"):
                            SigmaDataCash.ChartElementList.Add(DeserializeObject<GraphicsChartElement_NorthArrow>(_Row.get_Value(fID1).ToString()));
                            break;
                        default:
                            break;
                    }

                }
            }

            Marshal.ReleaseComObject(featCur);
        }



        public static int HideSingleElement<T>(int Status, ref T chartEl)
        {
            IFeatureCursor featCur = null;
            try
            {
                AbstractChartElement cartoElement = chartEl as AbstractChartElement;
                string ElementName = cartoElement.Name;
                string pdmElementID = cartoElement.Id.ToString();

                IFeatureClass featureClass = GetLinkedFeatureClass(ElementName);

                if (featureClass == null && (ElementName.StartsWith("HoldingPatternInboundCource") || ElementName.StartsWith("HoldingPatternOutboundCource")))
                    featureClass = GetLinkedFeatureClass("RouteSegment_ValMagTrack");

                if (featureClass != null)
                {
                    IQueryFilter featFilter = new QueryFilterClass();

                    featFilter.WhereClause = "AncorUID= '" + pdmElementID + "'";

                    featCur = featureClass.Update(featFilter, false);

                    IFeature feat = featCur.NextFeature();

                    if (feat != null)
                    {

                        int fID = feat.Fields.FindField("Status");
                        feat.set_Value(fID, Status);

                        fID = feat.Fields.FindField("ObjectType");
                        string ObjType = feat.get_Value(fID).ToString();

                        fID = feat.Fields.FindField("OBJ");
                        if (fID > 0)
                        {
                            switch (ObjType)
                            {

                                case ("ChartElement_SimpleText"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SimpleText));
                                    break;
                                case ("ChartElement_RouteDesignator"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_RouteDesignator));
                                    break;
                                case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom));
                                    break;
                                case ("ChartElement_MarkerSymbol"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_MarkerSymbol));
                                    break;
                                case ("ChartElement_TextArrow"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_TextArrow));
                                    break;
                                case ("ChartElement_BorderedText"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_BorderedText));
                                    break;
                                case ("ChartElement_BorderedText_Collout"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_BorderedText_Collout));
                                    break;
                                case ("ChartElement_SigmaCollout_Navaid"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid));
                                    break;
                                case ("ChartElement_Radial"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_Radial));
                                    break;
                                case ("ChartElement_SigmaCollout_Designatedpoint"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint));
                                    break;
                                case ("ChartElement_SigmaCollout_Airspace"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace));
                                    break;
                                case ("ChartElement_SigmaCollout_AccentBar"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar));
                                    break;
                                case ("ChartElement_ILSCollout"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_ILSCollout));
                                    break;
                                default:
                                    break;


                            }
                        }

                        feat.Store();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (featCur != null) Marshal.ReleaseComObject(featCur);

            }


            return 1;
        }

        public static AbstractChartElement GetClickedChartElement(IMap pMap, int X, int Y, string frameName, bool Multi = false, bool LoadToPropertyGrid = true, bool UpdateAfterSelect = false)
        {
            ClearFeatureSelections(pMap);
            AbstractChartElement el = null;
            if (SigmaDataCash.SelectedChartElements == null) SigmaDataCash.SelectedChartElements = new List<AbstractChartElement>();
            if (!Multi) SigmaDataCash.SelectedChartElements.Clear();

            //bool searchInMirror = !frameName.StartsWith("LAYERS");


            // определяем workspaceEdit
            ILayer _Layer = EsriUtils.getLayerByName(pMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(pMap, "AirportCartography");
            if (_Layer == null) return null;

            var fc = ((IFeatureLayer)_Layer).FeatureClass;
            var workspaceEdit = (IWorkspaceEdit)fc.FeatureDataset.Workspace;


            //инициализаия
            var activeView = pMap as IActiveView;
            IPoint mapPoint = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            IPoint geoPoint = (IPoint)EsriUtils.ToGeo(mapPoint, pMap, (fc as IGeoDataset).SpatialReference);

            List<string> ancorUids = new List<string>();

            string ObjectReflection = "";
            string pdmUID = "";

            if (SigmaDataCash.AnnotationFeatureClassList == null || SigmaDataCash.AnnotationFeatureClassList.Count <= 0)
            {
                LoadChartElements(workspaceEdit);
                SigmaDataCash.SelectedChartElements = new List<AbstractChartElement>();

            }

            //находим идентификаторы объектов AncorUID 

            foreach (KeyValuePair<string, object> item in SigmaDataCash.AnnotationFeatureClassList)
            {
                if (!(item.Value is IFeatureClass)) continue;

                IFeatureClass Anno_featClass = (IFeatureClass)item.Value;
                string layerAliasName = Anno_featClass.AliasName;
                _Layer = EsriUtils.getLayerByName(pMap, layerAliasName);

                if (_Layer == null || !_Layer.Visible) continue;

                IFeature res = SearchBySpatialFilter(geoPoint, Anno_featClass, "OBJECTID >0 AND Status=0");
                if (res != null)
                {
                    int FI = res.Fields.FindField("AncorUID");
                    if (FI >= 0)
                    {
                        ancorUids.Add(res.get_Value(FI).ToString());

                        #region mistika
                        if (UpdateAfterSelect) //убрать условие в случае проблем
                        {

                            var obj = (from element in SigmaDataCash.ChartElementList where (element != null) && (element is ChartElement_SimpleText) && (((ChartElement_SimpleText)element).Id.ToString().StartsWith(res.get_Value(FI).ToString())) select element).FirstOrDefault();
                            if (obj != null)
                            {

                                SigmaDataCash.environmentWorkspaceEdit.StartEditing(false);
                                SigmaDataCash.environmentWorkspaceEdit.StartEditOperation();

                                AbstractChartElement cartoEl = (AbstractChartElement)obj;
                                IElement iEl = cartoEl.ConvertToIElement() as IElement;

                                if (cartoEl is ChartElement_SimpleText)
                                    ChartElementsManipulator.UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), iEl, ref cartoEl, false);


                                SigmaDataCash.environmentWorkspaceEdit.StopEditOperation();
                                SigmaDataCash.environmentWorkspaceEdit.StopEditing(true);

                            }
                        }
                        #endregion

                        SelectFeature(_Layer, res, Multi);

                        FI = res.Fields.FindField("PdmUID");
                        if (FI >= 0)
                        {
                            pdmUID = res.get_Value(FI).ToString();

                        }

                        if (!frameName.StartsWith("LAYERS"))
                        //if (frameName.StartsWith("SigmaFrame"))
                        {


                            FI = res.Fields.FindField("OBJ");
                            if (FI >= 0)
                            {
                                ObjectReflection = res.get_Value(FI).ToString();

                            }


                            
                            //AbstractChartElement cartoEl = (AbstractChartElement)obj;
                            //searchInMirror = searchInMirror && cartoEl.ReflectionHidden;
                        }

                        if (!Multi) break;
                    }



                }

            }



            //если список найденых идентификаиторов не пуст - подсветим Ancor объекты в дереве
            if (ancorUids.Count > 0)
            {

                if (LoadToPropertyGrid) el = SigmaDataCash.HighLightChartElement(ancorUids[0], ObjectReflection);
                if (el == null)
                    el = (AbstractChartElement)(from element in SigmaDataCash.ChartElementList where (element != null) && (((AbstractChartElement)element).Id.ToString().CompareTo(ancorUids[0]) == 0) select element).FirstOrDefault();

                if (!SigmaDataCash.SelectedChartElements.Contains(el))
                    SigmaDataCash.SelectedChartElements.Add(el);

                //заглушка
                el.LinckedGeoId = pdmUID.Length > 0? pdmUID : el.LinckedGeoId;
            }
            else //иначе все очищаем и выходим
            {
                //ClearFeatureSelections(pMap);
                SigmaDataCash.PutOutPropertyGrid();
                SigmaDataCash.SelectedChartElements.Clear();
            }

            return el;

        }

        public static AbstractChartElement GetClickedChartElement(IActiveView activeView, int X, int Y)
        {
            AbstractChartElement el = null; ;
            string UID;
            IPoint point = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            IEnumElement ELList = activeView.GraphicsContainer.LocateElements(point, 0);
            if (ELList != null)
            {

                ELList.Reset();
                IElement SelectedElement = ELList.Next();
                while (true && SelectedElement != null)
                {
                    if ((SelectedElement as IElementProperties3).Name.StartsWith("SigmaTable"))
                    {
                        IACProfilesTable tblForm = new IACProfilesTable();

                        #region Fill IACProfilesTables grids

                        activeView.GraphicsContainer.Reset();
                        IElementProperties docElementProperties2;
                        IElement dinamic_el = activeView.GraphicsContainer.Next();
                        List<IElement> tbls = new List<IElement>();

                        while (dinamic_el != null)
                        {
                            docElementProperties2 = dinamic_el as IElementProperties;

                            if (docElementProperties2.Name.StartsWith("SigmaTable_Distance"))
                            {
                                tblForm.DistanceTbl = dinamic_el;
                                tbls.Add(dinamic_el);
                            }
                            if (docElementProperties2.Name.StartsWith("SigmaTable_Speed"))
                            {
                                tblForm.SpeedTbl = dinamic_el;
                                tbls.Add(dinamic_el);
                            }
                            if (docElementProperties2.Name.StartsWith("SigmaTable_OCH"))
                            {
                                tblForm.OCHTbl = dinamic_el;
                                tbls.Add(dinamic_el);
                            }
                            if (docElementProperties2.Name.StartsWith("SigmaTable_Circling"))
                            {
                                //tblForm.CircleTbl = dinamic_el;
                            }

                            dinamic_el = activeView.GraphicsContainer.Next();
                        }

                        #endregion


                        if (tblForm.BuildForm() == DialogResult.OK)
                        {
                            foreach (var tbl in tbls)
                            {
                                activeView.GraphicsContainer.DeleteElement(tbl);

                            }
                            activeView.GraphicsContainer.AddElement(tblForm.DistanceTbl, 0);
                            activeView.GraphicsContainer.AddElement(tblForm.SpeedTbl, 0);
                            activeView.GraphicsContainer.AddElement(tblForm.OCHTbl, 0);
                        }

                        break;
                    }

                    else if ((SelectedElement as IElementProperties3).Name.StartsWith("Sigma") && (SelectedElement as IElementProperties3).Name.Split('_').Length > 2)
                    {
                        UID = (SelectedElement as IElementProperties3).Name.Split('_')[2];

                        el = SigmaDataCash.HighLightChartElement(UID);
                        if (el == null)
                            el = (AbstractChartElement)(from element in SigmaDataCash.ChartElementList where (element != null) && (((AbstractChartElement)element).Id.ToString().CompareTo(UID) == 0) select element).FirstOrDefault();

                        if (!SigmaDataCash.SelectedChartElements.Contains(el)) SigmaDataCash.SelectedChartElements.Add(el);

                        break;
                    }
                    SelectedElement = ELList.Next();
                }


            }

            return el;

        }

        public static void SelectFeature(ILayer _Layer, IFeature pFeature, bool MultiSelectFlag = false)
        {


            IFeatureSelection pSelect = (IFeatureSelection)_Layer;



            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "OBJECTID = " + pFeature.OID;

            if (MultiSelectFlag && SigmaDataCash.SelectedChartElements != null && SigmaDataCash.SelectedChartElements.Count > 0)
            {
                string f = "'" + pFeature.get_Value(pFeature.Fields.FindField("AncorUID")).ToString() + "'";
                foreach (AbstractChartElement item in SigmaDataCash.SelectedChartElements)
                {
                    f = "'" + item.Id.ToString() + "'," + f;
                }
                queryFilter.WhereClause = "AncorUID in (" + f + ")";
            }

            if (pSelect != null)
            {
                IColor selColor = new RgbColor();
                ((RgbColor)selColor).Red = 255;
                ((RgbColor)selColor).Green = 0;
                ((RgbColor)selColor).Blue = 0;

                ISimpleFillSymbol smplFill1 = new SimpleFillSymbol();

                smplFill1.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;

                ISimpleLineSymbol pSimpleLine1 = new SimpleLineSymbolClass();

                pSimpleLine1.Style = esriSimpleLineStyle.esriSLSDashDot;
                pSimpleLine1.Color = selColor;
                pSimpleLine1.Width = 1;

                smplFill1.Outline = pSimpleLine1;

                smplFill1.Color = selColor;

                pSelect.BufferDistance = 0.1; ;
                pSelect.SetSelectionSymbol = true;


                pSelect.SelectionSymbol = (ISymbol)smplFill1;


                if (MultiSelectFlag) pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);
                else pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);


            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(queryFilter);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pSelect);
        }

        public static void SelectFeature(ILayer _Layer, string _WhereClause)
        {
            IFeatureSelection pSelect = (IFeatureSelection)_Layer;

            IQueryFilter queryFilter = new QueryFilterClass();
            //queryFilter.WhereClause = "OBJECTID = " + pFeature.OID;
            queryFilter.WhereClause = _WhereClause;

            if (pSelect != null)
            {
                IColor selColor = new RgbColor();
                ((RgbColor)selColor).Red = 255;
                ((RgbColor)selColor).Green = 0;
                ((RgbColor)selColor).Blue = 0;

                ISimpleFillSymbol smplFill1 = new SimpleFillSymbol();

                smplFill1.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;

                ISimpleLineSymbol pSimpleLine1 = new SimpleLineSymbolClass();

                pSimpleLine1.Style = esriSimpleLineStyle.esriSLSDashDot;
                pSimpleLine1.Color = selColor;
                pSimpleLine1.Width = 1;
                smplFill1.Outline = pSimpleLine1;

                smplFill1.Color = selColor;

                pSelect.BufferDistance = 0.1;
                pSelect.SetSelectionSymbol = true;

                pSelect.SelectionSymbol = (ISymbol)smplFill1;

                pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);


            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(queryFilter);
        }

        public static void SelectFeature(ILayer _Layer, List<string> _ancorUids)
        {
            string Qr = "(";
            foreach (var item in _ancorUids)
            {
                Qr = Qr + "\"" + item + "\",";
            }
            Qr = Qr + "\"0\")";

            IFeatureSelection pSelect = (IFeatureSelection)_Layer;
            if (pSelect != null) pSelect.CombinationMethod = esriSelectionResultEnum.esriSelectionResultNew;
            pSelect.Clear();

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = "AncorUID  in " + Qr;
            if (pSelect != null)
            {
                IColor selColor = new RgbColor();
                ((RgbColor)selColor).Red = 255;
                ((RgbColor)selColor).Green = 0;
                ((RgbColor)selColor).Blue = 0;

                ISimpleFillSymbol smplFill1 = new SimpleFillSymbol();

                smplFill1.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;

                ISimpleLineSymbol pSimpleLine1 = new SimpleLineSymbolClass();

                pSimpleLine1.Style = esriSimpleLineStyle.esriSLSDashDot;
                pSimpleLine1.Color = selColor;
                pSimpleLine1.Width = 1;
                smplFill1.Outline = pSimpleLine1;

                smplFill1.Color = selColor;


                pSelect.SetSelectionSymbol = true;

                pSelect.SelectionSymbol = (ISymbol)smplFill1;
                pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);


            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(queryFilter);
        }

        public static void RefreshEnvelopeTracker(IActiveView activeView, IElement element)
        {
            IScreenDisplay screenDisplay = activeView.ScreenDisplay;
            if ((element is IElementEditVertices))
            {
                IElementEditVertices elemVert = element as IElementEditVertices;
                if ((elemVert.MovingVertices))
                {
                    activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection,
                      null, element.SelectionTracker.get_Bounds(screenDisplay));
                }
            }
        }

        public static void ClearFeatureSelections(IMap pFocusMap)
        {
            if (SigmaDataCash.AnnotationFeatureClassList == null) return;

            foreach (KeyValuePair<string, object> item in SigmaDataCash.AnnotationFeatureClassList)
            {
                if (!(item.Value is IFeatureClass)) continue;

                IFeatureClass Anno_featClass = (IFeatureClass)item.Value;
                ///////

                ILayer _Layer = EsriUtils.getLayerByName(pFocusMap, Anno_featClass.AliasName);
                IFeatureSelection pSelect = (IFeatureSelection)_Layer;
                if (pSelect != null && pSelect.SelectionSet.Count > 0)
                {
                    pSelect.SelectionSymbol = null;
                    pSelect.Clear();
                    pSelect = null;
                }

                //////
            }



        }

        public static IFeature _SearchBySpatialFilter(IGeometry pp, IFeatureClass _featureClass, string definitionExpression)
        {
            try
            {
                double Ext = -1;
                ISpatialFilter filter = new SpatialFilterClass();
                filter.Geometry = pp;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                filter.WhereClause = definitionExpression;
                IFeature selFeature = null;
                IFeature res = null;
                IFeatureCursor cursor = _featureClass.Search(filter, false);

                while ((selFeature = cursor.NextFeature()) != null)
                {
                    if (selFeature.Extent != null && (selFeature.Extent is IArea) && ((IArea)selFeature.Extent).Area > Ext)
                    {
                        Ext = ((IArea)selFeature.Extent).Area;
                        res = selFeature;
                    }
                }
               

                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                return res;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static IFeature SearchBySpatialFilter(IGeometry pp, IFeatureClass _featureClass, string definitionExpression)
        {
            try
            {
                ISpatialFilter filter = new SpatialFilterClass();
                filter.Geometry = pp;
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                filter.WhereClause = definitionExpression;
                IFeature selFeature = null;

                IFeatureCursor cursor = _featureClass.Search(filter, false);
                selFeature = cursor.NextFeature();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                return selFeature;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static IFeature SearchBySpatialFilter(IFeatureClass _featureClass, string definitionExpression)
        {
            IQueryFilter featFilter = new QueryFilterClass();

            featFilter.WhereClause = definitionExpression;

            IFeatureCursor featCur = _featureClass.Search(featFilter, false);

            IFeature feat = featCur.NextFeature();

            Marshal.ReleaseComObject(featCur);

            return feat;

        }

        private static int LoadToMirror(IElement mapElem, Guid chartElID, string ElementName, double mapScale, string serObj, int PlasedStatus )
        {
            int res = 0;

            try
            {
                IFeatureClass featureClass = GetLinkedFeatureClass("Mirror");

                IAnnoClass pAnnoClass = (IAnnoClass)featureClass.Extension;
                IFeatureClass pClass = pAnnoClass.FeatureClass;
                IAnnoClassAdmin3 pACAdmin = (IAnnoClassAdmin3)pAnnoClass;
                pACAdmin.ReferenceScale = (double)mapScale;

                IFeature pFeat = pClass.CreateFeature();
                IAnnotationFeature pAnnoFeat = (IAnnotationFeature)pFeat;


                pAnnoFeat.Annotation = mapElem;
                pAnnoFeat.LinkedFeatureID = 1;


                int fID = pFeat.Fields.FindField("AnnotationClassID");
                if (fID > 0) pFeat.set_Value(fID, 0);

                fID = pFeat.Fields.FindField("Status");
                if (fID > 0) pFeat.set_Value(fID, PlasedStatus);

                fID = pFeat.Fields.FindField("SymbolID");
                if (fID > 0) pFeat.set_Value(fID, 0);

                fID = pFeat.Fields.FindField("AncorUID");
                if (fID > 0) pFeat.set_Value(fID, chartElID.ToString());

                fID = pFeat.Fields.FindField("ElementType");
                if (fID > 0) pFeat.set_Value(fID, DefinelayerName(ElementName));

                fID = pFeat.Fields.FindField("OBJ");
                if (fID > 0) pFeat.set_Value(fID, serObj);



                pFeat.Store();

                res++;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                res = -1;
            }
            return res;
        }

        public static int UpdateMirror(IElement mapElem, string chartElID, string _objSer)
        {
            IFeatureCursor featCur = null;
            try
            {

                IFeatureClass featureClass = GetLinkedFeatureClass("Mirror");


                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + chartElID.ToString() + "'";

                featCur = featureClass.Update(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {
                    if (mapElem.Geometry == null || mapElem.Geometry.GeometryType == esriGeometryType.esriGeometryPolygon)
                    {
                        IElement OldEl = ((IAnnotationFeature)feat).Annotation;
                        IGeometry gm = OldEl.Geometry;
                        if (OldEl is IGroupElement)
                        {
                            if ((mapElem is IGroupElement) && ((IGroupElement)mapElem).ElementCount > 2)
                                gm = (((IGroupElement)OldEl).get_Element(1)).Geometry;
                            else
                            {
                                gm = (((IGroupElement)OldEl).get_Element(0)).Geometry;

                            }
                        }

                        if (mapElem is IGroupElement)
                        {
                            IGroupElement GrEl = mapElem as IGroupElement;
                            for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                            {
                                GrEl.get_Element(i).Geometry = gm;
                            }
                        }
                        else
                            mapElem.Geometry = gm;
                    }

                    ((IAnnotationFeature)feat).Annotation = mapElem;

                    int fID = feat.Fields.FindField("OBJ");
                    feat.set_Value(fID, _objSer);

                    feat.Store();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(featCur);




            }


            return 1;
        }

        public static int HideReflection(string chartElID, int status)
        {
            IFeatureCursor featCur = null;
            try
            {

                IFeatureClass featureClass = GetLinkedFeatureClass("Mirror");


                IQueryFilter featFilter = new QueryFilterClass();

                featFilter.WhereClause = "AncorUID= '" + chartElID.ToString() + "'";

                featCur = featureClass.Update(featFilter, false);

                IFeature feat = featCur.NextFeature();

                if (feat != null)
                {

                    //((IAnnotationFeature)feat).Annotation = mapElem;


                    int fID = feat.Fields.FindField("Status");
                    feat.set_Value(fID, status);


                    feat.Store();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(featCur);




            }


            return 1;
        }

        public static int _HideReflection<T>(int Status, ref T chartEl)
        {
            IFeatureCursor featCur = null;
            try
            {
                AbstractChartElement cartoElement = chartEl as AbstractChartElement;
                string ElementName = cartoElement.Name;
                string pdmElementID = cartoElement.Id.ToString();

                IFeatureClass featureClass = GetLinkedFeatureClass("Mirror");

                if (featureClass != null)
                {
                    IQueryFilter featFilter = new QueryFilterClass();

                    featFilter.WhereClause = "AncorUID= '" + pdmElementID + "'";

                    featCur = featureClass.Update(featFilter, false);

                    IFeature feat = featCur.NextFeature();

                    if (feat != null)
                    {

                        int fID = feat.Fields.FindField("Status");
                        feat.set_Value(fID, Status);

                        fID = feat.Fields.FindField("ObjectType");
                        string ObjType = feat.get_Value(fID).ToString();

                        fID = feat.Fields.FindField("OBJ");
                        if (fID > 0)
                        {
                            switch (ObjType)
                            {

                                case ("ChartElement_SimpleText"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SimpleText));
                                    break;
                                case ("ChartElement_RouteDesignator"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_RouteDesignator));
                                    break;
                                case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_BorderedText_Collout_CaptionBottom));
                                    break;
                                case ("ChartElement_MarkerSymbol"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_MarkerSymbol));
                                    break;
                                case ("ChartElement_TextArrow"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_TextArrow));
                                    break;
                                case ("ChartElement_BorderedText"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_BorderedText));
                                    break;
                                case ("ChartElement_BorderedText_Collout"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_BorderedText_Collout));
                                    break;
                                case ("ChartElement_SigmaCollout_Navaid"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_Navaid));
                                    break;
                                case ("ChartElement_Radial"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_Radial));
                                    break;
                                case ("ChartElement_SigmaCollout_Designatedpoint"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_Designatedpoint));
                                    break;
                                case ("ChartElement_SigmaCollout_Airspace"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_Airspace));
                                    break;
                                case ("ChartElement_SigmaCollout_AccentBar"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_SigmaCollout_AccentBar));
                                    break;
                                case ("ChartElement_ILSCollout"):
                                    feat.set_Value(fID, SerializeObject(chartEl as ChartElement_ILSCollout));
                                    break;
                                default:
                                    break;


                            }
                        }

                        feat.Store();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (featCur != null) Marshal.ReleaseComObject(featCur);

            }


            return 1;
        }


        public static void UpdateGraphicsElement(IGraphicsContainer pGraphicsContainer, AbstractChartElement chartElement, ref IElement el, IPoint newPos = null)
        {

            pGraphicsContainer.Reset();
            IElementProperties3 docElementProperties;
            IElement sigma_el = pGraphicsContainer.Next();
            while (sigma_el != null)
            {
                docElementProperties = sigma_el as IElementProperties3;
                if (docElementProperties.Name.StartsWith(chartElement.Name))
                {
                    if (newPos == null)
                    {
                        if (((IArea)sigma_el.Geometry).Area > 0)
                        {
                            IPoint pos = ((IArea)sigma_el.Geometry).Centroid;
                            ((GraphicsChartElement)chartElement).Position = new ANCOR.MapCore.AncorPoint(pos.X, pos.Y);
                        }
                    }
                    pGraphicsContainer.DeleteElement(sigma_el);

                    break;
                }
                sigma_el = pGraphicsContainer.Next();

            }


            docElementProperties = el as IElementProperties3;
            docElementProperties.Name = chartElement.Name;
            if (chartElement.Placed) pGraphicsContainer.AddElement(el, 0);
        }

        public static void SetNoneScale_ProcedureLeg(IMap pMap, int X, int Y)
        {
            //    EnrouteChart_Type = 1,
            //SIDChart_Type = 2,
            //ChartTypeA =3,
            //STARChart_Type = 4,
            //IAPChart_Type = 5,

            if (SigmaDataCash.SigmaChartType != 2 && SigmaDataCash.SigmaChartType != 4) return;

            //инициализаия
            ILayer _Layer = EsriUtils.getLayerByName2(pMap, "ProcedureLegsCarto");
            //if (_Layer == null) _Layer = EsriUtils.getLayerByName2(pMap, "ProcedureLegsSTAR"); ;
            if (_Layer == null) return;
            IFeatureClass fcLeg = ((IFeatureLayer)_Layer).FeatureClass;
            ISpatialReference pSpatialReference = (fcLeg as IGeoDataset).SpatialReference;


            var activeView = pMap as IActiveView;
            IPoint mapPoint = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            IPoint geoPoint = (IPoint)EsriUtils.ToGeo(mapPoint, pMap, (fcLeg as IGeoDataset).SpatialReference);

            double pixM = (2 * pMap.MapScale / 111195) * (((0.0254 / 96) * pMap.MapScale) / 111195);

            IEnvelope env = new EnvelopeClass();
            env.PutCoords(geoPoint.X - pixM, geoPoint.Y - pixM, geoPoint.X + pixM, geoPoint.Y + pixM);

            //определяем выбранный Leg

            IFeature legFeature = SearchBySpatialFilter(env, fcLeg, "OBJECTID >0");
            if (legFeature != null)
            {
                // проверка ветвистости

                int FI = SigmaDataCash.SigmaChartType == 2 ? legFeature.Fields.FindField("EndPontID") : legFeature.Fields.FindField("StartPontID");
                if (FI >= 0)
                {
                    string _PointId = legFeature.get_Value(FI).ToString();
                    string _legUID = legFeature.get_Value(legFeature.Fields.FindField("FeatureGUID")).ToString();

                    string lstleg = SigmaDataCash.SigmaChartType == 2 ? "111" : "1";

                    //if (legFeature.get_Value(legFeature.Fields.FindField("FirstLastFlag")).ToString().CompareTo(lstleg) == 0) //убрал проверку ветвистости
                    {
                        // не обнаружено исходящих ветвей

                        // ближайшая к выбранному месту на леге точка
                        IProximityOperator pProximity = legFeature.Shape as IProximityOperator;
                        IPoint pNearestPt = pProximity.ReturnNearestPoint(geoPoint, esriSegmentExtension.esriNoExtension);

                        IFeature featDpn = GetWayPnt(pMap, _PointId);

                        if (featDpn != null)
                        {

                            ILayer _annoLayer = EsriUtils.getLayerByName2(pMap, "ProcedureLegsAnnoLength");



                            if (_annoLayer != null)
                            {
                                //если выбранный лег уже "сжимали" его надо "воcстановить"
                                IFeatureClass fcAnno = ((IFeatureLayer)_annoLayer).FeatureClass;
                                IFeature featAnno = SearchBySpatialFilter(fcAnno, "FeatureID= -100 AND PdmUid  = '" + _legUID + "'");
                                if (featAnno != null)
                                {
                                    //воостановить
                                    int _xField = featDpn.Fields.FindField("CoordX");
                                    if (_xField < 0) _xField = featDpn.Fields.FindField("location_x");
                                    double _x = Convert.ToDouble(featDpn.get_Value(_xField));


                                    int _yField = featDpn.Fields.FindField("CoordY");
                                    if (_yField < 0) _yField = featDpn.Fields.FindField("location_y");
                                    double _y = Convert.ToDouble(featDpn.get_Value(_yField));


                                    pNearestPt.PutCoords(_x, _y);
                                    DeleteNoneScaleSign(pNearestPt, featDpn, legFeature, _PointId, pMap, pSpatialReference, SigmaDataCash.SigmaChartType == 2);
                                    featAnno.Delete();

                                }
                                else
                                {
                                    // сжимать
                                    CreateNoneScaleSign(pNearestPt, featDpn, legFeature, _PointId, pMap, pSpatialReference, SigmaDataCash.SigmaChartType == 2);
                                }
                            }

                        }



                    }

                }

            }

        }

        private static IFeature GetWayPnt(IMap pMap, string pID)
        {
            IFeature featPnt = null;
            IFeatureClass fcPnt = null;
            ILayer _Layer = EsriUtils.getLayerByName2(pMap, "DesignatedPoint");

            if (_Layer != null)
            {

                fcPnt = ((IFeatureLayer)_Layer).FeatureClass;
                featPnt = SearchBySpatialFilter(fcPnt, "FeatureGUID= '" + pID + "'");
            }

            if (featPnt == null) //NavaidsCSID
            {
                _Layer = EsriUtils.getLayerByName2(pMap, "Navaids");
                if (_Layer != null)
                {

                    fcPnt = ((IFeatureLayer)_Layer).FeatureClass;
                    featPnt = SearchBySpatialFilter(fcPnt, "FeatureGUID= '" + pID + "'");
                }
            }

            return featPnt;
        }

        private static void CreateNoneScaleSign(IPoint pNearestPt, IFeature featDpn, IFeature legFeature, string _PointId, IMap pMap, ISpatialReference pSpatialReference, bool SidFalg)
        {
            string LegName = SidFalg ? "ProcedureLegsCarto" : "ProcedureLegsCarto";

            IPoint _shape = new PointClass();
            _shape.PutCoords(pNearestPt.X, pNearestPt.Y);

            string legId = legFeature.get_Value(legFeature.Fields.FindField("FeatureGUID")).ToString();

            //переместим конец лега к выбранной точке
            featDpn.Shape = _shape;
            featDpn.Store();

            // теперь переделаем геомерию лега
            IPolyline4 RealShape = legFeature.Shape as IPolyline4;
            if (SidFalg)
                RealShape.ToPoint = _shape;
            else
                RealShape.FromPoint = _shape;

            legFeature.Store();

            #region Создание NoneScale элемента

            var featureLayer = (IFeatureLayer)EsriUtils.getLayerByName2(pMap, LegName);
            IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)featureLayer;

            RgbColor rgbClr = new RgbColorClass();
            rgbClr.Blue = 0;
            rgbClr.Red = 0;
            rgbClr.Green = 0;

            if (geoFeatureLayer.Renderer is ISimpleRenderer)
            {
                ISimpleRenderer simpleRenderer = (ISimpleRenderer)geoFeatureLayer.Renderer;
                var lnSymb = (ILineSymbol)simpleRenderer.Symbol;
                rgbClr = lnSymb.Color as RgbColor;
            }

            ChartElement_MarkerSymbol ChartElement_NoneScale = (ChartElement_MarkerSymbol)SigmaDataCash.prototype_anno_lst.FirstOrDefault(x => x.Name.StartsWith("NoneScale")).Clone();

            ChartElement_NoneScale.TextContents[0][0].TextValue = ".";
            ChartElement_NoneScale.TextContents[0][0].Font.FontColor = ChartElement_NoneScale.FillColor;
            // ChartElement_NoneScale.Name = "ProcedureLegLength";
            ChartElement_NoneScale.Name = "NoneScale";
            ChartElement_NoneScale.TextContents[0][0].DataSource.Condition = "NoneScale";
            ChartElement_NoneScale.MarkerFrameColor = new ANCOR.MapCore.AncorColor(rgbClr.Red, rgbClr.Green, rgbClr.Blue);
            ChartElement_NoneScale.TextContents[0][0].TextPosition = ANCOR.MapCore.textPosition.Subscript;

            //IPoint startPoint = ((IPointCollection)RealShape).PointCount > 2 ? ((IPointCollection)RealShape).get_Point(((IPointCollection)RealShape).PointCount - 2) : ((IPointCollection)RealShape).get_Point(0);
            IPoint startPoint = ((IPointCollection)RealShape).get_Point(0);
            int pcnt = ((IPointCollection)RealShape).PointCount;
            IPoint endPoint = ((IPointCollection)RealShape).get_Point(((IPointCollection)RealShape).PointCount - 1);


            #endregion

            #region определим новые места 

            IPoint StPnt = new PointClass();
            IPoint CntrPnt = new PointClass();
            IPoint EndPnt = new PointClass();
            double st; double en; double cn;


            Polyline PolyLn = new PolylineClass();
            IPointCollection RealShape2 = RealShape as IPointCollection;
            {
                (PolyLn as IPointCollection).AddPointCollection(RealShape2);
            }

            bool reverse = GetInterestPoints(pMap, pSpatialReference, out StPnt, out CntrPnt, out EndPnt, (IPolyline)PolyLn, out cn);


            #endregion

            #region перетащим аннотации относящиеся к перемещенному featDpn на новое место

            var chartElObjList = (from element in SigmaDataCash.ChartElementList
                                  where
                                      (element != null) &&
                                      (element is ChartElement_SimpleText) &&
                                      (((ChartElement_SimpleText)element).LinckedGeoId.CompareTo(_PointId) == 0)
                                  select element).ToList();

            foreach (AbstractChartElement item in chartElObjList)
            {
                ChartElement_SimpleText cartoEl = (ChartElement_SimpleText)item;
                if (cartoEl.TextContents[0][0].DataSource.Condition.StartsWith("NoneScale")) continue;
                cartoEl.Anchor = new ANCOR.MapCore.AncorPoint(_shape.X, _shape.Y);
                IElement el = cartoEl.ConvertToIElement() as IElement;
                if (SidFalg)
                    UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, ((IPolyline)legFeature.Shape).ToPoint);
                else
                    UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, ((IPolyline)legFeature.Shape).FromPoint);

            }


            #endregion

            #region перетащим аннотации относящиеся к перемещенному Leg на новое место

            chartElObjList = (from element in SigmaDataCash.ChartElementList
                              where
                                  (element != null) &&
                                  (element is ChartElement_SimpleText) &&
                                  (((ChartElement_SimpleText)element).LinckedGeoId.CompareTo(legId) == 0)
                              select element).ToList();

            foreach (AbstractChartElement item in chartElObjList)
            {
                ChartElement_SimpleText cartoEl = (ChartElement_SimpleText)item;
                if (cartoEl.TextContents[0][0].DataSource.Condition.StartsWith("NoneScale")) continue;
                IPoint newGmtr = SidFalg ? getPointOnLine(endPoint, CntrPnt,50, pMap, pSpatialReference) : getPointOnLine(CntrPnt, endPoint, 50, pMap, pSpatialReference); ;

                IElement el = cartoEl.ConvertToIElement() as IElement;
                UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, newGmtr);

                if (item is ChartElement_MarkerSymbol) ChartElement_NoneScale.Slope = cartoEl.Slope;

            }
            #endregion

            #region сохраним IElement в базе


            IElement el_NoneSacle = (IElement)ChartElement_NoneScale.ConvertToIElement();

            IPoint gm = SidFalg ? getPointOnLine(endPoint, CntrPnt, 30, pMap, pSpatialReference) : getPointOnLine(CntrPnt, endPoint, 30, pMap, pSpatialReference);



            for (int i = 0; i < ((IGroupElement3)el_NoneSacle).ElementCount; i++)
            {
                ((IGroupElement3)el_NoneSacle).get_Element(i).Geometry = gm;
            }

            StoreSingleElementToDataSet("ProcedureLegLength", legId, el_NoneSacle, ref ChartElement_NoneScale, ChartElement_NoneScale.Id, pMap.MapScale, -100);


            #endregion


        }

        private static bool GetInterestPoints(IMap FocusMap, ISpatialReference pSpatialReference, out IPoint StPnt, out IPoint CntrPnt, out IPoint EndPnt, IPolyline polyLine, out double anglCnt)
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
                //ln.FromPoint = ((IPointCollection)polyLine).get_Point(indx - 1);
                //ln.ToPoint = ((IPointCollection)polyLine).get_Point(indx + 1);
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

            for (int i = 0; i < lnPnts.PointCount - 1; i++)
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

        private static void DeleteNoneScaleSign(IPoint pNearestPt, IFeature featDpn, IFeature legFeature, string _PointId, IMap pMap, ISpatialReference pSpatialReference, bool SidFalg)
        {
            IPoint _shape = new PointClass();
            _shape.PutCoords(pNearestPt.X, pNearestPt.Y);
            string legId = legFeature.get_Value(legFeature.Fields.FindField("FeatureGUID")).ToString();

            //переместим конец лега к выбранной точке
            featDpn.Shape = _shape;
            featDpn.Store();

            // теперь переделаем геомерию лега
            IPolyline4 RealShape = legFeature.Shape as IPolyline4;
            if (SidFalg)
                RealShape.ToPoint = _shape;
            else
                RealShape.FromPoint = _shape;

            legFeature.Store();

            IPoint startPoint = ((IPointCollection)RealShape).get_Point(0);
            IPoint endPoint = ((IPointCollection)RealShape).get_Point(((IPointCollection)RealShape).PointCount - 1);


            #region определим новые места 

            IPoint StPnt = new PointClass();
            IPoint CntrPnt = new PointClass();
            IPoint EndPnt = new PointClass();
            double st; double en; double cn;


            Polyline PolyLn = new PolylineClass();
            IPointCollection RealShape2 = RealShape as IPointCollection;
            {
                (PolyLn as IPointCollection).AddPointCollection(RealShape2);
            }

            bool reverse = GetInterestPoints(pMap, pSpatialReference, out StPnt, out CntrPnt, out EndPnt, (IPolyline)PolyLn, out cn);


            #endregion

            #region перетащим аннотации относящиеся к перемщенному featDpn на новое место

            var chartElObjList = (from element in SigmaDataCash.ChartElementList
                                  where
                                      (element != null) &&
                                      (element is ChartElement_SimpleText) &&
                                      (((ChartElement_SimpleText)element).LinckedGeoId.CompareTo(_PointId) == 0)
                                  select element).ToList();

            foreach (AbstractChartElement item in chartElObjList)
            {
                ChartElement_SimpleText cartoEl = (ChartElement_SimpleText)item;
                // cartoEl.Anchor = new ANCOR.MapCore.AncorPoint(_shape.X, _shape.Y);
                cartoEl.Anchor = new ANCOR.MapCore.AncorPoint(_shape.X, _shape.Y);
                IElement el = cartoEl.ConvertToIElement() as IElement;
                //UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl);
                if (SidFalg)
                    UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, ((IPolyline)legFeature.Shape).ToPoint);
                else
                    UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, ((IPolyline)legFeature.Shape).FromPoint);


            }

            #endregion

            #region перетащим аннотации относящиеся к перемещенному Leg на новое место

            chartElObjList = (from element in SigmaDataCash.ChartElementList
                              where
                                  (element != null) &&
                                  (element is ChartElement_SimpleText) &&
                                  (((ChartElement_SimpleText)element).LinckedGeoId.CompareTo(legId) == 0)
                              select element).ToList();

            foreach (AbstractChartElement item in chartElObjList)
            {
                ChartElement_SimpleText cartoEl = (ChartElement_SimpleText)item;
                if (cartoEl.TextContents[0][0].DataSource.Condition.StartsWith("NoneScale")) continue;
                IPoint newGmtr = getPointOnLine(EndPnt, StPnt, 50, pMap, pSpatialReference);

                IElement el = cartoEl.ConvertToIElement() as IElement;
                UpdateSingleElementToDataSet(cartoEl.Name, cartoEl.Id.ToString(), el, ref cartoEl, newGmtr);

            }
            #endregion

            #region удалим  NoneScale из списка элементов

            legId = legFeature.get_Value(legFeature.Fields.FindField("FeatureGUID")).ToString();
            var nonescaleList = (from element in SigmaDataCash.ChartElementList
                                 where
                                     (element != null) &&
                                     (element is ChartElement_SimpleText) &&
                                     (((ChartElement_SimpleText)element).LinckedGeoId.CompareTo(legId) == 0)
                                 select element).ToList();

            foreach (AbstractChartElement item in nonescaleList)
            {
                ChartElement_SimpleText cartoEl = (ChartElement_SimpleText)item;
                if (cartoEl.TextContents[0][0].DataSource.Condition.StartsWith("NoneScale"))
                {
                    SigmaDataCash.ChartElementList.Remove(cartoEl);
                }
            }

            #endregion


        }

        public static void CreateSigmaLog(string filePath)
        {
            try
            {
                string FN = filePath + @"\SIGMA_ResultsInfo.txt";

                if (File.Exists(FN)) File.Delete(FN);


                List<string> tmp = new List<string>();


                var _itemsRelFeatrure = from _item in SigmaDataCash.ChartElementList
                                        where _item is AbstractChartElement
                                        group new
                                        {
                                            ((AbstractChartElement)_item).LinckedGeoId,
                                            ((AbstractChartElement)_item).RelatedFeature,
                                            ((AbstractChartElement)_item).Name,
                                            ((AbstractChartElement)_item).Id
                                        }
                                             by ((AbstractChartElement)_item).LinckedGeoId into _Group
                                        select _Group;


                foreach (var item in _itemsRelFeatrure)
                {
                    foreach (var obj in item)
                    {
                        tmp.Add(obj.RelatedFeature + (char)9 + obj.LinckedGeoId + (char)9 + ChartElementsManipulator.DefinelayerName(obj.Name));


                        var el = (AbstractChartElement)(from element in SigmaDataCash.ChartElementList
                                                        where (element != null) && 
                                                        element is AbstractChartElement &&
                                                        ((AbstractChartElement)element).LinckedGeoId !=null &&
                                                        (((AbstractChartElement)element).Id.ToString().CompareTo(obj.Id.ToString()) == 0)
                                                        select element).FirstOrDefault();

                        if (el != null && el is ChartElement_SimpleText)
                        {
                            foreach (var line in ((ChartElement_SimpleText)el).TextContents)
                            {
                                foreach (var wrd in line)
                                {
                                    if (wrd.TextValue.CompareTo("NaN") == 0 || wrd.TextValue.CompareTo("OTHER") == 0)
                                    {
                                        tmp.Add("           " + wrd.TextValue + "   " + wrd.DataSource.Value);

                                    }
                                }
                            }
                        }
                        if (el != null && el is ChartElement_BorderedText_Collout_CaptionBottom )
                        {
                            if (((ChartElement_BorderedText_Collout_CaptionBottom)el).CaptionTextLine != null)
                            {
                                foreach (var line in ((ChartElement_BorderedText_Collout_CaptionBottom)el).CaptionTextLine)
                                {
                                    foreach (var wrd in line)
                                    {
                                        if (wrd.TextValue.CompareTo("NaN") == 0 || wrd.TextValue.CompareTo("OTHER") == 0)
                                        {
                                            tmp.Add("           " + wrd.TextValue + "   " + wrd.DataSource.Value);

                                        }
                                    }
                                }
                            }
                            if (((ChartElement_BorderedText_Collout_CaptionBottom)el).BottomTextLine != null)
                            {
                                foreach (var line in ((ChartElement_BorderedText_Collout_CaptionBottom)el).BottomTextLine)
                                {
                                    foreach (var wrd in line)
                                    {
                                        if (wrd.TextValue.CompareTo("NaN") == 0 || wrd.TextValue.CompareTo("OTHER") == 0)
                                        {
                                            tmp.Add("           " + wrd.TextValue + "   " + wrd.DataSource.Value);

                                        }
                                    }
                                }
                            }

                        }

                    }
                }


                #region Errors

                if(SigmaDataCash.Report!=null && SigmaDataCash.Report.Count >0)
                {
                    tmp.Add("");
                    tmp.Add(" = = = = = = = = = = = = SKIPPED = = = = = = = = = = = = ");
                    tmp.AddRange(SigmaDataCash.Report);
                }

                #endregion

                System.IO.File.WriteAllLines(FN, tmp.ToArray());


                #region Log файл изменений

                FN = filePath + @"\SIGMA_LOG.txt";

                if (File.Exists(FN)) File.Delete(FN);
                tmp = new List<string>();
                tmp.Add("Project was created " + DateTime.Now.ToShortDateString() + "\t" + DateTime.Now.ToShortTimeString());
                System.IO.File.WriteAllLines(FN, tmp.ToArray());


                #endregion

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
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

        public static IPoint getPointAlongDirection(IPoint LineStartGeo, Double directionAngle, double DistamseInM, IMap FocusMap, ISpatialReference pSpatialReference)
        {
            GeometryFunctions.TapFunctions GF = new GeometryFunctions.TapFunctions();
            IPoint pRes = new PointClass();
            pRes.PutCoords(LineStartGeo.X, LineStartGeo.Y);
            pRes = (IPoint)EsriUtils.ToProject(pRes, FocusMap, pSpatialReference);


            IPoint res = GF.PointAlongPlane(pRes, directionAngle, DistamseInM);
            pRes = (IPoint)EsriUtils.ToGeo(res, FocusMap, pSpatialReference);

            GF = null;

            return pRes;
        }

        public static double GetLineSlopE(ILine ln, IMap FocusMap, ISpatialReference pSpatialReference)
        {
            IPoint startP = new PointClass();
            startP.PutCoords(ln.FromPoint.X, ln.FromPoint.Y);

            IPoint endP = new PointClass();
            endP.PutCoords(ln.ToPoint.X, ln.ToPoint.Y);

            startP = (IPoint)EsriUtils.ToProject(startP, FocusMap, pSpatialReference);
            endP = (IPoint)EsriUtils.ToProject(endP, FocusMap, pSpatialReference);

            return Math.Atan2(endP.Y - startP.Y, endP.X - startP.X);
        }

        public static ChartElement_SimpleText ConstructDesignatorElement(ChartElement_RouteDesignator chrtEl_RouteSign)
        {
            ChartElement_SimpleText DesignatorElement = new ChartElement_SimpleText();
            DesignatorElement.Name = chrtEl_RouteSign.Name;
            DesignatorElement.RelatedFeature = chrtEl_RouteSign.RelatedFeature;
            DesignatorElement.Placed = chrtEl_RouteSign.Placed;
            DesignatorElement.ReflectionHidden = chrtEl_RouteSign.ReflectionHidden;
            DesignatorElement.LinckedGeoId = chrtEl_RouteSign.LinckedGeoId;

            //////////////////////////////////////
            DesignatorElement.Font = new AncorFont { Bold = chrtEl_RouteSign.Font.Bold, FontColor = new AncorColor(0, 0, 0), Italic = chrtEl_RouteSign.Font.Italic, Name = chrtEl_RouteSign.Font.Name, Size = chrtEl_RouteSign.Font.Size, UnderLine = chrtEl_RouteSign.Font.UnderLine };
            DesignatorElement.HorizontalAlignment = chrtEl_RouteSign.HorizontalAlignment;
            DesignatorElement.VerticalAlignment = chrtEl_RouteSign.VerticalAlignment;
            DesignatorElement.Slope = chrtEl_RouteSign.Slope;
            DesignatorElement.TextPosition = chrtEl_RouteSign.TextPosition;
            DesignatorElement.TextCase = chrtEl_RouteSign.TextCase;
            DesignatorElement.Anchor = new AncorPoint(chrtEl_RouteSign.Anchor.X, chrtEl_RouteSign.Anchor.Y);
            DesignatorElement.WordSpacing = chrtEl_RouteSign.WordSpacing;
            DesignatorElement.HaloColor = new AncorColor(chrtEl_RouteSign.HaloColor.Red, chrtEl_RouteSign.HaloColor.Green, chrtEl_RouteSign.HaloColor.Blue);
            DesignatorElement.FillColor = new AncorColor(chrtEl_RouteSign.FillColor.Blue, chrtEl_RouteSign.FillColor.Green, chrtEl_RouteSign.FillColor.Red);
            DesignatorElement.FillStyle = fillStyle.fSNull;//chrtEl_RouteSign.FillStyle;
            DesignatorElement.CoordType = chrtEl_RouteSign.CoordType;
            DesignatorElement.Leading = chrtEl_RouteSign.Leading;
            DesignatorElement.CharacterSpacing = chrtEl_RouteSign.CharacterSpacing;
            DesignatorElement.CharacterWidth = chrtEl_RouteSign.CharacterWidth;
            DesignatorElement.HaloMaskSize = chrtEl_RouteSign.HaloMaskSize;
            /////////////////////////////////////////////////////////////////////////////////////////////
            DesignatorElement.TextContents = new List<List<AncorChartElementWord>>();

            foreach (var _line in chrtEl_RouteSign.RouteDesignatorSource)
            {
                if (isEmptyLine(_line)) continue;

                List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
                foreach (var wrd in _line)
                {
                    AncorChartElementWord cloneWrd = (AncorChartElementWord)wrd.Clone();
                    txtLine.Add(cloneWrd);
                }

                DesignatorElement.TextContents.Add(txtLine);
            }

            return DesignatorElement;
        }

        private static bool isEmptyLine(List<AncorChartElementWord> _line)
        {
            bool res = true;
            foreach (var item in _line)
            {
                if (item.TextValue.Trim().Length > 0)
                {
                    res = false;
                    break;
                }
            }

            return res;
        }

        public static void RefreshChart(IMxDocument document)
        {
            IMaps maps = document.Maps;


            for (int i = 0; i <= maps.Count - 1; i++)
            {
                IGraphicsContainer pGraphicsContainer = document.ActiveView.GraphicsContainer;
                IMap map = maps.get_Item(i);
                IFrameElement frameElement = pGraphicsContainer.FindFrame(maps.get_Item(i));
                IMapFrame mapFrame = frameElement as IMapFrame;

                if (mapFrame != null) (mapFrame.Map as IActiveView).Refresh();
                (map as IActiveView).Refresh();
            }



            //SystemSounds.Asterisk.Play();

            double mod = Modulus(90, 360.0);
        }

        public static AbstractChartElement DoCloneElement(AbstractChartElement cartoEl)
        {

            return (AbstractChartElement)cartoEl.Clone();


        }
    }
}
