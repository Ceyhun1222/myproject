using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;


namespace ANCOR.MapCore
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorShadow : AbstractChartClass
    {
        private AncorColor _ShadowColor;
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor ShadowColor
        {
            get { return _ShadowColor; }
            set { _ShadowColor = value; }
        }

        private double _ShadowOffSet;

        public double ShadowOffSet
        {
            get { return _ShadowOffSet; }
            set { _ShadowOffSet = value; }
        }

        public AncorShadow()
        {
        }

        public override object Clone()
        {
            AncorShadow o = (AncorShadow)base.Clone();
            o.ShadowColor = (AncorColor)o.ShadowColor.Clone();
            return o;

        }

    }
}
