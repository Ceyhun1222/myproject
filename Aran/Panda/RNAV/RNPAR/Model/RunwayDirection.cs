using Aran.Aim.DataTypes;
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
    class RunwayDirection
    {
        public EnumArray<Point, eRWY> PointGeo{ get; }
        public EnumArray<Point, eRWY> PointProjecion{ get; }
        public EnumArray<Guid, eRWY> ClptIdent{ get; }
        public double StartHorAccuracy{ get; }
        public double DERHorAccuracy{ get; }

        public string Name{ get; }
        public string PairName{ get; }
        public double TrueBearing{ get; }
        public double MagneticBearing{ get; }
        public double Length{ get; }

        public double ClearWay{ get; }
        public double TODA{ get; }
        public double TODAAccuracy{ get; }
        public double ASDA{ get; }

        public Guid Identifier{ get; }
        public Guid PairID{ get; }
        public Guid AirportID{ get; }

        public RunwayDirection(RWYType rwy)
        {
            PointGeo = rwy.pPtGeo;
            PointProjecion = rwy.pPtPrj;
            ClptIdent = rwy.clptIdent;
            StartHorAccuracy = rwy.StartHorAccuracy;
            DERHorAccuracy = rwy.StartHorAccuracy;
            Name = rwy.Name;
            PairName = rwy.PairName;
            TrueBearing = rwy.TrueBearing;
            MagneticBearing = rwy.MagneticBearing;
            Length = rwy.Length;
            ClearWay = rwy.ClearWay;
            TODA = rwy.TODA;
            TODAAccuracy = rwy.TODAAccuracy;
            ASDA = rwy.ASDA;
            Identifier = rwy.Identifier;
            PairID = rwy.PairID;
            AirportID = rwy.ADHP_ID;
        }

        public FeatureRefObject GetFeatureRefObject()
        {
            FeatureRefObject fro = new FeatureRefObject();
            fro.Feature = new FeatureRef(Identifier);
            return fro;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
