using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Holding
{
    public  class ModelPBN:INotifyPropertyChanged
    {
        #region :>Fields
        private FlightCondition _condition;
        private PhaseRecieverCondition _curPhase;
        private PhaseRecieverCondition _curReciever;
        private PBNCondition _curPBN;
        #endregion

        #region :>Constructor
        public ModelPBN()
        {
            _condition = new FlightCondition();
            PhaseList = _condition.GetFlightPhases(flightPhase.Enroute | flightPhase.STARUpTo30 | flightPhase.STARDownTo30);
            CurFlightPhase = PhaseList[0];

            CurReciever = RecieverList[0];
            MinDistance = Common.MinEnrouteDistance;
           
        }
        #endregion

        #region :>Property

        #region Lists
        public List<PhaseRecieverCondition> PhaseList { get; private set; } 
        
        public List<PhaseRecieverCondition> RecieverList { get; private set; }

        public List<PBNCondition> PbnConditionList { get; private set; }
        #endregion

        #region curvalue
        public double MinDistance { get; set; }
        public double MaxDistance { get; set; }
     
        public PhaseRecieverCondition CurReciever 
        {
            get{return _curReciever;}
            set
            {
                PhaseRecieverCondition tmpReciever = _curReciever;
                if (FlightConditionChange<PhaseRecieverCondition>(this, "CurReciever", ref _curReciever, value, PropertyChanged))
                {
                    if (CurRecieverChanged != null)
                        CurRecieverChanged(this,new EventArgs());
                } 
            }
        }

        public PhaseRecieverCondition CurFlightPhase 
        {
            get{return _curPhase;}
            set
            {

                if (FlightConditionChange<PhaseRecieverCondition>(this, "CurFlightPhase", ref _curPhase, value, PropertyChanged))
                {
                    if (CurFlightPhaseChanged != null)
                        CurFlightPhaseChanged(this, new EventArgs());
                }
            }
        }

        public PBNCondition CurPBN 
        {
            get { return _curPBN; }
            set 
            {
                if (_curPBN != value)
                {
                    _curPBN = value;
                    if (CurPbnChanged != null)
                        CurPbnChanged(this, new EventArgs());
                }
            }
        }

        public bool CoverageIsEnabled { get; set; }

      //  public bool DmeCovTypeIsEnabled { get; set; }

        #endregion
        #endregion

        #region :>Methods

        private bool FlightConditionChange<T>(object owner, string propName, ref T oldValue,T newValue, PropertyChangedEventHandler eventHandler)
         {
             if (owner.GetType().GetProperty(propName) == null)
             {
                 throw new ArgumentException("No property named " + propName + " on " + owner.GetType().FullName);
             }
             // we only raise an event if the value has changed
            
            if (!Equals(oldValue, newValue) && newValue !=null)
             {

                 PhaseRecieverCondition tmpPhaseReciever = newValue as PhaseRecieverCondition;
                 if (propName == "CurFlightPhase")
                 {
                     RecieverList = _condition.GetReciever(newValue as PhaseRecieverCondition);

                     if (tmpPhaseReciever.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.STARDownTo30])
                     {
                         MaxDistance = Common.MaxStarDownTo30Distance;
                         MinDistance = Common.MinEnrouteDistance;

                     }
                     else if (tmpPhaseReciever.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.STARUpTo30])
                     {
                         MaxDistance = Common.MaxEnrouteDistance;
                         MinDistance = Common.MinStarUpTO30Distance;
                     }
                     else if (tmpPhaseReciever.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
                     {
                         MaxDistance = Common.MaxEnrouteDistance;
                         MinDistance = Common.MinEnrouteDistance;

                     }
                     oldValue = newValue;
                     
                 }
                 if (propName == "CurReciever")
                 {
                     PbnConditionList = _condition.GetPBNCondition(newValue as PhaseRecieverCondition);
                     if (PbnConditionList.Count > 0)
                         _curPBN = PbnConditionList[0];
                     if (tmpPhaseReciever.RecieverName ==InitHolding.FlightRecieverValue[flightReciever.DMEDME])
                     {
                         CoverageIsEnabled = true;
                     }
                     else if (tmpPhaseReciever.RecieverName == InitHolding.FlightRecieverValue[flightReciever.VORDME])
                     {
                         CoverageIsEnabled = true;
                     }
                     else
                     {
                         CoverageIsEnabled = false;
                     }

                     oldValue = newValue;
                                          
                 }

                 if (eventHandler != null)
                 {
                     eventHandler(owner, new PropertyChangedEventArgs(propName));
                 }
                 return true;
 
             }
            return false;

         }
        
        #endregion
       
        #region :>event 
        
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CurPbnChanged;
        public event EventHandler CurFlightPhaseChanged;
        public event EventHandler CurRecieverChanged;
        
        #endregion

    }

   
}
