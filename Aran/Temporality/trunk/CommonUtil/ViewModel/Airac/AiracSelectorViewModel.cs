using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.ViewModel.Airac;
using MvvmCore;

namespace TOSSM.ViewModel.Control.Airac
{
    public class AiracSelectorViewModel : ViewModelBase
    {
        public AiracSelectorViewModel()
        {
            SelectedCycle = AiracList.First();
        }

        public static string AiracMessage(DateTime date)
        {
            var cycle = GetAiracCycleByDate(date);
            var airacMessage = (cycle > -1) ? "; AIRAC: " + cycle : "; custom AIRAC";
            return airacMessage;
        }

        public static DateTime GetAiracCycleByIndex(int index)
        {
            if (index<1408) throw new Exception("Can not set past date");
           //1408 cycle = 24 july 2014
           return new DateTime(2014,7,24,0,0,0).AddDays((index - 1408) * 28);
        }

        public static int GetAiracCycleByDate(DateTime dateTime)
        {
            var d = dateTime.Subtract(new DateTime(2014, 7, 24, 0, 0, 0));
            if (d.Seconds > 0) return -1;
            if (d.Minutes > 0) return -1;
            if (d.Hours > 0) return -1;

            if (d.Days%28 != 0) return -1;

            return 1408 + (d.Days / 28);
        }

        public static int GetCurrentAiracCycle()
        {
            var r=1408 + DateTime.Now.Subtract(new DateTime(2014, 7, 24, 0, 0, 0)).Days/28;
            return r < 1408 ? 1408 : r;
        }

        private List<AiracViewModel> _airacList;
        public List<AiracViewModel> AiracList
        {
            get
            {
                if (_airacList==null)
                {
                    _airacList = new List<AiracViewModel>();

                    var cycle = GetCurrentAiracCycle();

                    for (int i = 0; i < 12; i++)
                    {
                        _airacList.Add(new AiracViewModel { Index = (cycle + i) });
                    }


                    _airacList.Add(new AiracViewModel { IsSeparator = true });
                    _airacList.Add(new AiracViewModel { Index = -1 });
                }
              
            
                return _airacList;
            }
            set { _airacList = value; }
        }

        private AiracViewModel _selectedCycle;
        public AiracViewModel SelectedCycle
        {
            get { return _selectedCycle; }
            set
            {
                _selectedCycle = value;
                if (SelectedCycle!=null)
                {
                    if (SelectedCycle.Index>0)
                    {
                        SelectedDate = GetAiracCycleByIndex(SelectedCycle.Index);
                    }
                }
                OnPropertyChanged("CustomDateVisibility");
                OnPropertyChanged("SelectedCycle");
            }
        }

        public Visibility CustomDateVisibility
        {
            get
            {
                return SelectedCycle != null && SelectedCycle.Index == -1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public DateTime InitialDate
        {
            set
            {
                var cycle = GetAiracCycleByDate(value);
                //select cycle
                SelectedCycle = AiracList.FirstOrDefault(t => t.Index == cycle);
                if (SelectedCycle != null) return;
                //set custom
                SelectedCycle = AiracList.Last();
                OnPropertyChanged("CustomDateVisibility");
                SelectedDate = value;
            }
        }

        public Action SelectedDateChanged { get; set; }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
                if (SelectedDateChanged != null)
                {
                    SelectedDateChanged();
                }
            }
        }

        private RelayCommand _unlockCommand;
        public RelayCommand UnlockCommand
        {
            get
            {
                return _unlockCommand ?? (_unlockCommand=new RelayCommand(
                    t=>
                        {
                            if (CurrentDataContext.CurrentUser == null) return;
                            if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return;
                            InitialDate = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
                        },
                    t=>
                        {
                            if (CurrentDataContext.CurrentUser == null) return false;
                            if (CurrentDataContext.CurrentUser.ActivePrivateSlot == null) return false;

                            return CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate != SelectedDate;
                        }));
            }
            set { _unlockCommand = value; }
        }
    }
}
