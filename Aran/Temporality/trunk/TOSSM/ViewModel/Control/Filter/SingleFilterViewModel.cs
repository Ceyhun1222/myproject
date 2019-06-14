using Aran.Aim;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace TOSSM.ViewModel.Control.Filter
{
    class SingleFilterViewModel : ViewModelBase
    {
        private FeatureType _mainFeature;

        public FeatureType MainFeature
        {
            get => _mainFeature;
            set => _mainFeature = value;
        }

        private MtObservableCollection<FeatureTreeViewItemViewModel> _firstGeneration;
        public MtObservableCollection<FeatureTreeViewItemViewModel> FirstGeneration
        {
            get => _firstGeneration ?? (_firstGeneration = new MtObservableCollection<FeatureTreeViewItemViewModel>());
            set
            {
                _firstGeneration = value;
                OnPropertyChanged("FirstGeneration");
            }
        }

    }
}
