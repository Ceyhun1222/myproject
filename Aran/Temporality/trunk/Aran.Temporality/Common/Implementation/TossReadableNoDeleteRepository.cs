using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.MetaData.Offset;
using Aran.Temporality.Internal.MetaStorage;

namespace Aran.Temporality.Common.Implementation
{
    internal class TossReadableNoDeleteRepository : ITossReadableRepository
    {
        private string RepositoryName { get; }
        private readonly Dictionary<string, NoDeleteOffsetRepository<AimFeature>> _eventRepositories;

        public TossReadableNoDeleteRepository(string repositoryName)
        {
            RepositoryName = repositoryName;
            _eventRepositories = new Dictionary<string, NoDeleteOffsetRepository<AimFeature>>();
        }

        private NoDeleteOffsetRepository<AimFeature> GetEventRepository(string marker, bool rewrite)
        {
            lock (this)
            {
                if (!_eventRepositories.ContainsKey(marker))
                    _eventRepositories[marker] = new NoDeleteOffsetRepository<AimFeature>(RepositoryName, marker, rewrite);
                return _eventRepositories[marker];
            }
        }

        public List<int> GetWorkPackages()
        {
            var directoryInfo = new DirectoryInfo(ConfigUtil.StoragePath + "\\" + RepositoryName + "\\events\\");
            var workPackages = new List<int>();
            foreach (var eventFile in directoryInfo.GetFiles("events_mt_*.fdb"))
            {
                string stringWorkpackage = string.Join("", eventFile.Name.Where(char.IsDigit));
                if (int.TryParse(stringWorkpackage, out var intWorkpackage) && directoryInfo.GetFiles($"events_{intWorkpackage}.fdb").Count() > 0)
                    workPackages.Add(intWorkpackage);
            }

            return workPackages;
        }

        public IEnumerable<AbstractEvent<AimFeature>> GetEventStorages(int workPackage)
        {
            var aimEventStorage = new AimEventStorage<long>();
            var eventRepository = GetEventRepository($"events\\events_{workPackage}", false);
            foreach (OffsetEventMetaData<long> metaData in eventRepository.GetMetaDatas())
            {
                var eventData = eventRepository.Get(metaData.Offset, metaData.FeatureTypeId);
                yield return aimEventStorage.GetEventFromData(eventData, metaData);
            }
        }
    }
}
