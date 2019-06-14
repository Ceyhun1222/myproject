using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.OperationResult;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using FluentNHibernate.Conventions;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Document.Editor;

namespace TOSSM.Util.Notams.Operations
{
    internal class TemporaryAirspaceOperation : AirspaceOperationBase
    {
        public override NotamFeatureEditorViewModel GetViewModel(Notam notam, int workpackage = 0)
        {
            if (notam.Type == (int)NotamType.C)
            {
                return GetViewModelForNotamC(notam, workpackage);
            }


            Airspace airspace = AimObjectFactory.CreateFeature(FeatureType.Airspace) as Airspace;
            airspace.Identifier = Guid.NewGuid();
            airspace.TimeSlice = new TimeSlice
            {
                ValidTime = new TimePeriod(notam.StartValidity, notam.EndValidity),
                FeatureLifetime = new TimePeriod(notam.StartValidity),
                SequenceNumber = 1,
                CorrectionNumber = 0,
                Interpretation = TimeSliceInterpretationType.TEMPDELTA
            };

            return new NotamFeatureEditorViewModel(notam, airspace, workpackage)
            {
                IsNotNullFilter = true
            };
        }

        public override void Prepare(NotamFeatureEditorViewModel notamViewModel)
        {
            if (notamViewModel.Notam.Type == (int)NotamType.C)
                PrepareForCancelation(notamViewModel);
            else if (notamViewModel.Notam.Code45.Equals("CA"))
                PrepareForActivation(notamViewModel);
            else
                throw new NotImplementedException($"Q-Code 45 {notamViewModel.Notam.Code45} is not supported");
        }

        public override void Save(NotamFeatureEditorViewModel notamViewModel)
        {
            if (notamViewModel.Notam.Type == (int)NotamType.C)
                Cancel(notamViewModel);
            else if (notamViewModel.Notam.Code45.Equals("CA"))
                Activate(notamViewModel);
            else
                throw new NotImplementedException($"Q-Code 45 {notamViewModel.Notam.Code45} is not supported");
        }


        public override FeatureType GetFeatureType(Notam notam, int workpackage = 0)
        {
            return FeatureType.Airspace;
        }



