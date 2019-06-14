using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class SigmaCallout_Shadow : AbstractChartClass
    {
        private int _ShadowSize;
        [PropertyOrder(10)]
        public int ShadowSize
        {
            get { return _ShadowSize; }
            set { _ShadowSize = value; }
        }

        private AncorColor _ShadowCoLor;
        [PropertyOrder(20)]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor ShadowCoLor
        {
            get { return _ShadowCoLor; }
            set { _ShadowCoLor = value; }
        }

        public override object Clone()
        {
            SigmaCallout_Shadow o = (SigmaCallout_Shadow)base.Clone();
            if (o.ShadowCoLor != null) o.ShadowCoLor = (AncorColor)o.ShadowCoLor.Clone();
            return o;
        }
        public SigmaCallout_Shadow()
        {
        }

    }
}
