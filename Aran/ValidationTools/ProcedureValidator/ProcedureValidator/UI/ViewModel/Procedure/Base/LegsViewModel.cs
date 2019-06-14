using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PVT.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LegsViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the LegViewModel class.
        /// </summary>
        /// 


        public LegType LegType { get; }
        public string Type { get { return LegType.ToString(); } }

        public LegsViewModel(LegType type, List<TransitionLeg> legs)
        {
            LegType = type;
            Legs = new ObservableCollection<TransitionLeg>();
            for (var i = 0; i < legs.Count; i++)
            {
                Legs.Add(legs[i]);
            }
        }

        public ObservableCollection<TransitionLeg> Legs { get; }
        public event SelectionChangedEventHandler SelectionChanged;

        private TransitionLeg _currentLeg;

        public TransitionLeg CurrentLeg
        {
            get
            {
                return _currentLeg;
            }
            set
            {
                Set(() => CurrentLeg, ref _currentLeg, value);
            }
        }

        private RelayCommand<SelectionChangedEventArgs> _legRowSelectedCommand;

        public RelayCommand<SelectionChangedEventArgs> LegRowSelectedCommand
        {
            get
            {
                return _legRowSelectedCommand
                    ?? (_legRowSelectedCommand = new RelayCommand<SelectionChangedEventArgs>(
                    (e) =>
                    {
                        if (SelectionChanged != null)
                            SelectionChanged(this, e);

                    }));
            }
        }
    }
}