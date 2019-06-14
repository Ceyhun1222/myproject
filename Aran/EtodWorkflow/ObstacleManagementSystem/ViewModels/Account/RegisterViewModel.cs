using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ObstacleManagementSystem.Controllers;
using ObstacleManagementSystem.Extensions;

namespace ObstacleManagementSystem.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "RequiredAttr")]
        [StringLength(50, ErrorMessage = "UsernameErrorMessage", MinimumLength = 3)]
        [Display(Name = "Username")]
        [Remote(nameof(AccountController.ValidateUsernameAsync), "Account")]
        public string Username { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "EmailDisplay", ResourceType = typeof(Resources.DataAnnotations))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.DataAnnotations), ErrorMessageResourceName = "InvalidEmail")]
        [Remote(nameof(AccountController.ValidateEmailAsync), "Account")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Common), ErrorMessageResourceName = "RequiredAttr")]
        [StringLength(50, ErrorMessage = "NameErrorMessage", MinimumLength = 3)]
        [Display(Name = "Name")]        
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        //public MobileOperator SelectedMobileOperator { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "MobileNumberDisplay", ResourceType = typeof(Resources.DataAnnotations))]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [DataType(DataType.Date)]
        [Display(Name = "BirthdayDisplay", ResourceType = typeof(Resources.Models.ApplicationUser))]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
