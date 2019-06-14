using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TossWebApi.Models.DTO
{
    public class RunwayDto
    {
        public Guid Identifier { get; set; }

        public string Designator { get; set; }
    }
}