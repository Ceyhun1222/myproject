using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.PANDA.Common;
using ChartTypeA.Models;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace ChartTypeA
{
    class Extensions
    {
        public static List<RunwayCenterLinePoint> SortCenterlinePoints(RwyDirWrapper rwyDir,List<RunwayCenterLinePoint> centerLineptList)
        {
            if (centerLineptList == null) return null;

            centerLineptList.ForEach(center=>center.RebuildGeo());
            var startCnt =centerLineptList.FirstOrDefault(
                center =>
                    (center.Role == CodeRunwayCenterLinePointRoleType.START || center.Role== CodeRunwayCenterLinePointRoleType.THR));


            if (startCnt == null) return null;
            List<RunwayCenterLinePoint> tmpCntLnList = new List<RunwayCenterLinePoint>();
            double directAzimuth, dirInverse;
            tmpCntLnList.Add(startCnt);

            double rwyDirAzimuth = ARANMath.Modulus(rwyDir.Aziumuth, 180);
            foreach (var cnt in centerLineptList)
            {
                var azimuth = EsriFunctions.ReturnGeodesicAzimuth((IPoint) startCnt.Geo, (IPoint) cnt.Geo, out directAzimuth,
                    out dirInverse);

                
                if (Math.Abs(rwyDirAzimuth - ARANMath.Modulus(directAzimuth, 180)) < 10)
                    tmpCntLnList.Add(cnt);
            }

            var endCnt = tmpCntLnList.FirstOrDefault(tmp => tmp.Role == CodeRunwayCenterLinePointRoleType.END);
            if (endCnt==null)
                tmpCntLnList.Add(centerLineptList.FirstOrDefault(cnt=>cnt.Role== CodeRunwayCenterLinePointRoleType.END));
            var result = tmpCntLnList.OrderBy(center => EsriFunctions.ReturnGeodesicDistance((IPoint)startCnt.Geo, (IPoint)center.Geo)).ToList();
            return result;
        }

    }
}
