using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRules.Data
{
    public class BRuleInfo
    {
        public long DbItemId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Comment { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public string TaggedDescription { get; set; }
        public bool IsCustom { get; set; }
        public bool IsActive { get; set; }
        public string Tags { get; set; }
    }
}
