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
    internal class RNPSegmentWidth : CalculationRule
    {

        public override string Identifier => "4.1.7";

        private double _fasSemiWidth;
        private double _result;

        public RNPSegmentWidth(SegmentType phase, string comment) : base(phase, comment)
        {
        }

        public override string Text =>
            @"RNP navigation accuracy requirements are specified in increments of a hundredth (0.01) of a NM.
            Segment width is defined as 4 × the RNP navigation accuracy requirement and segment half-width(semi-width) is
            defined as 2 × the RNP navigation accuracy requirement(see Figure 4-2). Standard RNP values for instrument
            procedures are listed in Table 4-1.";

        public override List<Tuple<string, Type>> ParameterTypes => new List<Tuple<string, Type>>()
        {
            new Tuple<string, Type>("RNP NAVIGATION ACCURACY REQUIREMENT", typeof(double))
        };

        public override Tuple<string, Type> ResultType => new Tuple<string, Type>("segment half-width", typeof(double));

        public override string Description => base.Description + $" RNP NAVIGATION ACCURACY REQUIREMENT: ${_fasSemiWidth}. RNP segment half-width {_result}";

        protected override object GetResult(object[] list)
        {
            _fasSemiWidth = (double)list[0];
            _result = 2.0 * _fasSemiWidth;
            return _result;
        }
    }
}
