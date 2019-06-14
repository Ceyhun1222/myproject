using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;


namespace AIXM45Loader
{
    public class AIXM45_RDN_SWY : AIXM45_Object
    {
        private double _valLen;

        public double valLen
        {
            get { return _valLen; }
            set { _valLen = value; }
        }
        private double _valWid;

        public double valWid
        {
            get { return _valWid; }
            set { _valWid = value; }
        }
        private string _uomDim;

        public string uomDim
        {
            get { return _uomDim; }
            set { _uomDim = value; }
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

        public AIXM45_RDN_SWY(AIXM45_RDN linkedRDN, double val)
        {
            this._R_mid = Guid.NewGuid().ToString();
            this._R_RdnMid = linkedRDN.R_mid;
            this._R_RdnTxtDesid = linkedRDN.R_txtDesig;
            this._R_RWYMid = linkedRDN.R_RWYmid;
            this._uomDim = "FT";
            this._valLen = val;
            this._valWid = linkedRDN._valWid;
        }


        public AIXM45_RDN_SWY()
        {
        }
    }
}
