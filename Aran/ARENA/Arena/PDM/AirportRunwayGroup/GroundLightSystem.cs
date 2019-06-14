using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace PDM
{
    [Serializable()]
    public class LightSystem : PDMObject
    {
        public bool? EmergencyLighting { get; set; }

        public CodeLightIntensity IntensityLevel { get; set; }

        public ColourType Colour { get; set; }

        [Browsable(false)]
        public override double? Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }

        [Browsable(false)]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }

        [Browsable(false)]
        public override string Lat
        {
            get
            {
                return base.Lat;
            }
            set
            {
                base.Lat = value;
            }
        }

        [Browsable(false)]
        public override string Lon
        {
            get
            {
                return base.Lon;
            }
            set
            {
                base.Lon = value;
            }
        }

        [Browsable(false)]
        public override string SourceDetail { get => base.SourceDetail; set => base.SourceDetail = value; }



        public LightSystem()
        {
        }

        public List<LightElement> Elements { get; set; }

        public override string GetObjectLabel()
        {
            return this.IntensityLevel.ToString()+" "+ Colour.ToString();
        }      

    }

    [Serializable()]
    public class LightElement : PDMObject
    {
        public string GroundLightSystem_ID { get; set; }
        public CodeLightIntensity IntensityLevel { get; set; }

        public string LightedElement { get; set; }     

        public ColourType Colour { get; set; }

        public double? Intensity { get; set; }

        public CodeLightSource LightSourceType { get; set; }

        [ReadOnly(true)]
        public override double? Elev
        {
            get
            {
                return base.Elev;
            }
            set
            {
                base.Elev = value;
            }
        }

        [ReadOnly(true)]
        public override UOM_DIST_VERT Elev_UOM
        {
            get
            {
                return base.Elev_UOM;
            }
            set
            {
                base.Elev_UOM = value;
            }
        }

        [Browsable(false)]
        public override string Lat
        {
            get
            {
                return base.Lat;
            }
            set
            {
                base.Lat = value;
            }
        }

        [Browsable(false)]
        public override string Lon
        {
            get
            {
                return base.Lon;
            }
            set
            {
                base.Lon = value;
            }
        }

        [Browsable(false)]
        public override string SourceDetail { get => base.SourceDetail; set => base.SourceDetail = value; }


        public LightElement()
        {
        }

        public override string GetObjectLabel()
        {
            return  this.Colour.ToString() + " " + Intensity.ToString();
        }


        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                if (!AIRTRACK_TableDic.ContainsKey(this.GetType())) return false;
                ITable tbl = AIRTRACK_TableDic[this.GetType()];

                if (EsriWorkEnvironment.EsriUtils.RowExist(tbl, this.ID)) return true;

                IRow row = tbl.CreateRow();


                CompileRow(ref row);

                row.Store();


            }
            catch (Exception ex)
            {
                this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace };
                res = false;
            }
            

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);//
            findx = row.Fields.FindField("GroundLightSystem_ID"); if (findx >= 0) row.set_Value(findx, this.GroundLightSystem_ID);//
            findx = row.Fields.FindField("IntensityLevel"); if (findx >= 0) row.set_Value(findx, this.IntensityLevel.ToString());
            findx = row.Fields.FindField("Colour"); if (findx >= 0) row.set_Value(findx, this.Colour.ToString());
            findx = row.Fields.FindField("Intensity"); if (findx >= 0 && this.Intensity.HasValue) row.set_Value(findx, this.Intensity);
            findx = row.Fields.FindField("Elevation"); if (findx >= 0 && this.Elev.HasValue) row.set_Value(findx, this.Elev);
            findx = row.Fields.FindField("ElevUom"); if (findx >= 0) row.set_Value(findx, this.Elev_UOM.ToString());
            findx = row.Fields.FindField("LightSourceType"); if (findx >= 0) row.set_Value(findx, this.LightSourceType.ToString());
            findx = row.Fields.FindField("LightedElement"); if (findx >= 0) row.set_Value(findx, this.LightedElement);
            

            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);

        }



    }

}
