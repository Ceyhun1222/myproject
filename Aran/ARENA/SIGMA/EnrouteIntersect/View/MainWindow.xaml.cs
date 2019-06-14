using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EnrouteIntersect.ViewModel;

namespace EnrouteIntersect.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow ( )
		{
			InitializeComponent ( );
		}

		public void SetData ( ESRI.ArcGIS.Controls.IHookHelper m_hookHelper )
		{
			GlobalParams.HookHelper = m_hookHelper;
			MainViewModel mV = new MainViewModel ( this.Dispatcher );
			mV.RequestClose += delegate ( )
			{
				this.Close ( );
			};
			DataContext = mV;
		}

		private void Window_Closing ( object sender, System.ComponentModel.CancelEventArgs e )
		{
			( DataContext as MainViewModel ).Clear ( );
		}
	}
}