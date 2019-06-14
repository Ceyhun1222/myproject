using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ANCOR.MapElements;
using ANCOR.MapCore;
using ESRI.ArcGIS.Geodatabase;
using EsriWorkEnvironment;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.esriSystem;
using System.Collections.Generic;

namespace SigmaChart.CmdsMenu
{
    /// <summary>
    /// Summary description for SigmaFrame.
    /// </summary>
    [Guid("a78d7d7b-7589-4602-9c10-abb564ead78c")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SigmaChart.SigmaFrame")]
    public sealed class SigmaFrame : BaseTool
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

        public SigmaFrame()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "SigmaChart"; //localizable text
            base.m_caption = "create Sigma Chart Frame";  //localizable text 
            base.m_message = "create Sigma Chart Frame";  //localizable text
            base.m_toolTip = "Sigma Chart";  //localizable text
            base.m_name = "Sigma Chart";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            try
            {
                //
                // TODO: change resource name if necessary 
                //
                base.m_bitmap = global::SigmaChart.Properties.Resources.EditingGeneralize16;
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
                m_application = hook as IApplication;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
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
            // TODO: Add SigmaFrame.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (!m_hookHelper.ActiveView.FocusMap.Name.StartsWith("Layers"))
            {
                MessageBox.Show("Please, activate main data view 'Layers'");
                return;
            }


            #region инициализация SigmaDataCash
            if (SigmaDataCash.AnnotationFeatureClassList == null || SigmaDataCash.AnnotationFeatureClassList.Count <= 0)
            {
                IApplication m_application = m_hookHelper.Hook as IApplication;

                for (int i = 0; i < ((IMxDocument)m_application.Document).ContentsViewCount; i++)
                {
                    IContentsView cnts = ((IMxDocument)m_application.Document).get_ContentsView(i);

                    string cntxtName = ((IMxDocument)m_application.Document).ContentsView[i].Name;

                    if (cntxtName.StartsWith("ANCORTOCLayerView"))
                    {
                        ((IMxDocument)m_application.Document).CurrentContentsView = cnts;
                        ((IMxDocument)m_application.Document).ContentsView[i].Refresh(cntxtName);
                    }

                }
            }
            #endregion

            // создание DataFrame
            if (!(m_hookHelper.ActiveView is IPageLayout)) return;

            IPolygon rctngl = DrawPolygon(m_hookHelper.ActiveView);
            if (rctngl.IsEmpty) return;
            

            ESRI.ArcGIS.Display.IRgbColor rgbColor = new ESRI.ArcGIS.Display.RgbColorClass();
            rgbColor.Red = 255;

            //AddGraphicToMap(m_hookHelper.ActiveView.FocusMap, rctngl, rgbColor, rgbColor);

            IPoint cntr = (rctngl as IArea).Centroid;

            AncorPoint ancrpnt = new AncorPoint(cntr.X, cntr.Y);

            ChartElement_Frame mpframe = new ChartElement_Frame(ancrpnt);
            mpframe.FrameName = GetName(); //"SigmaFrame "+Guid.NewGuid().ToString();
            mpframe.FrameWidth = rctngl.Envelope.LowerRight.X - rctngl.Envelope.LowerLeft.X;
            mpframe.FrameHeight =rctngl.Envelope.UpperLeft.Y - rctngl.Envelope.LowerRight.Y;
         

            IElement pElement = mpframe.ConvertToIElement(m_hookHelper.ActiveView.FocusMap, rctngl.Envelope,false) as IElement;

            #region позиционирование выбранного региона карты

            int x; int y;
            m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(rctngl.Envelope.UpperLeft, out x, out y);
            IPoint Upperleft = ((IActiveView)m_hookHelper.ActiveView.FocusMap).ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

            m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(rctngl.Envelope.UpperRight, out x, out y);
            IPoint UpperRight = ((IActiveView)m_hookHelper.ActiveView.FocusMap).ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

            m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(rctngl.Envelope.LowerRight, out x, out y);
            IPoint LowerRight = ((IActiveView)m_hookHelper.ActiveView.FocusMap).ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

            m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(rctngl.Envelope.LowerLeft, out x, out y);
            IPoint LowerLeft = ((IActiveView)m_hookHelper.ActiveView.FocusMap).ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

            ILayer _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportHeliport");
            if (_Layer == null) _Layer = EsriUtils.getLayerByName(m_hookHelper.ActiveView.FocusMap, "AirportCartography");
            var fc = ((IFeatureLayer)_Layer).FeatureClass;

            ISpatialReference pSpatialReference = (fc as IGeoDataset).SpatialReference;
            Upperleft = (IPoint)EsriUtils.ToGeo(Upperleft, m_hookHelper.ActiveView.FocusMap, pSpatialReference);
            UpperRight = (IPoint)EsriUtils.ToGeo(UpperRight, m_hookHelper.ActiveView.FocusMap, pSpatialReference);
            LowerRight = (IPoint)EsriUtils.ToGeo(LowerRight, m_hookHelper.ActiveView.FocusMap, pSpatialReference);
            LowerLeft = (IPoint)EsriUtils.ToGeo(LowerLeft, m_hookHelper.ActiveView.FocusMap, pSpatialReference);

            IEnvelope selectedRec = new EnvelopeClass();
            selectedRec.LowerLeft = LowerLeft;
            selectedRec.LowerRight = LowerRight;
            selectedRec.UpperLeft = Upperleft;
            selectedRec.UpperRight = UpperRight;


            ((IActiveView)((IMapFrame)pElement).Map).Extent = selectedRec;//((IActiveView)m_hookHelper.ActiveView.FocusMap).Extent; // мистика ???
            LoadLayersToFrame(((IMapFrame)pElement).Map, m_hookHelper.ActiveView.FocusMap);
            ((IActiveView)((IMapFrame)pElement).Map).Extent = selectedRec;//((IActiveView)m_hookHelper.ActiveView.FocusMap).Extent; // мистика ???

            #endregion

            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)m_hookHelper.ActiveView;

            #region 

            ///<remarks>
            ///прежде чем создавать новый frame - проверяем есть ли на карте уже созданные fram'ы
            ///если нет, т.е это первый создаваемый frame - скидываем свойтсво ReflectionHidden у всех элементов на false
            ///</remarks>

            bool res = false;
            pGraphicsContainer.Reset();
            IElementProperties3 docElementProperties;
            IElement sigma_el = pGraphicsContainer.Next();
            while (sigma_el != null)
            {
                docElementProperties = sigma_el as IElementProperties3;
                if (docElementProperties.Name.StartsWith("SigmaFrame"))
                {
                    res = true;
                    break;
                }
                sigma_el = pGraphicsContainer.Next();

            }
            if (!res) SigmaDataCash.ChartElementList.ForEach(ResetMirrorPlaced);

            #endregion
            


            pGraphicsContainer.AddElement(pElement, 0);


            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void ResetMirrorPlaced(object obj)
        {
            ((AbstractChartElement)obj).ReflectionHidden = false;
        }

        private string GetName()
        {
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)m_hookHelper.ActiveView;
            IMxDocument document = (IMxDocument)m_application.Document;
            IMaps maps = document.Maps;

            string res = "SigmaFrame "+ Guid.NewGuid().ToString();
            //List<int> lst = new List<int>();


            //for (int i = 0; i <= maps.Count - 1; i++)
            //{

            //    IFrameElement frameElement = pGraphicsContainer.FindFrame(maps.get_Item(i));
            //    IMapFrame mapFrame = frameElement as IMapFrame;
                

            //    if (((IElementProperties2)mapFrame).Name.StartsWith("Layer")) continue;

            //    string[] arr = ((IElementProperties2)mapFrame).Name.Split(' ');

            //    lst.Add(Convert.ToInt32(arr[1]));
            //}


            //if (lst.Count > 0)
            //{
            //    lst.Sort();
            //    res = "SigmaFrame " + (lst[lst.Count - 1]+1).ToString();
            //}


            return res;

        }

