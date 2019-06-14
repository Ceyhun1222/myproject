using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_RDN : AIXM45_Object
    {
        private string _R_mid;
        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }

        private string _R_RWYmid;
        public string R_RWYmid
        {
            get { return _R_RWYmid; }
            set { _R_RWYmid = value; }
        }

        private string _geoLat;
        public string GeoLat
        {
            get { return _geoLat; }
            set { _geoLat = value; }
        }

        private string _geoLong;
        public string GeoLong
        {
            get { return _geoLong; }
            set { _geoLong = value; }
        }

        private string _R_txtDesig;
        public string R_txtDesig
        {
            get { return _R_txtDesig; }
            set { _R_txtDesig = value; }
        }

        private double _valElevTdz;
        public double ValElevTdz
        {
            get { return _valElevTdz; }
            set { _valElevTdz = value; }
        }

        private string _uomElevTdz;
        public string UomElevTdz
        {
            get { return _uomElevTdz; }
            set { _uomElevTdz = value; }
        }

        private double _valMagBrg;
        public double ValMagBrg
        {
            get { return _valMagBrg; }
            set { _valMagBrg = value; }
        }

        private string _R_AhpMid;
        public string R_AhpMid
        {
            get { return _R_AhpMid; }
            set { _R_AhpMid = value; }
        }

        private double _valTrueBrg;
        public double ValTrueBrg
        {
            get { return _valTrueBrg; }
            set { _valTrueBrg = value; }
        }

        private double _R_RdnElev;
        public double R_RdnElev
        {
            get { return _R_RdnElev; }
            set { _R_RdnElev = value; }
        }

        private string _R_RdnElevUom;
        public string R_RdnElevUom
        {
            get { return _R_RdnElevUom; }
            set { _R_RdnElevUom = value; }
        }

        private AIXM45_RDN_DECLARED_DISTANCE _DeclaredDistance;

        public AIXM45_RDN_DECLARED_DISTANCE DeclaredDistance
        {
            get { return _DeclaredDistance; }
            set { _DeclaredDistance = value; }
        }

        private AIXM45_RDN_SWY _StopWay;

        public AIXM45_RDN_SWY StopWay
        {
            get { return _StopWay; }
            set { _StopWay = value; }
        }

        /// <summary>
        /// vspomogatelniye svoystva
        /// </summary>
        public string _ARP_ICAO_CODE;
        public string _OPPOSITE_RDN_TXT_DESIG;
        public double _valLen;
        public double _valWid;
        public string _uomDimRwy;

 
        public AIXM45_RDN()
        {
        }

      }
}
