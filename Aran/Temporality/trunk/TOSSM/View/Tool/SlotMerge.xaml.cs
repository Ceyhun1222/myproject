using System.Windows.Controls;
using System.Windows.Input;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class SlotMerge : UserControl
    {
        public SlotMerge()
        {
            InitializeComponent();
        }

        private void FiltererComboBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (!FiltererComboBox.IsDropDownOpen && (e.Key == Key.Up || e.Key == Key.Down))
            //{
            //    e.Handled = true;
            //    return;
            //}

            //if (e.Key == Key.Enter)
            //{
            //    FiltererComboBox.IsDropDownOpen = false;
            //}
            //else
            //{
            //    FiltererComboBox.IsDropDownOpen = true;
            //}
        }
    }
}