using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ANCOR.MapCore;
using System.Linq;
using System.Windows.Forms;
using PDM;
using ARENA;
using ArenaStatic;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using Aran.PANDA.Common;
using ChartPApproachTerrain.Models;
using EsriWorkEnvironment;
using SigmaChart;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.ArcMapUI;
using GeometryFunctions;
using AranSupport;
using SigmaChart.CmdsMenu;
using System.Globalization;
using System.Threading;

namespace ChartPApproachTerrain
{
    public partial class PATerrainWizardForm : Form
    {
        IActiveView _dataView;
        
        IGroupElement allElemGroup = new GroupElementClass();
        
        IPointCollection profileCntrLinePntColl = new PolylineClass();
        IPointCollection highProfilePointColl = new PolylineClass();
        IPointCollection underProfilePointColl = new PolylineClass();
        List<ObstacleReport> _reportList;

        public string _FolderName;
        public string _ProjectName;
        public string _TemplatetName;
        public string _selectedRasterPath;
        public IAOIBookmark _bookmark;
        public RunwayCenterLinePoint _thresholdPoint;
        public double _thrPointElevInM;

        public IRasterDataset rasterDataset;
        public IRaster2 _selectedRaster;
        public double _glidePathAngle;
        public double _thresholdCrossingHeight;
        public double mapSize_Height;
        public double mapSize_Width;
        
        public double rasterHeightCoef = 1;
        string isolineLayerName = "IsogonalLinesCartography";
        string profileElemName = "Profile";
        string rwyElemName = "RwyElement";

        private List<AirportHeliport> AirportList { get; set; }
        private ObservableCollection<Runway> RunwayList { get; set; }
        private ObservableCollection<RwyDirWrapper> RwyDirList { get; set; }

        public AirportHeliport selectedAirport = null;
        public Runway selectedRunway = null;
        public RwyDirWrapper selectedRunwayDir = null;

