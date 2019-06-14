using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using NotamViewer.Converter;
using NotamViewer.View;

namespace NotamViewer.ViewModel
{
    public class NotamViewerViewModel : ViewModelBase
    {
        public NotamViewerViewModel()
        {
            TempChangeDates.Add(DateTime.Now.AddMonths(-9));
            PermChangeDates.Add(DateTime.Now.AddMonths(-8));

            ReloadData(); 
        }

        #region BlockerModel

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get { return _blockerModel??(_blockerModel=new BlockerModel()); }
            set { _blockerModel = value; }
        }

        #endregion

        #region Feature Data

        private List<SingleFeatureViewModel> _featureData;
        public List<SingleFeatureViewModel> FeatureData
        {
            get { return _featureData ?? (_featureData = new List<SingleFeatureViewModel>()); }
            set { _featureData = value; }
        }

        private void UpdateFeatureDataFiltered()
        {
            var featureDataFiltered = FeatureData.ToList();

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                (Action) (
                             () =>
                                 {
                                    
                                     foreach (var model in featureDataFiltered.Where(t => t.History != null))
                                     {
                                         model.History.UpdateLines();
                                     }
                                    
                                 }));

            FeatureDataFiltered = featureDataFiltered;
        }

        private List<SingleFeatureViewModel> _featureDataFiltered;
        public List<SingleFeatureViewModel> FeatureDataFiltered
        {
            get { return _featureDataFiltered; }
            set
            {
                _featureDataFiltered = value;
                OnPropertyChanged("FeatureDataFiltered");
            }
        }

        #endregion

        #region Time Interval


        private HashSet<DateTime> _tempChangeDates;
        private HashSet<DateTime> TempChangeDates
        {
            get { return _tempChangeDates ?? (_tempChangeDates = new HashSet<DateTime>()); }
        }

        private HashSet<DateTime> _permChangeDates;
        private HashSet<DateTime> PermChangeDates
        {
            get { return _permChangeDates ?? (_permChangeDates = new HashSet<DateTime>()); }
        }


        private HashSet<DateTime> _keyDates;
        public HashSet<DateTime> KeyDates
        {
            get { return _keyDates??(_keyDates=new HashSet<DateTime>()); }
        }

        private void ReloadData()
        {
            BlockerModel.Block();

          
            var eventMetaDatas = CurrentDataContext.CurrentService.GetChangesInInterval(
                          new FeatureId { FeatureTypeId = (int)FeatureType.Airspace }, StartDate, EndDate, false);

            FeatureData.Clear();
            TempChangeDates.Clear();
            PermChangeDates.Clear();

            var featureIds = eventMetaDatas.Select(t => t.Guid).Distinct().ToList();
            foreach (var featureId in featureIds)
            {

                //get first event with it
                var correspondingEvents = eventMetaDatas.Where(t => t.Guid == featureId).OrderBy(t=>t.TimeSlice.BeginPosition).ToList();
                var firstEvent = correspondingEvents.First();



				var correspondingState = CurrentDataContext.CurrentService.GetActualDataByDate ( new FeatureId ( firstEvent )
																	  {
																		  WorkPackage = 0
																	  }, false, firstEvent.TimeSlice.BeginPosition );

                var singleFeatureViewModel = new SingleFeatureViewModel
                {
                    Description = HumanReadableConverter.ShortAimDescription(correspondingState.First().Data.Feature),
                    FeatureType = (FeatureType)firstEvent.FeatureTypeId,
                    History = new SingleFeatureHistoryViewModel(this)
                    {
                        StartDate = StartDate,
                        EndDate = EndDate,
                        TempChangeDates = TempChangeDates,
                        PermChangeDates = PermChangeDates,
                        Identifier = (Guid)firstEvent.Guid,
                        FeatureType = (FeatureType)firstEvent.FeatureTypeId,
                    }
                };
               
                var changes = new List<SingleChangeViewModel>();
                foreach (var myEvent in correspondingEvents)
                {
                    if (myEvent.Interpretation == Interpretation.TempDelta)
                    {
                        TempChangeDates.Add(myEvent.TimeSlice.BeginPosition);
                        if (myEvent.TimeSlice.EndPosition!=null)
                        {
                            TempChangeDates.Add((DateTime)myEvent.TimeSlice.EndPosition);
                        }
                        changes.Add(new SingleChangeViewModel(singleFeatureViewModel.History)
                        {
                            RowPosition = 0,
                            Change = myEvent.Data.Feature,
                            ControlBackground = Config.TemporaryChangeBrush,
                            StartDate = myEvent.TimeSlice.BeginPosition,
                            EndDate = myEvent.TimeSlice.EndPosition ?? EndDate,
                            ControlMargin = Config.DayInPixel * (myEvent.TimeSlice.BeginPosition - StartDate).TotalDays
                        });
                    }
                    else
                    {
                        PermChangeDates.Add(myEvent.TimeSlice.BeginPosition);
                        if (myEvent.TimeSlice.EndPosition != null)
                        {
                            PermChangeDates.Add((DateTime)myEvent.TimeSlice.EndPosition);
                        }
                        changes.Add(new SingleChangeViewModel(singleFeatureViewModel.History)
                        {
                            Change = myEvent.Data.Feature,
                            RowPosition = 1,
                            ControlBackground = Config.PermanentChangeBrush,
                            StartDate = myEvent.TimeSlice.BeginPosition,
                            EndDate = myEvent.TimeSlice.EndPosition ?? EndDate,
                            ControlMargin = Config.DayInPixel * (myEvent.TimeSlice.BeginPosition - StartDate).TotalDays
                        });
                    }
                    
                }

                singleFeatureViewModel.History.Changes = changes;
                FeatureData.Add(singleFeatureViewModel);
            }

            KeyDates.Clear();
            foreach (var date in PermChangeDates)
            {
                KeyDates.Add(date);
            }
            foreach (var date in TempChangeDates)
            {
                KeyDates.Add(date);
            }

            UpdateFeatureDataFiltered();

            BlockerModel.Unblock();
        }


        private DateTime _startDate = DateTime.Now.AddMonths(-5);
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate==value) return;
                _startDate = value;
                OnPropertyChanged("StartDate");
                ReloadData();
            }
        }


        private DateTime _endDate=DateTime.Now.AddMonths(1);
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                OnPropertyChanged("EndDate");
                ReloadData();
            }
        }

        private RelayCommand _onMapCommand;
        public RelayCommand OnMapCommand
        {
            get { return _onMapCommand; }
            set
            {
                _onMapCommand = value;
                OnPropertyChanged("OnMapCommand");
            }
        }

        private Visibility _progressVisible=Visibility.Hidden;
        public Visibility ProgressVisible
        {
            get { return _progressVisible; }
            set
            {
                _progressVisible = value;
                OnPropertyChanged("ProgressVisible");
            }
        }

        private int _progressMinimum;
        public int ProgressMinimum
        {
            get { return _progressMinimum; }
            set
            {
                _progressMinimum = value;
                OnPropertyChanged("ProgressMinimum");
            }
        }

        private int _progressMaximum;
        public int ProgressMaximum
        {
            get { return _progressMaximum; }
            set
            {
                _progressMaximum = value;
                OnPropertyChanged("ProgressMaximum");
            }
        }

        private int _progressValue;
        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        public static string []IgnoredProperties=new string[]
                                                     {
                                                         "Id",
                                                         "WorksPackageId",
                                                         "TimeSlice",
                                                         "Identifier",
                                                         "FeatureType"
                                                     };

        private SingleFeatureHistoryViewModel _selectedChild;
        public SingleFeatureHistoryViewModel SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                foreach (var viewModel in FeatureData.Select(t=>t.History).Except(new[] { SelectedChild }))
                {
                    viewModel.Deselect();
                }


              
                var change = SelectedChild.SelectedChild.Change;
                if (change == null)
                {
                    //init change here
                    var id = SelectedChild.Identifier;
                    var featureType = SelectedChild.FeatureType;
                    //get change from id and featureType
                }

                if (change==null)
                {
                    SelectedChild.SelectedChild.ChangeList = new List<ChangeViewModel>();
                    return;
                }

                //prepare change data
                var changeList = new List<ChangeViewModel>();
                foreach (var prop in change.GetType().GetProperties())
                {
                    if (IgnoredProperties.Contains(prop.Name)) continue;
                    var val = prop.GetValue(change, null);
                    if ((val != null && !(val is IList)) || (val is IList && (val as IList).Count>0))
                    {
                        changeList.Add(new ChangeViewModel
                                           {
                                               ChangedProperty = prop .Name,
                                               NewStringValue = HumanReadableConverter.ToHuman(val)
                                           });
                    }
                }
                SelectedChild.SelectedChild.ChangeList = changeList.OrderBy(t=>t.ChangedProperty).ToList();
            }
        }

        #endregion

        public bool IsOnCommandEnable()
        {
            return true;
        }
    }
}
