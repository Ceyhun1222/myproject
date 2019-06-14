using System.Windows.Controls;
using System.Windows.Input;
using Aran.Temporality.CommonUtil.Context;
using TOSSM.ViewModel;
using TOSSM.ViewModel.Tool;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : UserControl
    {
        public ExportView()
        {
            InitializeComponent();

            var loaded = false;
            Loaded += (a, b) =>
            {
                if (loaded) return;
                loaded = true;
                var model = DataContext as ExportToolViewModel;
                if (model == null) return;

               

                var date = MainManagerModel.Instance.FeaturePresenterToolViewModel.AiracDate;

                if (CurrentDataContext.CurrentUser.ActivePrivateSlot != null)
                {
                    date = CurrentDataContext.CurrentUser.ActivePrivateSlot.PublicSlot.EffectiveDate;
                }

               
                model.AiracDate = date;
            };
        }

        private void FiltererComboBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!FiltererComboBox.IsDropDownOpen && (e.Key == Key.Up || e.Key == Key.Down))
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                //if (!FiltererComboBox.IsDropDownOpen && MySelectedItem != null)
                //{
                //    SelectedItem = MySelectedItem;
                //}
                FiltererComboBox.IsDropDownOpen = false;
            }
            else
            {
                FiltererComboBox.IsDropDownOpen = true;
            }
        }

        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as ExportToolViewModel;
            if (model == null) return;

            if (model.DataPresenter.ViewCommand.CanExecute(null))
            {
                model.DataPresenter.ViewCommand.Execute(null);
            }
        }

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as ExportToolViewModel;
            if (model == null) return;


            var items = PresenterDataGrid.SelectedItems;
            model.DataPresenter.SelectedFeatures = items;
        }

        private void RelationDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as ExportToolViewModel;
            if (model == null) return;
            if (model.DataPresenter == null) return;

            //var featureTye = model.DataPresenter.FeatureType;
            //if (featureTye != null)
            //{
            //    MainManagerModel.Instance.FeaturePresenterToolViewModel.DataPresenter.FeatureType = featureTye;
            //}
        }
    }
}
