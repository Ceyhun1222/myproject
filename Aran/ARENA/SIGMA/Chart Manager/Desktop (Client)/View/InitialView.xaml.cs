using System.Windows;
using System.Windows.Controls;
using ChartManager.ViewModel;

namespace ChartManager.View
{
    /// <summary>
    /// Interaction logic for InitialView.xaml
    /// </summary>
    public partial class InitialView : UserControl
    {
        public InitialView()
        {
            InitializeComponent();
        }
       private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as InitialViewModel).OpenCommand.Execute(null);

            SplitBtnOpen.IsOpen = false;
        }

        private void btnOpenAsNewChart_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as InitialViewModel).OpenAsNewChartCommand.Execute(null);
            SplitBtnOpen.IsOpen = false;
        }

        private void btnDeleteLatest_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as InitialViewModel).DeleteLatestVersion.Execute(null);
            SplitBtnDelete.IsOpen = false;
        }

        private void btnDeleteAllCurrentVersions_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as InitialViewModel).DeleteAllCurrentVersions.Execute(null);
            SplitBtnDelete.IsOpen = false;
        }

    }
}
