namespace ObstacleCalculator.Domain.Models
{
    public class ObstacleReport
    {
        public ObstacleReport(string surfaceName,double penetrate,string surfaceIdentifier,string runwayDirection)
        {
            SurfaceName = surfaceName;
            Penetrate = penetrate;
            SurfaceIdentifier = surfaceIdentifier;
            RunwayDirection = runwayDirection;
        }

        public bool IsPenetrated => Penetrate > 0;

        public string SurfaceName { get;}

        public double Penetrate { get;}

        public string SurfaceIdentifier { get;}

        public string RunwayDirection { get;}


    }
}
