using System;
using System.Windows;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Document.Evolution
{
    public class EvolutionDocViewModel : DocViewModel, IPresenterParent
    {
        public static string ToolContentId = "Feature Evolution";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/evolution.png", UriKind.RelativeOrAbsolute);

        public DataPresenterModel DataPresenter { get; set; }

        #region Load

        public override async void Load()
        {
            if (IsLoaded) return;

            await DataPresenter.BlockerModel.BlockForAction(
                    () =>
                    {
                        var editedFeature = CommonDataProvider.GetState(FeatureType, FeatureIdentifier, AiracDate);

                        Title = "Change history of " + editedFeature.Feature.GetType().Name + " " + 
                            HumanReadableConverter.ShortAimDescription(editedFeature.Feature);

                        //DataPresenter.AddPredefinedColumn("#", "Picture", 0);

                        if (CurrentDataContext.CurrentUser!=null && CurrentDataContext.CurrentUser.ActivePrivateSlot!=null)
                        {
                            DataPresenter.AddPredefinedColumn("User", "User", 0);
                            DataPresenter.AddPredefinedColumn("Slot Status", "Conflict", 1);
                            DataPresenter.AddPredefinedColumn("Submit Date", "SubmitDate", 2);
                            DataPresenter.AddPredefinedColumn("Cancelled", "Cancelled", 3);
                        }
                        else
                        {
                            DataPresenter.AddPredefinedColumn("User", "User", 0);
                            DataPresenter.AddPredefinedColumn("Submit Date", "SubmitDate", 1);
                            DataPresenter.AddPredefinedColumn("Cancelled", "Cancelled", 2);
                        }
                        

                       

                        DataPresenter.FeatureType = FeatureType;
                        

                        IsLoaded = true;
                    });
        }

        #endregion


        public EvolutionDocViewModel(FeatureType type, Guid id, DateTime date)
            : base(type, id, date)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };

           
        }


        #region Implementation of IPresenterParent

        public override void OnClosed()
        {
            base.OnClosed();
            DataPresenter.IsTerminated = true;
        }

        protected override void OnDispose()
        {
            DataPresenter.IsTerminated = true;
        }


        public void ReloadData(DataPresenterModel model)
        {
            DataPresenter.FeatureData = DataProvider.GetDataForEvolution(FeatureType, FeatureIdentifier, AiracDate);
            DataPresenter.UpdateFeatureDataFiltered();
        }

        #endregion

        #region Selection

        private ReadonlyFeatureWrapper SelectedWrapper()
        {
            var selected = DataPresenter.SelectedFeature;

            return selected as ReadonlyFeatureWrapper;
        }

        private string _selectedCellColumnHeader;
        public string SelectedCellColumnHeader
        {
            get => _selectedCellColumnHeader;
            set
            {
                if (_selectedCellColumnHeader == value) return;
                _selectedCellColumnHeader = value;
            }
        }

        #endregion
    
        #region Commands

        private RelayCommand _viewCommand;
        public RelayCommand ViewCommand
        {
            get { return _viewCommand??(_viewCommand=
                new RelayCommand(
                    t=> MainManagerModel.Instance.View(SelectedWrapper(),SelectedWrapper().Feature.Feature.TimeSlice.ValidTime.BeginPosition),
                    t=> SelectedWrapper() != null && SelectedWrapper().GetProperty("Cancelled") != "true")); }
        }


        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand =
                    new RelayCommand(
                  t=> MainManagerModel.Instance.Edit(SelectedWrapper(),SelectedWrapper().Feature.Feature.TimeSlice.ValidTime.BeginPosition),
                  t =>
                  {
                      var wrapper = SelectedWrapper();
                      if (wrapper == null) return false;
                      if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return false;
                      if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;

                      if (wrapper.GetProperty("Cancelled") == "true") return false;

                      return wrapper.Feature.Feature.TimeSlice.ValidTime.BeginPosition ==
                          CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
                  }));
            }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand??(_deleteCommand=
                new RelayCommand(
                    t=>
                        {
                            if (MessageBoxHelper.Show("Are you sure you want to cancel selected change?",
                                                "Cancel Sequence", MessageBoxButton.YesNo,
                                                MessageBoxImage.Warning)
                                == MessageBoxResult.Yes)
                            {
                                var feature = SelectedWrapper().Feature.Feature;
                                var timeSliceId = new TimeSliceId
                                            {
                                                FeatureTypeId = (int) feature.FeatureType,
                                                Guid = feature.Identifier,
                                                Version = new TimeSliceVersion(feature.TimeSlice.SequenceNumber,-1)
                                            };
                                var result = CurrentDataContext.CurrentService.CancelSequence(timeSliceId, feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA? Interpretation.TempDelta : Interpretation.PermanentDelta,
                                    SelectedWrapper().Feature.Feature.TimeSlice.ValidTime.BeginPosition);

                                MainManagerModel.Instance.OnFeatureChanged(SelectedWrapper().Feature.Feature);
                                DataPresenter.FeatureType = FeatureType;
                            }
                        },
                    t=>
                        {
                            var wrapper = SelectedWrapper();
                            if (wrapper == null) return false;
                            if (CurrentDataContext.CurrentUser.ActivePrivateSlot==null) return false;
                            if (!CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.Editable) return false;
                            return true;
                            //TODDO: not safe!
                            // added for Teodor - now we can cancel any event at any time
                            //wrapper.Feature.Feature.TimeSlice.ValidTime.BeginPosition==
                              //  CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
                        })); }
        }

        #endregion
    
    }
}
