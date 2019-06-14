using System.ComponentModel.DataAnnotations;

namespace ObstacleManagementSystem.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "EmailDisplay", ResourceType = typeof(Resources.DataAnnotations))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.DataAnnotations), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }
    }
}
