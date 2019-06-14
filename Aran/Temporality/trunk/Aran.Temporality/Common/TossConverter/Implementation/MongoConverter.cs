using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.TossConverter.Interface;
using Aran.Temporality.Internal.Implementation.Repository.Linq;
using Aran.Temporality.Internal.MetaData.Offset;
using MongoDB.Driver;
using Aran.Aim;
using Aran.Temporality.Common.Implementation;

namespace Aran.Temporality.Common.TossConverter.Implementation
{
    internal class MongoConverter : TossReadableWritebleMongoRepository, ITossConverterTo, ITossGeometryCleaner
    {
        private readonly Dictionary<string, MongoRepository> _problemsRepository;

        public MongoConverter(string repositoryName) : base(repositoryName)
        {
            _problemsRepository = new Dictionary<string, MongoRepository>();
        }

        public void AddEventWithGeoProblem(int workPackage, AbstractEvent<AimFeature> abstractEvent, MessageCauseType type)
        {
            try
            {
                var marker = $"events_{workPackage}_{type}";
                if (!_problemsRepository.Keys.Contains(marker))
                    _problemsRepository[marker] = new MongoRepository($"{RepositoryName}_geo_problems", marker, false, false);

                _problemsRepository[marker].Add(abstractEvent?.Data, new OffsetEventMetaData<string>(abstractEvent));
            }
            catch
            {
            }
        }

        public IEnumerable<Tuple<MessageCauseType, string, string>> Clean(int workPackage)
        {
            var repositoryWithoutIndex = GetEventRepository($"events_{workPackage}", false, true);

            string log;
            MessageCauseType type;
            string errorMessage;

            foreach (FeatureType featureType in System.Enum.GetValues(typeof(FeatureType)))
            {
                while (true)
                {
                    log = null;
                    type = MessageCauseType.UnknownGeometryError;
                    errorMessage = null;

                    Exception exception = null;

                    try
                    {
                        if (repositoryWithoutIndex.IsFeaturesExist((int)featureType))
                            repositoryWithoutIndex.CreateGeoIndexes((int)featureType);
                        break;
                    }
                    catch (MongoException mongoException)
                    {
                        if (mongoException.Message.Contains("Can't extract geo keys"))
                        {
                            var objectIdIndex = mongoException.Message.IndexOf("ObjectId('") + 10;
                            if (objectIdIndex > 10)
                            {
                                var objectId = mongoException.Message.Substring(objectIdIndex, 24);
                                var aimEvent = repositoryWithoutIndex.GetEvent(objectId, (int)featureType);
                                var feature = aimEvent?.Data?.Feature;

                                log = $"{workPackage}, {feature?.Identifier}, {feature?.FeatureType}, {feature?.TimeSlice?.Interpretation}, " +
                                      $"{feature?.TimeSlice?.SequenceNumber}.{feature?.TimeSlice?.CorrectionNumber}, " +
                                      $"{feature?.TimeSlice?.ValidTime?.BeginPosition} - {feature?.TimeSlice?.ValidTime?.EndPosition?.ToString() ?? "~"}";

                                errorMessage = mongoException.Message;

                                var reasons = new Dictionary<MessageCauseType, string>
                                {
                                    {MessageCauseType.DuplicateVertices, "Duplicate vertices:"},
                                    {MessageCauseType.SelfIntersect, "Edges "},
                                    {MessageCauseType.IncorrectLatLon, "longitude/latitude is out of bounds"},
                                    {MessageCauseType.Less3UniqueVertices, "Loop must have at least 3 different vertices:"},
                                    {MessageCauseType.LineHasOneUniqueVertices, "GeoJSON LineString must have at least 2 vertices"},
                                };

                                foreach (var reason in reasons)
                                {
                                    var messageIndex = errorMessage.IndexOf(reason.Value);
                                    if (messageIndex > 0)
                                    {
                                        var additionalMessage = errorMessage.Substring(messageIndex);
                                        log += $"\n{additionalMessage}";
                                        type = reason.Key;
                                        break;
                                    }
                                }

                                if (type == MessageCauseType.UnknownGeometryError)
                                    log += $"\n{errorMessage.Substring(Math.Max(0, errorMessage.Length - 200))}";

                                AddEventWithGeoProblem(workPackage, aimEvent, type);

                                repositoryWithoutIndex.RemoveByKey(objectId, (int)featureType);
                            }
                            else
                            {
                                log = $"Unknown mongo exception: {mongoException.Message}";
                                type = MessageCauseType.UnknownGeometryError;
                                exception = mongoException;
                            }
                        }
                        else
                        {
                            log = $"Unknown mongo exception: {mongoException.Message}";
                            type = MessageCauseType.UnknownGeometryError;
                            errorMessage = mongoException.Message;
                            exception = mongoException;
                        }
                    }
                    catch (Exception ex)
                    {
                        log = $"Unknown exception: {ex.Message}";
                        type = MessageCauseType.UnknownGeometryError;
                        errorMessage = ex.Message;
                        exception = ex;
                    }

                    if (log != null)
                        yield return Tuple.Create(type, log, errorMessage);

                    if (exception != null)
                        throw exception;
                }
            }
        }
    }
}
