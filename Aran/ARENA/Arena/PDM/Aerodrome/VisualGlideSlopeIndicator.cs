using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{
    public class VisualGlideSlopeIndicator:LightSystem
    {
        public string ID_RunwayDirection { get; set; }

        public CodeVASISType Type { get; set; }

        public CodeSide Position { get; set; }

        public uint? NumberBox { get; set; }

        public bool Portable { get; set; }

        public double? SlopeAngle { get; set; }

        public double? MinimumEyeHeightOverThreshold { get; set; }

        public UOM_DIST_VERT MinimumEyeHeightOverThreshold_UOM { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.VisualGlideSlopeIndicator;

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

                if (this.Elements != null)
                {

                    foreach (LightElement lgtEl in this.Elements)
                    {
                        lgtEl.GroundLightSystem_ID = this.ID;
                        lgtEl.StoreToDB(AIRTRACK_TableDic);

                    }
                }

            }
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }

            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_RunwayDirection"); if (findx >= 0) row.set_Value(findx, this.ID_RunwayDirection);
            findx = row.Fields.FindField("EmergencyLighting"); if (findx >= 0) row.set_Value(findx, this.EmergencyLighting);
            findx = row.Fields.FindField("IntensityLevel"); if (findx >= 0) row.set_Value(findx, this.IntensityLevel.ToString());
            findx = row.Fields.FindField("Colour"); if (findx >= 0) row.set_Value(findx, this.Colour.ToString());
            findx = row.Fields.FindField("Type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("Portable"); if (findx >= 0) row.set_Value(findx, this.Portable.ToString());
            findx = row.Fields.FindField("SlopePosition"); if (findx >= 0) row.set_Value(findx, this.Position.ToString());
            findx = row.Fields.FindField("SlopeAngle"); if (findx >= 0 && this.SlopeAngle.HasValue) row.set_Value(findx, this.SlopeAngle);
            findx = row.Fields.FindField("MinimumEyeHeightOverThreshold"); if (findx >= 0 && this.MinimumEyeHeightOverThreshold.HasValue) row.set_Value(findx, this.MinimumEyeHeightOverThreshold);
            findx = row.Fields.FindField("MinimumEyeHeightOverThreshold_UOM"); if (findx >= 0) row.set_Value(findx, this.MinimumEyeHeightOverThreshold_UOM.ToString());
            findx = row.Fields.FindField("NumberBox"); if (findx >= 0 && this.NumberBox.HasValue) row.set_Value(findx, this.NumberBox);

        }
    }
}