        public PATerrainWizardForm()
        {
            NativeMethods.InitAll();

            RwyDirList = new ObservableCollection<RwyDirWrapper>();
            RunwayList = new ObservableCollection<Runway>();

            InitializeComponent();

            try
            {
                _FolderName = "";                
                label9.Text = "";

                var acDate = DataCash.ProjectEnvironment.Data.PdmObjectList.Select(x => x.ActualDate).ToList().Max();
                int _AiracCircle = AiracUtil.AiracUtil.GetAiracCycleByDate(acDate);
                airacControlEffDate.AiracCircleValue = _AiracCircle;
                
                GlobalParams.EffectiveDate = acDate;
                TemplateListComboBox.Items.Clear();
                var tmp = ArenaStaticProc.GetPathToTemplate() + @"\PATC\";
                
                string[] FN = Directory.GetFiles(tmp, "*.mxd");
                foreach (var fl in FN)
                {
                    TemplateListComboBox.Items.Add(System.IO.Path.GetFileName(fl));
                }

                TemplateListComboBox.SelectedIndex = 0;
                _FolderName = ArenaStaticProc.GetPathToMapFolder();

                textBox2.Text = _FolderName;

                AirportList = DataCash.GetAirportlist();
                comboBoxAirport.Items.Clear();

                foreach (var itemADHP in AirportList)
                {
                    comboBoxAirport.Items.Add(itemADHP.Designator +"  (" + itemADHP.Name + ")");
                }

                if (comboBoxAirport.Items.Count > 0)
                {
                    comboBoxAirport.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            _ProjectName = textBox1.Text;
            _FolderName = textBox2.Text;
            _TemplatetName = TemplateListComboBox.Text;

            CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;            
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            _thresholdPoint = selectedRunwayDir.CenterLinePoints.FirstOrDefault(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.DISTHR);

            if (_thresholdPoint?.Elev == null)
                _thresholdPoint = selectedRunwayDir.CenterLinePoints.FirstOrDefault(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.THR);

            if (_thresholdPoint?.Elev == null)
            {
                MessageBox.Show("Sigma","Threshold is null");
                Close();
                return;
            }

                GlobalParams.Area4Rectangle = ChartHelperClass.GetArea4Rectangle(_thresholdPoint, selectedRunwayDir);

            _thrPointElevInM = EsriFunctions.FromHeightVerticalM(_thresholdPoint.Elev_UOM, _thresholdPoint.Elev.Value);

            int thrPixelCol = _selectedRaster.ToPixelColumn(_thresholdPoint.X.Value);
            int thrPixelRow = _selectedRaster.ToPixelRow(_thresholdPoint.Y.Value);
            double thrPixelElev = Convert.ToDouble(_selectedRaster.GetPixelValue(0, thrPixelCol, thrPixelRow));
            if (thrPixelElev / _thrPointElevInM > 2)
                rasterHeightCoef = 0.3048;

            if (!TerminalChartsUtil.CheckFileExisting(_ProjectName, _FolderName)) return;

            Close();
            AlertForm alrtForm = new AlertForm
            {
                FormBorderStyle = FormBorderStyle.None,
                Opacity = 0.5,
                BackgroundImage = Properties.Resources.SigmaSplash
            };


            if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();


            alrtForm.progressBar1.Visible = true;
            alrtForm.progressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.progressBar1.Maximum = 20;
            alrtForm.progressBar1.Value = 0;


            alrtForm.label1.BackColor = System.Drawing.Color.FromArgb(255, 26, 119, 85);
            alrtForm.label1.Text = "Start process";
            alrtForm.label1.Visible = true;
            alrtForm.BringToFront();

            ArenaStaticProc.SetPathToMapFolder(_FolderName);

            GlobalParams.SelectedAirport = selectedAirport;
            GlobalParams.SelectedRunway = selectedRunway;

            if (selectedRunwayDir.RwyDir.TrueBearing.Value < 180)
            {
                GlobalParams.RotateVal = ARANMath.DegToRad(90 - selectedRunwayDir.RwyDir.TrueBearing.Value);
            }
            else
            {
                GlobalParams.RotateVal = ARANMath.DegToRad(270 - selectedRunwayDir.RwyDir.TrueBearing.Value);
            }

            alrtForm.label1.Text = "Preparation";
            alrtForm.progressBar1.Value++;
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();

            var projectName = _ProjectName.EndsWith(".mxd") ? _ProjectName : _ProjectName + ".mxd";
            var destPath2 = Directory.CreateDirectory(_FolderName + @"\" + _ProjectName).FullName;
            var tmpName = _TemplatetName;

            alrtForm.progressBar1.Value++;
            
            IMap FocusMap = ChartsHelperClass.ChartsPreparation((int)SigmaChartTypes.PATChart_Type, projectName,
                destPath2, tmpName, GlobalParams.Application);
            if (FocusMap == null)
            {
                MessageBox.Show("Sigma", "FocusMap is null");
                return;
            }
            GlobalParams.Map = FocusMap;
            var workspaceEdit = PATCChartPreperation.GetWorkspace(FocusMap);

            if(workspaceEdit is null)
            {
                MessageBox.Show("Sigma", "AirportCartography layer not found");
            }

            GlobalParams.HookHelper.PageLayout.Page.Units = esriUnits.esriCentimeters;

            alrtForm.progressBar1.Value++;

            double meridian = GlobalParams.SelectedAirport.X.Value;

            alrtForm.label1.Text = "Change projection";
            alrtForm.progressBar1.Value++;
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();

            EsriUtils.ChangeProjectionAndMeredian(meridian, GlobalParams.HookHelper.FocusMap);

            alrtForm.label1.Text = "Change map rotation";
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();

            EsriFunctions.ChangeMapRotation(GlobalParams.RotateVal);

            alrtForm.label1.Text = "Change spatial reference and map scale";
            alrtForm.progressBar1.Value++;
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();

             ChartHelperClass.UpdateMapFrame();

            alrtForm.label1.Text = "Create Isolines";
            alrtForm.progressBar1.Value++;
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();

            #region Создание изолиний

            IRasterProps RasterProps = (IRasterProps)_selectedRaster;
            IRaster2 Raster2 = _selectedRaster;
            IPnt PntCellSize = RasterProps.MeanCellSize();

            double fNoDataValue;

            try
            {
                fNoDataValue = ((float[])RasterProps.NoDataValue)[0];
            }
            catch
            {
                fNoDataValue = Convert.ToDouble(RasterProps.NoDataValue);
            }

            int GridWidth = RasterProps.Width;
            int GridHeight = RasterProps.Height;

            PolylineBuilder pbI = new PolylineBuilder();
            IEnvelope envelope = RasterProps.Extent;

            IPixelBlockCursor pPixelBlockCursor = new PixelBlockCursor();
            pPixelBlockCursor.InitByRaster(Raster2 as IRaster);
            pPixelBlockCursor.ScanMode = 0;
            pPixelBlockCursor.UpdateBlockSize(GridWidth, GridHeight);

            try
            {
                int L = 0, T = 0, W = 0, H = 0;
                IPixelBlock pPixelBlock = pPixelBlockCursor.NextBlock(ref L, ref T, ref W, ref H);

                double[,] approxiMap = new double[GridHeight, GridWidth];

                double MaxVal = double.MinValue;
                double MinVal = double.MaxValue;

                for (int j = 0; j < GridHeight; j++)
                {
                    for (int i = 0; i < GridWidth; i++)
                    {
                        object PixVal = pPixelBlock.GetVal(0, i, j);

                        approxiMap[GridHeight - j - 1, i] = double.NaN;
                        if (PixVal != null)
                        {
                            double fDixVal = Convert.ToDouble(PixVal) - _thrPointElevInM;

                            approxiMap[GridHeight - j - 1, i] = fDixVal;
                            if (MaxVal < fDixVal) MaxVal = fDixVal;
                            if (MinVal > fDixVal) MinVal = fDixVal;
                        }
                        else
                            approxiMap[GridHeight - j - 1, i] = fNoDataValue;
                    }
                }

                int dVal = 1;
                int nVal = (int)Math.Ceiling((MaxVal - MinVal) / dVal);
                MinVal = Math.Floor(MinVal / dVal) * dVal;

                Conrec.NoDataValue = fNoDataValue;
                pbI.isCircular = true;
                pbI.CreateContour(envelope.XMin, PntCellSize.X, GridWidth, envelope.YMin, PntCellSize.Y, GridHeight, MinVal, dVal, nVal, approxiMap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
                alrtForm.Close();
                return;
            }

            try
            {
                SaveData.JoinSegments(pbI, isolineLayerName, GlobalParams.Step, _thresholdPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
                alrtForm.Close();
                return;

            }

            #endregion

            #region Получить итоговый ObstacleList

            List<VerticalStructure> _vsList =
                        DataCash.GetObstaclesWithinPolygon(GlobalParams.Area4Rectangle).Where(pdmObject => pdmObject is VerticalStructure)
                            .Cast<VerticalStructure>()
                            .ToList();

            TapFunctions tapFunctions = new TapFunctions();
            double maxElevObstacle = -10;

            List<VerticalStructure> _vsResultList = new List<VerticalStructure>();
            _reportList = new List<ObstacleReport>();
            int x, y;
            ITopologicalOperator2 topoOperator = GlobalParams.Area4Rectangle as ITopologicalOperator2;
            _dataView = (IActiveView)GlobalParams.HookHelper.FocusMap;
            var spRefOperation = new SpatialReferenceOperation(GlobalParams.Map.SpatialReference);

            if (!topoOperator.IsKnownSimple)
                topoOperator.Simplify();

            foreach (var vs in _vsList.ToList())
            {
                foreach (var vsPart in vs.Parts.ToList())
                {
                    vsPart.RebuildGeo();
                    if (vsPart.Geo is IPoint)
                    {
                        if (!vsPart.Elev.HasValue)
                            continue;
                        double verAccuracy = 0;
                        if (vsPart.VerticalExtentAccuracy.HasValue)
                            verAccuracy = vsPart.ConvertValueToMeter(vsPart.VerticalExtentAccuracy.Value, vsPart.VerticalExtentAccuracy_UOM.ToString());

                        var partPoint = vsPart.Geo as IPoint;
                        var thrPoint = _thresholdPoint.Geo as IPoint;

                        double resx, resy;
                        NativeMethods.Calc2VectIntersect(thrPoint.X, thrPoint.Y, selectedRunwayDir.RwyDir.TrueBearing.Value, partPoint.X, partPoint.Y, selectedRunwayDir.RwyDir.TrueBearing.Value - 90, out resx, out resy);
                        IPoint intersectPoint = new PointClass();
                        intersectPoint.PutCoords(resx, resy);

                        int pixelCol = _selectedRaster.ToPixelColumn(intersectPoint.X);
                        int pixelRow = _selectedRaster.ToPixelRow(intersectPoint.Y);
                        double centerLinePointElev = Convert.ToDouble(_selectedRaster.GetPixelValue(0, pixelCol, pixelRow)) * rasterHeightCoef;

                        var obsElevationInM = vsPart.ConvertValueToMeter(vsPart.Elev.Value, vsPart.Elev_UOM.ToString()) + verAccuracy;

                        if (obsElevationInM -  centerLinePointElev < Common.offsetValueInM)
                            continue;

                        if (maxElevObstacle < obsElevationInM - centerLinePointElev)
                        {
                            maxElevObstacle = obsElevationInM  - centerLinePointElev;
                        }

                        IPoint pntPrj = spRefOperation.ToEsriPrj(vsPart.Geo) as IPoint;

                        IActiveView _pageLayoutView = (IActiveView)GlobalParams.HookHelper.PageLayout;
                        _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(pntPrj, out x, out y);
                        var pnt1PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                        pnt1PgLayout.X = pnt1PgLayout.X;
                        pnt1PgLayout.Y = Common.yStart + (centerLinePointElev - _thrPointElevInM) * 100 / Common.verScale;

                        IPoint pnt2PgLayout = new Point();
                        pnt2PgLayout.X = pnt1PgLayout.X;

                        pnt2PgLayout.Y = pnt1PgLayout.Y + (obsElevationInM - centerLinePointElev) * 100 / Common.verScale;
                        IPoint[] points = new IPoint[2];
                        points[0] = pnt1PgLayout;
                        points[1] = pnt2PgLayout;

                        _vsResultList.Add(vs);

                        var obstacleReport = new ObstacleReport
                        {
                            Name = vs.Name,
                            Points = points,
                            Elevation = obsElevationInM
                        };
                        _reportList.Add(obstacleReport);

                    }
                    else
                    {
                        if (!vsPart.Elev.HasValue)
                            continue;
                        double verAccuracy = 0;
                        if (vsPart.VerticalExtentAccuracy.HasValue)
                            verAccuracy = vsPart.ConvertValueToMeter(vsPart.VerticalExtentAccuracy.Value, vsPart.VerticalExtentAccuracy_UOM.ToString());

                        IGeometry geo;
                        if (vsPart.Geo is IPolygon)
                            geo = topoOperator.Intersect(vsPart.Geo, esriGeometryDimension.esriGeometry2Dimension);
                        else
                            geo = topoOperator.Intersect(vsPart.Geo, esriGeometryDimension.esriGeometry1Dimension);

                        IPointCollection partPntColl = (IPointCollection)geo;
                        if(partPntColl==null || partPntColl.PointCount==0)
                            continue;

                        if (!vsPart.Elev.HasValue)
                            continue;
                        var obstacleElev = EsriFunctions.FromHeightVerticalM(vsPart.Elev_UOM, vsPart.Elev.Value);

                        double min = 10000;
                        double max = -10000;
                        IPoint nearestPnt = new PointClass();
                        IPoint distantPnt = new PointClass();
                        for (int i = 0; i < partPntColl.PointCount; i++)
                        {
                            var partPoint = partPntColl.get_Point(i);
                            var thrPoint = _thresholdPoint.Geo as IPoint;
                            double resx, resy;
                            NativeMethods.Calc2VectIntersect(thrPoint.X, thrPoint.Y, selectedRunwayDir.RwyDir.TrueBearing.Value, partPoint.X, partPoint.Y, selectedRunwayDir.RwyDir.TrueBearing.Value - 90, out resx, out resy);
                            var dist = NativeMethods.ReturnGeodesicDistance(resx, resy, thrPoint.X, thrPoint.Y);
                            if (dist < min)
                            {
                                IPoint pnt = new PointClass();
                                pnt.PutCoords(resx, resy);
                                nearestPnt = pnt;
                                min = dist;
                            }
                            if (dist > max)
                            {
                                IPoint pnt = new PointClass();
                                pnt.PutCoords(resx, resy);
                                distantPnt = pnt;
                                max = dist;
                            }

                        }

                        int pixelCol = _selectedRaster.ToPixelColumn(nearestPnt.X);
                        int pixelRow = _selectedRaster.ToPixelRow(nearestPnt.Y);
                        double centerLinePoint1Elev = Convert.ToDouble(_selectedRaster.GetPixelValue(0, pixelCol, pixelRow)) * rasterHeightCoef;

                        var obsElevationInM = vsPart.ConvertValueToMeter(vsPart.Elev.Value, vsPart.Elev_UOM.ToString()) + verAccuracy;

                        pixelCol = _selectedRaster.ToPixelColumn(distantPnt.X);
                        pixelRow = _selectedRaster.ToPixelRow(distantPnt.Y);
                        double centerLinePoint2Elev = Convert.ToDouble(_selectedRaster.GetPixelValue(0, pixelCol, pixelRow)) * rasterHeightCoef;

                        if ((obsElevationInM -  centerLinePoint1Elev < Common.offsetValueInM) && (obsElevationInM -  centerLinePoint2Elev) < Common.offsetValueInM)
                            continue;

                        if (maxElevObstacle < obsElevationInM - centerLinePoint1Elev)
                        {
                            maxElevObstacle = obsElevationInM - centerLinePoint1Elev;
                        }

                        IPoint pnt1Prj = spRefOperation.ToEsriPrj(nearestPnt) as IPoint;

                        IActiveView _pageLayoutView = (IActiveView)GlobalParams.HookHelper.PageLayout;
                        _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(pnt1Prj, out x, out y);
                        var pnt1PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                        pnt1PgLayout.X = pnt1PgLayout.X;
                        pnt1PgLayout.Y = Common.yStart + (centerLinePoint1Elev - _thrPointElevInM) * 100 / Common.verScale;

                        IPoint pnt2PgLayout = new Point();
                        pnt2PgLayout.X = pnt1PgLayout.X;

                        pnt2PgLayout.Y = pnt1PgLayout.Y + (obsElevationInM - centerLinePoint1Elev) * 100 / Common.verScale;

                        //
                        IPoint pnt2Prj = spRefOperation.ToEsriPrj(distantPnt) as IPoint;
                        _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(pnt2Prj, out x, out y);
                        var pnt3PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                        pnt3PgLayout.Y = pnt2PgLayout.Y;

                        pixelCol = _selectedRaster.ToPixelColumn(distantPnt.X);
                        pixelRow = _selectedRaster.ToPixelRow(distantPnt.Y);

                        var pnt4PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                        pnt4PgLayout.X = pnt3PgLayout.X;
                        pnt4PgLayout.Y = Common.yStart + (centerLinePoint2Elev - _thrPointElevInM) * 100 / Common.verScale;

                        IPoint[] points = new IPoint[4];
                        points[0] = pnt1PgLayout;
                        points[1] = pnt2PgLayout;
                        points[2] = pnt3PgLayout;
                        points[3] = pnt4PgLayout;

                        _vsResultList.Add(vs);

                        var obstacleReport = new ObstacleReport
                        {
                            Name = vs.Name,
                            Points = points,
                            Elevation = obsElevationInM
                        };
                        _reportList.Add(obstacleReport);
                    }
                }

            }
            #endregion

            #region Итоговый LightingSystems

            //List<LightElement> lightElements = DataCash.GetObstaclesWithinPolygon(GlobalParams.Area4Rectangle).Where(pdmObject => pdmObject is PDM.LightElement)
            //                .Cast<PDM.LightElement>()
            //                .ToList();

            List<LightElement> lightElements = selectedRunwayDir.RwyDir?.RdnLightSystem?.Elements;
            //var list = lightElements?.Select(v => v.ID).ToList();
            List<string> resultLightElements = new List<string>();
            List<ObstacleReport> _lightSysList = new List<ObstacleReport>();
            

            double maxElevLightSys = -10;
            if (lightElements != null)
            {
                foreach (var elem in lightElements)
                {
                    if (elem.Geo is null)
                        elem.RebuildGeo();

                    if (elem.Geo is IPoint)
                    {
                        if (!elem.Elev.HasValue)
                            continue;

                        var partPoint = elem.Geo as IPoint;
                        var thrPoint = _thresholdPoint.Geo as IPoint;

                        double resx, resy;
                        NativeMethods.Calc2VectIntersect(thrPoint.X, thrPoint.Y,
                            selectedRunwayDir.RwyDir.TrueBearing.Value, partPoint.X, partPoint.Y,
                            selectedRunwayDir.RwyDir.TrueBearing.Value - 90, out resx, out resy);
                        IPoint intersectPoint = new PointClass();
                        intersectPoint.PutCoords(resx, resy);

                        int pixelCol = _selectedRaster.ToPixelColumn(intersectPoint.X);
                        int pixelRow = _selectedRaster.ToPixelRow(intersectPoint.Y);
                        double centerLinePointElev =
                            Convert.ToDouble(_selectedRaster.GetPixelValue(0, pixelCol, pixelRow)) * rasterHeightCoef;

                        var lightSysElevInM = elem.ConvertValueToMeter(elem.Elev.Value, elem.Elev_UOM.ToString());

                        //if (lightSysElevInM - (_thrPointElevInM - centerLinePointElev) < Common.offsetValueInM)
                        if (lightSysElevInM - centerLinePointElev < Common.offsetValueInM)
                            continue;

                        if (maxElevLightSys < lightSysElevInM - centerLinePointElev)
                        {
                            maxElevLightSys = lightSysElevInM - centerLinePointElev;
                        }

                        var pntGeo = elem.Geo;

                        var pntPrj = spRefOperation.ToEsriPrj(pntGeo);

                        IActiveView _pageLayoutView = (IActiveView) GlobalParams.HookHelper.PageLayout;
                        _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint((IPoint) pntPrj, out x, out y);
                        var pnt1PgLayout = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                        pnt1PgLayout.X = pnt1PgLayout.X;
                        pnt1PgLayout.Y = Common.yStart + (centerLinePointElev - _thrPointElevInM) * 100 /
                                         Common.verScale;

                        IPoint pnt2PgLayout = new Point();
                        pnt2PgLayout.X = pnt1PgLayout.X;

                        pnt2PgLayout.Y = pnt1PgLayout.Y + (lightSysElevInM) * 100 / Common.verScale;
                        IPoint[] points = new IPoint[2];
                        points[0] = pnt1PgLayout;
                        points[1] = pnt2PgLayout;

                        resultLightElements.Add(elem.ID);

                        var obstacleReport = new ObstacleReport
                        {
                            Name = elem.ID,
                            Points = points,
                            Elevation = lightSysElevInM
                        };
                        _lightSysList.Add(obstacleReport);
                    }
                }
            }

            

            #endregion

            GlobalParams.ObstacleList = _vsResultList;

            alrtForm.label1.Text = "Data quering";
            alrtForm.progressBar1.Value++;
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();

            #region Сохранение данных

            workspaceEdit.StartEditing(false);
            workspaceEdit.StartEditOperation();

            Application.DoEvents();

            #region Data Query

            if (lightElements != null && lightElements.Count > 0)
            {
                string lightID = selectedRunwayDir.RwyDir?.RdnLightSystem?.ID;
                if (checkBox1.Checked && lightID != null)
                {
                    ChartHelperClass.SaveLightingSystems(lightElements.Select(v => v.ID).ToList(), lightID);
                }
                else if (lightID != null)
                {
                    ChartHelperClass.SaveLightingSystems(resultLightElements, lightID);
                }
            }

            ChartHelperClass.SaveObstacles(GlobalParams.ObstacleList);
            ChartHelperClass.SaveCenterLinePoints(_thresholdPoint);
            ChartHelperClass.SaveRunwayElement(selectedRunway, RwyDirList.ToList());
            SaveData.AddRasterLayer(rasterDataset);
            #endregion

            ChartHelperClass.SaveOtherParams(selectedAirport, selectedRunwayDir, selectedRunwayDir.OffsetWithDeg);

            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);

            #endregion

            alrtForm.label1.Text = "Create profile";
            alrtForm.progressBar1.Value++;
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();

            #region Добавление препятствий на профиль

            IGraphicsContainer graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;            
           
            IRgbColor lineColorObs = new RgbColorClass();
            lineColorObs.Red = 0; lineColorObs.Blue = 0; lineColorObs.Green = 0;

            ISimpleLineSymbol dashLineSymbolObs = new SimpleLineSymbolClass();
            dashLineSymbolObs.Color = lineColorObs;
            dashLineSymbolObs.Width = 1;
            dashLineSymbolObs.Style = esriSimpleLineStyle.esriSLSDot;
            ILineElement lineElementObs;

            foreach (var report in _reportList)
            {
                                                 
                    var lineElem =ChartHelperClass.CreateLineElement(report.Points);

                    lineElementObs = lineElem as ILineElement;
                    lineElementObs.Symbol = dashLineSymbolObs;

                    IGroupElement grElement = new GroupElementClass();
                    grElement.AddElement(lineElem);

                    allElemGroup.AddElement((IElement)grElement);

            }

            #endregion

            #region Добавление LightSystem's на профиль

            IRgbColor lineColorLightSys = new RgbColorClass();
            lineColorLightSys.Red = 0; lineColorLightSys.Blue = 0; lineColorLightSys.Green = 0;

            ISymbol symbol = ChartHelperClass.LoadStyleSymbol();

            ISimpleLineSymbol dashLineSymbolLightSys = new SimpleLineSymbolClass();
            dashLineSymbolLightSys.Color = lineColorLightSys;
            dashLineSymbolLightSys.Width = 1;           
            ILineElement lineElementLightSys;

            foreach (var report in _lightSysList)
            {
                var lineElem = ChartHelperClass.CreateLineElement(report.Points);

                lineElementLightSys = lineElem as ILineElement;

                if (symbol != null)
                    lineElementLightSys.Symbol = (ILineSymbol)symbol;

                IGroupElement grElement = new GroupElementClass();
                grElement.AddElement(lineElem);

                allElemGroup.AddElement((IElement)grElement);

            }

            #endregion

            alrtForm.progressBar1.Value++;

            #region Профиль по центральной линии и получение остальных препятствий(+- 3м)

            double absaluteMaxElev, absaluteMinElev;
            absaluteMaxElev = absaluteMinElev = 0;

            double distance = 0;
            double step;
            IRasterProps rasterProps = (IRasterProps)_selectedRaster;
            if (rasterProps.SpatialReference is IGeographicCoordinateSystem)
            {
                step = NativeMethods.ReturnGeodesicDistance(_selectedRaster.ToMapX(0), _selectedRaster.ToMapY(0), _selectedRaster.ToMapX(1), _selectedRaster.ToMapY(0));
            }
            else
            {
                step = Math.Abs(_selectedRaster.ToMapX(0) - _selectedRaster.ToMapX(1));
            }
            double profCenterLineX, profCenterLineY;
            double vertCycleStartX, vertCycleStartY;

            while (distance <= Common.areaWidth)
            {
                NativeMethods.PointAlongGeodesic(_thresholdPoint.X.Value, _thresholdPoint.Y.Value, distance, selectedRunwayDir.RwyDir.TrueBearing.Value + 180, out profCenterLineX, out profCenterLineY);

                int pixelCol = _selectedRaster.ToPixelColumn(profCenterLineX);
                int pixelRow = _selectedRaster.ToPixelRow(profCenterLineY);

                double centerLinePixelElev;

                if ((_selectedRaster.GetPixelValue(0, pixelCol, pixelRow) == null) || Convert.ToDouble(_selectedRaster.GetPixelValue(0, pixelCol, pixelRow)) > 5000)

                    centerLinePixelElev = _thrPointElevInM * rasterHeightCoef;

                else
                    centerLinePixelElev = Convert.ToDouble(_selectedRaster.GetPixelValue(0, pixelCol, pixelRow)) * rasterHeightCoef;

                IPoint profCntrPnt = new PointClass();
                profCntrPnt.PutCoords(profCenterLineX, profCenterLineY);

                IActiveView _pageLayoutView = (IActiveView)GlobalParams.HookHelper.PageLayout;
                
                IPoint profCntrPntPrj = spRefOperation.ToEsriPrj(profCntrPnt) as IPoint;

                _dataView.ScreenDisplay.DisplayTransformation.FromMapPoint(profCntrPntPrj, out x, out y);
                var centerLinePoint = _pageLayoutView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

                centerLinePoint.Y = Common.yStart + (centerLinePixelElev - _thrPointElevInM) * 100 / Common.verScale;
                profileCntrLinePntColl.AddPoint(centerLinePoint);

                NativeMethods.PointAlongGeodesic(profCenterLineX, profCenterLineY, Common.areaHeight / 2, selectedRunwayDir.RwyDir.TrueBearing.Value + 90, out vertCycleStartX, out vertCycleStartY);

                double vertDistance = 0;

                double maxElev = centerLinePixelElev;
                double minElev = centerLinePixelElev;
                while (vertDistance <= Common.areaHeight)
                {
                    double vertNextX, vertNextY;
                    NativeMethods.PointAlongGeodesic(vertCycleStartX, vertCycleStartY, vertDistance, selectedRunwayDir.RwyDir.TrueBearing.Value - 90, out vertNextX, out vertNextY);

                    int colVert = _selectedRaster.ToPixelColumn(vertNextX);
                    int rowVert = _selectedRaster.ToPixelRow(vertNextY);

                    double vertPixelElev; 

                    if ((_selectedRaster.GetPixelValue(0, colVert, rowVert) == null) || Convert.ToDouble(_selectedRaster.GetPixelValue(0, colVert, rowVert)) > 5000)
                        vertPixelElev = centerLinePixelElev * rasterHeightCoef;
                    else
                        vertPixelElev = Convert.ToDouble(_selectedRaster.GetPixelValue(0, colVert, rowVert)) * rasterHeightCoef;


                    if (vertPixelElev > maxElev)
                    {
                        maxElev = vertPixelElev;
                    }

                    if (vertPixelElev < minElev)
                    {
                        minElev = vertPixelElev;
                    }

                    vertDistance += step;

                }
                if (maxElev > absaluteMaxElev - _thrPointElevInM)
                {
                    absaluteMaxElev = maxElev - _thrPointElevInM;
                }

                if (maxElev - centerLinePixelElev < Common.offsetValueInM)
                {
                    maxElev = centerLinePixelElev;
                }

                IPoint profileHighPoint = centerLinePoint;
                profileHighPoint.Y = Common.yStart + (maxElev - _thrPointElevInM) * 100 / Common.verScale;
                highProfilePointColl.AddPoint(profileHighPoint);

                if (minElev - _thrPointElevInM < absaluteMinElev)
                {
                    absaluteMinElev = minElev - _thrPointElevInM;
                }

                if (centerLinePixelElev - minElev < Common.offsetValueInM)
                {
                    minElev = centerLinePixelElev;
                }

                IPoint profileUnderPoint = centerLinePoint;
                profileUnderPoint.Y = Common.yStart + (minElev - _thrPointElevInM) * 100 / Common.verScale;
                underProfilePointColl.AddPoint(profileUnderPoint);

                distance += step;
            }
            #endregion

            alrtForm.progressBar1.Value++;
           
            #region Scalebar's
            double scaleBarStart = -4;
            if (scaleBarStart > Math.Floor(absaluteMinElev))
            {
                scaleBarStart = Math.Floor(absaluteMinElev);
                if (Math.Abs(scaleBarStart) % 2 == 1)
                {
                    scaleBarStart -= 1;
                }
            }
            double scaleBarEnd = 10;
            if (scaleBarEnd < Math.Ceiling(absaluteMaxElev))
            {
                scaleBarEnd = Math.Ceiling(absaluteMaxElev);
                if (Math.Abs(scaleBarEnd) % 2 == 1)
                {
                    scaleBarEnd += 1;
                }
            }
            if (scaleBarEnd < Math.Ceiling(_thresholdCrossingHeight))
            {
                scaleBarEnd = Math.Ceiling(_thresholdCrossingHeight);
                if (Math.Abs(scaleBarEnd) % 2 == 1)
                {
                    scaleBarEnd += 1;
                }
            }

            if (scaleBarEnd < Math.Ceiling(maxElevObstacle))
            {
                scaleBarEnd = Math.Ceiling(maxElevObstacle);
                if (Math.Abs(scaleBarEnd) % 2 == 1)
                {
                    scaleBarEnd += 1;
                }
            }
            
            double horLevelHeight = scaleBarEnd - 2;

            IGroupElement horScaleBar =ChartHelperClass.HorizontalScaleBar(_thresholdPoint.X.Value, _thresholdPoint.Y.Value, selectedRunwayDir.RwyDir.TrueBearing.Value, horLevelHeight);

            IGroupElement leftScalebar = ChartHelperClass.VerticalScaleBar(scaleBarStart, scaleBarEnd, "Left");
            IGroupElement rightScalebar = ChartHelperClass.VerticalScaleBar(scaleBarStart, scaleBarEnd, "Right");
            IGroupElement scalebarFT = ChartHelperClass.VerticalScaleBarFT(Math.Floor(scaleBarStart / 3.048) * 10, Math.Ceiling(scaleBarEnd / 3.048) * 10);

            #endregion

            alrtForm.progressBar1.Value++;

            #region Nominal Glidepath

            IPoint startGlideBar = new PointClass();
            IPoint endGlideBar = new PointClass();
            IElement glideText;
            IElement glideTextHeight;
            if (selectedRunwayDir.RwyDir.TrueBearing.Value > 180)
            {
                startGlideBar.PutCoords(Common.xStart - 1, Common.yStart - 0.4 + _thresholdCrossingHeight * 100 / Common.verScale);
                endGlideBar.PutCoords(Common.xStart + 3.5, Common.yStart - 0.4 + _thresholdCrossingHeight * 100 / Common.verScale);
                ITransform2D transform = endGlideBar as ITransform2D;
                transform.Rotate(startGlideBar, ARANMath.DegToRad(20));
            }
            else
            {
                startGlideBar.PutCoords(Common.xStart - 3.5 + Common.areaWidth * 100 / Common.horScale, Common.yStart - 0.4 + _thresholdCrossingHeight * 100 / Common.verScale);
                endGlideBar.PutCoords(Common.xStart + 1 + Common.areaWidth * 100 / Common.horScale, Common.yStart - 0.4 + _thresholdCrossingHeight * 100 / Common.verScale);
                ITransform2D transform = startGlideBar as ITransform2D;
                transform.Rotate(endGlideBar, ARANMath.DegToRad(-20));
            }

            List<IPoint> glidePoints = new List<IPoint> {startGlideBar, endGlideBar};

            var glideLine = AnnotationUtil.GetPolylineElement_Simle(glidePoints, 0, style: esriSimpleLineStyle.esriSLSDashDot);
            if (selectedRunwayDir.RwyDir.TrueBearing.Value > 180)
            {
                glideText = AnnotationUtil.CreateFreeTextElement(glideLine.Geometry, "Nominal glide path " + _glidePathAngle + "°", false, verAlignment: verticalAlignment.Bottom, horAlligment: horizontalAlignment.Right);

            }
            else
            {
                glideText = AnnotationUtil.CreateFreeTextElement(glideLine.Geometry, "Nominal glide path" + _glidePathAngle + "°", false, verAlignment: verticalAlignment.Bottom, horAlligment: horizontalAlignment.Left); 
            }

            if (selectedRunwayDir.RwyDir.TrueBearing.Value > 180)
            {
                glideTextHeight = AnnotationUtil.CreateFreeTextElement(glideLine.Geometry, Math.Round(_thresholdCrossingHeight, 1) + " M" + " (" + Math.Round(_thresholdCrossingHeight / 0.3048, 1) + " FT)", false, verAlignment: verticalAlignment.Top, horAlligment: horizontalAlignment.Right);

            }
            else
            {
                glideTextHeight = AnnotationUtil.CreateFreeTextElement(glideLine.Geometry, Math.Round(_thresholdCrossingHeight, 1) + " M" + " (" + Math.Round(_thresholdCrossingHeight / 0.3048, 1) + " FT)", false, verAlignment: verticalAlignment.Top, horAlligment: horizontalAlignment.Left);
            }


            #endregion

            #region Торец полосы перед MapFrame

            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)GlobalParams.HookHelper.PageLayout;
            pGraphicsContainer.Reset();
            IFrameElement frameElement = pGraphicsContainer.FindFrame(GlobalParams.ActiveView.FocusMap);
            IMapFrame mapFrame = (IMapFrame)frameElement;
            IElement mapElement = (IElement)mapFrame;

            double runwayElementWidth = 45;
            if (selectedRunway.Width.HasValue)
                runwayElementWidth = selectedRunway.Width.Value;

            IPoint pnt1, pnt2, pnt3, pnt4;
            double offsetOfBound = ((Common.areaHeight - runwayElementWidth) / 2) * 100 / Common.horScale;
            if (selectedRunwayDir.RwyDir.TrueBearing.Value > 180)
            {
                pnt1 = new PointClass();
                pnt1.PutCoords(Common.xStart - 1.5, mapElement.Geometry.Envelope.YMax - offsetOfBound);
                pnt2 = new PointClass();
                pnt2.PutCoords(Common.xStart, mapElement.Geometry.Envelope.YMax - offsetOfBound);
                pnt3 = new PointClass();
                pnt3.PutCoords(Common.xStart, mapElement.Geometry.Envelope.YMin + offsetOfBound);
                pnt4 = new PointClass();
                pnt4.PutCoords(Common.xStart - 1.5, mapElement.Geometry.Envelope.YMin + offsetOfBound);
            }
            else
            {
                pnt1 = new PointClass();
                pnt1.PutCoords(mapElement.Geometry.Envelope.XMax + 1.5, mapElement.Geometry.Envelope.YMax - offsetOfBound);
                pnt2 = new PointClass();
                pnt2.PutCoords(mapElement.Geometry.Envelope.XMax, mapElement.Geometry.Envelope.YMax - offsetOfBound);
                pnt3 = new PointClass();
                pnt3.PutCoords(mapElement.Geometry.Envelope.XMax, mapElement.Geometry.Envelope.YMin + offsetOfBound);
                pnt4 = new PointClass();
                pnt4.PutCoords(mapElement.Geometry.Envelope.XMax + 1.5, mapElement.Geometry.Envelope.YMin + offsetOfBound);
 
            }

            List<IPoint> rwyElemPoints = new List<IPoint> {pnt1, pnt2, pnt3, pnt4};

            var rwyElemLine = AnnotationUtil.GetPolylineElement_Simle(rwyElemPoints, 0, lineWidth: 3);

            List<IPoint> pointsForText = new List<IPoint> {pnt2, pnt3};
            var lineForText = AnnotationUtil.GetPolylineElement_Simle(pointsForText, 0, lineWidth: 3);
            IElement rwyElemText;
            if (selectedRunwayDir.RwyDir.TrueBearing.Value > 180)
            {
                rwyElemText = AnnotationUtil.CreateFreeTextElement(lineForText.Geometry, selectedRunwayDir.RwyDir.Designator, true, fontSize: 24, verAlignment: verticalAlignment.Bottom);
            }
            else
            {
                rwyElemText = AnnotationUtil.CreateFreeTextElement(lineForText.Geometry, selectedRunwayDir.RwyDir.Designator, false, fontSize: 24, verAlignment: verticalAlignment.Bottom); 
            }


            #endregion

            #region Color, Line symbol and add to graphicsContainer

            IRgbColor lineColor = new RgbColorClass();
            lineColor.Red = 0; lineColor.Blue = 0; lineColor.Green = 0;

            ISimpleLineSymbol lineSymbol = new SimpleLineSymbolClass();
            lineSymbol.Color = lineColor;
            lineSymbol.Width = 2;

            ISimpleLineSymbol dashLineSymbol = new SimpleLineSymbolClass();
            dashLineSymbol.Color = lineColor;
            dashLineSymbol.Width = 1;
            dashLineSymbol.Style = esriSimpleLineStyle.esriSLSDot;

            IElement elemProfCtrLine = new LineElement();
            IPolyline polyline = (IPolyline)profileCntrLinePntColl;
            elemProfCtrLine.Geometry = polyline;

            IElement elemHighProfLine = new LineElement();
            IPolyline highPolyline = (IPolyline)highProfilePointColl;
            elemHighProfLine.Geometry = highPolyline;

            IElement elemUnderProfLine = new LineElement();
            IPolyline underPolyline = (IPolyline)underProfilePointColl;
            elemUnderProfLine.Geometry = underPolyline;

            ILineElement lineElement;
            lineElement = elemProfCtrLine as ILineElement;
            lineElement.Symbol = lineSymbol;

            lineElement = elemHighProfLine as ILineElement;
            lineElement.Symbol = dashLineSymbol;

            lineElement = elemUnderProfLine as ILineElement;
            lineElement.Symbol = dashLineSymbol;

            IMxDocument mxdoc = GlobalParams.Application.Document as IMxDocument;
            IPageLayout pageLayout = new PageLayout();
            pageLayout = mxdoc.PageLayout;
            
            
            allElemGroup.AddElement(elemProfCtrLine);
            allElemGroup.AddElement(elemHighProfLine);
            allElemGroup.AddElement(elemUnderProfLine);
            allElemGroup.AddElement((IElement)horScaleBar);
            allElemGroup.AddElement((IElement)leftScalebar);
            allElemGroup.AddElement((IElement)rightScalebar);
            allElemGroup.AddElement((IElement)scalebarFT);
            allElemGroup.AddElement(glideLine);
            allElemGroup.AddElement(glideText);
            allElemGroup.AddElement(glideTextHeight);

            IGroupElement rwyElemGroup = new GroupElementClass();
            rwyElemGroup.AddElement(rwyElemLine);
            rwyElemGroup.AddElement(rwyElemText);

            IElement allElement = allElemGroup as IElement;

            IElement rwyElement = rwyElemGroup as IElement;
            
            IElementProperties3 elemProp = (IElementProperties3)allElement;

            IElementProperties3 rwyElemProp = (IElementProperties3)rwyElement;

            elemProp.Name = profileElemName;
            rwyElemProp.Name = rwyElemName;
            
            graphicsContainer.AddElement(allElement, 0);
            //allElement.Locked = true;

            graphicsContainer.AddElement(rwyElement, 0);
            rwyElement.Locked = true;
           
            IActiveView activeView = pageLayout as IActiveView;
            activeView.Refresh();

            #endregion

            GlobalParams.Map.Layer[GlobalParams.Map.LayerCount - 1].Visible = false;

            #region Bookmark

            _bookmark = ChartsHelperClass.CreateBookmark(GlobalParams.ActiveView.FocusMap, "PATC_Bookmark");
            
            if (_bookmark != null)
            {
                IMapBookmarks mapBookmarks = (IMapBookmarks)FocusMap;
                //Add the bookmark to the bookmarks collection
                mapBookmarks.RemoveAllBookmarks();
                mapBookmarks.AddBookmark(_bookmark);

                ISpatialBookmark spatialBookmark = _bookmark;
                //Zoom to the bookmark
                spatialBookmark.ZoomTo(FocusMap);

            }
            #endregion

            alrtForm.label1.Text = "Finalisation";
            alrtForm.progressBar1.Value++;
            alrtForm.progressBar1.Value++;
            alrtForm.BringToFront();
            
            ((IGraphicsContainerSelect)graphicsContainer).UnselectAllElements();
            ChartElementsManipulator.RefreshChart((IMxDocument)GlobalParams.Application.Document);
            
            //Создание Isogonal Annotation
            PrecisionApproachChartClass _Chart = new PrecisionApproachChartClass { SigmaHookHelper = GlobalParams.HookHelper, SigmaApplication = GlobalParams.Application, PName = projectName };
            _Chart.CreateChart();

            ArenaMessageForm msgFrm = new ArenaMessageForm("SIGMA", "Chart is created successfully",
                Properties.Resources.SigmaMessage)
            {
                TopMost = true,
                checkBox1 = {Visible = false}
            };
            msgFrm.ShowDialog();
            

            FireEvents();
            Application.DoEvents();
            
            alrtForm.Close();

            Thread.CurrentThread.CurrentCulture = oldCI;
            return;
              
        }       
       
        private void btnOk_Validated(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Length <= 0 ? "NewChart" : textBox1.Text;
        }

        private void PATerrainWizardForm_Load(object sender, EventArgs e)
        {
            axToolbarControl2.SetBuddyControl(axPageLayoutControl1);            
        }

        private void btnSelectRaster_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Files|*.tif"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _selectedRasterPath = ofd.FileName.ToString();
                textBox3.Text = ofd.FileName.ToString();

                FileInfo fInfo = new FileInfo(ofd.FileName);

                string strFileName = fInfo.Name;

                string strFilePath = fInfo.DirectoryName;

                rasterDataset =ChartHelperClass.OpenFileRasterDataset(strFilePath, strFileName);

                IRasterDataset2 rasterDataset2 = (IRasterDataset2)rasterDataset;
                _selectedRaster = (IRaster2)rasterDataset2.CreateFullRaster();
                if (_selectedRaster == null)
                {
                    return;
                }
                IRasterProps rasterProps = (IRasterProps)_selectedRaster;

                if (rasterProps.SpatialReference is IProjectedCoordinateSystem)
                {
                    label9.ForeColor = System.Drawing.Color.Red;
                    label9.Text = "Raster must be in Geographic Coordinate System";
                    btnOk.Enabled = false;
                    return;
                }

                double step = NativeMethods.ReturnGeodesicDistance(_selectedRaster.ToMapX(0), _selectedRaster.ToMapY(0), _selectedRaster.ToMapX(1), _selectedRaster.ToMapY(0));

                GlobalParams.Step = step;

                //if (rasterHeightCoef != 1)
                //{
                //    step = step * rasterHeightCoef;
                //}

                label9.Text = "Cell size:" + Math.Round(step, 2) + " M" + "\n";
                
                

                var thrCntlPt = selectedRunwayDir.CenterLinePoints.FirstOrDefault(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.THR);

                if (thrCntlPt != null)
                {
                    thrCntlPt.RebuildGeo();

                    _thresholdPoint = thrCntlPt;

                    bool check = ChartHelperClass.CheckContain(_selectedRaster, (IPoint)_thresholdPoint.Geo);

                    if (!check)
                    {
                        label9.ForeColor = System.Drawing.Color.Red;
                        label9.Text += "Raster does not match with area4";
                        btnOk.Enabled = false;
                    }
                    else
                    {
                        label9.ForeColor = System.Drawing.Color.Blue;
                        label9.Text += "Correct Raster";
                        btnOk.Enabled = true;
                    }

                }
            }
            
        }

        private void btnPrjLocation_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            CalendarForm frm = new CalendarForm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                airacControlEffDate.AiracCircleValue = AiracUtil.AiracUtil.GetAiracCycleByDate(frm.selectedDate);
                GlobalParams.EffectiveDate = frm.selectedDate;
            }
        }

