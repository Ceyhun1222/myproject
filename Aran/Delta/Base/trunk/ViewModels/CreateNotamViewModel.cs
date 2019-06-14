using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using System.Windows;
using Aran.Delta.Model;

namespace Aran.Delta.ViewModels
{
    public class CreateNotamViewModel:ViewModel
    {
        private string[] _pointList;
        private Aran.Geometries.MultiPolygon _curGeo;
        private int _curGeometryHandle;
        private List<NotamPointClass> _pointFormatList;
        public CreateNotamViewModel()
        {
            SelectedIndex = 0;
            StartDate = DateTime.Now;
            StopDate = DateTime.UtcNow;
            DistanceVerticalList = Enum.GetValues(typeof(UomDistanceVertical)).Cast<UomDistanceVertical>().ToList<UomDistanceVertical>();
            SelectedMinDistanceParam = DistanceVerticalList[0];
            SelectedMaxDistanceParam = DistanceVerticalList[0];

            DrawCommand = new RelayCommand(new Action<object>(draw_onClick));
            ClearCommand = new RelayCommand(new Action<object>(clear_onClick));
            if (GlobalParams.AranEnv != null)
                SaveCommand = new RelayCommand(new Action<object>(save_onClick));
            else
            {
                SaveCommand = new RelayCommand(new Action<object>(saveToArena));
            }
        }
    
        public RelayCommand DrawCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        private double _minAltitude;
        public double MinAltitude
        {
            get { return _minAltitude; }
            set 
            {
                _minAltitude = value;
                NotifyPropertyChanged("MinAltitude");
            }
        }

        private double _maxAltitude;
        public double MaxAltitude
        {
            get { return _maxAltitude; }
            set 
            {
                _maxAltitude = value;
                NotifyPropertyChanged("MaxAltitude");
            }
        }

        public List<UomDistanceVertical> DistanceVerticalList { get; set; }

        private UomDistanceVertical _selectedMinDistanceParam;
        public UomDistanceVertical SelectedMinDistanceParam
        {
            get { return _selectedMinDistanceParam; }
            set 
            {
                _selectedMinDistanceParam = value;
                NotifyPropertyChanged("SelectedMinDistanceParam");
            }
        }

