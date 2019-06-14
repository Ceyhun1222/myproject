using System.ComponentModel.DataAnnotations;

namespace ObstacleManagementSystem.ViewModels.Account
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
