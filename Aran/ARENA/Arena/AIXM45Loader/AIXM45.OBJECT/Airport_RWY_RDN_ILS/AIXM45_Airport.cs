using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{

    public class AIXM45_Airport : AIXM45_Object
    {
        private string _R_codeId;
        public string R_codeId
        {
            get { return _R_codeId; }
            set { _R_codeId = value; }
        }

        private string _R_MID;
        public string R_MID
        {
            get { return _R_MID; }
            set { _R_MID = value; }
        }

        private string _codeIcao;
        public string CodeIcao
        {
            get { return _codeIcao; }
            set { _codeIcao = value; }
        }

        private string _codeIata;
        public string CodeIata
        {
            get { return _codeIata; }
            set { _codeIata = value; }
        }

        private string _codeType;
        public string CodeType
        {
            get { return _codeType; }
            set { _codeType = value; }
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

        private double _valMagVar;
        public double ValMagVar
        {
            get { return _valMagVar; }
            set { _valMagVar = value; }
        }

        private double _valTransitionAlt;
        public double ValTransitionAlt
        {
            get { return _valTransitionAlt; }
            set { _valTransitionAlt = value; }
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

        private string _uomDistVer;
        public string UomDistVer
        {
            get { return _uomDistVer; }
            set { _uomDistVer = value; }
        }

        private string _uomGeoAccuracy;
        public string UomGeoAccuracy
        {
            get { return _uomGeoAccuracy; }
            set { _uomGeoAccuracy = value; }
        }

        private string uomTransitionAlt;
        public string UomTransitionAlt
        {
            get { return uomTransitionAlt; }
            set { uomTransitionAlt = value; }
        }

        private string _txtDescrRefPt;
        public string TxtDescrRefPt
        {
            get { return _txtDescrRefPt; }
            set { _txtDescrRefPt = value; }
        }

        private string _txtName;
        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private string _codeTypeMilOps;
        public string CodeTypeMilOps
        {
            get { return _codeTypeMilOps; }
            set { _codeTypeMilOps = value; }
        }

        double _valGeoAccuracy;
        public double ValGeoAccuracy
        {
            get { return _valGeoAccuracy; }
            set { _valGeoAccuracy = value; }
        }


        private string _txtNameCitySer;
        public string TxtNameCitySer
        {
            get { return _txtNameCitySer; }
            set { _txtNameCitySer = value; }
        }

        


        public AIXM45_Airport()
        {
        }

    }


}
