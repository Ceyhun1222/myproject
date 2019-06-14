using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChartServices.DataContract
{
    [DataContract]
    public class ChartUpdateData : IEntity
    {
        //[DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual ChartUser CreatedBy { get; set; }

        [DataMember]
        public virtual DateTime CreatedAt { get; set; }

        [DataMember]
        public virtual DateTime EffectiveDate { get; set; }

        [DataMember]
        public virtual string Note { get; set; }

        public virtual IList<ChartUpdateReference> References { get; set; }
    }
}