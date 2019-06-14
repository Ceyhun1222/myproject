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
using System.Xml.Serialization;

namespace PDM
{
    public class SurfaceCharacteristics:PDMObject
    {
        public string ID_Parent { get; set; }

        public string ParentName { get; set; }

        public CodeSurfaceCompositionType Composition { get; set; }

        public CodeSurfacePreparationType Preparation { get; set; }

        public CodeSurfaceConditionType SurfaceCondition { get; set; }

        public double? ClassPCN { get; set; }        

        public CodePCNPavementType PavementTypePCN { get; set; }

        public CodePCNSubgradeType PavementSubgradePCN { get; set; }

        public CodePCNTyrePressureType MaxTyrePressurePCN { get; set; }

        public CodePCNMethodType EvaluationMethodPCN { get; set; }

        public double? ClassLCN { get; set; }       

        public double? WeightSIWL { get; set; }

        public string WeightSIWL_UOM { get; set; }

        public double? TyrePressureSIWL { get; set; }

        public string TyrePressureSIWL_UOM { get; set; }

        public double? WeightAUW { get; set; }

        public string WeightAUW_UOM { get; set; }

        public override PDM_ENUM PDM_Type => PDM_ENUM.SurfaceCharacteristics;


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
            findx = row.Fields.FindField("ID_Parent"); if (findx >= 0) row.set_Value(findx, this.ID_Parent);
            findx = row.Fields.FindField("ParentName"); if (findx >= 0) row.set_Value(findx, this.ParentName);           
            findx = row.Fields.FindField("Composition"); if (findx >= 0) row.set_Value(findx, this.Composition.ToString());
            findx = row.Fields.FindField("Preparation"); if (findx >= 0) row.set_Value(findx, this.Preparation.ToString());
            findx = row.Fields.FindField("SurfaceCondition"); if (findx >= 0) row.set_Value(findx, this.SurfaceCondition.ToString());
            findx = row.Fields.FindField("ClassLCN"); if (findx >= 0 && this.ClassLCN!=null) row.set_Value(findx, this.ClassLCN);
            findx = row.Fields.FindField("ClassPCN"); if (findx >= 0 && this.ClassPCN != null) row.set_Value(findx, this.ClassPCN);
            findx = row.Fields.FindField("PavementSubgradePCN"); if (findx >= 0) row.set_Value(findx, this.PavementSubgradePCN.ToString());
            findx = row.Fields.FindField("PavementTypePCN"); if (findx >= 0) row.set_Value(findx, this.PavementTypePCN.ToString());
            findx = row.Fields.FindField("MaxTyrePressurePCN"); if (findx >= 0) row.set_Value(findx, this.MaxTyrePressurePCN.ToString());
            findx = row.Fields.FindField("EvaluationMethodPCN"); if (findx >= 0) row.set_Value(findx, this.EvaluationMethodPCN.ToString());
            findx = row.Fields.FindField("WeightAUW"); if (findx >= 0 && this.WeightAUW.HasValue) row.set_Value(findx, this.WeightAUW);
            findx = row.Fields.FindField("WeightAUW_UOM"); if (findx >= 0) row.set_Value(findx, this.WeightAUW_UOM);
            findx = row.Fields.FindField("WeightSIWL"); if (findx >= 0 && this.WeightSIWL.HasValue) row.set_Value(findx, this.WeightSIWL);
            findx = row.Fields.FindField("WeightSIWL_UOM"); if (findx >= 0) row.set_Value(findx, this.WeightSIWL_UOM);
            findx = row.Fields.FindField("TyrePressureSIWL"); if (findx >= 0 && this.TyrePressureSIWL.HasValue) row.set_Value(findx, this.TyrePressureSIWL);
            findx = row.Fields.FindField("TyrePressureSIWL_UOM"); if (findx >= 0) row.set_Value(findx, this.TyrePressureSIWL_UOM);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            
        }
    }
}
