using System;
using System.Collections.Generic;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using MvvmCore;
using VisualizerCommon.Remote;
using VisualizerEnvironment.Properties;
using VisualizerEnvironment.Util;
using VisualizerEnvironment.View;

namespace VisualizerEnvironment.ViewModel
{
    public class OnMapViewerToolViewModel : ViewModelBase , IMapProvider
    {
        #region esri serialization
        private const string PropertyName = "esri";

        public static byte[] EsriToBytes(object esriGeometry)
        {
            var memBlb = new MemoryBlobStream();
            IObjectStream objStr = new ObjectStream();
            objStr.Stream = memBlb;

            IPropertySet propertySet = new PropertySetClass();

            var perStr = (IPersistStream)propertySet;
            propertySet.SetProperty(PropertyName, esriGeometry);
            perStr.Save(objStr, 0);

            object obj;
            ((IMemoryBlobStreamVariant)memBlb).ExportToVariant(out obj);

            return (byte[])obj;
        }

        public static object EsriFromBytes(byte[] bytes)
        {
            try
            {
                var memBlobStream = new MemoryBlobStream();

                var varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                var anObjectStream = new ObjectStreamClass { Stream = memBlobStream };

                IPropertySet aPropSet = new PropertySetClass();

                var aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                return aPropSet.GetProperty(PropertyName);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Server handler
        private readonly List<GeometrySelection> _selection = new List<GeometrySelection>(); 
        private bool _isSelectionChanged;
        private void SetSelection(List<GeometrySelection> newSelection)
        {
            lock (_selection)
            {
                _selection.Clear();
                _selection.AddRange(newSelection);
                _isSelectionChanged = true;
            }
        }

        private void ClearSelection()
        {
            SetSelection(new List<GeometrySelection>());
        }
        #endregion

        #region Map updater
        public DispatcherTimer MapUpdater;

        public void OnMapChanged(string mapFile)
        {
            Settings.Default["MapDocument"] = mapFile;
        }
        public void UpdateMap()
        {
            if (!_isSelectionChanged) return;
            _isSelectionChanged = false;

            List<GeometrySelection> selectionToBeSet;
            lock (_selection)
            {
                selectionToBeSet = new List<GeometrySelection>(_selection);
            }
            try
            {
                SetSelectionInternal(selectionToBeSet);
            }
            catch (Exception)
            {
                
            }
        }
        #endregion

        #region UI

        public void OnLoaded()
        {

            MapForm = new MapForm
            {
                TopLevel = false,
                MapChangedAction = OnMapChanged
            };

           WindowsFormsHost = new WindowsFormsHost { Child = MapForm };

            MapForm.splitContainer1.Panel1Collapsed = true;
            MapForm.splitContainer1.Panel1.Hide();

            TocManagerAddIn.TocButton.IsCheckedChanged =
                () =>
                {
                    if (TocManagerAddIn.TocButton.IsChecked)
                    {
                        MapForm.splitContainer1.Panel1Collapsed = false;
                        MapForm.splitContainer1.Panel1.Show();

                        //MapForm.splitContainer1.SplitterDistance = 100;
                    }
                    else
                    {
                        MapForm.splitContainer1.Panel1Collapsed = true;
                        MapForm.splitContainer1.Panel1.Hide();
                    }
                };


            MapUpdater = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            MapUpdater.Tick += (a, b) =>
            {
                UpdateMap();
            };
            MapUpdater.Start();

            _mapAnimator = new ZoomAnimator
            {
                MapProvider = this
            };

            VisualizerServer.SetSelectionHandler = SetSelection;
            VisualizerServer.ClearSelectionHandler = ClearSelection;
            VisualizerServer.StartServer();
        }

        private WindowsFormsHost _windowsFormsHost;
        public WindowsFormsHost WindowsFormsHost
        {
            get { return _windowsFormsHost; }
            set
            {
                _windowsFormsHost = value;
                OnPropertyChanged("WindowsFormsHost");
            }
        }

        public MapForm MapForm { get; set; }

        #endregion

        #region Util

        private IPoint GetAnchor(IGeometry geometry)
        {
            switch ((geometry.GeometryType))
            {
                case esriGeometryType.esriGeometryPoint:
                    {
                        return (IPoint)geometry;
                    }
                case esriGeometryType.esriGeometryPolyline:
                    {
                        IPoint point = new PointClass();
                        var pc = ((IPointCollection)geometry);
                        if (pc.PointCount == 0) return null;
                        for (int i = 0; i < pc.PointCount; i++)
                        {
                            var currentpoint = pc.Point[i];
                            if (i == 0 || point.Y < currentpoint.Y)
                            {
                                point = currentpoint;
                            }
                        }
                        return point;
                    }
                case esriGeometryType.esriGeometryPolygon:
                    {
                        IPoint point = new PointClass();
                        var pc = ((IPointCollection)geometry);
                        if (pc.PointCount == 0) return null;
                        for (int i = 0; i < pc.PointCount; i++)
                        {
                            var currentpoint = pc.Point[i];
                            if (i == 0 || point.Y < currentpoint.Y)
                            {
                                point = currentpoint;
                            }
                        }
                        return point;
                    }
            }
            return null;
        }

        public IElement CreateCallout(string calloutText, IPoint anchorPoint)
        {
            var group = new GroupElementClass();

            var textElement = new TextElementClass
            {
                Text = calloutText,
                XOffset = -5,
                YOffset = 20,
                Background = new BalloonCalloutClass
                {
                    Style = esriBalloonCalloutStyle.esriBCSRoundedRectangle,
                    Symbol = new SimpleFillSymbolClass
                    {
                        Color = EsriUtil.CreateRgbColor(50, 200, 200, 255),
                        Style = esriSimpleFillStyle.esriSFSSolid,
                        Outline = new SimpleLineSymbolClass
                        {
                            Color = EsriUtil.CreateRgbColor(255, 0, 0, 0),
                            Style = esriSimpleLineStyle.esriSLSSolid,
                            Width = 1
                        }
                    },
                    AnchorPoint = anchorPoint,
                    LeaderTolerance = 0,
                    TopMargin = 2,
                    BottomMargin = 2,
                    LeftMargin = 2,
                    RightMargin = 2
                },
                Size = 10
            };
            ((IElement)textElement).Geometry = anchorPoint;


            //var shadowDepth = 0;
            //var m = 30;
            //var initedColor = 20;
            //for (var i = shadowDepth; i >= 1; i--)
            //{
            //    var shadowTextElement = new TextElementClass
            //    {
            //        Text = textElement.Text,
            //        XOffset = textElement.XOffset + i / 2.0,
            //        YOffset = textElement.YOffset - i,
            //        Background = new BalloonCalloutClass
            //        {
            //            Style = ((BalloonCalloutClass)textElement.Background).Style,
            //            Symbol = new SimpleFillSymbolClass
            //            {
            //                Color = EsriUtil.CreateRgbColor(255, (byte)(initedColor + i * m), (byte)(initedColor + i * m), (byte)(initedColor + i * m)),
            //                Style = esriSimpleFillStyle.esriSFSSolid,
            //                Outline = new SimpleLineSymbolClass
            //                {
            //                    Color = EsriUtil.CreateRgbColor(255, (byte)(initedColor + i * m), (byte)(initedColor + i * m), (byte)(initedColor + i * m)),
            //                    Style = esriSimpleLineStyle.esriSLSSolid,
            //                    Width = 1
            //                }
            //            },
            //            AnchorPoint = ((BalloonCalloutClass)textElement.Background).AnchorPoint,
            //            LeaderTolerance = ((BalloonCalloutClass)textElement.Background).LeaderTolerance,
            //            TopMargin = ((BalloonCalloutClass)textElement.Background).TopMargin,
            //            BottomMargin = ((BalloonCalloutClass)textElement.Background).BottomMargin,
            //            LeftMargin = ((BalloonCalloutClass)textElement.Background).LeftMargin,
            //            RightMargin = ((BalloonCalloutClass)textElement.Background).RightMargin,
            //        },
            //        Size = textElement.Size
            //    };
            //    ((IElement)shadowTextElement).Geometry = textElement.Geometry;
            //    group.AddElement(shadowTextElement);
            //}

            group.AddElement(textElement);
            return group;
        }

        public static IElement CreateElement(IGeometry geometry, IRgbColor rgbColor, IRgbColor outlineRgbColor)
        {
            IElement element = null;
            switch ((geometry.GeometryType))
            {
                case esriGeometryType.esriGeometryPoint:
                    {
                        // Marker symbols
                        ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbolClass();
                        simpleMarkerSymbol.Color = rgbColor;
                        simpleMarkerSymbol.Outline = true;
                        simpleMarkerSymbol.OutlineColor = outlineRgbColor;
                        simpleMarkerSymbol.Size = 10;
                        simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;

                        simpleMarkerSymbol.Color.Transparency = 10;

                        IMarkerElement markerElement = new MarkerElementClass();
                        markerElement.Symbol = simpleMarkerSymbol;
                        element = (IElement)markerElement;
                    }
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    {
                        //  Line elements
                        ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                        simpleLineSymbol.Color = rgbColor;
                        simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                        simpleLineSymbol.Width = 2;
                        ILineElement lineElement = new LineElementClass();
                        lineElement.Symbol = simpleLineSymbol;
                        element = (IElement)lineElement;
                    }
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    {
                        // Polygon elements
                        ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                        simpleLineSymbol.Color = outlineRgbColor;
                        simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                        simpleLineSymbol.Width = 2;

                        ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                        simpleFillSymbol.Color = rgbColor;
                        simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
                        simpleFillSymbol.Outline = simpleLineSymbol;

                        IFillShapeElement fillShapeElement = new PolygonElementClass();
                        fillShapeElement.Symbol = simpleFillSymbol;
                        element = (IElement)fillShapeElement;
                    }
                    break;
            }

            if (element != null)
            {
                element.Geometry = geometry;
            }

            return element;
        }

        #endregion

        #region Public Commands


        public string StatusText { get; set; }

        private void SetSelectionInternal(List<GeometrySelection> selections)
        {
            StatusText = "Analysing selected feature geometry...";

            foreach (var selection in selections)
            {
                selection.Geometry = EsriFromBytes(selection.Data);
            }

            var map = MapForm.axMapControl1.Map;
            var activeView = map as IActiveView;
            if (activeView == null) return;
            var graphicsContainer = (IGraphicsContainer)map;

            try
            {
                graphicsContainer.DeleteAllElements();
            }
            catch (Exception)
            {
            }

            var createdCalout = 0;
            var groupElement = new GroupElementClass();
            var groupEnvelop = new EnvelopeClass();
            string lastName = null;
            foreach (var selection in selections)
            {
                IElement element = CreateElement((IGeometry)selection.Geometry, SelectionFillColor, SelectionStrokeColor);
                if (element == null) continue;
                groupElement.AddElement(element);
                if (!element.Geometry.Envelope.IsEmpty)
                {
                    groupEnvelop.Union(element.Geometry.Envelope);
                }

                if (!string.IsNullOrWhiteSpace(selection.Name))
                {
                    lastName = selection.Name;
                    var anchorPoint = GetAnchor((IGeometry)selection.Geometry);
                    if (anchorPoint == null) continue;
                    var callout = CreateCallout(selection.Name, anchorPoint);
                    groupElement.AddElement(callout);
                    createdCalout++;
                }
            }
            graphicsContainer.AddElement(groupElement, 0);

            if (string.IsNullOrWhiteSpace(lastName))
            {
                lastName = "Selected Feature";
            }

            if (createdCalout > 0)
            {
                StatusText = createdCalout + " geometry components of " + lastName + " are presented on map, zooming map...";
                ZoomToInterest(groupEnvelop);
                StatusText = createdCalout + " geometry components of " + lastName + " are presented on map";
            }
            else
            {
                StatusText = lastName + " does not have any geometry components";
            }
        }

        #endregion
      
       
        #region Utils
        //private void ClearMap()
        //{
        //    var map = MapForm.axMapControl1.Map;
        //    var graphicsContainer = (IGraphicsContainer) map;

        //    try
        //    {
        //        graphicsContainer.DeleteAllElements();
        //    }
        //    catch
        //    {

        //    }

        //    var activeView = map as IActiveView;
        //    if (activeView != null) activeView.Refresh();
        //}
        #endregion

        #region Zoom

        private bool _isZoomToInterest=true;
        public bool IsZoomToInterest
        {
            get { return _isZoomToInterest; }
            set { _isZoomToInterest = value; }
        }



        private void ZoomToInterest(IEnvelope envelope)
        {
            var map = MapForm.axMapControl1.Map;
            var activeView = map as IActiveView;
            if (activeView == null) return;
            _mapAnimator.SetZoom(envelope);
        }


        #endregion

        #region Colors

        private static readonly IRgbColor GeometryFillColor = EsriUtil.CreateRgbColor(10, 255, 100, 100);
        private static readonly IRgbColor GeometryStrokeColor = EsriUtil.CreateRgbColor(10, 100, 0, 0);


        private static readonly IRgbColor SelectionFillColor = EsriUtil.CreateRgbColor(10, 100, 100, 255);
        private static readonly IRgbColor SelectionStrokeColor = EsriUtil.CreateRgbColor(10, 0, 0, 100);


        private static readonly IRgbColor IntersectionBaseFillColor = EsriUtil.CreateRgbColor(10, 100, 255, 100);
        private static readonly IRgbColor IntersectionBaseStrokeColor = EsriUtil.CreateRgbColor(10, 0, 100, 0);


        private static readonly IRgbColor IntersectorFillColor = EsriUtil.CreateRgbColor(10, 100, 100, 255);
        private static readonly IRgbColor IntersectorStrokeColor = EsriUtil.CreateRgbColor(10, 0, 0, 100);

        #endregion

        #region Zoom animation
        private ZoomAnimator _mapAnimator;

        public IDynamicMap DynamicMap
        {
            get
            {
                return MapForm == null ? null : MapForm.axMapControl1.Map as IDynamicMap;
            }
        }
        #endregion
    }

