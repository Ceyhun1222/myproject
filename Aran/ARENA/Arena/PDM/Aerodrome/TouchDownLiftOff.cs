using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace PDM
{
    public class TouchDownLiftOff:PDMObject
    {
        public string Designator { get; set; }

        public double? Length { get; set; }

        public UOM_DIST_HORZ UOM_Length { get; set; }

        public double? Width { get; set; }

        public UOM_DIST_HORZ UOM_Width { get; set; }

        public double? Slope { get; set; }

        public string HelicopterClass { get; set; }

        public bool? Abandoned { get; set; }

        public SurfaceCharacteristics SurfaceProperties { get; set; }

        public string ID_AirportHeliport { get; set; }

        public string ID_Runway { get; set; }

        [XmlIgnore]
        public IGeometry AimingPoint { get; set; }

        public List<TouchDownLiftOffMarking> TouchDownLiftOffMarkingList { get; set; }

        //[Browsable(false)]
        public TouchDownLiftOffLightSystem LightSystem { get; set; }

        //[Browsable(false)]
        public TouchDownLiftOffSafeArea TouchDownSafeArea { get; set; }

        public List<GuidanceLine> GuidanceLineList { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.TouchDownLiftOff;


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

                if (this.TouchDownLiftOffMarkingList != null)
                {

                    foreach (TouchDownLiftOffMarking rel in this.TouchDownLiftOffMarkingList)
                    {
                        rel.ID_TouchDownLiftOff = this.ID;
                        rel.StoreToDB(AIRTRACK_TableDic);

                    }
                }

                if (this.LightSystem != null)
                {
                    this.LightSystem.ID_TouchDownLiftOff = this.ID;
                    this.LightSystem.StoreToDB(AIRTRACK_TableDic);
                }

                if (this.TouchDownSafeArea != null)
                {
                    this.TouchDownSafeArea.ID_TouchDownLiftOff = this.ID;
                    this.TouchDownSafeArea.StoreToDB(AIRTRACK_TableDic);
                }

                SurfaceProperties?.StoreToDB(AIRTRACK_TableDic);


                if (this.AimingPoint != null)
                {
                    int findx = -1;
                    ITable tblGeoPnt = AIRTRACK_TableDic[typeof(TouchDownLiftOffAimingPoint)];
                    IRow rowGeoExtent = tblGeoPnt.CreateRow();
                    findx = -1;
                    findx = rowGeoExtent.Fields.FindField("FeatureGUID");
                    if (findx >= 0) rowGeoExtent.set_Value(findx, this.ID);

                    findx = rowGeoExtent.Fields.FindField("Shape");
                    rowGeoExtent.set_Value(findx, this.AimingPoint);

                    rowGeoExtent.Store();
                }

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

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Designator"); if (findx >= 0) row.set_Value(findx, this.Designator);
            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("ID_Runway"); if (findx >= 0) row.set_Value(findx, this.ID_Runway);
            findx = row.Fields.FindField("Length"); if (findx >= 0 && this.Length.HasValue) row.set_Value(findx, this.Length);
            findx = row.Fields.FindField("UOM_Length"); if (findx >= 0) row.set_Value(findx, this.UOM_Length.ToString());
            findx = row.Fields.FindField("Width"); if (findx >= 0 && this.Width.HasValue) row.set_Value(findx, this.Width);
            findx = row.Fields.FindField("UOM_Width"); if (findx >= 0) row.set_Value(findx, this.UOM_Width.ToString());
            findx = row.Fields.FindField("Slope"); if (findx >= 0 && this.Slope.HasValue) row.set_Value(findx, this.Slope);
            findx = row.Fields.FindField("HelicopterClass"); if (findx >= 0) row.set_Value(findx, this.HelicopterClass);
            findx = row.Fields.FindField("Abandoned"); if (findx >= 0) row.set_Value(findx, this.Abandoned);

            if (this.Geo != null)
            {               
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);
            }

        }

    }

    public class TouchDownLiftOffAimingPoint
    {
        
    }
}
