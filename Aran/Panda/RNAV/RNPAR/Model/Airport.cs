using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Geometries;
using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RNAV.RNPAR.Model
{
    public class Airport
    {

        public Point PointGeo{ get; }
        public Point PointProjection{ get; }
        public AirportHeliport AirportHeliport{ get; }
        public List<FeatureRefObject> AltimeterSource{ get; }

        public string Name{ get; }
        public double MagVar{ get; }
        public double Elev{ get; }
        public double WindSpeed{ get; }
        public double ISAtC{ get; }
        public double LowestTemperature{ get; }
        //public double MaximumTemperature{ get; }
        public double TransitionLevel{ get; }
        public Guid Identifier{ get; }
        public Guid OrgID{ get; }

        public Airport(ADHPType adhp)
        {
            PointGeo = adhp.pPtGeo;
            PointProjection = adhp.pPtPrj;
            AirportHeliport = adhp.pAirportHeliport;
            AltimeterSource = adhp.AltimeterSource;
            Name = adhp.Name;
            MagVar = adhp.MagVar;
            WindSpeed = adhp.WindSpeed;
            ISAtC = adhp.ISAtC;
            LowestTemperature = adhp.LowestTemperature;
            TransitionLevel = adhp.TransitionLevel;
            Identifier = adhp.Identifier;
            OrgID = adhp.OrgID;
        }


        public override string ToString()
        {
            if (Name != null)
                return Name;

            return "";
        }
    }
}
