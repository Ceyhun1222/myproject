using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntLeaderLine
    {
        private AcntColor _leaderColor;
        [XmlElement]
        [Editor(typeof(MyColorEdotor), typeof(UITypeEditor))]
        public AcntColor LeaderColor
        {
            get { return _leaderColor; }
            set { _leaderColor = value; }
        }
        private lineStyle _leaderLineStyle;
        [XmlElement]
        //[Editor(typeof(LineStyleEditor), typeof(UITypeEditor))]
        public lineStyle LeaderLineStyle
        {
            get { return _leaderLineStyle; }
            set { _leaderLineStyle = value; }
        }
        private int _leaderLineWidth;

        public int LeaderLineWidth
        {
            get { return _leaderLineWidth; }
            set { _leaderLineWidth = value; }
        }

        private lineCalloutStyle _leaderStyle;
        [XmlElement]
        //[Editor(typeof(LineCalloutStyleEditor), typeof(UITypeEditor))]
        public lineCalloutStyle LeaderStyle
        {
            get { return _leaderStyle; }
            set { _leaderStyle = value; }
        }

        public AcntLeaderLine()
        {
            this.LeaderColor = new AcntColor();
            this.LeaderLineStyle = lineStyle.lsSolid;
            this.LeaderLineWidth = 1;
            this.LeaderStyle = lineCalloutStyle.CSBase;
        }


    }

}
