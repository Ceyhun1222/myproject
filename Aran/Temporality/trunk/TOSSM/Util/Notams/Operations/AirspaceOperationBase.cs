using System;
using System.Collections;
using System.Linq;
using System.Windows;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using FluentNHibernate.Conventions;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Control.LightElementControl;
using TOSSM.ViewModel.Document;
using TOSSM.ViewModel.Document.Editor;
using TOSSM.ViewModel.Document.Single.Editable;

namespace TOSSM.Util.Notams.Operations
{
    internal abstract class AirspaceOperationBase : BaseOperation
    {
        protected abstract void PrepareForActivation(NotamFeatureEditorViewModel notamViewModel);
        protected abstract void Activate(NotamFeatureEditorViewModel notamViewModel);
        protected abstract void PrepareForCancelation(NotamFeatureEditorViewModel notamViewModel);
        protected abstract void Cancel(NotamFeatureEditorViewModel notamViewModel);



        protected void SetUpperLimit(NotamFeatureEditorViewModel notamViewModel, AirspaceGeometryComponent geometryComponent)
        {
            var upperLimitString = notamViewModel.ActualNotam.ItemG;
            if (upperLimitString != null)
            {
                ValDistanceVertical upperLimit = null;
                CodeVerticalReference? upperLimitReference = null;
                GetLimits(upperLimitString, out upperLimit, out upperLimitReference);
                geometryComponent.TheAirspaceVolume.UpperLimit = upperLimit;
                geometryComponent.TheAirspaceVolume.UpperLimitReference = upperLimitReference;
            }
        }

        protected void SetLowerLimit(NotamFeatureEditorViewModel notamViewModel, AirspaceGeometryComponent geometryComponent)
        {
            var lowLimitString = notamViewModel.ActualNotam.ItemF;

            if (lowLimitString != null)
            {
                ValDistanceVertical lowerLimit = null;
                CodeVerticalReference? lowerLimitReference = null;
                GetLimits(lowLimitString, out lowerLimit, out lowerLimitReference);
                geometryComponent.TheAirspaceVolume.LowerLimit = lowerLimit;
                geometryComponent.TheAirspaceVolume.LowerLimitReference = lowerLimitReference;
            }
        }

        protected AirspaceActivation CreateActivation(NotamFeatureEditorViewModel notamViewModel)
        {

            var activation = new AirspaceActivation { Status = CodeStatusAirspace.ACTIVE, Activity = GetCodeAirspaceActivity(notamViewModel) };

            if (notamViewModel.ActualNotam.Schedule == null)
                activation.TimeInterval.Add(new Timesheet
                {
                    TimeReference = CodeTimeReference.UTC,
                    StartDate = notamViewModel.StartDateTime.ToString("dd-MM"),
                    EndDate = ((DateTime)notamViewModel.EndDateTime).ToString("dd-MM"),
                    StartTime = notamViewModel.StartDateTime.ToString("HH:mm"),
                    EndTime = ((DateTime)notamViewModel.EndDateTime).ToString("HH:mm")
                });
            else
            {
                var intervals = notamViewModel.ActualNotam.Schedule.Split(' ');
                if (intervals.Length == 1 && intervals[0].Equals("SR-SS"))
                {
                    activation.TimeInterval.Add(new Timesheet
                    {
                        TimeReference = CodeTimeReference.UTC,
                        StartDate = notamViewModel.StartDateTime.ToString("dd-MM"),
                        EndDate = ((DateTime)notamViewModel.EndDateTime).ToString("dd-MM"),
                        StartEvent = CodeTimeEvent.SR,
                        EndEvent = CodeTimeEvent.SS
                    });
                }
                else
                    foreach (var interval in intervals)
                    {
                        for (DateTime date = notamViewModel.StartDateTime.Date; date.Date <= ((DateTime)notamViewModel.EndDateTime).Date; date = date.AddDays(1))
                        {
                            activation.TimeInterval.Add(new Timesheet
                            {
                                TimeReference = CodeTimeReference.UTC,
                                StartDate = date.ToString("dd-MM"),
                                EndDate = date.ToString("dd-MM"),
                                StartTime = $"{interval.Substring(0, 2)}:{interval.Substring(2, 2)}",
                                EndTime = $"{interval.Substring(5, 2)}:{interval.Substring(7, 2)}"
                            });
                        }

                    }
            }
            var note = new Note
            {
                Purpose = CodeNotePurpose.DESCRIPTION,
                TranslatedNote =
                {
                    new LinguisticNote
                    {
                        Note = new TextNote
                        {
                            Value =
                                $"NOTAM {notamViewModel.Notam.Series}{notamViewModel.Notam.Number}/{notamViewModel.Notam.Year.ToString().Substring(2)}",
                            Lang = language.ENG
                        }
                    }
                }
            };
            activation.Annotation.Add(note);
            return activation;
        }

