using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Context;
using ImpromptuInterface.Dynamic;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.View
{
    /// <summary>
    /// Interaction logic for AiracSelector.xaml
    /// </summary>
    public partial class AiracSelectorAlternative : UserControl
    {
        public static readonly DependencyProperty SelectedCycleProperty =
            DependencyProperty.Register("SelectedCycle",
                typeof(AiracViewModel),
                typeof(AiracSelectorAlternative),
                new PropertyMetadata(default(AiracViewModel), OnCycleChanged));


        private static void OnCycleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = d as AiracSelectorAlternative;
            if (selector != null && e.NewValue is AiracViewModel)
            {
                if (selector.SelectedCycle?.Index > 0)
                {
                    var newDate = AiracCycle.GetAiracCycleByIndex(selector.SelectedCycle.Index);
                    if (selector.SelectedDate != newDate)
                    {
                        selector.SelectedDate = newDate;
                    }
                }
                selector.CustomDatePicker.Visibility = selector.SelectedCycle != null &&
                                                       selector.SelectedCycle.Index == -1
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }



        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate",
                typeof(DateTime),
                typeof(AiracSelectorAlternative),
                new PropertyMetadata(DateTime.Now, OnDateChanged));

        private static void OnDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = d as AiracSelectorAlternative;
            if (selector != null && e.NewValue is DateTime)
            {
                if (selector.SelectedCycle != null && selector.SelectedCycle.Index > 0)
                {
                    var date = AiracCycle.GetAiracCycleByIndex(selector.SelectedCycle.Index);
                    if (date != selector.SelectedDate)
                    {
                        var newCycle = AiracCycle.GetAiracCycleByDate(selector.SelectedDate);
                        if (selector.SelectedCycle == null || selector.SelectedCycle.Index != newCycle)
                        {
                            var newModel = selector.AiracList.FirstOrDefault(t => t.Index == newCycle);
                            if (newModel != null && newModel != selector.SelectedCycle)
                            {
                                selector.SelectedCycle = newModel;
                            }
                        }
                    }
                }
                else if ((selector.StartDate != default(DateTime) && selector.StartDate.CompareTo(e.NewValue) >= 0)
                    || !AiracCycle.CanCreateCycle((DateTime)e.NewValue, ((AiracSelectorAlternative)d).InterpretationType == 0))
                    d.SetValue(e.Property, e.OldValue);
            }
        }

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set
            {
                DateTime oldDate = (DateTime)GetValue(SelectedDateProperty);
                DateTime newDate = (DateTime)value;

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
                OnPropertyChanged(new DependencyPropertyChangedEventArgs(SelectedCycleProperty, null, SelectedCycle));
            }
        }

        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(
            "StartDate", typeof(DateTime), typeof(AiracSelectorAlternative), new PropertyMetadata(default(DateTime), OnStartDateChanged));

        private static void OnStartDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue != e.OldValue)
                (d as AiracSelectorAlternative)?.Refresh();
        }

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }


        public static readonly DependencyProperty InterpretationTypeProperty = DependencyProperty.Register(
            "InterpretationType", typeof(int), typeof(AiracSelectorAlternative), new PropertyMetadata(default(int), OnInterpretationTypeChanged));


        private static void OnInterpretationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                (d as AiracSelectorAlternative)?.Refresh();
            }

        }

        public int InterpretationType
        {
            get { return (int)GetValue(InterpretationTypeProperty); }
            set { SetValue(InterpretationTypeProperty, value); }
        }

        public AiracSelectorAlternative()
        {
            InitializeComponent();
            DataContext = this;
            Init();
        }

        private void Init()
        {
            SelectedCycle = AiracList.First();
            var date = AiracCycle.GetAiracCycleByIndex(SelectedCycle.Index);
            OnDateChanged(this, new DependencyPropertyChangedEventArgs(SelectedDateProperty, null, date));
        }

        private void Refresh()
        {
            RefreshAiracList();
            Init();
        }

        private ObservableCollection<AiracViewModel> _airacList;

        public ObservableCollection<AiracViewModel> AiracList
        {
            get
            {
                if (_airacList == null)
                {
                    _airacList = new ObservableCollection<AiracViewModel>();

                    RefreshAiracList();
                }


                return _airacList;
            }
            set { _airacList = value; }
        }

        private void RefreshAiracList()
        {
            _airacList.Clear();
            var cycle = -1;

            if (StartDate != default(DateTime))
                cycle = AiracCycle.GetAiracCycleByDate(StartDate) + 1;
            else
                cycle = InterpretationType == 0
                ? AiracCycle.GetPermittedAiracCycle()
                : AiracCycle.GetNextAiracCycle();

            for (var i = 0; i < 12; i++)
            {
                _airacList.Add(new AiracViewModel { Index = (cycle + i) });
            }


            _airacList.Add(new AiracViewModel { IsSeparator = true });
            _airacList.Add(new AiracViewModel { Index = -1 });
        }
    }
}
