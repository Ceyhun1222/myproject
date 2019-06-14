using System;
using System.ComponentModel.DataAnnotations;
using OmsApi.Entity;

namespace OmsApi.Dto
{
    public class UserRegistration : IUser
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

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

        [Required]
        public CompanyDto Company { get; set; }
    }
}