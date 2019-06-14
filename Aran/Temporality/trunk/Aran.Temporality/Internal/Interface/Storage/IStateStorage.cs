#region

using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.OperationResult;

#endregion

namespace Aran.Temporality.Internal.Interface.Storage
{
    internal interface IStateStorage<T> : IDisposable where T : class //T is feature type
    {
        #region Properties

        TemporalityLogicOptions Options { get; set; }
        string StorageName { get; set; }

        #endregion

        void Truncate();
        bool Optimize();

        CommonOperationResult SaveStateInternal(AbstractState<T> state);
        void ClearStatesAfter(AbstractMetaData featureId, DateTime dateTime);
        //delete states with (DateTime >= specified)
        AbstractState<T> GetLastKnownState(IFeatureId featureId, DateTime dateTime, bool equalDateIsOk);
        //state with (DateTime <= or < specified)
        IList<AbstractState<T>> GetLastKnownStatesByFeatureName(IFeatureId featureId, DateTime dateTime,
                                                                bool equalDateIsOk);

        //by feature name

        AbstractState<T> GetStateFromData(T data, AbstractStateMetaData meta);
        AbstractState<T> GetByDefaultData(AbstractStateMetaData meta);

        void DeleteWorkPackage(int workpackage);
    }
}