using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using FluentNHibernate.Conventions;
using MvvmCore;
using TOSSM.Util;
using TOSSM.Util.Wrapper;
using TOSSM.View.Tool;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Pane;
using TOSSM.ViewModel.Pane.Base;
using Aran.Aim.Features;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.CommonUtil.View.Dialog;
using TOSSM.ViewModel.Control.SlotSelector;

namespace TOSSM.ViewModel.Tool
{
    class SlotMergeViewModel : ToolViewModel, IPresenterParent
    {
        public static string ToolContentId = "Merge";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/merge.png", UriKind.RelativeOrAbsolute);

        public SlotMergeViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
            DataPresenter = new DataPresenterModel { ViewModel = this };
        }

        public void ReloadData(DataPresenterModel model)
        {
            if (model == DataPresenter)
            {
                if (SelectedRelation == null)
                {
                    DataPresenter.FeatureData = new List<object>();
                    DataPresenter.UpdateFeatureDataFiltered();
                    return;
                }
                DataPresenter.FeatureData = new List<object>(SelectedRelation.Items);
            }
        }

        public void Reload(Feature feature)
        {
            if (!IsLoaded) return;

            if (Contains(feature))
                InitMerge(Slot1.Id, Slot2.Id);
        }

        public bool Contains(Feature feature)
        {

            return RelationsData1.Any(t => t.FeatureType == feature.FeatureType && t.Items.Any(f => ((ReadonlyFeatureWrapper)f).Identifier == feature.Identifier)) || RelationsData2.Any(t => t.FeatureType == feature.FeatureType && t.Items.Any(f => ((ReadonlyFeatureWrapper)f).Identifier == feature.Identifier));

        }

        public DateTime AiracDate { get; set; }

        public DataPresenterModel DataPresenter { get; private set; }
        public bool IsBusy { get; private set; } = false;

        private PrivateSlot _slot1;
        public PrivateSlot Slot1
        {
            get => _slot1;
            set
            {
                _slot1 = value;
                OnPropertyChanged(nameof(Slot1));
            }
        }

        private PrivateSlot _slot2;
        public PrivateSlot Slot2
        {
            get => _slot2;
            set
            {
                _slot2 = value;
                OnPropertyChanged(nameof(Slot2));
            }
        }

        #region Public methods

        public void InitMerge(int slot1, int slot2)
        {

            DataPresenter.BlockerModel.BlockForAction(
                () =>
                {
                    if (IsBusy == true) return;
                    IsBusy = true;
                    RelationsTabControlVisibility = Visibility.Collapsed;
                    Slots.Clear();
                    RelationsData1.Clear();
                    RelationsData2.Clear();
                    _nonConflictedFeatures.Clear();

                    Slot1 = CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlotById(slot1);
                    Slot2 = CurrentDataContext.CurrentNoAixmDataService.GetPrivateSlotById(slot2);

                    Slots.Add(Slot1);
                    Slots.Add(Slot2);



                    DataPresenter.EffectiveDate = Slot1.PublicSlot.EffectiveDate;
                    AiracDate = Slot1.PublicSlot.EffectiveDate;
                    LoadRelations(Slot1, RelationsData1);
                    LoadRelations(Slot2, RelationsData2);

                    Slot1Selected = true;

                    SetGridVisibility(RelationsData1);
                    RelationsTabControlVisibility = (RelationsData1.IsNotEmpty() || RelationsData1.IsNotEmpty())
                        ? Visibility.Visible
                        : Visibility.Collapsed;

                    FindConflicts();

                    IsLoaded = true;
                    IsBusy = false;

                    MainManagerModel.Instance.StatusText = $"{RelationsData1.Count + RelationsData2.Count} features loaded, {ConflictedFeatures.Count} conflicts found...";
                });
        }


        #endregion

        #region Commands

        private RelayCommand _mergeCommand;

        public RelayCommand MergeCommand
        {
            get => _mergeCommand ?? (_mergeCommand = new RelayCommand(t =>
                       {
                           Merge();
                       },
                t => (_nonConflictedFeatures.IsNotEmpty() && (ConflictedFeatures == null || ConflictedFeatures.IsEmpty())) ||
                      (ConflictedFeatures!= null && (ConflictedFeatures.IsEmpty() || ConflictedFeatures.All(f => f.Slot1Selected || f.Slot2Selected)))));
            set => _mergeCommand = value;
        }

