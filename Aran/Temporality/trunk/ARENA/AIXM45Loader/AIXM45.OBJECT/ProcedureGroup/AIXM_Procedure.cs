using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public class AIXM45_Procedure : AIXM45_Object
    {
        private string _R_mid;
        public string R_mid
        {
            get { return _R_mid; }
            set { _R_mid = value; }
        }
        
        private string _codeRnp;
        public string codeRnp
        {
            get { return _codeRnp; }
            set { _codeRnp = value; }
        }

        private string _txtDescrComFail;
        public string txtDescrComFail
        {
            get { return _txtDescrComFail; }
            set { _txtDescrComFail = value; }
        }

        private string _codeTypeRte;
        public string codeTypeRte
        {
            get { return _codeTypeRte; }
            set { _codeTypeRte = value; }
        }

        private string _txtDescr;
        public string txtDescr
        {
            get { return _txtDescr; }
            set { _txtDescr = value; }
        }

        private string _txtRmk;
        public string txtRmk
        {
            get { return _txtRmk; }
            set { _txtRmk = value; }
        }

        private string _R_AhpMid;
        public string R_AhpMid
        {
            get { return _R_AhpMid; }
            set { _R_AhpMid = value; }
        }

        private string _R_txtDesig;
        public string R_txtDesig
        {
            get { return _R_txtDesig; }
            set { _R_txtDesig = value; }
        }
 
        private string _R_CODETRANSTYPE;
        public string R_CODETRANSTYPE
        {
            get { return _R_CODETRANSTYPE; }
            set { _R_CODETRANSTYPE = value; }
        }

        private string _R_codeTransId;
        public string R_codeTransId
        {
            get { return _R_codeTransId; }
            set { _R_codeTransId = value; }
        }

        private string _R_codeCatAcft;
        public string R_codeCatAcft
        {
            get { return _R_codeCatAcft; }
            set { _R_codeCatAcft = value; }
        }

        private string _R_RdnMid;
        public string R_RdnMid
        {
            get { return _R_RdnMid; }
            set { _R_RdnMid = value; }
        }

        //private string _txtDescrMiss;
        //public string txtDescrMiss
        //{
        //    get { return _txtDescrMiss; }
        //    set { _txtDescrMiss = value; }
        //}

        private List<AIXM45_Procedure_Leg> _ProcedureLegs;
        public List<AIXM45_Procedure_Leg> ProcedureLegs
        {
            get { return _ProcedureLegs; }
            set { _ProcedureLegs = value; }
        }


        public void LoadLegs(List<AIXM45_Procedure_Leg> Leg_Records)
        {
            //AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();
            AIXM45_Procedure_Leg _Record = (AIXM45_Procedure_Leg)Leg_Records[0];

            this.ProcedureLegs = new List<AIXM45_Procedure_Leg>();
            ProcedureLegs.AddRange(Leg_Records);

            this.codeTypeRte = _Record.Route_Type;
            this.R_txtDesig = _Record.ProcedureIdentifier;
            this.R_AhpMid = _Record.AirportIdentifier;
            this.R_RdnMid = _Record.Transition_Identifier;

        }


        public AIXM45_Procedure()
        {
        }


    }

    public class AIXM45_Procedure_IAP : AIXM45_Procedure
    {

        public AIXM45_Procedure_IAP()
        {
            this.R_mid = Guid.NewGuid().ToString();
            //this.ObjectARINCType = "ApproachProcedures";
        }

    }

    public class AIXM45_Procedure_SID : AIXM45_Procedure
    {

        public AIXM45_Procedure_SID()
        {
            this.R_mid = Guid.NewGuid().ToString();
            //this.ObjectARINCType = "SIDProcedures";
        }

    }

    public class AIXM45_Procedure_STAR : AIXM45_Procedure
    {

        public AIXM45_Procedure_STAR()
        {
            this.R_mid = Guid.NewGuid().ToString();
            //this.ObjectARINCType = "STARProcedures";
        }

    }

}
