using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment.Symbols;
using Aran.Delta.Model;
using Aran.Geometries;
using DoddleReport;
using DoddleReport.Writers;

namespace Aran.Delta.ViewModels
{
    class ThreeDIntersectionViewModel:ViewModel
    {
        private Airspace _selectedAirspace;
        private int _airspaceHandle;
        private Geometries.MultiPolygon _selectedAirspaceGeo;
        private int _airspaceBufferHandle;
        private ThreeDIntersectionModel _selectedIntersectedAirsapce;
        private bool _leftShowBufferIsChecked;
        private double _leftBufferWidth;
        private int _leftRnp;
        private double _leftBv;
        private string _leftSelectedUpperUnit;
        private string _leftSelectedLowerUnit;
        private double _leftUpperLimit;
        private double _leftLowerLimit;
        private bool _rightShowBufferIsChecked;
        private double _rightBufferWidth;
        private int _rightRnp;
        private double _rightBv;
        private string _rightSelectedUpperUnit;
        private string _rightSelectedLowerUnit;
        private double _rightUpperLimit;
        private double _rightLowerLimit;
        private int _intersectedItemHandle;
        private MultiPolygon _selectedIntersectedItemGeo;
        private int _intersectedBufferHandle;
        private bool _createReportIsEnabled;
        private bool _intersectBufferIsEnabled;
        private string _resultMin;
        private string _resultMax;
        private int _intersectGeoHandle;

