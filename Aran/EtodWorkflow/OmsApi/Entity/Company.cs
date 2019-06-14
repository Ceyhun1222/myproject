using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Entity
{
    [Owned]
    public class Company : IBase
    {
        public string Name { get; set; }

        public string Address { get; set; }        

        public string Zip { get; set; }

        public string Telephone { get; set; }

        public string AirportName { get; set; }

        public string AirportId { get; set; }

        public string Fax { get; set; }

        public long Id { get; set; }
    }
}