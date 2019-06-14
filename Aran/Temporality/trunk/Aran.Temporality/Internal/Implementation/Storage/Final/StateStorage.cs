#region

using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Abstract.MetaData;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Abstract.Storage.Cached;
using Aran.Temporality.Internal.Implementation.Repository.Block;
using Aran.Temporality.Internal.MetaData;
using Aran.Temporality.Internal.MetaData.Offset;
using Aran.Temporality.Internal.MetaData.Util;
using Aran.Temporality.Internal.Struct;

#endregion

namespace Aran.Temporality.Internal.Implementation.Storage.Final
{
    internal abstract class StateStorage<TDataType> : CachedStateStorage<TDataType>
        where TDataType : class //T is feature type
    {
        #region States repository

        private BlockRepository<TDataType> _states;

        private BlockRepository<TDataType> States
        {
            get { return _states ?? (_states = new BlockRepository<TDataType>(StorageName, "states\\states")); }
        }

        #endregion

        #region Overrides of AbstractStateStorage<TDataType>

        public override void Truncate()
        {
            MetaRepository.RemoveAll();
            MetaRepository.Close();
            MetaCache.Clear();
            FileUtil.DeleteDirectory(StorageName + "\\" + "states");
            MetaRepository.Open();

            DeletedWorkPackagesCache.Clear();
            DeletedWorkPackagesRepository.RemoveAll();
        }

        public override CommonOperationResult SaveStateInternal(AbstractState<TDataType> state)
        {
            var meta = new OffsetStateMetaData<BlockOffsetStructure>(state)
            {
                Offset = States.Add(state.Data)
            };

            MetaCache.GetByFeatureId(meta).Add(meta);
            StoreMeta(meta);

            return new CommonOperationResult();
        }


        public override void ClearStatesAfter(AbstractMetaData featureId, DateTime dateTime)
        {
            //add CSA(ClearStatesAfter) to cache
            StateMetaSet<OffsetStateMetaData<BlockOffsetStructure>, BlockOffsetStructure> meta =
                MetaCache.GetByFeatureId(featureId);
            var csa = new OffsetStateMetaData<BlockOffsetStructure>(featureId) { ClearStatesAfter = dateTime };
            if (meta.Add(csa))
            {
                //if it was actual CSA
                StoreMeta(csa);
            }

            //remove states from state cache
            StatesCache.ClearStatesAfter(featureId, dateTime);
        }

        public override AbstractState<TDataType> GetLastKnownState(IFeatureId featureId, DateTime dateTime,
                                                                   bool equalDateIsOk)
        {
            AbstractState<TDataType> result1 = GetLastKnownStateInternal(featureId, dateTime, equalDateIsOk);
            AbstractState<TDataType> result2 = StatesCache.GetLastKnownState(featureId, dateTime, equalDateIsOk);

            if (result1 != null)
            {
                if (result2 != null && result2.TimeSlice.BeginPosition > result1.TimeSlice.BeginPosition)
                {
                    return CloneUtil.DeepClone(result2);
                }
                return result1;
            }

            if (result2 != null)
            {
                return CloneUtil.DeepClone(result2);
            }

            return null;
        }

        public override IList<AbstractState<TDataType>> GetLastKnownStatesByFeatureName(IFeatureId featureId,
                                                                                        DateTime dateTime,
                                                                                        bool equalDateIsOk)
        {
            IList<AbstractState<TDataType>> result1 = GetLastKnownStatesByFeatureNameInternal(featureId, dateTime,
                                                                                              equalDateIsOk);
            //result 1 is full result

            IList<AbstractState<TDataType>> result2 = StatesCache.GetLastKnownStatesByFeatureName(featureId, dateTime,
                                                                                                  equalDateIsOk);
            //result2 is additional

            if (result2 != null)
            {
                foreach (var state in result2)
                {
                    //find similar state in result1
                    //it should be no more than one per Guid because it is LastKnown
                    AbstractState<TDataType> similar = result1.FirstOrDefault(t => t.Guid == state.Guid);


                    if (similar == null)
                    {
                        result1.Add(state);
                    }
                    else
                    {
                        if (similar.TimeSlice.BeginPosition < state.TimeSlice.BeginPosition)
                        {
                            result1.Remove(similar);
                            result1.Add(CloneUtil.DeepClone(state));
                        }
                    }
                }
            }


            return result1;
        }

        private AbstractState<TDataType> GetLastKnownStateInternal(IFeatureId featureId, DateTime dateTime,
                                                                   bool equalDateIsOk)
        {
            StateMetaSet<OffsetStateMetaData<BlockOffsetStructure>, BlockOffsetStructure> container =
                MetaCache.GetByFeatureId(featureId);
            OffsetStateMetaData<BlockOffsetStructure> metaData = container.GetLastItemBefore(dateTime, equalDateIsOk);

            if (metaData == null) return null;

            return GetStateFromData(States.Get(metaData.Offset, metaData.FeatureTypeId), metaData);
        }

        public IList<AbstractState<TDataType>> GetLastKnownStatesByFeatureNameInternal(IFeatureId featureId,
                                                                                       DateTime dateTime,
                                                                                       bool equalDateIsOk)
        {
            List<OffsetStateMetaData<BlockOffsetStructure>> metaList =
                MetaCache.GetByFeatureTypeNameBefore(featureId, dateTime, equalDateIsOk).ToList();

            metaList.RemoveAll(t => t == null);

            return metaList.Select(metaData => GetStateFromData(States.Get(metaData.Offset, metaData.FeatureTypeId), metaData)).ToList();
        }

        public override void Dispose()
        {
        }

        private bool CanCache()
        {
            return true;
        }

        public override void SaveStateInternalLater(AbstractState<TDataType> clone)
        {
            if (CanCache())
            {
                StatesCache.SaveStateInternal(clone);
            }
        }

        #endregion

        protected override void AddHole(OffsetStateMetaData<BlockOffsetStructure> meta, DataSegment hole)
        {
            States.AddHole(hole);
        }
    }
}