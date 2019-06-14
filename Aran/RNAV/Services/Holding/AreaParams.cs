using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ARAN.Common;

namespace Holding
{
    public class AreaParams:INotifyPropertyChanged
    {
        public AreaParams(double altitude,double radial,double ias,categories airCraftCategory,SideDirection turn,double moc)
        {
            Altitude = altitude;
            Radial = radial;
            Ias = ias;
            CurCategory = airCraftCategory;
            Turn = turn;
            CurMoc = moc;
        }

        #region Altidue
        private double _minAltitude;

        public double MinAltitude
        {
            get { return _minAltitude; }
            set { _minAltitude = value; }
        }

        private double _maxAltitude;

        public double MaxAltitude
        {
            get { return _maxAltitude; }
            set { _maxAltitude = value; }
        }

        private double _altitude;

        public double Altitude
        {
            get { return _altitude; }
            set { _altitude = value; }
        }

        #endregion

        #region Radial

        private double _minRadial;

        public double MinRadial
        {
            get { return _minRadial; }
            set { _minRadial = value; }
        }

        private double _maxRadial;
        public double MaxRadial
        {
            get { return _minRadial; }
            set { _maxRadial = value; }
        }

        private double _radial;

        public double Radial
        {
            get { return _radial; }
            set { _radial = value; }
        }
        
        
        #endregion

        #region Ias

        private double _minIas;

        public double MinIas
        {
            get { return _minIas; }
            set { _minIas = value; }
        }

        private double _maxIas;

        public double MaxIas
        {
            get { return _maxIas; }
            set { _maxIas = value; }
        }

        private double _ias;

        public double Ias
        {
            get { return _ias; }
            set { _ias = value; }
        }

        public bool NormalCondition { get; set; }

        public bool TurboCondition { get; set; }

#endregion

        #region AircraftCategory

        public List<categories> AircraftCategories { get; set; }
        private categories _curCategory;

        public categories CurCategory
        {
            get { return _curCategory; }
            set { _curCategory = value; }
        }
        

#endregion

        #region Turn
        public  SideDirection Turn { get; set; }
        #endregion

        #region Moc
        public List<double> MocList { get; set; }
        
        private double _curMoc;

        public double CurMoc
        {
            get { return _curMoc; }
            set { _curMoc = value; }
        }
        
#endregion
    }
}
