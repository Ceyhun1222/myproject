using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.FeatureInfo
{
    public class MultiPolyModel
    {
        private Geometries.Geometry _geom;

        public MultiPolyModel(Geometries.Geometry geom)
        {
            _geom = geom;
            RingOrParts = new List<RingOrPartModel>();

            if (geom is MultiPolygon) {
                GeometryName = "Polygon";

                var mp = geom as MultiPolygon;
                foreach (Polygon polygon in mp) {
                    var polygonModel = new PolygonModel();
                    polygonModel.ExteriorPoints.AddRange(polygon.ExteriorRing);

                    foreach (Ring ring in polygon.InteriorRingList)
                        polygonModel.InteriorModels.Add(new PolygonModel(ring));

                    RingOrParts.Add(polygonModel);
                }
            }
            else if (geom is MultiLineString) {
                GeometryName = "LineString";

                var mls = geom as MultiLineString;
                foreach (LineString ls in mls) {
                    var lsModel = new LineStringModel(ls);
                    RingOrParts.Add(lsModel);
                }
            }
        }

        public string GeometryName { get; private set; }

        public List<RingOrPartModel> RingOrParts { get; private set; }
    }

    public abstract class RingOrPartModel
    {
        public RingOrPartModel()
        {
            Points = new List<Geometries.Point>();
        }

        public List<Geometries.Point> Points { get; private set; }
    }

    public class PolygonModel : RingOrPartModel
    {
        public PolygonModel()
        {
            InteriorModels = new List<PolygonModel>();
        }

        public PolygonModel(MultiPoint multiPoint) :
            this ()
        {
            Points.AddRange(multiPoint);
        }

        public List<Geometries.Point> ExteriorPoints
        {
            get { return base.Points; }
        }

        public bool HasInterior
        {
            get { return InteriorModels.Count > 0; }
        }

        public List<PolygonModel> InteriorModels { get; private set; }
    }

    public class LineStringModel : RingOrPartModel
    {
        public LineStringModel()
        {
        }

        public LineStringModel(MultiPoint multiPoint)
        {
            Points.AddRange(multiPoint);
        }
    }
}
