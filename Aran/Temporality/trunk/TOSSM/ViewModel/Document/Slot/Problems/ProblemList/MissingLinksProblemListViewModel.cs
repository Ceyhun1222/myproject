using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Aran.Temporality.CommonUtil.ViewModel;
using MvvmCore;
using TOSSM.ViewModel.Document.Slot.Problems.Single;

namespace TOSSM.ViewModel.Document.Slot.Problems.ProblemList
{
    public class MissingLinksProblemListViewModel : ProblemListViewModel
    {
        public MissingLinksProblemListViewModel()
        {
            DataGridVisibility = Visibility.Visible;
        }

        #region Filterer logic

        public void UpdateRulesFiltered()
        {
            BlockerModel.Block();

            var result = new List<MissingLinkViewModel>();

            if (Rules != null)
            {
                result = Rules.ToList();
            }


            DataGridVisibility = (Rules != null && result.Count > 0)
                                                          ? Visibility.Visible
                                                          : Visibility.Collapsed;

            Status = (Rules != null && result.Count > 0) ? "Displayed " + result.Count + " items from " + Rules.Count : "";
      

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                (Action)(() =>
                {
                    RulesFiltered = result;

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

        #region Rules

        public MissingLinkViewModel SelectedRule { get; set; }

        private List<MissingLinkViewModel> _rules;
        public List<MissingLinkViewModel> Rules
        {
            get => _rules ?? (_rules = new List<MissingLinkViewModel>());
            set => _rules = value;
        }


        private List<MissingLinkViewModel> _rulesFiltered;
        public List<MissingLinkViewModel> RulesFiltered
        {
            get => _rulesFiltered;
            set
            {
                _rulesFiltered = value;
                OnPropertyChanged("RulesFiltered");
            }
        }



        public override void Clear()
        {
            Rules.Clear();
        }

        public override void Add(ViewModelBase problemViewModel)
        {
            var ruleViewModel = problemViewModel as MissingLinkViewModel;
            if (ruleViewModel != null)
            {
                Rules.Add(ruleViewModel);
            }
        }

        public override void Update()
        {
            UpdateRulesFiltered();
        }

        #endregion
    }
}
