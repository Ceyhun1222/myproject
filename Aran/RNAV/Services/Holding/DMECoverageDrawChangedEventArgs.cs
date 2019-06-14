using System;

namespace Holding
{
    public class DMECoverageDrawChangedEventArgs:EventArgs
    {
        public DMECoverageDrawChangedEventArgs(bool draw)
        {
            Draw = draw;
        }

        public bool Draw { get; set; }
    }
}