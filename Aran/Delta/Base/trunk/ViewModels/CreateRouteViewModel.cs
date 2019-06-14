using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Delta.Builders;
using Aran.Delta.Enums;
using Aran.Delta.Model;
using Aran.Queries.Common;
using Aran.Queries;
using ARENA;
using PDM;
using RouteSegment = PDM.RouteSegment;
using Aran.Metadata.Utils;

namespace Aran.Delta.ViewModels
{
    public class CreateRouteViewModel:ViewModel, ICreateRouteViewModel
    {
        #region :>Fields
        private List<Model.IPointModel> _pointList;
        private List<Aim.Features.DesignatedPoint> _designatedPointList;
        private List<Aim.Features.Navaid> _navaidEquipmentList;
        private int _endPointHandle,_startPointHandle;
        private Visibility _nameVisibility;
        private RouteSegmentModel _selectedSegment;
        private bool _saveIsEnabled;
        private ObjectStatus _selectedOperType;
        private Route _selectedRoute;
        private readonly List<RouteSegmentModel> _deletedSegmentList;
        private ILogger _logger;
        private readonly IDBModule _dbModule;

        #endregion

        #region :>Ctor
        public CreateRouteViewModel(ILogger logger,IDBModule dbModule, ISignificantPointBuilder significantPointBuilder,bool isArena)
        {
            _logger = logger;
            _dbModule = dbModule;
            SegmentList = new ObservableCollection<Model.RouteSegmentModel>();

            NameVisibility = isArena ? Visibility.Collapsed : Visibility.Visible;
            SaveCommand = isArena ? new RelayCommand(SaveToArena) : new RelayCommand(Save);

            RouteList = dbModule.RouteList
                       .Where(dp=>!string.IsNullOrEmpty(dp.Name))
                       .ToList();

            _deletedSegmentList = new List<RouteSegmentModel>();

            SelectedOperType = (int)ObjectStatus.New;

            var significantPointDirector = new SignificantPointDirector();
            _pointList = significantPointDirector.Create(significantPointBuilder).ToList();

            _segmentModelCreater = new SegmentModelCreater(_pointList);

            SaveIsEnabled = false;
        }
        
        #endregion

        #region :>Property
        public ObservableCollection<Model.RouteSegmentModel> SegmentList { get; set; }
        public List<Route> RouteList { get; set; }

        private System.Collections.IList _selectedModels = new System.Collections.ArrayList();
        public System.Collections.IList SelectedSegmentList
        {
            get { return _selectedModels; }
            set
            {
                _selectedModels = value;
                NotifyPropertyChanged("SelectedSegmentList");
            }
        }

        public RelayCommand AddCommand =>new RelayCommand((obj) =>
        {
            var segment = new RouteSegmentModel(ObjectStatus.New);
            segment.ApplyAllClicked += segment_ApplyAllClicked;

            if (SelectedSegment == null || SelectedSegment.Index > 1 || SegmentList.Count == 1)
            {
                segment.Index = SegmentList.Count + 1;

                if (SegmentList.Count == 0)
                    segment.SetPointList(_pointList);
                else
                {
                    var ptList = new List<IPointModel>(_pointList);
                    foreach (var curSegment in SegmentList)
                    {
                        if (curSegment.SelectedStartPoint != null)
                            ptList.Remove(curSegment.SelectedStartPoint);
                        if (curSegment.SelectedEndPoint != null)
                            ptList.Remove(curSegment.SelectedEndPoint);
                    }

                    var lastSegment = SegmentList[SegmentList.Count - 1];
                    lastSegment.RemoveEndPoints();

                    if (lastSegment.SelectedEndPoint != null)
                        segment.SetPointList(ptList, lastSegment.SelectedEndPoint);
                    else
                    {
                        Messages.Warning("Please first fill Route Segment end point!");
                        return;
                    }
                }
                SegmentList.Add(segment);
            }
            else
            {
                segment.Index = 1;
                SegmentList.Insert(0, segment);

                segment.EndPointList.Add(SelectedSegment.SelectedStartPoint);
                segment.SelectedEndPoint = SelectedSegment.SelectedStartPoint;// SetPointList(pt1List, SelectedSegment.SelectedStartPoint);

                _pointList.ForEach(segmentModel =>
                {
                    segment.StartPointList.Add(segmentModel);
                });

                ReArangeIndexes();
            }

            if (SegmentList.Count > 0)
                SaveIsEnabled = true;
        });

