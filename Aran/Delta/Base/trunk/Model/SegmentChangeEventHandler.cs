using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Delta.Model
{
    public delegate void SegmentPointChangedEvent(object sender, SegmentChangeEventArgs argNavList);

    public class SegmentChangeEventArgs:EventArgs
    {
        public bool StartIsChanged { get; set; }
    }
}
