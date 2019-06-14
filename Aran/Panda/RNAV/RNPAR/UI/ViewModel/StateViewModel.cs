using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public abstract class StateViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the StateViewModel class.
        /// </summary>
        /// 

        public  string Header { get; set; }


        protected MainViewModel mainViewModel;
        public StateViewModel NextState { get; protected set; }
        public StateViewModel PreviousState { get; protected set; }


        public StateViewModel(MainViewModel main)
        {
            mainViewModel = main;
        }

        public StateViewModel(MainViewModel main, StateViewModel previous)
        {
            mainViewModel = main;
            PreviousState = previous;
        }

        public virtual void Next()
        {
            if (CanNext())
            {
                next();
                if (NextState != null)
                    mainViewModel.Next(NextState);
            }
        }

        public virtual void Previous()
        {
            if (CanPrevious())
            {

                previous();
                if (PreviousState != null)
                {
                    mainViewModel.Previous(PreviousState);
                    PreviousState.ReInit();
                }
            }

        }


        public abstract bool CanNext();
        public abstract bool CanPrevious();


        protected abstract void next();
        protected abstract void previous();
        protected abstract void saveReport();

        public void Destroy()
        {
            _destroy();
            PreviousState?.Destroy();
        }
        protected abstract void _destroy();

        protected virtual void ReInit() { }

        private RelayCommand _nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                return _nextCommand
                    ?? (_nextCommand = new RelayCommand(
                    () =>
                    {
                        Next();
                    }, () => CanNext()));
            }
        }
        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand
                    ?? (_backCommand = new RelayCommand(
                    () =>
                    {
                        Previous();
                    }, () => CanPrevious()));
            }
        }
      
    }
}