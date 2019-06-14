using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ARAN.Common;
using ARAN.Contracts.Constants;

namespace Holding
{
    public enum ConditionType
    {
        normal,
        turbo
    }
    
    public class ModelAreaParams:Changed,INotifyPropertyChanged
    {

        #region :>Fields
        
        private AircraftCategoryList _aircraftCategoryList;
        private double _applyAltitude, _applyRadial, _applyIas,
            _applyWD,_minAltitude,_maxAltitude;
        private SideDirection _applyTurn;
        #endregion

        #region :> Constructor

        public ModelAreaParams(double altitude,double radial,double ias,categories aircraftCategory,SideDirection turn,double moc,ConditionType condition)
        {
            _aircraftCategoryList = GlobalParams.Constant_G.AircraftCategory;
           
            AircraftCategories = new List<categories> { categories.A, categories.B, categories.C, categories.D, categories.E };
            _minAltitude = InitHolding.MinCatAltitudeVal;
            _maxAltitude = InitHolding.MaxCDECatAltitudeVal;
            MinRadialInDegree = -1;
            MaxRadialInDegree = 360;
            _radial = radial;
            _ias = ias;
            Turn = turn;
            CurMoc = Common.ConvertHeight(moc, roundType.toNearest) ;
            Condition = condition;
            _curCategory = aircraftCategory;
            Altitude = altitude;
                        

            MocList = new List<double>();
            for (int i = 0; i < 3; i++)
            {
                MocList.Add(Math.Round(Common.ConvertHeight(300 + 150 * i, roundType.toNearest),0));
            }
                        
        }

        #endregion
       
        #region :>Property
        #region Altidue
        private double _altitudeIndex = -1;
        
        public double MinAltitude
        {
            get{return Common.ConvertHeight(_minAltitude,roundType.toUp);}
            
        }

        public double MaxAltitude { get { return Common.ConvertHeight(_maxAltitude, roundType.toDown); } }

        public double  DOC { get; set; }

        public double DOCMin { get; set; }

        private double _altitude = 0;
        public double Altitude
        {
            get { return Common.ConvertHeight(_altitude,roundType.toNearest); }
            set 
            {
                double tmp = Common.DeConvertHeight(value);
                if (tmp == _altitude)
                    return;

                CalculateDoc(tmp);
                if (AltitudeChange(tmp))
                    SetIasParams();

                ChangeModelChanged(_altitude, tmp, _applyAltitude);
            
                _altitude = tmp;


                if (AltitudeChanged != null)
                    AltitudeChanged(this, new EventArgs());
                
                if (PropertyChanged!=null)
                        PropertyChanged(this,new PropertyChangedEventArgs("Altitude"));

              
            }
        }

        
        #endregion

        #region Radial


        public double MinRadialInDegree { get; private set; }

        public double MaxRadialInDegree { get; private set; }
       
        private double _radial;
        public double Radial
        {
            get { return ARANFunctions.RadToDeg(_radial); }
            set
            {

                double tmpRad = ARANFunctions.DegToRad(value);
                if (tmpRad!=_radial)
                {
                    ChangeModelChanged(_radial, tmpRad, _applyRadial);                    
                    _radial = tmpRad;

                    if (SpecialParamChanged!=null)
                        SpecialParamChanged(this,new EventArgs());

                    if (PropertyChanged!=null)
                        PropertyChanged(this,new PropertyChangedEventArgs("Radial"));
                
                }
            }
        }

        private double _tag;
        public double Tag
        {
            get { return _tag; }
            set 
            {
                _tag = value;
                //if (PropertyChanged != null)
                //    PropertyChanged(this, new PropertyChangedEventArgs("Radial"));
            }
        }
        
        #endregion

        #region Ias

        private double _minIas;
        public double MinIas
        {
            get { return Common.ConvertSpeed_(_minIas, roundType.toUp); }
            set { _minIas = Common.DeConvertSpeed(value); }
        }