        protected override void PrepareForActivation(NotamFeatureEditorViewModel notamViewModel)
        {

            try
            {
                var airspace = notamViewModel.Feature;

                notamViewModel.AiracDate = airspace.TimeSlice.ValidTime.BeginPosition;
                notamViewModel.StartDateTime = airspace.TimeSlice.ValidTime.BeginPosition;
                notamViewModel.EndDateTime = airspace.TimeSlice.ValidTime.EndPosition;

                AddActivation(notamViewModel, (Airspace)notamViewModel.Feature);
                CreateGeometryComponent(notamViewModel, (Airspace)notamViewModel.Feature);
                notamViewModel.EditedFeature = airspace;
                notamViewModel.IsLoaded = true;
            }
            catch (Exception e)
            {
                notamViewModel.EditedFeature = null;
                MessageBoxHelper.Show(
                    "Sorry, there was error while processing data. Unexpected error.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                notamViewModel.IsLoaded = true;
                LogManager.GetLogger(typeof(TemporaryAirspaceOperation)).Error(e, e.Message);
            }

        }



        protected override void Activate(NotamFeatureEditorViewModel notamViewModel)
        {
            Airspace tempDelta = notamViewModel.EditedFeature as Airspace;
            Airspace airspace = AimObjectFactory.CreateFeature(FeatureType.Airspace) as Airspace;

            airspace.Identifier = tempDelta.Identifier;
            airspace.TimeSlice = new TimeSlice
            {
                Interpretation = TimeSliceInterpretationType.PERMDELTA,
                CorrectionNumber = 0,
                SequenceNumber = 1,
                FeatureLifetime = new TimePeriod(tempDelta.TimeSlice.FeatureLifetime.BeginPosition),
                ValidTime = new TimePeriod(tempDelta.TimeSlice.FeatureLifetime.BeginPosition)
            };

            airspace.Designator = tempDelta.Designator;
            airspace.Name = tempDelta.Name;
            AirspaceGeometryComponent geometryComponent =
                new AirspaceGeometryComponent { TheAirspaceVolume = new AirspaceVolume() };
            geometryComponent.TheAirspaceVolume.HorizontalProjection = new Surface();
            geometryComponent.TheAirspaceVolume.HorizontalProjection.Geo.Assign(tempDelta.GeometryComponent.First().TheAirspaceVolume.HorizontalProjection.Geo);
            airspace.GeometryComponent.Add(geometryComponent);

            if (notamViewModel.Notam.Type == (int)NotamType.N)
                CommonDataProvider.CommitAsNewSequence(airspace, notamViewModel.Workpackage);
            CommonDataProvider.CommitAsNewSequence(tempDelta, notamViewModel.Workpackage);

            airspace.TimeSlice.FeatureLifetime.EndPosition = notamViewModel.EndDateTime;
            airspace.TimeSlice.ValidTime.EndPosition = notamViewModel.EndDateTime;
            airspace.TimeSlice.ValidTime.BeginPosition = (DateTime)notamViewModel.EndDateTime;
            CommonDataProvider.Decomission(airspace, notamViewModel.Workpackage);
            MainManagerModel.Instance.View(notamViewModel.EditedFeature as Feature, notamViewModel.StartDateTime, notamViewModel.Workpackage);
            MainManagerModel.Instance.Close(notamViewModel);
        }

        protected override void PrepareForCancelation(NotamFeatureEditorViewModel notamViewModel)
        {
            PrepareExistingAirspaceForActivation(notamViewModel, notamViewModel.ActualNotam.StartValidity, notamViewModel.Notam.StartValidity);
            //try
            //{
            //    notamViewModel.StartDateTime = notamViewModel.ActualNotam.StartValidity;
            //    notamViewModel.EndDateTime = notamViewModel.Notam.StartValidity;
            //    notamViewModel.AiracDate = notamViewModel.StartDateTime;

            //    var airspace = notamViewModel.Feature.Clone() as Airspace;
            //    airspace.Identifier = Guid.NewGuid();
            //    airspace.TimeSlice = new TimeSlice
            //    {
            //        ValidTime = new TimePeriod(notamViewModel.StartDateTime, (DateTime)notamViewModel.EndDateTime),
            //        FeatureLifetime = new TimePeriod(notamViewModel.StartDateTime),
            //        SequenceNumber = 1,
            //        CorrectionNumber = 0,
            //        Interpretation = TimeSliceInterpretationType.TEMPDELTA
            //    };

            //    airspace.Activation.Clear();
            //    airspace.GeometryComponent.Clear();
            //    AddActivation(notamViewModel, (Airspace)airspace);
            //    CreateGeometryComponent(notamViewModel, (Airspace)airspace);
            //    notamViewModel.EditedFeature = airspace;
            //    notamViewModel.IsLoaded = true;
            //}
            //catch (Exception e)
            //{
            //    notamViewModel.EditedFeature = null;
            //    MessageBoxHelper.Show(
            //        "Sorry, there was error while processing data. Unexpected error.",
            //        "Error while requesting data", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    notamViewModel.IsLoaded = true;
            //    LogManager.GetLogger(typeof(TemporaryAirspaceOperation)).Error(e, e.Message);
            //}
        }

        protected override void Cancel(NotamFeatureEditorViewModel notamViewModel)
        {
            //if (!CancelAllSequences(notamViewModel)) return;
            if (!CancelSequences(notamViewModel)) return;

            Activate(notamViewModel);
        }

        private bool CancelSequences(NotamFeatureEditorViewModel notamViewModel)
        {
            var feature = notamViewModel.Feature as Feature;


            var states = CurrentDataContext.CurrentService.GetEvolution(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.Airspace,
                WorkPackage = notamViewModel.Workpackage,
                Guid = feature.Identifier
            });


            if (states.IsEmpty() || !states.Any(t => t.Data.Feature.TimeSlice.Interpretation ==
                                                    TimeSliceInterpretationType.TEMPDELTA
                                                    && t.Data.Feature.TimeSlice.ValidTime.BeginPosition.Equals(notamViewModel.ActualNotam.StartValidity)
                                                    && t.Data.Feature.TimeSlice.ValidTime.EndPosition.Equals(notamViewModel.ActualNotam.EndValidity)))
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while requesting data. There is no TEMPDELTA based on reference NOTAM.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return true;
            }


            if (states.Select(t => t.Data.Feature).Count(t => t.TimeSlice.FeatureLifetime.EndPosition != null) == 0)
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while requesting data. Temporary airspace need to be decomissioned.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return true;
            }

