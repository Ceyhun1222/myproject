using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;

namespace Holding.Models
{
    public class ModelAreaParams : Changed, INotifyPropertyChanged
    {

        #region :>Fields

        private AircraftCategoryList _aircraftCategoryList;
        private double _applyAltitude, _applyRadial, _applyIas,
            _applyWd, _minAltitude, _maxAltitude;
        private TurnDirection _applyTurn;
        private const double _dT = 15;
        #endregion

        #region :> Constructor

        public ModelAreaParams(double altitude, double radial, double ias, categories aircraftCategory, TurnDirection turn, double moc, ConditionType condition,flightPhase fPhase)
        {
            _aircraftCategoryList = GlobalParams.Constant_G.AircraftCategory;

            //AircraftCategories = new List<categories> { categories.AB, categories.CD, categories.E };
            AircraftCategories = new List<CategoryClass>();

            if (fPhase == flightPhase.Enroute)
                _minAltitude = InitHolding.MaxAbCatAltitudeVal;
            else
                _minAltitude = InitHolding.MinCatAltitudeVal;

            _maxAltitude =5* InitHolding.MaxCdeCatAltitudeVal;
            MinRadialInDegree = -1;
            MaxRadialInDegree = 360;
            _radial = radial;
            _ias = ias;
            Turn = turn;
            CurMoc = Common.ConvertHeight(moc, roundType.toNearest);
            Condition = condition;
           // _curCategory =(int) aircraftCategory;
            //_catIndex = (int)categories.D;
            _altitude = altitude;
            _fPhase = fPhase;   


            MocList = new List<double>();
            for (int i = 0; i < 3; i++)
            {
                MocList.Add(Math.Round(Common.ConvertHeight(300 + 150 * i, roundType.toNearest), 0));
            }

            FillCategories();
            SetIasParams();

        }

        #endregion

        #region :>Property
        #region Altidue
        private double _altitudeIndex = -1;

        public double MinAltitude
        {
            get { return Common.ConvertHeight(_minAltitude, roundType.toUp); }

        }

        public double MaxAltitude { get { return Common.ConvertHeight(_maxAltitude, roundType.toDown); } }

        public double DOC { get; set; }

        public double DOCMin { get; set; }

        private double _altitude = 0;
        public double Altitude
        {
            get { return Common.ConvertHeight(_altitude, roundType.toNearest); }
            set
            {
                double tmp = Common.DeConvertHeight(value);
                if (Math.Abs(tmp - _altitude) < 0.1)
                    return;

                CalculateDoc(tmp);
                if (AltitudeChange(tmp))
                {
                    _altitude = tmp;
                    FillCategories();
                    SetIasParams();
                }

                ChangeModelChanged(_altitude, tmp, _applyAltitude);
                
                _altitude = tmp;

                if (AltitudeChanged != null)
                    AltitudeChanged(this, new EventArgs());

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Altitude"));
            }
        }


        #endregion

        #region Radial


        public double MinRadialInDegree { get; private set; }

        public double MaxRadialInDegree { get; private set; }

        private double _radial;
        public double Radial
        {
            get { return ARANMath.RadToDeg(_radial); }
            set
            {

                double tmpRad = ARANMath.DegToRad(value);
                if (tmpRad != _radial)
                {
                    ChangeModelChanged(_radial, tmpRad, _applyRadial);
                    _radial = tmpRad;

                    if (SpecialParamChanged != null)
                        SpecialParamChanged(this, new EventArgs());

                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Radial"));

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
            get { return Common.ConvertSpeed_(_minIas, roundType.toNearest); }
            set { _minIas = Common.DeConvertSpeed(value); }
        }

        private double _maxIas;
        public double MaxIas
        {
            get { return Common.ConvertSpeed_(_maxIas, roundType.toNearest); }
            set { _maxIas = Common.DeConvertSpeed(value); }
        }

        private double _ias;
        public double Ias
        {
            get { return Common.ConvertSpeed_(_ias, roundType.toNearest); }
            set
            {
                if (Math.Abs(_ias - Common.DeConvertSpeed(value)) < 0.2)
                    return;

                ChangeModelChanged(_ias, Common.DeConvertSpeed(value), _applyIas);
                _ias = Common.DeConvertSpeed(value);

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
                    FillCategories();
                    SetIasParams();
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("NormalCondition"));
                }
            }
        }
        
