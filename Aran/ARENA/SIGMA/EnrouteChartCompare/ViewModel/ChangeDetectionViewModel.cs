using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Aran.PANDA.Common;
using ChartCompare;
using DoddleReport;
using DoddleReport.Writers;
using EnrouteChartCompare.Helper;
using EnrouteChartCompare.Model;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDM;

namespace EnrouteChartCompare.ViewModel
{
    public class ChangeDetectionViewModel : ViewModelBase
    {
        private DetectFeatureType _selectedDetectFeatureType;
        private DetailedItem _selectedFeature;
        private FilterType _filterType;
        private int _featureCount;
        private readonly IList<int> _selectedItemhandle;
        private RelayCommand _saveReportCommand;
        private bool _allSelected;
        private List<LogItem> _logs;

        public ChangeDetectionViewModel(List<DetectFeatureType> detectFeatureTypeList,
            List<LogItem> logs)
        {
            FeatureTypeList = detectFeatureTypeList;
            _logs = logs;
            FeatureItemList = new ObservableCollection<DetailedItem>();
            FieldLogList = new ObservableCollection<FieldLog>();

            if (FeatureTypeList.Count > 0)
                SelectedDetectFeatureType = FeatureTypeList[0];
            _filterType = FilterType.All;

            GlobalParams.Graphics = new Graphics(GlobalParams.HookHelper.ActiveView);

            _selectedItemhandle = new List<int>();
        }


