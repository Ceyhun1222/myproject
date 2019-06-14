using Aran.Aim;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Implementation;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Implementation.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.Consistency
{
    public class EventConsistencyManager
    {
        private readonly int _insertGroupSize = 1000;

        private AbstractConsistencyRepository<AbstractEvent<AimFeature>> ConsistencyRepository { get; }
        private ITossReadableRepository TossReadableRepository { get; }

        public Action<string> Logger { get; set; }

        public EventConsistencyManager(RepositoryType repositoryType, string storageName)
        {
            ConsistencyRepository = new EventConsistencyRepository(repositoryType, storageName);

            if (repositoryType == RepositoryType.MongoRepository || repositoryType == RepositoryType.MongoWithBackupRepository)
                TossReadableRepository = new TossReadableWritebleMongoRepository(storageName);
            else
                TossReadableRepository = new TossReadableNoDeleteRepository(storageName);

            StorageService.Init();
            StorageService.PreStart();
        }

        public List<PrivateSlot> GetPrivateSlots()
        {
            var publicSlots = StorageService.GetPublicSlots();
            return publicSlots.SelectMany(x => StorageService.GetPrivateSlots(x.Id, 0)).ToList();
        }

        public void ReCalculateConsistencies()
        {
            Logger?.Invoke($"Existing consistencies cleaning started");
            ConsistencyRepository.ClearConsistencies();
            Logger?.Invoke($"Cleaning is finished");

            var wpNumber = 0;
            var privateSlots = GetPrivateSlots();
            var total = privateSlots.Count;
            var hashAlgorithm = MD5.Create();

            Logger?.Invoke($"Consistencies calculation started");
            foreach (var slot in privateSlots)
            {
                wpNumber++;
                Logger?.Invoke($"Workpackage {wpNumber}/{total}");

                var events = new List<AbstractEvent<AimFeature>>();
                var eventNumber = 0;

                foreach (var value in TossReadableRepository.GetEventStorages(slot.Id))
                {
                    events.Add(value);
                    eventNumber++;

                    if (eventNumber % _insertGroupSize == 0)
                    {
                        ConsistencyRepository.Add(events, hashAlgorithm);

                        events = new List<AbstractEvent<AimFeature>>();
                        eventNumber = 0;
                    }
                }

                if (events.Count > 0)
                    ConsistencyRepository.Add(events);
            }
        }

        public List<EventConsistencyReportModel> CheckConsistencies()
        {
            var privateSlots = GetPrivateSlots();
            var total = privateSlots.Count;

            var result = new List<EventConsistencyReportModel>();
            var hashAlgorithm = MD5.Create();

            Logger?.Invoke($"Consistency check started");

            int wpNumber = 0;
            foreach (var slot in privateSlots)
            {
                wpNumber++;
                Logger?.Invoke($"Workpackage {wpNumber}/{total}");

                var hashes = new Dictionary<Tuple<int, FeatureType, Guid?, Interpretation, int, int>, string>();
                var duplicates = new HashSet<Tuple<int, FeatureType, Guid?, Interpretation, int, int>>();

                foreach (var value in TossReadableRepository.GetEventStorages(slot.Id))
                {
                    var key = Tuple.Create(value.WorkPackage, (FeatureType)value.FeatureTypeId, value.Guid,
                        value.Interpretation, value.Version.SequenceNumber, value.Version.CorrectionNumber);

                    if (hashes.ContainsKey(key))
                        duplicates.Add(key);
                    else
                        hashes[key] = ConsistencyRepository.CalculateConsistency(value, hashAlgorithm);
                }

                var dbConsistencies = ConsistencyRepository.Get(slot.Id);

                foreach (var consistency in dbConsistencies)
                {
                    var key = Tuple.Create(consistency.WorkPackage, consistency.FeatureType, consistency.Identifier,
                        consistency.Interpretation, consistency.SequenceNumber, consistency.CorrectionNumber);

                    EventConsistencyErrorType? errorType = null;

                    if (duplicates.Contains(key))
                        errorType = EventConsistencyErrorType.Duplicate;
                    else if (!hashes.ContainsKey(key))
                        errorType = EventConsistencyErrorType.HashNotFound;
                    else if (hashes[key] == null)
                        errorType = EventConsistencyErrorType.HashIsNull;
                    else if (hashes[key] != consistency.Hash)
                        errorType = EventConsistencyErrorType.DataChanged;

                    if (errorType.HasValue)
                        result.Add(new EventConsistencyReportModel { ErrorType = errorType.Value, EventConsistency = consistency });

                    hashes.Remove(key);
                }

                foreach (var hash in hashes)
                {
                    var consistency = new EventConsistency
                    {
                        WorkPackage = hash.Key.Item1,
                        FeatureType = hash.Key.Item2,
                        Identifier = hash.Key.Item3,
                        Interpretation = hash.Key.Item4,
                        SequenceNumber = hash.Key.Item5,
                        CorrectionNumber = hash.Key.Item6
                    };

                    result.Add(new EventConsistencyReportModel { ErrorType = EventConsistencyErrorType.DataDeleted, EventConsistency = consistency });
                }
            }

            return result.ToList();
        }
    }
}