    public interface IMapProvider
    {
        IDynamicMap DynamicMap { get; }
    }

    public sealed class ZoomAnimator 
    {
        #region class members

        public IMapProvider MapProvider;

        private bool _isAnimating;
        private double _stepCount;
        private int _totalSteps;
        private readonly IPoint _mCenter = new PointClass();

        private WKSEnvelope _mWksStep;
        private IDynamicMapEvents_Event _mDynamicMapEvents;
        private const double CdMinimumDelta = 0.01;
        private const double CdSmoothFactor = 100000.0;
        private const double CdMinimumSmoothZoom = 0.01;

        #endregion

        #region Overriden Class Methods

        private IDynamicMapEvents_DynamicMapStartedEventHandler Started { get; set; }

        private IDynamicMapEvents_DynamicMapFinishedEventHandler Ended { get; set; }

        public ZoomAnimator()
        {
            Started = StartFrame;
            Ended = EndFrame;
        }

        public void StartAnimation()
        {
            _isAnimating = true;
            _stepCount = 0;
            _totalSteps = 0;

            //allow DynamicMap
            if (!MapProvider.DynamicMap.DynamicMapEnabled)
            {
                MapProvider.DynamicMap.DynamicMapEnabled = true;
                //prepare delegate
                _mDynamicMapEvents = null;
                _mDynamicMapEvents = MapProvider.DynamicMap as IDynamicMapEvents_Event;
                if (_mDynamicMapEvents != null)
                {
                    _mDynamicMapEvents.DynamicMapStarted += Started;
                    _mDynamicMapEvents.DynamicMapFinished += Ended;
                }
            }
        }

