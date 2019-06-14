namespace ChartPApproachTerrain.Models 
{
    public enum ObstacleGeomType
    {
        Point,
        Polygon,
        PolyLine
    }

    public class ObstacleReport 
    {
        
        public ObstacleReport()
        {
           
        }
        public long Id { get; set; }

        public string Name { get; set; }       

        public ObstacleGeomType GeomType { get; set; }    

        public double X { get; set; }
        public double Y { get; set; }

        public double VerticalAccuracy { get; set; }
        public double HorizontalAccuracy { get; set; }

        public double Elevation { get; set; }

        public ESRI.ArcGIS.Geometry.IPoint[] Points { get; set; }
       
    }
}
