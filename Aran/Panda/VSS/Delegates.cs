using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.PANDA.Vss
{
    public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

    public class PageChangedEventArgs : EventArgs
    {
        public PageChangedEventArgs(PageControl page, bool isNext)
        {
            Page = page;
            IsNext = isNext;
        }

        public PageControl Page { get; private set; }

        public bool IsNext { get; private set; }
    }
}
