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
    public class SigmaCallout_Morse : AbstractChartClass
    {
        private string _MorseText;
        //[Browsable(false)]
        [ReadOnly(true)]
        [SkipAttribute(true)]
        [PropertyOrder(10)]
        public string MorseText
        {
            get { return _MorseText; }
            set { _MorseText = value; }
        }

        private int _MoseSize;
        //[Browsable(false)]
        [SkipAttribute(false)]
        [PropertyOrder(10)]
        public int MorseSize
        {
            get { return _MoseSize; }
            set { _MoseSize = value; }
        }

        private AncorColor _MorseColor;
        [SkipAttribute(false)]
        [PropertyOrder(10)]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor MorseColor
        {
            get { return _MorseColor; }
            set { _MorseColor = value; }
        }

        double _MorseLocationShiftOnX;
        [SkipAttribute(false)]
        [PropertyOrder(20)]
        public double MorseLocationShiftOnX
        {
            get { return _MorseLocationShiftOnX; }
            set { _MorseLocationShiftOnX = value; }
        }

        double _MorseLocationShiftOnY;
        [SkipAttribute(false)]
        [PropertyOrder(30)]
        public double MorseLocationShiftOnY
        {
            get { return _MorseLocationShiftOnY; }
            set { _MorseLocationShiftOnY = value; }
        }

        double _MorseLeading;
        [SkipAttribute(false)]
        [PropertyOrder(40)]
        public double MorseLeading
        {
            get { return _MorseLeading; }
            set { _MorseLeading = value; }
        }

        private bool _verticalMorse;
        [SkipAttribute(false)]
        [PropertyOrder(50)]
        public bool VerticalMorse
        {
            get { return _verticalMorse; }
            set { _verticalMorse = value; }
        }

        public SigmaCallout_Morse()
        {
        }

        public override object Clone()
        {
            SigmaCallout_Morse o = (SigmaCallout_Morse)base.Clone();
            if(o.MorseColor !=null) o.MorseColor = (AncorColor)o.MorseColor.Clone();
            return o;
        }

    }
}
