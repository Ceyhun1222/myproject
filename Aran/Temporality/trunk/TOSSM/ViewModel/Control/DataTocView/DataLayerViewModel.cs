using System;
using Aran.Aim;
using MvvmCore;

namespace TOSSM.ViewModel.Control.DataTocView
{
    public class DataLayerViewModel : ViewModelBase
    {
        public Action OnUpdateAction { get; set; }

        private FeatureType _featureType;
        public FeatureType FeatureType
        {
            get { return _featureType; }
            set
            {
                _featureType = value;
                OnPropertyChanged("FeatureType");
            }
        }

        public virtual void OnVisibilityChangedAction()
        {
            if (OnUpdateAction!=null)
            {
                OnUpdateAction();
            }
        }

        private bool _isVisible=true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible==value) return;
                _isVisible = value;
                OnPropertyChanged("IsVisible");
                OnVisibilityChangedAction();
                
            }
        }

        public virtual void OnLabelingChangedAction()
        {
            if (OnUpdateAction != null)
            {
                OnUpdateAction();
            }
        }
        private bool _isLabeling;
        public bool IsLabeling
        {
            get { return _isLabeling; }
            set
            {
                if (_isLabeling==value) return;
                _isLabeling = value;
                OnPropertyChanged("IsLabeling");
                OnLabelingChangedAction();
            }
        }

        public virtual void OnClusteringChangedAction()
        {
            if (OnUpdateAction != null)
            {
                OnUpdateAction();
            }
        }
        private bool _isClustering;
        public bool IsClustering
        {
            get { return _isClustering; }
            set
            {
                if (_isClustering==value) return;
                _isClustering = value;
                OnPropertyChanged("IsClustering");
                OnClusteringChangedAction();
            }
        }
    }
}