using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Holding
{
    public delegate void FlightRecieverChanged(object sender,FlightRecieverChangedEventArgs flightPhaseChangedEventArgs);
    
    public class FlightRecieverChangedEventArgs : EventArgs
    {  
        public FlightRecieverChangedEventArgs(PhaseRecieverCondition oldPhase, PhaseRecieverCondition newPhase)
        {
            OldFlightPhase = oldPhase;
            NewFlightPhase = newPhase;
        }

        public PhaseRecieverCondition OldFlightPhase { get; set; }
        public PhaseRecieverCondition NewFlightPhase { get; set; }
     }
}
