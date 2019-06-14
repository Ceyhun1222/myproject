using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIP.DB;

namespace AIP.GUI.Classes
{
    public class Page
    {
        [DisplayName("Created")]
        public bool isCreated { get; set; }

        [DisplayName("Page type")]
        public PageType pageType { get; set; }

        [DisplayName("Extention")]
        public DocType docType { get; set; }

        [DisplayName("Created by")]
        public string CreatedUser { get; set; }

        [DisplayName("Creation date")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Changed by")]
        public string ChangedUser { get; set; }

        [DisplayName("Modified date")]
        public DateTime? ChangedDate { get; set; }
    }
}
