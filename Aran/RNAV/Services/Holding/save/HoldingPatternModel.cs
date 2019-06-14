using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Delib.Classes.Features.Holding;
using Delib.Classes.Codes;
using Delib.Classes.Types;
using ARAN.Common;
using ARAN.Contracts.Settings;
using Delib.Classes.Objects.Holding;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Holding.HoldingSave
{
    public class HoldingPatternModel
    {
        #region :>Fields
        private ModelAreaParams _modelAreaParam;
        private HoldingGeometry _holdingGeom;
        private ModelProcedureType _modelProcedureType;
        private ModelPBN _modelPBN;
        private string _designator;
        private double _lowerLimit,_minLowerLimit;
        #endregion

        #region :>Ctor
        
        public HoldingPatternModel(ModelPBN modelPBN,ModelAreaParams modelAreaParam,ModelProcedureType modelProcedureType,
            HoldingGeometry holdingGeom,double assessedAltitude)
        {
            _modelAreaParam = modelAreaParam;
            _holdingGeom = holdingGeom;
            _modelProcedureType = modelProcedureType;
            _modelPBN = modelPBN;
           
            HPattern = new HoldingPattern();
            if (modelPBN.CurFlightPhase.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
                 HPattern.Type = CodeHoldingUsage.ENR;
            else
                HPattern.Type = CodeHoldingUsage.TER;
           
            InboundCourse = modelAreaParam.Radial;
            OutboundCourse = ARANMath.Modulus(InboundCourse-180,360);
            if (modelAreaParam.Turn == SideDirection.sideRight)
            {
                TurnDirection = "Right";
                HPattern.NonStandardHolding =true;
            }
            else
            {
                TurnDirection = "Left";
                HPattern.NonStandardHolding = false;
            }
            SpeedLimit = modelAreaParam.Ias;
            UpperLimit = modelAreaParam.Altitude;
            _minLowerLimit = Common.DeConvertHeight(assessedAltitude);
            LowerLimit = assessedAltitude;
            
            OutboundCourseType = CodeCourse.TRUE_BRG;

            Instruction = "RNAV," + _modelProcedureType.PropType + "," + _modelPBN.CurReciever.RecieverName + "," +
               _modelPBN.CurPBN.PBNName;

            CreateHoldingPattern();

        }
        #endregion


        #region :>Property

        public HoldingPattern HPattern { get; set; }

     
        #region :>Methods

        private void CreateHoldingPattern()
        {
            HPattern.Type = Type;

            HPattern.inboundCourse = (decimal)InboundCourse;
            HPattern.outboundCourse = (decimal)OutboundCourse;

            if (_modelAreaParam.Turn == SideDirection.sideRight)
            {
                HPattern.turnDirection = DirectionTurnType.RIGHT;
            }
            else if (_modelAreaParam.Turn== SideDirection.sideLeft)
            {
                HPattern.turnDirection = DirectionTurnType.LEFT;

            }
            
            HPattern.lowerLimit = new ValDistanceVerticalType();
            if (InitHolding.HeightUnit == ARAN.Contracts.Settings.VerticalDistanceUnit.vduMeter)
                HPattern.lowerLimit.uom = Delib.Classes.UOM.DistanceVerticalType.M;
            else if (InitHolding.HeightUnit == ARAN.Contracts.Settings.VerticalDistanceUnit.vduFeet)
                HPattern.lowerLimit.uom = Delib.Classes.UOM.DistanceVerticalType.FT;

            HPattern.lowerLimit.value = LowerLimit.ToString();
            HPattern.lowerLimitReference = VerticalReferenceType.MSL;

            HPattern.upperLimit = new ValDistanceVerticalType();
            HPattern.upperLimit.uom = HPattern.lowerLimit.uom;
            HPattern.upperLimit.value = UpperLimit.ToString();
            HPattern.upperLimitReference = VerticalReferenceType.MSL;

            HPattern.speedLimit = new ValSpeedType();
            if (InitHolding.SpeedUnit == HorisontalSpeedUnit.hsuKMInHour)
                HPattern.speedLimit.uom = Delib.Classes.UOM.SpeedType.KM_H;
            else if (InitHolding.SpeedUnit == HorisontalSpeedUnit.hsuKnot)
                HPattern.speedLimit.uom = Delib.Classes.UOM.SpeedType.KT;
            HPattern.speedLimit.value = (decimal)SpeedLimit;

            HPattern.outboundCourseType = OutboundCourseType;

            HoldingPatternLength hpLength = null;
            if (_modelProcedureType.CurDistanceType == DistanceType.Time)
            {
                ValDurationType duration =new ValDurationType();
                duration.uom = Delib.Classes.UOM.DurationType.MIN;
                duration.value = (decimal)_modelProcedureType.Time;
                hpLength = new HoldingPatternLength(duration);
            }
            else
                if (_modelProcedureType.CurDistanceType == DistanceType.Wd)
                {
                    ValDistanceType distanceType = new ValDistanceType();
                    if (InitHolding.DistanceUnit == HorisontalDistanceUnit.hduKM)
                        distanceType.uom = Delib.Classes.UOM.DistanceType.KM;
                    else
                        if (InitHolding.DistanceUnit == HorisontalDistanceUnit.hduNM)
                            distanceType.uom = Delib.Classes.UOM.DistanceType.NM;
                        else
                            if (InitHolding.DistanceUnit == HorisontalDistanceUnit.hduNM)
                                distanceType.uom = Delib.Classes.UOM.DistanceType.NM;
                    distanceType.value = (decimal)_modelProcedureType.WD;
                    hpLength = new HoldingPatternLength(distanceType);
                }

            HPattern.outbondLegSpan = hpLength;
            HPattern.extent = GeomFunctions.ConvertPolylineToDelib(_holdingGeom.HoldingTrack);

            HPattern.instruction = Instruction;
           

        }
        #endregion

    }
}
