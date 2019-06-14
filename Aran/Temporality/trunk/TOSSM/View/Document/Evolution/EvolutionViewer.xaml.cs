using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.ViewModel.Document.Evolution;

namespace TOSSM.View.Document.Evolution
{
    /// <summary>
    /// Interaction logic for EvolutionViewer.xaml
    /// </summary>
    public partial class EvolutionViewer : UserControl
    {
        public EvolutionViewer()
        {
            InitializeComponent();
        }

        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as EvolutionDocViewModel;
            if (model == null) return;

            //if (model.ViewCommand.CanExecute(null))
            //{
            //    model.ViewCommand.Execute(null);
            //}
        }

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as EvolutionDocViewModel;
            if (model == null) return;

            //model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column.Header.ToString();
        }
    }
}
