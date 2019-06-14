using Aran.Aim;
using Aran.Aim.AixmMessage;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.InputFormLib;
using Aran.Converters;
using Aran.Geometries;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Feature = Aran.Aim.Features.Feature;
using FileStream = System.IO.FileStream;
using IRow = NPOI.SS.UserModel.IRow;
using Path = System.IO.Path;
using System.Diagnostics.CodeAnalysis;
using Aran.Queries;
using System.Reflection;
using NPOI.SS.UserModel;

namespace DataConverter
{
    internal enum GeomType
    {
        Point,
        Polyline,
        Polygon
    }

    internal enum SourceType
    {
        Shape,
        Gdb,
        Access
    }

    public partial class FrmMain : Form
    {
        private readonly List<FeatureType> _featureTypeList =
            new List<FeatureType>()
                {
                    FeatureType.VerticalStructure,
                    FeatureType.RunwayProtectArea,
                    FeatureType.RunwayCentrelinePoint,
                    FeatureType.GuidanceLine,
                    FeatureType.DesignatedPoint,
                    FeatureType.ApronElement,
                    FeatureType.TaxiHoldingPosition,
                    FeatureType.DME,
                    FeatureType.VOR,
                    FeatureType.Glidepath,
                    FeatureType.NDB,
                    FeatureType.Localizer
                };

        private IFeatureWorkspace _featureWorkspace;

        private FeatureType _selectedFeatType;

        private int _mergeLetterCount;

        public FrmMain()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop);
            InitializeComponent();
            cmbBxFeatTypes.DataSource = _featureTypeList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Width = grpBxProperties.Location.X + grpBxProperties.Width + 25;
            Height = grpBxProperties.Location.Y + grpBxProperties.Height + 80;
            //this.MinimumSize = this.Size;
            //this.MaximumSize = this.Size;

            IAoInitialize ao = new AoInitialize();
            ao.Initialize(esriLicenseProductCode.esriLicenseProductCodeStandard);

            txtBxPath.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void ui_selectFolderButton_Click(object sender, EventArgs e)
        {
            if (radBtnSourcGdb.Checked)
            {
                FolderBrowserDialog folderBrowserDlg =
                    new FolderBrowserDialog { RootFolder = Environment.SpecialFolder.MyComputer, };
                if (folderBrowserDlg.ShowDialog() != DialogResult.OK || !folderBrowserDlg.SelectedPath.EndsWith(".gdb"))
                    return;
                txtBxPath.Text = folderBrowserDlg.SelectedPath;
                GetLayers(folderBrowserDlg.SelectedPath, SourceType.Gdb);
            }
            else if (radBtnShapeFile.Checked)
            {
                FolderBrowserDialog folderBrowserDlg =
                    new FolderBrowserDialog { RootFolder = Environment.SpecialFolder.MyComputer, };
                if (folderBrowserDlg.ShowDialog() != DialogResult.OK)
                    return;
                txtBxPath.Text = folderBrowserDlg.SelectedPath;
                GetLayers(folderBrowserDlg.SelectedPath, SourceType.Shape);
            }
            else
            {
                var filter = "Personal geodatabase | *.mdb";
                if (radBtnSourceExcel.Checked)
                    filter = "Excel Worksheets | *.xlsx";
                FileDialog fileDlg = new OpenFileDialog();
                fileDlg.Filter = filter;

                if (fileDlg.ShowDialog() != DialogResult.OK)
                    return;

                if (radBtnSourceMdb.Checked)
                    GetLayers(fileDlg.FileName, SourceType.Access);
                else if (radBtnShapeFile.Checked)
                    GetLayers(fileDlg.FileName, SourceType.Shape);
                else if (radBtnSourceExcel.Checked)
                    SetColumnNames(fileDlg.FileName);
                txtBxPath.Text = fileDlg.FileName;
            }

        }

        private void SetColumnNames(string fileName)
        {
            var xssfwb = new XSSFWorkbook(fileName);
            var sheet = xssfwb.GetSheetAt(0);
            var row = sheet.GetRow(0);
            var columns = new List<string>();
            for (int i = 0; i < 13; i++)
            {
                if (row.GetCell(i) != null)
                    columns.Add(row.GetCell(i).ToString());
            }

            var arrayCols = columns.ToArray();
            cmbBxNameNavaid.Items.Clear();
            cmbBxNameNavaid.Items.AddRange(arrayCols);

            cmbBxElevationNavaid.Items.Clear();
            cmbBxElevationNavaid.Items.AddRange(arrayCols);

            cmbBxNavaidLatitude.Items.Clear();
            cmbBxNavaidLatitude.Items.AddRange(arrayCols);

            cmbBxNavaidLongitude.Items.Clear();
            cmbBxNavaidLongitude.Items.AddRange(arrayCols);

            cmbBxHorAccuracyNavaid.Items.Clear();
            cmbBxHorAccuracyNavaid.Items.AddRange(arrayCols);

            cmbBxVertAccuracyNavaid.Items.Clear();
            cmbBxVertAccuracyNavaid.Items.AddRange(arrayCols);

            cmbBxNames.Items.Clear();
            cmbBxNames.Items.AddRange(arrayCols);

            cmbBxIdentifiers.Items.Clear();
            cmbBxIdentifiers.Items.AddRange(arrayCols);

            cmbBxTypes.Items.Clear();
            cmbBxTypes.Items.AddRange(arrayCols);

            cmbBxElevations.Items.Clear();
            cmbBxElevations.Items.AddRange(arrayCols);

            cmbBxHeights.Items.Clear();
            cmbBxHeights.Items.AddRange(arrayCols);

            cmbBxUom.Items.Clear();
            cmbBxUom.Items.AddRange(arrayCols);

            cmbBxLatitude.Items.Clear();
            cmbBxLatitude.Items.AddRange(arrayCols);

            cmbBxLongitude.Items.Clear();
            cmbBxLongitude.Items.AddRange(arrayCols);

            cmbBxLighting.Items.Clear();
            cmbBxLighting.Items.AddRange(arrayCols);

            cmbBxMarking.Items.Clear();
            cmbBxMarking.Items.AddRange(arrayCols);

            cmbBxGroup.Items.Clear();
            cmbBxGroup.Items.AddRange(arrayCols);

            cmbBxHorAccuracy.Items.Clear();
            cmbBxHorAccuracy.Items.AddRange(arrayCols);

            cmbBxVertAccuracy.Items.Clear();
            cmbBxVertAccuracy.Items.AddRange(arrayCols);

            cmbBxNote.Items.Clear();
            cmbBxNote.Items.AddRange(arrayCols);

            cmbBxNameDsgPnt.Items.Clear();
            cmbBxNameDsgPnt.Items.AddRange(arrayCols);

            cmbBxLatDsgPnt.Items.Clear();
            cmbBxLatDsgPnt.Items.AddRange(arrayCols);

            cmbBxLongDsgPnt.Items.Clear();
            cmbBxLongDsgPnt.Items.AddRange(arrayCols);
        }

        private void GetLayers(string fileName, SourceType sourceType)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            IWorkspaceFactory2 workspaceFactory = null;
            IWorkspace workspace;
            switch (sourceType)
            {
                case SourceType.Shape:
                    // Temporary opening gdb file is the solution for opening ShapeFile without exception
                    workspaceFactory = (IWorkspaceFactory2)new FileGDBWorkspaceFactory();
                    string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    UriBuilder uri = new UriBuilder(codeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    var tmpFile = Path.Combine(Path.GetDirectoryName(path), "dataconverter.gdb");
                    workspace = workspaceFactory.OpenFromFile(tmpFile, 0);
                    //
                    workspaceFactory = (IWorkspaceFactory2)new ShapefileWorkspaceFactory();
                    break;
                case SourceType.Gdb:
                    workspaceFactory = (IWorkspaceFactory2)new FileGDBWorkspaceFactory();
                    break;
                case SourceType.Access:
                    workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null);
            }

            workspace = workspaceFactory.OpenFromFile(fileName, 0);
            _featureWorkspace = (IFeatureWorkspace)workspace;
            IEnumDataset datasets = workspace.get_Datasets(esriDatasetType.esriDTAny);
            cmbBxLayers.Items.Clear();
            IDataset dataset = null;
            while ((dataset = datasets.Next()) != null)
            {
                if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                {
                    cmbBxLayers.Items.Add(dataset.Name);
                }
                else if (dataset.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    var featureDataset = (IFeatureDataset)dataset;
                    IEnumDataset enumDataset = featureDataset.Subsets;
                    dataset = enumDataset.Next();
                    while (dataset != null)
                    {
                        if (dataset.Type == esriDatasetType.esriDTFeatureClass)
                            cmbBxLayers.Items.Add(dataset.Name);
                        dataset = enumDataset.Next();
                    }
                }
            }

