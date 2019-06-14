using System.Windows.Controls;
using TOSSM.ViewModel.Tool;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for SlotSelector.xaml
    /// </summary>
    public partial class SlotSelector : UserControl
    {
        public SlotSelector()
        {
            InitializeComponent();
        }

        private void SlotSelectorDataGridSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var model = DataContext as SlotSelectorToolViewModel;
            if (model == null) return;



            var items = PrivateSlotSelectorDataGrid.SelectedItems;
            model.SelectedPrivateSlots = items;
        }
    }
}
