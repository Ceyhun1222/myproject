using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ANCOR.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AncorDataSource : AbstractChartClass
    {
        private string _Link;
        [ReadOnly(true)]
        public string Link
        {
            get { return _Link; }
            set { _Link = value; }
        }

        private string _Condition;
        [Browsable(false)]
        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }

        private string _value;
        [Browsable(false)]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public AncorDataSource()
        {
            
        }

        public AncorDataSource(string _Condition, string _Link)
        {
            this.Condition = _Condition;
            this.Link = _Link;
            this.Value = "value";
        }

        public override string ToString()
        {
            return "Condition " + this.Condition + " Link " + this.Link;
        }
    }
}
