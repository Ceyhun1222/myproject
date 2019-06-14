using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.Common.Util;
using Moq;
using TossMetaData = Aran.Temporality.Common.MetaData;
using Npgsql;
using Ploeh.AutoFixture;
using Toss.Tests.AutoFixture;

namespace Toss.Tests
{
    public class DataFixture : IDisposable
    {
        private bool _tossMode = false;
        private readonly string _noAixmDbName, _aixmDbName, _noAixmDefaultDbName, _adminName;
        private readonly object _service;
        private int _slotCount;
        private readonly List<PublicSlot> _publicSlots;
        private readonly List<PrivateSlot> _privateSlots;
        private readonly int _adminUserId;
        public ITemporalityService<AimFeature> TemporalityService => _service as ITemporalityService<AimFeature>;
        public INoAixmDataService NoAixmDataService => _service as INoAixmDataService;


        public DataFixture()
        {
            TossMode();
            _noAixmDefaultDbName = @"postgres";
            _adminName = "Admin";
            _publicSlots = new List<PublicSlot>();
            _privateSlots = new List<PrivateSlot>();
            _slotCount = 0;

            ConfigUtil.RepositoryType = RepositoryType.MongoRepository;
            ConfigUtil.NoDataServiceAddress = @"localhost";
            ConfigUtil.NoDataServicePort = @"5432";
            ConfigUtil.NoDataUser = @"aran";
            ConfigUtil.NoDataPassword = @"airnav2012";

            if (_tossMode)
            {
                _noAixmDbName = $"{ConfigUtil.NoDataDatabase}";
                _aixmDbName = @"test";
            }
            else
            {
                _aixmDbName = $"test_(toss)";
                _noAixmDbName = $"test_{ConfigUtil.NoDataDatabase}";
                ConfigUtil.StoragePath = Path.Combine(Directory.GetCurrentDirectory(), "Test (Toss)");

                if (Directory.Exists(ConfigUtil.StoragePath))
                    Directory.Delete(ConfigUtil.StoragePath, true);

                CreateNoAixmDb();
                AimServiceFactory.Setup();
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            _service = AimServiceFactory.OpenLocal(_aixmDbName);

            _adminUserId = NoAixmDataService.CreateUser(_adminName);
            NoAixmDataService.SetUserRole(_adminUserId, 68);
        }

        protected Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("NHibernate"))
                return typeof(NHibernate.NHibernateUtil).Assembly;

            if (args.Name.Contains("Iesi"))
                return typeof(Iesi.Collections.DictionarySet).Assembly;

            return typeof(AimFeature).Assembly;
        }

