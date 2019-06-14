﻿using System;
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
    public class ApronLightSystem:LightSystem
    {
        [XmlElement]
        [Browsable(false)]
        public CodeApronSectionType Position { get; set; }

        [XmlElement]
        [Browsable(false)]
        public string Apron_ID { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.ApronLightSystem;

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
            findx = row.Fields.FindField("Apron_ID"); if (findx >= 0) row.set_Value(findx, this.Apron_ID);
            findx = row.Fields.FindField("EmergencyLighting"); if (findx >= 0) row.set_Value(findx, this.EmergencyLighting);
            findx = row.Fields.FindField("IntensityLevel"); if (findx >= 0) row.set_Value(findx, this.IntensityLevel.ToString());
            findx = row.Fields.FindField("Colour"); if (findx >= 0) row.set_Value(findx, this.Colour.ToString());
            //findx = row.Fields.FindField("Color"); if (findx >= 0) row.set_Value(findx, this.Colour.ToString());
            //findx = row.Fields.FindField("Elevation"); if (findx >= 0 && this.Elev.HasValue) row.set_Value(findx, this.Elev);
            //findx = row.Fields.FindField("ElevUom"); if (findx >= 0) row.set_Value(findx, this.Elev_UOM.ToString());
            //findx = row.Fields.FindField("Position"); if (findx >= 0) row.set_Value(findx, this.Position.ToString());

        }
    }
}
