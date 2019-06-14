using System;
using System.Collections.Generic;
using Aran.Aim.DB;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim;
using System.Collections;
using Aran.Aim.DataTypes;

namespace Aran.Queries
{
    public interface ICommonQPI
    {
        void Close ();
        void Open (IDbProvider dbProvider);
        void Open(string connectionString);

        TimeSliceFilter TimeSlice { get; }
        TimeSliceInterpretationType Interpretation { get; }

        void Commit (FeatureType [] featureTypes);
        void Commit ();

        void ExcludeFeature (Guid identifier);
        void SetFeature (Feature feature);
        TFeature CreateFeature<TFeature>() where TFeature : Feature, new();

        IList GetFeatureList (FeatureType featureType,Filter filter = null);
		Feature GetFeature(FeatureType featureType, Guid identifier);
        Feature GetAbstractFeature(IAbstractFeatureRef abstractFeatureRef);
    }
}
