using System;
using Aran.Temporality.CommonUtil.Control.TreeViewDragDrop;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel
{
    public class PropertyViewModel : ViewModelBase
    {
        public FeatureTreeViewItemViewModel Parent { get; set; }

        private string _parentName;
        public string ParentName
        {
            get { return _parentName; }
            set
            {
                _parentName = value;
                OnPropertyChanged("ParentName");
            }
        }

        private string _childName;
        public string ChildName
        {
            get { return _childName; }
            set
            {
                _childName = value;
                OnPropertyChanged("ChildName");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");

                var i = Name.LastIndexOf('\\');
                if (i==-1)
                {
                    ParentName = string.Empty;
                    ChildName = Name;
                }
                else
                {
                    ChildName = Name.Substring(i+1);
                    ParentName = Name.Substring(0, i+1);
                }
                
            }
        }

        public bool EditingEnabled
        {
            get
            {
                if (Parent == null) return false;
                var parent = Parent.SelectedItemHolder as FeatureDependencyConfigurationViewModel;
                if (parent == null) return false;
                return parent.Mode == ConfiguratorMode.ReadWrite;
            }
        }

        public Action<PropertyViewModel> MandatoryChangedAction { get; set; }
        public Action<PropertyViewModel> OptionalChangedAction { get; set; }

        public void SetIsError(bool value)
        {
            _isError = value;
            OnPropertyChanged("IsError");
            if (IsError)
            {
                SetIsWarning(true);
            }
        }

        public void SetIsWarning(bool value)
        {
            _isWarning = value;
            OnPropertyChanged("IsWarning");
        }


        private bool _isError;
        public bool IsError
        {
            get { return _isError; }
            set
            {
                if (_isError==value) return;

                _isError = value;
                OnPropertyChanged("IsError");
                if (IsError)
                {
                    IsWarning = true;
                }

                if (MandatoryChangedAction != null)
                {
                    MandatoryChangedAction(this);
                }
            }
        }

        private bool _isWarning;
        public bool IsWarning
        {
            get { return _isWarning; }
            set
            {
                if (_isWarning == value) return;

                _isWarning = value;
                OnPropertyChanged("IsWarning");

                if (OptionalChangedAction != null)
                {
                    OptionalChangedAction(this);
                }
            }
        }
    }
}