        private void LoadLayersToFrame(IMap targetMap, IMap sourceMap)
        {
            ILegendInfo legendInfo;
            ILegendGroup legendGroup;

            for (int i = sourceMap.LayerCount - 1; i >= 0; i--)
            {
                ILayer L = sourceMap.get_Layer(i);
                if (L.Name.ToUpper().StartsWith("MIRROR")) continue;
                if (L.Name.ToUpper().StartsWith("GEOBORDER")) continue;
                if ((L is IRasterLayer) || (L is IFeatureLayer) || (L is IGeoFeatureLayer))
                {
                    legendInfo = (ILegendInfo)L;
                    legendGroup = legendInfo.get_LegendGroup(0);
                    legendGroup.Visible = false;
                }

                if (L is ICompositeLayer )
                {
                    if (!L.Name.StartsWith("Annotations"))
                    {
                        targetMap.AddLayer(L);
                        for (int j = 0; j <= ((ICompositeLayer)L).Count - 1; j++)
                        {

                            ILayer L1 = ((ICompositeLayer)L).get_Layer(j);

                            if (L1 is ICompositeLayer)
                            {
                                targetMap.AddLayer(L1);
                                for (int k = 0; k <= ((ICompositeLayer)L1).Count - 1; k++)
                                {
                                    ILayer L2 = ((ICompositeLayer)L1).get_Layer(k);
                                    L2.Visible = ((ICompositeLayer)L1).get_Layer(k).Visible;
                                    targetMap.AddLayer(L2);
                                }
                            }
                            else
                            {
                                L1.Visible = L.Visible;
                                targetMap.AddLayer(L1);
                            }
                        }
                    }
                }
                else
                {
                    ILayer nlayer = CloneLayer(L);
                    if (nlayer != null)
                        targetMap.AddLayer(nlayer);
                    //targetMap.AddLayer(L);
                }

            }

            if (SigmaDataCash.ChartElementList != null && SigmaDataCash.ChartElementList.Count > 0)
            {
                
                ILayer nlayer = (ILayer)(new FDOGraphicsLayer());
                nlayer.Name = "mirror";
                IFeatureLayer newlayer = (IFeatureLayer)nlayer;
                newlayer.FeatureClass = ChartElementsManipulator.GetLinkedFeatureClass("Mirror");
                newlayer.Name = "mirror";

                targetMap.AddLayer(newlayer);
            }
            
        }

