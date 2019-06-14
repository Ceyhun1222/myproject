using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;

namespace Aran.Panda.RNAV.RNPAR.Rules.Core
{
    interface IRule
    {
        string Identifier { get; }
        string Description { get; }
        string Text { get; }
        SegmentType Phase { get; }
        List<Tuple<string, Type>> ParameterTypes { get; }
        Tuple<string, Type> ResultType { get; }
        object[] Params { get;  }
        object Result { get; }
    }

    interface ICalculationRule
    {
        object Calculate(params object[] list);
    }


    interface ICheckRule
    {
        bool Check(params object[] list);
    }
}
