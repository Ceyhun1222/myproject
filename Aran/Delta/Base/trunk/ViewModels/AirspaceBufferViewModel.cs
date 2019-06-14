using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.Delta.Model;
using Aran.Delta.View;
using Aran.Geometries;
using Aran.Queries;
using PDM;
using Airspace = Aran.Aim.Features.Airspace;
using AirspaceType = PDM.AirspaceType;
using AirspaceVolume = PDM.AirspaceVolume;
using RouteSegment = Aran.Aim.Features.RouteSegment;
using AranSupport;

namespace Aran.Delta.ViewModels
{
    public class AirspaceBufferViewModel : ViewModel
    {
        private BufferModel _selectedBufferModel;
        private int _drawBufferHandle;
        
        #region :>Ctor
        public AirspaceBufferViewModel()
        {
            try
            {
                AirspaceList = GlobalParams.Database.GetAirspaceList;
                if (AirspaceList!=null && AirspaceList.Count>0)
                    SelectedAirspace = AirspaceList[0];
                
                BufferList = new ObservableCollection<BufferModel>();

                DrawBufferCommand = new RelayCommand(new Action<object>(drawBuffer_onClick));
                AddBufferListCommand = new RelayCommand(new Action<object>(addToBufferList_onClick));

                if (GlobalParams.AranEnv != null)
                    SaveBufferCommand = new RelayCommand(new Action<object>(saveBuffer_onClick));
                else
                {
                    SaveBufferCommand = new RelayCommand(new Action<object>(saveToArena));

                }

                CancelCommand = new RelayCommand(new Action<object>(cancel_onClick));
                RemoveCommand = new RelayCommand(new Action<object>(remove_onClick));

                BufferWidth = 5; //5 NM

                _spatialRefOperation = GlobalParams.SpatialRefOperation;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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

        private void cancel_onClick(object obj)
        {
            Close();
        }

        #endregion

        #region :>Property

        public List<Airspace> AirspaceList { get; set; }
        public ObservableCollection<BufferModel> BufferList { get; set; }

        public RelayCommand DrawBufferCommand { get; set; }
        public RelayCommand AddBufferListCommand { get; set; }
        public RelayCommand SaveBufferCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        private Airspace _selectedAirspace;
        public Airspace SelectedAirspace
        {
            get { return _selectedAirspace; }
            set
            {
                _selectedAirspace = value;
                if (value != null)
                {
                    DrawAirspace();
                }
                else
                    ClearAirspace();

                NotifyPropertyChanged("DrawBufferIsEnabled");
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

        public string DistanceUnit { get { return InitDelta.DistanceConverter.Unit; } }

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

        public bool InnerBufferIsChecked
        {
            get { return _innerBufferIsChecked; }
            set
            {
                _innerBufferIsChecked = value;

                if (_innerBufferIsChecked)
                    OuterBufferIsChecked = false;

                NotifyPropertyChanged("InnerBufferIsChecked");
            }
        }

        public bool OuterBufferIsChecked
        {
            get { return _outerBufferIsChecked; }
            set
            {
                _outerBufferIsChecked = value;

                if (_outerBufferIsChecked)
                    InnerBufferIsChecked = false;
                NotifyPropertyChanged("OuterBufferIsChecked");
            }
        }

        private bool _saveBufferIsEnabled;
        private int _airspaceHandle;
        private Geometries.MultiPolygon _selectedAirspaceGeo;
        private bool _innerBufferIsChecked;
        private bool _outerBufferIsChecked;
        private SpatialReferenceOperation _spatialRefOperation;

        public bool SaveBufferIsEnabled
        {
            get { return _saveBufferIsEnabled; }
            set 
            {
                _saveBufferIsEnabled = value;
                NotifyPropertyChanged("SaveBufferIsEnabled");
            }
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
            if (SelectedAirspace != null)
            {
                _selectedBufferModel = new BufferModel(this);
                if (_selectedAirspaceGeo != null && !_selectedAirspaceGeo.IsEmpty)
                {
                    _selectedBufferModel.FeatureGeo = _selectedAirspaceGeo;
                    _selectedBufferModel.SelectedFeature = SelectedAirspace;
                    _selectedBufferModel.Width = BufferWidth;
                    _selectedBufferModel.Name = _selectedAirspace.Name + "_" + BufferWidth + DistanceUnit;
                    _selectedBufferModel.MarkerLayer = SelectedAirspace.GetLayerName();
                    _selectedBufferModel.MarkerObjectName = SelectedAirspace.Designator;
                    BufferName = _selectedBufferModel.Name;

                    Aran.Geometries.Geometry buffer;

                    if (InnerBufferIsChecked)
                    {
                        var geo = GlobalParams.GeometryOperators.Buffer(_selectedAirspaceGeo, -_bufferWidth);
                        buffer = GlobalParams.GeometryOperators.Difference(_selectedAirspaceGeo, geo);
                    }
                    else
                    {
                        buffer = GlobalParams.GeometryOperators.Buffer(_selectedAirspaceGeo, _bufferWidth);
                    }

                    var resultGeo = buffer;

                    if (OuterBufferIsChecked)
                    {
                        resultGeo = GlobalParams.GeometryOperators.Difference(buffer, _selectedAirspaceGeo);
                    }

                    if (resultGeo.Type == Geometries.GeometryType.Polygon)
                        _selectedBufferModel.BufferGeom = new Aran.Geometries.MultiPolygon
                        {
                            resultGeo as Aran.Geometries.Polygon
                        };
                    else
                        _selectedBufferModel.BufferGeom = resultGeo as Aran.Geometries.MultiPolygon;

                    GlobalParams.UI.SafeDeleteGraphic(_drawBufferHandle);
                    _drawBufferHandle = GlobalParams.UI.DrawMultiPolygon(_selectedBufferModel.BufferGeom, 100,
                        AranEnvironment.Symbols.eFillStyle.sfsBackwardDiagonal);

                    AddBufferListIsEnabled = true;
                }
            }
        }

        private void DrawAirspace()
        {
            GlobalParams.UI.SafeDeleteGraphic(_airspaceHandle);
            
            _selectedAirspaceGeo = new Aran.Geometries.MultiPolygon();
            if (SelectedAirspace != null)
            {
                foreach (var airspaceGeometryComponent in SelectedAirspace.GeometryComponent)
                {
                    if (airspaceGeometryComponent.TheAirspaceVolume != null &&
                        airspaceGeometryComponent.TheAirspaceVolume.HorizontalProjection != null)
                    {
                        var prjGeo =
                            GlobalParams.SpatialRefOperation.ToPrj(
                                airspaceGeometryComponent.TheAirspaceVolume.HorizontalProjection.Geo);
                        if (prjGeo != null && !prjGeo.IsEmpty)
                        {
                            _selectedAirspaceGeo =
                                (Aran.Geometries.MultiPolygon)
                                    GlobalParams.GeometryOperators.UnionGeometry(_selectedAirspaceGeo, prjGeo);
                        }
                    }
                }
            }

            if (!_selectedAirspaceGeo.IsEmpty)
                _airspaceHandle = GlobalParams.UI.DrawDefaultMultiPolygon(_selectedAirspaceGeo);
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
            ClearAirspace();
            ClearBuffers();
        }

        private void ClearAirspace()
        {
            GlobalParams.UI.SafeDeleteGraphic(_drawBufferHandle);
            GlobalParams.UI.SafeDeleteGraphic(_airspaceHandle);
        }

        private void ClearBuffers() 
        {
            if (BufferList != null)
            {
                foreach (var buffer in BufferList)
                    buffer.Clear();
            }
        }

        public override void Close()
        {
            Clear();
            base.Close();
        }


        private void saveBuffer_onClick(object obj)
        {
            try
            {
                foreach (var buffer in BufferList)
                {
                    if (buffer.IsSelected && buffer.BufferGeom != null)
                    {
                        var questionResult =
                            Messages.Question("Do you want to save " + buffer.Name);
                        if (questionResult != System.Windows.MessageBoxResult.Yes)
                            continue;

                        var airspace = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.Airspace>();

                        var copyAirspace = CopyAirspace(buffer.SelectedFeature as Airspace);
                        copyAirspace.TimeSlice = airspace.TimeSlice;
                        copyAirspace.Identifier = airspace.Identifier;

                        airspace = copyAirspace;

                        GlobalParams.Database.DeltaQPI.ClearAllFeatures();

                        GlobalParams.Database.DeltaQPI.SetFeature(airspace);

                        var note = Functions.CreateNote();
                        airspace.Annotation.Add(note);

                        airspace.Name = buffer.Name.ToUpper();

                        var airspaceComponent = airspace.GeometryComponent[0];
                        airspace.GeometryComponent.Clear();
                        //var airspaceComponent = new Aim.Features.AirspaceGeometryComponent
                        //{
                        //    TheAirspaceVolume = new Aim.Features.AirspaceVolume{HorizontalProjection = new Aim.Features.Surface()}
                        //};

                        var airsapceGeo =_spatialRefOperation.ToGeo<MultiPolygon>(buffer.BufferGeom);

                        foreach (Aran.Geometries.Polygon geom in airsapceGeo)
                            airspaceComponent.TheAirspaceVolume.HorizontalProjection.Geo.Add(geom);

                        airspace.GeometryComponent.Add(airspaceComponent);

                        GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.Airspace);

                        var dbProvider = GlobalParams.AranEnv.DbProvider as DbProvider;
                        if (dbProvider != null && GlobalParams.Database.DeltaQPI.Commit(dbProvider.ProviderType != DbProviderType.TDB))
                            Model.Messages.Info("Airspace successfully saved to Aixm 5.1 DB");

                        GlobalParams.Database.DeltaQPI.ExcludeFeature(airspace.Identifier);
                        Clear();
                    }
                }
            }
            catch (Exception e)
            {
                GlobalParams.AranEnv.GetLogger("DELTA").Error(e.Message);
                Messages.Error("Error saving Airspace to AIXM 5.1 DB : ");
            }
        }


        //private void saveToArena(object obj)
        //{
        //    try
        //    {
        //        foreach (var buffer in BufferList)
        //        {
        //            if (buffer.IsSelected)
        //            {
        //                var questionResult =
        //                    Messages.Question("Do you want to save " + buffer.Name);
        //                if (questionResult != System.Windows.MessageBoxResult.Yes)
        //                    continue;

        //                if (buffer.BufferGeom != null && !buffer.BufferGeom.IsEmpty)
        //                {
        //                    var volume = new AirspaceVolume();
        //                    var pdmAirspace = new PDM.Airspace {AirspaceVolumeList = new List<AirspaceVolume> {volume}};
        //                    volume.Geo =
        //                        Aran.Converters.ConvertToEsriGeom.FromGeometry(
        //                            GlobalParams.SpatialRefOperation.ToGeo(buffer.BufferGeom), true);

        //                    volume.BrdrGeometry = HelperClass.SetObjectToBlob(volume.Geo, "Border");

        //                    pdmAirspace.TxtName = buffer.Name;

        //                    var window = new SavePoint(pdmAirspace);  

        //                    var helper = new WindowInteropHelper(window);
        //                    helper.Owner = new IntPtr(GlobalParams.HWND);
        //                    ElementHost.EnableModelessKeyboardInterop(window);
        //                    window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
        //                    window.ShowDialog();
        //                }
        //            }
        //        }
        //        Functions.SaveArenaProject();

        //    }
        //    catch(Exception e)
        //    {
        //        Messages.Error("Error saving Airspace to GeoDB : " + e.Message);
        //    }


        //}

        private void saveToArena(object obj)
        {
            try
            {
                var savedBufferList = new List<BufferModel>();
                bool isSaved = false;
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
                            designBuffer.BufferWidth = buffer.Width;
                            designBuffer.UOMBufferWidth = DistanceUnit;

                            if (GlobalParams.DesigningAreaReader.SaveBuffer(designBuffer))
                            {
                                isSaved = true;
                                savedBufferList.Add(buffer);
                            }
                        }
                    }
                }
                if (isSaved)
                {
                    Aran.Delta.Model.Messages.Info("Feature saved database successfully");
                    Clear();
                    Functions.SaveArenaProject();
                }
                savedBufferList.ForEach(buf => BufferList.Remove(buf));
            }

            catch (Exception e)
            {
                Model.Messages.Error("Error appered when trying save feature to DB!" + e.Message);
            }
        }

        private Airspace CopyAirspace(Airspace to)
        {
            var airspace =(Airspace) to.Clone();
            return airspace;

        }

        #endregion
    }
}
