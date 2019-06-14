using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Data.XmlProvider;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;

namespace Aran.Temporality.CommonUtil.Util
{

    public class Migrator
    {
        public static void AddFeatureToSlot(Feature feature, int slot, DateTime actualDate, bool toActiveSlot = true)
        {
            var operation = new Operation
            {
                Result = true,
                Type =
                    (feature.TimeSlice?.FeatureLifetime?.EndPosition != null)
                        ? OperationType.Decommision
                        : OperationType.NewEvent
            };

            ValidateFeatureTimeslice(feature, operation);


            if (operation.Result)
            {
                feature.TimeSlice.Interpretation = feature.TimeSlice.Interpretation ==
                                                   TimeSliceInterpretationType.TEMPDELTA
                    ? TimeSliceInterpretationType.TEMPDELTA
                    : TimeSliceInterpretationType.PERMDELTA;

                feature.TimeSlice.FeatureLifetime = new TimePeriod(actualDate);
                if (feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA)
                {
                    feature.TimeSlice.ValidTime = new TimePeriod(actualDate);
                }

                var events = CurrentDataContext.CurrentService.GetActualDataByDate(
                    new FeatureId
                    {
                        Guid = feature.Identifier,
                        WorkPackage = slot,
                        FeatureTypeId = (int)feature.FeatureType
                    },
                    false, actualDate);

                ValidateFeature(feature, actualDate, events, operation);
            }

            if (operation.Result)
            {
                var counter = 2;
                while (counter-- > 0)
                {
                    try
                    {
                        var result = false;
                        switch (operation.Type)
                        {
                            case OperationType.Decommision:
                                result = CommonDataProvider.Decomission(feature, toActiveSlot ? 0 : slot);
                                break;
                            case OperationType.Correction:
                                CommonDataProvider.CommitAsCorrection(feature, toActiveSlot ? 0 : slot);
                                result = true;
                                break;
                            case OperationType.NewEvent:
                                CommonDataProvider.CommitAsNewSequence(feature, toActiveSlot ? 0 : slot);
                                result = true;
                                break;
                            case OperationType.Comission:
                                CommonDataProvider.Commission(feature, toActiveSlot ? 0 : slot);
                                result = true;
                                break;
                        }

                        if (result)
                            break;
                    }
                    catch (Exception exception)
                    {
                        string message = $"Error on importing the feature. Identifier: {feature.Identifier}, type: {feature.FeatureType}";
                        LogManager.GetLogger(typeof(Migrator)).Error(exception, message);
                        throw new Exception(message);
                    }
                }
                if (counter <= 0)
                {
                    string message =
                        $"Couldn't import the feature. Identifier :{feature.Identifier}, type: {feature.FeatureType}";
                    LogManager.GetLogger(typeof(Migrator)).Warn(message);
                    throw new Exception(message);
                }
            }
            else
            {
                string message = $"{operation.Message}. Identifier :{feature.Identifier}, type: {feature.FeatureType}";
                LogManager.GetLogger(typeof(Migrator)).Warn(message);
                throw new Exception(message);
            }
        }

        private static void ValidateFeatureTimeslice(Feature feature, Operation operation)
        {
            if (feature.TimeSlice == null)
            {
                operation.Result = false;
                operation.Message = "Feature Timeslice is null";
                return;
            }

            if (feature.TimeSlice.FeatureLifetime == null)
            {
                operation.Result = false;
                operation.Message = "Feature Lifetime is null";
                return;
            }

            if (feature.TimeSlice.ValidTime == null)
            {
                operation.Result = false;
                operation.Message = "Feature Lifetime is null";
                return;
            }

            if (feature.TimeSlice.SequenceNumber <= 0)
            {
                operation.Result = false;
                operation.Message = "Feature Sequence number is less or equal to 0";
                return;
            }

            if (feature.TimeSlice.CorrectionNumber < 0)
            {
                operation.Result = false;
                operation.Message = "Feature Correncetion number is less than 0";
                return;
            }
        }