        private void Merge()
        {
            var viewModel = new PrivateSlotViewModel
            {
                Id = 0,
                Name = null,
                Reason = null,
                SlotStatus = SlotStatus.Opened
            };
            var dialog = new EditPrivateSlotDialog();

            viewModel.CancelAction = () => { dialog.Close(); };
            SavePrivateSlotAndMerge(viewModel, dialog);

            dialog.Owner = Application.Current.MainWindow;
            dialog.DataContext = viewModel;

            viewModel.Show();
            dialog.ShowDialog();
        }

        private void SavePrivateSlotAndMerge(PrivateSlotViewModel viewModel, EditPrivateSlotDialog dialog)
        {
            viewModel.OkAction = () =>
            {
                dialog.Close();
                DataPresenter.BlockerModel.BlockForAction(
                    () =>
                    {
                        int slotId = MainManagerModel.Instance.SlotSelectorToolViewModel.SavePrivateSlot(viewModel);
                        if (slotId <= 0)
                        {
                            MainManagerModel.Instance.StatusText = $"Operation failed.";
                            return;
                        }

                        int totalCount = 0;
                        int count = 0;
                        totalCount = ConflictedFeatures.Count + _nonConflictedFeatures.Count;
                        foreach (var conflict in ConflictedFeatures)
                        {
                            try
                            {
                                Migrator.AddFeatureToSlot(conflict.SelectedFeature.Feature.Feature, slotId, AiracDate, false);
                            }
                            catch (Exception e)
                            {
                                MessageBoxHelper.Show(e.Message);
                            }
                            count++;
                            MainManagerModel.Instance.StatusText = "Processed " + count + " features from " + totalCount + "...";

                        }

                        foreach (var feature in _nonConflictedFeatures.Values)
                        {
                            try
                            {
                                Migrator.AddFeatureToSlot(feature.Feature.Feature, slotId, AiracDate, false);
                            }
                            catch (Exception e)
                            {
                                MessageBoxHelper.Show(e.Message);
                            }
                            count++;
                            MainManagerModel.Instance.StatusText = "Processed " + count + " features from " + totalCount + "...";
                        }

                        MainManagerModel.Instance.StatusText = "Done";
                    }
                );
            };
        }
        #endregion

        #region Relations UI

        public ObservableCollection<PrivateSlot> Slots = new ObservableCollection<PrivateSlot>();

        private PrivateSlot _selectedSlot;
        public PrivateSlot SelectedSlot
        {
            get => _selectedSlot;
            set
            {
                _selectedSlot = value;
                OnPropertyChanged("SelectedSlot");
                SetRelarationDataFiltered();
            }
        }

        private bool _slot1Selected;
        public bool Slot1Selected
        {
            get => _slot1Selected;
            set
            {
                _slot1Selected = value;
                OnPropertyChanged("Slot1Selected");
                if (_slot1Selected && Slots.IsNotEmpty())
                {
                    SelectedSlot = Slots.First();
                    SetGridVisibility(RelationsData1);
                }
            }
        }

