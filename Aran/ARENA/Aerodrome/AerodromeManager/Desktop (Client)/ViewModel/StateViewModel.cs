namespace AmdbManager.ViewModel
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

        protected readonly MainViewModel MainViewModel;
        public StateViewModel NextState { get; protected set; }
        public StateViewModel PreviousState { get; protected set; }

        public StateViewModel(MainViewModel main)
        {
            MainViewModel = main;
        }

        public StateViewModel(MainViewModel main, StateViewModel previous)
        {
            MainViewModel = main;
            PreviousState = previous;
        }

        public void Next()
        {
            if (CanNext())
            {
                SetNext();
                if (NextState != null)
                    MainViewModel.Next(NextState);
            }
        }

        public void Previous()
        {
            if (CanPrevious())
            {
                SetPrevious();
                if (PreviousState != null)
                {
                    MainViewModel.Previous(PreviousState);
                    PreviousState.ReInit();
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

        protected abstract void SetNext();

        protected abstract void SetPrevious();

        protected virtual void ReInit() { }
    }
}