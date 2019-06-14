using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_Obstacles : AIXM45_Object
    {
        private string _R_mid;

        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
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
        private string _txtName;

        public string txtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }
        private string _txtDescrType;

        public string txtDescrType
        {
            get { return _txtDescrType; }
            set { _txtDescrType = value; }
        }
        private bool _codeGroup;

        public bool codeGroup
        {
            get { return _codeGroup; }
            set { _codeGroup = value; }
        }
        private bool _codeLgt;

        public bool codeLgt
        {
            get { return _codeLgt; }
            set { _codeLgt = value; }
        }
        private string _txtDescrLgt;

        public string txtDescrLgt
        {
            get { return _txtDescrLgt; }
            set { _txtDescrLgt = value; }
        }
        private string _txtDescrMarking;

        public string txtDescrMarking
        {
            get { return _txtDescrMarking; }
            set { _txtDescrMarking = value; }
        }
        private string _codeDatum;

        public string CodeDatum
        {
            get { return _codeDatum; }
            set { _codeDatum = value; }
        }
        private double _valGeoAccuracy;

        public double valGeoAccuracy
        {
            get { return _valGeoAccuracy; }
            set { _valGeoAccuracy = value; }
        }
        private string _uomGeoAccuracy;

        public string uomGeoAccuracy
        {
            get { return _uomGeoAccuracy; }
            set { _uomGeoAccuracy = value; }
        }
        private double _valElev;

        public double valElev
        {
            get { return _valElev; }
            set { _valElev = value; }
        }
        private double _valElevAccuracy;

        public double valElevAccuracy
        {
            get { return _valElevAccuracy; }
            set { _valElevAccuracy = value; }
        }
        private double _valHgt;

        public double valHgt
        {
            get { return _valHgt; }
            set { _valHgt = value; }
        }
        private double _valGeoidUndulation;

        public double valGeoidUndulation
        {
            get { return _valGeoidUndulation; }
            set { _valGeoidUndulation = value; }
        }
        private string _uomDistVer;

        public string uomDistVer
        {
            get { return _uomDistVer; }
            set { _uomDistVer = value; }
        }
        private string _valCrc;

        public string ValCrc
        {
            get { return _valCrc; }
            set { _valCrc = value; }
        }
        private string _txtVerDatum;

        public string txtVerDatum
        {
            get { return _txtVerDatum; }
            set { _txtVerDatum = value; }
        }
        private string _txtRmk;

        public string txtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }


        public AIXM45_Obstacles()
        {
        }
    }
}
