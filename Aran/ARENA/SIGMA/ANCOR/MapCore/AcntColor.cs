using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ANCOR.MapCore
{
    [XmlType]
    [Serializable()]
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    [TypeConverter(typeof(PropertySorter))]
    public class AncorColor : AbstractChartClass
    {
        private int _blue;
        //[XmlElement]
        [PropertyOrder(30)]
        public int Blue
        {
            get { return _blue; }
            set { _blue = value; }
        }

        private int _green;
        //[XmlElement]
        [PropertyOrder(20)]
        public int Green
        {
            get { return _green; }
            set { _green = value; }
        }
       
        private int _red;
        //[XmlElement]
        [PropertyOrder(10)]
        public int Red
        {
            get { return _red; }
            set { _red = value; }
        }
    
        public AncorColor()
        {
           
        }

        public AncorColor(int _red, int _green, int _blue)
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
