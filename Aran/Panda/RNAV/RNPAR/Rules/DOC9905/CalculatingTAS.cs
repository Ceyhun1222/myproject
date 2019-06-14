using System;
using System.Collections.Generic;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using Aran.PANDA.Common;

namespace Aran.Panda.RNAV.RNPAR.Rules.DOC9905
{
    class CalculatingTAS : CalculationRule
    {
        public CalculatingTAS(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public override List<Tuple<string, Type>> ParameterTypes => new List<Tuple<string, Type>>()
        {
            new Tuple<string, Type>("indicated airspeed in km/h", typeof(double)),
            new Tuple<string, Type>("altitude in m", typeof(double)),
            new Tuple<string, Type>("variation from international standard atmosphere (ISA) (standard value +15) or local data for 95 per cent high temperature, if available", typeof(double))
             
        };

        public override Tuple<string, Type> ResultType => new Tuple<string, Type>("true airspeed in km/h", typeof(double));

        public override string Identifier => "3.1.7";

        public override string Text =>@"IAS to TAS conversion for RNP AR procedures uses the following standard equations:
                                    SI units: TAS = IAS * 171233 * [(288 + VAR) – 0.006496 * H]^0.5/(288 – 0.006496 * H)^2.628
                                    Chapter 3. General criteria 3-3
                                    where
                                    IAS = indicated airspeed (kt or km/h, as appropriate)
                                    TAS = true airspeed (kt or km/h, as appropriate)
                                    VAR = variation from international standard atmosphere (ISA) (standard value +15) or local data for
                                    95 per cent high temperature, if available
                                    H = altitude (ft or m, as appropriate)";

        protected override object GetResult(object[] list)
        {
            return ARANMath.IASToTAS((double) list[0], (double) list[1], (double)list[2]);
        }
    }
}