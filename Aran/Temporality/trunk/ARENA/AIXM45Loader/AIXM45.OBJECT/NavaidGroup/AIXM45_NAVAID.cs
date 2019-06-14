using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIXM45Loader
{
    public class AIXM45_NAVAID : AIXM45_Object
    {
        private string _R_RdnMid;
        public string R_RdnMid
        {
            get { return _R_RdnMid; }
            set { _R_RdnMid = value; }
        }

        private string _R_RWYmid;
        public string R_RWYmid
        {
            get { return _R_RWYmid; }
            set { _R_RWYmid = value; }
        }

        private string _R_AhpMid;
        public string R_AhpMid
        {
            get { return _R_AhpMid; }
            set { _R_AhpMid = value; }
        }

        public AIXM45_NAVAID()
        {
        }
    }
}