        private bool _slot2Selected;
        public bool Slot2Selected
        {
            get => _slot2Selected;
            set
            {
                _slot2Selected = value;
                OnPropertyChanged("Slot2Selected");
                if (_slot2Selected && Slots.IsNotEmpty())
                {
                    SelectedSlot = Slots.Last();
                    SetGridVisibility(RelationsData2);
                }
            }
        }

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
                    SetRelarationDataFiltered();
                }
                return _relationsDataFiltered;
            }
            set => _relationsDataFiltered = value;
        }


        private void SetRelarationDataFiltered()
        {
            if (SelectedSlot != null)
            {
                if (SelectedSlot == Slots.First())
                    _relationsDataFiltered = CollectionViewSource.GetDefaultView(RelationsData1);
                else
                    _relationsDataFiltered = CollectionViewSource.GetDefaultView(RelationsData2);
                _relationsDataFiltered.GroupDescriptions.Clear();
                _relationsDataFiltered.GroupDescriptions.Add(
                    new PropertyGroupDescription("PurposeDescription"));
                OnPropertyChanged(nameof(RelationsDataFiltered));
            }
        }

        private MtObservableCollection<SingleTypeRelationViewModel> _relationsData1;
        public MtObservableCollection<SingleTypeRelationViewModel> RelationsData1
        {
            get => _relationsData1 ?? (_relationsData1 = new
                       MtObservableCollection<SingleTypeRelationViewModel>());
            set
            {

                _relationsData1 = value;
                OnPropertyChanged("RelationsData1");
            }
        }


        private MtObservableCollection<SingleTypeRelationViewModel> _relationsData2;
        public MtObservableCollection<SingleTypeRelationViewModel> RelationsData2
        {
            get => _relationsData2 ?? (_relationsData2 = new
                       MtObservableCollection<SingleTypeRelationViewModel>());
            set
            {

                _relationsData2 = value;
                OnPropertyChanged("RelationsData");
            }
        }


        private ObservableCollection<FeatureConflictViewModel> _conflictedFeatures;
        public ObservableCollection<FeatureConflictViewModel> ConflictedFeatures
        {
            get => _conflictedFeatures;
            set
            {
                _conflictedFeatures = value;
                OnPropertyChanged(nameof(ConflictedFeatures));
            }
        }

        private readonly Dictionary<Guid, ReadonlyFeatureWrapper> _nonConflictedFeatures = new Dictionary<Guid, ReadonlyFeatureWrapper>();


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

        private Visibility _relationsTabControlVisibility = Visibility.Collapsed;
        public Visibility RelationsTabControlVisibility
        {
            get => _relationsTabControlVisibility;
            set
            {
                _relationsTabControlVisibility = value;
                OnPropertyChanged(nameof(RelationsTabControlVisibility));
            }
        }


        #endregion

        private void LoadRelations(PrivateSlot slot, MtObservableCollection<SingleTypeRelationViewModel> relationsData)
        {

            var featureTypes = CurrentDataContext.CurrentService.GetPrivateSlotFeatureTypes(slot.Id).OrderBy(t => ((FeatureType)t).ToString());
            if (featureTypes != null)
            {
                foreach (var featureType in featureTypes)
                {
                    var model = new SingleTypeRelationViewModel { FeatureType = (FeatureType)featureType };
                    var items = DataProvider.GetSlotContent(slot, featureType, true);

                    foreach (var item in items)
                    {
                        if (_nonConflictedFeatures.ContainsKey(item.Identifier))
                        {
                            _nonConflictedFeatures.Remove(item.Identifier);
                        }
                        else
                        {
                            _nonConflictedFeatures.Add(item.Identifier, item);
                        }
                    }

                    if (items.Count > 0)
                    {
                        foreach (var aimFeature in items)
                        {
                            model.Items.Add(aimFeature);
                        }

                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Background,
                            (Action)(() => relationsData.Add(model)));

                    }

                }
            }
        }

        private void SetGridVisibility(MtObservableCollection<SingleTypeRelationViewModel> relationsData)
        {
            if (relationsData.Count > 0)
            {
                RelationsDataGridVisibility = Visibility.Visible;
                SelectedRelation = relationsData.First();
            }
            else
            {
                RelationsDataGridVisibility = Visibility.Hidden;
            }
        }


        private void FindConflicts()
        {
            var result = new ObservableCollection<FeatureConflictViewModel>();
            foreach (var singleTypeRelationViewModel1 in RelationsData1)
            {
                foreach (var item1 in singleTypeRelationViewModel1.Items)
                {
                    foreach (var singleTypeRelationViewModel2 in RelationsData2)
                    {
                        foreach (var item2 in singleTypeRelationViewModel2.Items)
                        {
                            var feature1 = (ReadonlyFeatureWrapper)item1;
                            var feature2 = (ReadonlyFeatureWrapper)item2;
                            if (feature1.Identifier == feature2.Identifier)
                            {
                                result.Add(new FeatureConflictViewModel(feature1, feature2, Slot1, Slot2));
                            }
                        }
                    }
                }
            }


            ConflictedFeatures = result;

            MainManagerModel.Instance.StatusText = $"{RelationsData1.Count + RelationsData2.Count} features loaded, {ConflictedFeatures.Count} conflicts found...";
        }

    }
}
