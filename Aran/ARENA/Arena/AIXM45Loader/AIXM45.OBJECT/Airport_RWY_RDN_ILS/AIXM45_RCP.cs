using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_RCP : AIXM45_Object
    {
        private string _R_mid;

        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }
        private string _R_RWYMID;

        public string R_RWYMID
        {
            get { return _R_RWYMID; }
            set { _R_RWYMID = value; }
        }
        private string _R_geoLat;

        public string R_geoLat
        {
            get { return _R_geoLat; }
            set { _R_geoLat = value; }
        }
        private string _R_geoLong;

        public string R_geoLong
        {
            get { return _R_geoLong; }
            set { _R_geoLong = value; }
        }
        private double _valElev;

        public double valElev
        {
            get { return _valElev; }
            set { _valElev = value; }
        }
        private string _uomDistVer;

        public string uomDistVer
        {
            get { return _uomDistVer; }
            set { _uomDistVer = value; }
        }


        public AIXM45_RCP()
        {
        }


        public AIXM45_RCP(AIXM45_RDN Rdn, IPoint RCPGeometry, string RWYmid)
        {
            this._R_geoLat = Rdn.GeoLat;
            this._R_geoLong = Rdn.GeoLong;
            this._R_mid = Guid.NewGuid().ToString();
            this._valElev = Rdn.R_RdnElev;
            this._uomDistVer = "FT";
            this.Geometry = (IGeometry)RCPGeometry;
            this.R_RWYMID = RWYmid;
        }
    
    
    }
}
