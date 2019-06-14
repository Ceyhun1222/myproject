using Aran.Aim;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Extensions;
using Aran.Temporality.Internal.Implementation.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Temporality.Common.Consistency
{
    public class EventConsistencyRepository : AbstractConsistencyRepository<AbstractEvent<AimFeature>>
    {
        #region Constructors

        public EventConsistencyRepository(RepositoryType repositoryType, string storageName) : base(repositoryType, storageName)
        {
        }

        #endregion

        #region Implementation of AbstractConsistencRepository

        public override void Add(AbstractEvent<AimFeature> value, HashAlgorithm hashAlgorithm = null)
        {
            var hash = CalculateConsistency(value, hashAlgorithm);

            if (value == null)
                throw new ArgumentNullException("value", "value is null");

            var eventConsistency = new EventConsistency
            {
                RepositoryType = RepositoryType,
                StorageName = StorageName,
                WorkPackage = value.WorkPackage,
                FeatureType = (FeatureType)value.FeatureTypeId,
                Identifier = value.Guid,
                Interpretation = value.Interpretation,
                SequenceNumber = value.Version.SequenceNumber,
                CorrectionNumber = value.Version.CorrectionNumber,
                ValidTimeBegin = value.LifeTimeBegin.Value,
                ValidTimeEnd = value.LifeTimeEnd,
                SubmitDate = value.SubmitDate,
                Hash = hash
            };

            StorageService.SaveEventConsistency(eventConsistency);
        }

        public override bool Add(List<AbstractEvent<AimFeature>> values, HashAlgorithm hashAlgorithm = null)
        {
            var eventConsistencies = new List<EventConsistency>();
            foreach (var value in values)
            {
                var hash = CalculateConsistency(value, hashAlgorithm);

                if (value == null)
                    throw new ArgumentNullException("value", "value is null");

                var eventConsistency = new EventConsistency
                {
                    RepositoryType = RepositoryType,
                    StorageName = StorageName,
                    WorkPackage = value.WorkPackage,
                    FeatureType = (FeatureType)value.FeatureTypeId,
                    Identifier = value.Guid,
                    Interpretation = value.Interpretation,
                    SequenceNumber = value.Version.SequenceNumber,
                    CorrectionNumber = value.Version.CorrectionNumber,
                    ValidTimeBegin = value.LifeTimeBegin,
                    ValidTimeEnd = value.LifeTimeEnd,
                    SubmitDate = value.SubmitDate,
                    Hash = hash
                };

                eventConsistencies.Add(eventConsistency);
            }

            return StorageService.SaveEventConsistencies(eventConsistencies);
        }

        public override List<EventConsistency> Get(int workPackage)
        {
            return StorageService.GetConsistencies(RepositoryType, StorageName, workPackage);
        }

        #endregion
    }
}
