namespace Aran.Omega.TypeBEsri.Models
{
    public class GeoMinMaxPoint
    {
        public void AddMinPoint(Aran.Geometries.Point minPoint)
        {
            XMin = minPoint.X;
            YMin = minPoint.Y;
        }

        public void AddMaxPoint(Aran.Geometries.Point maxPoint)
        {
            XMax = maxPoint.X;
            YMax = maxPoint.Y;
        }

        public void AddMinMaxPoint(Aran.Geometries.Point minPoint, Aran.Geometries.Point maxPoint)
        {
            XMin = minPoint.X;
            YMin = minPoint.Y;
            XMax = maxPoint.X;
            YMax = maxPoint.Y;
        }

        public double XMin { get; set; }
        public double YMin { get; set; }
        public double XMax { get; set; }
        public double YMax { get; set; }
    }
}