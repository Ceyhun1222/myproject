#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.Implementation.Repository.Meta;
using Aran.Temporality.Internal.Implementation.Storage.Linq;
using Aran.Temporality.Internal.MetaData;
using Aran.Temporality.Internal.MetaData.Offset;
using Aran.Temporality.Internal.MetaData.Util;
using Aran.Temporality.Internal.Struct;

#endregion

namespace Aran.Temporality.Internal.Abstract.Storage.Cached
{
    internal abstract class CachedStateStorage<TDataType> : AbstractStateStorage<TDataType> where TDataType : class
        //T is feature type
    {
        #region Cached states

        private LinqStateStorage<TDataType, MemoryRepository<AbstractState<TDataType>>, long> _statesCache;

        protected LinqStateStorage<TDataType, MemoryRepository<AbstractState<TDataType>>, long> StatesCache
        {
            get
            {
                return _statesCache ??
                       (_statesCache =
                        new LinqStateStorage<TDataType, MemoryRepository<AbstractState<TDataType>>, long>
                        { StorageName = StorageName + "states\\cache" });
            }
            set { _statesCache = value; }
        }

        #endregion

        #region Metadata

        protected readonly StateMetaCache<OffsetStateMetaData<BlockOffsetStructure>, BlockOffsetStructure> MetaCache =
            new StateMetaCache<OffsetStateMetaData<BlockOffsetStructure>, BlockOffsetStructure>();

        private StructRepository<StateMetaStructure> _metaRepository;

        protected StructRepository<StateMetaStructure> MetaRepository
        {
            get
            {
                return _metaRepository ??
                       (_metaRepository = new StructRepository<StateMetaStructure>(StorageName, "states\\states_mt"));
            }
        }

        protected void StoreMeta(OffsetStateMetaData<BlockOffsetStructure> meta)
        {
            MetaRepository.Add(new StateMetaStructure(meta));
        }


        protected abstract void AddHole(OffsetStateMetaData<BlockOffsetStructure> meta, DataSegment hole);

        public virtual void DeleteMeta(OffsetStateMetaData<BlockOffsetStructure> meta)
        {
            //dispose all its segments
            foreach (var segment in meta.Offset.GetSegments(false))
            {
                AddHole(meta, segment);
            }
            //remove meta itself
            MetaRepository.RemoveByIndex(meta.Index);
        }


        private void LoadMeta()
        {
            MetaCache.Clear();

            var s = new Stopwatch();

            s.Start();
            int index = 0;
            List<StateMetaStructure?> metaList = MetaRepository.LoadAll();
            foreach (var data in metaList)
            {
                if (data != null)
                {
                    var metaStructure = (StateMetaStructure)data;
                    OffsetStateMetaData<BlockOffsetStructure> offsetMetaData = metaStructure.OffsetStateMetaData();
                    offsetMetaData.Index = index;
                    MetaCache.GetByFeatureId(offsetMetaData).Add(offsetMetaData);
                }
                index++;
            }
            s.Stop();
            //Console.WriteLine(index + " state meta loaded in " + s.ElapsedMilliseconds);
        }

        protected override void Load()
        {
            LoadMeta();
            LoadWorkPackages();
        }

        #endregion

        #region Optimization methods

        private bool FlushSingleCacheItem()
        {
            AbstractState<TDataType> state = StatesCache.Poke();
            if (state != null)
            {
                SaveStateInternal(state);
                return true;
            }
            return false;
        }

        private bool FlushSingleMetaItem()
        {
            var meta = MetaCache.PokeInvalid();

            if (meta != null)
            {
                DeleteMeta(meta);
                return true;
            }

            return false;
        }

        private bool FlushDeletedWorkPackage()
        {
            var wp = GetLastDeletedWorkPackage();

            if (wp > -1)
            {
                var meta = MetaCache.PokeAny(wp);
                if (meta != null)
                {
                    DeleteMeta(meta);//get any state from deleted workPackage and delete that state
                    return true;
                }

                //if every state in wp is deleted do delete last workPackage
                DeletedWorkPackagesCache.RemoveAt(DeletedWorkPackagesCache.Count - 1);//delete from cache
                _deletedWorkPackagesRepository.Poke();//delete from storage
                return true;
            }

            return false;
        }

        //return true in order to continue
        public override bool Optimize()
        {

            if (FlushSingleCacheItem()) return true;

            if (FlushSingleMetaItem()) return true;

            if (FlushDeletedWorkPackage()) return true;




            return false;
        }

        #endregion

        #region WorkPackage methods

        private OffsetRepository _deletedWorkPackagesRepository;
        public OffsetRepository DeletedWorkPackagesRepository
        {
            get
            {
                return _deletedWorkPackagesRepository ??
                       (_deletedWorkPackagesRepository = new OffsetRepository(StorageName, "states\\states_dwp"));
            }
        }

        private void LoadWorkPackages()
        {
            DeletedWorkPackagesCache = DeletedWorkPackagesRepository.LoadAll();
            foreach (var workPackage in DeletedWorkPackagesCache)
            {
                MetaCache.PrepareForDeleteWorkPackage(workPackage);
            }
        }

        public List<int> DeletedWorkPackagesCache = new List<int>();

        public int GetLastDeletedWorkPackage()
        {
            if (DeletedWorkPackagesCache.Count == 0) return -1;
            return DeletedWorkPackagesCache[DeletedWorkPackagesCache.Count - 1];
        }

        public override void DeleteWorkPackage(int workpackage)
        {
            if (DeletedWorkPackagesCache.Contains(workpackage))
            {
                throw new Exception("can not delete workpackage that was already deleted");
            }

            DeletedWorkPackagesCache.Add(workpackage); //memory cache store
            _deletedWorkPackagesRepository.Add(workpackage); //permanent store
            MetaCache.PrepareForDeleteWorkPackage(workpackage); //remove CSA from state cache
        }

        #endregion
    }
}