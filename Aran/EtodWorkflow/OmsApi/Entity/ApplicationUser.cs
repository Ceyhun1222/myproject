using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace OmsApi.Entity
{
    public class ApplicationUser : IdentityUser<long>, IBaseEntity
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string RefreshToken { get; set; }

        public DateTime CreatedAt { get; set; }

        public Status Status { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public bool Disabled { get; set; }

        public Company Company { get; set; }
    }

    public enum Status
    {
        New,
        Pending,
        Accepted,
        Declined
    }
}