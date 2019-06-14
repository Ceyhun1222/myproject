using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    [Serializable()]
    public class MissaedApproachLeg : ProcedureLeg
    {

        private string _ID_ProcedreLeg;
        [Browsable(false)]
        public string ID_ProcedreLeg
        {
            get { return _ID_ProcedreLeg; }
            set { _ID_ProcedreLeg = value; }
        }


        private CodeMissedApproach _type;

        public CodeMissedApproach Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _thresholdAfterMAPT;

        public bool ThresholdAfterMAPT
        {
            get { return _thresholdAfterMAPT; }
            set { _thresholdAfterMAPT = value; }
        }

        private double? _heightMAPT = null;

        public double? HeightMAPT
        {
            get { return _heightMAPT; }
            set { _heightMAPT = value; }
        }

        private UOM_DIST_HORZ _heightMAPTUOM;

        public UOM_DIST_HORZ HeightMAPTUOM
        {
            get { return _heightMAPTUOM; }
            set { _heightMAPTUOM = value; }
        }

        //private double? _requiredNavigationPerformance = null;

        //public double? RequiredNavigationPerformance
        //{
        //    get { return _requiredNavigationPerformance; }
        //    set { _requiredNavigationPerformance = value; }
        //}


        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.MissaedApproachLeg;
            }
        } 

        public MissaedApproachLeg()
        {
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_Transition"); if (findx >= 0) row.set_Value(findx, this.TransitionIdentifier);
            findx = row.Fields.FindField("ProcedreLegID"); if (findx >= 0) row.set_Value(findx, this.ID_ProcedreLeg);
            findx = row.Fields.FindField("type"); if (findx >= 0) row.set_Value(findx, this.Type.ToString());
            findx = row.Fields.FindField("thresholdAfterMAPT"); if (findx >= 0) row.set_Value(findx, this.ThresholdAfterMAPT);
            findx = row.Fields.FindField("heightMAPT"); if (findx >= 0) row.set_Value(findx, this.HeightMAPT);
            findx = row.Fields.FindField("heightMAPTUOM"); if (findx >= 0) row.set_Value(findx, this.HeightMAPTUOM.ToString());
            findx = row.Fields.FindField("requiredNavigationPerformance"); if (findx >= 0) row.set_Value(findx, this.RequiredNavigationPerformance);
           
        }


    }
}
