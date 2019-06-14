using GalaSoft.MvvmLight;

namespace PVT.UI.ViewModel
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
                    PreviousState.reInit();
                }
            }

        }

        public abstract bool CanNext();
        public abstract bool CanPrevious();

        public void Destroy()
        {
            _destroy();
            if (PreviousState != null)
                PreviousState.Destroy();
        }

        protected abstract void _destroy();

        protected abstract void next();
        protected abstract void previous();
        protected virtual void reInit() { }
    }
}