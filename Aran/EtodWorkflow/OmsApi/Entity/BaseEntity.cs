using System;

namespace OmsApi.Entity
{
    public class BaseEntity : IBaseEntity
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}