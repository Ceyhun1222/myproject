using Aran.Aim;
using Aran.Temporality.Common.Entity.Enum;
using MvvmCore;

namespace Aran.Temporality.UserManager.ViewModel.Single
{
    public class SingleFeatureAccessModel : ViewModelBase
    {
        public FeatureType Feature { get; set; }

        public delegate void StateHandler();
        public StateHandler OnStateChanged;

        private bool _isEnabled=true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        private bool _isRead;
        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                _isRead = value;
                if (OnStateChanged!=null) OnStateChanged();
                OnPropertyChanged("IsRead");
            }
        }

        private bool _isWrite;
        public bool IsWrite
        {
            get { return _isWrite; }
            set
            {
                _isWrite = value;
                if (value) IsRead = true;
                if (OnStateChanged!=null) OnStateChanged();
                OnPropertyChanged("IsWrite");
            }
        }


        public SingleFeatureAccessModel(SingleFeatureAccessModel accessModel)
        {
            Feature = accessModel.Feature;
            IsRead = accessModel.IsRead;
            IsWrite = accessModel.IsWrite;
            OnStateChanged = accessModel.OnStateChanged;
        }

        public SingleFeatureAccessModel()
        {
           
        }

        public int DataOperationFlag
        {
            get
            {
                var result=0;
                if (IsRead) result += (int)DataOperation.ReadData;
                if (IsWrite) result += (int)DataOperation.WriteData;
                return result;
            }
        }
    }
}
