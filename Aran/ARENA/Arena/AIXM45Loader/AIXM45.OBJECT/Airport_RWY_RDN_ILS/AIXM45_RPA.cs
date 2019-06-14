using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;


namespace AIXM45Loader
{
    public class AIXM45_RPA : AIXM45_Object
    {
        private double _valWid;
        public double valWid
        {
            get { return _valWid; }
            set { _valWid = value; }
        }

        private double _valLen;
        public double valLen
        {
            get { return _valLen; }
            set { _valLen = value; }
        }

        private string _uomDim;
        public string UomDim
        {
            get { return _uomDim; }
            set { _uomDim = value; }
        }
        //private string _codeComposition;
        //private string _codePreparation;
        //private string _codeCondSfc;
        //private double _valPcnClass;
        //private string _codePcnPavementType;
        //private string _codePcnPavementSubgrade;
        //private string _codePcnMaxTirePressure;
        //private double _valPcnMaxTirePressure;
        //private string _codePcnEvalMethod;
        //private string _txtPcnNote;
        //private double _valLcnClass;
        //private double _valSiwlWeight;
        //private string _uomSiwlWeight;
        //private double _valSiwlTirePressure;
        //private string _uomSiwlTirePressure;
        //private double _valAuwWeight;
        //private string _uomAuwWeight;
        //private string _codeSts;
        //private string _txtLgt;
        //private string _txtRmk;

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

        public AIXM45_RPA()
        {
        }

    }
}
