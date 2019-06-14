using Aran.Aim.Features;

namespace Holding
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Elevation { get; set; }
        public double Altitude { get; set; }
        public double Moc { get; set; }
        public double Req_H { get; set; }
        public double Penetrate { get; set; }
        public bool Validation { get; set; }
        public VerticalStructure Obstacle { get; set; }
        public ObstactleReportType SurfaceType { get; set; }
        public string  Area{ get; set; }
        public int AreaNumber { get; set; }
        public ObstacleGeomType GeomType { get; set; }
        public Aran.Geometries.Geometry GeomPrj { get; set; }
        public double VerAccuracy { get; set; }
        public double HorAccuracy { get; set; }
    }
}