        private ILayer CloneLayer(ILayer L)
        {

            ILayer nlayer = (ILayer)(new FeatureLayer());
            nlayer.Name = L.Name;
            IFeatureLayer newlayer = (IFeatureLayer)nlayer;

            if (((IFeatureLayer)L).FeatureClass == null)
                return null;

            newlayer.FeatureClass = ((IFeatureLayer)L).FeatureClass;
            newlayer.Name = L.Name;
            newlayer.Visible = L.Visible;

            IUniqueValueRenderer pUniqueValueRenderer = (L as IGeoFeatureLayer).Renderer as IUniqueValueRenderer;
            if (pUniqueValueRenderer != null)
            {
                (newlayer as IGeoFeatureLayer).Renderer = (IFeatureRenderer)pUniqueValueRenderer;
            }
            else
                (newlayer as IGeoFeatureLayer).Renderer = (L as IGeoFeatureLayer).Renderer;

            return newlayer;

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add SigmaFrame.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add SigmaFrame.OnMouseUp implementation
        }

        #endregion

        public IPolygon DrawPolygon(ESRI.ArcGIS.Carto.IActiveView activeView)
        {

            if (activeView == null)
            {
                return null;
            }

            ESRI.ArcGIS.Display.IScreenDisplay screenDisplay = activeView.ScreenDisplay;

            // Constant
            screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast
            ESRI.ArcGIS.Display.IRgbColor rgbColor = new ESRI.ArcGIS.Display.RgbColorClass();
            rgbColor.Red = 255;

            ESRI.ArcGIS.Display.IColor color = rgbColor; // Implicit Cast
            ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
            simpleFillSymbol.Color = color;

            ESRI.ArcGIS.Display.ISymbol symbol = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
            ESRI.ArcGIS.Display.IRubberBand rubberBand = new ESRI.ArcGIS.Display.RubberRectangularPolygonClass();
            ESRI.ArcGIS.Geometry.IGeometry geometry = rubberBand.TrackNew(screenDisplay, symbol);

            screenDisplay.SetSymbol(symbol);
            screenDisplay.DrawPolygon(geometry);
            screenDisplay.FinishDrawing();

            IPolygon polygone = (IPolygon)geometry;

            return polygone;

            
        }

        public void AddGraphicToMap(IMap map, IGeometry geometry, IRgbColor rgbColor, IRgbColor outlineRgbColor)
        {
            IGraphicsContainer graphicsContainer = (IGraphicsContainer)map;
            // Explicit cast.
            IElement element = null;

            if ((geometry.GeometryType) == esriGeometryType.esriGeometryPoint)
            {
                // Marker symbols.
                ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
                simpleMarkerSymbol.Color = rgbColor;
                simpleMarkerSymbol.Outline = true;
                simpleMarkerSymbol.OutlineColor = outlineRgbColor;
                simpleMarkerSymbol.Size = 15;
                simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                IMarkerElement markerElement = new MarkerElementClass();
                markerElement.Symbol = simpleMarkerSymbol;
                element = (IElement)markerElement; // Explicit cast.
            }
            else if ((geometry.GeometryType) == esriGeometryType.esriGeometryPolyline)
            {
                //  Line elements.
                ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                simpleLineSymbol.Color = rgbColor;
                simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                simpleLineSymbol.Width = 5;
                ILineElement lineElement = new LineElementClass();
                lineElement.Symbol = simpleLineSymbol;
                element = (IElement)lineElement; // Explicit cast.
            }
            else if ((geometry.GeometryType) == esriGeometryType.esriGeometryPolygon)
            {
                // Polygon elements.
                ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                simpleFillSymbol.Color = rgbColor;
                simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                IFillShapeElement fillShapeElement = new PolygonElementClass();
                fillShapeElement.Symbol = simpleFillSymbol;
                element = (IElement)fillShapeElement; // Explicit cast.
            }

            if (!(element == null))
            {
                element.Geometry = geometry;
                IElementProperties prop = (IElementProperties)element;
                prop.Name = "SigmaFrame";

                graphicsContainer.AddElement(element, 0);
            }

        }
    }
}
