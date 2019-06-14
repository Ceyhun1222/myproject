using Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Media;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BaseViewModel
    {

        private readonly CheckBox CheckBox = new CheckBox();
        private StateViewModel _viewModel;

        public StateViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                Set(() => ViewModel, ref _viewModel, value);
            }
        }


        public MainViewModel()
        {
            Env.Current.RNPContext.CreateReportForm();
            Env.Current.RNPContext.ReportForm.Init(CheckBox);
            ViewModel = new InitializationViewModel(this);

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

        private RelayCommand _reportCommand;
        public RelayCommand ReportCommand
        {
            get
            {
                return _reportCommand
                       ?? (_reportCommand = new RelayCommand(
                           () =>
                           {
                               
                               CheckBox.Checked = !CheckBox.Checked;
                               if (CheckBox.Checked)
                               {
                                   Env.Current.RNPContext.ReportForm.Show();
                               }
                               else
                                   Env.Current.RNPContext.ReportForm.Hide();

                           }));
            }
        }

       

    }
}