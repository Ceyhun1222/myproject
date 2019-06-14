using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;

namespace ArenaLauncher.Converter
{
    public class PdmConverter
    {
        public static PDM.Airspace ConvertFromAim(Aran.Aim.Features.Airspace source, DateTime actualDate)
        {
            var result = new PDM.Airspace
                             {
                                 ActualDate = actualDate,
                                 TxtName = source.Name,
                                 CodeID = source.Designator,
                                 ValidStartDateTime = source.TimeSlice.ValidTime.BeginPosition
                             };

            if (source.TimeSlice.ValidTime.EndPosition.HasValue)
                result.ValidEndDateTime = source.TimeSlice.ValidTime.EndPosition.Value;
            foreach (var activation in source.Activation)
            {
                if (activation.TimeInterval != null && activation.TimeInterval.Count > 0)
                {
                    if (activation.TimeInterval[0].Day != null)
                        result.ActivationStartDay = (PDM.CodeDayBase)Enum.Parse(typeof(PDM.CodeDayBase), activation.TimeInterval[0].Day.ToString());
                    if (activation.TimeInterval[0].StartTime != null)
                        result.ActivationStartTime = activation.TimeInterval[0].StartTime.ToString();
                    if (activation.TimeInterval[0].DayTil != null)
                        result.ActivationEndDay = (PDM.CodeDayBase)Enum.Parse(typeof(PDM.CodeDayBase), activation.TimeInterval[0].DayTil.ToString());
                    if (activation.TimeInterval[0].EndTime != null)
                        result.ActivationEndTime = activation.TimeInterval[0].EndTime.ToString();
                }
                if (activation.Status != null)
                    result.ActivationStatus = (PDM.CodeStatusAirspaceType)Enum.Parse(typeof(PDM.CodeStatusAirspaceType), activation.Status.ToString());
            }

            foreach (var geomComp in source.GeometryComponent)
            {
                PDM.AirspaceVolume airspaceVol = new PDM.AirspaceVolume();
                if (geomComp.TheAirspaceVolume != null)
                {
                    if (geomComp.TheAirspaceVolume.UpperLimit != null)
                    {
                        airspaceVol.ValDistVerUpper = geomComp.TheAirspaceVolume.UpperLimit.Value;
                        airspaceVol.UomDistVerUpper = (PDM.UOM_DIST_VERT)Enum.Parse(typeof(PDM.UOM_DIST_VERT), geomComp.TheAirspaceVolume.UpperLimit.Uom.ToString());
                    }

                    if (geomComp.TheAirspaceVolume.LowerLimit != null)
                    {
                        airspaceVol.ValDistVerLower = geomComp.TheAirspaceVolume.LowerLimit.Value;
                        airspaceVol.UomDistVerLower = (PDM.UOM_DIST_VERT)Enum.Parse(typeof(PDM.UOM_DIST_VERT), geomComp.TheAirspaceVolume.LowerLimit.Uom.ToString());
                    }
                }
                if (result.AirspaceVolumeList == null)
                    result.AirspaceVolumeList = new List<PDM.AirspaceVolume>();
                result.AirspaceVolumeList.Add(airspaceVol);
            }
            return result;
        }

        public static AirspaceVolume ConvertFromAim(Aran.Aim.Features.AirspaceVolume aimVolume)
        {
            var result = new AirspaceVolume();
            return result;
        }
    }
}
