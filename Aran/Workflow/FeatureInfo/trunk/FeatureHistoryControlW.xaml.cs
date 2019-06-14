using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aran.Aim.Features;

namespace Aran.Aim.FeatureInfo
{
	/// <summary>
	/// Interaction logic for FeatureHistoryControlW.xaml
	/// </summary>
	public partial class FeatureHistoryControlW : UserControl
	{
		public event EventHandler CloseClicked;

		public FeatureHistoryControlW ()
		{
			InitializeComponent ();

			Model = new FeatureHistoryModel ();
			DataContext = Model;
			ui_featCC.HideTopPanel ();
		}

		public FeatureHistoryModel Model { get; private set; }

		public void SelectFirst ()
		{
			if (Model.Items.Count > 0)
				ui_listBox.SelectedIndex = 0;
		}

		private void ListBox_SelectionChanged (object sender, SelectionChangedEventArgs e)
		{
			var fhi = (sender as ListBox).SelectedItem as FeatureHistoryInfo;
			var fl = new List<Feature> ();
			fl.Add (fhi.Feature);
			ui_featCC.SetFeature (fl);
		}

		private void Close_Click (object sender, RoutedEventArgs e)
		{
			if (CloseClicked != null)
				CloseClicked (sender, e);
		}
	}
}
