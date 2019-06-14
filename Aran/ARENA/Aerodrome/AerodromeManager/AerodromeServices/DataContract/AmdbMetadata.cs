using System;
using System.Runtime.Serialization;

namespace AerodromeServices.DataContract
{
    [DataContract]
    //[KnownType(typeof(ChartData))]
    public class AmdbMetadata:IEntity
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual User CreatedBy { get; set; }

        [DataMember]
        public virtual DateTime CreatedAt { get; set; }

        [DataMember]
        public virtual Guid Identifier { get; set; }            

        [DataMember]
        public virtual string Organization { get; set; }

        [DataMember]
        public virtual string Airport { get; set; }

        [DataMember]
        public virtual bool IsLocked { get; set; }

        [DataMember]
        public virtual User LockedBy { get; set; }   

        [DataMember]
        public virtual  string Version { get; set; }


    }   
}
