#region

using System;
using System.Collections.Generic;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Internal.Interface.Storage;

#endregion

namespace Aran.Temporality.Internal.Abstract.Storage
{
    internal abstract class AbstractStateStorage<T> : IStateStorage<T> where T : class //T is feature type
    {
        public AbstractTemporalityService<T> TemporalityService { get; set; }

        #region Helpers

        public void ClearStatesAfter(AbstractEvent<T> myEvent)
        {
            ClearStatesAfter(myEvent, myEvent.TimeSlice.BeginPosition);
        }

        #endregion

        #region Abstract implementation of IStateStorage<T>

        public abstract void Truncate();
        public abstract bool Optimize();
        public abstract CommonOperationResult SaveStateInternal(AbstractState<T> state);
        public abstract void ClearStatesAfter(AbstractMetaData featureId, DateTime dateTime);
        public abstract AbstractState<T> GetLastKnownState(IFeatureId featureId, DateTime dateTime, bool equalDateIsOk);
        public abstract IList<AbstractState<T>> GetLastKnownStatesByFeatureName(IFeatureId featureId, DateTime dateTime,
                                                                                bool equalDateIsOk);
        public abstract AbstractState<T> GetStateFromData(T data, AbstractStateMetaData meta);
        public abstract AbstractState<T> GetByDefaultData(AbstractStateMetaData meta);
        public abstract void DeleteWorkPackage(int workpackage);

        #endregion

        #region Virtual methods

        protected abstract void Load();

        public abstract void SaveStateInternalLater(AbstractState<T> clone);

        #endregion

        #region Implementation of IStateStorage<T>

        private string _storageName;
        public TemporalityLogicOptions Options { get; set; }

        public string StorageName
        {
            get { return _storageName; }
            set
            {
                _storageName = value;
                Load();
            }
        }

        #endregion

        #region Implementation of IDisposable

        public abstract void Dispose();

        #endregion

        
    }
}