using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
    public class VHF_NAVAID_AIRTRACK : Object_AIRTRACK
    {
        public enum VHF_NAVAID_code
        {
            VOR = 0,
            VOR_DME = 1,
            VOR_TACAN = 2,
            DME = 3,
            TACAN = 4,
            DME_ILS,
            TACAN_ILS
        }


        private VHF_NAVAID_code _VHF_code;
        [System.ComponentModel.Browsable(false)]
        public VHF_NAVAID_code VHF_code
        {
            get { return _VHF_code; }
            set { _VHF_code = value; }
        }

        private string _airportIcaoID;

        public string AirportIcaoID
        {
            get { return _airportIcaoID; }
            set { _airportIcaoID = value; }
        }

        public VHF_NAVAID_AIRTRACK()
        {
        }
    }
}
