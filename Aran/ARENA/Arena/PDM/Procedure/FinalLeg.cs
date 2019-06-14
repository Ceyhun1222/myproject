using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
//using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ESRI.ArcGIS.Geodatabase;

namespace PDM
{
    [Serializable()]
    public class FinalLeg : ProcedureLeg
    {

         private string _ID_ProcedreLeg;
         [Browsable(false)]
         public string ID_ProcedreLeg
         {
             get { return _ID_ProcedreLeg; }
             set { _ID_ProcedreLeg = value; }
         }

        private CodeFinalGuidance _guidanceSystem;
        [PropertyOrder(10)]
        public CodeFinalGuidance GuidanceSystem
        {
            get { return _guidanceSystem; }
            set { _guidanceSystem = value; }
        }

        private CodeApproachGuidance _landingSystemCategory;
        [PropertyOrder(20)]
        public CodeApproachGuidance LandingSystemCategory
        {
            get { return _landingSystemCategory; }
            set { _landingSystemCategory = value; }
        }

        private double? _minimumBaroVnavTemperature = null;
        [PropertyOrder(30)]
        public double? MinimumBaroVnavTemperature
        {
            get { return _minimumBaroVnavTemperature; }
            set { _minimumBaroVnavTemperature = value; }
        }

        private Uom_Temperature _minimumBaroVnavTemperatureUOM;
        [PropertyOrder(40)]
        public Uom_Temperature MinimumBaroVnavTemperatureUOM
        {
            get { return _minimumBaroVnavTemperatureUOM; }
            set { _minimumBaroVnavTemperatureUOM = value; }
        }

        private bool _rnpDMEAuthorized;
        [PropertyOrder(50)]
        public bool RnpDMEAuthorized
        {
            get { return _rnpDMEAuthorized; }
            set { _rnpDMEAuthorized = value; }
        }

        private double? _courseOffsetAngle = null;
        [PropertyOrder(60)]
        public double? CourseOffsetAngle
        {
            get { return _courseOffsetAngle; }
            set { _courseOffsetAngle = value; }
        }

        private CodeSide _courseOffsetSide;
        [PropertyOrder(70)]
        public CodeSide CourseOffsetSide
        {
            get { return _courseOffsetSide; }
            set { _courseOffsetSide = value; }
        }

        private double? _courseCentrelineDistance = null;
        [PropertyOrder(80)]
        public double? CourseCentrelineDistance
        {
            get { return _courseCentrelineDistance; }
            set { _courseCentrelineDistance = value; }
        }

        private UOM_DIST_HORZ _courseCentrelineDistanceUOM;
        [PropertyOrder(90)]
        public UOM_DIST_HORZ CourseCentrelineDistanceUOM
        {
            get { return _courseCentrelineDistanceUOM; }
            set { _courseCentrelineDistanceUOM = value; }
        }

        private double? _courseOffsetDistance = null;
        [PropertyOrder(100)]
        public double? CourseOffsetDistance
        {
            get { return _courseOffsetDistance; }
            set { _courseOffsetDistance = value; }
        }

        private UOM_DIST_HORZ _courseOffsetDistanceUOM;
        [PropertyOrder(110)]
        public UOM_DIST_HORZ CourseOffsetDistanceUOM
        {
            get { return _courseOffsetDistanceUOM; }
            set { _courseOffsetDistanceUOM = value; }
        }

        private CodeRelativePosition _courseCentrelineIntersect;
        [PropertyOrder(120)]
        public CodeRelativePosition CourseCentrelineIntersect
        {
            get { return _courseCentrelineIntersect; }
            set { _courseCentrelineIntersect = value; }
        }

        private List<ApproachCondition> _Condition_Minima;
        [PropertyOrder(130)]
        public List<ApproachCondition> Condition_Minima
        {
            get { return _Condition_Minima; }
            set { _Condition_Minima = value; }
        }

        [Browsable(false)]
        public override PDM_ENUM PDM_Type
        {
            get
            {
                return PDM_ENUM.FinalLeg;
            }
        } 

        public FinalLeg()
        {
            
        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("ID_Transition"); if (findx >= 0) row.set_Value(findx, this.TransitionIdentifier);
            findx = row.Fields.FindField("ProcedreLegID"); if (findx >= 0) row.set_Value(findx, this.ID_ProcedreLeg);
            findx = row.Fields.FindField("guidanceSystem"); if (findx >= 0) row.set_Value(findx, this.GuidanceSystem.ToString());
            findx = row.Fields.FindField("landingSystemCategory"); if (findx >= 0) row.set_Value(findx, this.LandingSystemCategory.ToString());
            findx = row.Fields.FindField("minimumBaroVnavTemperature"); if (findx >= 0) row.set_Value(findx, this.MinimumBaroVnavTemperature);
            findx = row.Fields.FindField("minimumBaroVnavTemperatureUOM"); if (findx >= 0) row.set_Value(findx, this.MinimumBaroVnavTemperatureUOM.ToString());
            findx = row.Fields.FindField("rnpDMEAuthorized"); if (findx >= 0) row.set_Value(findx, this.RnpDMEAuthorized);
            findx = row.Fields.FindField("courseOffsetAngle"); if (findx >= 0) row.set_Value(findx, this.CourseOffsetAngle);
            findx = row.Fields.FindField("courseOffsetSide"); if (findx >= 0) row.set_Value(findx, this.CourseOffsetSide.ToString());
            findx = row.Fields.FindField("courseCentrelineDistance"); if (findx >= 0) row.set_Value(findx, this.CourseCentrelineDistance);
            findx = row.Fields.FindField("courseCentrelineDistanceUOM"); if (findx >= 0) row.set_Value(findx, this.CourseCentrelineDistanceUOM.ToString());
            findx = row.Fields.FindField("courseOffsetDistance"); if (findx >= 0) row.set_Value(findx, this.CourseOffsetDistance);
            findx = row.Fields.FindField("courseOffsetDistanceUOM"); if (findx >= 0) row.set_Value(findx, this.CourseOffsetDistanceUOM.ToString());
            findx = row.Fields.FindField("courseCentrelineIntersect"); if (findx >= 0) row.set_Value(findx, this.CourseCentrelineIntersect.ToString());



        }

      
  

    }
}
