using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Aim.Data;
using Aran.Aim.PropertyEnum;
using Aran.Delta.Enums;
using Aran.Delta.Model;
using Aran.Delta.View;
using Aran.Geometries;
using Aran.PANDA.Common;
using ARENA;
using Aran.Aim.Features;

namespace Aran.Delta.ViewModels
{
    public class IntersectionViewModel:ViewModel
    {
        private List<Model.IntersectItemType> _itemTypeList;
        private List<Model.IntersectionItem> _allItems; 
        private IntersectItemType _leftSelItemType;
        private IntersectItemType _rightSelItemType;
        private bool _leftSegmentIsChecked;
        private bool _rightSegmentIsChecked;
        private bool _leftAreasIsChecked;
        private bool _rightAreasIsChecked;
        private IntersectionItem _leftSelItem;
        private IntersectionItem _rightSelItem;
        private string _pointLatLongStr;
        private IntersectResultPoint _selectedResultPoint;
        private int _resultPtHandle;


        public IntersectionViewModel()
        {
            LeftItemTypeList = new ObservableCollection<IntersectItemType>();
            RightItemTypeList = new ObservableCollection<IntersectItemType>();

            _itemTypeList = new List<IntersectItemType>
            {
                new IntersectItemType
                {
                    FeatureType = FeatureType.Airspace,
                    Header = "Airspace"
                },
                new IntersectItemType
                {
                    FeatureType = FeatureType.Route,
                    Header = "Routes",
                    IsArea = false
                }
            };

            _allItems = new List<IntersectionItem>();
            FillAllItems();

            LeftItemList  = new ObservableCollection<IntersectionItem>();
            RightItemList = new ObservableCollection<IntersectionItem>();
            ResultPointList  = new ObservableCollection<IntersectResultPoint>();

            LeftSegmentIsChecked = true;
            RightSegmentIsChecked = true;

            FindIntersectionCommand= new RelayCommand(new Action<object>(FindIntersection));
            if (GlobalParams.AranEnv != null)
                SaveCommand = new RelayCommand(new Action<object>(saveToAim));
            else
                SaveCommand = new RelayCommand(new Action<object>(SaveToArena));
            CloseCommand = new RelayCommand(new Action<object>(Close_OnClick));
        }

        private void saveToAim(object obj)
        {
            if (GlobalParams.AranEnv != null)
            {
                var dPoint = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.DesignatedPoint>();
                var note = new Aim.Features.Note();
                note.Purpose = Aim.Enums.CodeNotePurpose.REMARK;
                var linguisticNote = new LinguisticNote();
                linguisticNote.Note = new Aim.DataTypes.TextNote();
                var noteText = "Has created by Delta!";
                linguisticNote.Note.Value = noteText;
                note.TranslatedNote.Add(linguisticNote);
                dPoint.Annotation.Add(note);

                dPoint.Location = new Aim.Features.AixmPoint();
                dPoint.Location.Geo.X = SelectedResultPoint.Geo.X;
                dPoint.Location.Geo.Y = SelectedResultPoint.Geo.Y;
                GlobalParams.Database.DeltaQPI.SetFeature(dPoint);
                GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.DesignatedPoint);

                var dbProvider = GlobalParams.AranEnv.DbProvider as DbProvider;
                if (dbProvider == null) return;
                GlobalParams.Database.DeltaQPI.Commit(dbProvider.ProviderType != DbProviderType.TDB);
                GlobalParams.Database.DeltaQPI.ExcludeFeature(dPoint.Identifier);
            }

        }

        private void Close_OnClick(object obj)
        {
            this.Close();
        }

        private void SaveToDb(object obj)
        {
            if (SelectedResultPoint != null)
            {
                var pdmObject = new PDM.WayPoint();
                SelectedResultPoint.Geo.M = 0;
                pdmObject.Geo =Aran.Converters.ConvertToEsriGeom.FromPoint(SelectedResultPoint.Geo,true);

                var window = new SavePoint(pdmObject);

                var helper = new WindowInteropHelper(window);
                helper.Owner =new IntPtr(GlobalParams.HWND) ;
                ElementHost.EnableModelessKeyboardInterop(window);
                window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
                window.ShowDialog();
                Functions.SaveArenaProject();
            }
        }

