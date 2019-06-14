using System;
using System.Collections.Generic;

namespace PVT.Model
{
    public class Feature : BaseClass
    {
        public DateTime BeginDate { get; protected set; }
        public DateTime? EndDate { get; protected set; }

        public Feature(Aran.Aim.Features.Feature feature)
        {
            if (feature == null)
                return;
            Identifier = feature.Identifier;

            if (feature.TimeSlice?.ValidTime != null)
            {
                BeginDate = feature.TimeSlice.ValidTime.BeginPosition;
                EndDate = feature.TimeSlice.ValidTime.EndPosition;
            }
        }

        public List<FeatureReport> Reports { get; protected set; }
    }
}