        public ThreeDIntersectionViewModel()
        {
            CurrCursor = System.Windows.Input.Cursors.Wait;
            Units = Enum.GetNames(typeof (UomDistanceVertical)).ToList();
            _leftRnp = 5;
            LeftBV = 1;

            _rightRnp = 5;
            RightBV = 1;

            FindIntersection = new RelayCommand(new Action<object>(FindIntersection_click));
            CloseCommand=new RelayCommand(new Action<object>(close_click));
            ReportCommand=new RelayCommand(new Action<object>(report_onClick));
            ClearCommand = new RelayCommand(new Action<object>(clear_onClick));

            IntersectedItemList = new ObservableCollection<ThreeDIntersectionModel>();

            CreateReportIsEnabled = false;
            IntersectBufferIsEnabled = true;

            AirspaceList = GlobalParams.Database.GetAirspaceList.OrderBy(airspace => airspace.Name).ToList();
            RouteList = GlobalParams.Database.RouteList.OrderBy(route=>route.Name).ToList();

            InitializeAirspaceExtensions();
            InitializeRouteExtensions();

            if (AirspaceList != null && AirspaceList.Count > 0)
            {
                SelectedAirspace = AirspaceList[0];
            }
            else
            {
                Close();
            }

            LayerList = new List<string> { "Airspace", "Route" };
            SelectedLayerType = LayerList[0];

          
            CurrCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void clear_onClick(object obj)
        {
            ResetIntersectResult();
        }

        #region :>Property
        public List<Airspace> AirspaceList { get; set; }
        public List<Route> RouteList{ get; set; }
        public ObservableCollection<ThreeDIntersectionModel> IntersectedItemList { get; set; }
        public List<string> LayerList { get; set; }
        public List<string> Units { get; set; }

        public RelayCommand FindIntersection { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand ReportCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }

        private System.Windows.Input.Cursor _curCursor;

        public System.Windows.Input.Cursor CurrCursor
        {
            get { return _curCursor; }
            set 
            {
                _curCursor = value;
                NotifyPropertyChanged("CurrCursor");
            }
        }

        public Airspace SelectedAirspace
        {
            get { return _selectedAirspace; }
            set
            {
                _selectedAirspace = value;
                if (value != null)
                {
                    DrawLeftAirspace();
                    if (LeftShowBufferIsChecked)
                        DrawLeftBuffer();


                    var lowerLimit = _selectedAirspace.GetLowerLimit();
                    if (lowerLimit != null)
                    {
                        LeftSelectedLowerUnit = Units[(int) lowerLimit.Uom];
                        LeftLowerLimit = lowerLimit.Value;
                    }
                    else
                    {
                        LeftLowerLimit = 0;
                    }
                    var upperLimit = _selectedAirspace.GetUpperLimit();
                    if (upperLimit != null)
                    {
                        LeftSelectedUpperUnit = Units[(int) upperLimit.Uom];
                        LeftUpperLimit = upperLimit.Value;
                    }
                    else
                    {
                        LeftUpperLimit = 0;
                    }


                }
                else
                    ClearLeftAirspace();
                ResetIntersectResult();
                NotifyPropertyChanged("SelectedAirspace");
            }
        }

        private string _selectedLayerType;
        public string SelectedLayerType
        {
            get { return _selectedLayerType; }
            set 
            {
                _selectedLayerType = value;
                if (_selectedLayerType != null)
                    ResetIntersectResult();
            }
        }
        
        public ThreeDIntersectionModel SelectedIntersectedItem
        {
            get { return _selectedIntersectedAirsapce; }
            set
            {
                _selectedIntersectedAirsapce = value;
                if (value != null)
                {
                    DrawRightItem();
                    if (RightShowBufferIsChecked)
                        DrawRightBuffer();

                    if (_selectedIntersectedAirsapce.LowerLimit != null)
                    {
                        RightSelectedLowerUnit = Units[(int)_selectedIntersectedAirsapce.LowerLimit.Uom];
                        RightLowerLimit = _selectedIntersectedAirsapce.LowerLimit.Value;
                    }
                    else
                    {
                        RightLowerLimit = 0;
                    }
                    if (_selectedIntersectedAirsapce.UpperLimit != null)
                    {
                        RightSelectedUpperUnit = Units[(int)_selectedIntersectedAirsapce.UpperLimit.Uom];
                        RightUpperLimit = _selectedIntersectedAirsapce.UpperLimit.Value;
                    }
                    else
                    {
                        RightUpperLimit = 0;
                    }

                    if (_selectedIntersectedAirsapce.IntersectedIn3D)
                    {
                        ResultMin = "Min : " + _selectedIntersectedAirsapce.MinIntersectValue + " " +
                                    _selectedIntersectedAirsapce.MinIntersectUnit;

                        ResultMax = "Max : " + _selectedIntersectedAirsapce.MaxIntersectValue + " " +
                                    _selectedIntersectedAirsapce.MaxIntersectUnit;
                    }
                    else
                    {
                        ResultMin = "There are not any 3D intersection";
                        ResultMax = "";
                    }

                }
                else 
                    ClearRightItem();
                NotifyPropertyChanged("SelectedIntersectedItem");
            }
        }

        public string DistanceUnit { get { return InitDelta.DistanceConverter.Unit; } }

        public bool LeftShowBufferIsChecked
        {
            get { return _leftShowBufferIsChecked; }
            set
            {
                _leftShowBufferIsChecked = value;
                if (_leftShowBufferIsChecked)
                    DrawLeftBuffer();
                else
                    ClearLeftBuffer();
                NotifyPropertyChanged("LeftShowBufferIsChecked");
            }
        }

        public double LeftBufferWidth
        {
            get { return Common.ConvertDistance(_leftBufferWidth,RoundType.ToNearest); }
            set
            {
                _leftBufferWidth =Common.DeConvertDistance(value);
                if (LeftShowBufferIsChecked)
                    DrawLeftBuffer();
                NotifyPropertyChanged("LeftBufferWidth");
            }
        }

        public int LeftRNP
        {
            get { return _leftRnp; }
            set
            {
                if (Math.Abs(_leftRnp - value) < 0.1) return;
    
                _leftRnp = value;
                LeftBufferWidth = Common.ConvertDistance(CalculateBufferWidth(_leftRnp, _leftBv),RoundType.ToNearest);
                NotifyPropertyChanged("LeftRNP");
            }
        }

        public double LeftBV
        {
            get { return Common.ConvertDistance(_leftBv,RoundType.ToNearest); }
            set
            {

                var tmpValue =Common.DeConvertDistance(value);
                if (Math.Abs(tmpValue - _leftBv) < 0.01)
                    return;
                _leftBv = tmpValue;
                LeftBufferWidth = Common.ConvertDistance(CalculateBufferWidth(_leftRnp, _leftBv),RoundType.ToNearest);
                
                NotifyPropertyChanged("LeftBV");
            }
        }

        public string LeftSelectedUpperUnit
        {
            get { return _leftSelectedUpperUnit; }
            set
            {
                if (LeftUpperLimit > 0)
                {
                    UomDistanceVertical fromUnit =(UomDistanceVertical) Enum.Parse(typeof (UomDistanceVertical), _leftSelectedUpperUnit);
                    UomDistanceVertical toUnit = (UomDistanceVertical) Enum.Parse(typeof (UomDistanceVertical), value);

                    double val = DeltaHeightConverters.ConvertTo(_leftUpperLimit, fromUnit, toUnit);
                    LeftUpperLimit =val;
                }
                 _leftSelectedUpperUnit = value;
               
                NotifyPropertyChanged("LeftSelectedUpperUnit");
            }
        }

        public string LeftSelectedLowerUnit
        {
            get { return _leftSelectedLowerUnit; }
            set
            {

                if (LeftLowerLimit> 0)
                {
                    UomDistanceVertical fromUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), _leftSelectedLowerUnit);
                    UomDistanceVertical toUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), value);