            if (!CancelSequences(states.Where(t => t.Data.Feature.TimeSlice.Interpretation ==
                                                   TimeSliceInterpretationType.TEMPDELTA
                                                   && t.Data.Feature.TimeSlice.ValidTime.BeginPosition.Equals(notamViewModel.ActualNotam.StartValidity)
                                                   && t.Data.Feature.TimeSlice.ValidTime.EndPosition.Equals(notamViewModel.ActualNotam.EndValidity)).ToList(), true)) return false;
            return CancelSequences(states.Where(t => t.Data.Feature.TimeSlice.FeatureLifetime.EndPosition != null).ToList(), false);
        }

        private bool CancelAllSequences(NotamFeatureEditorViewModel notamViewModel)
        {
            var feature = notamViewModel.Feature as Feature;


            var states = CurrentDataContext.CurrentService.GetEvolution(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.Airspace,
                WorkPackage = notamViewModel.Workpackage,
                Guid = feature.Identifier
            });


            if (states.IsEmpty() || states.All(t => t.Data.Feature.TimeSlice.Interpretation !=
                                                    TimeSliceInterpretationType.TEMPDELTA))
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while requesting data. There is no TEMPDELTA based on reference NOTAM.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return true;
            }


            if (states.Select(t => t.Data.Feature).Count(t => t.TimeSlice.FeatureLifetime.EndPosition != null) == 0)
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while requesting data. Temporary airspace need to be decomissioned.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return true;
            }


            if (!CancelSequences(states, true)) return false;
            return CancelSequences(states, false);
        }

        private bool CancelSequences(IList<AbstractEvent<AimFeature>> states, bool temp)
        {

            List<Feature> toCancel = null;
            if (temp)
                toCancel = states.Select(t => t.Data.Feature)
                .Where(t => t.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA).ToList();
            else
            {
                toCancel = states.Select(t => t.Data.Feature)
                    .Where(t => t.TimeSlice.Interpretation != TimeSliceInterpretationType.TEMPDELTA).ToList();
            }


            toCancel.Sort(
                (feature1, feature2) => feature2.TimeSlice.SequenceNumber.CompareTo(feature1.TimeSlice.SequenceNumber));
            foreach (var cancelFeature in toCancel)
            {
                var result = CancelSequence(cancelFeature, temp ? Interpretation.TempDelta : Interpretation.PermanentDelta);
                if (!result.IsOk)
                {
                    MessageBoxHelper.Show(
                        $"Sorry, there was error while processing data. {result.ErrorMessage}",
                        "Error while requesting data", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }


        private void AddActivation(NotamFeatureEditorViewModel notamViewModel, Airspace airspace)
        {
            var activation = CreateActivation(notamViewModel);
            airspace.Activation.Add(activation);
        }

        private void CreateGeometryComponent(NotamFeatureEditorViewModel notamViewModel, Airspace airspace)
        {

            var geometryComponent = new AirspaceGeometryComponent { TheAirspaceVolume = new AirspaceVolume() };
            SetLowerLimit(notamViewModel, geometryComponent);
            SetUpperLimit(notamViewModel, geometryComponent);

            List<string> coordinates = NotamUtils.GetCoordinates(notamViewModel.ActualNotam);

            CreateHorizontalProjection(coordinates, geometryComponent);
            airspace.GeometryComponent.Add(geometryComponent);
        }

        private void CreateHorizontalProjection(List<string> coordinates, AirspaceGeometryComponent geometryComponent)
        {
            var polygon = new Polygon
            {
                ExteriorRing = new Ring()
            };

            foreach (var coordinate in coordinates)
            {
                double x = GetLongitudeFromString(Regex.Match(coordinate, "[0-9]{1,7}[EW]").Value);
                double y = GetLatitudeFromString(Regex.Match(coordinate, "[0-9]{1,6}[NS]").Value);
                polygon.ExteriorRing.Add(new Aran.Geometries.Point(x, y));
            }
            var multuPolygon = new MultiPolygon { polygon };
            multuPolygon.Close();
            geometryComponent.TheAirspaceVolume.HorizontalProjection = new Surface();
            geometryComponent.TheAirspaceVolume.HorizontalProjection.Geo.Assign(multuPolygon);
        }

    }
}