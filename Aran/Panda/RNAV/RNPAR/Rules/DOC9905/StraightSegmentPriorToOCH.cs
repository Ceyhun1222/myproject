using System;
using System.Collections.Generic;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using Aran.PANDA.Common;

namespace Aran.Panda.RNAV.RNPAR.Rules.DOC9905
{
    class StraightSegmentPriorToOCH : CalculationRule
    {
        public StraightSegmentPriorToOCH(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public override List<Tuple<string, Type>> ParameterTypes => new List<Tuple<string, Type>>()
        {
            new Tuple<string, Type>("height above threshold of the DH", typeof(double)),      
            new Tuple<string, Type>("RDH", typeof(double)),      
            new Tuple<string, Type>("VPA", typeof(double)),        
            new Tuple<string, Type>(@"TAS based on the IAS for the fastest aircraft category for which the procedure is designed at ISA + 15o C
            at aerodrome elevation, plus a 15-kt tailwind for a time of:", typeof(double)),
            new Tuple<string, Type>("Missed approach RNP value", typeof(double))
        };

        public override Tuple<string, Type> ResultType => new Tuple<string, Type>("Minimum FROP distance from runway in meters", typeof(double));

        public override string Identifier => "4.5.11";

        public override string Text => @"Procedures that incorporate an RF leg in the final segment shall establish the aircraft at a final approach
                            roll-out point (FROP) aligned with the runway centreline prior to the greater of: ... see DOC 9905";

        protected override object GetResult(object[] list)
        {
            double prelDHval = (double)list[0];
            double rdh = (double)list[1];
            double vpa = (double)list[2];
            double vTas = (double)list[3];
            double mRNPValue = (double)list[4];


            double dt;

            //RNPAR 4.5.12
            if (mRNPValue >= 1852.0)
            {
                double d15 = (prelDHval - rdh) /Math.Tan(vpa) +
                             4.1666666666666666 * vTas + 4.1666666666666666 * 27.78; //115.75
                dt = d15;
            }
            else
            {
                double d50 = (prelDHval - rdh) / Math.Tan(vpa) +
                             (vTas + 27.78) * 13.89;
                dt = d50;
            }

            //RNPAR 4.5.11
            double D150 = (150.0 - rdh)/Math.Tan(vpa);

           return Math.Max(D150, dt);
        }
    }
}