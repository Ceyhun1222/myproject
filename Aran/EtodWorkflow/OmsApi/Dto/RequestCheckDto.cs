using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class RequestCheckDto
    {
        public string AdhpIdentifier { get; set; }

        public int WorkPackage { get; set; }

        public IList<MyPoint> Points { get; set; }

        public double Elevation { get; set; }

        public int GeometryType { get; set; }

        public double HorizontalAccuracy { get; set; }

        public double VerticalAccuracy
        {
            get; set;
        }

        public double Height { get; set; }
    }
}
