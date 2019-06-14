using System;

namespace Holding.Models
{
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
