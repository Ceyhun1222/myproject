﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using PDM.PropertyExtension;
using System.ComponentModel;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Xml.Serialization;

namespace PDM
{
    public class RunwayVisualRange:PDMObject
    {
        public CodeRVRReadingType ReadingPosition { get; set; }

        public string ID_RunwayDirection { get; set; }

        public bool? ManyLinks { get; set; }

        public double? AngleDir { get; set; }

        public CodeRVRDirectionType Direction { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.RunwayVisualRange;

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
            catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }
            
            return res;
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            
            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ReadingPosition"); if (findx >= 0) row.set_Value(findx, this.ReadingPosition.ToString());
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            findx = row.Fields.FindField("ID_RunwayDirection"); if (findx >= 0) row.set_Value(findx, this.ID_RunwayDirection);
            findx = row.Fields.FindField("ManyLinks"); if (findx >= 0) row.set_Value(findx, this.ManyLinks);
            findx = row.Fields.FindField("AngleDir"); if (findx >= 0 && this.AngleDir!=null) row.set_Value(findx, this.AngleDir);
            findx = row.Fields.FindField("Direction"); if (findx >= 0) row.set_Value(findx, this.Direction.ToString());

            findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);
        }
    }
}