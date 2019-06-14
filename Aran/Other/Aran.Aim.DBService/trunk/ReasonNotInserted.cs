using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.DBService
{
    public class ReasonNotInserted
    {
        public FeatureType FeatType
        {
            get;
            set;
        }

        public Guid Identifier
        {
            get;
            set;
        }

        public bool IsReasonFeatInserted
        {
            get;
            set;
        }
    }
}
