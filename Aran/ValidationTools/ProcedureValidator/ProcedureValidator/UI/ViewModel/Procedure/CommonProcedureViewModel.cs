using PVT.Model;
using System.Collections.ObjectModel;
using System;
using PVT.Graphics;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using PVT.Settings;
using System.Windows.Controls;
using System.Collections.Generic;
using Aran.PANDA.Common;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class CommonProcedureViewModel : StateViewModel
    {
        /// <summary>
        /// Initializes a new instance of the CommonProcedureViewModel class.
        /// </summary>
        /// 

        private RouteType _selectedRouteType;
        private readonly IProcedureDrawer _procedureDrawer;
        private readonly IHoldingDrawer _holdingDrawer;

        public DateTime EffectiveDate { get; }
        public string EffectiveDateString => $" {EffectiveDate:dd/MM/yyyy} 0000 UTC";


        public ObservableCollection<ApproachProcedure> ApproachProcedures { get; }
        public ObservableCollection<ArrivalProcedure> ArrivalProcedures { get; }
        public ObservableCollection<DepartureProcedure> DepartureProcedures { get; }
        public ObservableCollection<HoldingPattern> HoldingPatterns { get; }

        public ApproachProcedure CurrentApproachProcedure { get; set; }

        public ArrivalProcedure CurrentArrivalProcedure { get; set; }

        public DepartureProcedure CurrentDepartureProcedure { get; set; }

        public HoldingPattern CurrentHoldingPattern { get; set; }


        public CommonProcedureViewModel(MainViewModel main) : base(main)
        {
            ApproachProcedures = new ObservableCollection<ApproachProcedure>();
            ArrivalProcedures = new ObservableCollection<ArrivalProcedure>();
            DepartureProcedures = new ObservableCollection<DepartureProcedure>();
            HoldingPatterns = new ObservableCollection<HoldingPattern>();
            BlockerModel.BlockForAction(new List<Action> { Engine.Environment.Current.DbProvider.Load, ApproachProcedure.Fetch, ArrivalProcedure.Fetch, DepartureProcedure.Fetch, HoldingPattern.Fetch }, Load);
            _procedureDrawer = new CommonProcedureDrawer(Options.Current.CommonViewColorOptions);
            _holdingDrawer = new CommonHoldingDrawer(Options.Current.CommonViewColorOptions);
            EffectiveDate = Engine.Environment.Current.EffectiveDate;

        }

        private void Load()
        {

            try
            {

                var approaches = ApproachProcedure.Load();
                foreach (var t in approaches)
                {
                    ApproachProcedures.Add(t);
                }
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "Error on loading approach procedures");
            }


            try
            {
                var arrivals = ArrivalProcedure.Load();
                foreach (var t in arrivals)
                {
                    ArrivalProcedures.Add(t);
                }
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "Error on loading arrival procedures");
            }

            try
            {
                var departures = DepartureProcedure.Load();
                foreach (var t in departures)
                {
                    DepartureProcedures.Add(t);
                }
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "Error on loading departure procedures");
            }
            try
            {
                var holdingPatterns = HoldingPattern.Load();
                foreach (var t in holdingPatterns)
                {
                    HoldingPatterns.Add(t);
                }
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "Error on loading holding patterns");
            }
        }

        private RelayCommand<RoutedEventArgs> _openApproachProcedureCommand;
        public RelayCommand<RoutedEventArgs> OpenApproachProcedure
        {
            get
            {
                return _openApproachProcedureCommand
                    ?? (_openApproachProcedureCommand = new RelayCommand<RoutedEventArgs>(
                    (e) =>
                    {
                        _selectedRouteType = RouteType.Approach;
                        OpenProcedureView();

                    }));
            }
        }

        private RelayCommand<RoutedEventArgs> _openDepartureProcedureCommand;
        public RelayCommand<RoutedEventArgs> OpenDepartureProcedure
        {
            get
            {
                return _openDepartureProcedureCommand
                    ?? (_openDepartureProcedureCommand = new RelayCommand<RoutedEventArgs>(
                    (e) =>
                    {
                        _selectedRouteType = RouteType.Departure;
                        OpenProcedureView();

                    }));
            }
        }

        private RelayCommand<RoutedEventArgs> _openArrivalProcedureCommand;
        public RelayCommand<RoutedEventArgs> OpenArrivalProcedure
        {
            get
            {
                return _openArrivalProcedureCommand
                    ?? (_openArrivalProcedureCommand = new RelayCommand<RoutedEventArgs>(
                    (e) =>
                    {
                        _selectedRouteType = RouteType.Arrival;
                        OpenProcedureView();

                    }));
            }
        }

        private RelayCommand<RoutedEventArgs> _openHoldingPatternCommand;
        public RelayCommand<RoutedEventArgs> OpenHoldingPattern
        {
            get
            {
                return _openHoldingPatternCommand
                       ?? (_openHoldingPatternCommand = new RelayCommand<RoutedEventArgs>(
                           (e) =>
                           {
                               _selectedRouteType = RouteType.Holding;
                               OpenProcedureView();

                           }));
            }
        }

        private void OpenProcedureView()
        {
            Next();
            DeselectAll = true;
        }

        private bool _deselectAll;
        public bool DeselectAll
        {
            get => _deselectAll;
            set
            {
                Set(() => DeselectAll, ref _deselectAll, value);
            }
        }

        private RelayCommand _reportCommand;

        public RelayCommand ReportCommand => _reportCommand ?? (_reportCommand = new RelayCommand(
                                                 ExecuteReportCommand,
                                                 CanExecuteReportCommand));

        private void ExecuteReportCommand()
        {

        }

        private bool CanExecuteReportCommand()
        {
            return true;
        }

        private RelayCommand<SelectionChangedEventArgs> _approachRowSelectedCommand;

        public RelayCommand<SelectionChangedEventArgs> ApproachRowSelectedCommand
        {
            get
            {
                return _approachRowSelectedCommand
                    ?? (_approachRowSelectedCommand = new RelayCommand<SelectionChangedEventArgs>(
                    (rows) =>
                    {
                        NativeMethods.ShowPandaBox(Engine.Environment.Current.EnvWin32Window.ToInt32());
                        foreach (var t in rows.RemovedItems)
                            Clean((ApproachProcedure)t);


                        foreach (var t in rows.AddedItems)
                            Draw((ApproachProcedure)t);
                        NativeMethods.HidePandaBox();

                    }));
            }
        }


        private RelayCommand<SelectionChangedEventArgs> _arrivalRowSelectedCommand;

        public RelayCommand<SelectionChangedEventArgs> ArrivalRowSelectedCommand
        {
            get
            {
                return _arrivalRowSelectedCommand
                    ?? (_arrivalRowSelectedCommand = new RelayCommand<SelectionChangedEventArgs>(
                    (rows) =>
                    {
                        NativeMethods.ShowPandaBox(Engine.Environment.Current.EnvWin32Window.ToInt32());
                        foreach (var t in rows.RemovedItems)
                            Clean((ArrivalProcedure)t);

                        foreach (var t in rows.AddedItems)
                            Draw((ArrivalProcedure)t);
                        NativeMethods.HidePandaBox();
                    }));
            }
        }

        private RelayCommand<SelectionChangedEventArgs> _depurtureRowSelectedCommand;

        public RelayCommand<SelectionChangedEventArgs> DepurtureRowSelectedCommand
        {
            get
            {
                return _depurtureRowSelectedCommand
                    ?? (_depurtureRowSelectedCommand = new RelayCommand<SelectionChangedEventArgs>(
                    (rows) =>
                    {
                        NativeMethods.ShowPandaBox(Engine.Environment.Current.EnvWin32Window.ToInt32());
                        foreach (var t in rows.RemovedItems)
                            Clean((DepartureProcedure)t);

                        foreach (var t in rows.AddedItems)
                            Draw((DepartureProcedure)t);
                        NativeMethods.HidePandaBox();

                    }));
            }
        }

        private RelayCommand<SelectionChangedEventArgs> _holdingRowSelectedCommand;

        public RelayCommand<SelectionChangedEventArgs> HoldingRowSelectedCommand
        {
            get
            {
                return _holdingRowSelectedCommand
                       ?? (_holdingRowSelectedCommand = new RelayCommand<SelectionChangedEventArgs>(
                           (rows) =>
                           {
                               NativeMethods.ShowPandaBox(Engine.Environment.Current.EnvWin32Window.ToInt32());
                               foreach (var t in rows.RemovedItems)
                                   Clean((HoldingPattern)t);

                               foreach (var t in rows.AddedItems)
                                   Draw((HoldingPattern)t);
                               NativeMethods.HidePandaBox();

                           }));
            }
        }

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
            if (_selectedRouteType == RouteType.Approach)
                NextState = new ApproachViewModel(mainViewModel, this, CurrentApproachProcedure);
            if (_selectedRouteType == RouteType.Departure)
                NextState = new DepartureViewModel(mainViewModel, this, CurrentDepartureProcedure);
            if (_selectedRouteType == RouteType.Arrival)
                NextState = new ArrivalViewModel(mainViewModel, this, CurrentArrivalProcedure);
            if (_selectedRouteType == RouteType.Holding)
                NextState = new HoldingViewModel(mainViewModel, this, CurrentHoldingPattern);
        }

        protected override void previous() { }
        protected override void reInit()
        {
            DeselectAll = false;
        }

        private void Draw<T>(Procedure<T> proc) where T : Aran.Aim.Features.Procedure
        {
            _procedureDrawer.Draw(proc);
            var text1 = "UUID: " + proc.Identifier;
            var text2 = "Name\\Designator: " + proc.Name + "\\" + (proc.Designator ?? "");
            var text3 = "Begin Date: " + proc.BeginDate.ToShortDateString() +
                 " EndDate: " + (proc.EndDate?.ToShortDateString() ?? "") + " EffectiveDate: " + EffectiveDate.ToShortDateString();
            Engine.Environment.Current.Graphics.SetLayoutText(new List<string>() { text1, text2, text3 });

        }


        private void Clean<T>(Procedure<T> proc) where T : Aran.Aim.Features.Procedure
        {
            _procedureDrawer.Clean(proc);
            Engine.Environment.Current.Graphics.SetLayoutText(null);
        }


        private void Draw(HoldingPattern pattern)
        {
            _holdingDrawer.Draw(pattern);
            var text1 = "UUID: " + pattern.Identifier;
            var text3 = "Begin Date: " + pattern.BeginDate.ToShortDateString() +
                        " EndDate: " + (pattern.EndDate?.ToShortDateString() ?? "") + " EffectiveDate: " + EffectiveDate.ToShortDateString();
            Engine.Environment.Current.Graphics.SetLayoutText(new List<string>() { text1, text3 });

        }

        private void Clean(HoldingPattern pattern)
        {
            _holdingDrawer.Clean(pattern);
        }

        private void ClearScreen()
        {
            _procedureDrawer.Clean();
            _holdingDrawer.Clean();
        }

        protected override void _destroy()
        {
            ClearScreen();
        }


        enum RouteType
        {
            Approach,
            Departure,
            Arrival,
            Holding
        }
    }
}