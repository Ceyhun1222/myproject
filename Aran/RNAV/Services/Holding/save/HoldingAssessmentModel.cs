using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delib.Classes.Features.Holding;
using ARAN.Common;
using Delib.Classes.Types;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Holding.HoldingSave
{
    public class HoldingAssessmentModel
    {
        public HoldingAssessmentModel(ModelAreaParams modelAreaParam, double time, double wd, DistanceType distanceType)
        {
            HAssessMent = new HoldingAssessment();
            double length = 0;

            if (distanceType == DistanceType.Time)
            {
                double tas = ARANMath.IASToTAS(Common.DeConvertSpeed(modelAreaParam.Ias), Common.DeConvertSpeed(modelAreaParam.Altitude), 15);
                length = time * tas * 60;
            }
            else if (distanceType == DistanceType.Wd)
            {
                double RV = Shablons.TurnRadius(Common.DeConvertSpeed(modelAreaParam.Ias), Common.DeConvertSpeed(modelAreaParam.Altitude), 15);
                length = Math.Sqrt(Common.DeConvertDistance(wd) * Common.DeConvertDistance(wd) - 4 * RV * RV);
            }
            ValDistance legLength = new ValDistance();
            if (InitHolding.DistanceUnit == ARAN.Contracts.Settings.HorisontalDistanceUnit.hduKM)
            {
                legLength.Uom = UomDistance.KM;
                legLength.Value = (length * 0.001);
            }
            else if (InitHolding.DistanceUnit == ARAN.Contracts.Settings.HorisontalDistanceUnit.hduMeter)
            {
                legLength.Uom = UomDistance.M;
                legLength.Value = length;
            }
            else if (InitHolding.DistanceUnit == ARAN.Contracts.Settings.HorisontalDistanceUnit.hduNM)
            {
                legLength.Uom = UomDistance.NM;
                legLength.Value = (length * 1.0 / 1852.0);

            }
            HAssessMent.TurbulentAir = modelAreaParam.TurboCondition;
            HAssessMent.LegLengthAway = legLength;
            HAssessMent.LegLengthToward = legLength;
        }
        public HoldingAssessment HAssessMent { get; private set; }
    }
}
