using System;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.Converters;
using Aran.Omega.View;
using Aran.Omega.ViewModels;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System.Windows.Input;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using Aran.Geometries.Operators;

namespace Aran.Omega.Models
{
    public class DrawingSurface : ViewModel
    {
        private int _selectedObstacleHandle, _selectedBufferHandle;
        private ObstacleReport _selectedObstacle;
        private int _selectedExactVertex;
        private readonly FillSymbol _polygonFillSymbol, _polygonBufferFillSymbol;
        private readonly PointSymbol _exactVertexSymbol;
        private readonly PointSymbol _ptSymbol;
        private bool _isSelected;
        private SurfaceBase _surfaceBase;

        public DrawingSurface(string viewCaption)
        {
            ViewCaption = viewCaption;
            _polygonFillSymbol = new FillSymbol
            {
                Color = 242424,
                Outline = new LineSymbol(eLineStyle.slsDash,
                    Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 12),
                Style = eFillStyle.sfsNull
            };

            _polygonBufferFillSymbol = new FillSymbol
            {
                Color = 242424,
                Outline = new LineSymbol(eLineStyle.slsDash, Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0), 12)
            };
            _polygonBufferFillSymbol.Size = 12;
            _polygonBufferFillSymbol.Style = eFillStyle.sfsNull;

            _ptSymbol = new PointSymbol();
            _ptSymbol.Color = Aran.PANDA.Common.ARANFunctions.RGB(255, 0, 0);
            _ptSymbol.Size = 14;
            _ptSymbol.Style = ePointStyle.smsCircle;

            _exactVertexSymbol = new PointSymbol(ePointStyle.smsX, ARANFunctions.RGB(0, 0, 0), 13);
        }

        public DrawingSurface(RunwayConstants rwyConstant) : this(rwyConstant.SurfaceName)
        {
            SurfaceType = rwyConstant.Surface;
        }

        public DrawingSurface(RunwayConstants rwyConstant, RwyDirClass rwyDirection)
            : this(rwyConstant.SurfaceName)
        {
            SurfaceType = rwyConstant.Surface;
            RwyDirClass = rwyDirection;
        }

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
					_selectedObstacleHandle =
						GlobalParams.UI.DrawPoint(_selectedObstacle.GeomPrj as Aran.Geometries.Point, _ptSymbol);

				else if (_selectedObstacle.GeomPrj is MultiLineString)
					_selectedObstacleHandle = GlobalParams.UI.DrawMultiLineString(
						_selectedObstacle.GeomPrj as MultiLineString,
						4, ARANFunctions.RGB(255, 0, 0));

				else if (_selectedObstacle.GeomPrj is Aran.Geometries.MultiPolygon)
				{
					_selectedObstacleHandle = GlobalParams.UI.DrawMultiPolygon(
						_selectedObstacle.GeomPrj as MultiPolygon, _polygonFillSymbol, true, false);
				}

                if (!(_selectedObstacle.GeomPrj is Aran.Geometries.Point) && SelectedObstacle.ExactVertexGeom != null)
                    _selectedExactVertex =
                        GlobalParams.UI.DrawPoint(SelectedObstacle.ExactVertexGeom, _exactVertexSymbol);
            }
        }

        public ObstacleReport SelectedEtodObstacle
        {
            get { return _selectedObstacle; }
            set
            {
                _selectedObstacle = value;
                ClearSelectedObstacle();

                if (_selectedObstacle == null)
                    return;

                if (_selectedObstacle.BufferPrj != null)
                    _selectedBufferHandle =
                        GlobalParams.UI.DrawMultiPolygon(_selectedObstacle.BufferPrj as Aran.Geometries.MultiPolygon,
                            _polygonBufferFillSymbol, true, false);

                if (!(_selectedObstacle.GeomPrj is Aran.Geometries.Point) && SelectedObstacle.ExactVertexGeom != null)
                    _selectedExactVertex = GlobalParams.UI.DrawPoint(SelectedObstacle.ExactVertexGeom, _ptSymbol);
            }
        }

        public SurfaceType SurfaceType { get; set; }

        public EtodSurfaceType EtodSurfaceType { get; set; }
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
            GlobalParams.UI.SafeDeleteGraphic(_selectedBufferHandle);
        }

        public RwyDirClass RwyDirClass { get; set; }
    }
}
