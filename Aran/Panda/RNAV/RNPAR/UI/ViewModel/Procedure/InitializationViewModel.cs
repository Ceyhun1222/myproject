using Aran.Geometries;
using Aran.Panda.RNAV.RNPAR.Context;
using Aran.Panda.RNAV.RNPAR.Utils;
using Aran.PANDA.Common;
using Aran.PANDA.Constants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure
{
    

    class InitializationViewModel : UnitViewModel
    {
       
        public InitializationViewModel(MainViewModel main) : base(main)
        {

            Header = "Final approach track";
            Runways = new ObservableCollection<Runway>();
            Env.Current.DataContext.RWYList.ToList().ForEach(t => Runways.Add(new Runway(t)));
            if (Runways.Count > 0)
                _selectedRunway = Runways.First();

            Categories = new ObservableCollection<AircraftCategory>();
            ((AircraftCategory[])Enum.GetValues(typeof(AircraftCategory))).ToList().ForEach(t => Categories.Add(t));
            _selectedAircraftCategory = AircraftCategory.A;

            AltimeterMarginTypes = new ObservableCollection<AltimeterMargin>();
            ((AltimeterMargin[])Enum.GetValues(typeof(AltimeterMargin))).ToList().ForEach(t => AltimeterMarginTypes.Add(t));
            _selectedAltimeterMarginType = Procedure.AltimeterMargin.RA;

            Env.Current.RNPContext.PreFinalPhase.Init();
            Phase = Env.Current.RNPContext.PreFinalPhase;

            _datumHeight = UnitConverter.HeightToDisplayUnits(State._GP_RDH, eRoundMode.NEAREST);
            _altimeterTHR = System.Math.Round(State._AlignP_THR, 2);
            _FASRNPValue = UnitConverter.DistanceToDisplayUnits(State._FASRNPval, eRoundMode.NEAREST);
            _MARNPValue = UnitConverter.DistanceToDisplayUnits(State._MARNPval, eRoundMode.NEAREST);
            _dh = UnitConverter.HeightToDisplayUnits(State._PrelDHval, eRoundMode.NEAREST);

            //  ?.?.? need to be calculated
            _maxFAPAlt = UnitConverter.HeightToDisplayUnits(600, eRoundMode.NEAREST);
            _minTemperature = State._Tmin;

            State.MAGUpDwnChanged(_missedApproachGradient);
            SetVPA(VPA);
            SetRunway(SelectedRunway);
            SetAircraftCategory(_selectedAircraftCategory);
            SetAltimeterMarginType(_selectedAltimeterMarginType);
            SetMaxFAPAlt(MaxFAPAlt);
            State.CreateMAProtArea();
        }

        public InitializationViewModel(MainViewModel main, StateViewModel previous) : base(main, previous)
        {
        }


        public PreFinalPhase Phase { get; }
        public PreFinalState State => Phase.CurrentState;


        private Runway _selectedRunway;
        public Runway SelectedRunway
        {
            get => _selectedRunway;

            set
            {
                SetRunway(value);
                State.CreateMAProtArea();
            }
        }


        public void SetRunway(Runway runway)
        {
            Set(() => SelectedRunway, ref _selectedRunway, runway);

            NativeMethods.ShowPandaBox(Env.Current.SystemContext.EnvWin32Window.ToInt32());
            RWYType rwy = Env.Current.DataContext.RWYList.ToList().First(t => t.Identifier.Equals(runway.Identifier));
            State._SelectedRWY = rwy;

            State.ptTHRprj = rwy.pPtPrj[eRWY.ptTHR];
            State.ptTHRgeo = rwy.pPtGeo[eRWY.ptTHR];
            State._RWYDir = State.ptTHRprj.M;

            Env.Current.DataContext.GetObstaclesByDist(State.ptTHRgeo.Z);

            THR = UnitConverter.HeightToDisplayUnits(State.ptTHRgeo.Z, eRoundMode.NEAREST);
            TrueBearing = State.ptTHRgeo.M;
            SetFAOffsetAngle(FAOffsetAngle);


            State._OCHbyObctacle = 75.0;

            CalcOCHMin();
            State.CreateOFZPlanes();
            CalcOCHMin();

            NativeMethods.HidePandaBox();
        }


        private void CalcOCHMin()
        {
            double OldOCHMin = State._OCHMin;
            State.CalcOCHMin();
            if (State._OCHMin != OldOCHMin)
            {
                MinOCH = UnitConverter.HeightToDisplayUnits(State._OCHMin, eRoundMode.NEAREST);
                SetDH(DH);
            }
        }


        public ObservableCollection<Runway> Runways { get; }




        private double _trueBearing;
        public double TrueBearing
        {
            get => _trueBearing;
            set
            {
                Set(() => TrueBearing, ref _trueBearing, value);
            }
        }



        private double _ias;
        public double IAS
        {
            get => _ias;
            set
            {
                Set(() => IAS, ref _ias, value);
            }
        }


        private decimal _vpa = 3.0m;
        public decimal VPA
        {
            get => _vpa;
            set => SetVPA(value);
        }

        public void SetVPA(decimal vpa)
        {
            Set(() => VPA, ref _vpa, vpa);
            State.SetVPA((double)vpa);
            SetAltimeterMarginType(SelectedAltimeterMarginType);
        }

        private decimal _vpaMax = 5.0m;
        public decimal VPAMax
        {
            get => _vpaMax;
            set
            {
                Set(() => VPAMax, ref _vpaMax, value);
            }
        }

        private decimal _vpaMin = 3.0m;
        public decimal VPAMin
        {
            get => _vpaMin;
            set
            {
                Set(() => VPAMin, ref _vpaMin, value);
            }
        }

        private decimal _missedApproachGradient= 2.5m;

        public decimal MissedApproachGradient
        {
            get => _missedApproachGradient;
            set
            {
                State.MAGUpDwnChanged(value);
                State.CreateMAProtArea();
                Set(() => MissedApproachGradient, ref _missedApproachGradient, value);
            }
        }


        private decimal _FAOffsetAngle = 0.0m;
        public decimal FAOffsetAngle
        {
            get => _FAOffsetAngle;
            set
            {
                SetFAOffsetAngle(value);
                State.CreateMAProtArea();
                FATrueBearing = State._ArCourse;
            }
        }

        public void SetFAOffsetAngle(decimal angle)
        {
            Set(() => FAOffsetAngle, ref _FAOffsetAngle, angle);
            State.SetFAOffsetAngle((double)angle);  
        }



        private double _FATrueBearing;
        public double FATrueBearing
        {
            get => _FATrueBearing;
            set
            {
                Set(() => FATrueBearing, ref _FATrueBearing, value);
            }
        }


        private double _FASRNPValue;
        public double FASRNPValue
        {
            get => _FASRNPValue;
            set
            {
                SetFASRNPValue(value);
                State.CreateMAProtArea();
            }
        }

        private void SetFASRNPValue(double val)
        {
            State.SetFASRNPValue(UnitConverter.DistanceToInternalUnits(val));
            Set(() => FASRNPValue, ref _FASRNPValue, UnitConverter.DistanceToDisplayUnits(State._FASRNPval, eRoundMode.NEAREST));

        }

        private double _dh;
        public double DH
        {
            get => _dh;
            set
            {
                double oldPrelDHval = State._PrelDHval;
                SetDH(value);
                if (oldPrelDHval != State._PrelDHval)
                {
                    SetTWC(TWC);
                    State.CreateMAProtArea();
                }
            }
        }


        public void SetDH(double dh)
        {
            State.SetDH(UnitConverter.HeightToInternalUnits(dh));
            dh = UnitConverter.HeightToDisplayUnits(State._PrelDHval, eRoundMode.NEAREST);
            Set(() => DH, ref _dh, dh);
        }




        private double _thr;
        public double THR
        {
            get => _thr;
            set
            {
                Set(() => THR, ref _thr, value);
            }
        }


        private double _datumHeight;

        public double DatumHeight
        {
            get => _datumHeight;
            set
            {
                double oldPrelDHval = State._PrelDHval;
                SetDatumHeight(value);
                if (oldPrelDHval != State._PrelDHval)
                {
                    SetTWC(TWC);
                    State.CreateMAProtArea();
                }     
            }
        }

        public void SetDatumHeight(double dh)
        {
            State.SetDatumHeight(UnitConverter.HeightToInternalUnits(dh));
            Set(() => DatumHeight, ref _datumHeight, UnitConverter.HeightToDisplayUnits(State._GP_RDH, eRoundMode.NEAREST));
            SetAltimeterTHR(AltimeterTHR);
        }


        private double _twc;

        public double TWC
        {
            get => _twc;
            set => SetTWC(value);
        }


        public void SetTWC(double twc)
        {
            State.CalcTWC(UnitConverter.SpeedToInternalUnits(twc));
            twc = UnitConverter.SpeedToDisplayUnits(State._CurrTWC);
            Set(() => TWC, ref _twc, twc);
        }


        private double _minTemperature;

        public double MinTemperature
        {
            get => _minTemperature;
            set
            {
                State.SetMinTemperature(value);
                Set(() => MinTemperature, ref _minTemperature, State._Tmin);
                if (State._PrelFAPalt > 0)
                {
                    NativeMethods.ShowPandaBox(Context.AppEnvironment.Current.SystemContext.EnvWin32Window.ToInt32());
                    State.FillFAMAObstaclesFields(State._PrelFAPalt);
                    NativeMethods.HidePandaBox();
                }

            }
        }


        private double _altimeterMargin;

        public double AltimeterMargin
        {
            get => _altimeterMargin;
            set
            {
                Set(() => AltimeterMargin, ref _altimeterMargin, value);
               
            }
        }



        private double _altimeterTHR;

        public double AltimeterTHR
        {
            get => _altimeterTHR;
            set
            {
                double oldPrelDHval = State._PrelDHval;
                SetAltimeterTHR(value);
                if (oldPrelDHval != State._PrelDHval)
                {
                    SetTWC(TWC);
                    State.CreateMAProtArea();
                }
            }
        }

        public void SetAltimeterTHR(double altimeterTHR)
        {
            altimeterTHR = State.SetAlitemertTHR(altimeterTHR);
            Set(() => AltimeterTHR, ref _altimeterTHR, altimeterTHR);
            State.CalcOCHByAlighment();
            CalcOCHMin();
        }

        private double _minOCH;

        public double MinOCH
        {
            get => _minOCH;
            set
            {
                Set(() => MinOCH, ref _minOCH, value);
            }
        }


        private double _MARNPValue;

        public double MARNPValue
        {
            get => _MARNPValue;
            set
            {
                SetMapRNPValue(value);
                State.CreateMAProtArea();
            }
        }

        private void SetMapRNPValue(double val)
        {
            State.SetMapRNPValue(UnitConverter.DistanceToInternalUnits(val));
            Set(() => MARNPValue, ref _MARNPValue, UnitConverter.DistanceToDisplayUnits(State._MARNPval, eRoundMode.NEAREST));
        }



        private double _maxFAPAlt;
        public double MaxFAPAlt
        {
            get => _maxFAPAlt;
            set
            {
                SetMaxFAPAlt(value);
                State.CreateMAProtArea();
            }
        }

        public void SetMaxFAPAlt(double maxFap)
        {
            State.SetMaxFAPAlt(UnitConverter.HeightToInternalUnits(maxFap));
            Set(() => MaxFAPAlt, ref _maxFAPAlt, UnitConverter.HeightToDisplayUnits(State._PrelFAPalt, eRoundMode.NEAREST));
            Set(() => TWC, ref _twc, UnitConverter.SpeedToDisplayUnits(State._MaxTWC));
           
        }

        AltimeterMargin _selectedAltimeterMarginType;
        public AltimeterMargin SelectedAltimeterMarginType
        {
            get => _selectedAltimeterMarginType;
            set => SetAltimeterMarginType(value);
        }


        public void SetAltimeterMarginType(AltimeterMargin margin)
        {
            Set(() => SelectedAltimeterMarginType, ref _selectedAltimeterMarginType, margin);
            State.SetAltimeterMargin((int)margin);
            AltimeterMargin = UnitConverter.HeightToDisplayUnits(State._HeightLoss, eRoundMode.NEAREST);
        }

        public ObservableCollection<AltimeterMargin> AltimeterMarginTypes { get; }



        private AircraftCategory _selectedAircraftCategory;

        public AircraftCategory SelectedAircraftCategory
        {
            get => _selectedAircraftCategory;
            set
            {
                SetAircraftCategory(value);
                State.CreateMAProtArea();
            }
        }

        public void SetAircraftCategory(AircraftCategory category)
        {
            State.SetCategory((int)category);
            State.SetAltimeterMargin((int)SelectedAltimeterMarginType);
            SetAltimeterMarginType(SelectedAltimeterMarginType);
            SetDatumHeight(DatumHeight);
           
            Set(() => SelectedAircraftCategory, ref _selectedAircraftCategory, category);
            IAS = UnitConverter.SpeedToDisplayUnits(State._IAS);
            VPAMin = State._VPAMin;
            VPAMax = State._VPAMax;
        }

        public ObservableCollection<AircraftCategory> Categories { get; }

        public override bool CanNext()
        {
            return true;
        }

        public override bool CanPrevious()
        {
            return false;
        }

        protected override void next()
        {
            Phase.Commit();
            NextState = new FinalApproachViewModel(mainViewModel, this);
        }

        protected override void previous()
        {

        }

        protected override void _destroy()
        {
            Phase.Clear();
        }

        protected override void ReInit()
        {
            Phase.ReInit();
        }

        protected override void saveReport()
        {
            
        }
    }

    class Runway
    {
        public string Name { get; set; }
        public Guid Identifier { get; set; }

        public Runway(RWYType rwy)
        {
            Name = rwy.Name;
            Identifier = rwy.Identifier;
        }
    }

    enum AircraftCategory
    {
        A,
        B,
        C,
        D,
        DL
    }

    enum AltimeterMargin
    {
        [Description("Radio altimeter")]
        RA,
        [Description("Pressure altimeter")]
        PA
    }


}
