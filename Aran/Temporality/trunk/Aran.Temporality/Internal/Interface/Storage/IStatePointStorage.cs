#region

using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Util;

#endregion

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IStatePointStorage : IDisposable
    {
        #region Properties

        TemporalityLogicOptions Options { get; set; }
        string Path { get; set; }

        #endregion

        void Truncate();

        StatePoint CreateStatePointInternal(IFeatureId featureId, DateTime date);
        void DeleteStatePointInternal(StatePoint point);

        IList<StatePoint> GetStatePoints(IFeatureId featureId, DateTime fromDate, DateTime toDate);
        //sorted by DateTime incremented 
    }
}