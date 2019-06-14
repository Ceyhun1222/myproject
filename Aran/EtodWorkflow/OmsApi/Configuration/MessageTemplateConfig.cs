using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetEscapades.Configuration.Validation;

namespace OmsApi.Configuration
{
    public class MessageTemplateConfig : IValidatable
    {
        [Required]
        public string SignupSubject { get; set; }

        [Required]
        public string SignupBody { get; set; }

        [Required]
        public string SignupResponseSubject { get; set; }

        [Required]
        public string SignupResponseBody { get; set; }

        [Required]
        public string ForgotPasswordSubject { get; set; }

        [Required]
        public string ForgotPasswordBody { get; set; }

        [Required]
        public string RequestSubmitSubject { get; set; }

        [Required]
        public string RequestSubmitBody { get; set; }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }
}
