using PVT.Model;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DepartureViewModel : ProcedureViewModel<DepartureProcedure>
    {
        /// <summary>
        /// Initializes a new instance of the DepartureViewModel class.
        /// </summary>
        public DepartureViewModel(MainViewModel main, StateViewModel previous, DepartureProcedure procedure) : 
            base(main, previous, procedure)
        {

        }
    }
}