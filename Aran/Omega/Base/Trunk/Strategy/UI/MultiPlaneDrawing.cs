using Aran.Omega.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.Strategy.UI
{
    public class MultiPlaneDrawing : IPlaneDrawing
    {
        private SurfaceBase _surface;
        private List<int> _selectedHandles;
        private List<int> _defaultHandles;

        public MultiPlaneDrawing()
        {
            _selectedHandles = new List<int>();
            _defaultHandles = new List<int>();
        }

        public void SetSurface(SurfaceBase surface)
        {
            _surface = surface;
        }

        public void ClearAll()
        {
            ClearDefault();
            ClearSelected();
        }

        public void ClearDefault()
        {
            foreach (int handle in _defaultHandles)
                GlobalParams.UI.SafeDeleteGraphic(handle);

            _defaultHandles.Clear();
        }

        public void ClearSelected()
        {
            foreach (int handle in _selectedHandles)
                GlobalParams.UI.SafeDeleteGraphic(handle);

            _selectedHandles.Clear();
        }

        public void Draw(bool isSelected)
        {
            if (isSelected)
            {
                ClearSelected();
                foreach (Plane strip in _surface.Planes)
                    _selectedHandles.Add(GlobalParams.UI.DrawRing(strip.Geo, _surface.SelectedSymbol));
            }
            else
            {
                ClearDefault();
                foreach (Plane strip in _surface.Planes)
                    _defaultHandles.Add(GlobalParams.UI.DrawRing(strip.Geo, _surface.DefaultSymbol));
            }
        }
    }
}
