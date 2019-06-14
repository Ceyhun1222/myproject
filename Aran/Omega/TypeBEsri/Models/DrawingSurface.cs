using System;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Converters;
using Aran.Omega.TypeBEsri.View;
using Aran.Omega.TypeBEsri.ViewModels;
using Aran.Panda.Common;
using Aran.Panda.Constants;
using System.Windows.Input;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace Aran.Omega.TypeBEsri.Models
{
    public class DrawingSurface : ViewModel
    {
        private int _selectedObstacleHandle;
        private ObstacleReport _selectedObstacle;
        private int _selectedExactVertex;
        private readonly FillSymbol _polygonFillSymbol;
        private readonly PointSymbol _exactVertexSymbol;

        public DrawingSurface(RunwayConstants rwyConstant)
        {
            SurfaceType = rwyConstant.Surface;
            ViewCaption = rwyConstant.SurfaceName;

            _polygonFillSymbol = new FillSymbol();
            _polygonFillSymbol.Color = 242424;
            _polygonFillSymbol.Outline = new LineSymbol(eLineStyle.slsDash,
                Aran.Panda.Common.ARANFunctions.RGB(255, 0, 0), 4);
            _polygonFillSymbol.Style = eFillStyle.sfsBackwardDiagonal;

            _exactVertexSymbol = new PointSymbol(ePointStyle.smsX, ARANFunctions.RGB(0, 0, 0), 5);

            InfoCommand = new RelayCommand(new Action<object>(infoCommand_onClick));
            ExportToShape = new RelayCommand(new Action<object>(exportToShape_onClick));
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (_isSelected)
                    _surfaceBase.Draw(_isSelected);
                else
                    _surfaceBase.ClearSelected();

                NotifyPropertyChanged("IsSelected");
            }
        }

        private SurfaceBase _surfaceBase;
        public SurfaceBase SurfaceBase
        {
            get { return _surfaceBase; }
            set
            {
                _surfaceBase = value;
                _surfaceBase.Draw(false);
            }
        }

        public ObstacleReport SelectedObstacle
        {
            get { return _selectedObstacle; }
            set
            {
                _selectedObstacle = value;
                ClearSelectedObstacle();

                if (_selectedObstacle == null)
                    return;

                if (_selectedObstacle.GeomPrj is Aran.Geometries.Point)
                    _selectedObstacleHandle = GlobalParams.UI.DrawPoint(_selectedObstacle.GeomPrj as Aran.Geometries.Point, Aran.Panda.Common.ARANFunctions.RGB(255, 0, 0), Aran.AranEnvironment.Symbols.ePointStyle.smsCircle);
                
                else if (_selectedObstacle.GeomPrj is MultiLineString)
                    _selectedObstacleHandle =GlobalParams.UI.DrawMultiLineString(_selectedObstacle.GeomPrj as MultiLineString,
                            ARANFunctions.RGB(255, 0, 0), 4);

                else if (_selectedObstacle.GeomPrj is Aran.Geometries.MultiPolygon)
                    _selectedObstacleHandle = GlobalParams.UI.DrawMultiPolygon(
                        _selectedObstacle.GeomPrj as MultiPolygon, _polygonFillSymbol);

                if (SelectedObstacle.ExactVertexGeom != null)
                     _selectedExactVertex = GlobalParams.UI.DrawPoint(SelectedObstacle.ExactVertexGeom, _exactVertexSymbol);
            }
        }

        public SurfaceType SurfaceType { get; set; }
        public string ViewCaption { get; set; }

        public void SetDrawingParam(SurfaceBase surface, FillSymbol defaultSymbol, FillSymbol selectedSymbol)
        {
            surface.DefaultSymbol = defaultSymbol;
            surface.SelectedSymbol = selectedSymbol;
            SurfaceBase = surface;
        }

        public void ClearSelectedObstacle()
        {
            GlobalParams.UI.SafeDeleteGraphic(_selectedObstacleHandle);
            GlobalParams.UI.SafeDeleteGraphic(_selectedExactVertex);
        }

        public ICommand InfoCommand { get; set; }
        public ICommand ExportToShape { get; set; }

        private void infoCommand_onClick(object obj)
        {
            var abstractView = new AbstractView();
            abstractView.LoadSurfaces(SurfaceBase.PropertyList, ViewCaption);

            var helper = new WindowInteropHelper(abstractView);
            ElementHost.EnableModelessKeyboardInterop(abstractView);
            helper.Owner = GlobalParams.AranEnvironment.Win32Window.Handle;
            abstractView.ShowInTaskbar = false;
            
            if (GlobalParams.TypeBView.Left < abstractView.Width)
                abstractView.Left = GlobalParams.TypeBView.Left + GlobalParams.TypeBView.Width + 4;
            else
                abstractView.Left = GlobalParams.TypeBView.Left - abstractView.Width - 4;
            
            abstractView.Top = GlobalParams.TypeBView.Top+5;
            
            // hide from taskbar and alt-tab list
            abstractView.Show();
        }

        private void exportToShape_onClick(object obj) 
        {
            string fileNamePath = null;
			SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = " Projection File File(*.shp) |*.shp";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                fileNamePath = saveFileDialog.FileName;
            }
            else
            {
                fileNamePath = null;
				return;
            }
            int index = fileNamePath.LastIndexOf('\\');
            string folder = fileNamePath.Substring(0, index);
            string nameOfShapeFile = fileNamePath.Substring(index + 1);
            string shapeFieldName = "Shape";
            try
            {
                IWorkspaceFactory workspaceFactory = null;
                workspaceFactory = new ShapefileWorkspaceFactory();
                IWorkspace workspace = workspaceFactory.OpenFromFile(folder, 0);
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                IField field = new Field();///###########
                IFieldEdit fieldEdit = (IFieldEdit)field;
                fieldEdit.Name_2 = "Shape";
                fieldEdit.Type_2 = (esriFieldType.esriFieldTypeGeometry);

                IGeometryDef geomDef = new GeometryDef();///#########
                IGeometryDefEdit geomDefEdit = (IGeometryDefEdit)geomDef;
                geomDefEdit.HasZ_2 = true;
                
                geomDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                
                fieldEdit.GeometryDef_2 = geomDef;
                fieldsEdit.AddField(field);
                //Add another miscellaneous text field
                
                IFeatureClass featClass = null;
                featClass = featureWorkspace.CreateFeatureClass(nameOfShapeFile, fields, null, null, esriFeatureType.esriFTSimple, shapeFieldName, "");

                IFeatureClass featureClass = featClass;

                IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)workspace;
                workspaceEdit.StartEditing(false);
                workspaceEdit.StartEditOperation();

                IPointCollection ptColl = new ESRI.ArcGIS.Geometry.Ring();
                foreach (Aran.Geometries.Point pt in this.SurfaceBase.Geo.ToMultiPoint())
                {
                    //var tmpPt = GlobalParams.SpatialRefOperation.ToGeo(pt);
                    var tmpPt = pt;
                    IPoint esriPt = new ESRI.ArcGIS.Geometry.Point();
                    esriPt.PutCoords(tmpPt.X, tmpPt.Y);
                    IZAware zAware = (IZAware)esriPt;
                    zAware.ZAware = true;
                    esriPt.Z = pt.Z;
                    ptColl.AddPoint(esriPt);
                }

                var zAwareRing = (IZAware)ptColl;
                zAwareRing.ZAware = true;

                IGeometryCollection geomCollection = new ESRI.ArcGIS.Geometry.Polygon() as IGeometryCollection;
                IZAware zAware1 = (IZAware)geomCollection;
                zAware1.ZAware = true;
                geomCollection.AddGeometry(ptColl as ESRI.ArcGIS.Geometry.IGeometry);
               
                IFeature feat = featureClass.CreateFeature();
                feat.set_Value(1, geomCollection);
                feat.Store();
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
                MessageBox.Show("Succesfully created shape file");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
