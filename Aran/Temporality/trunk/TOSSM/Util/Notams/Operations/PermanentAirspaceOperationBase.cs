using System;
using System.Collections;
using System.Linq;
using System.Windows;
using Aran.Aim;
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
    internal class PermanentAirspaceOperationBase : AirspaceOperationBase
    {
        public override void Save(NotamFeatureEditorViewModel notamViewModel)
        {
            if (notamViewModel.Notam.Type == (int)NotamType.C)
                Cancel(notamViewModel);
            else if (notamViewModel.Notam.Code45.Equals("CA"))
                Activate(notamViewModel);
            else
                throw new NotImplementedException();

        }


        public override FeatureType GetFeatureType(Notam notam, int workpackage = 0)
        {
            return FeatureType.Airspace;
        }

        public override NotamFeatureEditorViewModel GetViewModel(Notam notam, int workpackage = 0)
        {

            if (notam.Type == (int)NotamType.C)
            {
                return GetViewModelForNotamC(notam, workpackage);
            }

            var feature = ChooseAirspace(notam, workpackage);

            if (feature == null) return null;
            return new NotamFeatureEditorViewModel(notam, feature, workpackage)
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
                throw new NotImplementedException();

        }

        protected override void PrepareForActivation(NotamFeatureEditorViewModel notamViewModel)
        {
            PrepareExistingAirspaceForActivation(notamViewModel, notamViewModel.Notam.StartValidity, notamViewModel.Notam.EndValidity);
        }
   
        protected override void Activate(NotamFeatureEditorViewModel notamViewModel)
        {
            CreateTempDeltaForAirspace(notamViewModel);
        }

        protected override void PrepareForCancelation(NotamFeatureEditorViewModel notamViewModel)
        {
            PrepareExistingAirspaceForActivation(notamViewModel, notamViewModel.ActualNotam.StartValidity, notamViewModel.Notam.StartValidity);
        }

        protected override void Cancel(NotamFeatureEditorViewModel notamViewModel)
        {
            var feature = notamViewModel.EditedFeature as Feature;

            var states = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId
            {
                FeatureTypeId = (int)FeatureType.Airspace,
                WorkPackage = notamViewModel.Workpackage,
                Guid = feature.Identifier
            }, false, notamViewModel.RefNotam.StartValidity, Interpretation.Snapshot, notamViewModel.RefNotam.EndValidity);


            if (states.IsEmpty() || states.All(t => t.Data.Feature.TimeSlice.Interpretation != TimeSliceInterpretationType.TEMPDELTA))
            {
                MessageBoxHelper.Show(
                    "Sorry, there was error while requesting data. There is no TEMPDELTA based on reference NOTAM.",
                    "Error while requesting data", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            feature = states.Select(t => t.Data.Feature).First();

            var result = CancelSequence(feature, Interpretation.TempDelta);
            if (result.IsOk)
            {
                CreateTempDeltaForAirspace(notamViewModel);
            }


        }
    }
}