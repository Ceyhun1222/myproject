using System.ComponentModel.DataAnnotations;

namespace OmsApi.Dto
{
    public class ForgotPasword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
