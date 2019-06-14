using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorLeaderLine : AbstractChartClass
    {
        private AncorColor _leaderColor;
        //[XmlElement]
        [Editor(typeof(SigmaColorEdotor), typeof(UITypeEditor))]
        public AncorColor LeaderColor
        {
            get { return _leaderColor; }
            set { _leaderColor = value; }
        }

        private lineStyle _leaderLineStyle;
        //[XmlElement]
        [Editor(typeof(LineStyleEditor), typeof(UITypeEditor))]
        public lineStyle LeaderLineStyle
        {
            get { return _leaderLineStyle; }
            set { _leaderLineStyle = value; }
        }

        private double _leaderLineWidth;
        public double LeaderLineWidth
        {
            get { return _leaderLineWidth; }
            set { _leaderLineWidth = value; }
        }

        private lineCalloutStyle _leaderStyle;
        //[XmlElement]
        [Editor(typeof(LineCalloutStyleEditor), typeof(UITypeEditor))]
        public lineCalloutStyle LeaderStyle
        {
            get { return _leaderStyle; }
            set { _leaderStyle = value; }
        }

        private bool _endsWithArrow;
         [Category("Arrow")]
        public bool EndsWithArrow
        {
            get { return _endsWithArrow; }
            set { _endsWithArrow = value; }
        }

        private AncorArrowMarker _arrowMarker;
        //[Browsable(false)]
        [Category("Arrow")]
        public AncorArrowMarker ArrowMarker
        {
            get { return _arrowMarker; }
            set { _arrowMarker = value; }
        }


        public AncorLeaderLine()
        { }

        public AncorLeaderLine(lineCalloutStyle _LeaderStyle)
        {
            this.LeaderColor = new AncorColor (0,0,0);
            this.LeaderLineStyle = lineStyle.lsSolid;
            this.LeaderLineWidth = 1;
            this.LeaderStyle = _LeaderStyle;
            this.EndsWithArrow = false;
            this.ArrowMarker = new AncorArrowMarker(this.LeaderLineWidth, this.LeaderLineWidth, arrowPosition.Start);
        }

        public override object Clone()
        {
            AncorLeaderLine o = (AncorLeaderLine)base.Clone();
            o.LeaderColor = (AncorColor)o.LeaderColor.Clone();
            o.ArrowMarker = (AncorArrowMarker)o.ArrowMarker.Clone();
            return o;
        }

    }

}
