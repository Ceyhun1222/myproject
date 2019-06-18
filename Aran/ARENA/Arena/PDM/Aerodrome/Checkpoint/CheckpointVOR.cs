﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel;
//using System.Diagnostics.Debug;
using PDM.PropertyExtension;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using System.Xml.Serialization;

namespace PDM
{
    public class CheckpointVOR: NavigationSystemCheckpoint
    {
        public string ID_VOR { get; set; }

        public string DesignatorVOR { get; set; }

        [ReadOnly(true)]
        public double? Frequency { get; set; }

        [ReadOnly(true)]
        public UOM_FREQ Frequency_UOM { get; set; }


        [Browsable(false)]
        public override PDM_ENUM PDM_Type => PDM_ENUM.CheckpointVOR;

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

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);            
            findx = row.Fields.FindField("Category"); if (findx >= 0) row.set_Value(findx, this.Category.ToString());
            findx = row.Fields.FindField("UpperLimitReference"); if (findx >= 0) row.set_Value(findx, this.UpperLimitReference.ToString());
            findx = row.Fields.FindField("LowerLimitReference"); if (findx >= 0) row.set_Value(findx, this.LowerLimitReference.ToString());
            findx = row.Fields.FindField("AltitudeInterpretation"); if (findx >= 0) row.set_Value(findx, this.AltitudeInterpretation.ToString());
            findx = row.Fields.FindField("UpperLimit"); if (findx >= 0 && this.UpperLimit.HasValue) row.set_Value(findx, this.UpperLimit);
            findx = row.Fields.FindField("UpperLimit_UOM"); if (findx >= 0) row.set_Value(findx, this.UpperLimit_UOM.ToString());
            findx = row.Fields.FindField("LowerLimit"); if (findx >= 0 && this.LowerLimit.HasValue) row.set_Value(findx, this.LowerLimit);
            findx = row.Fields.FindField("LowerLimit_UOM"); if (findx >= 0) row.set_Value(findx, this.LowerLimit_UOM.ToString());
            findx = row.Fields.FindField("Distance"); if (findx >= 0 && this.LowerLimit.HasValue) row.set_Value(findx, this.Distance);
            findx = row.Fields.FindField("Distance_UOM"); if (findx >= 0) row.set_Value(findx, this.Distance_UOM.ToString());
            findx = row.Fields.FindField("Angle"); if (findx >= 0 && this.LowerLimit.HasValue) row.set_Value(findx, this.Angle);
            findx = row.Fields.FindField("ID_VOR"); if (findx >= 0) row.set_Value(findx, this.ID_VOR);
            findx = row.Fields.FindField("DesignatorVOR"); if (findx >= 0) row.set_Value(findx, this.DesignatorVOR);
            findx = row.Fields.FindField("ID_AirportHeliport"); if (findx >= 0) row.set_Value(findx, this.ID_AirportHeliport);
            findx = row.Fields.FindField("Frequency"); if (findx >= 0 && this.Frequency.HasValue) row.set_Value(findx, this.Frequency);
            findx = row.Fields.FindField("Frequency_UOM"); if (findx >= 0) row.set_Value(findx, this.Frequency_UOM.ToString());          

            if (this.Geo != null)
            {
                
                findx = row.Fields.FindField("Shape"); row.set_Value(findx, this.Geo);

            }

        }
    }
}