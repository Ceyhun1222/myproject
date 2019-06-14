using System;
using System.Runtime.Serialization;

namespace ChartServices.DataContract
{
    //[DataContract]
    public class ChartUpdateReference : IEntity
    {
        public virtual long Id { get; set; }

        public virtual Guid RefId { get; set; }

        public virtual int Index { get; set; }

        public virtual bool Done { get; set; }
    }
}