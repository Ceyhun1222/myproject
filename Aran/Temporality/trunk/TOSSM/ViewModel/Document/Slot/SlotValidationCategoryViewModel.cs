using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;
using TOSSM.ViewModel.Document.Relations.Util;
using TOSSM.ViewModel.Document.Slot.Problems.ProblemList;
using TOSSM.ViewModel.Pane;

namespace TOSSM.ViewModel.Document.Slot
{
    public class SlotValidationCategoryViewModel : ViewModelBase, IPresenterParent
    {
        #region Actions
        
        public Action OnSelectedFeatureChanged { get; set; }
        public Action OnReloadData { get; set; }

        private async void SelectedFeatureChanged(DataPresenterModel presenterModel)
        {
            if (OnSelectedFeatureChanged != null)
            {
                await DataPresenter.BlockerModel.BlockForAction(() => OnSelectedFeatureChanged());
            }
        }

        #endregion

        #region ViewModels

        public DataPresenterModel DataPresenter { get; private set; }
        public SlotValidationCategoryViewModel(string violatingFeaturesText, ProblemListViewModel problemListViewModel, Visibility specialViewVisible = Visibility.Visible)
        {
            DataPresenter = new DataPresenterModel
                                {
                                    ViewModel = this,
                                    OnSelectionChanged = SelectedFeatureChanged
                                };
			ViolatingFeaturesText = violatingFeaturesText;
            ProblemListViewModel = problemListViewModel;
			SpecialViewVisible = specialViewVisible;
        }

        public ProblemListViewModel ProblemListViewModel { get; set; }

		private string _violatingFeaturesText;
		public string ViolatingFeaturesText
		{
			get => _violatingFeaturesText;
		    set
			{
				_violatingFeaturesText = value;
                OnPropertyChanged("ViolatingFeaturesText");
			}
		}

        #endregion

        #region Implementation of IPresenterParent

        protected override void OnDispose()
        {
            DataPresenter.IsTerminated = true;
        }

        public async void ReloadData(DataPresenterModel model)
        {
            if (OnReloadData!=null)
            {
                await DataPresenter.BlockerModel.BlockForAction(() => OnReloadData());            
            }
        }

        public DateTime AiracDate { get; set; }

        #endregion

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

		private Visibility _specialViewVisible;
		public Visibility SpecialViewVisible 
		{
			get => _specialViewVisible;
		    set 
			{ 
				_specialViewVisible = value;
				OnPropertyChanged("SpecialViewVisible");
			} 
		}
        #endregion
    }
}