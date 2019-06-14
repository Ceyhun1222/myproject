using System;
using System.Collections.Generic;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using Aran.PANDA.Common;

namespace Aran.Panda.RNAV.RNPAR.Rules.DOC9905
{
    class BankAngleRF : CalculationRule
    {
        public BankAngleRF(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public override List<Tuple<string, Type>> ParameterTypes => new List<Tuple<string, Type>>()
        {
            new Tuple<string, Type>("TAS", typeof(double)),      
            new Tuple<string, Type>("tailwind speed", typeof(double)),      
            new Tuple<string, Type>("turn radius", typeof(double))       
        };

        public override Tuple<string, Type> ResultType => new Tuple<string, Type>("bank angle", typeof(double));

        public override string Identifier => "3.2.11";

        public override string Text => @"Calculation of bank angle for specific RF leg radius, where RF legs are necessary, the bank angle required for a given TAS, tailwind speed and turn radius is:";

        protected override object GetResult(object[] list)
        {
            double vtas = (double)list[0];
            double wind = (double)list[1];
            double radius = (double)list[2];


            double V = 3.6 * (vtas + wind);

            double R = 50.0 * V / (radius * Math.PI);

            if (R > 3.0)
                R = 3.0;

            return ARANMath.RadToDeg(Math.Atan(R * Math.PI * V / 6355.0));
        }
    }
}