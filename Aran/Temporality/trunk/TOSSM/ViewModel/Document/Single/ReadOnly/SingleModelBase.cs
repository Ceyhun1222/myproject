using System.ComponentModel;
using MvvmCore;

namespace TOSSM.ViewModel.Document.Single.ReadOnly
{
    public abstract class SingleModelBase : ViewModelBase, IDataErrorInfo
    {
        public static string SimplePicture = "bullet";
        public static string LockedPicture = "locked_black";
        public static string ListPicture = "list";
        public static string ComplexPicture = "more";
        public static string GeoPicture = "geo";

        private string _toolTip;
        public string ToolTip
        {
            get => _toolTip;
            set
            {
                _toolTip = value;
                OnPropertyChanged("ToolTip");
            }
        }

        private string _pictureId;
        public virtual string PictureId
        {
            get => _pictureId;
            set
            {
                if (_pictureId==value) return;
                _pictureId = value;
                OnPropertyChanged("PictureId");
            }
        }

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                if (_groupName==value) return;
                _groupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        private string _propertyName;
        public string PropertyName
        {
            get => _propertyName;
            set
            {
                if (_propertyName==value) return;
                _propertyName = value;
                OnPropertyChanged("PropertyName");
            }
        }
        
        private bool _isNotNull;
        public bool IsNotNull
        {
            get => _isNotNull;
            set
            {
                if (_isNotNull == value) return;
                _isNotNull = value;
                OnPropertyChanged("IsNotNull");
            }
        }

        private string _stringValue;
        public string StringValue
        {
            get => _stringValue;
            set
            {
                if (_stringValue==value) return;
                _stringValue = value;
                OnPropertyChanged("StringValue");
            }
        }

        #region Implementation of IDataErrorInfo

        public virtual string ValidateValue()
        {
            return null;
        }

        public string this[string columnName] => columnName=="Value" ? ValidateValue() : null;

        public string Error => null;

        #endregion
    }
}
