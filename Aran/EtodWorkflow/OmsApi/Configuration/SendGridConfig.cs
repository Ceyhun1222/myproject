using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetEscapades.Configuration.Validation;

namespace OmsApi.Configuration
{
    public class SendGridConfig : IValidatable
    {
        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string FromAddress { get; set; }

        [Required]
        public string Title { get; set; }

        public string FromName { get; set; }

        public string ProxyHost { get; set; }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }   
}