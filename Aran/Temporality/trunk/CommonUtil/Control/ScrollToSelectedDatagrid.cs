using System.Windows.Controls;

namespace Aran.Temporality.CommonUtil.Control
{
    public class ScrollToSelectedDatagrid : DataGrid
    {
        public ScrollToSelectedDatagrid()
        {
            SelectionChanged += DataGridSelectionChanged;
        }

        void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItem!=null)
            {
                ScrollIntoView(SelectedItem);
            }
        }
    }
}
