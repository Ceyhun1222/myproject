using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.Delta.Model;
using Aran.Delta.View;
using Aran.Geometries;
using Aran.Queries;
using AranSupport;
using PDM;
using Airspace = Aran.Aim.Features.Airspace;
using AirspaceType = PDM.AirspaceType;
using AirspaceVolume = PDM.AirspaceVolume;
using RouteSegment = Aran.Aim.Features.RouteSegment;

namespace Aran.Delta.ViewModels
{
    public class RouteBufferViewModel : ViewModel
    {
        private BufferModel _selectedBufferModel;
        private int _drawBufferHandle;
        
        #region :>Ctor

        public RouteBufferViewModel()
        {
            try
            {
                RouteList = GlobalParams.Database.RouteList;
                RouteSegmentList = new List<BufferSegmentModel>();
                SelectedRoute = RouteList[0];
                RNPValue = 5;
                BufferList = new ObservableCollection<BufferModel>();
                SegmentsTypeList = new List<Segment>
                {
                    new Segment {Name = "Departure", BufferValue = 930},
                    new Segment {Name = "Enroute and Arrival", BufferValue = 1850},
                    new Segment {Name = "Arrival/initial/intermediate approach", BufferValue = 930},
                    new Segment {Name = "Final", BufferValue = 370},
                    new Segment {Name = "Missed Approach", BufferValue = 560}
                };
                SelectedSegmentType = SegmentsTypeList[0];

                DrawBufferCommand = new RelayCommand(new Action<object>(drawBuffer_onClick));
                AddBufferListCommand = new RelayCommand(new Action<object>(addToBufferList_onClick));
                CloseCommand = new RelayCommand(new Action<object>(close_onClick));
                RemoveCommand = new RelayCommand(new Action<object>(remove_onClick));

                if (GlobalParams.AranEnv != null)
                    SaveBufferCommand = new RelayCommand(new Action<object>(saveBuffer_onClick));
                else
                {
                    SaveBufferCommand = new RelayCommand(new Action<object>(saveToArena));

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        internal void Dispose()
        {
            RouteList = null;
            RouteSegmentList = null;
            BufferList = null;
            //(this as IDisposable).Dispose();
        }

        private void remove_onClick(object obj)
        {
            if (BufferList != null)
            {
                var a = BufferList.Where(buf => buf.IsSelected).ToList();

                foreach (var buffer in a)
                {
                    buffer.Clear();
                    BufferList.Remove(buffer);
                }
            }
        }

        private void close_onClick(object obj)
        {
            Close();
        }

        #endregion

        #region :>Property

        public List<Route> RouteList { get; set; }
        public List<Model.BufferSegmentModel> RouteSegmentList { get; set; }
        public List<Segment> SegmentsTypeList { get; set; }
        public ObservableCollection<BufferModel> BufferList { get; set; }

        public RelayCommand DrawBufferCommand { get; set; }
        public RelayCommand AddBufferListCommand { get; set; }
        public RelayCommand SaveBufferCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        private Route _selectedRoute;
        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            set
            {
                _selectedRoute = value;
                ClearRouteSegments();
                if (value != null)
                {
                    RouteSegmentList.Clear();

                    var segmentList = GlobalParams.Database.GetRouteSegmentList(_selectedRoute.Identifier);
                    foreach (var segment in segmentList)
                        RouteSegmentList.Add(new BufferSegmentModel(this, segment));

                    if (RouteSegmentList.Count == 0) 
                    {
                        if (GlobalParams.DesigningAreaReader != null) 
                        {
                           List<DesigningSegment> designingSegmentList = GlobalParams.DesigningAreaReader.GetDesigningSegments(_selectedRoute.Name);
                           if (designingSegmentList != null)
                           {
                               foreach (var segment in designingSegmentList)
                                   RouteSegmentList.Add(new BufferSegmentModel(this, segment));
                           }
                        }
                    }

                    SortAndFillRouteSegments();
                    
                }
                else
                    RouteSegmentList.Clear();

                NotifyPropertyChanged("SelectedRoute");
                NotifyPropertyChanged("RouteSegmentList");
                NotifyPropertyChanged("DrawBufferIsEnabled");
            }
        }

        private RouteSegment _routeSegment;
        public RouteSegment SelectedRouteSegment
        {
            get { return _routeSegment; }
            set
            {
                _routeSegment = value;
                DrawRouteSegments();
                NotifyPropertyChanged("SelectedRouteSegment");
            }
        }

        private Segment _selectedSegmentType;

        public Segment SelectedSegmentType
        {
            get { return _selectedSegmentType; }
            set
            {
                _selectedSegmentType = value;
                BufferValue = Common.ConvertDistance(_selectedSegmentType.BufferValue, RoundType.RealValue);
                NotifyPropertyChanged("SelectedSegmentType");
            }
        }

        private bool _isDrawBuffer;
        public bool IsDrawBuffer
        {
            get { return _isDrawBuffer; }
            set
            {
                _isDrawBuffer = value;
                foreach (var buffer in BufferList)
                {
                    if (_isDrawBuffer)
                        buffer.Draw();
                    else
                        buffer.Clear();
                }
            }
        }

        private bool _isDrawSegments;
        public bool IsDrawSegments
        {
            get { return _isDrawSegments; }
            set
            {
                _isDrawSegments = value;
                if (_isDrawSegments)
                    DrawRouteSegments();
                else
                    ClearRouteSegments();
            }
        }

        public bool DrawBufferIsEnabled
        {
            get
            {
                if (RouteSegmentList == null && RouteSegmentList.Count == 0)
                    return false;
                foreach (var routeSegment in RouteSegmentList)
                {
                    if (!routeSegment.IsValid)
                    {
                        Messages.Warning("Route is not correct!");
                        return false;
                    }
                }
                return true;
            }
        }

        public string DistanceUnit { get { return InitDelta.DistanceConverter.Unit; } }

        private int _rnpValue;
        public int RNPValue
        {
            get { return _rnpValue; }
            set 
            {
                _rnpValue = value;
                CalculateBufferWidth();
                NotifyPropertyChanged("RnpValue");
            }
        }

        private double _bufferValue;
        public double BufferValue
        {
            get { return Common.ConvertDistance(_bufferValue,RoundType.ToNearest); }
            set 
            {
                _bufferValue = Common.DeConvertDistance(value);
                CalculateBufferWidth();
                NotifyPropertyChanged("BufferValue");
            }
        }

        private double _bufferWidth;
        public double BufferWidth
        {
            get { return Common.ConvertDistance(_bufferWidth,RoundType.ToNearest); }
            set 
            {
                _bufferWidth = Common.DeConvertDistance(value);
                NotifyPropertyChanged("BufferWidth");
            }
        }

        private string _bufferName;
        public string BufferName
        {
            get { return _bufferName; }
            set
            {
                _bufferName = value;
                if (_selectedBufferModel!=null)
                    _selectedBufferModel.Name = _bufferName;
                NotifyPropertyChanged("BufferName");
            }
        }

        private bool _addBufferListIsEnabled;
        public bool AddBufferListIsEnabled
        {
            get { return _addBufferListIsEnabled; }
            set 
            {
                _addBufferListIsEnabled = value;
                NotifyPropertyChanged("AddBufferListIsEnabled");
            }
        }

        public string EquationText { get { return "2 * XTT + BV"; } }

        private bool _saveBufferIsEnabled;
        public bool SaveBufferIsEnabled
        {
            get { return _saveBufferIsEnabled; }
            set 
            {
                _saveBufferIsEnabled = value;
                NotifyPropertyChanged("SaveBufferIsEnabled");
            }
        }
        
        private void CalculateBufferWidth()
        {
            _bufferWidth = 2 * 1852 * RNPValue + _bufferValue;
            NotifyPropertyChanged("BufferWidth");
        }

#endregion

        #region :>Methods

        private void addToBufferList_onClick(object obj)
        {
            if (_selectedBufferModel != null)
            {
                foreach (var buffer in BufferList)
                {
                    if (buffer.Name == _selectedBufferModel.Name)
                    {
                        Messages.Warning("Name is already exist!");
                        return;
                    }
                }
                _selectedBufferModel.IsSelected = true;
                BufferList.Add(_selectedBufferModel);
                _selectedBufferModel = new BufferModel(this);
                BufferName = "";
                AddBufferListIsEnabled = false;
                GlobalParams.UI.SafeDeleteGraphic(_drawBufferHandle);
                SaveBufferIsEnabled = true;
            }
        }

        private void drawBuffer_onClick(object obj)
        {
            try
            {
                if (SelectedRoute != null)
                {
                    if (RouteSegmentList.Count > 0)
                    {
                        var routeGeo = RouteSegmentList[0].Geo;
                        int segmentIndex1 = RouteSegmentList[0].Index;
                        for (int i = 1; i < RouteSegmentList.Count; i++)
                        {
                            if (RouteSegmentList[i].IsSelected)
                            {
                                var segmentIndex2 = RouteSegmentList[i].Index;
                                if ((segmentIndex2 - segmentIndex1) > 1)
                                {
                                    Messages.Warning("You can select only adjoining segments");
                                    return;
                                }
                                routeGeo =
                                    GlobalParams.GeometryOperators.UnionGeometry(routeGeo, RouteSegmentList[i].Geo) as
                                        Aran.Geometries.MultiLineString;
                                segmentIndex1 = segmentIndex2;
                            }

                        }
                        _selectedBufferModel = new BufferModel(this);

                        if (routeGeo != null)
                        {
                            _selectedBufferModel.FeatureGeo = routeGeo;
                            _selectedBufferModel.SelectedFeature = SelectedRoute;
                            _selectedBufferModel.Width = BufferWidth;
                            _selectedBufferModel.MarkerLayer = SelectedRoute.GetLayerName();
                            _selectedBufferModel.MarkerObjectName = SelectedRoute.Name;
                            _selectedBufferModel.RouteSegmentList = new List<BufferSegmentModel>(RouteSegmentList);

                            var buffer = GlobalParams.GeometryOperators.Buffer(routeGeo, _bufferWidth);
                            if (buffer.Type == Geometries.GeometryType.Polygon)
                                _selectedBufferModel.BufferGeom = new Aran.Geometries.MultiPolygon
                                {
                                    buffer as Aran.Geometries.Polygon
                                };
                            else
                                _selectedBufferModel.BufferGeom = buffer as Aran.Geometries.MultiPolygon;

                            GlobalParams.UI.SafeDeleteGraphic(_drawBufferHandle);
                            _drawBufferHandle = GlobalParams.UI.DrawMultiPolygon(_selectedBufferModel.BufferGeom, 100,
                                AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);
                        }

                        _selectedBufferModel.Name = "Route_" + _selectedRoute.Name + "_" + BufferWidth + DistanceUnit;
                        BufferName = _selectedBufferModel.Name;
                        AddBufferListIsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Messages.Error(ex.Message);
            }


        }

        public void DrawRouteSegments()
        {
            ClearRouteSegments();
            if (RouteSegmentList != null)
            {
                foreach (var routeSegment in RouteSegmentList)
                {
                    if (routeSegment.IsSelected)
                        routeSegment.Draw();
                }
            }
        }

        public void DrawBuffers()
        {
            ClearBuffers();
            if (BufferList != null) 
            {
                foreach (var buffer in BufferList)
                    buffer.Draw();
            }
        
        }

        public void Clear() 
        {
            ClearRouteSegments();
            ClearBuffers();
        }

        private void ClearRouteSegments()
        {
            GlobalParams.UI.SafeDeleteGraphic(_drawBufferHandle);
            if (RouteSegmentList != null)
            {
                foreach (var routeSegment in RouteSegmentList)
                    routeSegment.Clear();
            }
        }

        private void ClearBuffers() 
        {
            if (BufferList != null)
            {
                foreach (var buffer in BufferList)
                    buffer.Clear();
            }
        }

        internal void SelectAllSegments()
        {
            if (RouteSegmentList != null)
            {
                foreach (var routeSegment in RouteSegmentList)
                    routeSegment.IsSelected = true;
            }
        }

        internal void UnselectAllSegments()
        {
            if (RouteSegmentList != null)
            {
                foreach (var routeSegment in RouteSegmentList)
                    routeSegment.IsSelected = false;
            }
        }

        public void SortAndFillRouteSegments() 
        {
            List<BufferSegmentModel> segmentModel = new List<BufferSegmentModel>(RouteSegmentList.Count);

            for (int i = 0; i < RouteSegmentList.Count-1; i++)
            {
                bool isStart = true;
                for (int j = 0; j < RouteSegmentList.Count; j++)
                {
                    if (RouteSegmentList[i].StartPoint == RouteSegmentList[j].EndPoint)
                    {
                        isStart = false;
                        break;
                    }
                }
                if (isStart)
                {
                    RouteSegmentList[i].Index = 1;
                    AssignIndex(RouteSegmentList[i]);
                    break;
                }
            }
            
            RouteSegmentList = RouteSegmentList.OrderBy(routeSegment => routeSegment.Index).ToList<BufferSegmentModel>();
            NotifyPropertyChanged("RouteSegmentList");
        }

        private void AssignIndex(BufferSegmentModel routeSegment)
        {
            if (routeSegment.Index == RouteSegmentList.Count)
                return;
            foreach (var tmpRouteSegment in RouteSegmentList)
            {
                if (routeSegment.EndPoint == tmpRouteSegment.StartPoint)
                {
                    tmpRouteSegment.Index = routeSegment.Index + 1;
                    AssignIndex(tmpRouteSegment);
                    break;
                }
            }
        }

        public override void Close()
        {
            Clear();
            base.Close();
        }

        private void saveBuffer_onClick(object obj)
        {
            foreach (var buffer in BufferList)
            {
                if (buffer.IsSelected)
                {
                    var questionResult = Messages.Question("Do you want to save " + buffer.Name);
                    if (questionResult != System.Windows.MessageBoxResult.Yes)
                        continue;

                    Aim.DataTypes.ValDistanceVertical lowerDistanceVertical = buffer.RouteSegmentList[0].RouteSegment.LowerLimit;
                    Aim.DataTypes.ValDistanceVertical upperDistanceVertical = buffer.RouteSegmentList[0].RouteSegment.UpperLimit;

                    double lowerLimit = Converters.ConverterToSI.Convert(lowerDistanceVertical, 0);
                    double upperLimit = Converters.ConverterToSI.Convert(upperDistanceVertical, 0);

                    bool altitudeDifferences = false;
                    for (int i = 1; i < buffer.RouteSegmentList.Count; i++)
                    {
                        var routeSegment = buffer.RouteSegmentList[i].RouteSegment;
                        double tmpUpperLimit = Converters.ConverterToSI.Convert(routeSegment.UpperLimit, 0);
                        if (tmpUpperLimit - upperLimit > 0.1)
                        {
                            upperLimit = tmpUpperLimit;
                            upperDistanceVertical = routeSegment.UpperLimit;
                            altitudeDifferences = true;
                        }
                        double tmpLowerLimit = Converters.ConverterToSI.Convert(routeSegment.LowerLimit, 0);
                        if (lowerLimit - tmpLowerLimit > 0.1)
                        {
                            lowerLimit = tmpLowerLimit;
                            lowerDistanceVertical = routeSegment.LowerLimit;
                            altitudeDifferences = true;
                        }
                    }

                    if (altitudeDifferences)
                    {
                        if (Messages.WarningWithResult("Route Segments lower and upper limit is not equal!Do you want to continue") == System.Windows.MessageBoxResult.No)
                            continue;
                    }

                    var airCorridor = GlobalParams.Database.DeltaQPI.CreateFeature<Airspace>();

                    var note = new Note();
                    note.Purpose = Aim.Enums.CodeNotePurpose.REMARK;
                    var linguisticNote = new LinguisticNote();
                    linguisticNote.Note = new Aim.DataTypes.TextNote();
                    var noteText = "Has created by Delta!";
                    linguisticNote.Note.Value = noteText;
                    note.TranslatedNote.Add(linguisticNote);
                    airCorridor.Annotation.Add(note);

                    var airspaceComponent = new Aim.Features.AirspaceGeometryComponent();
                    airspaceComponent.TheAirspaceVolume = new Aim.Features.AirspaceVolume();
                    airspaceComponent.TheAirspaceVolume.HorizontalProjection = new Aim.Features.Surface();
                    airspaceComponent.TheAirspaceVolume.Width = new Aim.DataTypes.ValDistance();
                    airspaceComponent.TheAirspaceVolume.Width.Value = Common.ConvertDistance(buffer.Width, RoundType.ToNearest);
                    if (InitDelta.DistanceConverter.Unit == "km")
                        airspaceComponent.TheAirspaceVolume.Width.Uom = Aim.Enums.UomDistance.KM;
                    else
                        airspaceComponent.TheAirspaceVolume.Width.Uom = Aim.Enums.UomDistance.NM;

                    airspaceComponent.TheAirspaceVolume.LowerLimit = lowerDistanceVertical;
                    airspaceComponent.TheAirspaceVolume.LowerLimitReference = Aim.Enums.CodeVerticalReference.MSL;
                    airspaceComponent.TheAirspaceVolume.UpperLimit = upperDistanceVertical;
                    airspaceComponent.TheAirspaceVolume.UpperLimitReference = Aim.Enums.CodeVerticalReference.MSL;


                    var centerLineGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.MultiLineString>((MultiLineString)buffer.FeatureGeo);
                    airspaceComponent.TheAirspaceVolume.Centreline = new Curve();
                    foreach (Aran.Geometries.LineString routeSegment in centerLineGeo)
                        airspaceComponent.TheAirspaceVolume.Centreline.Geo.Add(routeSegment);

                    var airsapceGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.MultiPolygon>(buffer.BufferGeom);
                    foreach (Aran.Geometries.Polygon geom in airsapceGeo)
                        airspaceComponent.TheAirspaceVolume.HorizontalProjection.Geo.Add(geom);

                    airCorridor.GeometryComponent.Add(airspaceComponent);
                    airCorridor.Name = buffer.Name;
                    airCorridor.Type = Aim.Enums.CodeAirspace.AWY;
                    airCorridor.ProtectedRoute = buffer.SelectedFeature.GetFeatureRef();

                    //GlobalParams.Database.DeltaQPI.SetFeature(airCorridor);
                    GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.Airspace);
                    var dbProvider = GlobalParams.AranEnv.DbProvider as DbProvider;

                    if (dbProvider != null)
                    {
                        GlobalParams.Database.DeltaQPI.Commit(dbProvider.ProviderType != DbProviderType.TDB);
                        GlobalParams.Database.DeltaQPI.ExcludeFeature(airCorridor.Identifier);
                    }
                }
            }
        }

       private void saveToAre(object obj)
        {
            foreach (var buffer in BufferList)
            {
                if (buffer.IsSelected)
                {
                    var questionResult = Messages.Question("Do you want to save " + buffer.Name);
                    if (questionResult != System.Windows.MessageBoxResult.Yes)
                        continue;

                    Aim.DataTypes.ValDistanceVertical lowerDistanceVertical = buffer.RouteSegmentList[0].RouteSegment.LowerLimit;
                    Aim.DataTypes.ValDistanceVertical upperDistanceVertical = buffer.RouteSegmentList[0].RouteSegment.UpperLimit;

                    double lowerLimit = Converters.ConverterToSI.Convert(lowerDistanceVertical, 0);
                    double upperLimit = Converters.ConverterToSI.Convert(upperDistanceVertical, 0);

                    bool altitudeDifferences = false;
                    for (int i = 1; i < buffer.RouteSegmentList.Count; i++)
                    {
                        var routeSegment = buffer.RouteSegmentList[i].RouteSegment;
                        double tmpUpperLimit = Converters.ConverterToSI.Convert(routeSegment.UpperLimit, 0);
                        if (tmpUpperLimit - upperLimit > 0.1)
                        {
                            upperLimit = tmpUpperLimit;
                            upperDistanceVertical = routeSegment.UpperLimit;
                            altitudeDifferences = true;
                        }
                        double tmpLowerLimit = Converters.ConverterToSI.Convert(routeSegment.LowerLimit, 0);
                        if (lowerLimit - tmpLowerLimit > 0.1)
                        {
                            lowerLimit = tmpLowerLimit;
                            lowerDistanceVertical = routeSegment.LowerLimit;
                            altitudeDifferences = true;
                        }
                    }

                    if (altitudeDifferences)
                    {
                        if (Messages.WarningWithResult("Route Segments lower and upper limit is not equal!Do you want to continue") == System.Windows.MessageBoxResult.No)
                            continue;
                    }

                    var pdmAirspaceCorridor = new PDM.Airspace();
                    var airspaceVolume = new PDM.AirspaceVolume();


                    if (InitDelta.HeightConverter.Unit == "ft")
                        airspaceVolume.UomValDistVerLower = airspaceVolume.UomValDistVerUpper = UOM_DIST_VERT.FT;
                    else
                        airspaceVolume.UomValDistVerLower = airspaceVolume.UomValDistVerUpper = UOM_DIST_VERT.M;

                    airspaceVolume.ValDistVerLower = lowerDistanceVertical.Value;
                    airspaceVolume.ValDistVerUpper = upperDistanceVertical.Value;

                    var centerLineGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.MultiLineString>((MultiLineString)buffer.FeatureGeo);
                    //airspaceVolume.CenterLine = Aran.Converters.ConvertToEsriGeom.FromMultiLineStringZ(centerLineGeo);

                    var airsapceGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.MultiPolygon>(buffer.BufferGeom);
                    airspaceVolume.Geo = Aran.Converters.ConvertToEsriGeom.FromGeometry(airsapceGeo, true);
                    airspaceVolume.BrdrGeometry = HelperClass.SetObjectToBlob(airspaceVolume.Geo, "Border");
                    airspaceVolume.CodeId = buffer.Name;

                    var pdmAirsapce = new PDM.Airspace
                    {
                        AirspaceVolumeList = new List<AirspaceVolume> { airspaceVolume },
                        TxtName = buffer.Name,
                        CodeType = AirspaceType.AWY
                    };

                   //dmAirspaceCorridor.ProtectedRoute = buffer.SelectedFeature

                   

                    //GlobalParams.Database.DeltaQPI.SetFeature(pdmAirspaceCorridor);

                    var window = new SavePoint(pdmAirsapce);

                    var helper = new WindowInteropHelper(window);
                    helper.Owner = new IntPtr(GlobalParams.HWND);
                    ElementHost.EnableModelessKeyboardInterop(window);
                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                    window.ShowDialog();

                    Functions.SaveArenaProject();
                }
            }
        }

        private void saveToArena(object obj)
        {
            try
            {
                bool isSaved = false;
                var savedBufferList = new List<BufferModel>();
                foreach (var buffer in BufferList)
                {
                    if (buffer.IsSelected)
                    {
                        var questionResult =
                            Messages.Question("Do you want to save " + buffer.Name);
                        if (questionResult != System.Windows.MessageBoxResult.Yes)
                            continue;

                        if (buffer.BufferGeom != null && !buffer.BufferGeom.IsEmpty)
                        {
                            var designBuffer = new Model.DesigningBuffer();
                            designBuffer.MarkerLayer = buffer.MarkerLayer;
                            designBuffer.MarkerObjectName = buffer.MarkerObjectName;
                            designBuffer.Name = buffer.Name;
                            designBuffer.Geo = GlobalParams.SpatialRefOperation.ToGeo(buffer.BufferGeom);
                            designBuffer.Code_Type = "AWY";

                            if (GlobalParams.DesigningAreaReader.SaveBuffer(designBuffer))
                            {
                                isSaved = true;
                                savedBufferList.Add(buffer);
                            }
                        }
                    }

                }

                savedBufferList.ForEach(buf => BufferList.Remove(buf));

                if (isSaved)
                {
                    Aran.Delta.Model.Messages.Info("Feature saved database successfully");
                    Functions.SaveArenaProject();
                    Clear();
                }
            }

            catch (Exception e)
            {
                Model.Messages.Error("Error appered when trying save feature to DB!" + e.Message);
            }
        }

        #endregion
    }
}
