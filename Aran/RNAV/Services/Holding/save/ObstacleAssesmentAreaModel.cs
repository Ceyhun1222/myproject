using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delib.Classes.Features.Obstacle;
using Delib.Classes.Codes;
using Delib.Classes.Objects.SurfaceAssestment;
using Delib.Classes.Objects.Aircraft;

namespace Holding.HoldingSave
{
    public class ObstacleAssesmentAreaModel
    {
        private HoldingGeometry _holdingGeom;
        private double _assessedAltitude;

        public ObstacleAssesmentAreaModel(HoldingReport hReport, ModelAreaParams modelAreaParam, HoldingGeometry holdingGeom, double assessedAltitude)
        {
            _holdingGeom = holdingGeom;

            ObstacleAssessment = new ObstacleAssessmentArea();

            AssesedAltitude = assessedAltitude;

            AircraftCategories = modelAreaParam.Category;
            VerticalStructureList = hReport.VerticalStructureList;

            AssesmentType = ObstacleAssessmentSurfaceType.PRIMARY;
            CreateObstacleAssesmentArea();
        }


        #region :>Property

        public ObstacleAssessmentSurfaceType? AssesmentType 
        {
            get { return ObstacleAssessment.type; }
            set
            {
                ObstacleAssessment.type = value;
            }
        }
       
        public double AssesedAltitude
        {
            get { return Common.ConvertHeight(_assessedAltitude, roundType.toUp); }
            set
            {
                _assessedAltitude = Common.DeConvertHeight(value);
            }
        }
        
        public categories AircraftCategories { get; private set; }
        public List<VerticalStructure> VerticalStructureList { get; set; }
        public ObstacleAssessmentArea ObstacleAssessment { get; private set; }

        #endregion

        public void CreateObstacleAssesmentArea()
        {
            

            ObstacleAssessment.type = AssesmentType;
            
            Delib.Classes.Types.ValDistanceVerticalType assessedAltitude = new Delib.Classes.Types.ValDistanceVerticalType();
            assessedAltitude.value = AssesedAltitude.ToString();
            if (InitHolding.HeightUnit == ARAN.Contracts.Settings.VerticalDistanceUnit.vduMeter)
                assessedAltitude.uom = Delib.Classes.UOM.DistanceVerticalType.M;
            else 
                if (InitHolding.HeightUnit == ARAN.Contracts.Settings.VerticalDistanceUnit.vduFL)
                assessedAltitude.uom = Delib.Classes.UOM.DistanceVerticalType.FL;
                else
                    if (InitHolding.HeightUnit == ARAN.Contracts.Settings.VerticalDistanceUnit.vduFeet)
                        assessedAltitude.uom = Delib.Classes.UOM.DistanceVerticalType.FT;
                    else
                        if (InitHolding.HeightUnit == ARAN.Contracts.Settings.VerticalDistanceUnit.vduSM)
                            assessedAltitude.uom = Delib.Classes.UOM.DistanceVerticalType.SM;

            ObstacleAssessment.assessedAltitude = assessedAltitude;

            AircraftCharacteristic aircraftCharecteristic = new AircraftCharacteristic();
            if (AircraftCategories == categories.A)
                aircraftCharecteristic.aircraftLandingCategory = AircraftCategoryType.A;
            else
                if (AircraftCategories == categories.C)
                    aircraftCharecteristic.aircraftLandingCategory = AircraftCategoryType.C;
                else
                    if (AircraftCategories == categories.D)
                        aircraftCharecteristic.aircraftLandingCategory = AircraftCategoryType.D;
                    else
                        if (AircraftCategories == categories.E)
                            aircraftCharecteristic.aircraftLandingCategory = AircraftCategoryType.E;
            ObstacleAssessment.aircraftCategory.Add(aircraftCharecteristic);

            ObstacleAssessment.surface = GeomFunctions.ConvertPolygonToDelib(_holdingGeom.FullArea);

            foreach (VerticalStructure vs in VerticalStructureList)
            {
                ObstacleAssessment.significantObstacle.Add(new Obstruction { obstruction = vs });
            }

        }
        
    }
}
