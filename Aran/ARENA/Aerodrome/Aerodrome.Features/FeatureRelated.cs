using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aerodrome.Enums;

namespace Aerodrome.Features
{
    public class FeatureRelated
    {
        public void AddRelatedFeat(Feat_Type feat_type, string id)
        {
            Feat_Type = feat_type;
            Id = id;
        }
        public Feat_Type Feat_Type { get; set; }

        public string Id { get; set; }
    }
}
