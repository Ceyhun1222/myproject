using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Aran.Panda.RNAV.RNPAR.Context;
using Aran.Panda.RNAV.RNPAR.Core.Model;
using Aran.Panda.RNAV.RNPAR.Report.Models.BasicData;
using Aran.Panda.RNAV.RNPAR.Report.Models.Paragraph;
using Aran.Panda.RNAV.RNPAR.Rules.Core;
using Aran.Panda.RNAV.RNPAR.Utils;
using GalaSoft.MvvmLight.Command;

namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure
{
    class MissedApproachViewModel : UnitViewModel
    {
        public MissedApproachViewModel(MainViewModel main) : base(main)
        {
            Init();
        }

        public MissedApproachViewModel(MainViewModel main, StateViewModel previous) : base(main, previous)
        {
            Init();
        }

        public MissedApproachPhase Phase => AppEnvironment.Current.RNPContext.MissedApproachPhase;
        public MissedApproachState State => Phase?.CurrentState;
        public PreFinalState PreFinalState => AppEnvironment.Current.RNPContext.PreFinalPhase.CurrentState;
        public FinalState FinalState => AppEnvironment.Current.RNPContext.FinalPhase.CurrentState;

        public void Init()
        {
            Header = "Missed approach segment";
            RfWptList = new ObservableCollection<WPT>();
            StraightWptList = new ObservableCollection<WPT>();
            FlyByWptList = new ObservableCollection<WPT>();
            Phase.Init();
            SetValues();
        }

        private void SetValues()
        {

            Set(() => WptAltitude, ref _wptAltitude, UnitConverter.HeightToDisplayUnits(State._CurrMASLeg.RollOutAltitude));
            Set(() => TurnRadius, ref _turnRadius, UnitConverter.DistanceToDisplayUnits(State._CurrMASLeg.Radius));
            Set(() => TrackDistance, ref _trackDistance, UnitConverter.DistanceToDisplayUnits(State.TrackDistance));
            Set(() => Ias, ref _ias, UnitConverter.SpeedToDisplayUnits(State._CurrMASLeg.IAS));
            Set(() => SegmentRnpValue, ref _segmentRnpValue, UnitConverter.DistanceToNM(State._CurrMASLeg.RNPvalue));
            Set(() => Moc, ref _moc, UnitConverter.HeightToDisplayUnits(State._CurrMASLeg.MOC));
            Set(() => TurnDirection, ref _turnDirection, State.TurnDirection);
            Set(() => BankAngle, ref _bankAngle, Math.Round(State._CurrMASLeg.BankAngle, 1));
            Set(() => EntryTrueCourse, ref _entryTrueCourse, Math.Round(State._CurrMASLeg.Course, 2));
            Set(() => RfLegEnabled, ref _rfLegEnabled, State.RfLegEnabled);
            Set(() => FlyByLegEnabled, ref _flyByLegEnabled, State.FlyByLegEnabled);
            Set(() => RfLeg, ref _rfLeg, State._CurrMASLeg.legType.Equals(Model.LegType.FixedRadius));
            Set(() => FlyByLeg, ref _flyByLeg, State._CurrMASLeg.legType.Equals(Model.LegType.FlyBy));
            Set(() => StraightLeg, ref _straightLeg, State._CurrMASLeg.legType.Equals(Model.LegType.Straight));

            Set(() => DG, ref _dg, (decimal)State._dg);
            Set(() => DGMax, ref _dgMax, (decimal)State.DGMax);
            Set(() => DGMin, ref _dgMin, (decimal)State.DGMax);
            SelectedTab = RfLeg ? 0 : (FlyByLeg ? 1 : 2);
        }



        public ObservableCollection<WPT> RfWptList { get; set; }
        public ObservableCollection<WPT> StraightWptList { get; set; }
        public ObservableCollection<WPT> FlyByWptList { get; set; }

        private bool _rfLegEnabled = true;
        public bool RfLegEnabled
        {
            get => _rfLegEnabled;
            set
            {
                Set(() => RfLegEnabled, ref _rfLegEnabled, value);
            }
        }


        private bool _rfLeg = true;
        public bool RfLeg
        {
            get => _rfLeg;
            set
            {
                if (!value)
                    return;
                UseRfRadius = true;
                State.LegTypeChanged(Model.LegType.FixedRadius);
                State.CreateMASLeg();
                SetValues();
                RfWptList.Clear();

            }
        }

        private bool _flyByLegEnabled;
        public bool FlyByLegEnabled
        {
            get => _flyByLegEnabled;
            set { Set(() => FlyByLegEnabled, ref _flyByLegEnabled, value); }
        }


        private bool _flyByLeg;
        public bool FlyByLeg
        {
            get => _flyByLeg;
            set
            {
                if (!value)
                    return;
                UseFlyByRadius = true;
                State.LegTypeChanged(Model.LegType.FlyBy);
                State.BankAngleChanged(18);
                State.CreateMASLeg();
                SetValues();
                FlyByWptList.Clear();
            }
        }

        private bool _straightLegEnabled;
        public bool StraightLegEnabled
        {
            get => _straightLegEnabled;
            set
            {
                Set(() => StraightLegEnabled, ref _straightLegEnabled, value);
            }
        }

        private bool _straightLeg;
        public bool StraightLeg
        {
            get => _straightLeg;
            set
            {
                if (!value)
                    return;
                UseTrackDistance = true;
                State.LegTypeChanged(Model.LegType.Straight);
                State.CreateMASLeg();
                SetValues();
                StraightWptList.Clear();
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
                RfWptList.Clear();
            }
        }


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
                    State.RfWptSelected(AppEnvironment.Current.DataContext.WPTList.ToList()
                        .First(t => t.Identifier.Equals(SelectedRfWpt.Identifier)));
                    State.CreateMASLeg();
                }
                SetValues();
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
                RefreshRfWpt();


            }
        }

        private void RefreshRfWpt()
        {
            FillRfWptList();
            if (RfWptList.Count > 0)
                SelectedRfWpt = RfWptList[0];
        }



        private void FillRfWptList()
        {
            RfWptList.Clear();
            State.GetRfWptList(AppEnvironment.Current.DataContext.WPTList).ToList()
                .ForEach(t => RfWptList.Add(new WPT { Name = t.Name, Identifier = t.Identifier }));
        }


        private bool _useFlyByRadius = true;

        public bool UseFlyByRadius
        {
            get => _useFlyByRadius;
            set
            {
                Set(() => UseFlyByRadius, ref _useFlyByRadius, value);
                Set(() => UseFlyByWpt, ref _useFlyByWpt, !value);
                FlyByWptList.Clear();
            }
        }


        private WPT _selectedFlyByWpt;
        public WPT SelectedFlyByWpt
        {
            get => _selectedFlyByWpt;

            set
            {
                Set(() => SelectedFlyByWpt, ref _selectedFlyByWpt, value);
                if (SelectedFlyByWpt == null && FlyByWptList.Count > 0)
                    Set(() => SelectedFlyByWpt, ref _selectedFlyByWpt, FlyByWptList[0]);
                if (SelectedFlyByWpt != null)
                {
                    State.FlyByWptSelected(AppEnvironment.Current.DataContext.WPTList.ToList()
                        .First(t => t.Identifier.Equals(SelectedFlyByWpt.Identifier)));
                    State.CreateMASLeg();
                }
                SetValues();
            }
        }

        private bool _useFlyByWpt = false;

        public bool UseFlyByWpt
        {
            get => _useFlyByWpt;

            set
            {
                Set(() => UseFlyByWpt, ref _useFlyByWpt, value);
                Set(() => UseFlyByRadius, ref _useFlyByRadius, !value);
                RefreshFlyByWpt();


            }
        }

        private void RefreshFlyByWpt()
        {
            FillFlyByWptList();
            if (FlyByWptList.Count > 0)
                SelectedFlyByWpt = FlyByWptList[0];
        }



        private void FillFlyByWptList()
        {
            FlyByWptList.Clear();
            State.GetFlyByWptList(AppEnvironment.Current.DataContext.WPTList).ToList()
                .ForEach(t => FlyByWptList.Add(new WPT { Name = t.Name, Identifier = t.Identifier }));
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
                    State.StraightWptSelected(AppEnvironment.Current.DataContext.WPTList.ToList()
                        .First(t => t.Identifier.Equals(SelectedStraightWpt.Identifier)));
                    State.CreateMASLeg();

                }
                SetValues();
            }
        }

        private void FillStraightWptList()
        {
            StraightWptList.Clear();
            State.GetStraightWptList(AppEnvironment.Current.DataContext.WPTList).ToList()
                .ForEach(t => StraightWptList.Add(new WPT { Name = t.Name, Identifier = t.Identifier }));
        }

        private bool _useStraightWpt;

        public bool UseStraightWpt
        {
            get => _useStraightWpt;

            set
            {
                Set(() => UseStraightWpt, ref _useStraightWpt, value);
                Set(() => UseTrackDistance, ref _useTrackDistance, !value);
                RefreshStraightWpt();

            }
        }

        private void RefreshStraightWpt()
        {
            FillStraightWptList();
            if (StraightWptList.Count > 0)
                SelectedStraightWpt = StraightWptList[0];
        }


        private bool _useTrackDistance = true;

        public bool UseTrackDistance
        {
            get => _useTrackDistance;
            set
            {
                Set(() => UseTrackDistance, ref _useTrackDistance, value);
                Set(() => UseStraightWpt, ref _useStraightWpt, !value);
                StraightWptList.Clear();
            }
        }

        private double _moc;

        public double Moc
        {
            get => _moc;
            set
            {
                State.MocChanged(UnitConverter.HeightToInternalUnits(value));
                SetValues();
            }
        }


        private double _wptAltitude;

        public double WptAltitude
        {
            get => _wptAltitude;
            set
            {
                State.WptAltitudeChanged(UnitConverter.HeightToInternalUnits(value));
                State.CreateMASLeg();
                SetValues();
            }
        }


        private double _segmentRnpValue;

        public double SegmentRnpValue
        {
            get => _segmentRnpValue;
            set
            {
                State.SegmentRnpValueChanged(value);
                State.CreateMASLeg();
                SetValues();
            }
        }

        private double _ias;

        public double Ias
        {
            get => _ias;
            set
            {
                State.IasChanged(UnitConverter.SpeedToInternalUnits(value));
                State.CreateMASLeg();
                SetValues();
            }
        }


        private decimal _dg = 3.0m;
        public decimal DG
        {
            get => _dg;
            set => SetDescentGradient(value);
        }

        public void SetDescentGradient(decimal dg)
        {
            State.SetDescentGradient((double)dg);
            State.CreateMASLeg();
            SetValues();
        }

        private decimal _dgMax = 5.0m;
        public decimal DGMax
        {
            get => _dgMax;
            set
            {
                Set(() => DGMax, ref _dgMax, value);
            }
        }


        private decimal _dgMin = 3.0m;
        public decimal DGMin
        {
            get => _dgMin;
            set
            {
                Set(() => DGMin, ref _dgMin, value);
            }
        }

        private int _turnDirection;

        public int TurnDirection
        {
            get => _turnDirection;

            set
            {
                State.TurnDirectionChanged(value);
                if (UseRfWpt)
                    RefreshRfWpt();
                else if (UseFlyByWpt)
                    RefreshFlyByWpt();
                else
                    State.CreateMASLeg();
                SetValues();
            }
        }

        private double _bankAngle;

        public double BankAngle
        {
            get => _bankAngle;
            set
            {
                State.BankAngleChanged(value);
                State.CreateMASLeg();
                SetValues();
            }
        }

        private double _turnRadius;

        public double TurnRadius
        {
            get => _turnRadius;
            set
            {
                State.RadiusChanged(UnitConverter.DistanceToInternalUnits(value));
                State.CreateMASLeg();
                SetValues();
            }
        }

        private double _entryTrueCourse;

        public double EntryTrueCourse
        {
            get => _entryTrueCourse;
            set
            {
                State.EntryCourseChanged(value);
                State.CreateMASLeg();
                SetValues();
            }
        }

        private double _trackDistance;

        public double TrackDistance
        {
            get => _trackDistance;
            set
            {
                State.TrackDistanceChanged(UnitConverter.DistanceToInternalUnits(value));
                State.CreateMASLeg();
                SetValues();
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
                               Committed = Phase.IsCommitted;
                               SetValues();
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
            if (State._CurrMASLeg.legType == Model.LegType.FixedRadius)
                RfLeg = true;

            if (State._CurrMASLeg.legType == Model.LegType.Straight)
                StraightLeg = true;

            if (State._CurrMASLeg.legType == Model.LegType.FlyBy)
                FlyByLeg = true;
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
            return false;
        }

        public override bool CanPrevious()
        {
            return true;
        }


        protected override void next()
        {
            NextState = new InitialApproachViewModel(mainViewModel, this);
        }

        protected override void previous()
        {
            PreFinalState.DrawMAProtArea();
            Phase.Clear();
        }

        protected override void _destroy()
        {
            Phase.Clear();
        }

        protected override void saveReport()
        {
            string RepFileName, RepFileTitle;

            if (!Functions.ShowSaveDialog(out RepFileName, out RepFileTitle))
                return;

            var pReport = new ReportHeader
            {
                Procedure = RepFileTitle,
                Database = AppEnvironment.Current.AranEnv.ConnectionInfo.Database,
                Aerodrome = AppEnvironment.Current.DataContext.CurrADHP.Name
            };

            var reportUtil = new Utils.Report();

            // reportUtil.SaveAccuracy(RepFileName, RepFileTitle, pReport);
            reportUtil.SaveLog(RepFileName, RepFileTitle, pReport);
            reportUtil.SaveProtocol(RepFileName, RepFileTitle, pReport);

            ReportPoint[] points;
            var length = reportUtil.ConvertTracToPoints(out points);
            reportUtil.SaveGeometry(RepFileName, RepFileTitle, pReport, points, length);
            SaveValidationReport(RepFileName);
        }

        private void SaveValidationReport(string fileName)
        {
            Report.Report report = new Report.Report("Validation report");
            report.Paragraphs.Add(CreateCriteriaParagraph());

            var html = report.GenerateHtml();

            File.WriteAllText(fileName + "_validation.html", html);
        }


        private Paragraph CreateCriteriaParagraph()
        {
            List<IRule> Rules = new List<IRule>();
            Rules.AddRange(AppEnvironment.Current.RNPContext.PreFinalPhase.CurrentState.Rules);
            Rules.AddRange(AppEnvironment.Current.RNPContext.FinalPhase.CurrentState.Rules);

            var MissedApproachRules = Rules.Where(t => t.Phase == SegmentType.MissedApproach).ToList();
            var FinalApproachRules = Rules.Where(t => t.Phase == SegmentType.Final).ToList();

            var paragraph = new Paragraph("Criteria Validation");
            var missedSubParagraph = new SubParagraph("Missed Approach segment");

            AddRules(MissedApproachRules, missedSubParagraph);
            paragraph.SubParagraphs.Add(missedSubParagraph);

            var finalSubParagraph = new SubParagraph("Final Approach segment");
            AddRules(FinalApproachRules, finalSubParagraph);
            paragraph.SubParagraphs.Add(finalSubParagraph);

            return paragraph;
        }

        public void AddRules(List<IRule> Rules, SubParagraph subParagraph)
        {
            foreach (var rule in Rules)
            {
                subParagraph.Contents.Add(new TextSubParagraphContent($"Criteria: {rule.Identifier}"));
                subParagraph.Contents.Add(new TextSubParagraphContent($"Text: {rule.Text}"));
                subParagraph.Contents.Add(new TextSubParagraphContent("Input data"));

                var inputDataTable = new BasicDataTable();
                for (int i = 0; i < rule.ParameterTypes.Count; i++)
                {
                    inputDataTable.Rows.Add(new KeyValueBasicDataTableRow(rule.ParameterTypes[i].Item1,
                        rule.Params[i].ToString()));
                }
                subParagraph.Contents.Add(inputDataTable);
                subParagraph.Contents.Add(new TextSubParagraphContent("Output data"));
                var outputDataTable = new BasicDataTable();
                outputDataTable.Rows.Add(new KeyValueBasicDataTableRow(rule.ResultType.Item1, rule.Result.ToString()));
                subParagraph.Contents.Add(outputDataTable);
            }
        }


        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               //  Phase.Commit();
                               //  Committed = Phase.IsCommitted;
                               //  SetValues();
                               State.AddLeg(false);
                               saveReport();
                               if (AppEnvironment.Current.DataContext.SaveProcedure())
                               {
                                   System.Windows.MessageBox.Show("Procedure successfully saved");
                                   

                               }

                               State.RemoveLeg();
                           }, () => true)
                           
                       );
            }
        }
    }
}