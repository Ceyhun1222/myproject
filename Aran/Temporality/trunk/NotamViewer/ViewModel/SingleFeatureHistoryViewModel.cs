using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.Features;
using MvvmCore;

namespace NotamViewer.ViewModel
{
    public class SingleFeatureHistoryViewModel : ViewModelBase
    {
        public SingleFeatureHistoryViewModel(NotamViewerViewModel parent)
        {
            NotamViewerViewModel = parent;
        }

        public NotamViewerViewModel NotamViewerViewModel { get; set; }
        
        private IEnumerable<DateTime> GetAiracCycles()
        {
            if (StartDate >= EndDate) return new List<DateTime>();

            var defenetlyAirac = new DateTime(2014, 7, 24, 0, 0, 0);
            while (defenetlyAirac >= StartDate)
            {
                defenetlyAirac = defenetlyAirac.AddDays(-28);
            }

            while (defenetlyAirac<StartDate)
            {
                defenetlyAirac=defenetlyAirac.AddDays(28);
            }

            var result = new List<DateTime>();
            while (defenetlyAirac < EndDate)
            {
                result.Add(defenetlyAirac);
                defenetlyAirac = defenetlyAirac.AddDays(28);
            }
           
            return result;
        }


        public Guid Identifier { get; set; }
        public FeatureType FeatureType { get; set; }

        public void UpdateLines()
        {
            Application.Current.Dispatcher.Invoke(
                 DispatcherPriority.Background,
                 (Action)(
                              () =>
                              {
                                  //add airac dates
                                  var list = (from airacDate in GetAiracCycles().Except(PermChangeDates).Except(TempChangeDates)
                                              let x = Config.DayInPixel * (airacDate - StartDate).TotalDays
                                              select new Line
                                              {
                                                  X1 = x,
                                                  Y1 = 0,
                                                  X2 = x,
                                                  Y2 = Config.DefaultRowHeight,
                                                  HorizontalAlignment = HorizontalAlignment.Left,
                                                  VerticalAlignment = VerticalAlignment.Center,
                                                  StrokeThickness = Config.DefaultStrokeThickness,
                                                  Stroke = Config.AiracBrush,
                                                  ToolTip = "AIRAC cycle " + airacDate.ToString("dd/MM/yyyy"),
                                                  StrokeDashArray = new DoubleCollection(new double[] { 6, 2})
                                              }).ToList();

                                  list.AddRange(from date in PermChangeDates
                                                let x = Config.DayInPixel * (date - StartDate).TotalDays
                                             select new Line
                                  {
                                      X1 = x,
                                      Y1 = 0,
                                      X2 = x,
                                      Y2 = Config.DefaultRowHeight,
                                      HorizontalAlignment = HorizontalAlignment.Left,
                                      VerticalAlignment = VerticalAlignment.Center,
                                      StrokeThickness = Config.DefaultStrokeThickness,
                                      Stroke = Config.PermBrush,
                                      ToolTip = "Permanent change " + date.ToString("dd/MM/yyyy"),
                                      StrokeDashArray = new DoubleCollection(new double[] { 6, 2 })
                                  });

                                  list.AddRange(from date in TempChangeDates
                                                let x = Config.DayInPixel * (date - StartDate).TotalDays
                                                select new Line
                                                {
                                                    X1 = x,
                                                    Y1 = 0,
                                                    X2 = x,
                                                    Y2 = Config.DefaultRowHeight,
                                                    HorizontalAlignment = HorizontalAlignment.Left,
                                                    VerticalAlignment = VerticalAlignment.Center,
                                                    StrokeThickness = Config.DefaultStrokeThickness,
                                                    Stroke = Config.TempBrush,
                                                    ToolTip = "Temporary change " + date.ToString("dd/MM/yyyy"),
                                                    StrokeDashArray = new DoubleCollection(new double[] { 3, 3 })
                                                });
                                     
                                  KeyDates = list;
                              }));


        }

        private void RecalculateSize()
        {
            if (EndDate <= StartDate) return;
            ControlWidth = Config.DayInPixel * (EndDate - StartDate).TotalDays;
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
                RecalculateSize();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
                RecalculateSize();
            }
        }

        private double _controlWidth;
        public double ControlWidth
        {
            get { return _controlWidth; }
            set
            {
                _controlWidth = value;
                OnPropertyChanged("ControlWidth");
            }
        }

        private List<Line> _keyDates;
        public List<Line> KeyDates
        {
            get { return _keyDates; }
            set
            {
                _keyDates = value;
                OnPropertyChanged("KeyDates");
            }
        }

        private List<SingleChangeViewModel> _changes;
        public List<SingleChangeViewModel> Changes
        {
            get { return _changes; }
            set
            {
                _changes = value;
                OnPropertyChanged("Changes");
            }
        }

        private HashSet<DateTime> _tempChangeDates;
        public HashSet<DateTime> TempChangeDates
        {
            get
            {
                return _tempChangeDates ?? (_tempChangeDates = new HashSet<DateTime>());
            }
            set { _tempChangeDates = value; }
        }

        private HashSet<DateTime> _permChangeDates;
        public HashSet<DateTime> PermChangeDates
        {
            get { return _permChangeDates ?? (_permChangeDates = new HashSet<DateTime>()); }
            set { _permChangeDates = value; }
        }

        private SingleChangeViewModel _selectedChild;
        public SingleChangeViewModel SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                foreach (var viewModel in Changes.Except(new []{SelectedChild}))
                {
                    viewModel.PopupIsOpen = false;
                }

                NotamViewerViewModel.SelectedChild = this;
            }
        }

        public void Deselect()
        {
             foreach (var viewModel in Changes)
             {
                 viewModel.PopupIsOpen = false;
             }
        }
    }
}
