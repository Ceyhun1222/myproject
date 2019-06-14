using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Accent.MapCore
{
    [XmlType]
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntColor
    {
        private int _blue;
        [XmlElement]
        public int Blue
        {
            get { return _blue; }
            set { _blue = value; }
        }

        private int _green;
        [XmlElement]
        public int Green
        {
            get { return _green; }
            set { _green = value; }
        }
       
        private int _red;
        [XmlElement]
        public int Red
        {
            get { return _red; }
            set { _red = value; }
        }
    
        public AcntColor()
        {
            this.Blue = 255;
            this.Green = 255;
            this.Red = 255;
        }

        public AcntColor(int _red, int _green, int _blue)
        {
            this.Blue = _blue;
            this.Green = _green;
            this.Red = _red;
        }

    
   
        public override string ToString()
        {
            string res = "red:" + this.Red.ToString();
            res = res + " green:" + this.Green.ToString(); ;
            res = res + " blue:" + this.Blue.ToString();
            return res;
        }
    }
}
