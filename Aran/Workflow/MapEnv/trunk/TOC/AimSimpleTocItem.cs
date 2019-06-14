using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MapEnv.Toc
{
    public class AimSimpleTocItem : TocItem
    {
        public AimSimpleTocItem ()
        {
            SymbolItems = new ObservableCollection<TocSymbolItem> ();
        }

        public override TocItemType TocType
        {
            get { return TocItemType.AimSimple; }
        }

        public ObservableCollection<TocSymbolItem> SymbolItems { get; private set; }
    }
}
