using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntArrowMarker
    {
        private int _lenngth;

        public int Length
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
        private int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public AcntArrowMarker()
        {
            this.Length = 4;
            this.Width = 4;
            this.Position = arrowPosition.Start;
        }
    }

}
