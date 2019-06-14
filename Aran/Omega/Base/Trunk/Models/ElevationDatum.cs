using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Aim.Enums;
using Aran.Omega.Enums;

namespace Aran.Omega.Models
{
    public class ElevationDatum
    {
        public ElevationDatum(Aran.Aim.Features.Feature feature,string name)
        {
            ViewName = name;
            if (feature.FeatureType == Aran.Aim.FeatureType.AirportHeliport)
            {
                var adhp = (AirportHeliport)feature;
                Height = ConverterToSI.Convert(adhp.ARP.Elevation, 0);
              //  ViewName += Common.ConvertHeight(Height, RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
                ReferenceObject = feature;
            }
            else if (feature.FeatureType == Aran.Aim.FeatureType.RunwayCentrelinePoint) 
            {
                var cntlPoint = (RunwayCentrelinePoint)feature;

                Height = ConverterToSI.Convert(cntlPoint.Location.Elevation, 0) ;
            }

            ViewName +=" "+ Common.ConvertHeight(Height, RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
        }

        public ElevationDatum(double height,string name)
        {
            Height =Common.ConvertHeight(height,RoundType.ToNearest);
            //ViewName = "TDZ " + Common.ConvertHeight(height, RoundType.ToNearest) + InitOmega.HeightConverter.Unit;
            ViewName = $"{name} {Common.ConvertHeight(height, RoundType.ToNearest)}{InitOmega.HeightConverter.Unit}";
        }

        public double Height { get; set; }
       
        public string ViewName { get; set; }

        public Aran.Aim.Features.Feature ReferenceObject { get; set; }
    }
}
