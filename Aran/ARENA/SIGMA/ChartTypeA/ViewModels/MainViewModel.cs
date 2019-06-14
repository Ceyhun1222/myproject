using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.PANDA.Common;
using ChartTypeA.Models;
using ChartTypeA.View;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using PDM;
using SigmaChart;
using Application = System.Windows.Forms.Application;
using Cursor = System.Windows.Input.Cursor;
using IActiveViewEvents_ItemReorderedEventHandler = ESRI.ArcGIS.Carto.IActiveViewEvents_ItemReorderedEventHandler;
using MessageBox = System.Windows.MessageBox;
using EsriWorkEnvironment;
using DoddleReport;
using DoddleReport.Writers;
using System.Runtime.ExceptionServices;
using ChartTypeA.Utils;

namespace ChartTypeA.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        private Models.ViewType _viewType;
        private bool _backCommandIsEnabled;
        private string _nextContent;
        private ViewModel _currentViewModel;
        private bool _nextCommandIsEnabled;
        private bool _isOpened;
        private List<RwyDirWrapper> _sortedRwyList;
        private List<RwyDirWrapper> _selectedRwyDirList;
        private Cursor _currCursor;
        private VerticalObstacleCreater _verticalObstacleCreater;
        private double _offset;

        public MainViewModel()
        {
            try
            {
                NextCommand = new RelayCommand(new Action<object>(Next_Click));
                BackCommand = new RelayCommand(new Action<object>(Back_Click));
                CurrentViewModel = new SelectRunwayViewModel(); //SelectRunwayViewModel();
                (CurrentViewModel as SelectRunwayViewModel).CanGoNextEvent += MainViewModel_CanGoNextEvent;
                NextCommandIsEnabled = true;
                _viewType = Models.ViewType.SelectRunway;

                NextCommandIsEnabled = false;
                PageList = new List<ViewModel>();
                PageList.Add(CurrentViewModel);

                NextContent = "Next";
                GlobalParams.GrCreater = new GridCreater();

                CurCursor = System.Windows.Input.Cursors.Arrow;

                _offset = 0.0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Sigma Chart TypeA",MessageBoxButton.OK,MessageBoxImage.Error);
                throw;
            }
        }

        private void MainViewModel_CanGoNextEvent(object sender, EventArgs e)
        {
            var canGo = sender is bool && (bool) sender;
            if (canGo)
                NextCommandIsEnabled = true;
            else
                NextCommandIsEnabled = false;
        }

        public List<ViewModel> PageList { get; set; }

        public new string Header => CurrentViewModel.Header;

        public string NextContent
        {
            get { return _nextContent; }
            set
            {
                _nextContent = value;
                NotifyPropertyChanged("NextContent");
            }
        }

        public RelayCommand NextCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public RelayCommand ReportCommand { get; set; }

        public ViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                NotifyPropertyChanged("Header");
                NotifyPropertyChanged("CurrentViewModel");
            }
        }

        public bool NextCommandIsEnabled
        {
            get { return _nextCommandIsEnabled; }
            set
            {
                _nextCommandIsEnabled = value;
                NotifyPropertyChanged("NextCommandIsEnabled");
            }
        }

        public bool BackCommandIsEnabled
        {
            get { return _backCommandIsEnabled; }
            set
            {
                _backCommandIsEnabled = value;
                NotifyPropertyChanged("BackCommandIsEnabled");
            }
        }

        public System.Windows.Input.Cursor CurCursor
        {
            get { return _currCursor; }
            set
            {
                _currCursor = value;
                NotifyPropertyChanged("CurCursor");
            }
        }

        private void Back_Click(object obj)
        {
            NextCommandIsEnabled = true;
            NextContent = "Next";

            if (CurrentViewModel != null)
                CurrentViewModel.Clear();

            if (_viewType == Models.ViewType.ChartParametr)
            {
                var vModel = PageList.FirstOrDefault(page => page is ConstructSurfaceViewModel);
                if (vModel != null)
                {
                    vModel.Clear();
                    PageList.Remove(CurrentViewModel);
                    CurrentViewModel = vModel;
                }
                _viewType = Models.ViewType.ConstructSurface;
            }
            else if (_viewType == Models.ViewType.ConstructSurface)
            {

                var vModel = PageList.FirstOrDefault(page => page is SelectRunwayViewModel);
                if (vModel != null)
                {
                    PageList.Remove(CurrentViewModel);
                    CurrentViewModel = vModel;
                    _viewType = Models.ViewType.SelectRunway;
                    BackCommandIsEnabled = false;
                }
            }
        }

        private void Next_Click(object obj)
        {
            try
            {
                CurCursor = System.Windows.Input.Cursors.Wait;
                BackCommandIsEnabled = true;

                var selectRunwayVModel = (SelectRunwayViewModel)PageList.First(page => page is SelectRunwayViewModel);
                GlobalParams.RModel = selectRunwayVModel;

                if (_viewType == Models.ViewType.SelectRunway)
                {
                    if (selectRunwayVModel != null)
                    {
                        _sortedRwyList =
                            selectRunwayVModel.RwyDirList.OrderBy(rwyDir => Convert.ToInt32(rwyDir.Name.Substring(0, 2)))
                                .ToList();
                        _selectedRwyDirList = _sortedRwyList.Where(sortedRwy => sortedRwy.Checked).ToList();

                        GlobalParams.TypeAChartParams.HorAccuracy = selectRunwayVModel.HorAccuracy;
                        GlobalParams.TypeAChartParams.VerAccuracy = selectRunwayVModel.VerAccuracy;
                        try
                        {
                            var vModel = new ConstructSurfaceViewModel(_selectedRwyDirList);

                            CurrentViewModel = vModel;
                            PageList.Add(vModel);
                        }
                        catch (Exception e)
                        {
                            CurCursor = System.Windows.Input.Cursors.Arrow;
                            MessageBox.Show(e.Message, "Chart Type A", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    _viewType = Models.ViewType.ConstructSurface;
                }

                else if (_viewType == Models.ViewType.ConstructSurface)
                {
                    var constructSurfaceViewModel = CurrentViewModel as ConstructSurfaceViewModel;

                    if (constructSurfaceViewModel != null)
                    {
                        var cntList = Extensions.SortCenterlinePoints(_sortedRwyList[0],
                            _sortedRwyList[0].CenterLinePoints);
                        var vModel = new ChartParamsViewModel(cntList, constructSurfaceViewModel.GetMapChartWidth(),
                            constructSurfaceViewModel.GetMapChartHeight());

                        _offset = 0;
                        if (constructSurfaceViewModel.IsOffset)
                        {
                            _selectedRwyDirList.Clear();
                            _selectedRwyDirList.Add(constructSurfaceViewModel.SelectedRwyDir);

                            constructSurfaceViewModel.SelectedRwyDir.OffsetWithDeg = constructSurfaceViewModel.Offset;

                            _offset = -Aran.PANDA.Common.ARANMath.DegToRad(constructSurfaceViewModel.Offset);
                            if (constructSurfaceViewModel.SelectedRwyDir.Equals(_sortedRwyList[0]) && constructSurfaceViewModel.SelectedTakeOffSide == TakeoffSide.Left)
                                _offset += Math.PI;
                            else if (constructSurfaceViewModel.SelectedRwyDir.Equals(_sortedRwyList[1]) && constructSurfaceViewModel.SelectedTakeOffSide == TakeoffSide.Right)
                                _offset += Math.PI;


                            // GlobalParams.RotateVal = _selectedRwyDirList[0].Direction - _selectedRwyDirList[0].Offset; // _offset;
                            //EsriFunctions.ChangeMapRotation(GlobalParams.RotateVal);
                        }

                        CurrentViewModel = vModel;
                        NextContent = "Ok";
                        PageList.Add(vModel);
                        _viewType = Models.ViewType.ChartParametr;
                    }
                }
                else if (_viewType == Models.ViewType.ChartParametr)
                {

                    if (selectRunwayVModel == null)
                    {
                        CurCursor = System.Windows.Input.Cursors.Arrow;
                        return;
                    }

                    GlobalParams.RotateVal = _sortedRwyList[0].Direction + _offset;

                    if (!GenerateChart()) return;

                    EsriFunctions.ChangeMapRotation(GlobalParams.RotateVal);

                    UpdateMapFrame();
                    SaveReports();
                    Close();
                }

                BackCommandIsEnabled = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+" "+e.StackTrace);
            }
            finally 
            {
                CurCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void UpdateMapFrame()
        {

            IGraphicsContainer pGraphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;
            pGraphicsContainer.Reset();
            IElement pElement = pGraphicsContainer.Next();
            while (pElement != null)
            {
                if (pElement is IMapFrame)
                {
                    var mapChartHeight = (CurrentViewModel as ChartParamsViewModel).MapChartHeight;
                    var mapChartWidth = (CurrentViewModel as ChartParamsViewModel).MapChartWidth;
                    IEnvelope pEnvelope = new EnvelopeClass();
                    pEnvelope.PutCoords(pElement.Geometry.Envelope.XMin, pElement.Geometry.Envelope.YMin,
                    pElement.Geometry.Envelope.XMax, pElement.Geometry.Envelope.YMin + mapChartHeight);
                    pElement.Geometry = pEnvelope;

                    var mapFrame = pElement as IMapFrame;

                    var activeView = GlobalParams.HookHelper.FocusMap as IActiveView;
                    if (activeView != null)
                    {

                        var spRefOperation = new SpatialReferenceOperation(GlobalParams.Map.SpatialReference);
                        var adhpGeo = (IPoint)spRefOperation.ToEsriPrj(GlobalParams.RModel.SelectedAirport.Geo);

                        var env = activeView.Extent;

                        var envelope = new EnvelopeClass();
                        //var diffX = adhpGeo.X - (env.XMax - env.XMin)*0.5;
                        var diffX = (env.XMax - env.XMin) * 0.5;
                        envelope.XMin = adhpGeo.X - diffX;// env.XMin + diffX;
                        envelope.XMax = adhpGeo.X + diffX;// env.XMax + diffX;

                        //var diffY = adhpGeo.Y - (env.YMax + env.YMin) * 0.5;
                        var diffY = (env.YMax - env.YMin) * 0.5;
                        envelope.YMin = adhpGeo.Y - diffY; //env.YMin + diffY;
                        envelope.YMax = adhpGeo.Y + diffY; //env.YMax + diffY;

                        activeView.Extent = envelope;
                        activeView.Extent.CenterAt(adhpGeo);

                        //IActiveViewEvents aa =

                    }
                    break;
                }
                pElement = pGraphicsContainer.Next();
            }
            GlobalParams.HookHelper.ActiveView.Refresh();
            GenerateGrid();
            GlobalParams.Application.SaveDocument(GlobalParams.ProjectName);
            TypeAChartPreperation.FireEvents();
        }
    
        private bool GenerateChart()
        {
            if (_isOpened)
                return false;
            var frm = new ChartTypeAWizardForm {ShowInTaskbar = false};

            if (frm.ShowDialog() == DialogResult.Cancel) return false;

            _isOpened = true;

            var projectName = frm._ProjectName.EndsWith(".mxd") ? frm._ProjectName : frm._ProjectName + ".mxd";
            var destPath2 = System.IO.Directory.CreateDirectory(frm._FolderName + @"\" + frm._ProjectName).FullName;
            var tmpName = frm._TemplatetName;

            ILayer _Layer = EsriUtils.getLayerByName(GlobalParams.HookHelper.ActiveView.FocusMap, "AirportHeliport");
            var fc = ((IFeatureLayer) _Layer).FeatureClass;

            var airacDate = frm.AiracDate;

            ISpatialReference pSpatialReference = (fc as IGeoDataset).SpatialReference;

            IMap FocusMap = TypeAChartPreperation.ChartsPreparation((int) SigmaChartTypes.ChartTypeA, projectName,
                destPath2, tmpName, GlobalParams.Application);
            if (FocusMap == null) return false;
            FocusMap.SpatialReference = GlobalParams.SpatialRefOperation.PrjSpatialReference;
            

            var workspaceEdit = TypeAChartPreperation.GetWorkspace(FocusMap);

            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            Application.DoEvents();

            var constructSurfaceVModel =
                (ConstructSurfaceViewModel) PageList.First(page => page is ConstructSurfaceViewModel);
            var selectRunwayVModel = (SelectRunwayViewModel) PageList.First(page => page is SelectRunwayViewModel);

            var chartParamsViewModel = (ChartParamsViewModel) PageList.First(page => page is ChartParamsViewModel);


            RwyDirWrapper leftRwyDir = null;
            RwyDirWrapper rightRwyDir = null;
            if (_selectedRwyDirList.Count == 2)
            {
                leftRwyDir = _sortedRwyList[0];
                rightRwyDir = _sortedRwyList[1];
            }
            else if (_selectedRwyDirList[0].Equals(_sortedRwyList[0]))
                leftRwyDir = _selectedRwyDirList[0];
            else
                rightRwyDir = _selectedRwyDirList[0];

            SaveToChartUtils.SaveObstacleArea(constructSurfaceVModel.TakeOffClimbs);
            SaveToChartUtils.SaveRunwayElement(selectRunwayVModel.SelectedRunway, _sortedRwyList);
            SaveToChartUtils.SaveClearAndStopWay(selectRunwayVModel.SelectedAirport,selectRunwayVModel.SelectedRunway, _selectedRwyDirList);
            DecDistancesSaver.SaveDecDistances(leftRwyDir, rightRwyDir);
            SaveToChartUtils.SaveStrip(selectRunwayVModel.SelectedRunway, _sortedRwyList);
            ElementSaver.Save(selectRunwayVModel.SelectedAirport,_selectedRwyDirList.Count<2?_selectedRwyDirList[0]:null, airacDate, _selectedRwyDirList[0].OffsetWithDeg);

            var sortedObstacleList =
                constructSurfaceVModel.ShadowedObstacleList.OrderByDescending(
                    obstacle => Convert.ToInt32(obstacle.RwyDir.Substring(0, 2)))
                    .ThenBy(obstacle => obstacle.X).ToList();

            SaveToChartUtils.SaveObstacles(sortedObstacleList);

            var centerlinePoints = chartParamsViewModel.ProfileCenterLines.Select(profile => profile.Start).ToList();
            if (centerlinePoints?.Count == 0)
                throw new Exception("There are not any centerline points!");

            var endCnt = _sortedRwyList[0].CenterLinePoints.Find(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.END);
            if (endCnt != null)
                centerlinePoints.Add(endCnt);


            SaveToChartUtils.SaveCenterLinePoints(centerlinePoints);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            ChartsHelperClass.ZoomToAreaOfInterestLayer(GlobalParams.Application, FocusMap, "ObstacleArea");

            FocusMap.MapScale = chartParamsViewModel.HorScale;

            GlobalParams.HookHelper.PageLayout.Page.Units = esriUnits.esriCentimeters;

     //       GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(FocusMap.SpatialReference);

            return true;
        }

        private void GenerateGrid()
        {
            var selectRunwayVModel = (SelectRunwayViewModel)PageList.First(page => page is SelectRunwayViewModel);
            if (selectRunwayVModel == null)
                return;

            if (Math.Abs(_offset) < 0.1)
                GlobalParams.GrCreater = new GridCreater();
            else
                GlobalParams.GrCreater = new GridCreaterWithOffset();

            var gc = GlobalParams.GrCreater;
            var chartParamVModel = (ChartParamsViewModel)PageList.First(page => page is ChartParamsViewModel);

            if (chartParamVModel == null)
                return;

            var constructSurfaceVModel =
                (ConstructSurfaceViewModel)PageList.First(page => page is ConstructSurfaceViewModel);

            if (constructSurfaceVModel == null)
                return;

            var baseElevVal = _sortedRwyList[0].CenterLinePoints.Where(cntln => cntln.Elev.HasValue)
                .Min(cntln => cntln.ConvertValueToMeter(cntln.Elev, cntln.Elev_UOM.ToString()));

            if (baseElevVal.HasValue)
                gc.BaseElevation =  Common.RoundByDistance(Common.ConvertHeight(baseElevVal.Value, roundType.toDown),roundType.toDown,50);

            gc.CenterlinePoints = chartParamVModel.ProfileCenterLines.Select(profile => profile.Start).ToList();
            if (gc.CenterlinePoints?.Count == 0)
                throw new Exception("There are not any centerline points!");

            var endCnt = _sortedRwyList[0].CenterLinePoints.Find(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.END);
            if (endCnt!=null)
                gc.CenterlinePoints.Add(endCnt);

            if (_selectedRwyDirList.Count == 2 || _selectedRwyDirList[0].Name == _sortedRwyList[1].Name)
            {
                var secondRwyDir = _sortedRwyList[1];

                var obstacleList = constructSurfaceVModel.ShadowedObstacleList.Where(obs => obs.RwyDir == secondRwyDir.Name).ToList();
                if (obstacleList.Count > 0)
                {
                    double secondHorGridWidth = obstacleList.Max(obs => obs.X);
                    gc.ColumnCount2 =Math.Abs(
                        (int)
                            Math.Ceiling(secondHorGridWidth /chartParamVModel.HorGridInterval) + 1);

                    gc.RowCount2 =Math.Abs(
                        (int)
                            Math.Ceiling((obstacleList.Max(obs => obs.Elevation) - gc.BaseElevation) /
                                chartParamVModel.VerGridInterval) + 1);


                    gc.Pnt2 = secondRwyDir.EndPt;
                    gc.LengthRwy = secondRwyDir.TORA;
                    gc.ClearWay2 = Common.DeConvertDistance(secondRwyDir.ClearWay);
                    gc.CenterlinePoints2 = Extensions.SortCenterlinePoints(_sortedRwyList[1],
                            _sortedRwyList[1].CenterLinePoints);
                }
            }
            if (_selectedRwyDirList.Count == 2 || _selectedRwyDirList[0].Name == _sortedRwyList[0].Name)
            {
                var firstRwyDir = _sortedRwyList[0];

                var obstacleList = constructSurfaceVModel.ShadowedObstacleList.Where(obs => obs.RwyDir == firstRwyDir.Name).ToList();
                if (obstacleList.Count > 0)
                {
                    double firstHorGridWidth = obstacleList.Max(obs => obs.X);

                    gc.ColumnCount1 = Math.Abs((int)Math.Ceiling(firstHorGridWidth / chartParamVModel.HorGridInterval) + 1);

                    gc.RowCount1 =Math.Abs((int)Math.Ceiling((obstacleList.Max(obs => obs.Elevation) - gc.BaseElevation) /
                                          chartParamVModel.VerGridInterval) + 1);


                    gc.LengthRwy = firstRwyDir.TORA;
                    gc.Pnt1 = firstRwyDir.EndPt;
                    gc.ClearWay1 = Common.DeConvertDistance(firstRwyDir.ClearWay);
                }
            }

            gc.Slope = constructSurfaceVModel.Slope;

            var sortedObstacleList =
              constructSurfaceVModel.ShadowedObstacleList.OrderByDescending(
                  obstacle => Convert.ToInt32(obstacle.RwyDir.Substring(0, 2)))
                  .ThenBy(obstacle => obstacle.X).ToList();

            _verticalObstacleCreater = new VerticalObstacleCreater(sortedObstacleList, chartParamVModel.VerScale,
                chartParamVModel.HorScale, chartParamVModel.MapChartHeight, gc.BaseElevation, gc.Pnt1 ?? gc.Pnt2);
            GlobalParams.VerticalObstacleCreater = _verticalObstacleCreater;
            _verticalObstacleCreater.Create();

            gc.ObstacleElements = _verticalObstacleCreater.ObstaGroupElement;

            gc.HorizontalStep = chartParamVModel.HorGridInterval;
            gc.VerticalStep = chartParamVModel.VerGridInterval;
            gc.HorScale = chartParamVModel.HorScale;
            gc.VerScale = chartParamVModel.VerScale;
            gc.TickLength = chartParamVModel.HorGridInterval / 20;
            gc.FrameHeight = chartParamVModel.MapChartHeight;
            gc.OffSet = ARANMath.DegToRad(constructSurfaceVModel.SelectedRwyDir.OffsetWithDeg);
            gc.Side = constructSurfaceVModel.SelectedTakeOffSide;
            gc.ReCreate();

            ScaleElement scaleElement = new ScaleElement((IGraphicsContainer)GlobalParams.HookHelper.PageLayout,100,new LineElementCreater(100));

            var pt = new PointClass(){X = 30,Y = 30};
            scaleElement.CreateScale(pt, GlobalParams.HookHelper.FocusMap.MapScale / 10);
            GlobalParams.ScaleElement = scaleElement;
        }

        private void SaveReports()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Type A Obstacles in Chart"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Title = "Save Omega Report";
            dlg.Filter = "Html documents|*.htm" +
                         "|Pdf document|*.pdf" +
                         "|Excel Worksheets|*.xls";
            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            if (result==true)
            {
                var folderPath = System.IO.Path.GetDirectoryName(dlg.FileName);
                SaveObstacleReports(dlg.FileName,dlg.FilterIndex);
                SavePenetratedObstacleReports(folderPath, dlg.FilterIndex);
            }
        }

        private void SaveObstacleReports(string fileName,int filterIndex)
        {
            try
            {
                var constructSurfaceVModel = (ConstructSurfaceViewModel)PageList.First(page => page is ConstructSurfaceViewModel);
                if (constructSurfaceVModel == null)
                    throw new Exception("Surface is empty!");

                var runwayVModel = (SelectRunwayViewModel)PageList.First(page => page is SelectRunwayViewModel);

                var report = new DoddleReport.Report(constructSurfaceVModel.ShadowedObstacleList.OrderBy(item => item.Name).ToReportSource());

                report.TextFields.Title = "Type A Obstacle Report";

                report.TextFields.SubTitle = "";
                report.TextFields.Footer = report.TextFields.Footer = "Obstacle Count :" + constructSurfaceVModel.ShadowedObstacleList.Count + "<br/>" +
                    "<br/><br/><b>Copyright 2017 &copy; R.I.S.K Company</b>";
                string rwyDesignators = runwayVModel.SelectedRunway.Designator;

                var selectedRwyDirList = runwayVModel.RwyDirList.Where(dir => dir.Checked).ToList<RwyDirWrapper>();

                var rwyDirStr = selectedRwyDirList?[0].Name + (selectedRwyDirList?.Count == 2 ? selectedRwyDirList[1].Name : "");

                report.TextFields.Header = string.Format(@"
                Report Generated: {0}
                Airport/Heliport: {1}
                RWY: {2}
                RWY direction: {3}
                Distance units:: {4}
                Heigh units:: {4}
                ", DateTime.Now, runwayVModel.SelectedAirport.Name, rwyDesignators,
                    rwyDirStr, runwayVModel.DistanceUnits[runwayVModel.SelectedDistanceUnit], runwayVModel.HeightUnits[runwayVModel.SelectedElevUnit]);

                // Customize the data fields
                report.DataFields["GeomPrj"].Hidden = true;
                report.DataFields["IntersectGeom"].Hidden = true;
                report.DataFields["ExactVertexGeom"].Hidden = true;
                report.DataFields["ShadowedBy"].Hidden = true;
                report.DataFields["Plane"].Hidden = true;
                report.DataFields["Y"].Hidden = true;
                report.DataFields["Obstacle"].Hidden = true;
                report.DataFields["ExactVertexGeom"].Hidden = true;
                report.DataFields["CellColor"].Hidden = true;
                report.DataFields["VerticalAccuracy"].Hidden = true;
                report.DataFields["HorizontalAccuracy"].Hidden = true;
                report.DataFields["SurfaceElevation"].Hidden = true;


                report.DataFields["Elevation"].HeaderText = "Elevation (" + runwayVModel.HeightUnits[runwayVModel.SelectedElevUnit] + ")";
                report.DataFields["Penetrate"].HeaderText = "Penetrate (" + runwayVModel.HeightUnits[runwayVModel.SelectedElevUnit] + ")";
                report.DataFields["X"].HeaderText = "Distance from Threshold (" + runwayVModel.DistanceUnits[runwayVModel.SelectedDistanceUnit] + ")";
                //report.DataFields["Plane"].HeaderText = "Equation (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["VsType"].HeaderText = "Type";
                //report.DataFields["VerticalAccuracy"].HeaderText = "Vertical Accuracy (m)";
                //report.DataFields["HorizontalAccuracy"].HeaderText = "Horizontal Accuracy (m)";



                // Process save file dialog box results
                System.IO.Stream stream = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate);
                if (filterIndex == 1)
                {
                    var writer = new HtmlReportWriter();
                    writer.WriteReport(report, stream);
                }
                else if (filterIndex == 2)
                {
                    var writer = new DoddleReport.iTextSharp.PdfReportWriter();
                    writer.WriteReport(report, stream);
                }
                else if (filterIndex == 3)
                {
                    var writer = new ExcelReportWriter();
                    writer.WriteReport(report, stream);
                }
                MessageBox.Show("The document was saved successfully!", "Type A", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured while saving document!", "Type A", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SavePenetratedObstacleReports(string filePath, int filterIndex)
        {
            try
            {
                var constructSurfaceVModel = (ConstructSurfaceViewModel)PageList.First(page => page is ConstructSurfaceViewModel);
                if (constructSurfaceVModel == null)
                    throw new Exception("Surface is empty!");

                var runwayVModel = (SelectRunwayViewModel)PageList.First(page => page is SelectRunwayViewModel);

                var report = new DoddleReport.Report(constructSurfaceVModel.PenetratedObstacleList.OrderBy(item => item.Name).ToReportSource());

                report.TextFields.Title = "Type A Shadowed Obstacle Report";

                report.TextFields.SubTitle = "";
                report.TextFields.Footer = report.TextFields.Footer = "Shadowed Obstacle Count :" + constructSurfaceVModel.ShadowedObstacleList.Count + "<br/>" +
                    "<br/><br/><b>Copyright 2017 &copy; R.I.S.K Company</b>";
                string rwyDesignators = runwayVModel.SelectedRunway.Designator;

                var selectedRwyDirList = runwayVModel.RwyDirList.Where(dir => dir.Checked).ToList<RwyDirWrapper>();

                var rwyDirStr = selectedRwyDirList?[0].Name + (selectedRwyDirList?.Count == 2 ? selectedRwyDirList[1].Name : "");
                foreach (var item in selectedRwyDirList)
                    rwyDirStr += item.Name + "";

                report.TextFields.Header = string.Format(@"
                Report Generated: {0}
                Airport/Heliport: {1}
                RWY: {2}
                RWY direction: {3}
                Distance units:: {4}
                Heigh units:: {4}
                ", DateTime.Now, runwayVModel.SelectedAirport.Name, rwyDesignators,
                    rwyDirStr, runwayVModel.DistanceUnits[runwayVModel.SelectedDistanceUnit], runwayVModel.HeightUnits[runwayVModel.SelectedElevUnit]);

                // Customize the data fields
                report.DataFields["GeomPrj"].Hidden = true;
                report.DataFields["Id"].Hidden = true;
                report.DataFields["IntersectGeom"].Hidden = true;
                report.DataFields["ExactVertexGeom"].Hidden = true;
                report.DataFields["Plane"].Hidden = true;
                report.DataFields["Y"].Hidden = true;
                report.DataFields["Obstacle"].Hidden = true;
                report.DataFields["ExactVertexGeom"].Hidden = true;
                report.DataFields["CellColor"].Hidden = true;
                report.DataFields["VerticalAccuracy"].Hidden = true;
                report.DataFields["HorizontalAccuracy"].Hidden = true;
                report.DataFields["SurfaceElevation"].Hidden = true;


                report.DataFields["Elevation"].HeaderText = "Elevation (" + runwayVModel.HeightUnits[runwayVModel.SelectedElevUnit] + ")";
                report.DataFields["Penetrate"].HeaderText = "Penetrate (" + runwayVModel.HeightUnits[runwayVModel.SelectedElevUnit] + ")";
                report.DataFields["X"].HeaderText = "Distance from Threshold (" + runwayVModel.DistanceUnits[runwayVModel.SelectedDistanceUnit] + ")";


                //report.DataFields["Elevation"].HeaderText = "Elevation (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["Penetrate"].HeaderText = "Penetrate (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["Plane"].HeaderText = "Equation (" + InitOmega.HeightConverter.Unit + ")";
                //report.DataFields["VsType"].HeaderText = "Type";
                //report.DataFields["VerticalAccuracy"].HeaderText = "Vertical Accuracy (m)";
                //report.DataFields["HorizontalAccuracy"].HeaderText = "Horizontal Accuracy (m)";


                string fileName = filePath + "//Shadowed Obstacle Report";

                IReportWriter writer = null;
                if (filterIndex == 1)
                {
                    fileName += ".htm";
                    writer = new HtmlReportWriter();
                }
                else if (filterIndex == 2)
                {
                    fileName += ".pdf";
                    writer = new DoddleReport.iTextSharp.PdfReportWriter();
                }
                else if (filterIndex == 3)
                {
                    fileName += ".excel";
                    writer = new ExcelReportWriter();
                }

                System.IO.Stream stream = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate);
                writer.WriteReport(report, stream);

                MessageBox.Show("Document was saved successfully!", "Type A", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured while saving document!", "Type A", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void PageLayoutViewRefreshed(IActiveView View, esriViewDrawPhase phase, object Data, IEnvelope envelope)
        {
            if (phase == esriViewDrawPhase.esriViewGraphicSelection || phase == esriViewDrawPhase.esriViewGraphicSelection)
            {
                if (_verticalObstacleCreater != null)
                {
                    _verticalObstacleCreater.Create();

                    if (GlobalParams.GrCreater != null)
                    {
                      //  GlobalParams.GC.ObstacleElements = _verticalObstacleCreater.ObstaGroupElement;
                       // GlobalParams.GC.ReCreate();
                    }
                }
            }
        }

        public override void Clear()
        {
            PageList.ForEach(page => page.Clear());
        }
    }
}
