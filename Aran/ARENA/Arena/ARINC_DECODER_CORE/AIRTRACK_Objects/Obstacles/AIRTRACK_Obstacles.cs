using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System.Data;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects 
{
    public class Obstacle_AIRTRACK : Object_AIRTRACK
    {

        private string _iD;

        public string ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private double _accHor;

        public double AccHor_M
        {
            get { return _accHor; }
            set { _accHor = value; }
        }

        private double _accVert;

        public double AccVert_M
        {
            get { return _accVert; }
            set { _accVert = value; }
        }

        private double _elev;

        public double Elev
        {
            get { return _elev; }
            set { _elev = value; }
        }

        private string _elevUOM;

        public string ElevUOM
        {
            get { return _elevUOM; }
            set { _elevUOM = value; }
        }

        private double _elevM;

        public double ElevM
        {
            get { return _elevM; }
            set { _elevM = value; }
        }

        private double _height;

        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private string _heightUOM;

        public string HeightUOM
        {
            get { return _heightUOM; }
            set { _heightUOM = value; }
        }

        private double _heightM;

        public double HeightM
        {
            get { return _heightM; }
            set { _heightM = value; }
        }

        private bool _lighting;

        public bool Lighting
        {
            get { return _lighting; }
            set { _lighting = value; }
        }

        private bool _marking;

        public bool Marking
        {
            get { return _marking; }
            set { _marking = value; }
        }

        private string _obs_type;

        public string Obs_type
        {
            get { return _obs_type; }
            set { _obs_type = value; }
        }

        public Obstacle_AIRTRACK()
        {
        }

        public Obstacle_AIRTRACK(DataRow JeppesenobsRow)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            //this.ARINC_OBJ = arincObj;
   
            #region

            double dbl = 0;

            Double.TryParse(JeppesenobsRow["ACC_HORIZ"].ToString(), out dbl);
            this.AccHor_M = ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT");

            Double.TryParse(JeppesenobsRow["ACC_VERT"].ToString(), out dbl);
            this.AccVert_M= ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT");

            this.City = JeppesenobsRow["CITY"].ToString();

            Double.TryParse(JeppesenobsRow["ELEV_FT"].ToString(), out dbl);
            this.Elev = dbl;
            this.ElevM= ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT");
            this.ElevUOM ="FT";

            
            Double.TryParse(JeppesenobsRow["HEIGHT_FT"].ToString(), out dbl);
            this.Height = dbl;
            this.HeightM= ArnUtil.ConvertValueToMeter(dbl.ToString(), "FT");
            this.HeightUOM ="FT";

            this.Lighting =  JeppesenobsRow["LIGHTING"].ToString().StartsWith("L");
            this.Lighting =  JeppesenobsRow["MARKING"].ToString().StartsWith("M");
            this.Obs_type = JeppesenobsRow["OBS_TYPE"].ToString();
            this.ID = JeppesenobsRow["ID"].ToString();




            dbl = 0;


            if ((JeppesenobsRow["DEC_LONG"].ToString().Trim().Length > 0) && (JeppesenobsRow["DEC_LAT"].ToString().Trim().Length > 0))
            {

                IPoint pnt = ArnUtil.Create_ESRI_POINT(JeppesenobsRow["DEC_LAT"].ToString().Trim(), JeppesenobsRow["DEC_LONG"].ToString().Trim(), this.ElevM.ToString(), "M");


                if (pnt != null)
                {
                    this.Shape = new Shape_AIRTRACK(JeppesenobsRow["DEC_LAT"].ToString().Trim(), JeppesenobsRow["DEC_LONG"].ToString().Trim());
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }

            this.INFO_AIRTRACK = JeppesenobsRow["OBS_TYPE"].ToString();
            #endregion
        }
    }
}
