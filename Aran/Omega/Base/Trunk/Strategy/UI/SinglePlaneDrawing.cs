using Aran.AranEnvironment.Symbols;
using Aran.Omega.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.Strategy.UI
{
    class SinglePlaneDrawing : IPlaneDrawing
    {
        private SurfaceBase _surface;
        private int _geomSelectedHandle;
        private int _geomDefautlHandle;

        public SinglePlaneDrawing()
        {
        }

        public void SetSurface(SurfaceBase surface)
        {
            _surface = surface;
        }
        public virtual void Draw(bool isSelected)
        {
            if (isSelected)
            {
                ClearSelected();
                if (_surface.GeoPrj != null)
                    _geomSelectedHandle = GlobalParams.UI.DrawMultiPolygon(_surface.GeoPrj,_surface.SelectedSymbol, true, false);
            }
            else
            {
                ClearDefault();
                if (_surface.GeoPrj != null)
                    _geomDefautlHandle = GlobalParams.UI.DrawMultiPolygon(_surface.GeoPrj, _surface.DefaultSymbol, true, false);
            }
        }

        public virtual void ClearSelected()
        {
            GlobalParams.UI.SafeDeleteGraphic(_geomSelectedHandle);
        }

        public virtual void ClearAll()
        {
            ClearDefault();
            ClearSelected();
        }

        public virtual void ClearDefault()
        {
            GlobalParams.UI.SafeDeleteGraphic(_geomDefautlHandle);

        }
    }
}
