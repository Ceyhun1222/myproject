using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace ArenaErrorReport.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private StateViewModel _stateViewModel;

        public MainViewModel(System.Collections.Generic.List<Aran.Aim.DeserializedErrorInfo> errorInfoList)
        {
            
            ViewModel = new ErrorReportViewModel(this,errorInfoList);
        }


        public StateViewModel ViewModel
        {
            get => _stateViewModel;
            set => Set(ref _stateViewModel, value);
        }


        public event Action RequestClose;
        public void Close()
        {
            RequestClose?.Invoke();
        }

        public void Next(StateViewModel nextState)
        {
            ViewModel = nextState;
        }

        public void Previous(StateViewModel previousState)
        {
            ViewModel = previousState;
        }
    }
}