        private void CreateNoAixmDb()
        {
            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(
                $"Server={ConfigUtil.NoDataServiceAddress};" +
                $"Port={ConfigUtil.NoDataServicePort};" +
                $"User Id={ConfigUtil.NoDataUser};" +
                $"Password={ConfigUtil.NoDataPassword};" +
                $"Database={_noAixmDefaultDbName}"))
            {
                npgsqlConnection.Open();
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(
                    $@"DROP DATABASE IF EXISTS {_noAixmDbName};", npgsqlConnection);
                npgsqlCommand.ExecuteNonQuery();
                npgsqlCommand.CommandText = $@"CREATE DATABASE {_noAixmDbName}
                    WITH OWNER = aran 
                    ENCODING = 'UTF8' 
                    CONNECTION LIMIT = -1;";
                npgsqlCommand.ExecuteNonQuery();
                npgsqlConnection.Close();
            }
            ConfigUtil.NoDataDatabase = _noAixmDbName;
        }


        [Conditional("TestByTossm")]
        public void TossMode()
        {
            _tossMode = true;
        }

        private void DeleteNoAixmDb()
        {
            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(
                $"Server={ConfigUtil.NoDataServiceAddress};" +
                $"Port={ConfigUtil.NoDataServicePort};" +
                $"User Id={ConfigUtil.NoDataUser};" +
                $"Password={ConfigUtil.NoDataPassword};" +
                $"Database={_noAixmDefaultDbName}"))
            {
                npgsqlConnection.Open();
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(
                    $"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{_noAixmDbName}' AND pid <> pg_backend_pid();",
                    //$"DISCONNECT ALL;"+
                    //$"DROP DATABASE IF EXISTS {_noAixmDbName}", 
                    npgsqlConnection);
                npgsqlCommand.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            //DeleteSlots();
            (NoAixmDataService as IDisposable)?.Dispose();
            if (!_tossMode)
            {
                DeleteNoAixmDb();
                Directory.Delete(ConfigUtil.StoragePath, true);
            }
        }

        /// <summary>
        /// Creates new public slot
        /// </summary>
        /// <param name="forNewSequence">Set true if you want to create slot for the next airac cycle. Else creates for the latest slot date time that created before.Mostly it is for correction slot</param>
        /// <param name="getPreviousDate">Set true if you want to create slot for the previous airac cycle that created latest.For this situation forNewSequence should be false</param>
        /// <param name="effectiveDate">effectiveDate = default(DateTime)</param>
        /// <returns></returns>
        public int CreateSlot(bool forNewSequence, bool getPreviousDate = false, DateTime effectiveDate = default(DateTime))
        {
            CreatePublicSlot(forNewSequence, getPreviousDate, effectiveDate);

            var id = CreatePrivateSlot(_publicSlots[_publicSlots.Count - 1]);
            NoAixmDataService.SetUserActiveSlotId(_adminUserId, id);
            return id;
        }

        public int AddPrivateSlot()
        {
            var id = CreatePrivateSlot(_publicSlots[_publicSlots.Count - 1]);
            NoAixmDataService.SetUserActiveSlotId(_adminUserId, id);
            return id;
        }

        private int CreatePrivateSlot(PublicSlot publicSlot)
        {
            PrivateSlot privateSlot = new PrivateSlot()
            {
                PublicSlot = publicSlot,
                Name = $"Private_{_slotCount}"
            };
            return CreatePrivateSlot(privateSlot);
        }

        public int CreatePrivateSlot(PrivateSlot privateSlot)
        {
            _privateSlots.Add(privateSlot);
            return NoAixmDataService.CreatePrivateSlot(privateSlot);
        }


        public int CreatePublicSlot(bool forNewSequence, bool getPreviousDate = false, DateTime effectiveDate = default(DateTime))
        {
            if (effectiveDate == default(DateTime))
            {
                if (forNewSequence)
                    effectiveDate = NextAiracCycle;
                else
                {
                    if (!getPreviousDate)
                        effectiveDate = _publicSlots[_publicSlots.Count - 1].EffectiveDate;
                    else
                    {
                        if (_publicSlots.Count < 2)
                            throw new Exception("There are no enought slots to enumerate");
                        var latestAirac = _publicSlots[_publicSlots.Count - 1].EffectiveDate;
                        for (int i = _publicSlots.Count - 2; i >= 0; i--)
                        {
                            if (_publicSlots[i].EffectiveDate < latestAirac)
                            {
                                effectiveDate = _publicSlots[i].EffectiveDate;
                                break;
                            }
                        }
                    }
                }
            }

            PublicSlot publicSlot = new PublicSlot()
            {
                Name = $"Public_{_slotCount + 1}",
                SlotType = 0,
                EffectiveDate = effectiveDate
            };
            return CreatePublicSlot(publicSlot);
        }

        public DateTime NextAiracCycle => AiracCycle.GetAiracCycleByIndex(AiracCycle.GetNextAiracCycle() + _slotCount + 1);
        public DateTime LastEffectiveDate => _publicSlots[_publicSlots.Count - 1].EffectiveDate;

        public int CreatePublicSlot(PublicSlot publicSlot)
        {
            _slotCount++;
            _publicSlots.Add(publicSlot);
            return NoAixmDataService.CreatePublicSlot(publicSlot);
        }

        /// <summary>
        /// Commits feature
        /// </summary>
        /// <param name="feature">The feature to commit</param>
        /// <param name="asCorrection">If you want to increase Correction number set to true, else this increases Sequence number(if it is not initial version)</param>
        /// <param name="workpackageId">Slot id that feature is created in. Don't set this value if you want to assign to latest slot</param>
        /// <param name="onlySetEffectiveDate">Set to true if you want to get only effectiveDate from slot(workpackageId) (Not to assign to slot (workpackageId) </param>
        public void Commit(Feature feature, bool asCorrection = false, int workpackageId = int.MinValue, bool onlySetEffectiveDate = false)
        {
            if (asCorrection)
                feature.TimeSlice.CorrectionNumber++;
            else
            {
                if (_publicSlots.Count != 1)
                    feature.TimeSlice.SequenceNumber++;
            }
            var publicSlot = int.MinValue != workpackageId
                ? _publicSlots.FirstOrDefault(t => t.Id == workpackageId)
                : _publicSlots[_publicSlots.Count - 1];



            if (feature.TimeSlice.Interpretation != TimeSliceInterpretationType.TEMPDELTA)
            {
                Debug.Assert(publicSlot != null, nameof(publicSlot) + " != null");
                feature.TimeSlice.ValidTime.BeginPosition = publicSlot.EffectiveDate;
                if (onlySetEffectiveDate)
                    publicSlot = _publicSlots[_publicSlots.Count - 1];
            }

            //feature.TimeSlice.ValidTime.BeginPosition = _publicSlots.FirstOrDefault(t=>t.Id ==workpackageId). [_publicSlots.Count - 1].EffectiveDate;


            var aimEvent = new AimEvent
            {
                TimeSlice = new TossMetaData.TimeSlice
                {
                    BeginPosition = feature.TimeSlice.ValidTime.BeginPosition,
                    EndPosition = feature.TimeSlice.ValidTime.EndPosition,
                },

                Interpretation = (feature.TimeSlice.Interpretation == TimeSliceInterpretationType.BASELINE || feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA) ? Interpretation.PermanentDelta : Interpretation.TempDelta,

                WorkPackage = _privateSlots.LastOrDefault(t => t.PublicSlot == publicSlot).Id,
                //int.MinValue != workpackageId ? _privateSlots.FirstOrDefault(t=>t.PublicSlot==_publicSlots.FirstOrDefault()) _privateSlots[_privateSlots.Count - 1].,
                Version = new TimeSliceVersion(feature.TimeSlice.SequenceNumber, feature.TimeSlice.CorrectionNumber),
                Data = feature
            };

            if (feature.TimeSlice.FeatureLifetime != null)
            {
                aimEvent.LifeTimeBegin = feature.TimeSlice.FeatureLifetime.BeginPosition;
                aimEvent.LifeTimeEnd = feature.TimeSlice.FeatureLifetime.EndPosition;
            }

            Mock<IOperationContext> mockContext = new Mock<IOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedOperationContext(mockContext.Object))
            {
                mockContext.SetupGet(t => t.ServiceSecurityContext.PrimaryIdentity.IsAuthenticated).Returns(true);
                mockContext.SetupGet(t => t.ServiceSecurityContext.PrimaryIdentity.Name).Returns("1\\TOSSM:1.0.3.120");
                CommonOperationResult result;
                if (asCorrection)
                    result = TemporalityService.CommitCorrection(aimEvent);
                else
                {
                    result = TemporalityService
                        .CommitNewEvent(aimEvent);
                }
                if (!result.IsOk) throw new Exception(result.ErrorMessage);
            }
        }

        public void Decommit(Feature arp)
        {
            Mock<IOperationContext> mockContext = new Mock<IOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedOperationContext(mockContext.Object))
            {
                mockContext.SetupGet(t => t.ServiceSecurityContext.PrimaryIdentity.IsAuthenticated).Returns(true);
                mockContext.SetupGet(t => t.ServiceSecurityContext.PrimaryIdentity.Name).Returns("1\\TOSSM:1.0.3.120");
                var deleted =
                    TemporalityService.Decommission(
                        new FeatureId()
                        {
                            FeatureTypeId = (int)arp.FeatureType,
                            WorkPackage = _privateSlots[_privateSlots.Count - 1].Id,
                            Guid = arp.Identifier
                        },
                        _publicSlots[_publicSlots.Count - 1].EffectiveDate);
                if (!deleted)
                    throw new Exception($"Couldn't decommit feature");
            }
            arp.TimeSlice.FeatureLifetime.EndPosition = _publicSlots[_publicSlots.Count - 1].EffectiveDate;
            arp.TimeSlice.CorrectionNumber++;
            arp.TimeSlice.ValidTime.EndPosition = arp.TimeSlice.FeatureLifetime.EndPosition;
        }

        public void Cancel(Feature arp, Interpretation interpretation = Interpretation.PermanentDelta)
        {
            var timeSliceId = new TimeSliceId
            {
                FeatureTypeId = (int)arp.FeatureType,
                Guid = arp.Identifier,
                Version = new TimeSliceVersion(arp.TimeSlice.SequenceNumber, -1),
                WorkPackage = _privateSlots[_privateSlots.Count - 1].Id
            };
            Mock<IOperationContext> mockContext = new Mock<IOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedOperationContext(mockContext.Object))
            {
                mockContext.SetupGet(t => t.ServiceSecurityContext.PrimaryIdentity.IsAuthenticated).Returns(true);
                mockContext.SetupGet(t => t.ServiceSecurityContext.PrimaryIdentity.Name).Returns("1\\TOSSM:1.0.3.120");
                var result = TemporalityService.CancelSequence(timeSliceId, interpretation, arp.TimeSlice.ValidTime.BeginPosition);
                if (!result.IsOk) throw new Exception(result.ErrorMessage);
            }
        }

        public void DeleteSlots()
        {
            if (_tossMode) return;
            try
            {
                _publicSlots.ForEach(t => NoAixmDataService.DeletePublicSlot(t.Id));
                _publicSlots.Clear();
                _privateSlots.ForEach(t => TemporalityService.DeletePrivateSlot(t.Id));
                _privateSlots.Clear();
            }
            catch
            {
                // ignored
            }

            _slotCount = 0;
        }

        public void PublishSlot(int slotId = int.MinValue)
        {
            var publicSlot = _publicSlots[_publicSlots.Count - 1];
            if (slotId != int.MinValue)
                publicSlot = _publicSlots.FirstOrDefault(t => t.Id == slotId);

            Debug.Assert(publicSlot != null, $"Couldn't find public slot which id is {slotId}");
            publicSlot.Status = SlotStatus.Published;
            NoAixmDataService.UpdatePublicSlot(publicSlot);
            TemporalityService.PublishPublicSlot(publicSlot);
        }

        public List<Feature> GetFeatures(Guid? identifier, FeatureType featureType, Interpretation interpretation = Interpretation.BaseLine, DateTime? dateTime = null, bool onlyPublishedSlot = false, int workPackage = int.MinValue)
        {
            var states = TemporalityService.GetActualDataByDate(
                new FeatureId
                {
                    FeatureTypeId = (int)featureType,
                    Guid = identifier,
                    WorkPackage = (workPackage != int.MinValue) ? workPackage : (onlyPublishedSlot) ? 0 : _privateSlots[_privateSlots.Count - 1].Id
                },
                false, dateTime ?? _publicSlots[_publicSlots.Count - 1].EffectiveDate, interpretation);
            return states.Select(state => state.Data.Feature).ToList();
        }

        public List<AbstractState<AimFeature>> GetStatesInRangeByInterpretation(Guid? identifier, FeatureType featureType, int workPackage, bool slotOnly,
            DateTime dateTimeStart, DateTime dateTimeEnd, Interpretation interpretation = Interpretation.BaseLine)
        {
            return TemporalityService.GetStatesInRangeByInterpretation(new FeatureId()
            {
                FeatureTypeId = (int)featureType,
                Guid = identifier,
                WorkPackage = workPackage
            }, slotOnly, dateTimeStart, dateTimeEnd, interpretation).ToList();
        }

        public List<AbstractEvent<AimFeature>> GetEvolution(Guid? identifier, FeatureType featureType, int workPackage)
        {
            return TemporalityService.GetEvolution(new FeatureId()
            {
                FeatureTypeId = (int)featureType,
                Guid = identifier,
                WorkPackage = workPackage
            }).ToList();
        }

        public StateWithDelta<AimFeature> GetActualDataForEditing(Guid? identifier, FeatureType featureType, int workPackage, DateTime actualDate, Interpretation interpretation, DateTime? endDate = null)
        {
            return TemporalityService.GetActualDataForEditing(new FeatureId()
            {
                FeatureTypeId = (int)featureType,
                Guid = identifier,
                WorkPackage = workPackage
            }, actualDate, interpretation, endDate);
        }

        public int LastSlotId => _privateSlots[_privateSlots.Count - 1].Id;

        public IList<AbstractState<AimFeature>> GetActualDataByFeatureType(FeatureType featureType, int workPackage, bool slotOnly, DateTime dateTimeParam, Interpretation interpretationParam = Interpretation.Snapshot, DateTime? currentDateParam = null, Filter filter = null, Projection projection = null)
        {
            return TemporalityService.GetActualDataByDate(new FeatureId()
            {
                FeatureTypeId = (int)featureType,
                Guid = null,
                WorkPackage = (workPackage != int.MinValue) ? workPackage : _privateSlots[_privateSlots.Count - 1].Id
            }, slotOnly, dateTimeParam, interpretationParam, currentDateParam, filter, projection);
        }

        public IList<AbstractState<AimFeature>> GetActualDataByDate(IFeatureId featureIdParam, bool slotOnly, DateTime dateTimeParam, Interpretation interpretationParam = Interpretation.Snapshot, DateTime? currentDateParam = null, Filter filter = null, Projection projection = null)
        {
            return TemporalityService.GetActualDataByDate(featureIdParam, slotOnly, dateTimeParam, interpretationParam, currentDateParam, filter, projection);
        }

        public StateWithDelta<AimFeature> GetActualDataForEditing(Feature feature, int workPackage, DateTime actualDate, Interpretation interpretation, DateTime? endDate = null)
        {
            return GetActualDataForEditing(feature.Identifier, feature.FeatureType, workPackage, actualDate, interpretation, endDate);
        }
    }
}