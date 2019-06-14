using System.ComponentModel.DataAnnotations;

namespace ObstacleManagementSystem.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name ="Username", ResourceType = typeof(Resources.ViewModels.Account.LoginViewModel))]
        public string Username { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [DataType(DataType.Password)]
        [Display(Name ="Password", ResourceType = typeof(Resources.ViewModels.Account.LoginViewModel))]
        public string Password { get; set; }

        [Display(Name = "Remember_me", ResourceType = typeof(Resources.ViewModels.Account.LoginViewModel))]
        public bool RememberMe { get; set; }
    }
}