        public RelayCommand RemoveCommand =>new RelayCommand((obj) =>
        {
            int segmentIndex = 0;
            if (_selectedSegment != null )
            {
                segmentIndex = _selectedSegment.Index;
            }
            
            if (_selectedSegment != null && (segmentIndex == SegmentList.Count || segmentIndex == 1))
            {
                _selectedSegment.Clear();
                _deletedSegmentList.Add(_selectedSegment);
                SegmentList.Remove(_selectedSegment);
                if (segmentIndex > 1)
                {
                    if (SegmentList.Count > 0)
                    {
                        var lastSegment = SegmentList[SegmentList.Count - 1];
                        lastSegment.BackEndPoints();
                    }
                }
                else
                {
                    if (SegmentList.Count == 1)
                    {
                        var lastSegment = SegmentList[SegmentList.Count - 1];
                        lastSegment.BackEndPoints();
                    }
                }
            }

            ReArangeIndexes();

            if (SegmentList.Count == 0)
                SaveIsEnabled = false;
        });

        public RelayCommand SaveCommand { get; set; }

        public new RelayCommand CloseCommand =>new RelayCommand((obj) =>
        {
            Clear();
            Close();
        });

        public RelayCommand SplitCommand=>new RelayCommand((obj) =>
        {
            if (SelectedSegment == null)
            {
                Messages.Warning("Please first select the Segment!");
                return;
            }
            if (SelectedSegment.SelectedStartPoint == null || SelectedSegment.SelectedEndPoint == null)
            {
                Messages.Warning("Segment start and end cannot be empty!");
                return;
            }
            if (SelectedSegment.Status == ObjectStatus.Changed || SelectedSegment.Status == ObjectStatus.Existing)
            {
                if (!_deletedSegmentList.Any(segment => segment.RSegment.Identifier == SelectedSegment.RSegment.Identifier))
                    _deletedSegmentList.Add(SelectedSegment);
            }

            var segment1 = new RouteSegmentModel(ObjectStatus.Split);

            segment1.Index = SelectedSegment.Index;
            SegmentList.Insert(SelectedSegment.Index - 1, segment1);

            var segment2 = new RouteSegmentModel(ObjectStatus.Split);

            segment2.Index = SelectedSegment.Index + 1;
            SegmentList.Insert(SelectedSegment.Index, segment2);


            var pt1List = new List<IPointModel>(_pointList);
            segment1.SetList(pt1List);
            segment1.StartPointList.Add(SelectedSegment.SelectedStartPoint);
            segment1.SelectedStartPoint = SelectedSegment.SelectedStartPoint;// SetPointList(pt1List, SelectedSegment.SelectedStartPoint);


            var pt2List = new List<IPointModel>(_pointList);
            segment2.SetList(pt2List);

            _pointList.ForEach(segmentModel =>
            {
                segment1.EndPointList.Add(segmentModel);
                segment2.StartPointList.Add(segmentModel);

            });

            segment2.EndPointList.Add(SelectedSegment.SelectedEndPoint);
            segment2.SelectedEndPoint = SelectedSegment.SelectedEndPoint;

            SelectedSegment.Clear();

            SegmentList.Remove(SelectedSegment);

            segment1.ApplyAllClicked += segment_ApplyAllClicked;
            segment1.SegmentPointChanged += Segment_SegmentPointChanged;

            segment2.ApplyAllClicked += segment_ApplyAllClicked;
            segment2.SegmentPointChanged += Segment_SegmentPointChanged;

            ReArangeIndexes();
        });

