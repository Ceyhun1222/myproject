using System.ComponentModel.DataAnnotations;

namespace ChartManagerWeb.Models.ViewModel
{
    public class UserPasswordViewModel : ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "Old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}