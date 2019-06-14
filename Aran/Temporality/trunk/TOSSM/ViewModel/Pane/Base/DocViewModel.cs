using System;
using Aran.Aim;

namespace TOSSM.ViewModel.Pane.Base
{
    public class DocViewModel : PaneViewModel
    {
        #region Ctor

        protected DocViewModel()
        {
        }

        public DocViewModel(FeatureType type, Guid id, DateTime date)
        {
            FeatureType = type;
            FeatureIdentifier = id;
            AiracDate = date;
            CanClose = true;
        }

        #endregion

        #region IsDirty

        public bool IsDirty { get; set; }

        #endregion

        #region IsVisible

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value) return;
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        #endregion

        #region FeatureType

        public FeatureType FeatureType { get; set; }

        #endregion

        #region FeatureIdentifier

        public Guid FeatureIdentifier { get; set; }

        #endregion

        #region Id

        public int Id { get; set; }

        #endregion

        #region AiracDate

        public virtual void OnAiracDateChanged()
        {
        }

        private DateTime _airacDate;
        public DateTime AiracDate
        {
            get => _airacDate;
            set
            {
                if (_airacDate==value) return;
                _airacDate = value;
                OnAiracDateChanged();
            }
        }

        #endregion
    }
}