        public bool InitialApproachCondition
        {
            get { return Equals(Condition, ConditionType.initialApproach); }
            set
            {
                if (value)
                {
                    Condition = ConditionType.initialApproach;
                    FillCategories();
                    SetIasParams();
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("InitialApproachCondition"));
                }
            }
        }

        public bool TurboCondition
        {
            get { return Equals(Condition, ConditionType.turbo); }
            set
            {
                if (value)
                {
                    Condition = ConditionType.turbo;
                    FillCategories();
                    SetIasParams();
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("TurboCondition"));
                }
            }
        }


        #endregion

        #region AircraftCategory

        public List<CategoryClass> AircraftCategories { get; private set; }

        private CategoryClass _curCategory;
        public CategoryClass Category
        {
            get { return _curCategory; }
            set
            {
                if (CategoryChange(value))
                    SetIasParams();
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Category"));
            }
        }

        #endregion

        #region Turn

        public TurnDirection Turn { get; set; }

        public bool RightTur
        {
            get { return Equals(Turn, TurnDirection.CW); }
            set
            {
                if (value)
                {
                    ChangeModelChanged((int)Turn, (int)SideDirection.sideRight, (int)_applyTurn);
                    Turn = TurnDirection.CW;
                }
            }

        }

        public bool LeftTur
        {
            get { return Equals(Turn, TurnDirection.CCW); }
            set
            {
                if (value)
                {
                    ChangeModelChanged(Turn, SideDirection.sideLeft, _applyTurn);

                    Turn = TurnDirection.CCW;
                }
            }


        }

        #endregion

        #region Moc
        public List<double> MocList { get; set; }

        private double _curMoc;
        public double CurMoc
        {
            get { return Common.ConvertHeight(_curMoc, roundType.toNearest); }
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
            get { return Common.ConvertDistance(_wd, roundType.toNearest); }
            set
            {
                double tmp = Common.DeConvertDistance(value);
                if (tmp == _wd)
                    return;
                ChangeModelChanged(_wd, tmp, _applyWd);
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

        public bool IsEnroute
        {
            get { return _isEnroute; }
            set
            {
                _isEnroute = value;
                if (PropertyChanged!=null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsEnroute"));
            }
        }

        #endregion

        #region :>Methods

        public override void SetApplyParams()
        {
            _applyAltitude = _altitude;
            _applyIas = _ias;
            _applyTurn = Turn;
            _applyRadial = _radial;
            _applyWd = _wd;
        }

        public void ChangeIas(double ias)
        {
            _ias = Common.DeConvertSpeed(ias);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Ias"));
        }

        private void SetIasParams()
        {

            if (_fPhase != flightPhase.Enroute)
            {
                switch (Condition)
                {
                    case ConditionType.normal:
                        if (_altitude < 4250)
                        {
                            
                            if (_curCategory.Category == categories.AB)
                            {
                                _minIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MinNormalTerminal][aircraftCategory.acA];
                                _maxIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MaxNormalTerminal][aircraftCategory.acA];
                            }
                            else
                            {
                                _minIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MinNormalTerminal][aircraftCategory.acC];
                                _maxIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MaxNormalTerminal][aircraftCategory.acC];
                            }

                        }
                        else if (_altitude <= 6100)
                        {
                            if (_curCategory.Category == categories.CDE)
                            {
                                _minIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo6100MinNormalTerminal][aircraftCategory.acC];
                                _maxIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo6100MaxNormalTerminal][aircraftCategory.acC];
                            }
                        }
                        else if (_altitude <= 10350)
                        {
                            if (_curCategory.Category == categories.CDE)
                            {
                                _minIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo10350MinNormalTerminal][aircraftCategory.acC];
                                _maxIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo10350MaxNormalTerminal][aircraftCategory.acC];
                            }
                        }
                        else
                        {
                            if (_curCategory.Category == categories.CDE)
                            {
                                _minIas = CalculateMach()*0.83;
                                _maxIas = CalculateMach()*0.83;
                            }
                        }
                        break;
                    case ConditionType.turbo:
                        if (_altitude < 4250)
                        {
                            if (_curCategory.Category == categories.AB)
                            {
                                _minIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MinTurbulenceTerminal][aircraftCategory.acA];
                                _maxIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MaxTurbulenceTerminal][aircraftCategory.acA];
                            }

                            if (_curCategory.Category == categories.CDE)
                            {
                                _minIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MinTurbulenceTerminal][aircraftCategory.acC];
                                _maxIas =
                                    GlobalParams.Constant_G.AircraftCategory[
                                        aircraftCategoryData.hldIASUpTo4250MaxTurbulenceTerminal][aircraftCategory.acC];
                            }
                        }
                        else if (_altitude <= 6100)
                        {
                            if (_curCategory.Category == categories.CDE)
                            {
                                var tmpMarch = CalculateMach() * 0.8;
                                var tmpTurboValue = GlobalParams.Constant_G.AircraftCategory[
                                       aircraftCategoryData.hldIASUpTo4250MaxTurbulenceTerminal][aircraftCategory.acC];

                                _minIas = tmpMarch < tmpTurboValue ? tmpMarch : tmpTurboValue;
                                _maxIas = _minIas;
                            }
                        }
                        else if (_altitude <= 10350)
                        {
                            if (_curCategory.Category == categories.CDE)
                            {
                                var tmpMarch = CalculateMach() * 0.8;
                                var tmpTurboValue = GlobalParams.Constant_G.AircraftCategory[
                                       aircraftCategoryData.hldIASUpTo4250MaxTurbulenceTerminal][aircraftCategory.acC];

                                _minIas = tmpMarch < tmpTurboValue ? tmpMarch : tmpTurboValue;
                                _maxIas = _minIas;
                            }
                        }
                        else
                        {
                            if (_curCategory.Category == categories.CDE)
                            {
                                _minIas = CalculateMach() * 0.83;
                                _maxIas = CalculateMach() * 0.83;
                            }
                        }
                        break;
                    case ConditionType.initialApproach:

                        if (_curCategory.Category == categories.A)
                        {
                            _minIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMinInitialApproachTerminal][aircraftCategory.acA];
                            _maxIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMaxInitialApproachTerminal][
                                        aircraftCategory.acA];
                        }
                        else if (_curCategory.Category == categories.B)
                        {
                            _minIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMinInitialApproachTerminal][aircraftCategory.acB];
                            _maxIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMaxInitialApproachTerminal][aircraftCategory.acB];
                        }
                        else if (_curCategory.Category == categories.C)
                        {
                            _minIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMinInitialApproachTerminal][aircraftCategory.acC];
                            _maxIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMaxInitialApproachTerminal][aircraftCategory.acC];
                        }
                        else if (_curCategory.Category == categories.D)
                        {
                            _minIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMinInitialApproachTerminal][aircraftCategory.acD];
                            _maxIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMaxInitialApproachTerminal][aircraftCategory.acD];
                        }

                        else if (_curCategory.Category == categories.E)
                        {
                            _minIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMinInitialApproachTerminal][aircraftCategory.acE];
                            _maxIas =
                                GlobalParams.Constant_G.AircraftCategory[
                                    aircraftCategoryData.hldIASMaxInitialApproachTerminal][aircraftCategory.acE];
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _minIas =
                    GlobalParams.Constant_G.AircraftCategory[aircraftCategoryData.hldIASEnroute][
                        aircraftCategory.acA];
                _maxIas =
                    GlobalParams.Constant_G.AircraftCategory[aircraftCategoryData.hldIASEnroute][
                        aircraftCategory.acA];
            }
            Ias = Common.ConvertSpeed_(_minIas, roundType.toDown);
            //if (_altitudeIndex == 0)
            //{
            //    if (Condition == ConditionType.normal)
            //    {
            //        _minIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo4250Min][_catIndex];
            //        _maxIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo4250Max][_catIndex];
            //        tmpIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo4250][_catIndex];
            //    }
            //    else if (Condition == ConditionType.turbo)
            //    {
            //        _minIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo4250Turb][_catIndex];
            //        _maxIas = _minIas;
            //        tmpIas = _maxIas;
            //    }
            //}
            //else if (_altitudeIndex == 1)
            //{
            //    if (Condition == ConditionType.normal)
            //    {
            //        _minIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo6100Min][_catIndex];
            //        _maxIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo6100Max][_catIndex];
            //        tmpIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo6100][_catIndex];
            //    }
            //    else if (Condition == ConditionType.turbo)
            //    {
            //        _minIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo6100Turb][_catIndex];
            //        _maxIas = _minIas;
            //        tmpIas = _maxIas;
            //    }
            //}
            //else if (_altitudeIndex == 2)
            //{
            //    if (Condition == ConditionType.normal)
            //    {
            //        _minIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo10350Min][_catIndex];
            //        _maxIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo10350Max][_catIndex];
            //        tmpIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo10350][_catIndex];
            //    }
            //    else if (Condition == ConditionType.turbo)
            //    {
            //        _minIas = _aircraftCategoryList[aircraftCategoryData.hldIASupTo4250Turb][_catIndex];
            //        _maxIas = _minIas;
            //        tmpIas = _maxIas;
            //    }
            //}
            //  Ias = Common.ConvertSpeed_(tmpIas, roundType.toNearest);

        }

        private bool CategoryChange(CategoryClass newValue)
        {
            bool result = true;
            
            if (newValue != null && _curCategory != null && _curCategory.Category == newValue.Category)
            {
                result = false;
            }
            _curCategory = newValue;
            return result;
        }

        private void FillCategories()
        {
            AircraftCategories.Clear();
            if (_fPhase == flightPhase.Enroute)
                return;
            switch (Condition)
            {
                case ConditionType.normal:
                    if (_altitude < 4250)
                    {
                        AircraftCategories.Clear();
                        AircraftCategories.Add(new CategoryClass {Category = categories.AB, Name = "A/B"});
                        AircraftCategories.Add(new CategoryClass {Category = categories.CDE, Name = "C/D/E"});
                        
                    }
                    else
                    {
                        AircraftCategories.Clear();
                        AircraftCategories.Add(new CategoryClass {Category = categories.CDE, Name = "C/D/E"});
                    }
                    break;
                case  ConditionType.turbo:
                    if (_altitude < 4250)
                    {
                        AircraftCategories.Clear();
                        AircraftCategories.Add(new CategoryClass {Category = categories.AB, Name = "A/B"});
                        AircraftCategories.Add(new CategoryClass {Category = categories.CDE, Name = "C/D/E"});
                    }
                    else
                    {
                        AircraftCategories.Clear();
                        AircraftCategories.Add(new CategoryClass { Category = categories.CDE, Name = "C/D/E" });
                    }
                    break;
                case ConditionType.initialApproach:
                     AircraftCategories.Clear();
                        AircraftCategories.Add(new CategoryClass {Category = categories.A, Name = "A"});
                        AircraftCategories.Add(new CategoryClass { Category = categories.B, Name = "B" });
                        AircraftCategories.Add(new CategoryClass { Category = categories.C, Name = "C" });
                        AircraftCategories.Add(new CategoryClass { Category = categories.D, Name = "D" });
                        AircraftCategories.Add(new CategoryClass { Category = categories.E, Name = "E" });
                    break;
            }
            if (AircraftCategories.Count > 0)
            {
                if (_curCategory != null)
                {
                    var catt = AircraftCategories.FirstOrDefault(cat => cat.Category == _curCategory.Category);
                    if (catt == null)
                        Category = AircraftCategories[0];
                    else
                        Category = catt;
                }
                else
                    Category = AircraftCategories[0];
            }
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
            else
                altindex = 3;

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

        public double CalculateMach(double value = 1)
        {
            double h = _altitude;
            if (_altitude > 11000)
                h = 11000;
            double result = System.Math.Sqrt((273.15 + _dT) - h * 6.5 / 1000) * 20.046796 * value;
            return result;
        }


        #endregion

        #region specialView

        public string MinMaxIas
        {
            get
            {
                double tmpMinIas = Common.ConvertSpeed_(_minIas, roundType.toDown);
                double tmpMaxIas = Common.ConvertSpeed_(_maxIas, roundType.toDown);
                if (tmpMinIas > tmpMaxIas)
                {
                    double tmp = Common.ConvertSpeed_(_ias, roundType.toDown);
                    return tmp + " / " + tmp;
                }
                else
                    return tmpMinIas + " / " + tmpMaxIas.ToString();
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
        private flightPhase _fPhase;
        private bool _isEnroute;
        // public event ModelChangedEventHandler ModelChangedEventHandler;

        internal void FlightPhaseChanged(PhaseRecieverCondition phaseRecieverCondition)
        {
            _fPhase = flightPhase.STARUpTo30;
            if (phaseRecieverCondition.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
            {
                _fPhase = flightPhase.Enroute;
                _minAltitude = InitHolding.MaxAbCatAltitudeVal;
                IsEnroute = false;
            }
            else
            {
                _minAltitude = InitHolding.MinCatAltitudeVal;
                IsEnroute = true;
            }

            FillCategories();
            SetIasParams();
        }
    }
}
