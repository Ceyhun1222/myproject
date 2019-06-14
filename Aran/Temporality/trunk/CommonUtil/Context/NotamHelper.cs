using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.MetaData;

namespace Aran.Temporality.CommonUtil.Context
{
    public static class NotamHelper
    {
        public static bool SaveNewFeature(Feature feature) //returns true if all is ok
        {
            if (feature == null) return false;
            
            var myEvent = new AimEvent
                                {
                                    Interpretation = feature.TimeSlice.Interpretation==TimeSliceInterpretationType.TEMPDELTA?Interpretation.TempDelta:Interpretation.PermanentDelta,
                                    TimeSlice = new TimeSlice(feature.TimeSlice.ValidTime.BeginPosition, feature.TimeSlice.ValidTime.EndPosition),
                                    LifeTimeBegin = feature.TimeSlice.FeatureLifetime.BeginPosition,
                                    LifeTimeEnd = feature.TimeSlice.FeatureLifetime.EndPosition,
                                    Data = feature,
                                };


            var counter = 10;
            while (counter-- > 0)
            {
                try
                {
                    var result2 = CurrentDataContext.CurrentService.CommitNewEvent(myEvent);
                    if (!result2.IsOk)
                    {
                        //log.Append(result2.ErrorMessage + "\n");
                        continue;
                    }

                    return true;
                }
                catch (Exception exception)
                {
                    //log.Append(exception.Message+"\n");
                }
            }

            return false;
        }
    }
}
