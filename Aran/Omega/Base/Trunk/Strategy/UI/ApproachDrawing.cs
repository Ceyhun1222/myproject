using Aran.Omega.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.Strategy.UI
{
    public class ApproachDrawing:IPlaneDrawing
    {
        private Approach _approach;
        private int _section1SelectedHandle;
        private int _section2SelectedHandle;
        private int _section3SelectedHandle;

        private int _section1Handle;
        private int _section2Handle;
        private int _section3Handle;

        public ApproachDrawing()
        {
        }

        public void SetSurface(SurfaceBase surface)
        {
            _approach = surface as Approach;
        }

        public  void Draw(bool isSelected)
        {
            var section1 = _approach.Section1;
            var section2 = _approach.Section2;
            var section3 = _approach.Section3;
            if (isSelected)
            {
                ClearSelected();
                if (section1 != null && !section1.Geo.IsEmpty)
                    _section1SelectedHandle = GlobalParams.UI.DrawRing(section1.Geo, _approach.SelectedSymbol);

                if (section2 != null && !section2.Geo.IsEmpty)
                    _section2SelectedHandle = GlobalParams.UI.DrawRing(section2.Geo, _approach.SelectedSymbol);

                if (section3 != null && !section3.Geo.IsEmpty)
                    _section3SelectedHandle = GlobalParams.UI.DrawRing(section3.Geo, _approach.SelectedSymbol);
            }
            else
            {
                ClearDefault();
                if (section1 != null && !section1.Geo.IsEmpty)
                    _section1Handle = GlobalParams.UI.DrawRing(section1.Geo, _approach.DefaultSymbol);

                if (section2 != null && !section2.Geo.IsEmpty)
                    _section2Handle = GlobalParams.UI.DrawRing(section2.Geo, _approach.DefaultSymbol);

                if (section3 != null && !section3.Geo.IsEmpty)
                    _section3Handle = GlobalParams.UI.DrawRing(section3.Geo, _approach.DefaultSymbol);

            }

        }

        public void ClearSelected()
        {
            GlobalParams.UI.SafeDeleteGraphic(_section1SelectedHandle);
            GlobalParams.UI.SafeDeleteGraphic(_section2SelectedHandle);
            GlobalParams.UI.SafeDeleteGraphic(_section3SelectedHandle);
        }

        public void ClearDefault()
        {
            GlobalParams.UI.SafeDeleteGraphic(_section1Handle);
            GlobalParams.UI.SafeDeleteGraphic(_section2Handle);
            GlobalParams.UI.SafeDeleteGraphic(_section3Handle);
        }

        public void ClearAll()
        {
            ClearSelected();
            ClearDefault();
        }
    }
}
