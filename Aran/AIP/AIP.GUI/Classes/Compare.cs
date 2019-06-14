using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIP.DB;

namespace AIP.GUI.Classes
{
    public class Compare
    {
        [DisplayName("File from comparing folder")]
        public string OtherFile { get; set; }

        [DisplayName("File from current eAIP")]
        public string AIPProdFile { get; set; }

        [DisplayName("Comparison Result")]
        public string Result { get; set; }
    }
}
