using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;

namespace AIXM45Loader
{
    public class AIXM45_RWY : AIXM45_Object
    {
        private string _R_mid;
        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }

        private string _R_AHPmid;
        public string R_AHPmid
        {
            get { return _R_AHPmid; }
            set { _R_AHPmid = value; }
        }

        private string _R_txtDesig;
        public string R_txtDesig
        {
            get { return _R_txtDesig; }
            set { _R_txtDesig = value; }
        }

        private double _valLen;
        public double ValLen
        {
            get { return _valLen; }
            set { _valLen = value; }
        }

        private double _valWid;
        public double ValWid
        {
            get { return _valWid; }
            set { _valWid = value; }
        }

        private string _uomDimRwy;
        public string UomDimRwy
        {
            get { return _uomDimRwy; }
            set { _uomDimRwy = value; }
        }

        private double _valLenStrip;
        public double ValLenStrip
        {
            get { return _valLenStrip; }
            set { _valLenStrip = value; }
        }

        private double _valWidStrip;
        public double ValWidStrip
        {
            get { return _valWidStrip; }
            set { _valWidStrip = value; }
        }

        private string _uomDimStrip;
        public string UomDimStrip
        {
            get { return _uomDimStrip; }
            set { _uomDimStrip = value; }
        } 

        private List<AIXM45_RDN> _related_RDN_List;
        public List<AIXM45_RDN> Related_RDN_List
        {
            get { return _related_RDN_List; }
            set { _related_RDN_List = value; }
        }

        private List<AIXM45_RCP> _related_RCP_List;
        public List<AIXM45_RCP> Related_RCP_List
        {
            get { return _related_RCP_List; }
            set { _related_RCP_List = value; }
        }

        public AIXM45_RWY()
        {
        }
    }
}
