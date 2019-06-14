using Aran.Aim;
using Aran.Temporality.Common.Entity.Enum;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.ViewModel.Single
{
    public class SingleFeatureAccessModel : ViewModelBase
    {
        public FeatureType Feature { get; set; }

        public delegate void StateHandler();
        public StateHandler OnStateChanged;

        public bool IsEnabled { get; set; } = true;

        private bool _isRead;
        public bool IsRead
        {
            get => _isRead;
            set
            {
                _isRead = value || _isWrite;

                OnStateChanged?.Invoke();
                OnPropertyChanged("IsRead");
            }
        }

        private bool _isWrite;
        public bool IsWrite
        {
            get => _isWrite;
            set
            {
                _isWrite = value;
                if (value) IsRead = true;
                OnStateChanged?.Invoke();
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
