using PVT.Model;
using System.Collections.Generic;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ApproachLegsViewModel : LegsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ApproachTransitionLegViewModel class.
        /// </summary>
        /// 


        public ApproachLegsViewModel(LegType type, List<TransitionLeg> legs):base(type, legs)
        {

        }
    }
}