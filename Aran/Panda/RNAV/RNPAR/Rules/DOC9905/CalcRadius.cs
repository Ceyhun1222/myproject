using System;
using System.Collections.Generic;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using Aran.PANDA.Common;

namespace Aran.Panda.RNAV.RNPAR.Rules.DOC9905
{
    class CalcRadius : CalculationRule
    {
        public CalcRadius(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public override List<Tuple<string, Type>> ParameterTypes => new List<Tuple<string, Type>>()
        {
            new Tuple<string, Type>("TAS", typeof(double)),      
            new Tuple<string, Type>("tailwind speed", typeof(double)),      
            new Tuple<string, Type>("bank angle", typeof(double))       
        };

        public override Tuple<string, Type> ResultType => new Tuple<string, Type>("turn radius", typeof(double));

        public override string Identifier => "3.2.4";

        public override string Text => @"Calculating the turn radius";

        protected override object GetResult(object[] list)
        {
            double vtas = (double)list[0];
            double wind = (double)list[1];
            double angle = (double)list[2];

            double V = 3.6 * (vtas + wind);
            double R = (6355.0 * Math.Tan(ARANMath.DegToRad(angle))) / (Math.PI * V);

            if (R > 3.0)
                R = 3.0;


            return 1000.0 * V / (20 * Math.PI * R);
        }
    }
}