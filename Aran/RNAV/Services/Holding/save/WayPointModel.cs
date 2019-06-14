using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Delib.Classes.Features.AirportHeliport;
using Delib.Classes.Codes;
using Delib.Classes.Features.Navaid;
using System.Windows.Forms;
using Aran.Aim.Features;

namespace Holding.HoldingSave
{
    
    public class WayPointModel:INotifyPropertyChanged
    {
        #region :>Fields

        private string _designator;
        private string _name;
        private AirportHeliport _adhp;
        private DesignatedPointType? _desingnatedType;
        private string _adhpChoise;

        #endregion
      
        #region :>Ctor
        public WayPointModel(ModelPBN modelPbn,ModelPointChoise modelPointChoise)
        {
            Latitude = ChangeDD(modelPointChoise.CurPoint.X);
            Longtitude = ChangeDD(modelPointChoise.CurPoint.Y);
            
            AdhpChoiseList = new List<string>();
            AdhpChoiseList.Add("None");
            AdhpChoiseList.Add(modelPointChoise.CurAdhp.designator);
            
            _adhp = modelPointChoise.CurAdhp;

            DesignatedPointTypeList = new List<DesignatedPointType>{DesignatedPointType.COORD,DesignatedPointType.BRG_DIST,DesignatedPointType.CNF,
                DesignatedPointType.DESIGNED,DesignatedPointType.ICAO,DesignatedPointType.MTR,DesignatedPointType.TERMINAL,DesignatedPointType.OTHER};

            
            if (modelPointChoise.PointChoise == ChoosePointNS.SignificantPointChoice.DesignatedPoint)
            {
                WayPoint = modelPointChoise.CurSigPoint;
                _designator = modelPointChoise.CurSigPoint.designator;
                _name = modelPointChoise.CurSigPoint.name;
                DesignatedType = modelPointChoise.CurSigPoint.type;               
                
              
            }
            else
            {
                WayPoint = new DesignatedPoint();
                WayPoint.location = new Delib.Classes.GeomObjects.Point();
                WayPoint.location.x = modelPointChoise.CurPoint.X;
                WayPoint.location.y = modelPointChoise.CurPoint.Y;
                NameIsEnabled = true;
                DesignatorIsEnabled = true;
                DesignatedPointTypeIsEnabled = true;
                AdhpChoiseIsEnabled = true;
                DesignatedPointTypeIsEnabled = true;
                
            }
            
            if (modelPbn.CurFlightPhase.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute] && 
                modelPointChoise.PointChoise== ChoosePointNS.SignificantPointChoice.Point)
            {
                AdhpChoise = AdhpChoiseList[0];
            }
            else
            {
                AdhpChoise = AdhpChoiseList[1];
            }
        }
        #endregion

        #region :>Property
        public List<string> AdhpChoiseList { get; set; }
        public List<DesignatedPointType> DesignatedPointTypeList { get; private set; }

        public string Designator
        {
            get { return _designator; }
            set 
            {
                if (value.Length > 5 || value.Length < 1)
                {
                    MessageBox.Show("You can write between 1 and 5 words ");
                    throw new Exception("Value isn't true");
                }
                else
                    _designator = value;
                WayPoint.designator = _designator;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Designator"));
            }
        }
        public bool DesignatorIsEnabled { get; set; }

       
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                WayPoint.name = _name;
            }
        }
        public bool NameIsEnabled { get; set; }

        public string Latitude { get;private set; }
        public string Longtitude { get;private set; }

        public string AdhpChoise
        {
            get { return _adhpChoise; }
            set 
            {
                if (_adhpChoise == value)
                    return;
                _adhpChoise = value;

                if (_adhpChoise != "None")
                    WayPoint.airportHeliport = _adhp;
                else
                    WayPoint.airportHeliport = null;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AdhpChoise"));
                    
            }
        }
        public bool AdhpChoiseIsEnabled { get; private set; }

        public DesignatedPointType? DesignatedType
        {
            get { return _desingnatedType; }
            set
            {
                if (_desingnatedType== value)
                    return;
                _desingnatedType = value;
                WayPoint.type = value;
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("DesignatedType"));
              
            }
        }
        public bool DesignatedPointTypeIsEnabled { get; private set; }

        public DesignatedPoint WayPoint { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region :>Methods
        public string ChangeDD(double value)
        {
            double xDeg,xMin,xSec;
            DD2DMS(value, out xDeg,out xMin, out xSec, 1);
            return xDeg.ToString() + "° " + xMin.ToString() + "' " + xSec.ToString()+"''";
        }

        private void DD2DMS(double val, out double xDeg, out double xMin, out double xSec, int Sign)
        {
            double x;
            double dx;

            x = System.Math.Abs(System.Math.Round(System.Math.Abs(val) * Sign, 10));

            xDeg = Fix(x);
            dx = (x - xDeg) * 60;
            dx = System.Math.Round(dx, 8);
            xMin = Fix(dx);
            xSec = (dx - xMin) * 60;
            xSec = System.Math.Round(xSec, 6);
        }

        private int Fix(double x)
        {
            return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
        }
        #endregion

        public void Store()
        {
            
        }
    }
}
