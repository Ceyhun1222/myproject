using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class Tokens
    {
        [Required]
        public string Refresh { get; set; }

        [Required]
        public string Access { get; set; }
    }
}
