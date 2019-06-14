using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using EsriWorkEnvironment;

namespace ARENA.Util
{
    public class ArenaSnapping : IDisposable
    {
        private bool m_disposed;
        private IFeatureCache2 m_FeatureCache2;
        private IEnumLayer pElayer;
        private IFeature m_pFeature;
        private ESRI.ArcGIS.Geometry.IPoint mousePoint_page = new ESRI.ArcGIS.Geometry.PointClass();
        private ESRI.ArcGIS.Geometry.IPoint prevmousePoint_page = new ESRI.ArcGIS.Geometry.PointClass();
        private ESRI.ArcGIS.Geometry.IPoint mousePoint_map = new ESRI.ArcGIS.Geometry.PointClass();
        private ESRI.ArcGIS.Geometry.IPoint prevmousePoint_map = new ESRI.ArcGIS.Geometry.PointClass();
        private ESRI.ArcGIS.Geometry.IPoint currentPoint = new ESRI.ArcGIS.Geometry.PointClass();

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageControl;
        public IDisplayTransformation ax_ActiveView_FocusMap_DispTransformation;
        public IDisplayTransformation ax_ActiveView_DispTransformation;

        private IMap pMap;
        private bool m_CacheFilled2;
        private double m_Tolerance;

        private IMapControlEvents2_Ax_OnMouseMoveEventHandler mouseMoveDelegate_MapControl;
        private IPageLayoutControlEvents_Ax_OnMouseMoveEventHandler mouseMoveDelegate_PageControl;
        private IPageLayoutControlEvents_Ax_OnMouseDownEventHandler mouseDoneDelegate_PageControl;

        private ISpatialReference _spatRefGeo;
        private snappingState state;
        public enum snappingState { Active = 1, NotActive = 2 };

        private List<string> selFID;

        private IFeature selFeature;

        public IFeature SelFeature
        {
            get { return selFeature; }
            set { selFeature = value; }
        }

        public List<string> SnappedFeatureId
        {
            get { return selFID; }
            set { selFID = value; }
        }

        public snappingState State
        {
            get { return state; }
            set { state = value; }
        }

        public ISpatialReference SpatRefGeo
        {
            get { return _spatRefGeo; }
            set { _spatRefGeo = value; }
        }

        public IEnumLayer SnappedLayers
        {
            get { return pElayer; }
            set { pElayer = value; }
        }

        public double Tolerance
        {
            get { return m_Tolerance; }
            set { m_Tolerance = value; }
        }

        public ESRI.ArcGIS.Geometry.IPoint SnapPoint
        {
            get { return currentPoint; }
            set { currentPoint = value; }
        }

        public ArenaSnapping(ESRI.ArcGIS.Controls.AxMapControl axmapcontrol, IEnumLayer pEnumlayer, double toler, ISpatialReference spatRefGeo, IDisplayTransformation dsplTrans)
        {
            pElayer = pEnumlayer;
            axMapControl = axmapcontrol;
            //pMapControl = axMapControl.GetOcx() as IMapControl2;
            pMap = (axMapControl.GetOcx() as IMapControl2).Map;
            m_Tolerance = toler;
            mouseMoveDelegate_MapControl = new IMapControlEvents2_Ax_OnMouseMoveEventHandler(axMap_OnMouseMove);

            m_disposed = false;
            SpatRefGeo = spatRefGeo;
            ax_ActiveView_FocusMap_DispTransformation = dsplTrans;
            //_tool = snappingTool.snapTool;
            selFID = new List<string>();
        }

        public ArenaSnapping(ESRI.ArcGIS.Controls.AxPageLayoutControl axpagecontrol, IEnumLayer pEnumlayer, double toler, ISpatialReference spatRefGeo, IDisplayTransformation dsplTrans)
        {
            pElayer = pEnumlayer;
            axPageControl = axpagecontrol;
            //pMapControl = axMapControl.GetOcx() as IMapControl2;
            pMap = axpagecontrol.ActiveView.FocusMap;
            m_Tolerance = toler;
            mouseMoveDelegate_PageControl = new IPageLayoutControlEvents_Ax_OnMouseMoveEventHandler(axPage_OnMouseMove);

            mouseDoneDelegate_PageControl = new IPageLayoutControlEvents_Ax_OnMouseDownEventHandler(axPage_OnMouseDown);

            m_disposed = false;
            SpatRefGeo = spatRefGeo;
            ax_ActiveView_FocusMap_DispTransformation = dsplTrans;
            //_tool = snappingTool.snapTool;
            selFID = new List<string>();

            //_map = FcsMap;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    m_FeatureCache2 = null;
                    mousePoint_page = null;


                }

                // Release unmanaged resources

