using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using MvvmCore;
using TOSSM.Util;

namespace TOSSM.ViewModel.Control.DataTocView
{
    public class GeoDataCalculationContext : ViewModelBase, ICalculationContext
    {
        private MtObservableCollection<DataLayer> _layers;
        public MtObservableCollection<DataLayer> Layers
        {
            get { return _layers ?? (_layers = new MtObservableCollection<DataLayer>()); }
        }

        public GeoDataCalculationContext()
        {
            MainManagerModel.Instance.GeoDataCalculationContext = this;
        }


        #region Implementation of ICalculationContext

        public double ScreenPixelXInMap { get; set; }
        public double ScreenPixelYInMap { get; set; }
        public IActiveView ActiveView { get; set; }

        #endregion

        //public readonly Dictionary<FeatureType, DataLayer> ShapesPerFeatureType = new Dictionary<FeatureType, DataLayer>();


        private bool _zoomChanged;
        private double _oldScreenPixelXInMap;

        public void MapZoomChanged()
        {
            const int radius = 4;
            IPoint screenPoint1=new PointClass{X=0,Y=0};
            IPoint screenPoint2 = new PointClass { X = radius, Y = radius };
            IPoint mapPoint1=EsriUtil.GetMapCoordinatesFromScreenCoordinates(screenPoint1, ActiveView);
            IPoint mapPoint2=EsriUtil.GetMapCoordinatesFromScreenCoordinates(screenPoint2, ActiveView);

            ScreenPixelXInMap = Math.Abs(mapPoint1.X - mapPoint2.X);
            ScreenPixelYInMap = Math.Abs(mapPoint1.Y - mapPoint2.Y);

            _zoomChanged = false;
            if (Math.Abs(_oldScreenPixelXInMap - ScreenPixelXInMap) < 0.00001) return;

            _zoomChanged = true;
            _oldScreenPixelXInMap = ScreenPixelXInMap;

            foreach (DataLayer packet in Layers)
            {
                foreach (var info in packet.Lines)
                {
                    info.CheckIfCanBeConsideredAsPoint(this);
                }
                foreach (var info in packet.Polygons)
                {
                    info.CheckIfCanBeConsideredAsPoint(this);
                }
            }
        }


        public void DrawAllShapes()
        {
            if (ActiveView == null) return;

            var stopwatch = new Stopwatch();
            stopwatch.Start();


            MapZoomChanged();

            IDisplay display = ActiveView.ScreenDisplay;

            display.StartDrawing(0, (short)esriScreenCache.esriNoScreenCache);

            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Color = EsriUtil.CreateRgbColor(10, 100, 255, 100);
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            simpleLineSymbol.Width = 1;


            ITextSymbol simpleTextSymbol = new TextSymbolClass();
            simpleTextSymbol.Color = EsriUtil.CreateRgbColor(10, 0, 100, 0);

            ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
            simpleMarkerSymbol.Color = EsriUtil.CreateRgbColor(10, 0, 100, 0); 
            simpleMarkerSymbol.Outline = false;
            simpleMarkerSymbol.OutlineColor = EsriUtil.CreateRgbColor(10, 0, 100, 0); 
            simpleMarkerSymbol.Size = 5;
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;


            // Set up the Raster Op-Code to help the flash mechanism
            //ISymbol pSymbol = pMarkerSymbol as ISymbol;
            //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            //display.SetSymbol((ISymbol)pSymbol);



            display.SetSymbol((ISymbol)simpleLineSymbol);
            //display.SetSymbol((ISymbol)simpleTextSymbol);
            display.SetSymbol((ISymbol)simpleMarkerSymbol);

            display.Filter = new TransparencyDisplayFilterClass { Transparency = 125 };
            //display.Filter = new DimDisplayFilterClass{DimValue = 100, Opacity = 100};

            var stopwatch2 = new Stopwatch();
            stopwatch2.Start();

           
            RebulidClusters();

            stopwatch2.Stop();
            ClusterTime = "Cluster created in: " + stopwatch2.ElapsedMilliseconds + " ms";

            foreach (var layer in Layers.Where(t=>t.IsVisible))
            {
                DrawLayer(layer, display);
            }

            display.Filter = new TransparencyDisplayFilterClass { Transparency = 255 };

            display.FinishDrawing();

            stopwatch.Stop();

            DrawTime = "Draw in: " + stopwatch.ElapsedMilliseconds + " ms";

        }