            cmbBxLayers.SelectedIndex = 0;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            List<Feature> featList;
            string resultFileName = txtBxResultFilePath.Text;
            if (radBtnSourceExcel.Checked)
            {
                switch (_selectedFeatType)
                {
                    case FeatureType.VerticalStructure:
                        featList = CreateVerticalStructureListFromExcel();
                        break;
                    case FeatureType.DesignatedPoint:
                        featList = CreateDesignatedPointListFromExcel();
                        break;
                    case FeatureType.DME:
                    case FeatureType.VOR:
                    case FeatureType.Glidepath:
                    case FeatureType.Localizer:
                    case FeatureType.NDB:
                        featList = CreateFeatListFromExcel(_selectedFeatType).Cast<Feature>().ToList();
                        break;
                    //    featList = CreateDmeListFromExcel();
                    //    break;
                    //case FeatureType.VOR:
                    //    featList = CreateVorListFromExcel();
                    //    break;
                    //case FeatureType.Glidepath:
                    //    featList = CreateGlidePathListFromExcel();
                    //    break;
                    //case FeatureType.Localizer:
                    //    featList = CreateLocalizerListFromExcel();
                    //    break;
                    default:
                        throw new NotImplementedException($"Сonverting from {_selectedFeatType} is not implemented");
                }

                InputFormController.WriteAllFeatureToXML(
                    featList,
                    Path.Combine(resultFileName),
                    false,
                    false,
                    new DateTime(
                        dateTimePicker1.Value.Year,
                        dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day,
                        0,
                        0,
                        0),
                    SrsNameType.EPSG_4326);
            }
            else
            {
                IFeatureClass featureClass = _featureWorkspace.OpenFeatureClass(cmbBxLayers.SelectedItem.ToString());
                featList = CreateFeatureListFromGeoDatabase(featureClass);
                InputFormController.WriteAllFeatureToXML(
                    featList,
                    Path.Combine(resultFileName),
                    false,
                    false,
                    new DateTime(
                        dateTimePicker1.Value.Year,
                        dateTimePicker1.Value.Month,
                        dateTimePicker1.Value.Day,
                        0,
                        0,
                        0),
                    SrsNameType.EPSG_4326);
            }