        public void SetZoom(IEnvelope zoomBounds)
        {
           IActiveView activeView = MapProvider.DynamicMap as IActiveView;
           if (null == zoomBounds || activeView == null)
                return;

            WKSEnvelope wksZoomBounds;
            zoomBounds.QueryWKSCoords(out wksZoomBounds);

            IEnvelope fittedBounds = activeView.ScreenDisplay.DisplayTransformation.VisibleBounds;
            WKSEnvelope wksFittedBounds;
            fittedBounds.QueryWKSCoords(out wksFittedBounds);

            _mCenter.X = (zoomBounds.XMin + zoomBounds.XMax) / 2;
            _mCenter.Y = (zoomBounds.YMin + zoomBounds.YMax) / 2;
        

            var intersect = fittedBounds.Envelope;
            intersect.Intersect(zoomBounds);
            if (!intersect.IsEmpty)
            {
                if (intersect.Width + 0.01 > zoomBounds.Width && intersect.Height + 0.01 > zoomBounds.Height)
                {
                    //it is visible
                    (activeView).Refresh();
                    return;
                }
            }
            

            StartAnimation();

            fittedBounds.CenterAt(_mCenter);

            {
                var w = wksZoomBounds.XMax - wksZoomBounds.XMin;
                var h = wksZoomBounds.YMax - wksZoomBounds.YMin;
                const double f = 0.10;

          
                if (w < 0.01 && h < 0.01)
                {
                    wksZoomBounds.XMin = Math.Min(wksZoomBounds.XMin, fittedBounds.XMin);
                    wksZoomBounds.YMin = Math.Min(wksZoomBounds.YMin, fittedBounds.YMin);
                    wksZoomBounds.YMax = Math.Max(wksZoomBounds.YMax, fittedBounds.YMax);
                    wksZoomBounds.XMax = Math.Max(wksZoomBounds.XMax, fittedBounds.XMax);
                }
          

                wksZoomBounds.XMin = wksZoomBounds.XMin - w * f;
                wksZoomBounds.YMin = wksZoomBounds.YMin - h * f;
                wksZoomBounds.XMax = wksZoomBounds.XMax + w * f;
                wksZoomBounds.YMax = wksZoomBounds.YMax + h * f;

            }

            _mWksStep.XMax = 1;
            _mWksStep.YMax = 1;
            _mWksStep.XMin = 1;
            _mWksStep.YMin = 1;
            _totalSteps = 0;

            // Calculate how fast the zoom will go by changing the step size
            while ((Math.Abs(_mWksStep.XMax) > CdMinimumDelta) ||
                   (Math.Abs(_mWksStep.YMax) > CdMinimumDelta) ||
                   (Math.Abs(_mWksStep.XMin) > CdMinimumDelta) ||
                   (Math.Abs(_mWksStep.YMin) > CdMinimumDelta))
            {
                _totalSteps++;

                // calculate the step size
                // step size is the difference between the zoom bounds and the fitted bounds
                _mWksStep.XMin = (wksZoomBounds.XMin - wksFittedBounds.XMin)/_totalSteps;
                _mWksStep.YMin = (wksZoomBounds.YMin - wksFittedBounds.YMin)/_totalSteps;
                _mWksStep.XMax = (wksZoomBounds.XMax - wksFittedBounds.XMax)/_totalSteps;
                _mWksStep.YMax = (wksZoomBounds.YMax - wksFittedBounds.YMax)/_totalSteps;
            }

            _stepCount = 0;
        }

