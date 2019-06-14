using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Panda.Constants;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Omega.ViewModels;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using System.Windows.Media;

namespace Omega.Models
{
    public class DrawingSurface : ViewModel
    {
        public DrawingSurface(RunwayConstants rwyConstant)
        {
            SurfaceType = rwyConstant.Surface;
            ViewCaption = rwyConstant.SurfaceName;
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

        private int _selectedObstacleHandle;
        private ObstacleReport _selectedObstacle;
        public ObstacleReport SelectedObstacle
        {
            get { return _selectedObstacle; }
            set
            {
                _selectedObstacle = value;
                GlobalParams.UI.SafeDeleteGraphic(_selectedObstacleHandle);

                if (_selectedObstacle.GeomPrj is Aran.Geometries.Point)
                    _selectedObstacleHandle = GlobalParams.UI.DrawPoint(_selectedObstacle.GeomPrj as Aran.Geometries.Point, 9821, Aran.AranEnvironment.Symbols.ePointStyle.smsCircle);
                else if (_selectedObstacle.GeomPrj is MultiLineString)
                    _selectedObstacleHandle = GlobalParams.UI.DrawMultiLineString(_selectedObstacle.GeomPrj as MultiLineString, 12314, 2);
                else if (_selectedObstacle.GeomPrj is Aran.Geometries.MultiPolygon)
                    _selectedObstacleHandle = GlobalParams.UI.DrawMultiPolygon(_selectedObstacle.GeomPrj as MultiPolygon, 2322, Aran.AranEnvironment.Symbols.eFillStyle.sfsCross);
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

    }
}
