using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVT.Model;

namespace PVT.Graphics
{
    class CurveHoldingDrawer : HoldingDrawerBase
    {
        protected override DrawObject DrawHoldingPattern(HoldingPattern pattern)
        {
            if (pattern.Extent != null)
            {
                return DrawCurve(pattern.Extent, pattern.Identifier);
            }

            return null;
        }
    }
}
