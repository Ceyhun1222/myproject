using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;

namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(PropertySorter))]
    public class SigmaCallout_Frame : AbstractChartClass
    {
        private AncorColor _frameColor;
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor FrameColor
        {
            get { return _frameColor; }
            set { _frameColor = value; }
        }

        private lineStyle _frameLineStyle;
        //[XmlElement]
        [Editor(typeof(LineStyleEditor), typeof(UITypeEditor))]
        public lineStyle FrameLineStyle
        {
            get { return _frameLineStyle; }
            set { _frameLineStyle = value; }
        }

        private SigmaCallout_FrameMargins _frameMargins;
        public SigmaCallout_FrameMargins FrameMargins
        {
            get { return _frameMargins; }
            set { _frameMargins = value; }
        }

        private int _offset;
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        
        private double _thickness;
        public double Thickness
        {
            get { return _thickness; }
            set { _thickness = value; }
        }

        private bool _drawLeader;
        public bool DrawLeader
        {
            get { return _drawLeader; }
            set { _drawLeader = value; }
        }

        public SigmaCallout_Frame()
        {
            
        }

        public SigmaCallout_Frame(lineStyle _FrameLineStyle)
        {
            this.FrameColor = new AncorColor(0,0,0);
            this.FrameMargins = new SigmaCallout_FrameMargins { BottomMargin = 1,  FooterHorizontalMargin= 1, HeaderHorizontalMargin = 1, TopMargin = 1 };
            this.Offset = 0;
            this.Thickness = 1;
            this.FrameLineStyle = _FrameLineStyle;
            this.DrawLeader = true;
        }

        public SigmaCallout_Frame(AncorColor _frameColor, SigmaCallout_FrameMargins _frameMargins, int _offset, double _thickness, lineStyle _frameLineStyle)
        {
            this.FrameColor = _frameColor;
            this.FrameMargins = _frameMargins;
            this.Offset = _offset;
            this.Thickness = _thickness;
            this.FrameLineStyle = _frameLineStyle;
            this.DrawLeader = true;

        }

        public override object Clone()
        {
            SigmaCallout_Frame o = (SigmaCallout_Frame)base.Clone();
            o.FrameColor = (AncorColor)o.FrameColor.Clone();
            o.FrameMargins = (SigmaCallout_FrameMargins)o.FrameMargins.Clone();
            return o;
        }

    }

    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SigmaCallout_FrameMargins : AbstractChartClass
    {
        private int _BottomMargin;
        public virtual int BottomMargin
        {
            get { return _BottomMargin; }
            set { _BottomMargin = value; }
        }

        private int _HeaderHorizontalMargin;
        public virtual int HeaderHorizontalMargin
        {
            get { return _HeaderHorizontalMargin; }
            set { _HeaderHorizontalMargin = value; }
        }

        private int _FooterHorizontalMargin;
        public virtual int FooterHorizontalMargin
        {
            get { return _FooterHorizontalMargin; }
            set { _FooterHorizontalMargin = value; }
        }

        private int _TopMargin;
        public virtual int TopMargin
        {
            get { return _TopMargin; }
            set { _TopMargin = value; }
        }

        public SigmaCallout_FrameMargins()
        {

        }

        public SigmaCallout_FrameMargins(int _TopMargin, int _BottomMargin, int _HeaderHorizontalMargin, int _FooterHorizontalMargin)
        {
            this.HeaderHorizontalMargin = _HeaderHorizontalMargin;
            this.FooterHorizontalMargin = _FooterHorizontalMargin;
            this.TopMargin = _TopMargin;
            this.BottomMargin = _BottomMargin;
        }

    }


}
