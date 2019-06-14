using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using TOSSM.ViewModel.Control.PropertySelector.Menu;
using TOSSM.ViewModel.Document.Slot.Problems.Single;
using TOSSM.ViewModel.Pane;

namespace TOSSM.ViewModel.Document.Slot.Problems.ProblemList
{

    public class BusinessRulesCategoryViewModel : ProblemListViewModel, IChangedHandler<PropertySetMenuItemViewModel>, IChangedHandler<PropertyMenuItemViewModel>
    {
        public BusinessRulesCategoryViewModel(bool hasCount=false)
        {
            var filtererThread = new Thread(FilterFunction)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true,
            };

            filtererThread.Start();


            DataGridVisibility = Visibility.Visible;

            if (hasCount) ShowCountColumn = true;
            InitColumns();
            InitContextMenuForColumns();
        }

        public int RuleCount { get { return Rules.Count(t => t.IsActive); } }

        #region Filterer logic

        public bool IsTerminated { get; set; }

        public void FilterFunction()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            while (!IsTerminated)
            {
                Thread.Sleep(200);
                if (!FilterChanged) continue;

                UpdateRulesFiltered();
            }
        }

        public void UpdateRulesFiltered()
        {
            BlockerModel.Block();

            OnPropertyChanged("RuleCount");
            

            FilterChanged = false;

            var result = new List<RuleViewModel>();
            var autocomplete = new HashSet<string>();

            if (Rules != null)
            {
                foreach (var rule in Rules)
                {
                    if (FilterChanged) return;

                    if (String.IsNullOrWhiteSpace(PropertyFilter))
                    {
                        result.Add(rule);
                    }
                    else
                    {
                        var found = false;
                        if (PropertyFilter.StartsWith("="))
                        {
                            var equalString = PropertyFilter.Substring(1, PropertyFilter.Length - 1);
                            foreach (var t in rule.GetDescriptions().Where(t => t != null && t.Equals(equalString)))
                            {
                                if (FilterChanged) return;

                                autocomplete.Add(t);
                                found = true;
                            }
                        }
                        else
                        {
                            foreach (
                                var t in
                                    rule.GetDescriptions().Where(
                                        t => t != null && t.ToLower().Contains(PropertyFilter.ToLower())))
                            {
                                if (FilterChanged) return;

                                autocomplete.Add(t);
                                found = true;
                            }
                        }


                        if (found)
                        {
                            result.Add(rule);
                        }

                    }
                }
            }


            AutoCompleteList = autocomplete;


            DataGridVisibility = (Rules != null && result.Count > 0)
                                                          ? Visibility.Visible
                                                          : Visibility.Collapsed;

            Status = (Rules != null && result.Count > 0) ? "Displayed " + result.Count + " items from " + Rules.Count : "";
      

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                (Action)(() =>
                {
                    RulesFiltered = new List<RuleViewModel>(result);

                    OnPropertyChanged("RulesFiltered");

                }));



            BlockerModel.Unblock();
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

        #endregion

        #region Other UI properties

        private BlockerModel _blockerModel;
        public BlockerModel BlockerModel
        {
            get => _blockerModel ?? (_blockerModel = new BlockerModel());
            set => _blockerModel = value;
        }
        private HashSet<string> _autoCompleteList;
        public HashSet<String> AutoCompleteList
        {
            get => _autoCompleteList;
            set
            {
                _autoCompleteList = value;
                OnPropertyChanged("AutoCompleteList");
            }
        }

        private Visibility _dataGridVisibility = Visibility.Collapsed;
        public Visibility DataGridVisibility
        {
            get => _dataGridVisibility;
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged("DataGridVisibility");
            }
        }



        #endregion

        public string Dummy { get; set; }

        public void OnChanged(RuleViewModel rule)
        {
            //OnPropertyChanged("Dummy");
            //CommandManager.InvalidateRequerySuggested();
        }

        public RuleViewModel SelectedRule { get; set; }

        private List<RuleViewModel> _rules;
        public List<RuleViewModel> Rules
        {
            get => _rules ?? (_rules = new List<RuleViewModel>());
            set => _rules = value;
        }

		private Visibility _dockPnlVisible;
		public Visibility DockPanelVisible
		{
			get => _dockPnlVisible;
		    set { _dockPnlVisible = value; OnPropertyChanged ( "DockPanelVisible" ); }
		}
		
        private List<RuleViewModel> _rulesFiltered;
        public List<RuleViewModel> RulesFiltered
        {
            get => _rulesFiltered;
            set
            {
                _rulesFiltered = value;
                OnPropertyChanged("RulesFiltered");
            }
        }

        #region Filterer

        public bool FilterChanged;

        private string _propertyFilter;
        public string PropertyFilter
        {
            get => _propertyFilter;
            set
            {
                _propertyFilter = value;
                OnPropertyChanged("PropertyFilter");
                OnPropertyChanged("FilterEmptyButtonVisibility");
                FilterChanged = true;
            }
        }

        private RelayCommand _onClearFilter;
        public RelayCommand OnClearFilter
        {
            get
            {
                return _onClearFilter ??
                       (_onClearFilter = new RelayCommand(t1 => PropertyFilter = "", t2 => !String.IsNullOrEmpty(PropertyFilter)));
            }
            set => _onClearFilter = value;
        }

        public Visibility FilterEmptyButtonVisibility => String.IsNullOrEmpty(PropertyFilter) ? Visibility.Collapsed : Visibility.Visible;

        #endregion

        #region Commands

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                       (_cancelCommand = new RelayCommand(
                           t =>
                           {
                               foreach (var rule in Rules)
                               {
                                   rule.CancelChange();
                               }

                               Application.Current.Dispatcher.Invoke(
                                                    DispatcherPriority.Background,
                                                    (Action)(CommandManager.InvalidateRequerySuggested));
                           },
                           t => Rules.ToList().Any(t1=>t1.HasChange())));
            }
            set => _cancelCommand = value;
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                       (_saveCommand = new RelayCommand(
                           t => BlockerModel.BlockForAction(
                               ()=>
                                   {
                                       foreach (var rule in Rules.Where(rule => rule.HasChange()))
                                       {
                                           CurrentDataContext.CurrentNoAixmDataService.ActivateRule(rule.RuleEntityId, rule.IsActive);
                                           rule.Util.IsActive = rule.IsActive;
                                       }

                                       Application.Current.Dispatcher.Invoke(
                                                    DispatcherPriority.Background,
                                                    (Action)(CommandManager.InvalidateRequerySuggested));

                                       MainManagerModel.Instance.OnSaveBusinessRules();

                                       OnPropertyChanged("RuleCount");
                                   }),
                           t => Rules.ToList().Any(t1 => t1.HasChange())));
            }
            set => _saveCommand = value;
        }

        private RelayCommand _activateCommand;
        public RelayCommand ActivateCommand
        {
            get
            {
                return _activateCommand ??
                       (_activateCommand = new RelayCommand(
                           t =>
                           {
                               var list = t as IList;
                               if (list == null) return;
                               foreach (var rule in list.OfType<RuleViewModel>())
                               {
                                   rule.IsActive = true;
                               }
                           },
                           t => SelectedRule != null));
            }
            set => _activateCommand = value;
        }

        private RelayCommand _deactivateCommand;
        public RelayCommand DeactivateCommand
        {
            get
            {
                return _deactivateCommand ??
                       (_deactivateCommand = new RelayCommand(
                           t =>
                           {
                               var list = t as IList;
                               if (list == null) return;
                               foreach (var rule in list.OfType<RuleViewModel>())
                               {
                                   rule.IsActive = false;
                               }
                           },
                           t => SelectedRule != null));
            }
            set => _deactivateCommand = value;
        }
        
        #endregion

        #region Rule Columns

        public void AddColumn(string header)
        {
			RuleColumnList.Add ( CreateColumn ( new KeyValuePair<string, string> ( header, header ) ) );
        }

        public void RemoveColumn(string header)
        {
            var column = RuleColumnList.FirstOrDefault(t => t.Header.ToString() == header);
            if (column != null)
            {
                RuleColumnList.Remove(column);
            }
        }

        private Dictionary<string,string> DisplayingRuleProperties()
        {
            var result = new List<ColumnModel>();

            switch (PropertySet)
            {
                case ColumnSet.All://all
						result.AddRange ( UiHelperMetadata.AllRulesColumns );
					
                    break;
                case ColumnSet.Custom://custom
						result.AddRange ( UiHelperMetadata.AllRulesColumns.Where ( prop => prop.CustomVisible ) );
                    break;
            }

            if (ShowCountColumn)
            {
                 result.Add(new ColumnModel{CustomVisible = true,Name = "Count",Position = -1});
            }

			return result.OrderBy ( t => t.Position ).Select ( item => new { name = item.Name, header = item.Header } ).ToDictionary ( item => item.name, item => item.header);
        }

        public bool ShowCountColumn { get; set; }

        private void InitColumns()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
            (Action)(() =>
            {
                RuleColumnList.Clear();

                foreach (var prop in DisplayingRuleProperties())
                {
                    RuleColumnList.Add(CreateColumn(prop));
                }
            }));
        }

        private static ColumnSet _initialColumnSet = ColumnSet.Custom;
        public static ColumnSet InitialColumnSet
        {
            get => _initialColumnSet;
            set => _initialColumnSet = value;
        }

        private ColumnSet _propertySet = InitialColumnSet;
        public ColumnSet PropertySet
        {
            get => _propertySet;
            set
            {
                if (_propertySet == value) return;
                _propertySet = value;
                InitColumns();
                SelectedColumnSetName = PropertySet.ToString();
                InitialColumnSet = PropertySet;
            }
        }

        private string _selectedColumnSetName = InitialColumnSet.ToString();
        public string SelectedColumnSetName
        {
            get => _selectedColumnSetName;
            set
            {
                _selectedColumnSetName = value;
                OnPropertyChanged("SelectedColumnSetName");
            }
        }

        private static DataGridColumn CreateColumn(KeyValuePair<string,string> keyValue)
        {
            if (keyValue.Key == "IsActive")
            {
                // create checkBox
                var checkBox = new FrameworkElementFactory(typeof (CheckBox));

                checkBox.SetValue(FrameworkElement.MarginProperty, new Thickness(1, 0, 1, 0));

                checkBox.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
                checkBox.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);


                checkBox.SetBinding(ToggleButton.IsCheckedProperty, new Binding
                                                             {
                                                                 Path = new PropertyPath(keyValue.Key),
                                                                 Mode = BindingMode.TwoWay,
                                                                 UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                                             });

                var column = new DataGridTemplateColumn
                                 {
                                     Header = "Active",
                                     CanUserSort = true,
                                     SortMemberPath = keyValue.Key,
                                     CellTemplate = new DataTemplate
                                                        {
                                                            VisualTree = checkBox
                                                        }
                                 };


                return column;
            }
            else
            {
                // create text
                var label = new FrameworkElementFactory(typeof (TextBlock));

                label.SetValue(FrameworkElement.MarginProperty, new Thickness(5, 0, 5, 0));

                label.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);

                label.SetBinding(TextBlock.TextProperty, new Binding
                                                             {
                                                                 Path = new PropertyPath(keyValue.Key),
                                                                 Mode = BindingMode.OneTime
                                                             });

                var column = new DataGridTemplateColumn
                                 {
                                     Header = keyValue.Value,
                                     CanUserSort = true,
                                     SortMemberPath = keyValue.Key,
                                     CellTemplate = new DataTemplate
                                                        {
                                                            VisualTree = label
                                                        }
                                 };


                return column;
            }

        }


        private MtObservableCollection<DataGridColumn> _ruleColumnList;
        public MtObservableCollection<DataGridColumn> RuleColumnList => _ruleColumnList ?? (_ruleColumnList = new MtObservableCollection<DataGridColumn>());

        #endregion


        #region Menu

        public void InitContextMenuForColumns()
        {
                _updatingMenu = true;

                CustomColumnsViewModel.ItemsSource = new ObservableCollection<MenuItemViewModel>(
                    UiHelperMetadata.AllRulesColumns.
                    Select(
                        t => new PropertyMenuItemViewModel(this)
                                 {
                                     Header = t.Name,
                                     IsChecked = t.CustomVisible
                                 }).
                        Cast<MenuItemViewModel>().ToList());

                _updatingMenu = false;
        }


        private bool _updatingMenu;
        private ObservableCollection<MenuItemViewModel> _menuItemList;
        public ObservableCollection<MenuItemViewModel> MenuItemList
        {
            get
            {
                if (_menuItemList == null)
                {
                    _updatingMenu = true;
                    _menuItemList = new ObservableCollection<MenuItemViewModel>
                                        {
                                            AllColumnsViewModel,
                                            CustomColumnsViewModel
                                        };
                    _updatingMenu = false;
                }

                return _menuItemList;
            }
            set
            {
                _menuItemList = value;
                OnPropertyChanged("MenuItemList");
            }
        }


        private PropertySetMenuItemViewModel _allColumnsViewModel;
        private PropertySetMenuItemViewModel AllColumnsViewModel => _allColumnsViewModel ?? (_allColumnsViewModel = new PropertySetMenuItemViewModel
        {
            PresenterModel = this,
            Header = ColumnSet.All.ToString(),
            IsChecked = PropertySet == ColumnSet.All
        });

        private PropertySetMenuItemViewModel _customColumnsViewModel;
        private PropertySetMenuItemViewModel CustomColumnsViewModel => _customColumnsViewModel ?? (_customColumnsViewModel = new PropertySetMenuItemViewModel
        {
            PresenterModel = this,
            Header = ColumnSet.Custom.ToString(),
            IsChecked = PropertySet == ColumnSet.Custom,
        });

        #endregion

        #region Implementation of IChangedHandler<in PropertySetMenuItemViewModel>

        public void OnChanged(PropertySetMenuItemViewModel propertyMenuItemViewModel)
        {
            if (propertyMenuItemViewModel.Header == ColumnSet.All.ToString())
            {
                CustomColumnsViewModel.IsChecked = false;
                PropertySet = ColumnSet.All;
            }

            if (propertyMenuItemViewModel.Header == ColumnSet.Custom.ToString())
            {
                AllColumnsViewModel.IsChecked = false;
                PropertySet = ColumnSet.Custom;
            }
        }

        #endregion

        #region Implementation of IChangedHandler<in PropertyMenuItemViewModel>

        public void OnChanged(PropertyMenuItemViewModel propertyMenuItemViewModel)
        {
            if (_updatingMenu) return;

            UiHelperMetadata.SetRuleColumn(propertyMenuItemViewModel.Header, propertyMenuItemViewModel.IsChecked);

            if (!CustomColumnsViewModel.IsChecked)
            {
                CustomColumnsViewModel.IsChecked = true;
            }
            else
            {


                if (propertyMenuItemViewModel.IsChecked)
                {
                    AddColumn(propertyMenuItemViewModel.Header);
                }
                else
                {
                    RemoveColumn(propertyMenuItemViewModel.Header);
                }
            }
        }

        #endregion

        public override void Clear()
        {
            Rules.Clear();
        }

        public override void Add(ViewModelBase problemViewModel)
        {
            var ruleViewModel = problemViewModel as RuleViewModel;
            if (ruleViewModel != null)
            {
                Rules.Add(ruleViewModel);
            }
        }

        public override void Update()
        {
            FilterChanged = true;
        }
    }
}
