using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.PropertyPrecision;
using Aran.Aim.Utilities;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.Extension.Message;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Control.DatabaseDate;
using TOSSM.ViewModel.Control.Interpretation;
using TOSSM.ViewModel.Control.OriginatorFilter;

namespace TOSSM.Util
{
    public class DataProvider : CommonDataProvider
    {
       

        public static IList<ReadonlyFeatureWrapper> GetSlotContent(PrivateSlot editedSlot, int featureType, bool slotOnly)
        {
            try
            {
                var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
                                                                                       {
                                                                                           FeatureTypeId = featureType, 
                                                                                           WorkPackage = editedSlot.Id
                                                                                       },
                    slotOnly,
                editedSlot.PublicSlot.EffectiveDate
                );

                return states.Select(state => new ReadonlyFeatureWrapper(state.Data)).ToList();
            }
            catch (Exception exception)
            {
                MessageBoxHelper.Show(exception.Message, "Error while retreiving data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return new List<ReadonlyFeatureWrapper>();
        }


        
        public static List<object> GetDataForEvolution(FeatureType featureType, Guid id, DateTime actualDate)
        {
            var events=CurrentDataContext.CurrentService.GetEvolution(
                new FeatureId
                    {
                        FeatureTypeId = (int)featureType,
                        Guid = id
                    });

            if (events==null) return new List<object>();

          
            if (CurrentDataContext.CurrentUser.ActivePrivateSlot != null)
            {
                var activeSlotId = CurrentDataContext.CurrentUser.ActivePrivateSlot.Id;
                var activeSlotValidDate = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;

                var inSlot = events.Where(t => t.WorkPackage == activeSlotId).ToList();
                if (inSlot.Count > 0)
                {

                    var minSubmitDate=inSlot.Min(t => t.SubmitDate);
                    var maxSubmitDate=inSlot.Max(t => t.SubmitDate);

                    var result = new List<object>();
                    foreach (var abstractEvent in events)
                    {
                        var wrapper = new ReadonlyFeatureWrapper(abstractEvent.Data);

                        

                        if (abstractEvent.WorkPackage == activeSlotId)
                        {
                            wrapper.SetProperty("Conflict", "Active Slot");
                        }
                        else
                        {
                            if (abstractEvent.SubmitDate < maxSubmitDate && abstractEvent.TimeSlice.BeginPosition > activeSlotValidDate)
                            {
                                wrapper.SetProperty("Conflict", "Active Slot Interferes");
                            }
                            else
                            //if (events2.Any(t => t.SubmitDate > minSubmitDate && t.TimeSlice.BeginPosition>=activeSlotValidDate))
                            //{
                            //    wrapper.SetProperty("Conflict", "Active Slot Interferes");
                            //}
                            //else
                            {
                                wrapper.SetProperty("Conflict", "Ok");
                            }
                        }

                        var userExtension = abstractEvent.Data.MessageExtensions.OfType<UserExtension>().FirstOrDefault();
                        if (userExtension != null)
                        {
                            wrapper.SetProperty("User", userExtension.User);
                        }
                        wrapper.SetProperty("Cancelled", abstractEvent.IsCanceled ? "true" : "");

                        wrapper.SetProperty("SubmitDate", abstractEvent.SubmitDate.ToString("yyyy/MM/dd HH:mm"));
                        result.Add(wrapper);
                    }
                    return result;

                }
                else
                {
                    var result = new List<object>();
                    foreach (var abstractEvent in events)
                    {
                        var wrapper = new ReadonlyFeatureWrapper(abstractEvent.Data);
                     
                        var userExtension = abstractEvent.Data.MessageExtensions.OfType<UserExtension>().FirstOrDefault();
                        if (userExtension != null)
                        {
                            wrapper.SetProperty("User", userExtension.User);
                        }

                        wrapper.SetProperty("Cancelled", abstractEvent.IsCanceled ? "true" : "");

                        wrapper.SetProperty("SubmitDate", abstractEvent.SubmitDate.ToString("yyyy/MM/dd HH:mm"));

                       

                        result.Add(wrapper);
                    }
                    return result;
                }
            }
            else
            {
                var result = new List<object>();
                foreach (var abstractEvent in events)
                {
                    var wrapper = new ReadonlyFeatureWrapper(abstractEvent.Data);
                    wrapper.SetProperty("SubmitDate", abstractEvent.SubmitDate.ToString("yyyy/MM/dd HH:mm"));
                    result.Add(wrapper);
                }
                return result;
            }
        }

        public static List<AimFeature> GetDataForGeoIntersections(AimFeature feature1, FeatureType featureType, DateTime actualDate)
        {
            var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType},
                      false,
                      actualDate
                      );