                m_disposed = true;
            }
        }

        ~ArenaSnapping()
        {
            Dispose(false);
        }

        public void StartSnapping()
        {
            StopSnapping();
            if (axMapControl != null) axMapControl.OnMouseMove += mouseMoveDelegate_MapControl;
            if (axPageControl != null)
            {
                axPageControl.OnMouseMove += mouseMoveDelegate_PageControl;
                axPageControl.OnMouseDown += mouseDoneDelegate_PageControl;
            }
            this.State = snappingState.Active;
        }

        public void StopSnapping()
        {
            if (axMapControl != null) axMapControl.OnMouseMove -= mouseMoveDelegate_MapControl;
            if (axPageControl != null)
            {
                axPageControl.OnMouseMove -= mouseMoveDelegate_PageControl;
                axPageControl.OnMouseDown -= mouseDoneDelegate_PageControl;

            }

            RemoveSnapSign();
            this.State = snappingState.NotActive;
            //_tool = snappingTool.snapTool;
        }

        private bool Snap(IEnumLayer pElayer, IPoint point, double tolerance)
        {

            if (point == null) return false;
            m_pFeature = null;
            double Dist, minDist;
            IFeature feature;
            IPoint pCachePt, pSnapPt;
            pSnapPt = point;
            IProximityOperator pProximity = (IProximityOperator)point;
            IProximityOperator pProximity1;
            minDist = tolerance * 1;
            if (m_FeatureCache2 == null)
                m_FeatureCache2 = new FeatureCacheClass();
            if (pElayer == null)
                return false;
            if (!m_CacheFilled2)
            {
                FillCache(pElayer, point, tolerance);
                m_CacheFilled2 = true;
            }

            if (!m_FeatureCache2.Contains(point)) FillCache(pElayer, point, tolerance);

            for (int Count = 0; Count < m_FeatureCache2.Count; Count++)
            {
                feature = m_FeatureCache2.get_Feature(Count);

                if ((feature.Shape != null) && (!feature.Shape.IsEmpty))
                {
                    Dist = pProximity.ReturnDistance(feature.Shape);
                    if (Dist == 0) continue;
                    pProximity1 = feature.Shape as IProximityOperator;
                    pCachePt = new PointClass();
                    pCachePt = pProximity1.ReturnNearestPoint(point, esriSegmentExtension.esriNoExtension);
                    if (Dist < minDist)
                    {
                        minDist = Dist;
                        pSnapPt = pCachePt;
                        m_pFeature = feature;

                        string FG = feature.get_Value(feature.Fields.FindField("FeatureGUID")).ToString();
                        if (this.SnappedFeatureId.IndexOf(FG) < 0) this.SnappedFeatureId.Add(FG);
                        //System.Diagnostics.Debug.WriteLine((feature.Class as IDataset).Name + " " + feature.get_Value(feature.Fields.FindField("FeatureGUID")).ToString());
                        //this.SelFeature = feature;

                        #region "подсветка IFeature"

                        ////System.Diagnostics.Debug.WriteLine((feature.Class as IDataset).Name);
                        // pMap.ClearSelection();
                        // ILayer _Layer = EsriUtils.getLayerByName(pMap, (feature.Class as IDataset).Name);
                        // IFeatureSelection pSelect = _Layer as IFeatureSelection;

                        // IQueryFilter queryFilter = new QueryFilterClass();
                        // queryFilter.WhereClause = "OBJECTID = " + feature.OID;
                        // if (pSelect != null)
                        // {
                        //     pSelect.Clear();
                        //     pSelect.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

                        // }

                        #endregion



                        break;
                    }
                }
            }
            if (minDist >= tolerance)
            {
                this.SnappedFeatureId.Clear();
                return false;
            }
            point.PutCoords(pSnapPt.X, pSnapPt.Y);
            return true;
        }

        private void FillCache(IEnumLayer pElayer, IPoint pPoint, double Distance)
        {
            m_FeatureCache2.Initialize(pPoint, Distance);
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.XMin = pPoint.X - Distance;
            pEnv.XMax = pPoint.X + Distance;
            pEnv.YMin = pPoint.Y - Distance;
            pEnv.YMax = pPoint.Y + Distance;
            m_FeatureCache2.AddLayers(pElayer, pEnv);
        }

        private void AddPoint(IMap pMap, IPoint pPoint, bool ShowSnapSign)
        {


            double R = 0.075;

            IGraphicsContainer pGraphicsContainer = null;
            if (pPoint == null) return;
            if (pMap == null) return;



            IConstructCircularArc pCircularArc = new CircularArcClass();

            pCircularArc.ConstructCircle(pPoint, R, true);


            ISegment pSegment = pCircularArc as ISegment;
            ISegmentCollection pSegCol = new RingClass();
            object Missing1 = Type.Missing;
            object Missing2 = Type.Missing;
            pSegCol.AddSegment(pSegment, ref Missing1, ref Missing2);
            IRing pRing = pSegCol as IRing;
            pRing.Close();
            IGeometryCollection pPolygon = new PolygonClass();
            pPolygon.AddGeometry(pRing, ref Missing1, ref Missing2);
            IGeometry pCirclGeo = pPolygon as IGeometry;
            ILineSymbol pLineSymble = new SimpleLineSymbolClass();
            pLineSymble.Width = 1.5;
            pLineSymble.Color = GetRGB(255, 0, 0, 255);
            ISimpleFillSymbol pSimFillSymbol = new SimpleFillSymbolClass();
            pSimFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;//esriSimpleFillStyle.esriSFSHollow;
            pSimFillSymbol.Color = GetRGB(255, 0, 0, 255);
            pSimFillSymbol.Outline = pLineSymble;
            IFillShapeElement pFillEle = new CircleElementClass();
            pFillEle.Symbol = pSimFillSymbol;

            IElement pEle = pFillEle as IElement;
            pEle.Geometry = pCirclGeo;
            IElementProperties docElementProperties = pEle as IElementProperties;
            docElementProperties.Name = "SnapSign";

            if (axMapControl != null) pGraphicsContainer = pMap as IGraphicsContainer;
            if (axPageControl != null)
            {
                pGraphicsContainer = axPageControl.ActiveView.GraphicsContainer;
            }


            if (ShowSnapSign) pGraphicsContainer.AddElement(pEle, 0);



            if (axMapControl != null) ((IActiveView)pMap).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            if (axPageControl != null) axPageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);




        }

        private void RemoveSnapSign()
        {
            return;
            IGraphicsContainer pGraphicsContainer = null;

            if (axMapControl != null) pGraphicsContainer = pMap as IGraphicsContainer;
            if (axPageControl != null)
            {
                pGraphicsContainer = axPageControl.ActiveView.GraphicsContainer;
            }


            pGraphicsContainer.Reset();
            IElement el = pGraphicsContainer.Next();
            while (el != null)
            {
                IElementProperties docElementProperties = el as IElementProperties;
                if (docElementProperties.Name.StartsWith("SnapSign"))
                {
                    pGraphicsContainer.DeleteElement(el);
                }
                el = pGraphicsContainer.Next();
            }
        }

        private void axMap_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {

            mousePoint_page.PutCoords(e.mapX, e.mapX);
            mousePoint_map = ax_ActiveView_FocusMap_DispTransformation.ToMapPoint(e.x, e.y);
            RemoveSnapSign();


            IPoint p = new PointClass();
            p.PutCoords(e.mapX, e.mapX);

            if (Snap(pElayer, mousePoint_map, m_Tolerance))
            {

                int x; int y;
                ax_ActiveView_FocusMap_DispTransformation.FromMapPoint(mousePoint_map, out x, out y);
                p = ax_ActiveView_DispTransformation.ToMapPoint(x, y);


                AddPoint(pMap, p, true);

                SnapPoint = mousePoint_map;

                axMapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;



            }
            else
            {
                SelFeature = null;
                SnappedFeatureId.Clear();
                RemoveSnapSign();
                AddPoint(pMap, p, false);
                SnapPoint = ax_ActiveView_FocusMap_DispTransformation.ToMapPoint(e.x, e.y);
                axMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;

            }


            ((IActiveView)pMap).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void axPage_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseMoveEvent e)
        {

            mousePoint_page.PutCoords(e.pageX, e.pageY);
            mousePoint_map = ax_ActiveView_FocusMap_DispTransformation.ToMapPoint(e.x, e.y);
            RemoveSnapSign();


            IPoint p = new PointClass();
            p.PutCoords(e.pageX, e.pageY);

            if (Snap(pElayer, mousePoint_map, m_Tolerance))
            {

                int x; int y;
                ax_ActiveView_FocusMap_DispTransformation.FromMapPoint(mousePoint_map, out x, out y);
                p = ax_ActiveView_DispTransformation.ToMapPoint(x, y);


                AddPoint(pMap, p, true);

                SnapPoint = mousePoint_map;

            }
            else
            {
                SelFeature = null;
                SnappedFeatureId.Clear();
                RemoveSnapSign();
                AddPoint(pMap, p, false);
                SnapPoint = ax_ActiveView_FocusMap_DispTransformation.ToMapPoint(e.x, e.y);
                axMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;

            }


            ((IActiveView)pMap).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }

        public IColor GetRGB(int R, int G, int B, int trancParency)
        {
            IRgbColor pRgb = new RgbColorClass();
            pRgb.Blue = B;
            pRgb.Green = G;
            pRgb.Red = R;
            IColor pColor = pRgb as IColor;
            pColor.Transparency = (byte)trancParency;
            return pColor;
        }

        public void axPage_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            if (prevmousePoint_page.IsEmpty)
            {
                prevmousePoint_page.PutCoords(e.pageX, e.pageY);
                prevmousePoint_map = ax_ActiveView_FocusMap_DispTransformation.ToMapPoint(e.x, e.y);
            }
            else
            {
                RemoveSnapSign();
                prevmousePoint_page = new ESRI.ArcGIS.Geometry.PointClass();
                prevmousePoint_map = new ESRI.ArcGIS.Geometry.PointClass();
            }

        }

        private double geTextAngle(ILine pLine)
        {
            double angle = 0;
            angle = pLine.Angle;
            return angle;

        }

    }
}
