using System.Windows;
using System.Windows.Interop;
using Aran.PANDA.LegCreator.ViewModel;

namespace Aran.PANDA.LegCreator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Wizard_Finish(object sender, RoutedEventArgs e)
		{
			WindowInteropHelper helper = new WindowInteropHelper(this);
		    var res = (DataContext as MainViewModel)?.Finish(helper.Handle);
		    if (!string.IsNullOrEmpty(res))
		        MessageBox.Show(res, "Error occured while saving", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Wizard_Next(object sender, Xceed.Wpf.Toolkit.Core.CancelRoutedEventArgs e)
		{
			WindowInteropHelper helper = new WindowInteropHelper(this);
			(DataContext as MainViewModel)?.Next(helper.Handle);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			(DataContext as MainViewModel)?.Closing();
		}
	}
}
