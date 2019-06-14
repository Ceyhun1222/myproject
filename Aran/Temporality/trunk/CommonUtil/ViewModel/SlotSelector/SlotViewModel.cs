using System.Windows;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace TOSSM.ViewModel.Control.SlotSelector
{
    public class SlotViewModel : ViewModelBase
    {
        public static string DeactivatedPicture = "lamp_off";
        public static string ActivatedPicture = "lamp_on";

        #region PictureId

        private string _pictureId = DeactivatedPicture;
        public string PictureId
        {
            get => _pictureId;
            set
            {
                _pictureId = value;
                OnPropertyChanged("PictureId");
            }
        }

        #endregion

        #region Status

        public bool ProgressIndeterminate
        {
            get => _progressIndeterminate;
            set
            {
                _progressIndeterminate = value;
                OnPropertyChanged("ProgressIndeterminate");
            }
        }

        private int _progressValue;
        public int ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
                ProgressIndeterminate = ProgressValue < 1;
            }
        }

        private Visibility _progressVisible = Visibility.Hidden;
        public Visibility ProgressVisible
        {
            get => _progressVisible;
            set
            {
                _progressVisible = value;
                OnPropertyChanged("ProgressVisible");
            }
        }


        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        private SlotStatus _slotStatus;
        public SlotStatus SlotStatus
        {
            get => _slotStatus;
            set
            {
                _slotStatus = value;
                Status = EnumHelper.GetDescription(SlotStatus);
            }
        }

        #endregion

        #region Name

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool _isNameFocused;
        public bool IsNameFocused
        {
            get => _isNameFocused;
            set
            {
                _isNameFocused = value;
                OnPropertyChanged("IsNameFocused");
            }
        }

        #endregion

        #region Visibility

        public void Show()
        {
            Visibility = Visibility.Visible;

            IsNameFocused = false;
            IsNameFocused = true;
        }

        private Visibility _visibility = Visibility.Hidden;
        private bool _progressIndeterminate;

        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        #endregion

        #region Id

        public int Id { get; set; }

        #endregion

        #region ActiveId

        public int ActiveId
        {
            set => PictureId =
                Id == value
                    ? ActivatedPicture
                    : DeactivatedPicture;
        }

        #endregion
    }
}