        protected CodeAirspaceActivity? GetCodeAirspaceActivity(NotamFeatureEditorViewModel notamViewModel)
        {
            if (notamViewModel.Notam.ItemE.Contains("MIL OP") || notamViewModel.Notam.ItemE.Contains("MILITARY"))
            {
                return CodeAirspaceActivity.MILOPS;
            }

            if (notamViewModel.Notam.ItemE.Contains("AERIAL FLT"))
            {
                return CodeAirspaceActivity.UAV;
            }

            if (notamViewModel.Notam.ItemE.Contains("AEROBATIC"))
            {
                return CodeAirspaceActivity.AEROBATICS;
            }

            return null;
        }


        protected  Feature ChooseAirspace(Notam notam, int workpackage)
        {
            Feature feature = null;
            var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.Airspace,
                WorkPackage = workpackage
            }, false, notam.StartValidity);


            var features = states.Select(t => t.Data.Feature).ToList();

            features.Sort(delegate (Feature x, Feature y)
                {
                    var air1 = x as Airspace;
                    var air2 = y as Airspace;
                    if (air1.Name == null && air2.Name == null) return 0;
                    if (air1.Name == null) return -1;
                    if (air2.Name == null) return 1;
                    return air1.Name.CompareTo(air2.Name);
                }
            );

