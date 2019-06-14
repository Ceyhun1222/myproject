using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Aran.Aim.Features;
using PVT.Graphics;
using PVT.Model;
using PVT.Settings;
using HoldingPattern = PVT.Model.HoldingPattern;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class HoldingViewModel : FeatureViewModel<HoldingPattern>
    {
        /// <summary>
        /// Initializes a new instance of the ProcedureViewModel class.
        /// </summary>
        readonly IHoldingDrawer _drawer;

        public HoldingPattern HoldingPattern
        {
            get => Feature;
            set => Feature = value;
        }


        public AssessmentAreasViewModel AreasViewModel { get; }


        public HoldingViewModel(MainViewModel main, StateViewModel previous, HoldingPattern pattern) : base(main, previous)
        {
            Type = ViewModelType.Holding;
            Feature = pattern;
            _drawer = new CommonHoldingDrawer(Options.Current.DetailedViewColorOptions);


            if (Feature.AssessmentAreas != null)
                AreasViewModel = new AssessmentAreasViewModel(Feature.AssessmentAreas);

             AreasViewModel.SelectionChanged += LegSelectionChanged;
   
        }


        private int GetIndex(Model.ObstacleAssessmentArea area)
        {
            for (int i = 0; i < Feature.AssessmentAreas.Count; i++)
            {
                if(Feature.AssessmentAreas[i].Equals(area))
                    return i;
            }
            return -1;
        }


        private void LegSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var t in e.RemovedItems)
                Clean(GetIndex((Model.ObstacleAssessmentArea)t));

            foreach (var t in e.AddedItems)
                Draw(GetIndex((Model.ObstacleAssessmentArea)t));
        }




        private void Draw(int index)
        {
            _drawer.Draw(Feature, index);
        }


        private void Clean(int index)
        {
            _drawer.Clean(Feature, index);
        }


        protected override void ClearScreen()
        {
            _drawer.Clean();
        }

        protected override void _destroy()
        {
            ClearScreen();
        }
    }
}