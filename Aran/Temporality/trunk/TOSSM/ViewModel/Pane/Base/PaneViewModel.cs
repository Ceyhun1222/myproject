using System;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;

namespace TOSSM.ViewModel.Pane.Base
{
    public class PaneViewModel : ViewModelBase, IActivate
    {
        public void ActivateMe()
        {
            MainManagerModel.Instance.ActiveDocument = this;
        }

        #region Load

        public bool IsLoaded { get; set; }

        public virtual void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;
        }

        #endregion

        #region IsClosed

        public virtual void OnClosed()
        {
        }

        private bool _isClosed;
        public bool IsClosed
        {
            get => _isClosed;
            set
            {
                _isClosed = value;
                if (IsClosed)
                {
                    OnClosed();
                }
            }
        }

        #endregion

        #region Title

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        #endregion

        #region IconSource

        private Uri _iconSource;
        public virtual Uri IconSource
        {
            get => _iconSource;

            set 
            { 
                if (_iconSource==value) return;
                _iconSource = value;
                OnPropertyChanged("IconSource");
            }
        }

        #endregion

        #region TitleToolTip

        private string _titleToolTip;
        public string TitleToolTip
        {
            get => _titleToolTip;
            set
            {
                _titleToolTip = value;
                OnPropertyChanged("TitleToolTip");
            }
        }

        #endregion

        #region ContentId

        private string _contentId;
        public string ContentId
        {
            get => _contentId;
            set
            {
                if (_contentId == value) return;
                _contentId = value;
                OnPropertyChanged("ContentId");
            }
        }

        #endregion

        #region IsSelected

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        #endregion

        #region IsActive

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value) return;
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        #endregion

        #region CanClose

        private bool _canClose;
        public bool CanClose
        {
            get => _canClose;
            set
            {
                if (_canClose == value) return;
                _canClose = value;
                OnPropertyChanged("CanClose");
            }
        }

        #endregion

        #region CanHide

        private bool _canHide;
        public bool CanHide
        {
            get => _canHide;
            set
            {
                if (_canHide == value) return;
                _canHide = value;
                OnPropertyChanged("CanHide");
            }
        }

        #endregion

        #region CanFloat

        private bool _canFloat = true;
        public bool CanFloat
        {
            get => _canFloat;
            set
            {
                if (_canFloat == value) return;
                _canFloat = value;
                OnPropertyChanged("CanFloat");
            }
        }

        #endregion
    }
}
