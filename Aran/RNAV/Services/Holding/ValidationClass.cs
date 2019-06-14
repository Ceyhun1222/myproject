using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Aran.Aim.Features;
using Holding.Models;

namespace Holding
{
    public class ValidationClass:INotifyPropertyChanged
    {
        public ValidationClass()
        {
            Apply = true;
        }        
        
        public FlightCondition FlihtPhase { get; set; }
        public FlightCondition Reciever { get; set; }
        public PBNCondition PBN { get; set; }
        public Aran.Geometries.Point RefPoint { get; set; }
       

        public double Altitiude { get; set; }

        double Radial { get; set; }
        public double Time { get; set; }
        public ProcedureType ProcedureType { get; set; }
        public DistanceType DistancType { get; set; }
        public double WD { get; set; }
        public double IAS { get; set; }
        public List<Navaid> NavList { get; set; }
        public double ATT { get; set; }
        public double XTT { get; set; }

        private bool _apply;
        public bool Apply
        {
            get{return _apply;}
            set
            {
                _apply = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Apply"));
            }
        }


        public double  Moc { get; set; }
        public void MocChange(object value,string propertyName)
        {
            if (Apply && value == typeof(ValidationClass).GetProperty(propertyName).GetValue(this,null))
                Apply = false;
            else if (!Apply && value != typeof(ValidationClass).GetProperty(propertyName).GetValue(this, null))
                    Apply = true ;
                        
            //if (PropertyChanged != null)
            //    PropertyChanged(this, new PropertyChangedEventArgs("Apply"));
            
        }
        //private double _applyDistance,_currentDistance;
        //public double Distancses
        //{
        //    get { return _distance; }
        //    set 
        //    {
        //        if (_distance != value)
        //            _holdingAreaEnabled = true;
        //        else
        //            Apply = false;
        //        _currentDistance = value;
        //    }
        //}



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
