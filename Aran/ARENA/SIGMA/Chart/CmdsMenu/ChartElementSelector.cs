using System;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Collections.Generic;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Display;
using ANCOR.MapElements;
using DataModule;
using ARENA.Enums_Const;
using ArenaStatic;
using ESRI.ArcGIS.ArcMapUI;
using ANCOR.MapCore;

namespace SigmaChart.CmdsMenu
{
    /// <summary>
    /// Summary description for ChartElementSelector.
    /// </summary>
    [Guid("66ec30f7-fd38-4b7a-880b-4bd2b357467b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.ChartElementSelector")]
    public sealed class ChartElementSelector : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private IApplication m_application;
        private IPoint _position;
        IDisplayFeedback feedBack;
        IActiveView activeView;
        AbstractChartElement SelectedElement;
        ISpatialReference pSpatialReference;
        IGeometry geom;
        bool FeedBackProcessStarted = false;
        IPoint startPosition;


        public ChartElementSelector()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "select Sigma Chart Element";  //localizable text 
            base.m_message = "select Sigma Chart Element";  //localizable text
            base.m_toolTip = "Sigma Chart";  //localizable text
            base.m_name = "Sigma Chart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.CadastralPRImage3_16; 
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }

                 m_application = hook as IApplication;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ChartElementSelector.OnClick implementation
           
        }

        //public override bool Enabled
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}



        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if ((Shift == 1 || Shift == 3)&& SigmaDataCash.SelectedChartElements!=null && SigmaDataCash.SelectedChartElements.Count >= 0) StartFeedBackProcess(X, Y);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            if (Shift == 1 || Shift == 3) MoveFeedback(X, Y, Shift);
            
        }

        
        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {

            ArenaStaticProc.BringToFrontToc((IMxDocument)m_application.Document, "ANCORTOCLayerView");
            string ActiveFrameName =ChartElementsManipulator.GetActiveFrameName(m_hookHelper.ActiveView.FocusMap, m_hookHelper);
            //SigmaDataCash.PutOutPropertyGrid();


            if (Shift == 1 || Shift == 3 || FeedBackProcessStarted)
            {
                StopFeedBackProcess(X, Y, Shift);
                //ChartElementsManipulator.GetClickedChartElement(m_hookHelper.ActiveView.FocusMap, X, Y, ActiveFrameName, SigmaDataCash.MultiSelectFlag);
                ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);
                return;

            }
            else if (Shift == 2)
            {
                SigmaDataCash.MultiSelectFlag = true;
                ChartElementsManipulator.GetClickedChartElement(m_hookHelper.ActiveView.FocusMap, X, Y,ActiveFrameName, SigmaDataCash.MultiSelectFlag);
            }
            else if (Shift == 4) //ALT
            {
                SigmaDataCash.MultiSelectFlag = false;
                if (ChartElementsManipulator.GetClickedChartElement(m_hookHelper.ActiveView.FocusMap, X, Y, ActiveFrameName, SigmaDataCash.MultiSelectFlag) != null)
                {
                    TextContestEditorForm txtFrm = new TextContestEditorForm();

                    if (!ArenaStaticProc.HasProperty(SigmaDataCash.AncorPropertyGrid.SelectedObject, "TextContents"))
                        txtFrm.TextContents = null;
                    else
                        txtFrm.TextContents = ((ChartElement_SimpleText)SigmaDataCash.AncorPropertyGrid.SelectedObject).TextContents;

                    if (!ArenaStaticProc.HasProperty(SigmaDataCash.AncorPropertyGrid.SelectedObject, "CaptionTextLine"))
                        txtFrm.CaptionTextContest = null;
                    else
                        txtFrm.CaptionTextContest = ((ChartElement_BorderedText_Collout_CaptionBottom)SigmaDataCash.AncorPropertyGrid.SelectedObject).CaptionTextLine;

                    if (!ArenaStaticProc.HasProperty(SigmaDataCash.AncorPropertyGrid.SelectedObject, "BottomTextLine"))
                        txtFrm.BottomTextContest = null;
                    else
                        txtFrm.BottomTextContest = ((ChartElement_BorderedText_Collout_CaptionBottom)SigmaDataCash.AncorPropertyGrid.SelectedObject).BottomTextLine;


                    txtFrm.ShowDialog();

                    ChartElementsManipulator.GetClickedChartElement(m_hookHelper.ActiveView.FocusMap, X, Y, ActiveFrameName, SigmaDataCash.MultiSelectFlag, UpdateAfterSelect: true);


                }
            }

            else
            {
                SigmaDataCash.MultiSelectFlag = false;
                IMxDocument document = (IMxDocument)m_application.Document;
                IMaps maps = document.Maps;
                for (int i = 0; i <= maps.Count - 1; i++)
                {
                   IMap map = maps.get_Item(i);
                    ActiveFrameName = ChartElementsManipulator.GetActiveFrameName(map, m_hookHelper);
                    if (ChartElementsManipulator.GetClickedChartElement(map, X, Y, ActiveFrameName, SigmaDataCash.MultiSelectFlag) != null)
                    {
                        break;
                    }
                   
                }

            }

            if (SigmaDataCash.SelectedChartElements.Count == 0)
            {
                ChartElementsManipulator.GetClickedChartElement((m_hookHelper.PageLayout as IActiveView), X, Y);

            }

            if (SigmaDataCash.SelectedChartElements.Count == 0)
                SigmaDataCash.PutOutPropertyGrid();

            //(m_hookHelper.FocusMap as IActiveView).Refresh();
            ((IGraphicsContainerSelect)m_hookHelper.ActiveView.GraphicsContainer).UnselectAllElements();
            ChartElementsManipulator.RefreshChart((IMxDocument)m_application.Document);




        }


        private void StartFeedBackProcess(int X, int Y)
        {
            activeView = m_hookHelper.ActiveView.FocusMap as IActiveView;

            ChartElementsManipulator.ClearFeatureSelections(m_hookHelper.ActiveView.FocusMap);


            SelectedElement = ChartElementsManipulator.GetClickedChartElement(m_hookHelper.ActiveView.FocusMap, X, Y, ChartElementsManipulator.GetActiveFrameName(m_hookHelper.ActiveView.FocusMap, m_hookHelper));
            if (SelectedElement == null) SelectedElement = ChartElementsManipulator.GetClickedChartElement((m_hookHelper.PageLayout as IActiveView), X, Y);
            if (SelectedElement == null) return;

            if (SelectedElement is ChartElement_SimpleText) feedbackElement_simpleText(X, Y);
            if (SelectedElement is GraphicsChartElement) feedbackElement_GraphicsChartElement(X, Y);

            startPosition = new PointClass();
            startPosition = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            startPosition = (IPoint)EsriUtils.ToGeo(startPosition, m_hookHelper.ActiveView.FocusMap, pSpatialReference);
        }

        private void feedbackElement_simpleText(int X, int Y)
        {

            FeedBackProcessStarted = true;


            ILayer _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportCartography");

            var fc = ((IFeatureLayer)_Layer).FeatureClass;

            pSpatialReference = (fc as IGeoDataset).SpatialReference;
            geom = ChartElementsManipulator.GetLinkedGeometry(SelectedElement.Name, SelectedElement.LinckedGeoId);

            if (geom == null && (SelectedElement.Name.StartsWith("HoldingPattern") || SelectedElement.Name.StartsWith("HoldingPatternInboundCource") || SelectedElement.Name.StartsWith("HoldingPatternOutboundCource")))
            {
                geom = ChartElementsManipulator.GetLinkedGeometry("DesignatedPoint", SelectedElement.LinckedGeoId);
            }
            if (geom == null && (SelectedElement.Name.StartsWith("HoldingPattern") || SelectedElement.Name.StartsWith("HoldingPatternInboundCource") || SelectedElement.Name.StartsWith("HoldingPatternOutboundCource")))
            {
                geom = ChartElementsManipulator.GetLinkedGeometry("Navaids", SelectedElement.LinckedGeoId);
            }
            if (geom == null && (SelectedElement.Name.StartsWith("ProcedureLegHeight"))) //для старых версий карт, у которых ProcedureLegHeight не связян с leg'ом
            {
                geom = ChartElementsManipulator.GetLinkedGeometry("DesignatedPoint_Simple", SelectedElement.LinckedGeoId);
            }
            if (geom == null)
            {
                geom = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                geom = EsriUtils.ToGeo(geom, m_hookHelper.ActiveView.FocusMap, pSpatialReference);
            }

            if (geom == null) return;
            geom = EsriUtils.ToProject(geom, m_hookHelper.ActiveView.FocusMap, pSpatialReference);


            feedBack = SelectedElement.GetFeedback();

            feedBack.Display = activeView.ScreenDisplay;


            _position = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            IFeatureClass featureClass = ChartElementsManipulator.GetLinkedFeatureClass(SelectedElement.Name);
            if (featureClass == null && (SelectedElement.Name.StartsWith("HoldingPatternInboundCource") || SelectedElement.Name.StartsWith("HoldingPatternOutboundCource")))
                featureClass = ChartElementsManipulator.GetLinkedFeatureClass("RouteSegment_ValMagTrack");

            IAnnoClass pAnnoClass = (IAnnoClass)featureClass.Extension;
            double scale = pAnnoClass.ReferenceScale;

            SelectedElement.StartFeedback(feedBack, _position, scale, geom);

        }

        private void feedbackElement_GraphicsChartElement(int X, int Y)
        {

            FeedBackProcessStarted = true;

            _position = (m_hookHelper.PageLayout as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            feedBack = SelectedElement.GetFeedback();
            feedBack.Display = (m_hookHelper.PageLayout as IActiveView).ScreenDisplay;


            _position = (m_hookHelper.PageLayout as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

             SelectedElement.StartFeedback(feedBack, _position, 0, geom);

        }

        private void MoveFeedback(int X, int Y, int Shift)
        {
            if (activeView == null || _position == null || SelectedElement == null) return;


            if (SelectedElement is ChartElement_SimpleText) _position = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (SelectedElement is GraphicsChartElement) _position = (m_hookHelper.PageLayout as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);


            SelectedElement.MoveFeedback(feedBack, _position, geom, Shift);
        }

        private void StopFeedBackProcess(int X, int Y, int Shift)
        {
            if (SelectedElement == null) return;

            FeedBackProcessStarted = false;

            if (SelectedElement is ChartElement_SimpleText) StopFeedBackProcess_ChartElement_SimpleText(X, Y, Shift);
            if (SelectedElement is GraphicsChartElement) StopFeedBackProcess_ChartElement_GraphicsChartElement(X, Y, Shift);

           
            SigmaDataCash.MultiSelectFlag = false;

            #region Select annotation after moving

            SigmaDataCash.MultiSelectFlag = false;
            IMxDocument document = (IMxDocument)m_application.Document;
            IMaps maps = document.Maps;
            for (int i = 0; i <= maps.Count - 1; i++)
            {
                IMap map = maps.get_Item(i);
                string ActiveFrameName = ChartElementsManipulator.GetActiveFrameName(map, m_hookHelper);
                if (ChartElementsManipulator.GetClickedChartElement(map, X, Y, ActiveFrameName, SigmaDataCash.MultiSelectFlag) != null)
                {
                    break;
                }

            }

            #endregion


            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, m_hookHelper.ActiveView.Extent);
            _position = (m_hookHelper.PageLayout as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
        }
        
        private void StopFeedBackProcess_ChartElement_SimpleText(int X, int Y, int Shift)
        {
            string LegArincType = "";
            string activeFrameName = ChartElementsManipulator.GetActiveFrameName(m_hookHelper.ActiveView.FocusMap, m_hookHelper);

            ChartElementsManipulator.ClearFeatureSelections(m_hookHelper.ActiveView.FocusMap);

            _position = activeView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            _position = (IPoint)SelectedElement.StopFeedback(feedBack, (int)_position.X, (int)_position.Y, geom, Shift);

            #region Check slope

            
            if (geom is IPointCollection && ((IPointCollection)geom).PointCount > 2 && Shift!=3 &&
                !SelectedElement.Name.StartsWith("ProcedureLegLength") && 
                !SelectedElement.Name.StartsWith("NoneScale") &&
                !SelectedElement.Name.StartsWith("ProcedureLegName") &&
                !SelectedElement.Name.StartsWith("SigmaCollout_Airspace") &&
                !SelectedElement.Name.StartsWith("Airspace_Class") &&
                !SelectedElement.Name.StartsWith("ATZ_ATZP_Airspace") &&
                !SelectedElement.Name.StartsWith("CTR_CTRP_Airspace") &&
                !SelectedElement.Name.StartsWith("TMA_TMAP_Airspace") &&
                !SelectedElement.Name.StartsWith("TIZ_Airspace") &&
                !SelectedElement.Name.StartsWith("TIA_Airspace") &&
                !SelectedElement.Name.StartsWith("FIS_Airspace") &&
                !SelectedElement.Name.StartsWith("SECTOR_SECTORC_Airspace") &&
                !SelectedElement.Name.StartsWith("R_D_P_Airspace") &&
                !SelectedElement.Name.StartsWith("R_D_P_AMC_Airspace") &&
                !SelectedElement.Name.StartsWith("TRA_TSA_Airspace") && 
                !SelectedElement.Name.StartsWith("PROTECT_Airspace") &&
                !SelectedElement.Name.StartsWith("AOR_Airspace") &&
                !SelectedElement.Name.StartsWith("Airspace_Simple") &&
                !SelectedElement.Name.StartsWith("AirportHotSpotElement") &&
                !SelectedElement.Name.StartsWith("VerticalStructureElement") &&
                !SelectedElement.Name.StartsWith("VerticalStructureElementPoint") &&
                !SelectedElement.Name.StartsWith("VerticalStructureElementElev") &&
                !SelectedElement.Name.StartsWith("VerticalStructureElementHeight") &&
                !SelectedElement.Name.StartsWith("SectorAirspace") &&
                !SelectedElement.Name.StartsWith("FreqAreaElement"))
                (SelectedElement as ChartElement_SimpleText).Slope = GetNewSlope(geom,_position);
            else if ((SelectedElement.Name.StartsWith("ProcedureLegLength")) && !(SelectedElement as ChartElement_SimpleText).TextContents[0][0].DataSource.Condition.StartsWith("NoneScale"))
            {
                ChartElement_MarkerSymbol chrtEl_legLength = (ChartElement_MarkerSymbol)SelectedElement;
                //chrtEl_legLength.MarkerBackGround.CharacterIndex = 94;
                //chrtEl_legLength.MarkerBackGround.InnerCharacterIndex = 95;

                bool Flag = false;
                (SelectedElement as ChartElement_SimpleText).Slope = GetNewSlope(geom, _position);
                if ((SelectedElement as ChartElement_SimpleText).Slope > 180) (SelectedElement as ChartElement_SimpleText).Slope -= 360;
                    double newSlope = ChartsHelperClass.CheckedAngle((SelectedElement as ChartElement_SimpleText).Slope, ref Flag);

                if (Flag)
                {
                    chrtEl_legLength.MarkerBackGround.CharacterIndex = chrtEl_legLength.MarkerBackGround.CharacterIndex == 118? 94 : 118;
                    chrtEl_legLength.MarkerBackGround.InnerCharacterIndex = chrtEl_legLength.MarkerBackGround.InnerCharacterIndex == 119 ? 95 : 119;
                }

            }
            else if (SelectedElement.Name.StartsWith("CircleDistace"))
            {
                bool Flag = false;
                ILine ln = new LineClass();
                ln.FromPoint = _position;
                ln.ToPoint = ((IArea)geom).Centroid;

                double angl = ChartsHelperClass.CheckedAngle(ln.Angle * 180 / Math.PI + 90, ref Flag);
                (SelectedElement as ChartElement_SimpleText).Slope = angl;
            }

                #endregion

             _position = (IPoint)EsriUtils.ToGeo(_position, m_hookHelper.ActiveView.FocusMap, pSpatialReference);

            #region "unlink" airspace annotation

            if ((SelectedElement is ChartElement_SigmaCollout_Airspace && Shift == 3 && ((ChartElement_SigmaCollout_Airspace)SelectedElement).Frame.DrawLeader)
            || (SelectedElement is ChartElement_TextArrow && Shift == 3 && ((ChartElement_TextArrow)SelectedElement).Name.StartsWith("Airspace_Simple")))
            {
                var ancrpnt = SelectedElement is ChartElement_SigmaCollout_Airspace ? ((ChartElement_SigmaCollout_Airspace)SelectedElement).Anchor : ((ChartElement_TextArrow)SelectedElement).Anchor;

                double dX = startPosition.X - ancrpnt.X;
                double dY = startPosition.Y - ancrpnt.Y;

                if (SelectedElement is ChartElement_SigmaCollout_Airspace)
                    ((ChartElement_SigmaCollout_Airspace)SelectedElement).Anchor = new AncorPoint(_position.X - dX, _position.Y - dY);
                else
                    ((ChartElement_TextArrow)SelectedElement).Anchor = new AncorPoint(_position.X - dX, _position.Y - dY);

            }

            #endregion

            if (SelectedElement is ChartElement_ILSCollout && SelectedElement.Name.StartsWith(""))
            {
                ((ChartElement_ILSCollout)SelectedElement).Anchor = new AncorPoint(_position.X, _position.Y);
            }

            IElement el = SelectedElement.ConvertToIElement() as IElement;
            if (!activeFrameName.ToUpper().StartsWith("LAYERS"))
            {
                ChartElementsManipulator.UpdateSingleElement_Mirror(SelectedElement.Id.ToString(), el, ref SelectedElement, _position);
            }
            else
                ChartElementsManipulator.UpdateSingleElementToDataSet(SelectedElement.Name, SelectedElement.Id.ToString(), el, ref SelectedElement, _position,false);


            #region move along curve

            //if (SelectedElement.Name.StartsWith("ProcedureLegName") || SelectedElement.Name.StartsWith("GeoBorder_name"))
            if (SelectedElement.Name.StartsWith("ProcedureLegName"))
            {
                //LegArincType = SelectedElement.Name.StartsWith("GeoBorder_name") ? "AF" : ChartElementsManipulator.GetLegArincType(SelectedElement.Name, SelectedElement.LinckedGeoId);
                LegArincType = ChartElementsManipulator.GetLegArincType(SelectedElement.Name, SelectedElement.LinckedGeoId);

                int pIndx = GetPointIndexOnLine(geom, _position);

                if (LegArincType.StartsWith("AF"))
                {
                    IPolyline ln = new PolylineClass();
                    IPointCollection geomCollection = (IPointCollection)geom;
                    IPointCollection NewgeomCollection = (IPointCollection)ln;

                    if ((SelectedElement as ChartElement_SimpleText).HorizontalAlignment == horizontalAlignment.Left)
                    {
                        if (geomCollection.PointCount - pIndx < 10) pIndx = pIndx = geomCollection.PointCount / 2;

                        for (int i = pIndx; i < geomCollection.PointCount; i++)
                        {
                            NewgeomCollection.AddPoint(geomCollection.get_Point(i));
                        }

                    }
                    else
                    {
                        if (pIndx == 0) pIndx = geomCollection.PointCount / 2;
                        for (int i = 0; i < pIndx; i++)
                        {
                            NewgeomCollection.AddPoint(geomCollection.get_Point(i));
                        }
                       // ln.ReverseOrientation();
                    }

                    el = SelectedElement.ConvertToIElement() as IElement;
                    ChartElementsManipulator.UpdateSingleElementToDataSet(SelectedElement.Name, SelectedElement.Id.ToString(), el, ref SelectedElement, ln);


                }
            }

            #endregion

            SigmaDataCash.ChartElementsTree.Focus();
        }

        private double GetNewSlope(IGeometry geom, IPoint _position)
        {
            try
            {
                IPoint endP = null;
                IPoint strP = null;
                int indx = -1;
                ILine ln = new LineClass();
                ln.FromPoint = _position;
                ln.ToPoint = ((IPointCollection)geom).get_Point(0);
                double len = ln.Length;

                for (int i = 0; i < ((IPointCollection)geom).PointCount; i++)
                {
                    ln.ToPoint = ((IPointCollection)geom).get_Point(i);
                    if (ln.Length <= len)
                    {
                        indx = i;
                        len = ln.Length;

                        if (i != 0 && i != ((IPointCollection)geom).PointCount - 1)
                        {
                            strP = ((IPointCollection)geom).get_Point(i - 1);
                            endP = ((IPointCollection)geom).get_Point(i + 1);
                        }
                        else if (i == 0)
                        {
                            strP = ((IPointCollection)geom).get_Point(0);
                            endP = ((IPointCollection)geom).get_Point(i + 1);
                        }
                        else if (i == ((IPointCollection)geom).PointCount - 1)
                        {
                            strP = ((IPointCollection)geom).get_Point(i - 1);
                            endP = ((IPointCollection)geom).get_Point(i);
                        }

                    }

                }

                ln.FromPoint = strP;
                ln.ToPoint = endP;
                bool Flag = false;

                double newSlope = ChartsHelperClass.CheckedAngle(ln.Angle * 180 / Math.PI, ref Flag);
                return newSlope;

            }
            catch { return 0; }
        }

        private int GetPointIndexOnLine(IGeometry geom, IPoint _position)
        {
            try
            {
                int indx = -1;
                ILine ln = new LineClass();
                ln.FromPoint = _position;
                ln.ToPoint = ((IPointCollection)geom).get_Point(0);
                double len = ln.Length;
                indx = -1;
                for (int i = 0; i < ((IPointCollection)geom).PointCount; i++)
                {
                    ln.ToPoint = ((IPointCollection)geom).get_Point(i);
                    if (ln.Length <= len)
                    {
                        indx = i;
                        len = ln.Length;
                    }

                }

                return indx;

            }
            catch { return -1; }
        }

        private void StopFeedBackProcess_ChartElement_GraphicsChartElement(int X, int Y, int Shift)
        {
            IPoint newPos = null;
            IGeometry res = SelectedElement.StopFeedback(feedBack, (int)_position.X, (int)_position.Y, geom, Shift);
            if (res.GeometryType == esriGeometryType.esriGeometryPolygon) newPos = ((IArea)res).Centroid;
            else newPos = (IPoint)res;
            ((GraphicsChartElement)SelectedElement).Position = new AncorPoint(newPos.X, newPos.Y);
            IElement el = SelectedElement.ConvertToIElement() as IElement;

            IGraphicsContainer pGraphicsContainer = (m_hookHelper.PageLayout as IActiveView).GraphicsContainer;

            ChartElementsManipulator.UpdateGraphicsElement(pGraphicsContainer, SelectedElement, ref el,newPos);
            ChartElementsManipulator.UpdateGraphicsElementToDataSet(SelectedElement.Name, SelectedElement.Id.ToString(), el, ref SelectedElement);
        }

        #endregion
    }
}
