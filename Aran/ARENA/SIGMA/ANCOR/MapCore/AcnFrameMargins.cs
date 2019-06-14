using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorFrameMargins : AbstractChartClass
    {
        private int _bottom;

        public int Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }
        private int _left;

        public int Left
        {
            get { return _left; }
            set { _left = value; }
        }
        private int _right;

        public int Right
        {
            get { return _right; }
            set { _right = value; }
        }
        private int _top;

        public int Top
        {
            get { return _top; }
            set { _top = value; }
        }
    
        public AncorFrameMargins()
        {
           
        }

        public AncorFrameMargins(int _top, int _bottom, int _left, int _right)
        {
            this.Left = _left;
            this.Right = _right;
            this.Top = _top;
            this.Bottom = _bottom;
        }
    }
}
