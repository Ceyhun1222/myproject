using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;

namespace AIXM45Loader
{
    public class AIXM45_RDN_DECLARED_DISTANCE : AIXM45_Object
    {

        private double _valDist;

        public double valDist
        {
            get { return _valDist; }
            set { _valDist = value; }
        }
        private string _uomDist;

        public string uomDist
        {
            get { return _uomDist; }
            set { _uomDist = value; }
        }
        private string _R_mid;

        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }
        private string _R_RdnMid;

        public string R_RdnMid
        {
            get { return _R_RdnMid; }
            set { _R_RdnMid = value; }
        }
        private string _R_codeType;

        public string R_codeType
        {
            get { return _R_codeType; }
            set { _R_codeType = value; }
        }
        private string _R_RWYMid;

        public string R_RWYMid
        {
            get { return _R_RWYMid; }
            set { _R_RWYMid = value; }
        }
        private string _R_RdnTxtDesid;

        public string R_RdnTxtDesid
        {
            get { return _R_RdnTxtDesid; }
            set { _R_RdnTxtDesid = value; }
        }

        public AIXM45_RDN_DECLARED_DISTANCE( AIXM45_RDN linkedRDN, string type, double val)
        {
            this._R_codeType = type;
            this._R_mid = Guid.NewGuid().ToString();
            this._R_RdnMid = linkedRDN.R_mid;
            this._R_RdnTxtDesid = linkedRDN.R_txtDesig;
            this._R_RWYMid = linkedRDN.R_RWYmid;
            this._uomDist = "FT";
            this._valDist = val;
        }

        public AIXM45_RDN_DECLARED_DISTANCE()
        {
        }

    }
}
