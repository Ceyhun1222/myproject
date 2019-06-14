using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIXM45Loader
{
    public class ProcTransition : AIXM45_Object
    {
        public string Mid { get; set; }

        public string TransId { get; set; }

        public string TransType { get; set; }

        public string Description { get; set; }

        public string ProcedureMid { get; set; }

        public string ProcedureType { get; set; }

        public string PhaseType { get; set; }
    }
}
