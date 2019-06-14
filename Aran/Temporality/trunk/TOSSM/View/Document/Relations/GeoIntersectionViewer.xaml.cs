using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Document.Relations;

namespace TOSSM.View.Document.Relations
{
    /// <summary>
    /// Interaction logic for GeoIntersectionViewer.xaml
    /// </summary>
    public partial class GeoIntersectionViewer : UserControl
    {
        public GeoIntersectionViewer()
        {
            InitializeComponent();
        }

        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as GeoIntersectionDocViewModel;
            if (model == null) return;

            //if (model.ViewCommand.CanExecute(null))
            //{
            //    model.ViewCommand.Execute(null);
            //}
        }

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as GeoIntersectionDocViewModel;
            if (model == null) return;

            //model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column.Header.ToString();
        }


        private void RelationDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as GeoIntersectionDocViewModel;
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
