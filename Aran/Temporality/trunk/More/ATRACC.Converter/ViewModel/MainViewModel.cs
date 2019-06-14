using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using ATRACC.Converter.Properties;
using ATRACC.Converter.Template.AtsGeoPoints;
using ATRACC.Converter.Template.AtsRoutes;
using ATRACC.Converter.Util;
using Microsoft.Win32;
using MvvmCore;

namespace ATRACC.Converter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _isFeatures = true;
        private bool _isLinks = true;
        private bool _isRootProperties = true;
        private bool _isAllProperties = true;

        public bool IsFeatures
        {
            get { return _isFeatures; }
            set
            {
                _isFeatures = value;
                OnPropertyChanged("IsFeatures");
            }
        }

        public bool IsLinks
        {
            get { return _isLinks; }
            set
            {
                _isLinks = value;
                OnPropertyChanged("IsLinks");
            }
        }

        public bool IsRootProperties
        {
            get { return _isRootProperties; }
            set
            {
                _isRootProperties = value;
                OnPropertyChanged("IsRootProperties");
            }
        }

        public bool IsAllProperties
        {
            get { return _isAllProperties; }
            set
            {
                _isAllProperties = value;
                OnPropertyChanged("IsAllProperties");
            }
        }


        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }


        private RelayCommand _setFileCommand;
        private string _filePath;
        private RelayCommand _saveRoutesCommand;
        private RelayCommand _savePointsCommand;

        public RelayCommand SetFileCommand
        {
            get { return _setFileCommand ?? (_setFileCommand = new RelayCommand(
                t =>
                {
                    var dialog = new OpenFileDialog
                    {
                        Multiselect = false,
                        Title = "Select AIX Message File",
                        Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        FilePath = dialog.FileName;
                    }
                })); }
        }

        private string _statusText;
        private BlockerModel _blockerModel;
        private string _startPoint;
        private string _endPoint;
        private string _geoPoint;

        public BlockerModel BlockerModel
        {
            get { return _blockerModel??(_blockerModel=new BlockerModel()); }
        }

        public RelayCommand SaveRoutesCommand
        {
            get { return _saveRoutesCommand??(_saveRoutesCommand=new RelayCommand(
                t =>
                {

                    CultureInfo ci = CultureInfo.InvariantCulture;
                    Thread.CurrentThread.CurrentCulture = ci;
                    Thread.CurrentThread.CurrentUICulture = ci;

                    var dialog = new SaveFileDialog
                    {
                        Title = "Save Statistics",
                        Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        BlockerModel.BlockForAction(
                            () =>
                            {
                                _features.Clear();
                                
                                AixmHelper aimHelper = new AixmHelper();
                                int totalCount = 0;
                                int count = 0;

                             
                                aimHelper.Open(FilePath,
                                                          () =>
                                                          {
                                                              totalCount++;
                                                              StatusText = totalCount + " features loaded...";
                                                          },
                                                          () =>
                                                          {
                                                              StatusText = "Cleaning memory...";
                                                          },
                                    (aixmFeatureList, collection) =>
                                    {
                                        foreach (Feature feature in aixmFeatureList)
                                        {
                                            ProcessFeature(feature);
                                            count++;
                                            StatusText = "Processed " + count + " features from " + totalCount + "...";
                                        }
                                        collection.Clear(); //clear memory
                                    });

                                StatusText = "Done";
                                if (aimHelper.IsOpened && _features.ContainsKey(FeatureType.Route) && _features.ContainsKey(FeatureType.RouteSegment))
                                {
                                    var routes = _features[FeatureType.Route].Cast<Route>().ToList();
                                    var routeSegments = _features[FeatureType.RouteSegment].Cast<RouteSegment>().ToList();

                                    routes=routes.OrderBy(t2 => t2.Name).ToList();
                                    Routes=new List<RouteInfo>();
                                    foreach (var route in routes)
                                    {
                                        
                                        var startAnno = route.Annotation == null
                                            ? null
                                            : route.Annotation.FirstOrDefault(
                                            t2 => t2.PropertyName.Equals(StartPoint,StringComparison.InvariantCultureIgnoreCase));

                                        var endAnno = route.Annotation == null
                                            ? null
                                            : route.Annotation.FirstOrDefault(
                                            t2 => t2.PropertyName.Equals(EndPoint,StringComparison.InvariantCultureIgnoreCase));

                                        var info = new RouteInfo
                                        {
                                            Name = route.Name,
                                            Start =
                                                (startAnno == null || startAnno.TranslatedNote == null ||
                                                 startAnno.TranslatedNote.Count == 0 ||
                                                 startAnno.TranslatedNote.First().Note == null)
                                                    ? "MISSING"
                                                    : startAnno.TranslatedNote.First().Note.Value,
                                            End =
                                                (endAnno == null || endAnno.TranslatedNote == null ||
                                                 endAnno.TranslatedNote.Count == 0 ||
                                                 endAnno.TranslatedNote.First().Note == null)
                                                    ? "MISSING"
                                                    : endAnno.TranslatedNote.First().Note.Value
                                        };


                                        var route1 = route;

                                        var relatedSegments =
                                            routeSegments.Where(
                                                t2 =>
                                                    t2.RouteFormed != null &&
                                                    t2.RouteFormed.Identifier == route1.Identifier).ToList();

                                        try
                                        {
                                            if (relatedSegments.Count == 0)
                                            {
                                                throw new Exception("No Segments");
                                            }
                                            if (relatedSegments.Any(t2=>t2.Start==null))
                                            {
                                                throw new Exception("No start point");
                                            }
                                            if (relatedSegments.Any(t2 => t2.End == null))
                                            {
                                                throw new Exception("No end point");
                                            }
                                        
                                            relatedSegments = SortSegments(relatedSegments);
                                        }
                                        catch (Exception exception)
                                        {
                                            MessageBox.Show("Problem with route " + route.Name + " " + route.Identifier +
                                                            "\n" + exception.Message+"\n"+"Route will be omitted");
                                            continue;
                                        }
                                        

                                        info.PointList = new List<string>();
                                        if (relatedSegments.Count > 0)
                                        {
                                            var first = relatedSegments.First();
                                            if (first != null)
                                            {
                                                info.PointList.Add(GetPointDesignator(first.Start));
                                                foreach (var segment in relatedSegments)
                                                {
                                                    info.PointList.Add(segment == null
                                                        ? "MISSING"
                                                        : GetPointDesignator(segment.End));
                                                }
                                                info.PointList.Reverse();
                                            }

                                            
                                        }
                                        Routes.Add(info);
                                    }
                                    
                                    var page = new AtsRoutesTemplate
                                    {
                                        Routes = Routes
                                    };

                                    var pageContent = page.TransformText();
                                    File.WriteAllText(dialog.FileName, pageContent);


                                    try
                                    {
                                        Process.Start(dialog.FileName);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show("Can not open " + dialog.FileName + " in default browser.",
                                            "Can Not Open", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                }

                            }
                            );
                    }
                },
                t => !string.IsNullOrEmpty(FilePath))); }
        }

     
        private List<RouteSegment> SortSegments(List<RouteSegment> relatedSegments)
        {
            if (relatedSegments == null || relatedSegments.Count < 2) return relatedSegments;

            //first is the one that not second
            var endIds = relatedSegments.Select(t => GetPointId(t.End)).ToList();

            var independent = relatedSegments.Where(t => !endIds.Contains(GetPointId(t.Start))).ToList();
            if (independent.Count == 0)
            {
                throw new Exception("Can not find first segment");
            }
            if (independent.Count > 1)
            {
                throw new Exception("Several first segments detected");
            }

            var result = new List<RouteSegment>();
            var current = independent.First();
            while (true)
            {
                result.Add(current);
                var id = GetPointId(current.End);

                var next = relatedSegments.Where(t => GetPointId(t.Start) == id).ToList();
                if (next.Count == 0)
                {
                    break;
                }

                if (next.Count > 1)
                {
                    throw new Exception("Several next segments detected");
                }

                current = next.First();
            }

            if (result.Count != relatedSegments.Count)
            {
                throw new Exception("Can not recreate segment sequence");
            }

            return result;
        }

        private Guid? GetPointId(EnRouteSegmentPoint point)
        {
            if (point != null)
            {
                switch (point.PointChoice.Choice)
                {
                    case SignificantPointChoice.DesignatedPoint:
                        return point.PointChoice.FixDesignatedPoint.Identifier;
                    case SignificantPointChoice.Navaid:
                        return point.PointChoice.NavaidSystem.Identifier;
                    case SignificantPointChoice.TouchDownLiftOff:
                        return point.PointChoice.AimingPoint.Identifier;
                    case SignificantPointChoice.RunwayCentrelinePoint:
                        return point.PointChoice.RunwayPoint.Identifier;
                    case SignificantPointChoice.AirportHeliport:
                        return point.PointChoice.AirportReferencePoint.Identifier;
                    case SignificantPointChoice.AixmPoint:
                        //no id here
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return null;
        }

        private string GetPointDesignator(EnRouteSegmentPoint point)
        {
            if (point != null && point.PointChoice!=null)
            {
                switch (point.PointChoice.Choice)
                {
                    case SignificantPointChoice.DesignatedPoint:
                        var designatedPoint =
                            GetFeature(FeatureType.DesignatedPoint, point.PointChoice.FixDesignatedPoint.Identifier) as
                                DesignatedPoint;
                        if (designatedPoint == null) break;
                        return designatedPoint.Designator;
                    case SignificantPointChoice.Navaid:
                        var navaid = GetFeature(FeatureType.Navaid, point.PointChoice.NavaidSystem.Identifier) as Navaid;
                        if (navaid == null) break;
                        return navaid.Designator;
                    case SignificantPointChoice.TouchDownLiftOff:
                        var touch =
                            GetFeature(FeatureType.TouchDownLiftOff, point.PointChoice.AimingPoint.Identifier) as
                                TouchDownLiftOff;
                        if (touch == null) break;
                        return touch.Designator;
                    case SignificantPointChoice.RunwayCentrelinePoint:
                        var cent =
                            GetFeature(FeatureType.RunwayCentrelinePoint, point.PointChoice.RunwayPoint.Identifier) as
                                RunwayCentrelinePoint;
                        if (cent == null) break;
                        return cent.Designator;
                    case SignificantPointChoice.AirportHeliport:
                        var port =
                            GetFeature(FeatureType.AirportHeliport, point.PointChoice.AirportReferencePoint.Identifier)
                                as AirportHeliport;
                        if (port == null) break;
                        return port.Designator;
                    case SignificantPointChoice.AixmPoint:
                        //no designator here
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return "MISSING";
        }

        private Feature GetFeature(FeatureType type, Guid identifier)
        {
            if (_features.ContainsKey(type))
            {
                var f=_features[type].FirstOrDefault(t=>t.Identifier==identifier);
                if (f == null)
                {
                    MessageBox.Show("Can not load " + type+" : "+identifier);
                }
                return f;
            }
            else
            {
                MessageBox.Show("Can not load feature type " + type);
            }
            return null;
        }


        public List<RouteInfo> Routes { get; set; }

        private GeoFormatter GeoFormatter;

        public RelayCommand SavePointsCommand
        {
            get
            {
                return _savePointsCommand ?? (_savePointsCommand = new RelayCommand(
                    t =>
                    {

                        CultureInfo ci = CultureInfo.InvariantCulture;
                        Thread.CurrentThread.CurrentCulture = ci;
                        Thread.CurrentThread.CurrentUICulture = ci;

                        var dialog = new SaveFileDialog
                        {
                            Title = "Save Statistics",
                            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                        };

                        if (dialog.ShowDialog() == true)
                        {
                            BlockerModel.BlockForAction(
                                () =>
                                {

                                    AixmHelper aimHelper = new AixmHelper();
                                    int totalCount = 0;
                                    int count = 0;

                                    aimHelper.Open(FilePath,
                                                              () =>
                                                              {
                                                                  totalCount++;
                                                                  StatusText = totalCount + " features loaded...";
                                                              },
                                                              () =>
                                                              {
                                                                  StatusText = "Cleaning memory...";
                                                              },
                                        (aixmFeatureList, collection) =>
                                        {
                                            foreach (Feature feature in aixmFeatureList)
                                            {
                                                ProcessFeature(feature);
                                                count++;
                                                StatusText = "Processed " + count + " features from " + totalCount + "...";
                                            }
                                            collection.Clear(); //clear memory
                                        });

                                    StatusText = "Done";
                                    if (aimHelper.IsOpened)
                                    {
                                        var page = new AtsGeoPointsTemplate();


                                        //prepare VOR/DME
                                        List<Feature> navaid;
                                        if (!_features.TryGetValue(FeatureType.Navaid, out navaid))
                                        {
                                            navaid = new List<Feature>();
                                        }
                                        var vordme =
                                            navaid.Cast<Navaid>()
                                                .Where(t2 => t2.Type == CodeNavaidService.VOR_DME).OrderBy(t2=>t2.Designator)
                                                .ToList();
                                        List<Feature> list;
                                        if (!_features.TryGetValue(FeatureType.VOR, out list))
                                        {
                                            list = new List<Feature>();
                                        }
                                        var vor = list.Cast<VOR>().ToList();
                                        foreach (var nav in vordme)
                                        {
                                            var model = new VorDme
                                            {
                                                Designator = nav.Designator
                                            };
                                            if (nav.NavaidEquipment != null)
                                            {
                                                foreach (var eq in nav.NavaidEquipment)
                                                {
                                                    var v =
                                                        vor.FirstOrDefault(
                                                            t2 => t2.Identifier == eq.TheNavaidEquipment.Identifier);
                                                    if (v != null)
                                                    {
                                                        model.MagneticVariation=string.Format("{0:0.0}", v.MagneticVariation);
                                                    }
                                                }
                                            }
                                            page.VorDme.Add(model);
                                        }
                                       
                                        //prepare GeoPoints
                                        List<Feature> desPoints;
                                        if (!_features.TryGetValue(FeatureType.DesignatedPoint, out desPoints))
                                        {
                                            desPoints=new List<Feature>();
                                        }
                                        foreach (var item in desPoints.Cast<DesignatedPoint>())
                                        {
                                            var geoAnno = item.Annotation.FirstOrDefault(
                                            t2 => t2.PropertyName.Equals(GeoPoint,StringComparison.InvariantCultureIgnoreCase));
                                            page.GeoPoints.Add(new GeoPoint
                                            {
                                                Designator = item.Designator,
                                                Coordinates = GeoFormatter.FormatYXdms(item.Location.Geo.X,item.Location.Geo.Y),
                                                More = (geoAnno == null || geoAnno.TranslatedNote == null ||
                                                 geoAnno.TranslatedNote.Count == 0 ||
                                                 geoAnno.TranslatedNote.First().Note == null)
                                                    ? ""
                                                    : geoAnno.TranslatedNote.First().Note.Value,
                                            });
                                        }


                                        var pageContent = page.TransformText();
                                        File.WriteAllText(dialog.FileName, pageContent);


                                        try
                                        {
                                            Process.Start(dialog.FileName);
                                        }
                                        catch (Exception exception)
                                        {
                                            MessageBox.Show("Can not open " + dialog.FileName + " in default browser.",
                                                "Can Not Open", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                    }

                                }
                                );
                        }
                    },
                    t => !string.IsNullOrEmpty(FilePath)));
            }
        }


        private Dictionary<FeatureType, List<Feature>> _features = new Dictionary<FeatureType, List<Feature>>(); 
        private readonly List<FeatureType> _acceptedTypes=new List<FeatureType>
        {
            FeatureType.Route,
            FeatureType.RouteSegment,
            FeatureType.VOR,
            FeatureType.DME,
            FeatureType.DesignatedPoint,
            FeatureType.Navaid,
            FeatureType.TouchDownLiftOff,
            FeatureType.RunwayCentrelinePoint,
            FeatureType.AirportHeliport,
        }; 

        private void ProcessFeature(Feature feature)
        {
            if (!_acceptedTypes.Contains(feature.FeatureType)) return;            

            List<Feature> list;
            if (!_features.TryGetValue(feature.FeatureType, out list))
            {
                list = new List<Feature>();
                _features[feature.FeatureType] = list;
            }

            list.Add(feature);              
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        public string StartPoint
        {
            get { return _startPoint ?? (_startPoint = Settings.Default.Start); }
            set
            {
                _startPoint = value;
                OnPropertyChanged("StartPoint");
            }
        }

        public string EndPoint
        {
            get { return _endPoint??(_endPoint=Settings.Default.End); }
            set
            {
                _endPoint = value;
                OnPropertyChanged("EndPoint");
            }
        }

        public string GeoPoint
        {
            get { return _geoPoint??(_geoPoint=Settings.Default.Geo); }
            set
            {
                _geoPoint = value;
                OnPropertyChanged("GeoPoint");
            }
        }
    }
}
