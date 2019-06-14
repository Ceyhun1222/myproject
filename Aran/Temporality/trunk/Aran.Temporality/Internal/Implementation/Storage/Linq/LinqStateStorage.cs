#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Internal.Abstract.Repository;
using Aran.Temporality.Internal.Abstract.Storage;

#endregion

namespace Aran.Temporality.Internal.Implementation.Storage.Linq
{
    internal class LinqStateStorage<TDataType, TRepositoryType, TKey> : AbstractStateStorage<TDataType>
        where TDataType : class
        where TRepositoryType : AbstractLinqDataRepository<AbstractState<TDataType>, TKey>

    {
        #region Internal storage properties

        private const String Marker = "states";

        private AbstractLinqDataRepository<AbstractState<TDataType>, TKey> _states;

        public AbstractLinqDataRepository<AbstractState<TDataType>, TKey> States
        {
            get
            {
                return _states ??
                       (_states = (AbstractLinqDataRepository<AbstractState<TDataType>, TKey>)
                                  Activator.CreateInstance(typeof(TRepositoryType), StorageName, Marker));
            }
            set { _states = value; }
        }

        #endregion

        #region Overrides of StateStorage

        public override void Truncate()
        {
            States.RemoveAll();
        }

        public override bool Optimize()
        {
            return false;
        }

        public override CommonOperationResult SaveStateInternal(AbstractState<TDataType> state)
        {
            var result = new CommonOperationResult();

            States.Add(state);

            return result;
        }

        public override void ClearStatesAfter(AbstractMetaData featureId, DateTime dateTime)
        {
            IEnumerable<AbstractState<TDataType>> toBeDeleted = States.Where(t =>
                                                                             t.Guid == featureId.Guid && //same id
                                                                             (featureId.FeatureTypeId == 0
                                                                              || t.FeatureTypeId == 0
                                                                              ||
                                                                              t.FeatureTypeId == featureId.FeatureTypeId) &&
                                                                             //same feature type
                                                                             t.WorkPackage == featureId.WorkPackage &&
                                                                             //same workpackage
                                                                             t.TimeSlice.BeginPosition > dateTime);
            //it is after

            foreach (var state in toBeDeleted)
            {
                States.Remove(state);
            }
        }

        public override AbstractState<TDataType> GetLastKnownState(IFeatureId featureId, DateTime dateTime,
                                                                   bool equalDateIsOk)
        {
            List<AbstractState<TDataType>> list = States.Where(t =>
                                                               t.Guid == featureId.Guid && //same id
                                                               (featureId.FeatureTypeId == 0
                                                                || t.FeatureTypeId == 0
                                                                || t.FeatureTypeId == featureId.FeatureTypeId) &&
                                                               //same feature type
                                                               t.WorkPackage == featureId.WorkPackage &&
                                                               //same workpackage
                                                               (t.TimeSlice.BeginPosition < dateTime ||
                                                                (equalDateIsOk && t.TimeSlice.BeginPosition == dateTime))
                //it is before
                ).ToList();

            //get the last one
            AbstractState<TDataType> lastState = list.FirstOrDefault();
            if (lastState != null)
            {
                foreach (var dummyState in list)
                {
                    if (dummyState.TimeSlice.BeginPosition > lastState.TimeSlice.BeginPosition)
                        lastState = dummyState;
                }
            }

            return lastState;
        }

        public override IList<AbstractState<TDataType>> GetLastKnownStatesByFeatureName(IFeatureId featureId,
                                                                                        DateTime dateTime,
                                                                                        bool equalDateIsOk)
        {
            List<AbstractState<TDataType>> allStates = States.Where(t =>
                                                                    (featureId.FeatureTypeId == 0
                                                                     || t.FeatureTypeId == 0
                                                                     || t.FeatureTypeId == featureId.FeatureTypeId) &&
                                                                    //same feature type
                                                                    t.WorkPackage == featureId.WorkPackage
                //same workpackage
                ).ToList();

            IEnumerable<IGrouping<Guid?, AbstractState<TDataType>>> groups = allStates.GroupBy(t => t.Guid);
            return groups.Select(g => g.OrderByDescending(t => t.TimeSlice.BeginPosition).First()).ToList();
        }

        #endregion

        #region Implementation of IDisposable

        public override void Dispose()
        {
            States.Dispose();
        }

        #endregion

        public bool Contains(AbstractState<TDataType> featureId)
        {
            return States.Where(t =>
                                t.Guid == featureId.Guid && //same id
                                t.Version == featureId.Version && //same Version
                                (featureId.FeatureTypeId == 0
                                 || t.FeatureTypeId == 0
                                 || t.FeatureTypeId == featureId.FeatureTypeId) && //same feature type
                                t.WorkPackage == featureId.WorkPackage). //same workpackage
                       FirstOrDefault() != null;
        }

        public IEnumerable<AbstractState<TDataType>> GetAll()
        {
            return States.Where(t => true);
        }

        public AbstractState<TDataType> Poke()
        {
            return States.Poke();
        }

        #region Not implemented

        public override AbstractState<TDataType> GetStateFromData(TDataType data, AbstractStateMetaData meta)
        {
            throw new NotImplementedException();
        }

        public override AbstractState<TDataType> GetByDefaultData(AbstractStateMetaData meta)
        {
            throw new NotImplementedException();
        }

        public override void DeleteWorkPackage(int workpackage)
        {
            throw new NotImplementedException();
        }

        protected override void Load()
        {
        }

        public override void SaveStateInternalLater(AbstractState<TDataType> clone)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}