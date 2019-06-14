using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Accent.MapCore
{
    [Serializable()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AcntDataSource
    {
        private string _Link;
        public string Link
        {
            get { return _Link; }
            set { _Link = value; }
        }

        private string _Condition;
        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }

        public AcntDataSource()
        {
            this.Condition = "";
            this.Link = "";
        }

        public AcntDataSource(string _Condition, string _Link)
        {
            this.Condition = _Condition;
            this.Link = _Link;
        }

        public override string ToString()
        {
            return "Condition " + this.Condition + " Link " + this.Link;
        }
    }
}
