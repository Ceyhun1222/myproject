using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Document;

namespace TOSSM.View.Document
{
    /// <summary>
    /// Interaction logic for FeatureDependencyReport.xaml
    /// </summary>
    public partial class FeatureDependencyReport : UserControl
    {
        public FeatureDependencyReport()
        {
            InitializeComponent();
        }


        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as FeatureDependencyReportDocViewModel;
            if (model == null) return;

            if (model.DataPresenter.ViewCommand.CanExecute(null))
            {
                model.DataPresenter.ViewCommand.Execute(null);
            }
        }

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as FeatureDependencyReportDocViewModel;
            if (model == null) return;

            //model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column.Header.ToString();
        }

        private void RelationDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as FeatureDependencyReportDocViewModel;
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
