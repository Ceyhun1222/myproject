using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChartServices.DataContract
{
    [DataContract]
    //[KnownType(typeof(ChartData))]
    public class Chart : IEntity
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual string Note { get; set; }

        [DataMember]
        public virtual ChartType Type { get; set; }

        [DataMember]
        public virtual ChartUser CreatedBy { get; set; }

        [DataMember]
        public virtual DateTime CreatedAt { get; set; }

        [DataMember]
        public virtual Guid Identifier { get; set; }

        [DataMember]
        public virtual DateTime BeginEffectiveDate { get; set; }

        [DataMember]
        public virtual DateTime PublicationDate { get; set; }

        [DataMember]
        public virtual DateTime? EndEffectiveDate { get; set; }

        [DataMember]
        public virtual string Organization { get; set; }

        [DataMember]
        public virtual string Airport { get; set; }

        [DataMember]
        public virtual string RunwayDirection { get; set; }

        [DataMember]
        public virtual bool IsLocked { get; set; }

        [DataMember]
        public virtual ChartUser LockedBy { get; set; }

        [DataMember]
        public virtual string Version { get; set; }

        [DataMember]
        public virtual bool HasUpdate { get; set; }

        public virtual long UpdatedBasedOn { get; set; }
    }
}