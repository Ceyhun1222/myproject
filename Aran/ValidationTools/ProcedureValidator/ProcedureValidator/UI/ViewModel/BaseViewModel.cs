using GalaSoft.MvvmLight;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public abstract class BaseViewModel : ViewModelBase
    {


        private BlockerModel _blockerModel;

        public BlockerModel BlockerModel
        {
            get { return _blockerModel ?? (_blockerModel = new BlockerModel()); }
        }
    }
}