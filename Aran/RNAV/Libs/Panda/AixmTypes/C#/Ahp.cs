using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.GeometryClasses;
using ARAN.Contracts.Registry;

namespace ARAN.AIXMTypes
{
    public class Ahp : AIXM
    {
       

        public Ahp()
            : base(AIXMType.AHP)
        {
            ptGeo = new Point();
            ptPrj = new Point();
            Elevation = 0;
            ElevAccuracy = 0;
            MagVar = 0;
            TemPerature = 0;
        }

        public double Elevation { get; set; }
        public double ElevAccuracy { get; set; }
        public double MagVar { get; set; }
        public double TemPerature { get; set; }
        public int Country_id { get; set; }

        public override Object Clone()
        {
            Ahp ahp = new Ahp();
            ahp.Assign(this);
            return ahp;
        }

        public override void Pack(int handle)
        {
            base.Pack(handle);
            ptGeo.Pack(handle);
            ptPrj.Pack(handle);
            Registry_Contract.PutDouble(handle, Elevation);
            Registry_Contract.PutDouble(handle, ElevAccuracy);
            Registry_Contract.PutDouble(handle, MagVar);
            Registry_Contract.PutDouble(handle, TemPerature);
            Registry_Contract.PutInt32(handle, Country_id);
        }

        public override void UnPack(int handle)
        {
            base.UnPack(handle);
            ptGeo.UnPack(handle);
            ptPrj.UnPack(handle);
            Elevation = Registry_Contract.GetDouble(handle);
            ElevAccuracy = Registry_Contract.GetDouble(handle);
            MagVar = Registry_Contract.GetDouble(handle);
            TemPerature = Registry_Contract.GetDouble(handle);
            Country_id = Registry_Contract.GetInt32(handle);
        }

        public Point GetPtGeo()
        {
            return ptGeo;
        }

        public Point GetPtPrj()
        {
            return ptPrj;
        }

        public override void Assign(PandaItem source)
        {
            base.Assign((AIXM)source);
            Ahp src = ((AIXM)source).AsAhp();
            ptGeo.Assign(src.ptGeo);
            ptPrj.Assign(src.ptPrj);
            Elevation = src.Elevation;
            ElevAccuracy = src.ElevAccuracy;
            MagVar = src.MagVar;
            TemPerature = src.TemPerature;
            Country_id = src.Country_id;
        }

        private Point ptGeo;
        private Point ptPrj;
    }
}
