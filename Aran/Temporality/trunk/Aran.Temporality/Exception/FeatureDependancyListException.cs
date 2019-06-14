using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Temporality.Exception
{
    public class FeatureDependancyListException: System.Exception
    {
        public FeatureDependancyListException(string desc)
            : base(desc)
        {
        }

        public List<FeatureDependancyException> ExceptionList { get; set; }
    }
}
