using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.Parser
{
    public class SbvrNoun
    {
        public SbvrNoun()
        {
            Childs = new List<Noun>();
        }

        public string Name { get; set; }

        public List<Noun> Childs { get; private set; }

        public SbvrConditionList With { get; set; }

    }
}