        private void SaveToArena(object obj)
        {
            var designingPoint = new Model.DesigningPoint();
            Aran.Geometries.Point savePt = null;
            if (SelectedResultPoint.Geo == null) Messages.Warning("Not intersected!");
            savePt = SelectedResultPoint.Geo;
            savePt.Z = 0;
            savePt.M = 0;
            designingPoint.Geo = savePt;

            designingPoint.Lon = ARANFunctions.Degree2String(savePt.X, Degree2StringMode.DMSLon,
                 Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

            designingPoint.Lat = ARANFunctions.Degree2String(savePt.Y, Degree2StringMode.DMSLat,
             Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));

            if (GlobalParams.DesigningAreaReader.SavePoint(designingPoint))
            {
                Messages.Info("Feature saved database successfully");
                Clear();
                Functions.SaveArenaProject();
            }
        }

        private void FindIntersection(object obj)
        {
            FindIntersection();
        }

        public ObservableCollection<Model.IntersectItemType> LeftItemTypeList { get; set; }
        public ObservableCollection<Model.IntersectItemType> RightItemTypeList { get; set; }

        public ObservableCollection<IntersectionItem> LeftItemList { get; set; }
        public ObservableCollection<IntersectionItem> RightItemList { get; set; }

        public ObservableCollection<IntersectResultPoint> ResultPointList { get; set; }

