using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;


namespace ANCOR.MapCore
{
    //[TypeConverter(typeof(ExpandableObjectConverter))]

    public class AbstractChartClass : ICloneable
    {
        public AbstractChartClass()
        {
        }

        virtual public object Clone()
        {
            return this.MemberwiseClone();
        }

       
    }
}
