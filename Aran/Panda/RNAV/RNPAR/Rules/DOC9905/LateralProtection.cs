using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Model;
using Aran.Panda.RNAV.RNPAR.Rules.Core;

namespace Aran.Panda.RNAV.RNPAR.Rules.DOC9905
{
    class LateralProtection : CalculationRule
    {
        public LateralProtection(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public override List<Tuple<string, Type>> ParameterTypes => new List<Tuple<string, Type>>()
        {
            new Tuple<string, Type>("Input RNP value in meters", typeof(double))        
        };

        public override Tuple<string, Type> ResultType => new Tuple<string, Type>("Output RNP value in meters", typeof(double));

        public override string Identifier => "2.5";

        public override string Text =>
            @"For RNP AR procedures, the semi-width of the primary area is defined as 2 × RNP navigation accuracy requirement.
            There are no buffer or secondary areas.Table 2-1 lists RNP navigation accuracy requirements applicable to the specific instrument procedure segments.";

        protected override object GetResult(object[] list)
        {
            List<double> constrains = null;
            switch (Phase)
            {
                case SegmentType.Final:
                    constrains = new List<double> { 0.3*1852, 0.1 * 1852, 0.5 * 1852 };
                    break;
                case SegmentType.Intermediate:
                    constrains = new List<double> { 1 * 1852, 0.1 * 1852, 1 * 1852 };
                    break;
                case SegmentType.Initial:
                    constrains = new List<double> { 1 * 1852, 0.1 * 1852, 1 * 1852 };
                    break;
                case SegmentType.Arrival:
                    constrains = new List<double> { 2 * 1852, 0.1 * 1852, 2 * 1852 };
                    break;
                case SegmentType.MissedApproach:
                    constrains = new List<double> { 1 * 1852, 0.1 * 1852, 1 * 1852 };
                    break;

            }
            double input = (double)list[0];
            if (input < constrains[1])
                return constrains[1];
            if (input > constrains[2])
                return constrains[2];
            return input;

        }
    }
}