        private static void ValidateFeature(Feature feature, DateTime actualDate, IList<AbstractState<AimFeature>> events, Operation operation)
        {
            if (events.Count == 0)
            {
                /*
                    Case when actual date is before the creation date is still unhandled. 
                    CurrentDataContext.CurrentService.GetActualDataByDate returns empty list.
                */
                if (feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA &&
                    operation.Type != OperationType.Decommision)
                {
                    operation.Type = OperationType.Comission;
                }
                else if (operation.Type == OperationType.Decommision)
                {
                    operation.Result = false;
                    operation.Message = "Can't decomission non existing feature";
                    return;
                }
                else
                {
                    operation.Result = false;
                    operation.Message = "Can't create temporary event for non existing feature";
                    return;
                }
            }
            else if (events.Count == 1)
            {
                var state = events.First();
                if (state.Data.Feature.TimeSlice.FeatureLifetime.EndPosition != null &&
                    state.Data.Feature.TimeSlice.FeatureLifetime.EndPosition <= actualDate)
                {
                    operation.Result = false;
                    operation.Message = "Can't edit decomissioned feature";
                    return;
                }

                if (state.Data.Feature.TimeSlice.FeatureLifetime.BeginPosition > actualDate)
                {
                    operation.Result = false;
                    operation.Message = "Can't edit feature before the creation date";
                    return;
                }

                if (feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA)
                {
                    if (feature.TimeSlice.ValidTime.EndPosition == null)
                    {
                        operation.Result = false;
                        operation.Message = "Can't create temporary event with undefined duration";
                        return;
                    }

                    if (feature.TimeSlice.ValidTime.EndPosition <= feature.TimeSlice.ValidTime.BeginPosition)
                    {
                        operation.Result = false;
                        operation.Message = "Can't create temporary event with negative duration";
                        return;
                    }

                    if (feature.TimeSlice.ValidTime.BeginPosition < feature.TimeSlice.FeatureLifetime.BeginPosition)
                    {
                        operation.Result = false;
                        operation.Message =
                            "Can't create temporary event with start of valid time before begin of life time";
                        return;
                    }
                }

                if (feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA)
                {
                    if (feature.TimeSlice.ValidTime.EndPosition != null)
                    {
                        operation.Result = false;
                        operation.Message = "Can't create permanent event with end of valid time";
                        return;
                    }
                }

                if (operation.Type != OperationType.Decommision)
                {
                    feature.TimeSlice.FeatureLifetime =
                        new TimePeriod(state.Data.Feature.TimeSlice.FeatureLifetime.BeginPosition);
                    if (state.Data.Feature.TimeSlice.ValidTime.BeginPosition == actualDate
                        || feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA)
                    {
                        //Commit as a correction
                        operation.Type = OperationType.Correction;

                        feature.TimeSlice.SequenceNumber = state.Data.Feature.TimeSlice.SequenceNumber;
                        feature.TimeSlice.CorrectionNumber = state.Data.Feature.TimeSlice.CorrectionNumber;
                    }
                    else
                    {
                        //Commit as new sequence
                        operation.Type = OperationType.NewEvent;
                        feature.TimeSlice.CorrectionNumber = 0;
                    }
                }
                else
                {
                    feature.TimeSlice.FeatureLifetime = new TimePeriod(state.Data.Feature.TimeSlice.FeatureLifetime.BeginPosition, actualDate);
                    feature.TimeSlice.ValidTime = new TimePeriod(actualDate, actualDate);
                }
            }
            else
            {
                operation.Result = false;
                operation.Message = "More than 2 states has been received";
                return;
            }
        }
        