            FeatureSelectionViewModel.ShowDialog(model => { feature = model.Feature; }
                , features);
            return feature;
        }

        protected NotamFeatureEditorViewModel GetViewModelForNotamC(Notam notam, int workpackage)
        {
            Notam notamToCancel = NotamUtils.GetNotamToCancel(notam);
            if (notamToCancel == null)
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while requesting data. Canceling notam is not exist.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            var prevFeature = ChooseAirspace(notamToCancel, workpackage);

            if (prevFeature == null)
            {
                return null;
            }

            var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int) FeatureType.Airspace,
                WorkPackage = workpackage,
                Guid = prevFeature.Identifier
            }, false, notamToCancel.StartValidity, Interpretation.Snapshot, notamToCancel.EndValidity);


            if (states.IsEmpty() || states.All(t => t.Data.Feature.TimeSlice.Interpretation !=
                                                    TimeSliceInterpretationType.TEMPDELTA))
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while requesting data. There is no TEMPDELTA based on this notam.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
            
            return new NotamFeatureEditorViewModel(notam, notamToCancel, prevFeature, workpackage)
            {
                IsNotNullFilter = true
            };
        }


        protected CommonOperationResult CancelSequence(Feature feature, Interpretation interpretation)
        {
            var timeSliceId = new TimeSliceId
            {
                FeatureTypeId = (int) feature.FeatureType,
                Guid = feature.Identifier,
                Version = new TimeSliceVersion(feature.TimeSlice.SequenceNumber, -1)
            };
            var result = CurrentDataContext.CurrentService.CancelSequence(timeSliceId, interpretation);
            return result;
        }

        protected void CreateTempDeltaForAirspace(NotamFeatureEditorViewModel notamViewModel)
        {
            notamViewModel.PrepareDelta();
            if (notamViewModel.EditingType == DocumentEditingType.EditAsSequence)
            {
                CommonDataProvider.CommitAsNewSequence(notamViewModel.EditedFeature as Feature,
                    notamViewModel.Workpackage);
                MainManagerModel.Instance.View(notamViewModel.EditedFeature as Feature, notamViewModel.StartDateTime,
                    notamViewModel.Workpackage);
                MainManagerModel.Instance.Close(notamViewModel);
            }
            else if (notamViewModel.EditingType == DocumentEditingType.EditAsCorrection)
            {
                CommonDataProvider.CommitAsCorrection(notamViewModel.EditedFeature as Feature,
                    notamViewModel.Workpackage);
                MainManagerModel.Instance.View(notamViewModel.EditedFeature as Feature, notamViewModel.StartDateTime,
                    notamViewModel.Workpackage);
                MainManagerModel.Instance.Close(notamViewModel);
            }
        }

        private bool IsValid(Airspace airspace)
        {
            if (airspace.Activation.Count > 1)
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while processing data. Airspace has 0 or more than 1 geometry components.",
                    "Error while processing data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            if (airspace.GeometryComponent.Count > 1)
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while processing data. Airspace has 0 or more than 1 geometry components.",
                    "Error while processing data", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            if (airspace.Activation != null && airspace.Activation.IsNotEmpty() && airspace.Activation[0].Status == CodeStatusAirspace.ACTIVE)
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while processing data. Airspace is already active.",
                    "Error while processing data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void UpdateActivation(NotamFeatureEditorViewModel notamViewModel)
        {
            var activation = CreateActivation(notamViewModel);
            var activationProperty = GetProperty(notamViewModel, "Activation") as AimEditablePropertyModel;
            var activations = activationProperty.Value as ArrayList ?? new ArrayList();
            activations.Clear();
            activations.Add(activation);
            FixValue(activationProperty);
        }

        private void UpdateGeometryComponent(NotamFeatureEditorViewModel notamViewModel)
        {

            var geomertyComponentProperty =
                GetProperty(notamViewModel, "GeometryComponent") as AimEditablePropertyModel;
            var geomertyComponents = geomertyComponentProperty.Value as ArrayList ?? new ArrayList();
            var geometryComponent = geomertyComponents[0] as AirspaceGeometryComponent;

            SetLowerLimit(notamViewModel, geometryComponent);
            SetUpperLimit(notamViewModel, geometryComponent);

            FixValue(geomertyComponentProperty);


        }

        protected void PrepareExistingAirspaceForActivation(NotamFeatureEditorViewModel notamViewModel, DateTime start, DateTime end)
        {
            try
            {
                Init(notamViewModel);
                notamViewModel.StartDateTime = start;
                notamViewModel.EndDateTime = end;

                notamViewModel.AiracDate = notamViewModel.StartDateTime;
                notamViewModel.UpdateTitle();
                notamViewModel.Interpretation = Interpretation.Snapshot;

                var data = CommonDataProvider.GetDataForEditor(notamViewModel.FeatureType, notamViewModel.Feature.Identifier,
                    notamViewModel.StartDateTime, notamViewModel.Workpackage, Interpretation.Snapshot,
                    notamViewModel.EndDateTime);

                if (data == null)
                {
                    MessageBoxHelper.Show(
                        "Sorry, there was error while requesting data. It is possible that you specified actual date when feature does not exist.",
                        "Error while requesting data", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    notamViewModel.IsLoaded = true;
                }
                else
                {
                    notamViewModel.IsLoaded = true;


                    Airspace airspace = null;
                    if (data.StateAfterDelta.Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA)
                    {
                        airspace = notamViewModel.PrepareAsCorrection(data, notamViewModel.StartDateTime,
                            notamViewModel.EndDateTime) as Airspace;
                        notamViewModel.EditingType = DocumentEditingType.EditAsCorrection;
                    }
                    else if (data.StateAfterDelta.Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.PERMDELTA)
                    {
                        airspace = notamViewModel.PrepareAsSequence(data, notamViewModel.StartDateTime,
                            notamViewModel.EndDateTime) as Airspace;
                        notamViewModel.EditingType = DocumentEditingType.EditAsSequence;
                    }

                    airspace.TimeSlice.CorrectionNumber = 0;
                    airspace.TimeSlice.Interpretation = TimeSliceInterpretationType.TEMPDELTA;

                    if (!IsValid(airspace))
                    {
                        notamViewModel.IsLoaded = true;
                        return;
                    }

                    notamViewModel.EditedFeature = airspace;
                    UpdateActivation(notamViewModel);
                    UpdateGeometryComponent(notamViewModel);
                }
            }
            catch (Exception e)
            {
                notamViewModel.EditedFeature = null;
                MessageBoxHelper.Show(
                    "Sorry, there was error while processing data. Unexpected error.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                notamViewModel.IsLoaded = true;
                LogManager.GetLogger(typeof(PermanentAirspaceOperationBase)).Error(e, e.Message);
            }
        }
    }
}