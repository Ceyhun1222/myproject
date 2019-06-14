﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ChartServices.DataContract
{

    [DataContract]
    public class ChartUser : IEntity
    {
        [DataMember]
        public virtual long Id { get; set; }

        [DataMember]
        public virtual string FirstName { get; set; }

        [DataMember]
        public virtual string LastName { get; set; }

        [DataMember]
        public virtual string UserName { get; set; }

        [DataMember]
        public virtual string Email { get; set; }

        [DataMember]
        public virtual string Position { get; set; }

        [DataMember]
        public virtual UserPrivilege Privilege { get; set; }

        [DataMember]
        public virtual bool Disabled { get; set; }

        [DataMember]
        public virtual string Password { get; set; }

        public virtual DateTime? LatestLoginAt { get; set; }

        [DataMember]
        public virtual bool IsAdmin { get; set; }
    }
}