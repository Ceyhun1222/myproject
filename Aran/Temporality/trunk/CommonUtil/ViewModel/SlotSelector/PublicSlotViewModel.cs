using System;
using System.Windows;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;
using MvvmCore;
using TOSSM.ViewModel.Control.SlotSelector;

namespace Aran.Temporality.CommonUtil.ViewModel.SlotSelector
{

    public class PublicSlotViewModel : SlotViewModel
    {
        #region Ctor

        public PublicSlot Slot()
        {
            return new PublicSlot
                       {
                           Name = Name,
                           EffectiveDate = ActualDate,
                           EndEffectiveDate = ActualDate2,
                           PlannedCommitDate = PlannedCommitDate,
                           Status = SlotStatus,
                           Id = Id,
                           SlotType = SlotType
                       };
        }

        public void InitFromSlot(PublicSlot publicSlot)
        {
            Name = publicSlot.Name;
            PlannedCommitDate = publicSlot.PlannedCommitDate;
            SlotStatus = publicSlot.Status;
            Id = publicSlot.Id;
            SlotType = publicSlot.SlotType;

            ActualDate = publicSlot.EffectiveDate;
            ActualDate2 = publicSlot.EndEffectiveDate;
            

            var publicSlotType = (PublicSlotType)SlotType;
            if (publicSlotType == PublicSlotType.Mixed)
            {
                EffectiveDate = null;
                EffectiveDate2 = null;
            }
            else if (publicSlotType == PublicSlotType.PermanentDelta)
            {
                EffectiveDate2 = null;
            }
            
        }


        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        public PublicSlotViewModel(PublicSlot publicSlot)
        {
            InitFromSlot(publicSlot);
        }

        public PublicSlotViewModel()
        {
        }

        #endregion

        #region Actual Date

        private DateTime _actualDate;
        public DateTime ActualDate
        {
            get => _actualDate;
            set
            {
                _actualDate = value;
                var cycle= AiracCycle.GetAiracCycleByStrictDate(ActualDate);
                Airac = cycle>-1 ? cycle.ToString() : "Custom";
                EffectiveDate = ActualDate.ToString("yyyy/MM/dd HH:mm")+" UTC";
            }
        }

        private DateTime _actualDate2;
        public DateTime ActualDate2
        {
            get => _actualDate2;
            set
            {
                _actualDate2 = value;
                var cycle2 = AiracCycle.GetAiracCycleByStrictDate(ActualDate2);
                Airac2 = cycle2 > -1 ? cycle2.ToString() : "Custom";
                EffectiveDate2 = ActualDate2.ToString("yyyy/MM/dd HH:mm") + " UTC";
            }
        }

        #endregion

        #region EffectiveDate

        private string _effectiveDate;
        public string EffectiveDate
        {
            get => _effectiveDate;
            set
            {
                _effectiveDate = value;
                OnPropertyChanged("EffectiveDate");
            }
        }

        private string _effectiveDate2;
        public string EffectiveDate2
        {
            get => _effectiveDate2;
            set
            {
                _effectiveDate2 = value;
                OnPropertyChanged("EffectiveDate2");
            }
        }
        
        #endregion 

        #region PlannedCommitDate

        private string _plannedCommitString;
        public string PlannedCommitString
        {
            get => _plannedCommitString;
            set
            {
                _plannedCommitString = value;
                OnPropertyChanged("PlannedCommitString");
            }
        }


        private DateTime _plannedCommitDate;
        public DateTime PlannedCommitDate
        {
            get => _plannedCommitDate;
            set
            {
                _plannedCommitDate = value;
                OnPropertyChanged("PlannedCommitDate");
                PlannedCommitString = PlannedCommitDate.ToString("yyyy/MM/dd HH:mm");
            }
        }
        
        #endregion 

        #region Airac

        private string _airac;
        public string Airac
        {
            get => _airac;
            set
            {
                _airac = value;
                OnPropertyChanged("Airac");
            }
        }

        private string _airac2;
        public string Airac2
        {
            get => _airac2;
            set
            {
                _airac2 = value;
                OnPropertyChanged("Airac2");
            }
        }
        
        #endregion 

        #region Commands

        public Action OkAction { get; set; }
        public Action CancelAction { get; set; }

        private int _slotType;
        public int SlotType
        {
            get => _slotType;
            set
            {
                _slotType = value;
                OnPropertyChanged("SlotType");
                Type = EnumHelper.GetDescription((PublicSlotType) SlotType);
            }
        }

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

                            OkAction?.Invoke();
                        },
                        t => !string.IsNullOrWhiteSpace(Name)));
            }
            set => _okCommand = value;
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                    new RelayCommand(
                        t =>
                        {
                            Visibility = Visibility.Hidden;

                            CancelAction?.Invoke();
                        }));
            }
            set => _cancelCommand = value;
        }
        #endregion

        #region AiracSelectorEnabled

        private bool _airacSelectorEnabled;
        public bool AiracSelectorEnabled
        {
            get => _airacSelectorEnabled;
            set
            {
                _airacSelectorEnabled = value;
                OnPropertyChanged("AiracSelectorEnabled");
            }
        }


        private bool _airacSelectorEnabled2;
        public bool AiracSelectorEnabled2
        {
            get => _airacSelectorEnabled2;
            set
            {
                _airacSelectorEnabled2 = value;
                OnPropertyChanged("AiracSelectorEnabled2");
            }
        }

        #endregion

        #region PlannedDateTimeSelectorEnabled

        private bool _plannedDateTimeSelectorEnabled;
        private string _type;

        public bool PlannedDateTimeSelectorEnabled
        {
            get => _plannedDateTimeSelectorEnabled;
            set
            {
                _plannedDateTimeSelectorEnabled = value;
                OnPropertyChanged("PlannedDateTimeSelectorEnabled");
            }
        }

        public bool Frozen => SlotStatus == SlotStatus.Published || SlotStatus == SlotStatus.Publishing || SlotStatus == SlotStatus.ToBePublished || SlotStatus == SlotStatus.Expired;

        #endregion
    }
}
