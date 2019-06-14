using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Model;

namespace Aran.Panda.RNAV.RNPAR.Rules.Core
{
    abstract class Rule : IRule
    {

        public abstract string Identifier { get; }

        public virtual string Description => $"{Phase}. Comments: {Comment}";

        public virtual string Text { get; }

        public SegmentType Phase { get; protected set; }
        public string Comment { get; protected set; }

        public object[] Params { get; protected set; }
        public object Result { get; protected set; }

        protected Rule(SegmentType phase, string comment)
        {
            Phase = phase;
            Comment = comment??"";
        }



        public virtual List<Tuple<string, Type>> ParameterTypes { get; }
        public virtual Tuple<string, Type> ResultType { get; }

        protected void CheckParameters(object[] paramеters)
        {
            if (ParameterTypes.Count != paramеters.Length)
                throw new FormatException($"Expected {ParameterTypes.Count} parameters, but received {paramеters.Length}");

            for (int i = 0; i < paramеters.Length; i++)
            {
                if (!paramеters[i].GetType().Equals(ParameterTypes[i].Item2))
                {
                    throw new FormatException($"Expected {ParameterTypes[i].Item2.ToString()} type for  { ParameterTypes[i].Item1.ToString()}, but recieved {paramеters[i].GetType().ToString()}");
                }
            }
        }
        

    }
}
