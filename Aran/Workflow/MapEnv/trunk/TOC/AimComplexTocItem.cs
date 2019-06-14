using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapEnv.Toc
{
    public class AimComplexTocItem : AimSimpleTocItem
    {
        public override TocItemType TocType
        {
            get { return TocItemType.AimComplex; }
        }
    }
}
