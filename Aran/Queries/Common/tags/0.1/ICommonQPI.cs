using System;
using System.Collections.Generic;
using Aran.Aim.CAWProvider;
using Aran.Aim.Features;
using Aran.Aim;

namespace Aran.Queries
{
    public interface ICommonQPI
    {
        ConnectionInfo ConnectionInfo { get; set; }
        void Close ();
        void Open (TemporalTimeslice timeSlice, InterpretationType interpretation);

        TemporalTimeslice TimeSlice { get; }
        InterpretationType Interpretation { get; }

        void Commit (FeatureType [] featureTypes);
        void Commit ();

        TFeature CreateFeature<TFeature> () where TFeature : Feature, new ();
        void ExcludeFeature (Guid identifier);
        void SetFeature (Feature feature);

        List<TFeature> GetFeatureList<TFeature> (Filter filter = null) where TFeature : Feature, new ();
        TFeature GetFeature<TFeature> (Guid identifier) where TFeature : Feature;
		Feature GetFeature(FeatureType featureType, Guid identifier);
    }
}
