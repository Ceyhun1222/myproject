using OmsApi.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    interface IUser
    {
        string UserName { get; set; }

        string PhoneNumber { get; set; }

        string Email { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }
    }
}