        private UomDistanceVertical _selectedMaxDistanceParam;
        public UomDistanceVertical SelectedMaxDistanceParam
        {
            get { return _selectedMaxDistanceParam; }
            set 
            {
                _selectedMaxDistanceParam = value;
                NotifyPropertyChanged("SelectedMaxDistanceParam");
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }

        private DateTime _stopDate;
        public DateTime StopDate
        {
            get
            {
                return _stopDate;
            }
            set
            {
                _stopDate = value;
                NotifyPropertyChanged("StopDate");
            }
        }

        private string _notamPoints;
        public string NotamPoints
        {
            get { return _notamPoints; }
            set 
            {
                _notamPoints = value;
                if (_notamPoints.Length > 0)
                    _pointFormatList = ParseText(_notamPoints);
               
                NotifyPropertyChanged("NotamPoints");
            }
        }

        private List<NotamPointClass> ParseText(string ptsText)
        {
            var ptList = new List<NotamPointClass>();
            if (ptsText.Length == 0) return ptList;

            //IsEnter seperated

            var lines = ptsText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            //1-4-6-7 variant.Seperated with enter
            if (lines.Length > 2)
            {
                //check is first variant. 572835N242301E
                //                       575308N235606E 

                foreach (var line in lines)
                {
                    //1 Variant

                    var coordText = line.TrimEnd(' ');
                    coordText = coordText.TrimStart(' ');


                    var splitByProbel = coordText.Split(' ');

                    //1 variant 572835N242301E 
                    if (splitByProbel == null || splitByProbel.Length == 1)
                    {
                        var ptFormat = ReturnLatLong(coordText);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);
                    }

                   //6 variant N572835 E242301 
                    else if (splitByProbel.Length == 2)
                    {
                        coordText = coordText.Replace(" ", "");
                        var ptFormat = ReturnLatLong(coordText);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);
                    }

                    //4 variant  572835N 242301E STARI 
                    else if (splitByProbel.Length == 3)
                    {
                        var text = splitByProbel[0] + splitByProbel[1];
                        var ptFormat = ReturnLatLong(text);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);

                    }
                    //7 variant N 57 28 35.26 E 24 23 01.48
                    if (splitByProbel.Length > 3)
                    {
                        coordText = coordText.Replace(" ", "");
                        var ptFormat = ReturnLatLong(coordText);
                        if (ptFormat != null)
                            ptList.Add(ptFormat);
                    }
                }
            }
            else 
            {
               ptsText = ptsText.Replace(" ", "");

                //2 variant
               var splitByDashes = ptsText.Split('–');
               if (splitByDashes.Length > 2)
               {
                   foreach (var coordText in splitByDashes)
                   {
                       var strVal = coordText;
                       if (coordText.Contains('('))
                       {
                           int startIndex= coordText.IndexOf('(');
                           int endIndex = coordText.IndexOf(')');
                           
                           if (endIndex>startIndex)
                           {
                               strVal = coordText.Remove(startIndex, endIndex - startIndex+1);   
                           }
                           else continue;
                       }

                       var ptFormat = ReturnLatLong(strVal);
                       if (ptFormat != null)
                           ptList.Add(ptFormat);
                   }
               }
               else 
               {
                   var splitByComma = ptsText.Split(';');
                   foreach (var coordText in splitByComma)
                   {
                       var strVal = coordText;
                       if (coordText.Contains('('))
                       {
                           int startIndex = coordText.IndexOf('(');
                           int endIndex = coordText.IndexOf(')');
                           if (endIndex > startIndex)
                           {
                               strVal = coordText.Remove(startIndex, startIndex - endIndex);
                           }
                           else continue;
                       }

                       var ptFormat = ReturnLatLong(strVal);
                       if (ptFormat != null)
                           ptList.Add(ptFormat);
                   }
               
               }
            
            }
            return ptList;

        }

        private NotamPointClass ReturnLatLong(string text)
        {
            var ptFormat = new NotamPointClass();
            if (text[0] == 'N')
            {
                var latIndexLast = text.IndexOf('E');

                ptFormat.Format = CordinateType.InStart;
                ptFormat.Y = text.Substring(1, latIndexLast - 1);
                ptFormat.X = text.Substring(latIndexLast + 1);
                if (ptFormat.X != null && !ptFormat.X.StartsWith("0"))
                    ptFormat.X = 0 + ptFormat.X;
                
            }
            else
            {
                var latIndexLast = text.IndexOf('N');
                ptFormat.Format = CordinateType.InStart;
                ptFormat.Y = text.Substring(0, latIndexLast);
                ptFormat.X = text.Substring(latIndexLast + 1,text.Length-latIndexLast-2);
                if (ptFormat.X!=null && !ptFormat.X.StartsWith("0"))
                    ptFormat.X = 0 + ptFormat.X;
            }
            return ptFormat;
        }

        private void clear_onClick(object obj)
        {
            if (_curGeo != null)
                _curGeo.Clear();
            GlobalParams.UI.SafeDeleteGraphic(_curGeometryHandle);
        }

        private void draw_onClick(object obj)
        {
            clear_onClick(null);
            if (_pointFormatList == null || _pointFormatList.Count < 4)
            {
                Model.Messages.Warning("Point list is not correct!");
                return;
            }
            var _notamRing = new Geometries.Ring();

            foreach (var ptFormat in _pointFormatList)
            {
                var latitude = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd(ptFormat.Y, true);
                var longtitude = Aran.PANDA.Common.ARANFunctions.DmsStr2Dd(ptFormat.X, false);

                var ptPrj = GlobalParams.SpatialRefOperation.ToPrj(new Aran.Geometries.Point(longtitude, latitude));
                //GlobalParams.UI.DrawPointWithText(ptPrj, i.ToString());
                _notamRing.Add(ptPrj);
            }

            //for (int i = 0; i < _pointFormatList.Count - 1; i++)
            //{
            //    //var ptTempStr = _pointList[i].Trim().Trim('-');
            //    //char[] charArray = { 'N', 'W' };
            //    //int index = ptTempStr.IndexOf(charArray[0]);
            //    //var latText = "";

            //    ////N465357 E0771718
            //    //if (index == 0)
            //    //{
            //    //    // string[] latLong = ptTempStr.Split(" ");
            //    //}
            //    //else
            //    //{
            //    //    //must see here again.
            //    //    //Sign parameter is not working here
            //    //    latText = ptTempStr.Substring(0, index);
            //    //}
              
            //}

            if (_notamRing != null && !_notamRing.IsEmpty)
            {
                _curGeo = new Geometries.MultiPolygon{
                        new Aran.Geometries.Polygon{ExteriorRing =_notamRing}
                    };

                _curGeometryHandle = GlobalParams.UI.DrawDefaultMultiPolygon(_curGeo);
            }
        }

        private void save_onClick(object obj)
        {
            if (_curGeo == null || _curGeo.IsEmpty)
            {
                Model.Messages.Warning("Please first create Area,then save to DB");
                return;
            }

            var airspace = GlobalParams.Database.DeltaQPI.CreateFeature<Aran.Aim.Features.Airspace>();
            //airspace.TimeSlice.Interpretation = TimeSliceInterpretationType.TEMPDELTA;
            //if (_stopDate != null)
            //    airspace.TimeSlice.ValidTime.EndPosition = _stopDate;

            //if (_drawCtrIsChecked)
            //    airspace.Type = Aran.Aim.Enums.CodeAirspace.

            var airspaceComponent = new Aim.Features.AirspaceGeometryComponent();

            airspaceComponent.TheAirspaceVolume = new Aim.Features.AirspaceVolume();
            airspaceComponent.TheAirspaceVolume.HorizontalProjection = new Aim.Features.Surface();
            var airsapceGeo = GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.MultiPolygon>(_curGeo);
            foreach (Aran.Geometries.Polygon geom in airsapceGeo)
                airspaceComponent.TheAirspaceVolume.HorizontalProjection.Geo.Add(geom);
            airspaceComponent.TheAirspaceVolume.LowerLimit = new Aim.DataTypes.ValDistanceVertical(MinAltitude, SelectedMinDistanceParam);
            airspaceComponent.TheAirspaceVolume.UpperLimit = new Aim.DataTypes.ValDistanceVertical(MaxAltitude, SelectedMaxDistanceParam);

            airspace.GeometryComponent.Add(airspaceComponent);

            //Aim.Features.AirspaceActivation activation = new Aim.Features.AirspaceActivation();
            ////airspace.Activation.Add(activation);
            //Aim.Features.AirspaceLayer level = new Aim.Features.AirspaceLayer();
            //activation.Levels.Add(level);
            //level.LowerLimit = new Aim.DataTypes.ValDistanceVertical(MinAltitude, SelectedMinDistanceParam);
            //level.UpperLimit = new Aim.DataTypes.ValDistanceVertical(MaxAltitude, SelectedMaxDistanceParam);

            //var timeSheet = new Aran.Aim.Features.Timesheet();
            //activation.TimeInterval.Add(timeSheet);
            //timeSheet.TimeReference = CodeTimeReference.UTC;
            //timeSheet.StartDate = _startDate.ToString("dd-MM");//Day - Month(15-02)
            //timeSheet.StartTime = _startDate.ToString("HH:mm");

            ////switch (_startDate.DayOfWeek)
            ////{
            ////    case DayOfWeek.Friday:
            ////        timeSheet.Day = CodeDay.FRI;
            ////        break;
            ////    case DayOfWeek.Monday:
            ////        timeSheet.Day = CodeDay.MON;
            ////        break;
            ////    case DayOfWeek.Saturday:
            ////        timeSheet.Day = CodeDay.SAT;
            ////        break;
            ////    case DayOfWeek.Sunday:
            ////        timeSheet.Day = CodeDay.SUN;
            ////        break;
            ////    case DayOfWeek.Thursday:
            ////        timeSheet.Day = CodeDay.THU;
            ////        break;
            ////    case DayOfWeek.Tuesday:
            ////        timeSheet.Day = CodeDay.TUE;
            ////        break;
            ////    case DayOfWeek.Wednesday:
            ////        timeSheet.Day = CodeDay.WED;
            ////        break;
            ////    default:
            ////        break;
            ////}

            //// _startDate.Hour + ":" + _startDate.Minute;
            //if (_stopDate != null) 
            //{
            //    timeSheet.EndDate = _stopDate.ToString("dd-MM");//Day - Month(15-02)
            //    timeSheet.EndTime = _stopDate.ToString("HH:mm");// _startDate.Hour + ":" + _startDate.Minute;
            //    //switch (_stopDate.DayOfWeek)
            //    //{
            //    //    case DayOfWeek.Friday:
            //    //        timeSheet.DayTil = CodeDay.FRI;
            //    //        break;
            //    //    case DayOfWeek.Monday:
            //    //        timeSheet.DayTil = CodeDay.MON;
            //    //        break;
            //    //    case DayOfWeek.Saturday:
            //    //        timeSheet.DayTil = CodeDay.SAT;
            //    //        break;
            //    //    case DayOfWeek.Sunday:
            //    //        timeSheet.DayTil = CodeDay.SUN;
            //    //        break;
            //    //    case DayOfWeek.Thursday:
            //    //        timeSheet.DayTil = CodeDay.THU;
            //    //        break;
            //    //    case DayOfWeek.Tuesday:
            //    //        timeSheet.DayTil = CodeDay.TUE;
            //    //        break;
            //    //    case DayOfWeek.Wednesday:
            //    //        timeSheet.DayTil = CodeDay.WED;
            //    //        break;
            //    //    default:
            //    //        break;
            //    //}
            //}

            //  GlobalParams.Database.DeltaQPI.SetFeature(airspace);
            try
            {

                GlobalParams.Database.DeltaQPI.SetRootFeatureType(Aim.FeatureType.Airspace);
                if (GlobalParams.Database.DeltaQPI.Commit())
                    Model.Messages.Info("Airspace successfully saved to Aixm 5.1 DB");
            }
            catch (Exception)
            {
                Model.Messages.Info("Error saving data to Aixm 5.1 DB");

            }
        }

        private void saveToArena(object obj)
        {
            try
            {
                if (_curGeo != null && !_curGeo.IsEmpty)
                {
                    var mlt = _curGeo.ToMultiPoint();
                    if (mlt.Count < 3) return;

                    var designingArea = new Model.DesigningArea();
                    designingArea.Geo = GlobalParams.SpatialRefOperation.ToGeo(_curGeo);

                    if (GlobalParams.DesigningAreaReader.SaveArea(designingArea))
                    {
                        Aran.Delta.Model.Messages.Info("Feature saved database successfully");
                        clear_onClick(null);
                        Functions.SaveArenaProject();
                    }
                }
                else
                {
                    Model.Messages.Warning("Area cannot be empty!Please firs create Area then Save to Database!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error appered when trying save feature to DB!" + e.Message);
            }
        }
    }
}