        private double _maxIas;
        public double MaxIas 
        { 
            get{return Common.ConvertSpeed_(_maxIas,roundType.toDown);} 
            set{_maxIas = Common.DeConvertSpeed(value);} 
        }

        private double _ias; 
        public double Ias
        {
            get { return Common.ConvertSpeed_(_ias,roundType.toNearest); }
            set
            {
                if (_ias == Common.DeConvertSpeed(value))
                    return;

                ChangeModelChanged(_ias, Common.DeConvertSpeed(value), _applyIas);
                _ias  = Common.DeConvertSpeed(value);

                if (SpecialParamChanged != null)
                    SpecialParamChanged(this, new EventArgs());
                
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Ias"));
            }
        }
        
        public ConditionType Condition { get; set; }

        public bool NormalCondition
        {
            get { return Equals(Condition, ConditionType.normal); }
            set
            {
                if (value)
                {
                    Condition = ConditionType.normal;
                    SetIasParams();
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("NormalCondition"));
                }
            }
        }
      
        public bool TurboCondition
        {
            get { return Equals(Condition,ConditionType.turbo); }
            set
            {
                if (value)
                {
                    Condition = ConditionType.turbo;
                    SetIasParams();
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("TurboCondition"));
                }
              
                
            }
        }
            

#endregion

        #region AircraftCategory

        public List<categories> AircraftCategories { get;private set; }
        
        private categories _curCategory;
        public categories Category
        {
            get { return _curCategory; }
            set 
            {
               if (CategoryChange(value))
                   SetIasParams();
               if (PropertyChanged != null)
                   PropertyChanged(this, new PropertyChangedEventArgs("AircraftCategories"));
            }
        }

#endregion

        #region Turn
        
        public  SideDirection Turn { get; set; }

        public bool RightTur
        {
            get {return  Equals(Turn, SideDirection.sideRight); }
            set
            {
                if (value)
                {
                    ChangeModelChanged((int)Turn, (int)SideDirection.sideRight, (int)_applyTurn);
                    Turn = SideDirection.sideRight;
                }
            }

        }

        public bool LeftTur
        {
            get {return Equals(Turn,SideDirection.sideLeft);}
            set
            {
                if (value)
                {
                    ChangeModelChanged(Turn, SideDirection.sideLeft, _applyTurn);
                    
                    Turn = SideDirection.sideLeft;
                }
            }

        
        }
        
        #endregion

        #region Moc
        public List<double> MocList { get; set; }
        
        private double _curMoc;
        public double CurMoc
        {
            get { return Common.ConvertHeight(_curMoc,roundType.toNearest); }
            set
            {
                _curMoc = Common.DeConvertHeight(value);
                if (MocChanged != null)
                    MocChanged(this, new EventArgs());
            }
        }
        
