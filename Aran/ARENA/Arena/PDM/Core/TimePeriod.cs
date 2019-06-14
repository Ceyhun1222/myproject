using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDM
{
    [Serializable()]
    public class TimePeriod 
    {
       
        public TimePeriod()
        {

        }
        public TimePeriod(DateTime beginPosition)
        {
            BeginPosition = beginPosition;
        }

        public TimePeriod(DateTime beginPosition, DateTime? endPosition)
            : this(beginPosition)
        {
            EndPosition = endPosition;
        }


        public DateTime BeginPosition { get; set; }
        

        public DateTime? EndPosition { get; set; }

        public override string ToString()
        {
            return this.EndPosition.HasValue ?  this.BeginPosition.ToShortDateString() + " - " + this.EndPosition.Value.ToShortDateString() : this.BeginPosition.ToShortDateString();
        }
    }
}
