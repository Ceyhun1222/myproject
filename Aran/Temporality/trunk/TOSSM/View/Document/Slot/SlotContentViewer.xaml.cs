using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.ViewModel.Document.Slot;

namespace TOSSM.View.Document.Slot
{
    /// <summary>
    /// Interaction logic for SlotContentViewer.xaml
    /// </summary>
    public partial class SlotContentViewer : UserControl
    {
        public SlotContentViewer()
        {
            InitializeComponent();
        }

        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as SlotContentDocViewModel;
            if (model == null) return;

            //if (model.ViewCommand.CanExecute(null))
            //{
            //    model.ViewCommand.Execute(null);
            //}
        }

        //private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        //{
        //    var model = DataContext as SlotContentDocViewModel;
        //    if (model == null) return;

        //    //model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column.Header.ToString();
        //}

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as SlotContentDocViewModel;
            if (model == null) return;

           // model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column?.Header?.ToString();


            var items = PresenterDataGrid.SelectedItems;
            model.DataPresenter.SelectedFeatures = items;
        }
    }
}