        public RelayCommand JoinCommand => new RelayCommand((obj) =>
        {
            if (SelectedSegmentList.Count < 2) { Messages.Info("You should select at least 2 Segment!"); return; }

            //Always first selected segment's start must be set
            var sortedSegmentList = SelectedSegmentList.Cast<RouteSegmentModel>().OrderBy(segment => segment.Index).ToList();
            var prevSegment = sortedSegmentList[0] as RouteSegmentModel;
            if (prevSegment.SelectedStartPoint == null)
            {
                Messages.Warning("First segment' start point cannot be empty!");
                return;
            }

            for (int i = 1; i < sortedSegmentList.Count; i++)
            {
                var curSegment = sortedSegmentList[i];
                if ((curSegment.Index - prevSegment.Index) != 1)
                {
                    Messages.Warning("All selected elements should consistently!");
                    return;
                }
                prevSegment = curSegment;
            }

            //last segment end point always must be set
            var lastSegment = sortedSegmentList[sortedSegmentList.Count - 1] as RouteSegmentModel;
            if (lastSegment.SelectedEndPoint == null)
            {
                Messages.Warning("Last segment' end point cannot be empty!");
                return;
            }
            var firstJoinSegment = sortedSegmentList[0] as RouteSegmentModel;

            var joinSegment = new RouteSegmentModel(ObjectStatus.New);
            joinSegment.Index = firstJoinSegment.Index;

            var pt1List = new List<IPointModel>(_pointList);
            joinSegment.SetList(pt1List);
            joinSegment.StartPointList.Add(firstJoinSegment.SelectedStartPoint);
            joinSegment.SelectedStartPoint = firstJoinSegment.SelectedStartPoint;

            if (lastSegment.Index == SegmentList[SegmentList.Count - 1].Index)
            {
                pt1List.ForEach(pt => joinSegment.EndPointList.Add(pt));
                joinSegment.SelectedEndPoint = lastSegment.SelectedEndPoint;
            }
            else
            {
                joinSegment.EndPointList.Add(lastSegment.SelectedEndPoint);
                joinSegment.SelectedEndPoint = lastSegment.SelectedEndPoint;
            }

            SegmentList.Insert(firstJoinSegment.Index - 1, joinSegment);

            var selectedSegmentCount = SelectedSegmentList.Count;
            for (var i = 0; i < selectedSegmentCount; i++)
            {
                var selSegment = sortedSegmentList[i];
                if (selSegment.Status == ObjectStatus.Existing)
                    _deletedSegmentList.Add(selSegment);

                selSegment.Clear();
                SegmentList.Remove(selSegment);
            }

            joinSegment.ApplyAllClicked += segment_ApplyAllClicked;
            joinSegment.SegmentPointChanged += Segment_SegmentPointChanged;
            ReArangeIndexes();
        });

        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            set
            {
                _selectedRoute = value;

                try
                {
                    Clear();

                    var tmpSegmentList = new List<RouteSegmentModel>();

                    if (_selectedRoute != null)
                    {
                        var segmentList = _dbModule.GetRouteSegmentList(_selectedRoute.Identifier);
                        if (segmentList != null && segmentList.Count > 0)
                        {
                            foreach (var routeSegment in segmentList)
                            {
                                var segment = _segmentModelCreater.GetSegmentModel(routeSegment);
                                if (segment is null)
                                    throw new ArgumentNullException("Segment is empty!");

                                tmpSegmentList.Add(segment);
                            }

                            int i = 1;
                            foreach (var routeSegmentModel in tmpSegmentList)
                            {
                                routeSegmentModel.Index = i++;
                                routeSegmentModel.ApplyAllClicked += segment_ApplyAllClicked;
                                routeSegmentModel.SetList(_pointList);

                                routeSegmentModel.InitializeSegment();
                                routeSegmentModel.Status = ObjectStatus.Existing;
                            }

                            //This function is not work properly
                            SortAndFillRouteSegments(tmpSegmentList);

                            tmpSegmentList.ForEach(segment=>segment.Draw());

                            _pointList.ForEach(ptModel => SegmentList[segmentList.Count - 1].EndPointList.Add(ptModel));
                        }

                    }
                }
                catch (Exception e)
                {
                    Messages.Error("Error loading segment parametrs" + Environment.NewLine + e.Message);
                    GlobalParams.AranEnv.GetLogger("Delta").Error(e);
                }
                if (SegmentList.Count > 0)
                    SaveIsEnabled = true;

                NotifyPropertyChanged("SelectedRoute");
            }
        }

        public RouteSegmentModel SelectedSegment
        {
            get => _selectedSegment;
            set
            {
                _selectedSegment?.ClearSelected();

                _selectedSegment = value;
                _selectedSegment?.DrawSelected();

                NotifyPropertyChanged(nameof(SelectedSegment));
            }
        }

