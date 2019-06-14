using PVT.Model;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ApproachTransitionViewModel : TransitionViewModel
    {

        public bool IsGraphEnabled {
            get {
                return Transition.Original.Type == Aran.Aim.Enums.CodeProcedurePhase.FINAL;
            }

        }

    
        public ApproachTransitionViewModel(Transition transition):base(transition)
        {

        }
    }
}