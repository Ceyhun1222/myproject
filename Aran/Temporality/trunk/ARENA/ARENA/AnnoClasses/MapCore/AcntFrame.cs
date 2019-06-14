using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;

namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntFrame
    {
        private AcntColor _frameColor;
        [XmlElement]
        [Editor(typeof(MyColorEdotor), typeof(UITypeEditor))]
        public AcntColor FrameColor
        {
            get { return _frameColor; }
            set { _frameColor = value; }
        }
        private lineStyle _frameLineStyle;
        [XmlElement]
        //[Editor(typeof(LineStyleEditor), typeof(UITypeEditor))]
        public lineStyle FrameLineStyle
        {
            get { return _frameLineStyle; }
            set { _frameLineStyle = value; }
        }
        private AcntFrameMargins _frameMargins;

        public AcntFrameMargins FrameMargins
        {
            get { return _frameMargins; }
            set { _frameMargins = value; }
        }
        private int _offset;
        [XmlElement]
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
    
        public AcntFrame()
        {
            this.FrameColor = new AcntColor(0,0,0);
            this.FrameMargins = new AcntFrameMargins();
            this.Offset = 0;
            this.Thickness = 1;
            this.FrameLineStyle = lineStyle.lsSolid;
        }

        public AcntFrame(AcntColor _frameColor, AcntFrameMargins _frameMargins, int _offset, double _thickness, lineStyle _frameLineStyle)
        {
            this.FrameColor = _frameColor;
            this.FrameMargins = _frameMargins;
            this.Offset = _offset;
            this.Thickness = _thickness;
            this.FrameLineStyle = _frameLineStyle;
        }
    }
}
