using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetEscapades.Configuration.Validation;

namespace OmsApi.Configuration
{
    public class TossConfig : IValidatable
    {
        [Required]
        public string Url { get; set; }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }
}
