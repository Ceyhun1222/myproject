using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public class DesigningRoute
    {
        public DesigningRoute()
        {
            SegmentList = new List<DesigningSegment>();
        }
        public string Name { get; set; }
        public string Designer { get; set; }
        public List<DesigningSegment> SegmentList { get; set; }
    }
}
