using PVT.Model;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ApproachViewModel : ProcedureViewModel<ApproachProcedure>
    {
        /// <summary>
        /// Initializes a new instance of the ApproachViewModel class.
        /// </summary>
        /// 

        public ApproachViewModel(MainViewModel main, StateViewModel previous, ApproachProcedure procedure) : 
            base(main, previous, procedure)
        {

        }
    }
}