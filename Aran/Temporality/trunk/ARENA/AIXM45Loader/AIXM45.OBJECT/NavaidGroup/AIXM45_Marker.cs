using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;


namespace AIXM45Loader  
{

    public class AIXM45_Marker : AIXM45_NAVAID
    {
        private string _R_mid;

        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }
        private string _R_codeId;

        public string R_codeId
        {
            get { return _R_codeId; }
            set { _R_codeId = value; }
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
        private string _codeClass;

        public string CodeClass
        {
            get { return _codeClass; }
            set { _codeClass = value; }
        }
        private string _codePsnIls;

        public string CodePsnIls
        {
            get { return _codePsnIls; }
            set { _codePsnIls = value; }
        }
        private double _valFreq;

        public double ValFreq
        {
            get { return _valFreq; }
            set { _valFreq = value; }
        }
        private string _uomFreq;

        public string UomFreq
        {
            get { return _uomFreq; }
            set { _uomFreq = value; }
        }
        private string _txtName;

        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }
        private double _valAxisBrg;

        public double ValAxisBrg
        {
            get { return _valAxisBrg; }
            set { _valAxisBrg = value; }
        }
        private string _codeEm;

        public string CodeEm
        {
            get { return _codeEm; }
            set { _codeEm = value; }
        }
        private string _codeDatum;

        public string CodeDatum
        {
            get { return _codeDatum; }
            set { _codeDatum = value; }
        }
        private string _uomGeoAccuracy;

        public string UomGeoAccuracy
        {
            get { return _uomGeoAccuracy; }
            set { _uomGeoAccuracy = value; }
        }
        private double _valElev;

        public double ValElev
        {
            get { return _valElev; }
            set { _valElev = value; }
        }
        private double _valElevAccuracy;

        public double ValElevAccuracy
        {
            get { return _valElevAccuracy; }
            set { _valElevAccuracy = value; }
        }
        private double _valGeoidUndulation;

        public double ValGeoidUndulation
        {
            get { return _valGeoidUndulation; }
            set { _valGeoidUndulation = value; }
        }
        private string _uomDistVer;

        public string UomDistVer
        {
            get { return _uomDistVer; }
            set { _uomDistVer = value; }
        }
        private string _R_NDBMID;

        public string R_NDBMID
        {
            get { return _R_NDBMID; }
            set { _R_NDBMID = value; }
        }
        private string _R_ILSMID;

        public string R_ILSMID
        {
            get { return _R_ILSMID; }
            set { _R_ILSMID = value; }
        }

        public AIXM45_Marker()
        {
        }

    }
}