        public RelayCommand FindIntersectionCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }


        public Model.IntersectItemType LeftSelItemType
        {
            get { return _leftSelItemType; }
            set
            {
                _leftSelItemType = value;

                if (_leftSelItemType == null) return;

                LeftItemList.Clear();
              
                foreach (var intersectionItem in _allItems)
                {
                    if (intersectionItem.FeatureType==_leftSelItemType.FeatureType)
                        LeftItemList.Add(intersectionItem);
                }

                if (LeftItemList.Count > 0)
                    LeftSelItem = LeftItemList[0];
                else
                    LeftSelItem = null;
                
                NotifyPropertyChanged("LeftSelItemType");
            }
        }

        public Model.IntersectItemType RightSelItemType
        {
            get { return _rightSelItemType; }
            set
            {
                _rightSelItemType = value;

                if (_rightSelItemType == null) return;

                RightItemList.Clear();

                foreach (var intersectionItem in _allItems)
                {
                    if (intersectionItem.FeatureType == _rightSelItemType.FeatureType)
                        RightItemList.Add(intersectionItem);
                }

                if (RightItemList.Count > 0)
                    RightSelItem = RightItemList[0];
                else
                    RightSelItem = null;

                NotifyPropertyChanged("RightSelItemType");
            }
        }

        public bool LeftSegmentIsChecked
        {
            get { return _leftSegmentIsChecked; }
            set
            {
                _leftSegmentIsChecked = value;

                if (!value) return;

                LeftItemTypeList.Clear();

                foreach (var intersectionFeature in _itemTypeList)
                {
                    if (!intersectionFeature.IsArea)
                        LeftItemTypeList.Add(intersectionFeature);
                }

                if (LeftItemTypeList.Count > 0)
                    LeftSelItemType = LeftItemTypeList[0];
            }
        }

        public bool RightSegmentIsChecked
        {
            get { return _rightSegmentIsChecked; }
            set
            {
                _rightSegmentIsChecked = value;

                if (!value) return;

                RightItemTypeList.Clear();

                foreach (var intersectionFeature in _itemTypeList)
                {
                    if (!intersectionFeature.IsArea)
                        RightItemTypeList.Add(intersectionFeature);
                }

                if (RightItemTypeList.Count > 0)
                    RightSelItemType = RightItemTypeList[0];
            }
        }

        public bool LeftAreasIsChecked
        {
            get { return _leftAreasIsChecked; }
            set
            {
                _leftAreasIsChecked = value;
                

                if (!value) return;

                LeftItemTypeList.Clear();

                foreach (var intersectionFeature in _itemTypeList)
                {
                    if (intersectionFeature.IsArea)
                        LeftItemTypeList.Add(intersectionFeature);
                }

                if (LeftItemTypeList.Count > 0)
                    LeftSelItemType = LeftItemTypeList[0];
            }
        }

        public bool RightAreasIsChecked
        {
            get { return _rightAreasIsChecked; }
            set
            {
                _rightAreasIsChecked = value;

                if (!value) return;

                RightItemTypeList.Clear();

                foreach (var intersectionFeature in _itemTypeList)
                {
                    if (intersectionFeature.IsArea)
                        RightItemTypeList.Add(intersectionFeature);
                }

                if (RightItemTypeList.Count > 0)
                    RightSelItemType = RightItemTypeList[0];
            }
        }

        public Model.IntersectionItem LeftSelItem
        {
            get { return _leftSelItem; }
            set
            {
                if (_leftSelItem != null && _leftSelItem!=_rightSelItem)
                    _leftSelItem.Clear();


                _leftSelItem = value;
                if (_leftSelItem!=null)
                    _leftSelItem.Draw();

                FindIntersection();

                NotifyPropertyChanged("LeftSelItem");
            }
        }

        public Model.IntersectionItem RightSelItem
        {
            get { return _rightSelItem; }
            set
            {
                if (_rightSelItem!=null && _leftSelItem!=_rightSelItem)
                    _rightSelItem.Clear();

                _rightSelItem = value;
                if (_rightSelItem!=null)
                    _rightSelItem.Draw();

                FindIntersection();

                NotifyPropertyChanged("RightSelItem");
            }
        }


        public IntersectResultPoint SelectedResultPoint
        {
            get { return _selectedResultPoint; }
            set
            {
                _selectedResultPoint = value;
                GlobalParams.UI.SafeDeleteGraphic(_resultPtHandle);
                if (_selectedResultPoint != null)
                {
                    PointLatLongStr = _selectedResultPoint.ToString();
                    _resultPtHandle = GlobalParams.UI.DrawPoint(_selectedResultPoint.Geo,
                        GlobalParams.Settings.SymbolModel.ResultPointSymbol);
                }
                else
                    PointLatLongStr = "";

                NotifyPropertyChanged("SelectedResultPoint");
            }
        }

        public string PointLatLongStr
        {
            get { return _pointLatLongStr; }
            set
            {
                _pointLatLongStr = value;
                NotifyPropertyChanged("PointLatLongStr");
            }
        }

        private void FillAllItems()
        {
            _allItems = new List<IntersectionItem>();

            var airspaceList = GlobalParams.Database.GetAirspaceList;

            if (airspaceList == null) return;

            foreach (var airspace in airspaceList)
            {
                var geo = new Aran.Geometries.MultiPolygon();
                foreach (var airspaceGeometryComponent in airspace.GeometryComponent)
                {

                    if (airspaceGeometryComponent.TheAirspaceVolume == null 
                        || airspaceGeometryComponent.TheAirspaceVolume.HorizontalProjection==null)
                    {
                        GlobalParams.AranEnv.GetLogger("Delta").Warn(airspace.Designator + " Geometry is empty");
                        continue;
                    }

                    var prjGeo =
                        GlobalParams.SpatialRefOperation.ToPrj(
                            airspaceGeometryComponent.TheAirspaceVolume.HorizontalProjection.Geo);
                    if (prjGeo != null && !prjGeo.IsEmpty)
                    {
                        var tmpConverGeo =
                                GlobalParams.GeometryOperators.UnionGeometry(geo, prjGeo);
                        if (tmpConverGeo == null)
                            continue;

                        if (tmpConverGeo.Type == GeometryType.MultiPolygon)
                            geo = (Aran.Geometries.MultiPolygon)tmpConverGeo;
                        else if (tmpConverGeo.Type == GeometryType.Polygon)
                            geo.Add((Aran.Geometries.Polygon)tmpConverGeo);
                    }
                }

                if (geo.IsEmpty) continue;
                var item = new IntersectionItem
                {
                    FeatureType = FeatureType.Airspace,
                    Feat = airspace,
                    Header = airspace.Name,
                    Geo = geo
                };
                _allItems.Add(item);
            }

            var routes = GlobalParams.Database.RouteList;
            foreach (var route in routes)
            {
                try
                {
                    var geo = new Aran.Geometries.MultiLineString();
                    var segmentList = GlobalParams.Database.GetRouteSegmentList(route.Identifier);
                    if (segmentList != null && segmentList.Count > 0)
                    {
                        foreach (var routeSegment in segmentList)
                        {
                            if (routeSegment.CurveExtent != null && routeSegment.CurveExtent.Geo != null)
                            {
                                var prjGeo = GlobalParams.SpatialRefOperation.ToPrj(routeSegment.CurveExtent.Geo);
                                geo = (MultiLineString)GlobalParams.GeometryOperators.UnionGeometry(geo, prjGeo);
                            }
                        }
                    }
                    else
                    {
                        if (GlobalParams.DesigningAreaReader != null)
                        {
                            List<DesigningSegment> designingSegmentList = GlobalParams.DesigningAreaReader.GetDesigningSegments(route.Name);
                            if (designingSegmentList != null)
                            {
                                foreach (var segment in designingSegmentList)
                                {
                                    if (segment.Geo != null && !segment.Geo.IsEmpty)
                                    {
                                        var prjGeo = GlobalParams.SpatialRefOperation.ToPrj(segment.Geo);
                                        geo = (MultiLineString)GlobalParams.GeometryOperators.UnionGeometry(geo, prjGeo);
                                    }
                                }
                            }
                        }

                    }
                    if (geo.IsEmpty) continue;

                    var item = new IntersectionItem
                    {
                        FeatureType = FeatureType.Route,
                        Feat = route,
                        Header = route.Name,
                        Geo = geo
                    };
                    _allItems.Add(item);
                }
                catch (Exception e)
                {
                    Messages.Error(e.Message + " " + route.Name);
                }
            }

            _allItems = _allItems.OrderBy(item => item.Header).ToList();
        }

        private void FindIntersection()
        {
            ResultPointList.Clear();
            if (LeftSelItem == null || RightSelItem == null) return;

            if (LeftSelItem.Geo.IsEmpty || RightSelItem.Geo.IsEmpty) return;

            var geo1 = LeftSelItem.Geo;
            if (geo1.Type == GeometryType.MultiPolygon)
                geo1 = ARANFunctions.PolygonToPolyLine(geo1 as Aran.Geometries.MultiPolygon);

            var geo2 = RightSelItem.Geo;
            if (geo2.Type == GeometryType.MultiPolygon)
                geo2 = ARANFunctions.PolygonToPolyLine(geo2 as Aran.Geometries.MultiPolygon);

            var geom = GlobalParams.GeometryOperators.Intersect(geo1, geo2);

            if (geom.Type == GeometryType.Point)
            {
                var intersectionItem = new IntersectResultPoint
                {
                    Geo = GlobalParams.SpatialRefOperation.ToGeo(geom as Aran.Geometries.Point),
                    Header = "Point 1"
                };
                ResultPointList.Add(intersectionItem);
            }
            else if (geom.Type == GeometryType.MultiPoint)
            {
                var mlt = geom as Aran.Geometries.MultiPoint;
                for (int i = 0; i < mlt.Count; i++)
                {
                    var intersectionItem = new IntersectResultPoint
                    {
                        Geo = GlobalParams.SpatialRefOperation.ToGeo(mlt[i] as Aran.Geometries.Point),
                        Header = "Point "+(i+1),
                    };
                    ResultPointList.Add(intersectionItem);
                }

            }

            if (ResultPointList.Count > 0)
                SelectedResultPoint = ResultPointList[0];
            else
                SelectedResultPoint = null;

        }

        internal void Clear()
        {
            GlobalParams.UI.SafeDeleteGraphic(_resultPtHandle);
            if (_leftSelItem != null) _leftSelItem.Clear();
            if (_rightSelItem != null) _rightSelItem.Clear();
        }
    }
}
