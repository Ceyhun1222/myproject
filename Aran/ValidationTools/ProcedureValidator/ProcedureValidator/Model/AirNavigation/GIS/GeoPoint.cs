using Aran.Aim.Features;
using Aran.PANDA.Common;

namespace PVT.Model
{
    public class GeoPoint : Point
    {
        public double Latitude
        {
            get { return Y; }
            set { Y = value; }
        }

        public double Longitude
        {
            get { return X; }
            set { X = value; }
        }

        public string LatitudeString
        {
            get
            {
                return Y.ToString();
            }
        }

        public string LongitudeString
        {
            get
            {
                return X.ToString();
            }
        }

        public string LatitudeDMSString
        {
            get
            {
                return ARANFunctions.Degree2String(Y, Degree2StringMode.DMSLat);
            }
        }

        public string LongitudeDMSString
        {
            get
            {
                return ARANFunctions.Degree2String(X, Degree2StringMode.DMSLon);
            }
        }

        public GeoPoint(AixmPoint point) : base(point) { }

        public GeoPoint(Aran.Geometries.Point point) : base(point) { }

        public GeoPoint(Point point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }
        public GeoPoint()
        {

        }
    }
}
