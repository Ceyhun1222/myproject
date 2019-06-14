using System.Collections.Generic;

namespace PVT.Model
{
    public class VerticalStructure : Feature
    {
        public Aran.Aim.Features.VerticalStructure Original { get; private set; }
        public List<VerticalStructurePart> Parts { get; }
        public string Name { get; private set; }
        public VerticalStructure(Aran.Aim.Features.VerticalStructure vStr) : base(vStr)
        {
            Original = vStr;
            Name = vStr.Name;
            Parts = new List<VerticalStructurePart>();
            for (var i = 0; i < vStr.Part.Count; i++)
            {
                if (vStr.Part[i].HorizontalProjection != null)
                {
                    var part = new VerticalStructurePart(vStr.Part[i]);
                    if (!part.IsEmpty)
                        Parts.Add(part);
                }
            }
        }
    }

    public class VerticalStructurePart
    {

        private readonly Aran.Aim.Features.ElevatedPoint _point;
        private readonly Aran.Aim.Features.ElevatedCurve _curve;
        private readonly Aran.Aim.Features.ElevatedSurface _polygon;

        public string Designator { get; private set; }
        public bool IsEmpty { get; }
        public Aran.Aim.Features.AixmPoint Point { get { return _point; } }
        public Aran.Geometries.Geometry Geo
        {
            get
            {
                if (_curve != null)
                    return _curve.Geo;
                return _polygon.Geo;
            }
        }

        public Aran.Aim.Features.VerticalStructurePart Original { get; }
        public VerticalStructurePartGeometryChoice Type { get; private set; }

        public VerticalStructurePart(Aran.Aim.Features.VerticalStructurePart part)
        {
            Original = part;
            Designator = part.Designator;
            switch (part.HorizontalProjection.Choice)
            {
                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedPoint:
                    Type = VerticalStructurePartGeometryChoice.ElevatedPoint;

                     _point = Original.HorizontalProjection.Location;
                    if (_point == null) IsEmpty = true;
                    if (_point.Elevation == null) IsEmpty = true;

                    break;
                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedCurve:
                    Type = VerticalStructurePartGeometryChoice.ElevatedCurve;

                     _curve = Original.HorizontalProjection.LinearExtent;
                    if (_curve == null) IsEmpty = true;
                    if (_curve.Elevation == null) IsEmpty = true;
                    break;
                case Aran.Aim.VerticalStructurePartGeometryChoice.ElevatedSurface:
                    Type = VerticalStructurePartGeometryChoice.ElevatedSurface;

                     _polygon = Original.HorizontalProjection.SurfaceExtent;
                    if (_polygon == null) IsEmpty = true;
                    if (_polygon.Elevation == null) IsEmpty = true;
                    break;
                default:
                    IsEmpty = true;
                    break;
            }
        }



    }

    public enum VerticalStructurePartGeometryChoice
    {
        ElevatedPoint,
        ElevatedCurve,
        ElevatedSurface
    }
}
