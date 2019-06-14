using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.RNAV.DMECoverage.Modules
{
    public class ThreeDmeCoverage
    {
        private int _elemAvailableZone;

        public ThreeDmeCoverage(CDMECoverage dmeCoverage1, CDMECoverage dmeCoverage2, MultiPolygon availableZone)
        {
            DmeCoverage1 = dmeCoverage1;
            DmeCoverage2 = dmeCoverage2;
            AvailableZone = availableZone;
        }

        ~ThreeDmeCoverage()
        {
            ClearImages();
        }

        public CDMECoverage DmeCoverage1 { get; }
        public CDMECoverage DmeCoverage2 { get; }

        public MultiPolygon AvailableZone { get; }

        public void DrawPolygons()
        {
            ClearImages();

			//    DmeCoverage1.DrawOnlyAvailableZone();
			//  DmeCoverage2.DrawOnlyAvailableZone();
			if (AvailableZone != null && !AvailableZone.IsEmpty)
				_elemAvailableZone = GlobalVars.gAranGraphics.DrawMultiPolygon(AvailableZone,
					eFillStyle.sfsCross, ARANFunctions.RGB(255, 0, 255));
        }

        public void ClearImages()
        {
            //DmeCoverage1.ClearImages();
            //DmeCoverage2.ClearImages();
            GlobalVars.gAranGraphics.SafeDeleteGraphic(_elemAvailableZone);
        }
    }
}