        public RelayCommand SaveReportCommand
        {
            get { return _saveReportCommand ?? (_saveReportCommand = new RelayCommand(() => 
            {
                try
                {
                    var reportItemList = new List<ReportItem>();
                    foreach (var featType in FeatureTypeList)
                    {
                        foreach (var feat in featType.FeatureList)
                        {
                            //if (!feat.IsChecked)
                            //    continue;
                            var reportItem = new ReportItem
                            {
                                Feature = featType.FeatName,
                                Status = feat.ChangedStatus.ToString(),
                                Name = feat.Name,
                                IsChecked = feat.IsChecked
                            };
                            if (feat.FieldLogList.Count == 0)
                                reportItemList.Add(reportItem);
                            else
                            {
                                foreach (var featField in feat.FieldLogList)
                                {
                                    reportItem.FieldName = featField.FieldName;
                                    reportItem.OldValue = featField.OldValueText;
                                    reportItem.NewValue = featField.NewValueText;
                                    reportItem.Description = featField.ChangeText;
                                    reportItemList.Add(reportItem);
                                }
                            }
                        }
                    }

                    var report = new Report(reportItemList.ToReportSource());
                    report.RenderingRow += new EventHandler<ReportRowEventArgs>(report_RenderingRow);

                    report.TextFields.Title = "Enroute Chart Update Report";
                    report.TextFields.Footer = "Copyright 2018 &copy; R.I.S.K Company";

                    report.TextFields.Header = $@"Report Generated: {DateTime.Now}";
                    report.TextFields.Header += Environment.NewLine + Environment.NewLine;
                    for (int i = 0; i < _logs.Count-2; i++)
                    {
                        report.TextFields.Header += _logs[i].Text;
                    }
                    report.TextFields.Header += Environment.NewLine;
                    report.TextFields.Header += "Please, see below for details.";
                    report.TextFields.Header += Environment.NewLine;

                    foreach (var dataField in report.DataFields)
                    {
                        dataField.HeaderStyle.Underline = true;
                        dataField.HeaderStyle.Bold = true;
                    }
                    // Customize the data fields

                    var dlg = new Microsoft.Win32.SaveFileDialog
                    {
                        FileName = "EnrouteUpdateList",
                        DefaultExt = ".text",
                        Title = "Save Enroute Update Report",
                        Filter = "Html documents|*.html"
                    };
                   
                    // Show save file dialog box
                    bool? result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result == true)
                    {
                        System.IO.Stream stream =
                            new System.IO.FileStream(dlg.FileName, System.IO.FileMode.OpenOrCreate);
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

                        MessageBox.Show("The document was saved successfully!", "Enroute Chart Update",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error occured while saving document!", "Enroute Chart Update", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            })); }
        }

        public List<DetectFeatureType> FeatureTypeList { get; set; }

        public ObservableCollection<DetailedItem> FeatureItemList { get; set; }

        public ObservableCollection<FieldLog> FieldLogList { get; set; }

        public DetectFeatureType SelectedDetectFeatureType
        {
            get => _selectedDetectFeatureType;
            set
            {
                Set(ref _selectedDetectFeatureType, value);
                UpdateFeatureList();
            }
        }

        public DetailedItem SelectedFeature
        {
            get => _selectedFeature;
            set
            {
                Set(ref _selectedFeature ,value);

                FieldLogList.Clear();
                if (_selectedFeature != null)
                {
                    foreach (var fieldLog in _selectedFeature.FieldLogList)
                        FieldLogList.Add(fieldLog);

                    Draw();
                }
            }
        }

        public FieldLog SelectedFieldLog { get; set; }

        public int FeatureCount
        {
            get => _featureCount;
            set => Set(ref _featureCount ,value);
        }

        public bool AllSelected
        {
            get => _allSelected;
            set
            {
                Set(ref _allSelected, value);
                foreach (var item in FeatureItemList)
                {
                    item.IsChecked = value;
                }
            }
        }

        public bool AllFeatureIsChecked
        {
            get => _filterType == FilterType.All;
            set
            {
                if (value)
                {
                    _filterType = FilterType.All;
                    UpdateFeatureList();
                }
                //NotifyPropertyChanged("FeatureItemList");
            }
        }

        public bool NewFeatureIsChecked
        {
            get => _filterType == FilterType.New;
            set
            {
                if (value)
                {
                    _filterType = FilterType.New;
                    UpdateFeatureList();
                }
                //NotifyPropertyChanged("NewFeatureIsChecked");
            }
        }

        public bool DeletedFeatureIsChecked
        {
            get => _filterType == FilterType.Deleted;
            set
            {
                if (value)
                {
                    _filterType = FilterType.Deleted;
                    UpdateFeatureList();
                }

                //NotifyPropertyChanged("DeletedFeatureIsChecked");
            }
        }

        public bool ChangedFeatureIsChecked
        {
            get => _filterType == FilterType.Changed;
            set
            {
                if (value)
                {
                    _filterType = FilterType.Changed;
                    UpdateFeatureList();
                }

                //NotifyPropertyChanged("ChangedFeatureIsChecked");
            }
        }

        public bool MissedFeatureIsChecked
        {
            get => _filterType == FilterType.Missed;
            set
            {
                if (value)
                {
                    _filterType = FilterType.Missed;
                    UpdateFeatureList();
                }

                //NotifyPropertyChanged("MissedFeatureIsChecked");
            }
        }

        private void report_RenderingRow(object sender, ReportRowEventArgs e)
        {
           
        }

        private void UpdateFeatureList()
        {
            FeatureItemList.Clear();
            if (_filterType == FilterType.All)
                SelectedDetectFeatureType.FeatureList.ForEach(item => FeatureItemList.Add(item));
            else if (_filterType == FilterType.Changed)
            {
                SelectedDetectFeatureType.FeatureList.ForEach(item =>
                {
                    if (item.ChangedStatus == Status.Changed)
                        FeatureItemList.Add(item);
                });
            }
            else if (_filterType == FilterType.New)
            {
                SelectedDetectFeatureType.FeatureList.ForEach(item =>
                {
                    if (item.ChangedStatus == Status.New)
                        FeatureItemList.Add(item);
                });
            }

            else if (_filterType == FilterType.Deleted)
            {
                SelectedDetectFeatureType.FeatureList.ForEach(item =>
                {
                    if (item.ChangedStatus == Status.Deleted)
                        FeatureItemList.Add(item);
                });
            }
            else if (_filterType == FilterType.Missed)
            {
                SelectedDetectFeatureType.FeatureList.ForEach(item =>
                {
                    if (item.ChangedStatus == Status.Missing)
                        FeatureItemList.Add(item);
                });
            }

            FeatureCount = FeatureItemList.Count;
            SelectedFieldLog = (FeatureItemList.Count > 0 && FeatureItemList[0].FieldLogList.Count >0) ? FeatureItemList[0].FieldLogList[0] : null;
        }

        private void Draw()
        {
            Clear();
            if (_selectedFeature.Feature.PDM_Type == PDM_ENUM.Enroute)
            {
                var route = _selectedFeature.Feature as Enroute;

                foreach (var routeSegment in route.Routes)
                {
                    routeSegment.RebuildGeo2();
                    var geo = routeSegment.Geo;

                    IClone clone = geo as IClone;
                    var cl = clone.Clone();

                    var newGeo = cl as IPolyline;
                    var prjGeo = GlobalParams.SpatialOperation.ToProject(newGeo);
                    if (prjGeo != null && !prjGeo.IsEmpty)
                        _selectedItemhandle.Add(GlobalParams.Graphics.DrawMultiLineString((IPolyline) prjGeo,
                            ARANFunctions.RGB(255, 0, 0), esriSimpleLineStyle.esriSLSDash));
                }

            }
            else if (_selectedFeature.Feature.PDM_Type == PDM_ENUM.RouteSegment)
            {
                var routeSegment = (RouteSegment) _selectedFeature.Feature;
                routeSegment.RebuildGeo2();
                var geo = routeSegment.Geo;

                IClone clone = geo as IClone;
                var cl = clone.Clone();

                var newGeo = cl as IPolyline;
                var prjGeo = GlobalParams.SpatialOperation.ToProject(newGeo);
                if (prjGeo != null && !prjGeo.IsEmpty)
                    _selectedItemhandle.Add(GlobalParams.Graphics.DrawMultiLineString((IPolyline) prjGeo,
                        ARANFunctions.RGB(255, 0, 0), esriSimpleLineStyle.esriSLSDash));

            }
            else if (_selectedFeature.Feature.PDM_Type == PDM_ENUM.WayPoint)
            {
                var wayPoint = (WayPoint) _selectedFeature.Feature;
                wayPoint.RebuildGeo();
                var geo = wayPoint.Geo;

                IClone clone = geo as IClone;
                var cl = clone.Clone();

                var newGeo = cl as IPoint;
                var prjGeo = GlobalParams.SpatialOperation.ToProject(newGeo);
                if (prjGeo != null && !prjGeo.IsEmpty)
                    _selectedItemhandle.Add(GlobalParams.Graphics.DrawPointWithText((IPoint) prjGeo,
                        wayPoint.Designator, 12, ARANFunctions.RGB(255, 0, 0)));

            }
            else if (_selectedFeature.Feature.PDM_Type == PDM_ENUM.Airspace)
            {
                var airspace = _selectedFeature.Feature as Airspace;

                foreach (var airspaceVolume in airspace.AirspaceVolumeList)
                {
                    airspaceVolume.RebuildGeo2();

                    var geo = airspaceVolume.Geo;

                    if (geo != null)
                    {
                        IClone clone = geo as IClone;
                        var cl = clone.Clone();

                        var newGeo = cl as IPolygon;
                        var prjGeo = GlobalParams.SpatialOperation.ToProject(newGeo);
                        if (prjGeo != null && !prjGeo.IsEmpty)
                            _selectedItemhandle.Add(
                                GlobalParams.Graphics.DrawEsriDefaultMultiPolygon((IPolygon) prjGeo));
                    }
                }
            }
            else if (_selectedFeature.Feature.PDM_Type == PDM_ENUM.HoldingPattern)
            {

            }
            else if (_selectedFeature.Feature.PDM_Type == PDM_ENUM.NavaidSystem)
            {

            }
        }

        public void Clear()
        {
            foreach (var handle in _selectedItemhandle)
                GlobalParams.Graphics.SafeDeleteGraphic(handle);

            _selectedItemhandle.Clear();
        }
    }
}