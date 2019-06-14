using ObstacleCalculator.Domain.Enums;
using System.Collections.Generic;

namespace ObstacleChecker.API.Model
{
    public class ObstacleReportRequest
    {
        public string AdhpIdentifier { get; set; }

        public int WorkPackage { get; set; }

        public IList<OmegaPoint> Points { get; set; }

        public double Elevation { get; set; }

        public ObsacleGeometryType GeometryType { get; set; }

        public double HorizontalAccuracy { get; set; }

        public double VerticalAccuracy { get; set; }

        public double Height { get; set; }

        public double HeightAccuracy { get; set; }
    }
}