        public bool TryConsume(ShapeInfo shape, List<ShapeCluster> clusters, bool createNewClusterOnFail = true)
        {
            if (!shape.IsVisible) return false;
            if (shape.IsConsumed) return false;
            var consumed = clusters.Any(cluster => cluster.TryConsume(shape, this));
            if (!consumed && createNewClusterOnFail)
            {
                clusters.Add(new ShapeCluster(shape.CenterPoint));
            }
            return consumed;
        }


        public void RebulidClusters()
        {
            foreach (var layer in Layers.Where(t => !t.IsClustering))
            {
                foreach (var info in layer.Points)
                {
                    info.ResetVisibility(ActiveView.Extent);
                }
                foreach (var info in layer.Lines)
                {
                    info.ResetVisibility(ActiveView.Extent);
                }
                foreach (var info in layer.Polygons)
                {
                    info.ResetVisibility(ActiveView.Extent);
                }
            }

            if (_zoomChanged)
            {

                foreach (var layer in Layers.Where(t => t.IsClustering))
                {
                    foreach (var info in layer.Points)
                    {
                        info.ResetVisibility(ActiveView.Extent);
                    }
                    foreach (var info in layer.Lines)
                    {
                        info.ResetVisibility(ActiveView.Extent);
                    }
                    foreach (var info in layer.Polygons)
                    {
                        info.ResetVisibility(ActiveView.Extent);
                    }

                    //use clusters for point
                    layer.Clusters.Clear();
                    foreach (var shape in layer.Points)
                    {
                        shape.IsConsumed = false;
                    }
                    foreach (var shape in layer.Lines)
                    {
                        shape.IsConsumed = false;
                    }
                    foreach (var shape in layer.Polygons)
                    {
                        shape.IsConsumed = false;
                    }
                    foreach (var shape in layer.Points)
                    {
                        TryConsume(shape, layer.Clusters);
                    }
                    foreach (var shape in layer.Lines.Where(t => t.IsConsideredAsPoint))
                    {
                        TryConsume(shape, layer.Clusters);
                    }
                    foreach (var shape in layer.Polygons.Where(t => t.IsConsideredAsPoint))
                    {
                        TryConsume(shape, layer.Clusters);
                    }

                    ClusterCount = "Clusters: " + layer.Clusters.Count.ToString();
                }
            }
            else
            {
                foreach (var layer in Layers.Where(t => t.IsClustering))
                {
                    foreach (var cluster in layer.Clusters)
                    {
                        cluster.ResetCenterPointVisibility(ActiveView.Extent);
                    }
                    
                    foreach (var info in layer.Lines)
                    {
                        info.ResetVisibility(ActiveView.Extent);
                    }
                    foreach (var info in layer.Polygons)
                    {
                        info.ResetVisibility(ActiveView.Extent);
                    }

                    //use clusters for point
                    foreach (var shape in layer.Points)
                    {
                        shape.ResetVisibility(ActiveView.Extent);
                        TryConsume(shape, layer.Clusters);
                    }

                    //foreach (var shape in shapePacket.Lines.Where(t => t.IsConsideredAsPoint))
                    //{
                    //    TryConsume(shape, shapePacket.Clusters);
                    //}
                    //foreach (var shape in shapePacket.Polygons.Where(t => t.IsConsideredAsPoint))
                    //{
                    //    TryConsume(shape, shapePacket.Clusters);
                    //}

                    ClusterCount = "Clusters: " + layer.Clusters.Count.ToString();
                }
            }
           
        }

        public string DrawTime;
        public string ClusterCount;
        public string ClusterTime;

