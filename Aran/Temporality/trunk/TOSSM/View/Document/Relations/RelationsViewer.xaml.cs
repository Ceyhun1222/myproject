using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Document.Relations;

namespace TOSSM.View.Document.Relations
{
    /// <summary>
    /// Interaction logic for RelationsViewer.xaml
    /// </summary>
    public partial class RelationsViewer : UserControl
    {
        public RelationsViewer()
        {
            InitializeComponent();
        }

        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as RelationsDocViewModel;
            if (model == null) return;

            if (model.DataPresenter.ViewCommand.CanExecute(null))
            {
                model.DataPresenter.ViewCommand.Execute(null);
            }
        }

        //private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        //{
        //    var model = DataContext as RelationsDocViewModel;
        //    if (model == null) return;

        //    //model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column.Header.ToString();
        //}

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as RelationsDocViewModel;
            if (model == null) return;

            model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column?.Header?.ToString();


            var items = PresenterDataGrid.SelectedItems;
            model.DataPresenter.SelectedFeatures = items;
        }

        private void RelationDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as RelationsDocViewModel;
            if (model == null) return;
            if (model.DataPresenter == null) return;

            var featureTye = model.DataPresenter.FeatureType;
            if (featureTye != null)
            {
                MainManagerModel.Instance.FeaturePresenterToolViewModel.DataPresenter.FeatureType = featureTye;
            }
        }
    }
}
