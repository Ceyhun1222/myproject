using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Geometries;
using ESRI.ArcGIS.Geometry;

namespace Aran.Omega.Extensions
{
    static class VerticalStructureExtension
    {
        public static Geometry GetPartGeometry(this VerticalStructurePart verticalStructurePart)
        {
            dynamic extent = null;
            VerticalStructurePartGeometry horizontalProj = verticalStructurePart.HorizontalProjection;

            if (horizontalProj == null) throw new ArgumentNullException();

            if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
            {
                extent = horizontalProj.Location;
            }
            else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
            {
                extent = horizontalProj.LinearExtent;
            }
            else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
            {
                extent = horizontalProj.SurfaceExtent;
            }

            if (extent == null)
                throw new ArgumentNullException();

            return extent.Geo as Aran.Geometries.Geometry;
        }

        public static dynamic GetExtent(this VerticalStructurePart verticalStructurePart)
        {
            dynamic extent = null;
            VerticalStructurePartGeometry horizontalProj = verticalStructurePart.HorizontalProjection;

            if (horizontalProj == null) throw new ArgumentNullException();

            if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
            {
                extent = horizontalProj.Location;
            }
            else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedCurve)
            {
                extent = horizontalProj.LinearExtent;
            }
            else if (horizontalProj.Choice == VerticalStructurePartGeometryChoice.ElevatedSurface)
            {
                extent = horizontalProj.SurfaceExtent;
            }

            return extent;
        }
    }
}