        public void DrawLayer(DataLayer dataLayer, IDisplay display)
        {
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Color = EsriUtil.CreateRgbColor(100, dataLayer.OutLineColor.R, dataLayer.OutLineColor.R, dataLayer.OutLineColor.R);
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            simpleLineSymbol.Width = 1;

            display.SetSymbol((ISymbol)simpleLineSymbol);
            
            foreach (var shape in dataLayer.Lines.Where(t => t.IsVisible && !t.IsConsideredAsPoint))
            {
                display.DrawPolyline(shape.Shape);
            }

            foreach (var shape in dataLayer.Polygons.Where(t => t.IsVisible && !t.IsConsideredAsPoint))
            {
                display.DrawPolyline(shape.Shape);
            }

            SetClusterSymbol(1, display);
            foreach (var shape in dataLayer.Points.Where(t => t.IsVisible && !t.IsConsumed))
            {
                display.DrawPoint(shape.Shape);
            }

            foreach (var shape in dataLayer.Lines.Where(t => t.IsVisible && t.IsConsideredAsPoint))
            {
                display.DrawPoint(shape.Shape);
            }

            foreach (var shape in dataLayer.Polygons.Where(t => t.IsVisible && t.IsConsideredAsPoint))
            {
                display.DrawPoint(shape.Shape);
            }

            if (!dataLayer.IsClustering) return;
            //redraw clusters
            var oldClusterSize = 0;
            foreach (var cluster in dataLayer.Clusters.Where(t => t.IsCenterPointVisible).OrderBy(t => t.Count))
            {
                var clusterSize = (int) Math.Min(16, Math.Max(4, Math.Sqrt(cluster.Count)));
                if (clusterSize != oldClusterSize)
                {
                    oldClusterSize = clusterSize;
                    SetClusterSymbol(cluster.Count, display);
                }
                display.DrawPoint(cluster.CenterPoint);
            }
        }


        private static void SetClusterSymbol(int clusterCount, IDisplay display)
        {
            ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
            simpleMarkerSymbol.Color = EsriUtil.CreateRgbColor(10, 0, 100, 0); 
            simpleMarkerSymbol.Outline = false;
            simpleMarkerSymbol.OutlineColor = EsriUtil.CreateRgbColor(10, 0, 100, 0);
            simpleMarkerSymbol.Size = Math.Min(16,Math.Max(4, Math.Sqrt(clusterCount)));
            simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            display.SetSymbol((ISymbol)simpleMarkerSymbol);
        }

        public void Update()
        {
            ActiveView.Refresh();
        }

        public void AddDataLayer(FeatureType featureType, DateTime actualDate)
        {
            var items = CurrentDataContext.CurrentService.GetActualDataByDate(
                   new FeatureId
                       {
                           FeatureTypeId = (int)featureType
                       }, 
                       false, 
                       actualDate);

            var dataLayer = new DataLayer
                                {
                                    OnUpdateAction = Update, 
                                    FeatureType = featureType, 
                                    ActualDate = actualDate
                                };
           
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                (Action)(
            () =>
            {
                foreach (var abstractState in items)
                {
                    var aimFeature = abstractState.Data;
                    GeometryFormatter.PrepareGeometry(aimFeature, true);

                    var esriList = aimFeature.PropertyExtensions.Where(
                        t => t is EsriPropertyExtension).Cast
                        <EsriPropertyExtension>().ToList();
                    if (esriList.Count <= 0) continue;

                    foreach (var propertyExtension in esriList)
                    {
                        var esri = propertyExtension.EsriObject;
                        if (esri.IsEmpty)
                        {
                            int g = 0;
                            //error!!
                        }
                        else
                        {
                            switch (esri.GeometryType)
                            {
                                case esriGeometryType.esriGeometryPoint:
                                    dataLayer.Points.Add(new ShapeInfo(esri, this));
                                    break;
                                case esriGeometryType.esriGeometryPolyline:
                                    dataLayer.Lines.Add(new ShapeInfo(esri, this));
                                    break;
                                case esriGeometryType.esriGeometryPolygon:
                                    dataLayer.Polygons.Add(new ShapeInfo(esri, this));
                                    break;
                            }
                        }

                    }
                }
                dataLayer.FeatureType = featureType;
                Layers.Add(dataLayer);
                Update();
            }));
        }
    }
}