                    double val = DeltaHeightConverters.ConvertTo(_leftLowerLimit, fromUnit, toUnit);
                    LeftLowerLimit = val;
                }

                _leftSelectedLowerUnit = value;
                NotifyPropertyChanged("LeftSelectedLowerUnit");
            }
        }

        public double LeftUpperLimit
        {
            get { return  Math.Round(_leftUpperLimit); }
            set
            {
                _leftUpperLimit = value;
                NotifyPropertyChanged("LeftUpperLimit");
            }
        }

        public double LeftLowerLimit
        {
            get { return Math.Round(_leftLowerLimit); }
            set
            {
                _leftLowerLimit = value;
                NotifyPropertyChanged("LeftLowerLimit");
            }
        }


        public bool RightShowBufferIsChecked
        {
            get { return _rightShowBufferIsChecked; }
            set
            {
                _rightShowBufferIsChecked = value;
                if (_rightShowBufferIsChecked)
                    DrawRightBuffer();
                else
                    ClearRightBuffer();
                NotifyPropertyChanged("RightShowBufferIsChecked");
            }
        }

        public double RightBufferWidth
        {
            get { return Common.ConvertDistance(_rightBufferWidth, RoundType.ToNearest); }
            set
            {
                _rightBufferWidth = Common.DeConvertDistance(value);
                if (RightShowBufferIsChecked)
                    DrawRightBuffer();
                NotifyPropertyChanged("RightBufferWidth");
            }
        }

        public int RightRNP
        {
            get { return _rightRnp; }
            set
            {
                if (Math.Abs(_rightRnp - value) < 0.1) return;

                _rightRnp = value;
                RightBufferWidth = Common.ConvertDistance(CalculateBufferWidth(_rightRnp, _rightBv), RoundType.ToNearest);
                NotifyPropertyChanged("RightRNP");
            }
        }

        public double RightBV
        {
            get { return Common.ConvertDistance(_rightBv, RoundType.ToNearest); }
            set
            {

                var tmpValue = Common.DeConvertDistance(value);
                if (Math.Abs(tmpValue - _rightBv) < 0.01)
                    return;
                _rightBv = tmpValue;
                RightBufferWidth = Common.ConvertDistance(CalculateBufferWidth(_rightRnp, _rightBv), RoundType.ToNearest);

                NotifyPropertyChanged("RightBV");
            }
        }

        public string RightSelectedUpperUnit
        {
            get { return _rightSelectedUpperUnit; }
            set
            {
                if (RightUpperLimit > 0)
                {
                    UomDistanceVertical fromUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), _rightSelectedUpperUnit);
                    UomDistanceVertical toUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), value);

                    double val = DeltaHeightConverters.ConvertTo(_rightUpperLimit, fromUnit, toUnit);
                    RightUpperLimit = val;
                }

                _rightSelectedUpperUnit = value;
                NotifyPropertyChanged("RightSelectedUpperUnit");
            }
        }

        public string RightSelectedLowerUnit
        {
            get { return _rightSelectedLowerUnit; }
            set
            {
                if (RightLowerLimit> 0)
                {
                    UomDistanceVertical fromUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), _rightSelectedLowerUnit);
                    UomDistanceVertical toUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), value);

                    double val = DeltaHeightConverters.ConvertTo(_rightLowerLimit, fromUnit, toUnit);
                    RightLowerLimit = val;
                }


                _rightSelectedLowerUnit = value;
                NotifyPropertyChanged("RightSelectedLowerUnit");
            }
        }

        public double RightUpperLimit
        {
            get { return Math.Round(_rightUpperLimit); }
            set
            {
                _rightUpperLimit = value;
                NotifyPropertyChanged("RightUpperLimit");
            }
        }

        public double RightLowerLimit
        {
            get { return Math.Round(_rightLowerLimit); }
            set
            {
                _rightLowerLimit = value;
                NotifyPropertyChanged("RightLowerLimit");
            }
        }


        public string IntersectedAirspaceCount
        {
            get { return "Airspace Count : " + IntersectedItemList.Count; }
        }

        public string AirspaceCount
        {
            get { return "Airspace Count : " + AirspaceList.Count; }

        }

        public bool CreateReportIsEnabled
        {
            get { return _createReportIsEnabled; }
            set
            {
                _createReportIsEnabled = value;
                NotifyPropertyChanged("CreateReportIsEnabled");
            }
        }

        public bool IntersectBufferIsEnabled
        {
            get { return _intersectBufferIsEnabled; }
            set
            {
                _intersectBufferIsEnabled = value;
                NotifyPropertyChanged("IntersectBufferIsEnabled");
            }
        }

        public bool TwoDIntersect { get; set; }

        public string ResultMin
        {
            get { return _resultMin; }
            set
            {
                _resultMin = value;
                NotifyPropertyChanged("ResultMin");
            }
        }

        public string ResultMax
        {
            get { return _resultMax; }
            set
            {
                _resultMax = value;
                NotifyPropertyChanged("ResultMax");
            }
        }

        #endregion

        #region :>Methods

        private List<ReportItem> GetReportList()
        {
            List<ReportItem> result = new List<ReportItem>();
            foreach (var intersectItem in IntersectedItemList)
            {
                ReportItem item =new ReportItem();
                item.IntersectObject = intersectItem.Name;
                item.Layer = intersectItem.Layer;
                if (SelectedLayerType=="Airspace")
                    item.Type = intersectItem.IntersectedAirspace.Type.ToString();
                else
                    item.Type = intersectItem.IntersectedRoute.Type.ToString();
                item.Intersect = intersectItem.IntersectedIn3D;
                item.BufferWidth = RightBufferWidth + DistanceUnit;
                if (intersectItem.LowerLimit != null && intersectItem.UpperLimit != null)
                    item.Levels = intersectItem.LowerLimit.Value + intersectItem.LowerLimit.Uom.ToString() + "-" +
                                  intersectItem.UpperLimit.Value + intersectItem.UpperLimit.Uom.ToString();

                if (intersectItem.IntersectedIn3D)
                {
                    item.IntersectLevels = intersectItem.MinIntersectValue + LeftSelectedLowerUnit + "-" +
                                           intersectItem.MaxIntersectValue + LeftSelectedUpperUnit;
                }

                result.Add(item);
            }
            return result;
        }

        private void report_onClick(object obj)
        {
            try
            {
                var reportList = GetReportList();

                var report = new DoddleReport.Report(reportList.ToReportSource());
              //  report.RenderingRow += new EventHandler<ReportRowEventArgs>(report_RenderingRow);

                report.TextFields.Title = "Delta 3D Intersection Reports";
                report.TextFields.SubTitle = "";
                report.TextFields.Footer = "Copyright 2016 &copy; R.I.S.K Company";
                string deltaVersion =Assembly.GetExecutingAssembly().GetName().Version.ToString();

                string objectName =SelectedAirspace.Designator;
                string objectType = SelectedAirspace.Type.ToString();
                string limits = LeftLowerLimit + LeftSelectedLowerUnit + "-" + LeftUpperLimit +
                                LeftSelectedUpperUnit;
                string bufferWidth = LeftBufferWidth + DistanceUnit;



                report.TextFields.Header = string.Format(@"
                Report Generated: {0}
                DELTA version : {1}
                Object Name : {2}
                Type: {3}
                Limits: {4}
                Buffer width: {5}
                ", DateTime.Now, deltaVersion, objectName,objectType,
                    limits, bufferWidth
                    );

                // Customize the data fields

                //report.DataFields["Elevation"].HeaderText = "Elevation (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["Penetrate"].HeaderText = "Penetrate (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["Plane"].HeaderText = "Equation (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["VsType"].HeaderText = "Type";
                //report.DataFields["VerticalAccuracy"].HeaderText = "Vertical Accuracy (m)";
                //report.DataFields["HorizontalAccuracy"].HeaderText = "Horizontal Accuracy (m)";

              
                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "3DIntersection Report"; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Title = "Save Delta 3D Intersection Report";
                dlg.Filter = "Html documents|*.htm" +
                             "|Pdf document|*.pdf" +
                             "|Excel Worksheets|*.xls";
                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    System.IO.Stream stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.OpenOrCreate);
                    if (dlg.FilterIndex == 1)
                    {
                        var writer = new HtmlReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    else if (dlg.FilterIndex == 2)
                    {
                        var writer = new DoddleReport.iTextSharp.PdfReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    else if (dlg.FilterIndex == 3)
                    {
                        var writer = new ExcelReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    MessageBox.Show("The document was saved successfully!", "Delta", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                  //  Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured while saving document!", "Delta", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void FindIntersection_click(object obj)
        {
            CurrCursor = System.Windows.Input.Cursors.Wait;
            if (SelectedLayerType == "Airspace")
                FindIntersectWithAirspace();
            else
                FindIntersectWithRoute();

            CurrCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void FindIntersectWithAirspace()
        {
            IntersectedItemList.Clear();

            if (_selectedAirspaceGeo == null || _selectedAirspaceGeo.IsEmpty) return;

            var airspaceBuffer = _selectedAirspaceGeo;
            if (_leftBufferWidth > 0.1)
            {
                var tmpBuffer = GlobalParams.GeometryOperators.Buffer(_selectedAirspaceGeo, _leftBufferWidth);
                if (tmpBuffer != null)
                    airspaceBuffer = tmpBuffer as MultiPolygon;
            }

            UomDistanceVertical leftLowerUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), _leftSelectedLowerUnit);
            double leftLowerInM = DeltaHeightConverters.FromDistanceVerticalM(leftLowerUnit, _leftLowerLimit);

            UomDistanceVertical leftUpperUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), _leftSelectedUpperUnit);
            double leftUpperInM = DeltaHeightConverters.FromDistanceVerticalM(leftUpperUnit, _leftUpperLimit);


            foreach (var airspace in AirspaceList)
            {
                if (airspace.Designator == _selectedAirspace.Designator)
                    continue;

                bool is3DIntersected = false;

                double min = 0, max = 0;
                double rightLowerInM = Aran.Converters.ConverterToSI.Convert(airspace.GetLowerLimit(), 0);
                double rightUpperInM = Aran.Converters.ConverterToSI.Convert(airspace.GetUpperLimit(), 0);

                is3DIntersected = Functions.IsOverLapping(leftLowerInM, leftUpperInM, rightLowerInM, rightUpperInM,
                    ref min, ref max);

                if (TwoDIntersect || is3DIntersected)
                {
                    var tmpAirspaceGeo = airspace.GetGeom();
                    if (tmpAirspaceGeo == null || tmpAirspaceGeo.IsEmpty) continue;
                    var tmpBuffer = GlobalParams.GeometryOperators.Buffer(tmpAirspaceGeo, _rightBufferWidth);
                    if (tmpBuffer != null)
                    {
                        if (!GlobalParams.GeometryOperators.Disjoint(airspaceBuffer, tmpBuffer))
                        {
                            ThreeDIntersectionModel airspaceModel = new ThreeDIntersectionModel();
                            airspaceModel.Name = airspace.Name;
                            airspaceModel.IntersectedAirspace = airspace;
                            airspaceModel.AirspaceGeo = (Aran.Geometries.MultiPolygon) tmpAirspaceGeo;
                            airspaceModel.BufferGeo = (Aran.Geometries.MultiPolygon) tmpBuffer;
                            var intersectGeo = GlobalParams.GeometryOperators.Intersect(airspaceBuffer,
                                tmpBuffer);
                            airspaceModel.IntersectGeo = intersectGeo;
                            airspaceModel.IntersectedIn3D = is3DIntersected;
                            airspaceModel.LowerLimit = airspace.GetLowerLimit();
                            airspaceModel.UpperLimit = airspace.GetUpperLimit();
                            if (is3DIntersected)
                            {
                                airspaceModel.MinIntersectUnit = LeftSelectedLowerUnit;
                                airspaceModel.MinIntersectValue = Math.Round(DeltaHeightConverters.ToDistanceVertical(
                                    leftLowerUnit, min));

                                airspaceModel.MaxIntersectUnit = LeftSelectedUpperUnit;
                                airspaceModel.MaxIntersectValue = Math.Round(DeltaHeightConverters.ToDistanceVertical(
                                    leftUpperUnit, max));
                            }
                            IntersectedItemList.Add(airspaceModel);
                        }
                    }
                }

            }

            if (IntersectedItemList.Count > 0)
            {
                CreateReportIsEnabled = true;
                IntersectBufferIsEnabled = false;
                SelectedIntersectedItem = IntersectedItemList[0];
            }

            NotifyPropertyChanged("IntersectedAirspaceCount");
        }

        private void FindIntersectWithRoute()
        {
            IntersectedItemList.Clear();

            if (_selectedAirspaceGeo == null || _selectedAirspaceGeo.IsEmpty) return;

            var airspaceBuffer = _selectedAirspaceGeo;
            if (_leftBufferWidth > 0.1)
            {
                var tmpBuffer = GlobalParams.GeometryOperators.Buffer(_selectedAirspaceGeo, _leftBufferWidth);
                if (tmpBuffer != null)
                    airspaceBuffer = tmpBuffer as MultiPolygon;
            }

            UomDistanceVertical leftLowerUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), _leftSelectedLowerUnit);
            double leftLowerInM = DeltaHeightConverters.FromDistanceVerticalM(leftLowerUnit, _leftLowerLimit);

            UomDistanceVertical leftUpperUnit = (UomDistanceVertical)Enum.Parse(typeof(UomDistanceVertical), _leftSelectedUpperUnit);
            double leftUpperInM = DeltaHeightConverters.FromDistanceVerticalM(leftUpperUnit, _leftUpperLimit);


            foreach (var route in RouteList)
            {
                bool is3DIntersected = false;

                double min = 0, max = 0;
                double rightLowerInM = Aran.Converters.ConverterToSI.Convert(route.GetLowerLimit(), 0);
                double rightUpperInM = Aran.Converters.ConverterToSI.Convert(route.GetUpperLimit(), 0);

                is3DIntersected = Functions.IsOverLapping(leftLowerInM, leftUpperInM, rightLowerInM, rightUpperInM,
                    ref min, ref max);

                if (TwoDIntersect || is3DIntersected)
                {
                    var tmpRouteGeo = route.GetGeom();
                    if (tmpRouteGeo == null || tmpRouteGeo.IsEmpty) continue;
                    var tmpBuffer = GlobalParams.GeometryOperators.Buffer(tmpRouteGeo, _rightBufferWidth);
                    if (tmpBuffer != null)
                    {
                        if (!GlobalParams.GeometryOperators.Disjoint(airspaceBuffer, tmpBuffer))
                        {
                            ThreeDIntersectionModel airspaceModel = new ThreeDIntersectionModel();
                            airspaceModel.Name = route.Name;
                            airspaceModel.IntersectedRoute = route;
                            airspaceModel.RouteGeo = (Aran.Geometries.MultiLineString)tmpRouteGeo;
                            airspaceModel.BufferGeo = (Aran.Geometries.MultiPolygon)tmpBuffer;
                            var intersectGeo = GlobalParams.GeometryOperators.Intersect(airspaceBuffer,
                                tmpBuffer);
                            airspaceModel.IntersectGeo = intersectGeo;
                            airspaceModel.IntersectedIn3D = is3DIntersected;
                            airspaceModel.LowerLimit = route.GetLowerLimit();
                            airspaceModel.UpperLimit = route.GetUpperLimit();
                            if (is3DIntersected)
                            {
                                airspaceModel.MinIntersectUnit = LeftSelectedLowerUnit;
                                airspaceModel.MinIntersectValue = Math.Round(DeltaHeightConverters.ToDistanceVertical(
                                    leftLowerUnit, min));

                                airspaceModel.MaxIntersectUnit = LeftSelectedUpperUnit;
                                airspaceModel.MaxIntersectValue = Math.Round(DeltaHeightConverters.ToDistanceVertical(
                                    leftUpperUnit, max));
                            }
                            IntersectedItemList.Add(airspaceModel);
                        }
                    }
                }

            }

            if (IntersectedItemList.Count > 0)
            {
                CreateReportIsEnabled = true;
                IntersectBufferIsEnabled = false;
                SelectedIntersectedItem = IntersectedItemList[0];
            }

            NotifyPropertyChanged("IntersectedAirspaceCount");
        }

        private void close_click(object obj)
        {
            Close();
        }

        private void ResetIntersectResult()
        {
            ClearRightItem();
            IntersectedItemList.Clear();
            RightLowerLimit = 0;
            RightUpperLimit = 0;
            CreateReportIsEnabled = false;
            IntersectBufferIsEnabled = true;
            ResultMin = "";
            ResultMax = "";
            NotifyPropertyChanged("IntersectedAirspaceCount");
            
        }

        private double CalculateBufferWidth(int rnp, double bv)
        {
            return 2 * 1852 * rnp + bv;
        }

        private void DrawLeftAirspace()
        {
            try
            {
                ClearLeftAirspace();

                _selectedAirspaceGeo = new Aran.Geometries.MultiPolygon();
                if (SelectedAirspace != null)
                {
                    _selectedAirspaceGeo = (Aran.Geometries.MultiPolygon) _selectedAirspace.GetGeom();
                    if (!_selectedAirspaceGeo.IsEmpty)
                        _airspaceHandle = GlobalParams.UI.DrawDefaultMultiPolygon(_selectedAirspaceGeo);
                }
            }
            catch (Exception e)
            {

            }
        }

        private void DrawLeftBuffer()
        {
            ClearLeftBuffer();
            if (_selectedAirspaceGeo != null && !_selectedAirspaceGeo.IsEmpty)
            {
               var buffer = GlobalParams.GeometryOperators.Buffer(_selectedAirspaceGeo, _leftBufferWidth);
                if (buffer != null && !buffer.IsEmpty)
                {
                    int color = Aran.PANDA.Common.ARANFunctions.RGB(0, 40, 200);
                    if (buffer.Type== GeometryType.MultiPolygon)
                        _airspaceBufferHandle = GlobalParams.UI.DrawMultiPolygon(buffer as MultiPolygon,color,eFillStyle.sfsBackwardDiagonal,true,false);
                    else if (buffer.Type==GeometryType.Polygon)
                    {
                        var mltPolygon = new MultiPolygon {buffer as Aran.Geometries.Polygon};
                        _airspaceBufferHandle = GlobalParams.UI.DrawMultiPolygon(mltPolygon, color,
                            eFillStyle.sfsBackwardDiagonal, true, false);
                    }
                }
            }
        }

        private void DrawRightItem()
        {
            ClearRightItem();

            if (_selectedLayerType == "Airspace")
            {
                _selectedIntersectedItemGeo = new Aran.Geometries.MultiPolygon();
                if (_selectedIntersectedAirsapce.AirspaceGeo != null && !_selectedIntersectedAirsapce.AirspaceGeo.IsEmpty)
                {
                    int rgb =Aran.PANDA.Common.ARANFunctions.RGB(140,255,255);
                    _intersectedItemHandle = GlobalParams.UI.DrawMultiPolygon(_selectedIntersectedAirsapce.AirspaceGeo,rgb,eFillStyle.sfsNull);
                }
            }
            else 
            {
                var routeGeo = SelectedIntersectedItem.RouteGeo;
                if (routeGeo != null)
                {
                    _intersectedItemHandle = GlobalParams.UI.DrawMultiLineStringPrj(routeGeo, GlobalParams.Settings.SymbolModel.LineCourseSymbol);
                }
            
            }

            if (_selectedIntersectedAirsapce.IntersectGeo != null && !_selectedIntersectedAirsapce.IntersectGeo.IsEmpty)
            {
                if (_selectedIntersectedAirsapce.IntersectGeo.Type == GeometryType.MultiPolygon)
                {
                    int color = Aran.PANDA.Common.ARANFunctions.RGB(100, 40, 200);
                    _intersectGeoHandle = GlobalParams.UI.DrawMultiPolygon(_selectedIntersectedAirsapce.IntersectGeo as Aran.Geometries.MultiPolygon, color,
                        eFillStyle.sfsForwardDiagonal, true, false);
                }
            }
        }

        private void DrawRightBuffer()
        {
            ClearRightBuffer();
            if (_selectedIntersectedAirsapce!=null &&_selectedIntersectedAirsapce.BufferGeo != null && !_selectedIntersectedAirsapce.BufferGeo.IsEmpty)
            {
                int color = Aran.PANDA.Common.ARANFunctions.RGB(0, 40, 200);
                _intersectedBufferHandle = GlobalParams.UI.DrawMultiPolygon(_selectedIntersectedAirsapce.BufferGeo, color,
                    eFillStyle.sfsBackwardDiagonal, true, false);

            }
        }

        private void ClearLeftAirspace()
        {
            GlobalParams.UI.SafeDeleteGraphic(_airspaceBufferHandle);
            GlobalParams.UI.SafeDeleteGraphic(_airspaceHandle);
        }

        private void ClearLeftBuffer()
        {
            GlobalParams.UI.SafeDeleteGraphic(_airspaceBufferHandle);
        }

        private void ClearRightItem()
        {
            ClearRightBuffer();
            GlobalParams.UI.SafeDeleteGraphic(_intersectGeoHandle);
            GlobalParams.UI.SafeDeleteGraphic(_intersectedItemHandle);
        }

        private void ClearRightBuffer()
        {
            GlobalParams.UI.SafeDeleteGraphic(_intersectedBufferHandle);
        }


        private void InitializeAirspaceExtensions()
        {
            if (AirspaceList == null) return;

            foreach (var airspace in AirspaceList)
            {
                var tmpAirspaceGeo = new MultiPolygon();
                ValDistanceVertical lowerLimit = null, upperLimit = null;
                foreach (var airspaceGeometryComponent in airspace.GeometryComponent)
                {
                    if (airspaceGeometryComponent.TheAirspaceVolume != null &&
                        airspaceGeometryComponent.TheAirspaceVolume.HorizontalProjection != null)
                    {
                        var prjGeo =
                            GlobalParams.SpatialRefOperation.ToPrj(
                                airspaceGeometryComponent.TheAirspaceVolume.HorizontalProjection.Geo);
                        if (prjGeo != null && !prjGeo.IsEmpty)
                        {
                            var tmpConverGeo =                                
                                    GlobalParams.GeometryOperators.UnionGeometry(tmpAirspaceGeo, prjGeo);
                            if (tmpConverGeo == null)
                                continue;

                            if (tmpConverGeo.Type == GeometryType.MultiPolygon)
                                tmpAirspaceGeo = (Aran.Geometries.MultiPolygon)tmpConverGeo;
                            else if (tmpConverGeo.Type == GeometryType.Polygon)
                                tmpAirspaceGeo.Add((Aran.Geometries.Polygon)tmpConverGeo);
                        }
                        lowerLimit = airspaceGeometryComponent.TheAirspaceVolume.LowerLimit;
                        upperLimit = airspaceGeometryComponent.TheAirspaceVolume.UpperLimit;
                    }
                }
                if (tmpAirspaceGeo != null)
                    airspace.SetGeom(tmpAirspaceGeo);
                if (lowerLimit != null)
                    airspace.SetLowerLimit(lowerLimit);
                if (upperLimit != null)
                    airspace.SetUpperLimit(upperLimit);
            }
        }

        private void InitializeRouteExtensions()
        {
            if (RouteList == null) return;

            foreach (var route in RouteList)
            {
                try
                {
                    var tmpAirspaceGeo = new MultiLineString();
                    ValDistanceVertical lowerLimit = null, upperLimit = null;

                    var segmentList = GlobalParams.Database.GetRouteSegmentList(route.Identifier);

                    if (segmentList.Count == 0)
                    {
                        if (GlobalParams.DesigningAreaReader != null)
                        {
                            List<DesigningSegment> designingSegmentList = GlobalParams.DesigningAreaReader.GetDesigningSegments(route.Name);
                            if (designingSegmentList != null)
                            {
                                foreach (var segment in designingSegmentList)
                                {
                                    var segmentGeo = GlobalParams.SpatialRefOperation.ToPrj(segment.Geo);
                                    if (segmentGeo != null && !segmentGeo.IsEmpty)
                                    {
                                        var tmpConverGeo =
                                                GlobalParams.GeometryOperators.UnionGeometry(tmpAirspaceGeo, segmentGeo);
                                        if (tmpConverGeo == null)
                                            continue;

                                        if (tmpConverGeo.Type == GeometryType.MultiLineString)
                                            tmpAirspaceGeo = (Aran.Geometries.MultiLineString)tmpConverGeo;
                                        else if (tmpConverGeo.Type == GeometryType.LineString)
                                            tmpAirspaceGeo.Add((Aran.Geometries.LineString)tmpConverGeo);
                                    }
                                    lowerLimit = new ValDistanceVertical();
                                    upperLimit = new ValDistanceVertical();
                                }
                                if (tmpAirspaceGeo != null)
                                    route.SetGeom(tmpAirspaceGeo);
                                if (lowerLimit != null)
                                    route.SetLowerLimit(lowerLimit);
                                if (upperLimit != null)
                                    route.SetUpperLimit(upperLimit);
                            }
                        }
                    }
                    else
                    {
                        foreach (var segment in segmentList)
                        {
                            if (segment.CurveExtent == null) continue;

                            var segmentGeo = GlobalParams.SpatialRefOperation.ToPrj(segment.CurveExtent.Geo);
                            if (segmentGeo != null && !segmentGeo.IsEmpty)
                            {
                                var tmpConverGeo =
                                        GlobalParams.GeometryOperators.UnionGeometry(tmpAirspaceGeo, segmentGeo);
                                if (tmpConverGeo == null)
                                    continue;

                                if (tmpConverGeo.Type == GeometryType.MultiLineString)
                                    tmpAirspaceGeo = (Aran.Geometries.MultiLineString)tmpConverGeo;
                                else if (tmpConverGeo.Type == GeometryType.LineString)
                                    tmpAirspaceGeo.Add((Aran.Geometries.LineString)tmpConverGeo);
                            }
                            lowerLimit = segment.LowerLimit;
                            upperLimit = segment.UpperLimit;
                        }

                        if (tmpAirspaceGeo != null)
                            route.SetGeom(tmpAirspaceGeo);
                        if (lowerLimit != null)
                            route.SetLowerLimit(lowerLimit);
                        if (upperLimit != null)
                            route.SetUpperLimit(upperLimit);
                    }
                }
                catch (Exception e)
                {
                    Messages.Error(e.Message + " " + route.Name);
                }
            }
        }

        public void Clear()
        {
            ClearLeftAirspace();
            ClearRightItem();
        }

        #endregion
    }
}
