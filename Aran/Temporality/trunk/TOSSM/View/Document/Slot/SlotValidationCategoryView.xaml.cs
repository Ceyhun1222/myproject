using System.Windows.Controls;
using System.Windows.Input;

namespace TOSSM.View.Document.Slot
{
    /// <summary>
    /// Interaction logic for SlotValidationCategoryView.xaml
    /// </summary>
    public partial class SlotValidationCategoryView : UserControl
    {
        public SlotValidationCategoryView()
        {
            InitializeComponent();
        }

        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as SlotValidationCategoryView;
            if (model == null) return;

            //if (model.ViewCommand.CanExecute(null))
            //{
            //    model.ViewCommand.Execute(null);
            //}
        }

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as SlotValidationCategoryView;
            if (model == null) return;

            //model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column.Header.ToString();
        }
    }
}
