using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIXM45Loader
{
    public class AIXM45_Enrote  : AIXM45_Object
    {
        private string _R_mid;
        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }

        private string _R_txtDesig;
        public string R_txtDesig
        {
            get { return _R_txtDesig; }
            set { _R_txtDesig = value; }
        }

        private string _R_txtLocDesig;
        public string R_txtLocDesig
        {
            get { return _R_txtLocDesig; }
            set { _R_txtLocDesig = value; }
        }

        private string _txtRmk;
        public string TxtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }

        public AIXM45_Enrote()
        {
        }
    }
}
