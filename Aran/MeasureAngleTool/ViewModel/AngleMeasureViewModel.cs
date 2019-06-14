using GalaSoft.MvvmLight;

namespace Aran.AimEnvironment.Tools.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
     class AngleMeasureViewModel : ViewModelBase
    {

        public AngleMeasure AngleMeasure { get; private set; }

        public AngleMeasureViewModel()
        {
            AngleMeasure = AngleMeasure.Current;
        }

        public void Dispose()
        {
            AngleMeasure.Stop();
            AngleMeasure = null;
        }
    }
}
