using System;
using System.Windows;
using Aran.Temporality.Common.Entity;
using MvvmCore;

namespace TOSSM.ViewModel.Control.SlotSelector
{
    public class PrivateSlotViewModel : SlotViewModel
    {
        #region Ctor

        public PrivateSlot Slot()
        {
            return new PrivateSlot
                       {
                            Id=Id,
                            Status = SlotStatus,
                            Name = Name,
                            Reason = Reason,
                            StatusChangeDate = StatusChangedDate,
                            CreationDate = CreationDate
                       };
        }

        public PrivateSlotViewModel()
        {
        }

        public void InitFromSlot(PrivateSlot privateSlot)
        {
            SlotStatus = privateSlot.Status;
            Name = privateSlot.Name;
            Reason = privateSlot.Reason;
            StatusChangedDate = privateSlot.StatusChangeDate;
            CreationDate = privateSlot.CreationDate;
            Id = privateSlot.Id;
            Type=privateSlot.OriginatorSlotId==-1?"Project Based":"Operational";
        }

        public PrivateSlotViewModel(PrivateSlot privateSlot)
        {
            InitFromSlot(privateSlot);
        }

        #endregion

        #region Features

        private string _features;
        public string Features
        {
            get { return _features; }
            set
            {
                _features = value;
                OnPropertyChanged("Features");
            }
        }

        #endregion 

        #region Reason

        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set
            {
                _reason = value;
                OnPropertyChanged("Reason");
            }
        }

        #endregion

        #region Date

        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set
            {
                _creationDate = value;
                if (_creationDate != DateTime.MinValue)
                    CreationString = CreationDate.ToString("yyyy/MM/dd HH:mm");
            }
        }

        private DateTime _statusChangedDate;
        public DateTime StatusChangedDate
        {
            get { return _statusChangedDate; }
            set
            {
                _statusChangedDate = value;
                StatusChangedString = StatusChangedDate.ToString("yyyy/MM/dd HH:mm");
            }
        }

        #endregion

        #region DateString

        private string _creationString;
        public string CreationString
        {
            get { return _creationString; }
            set
            {
                _creationString = value;
                OnPropertyChanged("CreationString");
            }
        }

        private string _statusChangedString;
        public string StatusChangedString
        {
            get { return _statusChangedString; }
            set
            {
                _statusChangedString = value;
                OnPropertyChanged("StatusChangedString");
            }
        }

        #endregion

        #region Commands

        public Action OkAction { get; set; }
        public Action CancelAction { get; set; }

    

        private RelayCommand _okCommand;
        public RelayCommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand =
                    new RelayCommand(
                        t =>
                        {
                            Visibility = Visibility.Hidden;

                            if (OkAction!=null)
                            {
                                OkAction();
                            }
                        },
                        t => !string.IsNullOrWhiteSpace(Name)));
            }
            set { _okCommand = value; }
        }

        private RelayCommand _cancelCommand;
        private string _type;

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                    new RelayCommand(
                        t =>
                            {
                                Visibility = Visibility.Hidden;

                                if (CancelAction != null)
                                {
                                    CancelAction();
                                }
                            }));
            }
            set { _cancelCommand = value; }
        }

        #endregion

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }
    }
}