        #endregion

        #region Dynamic Map Events

        private void EndFrame(IDisplay d, IDynamicDisplay dd)
        {
            if (MapProvider.DynamicMap.DynamicMapEnabled)
            {
                if (!_isAnimating)
                {
                    //prepare delegate
                    _mDynamicMapEvents = null;
                    _mDynamicMapEvents = MapProvider.DynamicMap as IDynamicMapEvents_Event;
                    if (_mDynamicMapEvents != null)
                    {
                        _mDynamicMapEvents.DynamicMapStarted -= Started;
                        _mDynamicMapEvents.DynamicMapFinished -= Ended;
                    }
                    MapProvider.DynamicMap.DynamicMapEnabled = false;
                }
            }
        }

        private void StartFrame(IDisplay d, IDynamicDisplay dd)
        {
            if (!_isAnimating)
            {
                _stepCount = 0;
                _totalSteps = 0;
                return;
            }

            if (_stepCount >= _totalSteps)
            {
                _stepCount = 0;
                _totalSteps = 0;
                _isAnimating = false;
                return;
            }

            IDynamicMap dynamicMap = MapProvider.DynamicMap;
            if (!dynamicMap.DynamicMapEnabled)
            {
                return;
            }

            // Increase the bounds by the step amount
            IActiveView activeView = MapProvider.DynamicMap as IActiveView;
            if (activeView == null) return;
            IEnvelope newVisibleBounds = activeView.ScreenDisplay.DisplayTransformation.FittedBounds;

            // Smooth the zooming.  Faster at higher scales, slower at lower
            double dSmoothZooom = activeView.FocusMap.MapScale/CdSmoothFactor;
            if (dSmoothZooom < CdMinimumSmoothZoom)
                dSmoothZooom = CdMinimumSmoothZoom;

            newVisibleBounds.XMin = newVisibleBounds.XMin + (_mWksStep.XMin*dSmoothZooom);
            newVisibleBounds.YMin = newVisibleBounds.YMin + (_mWksStep.YMin*dSmoothZooom);
            newVisibleBounds.XMax = newVisibleBounds.XMax + (_mWksStep.XMax*dSmoothZooom);
            newVisibleBounds.YMax = newVisibleBounds.YMax + (_mWksStep.YMax*dSmoothZooom);

            activeView.ScreenDisplay.DisplayTransformation.VisibleBounds = newVisibleBounds;

            _stepCount = _stepCount + dSmoothZooom;

            if (_stepCount >= _totalSteps)
            {
                _stepCount = 0;
                _totalSteps = 0;
                _isAnimating = false;
            }
        }

        #endregion





    }
}
