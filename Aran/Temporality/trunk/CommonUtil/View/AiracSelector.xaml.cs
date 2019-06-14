using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.View
{
    public class AiracSelectorViewModel
    {
        public static string AiracMessage(DateTime date)
        {
            var cycle = AiracCycle.GetAiracCycleByStrictDate(date);
            var airacMessage = (cycle > -1) ? "; AIRAC: " + cycle : "; custom AIRAC";
            return airacMessage;
        }

    }

    /// <summary>
    /// Interaction logic for AiracSelector.xaml
    /// </summary>
    public partial class AiracSelector : UserControl
    {
        public static readonly DependencyProperty SelectedCycleProperty =
          DependencyProperty.Register("SelectedCycle",
          typeof(AiracViewModel),
          typeof(AiracSelector),
          new PropertyMetadata(default(AiracViewModel), OnCycleChanged));


        private static void OnCycleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = d as AiracSelector;
            if (selector != null && e.NewValue is AiracViewModel)
            {
                if (selector.SelectedCycle?.Index > 0)
                {
                    var newDate= AiracCycle.GetAiracCycleByIndex(selector.SelectedCycle.Index);
                    if (selector.SelectedDate != newDate)
                    {
                        selector.SelectedDate = newDate;
                    }
                }
                selector.CustomDatePicker.Visibility = selector.SelectedCycle != null && selector.SelectedCycle.Index == -1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

     

        public static readonly DependencyProperty SelectedDateProperty =
          DependencyProperty.Register("SelectedDate",
          typeof(DateTime),
          typeof(AiracSelector),
          new PropertyMetadata(DateTime.Now, OnDateChanged));

        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = d as AiracSelector;
            if (selector != null && e.NewValue is DateTime)
            {
                if (selector.SelectedCycle != null && selector.SelectedCycle.Index > 0)
                {
                    var date = AiracCycle.GetAiracCycleByIndex(selector.SelectedCycle.Index);
                    if (date != selector.SelectedDate)
                    {
                        var newCycle = AiracCycle.GetAiracCycleByStrictDate(selector.SelectedDate);
                        if (selector.SelectedCycle == null || selector.SelectedCycle.Index != newCycle)
                        {
                            var newModel=selector.AiracList.FirstOrDefault(t => t.Index == newCycle);
                            if (newModel!=null && newModel != selector.SelectedCycle)
                            {
                                selector.SelectedCycle = newModel;
                            }
                        }
                    }
                }
            }
        }
        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set
            {
                var oldDate = (DateTime)GetValue(SelectedDateProperty);
                var newDate = (DateTime)value;
                if (oldDate != newDate)
                {
                    SetValue(SelectedDateProperty, value);
                   // OnPropertyChanged(new DependencyPropertyChangedEventArgs(SelectedDateProperty, null, value));
                }
            }
        }

        public AiracViewModel SelectedCycle
        {
            get { return (AiracViewModel)GetValue(SelectedCycleProperty); }
            set
            {
                SetValue(SelectedCycleProperty, value);
               // OnPropertyChanged(new DependencyPropertyChangedEventArgs(SelectedCycleProperty, null, SelectedCycle));
            }
        }

        public AiracSelector()
        {
            InitializeComponent();
            SelectedCycle = AiracList.First();
            DataContext = this;
        }

      

        private List<AiracViewModel> _airacList;
        public List<AiracViewModel> AiracList
        {
            get
            {
                if (_airacList==null)
                {
                    _airacList = new List<AiracViewModel>();

                    var cycle = AiracCycle.GetPermittedAiracCycle();

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
                            SelectedDate = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
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

    public class AiracViewModel : ViewModelBase
    {
        public bool IsSeparator { get; set; }

        public int Index { get; set; }

        public override string DisplayName
        {
            get
            {
                if (Index > 0) return Index + " : " +
                                      $"{AiracCycle.GetAiracCycleByIndex(Index):yyyy/MM/dd HH:mm}" + " UTC";
                return Index == 0 ? "" : "Custom";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }
    }
}
