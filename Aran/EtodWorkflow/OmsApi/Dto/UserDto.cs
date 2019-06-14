using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OmsApi.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class UserDto : IBaseDto, IUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DtoStatus Status { get; set; }

        public string Aerodrome { get; set; }

        [Required]
        public CompanyDto Company { get; set; }
        public long Id { get; set; }
    }


    public enum DtoStatus
    {
        All,
        Pending = Status.Pending,
        Accepted = Status.Accepted,
        Declined = Status.Declined
    }
}
