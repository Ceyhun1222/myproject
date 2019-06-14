using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorArrowMarker : AbstractChartClass
    {
        private double _lenngth;
        public double Length
        {
            get { return _lenngth; }
            set { _lenngth = value; }
        }

        private arrowPosition _position;
        public arrowPosition Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public AncorArrowMarker()
        {
            
        }

        public AncorArrowMarker(double len,double wid ,arrowPosition pos)
        {
            this.Length = len;
            this.Width = wid;
            this.Position = pos;
        }
    }

}
