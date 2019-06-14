using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace AIXM45Loader
{
    public class AIXM45_Airspace : AIXM45_Object
    {

        private string _txtLocalType;
        public string TxtLocalType
        {
            get { return _txtLocalType; }
            set { _txtLocalType = value; }
        }

        private string _txtName;
        public string TxtName
        {
            get { return _txtName; }
            set { _txtName = value; }
        }

        private string _codeClass;
        public string CodeClass
        {
            get { return _codeClass; }
            set { _codeClass = value; }
        }

        private string _codeLocInd;
        public string CodeLocInd
        {
            get { return _codeLocInd; }
            set { _codeLocInd = value; }
        }

        private string _codeActivity;
        public string CodeActivity
        {
            get { return _codeActivity; }
            set { _codeActivity = value; }
        }

        private string _codeMil;
        public string CodeMil
        {
            get { return _codeMil; }
            set { _codeMil = value; }
        }

        private string _codeDistVerUpper;
        public string CodeDistVerUpper
        {
            get { return _codeDistVerUpper; }
            set { _codeDistVerUpper = value; }
        }

        private double _valDistVerUpper;
        public double ValDistVerUpper
        {
            get { return _valDistVerUpper; }
            set { _valDistVerUpper = value; }
        }

        private string _uomDistVerUpper;
        public string UomDistVerUpper
        {
            get { return _uomDistVerUpper; }
            set { _uomDistVerUpper = value; }
        }

        private string _codeDistVerLower;
        public string CodeDistVerLower
        {
            get { return _codeDistVerLower; }
            set { _codeDistVerLower = value; }
        }

        private double _valDistVerLower;
        public double ValDistVerLower
        {
            get { return _valDistVerLower; }
            set { _valDistVerLower = value; }
        }

        private string _uomDistVerLower;
        public string UomDistVerLower
        {
            get { return _uomDistVerLower; }
            set { _uomDistVerLower = value; }
        }

        private string _codeDistVerMax;
        public string CodeDistVerMax
        {
            get { return _codeDistVerMax; }
            set { _codeDistVerMax = value; }
        }

        private double _valDistVerMax;
        public double ValDistVerMax
        {
            get { return _valDistVerMax; }
            set { _valDistVerMax = value; }
        }

        private string _uomDistVerMax;
        public string UomDistVerMax
        {
            get { return _uomDistVerMax; }
            set { _uomDistVerMax = value; }
        }

        private string _codeDistVerMnm;
        public string CodeDistVerMnm
        {
            get { return _codeDistVerMnm; }
            set { _codeDistVerMnm = value; }
        }

        private double _valDistVerMnm;
        public double ValDistVerMnm
        {
            get { return _valDistVerMnm; }
            set { _valDistVerMnm = value; }
        }

        private string _uomDistVerMnm;
        public string UomDistVerMnm
        {
            get { return _uomDistVerMnm; }
            set { _uomDistVerMnm = value; }
        }

        private double _valLowerLimit;
        public double ValLowerLimit
        {
            get { return _valLowerLimit; }
            set { _valLowerLimit = value; }
        }

        private string _txtRmk;
        public string TxtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }

        private string _R_mid;
        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }

        private string _R_codeType;
        public string R_codeType
        {
            get { return _R_codeType; }
            set { _R_codeType = value; }
        }

        private string _R_codeId;
        public string R_codeId
        {
            get { return _R_codeId; }
            set { _R_codeId = value; }
        }


        private List<AIXM45_AirspaceBorderItem> _border;
        public List<AIXM45_AirspaceBorderItem> Border
        {
            get { return _border; }
            set { _border = value; }
        }

        public AIXM45_Airspace()
        {
        }

       


    }
}
