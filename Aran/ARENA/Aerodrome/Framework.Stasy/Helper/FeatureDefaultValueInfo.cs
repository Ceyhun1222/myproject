using Aerodrome.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.Helper
{
    public class FeatureDefaultValueInfo
    {
        public NilReason NilReasonValue { get; set; }

        public string ValueForString { get; set; }

        public double ValueForDouble { get; set; }

        public DateTime ValueForDateTime { get; set; }

    }
}
