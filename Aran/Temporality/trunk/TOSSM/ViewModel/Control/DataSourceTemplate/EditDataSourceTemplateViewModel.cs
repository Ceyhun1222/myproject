using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;
using TOSSM.ViewModel.Tool;

namespace TOSSM.ViewModel.Control.DataSourceTemplate
{
    public class EnumDescriptionViewModel : ViewModelBase
    {
        private Enum _enum;

        public Enum Enum
        {
            get => _enum;
            set
            {
                _enum = value;
                Description = EnumHelper.GetDescription(Enum);
            }
        }

        public string Description { get; set; }
    }

    public class EditDataSourceTemplateViewModel : ViewModelBase
    {
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

        public void Show()
        {
            Visibility = Visibility.Visible;

            IsNameFocused = false;
            IsNameFocused = true;
        }

        private Visibility _visibility = Visibility.Hidden;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        public Action OkAction { get; set; }

       
        private string _name;
        private ChartType _group;

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

                            if (OkAction != null)
                            {
                                OkAction();
                            }
                        },
                        t => !string.IsNullOrWhiteSpace(Name)));
            }
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
                        }));
            }
        }

        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public ChartType Group
        {
            get => _group;
            set
            {
                if (_group==value) return;
                _group = value;
                SelectedGroup = Groups.FirstOrDefault(t => t.Enum.ToString()==Group.ToString());
            }
        }

        public EnumDescriptionViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                if (SelectedGroup != null)
                {
                    Group = (ChartType) SelectedGroup.Enum;
                }
                OnPropertyChanged("SelectedGroup");
            }
        }

        private List<EnumDescriptionViewModel> _groups;
        private EnumDescriptionViewModel _selectedGroup;

        public List<EnumDescriptionViewModel> Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = new List<EnumDescriptionViewModel>();
                    foreach (ChartType item in Enum.GetValues(typeof(ChartType)))
                    {
                        _groups.Add(new EnumDescriptionViewModel{Enum = item});
                    }
                }
                return _groups;
            }
        }
    }
}
