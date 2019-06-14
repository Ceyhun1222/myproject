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
    public class SigmaCallout_AirspaceSign : AbstractChartClass
    {
        private string _AirspaceSymbols;
        [PropertyOrder(10)]
        //[ReadOnly(true)]
        public string AirspaceSymbols
        {
            get { return _AirspaceSymbols; }
            set { _AirspaceSymbols = value; }
        }

        private AncorColor _AirspaceBackColor;
        [PropertyOrder(20)]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor AirspaceBackColor
        {
            get { return _AirspaceBackColor; }
            set { _AirspaceBackColor = value; }
        }

        private bool _AirspaceOnLeftSide;
        [PropertyOrder(30)]
        public bool AirspaceOnLeftSide
        {
            get { return _AirspaceOnLeftSide; }
            set { _AirspaceOnLeftSide = value; }
        }

        private bool _AirspaceSignScaleToFit;
        [PropertyOrder(40)]
        public bool AirspaceSignScaleToFit
        {
            get { return _AirspaceSignScaleToFit; }
            set { _AirspaceSignScaleToFit = value; }
        }

        private int _AirspaceSignSize;
        [PropertyOrder(50)]
        public int AirspaceSignSize
        {
            get { return _AirspaceSignSize; }
            set { _AirspaceSignSize = value; }
        }


        private AncorFont _AirspaceSignFont;
        [PropertyOrder(60)]
        public AncorFont AirspaceSignFont
        {
            get { return _AirspaceSignFont; }
            set { _AirspaceSignFont = value; }
        }

        

        public SigmaCallout_AirspaceSign()
        {
        }

        public override object Clone()
        {
            SigmaCallout_AirspaceSign o = (SigmaCallout_AirspaceSign)base.Clone();
            if (o.AirspaceBackColor != null) o.AirspaceBackColor = (AncorColor)o.AirspaceBackColor.Clone();
            if (o.AirspaceSignFont != null) o.AirspaceSignFont = (AncorFont)o.AirspaceSignFont.Clone();

            return o;
        }
    }
}
