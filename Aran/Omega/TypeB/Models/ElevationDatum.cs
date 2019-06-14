using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Aim.Enums;
using Aran.Omega.TypeB.Enums;

namespace Aran.Omega.TypeB.Models
{
    public class ElevationDatum
    {
        public ElevationDatum(Aran.Aim.Features.Feature feature)
        {
            if (feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport)
            {
                var adhp = (AirportHeliport)feature;
                Height = ConverterToSI.Convert(adhp.ARP.Elevation, 0);
                ViewName = "ARP " + Common.ConvertHeight(Height, RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
                ReferenceObject = feature;
            }
            else if (feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint) 
            {
                var cntlPoint = (RunwayCentrelinePoint)feature;
                Height = ConverterToSI.Convert(cntlPoint.Location.Elevation, 0) ;

                ViewName =
                    (cntlPoint.Role == CodeRunwayPointRole.THR)
                    || (cntlPoint.Role == CodeRunwayPointRole.START)
                    || (cntlPoint.Role == Aran.Aim.Enums.CodeRunwayPointRole.END)
                    ? "THR " : "CLP ";
                ViewName += cntlPoint.Designator + " " + Common.ConvertHeight(Height, RoundType.ToNearest)+InitOmega.HeightConverter.Unit;
            }
        }

        public ElevationDatum(double height)
        {
            Height =Common.ConvertHeight(height,RoundType.ToNearest);
            ViewName = "TDZ " + Common.ConvertHeight(height, RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
        }

        public double Height { get; set; }
       
        public string ViewName { get; set; }

        public Aran.Aim.Features.Feature ReferenceObject { get; set; }
    }
}