        public int SelectedOperType
        {
            get { return (int)_selectedOperType; }
            set
            {
                _selectedOperType =(ObjectStatus) value;
                NotifyPropertyChanged("SelectedOperType");
            }
        }

        private ISignificantPointDirector _significantPointDirector;

        public string RouteName { get; set; }

        public bool SaveIsEnabled
        {
            get { return _saveIsEnabled; }
            set
            {
                _saveIsEnabled = value;
                NotifyPropertyChanged("SaveIsEnabled");
            }
        }

        private readonly SegmentModelCreater _segmentModelCreater;

        public Visibility NameVisibility
        {
            get { return _nameVisibility; }
            set 
            {
                _nameVisibility = value;
                NotifyPropertyChanged("NameVisibility");
            }
        }

        public Visibility EditIsVisible {
            get { return GlobalParams.AranEnv != null ? Visibility.Visible : Visibility.Collapsed; }
        }
        

        #endregion

        #region :>Methods

        private void Segment_SegmentPointChanged(object sender, SegmentChangeEventArgs e)
        {
            var segment = sender as RouteSegmentModel;
            if (segment == null) return;

            if (segment.Status== ObjectStatus.Split)
            {
                if (!e.StartIsChanged)
                {
                    if (SegmentList.Count > segment.Index)
                    {
                        var nextSegment = SegmentList[segment.Index];
                        nextSegment.ChangeStartPoint(segment.SelectedEndPoint);// SelectedStartPoint = ;
                    }
                }
                else
                {
                    var prevSegment = SegmentList[segment.Index - 2];
                    prevSegment.ChangeEndPoint(segment.SelectedStartPoint);// SelectedEndPoint = 

                }

            }
        }

        void segment_ApplyAllClicked(object sender, EventArgs e)
        {
            var applySegment = sender as Aran.Aim.Features.RouteSegment;
            if (applySegment == null) return;

            foreach (var routeSegment in SegmentList)
            {
                var tmpSegment = routeSegment.RSegment;

                tmpSegment.Level = applySegment.Level;
                tmpSegment.UpperLimit = applySegment.UpperLimit;
                tmpSegment.LowerLimit = applySegment.LowerLimit;
                tmpSegment.UpperLimitReference = applySegment.UpperLimitReference;
                tmpSegment.LowerLimitReference = applySegment.LowerLimitReference;
                tmpSegment.TurnDirection = applySegment.TurnDirection;
                tmpSegment.SignalGap = applySegment.SignalGap;
                tmpSegment.MinimumEnrouteAltitude = applySegment.MinimumEnrouteAltitude;
                tmpSegment.NavigationType = applySegment.NavigationType;
                if (routeSegment.Status == ObjectStatus.Existing)
                    routeSegment.Status = ObjectStatus.Changed;

                tmpSegment.PathType = applySegment.PathType;
                tmpSegment.RequiredNavigationPerformance = tmpSegment.RequiredNavigationPerformance;
            }
        }

        void segment_EndPointChange(object sender, EventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(_endPointHandle);
            if (_selectedSegment != null && _selectedSegment.SelectedEndPoint != null)
              _endPointHandle =  GlobalParams.UI.DrawPointWithText(_selectedSegment.SelectedEndPoint.GeoPrj, _selectedSegment.SelectedEndPoint.Name);
        }
    
        void segment_StartPointChange(object sender, EventArgs e)
        {
            GlobalParams.UI.SafeDeleteGraphic(_startPointHandle);
            if (_selectedSegment != null && _selectedSegment.SelectedStartPoint != null)
                _startPointHandle = GlobalParams.UI.DrawPointWithText(_selectedSegment.SelectedStartPoint.GeoPrj, _selectedSegment.SelectedStartPoint.Name);
        }

