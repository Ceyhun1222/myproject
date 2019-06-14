using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delib.Classes.Features.Navaid;
using Delib.Classes.Objects.Navaid;
using Delib.Classes.Codes;
using Delib.Classes.UOM;

namespace Holding.HoldingSave
{
    public class SegmentPointWizardPage:IWizardPage
    {
        public SegmentPointWizardPage(DesignatedPoint dPoint,ModelPBN modelPbn)
        {
            if (modelPbn.CurFlightPhase.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
                HoldingSegmentPoint = new EnRouteSegmentPoint();
            else 
                HoldingSegmentPoint = new TerminalSegmentPoint();
        }

        public System.Windows.Forms.Control WizarControl { get; set; }
        
        public bool IsComplete { get; set; }
        
        public bool IsValidated { get; set; }

        public bool IsFeature { get { return false; } }

        public int PageIndex { get; set; }
        
        
        public void Save()
        {
        }

        public ATCReportingType ReportingType { get; set; }

        public YesNoType FlyOver { get; set; }

        public YesNoType WayPoint { get; set; }

        public YesNoType RadarGuidance { get; set; }

        public double PriorFixTolerance { get; set; }

        private DistanceVerticalType _priorFixType;
        public DistanceVerticalType PriorFixToleranceValType
        {
            get { return _priorFixType; }
            set
            {
                _priorFixType = value;

            }
        }

        public SegmentPoint HoldingSegmentPoint { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
