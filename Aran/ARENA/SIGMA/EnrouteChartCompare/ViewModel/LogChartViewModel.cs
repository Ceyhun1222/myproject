using ChartCompare;
using EnrouteChartCompare.Helper;
using EnrouteChartCompare.Model;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Framework;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PDM;
using SigmaChart;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Interop;
using ChangeDetectionWindow = EnrouteChartCompare.View.ChangeDetectionWindow;

namespace EnrouteChartCompare.ViewModel
{
    public class LogChartViewModel : ViewModelBase
    {
        private readonly List<PDMObject> _newPdmObjectList;
        private readonly List<PDMObject> _oldPdmObjectList;
        private readonly List<DetectFeatureType> _detectFetureTypes;
        private readonly EnrouteComparer _comparer;
        private bool _detailsIsEnable;
        private RelayCommand _updateCommand;
        private RelayCommand _showDetailCommand;
        private readonly string _fileName;
        private int _sumNewCount, _sumChangedCount, _sumDeletedCount;

        public LogChartViewModel(List<PDMObject> oldPdmObjectsList, List<PDMObject> newPdmObjectList, string fileName,
            HookHelperClass hookHelper, CODE_ROUTE_SEGMENT_CODE_LVL routeLevel)
        {
            _oldPdmObjectList = oldPdmObjectsList;
            _newPdmObjectList = newPdmObjectList;
            _fileName = fileName;
            GlobalParams.HookHelper = hookHelper;
            Logs = new ObservableConcurrentCollection<LogItem>();
            _comparer = new EnrouteComparer(_newPdmObjectList, _oldPdmObjectList, routeLevel);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _detectFetureTypes = _comparer.FeatureTypeList;
            _sumNewCount = 0;
            _sumDeletedCount = 0;
            _sumChangedCount = 0;
            var generateLogs = DoWork();
            generateLogs.ContinueWith(t =>
                {
                    stopwatch.Stop();
                    Logs.TryAdd(new LogItem(string.Empty));
                    Logs.TryAdd(new LogItem($"New feature count : {_sumNewCount}"));
                    Logs.TryAdd(new LogItem($"Changed feature count : {_sumChangedCount}"));
                    Logs.TryAdd(new LogItem($"Deleted feature count : {_sumDeletedCount}"));
                    Logs.TryAdd(new LogItem($"Time Elapsed {stopwatch.Elapsed:hh\\:mm\\:ss}"));                    
                    Logs.TryAdd(new LogItem($"========== Comparison succeeded =========="));
                    IsFinishedChecking = true;
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);

            generateLogs.ContinueWith(t =>
                {
                    Logs.TryAdd(new LogItem(string.Empty));
                    Logs.TryAdd(new LogItem($"Time Elapsed {stopwatch.Elapsed}"));
                    Logs.TryAdd(new LogItem($"========== Comparison failed =========="));
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }

        public List<DetailedItem> ResultList => _comparer.ResultList;

        public RelayCommand ApplyCommand
        {
            get
            {
                return _updateCommand ?? (_updateCommand = new RelayCommand(() =>
                {
                    SigmaDataCash.UpdateState = 1;
                    SigmaDataCash.ChangeList = ResultList;
                    SigmaDataCash.newPdmList = _newPdmObjectList;
                    SigmaDataCash.oldPdmList = _oldPdmObjectList;
                    if (GlobalParams.HookHelper.Hook is IApplication app)
                    {
                        app.OpenDocument(_fileName);
                        DoUpdate = true;
                    }

                    Close?.Invoke();
                }));
            }
        }

        public bool DoUpdate { get; set; }

        public RelayCommand ShowDetailCommand
        {
            get
            {
                return _showDetailCommand ?? (_showDetailCommand = new RelayCommand(() =>
                {
                    var viewModel = new ChangeDetectionViewModel(_detectFetureTypes, Logs.ToList());
                    var window = new ChangeDetectionWindow()
                    {
                        DataContext = viewModel
                    };
                    window.Closed += (sender, e) =>
                    {
                        viewModel.Clear();
                    };
                    var app = GlobalParams.HookHelper.Hook as IApplication;
                    var parentHandle = new IntPtr(app.hWnd);
                    var helper = new WindowInteropHelper(window) {Owner = parentHandle};
                    window.ShowInTaskbar = false;
                    // hide from taskbar and alt-tab list
                    window.Show();
                }));
            }
        }

        //public void DoSynchronous()
        //{
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.WorkerReportsProgress = true;
        //    worker.DoWork += worker_DoWork;
        //    worker.ProgressChanged += worker_ProgressChanged;
        //    worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        //    worker.RunWorkerAsync(10000);
        //}

        //private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    MessageBox.Show("Loading is completed");
        //}

        //private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    var item = (e.UserState as LogItem);
        //    item.Text += Environment.NewLine;
        //    //Logs.Add(item);


        //    //LogText += 
        //    //var t = message + Environment.NewLine;
        //    //if (e.ProgressPercentage != 0 && t.Contains(":"))
        //    //    Logs.Add(new LogItem(t, true));
        //    //else
        //    //    Logs.Add(new LogItem(t));
        //}

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    var bWorker = sender as BackgroundWorker;

        //    var allFeaturesInChart = _oldPdmObjectList.Count;
        //    var allFeaturesInCurrentPdm = _newPdmObjectList.Count;

        //    bWorker.ReportProgress(0, new LogItem($"Feature count in current project : {allFeaturesInCurrentPdm}"));
        //    bWorker.ReportProgress(0,
        //        new LogItem($"Feature count in chart : {allFeaturesInChart}"));
        //    bWorker.ReportProgress(0, new LogItem(string.Empty));
        //    if (_detectFetureTypes != null)
        //    {
        //        int i = 0;
        //        foreach (var detectFeatureType in _detectFetureTypes)
        //        {
        //            i++;
        //            System.Threading.Thread.Sleep(1000);
        //            bWorker.ReportProgress(i, new LogItem($"{detectFeatureType.FeatName} is Loading .....  "));


        //            int newFeatCount = 0, missingFeatCount = 0, changedFeatureCount = 0;
        //            detectFeatureType.FeatureList.ForEach(featItem =>
        //            {
        //                if (featItem.ChangedStatus == Status.New)
        //                    newFeatCount++;
        //                else if (featItem.ChangedStatus == Status.Deleted)
        //                    missingFeatCount++;
        //                else if (featItem.ChangedStatus == Status.Changed)
        //                    changedFeatureCount++;
        //            }
        //                );

        //            int allObjectCountInCurPdm = 0,allObjectCountInChart = 0;
        //            int difference = 0;
        //            if (detectFeatureType.FeatureType == PDM_ENUM.RouteSegment)
        //            {
        //                var allObjectInCurPdm =
        //                    _newPdmObjectList.Where(pdmObject => pdmObject.PDM_Type == PDM_ENUM.Enroute).
        //                        Select(pdmObject => pdmObject as PDM.Enroute).
        //                        SelectMany(enrouteSeg => enrouteSeg.Routes).ToList();

        //                var allObjectInChart =
        //                    _oldPdmObjectList.Where(pdmObject => pdmObject.PDM_Type == PDM_ENUM.Enroute).
        //                        Select(pdmObject => pdmObject as PDM.Enroute).
        //                        SelectMany(enrouteSeg => enrouteSeg.Routes).ToList();

        //                allObjectCountInChart = allObjectInChart.Count;
        //                allObjectCountInCurPdm = allObjectInCurPdm.Count;

        //                difference = (allObjectCountInChart - missingFeatCount + newFeatCount) -
        //                                 allObjectCountInCurPdm;
        //                if (difference > 0)
        //                {
        //                    foreach (var pdmObject in allObjectInChart)
        //                    {
        //                        bool isHave = allObjectInCurPdm.Any(newPdmObject => pdmObject.ID == newPdmObject.ID);
        //                        if (!isHave)
        //                        {
        //                            var detailedItem = new DetailedItem
        //                            {
        //                                Feature = pdmObject,
        //                                ChangedStatus = Status.Missing
        //                            };
        //                            var routeSegment = pdmObject as RouteSegment;
        //                            var route = _oldPdmObjectList.FirstOrDefault(
        //                                pdm => pdm.PDM_Type == PDM_ENUM.Enroute && pdm.ID == routeSegment.ID_Route);
        //                            if (route != null)
        //                            {
        //                                detailedItem.Name = (route as Enroute).TxtDesig + " => " +
        //                                                    routeSegment.ToString();
        //                            }


        //                            detectFeatureType.FeatureList.Add(detailedItem);
        //                        }
        //                    }

        //                }
        //            }

        //            else
        //            {
        //                allObjectCountInChart=
        //                    _oldPdmObjectList.Count(pdmObject => pdmObject.PDM_Type == detectFeatureType.FeatureType);

        //                allObjectCountInCurPdm =
        //                    _newPdmObjectList.Count(pdmObject => pdmObject.PDM_Type == detectFeatureType.FeatureType);
        //            }

        //            bWorker.ReportProgress(i,
        //                new LogItem($"Feature count in current project: {allObjectCountInCurPdm}"));
        //            bWorker.ReportProgress(i, new LogItem($"Feature count in chart: {allObjectCountInChart}"));
        //            bWorker.ReportProgress(i,
        //                new LogItem($"New Feature count in current project : {newFeatCount}", newFeatCount > 0));
        //            bWorker.ReportProgress(i,
        //                new LogItem($"Deleted Feature count in current project: {missingFeatCount}",
        //                    missingFeatCount > 0));
        //            bWorker.ReportProgress(i,
        //                new LogItem($"Changed Feature count in current project: {changedFeatureCount}",
        //                    changedFeatureCount > 0));
        //            if (difference > 0)
        //                bWorker.ReportProgress(i,
        //                    new LogItem($"Missing feature Link count : {difference}", difference > 0));

        //            bWorker.ReportProgress(i, new LogItem(String.Empty));
        //        }
        //    }
        //}

        //public string LogText
        //{
        //    get => _logText;
        //    set => Set(ref _logText, value);
        //}

        public IProducerConsumerCollection<LogItem> Logs { get; set; }

        private async Task DoWork()
        {
            await Task.Factory.StartNew(() =>
            {
                var allFeaturesInChart = _oldPdmObjectList.Count;
                var allFeaturesInCurrentPdm = _newPdmObjectList.Count;

                Logs.TryAdd(new LogItem($"Feature count in current project : {allFeaturesInCurrentPdm}"));
                Logs.TryAdd(new LogItem($"Feature count in chart : {allFeaturesInChart}"));
                Logs.TryAdd(new LogItem(string.Empty));
                if (_detectFetureTypes != null)
                {
                    int i = 0;
                    foreach (var detectFeatureType in _detectFetureTypes)
                    {
                        i++;
                        System.Threading.Thread.Sleep(1000);
                        Logs.TryAdd(new LogItem($"{detectFeatureType.FeatName} .....  "));


                        int newFeatCount = 0, missingFeatCount = 0, changedFeatureCount = 0;
                        detectFeatureType.FeatureList.ForEach(featItem =>
                            {
                                if (featItem.ChangedStatus == Status.New)
                                    newFeatCount++;
                                else if (featItem.ChangedStatus == Status.Deleted)
                                    missingFeatCount++;
                                else if (featItem.ChangedStatus == Status.Changed)
                                    changedFeatureCount++;
                            }
                        );

                        int allObjectCountInCurPdm = 0, allObjectCountInChart = 0;
                        int difference = 0;
                        if (detectFeatureType.FeatureType == PDM_ENUM.RouteSegment)
                        {
                            var allObjectInCurPdm =
                                _newPdmObjectList.Where(pdmObject => pdmObject.PDM_Type == PDM_ENUM.Enroute)
                                    .Select(pdmObject => pdmObject as Enroute)
                                    .SelectMany(enrouteSeg => enrouteSeg.Routes).ToList();

                            var allObjectInChart =
                                _oldPdmObjectList.Where(pdmObject => pdmObject.PDM_Type == PDM_ENUM.Enroute)
                                    .Select(pdmObject => pdmObject as Enroute)
                                    .SelectMany(enrouteSeg => enrouteSeg.Routes).ToList();

                            allObjectCountInChart = allObjectInChart.Count;
                            allObjectCountInCurPdm = allObjectInCurPdm.Count;

                            difference = (allObjectCountInChart - missingFeatCount + newFeatCount) -
                                         allObjectCountInCurPdm;
                            if (difference > 0)
                            {
                                foreach (var pdmObject in allObjectInChart)
                                {
                                    bool isHave =
                                        allObjectInCurPdm.Any(newPdmObject => pdmObject.ID == newPdmObject.ID);
                                    if (!isHave)
                                    {
                                        var detailedItem = new DetailedItem
                                        {
                                            Feature = pdmObject,
                                            ChangedStatus = Status.Missing
                                        };
                                        var routeSegment = (RouteSegment) pdmObject;
                                        var route = _oldPdmObjectList.FirstOrDefault(
                                            pdm => pdm.PDM_Type == PDM_ENUM.Enroute && pdm.ID == routeSegment.ID_Route);
                                        if (route != null)
                                        {
                                            detailedItem.Name = ((Enroute)route).TxtDesig + " => " +
                                                                routeSegment.ToString();
                                        }
                                        detectFeatureType.FeatureList.Add(detailedItem);
                                    }
                                }

                            }
                        }

                        else
                        {
                            allObjectCountInChart =
                                _oldPdmObjectList.Count(
                                    pdmObject => pdmObject.PDM_Type == detectFeatureType.FeatureType);

                            allObjectCountInCurPdm =
                                _newPdmObjectList.Count(
                                    pdmObject => pdmObject.PDM_Type == detectFeatureType.FeatureType);
                        }

                        Logs.TryAdd(
                            new LogItem($"Feature count in current project: {allObjectCountInCurPdm}"));
                        Logs.TryAdd(new LogItem($"Feature count in chart: {allObjectCountInChart}"));
                        Logs.TryAdd(
                            new LogItem($"New Feature count in current project : {newFeatCount}", newFeatCount > 0));
                        Logs.TryAdd(
                            new LogItem($"Deleted Feature count in current project: {missingFeatCount}",
                                missingFeatCount > 0));
                        Logs.TryAdd(
                            new LogItem($"Changed Feature count in current project: {changedFeatureCount}",
                                changedFeatureCount > 0));

                        _sumNewCount += newFeatCount;
                        _sumChangedCount += changedFeatureCount;
                        _sumDeletedCount += missingFeatCount;
                        if (difference > 0)
                            Logs.TryAdd(
                                new LogItem($"Missing feature Link count : {difference}", difference > 0));

                        Logs.TryAdd(new LogItem(String.Empty));
                    }
                }
            });

        }

        public bool IsFinishedChecking
        {
            get => _detailsIsEnable;
            set => Set(ref _detailsIsEnable, value);
        }

        public event Action Close;
    }
}