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
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorFrame : AbstractChartClass
    {
        private AncorColor _frameColor;
        //[XmlElement]
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
        private AncorFrameMargins _frameMargins;

        public AncorFrameMargins FrameMargins
        {
            get { return _frameMargins; }
            set { _frameMargins = value; }
        }
        private int _offset;
        //[XmlElement]
        [Browsable(false)]
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

        public AncorFrame()
        {
            
        }

        public AncorFrame(lineStyle _FrameLineStyle)
        {
            this.FrameColor = new AncorColor(0,0,0);
            this.FrameMargins = new AncorFrameMargins {Bottom =1,Left = 1,Right =1,Top =1 };
            this.Offset = 0;
            this.Thickness = 1;
            this.FrameLineStyle = _FrameLineStyle;
        }

        public AncorFrame(AncorColor _frameColor, AncorFrameMargins _frameMargins, int _offset, double _thickness, lineStyle _frameLineStyle)
        {
            this.FrameColor = _frameColor;
            this.FrameMargins = _frameMargins;
            this.Offset = _offset;
            this.Thickness = _thickness;
            this.FrameLineStyle = _frameLineStyle;
        }

        public override object Clone()
        {
            AncorFrame o = (AncorFrame)base.Clone();
            o.FrameColor = (AncorColor)o.FrameColor.Clone();
            o.FrameMargins = (AncorFrameMargins)FrameMargins.Clone();
            return o;
        }
    }
}
