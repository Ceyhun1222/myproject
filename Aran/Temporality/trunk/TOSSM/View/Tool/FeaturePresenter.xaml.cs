using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.ViewModel.Tool;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for FeaturePresenter.xaml
    /// </summary>
    public partial class FeaturePresenter
    {
        public FeaturePresenter()
        {
            InitializeComponent();
        }

        private void DataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var model = DataContext as FeaturePresenterToolViewModel;
            if (model == null) return;
         
            model.OnDoubleClick();
        }

        private void PresenterDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as FeaturePresenterToolViewModel;
            if (model == null) return;

            model.SelectedCellColumnHeader = PresenterDataGrid.CurrentCell.Column?.Header?.ToString();


            var items=PresenterDataGrid.SelectedItems;
            model.DataPresenter.SelectedFeatures = items;
        }

      
        //private void DataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{

        //    Type type = null;

        //    //process val types
        //    if (typeof(ValClassBase).IsAssignableFrom(e.PropertyType))
        //    {
        //        if (e.PropertyType.BaseType != null)
        //        {
        //            type = e.PropertyType.BaseType.GetGenericArguments()[0];
        //        }
        //    }


        //    // create text
        //    var label = new FrameworkElementFactory(typeof(TextBlock));
        //    label.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
        //    label.SetBinding(TextBlock.TextProperty, new Binding
        //    {
        //        Path = new PropertyPath(e.PropertyName),
        //        Converter = HumanReadableConverter.Instance,
        //        Mode = BindingMode.OneTime
        //    });



        //    e.Column = new DataGridTemplateColumn
        //    {
        //        Header = e.PropertyName,
        //        CanUserSort = true,
        //        SortMemberPath = e.PropertyName,
        //        CellTemplate = new DataTemplate
        //        {
        //            VisualTree = label
        //        }
        //    };


        //    if (e.PropertyName == "TimeSlice")
        //    {
        //        e.Column.DisplayIndex = 0;
        //    }

        //    if (e.PropertyName == "Name")
        //    {
        //        e.Column.DisplayIndex = 0;
        //    }

        //    if (e.PropertyName == "Designator")
        //    {
        //        e.Column.DisplayIndex = 0;
        //    }

           

        //    // Cancel AutoGeneration 
        //    if (NoGenerationFor.Contains(e.PropertyName))
        //        e.Cancel = true;
        //}


       

        public void DataGridOnColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            var model = DataContext as FeaturePresenterToolViewModel;
            if (model == null) return;

            var datagrid = sender as DataGrid;
            if (datagrid==null) return;

            model.DataPresenter.SetColumnOrder(datagrid.Columns.OrderBy(t=>t.DisplayIndex).Select(t=>t.Header.ToString()).ToList());
        }

       

    }
}
