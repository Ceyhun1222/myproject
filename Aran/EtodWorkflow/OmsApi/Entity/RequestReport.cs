using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Entity
{
    public class RequestReport : BaseEntity
    {
        public string SurfaceName { get; set; }

        public Guid SurfaceIdentifier { get; set; }

        public double Penetrate { get; set; }

        public string RunwayDirection { get; set; }

        [Required]
        public Request Request { get; set; }
    }
}