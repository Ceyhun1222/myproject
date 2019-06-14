using System;

namespace Holding
{
    public class ModelChangedEventArgs:EventArgs
    {
        public ModelChangedEventArgs (bool changed)
        {
            Changed = changed;
        }
        public bool Changed { get; set; }

    }
}