#endregion

        #region WD
        private double _wd;

        public double WD
        {
            get { return Common.ConvertDistance(_wd,roundType.toNearest); }
            set
            {
                double tmp = Common.DeConvertDistance(value);
                if (tmp == _wd)
                    return;
                ChangeModelChanged(_wd, tmp, _applyWD);
                _wd = tmp;
            }
        }
        #endregion

        //#region Changed

        //private bool _changed;
        //public bool ModelChanged
        //{
        //    get
        //    {
        //        return _changed;
        //    }
        //    set
        //    {
        //        //if (_changed == value)
        //        //    return;
        //        _changed = value;
        //        if (ModelChangedEventHandler != null)
        //            ModelChangedEventHandler(this, new ModelChangedEventArgs(value));
        //    }
        //}
        //#endregion

        #endregion

        #region :>Methods

        public override void SetApplyParams()
        {
            _applyAltitude = _altitude;
            _applyIas = _ias;
            _applyTurn = Turn;
            _applyRadial = _radial;
            _applyWD = _wd;
        }

             
        private void SetIasParams()
        {
             if (_altitudeIndex == 0)
            {
                if (Condition ==ConditionType.normal)
                {
                    _minIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo4250Min)[(int)_curCategory];
                    _maxIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo4250Max)[(int)_curCategory];
                }
                else if (Condition ==ConditionType.turbo)
                {
                    _minIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo4250Turb)[(int)_curCategory];
                    _maxIas = _minIas;
                }
            }
            else if (_altitudeIndex == 1)
            {
                if (Condition ==ConditionType.normal)
                {
                    _minIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo6100Min)[(int)_curCategory];
                    _maxIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo6100Max)[(int)_curCategory];
                }
                else if (Condition ==ConditionType.turbo)
                {
                    _minIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo6100Turb)[(int)_curCategory];
                    _maxIas = _minIas;
                }
            }
            else if (_altitudeIndex == 2)
            {
                if (Condition == ConditionType.normal)
                {
                    _minIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo10350Min)[(int)_curCategory];
                    _maxIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo10350Max)[(int)_curCategory];
                }
                else if (Condition == ConditionType.turbo)
                {
                    _minIas = _aircraftCategoryList.Constant(eAircraftCategoryData.hldIASupTo4250Turb)[(int)_curCategory];
                    _maxIas = _minIas;
                }
            }
            Ias = Common.ConvertSpeed_(Common.AdaptToInterval(_ias, _minIas, _maxIas, InitHolding.SpeedPrecision),roundType.toNearest);
           
        }

        private bool CategoryChange(categories newValue)
        {
            if (_curCategory == newValue)
                return false;
            if (newValue == categories.A || newValue == categories.B)
            {
                _maxAltitude = Common.SecondAltitude;
                _minAltitude = InitHolding.MinCatAltitudeVal;
            }
            else
                _maxAltitude = Common.MaxAltitude;
           
            AltitudeChange(Common.AdaptToInterval(_altitude, _minAltitude, _maxAltitude, InitHolding.HeightPrecision));
            _curCategory = newValue;
            return true;
        }

        private bool AltitudeChange(double newValue)
        {
            if (_altitude == newValue)
                return false;
            int altindex = 0;
            if ((newValue >= Common.MinAltitude) && (newValue <= Common.SecondAltitude))
                altindex = 0;
            else if ((newValue > Common.SecondAltitude) && (newValue <= Common.ThirdAltitude))
                altindex = 1;
            else if ((newValue > Common.ThirdAltitude) && (newValue <= Common.MaxAltitude))
                altindex = 2;

          //  _altitude = newValue;
            if (_altitudeIndex != altindex)
            {
                _altitudeIndex = altindex;
                return true;
            }
            else
            {                
                return false;
            }

        }

        private void CalculateDoc(double altitude)
        {
            double docMax;            
            docMax = 4110 * Math.Sqrt(altitude);
            DOC = docMax > Common.constDoc ? Common.constDoc : docMax;
            DOCMin = altitude * Math.Tan(35 * Math.PI / 180);
        }

#endregion

        #region specialView

        public string MinMaxIas
        {
            get 
            {
                double tmpMinIas = Common.ConvertSpeed_(_minIas, roundType.toUp);
                double tmpMaxIas = Common.ConvertSpeed_(_maxIas, roundType.toDown);
                if (tmpMinIas > tmpMaxIas)
                {
                    double tmp = Common.ConvertSpeed_(_ias, roundType.toNearest);
                    return tmp + " / " + tmp;
                }
                else
                    return Common.ConvertSpeed_(_minIas,roundType.toUp).ToString() + " / " + Common.ConvertSpeed_(_maxIas,roundType.toDown).ToString(); 
            }
        }

        public string MinMaxAltitude
        {
            get { return MinAltitude.ToString() + "/" + MaxAltitude.ToString(); }
        }

        #endregion



        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SpecialParamChanged;
        public event EventHandler AltitudeChanged;
        public event EventHandler MocChanged;
        //public event ModelChangedEventHandler ModelChangedEventHandler;
    

}
}
