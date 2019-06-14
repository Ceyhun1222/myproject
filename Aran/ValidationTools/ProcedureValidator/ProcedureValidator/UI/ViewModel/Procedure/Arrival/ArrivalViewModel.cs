using PVT.Model;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ArrivalViewModel : ProcedureViewModel<ArrivalProcedure>
    {
        /// <summary>
        /// Initializes a new instance of the ArrivalViewModel class.
        /// </summary>
        /// 

        public ArrivalViewModel(MainViewModel main, StateViewModel previous, ArrivalProcedure procedure) : 
            base(main, previous, procedure)
        {

        }
    }
}