        private void PATerrainWizardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalParams.IsOpened = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Close();
        }

        #region Combobox evets

        private void comboBoxAirport_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunwayList.Clear();
            RwyDirList.Clear();
            comboBoxRunway.Items.Clear();
            comboBoxRunwayDir.Items.Clear();

            selectedAirport = AirportList[comboBoxAirport.SelectedIndex];
            if (selectedAirport != null)
            {
                var tmpList = selectedAirport.RunwayList;
                if (tmpList != null)
                {
                    foreach (var runway in tmpList)
                    {
                        RunwayList.Add(runway);
                        comboBoxRunway.Items.Add(runway.Designator);
                    }
                }

                if (RunwayList.Count > 0)
                    comboBoxRunway.SelectedIndex = 0;

                if (RunwayList == null || RunwayList.Count == 0)
                {
                    labelLog.ForeColor = System.Drawing.Color.Red;
                    labelLog.Text = "Airport haven't runway's";
                    return;
                }
            }

        }

        private void comboBoxRunway_SelectedIndexChanged(object sender, EventArgs e)
        {
            RwyDirList.Clear();
            comboBoxRunwayDir.Items.Clear();
            selectedRunway = RunwayList[comboBoxRunway.SelectedIndex];
            if (selectedRunway != null)
            {

                var tmpList = selectedRunway.RunwayDirectionList;
                if (tmpList != null)
                {
                    foreach (var runwayDir in tmpList)
                    {
                        var rwyDirWrapper = new RwyDirWrapper(runwayDir);
                        RwyDirList.Add(rwyDirWrapper);
                        comboBoxRunwayDir.Items.Add(runwayDir.Designator);
                    }
                }

                if (RwyDirList.Count > 0)
                    comboBoxRunwayDir.SelectedIndex = 0;

                if (RwyDirList == null || RwyDirList.Count == 0)
                {
                    labelLog.ForeColor = System.Drawing.Color.Red;
                    labelLog.Text = "Runway haven't RunwayDirection";
                    return;
                }
            }
        }

        private void comboBoxRunwayDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            label9.Text = "";
            listBox2.Items.Clear();
            selectedRunwayDir = RwyDirList[comboBoxRunwayDir.SelectedIndex];
            GlobalParams.SelectedRwyDirection = selectedRunwayDir.RwyDir;
            if (selectedRunwayDir != null)
            {

                var navaidList = selectedRunwayDir.RwyDir.Related_NavaidSystem;

                if (navaidList is null || navaidList.Count == 0)
                {
                    btnOk.Enabled = false;
                    labelLog.ForeColor = System.Drawing.Color.Red;
                    labelLog.Text = "RunwayDirection haven't navaids";
                    return;
                }
                foreach (NavaidSystem ils_nav in navaidList)
                {
                    
                    if ((ils_nav.CodeNavaidSystemType == NavaidSystemType.ILS_DME || ils_nav.CodeNavaidSystemType == NavaidSystemType.ILS) && (ils_nav.SignalPerformance == CodeSignalPerformanceILS.I || ils_nav.SignalPerformance == CodeSignalPerformanceILS.II || ils_nav.SignalPerformance == CodeSignalPerformanceILS.III))
                    {
                        listBox2.Items.Clear();
                        foreach (var component in ils_nav.Components)
                        {
                            if (component.PDM_Type == PDM_ENUM.GlidePath)
                            {
                                if (((GlidePath)component).Angle.HasValue && ((GlidePath)component).ThresholdCrossingHeight.HasValue)
                                {
                                    
                                    _glidePathAngle = ((GlidePath)component).Angle.Value;
                                    _thresholdCrossingHeight = Convert.ToDouble(((GlidePath)component).ThresholdCrossingHeight);
                                    if (((GlidePath)component).UomDistVer.ToString() == "FT")
                                    {
                                        _thresholdCrossingHeight = _thresholdCrossingHeight * 0.3048;
                                    }
                                    listBox2.Items.Add("NavaidSystemType:           " + ils_nav.CodeNavaidSystemType);
                                    listBox2.Items.Add("SignalPerformance:           " + ils_nav.SignalPerformance);

                                    if (((GlidePath)component).UomDistVer.ToString() == "FT" || ((GlidePath)component).UomDistVer.ToString() == "M")
                                    {
                                        listBox2.Items.Add("ThresholdCrossingHeight:  " + Math.Round(_thresholdCrossingHeight, 1) + " M");
                                    }
                                    else
                                    {
                                        listBox2.Items.Add("ThresholdCrossingHeight:  " + " Incorrect unit of measure");
                                        //MessageBox.Show("Incorrect unit of measure (ThresholdCrossingHeight)");
                                    }
                                    listBox2.Items.Add("Angle:                              " + _glidePathAngle+ " °");
                                    btnOk.Enabled = true;
                                    labelLog.Text = "";
                                    RunwayCenterLinePoint thrCntlPt = selectedRunwayDir.CenterLinePoints.FirstOrDefault(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.DISTHR);
                                    if (thrCntlPt?.Elev == null)
                                        thrCntlPt = selectedRunwayDir.CenterLinePoints.FirstOrDefault(cnt => cnt.Role == CodeRunwayCenterLinePointRoleType.THR);


                                    if (thrCntlPt != null && ((IRasterProps) _selectedRaster)?.SpatialReference is IGeographicCoordinateSystem)
                                    {
                                        double step = NativeMethods.ReturnGeodesicDistance(_selectedRaster.ToMapX(0), _selectedRaster.ToMapY(0), _selectedRaster.ToMapX(1), _selectedRaster.ToMapY(0));
                                        
                                        label9.Text = "Cell size:" + Math.Round(step, 2) + " M" + "\n";                                        
                                        thrCntlPt.RebuildGeo();
                                       
                                        bool check = ChartHelperClass.CheckContain(_selectedRaster, (IPoint)thrCntlPt.Geo);

                                        if (!check)
                                        {
                                            label9.ForeColor = System.Drawing.Color.Red;
                                            label9.Text = "Raster does not match with area4";
                                            btnOk.Enabled = false;
                                        }
                                        else
                                        {
                                            label9.ForeColor = System.Drawing.Color.Blue;
                                            label9.Text = "Correct Raster";
                                            btnOk.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        btnOk.Enabled = false;
                                    }

                                    return;
                                }
                                else
                                {
                                    labelLog.ForeColor = System.Drawing.Color.Red;
                                    labelLog.Text = "Angle or ThresholdCrossingHeight haven't value";
                                    btnOk.Enabled = false;
                                }
                            }
                            else
                            {
                                labelLog.ForeColor = System.Drawing.Color.Red;
                                labelLog.Text = "ILS haven't GlidePath component";
                                btnOk.Enabled = false;
                            }
                        }
                        return;
                    }
                    else
                    {
                        labelLog.ForeColor = System.Drawing.Color.Red;
                        listBox2.Items.Add("NavaidSystemType:           " + ils_nav.CodeNavaidSystemType);
                        listBox2.Items.Add("SignalPerformance:           " + ils_nav.SignalPerformance);
                        labelLog.Text = "NavaidSystemType does not match";
                        btnOk.Enabled = false;
                        continue;
                    }
                }
            }
            
        }

        private void TemplateListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChartHelperClass.SelectChartTemplate(ref axPageLayoutControl1, ArenaStaticProc.GetPathToTemplate() + @"\PATC\" + TemplateListComboBox.Text, ref listBox1, out mapSize_Height, out mapSize_Width);

        }

        #endregion
       
        #region Events Pan, Resize

        private void FireEvents()
        {
            IActiveViewEvents_Event activeViewEvents = GlobalParams.Map as IActiveViewEvents_Event;
            if (activeViewEvents != null)
            {

                var m_ActiveViewEventViewRefreshed = new IActiveViewEvents_ViewRefreshedEventHandler(ViewRefreshed);
                activeViewEvents.ViewRefreshed += m_ActiveViewEventViewRefreshed;

                var m_ActiveViewEventsAfterDraw = new IActiveViewEvents_AfterDrawEventHandler(OnActiveViewEventsItemDraw);
                activeViewEvents.AfterDraw += m_ActiveViewEventsAfterDraw;

            }
        }

        private void ViewRefreshed(IActiveView View, esriViewDrawPhase phase, object Data, IEnvelope envelope)
        {

            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)GlobalParams.HookHelper.PageLayout;
            pGraphicsContainer.Reset();
            
            IFrameElement frameElement = pGraphicsContainer.FindFrame(GlobalParams.ActiveView.FocusMap);
            IMapFrame mapFrame = (IMapFrame)frameElement;
            IElement mapElement = (IElement)mapFrame;

            var mapChartHeight = Common.areaHeight * 100 / Common.horScale;
            var mapChartWidth = Common.areaWidth * 100 / Common.horScale;
            if (mapElement.Geometry.Envelope.Width != mapChartWidth)
            {                
                IEnvelope pEnvelope = new EnvelopeClass();
                pEnvelope.PutCoords(mapElement.Geometry.Envelope.XMin, mapElement.Geometry.Envelope.YMin,
                mapElement.Geometry.Envelope.XMin + mapChartWidth, mapElement.Geometry.Envelope.YMin + mapChartHeight);
                mapElement.Geometry = pEnvelope;

                var activeView = GlobalParams.HookHelper.FocusMap as IActiveView;
                if (activeView != null)
                {
                    var spRefOperation = new SpatialReferenceOperation(GlobalParams.Map.SpatialReference);
                    var areaGeo = spRefOperation.ToEsriPrj(GlobalParams.Area4Rectangle);
                    var env = areaGeo.Envelope;

                    activeView.Extent = env;
                    GlobalParams.HookHelper.FocusMap.MapScale = Common.horScale;
                }
                GlobalParams.ActiveView.Refresh();
                
            }
            

        }

        private void OnActiveViewEventsItemDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)GlobalParams.HookHelper.PageLayout;
            pGraphicsContainer.Reset();
            IElement elemProfile = pGraphicsContainer.Next();
            IElementProperties3 elemProp = (IElementProperties3)elemProfile;
            while (elemProp.Name != profileElemName)
            {
                elemProfile = pGraphicsContainer.Next();
                elemProp = (IElementProperties3)elemProfile;
            }
            
            pGraphicsContainer.Reset();
            IElement rwyElem = pGraphicsContainer.Next();
            IElementProperties3 rwyElemProp = (IElementProperties3)rwyElem;
            while (rwyElemProp.Name != rwyElemName)
            {
                rwyElem = pGraphicsContainer.Next();
                rwyElemProp = (IElementProperties3)rwyElem;
            }

            IFrameElement frameElement = pGraphicsContainer.FindFrame(GlobalParams.ActiveView.FocusMap);
            IMapFrame mapFrame = (IMapFrame)frameElement;
            IElement mapElement = (IElement)mapFrame;

            double runwayElementWidth = 45;
            if (selectedRunway.Width.HasValue)
                runwayElementWidth = selectedRunway.Width.Value;
            double offsetOfBound = ((Common.areaHeight - runwayElementWidth) / 2) * 100 / Common.horScale;
            double offsetRwyElemY = mapElement.Geometry.Envelope.YMax - rwyElem.Geometry.Envelope.YMax;

            if (selectedRunwayDir.RwyDir.TrueBearing.Value > 180)
            {
                double offset = mapElement.Geometry.Envelope.XMin - elemProfile.Geometry.Envelope.XMin;
                if (offset != 1)
                {
                    //elemProfile.Locked = false;
                    ITransform2D elemTransform2D;
                    elemTransform2D = elemProfile as ITransform2D;
                    elemTransform2D.Move(offset - 1, 0);
                    //elemProfile.Locked = true;
                    GlobalParams.ActiveView.Refresh();
                }                

                double offsetRwyElemX = mapElement.Geometry.Envelope.XMin - rwyElem.Geometry.Envelope.XMax;
                
                if (offsetRwyElemX != 0 || Math.Round(offsetRwyElemY, 2) != Math.Round(offsetOfBound, 2))
                {
                    rwyElem.Locked = false;

                    ITransform2D rwyElemTransform2D;
                    rwyElemTransform2D = rwyElem as ITransform2D;
                    rwyElemTransform2D.Move(offsetRwyElemX, offsetRwyElemY - offsetOfBound);
                    rwyElem.Locked = true;
                    GlobalParams.ActiveView.Refresh();

                }
            }
            else
            {
                double offset = mapElement.Geometry.Envelope.XMax - elemProfile.Geometry.Envelope.XMax;
                if (offset != -1)
                {
                    //elemProfile.Locked = false;
                    ITransform2D elemTransform2D;
                    elemTransform2D = elemProfile as ITransform2D;
                    elemTransform2D.Move(offset + 1, 0);
                    //elemProfile.Locked = true;
                    GlobalParams.ActiveView.Refresh();
                }

                double offsetRwyElemX = mapElement.Geometry.Envelope.XMax - rwyElem.Geometry.Envelope.XMin;

                if (offsetRwyElemX != 0 || Math.Round(offsetRwyElemY, 2) != Math.Round(offsetOfBound, 2))
                {
                    rwyElem.Locked = false;

                    ITransform2D rwyElemTransform2D;
                    rwyElemTransform2D = rwyElem as ITransform2D;
                    rwyElemTransform2D.Move(offsetRwyElemX, offsetRwyElemY - offsetOfBound);
                    rwyElem.Locked = true;
                    GlobalParams.ActiveView.Refresh();

                }
            }


        }

        #endregion

        
    }
}
