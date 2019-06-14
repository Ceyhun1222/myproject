using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ObstacleManagementSystem.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "FirstnameDisplay", ResourceType = typeof(Resources.Models.ApplicationUser))]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "LastnameDisplay", ResourceType = typeof(Resources.Models.ApplicationUser))]
        public string Lastname { get; set; }

        [Display(Name = "FathernameDisplay", ResourceType = typeof(Resources.Models.ApplicationUser))]
        public string Fathername { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [DataType(DataType.Date)]
        [Display(Name = "BirthdayDisplay", ResourceType = typeof(Resources.Models.ApplicationUser))]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "GenderDisplay", ResourceType = typeof(Resources.Models.ApplicationUser))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public bool PasswordChanged { get; set; }
        public DateTime DateRegistered { get; set; }
    }

    public enum Gender
    {
        [Display(Name = "Male", ResourceType = typeof(Resources.Models.ApplicationUser))]
        Male = 1,
        [Display(Name = "Female", ResourceType = typeof(Resources.Models.ApplicationUser))]
        Female = 2
    }
}
