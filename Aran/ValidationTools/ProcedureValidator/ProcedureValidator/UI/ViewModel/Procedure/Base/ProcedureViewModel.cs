using System;
using PVT.Model;
using PVT.Graphics;
using System.Collections.ObjectModel;
using PVT.Settings;
using GalaSoft.MvvmLight.Command;
using PVT.UI.View;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Interop;
using Aran.PANDA.Common;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProcedureViewModel<T> : FeatureViewModel<T> where T : ProcedureBase
    {
        /// <summary>
        /// Initializes a new instance of the ProcedureViewModel class.
        /// </summary>
        readonly IProcedureDrawer _drawer;

        public T Procedure
        {
            get => Feature;
            set => Feature = value;
        }

        private Aran.Aim.FeatureType ProcedureType
        {
            get
            {
                if (typeof(T) == typeof(ApproachProcedure))
                    return Aran.Aim.FeatureType.InstrumentApproachProcedure;
                if (typeof(T) == typeof(ArrivalProcedure))
                    return Aran.Aim.FeatureType.StandardInstrumentArrival;
                if (typeof(T) == typeof(DepartureProcedure))
                    return Aran.Aim.FeatureType.StandardInstrumentDeparture;
                return Aran.Aim.FeatureType.InstrumentApproachProcedure;
            }
        }
        public ObservableCollection<TransitionViewModel> TransitionViewModels { get; }

        public TransitionLeg CurrentLeg { get; set; }


        public ProcedureViewModel(MainViewModel main, StateViewModel previous, T procedure) : base(main, previous)
        {
            Type = ViewModelType.Procedure;
            Procedure = procedure;
            _drawer = new CommonProcedureDrawer(Options.Current.DetailedViewColorOptions);

            TransitionViewModels = new ObservableCollection<TransitionViewModel>();
            foreach (var t in Procedure.Transtions)
            {
                TransitionViewModels.Add(new TransitionViewModel(t));
            }

            foreach (var t in TransitionViewModels)
            {
                t.SelectionChanged += LegSelectionChanged;
            }
        }

      


        private void LegSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var t in e.RemovedItems)
                Clean((TransitionLeg) t);

            foreach (var t in e.AddedItems)
                Draw((TransitionLeg) t);
        }

       


        private void Draw(TransitionLeg leg)
        {
            _drawer.Draw(leg.SegmentLeg);
        }


        private void Clean(TransitionLeg leg)
        {
            _drawer.Clean(leg.SegmentLeg);
        }


        protected override  void ClearScreen()
        {
            _drawer.Clean();
        }

        protected override void _destroy()
        {
            ClearScreen();
        }
    }
}