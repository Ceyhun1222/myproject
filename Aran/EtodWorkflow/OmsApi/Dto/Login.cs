using OmsApi.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OmsApi.Dto
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }

        [Required]        
        public string Password { get; set; }

        public Roles Role { get; set; }
    }    
}
