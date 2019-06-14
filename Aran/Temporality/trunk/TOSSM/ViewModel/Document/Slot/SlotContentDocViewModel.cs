using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;
using TOSSM.Util;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Document.Slot
{
    public class SlotContentDocViewModel : DocViewModel, IPresenterParent
    {
        public static string ToolContentId = "Feature Relations";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/case.png", UriKind.RelativeOrAbsolute);

        public DataPresenterModel DataPresenter { get; private set; }

        #region Load


        //private Task Parallel(List<int> featureTypes, Action<int> action)
        //{
        //    List<Task> result = new List<Task>();
        //    foreach (var featureType in featureTypes)
        //    {
        //        result.Add(Task.Run(() =>
        //        {
        //            action(featureType);
        //        }));

        //    }

        //    return Task.WhenAny(result.ToArray());
        //}

        private async Task LoadRelations()
        {
            var featureTypes = CurrentDataContext.CurrentService.GetPrivateSlotFeatureTypes(_slotId).OrderBy(t=>((FeatureType)t).ToString());
            if (featureTypes != null)
            {
                await Util.Parallel.All(featureTypes.ToList(), (featureType) =>
                {
                    var model = new SingleTypeRelationViewModel { FeatureType = (FeatureType)featureType };
                    var items = DataProvider.GetSlotContent(EditedSlot, featureType, true);
                    if (items.Count > 0)
                    {
                        foreach (var aimFeature in items)
                        {
                            model.Items.Add(aimFeature);
                        }
                        // RelationsData.Add(model);
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Background,
                            (Action)(() => RelationsData.Add(model)));

                    }
                });
            }
        }


        public PrivateSlot EditedSlot { get; set; }

        public override async void Load()
        {
            if (IsLoaded) return;

            DataPresenter.BlockerModel.Block();
            EditedSlot = CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlotById(_slotId);

            Title = "Content of space " +
                    EditedSlot.Name +
                    AiracSelectorViewModel.AiracMessage(EditedSlot.PublicSlot.EffectiveDate);

            DataPresenter.EffectiveDate = EditedSlot.PublicSlot.EffectiveDate;
            AiracDate = EditedSlot.PublicSlot.EffectiveDate;

            await LoadRelations();

            if (RelationsData.Count > 0)
            {
                RelationsDataGridVisibility = Visibility.Visible;
                SelectedRelation = RelationsData.First();
            }
            else
            {
                RelationsDataGridVisibility = Visibility.Hidden;
            }

            IsLoaded = true;
            DataPresenter.BlockerModel.Unblock();
            //await DataPresenter.BlockerModel.BlockForAction(
            //        async  () =>
            //            {
                            

            //            });

        }

        #endregion

        private readonly int _slotId;
        public SlotContentDocViewModel(int slotId)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
            _slotId = slotId;
            CanClose = true;
        }

        #region Relations UI

        private SingleTypeRelationViewModel _selectedRelation;
        public SingleTypeRelationViewModel SelectedRelation
        {
            get => _selectedRelation;
            set
            {
                _selectedRelation = value;
                OnPropertyChanged("SelectedRelation");

                if (SelectedRelation == null) return;

                DataPresenter.FeatureType = SelectedRelation.FeatureType;
            }
        }

        private ICollectionView _relationsDataFiltered;
        public ICollectionView RelationsDataFiltered
        {
            get
            {
                if (_relationsDataFiltered == null)
                {
                    _relationsDataFiltered = CollectionViewSource.GetDefaultView(RelationsData);
                    _relationsDataFiltered.GroupDescriptions.Clear();
                }
                return _relationsDataFiltered;
            }
            set => _relationsDataFiltered = value;
        }

        private MtObservableCollection<SingleTypeRelationViewModel> _relationsData;
        public MtObservableCollection<SingleTypeRelationViewModel> RelationsData
        {
            get => _relationsData ?? (_relationsData = new MtObservableCollection<SingleTypeRelationViewModel>());
            set
            {

                _relationsData = value;
                OnPropertyChanged("RelationsData");
            }
        }

        private Visibility _relationsDataGridVisibility;
        public Visibility RelationsDataGridVisibility
        {
            get => _relationsDataGridVisibility;
            set
            {
                _relationsDataGridVisibility = value;
                OnPropertyChanged("RelationsDataGridVisibility");
            }
        }

      

        #endregion

        #region Commands



        #endregion

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
            if (SelectedRelation==null)
            {
                DataPresenter.FeatureData=new List<object>();
                DataPresenter.UpdateFeatureDataFiltered();
                return;
            }

            DataPresenter.FeatureData=new List<object>(SelectedRelation.Items);
            DataPresenter.UpdateFeatureDataFiltered();
        }

        #endregion

    }
}
