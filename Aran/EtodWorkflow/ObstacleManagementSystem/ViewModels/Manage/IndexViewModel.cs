using System.ComponentModel.DataAnnotations;
using ObstacleManagementSystem.Extensions;
using ObstacleManagementSystem.Models;

namespace ObstacleManagementSystem.ViewModels.Manage
{
    public class IndexViewModel
    {
        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "EmailDisplay", ResourceType = typeof(Resources.DataAnnotations))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.DataAnnotations), ErrorMessageResourceName = "InvalidEmail")]
        public string EmailAddress { get; set; }

        //public MobileOperator SelectedMobileOperator { get; set; }

        [Required(ErrorMessage = "RequiredAttr")]
        [Display(Name = "MobileNumberDisplay", ResourceType = typeof(Resources.DataAnnotations))]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([1-9]{1})\)?([0-9]{6})$", ErrorMessageResourceType = typeof(Resources.DataAnnotations), ErrorMessageResourceName = "InvalidPhone")]
        public string MobileNumber { get; set; }

        public string Username { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