            var result = new List<AimFeature>();

            var total = states.Count;

            if (total > 0)
            {
                var i = 0;
                foreach (AimFeature feature2 in states.Select(state => state.Data))
                if (feature1.Identifier!=feature2.Identifier)
                {
                    i++;
                    MainManagerModel.Instance.StatusText = "Searching intersections with " + featureType + " " + i + "/" + total + " ...";

                    if (GeometryFormatter.HasIntersection(feature1, feature2))
                    {
                        result.Add(feature2);
                    }
                }
            }

            return result;
        }


        private static DateTime cacheDate;
        private static Dictionary<FeatureType, IList<AbstractState<AimFeature>>> cachedStates =
            new Dictionary<FeatureType, IList<AbstractState<AimFeature>>>();

       
        public static List<AimFeature> GetReverseLinksTo(AimFeature feature1, FeatureType featureType, DateTime actualDate, bool useCache=false)
        {
            IList<AbstractState<AimFeature>> states;
            if (useCache)
            {
                if (cacheDate==actualDate)
                {
                    if (!cachedStates.TryGetValue(featureType,out states))
                    {
                        states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId {FeatureTypeId = (int) featureType},
                            false,
                            actualDate
                            );
                        cachedStates[featureType] = states;
                    }
                }
                else
                {
                    states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType },
                            false,
                            actualDate
                            );
                    cacheDate = actualDate;
                    cachedStates.Clear();
                    cachedStates[featureType] = states;
                }
            }
            else
            {
                states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType },
                    false,
                    actualDate
                    );
            }

            var result = new List<AimFeature>();

            var total = states.Count;

            if (total > 0)
            {
                var i = 0;
                foreach (AimFeature feature2 in states.Select(state => state.Data))
                    {
                        i++;
                        MainManagerModel.Instance.StatusText = "Searching reverse links from " + featureType + " " + i + "/" + total + " ...";

                        if (HasLinkTo(feature2, feature1))
                        {
                            result.Add(feature2);
                        }
                    }
            }

            return result;
        }

        private static bool HasLinkTo(AimFeature feature2, AimFeature feature1)
        {
            var featurePropList = new List<RefFeatureProp>();
            AimMetadataUtility.GetReferencesFeatures(feature2.Feature, featurePropList);
            return featurePropList.Any(t => t.FeatureType == feature1.FeatureType && t.RefIdentifier == feature1.Identifier);
        }



        public static List<object> GetDataForPresenter(FeatureType featureType, DateTime actualDate, Orinination selectedOrigination, 
            StateInterpretation selectedInterpretation, DatabaseState databaseState, DateTime endDate, string selectedLanguage = "")
        {
            try
            {
                IList<AbstractState<AimFeature>> states=new List<AbstractState<AimFeature>>();

                switch (selectedOrigination)
                {
                    case Orinination.New:
                        states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType },
                                    false,
                                    actualDate,
                                    selectedInterpretation == StateInterpretation.Baseline?Interpretation.BaseLine:Interpretation.Snapshot,
                                    (databaseState==DatabaseState.Now)?null:(DateTime?)endDate
                                    );
                        states = states.Where(t => t.Data.Feature.TimeSlice.FeatureLifetime.BeginPosition == actualDate).ToList();
                        break;
                    case Orinination.Deleted:
                        states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType },
                                    false,
                                    actualDate,
                            selectedInterpretation == StateInterpretation.Baseline ? Interpretation.BaseLine : Interpretation.Snapshot,
                                    (databaseState == DatabaseState.Now) ? null : (DateTime?)endDate
                                    );
                        states = states.Where(t => t.Data.Feature.TimeSlice.FeatureLifetime.EndPosition == actualDate).ToList();
                        break;
                    case Orinination.Changed:
                        states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType },
                                    false,
                                    actualDate
                                    );
                        states = states.Where(t => t.Data.Feature.TimeSlice.FeatureLifetime.BeginPosition != actualDate && 
                            t.Data.Feature.TimeSlice.ValidTime.BeginPosition == actualDate).ToList();
                        break;
                    case Orinination.InSlot:
                        states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType },
                                    true,
                                    actualDate,
                                    selectedInterpretation == StateInterpretation.Baseline ? Interpretation.BaseLine : Interpretation.Snapshot,
                                    (databaseState == DatabaseState.Now) ? null : (DateTime?)endDate
                                    );
                        break;
                    case Orinination.AllData:
                        states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)featureType },
                                   false,
                                   actualDate,
                                   selectedInterpretation == StateInterpretation.Baseline ? Interpretation.BaseLine : Interpretation.Snapshot,
                                   (databaseState == DatabaseState.Now) ? null : (DateTime?)endDate
                                   );
                        break;
                }

                var list = new List<object>();
                if (states.Count > 0)
                {
                    foreach (var state in states)
                    {
                        list.Add(new ReadonlyFeatureWrapper(state.Data));
                    }
                }

                if (String.IsNullOrEmpty(selectedLanguage))
                    return list;
                else
                {
                    Enum.TryParse(selectedLanguage, out language lang);
                    return list
                        .OfType<ReadonlyFeatureWrapper>()
                        .Where(x => x.Feature.Feature?.Annotation.Any(
                               y => y.TranslatedNote.Any(
                               z => z.Note.Lang == lang)) == true)
                        .ToList<object>();
                }

            }
            catch (Exception exception)
            {
                MessageBoxHelper.Show(exception.Message, "Error while retreiving data", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return new List<object>();
        }

       

        private static FeaturesPrecisionConfiguration _publicationConfiguration;
        public static FeaturesPrecisionConfiguration PublicationConfiguration {
            get
            {
                if (_publicationConfiguration == null)
                {
                    _publicationConfiguration=new FeaturesPrecisionConfiguration();
                    var pConf =
                        Configurations.FirstOrDefault(
                            t => t.Name == ConfigurationName.PublicationResolutionConfiguration);
                    if (pConf != null && pConf.Data != null)
                    {
                        _publicationConfiguration.FromBytes(pConf.Data);
                    }
                }
                return _publicationConfiguration;
            }
        }

        private static List<Configuration> _configurations;
        public static IEnumerable<Configuration> Configurations => _configurations ??
                                                                   (_configurations =
                                                                       CurrentDataContext.CurrentNoAixmDataService.GetAllConfigurations().ToList());

        public static void UpdatePrecisionConfiguration(Configuration selectedConfiguration)
        {
            if (selectedConfiguration.Name == ConfigurationName.PublicationResolutionConfiguration)
            {
                PublicationConfiguration.FromBytes(selectedConfiguration.Data);
            }

            CurrentDataContext.CurrentNoAixmDataService.UpdateConfiguration(selectedConfiguration);
            var old = _configurations.FirstOrDefault(t => t.Id == selectedConfiguration.Id);
            if (old == null)
            {
                _configurations.Add(selectedConfiguration);
            }
            else
            {
                var index = _configurations.IndexOf(old);
                _configurations.RemoveAll(t => t.Id == selectedConfiguration.Id);
                _configurations.Insert(index, selectedConfiguration);
            }
        }
    }
}
