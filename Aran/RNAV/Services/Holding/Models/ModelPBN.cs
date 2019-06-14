using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Holding.Models
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
            if (GlobalParams.Radius>Common.MinStarUpTO30Distance)
                PhaseList = _condition.GetFlightPhases(flightPhase.Enroute | flightPhase.STARUpTo30 | flightPhase.STARDownTo30);
            else
                PhaseList = _condition.GetFlightPhases(flightPhase.Enroute | flightPhase.STARDownTo30);
            CurFlightPhase = PhaseList[0];

            CurReciever = RecieverList[0];
            MinDistance = Common.MinEnrouteDistance;
            MaxDistance = GlobalParams.Radius;
           
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
                if (FlightConditionChange<PhaseRecieverCondition>(this, "CurReciever", ref _curReciever, value, PropertyChanged))
                {
                    if (CurRecieverChanged != null)
                        CurRecieverChanged(this,new EventArgs());
                    
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CurReciever"));
                    }
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
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CurReciever"));
                    }
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
                     PhaseRecieverCondition tmpReciever =null;
                     if (_curReciever!=null)
                        tmpReciever =RecieverList.Find(rec => rec.RecieverName == _curReciever.RecieverName);
                     
                     if (tmpReciever != null)
                         CurReciever = tmpReciever;
                     else
                         CurReciever = RecieverList[0];
                     
                     if 
                         (tmpPhaseReciever.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.STARDownTo30])
                     {
                         MinDistance = 0;
                         if (MaxDistance>Common.MinStarUpTO30Distance)
                            MaxDistance = Common.MinStarUpTO30Distance;
                     }
                     else if (tmpPhaseReciever.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.STARUpTo30])
                     {
                         MinDistance = Common.MinStarUpTO30Distance;
                         MaxDistance = GlobalParams.Radius;
                     }
                     else if (tmpPhaseReciever.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
                     {
                         MinDistance = 0;
                         MaxDistance = GlobalParams.Radius;

                     }
                     oldValue = newValue;
                 }
                 if (propName == "CurReciever")
                 {
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
                 PbnConditionList = _condition.GetPBNCondition(_curReciever);
                 if (PbnConditionList.Count > 0)
                     _curPBN = PbnConditionList[0];
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
