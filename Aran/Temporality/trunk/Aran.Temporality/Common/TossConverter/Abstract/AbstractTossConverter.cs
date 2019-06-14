using System;
using System.Collections.Generic;
using System.Threading;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.TossConverter.Interface;
using MongoDB.Driver;

namespace Aran.Temporality.Common.TossConverter.Abstract
{
    public abstract class AbstractTossConverter
    {
        protected abstract ITossReadableRepository GetConverterFrom(string repositoryNameFrom);

        protected abstract ITossConverterTo GetConverterTo(string repositoryNameTo);

        public Action<MessageCauseType, string, string> Logger { get; set; }

        public bool IsExist(string repositoryNameTo)
        {
            return GetConverterTo(repositoryNameTo).IsExist();
        }

        public void Convert(string repositoryNameFrom, string repositoryNameTo, bool createGeoIndex, CancellationTokenSource token = null)
        {
            var converterFrom = GetConverterFrom(repositoryNameFrom);
            var converterTo = GetConverterTo(repositoryNameTo);

            List<int> workPackages;
            try
            {
                workPackages = converterFrom.GetWorkPackages();
            }
            catch (Exception e)
            {
                Logger(MessageCauseType.CommonError, "Error while reading workpackages int the first repository", e.ToString());
                return;
            }

            Logger(MessageCauseType.SystemMessage, $"Repository '{repositoryNameTo}' cleaning in progress", "");

            converterTo.ClearRepository();

            Logger(MessageCauseType.SystemMessage, $"Convert from '{repositoryNameFrom}' to '{repositoryNameTo}' started", "");

            for (var index = 0; index < workPackages.Count; index++)
            {
                if (token?.IsCancellationRequested == true)
                    return;

                var workPackage = workPackages[index];
                IEnumerable<AbstractEvent<AimFeature>> eventStorages;
                try
                {
                    eventStorages = converterFrom.GetEventStorages(workPackage);
                }
                catch (Exception e)
                {
                    Logger(MessageCauseType.CommonError,
                        $"Error while reading Event Storages for worckpage {workPackage}", e.ToString());
                    continue;
                }

                Logger(MessageCauseType.SystemMessage, $"WorkPackage {workPackage} ({index + 1}/{workPackages.Count})", "");

                var number = 0;

                foreach (var abstractEvent in eventStorages)
                {
                    if (token?.IsCancellationRequested == true)
                        return;

                    number++;

                    AimFeature aimFeature;
                    try
                    {
                        aimFeature = converterTo.AddEvent(workPackage, abstractEvent, createGeoIndex);
                    }
                    catch (MongoException mongoException)
                    {
                        ConverterUtil.HandleMongoException(workPackage, number, mongoException, abstractEvent, Logger, out MessageCauseType type);
                        converterTo.AddEventWithGeoProblem(workPackage, abstractEvent, type);
                        continue;
                    }
                    catch (Exception e)
                    {
                        Logger(MessageCauseType.CommonError, $"{workPackage}.{number}) {e.Message}", e.ToString() + "\n" + e.StackTrace);
                        continue;
                    }

                    ConverterUtil.ApplyTimeSlice(abstractEvent);

                    ConverterUtil.CheckFeatures(abstractEvent?.Data, aimFeature, workPackage, number, Logger);
                }
            }

            Logger(MessageCauseType.SystemMessage, "Successfully completed!", "");
        }
    }
}