        private void SaveToArena(object obj)
        {
            if (SegmentList.Count == 0)
            {
                Messages.Warning("Segment Count can not be 0 !");
                return;
            }

            if (SegmentList[SegmentList.Count - 1].SelectedEndPoint == null)
            {
                Messages.Warning("Please close Route Segment!");
                return;
            }

            var questionResult = Messages.Question("Do you want to save Result!");
            if (questionResult != System.Windows.MessageBoxResult.Yes)
                return;

            var route = new DesigningRoute();

            foreach (var segment in SegmentList)
            {
                var designingSegment = new DesigningSegment();
                designingSegment.WptStart = segment.SelectedStartPoint.Name;
                designingSegment.WptEnd = segment.SelectedEndPoint.Name;
                designingSegment.Geo = segment.Geo;
                designingSegment.ValLen = Math.Round(segment.Length, 1);
                designingSegment.UomDist = GlobalParams.Settings.DeltaInterface.DistanceUnit.ToString();
                designingSegment.ValueTrueTrack = segment.Direction;
                designingSegment.ValueReverseTrueTrack = segment.ReverseTrueTrack;

                route.SegmentList.Add(designingSegment);
            }
            if (GlobalParams.DesigningAreaReader.SaveRoute(route))
            {
                Aran.Delta.Model.Messages.Info("Feature saved database successfully!");
                Clear();
                Functions.SaveArenaProject();
            }
        }