        /// <returns>Return log</returns>
        public static string ImportXmlToSlot(ITemporalityService<AimFeature> service, int slot, DateTime actualDate,
            string xmlPath, Action<string> statusChangedAction = null)
        {
            var provider = new XmlDbProvider();
            provider.DefaultEffectiveDate = actualDate;
            provider.Open(xmlPath);
            var log = new StringBuilder();
            bool foundFeature;
            var list = Enum.GetValues(typeof(FeatureType));
            //var list = new[] {FeatureType.RouteSegment};
            foreach (var featureType in list)
            {
                //get data from provider
                if ((FeatureType) featureType == FeatureType.Airspace)
                {
                }

                foundFeature = false;
                var result = provider.GetVersionsOf((FeatureType) featureType, TimeSliceInterpretationType.BASELINE);
                if (result.IsSucceed)
                {
                    if (result.List.Count > 0)
                    {
                        foundFeature = true;
                    }
                }
                else
                {
                    result = provider.GetVersionsOf((FeatureType) featureType, TimeSliceInterpretationType.TEMPDELTA);
                    if (result.IsSucceed)
                    {
                        if (result.List.Count > 0)
                        {
                            foundFeature = true;
                        }
                    }
                    else
                    {
                        log.Append("can not load " + featureType + "\n");
                    }
                }

                if (foundFeature)
                {
                    HashSet<Guid> featureIds = new HashSet<Guid>();

                    int i = 0;
                    //log.Append("Loaded " + result.List.Count + " " + featureType + "\n");
                    foreach (var data in result.List)
                    {
                        i++;
                        statusChangedAction?.Invoke("Importing " + featureType + " " + i + "/" + result.List.Count);
                        var feature = data as Feature;
                        if (feature == null)
                            continue;
                        //feature.TimeSlice.Interpretation = TimeSliceInterpretationType.TEMPDELTA;
                        if (!featureIds.Add(feature.Identifier))
                        {
                            log.Append("Problem with " + featureType + " " + feature.Identifier);
                            log.Append("There is more than one version of this feature in snapshot.\n");
                        }

                        //var clone=FormatterUtil.Clone(feature);

                        //var myEvent = new AimEvent
                        //                  {
                        //                      WorkPackage = slot,
                        //                      Interpretation = feature.TimeSlice.Interpretation==TimeSliceInterpretationType.TEMPDELTA?Interpretation.TempDelta:Interpretation.PermanentDelta,
                        //                      TimeSlice = new TimeSlice(feature.TimeSlice.ValidTime.BeginPosition, feature.TimeSlice.ValidTime.EndPosition),
                        //                      LifeTimeBegin = feature.TimeSlice.FeatureLifetime.BeginPosition,
                        //                      LifeTimeEnd = feature.TimeSlice.FeatureLifetime.EndPosition,
                        //                      Data = feature,
                        //                  };


                        if (feature.TimeSlice.FeatureLifetime == null)
                        {
                            feature.TimeSlice.FeatureLifetime = new TimePeriod(actualDate);
                        }

                        var myEvent = new AimEvent
                        {
                            WorkPackage = slot,
                            Interpretation = feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA
                                ? Interpretation.TempDelta
                                : Interpretation.PermanentDelta,
                            LifeTimeBegin = feature.TimeSlice.FeatureLifetime.BeginPosition <= actualDate
                                ? feature.TimeSlice.FeatureLifetime.BeginPosition
                                : actualDate,
                            LifeTimeEnd = feature.TimeSlice.FeatureLifetime.EndPosition,
                            TimeSlice = new TimeSlice(actualDate),
                            //TimeSlice = new TimeSlice ( feature.TimeSlice.FeatureLifetime.BeginPosition <= actualDate ?
                            //                    feature.TimeSlice.FeatureLifetime.BeginPosition : actualDate, feature.TimeSlice.FeatureLifetime.EndPosition ),
                            Data = feature,
                        };



                        var counter = 10;
                        while (counter-- > 0)
                        {
                            try
                            {
                                var result2 = service.CommitNewEvent(myEvent);
                                if (!result2.IsOk)
                                {
                                    log.Append("Problem with " + featureType + " " + feature.Identifier);
                                    log.Append(result2.ErrorMessage + "\n");
                                }

                                break;
                            }
                            catch (Exception exception)
                            {
                                log.Append(exception.Message + "\n");
                            }
                        }

                        //break;

                    }
                }
            }

            statusChangedAction?.Invoke("Done");

            return log.ToString();

            //var logContent = log.ToString();
            //if (!string.IsNullOrWhiteSpace(logContent))
            //{

            //    if (logContent.Length > 1000)
            //    {
            //        logContent = logContent.Substring(0, 1000) + "...";
            //    }

            //    MessageBoxHelper.Show(logContent, "Import log", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //else
            //{
            //    MessageBoxHelper.Show("Snapshot was imported successfully.", "Import log", MessageBoxButton.OK,
            //        MessageBoxImage.Information);
            //}
        }


        private class Operation
        {
            public bool Result { get; set; }
            public String Message { get; set; }

            public OperationType Type { get; set; }
        }

        private enum OperationType
        {
            NewEvent,
            Correction,
            Decommision,
            Comission
        }
    }
}
