using DataImporter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Models
{
    public class FeatTypeClass : IFeatType
    {
        public string Header { get; set; }
        public FeatureType FeatType { get; set; }
    }
}