        private void Save(object obj)
        {
            if (SegmentList.Count == 0)
            {
                Messages.Warning("Segment Count can not be 0 !");
                return;
            }

            foreach (var segment in SegmentList)
            {
                if (segment.SelectedStartPoint == null || segment.SelectedEndPoint == null)
                {
                    Messages.Warning("Please close Route Segment!");
                    return;
                }
            }

            var questionResult = Messages.Question("Do you want to save Result?");
            if (questionResult != System.Windows.MessageBoxResult.Yes)
                return;

            var note = new Note { Purpose = Aim.Enums.CodeNotePurpose.REMARK };

            var linguisticNote = new LinguisticNote { Note = new Aim.DataTypes.TextNote() };

            var noteText = "Has created by Delta!";
            linguisticNote.Note.Value = noteText;
            note.TranslatedNote.Add(linguisticNote);

            if (SelectedOperType == (int)ObjectStatus.New)
            {
                if (String.IsNullOrEmpty(RouteName))
                {
                    Messages.Warning("Please enter Route name");
                    return;
                }

                var route = GlobalParams.Database.DeltaQPI.CreateFeature<Route>();

                route.Annotation.Add(note);

                route.Name = RouteName;

                GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.Route);

                //GlobalParams.Database.DeltaQPI.Commit();

                var accuracyResolution = new List<GeoNumericalDataModel>
                {
                    new GeoNumericalDataModel
                    {
                        Accuracy = 0.0,
                        Resolution = 0.0
                    }
                };

                if ( GlobalParams.AranEnv.DbProvider is DbProvider dbProvider)
                {
                    bool result;
                    if (dbProvider.ProviderType == DbProviderType.TDB && GlobalParams.AranEnv.UseWebApi)
                    {
                        result = GlobalParams.Database.DeltaQPI.CommitWithMetadataViewer(
                            GlobalParams.AranEnv.Graphics.ViewProjection.Name, accuracyResolution,
                            dbProvider.ProviderType != DbProviderType.TDB);
                    }
                    else
                    {
                        result = GlobalParams.Database.DeltaQPI.Commit(dbProvider.ProviderType != DbProviderType.TDB);
                    }

                    if (result)
                    {
                        GlobalParams.Database.DeltaQPI.ExcludeFeature(route.Identifier);
                        foreach (var segment in SegmentList)
                        {
                            segment.RSegment.RouteFormed = route.GetFeatureRef();
                            GlobalParams.Database.DeltaQPI.SetFeature(segment.RSegment);
                        }
                        if (GlobalParams.Database.DeltaQPI.CommitWithoutViewer(null))
                        {
                            Messages.Info("All data save to Aixm 5.1 Database successfully");
                            SaveIsEnabled = false;
                            Clear();
                        }
                    }
                    
                }
            }
            else
            {
                foreach (var segment in SegmentList)
                {
                    if (segment.Status == ObjectStatus.New || segment.Status == ObjectStatus.Split)
                    {
                        segment.RSegment.RouteFormed = SelectedRoute.GetFeatureRef();
                        GlobalParams.Database.DeltaQPI.SetFeature(segment.RSegment);
                        segment.RSegment.Annotation.Add(note);
                    }
                    else if (segment.Status== ObjectStatus.Changed)
                    {
                        //   segment.RSegment.TimeSlice.Interpretation = Aim.Enums.TimeSliceInterpretationType.PERMDELTA;
                        // segment.RSegment.TimeSlice.CorrectionNumber += 1;

                        if (GlobalParams.AranEnv.DbProvider is DbProvider dbProvider &&
                            (dbProvider.ProviderType == DbProviderType.Aran ||
                             dbProvider.ProviderType == DbProviderType.AimLocal))
                        {
                            TimeSpan twoAndAHalfHours = new TimeSpan(1, 0, 0);
                            DateTimeOffset featLifeTime = segment.RSegment.TimeSlice.FeatureLifetime.BeginPosition;
                            DateTimeOffset mapEffectiveDate = dbProvider.DefaultEffectiveDate;

                            if (Math.Abs((featLifeTime - mapEffectiveDate).TotalHours) < 0.1)
                                segment.RSegment.TimeSlice.ValidTime.BeginPosition =
                                    dbProvider.DefaultEffectiveDate.Add(twoAndAHalfHours);
                            else
                            {
                                DateTimeOffset validTime = segment.RSegment.TimeSlice.ValidTime.BeginPosition;
                                if (Math.Abs((validTime - mapEffectiveDate).TotalHours) < 0.1)
                                    segment.RSegment.TimeSlice.ValidTime.BeginPosition =
                                        dbProvider.DefaultEffectiveDate.Add(twoAndAHalfHours);
                                else
                                    segment.RSegment.TimeSlice.ValidTime.BeginPosition = dbProvider.DefaultEffectiveDate;
                            }
                        }
                        //segment.RSegment.TimeSlice.CorrectionNumber++;
                        GlobalParams.Database.DeltaQPI.SetFeature(segment.RSegment,true);
                       // GlobalParams.Database.DeltaQPI.SetFeature(segment.RSegment);
                    }
                }

                foreach (var deletedSegment in _deletedSegmentList)
                {
                    deletedSegment.RSegment.TimeSlice.FeatureLifetime.EndPosition = GlobalParams.Database.DeltaQPI.TimeSlice.EffectiveDate;
                    deletedSegment.RSegment.TimeSlice.ValidTime.EndPosition = GlobalParams.Database.DeltaQPI.TimeSlice.EffectiveDate;
                    GlobalParams.Database.DeltaQPI.SetFeature(deletedSegment.RSegment);
                }

                if (GlobalParams.Database.DeltaQPI.CommitWithoutViewer(null))
                {
                    Messages.Info("All data has saved in Aixm 5.1 Database successfully");
                    SaveIsEnabled = false;
                    Clear();
                }
            }
        }
       
        private void SortAndFillRouteSegments(List<RouteSegmentModel> segmentList)
        {
            for (int i = 0; i < segmentList.Count - 1; i++)
            {
                bool isStart = true;
                foreach (RouteSegmentModel segment in segmentList)
                {
                    if (segmentList[i].SelectedStartPoint.Name == segment.SelectedEndPoint.Name)
                    {
                        isStart = false;
                        break;
                    }
                }
                if (isStart)
                {
                    segmentList[i].Index = 1;
                    AssignIndex(segmentList, segmentList[i]);
                    break;
                }
                else if (segmentList.Count == 2)
                {
                    segmentList[0].Index = 2;
                    segmentList[1].Index = 1;
                }
            }

            var tmpList = segmentList.OrderBy(routeSegment => routeSegment.Index).ToList<RouteSegmentModel>();
            tmpList.ForEach(segment => SegmentList.Add(segment));
        }

        private void AssignIndex(List<RouteSegmentModel> segmentList,RouteSegmentModel routeSegment)
        {
            if (routeSegment.Index == segmentList.Count)
                return;
            foreach (var tmpRouteSegment in segmentList)
            {
                if (routeSegment.SelectedEndPoint == tmpRouteSegment.SelectedStartPoint)
                {
                    tmpRouteSegment.Index = routeSegment.Index + 1;
                    AssignIndex(segmentList, tmpRouteSegment);
                    break;
                }
            }
        }

        private void ReArangeIndexes()
        {
            for (int i = 0; i < SegmentList.Count; i++)
                SegmentList[i].Index = i + 1;
        }

        public void Clear() 
        {
            foreach (var segment in SegmentList)
                segment.Clear();

            SegmentList.Clear();
            _deletedSegmentList.Clear();
        }

        #endregion
    }
}
