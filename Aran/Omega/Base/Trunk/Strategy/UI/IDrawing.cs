using Aran.Omega.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.Strategy.UI
{
    public interface IPlaneDrawing
    {
        void SetSurface(SurfaceBase surface);
        void Draw(bool isSelected);

        void ClearSelected();

        void ClearDefault();
        void ClearAll();
    }
}
