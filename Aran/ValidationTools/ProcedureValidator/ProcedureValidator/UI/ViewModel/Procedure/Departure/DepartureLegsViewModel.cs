using PVT.Model;
using System.Collections.Generic;

namespace PVT.UI.ViewModel
{
    class DepartureLegsViewModel:LegsViewModel
    {
        public DepartureLegsViewModel(LegType type, List<TransitionLeg> legs):base(type, legs)
        {

        }
    }
}
