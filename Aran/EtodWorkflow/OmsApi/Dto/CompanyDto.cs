using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class CompanyDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
        
        [Required]
        public string Zip { get; set; }

        [Required]
        public string AirportName { get; set; }

        [Required]
        public string AirportId { get; set; }

        [Required]
        public string Telephone { get; set; }

        public string Fax { get; set; }
    }
}
