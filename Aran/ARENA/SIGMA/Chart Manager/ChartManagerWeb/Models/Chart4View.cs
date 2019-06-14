using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChartManagerWeb.Models
{
    public class Chart4View
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public string Effective { get; set; }

        [Display(Name = "Published")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Published by")]
        public string CreatedBy { get; set; }

        public string Version { get; set; }

        public bool Locked { get; set; }

        [Display(Name = "Locked by")]
        public string LockedBy { get; set; }

        public string Airport { get; set; }
        public string Organization { get; set; }
        public string RunwayDirection { get; set; }
    }
}