            MessageBox.Show("Process finished");
        }

        private List<Feature> CreateDesignatedPointListFromExcel()
        {
            string sourcFile = txtBxPath.Text;
            var resultList = new List<Feature>();
            using (var file = new FileStream(sourcFile, FileMode.Open, FileAccess.Read))
            {
                var xssfwb = new XSSFWorkbook(file);
                var sheet = xssfwb.GetSheetAt(0);
                for (var index = 0; index <= sheet.LastRowNum; index++)
                {
                    var row = sheet.GetRow(index);
                    if (index == 0 || row == null || row.Cells.Count == 0 || row.GetCell(0) == null) continue;
                    DesignatedPoint dsgPnt =
                        CreateDesignatedPoint(row.GetCell(cmbBxNameDsgPnt.SelectedIndex).StringCellValue);

                    if (cmbBxIdentifiers.SelectedItem != null)
                        dsgPnt.Identifier = new Guid(row.GetCell(cmbBxIdentifiers.SelectedIndex).StringCellValue);

                    ParseCoordinates(
                        row,
                        index,
                        cmbBxLatDsgPnt.SelectedIndex,
                        cmbBxLongDsgPnt.SelectedIndex,
                        out var lat,
                        out var longitude);

                    dsgPnt.Location = new AixmPoint();
                    dsgPnt.Location.Geo.Y = lat;
                    dsgPnt.Location.Geo.X = longitude;
                    resultList.Add(dsgPnt);
                }
            }

            return resultList;
        }

        //private List<Feature> CreateDmeListFromExcel()
        //{
        //    var resultList = new List<Feature>();
        //    using (var file = new FileStream(txtBxPath.Text, FileMode.Open, FileAccess.Read))
        //    {
        //        var xssfwb = new XSSFWorkbook(file);
        //        var sheet = xssfwb.GetSheetAt(0);
        //        for (var index = 0; index <= sheet.LastRowNum; index++)
        //        {
        //            var row = sheet.GetRow(index);
        //            if (index == 0 || row == null || row.Cells.Count == 0 || row.GetCell(0) == null) continue;
        //            DME result =
        //                CreateDme(row.GetCell(cmbBxNameNavaid.SelectedIndex).StringCellValue);

        //            if (cmbBxIdentifiers.SelectedItem != null)
        //                result.Identifier = new Guid(row.GetCell(cmbBxIdentifiers.SelectedIndex).StringCellValue);

        //            ParseCoordinates(
        //                row,
        //                index,
        //                cmbBxNavaidLatitude.SelectedIndex,
        //                cmbBxNavaidLongitude.SelectedIndex,
        //                out var lat,
        //                out var longitude);

        //            result.Location = new ElevatedPoint();
        //            double tmp;
        //            if (cmbBxElevationNavaid.SelectedIndex != -1)
        //            {
        //                tmp = double.Parse(row.GetCell(cmbBxElevationNavaid.SelectedIndex).ToString());
        //                result.Location.Elevation = new ValDistanceVertical(tmp, UomDistanceVertical.M);
        //            }

        //            if(cmbBxHorAccuracyNavaid.SelectedIndex != -1)
        //            {
        //                tmp = double.Parse(row.GetCell(cmbBxHorAccuracyNavaid.SelectedIndex).ToString());
        //                result.Location.HorizontalAccuracy = new ValDistance(tmp, UomDistance.M);
        //            }
        //            result.Location.Geo.Y = lat;
        //            result.Location.Geo.X = longitude;
        //            resultList.Add(result);
        //        }
        //    }

        //    return resultList;
        //}

        //private List<Feature> CreateVorListFromExcel()
        //{
        //    var resultList = new List<Feature>();
        //    using (var file = new FileStream(txtBxPath.Text, FileMode.Open, FileAccess.Read))
        //    {
        //        var xssfwb = new XSSFWorkbook(file);
        //        var sheet = xssfwb.GetSheetAt(0);
        //        for (var index = 0; index <= sheet.LastRowNum; index++)
        //        {
        //            var row = sheet.GetRow(index);
        //            if (index == 0 || row == null || row.Cells.Count == 0 || row.GetCell(0) == null) continue;
        //            VOR result =
        //                CreateVOR(row.GetCell(cmbBxNameNavaid.SelectedIndex).StringCellValue);

        //            if (cmbBxIdentifiers.SelectedItem != null)
        //                result.Identifier = new Guid(row.GetCell(cmbBxIdentifiers.SelectedIndex).StringCellValue);

        //            ParseCoordinates(
        //                row,
        //                index,
        //                cmbBxNavaidLatitude.SelectedIndex,
        //                cmbBxNavaidLongitude.SelectedIndex,
        //                out var lat,
        //                out var longitude);

        //            result.Location = new ElevatedPoint();
        //            double tmp;
        //            if (cmbBxElevationNavaid.SelectedIndex != -1)
        //            {
        //                tmp = double.Parse(row.GetCell(cmbBxElevationNavaid.SelectedIndex).ToString());
        //                result.Location.Elevation = new ValDistanceVertical(tmp, UomDistanceVertical.M);
        //            }

        //            if (cmbBxHorAccuracyNavaid.SelectedIndex != -1)
        //            {
        //                tmp = double.Parse(row.GetCell(cmbBxHorAccuracyNavaid.SelectedIndex).ToString());
        //                result.Location.HorizontalAccuracy = new ValDistance(tmp, UomDistance.M);
        //            }
        //            result.Location.Geo.Y = lat;
        //            result.Location.Geo.X = longitude;
        //            resultList.Add(result);
        //        }
        //    }
        //    return resultList;
        //}

        //private List<Feature> CreateGlidePathListFromExcel()
        //{
        //    var resultList = new List<Feature>();
        //    using (var file = new FileStream(txtBxPath.Text, FileMode.Open, FileAccess.Read))
        //    {
        //        var xssfwb = new XSSFWorkbook(file);
        //        var sheet = xssfwb.GetSheetAt(0);
        //        for (var index = 0; index <= sheet.LastRowNum; index++)
        //        {
        //            var row = sheet.GetRow(index);
        //            if (index == 0 || row == null || row.Cells.Count == 0 || row.GetCell(0) == null) continue;
        //            Glidepath result =
        //                CreateGlidePath(row.GetCell(cmbBxNameNavaid.SelectedIndex).StringCellValue);

        //            if (cmbBxIdentifiers.SelectedItem != null)
        //                result.Identifier = new Guid(row.GetCell(cmbBxIdentifiers.SelectedIndex).StringCellValue);

        //            ParseCoordinates(
        //                row,
        //                index,
        //                cmbBxNavaidLatitude.SelectedIndex,
        //                cmbBxNavaidLongitude.SelectedIndex,
        //                out var lat,
        //                out var longitude);

        //            result.Location = new ElevatedPoint();
        //            double tmp;
        //            if (cmbBxElevationNavaid.SelectedIndex != -1)
        //            {
        //                tmp = double.Parse(row.GetCell(cmbBxElevationNavaid.SelectedIndex).ToString());
        //                result.Location.Elevation = new ValDistanceVertical(tmp, UomDistanceVertical.M);
        //            }

        //            if (cmbBxHorAccuracyNavaid.SelectedIndex != -1)
        //            {
        //                tmp = double.Parse(row.GetCell(cmbBxHorAccuracyNavaid.SelectedIndex).ToString());
        //                result.Location.HorizontalAccuracy = new ValDistance(tmp, UomDistance.M);
        //            }
        //            result.Location.Geo.Y = lat;
        //            result.Location.Geo.X = longitude;
        //            resultList.Add(result);
        //        }
        //    }
        //    return resultList;
        //}

        private List<NavaidEquipment> CreateFeatListFromExcel(FeatureType featureType)
        {
            var resultList = new List<NavaidEquipment>();
            using (var file = new FileStream(txtBxPath.Text, FileMode.Open, FileAccess.Read))
            {
                var xssfwb = new XSSFWorkbook(file);
                var sheet = xssfwb.GetSheetAt(0);
                for (var index = 0; index <= sheet.LastRowNum; index++)
                {
                    var row = sheet.GetRow(index);
                    if (index == 0 || row == null || row.Cells.Count == 0 || row.GetCell(0) == null) continue;
                    NavaidEquipment result = null;
                    var name = row.GetCell(cmbBxNameNavaid.SelectedIndex).StringCellValue;
                    result = (NavaidEquipment)AimObjectFactory.CreateFeature(featureType);
                    result.Name = row.GetCell(cmbBxNameNavaid.SelectedIndex).StringCellValue;
                    result.TimeSlice = CreateTimeSlice();

                    if (cmbBxIdentifiers.SelectedItem != null)
                        result.Identifier = new Guid(row.GetCell(cmbBxIdentifiers.SelectedIndex).StringCellValue);
                    else
                        result.Identifier = Guid.NewGuid();

                    ParseCoordinates(
                        row,
                        index,
                        cmbBxNavaidLatitude.SelectedIndex,
                        cmbBxNavaidLongitude.SelectedIndex,
                        out var lat,
                        out var longitude);

                    result.Location = new ElevatedPoint();
                    double tmp;
                    if (cmbBxElevationNavaid.SelectedIndex != -1)
                    {
                        tmp = double.Parse(row.GetCell(cmbBxElevationNavaid.SelectedIndex).ToString(), CultureInfo.InvariantCulture);
                        result.Location.Elevation = new ValDistanceVertical(tmp, UomDistanceVertical.M);
                    }

                    if (cmbBxHorAccuracyNavaid.SelectedIndex != -1)
                    {
                        tmp = double.Parse(row.GetCell(cmbBxHorAccuracyNavaid.SelectedIndex).ToString(), CultureInfo.InvariantCulture);
                        result.Location.HorizontalAccuracy = new ValDistance(tmp, UomDistance.M);
                    }
                    result.Location.Geo.Y = lat;
                    result.Location.Geo.X = longitude;
                    resultList.Add(result);
                }
            }
            return resultList;
        }

        private DesignatedPoint CreateDesignatedPoint(string name)
        {
            DesignatedPoint result = new DesignatedPoint
            {                
                Name = name
            };
            result.TimeSlice = CreateTimeSlice();
            return result;
        }

        //private DME CreateDme(string name)
        //{
        //    DME result = new DME
        //    {
        //        Name = name
        //    };
        //    result.TimeSlice = CreateTimeSlice();
        //    return result;
        //}

        //private VOR CreateVOR(string name)
        //{
        //    var result = new VOR()
        //    {
        //        Name = name
        //    };
        //    result.TimeSlice = CreateTimeSlice();
        //    return result;
        //}

        //private Localizer CreateLocalizer(string name)
        //{
        //    Localizer result = new Localizer()
        //    {
        //        Name = name
        //    };
        //    result.TimeSlice = CreateTimeSlice();
        //    return result;
        //}

        //private Glidepath CreateGlidePath(string name)
        //{
        //    Glidepath result = new Glidepath
        //    {
        //        Name = name
        //    };
        //    result.TimeSlice = CreateTimeSlice();
        //    return result;
        //}

        private TimeSlice CreateTimeSlice()
        {
            return new TimeSlice
            {
                FeatureLifetime = new TimePeriod(new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, 0, 0, 0)),
                ValidTime = new TimePeriod(new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, 0, 0, 0)),
                Interpretation = TimeSliceInterpretationType.BASELINE,
                SequenceNumber = 1
            };
        }

        private List<Feature> CreateVerticalStructureListFromExcel()
        {
            string sourcFile = txtBxPath.Text;
            var resultList = new List<Feature>();
            using (var file = new FileStream(sourcFile, FileMode.Open, FileAccess.Read))
            {
                var xssfwb = new XSSFWorkbook(file);
                Dictionary<string, VerticalStructure> obsList = new Dictionary<string, VerticalStructure>();
                var enums = Enum.GetValues(typeof(GeomType));
                GeomType geomType;
                for (int i = 0; i < Enum.GetValues(typeof(GeomType)).Length; i++)
                {
                    if (i >= xssfwb.NumberOfSheets)
                        break;
                    var sheet = xssfwb.GetSheetAt(i);
                    if (sheet.SheetName.ToLower().Contains("point"))
                        geomType = GeomType.Point;
                    else if (sheet.SheetName.ToLower().Contains("line"))
                        geomType = GeomType.Polyline;
                    else if (sheet.SheetName.ToLower().Contains("polygon"))
                        geomType = GeomType.Polygon;
                    else
                        throw new Exception("Geometry type is not supported");

                    DataFormatter formatter = new DataFormatter(); //creating formatter using the default locale
                    for (var index = 0; index <= sheet.LastRowNum; index++)
                    {
                        var row = sheet.GetRow(index);
                        if (index == 0 || row == null || row.Cells.Count == 0 || row.GetCell(0) == null) continue;
                        VerticalStructure vertStruct;

                        if (geomType == GeomType.Point)
                            vertStruct =
                                CreateVerticalStructure(
                                    formatter.FormatCellValue(row.GetCell(cmbBxNames.SelectedIndex)));
                        else
                        {
                            var obsName = row.GetCell(cmbBxNames.SelectedIndex).StringCellValue.Substring(0, 7);
                            if (!obsList.ContainsKey(obsName))
                                obsList.Add(
                                    obsName,
                                    CreateVerticalStructure(
                                        row.GetCell(cmbBxNames.SelectedIndex).StringCellValue.Substring(0, 7)));
                            vertStruct = obsList[obsName];
                        }

                        if (cmbBxIdentifiers.SelectedItem != null)
                            vertStruct.Identifier =
                                new Guid(row.GetCell(cmbBxIdentifiers.SelectedIndex).StringCellValue);

                        double lat;
                        double longitude;
                        ParseCoordinates(
                            row,
                            index,
                            cmbBxLatitude.SelectedIndex,
                            cmbBxLongitude.SelectedIndex,
                            out lat,
                            out longitude);

                        string tmp;
                        double value;
                        ValDistance vertAccuracy = null;
                        ValDistance horAccuracy = null;
                        ValDistanceVertical elevation = null;
                        UomDistanceVertical uomDistVertical = UomDistanceVertical.M;

                        if (cmbBxUom.SelectedIndex > -1)
                        {
                            string uomStr = "m";
                            uomStr = row.GetCell(cmbBxUom.SelectedIndex).StringCellValue;
                            if (!Enum.TryParse<UomDistanceVertical>(uomStr, out uomDistVertical))
                                throw new Exception(
                                    $"Couldn't parse Uom at cell[Row:{(index + 1)};Column:{cmbBxUom.SelectedItem}] which is {uomStr}");
                        }

                        if (cmbBxElevations.SelectedIndex > -1)
                        {
                            tmp = row.GetCell(cmbBxElevations.SelectedIndex).ToString();
                            if (!double.TryParse(tmp, out value))
                                throw new Exception(
                                    $"Couldn't parse elevation at cell[Row:{(index + 1)};Column:{cmbBxElevations.SelectedItem}] which is {tmp}");
                            elevation = new Aran.Aim.DataTypes.ValDistanceVertical(value, uomDistVertical);
                        }

                        UomDistance uomDistance = ConvertUom(uomDistVertical);
                        if (cmbBxHorAccuracy.SelectedIndex > -1)
                        {
                            tmp = row.GetCell(cmbBxHorAccuracy.SelectedIndex).ToString();
                            if (!double.TryParse(tmp, out value))
                                throw new Exception(
                                    $"Couldn't parse Horizontal accuracy at cell[Row:{(index + 1)};Column:{cmbBxHorAccuracy.SelectedItem}] which is {tmp}");
                            horAccuracy = new ValDistance(value, uomDistance);
                        }

                        if (cmbBxVertAccuracy.SelectedIndex > -1)
                        {
                            tmp = row.GetCell(cmbBxVertAccuracy.SelectedIndex).ToString();
                            if (!double.TryParse(tmp, out value))
                                throw new Exception(
                                    $"Couldn't parse Vertical accuracy at cell[Row:{(index + 1)};Column:{cmbBxVertAccuracy.SelectedItem}] which is {tmp}");
                            vertAccuracy = new ValDistance(value, uomDistance);
                        }

                        switch (geomType)
                        {
                            case GeomType.Point:
                                vertStruct.Part[0].HorizontalProjection.Location = new ElevatedPoint();
                                vertStruct.Part[0].HorizontalProjection.Location.Geo.Y = lat;
                                vertStruct.Part[0].HorizontalProjection.Location.Geo.X = longitude;
                                resultList.Add(vertStruct);
                                break;
                            case GeomType.Polyline:
                                if (vertStruct.Part[0].HorizontalProjection.LinearExtent == null)
                                {
                                    vertStruct.Part[0].HorizontalProjection.LinearExtent = new ElevatedCurve();
                                    vertStruct.Part[0].HorizontalProjection.LinearExtent.Geo.Add(new LineString());
                                }

                                vertStruct.Part[0].HorizontalProjection.LinearExtent.Geo[0].Add(
                                    elevation != null
                                        ? new Aran.Geometries.Point(longitude, lat, elevation.Value)
                                        : new Aran.Geometries.Point(longitude, lat));

                                vertStruct.Part[0].HorizontalProjection.LinearExtent.Elevation.Value =
                                    vertStruct.Part[0].HorizontalProjection.LinearExtent.Geo[0].Max(t => t.Z);
                                break;
                            case GeomType.Polygon:
                                if (vertStruct.Part[0].HorizontalProjection.SurfaceExtent == null)
                                {
                                    vertStruct.Part[0].HorizontalProjection.SurfaceExtent = new ElevatedSurface();
                                    vertStruct.Part[0].HorizontalProjection.SurfaceExtent.Geo
                                        .Add(new Aran.Geometries.Polygon());
                                }

                                vertStruct.Part[0].HorizontalProjection.SurfaceExtent.Geo[0].ExteriorRing.Add(
                                    elevation != null
                                        ? new Aran.Geometries.Point(longitude, lat, elevation.Value)
                                        : new Aran.Geometries.Point(longitude, lat));

                                vertStruct.Part[0].HorizontalProjection.SurfaceExtent.Elevation.Value = vertStruct
                                    .Part[0].HorizontalProjection.SurfaceExtent.Geo[0].ExteriorRing.Max(t => t.Z);
                                break;
                        }

                        SetElevAndAccurcies(geomType, vertStruct, elevation, vertAccuracy, horAccuracy);

                        if (cmbBxHeights.SelectedIndex != -1)
                        {
                            tmp = row.GetCell(cmbBxHeights.SelectedIndex).ToString();
                            if (!double.TryParse(tmp, out value))
                                throw new Exception(
                                    $"Couldn't parse Height at cell[Row:{(index + 1)};Column:{cmbBxHeights.SelectedItem}] which is {tmp}");
                            vertStruct.Part[0].VerticalExtent =
                                new Aran.Aim.DataTypes.ValDistance(value, UomDistance.M);
                        }

                        if (cmbBxNote.SelectedItem != null)
                        {
                            var note = new Note();
                            var lingNote = new LinguisticNote
                            {
                                Note = new TextNote()
                                {
                                    Lang = language.ENG,
                                    Value = row.GetCell(
                                                                          cmbBxNote.SelectedIndex)
                                                                      .StringCellValue
                                }
                            };
                            note.TranslatedNote.Add(lingNote);
                            vertStruct.Annotation.Add(note);
                        }

                        if (cmbBxTypes.SelectedIndex != -1)
                        {
                            CodeVerticalStructure vertStructType;
                            tmp = row.GetCell(cmbBxTypes.SelectedIndex).ToString();
                            if (!Enum.TryParse(tmp, true, out vertStructType))
                                throw new Exception(
                                    $"Couldn't parse Type at cell[Row:{(index + 1)};Column:{cmbBxTypes.SelectedItem}] which is {tmp}");
                            vertStruct.Part[0].Type = vertStructType;
                            if (geomType == GeomType.Point)
                                vertStruct.Type = vertStructType;
                        }

                        if (cmbBxLighting.SelectedIndex != -1)
                        {
                            tmp = row.GetCell(cmbBxLighting.SelectedIndex).ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                tmp = tmp.ToLower();
                                if (tmp == "no")
                                    vertStruct.Lighted = false;
                                else if (tmp == "yes")
                                    vertStruct.Lighted = true;
                            }
                            else
                                throw new Exception(
                                    $"Couldn't parse Lighted at cell[Row:{(index + 1)};Column:{cmbBxLighting.SelectedItem}] which is {tmp}");
                        }

                        if (cmbBxGroup.SelectedIndex != -1)
                        {
                            tmp = row.GetCell(cmbBxGroup.SelectedIndex).ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                tmp = tmp.ToLower();
                                if (tmp == "no")
                                    vertStruct.Group = false;
                                else if (tmp == "yes")
                                    vertStruct.Group = true;
                            }
                            else
                                throw new Exception(
                                    $"Couldn't parse Lighted at cell[Row:{(index + 1)};Column:{cmbBxLighting.SelectedItem}] which is {tmp}");
                        }

                        if (cmbBxMarking.SelectedIndex != -1)
                        {
                            tmp = row.GetCell(cmbBxMarking.SelectedIndex).ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                tmp = tmp.ToLower();
                                if (tmp == "no")
                                    vertStruct.MarkingICAOStandard = false;
                                else if (tmp == "yes")
                                    vertStruct.MarkingICAOStandard = true;
                            }
                            else
                                throw new Exception(
                                    $"Couldn't parse Marking at cell[Row:{(index + 1)};Column:{cmbBxMarking.SelectedItem}] which is {tmp}");
                        }


                    }
                }

                resultList.AddRange(obsList.Values.ToList());
            }

            return resultList;
        }

        private UomDistance ConvertUom(UomDistanceVertical uomDistVertical)
        {
            switch (uomDistVertical)
            {
                case UomDistanceVertical.FT:
                    return UomDistance.FT;

                case UomDistanceVertical.M:
                    return UomDistance.M;

                default:
                    throw new ArgumentOutOfRangeException(nameof(uomDistVertical), uomDistVertical, null);
            }
        }

        private void ParseCoordinates(
            IRow row,
            int index,
            int latColumnIndex,
            int longColumnIndex,
            out double lat,
            out double longitude)
        {
            var strValue = row.GetCell(latColumnIndex).StringCellValue;

            var degIndex = strValue.IndexOf('°');
            var minIndex = strValue.IndexOf(@"'");
            var secIndex = strValue.IndexOf('"');
            var sign = strValue[strValue.Length - 1] == 'N' ? 1 : -1;
            double deg = 0, min = 0, sec = 0;
            if (degIndex ==-1)
            {
                deg = double.Parse(strValue.Substring(0, 2), CultureInfo.InvariantCulture);
                min = double.Parse(strValue.Substring(2, 2), CultureInfo.InvariantCulture);
                sec = double.Parse(strValue.Substring(4, 4), CultureInfo.InvariantCulture);
                lat = Dms2Dd(deg, min, sec, sign);
            }
            else
            {
                if (!double.TryParse(strValue.Substring(0, 2), out deg)
                    || !double.TryParse(strValue.Substring(degIndex + 1, minIndex - degIndex - 1), out min)
                    || !double.TryParse(strValue.Substring(minIndex + 1, secIndex - minIndex - 1), out sec))
                    throw new Exception(
                        $"Couldn't parse latitude at cell[Row:{(index + 1)};Column:{latColumnIndex}] which is {strValue}");
                lat = Dms2Dd(deg, min, sec, sign);
            }

            strValue = row.GetCell(longColumnIndex).StringCellValue.Trim();
            degIndex = strValue.IndexOf('°');
            minIndex = strValue.IndexOf(@"'");
            secIndex = strValue.IndexOf('"');
            sign = strValue[strValue.Length - 1] == 'E' ? 1 : -1;
            if (degIndex == -1)
            {
                deg = double.Parse(strValue.Substring(0, 3), CultureInfo.InvariantCulture);
                min = double.Parse(strValue.Substring(3, 2), CultureInfo.InvariantCulture);
                sec = double.Parse(strValue.Substring(5, 4), CultureInfo.InvariantCulture);
                longitude = Dms2Dd(deg, min, sec, sign);
            }
            else
            {
                if (!double.TryParse(strValue.Substring(0, degIndex), out deg)
                || !double.TryParse(strValue.Substring(degIndex + 1, minIndex - degIndex - 1), out min)
                || !double.TryParse(strValue.Substring(minIndex + 1, secIndex - minIndex - 1), out sec))
                    throw new Exception(
                        $"Couldn't parse longitude at cell[Row:{(index + 1)};Column:{longColumnIndex}] which is {strValue}");
                longitude = Dms2Dd(deg, min, sec, sign);
            }
            
        }

        private List<Feature> CreateFeatureListFromGeoDatabase(IFeatureClass featureClass)
        {
            IFeatureCursor featCursor = featureClass.Search(null, true);
            IFeature esriFeature = featCursor.NextFeature();
            List<Feature> resultList = new List<Feature>();
            GeomType geomType = GeomType.Point;
            switch (esriFeature.Shape.GeometryType)
            {
                case esriGeometryType.esriGeometryLine:
                case esriGeometryType.esriGeometryPolyline:
                    geomType = GeomType.Polyline;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    geomType = GeomType.Polygon;
                    break;
                case esriGeometryType.esriGeometryPoint:
                    geomType = GeomType.Point;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            bool ignore;
            while (esriFeature != null)
            {
                ignore = false;
                dynamic aixmFeature = null;
                switch (_selectedFeatType)
                {
                    case FeatureType.VerticalStructure:
                        aixmFeature = CreateVerticalStructure(
                            this.GetFeatValueAsString(esriFeature, cmbBxNames.SelectedItem.ToString()));
                        SetGeometry(geomType, aixmFeature, esriFeature);

                        if (cmbBxIdentifiers.SelectedIndex != -1)
                        {
                            var t = this.GetFeatValueAsString(esriFeature, cmbBxIdentifiers.SelectedItem.ToString());
                            aixmFeature.Identifier = string.IsNullOrEmpty(t) ? Guid.NewGuid() : new Guid(t);
                        }

                        double tmp;
                        ValDistanceVertical elev = null;
                        ValDistance vertAccuracy = null;

                        if (cmbBxElevations.SelectedIndex != -1)
                        {

                            tmp = this.GetFeatValueAsDouble(esriFeature, cmbBxElevations.SelectedItem.ToString());
                            UomDistanceVertical uom = UomDistanceVertical.M;
                            if (cmbBxUom.SelectedIndex != -1)
                            {
                                var k = GetFeatValueAsString(esriFeature, cmbBxUom.SelectedItem.ToString().ToUpper()).ToUpper();
                                if (!Enum.TryParse<UomDistanceVertical>(k, out uom))
                                    throw new Exception($"Couldn't convert uom {cmbBxUom.SelectedItem.ToString()}");
                            }

                            if (!double.IsNaN(tmp))
                            {
                                elev = new Aran.Aim.DataTypes.ValDistanceVertical(tmp, uom);
                                if (cmbBxVertAccuracy.SelectedIndex != -1)
                                {
                                    var s = this.GetFeatValueAsString(
                                        esriFeature,
                                        cmbBxVertAccuracy.SelectedItem.ToString());
                                    if (!string.IsNullOrEmpty(s))
                                        s = s.Substring(0, s.Length - 1);
                                    if (double.TryParse(s, out tmp))
                                        //
                                        vertAccuracy = new Aran.Aim.DataTypes.ValDistance(tmp, UomDistance.M);
                                }

                            }
                        }

                        ValDistance horAccuracy = null;
                        if (cmbBxHorAccuracy.SelectedIndex != -1)
                        {
                            var s = this.GetFeatValueAsString(esriFeature, cmbBxHorAccuracy.SelectedItem.ToString());
                            if (!string.IsNullOrEmpty(s))
                                s = s.Substring(0, s.Length - 1);
                            if (double.TryParse(s, out tmp))
                                //
                                horAccuracy = new Aran.Aim.DataTypes.ValDistance(tmp, UomDistance.M);
                        }

                        if (cmbBxHeights.SelectedIndex != -1)
                        {
                            tmp = this.GetFeatValueAsDouble(esriFeature, cmbBxHeights.SelectedItem.ToString());
                            if (!double.IsNaN(tmp))
                                aixmFeature.Part[0].VerticalExtent =
                                    new Aran.Aim.DataTypes.ValDistance(tmp, UomDistance.M);
                        }

                        if (cmbBxTypes.SelectedIndex != -1)
                        {
                            CodeVerticalStructure vertStructType;
                            if (Enum.TryParse(
                                this.GetFeatValueAsString(esriFeature, cmbBxTypes.SelectedItem.ToString()),
                                true,
                                out vertStructType))
                                aixmFeature.Part[0].Type = vertStructType;
                        }

                        if (cmbBxLighting.SelectedIndex != -1)
                        {
                            var s = this.GetFeatValueAsString(esriFeature, cmbBxLighting.SelectedItem.ToString());
                            if (!string.IsNullOrEmpty(s))
                            {
                                s = s.ToLower();
                                if (s == "no")
                                    aixmFeature.Lighted = false;
                                else if (s == "yes")
                                    aixmFeature.Lighted = true;
                            }
                        }

                        if (cmbBxMarking.SelectedIndex != -1)
                        {
                            var s = this.GetFeatValueAsString(esriFeature, cmbBxMarking.SelectedItem.ToString());
                            if (!string.IsNullOrEmpty(s))
                            {
                                s = s.ToLower();
                                if (s == "no")
                                    aixmFeature.MarkingICAOStandard = false;
                                else if (s == "yes")
                                    aixmFeature.MarkingICAOStandard = true;
                            }
                        }

                        if (cmbBxGroup.SelectedIndex != -1)
                        {
                            var s = this.GetFeatValueAsString(esriFeature, cmbBxGroup.SelectedItem.ToString());
                            if (!string.IsNullOrEmpty(s))
                            {
                                s = s.ToLower();
                                if (s == "no")
                                    aixmFeature.Group = false;
                                else if (s == "yes")
                                    aixmFeature.Group = true;
                            }
                        }

                        SetElevAndAccurcies(geomType, aixmFeature, elev, vertAccuracy, horAccuracy);
                        break;
                    case FeatureType.RunwayProtectArea:
                        aixmFeature = CreateFeature(FeatureType.RunwayProtectArea);
                        
                        SetGeometry(geomType, aixmFeature, esriFeature);

                        if (cmbBxRwyPrtcAreaType.SelectedIndex != -1)
                        {
                            CodeRunwayProtectionArea codeProtectArea;
                            if (Enum.TryParse(
                                this.GetFeatValueAsString(esriFeature, cmbBxRwyPrtcAreaType.SelectedItem.ToString()),
                                true,
                                out codeProtectArea))
                                aixmFeature.Type = codeProtectArea;
                        }

                        if (cmbBxRwyPrtcAreaWidth.SelectedIndex != -1)
                        {
                            tmp = this.GetFeatValueAsDouble(esriFeature, cmbBxRwyPrtcAreaWidth.SelectedItem.ToString());
                            if (!double.IsNaN(tmp))
                                aixmFeature.Width = new ValDistance(tmp, UomDistance.M);
                        }

                        if (cmbBxRwyPrtcAreaLength.SelectedIndex != -1)
                        {
                            tmp = this.GetFeatValueAsDouble(
                                esriFeature,
                                cmbBxRwyPrtcAreaLength.SelectedItem.ToString());
                            if (!double.IsNaN(tmp))
                                aixmFeature.Length = new ValDistance(tmp, UomDistance.M);
                        }

                        break;
                    case FeatureType.RunwayCentrelinePoint:
                        aixmFeature = CreateFeature(FeatureType.RunwayCentrelinePoint);
                        SetGeometry(geomType, aixmFeature, esriFeature);
                        if (cmbBxElevationNavaid.SelectedIndex != -1)
                        {

                            tmp = this.GetFeatValueAsDouble(esriFeature, cmbBxElevationNavaid.SelectedItem.ToString());
                            UomDistanceVertical uom = UomDistanceVertical.M;
                            if (cmbBxUom.SelectedIndex != -1)
                            {
                                var k = GetFeatValueAsString(esriFeature, cmbBxUom.SelectedItem.ToString().ToUpper()).ToUpper();
                                if (!Enum.TryParse<UomDistanceVertical>(k, out uom))
                                    throw new Exception($"Couldn't convert uom {cmbBxUom.SelectedItem.ToString()}");
                            }

                            if (!double.IsNaN(tmp))
                            {
                                aixmFeature.Location.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical(tmp, uom);
                                //if (cmbBxVertAccuracy.SelectedIndex != -1)
                                //{
                                //    var s = this.GetFeatValueAsString(
                                //        esriFeature,
                                //        cmbBxVertAccuracy.SelectedItem.ToString());
                                //    if (!string.IsNullOrEmpty(s))
                                //        s = s.Substring(0, s.Length - 1);
                                //    if (double.TryParse(s, out tmp))
                                //        //
                                //        vertAccuracy = new Aran.Aim.DataTypes.ValDistance(tmp, UomDistance.M);
                                //}
                            }
                        }
                        if(cmbBxNameNavaid.SelectedIndex != -1)
                        {
                            aixmFeature.Designator = this.GetFeatValueAsString(esriFeature, cmbBxNameNavaid.SelectedItem.ToString());
                        }                        
                        break;
                    case FeatureType.GuidanceLine:
                        aixmFeature =  CreateFeature(FeatureType.GuidanceLine);
                        aixmFeature.Designatore = this.GetFeatValueAsString(esriFeature, cmbBxGuidanceLineDsg.SelectedItem.ToString());
                        Feature foundFeature = null;
                        if (chckBxGuidanceLineMerge.Checked)
                            foundFeature = resultList.FirstOrDefault(
                                ft => (ft as GuidanceLine).Designator?.Substring(0, _mergeLetterCount)
                                      == (aixmFeature as GuidanceLine).Designator?.Substring(0, _mergeLetterCount));

                        if (foundFeature != null)
                        {
                            (foundFeature as GuidanceLine).Extent.Geo.Add(
                                ConvertFromEsriGeom.ToPolyline((IPolyline)esriFeature.Shape, true));
                            ignore = true;
                        }
                        else
                            SetGeometry(geomType, aixmFeature, esriFeature);

                        break;
                    case FeatureType.ApronElement:
                        var apron = (Apron)CreateFeature(FeatureType.Apron);
                        apron.Name = GetFeatValueAsString(esriFeature, cmbBxNameApron.SelectedItem.ToString());
                        aixmFeature = CreateFeature(_selectedFeatType);
                        aixmFeature.AssociatedApron = apron.GetFeatureRef();
                        if (!ignore)
                            resultList.Add((Feature)apron);
                        SetGeometry(geomType, aixmFeature, esriFeature);
                        break;
                    case FeatureType.DME:
                    case FeatureType.VOR:
                    case FeatureType.Glidepath:
                    case FeatureType.NDB:
                    case FeatureType.Localizer:
                        aixmFeature = CreateFeature(_selectedFeatType);
                        SetGeometry(geomType, aixmFeature, esriFeature);
                        if (cmbBxElevationNavaid.SelectedIndex != -1)
                        {
                            tmp = this.GetFeatValueAsDouble(esriFeature, cmbBxElevationNavaid.SelectedItem.ToString());
                            UomDistanceVertical uom = UomDistanceVertical.M;
                            if (!double.IsNaN(tmp))
                            {
                                aixmFeature.Location.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical(tmp, uom);
                                if (cmbBxVertAccuracyNavaid.SelectedIndex != -1)
                                {
                                    var s = this.GetFeatValueAsString(esriFeature, cmbBxVertAccuracyNavaid.SelectedItem.ToString());
                                    if (double.TryParse(s, out tmp))
                                        aixmFeature.Location.VerticalAccuracy = new Aran.Aim.DataTypes.ValDistance(tmp, UomDistance.M);
                                }
                            }
                        }

                        if (cmbBxHorAccuracyNavaid.SelectedIndex != -1)
                        {
                            var s = this.GetFeatValueAsString(esriFeature, cmbBxHorAccuracyNavaid.SelectedItem.ToString());
                            if (double.TryParse(s, out tmp))
                                aixmFeature.Location.HorizontalAccuracy = new Aran.Aim.DataTypes.ValDistance(tmp, UomDistance.M);
                        }
                        aixmFeature.Name = this.GetFeatValueAsString(esriFeature, cmbBxNameNavaid.SelectedItem.ToString());
                        break;
                    case FeatureType.TaxiHoldingPosition:
                        aixmFeature = CreateFeature(_selectedFeatType);
                        AircraftStand aircraftStand = new AircraftStand();
                        SetGeometry(geomType, aixmFeature, esriFeature);
                        if (cmbBxElevationTaxiHoldingPosition.SelectedIndex != -1)
                        {
                            tmp = this.GetFeatValueAsDouble(esriFeature, cmbBxElevationTaxiHoldingPosition.SelectedItem.ToString());
                            UomDistanceVertical uom = UomDistanceVertical.M;
                            if (!double.IsNaN(tmp))
                            {
                                aixmFeature.Location.Elevation = new Aran.Aim.DataTypes.ValDistanceVertical(tmp, uom);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Population of {_selectedFeatType} is not implemented");
                }

                if (!ignore)
                    resultList.Add((Feature)aixmFeature);
                esriFeature = featCursor.NextFeature();
            }

            return resultList;
        }

        private Feature CreateFeature(FeatureType featureType)
        {
            var result = AimObjectFactory.CreateFeature(featureType);
            result.TimeSlice = new Aran.Aim.DataTypes.TimeSlice
            {
                FeatureLifetime = new Aran.Aim.DataTypes.TimePeriod(new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month,
                                                   dateTimePicker1.Value.Day, 0, 0, 0)),
                ValidTime = new TimePeriod(new DateTime(dateTimePicker1.Value.Year,
                                               dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, 0, 0, 0)),
                Interpretation = TimeSliceInterpretationType.BASELINE,
                SequenceNumber = 1
            };
            result.Identifier = Guid.NewGuid();
            return result;
        }

        private void SetGeometry(GeomType geomType, Feature feature, IFeature esriFeat)
        {
            IPolygon esriGeom;
            switch (_selectedFeatType)
            {
                case FeatureType.VerticalStructure:
                    SetObstacleGeometry(geomType, feature, esriFeat);
                    break;
                case FeatureType.RunwayProtectArea:
                case FeatureType.ApronElement:
                    esriGeom = (IPolygon)esriFeat.Shape;
                    ITopologicalOperator2 pTopo = esriGeom as ITopologicalOperator2;
                    pTopo.IsKnownSimple_2 = false;
                    pTopo.Simplify();
                    var geom = ConvertFromEsriGeom.ToPolygonGeo(esriGeom);
                    dynamic dynamicFeature = feature;
                    dynamicFeature.Extent = new ElevatedSurface();
                    if (geom is MultiPolygon)
                        dynamicFeature.Extent.Geo.Assign(geom);
                    else if (geom is Aran.Geometries.Polygon)
                        dynamicFeature.Extent.Geo.Add((Aran.Geometries.Polygon)geom);
                    break;
                case FeatureType.GuidanceLine:
                    GuidanceLine guidanceLine = (GuidanceLine)feature;
                    guidanceLine.Extent = new ElevatedCurve();
                    guidanceLine.Extent.Geo.Assign(ConvertFromEsriGeom.ToPolyline((IPolyline)esriFeat.Shape, true));
                    break;
                case FeatureType.DME:
                case FeatureType.NDB:
                case FeatureType.VOR:
                case FeatureType.Glidepath:
                case FeatureType.Localizer:
                case FeatureType.TaxiHoldingPosition:
                case FeatureType.RunwayCentrelinePoint:
                    dynamic feat = feature;
                    feat.Location = new ElevatedPoint();
                    feat.Location.Geo.Assign(ConvertFromEsriGeom.ToPoint((IPoint)esriFeat.Shape));
                    break;
                default:
                    throw new NotImplementedException(
                        $"Comboboxes in property panel for {_selectedFeatType} are not fullfilled");
            }
        }

        private static void SetObstacleGeometry(GeomType geomType, Feature feature, IFeature feat)
        {
            VerticalStructure vertStruct = (VerticalStructure)feature;
            switch (geomType)
            {
                case GeomType.Point:
                    vertStruct.Part[0].HorizontalProjection.Location = new ElevatedPoint();
                    vertStruct.Part[0].HorizontalProjection.Location.Geo
                        .Assign(ConvertFromEsriGeom.ToPoint((IPoint)feat.Shape));
                    break;
                case GeomType.Polyline:
                    vertStruct.Part[0].HorizontalProjection.LinearExtent = new ElevatedCurve();
                    vertStruct.Part[0].HorizontalProjection.LinearExtent.Geo.Assign(
                        ConvertFromEsriGeom.ToPolyline((IPolyline)feat.Shape, true));
                    break;
                case GeomType.Polygon:
                    var esriGeom = (IPolygon)feat.Shape;
                    ITopologicalOperator2 pTopo = esriGeom as ITopologicalOperator2;
                    pTopo.IsKnownSimple_2 = false;
                    pTopo.Simplify();
                    var geom = ConvertFromEsriGeom.ToPolygonGeo(esriGeom);
                    vertStruct.Part[0].HorizontalProjection.SurfaceExtent = new ElevatedSurface();
                    if (geom is MultiPolygon)
                        vertStruct.Part[0].HorizontalProjection.SurfaceExtent.Geo.Assign(geom);
                    else if (geom is Aran.Geometries.Polygon)
                        vertStruct.Part[0].HorizontalProjection.SurfaceExtent.Geo.Add((Aran.Geometries.Polygon)geom);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetElevAndAccurcies(
            GeomType geomType,
            VerticalStructure vertStruct,
            ValDistanceVertical elev,
            ValDistance vertAccuracy,
            ValDistance horAccuracy)
        {
            switch (geomType)
            {
                case GeomType.Point:
                    vertStruct.Part[0].HorizontalProjection.Location.Elevation = elev;
                    vertStruct.Part[0].HorizontalProjection.Location.VerticalAccuracy = vertAccuracy;
                    vertStruct.Part[0].HorizontalProjection.Location.HorizontalAccuracy = horAccuracy;
                    if (vertStruct.Part[0].VerticalExtent != null)
                        vertStruct.Part[0].VerticalExtentAccuracy = vertAccuracy;
                    break;
                case GeomType.Polyline:
                    vertStruct.Part[0].HorizontalProjection.LinearExtent.Elevation = elev;
                    vertStruct.Part[0].HorizontalProjection.LinearExtent.VerticalAccuracy = vertAccuracy;
                    vertStruct.Part[0].HorizontalProjection.LinearExtent.HorizontalAccuracy = horAccuracy;
                    if (vertStruct.Part[0].VerticalExtent != null)
                        vertStruct.Part[0].VerticalExtentAccuracy = vertAccuracy;
                    break;
                case GeomType.Polygon:
                    vertStruct.Part[0].HorizontalProjection.SurfaceExtent.Elevation = elev;
                    vertStruct.Part[0].HorizontalProjection.SurfaceExtent.VerticalAccuracy = vertAccuracy;
                    vertStruct.Part[0].HorizontalProjection.SurfaceExtent.HorizontalAccuracy = horAccuracy;
                    if (vertStruct.Part[0].VerticalExtent != null)
                        vertStruct.Part[0].VerticalExtentAccuracy = vertAccuracy;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private VerticalStructure CreateVerticalStructure(string name)
        {
            VerticalStructure vertStructure = new VerticalStructure
            {
                TimeSlice =
                                                          new Aran.Aim.DataTypes.TimeSlice
                                                          {
                                                              FeatureLifetime
                                                                      =
                                                                      new
                                                                          Aran
                                                                          .Aim
                                                                          .DataTypes
                                                                          .TimePeriod(
                                                                              new
                                                                                  DateTime(
                                                                                      dateTimePicker1
                                                                                          .Value
                                                                                          .Year,
                                                                                      dateTimePicker1
                                                                                          .Value
                                                                                          .Month,
                                                                                      dateTimePicker1
                                                                                          .Value
                                                                                          .Day,
                                                                                      0,
                                                                                      0,
                                                                                      0)),
                                                              ValidTime
                                                                      =
                                                                      new
                                                                          Aran
                                                                          .Aim
                                                                          .DataTypes
                                                                          .TimePeriod(
                                                                              new
                                                                                  DateTime(
                                                                                      dateTimePicker1
                                                                                          .Value
                                                                                          .Year,
                                                                                      dateTimePicker1
                                                                                          .Value
                                                                                          .Month,
                                                                                      dateTimePicker1
                                                                                          .Value
                                                                                          .Day,
                                                                                      0,
                                                                                      0,
                                                                                      0)),
                                                              Interpretation
                                                                      = TimeSliceInterpretationType
                                                                          .BASELINE,
                                                              SequenceNumber
                                                                      = 1
                                                          },
                Name = name,
                Group = false
            };
            if (cmbBxIdentifiers.SelectedItem == null)
                vertStructure.Identifier = Guid.NewGuid();
            VerticalStructurePart vertStructPart =
                new VerticalStructurePart { HorizontalProjection = new VerticalStructurePartGeometry() };
            vertStructure.Part.Add(vertStructPart);

            return vertStructure;
        }

        private GuidanceLine CreateGuidanceLine(string designator)
        {

            GuidanceLine result = new Aran.Aim.Features.GuidanceLine
            {
                TimeSlice =
                                              new Aran.Aim.DataTypes.TimeSlice
                                              {
                                                  FeatureLifetime
                                                          =
                                                          new
                                                              Aran
                                                              .Aim
                                                              .DataTypes
                                                              .TimePeriod(
                                                                  new
                                                                      DateTime(
                                                                          dateTimePicker1
                                                                              .Value
                                                                              .Year,
                                                                          dateTimePicker1
                                                                              .Value
                                                                              .Month,
                                                                          dateTimePicker1
                                                                              .Value
                                                                              .Day,
                                                                          0,
                                                                          0,
                                                                          0)),
                                                  ValidTime
                                                          =
                                                          new
                                                              Aran
                                                              .Aim
                                                              .DataTypes
                                                              .TimePeriod(
                                                                  new
                                                                      DateTime(
                                                                          dateTimePicker1
                                                                              .Value
                                                                              .Year,
                                                                          dateTimePicker1
                                                                              .Value
                                                                              .Month,
                                                                          dateTimePicker1
                                                                              .Value
                                                                              .Day,
                                                                          0,
                                                                          0,
                                                                          0)),
                                                  Interpretation
                                                          = TimeSliceInterpretationType
                                                              .BASELINE,
                                                  SequenceNumber
                                                          = 1
                                              },
                Designator = designator,
                Identifier = Guid.NewGuid()
            };
            return result;
        }

        private string GetFeatValueAsString(IFeature feat, string fieldName)
        {
            object tmp = feat.get_Value(feat.Fields.FindField(fieldName));
            if (DBNull.Value.Equals(tmp))
            {
                return string.Empty;
            }

            //if (tmp.ToString().ToLower() == "lightpole")
            //    return "pole";
            return tmp.ToString().Trim().ToString();
        }

        private double GetFeatValueAsDouble(IFeature feat, string fieldName)
        {
            string tmp = this.GetFeatValueAsString(feat, fieldName);
            tmp = tmp.Replace(',', '.');
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            ci.NumberFormat.NumberDecimalSeparator = ".";
            if (double.TryParse(tmp, NumberStyles.Any, ci, out var result))
            {
                return result;
            }

            return double.NaN;
        }

        private void BtnResultFilePathClick(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Aixm message|*.xml";
                saveFileDialog.DefaultExt = "xml";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtBxResultFilePath.Text = saveFileDialog.FileName;
            }
        }

        private void CmbBxLayersSelectedIndexChanged(object sender, EventArgs e)
        {
            IFeatureClass featureClass = _featureWorkspace.OpenFeatureClass(cmbBxLayers.SelectedItem.ToString());
            IFields fields = featureClass.Fields;
            FillPropertyComboboxes(fields);
        }

        private void FillPropertyComboboxes(IFields fields)
        {
            switch (_selectedFeatType)
            {
                case FeatureType.VerticalStructure:
                    FillPropertyOfObstacle(fields);
                    break;
                case FeatureType.RunwayProtectArea:
                    FillPropertyOfRwyProtectArea(fields);
                    break;
                case FeatureType.GuidanceLine:
                    FillPropertyOfGuidanceLine(fields);
                    break;
                case FeatureType.ApronElement:
                    FillPropertyOfApron(fields);
                    break;
                case FeatureType.TaxiHoldingPosition:
                    FillPropertyOfTaxiHoldingPosition(fields);
                    break;
                case FeatureType.DME:
                case FeatureType.VOR:
                case FeatureType.Glidepath:
                case FeatureType.NDB:
                case FeatureType.Localizer:
                case FeatureType.RunwayCentrelinePoint:
                    FillPropertyOfNavaid(fields);
                    break;

                default:
                    throw new NotImplementedException(
                        $"Comboboxes in property panel for {_selectedFeatType} are not fullfilled");
            }
        }

        private void FillPropertyOfTaxiHoldingPosition(IFields fields)
        {
            cmbBxElevationTaxiHoldingPosition.Items.Clear();
            if (fields == null)
                return;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                // Get the field at the given index.
                var field = fields.get_Field(i);
                cmbBxElevationTaxiHoldingPosition.Items.Add(field.Name);
            }

        }

        private void FillPropertyOfApron(IFields fields)
        {
            cmbBxNameApron.Items.Clear();
            if (fields == null)
                return;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                // Get the field at the given index.
                var field = fields.get_Field(i);
                cmbBxNameApron.Items.Add(field.Name);
            }
            cmbBxNames.SelectedIndex = 0;
        }

        private void FillPropertyOfObstacle(IFields fields)
        {
            ClearObstaclePropertyComboboxes();
            // On a zero-based index, iterate through the fields in the collection.
            if (fields == null)
                return;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                // Get the field at the given index.
                var field = fields.get_Field(i);
                cmbBxNames.Items.Add(field.Name);
                cmbBxIdentifiers.Items.Add(field.Name);
                cmbBxTypes.Items.Add(field.Name);
                cmbBxGroup.Items.Add(field.Name);
                cmbBxElevations.Items.Add(field.Name);
                cmbBxHeights.Items.Add(field.Name);
                cmbBxUom.Items.Add(field.Name);
                cmbBxLighting.Items.Add(field.Name);
                cmbBxMarking.Items.Add(field.Name);
                cmbBxHorAccuracy.Items.Add(field.Name);
                cmbBxVertAccuracy.Items.Add(field.Name);
            }

            cmbBxNames.SelectedIndex = 0;
        }

        private void FillPropertyOfRwyProtectArea(IFields fields)
        {
            cmbBxRwyPrtcAreaType.Items.Clear();
            cmbBxRwyPrtcAreaWidth.Items.Clear();
            cmbBxRwyPrtcAreaLength.Items.Clear();

            // On a zero-based index, iterate through the fields in the collection.
            if (fields == null)
                return;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                // Get the field at the given index.
                var field = fields.Field[i];
                cmbBxRwyPrtcAreaType.Items.Add(field.Name);
                cmbBxRwyPrtcAreaWidth.Items.Add(field.Name);
                cmbBxRwyPrtcAreaLength.Items.Add(field.Name);
            }
        }

        private void FillPropertyOfGuidanceLine(IFields fields)
        {
            cmbBxGuidanceLineDsg.Items.Clear();
            // On a zero-based index, iterate through the fields in the collection.
            if (fields == null)
                return;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                // Get the field at the given index.
                var field = fields.get_Field(i);
                cmbBxGuidanceLineDsg.Items.Add(field.Name);
            }
        }

        private void ClearObstaclePropertyComboboxes()
        {
            cmbBxNames.Items.Clear();

            cmbBxIdentifiers.Items.Clear();

            cmbBxTypes.Items.Clear();

            cmbBxElevations.Items.Clear();

            cmbBxHeights.Items.Clear();

            cmbBxUom.Items.Clear();

            cmbBxLatitude.Items.Clear();

            cmbBxLongitude.Items.Clear();

            cmbBxLighting.Items.Clear();

            cmbBxMarking.Items.Clear();

            cmbBxGroup.Items.Clear();

            cmbBxHorAccuracy.Items.Clear();

            cmbBxVertAccuracy.Items.Clear();
        }

        private void RadBtnSourcExcelCheckedChanged(object sender, EventArgs e)
        {
            cmbBxLayers.Enabled = !radBtnSourceExcel.Checked;
            if (radBtnSourceExcel.Checked)
            {
                cmbBxNames.Items.Clear();
                cmbBxIdentifiers.Items.Clear();
                cmbBxTypes.Items.Clear();
                cmbBxElevations.Items.Clear();
                cmbBxHeights.Items.Clear();
                cmbBxUom.Items.Clear();
                cmbBxLatitude.Items.Clear();
                cmbBxLongitude.Items.Clear();
                cmbBxLighting.Items.Clear();
                cmbBxMarking.Items.Clear();
                cmbBxGroup.Items.Clear();
                cmbBxHorAccuracy.Items.Clear();
                cmbBxVertAccuracy.Items.Clear();
                cmbBxNote.Items.Clear();
            }
            else
            {
                cmbBxLayers.Enabled = true;

                SourceType sourceType = SourceType.Gdb;
                if (radBtnShapeFile.Checked)
                    sourceType = SourceType.Shape;
                else if (radBtnSourceMdb.Checked)
                    sourceType = SourceType.Access;

                GetLayers(txtBxPath.Text, sourceType);
            }
        }

        private double Dms2Dd(double xDeg, double xMin, double xSec, int Sign)
        {
            var x = System.Math.Round(
                Sign * (System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60.0) + System.Math.Abs(xSec / 3600.0)),
                10);
            return x;
        }

        private void CmbBxFeatTypesSelectedIndexChanged(object sender, EventArgs e)
        {
            IFields fields = null;
            if (cmbBxLayers.SelectedItem != null)
            {
                IFeatureClass featureClass = _featureWorkspace.OpenFeatureClass(cmbBxLayers.SelectedItem.ToString());
                fields = featureClass.Fields;
            }

            if (grpBxProperties.Controls.Count > 0)
            {
                grpBxProperties.Controls[0].Dock = DockStyle.None;
            }

            grpBxProperties.Controls.Clear();
            _selectedFeatType = _featureTypeList[cmbBxFeatTypes.SelectedIndex];
            switch (_selectedFeatType)
            {
                case FeatureType.VerticalStructure:
                    grpBxProperties.Controls.Add(pnlObstacleProps);
                    FillPropertyOfObstacle(fields);
                    break;
                case FeatureType.RunwayProtectArea:
                    grpBxProperties.Controls.Add(pnlRwyPrtcAreaProps);
                    FillPropertyOfRwyProtectArea(fields);
                    break;
                case FeatureType.ApronElement:
                    grpBxProperties.Controls.Add(pnlApronProps);
                    FillPropertyOfApron(fields);
                    break;
                case FeatureType.TaxiHoldingPosition:
                    grpBxProperties.Controls.Add(pnlTaxiHoldingPositionProps);
                    FillPropertyOfTaxiHoldingPosition(fields);
                    break;
                case FeatureType.GuidanceLine:
                    grpBxProperties.Controls.Add(pnlGuidanceLineProps);
                    FillPropertyOfGuidanceLine(fields);
                    break;
                case FeatureType.DesignatedPoint:
                    grpBxProperties.Controls.Add(pnlDsgPntProps);
                    FillPropertyOfDsgPnt(fields);
                    break;
                case FeatureType.DME:
                case FeatureType.VOR:
                case FeatureType.Glidepath:
                case FeatureType.NDB:
                case FeatureType.Localizer:
                case FeatureType.RunwayCentrelinePoint:
                    grpBxProperties.Controls.Add(pnlNavaid);
                    FillPropertyOfNavaid(fields);
                    break;

                default:
                    throw new NotImplementedException(
                        $"Property panel for {_selectedFeatType} is not added into properties groupbox");
            }

            grpBxProperties.Height = grpBxProperties.Controls[0].Height + 15;
            grpBxProperties.Controls[0].Dock = DockStyle.Fill;
        }

        private void FillPropertyOfNavaid(IFields fields)
        {
            if (radBtnSourceExcel.Checked)
                return;
            cmbBxNameNavaid.Items.Clear();
            cmbBxElevationNavaid.Items.Clear();
            cmbBxHorAccuracyNavaid.Items.Clear();
            cmbBxVertAccuracyNavaid.Items.Clear();
            // On a zero-based index, iterate through the fields in the collection.
            if (fields == null)
                return;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                var field = fields.Field[i];
                cmbBxNameNavaid.Items.Add(field.Name);
                cmbBxElevationNavaid.Items.Add(field.Name);
                cmbBxHorAccuracyNavaid.Items.Add(field.Name);
                cmbBxVertAccuracyNavaid.Items.Add(field.Name);
            }

        }

        private void FillPropertyOfDsgPnt(IFields fields)
        {
            if (radBtnSourceExcel.Checked)
                return;
            cmbBxNameDsgPnt.Items.Clear();
            cmbBxLatDsgPnt.Items.Clear();
            cmbBxLongDsgPnt.Items.Clear();

            // On a zero-based index, iterate through the fields in the collection.
            if (fields == null)
                return;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                // Get the field at the given index.
                var field = fields.Field[i];
                cmbBxNameDsgPnt.Items.Add(field.Name);
                cmbBxLatDsgPnt.Items.Add(field.Name);
                cmbBxLongDsgPnt.Items.Add(field.Name);
            }
        }

        private void ChckBxGuidanceLineMergeCheckedChanged(object sender, EventArgs e)
        {
            nmrcUpDwnGuidanceLineMergeLetters.Enabled = chckBxGuidanceLineMerge.Checked;
        }

        private void NmrcUpDwnGuidanceLineMergeLettersValueChanged(object sender, EventArgs e)
        {
            _mergeLetterCount = (int)nmrcUpDwnGuidanceLineMergeLetters.Value;
        }
    }
}