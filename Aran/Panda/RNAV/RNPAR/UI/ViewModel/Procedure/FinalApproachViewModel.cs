using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.PANDA.Common;
using GalaSoft.MvvmLight.Command;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure
{
    class FinalApproachViewModel : UnitViewModel
    {
        public FinalApproachViewModel(MainViewModel main) : base(main)
        {
            Init();
        }


        public FinalApproachViewModel(MainViewModel main, StateViewModel previous) : base(main, previous)
        {
            Init();
        }

        public void Init()
        {
            Header = "FAS geometry construction";
            RfWptList = new ObservableCollection<WPT>();
            StraightWptList = new ObservableCollection<WPT>();
            FropWptList = new ObservableCollection<WPT>();
            Phase.Init();
            SetValues();
        }

        private void SetValues()
        {
            Set(() => TurnDirection, ref _turnDirection, State.TurnDirection);
            Set(() => OasGradient, ref _oasGradient, UnitConverter.GradientToDisplayUnits(PreFinalState._OASgradient));
            Set(() => OasOrigin, ref _oasOrigin, UnitConverter.DistanceToDisplayUnits(PreFinalState._OASorigin));
            Set(() => ZSurfaceOrigin, ref _zSurfaceOrigin, UnitConverter.DistanceToDisplayUnits(PreFinalState._zOrigin));
            Set(() => TrD, ref _trD, UnitConverter.DistanceToDisplayUnits(PreFinalState._TrD));


            Set(() => RfLegRadius, ref _rfLegRadius, UnitConverter.DistanceToDisplayUnits(State._CurrFASLeg.Radius));
            Set(() => BankAngle, ref _bankAngle, Math.Round(State._CurrFASLeg.BankAngle, 1));
            Set(() => FropDistance, ref _fropDistance, UnitConverter.DistanceToDisplayUnits(State._CurrFASLeg.DistToNext));
            Set(() => FropAltitude, ref _fropAltitude, UnitConverter.HeightToDisplayUnits(State._CurrFASLeg.RollOutAltitude));
            Set(() => FapDistance, ref _fapDistance, UnitConverter.DistanceToDisplayUnits(State._CurrFASLeg.DistToNext));
            Set(() => FapAltitude, ref _fapAltitude, UnitConverter.HeightToDisplayUnits(State._CurrFASLeg.StartAltitude));
            Set(() => EntryTrueCourse, ref _entryTrueCourse, Math.Round(State._CurrFASLeg.Course, 2));
            Set(() => RfStartAltitude, ref _rfStartAltitude, UnitConverter.HeightToDisplayUnits(State._CurrFASLeg.StartAltitude));
            Set(() => FasTotalLength, ref _fasFasTotalLength, UnitConverter.DistanceToDisplayUnits(State.Length));
           
        }




        public FinalPhase Phase => Env.Current.RNPContext.FinalPhase;
        public FinalState State => Phase?.CurrentState;
        public PreFinalState PreFinalState => Env.Current.RNPContext.PreFinalPhase.CurrentState;

        public ObservableCollection<WPT> RfWptList { get; set; }
        public ObservableCollection<WPT> StraightWptList { get; set; }
        public ObservableCollection<WPT> FropWptList { get; set; }


        private WPT _selectedRfWpt;
        public WPT SelectedRfWpt
        {
            get => _selectedRfWpt;

            set
            {
                Set(() => SelectedRfWpt, ref _selectedRfWpt, value);
                if (SelectedRfWpt == null && RfWptList.Count > 0)
                    Set(() => SelectedRfWpt, ref _selectedRfWpt, RfWptList[0]);
                if (SelectedRfWpt != null)
                {
                    State.RfWptSelected(Env.Current.DataContext.WPTList.ToList()
                        .First(t => t.Identifier.Equals(SelectedRfWpt.Identifier)));
                    State.CreateFASLeg();
                }
                SetValues();
            }
        }

        private WPT _selectedStraightWpt;
        public WPT SelectedStraightWpt
        {
            get => _selectedStraightWpt;

            set
            {
                Set(() => SelectedStraightWpt, ref _selectedStraightWpt, value);
                if (SelectedStraightWpt == null && StraightWptList.Count > 0)
                    Set(() => SelectedStraightWpt, ref _selectedStraightWpt, StraightWptList[0]);
                if (SelectedStraightWpt != null)
                {
                    State.StraightWptSelected(Env.Current.DataContext.WPTList.ToList()
                        .First(t => t.Identifier.Equals(SelectedStraightWpt.Identifier)));
                    State.CreateFASLeg();
                }
                SetValues();
            }
        }

        private WPT _selectedFropWpt;
        public WPT SelectedFropWpt
        {
            get => _selectedFropWpt;

            set
            {
                Set(() => SelectedFropWpt, ref _selectedFropWpt, value);
                if (SelectedFropWpt == null && FropWptList.Count > 0)
                    Set(() => SelectedFropWpt, ref _selectedFropWpt, FropWptList[0]);
                if (SelectedFropWpt != null)
                {
                    State.FropWptSelected(Env.Current.DataContext.WPTList.ToList()
                        .First(t => t.Identifier.Equals(SelectedFropWpt.Identifier)));
                    
                    State.CreateFASLeg();
                    State.FillObstacles();
                }
                SetValues();
            }
        }

        private bool _rfLeg = true;
        public bool RfLeg
        {
            get => _rfLeg;
            set
            {
                Set(() => StraightLeg, ref _straightLeg, !value);
                Set(() => RfLeg, ref _rfLeg, value);
                SelectedTab = 0;
                UseRfRadius = true;
                State.LegTypeChanged(true);
                State.CreateFASLeg();
                SetValues();
            }
        }

        private bool _straightLeg;
        public bool StraightLeg
        {
            get => _straightLeg;
            set
            {
                Set(() => StraightLeg, ref _straightLeg, value);
                Set(() => RfLeg, ref _rfLeg, !value);
                SelectedTab = 1;
                UseFapLocation = true;
                State.LegTypeChanged(false);
                State.CreateFASLeg();
                SetValues();
            }
        }


        private int _selectedTab;

        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                Set(() => SelectedTab, ref _selectedTab, value);
            }
        }


        private bool _useRfRadius = true;

        public bool UseRfRadius
        {
            get => _useRfRadius;
            set
            {
                Set(() => UseRfRadius, ref _useRfRadius, value);
                Set(() => UseRfWpt, ref _useRfWpt, !value);
                State.IsWpt(false);
                RfWptList.Clear();
            }
        }



        private bool _useRfWpt = false;

        public bool UseRfWpt
        {
            get => _useRfWpt;

            set
            {
                Set(() => UseRfWpt, ref _useRfWpt, value);
                Set(() => UseRfRadius, ref _useRfRadius, !value);
                State.IsWpt(true);
                RefreshRfWpt();         
            }
        }

        private void RefreshRfWpt()
        {
            FillRfWptList();
            if (RfWptList.Count > 0)
                SelectedRfWpt = RfWptList[0];
        }

    
        private bool _useFropDistance = true;

        public bool UseFropDistance
        {
            get => _useFropDistance;
            set
            {
                Set(() => UseFropDistance, ref _useFropDistance, value);
                Set(() => UseFropWpt, ref (_useFropWpt), !value);
                if(value)
                    FropWptList.Clear();
            }
        }



        private bool _useStraightWpt = false;

        public bool UseStraightWpt
        {
            get => _useStraightWpt;

            set
            {
                Set(() => UseStraightWpt, ref _useStraightWpt, value);
                Set(() => UseFropDistance, ref _useFropDistance, !value);
                State.IsWpt(true);
                RefreshStraightWpt();
              
            }
        }

        private void RefreshStraightWpt()
        {
            FillStraightWptList();
            if (StraightWptList.Count > 0)
                SelectedStraightWpt = StraightWptList[0];
        }


        private bool _useFapLocation = true;

        public bool UseFapLocation
        {
            get => _useFapLocation;
            set
            {
                Set(() => UseFapLocation, ref _useFapLocation, value);
                Set(() => UseFropWpt, ref _useFropWpt, !value);
                State.IsWpt(false);
                FropWptList.Clear();
            }
        }



        private bool _useFropWpt = false;

        public bool UseFropWpt
        {
            get => _useFropWpt;

            set
            {
                Set(() => UseFropWpt, ref _useFropWpt, value);
                Set(() => UseFapLocation, ref _useFapLocation, !value);
                RefreshFropWpt();

            }
        }

        private void RefreshFropWpt()
        {
            FillFropWptList();
            if (FropWptList.Count > 0)
                SelectedFropWpt = FropWptList[0];
        }

        private int _turnDirection;

        public int TurnDirection
        {
            get => _turnDirection;

            set
            {
                State.TurnDirectionChanged(value);
                if(UseRfWpt)
                    RefreshRfWpt();
                else
                    State.CreateFASLeg();
                SetValues();
            }
        }


        private double _fropDistance;

        public double FropDistance
        {
            get => _fropDistance;
            set
            {
                State.CalcFropDistance(UnitConverter.DistanceToInternalUnits(value));
                if (State._FASLegs.Count == 0)
                {
                    State.RemoveFropWpt();
                }

                State.CreateFASLeg();
                State.FillObstacles();
                SetValues();
            }
        }


        private double _fropAltitude;

        public double FropAltitude
        {
            get => _fropAltitude;
            set { Set(() => FropAltitude, ref _fropAltitude, value, true); }
        }

        private double _bankAngle;

        public double BankAngle
        {
            get => _bankAngle;
            set
            {
                State.BankAngleChanged(value);
                State.CreateFASLeg();
                SetValues();
            }
        }


        private double _rfLegRadius;

        public double RfLegRadius
        {
            get => _rfLegRadius;
            set
            {
                State.RadiusChanged(UnitConverter.DistanceToInternalUnits(value));
                SetValues();
            }
        }


        private double _entryTrueCourse;

        public double EntryTrueCourse
        {
            get => _entryTrueCourse;
            set
            {
                State.CalcEntryCourse(value);
                State.CreateFASLeg();
                SetValues();
                //  Set(() => EntryTrueCourse, ref _entryTrueCourse, value, true);
            }
        }


        private double _rfStartAltitude;

        public double RfStartAltitude
        {
            get => _rfStartAltitude;
            set
            {
                Set(() => RfStartAltitude, ref _rfStartAltitude, value, true);
            }
        }


        private double _fapDistance;

        public double FapDistance
        {
            get => _fapDistance;
            set
            {
                State.CalcFapDistance(UnitConverter.DistanceToInternalUnits(value));
                State.CreateFASLeg();
                State.FillObstacles();
                SetValues();
            }
        }


        private double _fapAltitude;

        public double FapAltitude
        {
            get => _fapAltitude;
            set
            {
                Set(() => FapAltitude, ref _fapAltitude, value, true);
            }
        }


        private double _fasFasTotalLength;

        public double FasTotalLength
        {
            get => _fasFasTotalLength;
            set
            {
                Set(() => FasTotalLength, ref _fasFasTotalLength, value, true);
            }
        }

        private double _oasGradient;

        public double OasGradient
        {
            get => _oasGradient;
            set
            {
                Set(() => OasGradient, ref _oasGradient, value, true);
            }
        }


        private double _oasOrigin;

        public double OasOrigin
        {
            get => _oasOrigin;
            set
            {
                Set(() => OasOrigin, ref _oasOrigin, value, true);
            }
        }


        private double _zSurfaceOrigin;

        public double ZSurfaceOrigin
        {
            get => _zSurfaceOrigin;
            set
            {
                Set(() => ZSurfaceOrigin, ref _zSurfaceOrigin, value, true);
            }
        }

        private double _trD;

        public double TrD
        {
            get => _trD;
            set
            {
                Set(() => TrD, ref _trD, value, true);
            }
        }

        private RelayCommand _addCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                    ?? (_addCommand = new RelayCommand(
                    () =>
                    {
                        Phase.Commit();
                        RfLeg = true;
                        Committed = Phase.IsCommitted;
                    }));
            }
        }

        private RelayCommand _removeCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand RemoveCommand => _removeCommand
                                             ?? (_removeCommand = new RelayCommand(
                                                 Rollback));

        private void Rollback()
        {
            Phase.Rollback();
            if (State._CurrFASLeg.legType == Model.LegType.FixedRadius)
                RfLeg = true;

            if (State._CurrFASLeg.legType == Model.LegType.Straight)
                StraightLeg = true;
            
            Committed = Phase.IsCommitted;
        }


        private bool _committed = false;

        public bool Committed
        {
            get => _committed;
            set
            {
                Set(() => Committed, ref _committed, value);
            }
        }


        public override bool CanNext()
        {
            return true;
        }

        public override bool CanPrevious()
        {
            return true;
        }

        protected override void next()
        {
            State.LastLeg = true;
            Phase.Commit();
            NextState = new IntermediateApproachViewModel(mainViewModel, this);
        }

        protected override void previous()
        {
            Phase.Clear();
        }

        protected override void _destroy()
        {
            Phase.Clear();
        }

        protected override void ReInit()
        {
            Phase.ReInit();          
        }

        private void FillStraightWptList()
        {
            StraightWptList.Clear();
            State.GetStraightWptList(Env.Current.DataContext.WPTList).ToList()
                .ForEach(t => StraightWptList.Add(new WPT {Name = t.Name, Identifier = t.Identifier}));
        }

        private void FillRfWptList()
        {
            RfWptList.Clear();
            State.GetRfWptList(Env.Current.DataContext.WPTList).ToList()
                .ForEach(t => RfWptList.Add(new WPT {Name = t.Name, Identifier = t.Identifier}));
        }

        private void FillFropWptList()
        {
            FropWptList.Clear();
            State.GetFropWptList(Env.Current.DataContext.WPTList).ToList()
                .ForEach(t => FropWptList.Add(new WPT { Name = t.Name, Identifier = t.Identifier }));
        }

        protected override void saveReport()
        {

        }
    }

    public class WPT
    {
        public string Name { get; set; }
        public Guid Identifier { get; set; }
    }

    enum Turn
    {
        Left,
        Right
    }
}
