using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VisibilityTool.Model;
using VisibilityTool.ViewModel;
using VisibilityTool;
using System.Collections.ObjectModel;

namespace VisibilityTool
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent ();            
		}

		public void SetData ( ESRI.ArcGIS.Controls.IHookHelper m_hookHelper, ObservableCollection<LayerTemplate> dataTemplates )
		{
			GlobalParams.HookHelper = m_hookHelper;
			MainViewModel viewModel = new MainViewModel ( this, dataTemplates );
			viewModel.RequestClose += delegate () { this.Close (); };
			DataContext = viewModel;
		}

		private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e)
		{
			(DataContext as MainViewModel).Clear ();
		}

		private void listBxVisible_Click (object sender, RoutedEventArgs e)
		{
			var checkBox = e.OriginalSource as CheckBox;
			if (checkBox != null && checkBox.Name == "chckBxSelectAll")
			{
				var groupItem = checkBox.DataContext as CollectionViewGroup;

				//// Assuming MyItem is the item level class and MyGroupedProperty 
				//// is the grouped property that you have added to the grouped
				//// description in your CollectionView.
				bool isSelected = false;
				if(checkBox.IsChecked.HasValue)
					isSelected = checkBox.IsChecked.Value;
				foreach (var item in groupItem.Items)
				{

					(item as FeatModel).IsSelected = isSelected;
				}
			}
		}
	}
}