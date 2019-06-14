using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARINC_Types;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace ARINC_DECODER_CORE.AIRTRACK_Objects
{
  
    public class ILS_AIRTRACK : Object_AIRTRACK
    {
        public ILS_AIRTRACK()
        {

        }

        public ILS_AIRTRACK(ARINC_OBJECT arincObj)
        {
            AranSupport.Utilitys ArnUtil = new AranSupport.Utilitys();

            this.ID_AIRTRACK = Guid.NewGuid().ToString();
            this.ARINC_OBJ = arincObj;
            ARINC_LocalizerGlideSlope_Primary_Record ARINC_ILS = (ARINC_LocalizerGlideSlope_Primary_Record)this.ARINC_OBJ;

            #region ILS.Localizer position

            if ((ARINC_ILS.Localizer_Latitude.Length > 0) && (ARINC_ILS.Localizer_Longitude.Trim().Length > 0))
            {

                string LonSign = ARINC_ILS.Localizer_Longitude.Trim().Substring(0, 1);
                string LatSign = ARINC_ILS.Localizer_Latitude.Trim().Substring(0, 1);

                string _geoLat = ARINC_ILS.Localizer_Latitude.Trim();
                string _geoLong = ARINC_ILS.Localizer_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, "0", "FT");

                if (pnt != null)
                {
                    //_geoLong = ArnUtil.ConvertLongitudeToAIXM45COORD(pnt.X, LonSign);
                    //_geoLat = ArnUtil.ConvertLatgitudeToAIXM45COORD(pnt.Y, LatSign);

                    this.Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.Shape.Geometry = pnt as IGeometry;
                }
            }

            #endregion

            #region ILS.GlideSlope position

            if ((ARINC_ILS.Glide_Slope_Latitude.Length > 0) && (ARINC_ILS.Glide_Slope_Longitude.Trim().Length > 0))
            {

                string LonSign = ARINC_ILS.Glide_Slope_Longitude.Trim().Substring(0, 1);
                string LatSign = ARINC_ILS.Glide_Slope_Latitude.Trim().Substring(0, 1);

                string _geoLat = ARINC_ILS.Glide_Slope_Latitude.Trim();
                string _geoLong = ARINC_ILS.Glide_Slope_Longitude.Trim();

                IPoint pnt = ArnUtil.Create_ESRI_POINT(_geoLat, _geoLong, "0", "FT");

                if (pnt != null)
                {
                    //_geoLong = ArnUtil.ConvertLongitudeToAIXM45COORD(pnt.X, LonSign);
                    //_geoLat = ArnUtil.ConvertLatgitudeToAIXM45COORD(pnt.Y, LatSign);

                    this.GlideSlope_Shape = new Shape_AIRTRACK(_geoLat, _geoLong);
                    this.GlideSlope_Shape.Geometry = pnt as IGeometry;
                }
            }

            #endregion

            this.INFO_AIRTRACK = ARINC_ILS.Name;
        }

        //private ARINC_LocalizerGlideSlope_Primary_Record _ARINC_LocalizerGlideSlope;

        //public ARINC_LocalizerGlideSlope_Primary_Record ARINC_ILS
        //{
        //    get { return _ARINC_LocalizerGlideSlope; }
        //    set { _ARINC_LocalizerGlideSlope = value; }
        //}


        private Shape_AIRTRACK _GlideSlope_Shape;
        [XmlIgnore]
        public Shape_AIRTRACK GlideSlope_Shape
        {
            get { return _GlideSlope_Shape; }
            set { _GlideSlope_Shape = value; }
        }

        private VHF_NAVAID_AIRTRACK _relatedDME;

        public VHF_NAVAID_AIRTRACK RelatedDME
        {
            get { return _relatedDME; }
            set { _relatedDME = value; }
        }

        private string _valMagBrg;

        public string ValMagBrg
        {
            get { return _valMagBrg; }
            set { _valMagBrg = value; }
        }

        private string _uomFreq;

        public string UomFreq
        {
            get { return _uomFreq; }
            set { _uomFreq = value; }
        }

        private string _ILS_Category;
        [XmlElement]
        public string ILS_Category
        {
            get { return _ILS_Category; }
            set { _ILS_Category = value; }
        }



    }
}
