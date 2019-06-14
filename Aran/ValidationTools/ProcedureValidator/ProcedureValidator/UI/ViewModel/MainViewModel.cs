using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PVT.Settings;
using PVT.UI.View;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PVT.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BaseViewModel
    {

        public ObservableCollection<DashStyle> LineStyles { get; }

        private StateViewModel _viewModel;

        public StateViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                Set(() => ViewModel, ref _viewModel, value);
            }
        }

        public Options Options { get { return Options.Current; } }

        public MainViewModel()
        {
            ViewModel = new CommonProcedureViewModel(this);
            LineStyles = new ObservableCollection<DashStyle>();
            LineStyles.Add(DashStyles.Solid);
            LineStyles.Add(DashStyles.Dash);
            LineStyles.Add(DashStyles.DashDot);
            LineStyles.Add(DashStyles.DashDotDot);
            LineStyles.Add(DashStyles.Dot);
            
        }

        public bool CanNext()
        {
            return ViewModel.CanNext();
        }

        public bool CanPrevious()
        {
            return ViewModel.CanPrevious();
        }

        public void Next(StateViewModel next)
        {
            ViewModel = next;
        }

        public void Previous(StateViewModel previous)
        {
            ViewModel = previous;
        }

        public void Destroy()
        {
            ViewModel.Destroy();
        }

        private RelayCommand _colorOptionsCommand;


        public RelayCommand ColorOptionsCommand
        {
            get
            {
                return _colorOptionsCommand
                    ?? (_colorOptionsCommand = new RelayCommand(
                    () =>
                    {
                        var colorOptionsView = new ColorOptionsView();
                        colorOptionsView.DataContext = this;
                        colorOptionsView.ShowDialog();
                    }));
            }
        }
    }
}