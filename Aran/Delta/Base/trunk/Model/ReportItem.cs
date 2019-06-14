using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public class ReportItem
    {
        public string IntersectObject { get; set; }
        public string Layer { get; set; }
        public string Type { get; set; }
        public string Levels { get; set; }
        public string IntersectLevels { get; set; }
        public string BufferWidth { get; set; }
        public bool Intersect { get; set; }
    }
}
