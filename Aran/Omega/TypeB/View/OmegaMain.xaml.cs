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
using System.Collections.ObjectModel;
using Aran.Panda.Constants;
using Aran.Panda.Common;
using Aran.AranEnvironment.Symbols;

namespace Omega
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModels.OmegaMainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModels.OmegaMainViewModel();
            if (viewModel.CloseAction == null)
                viewModel.CloseAction += new Action(() => this.Close());
            Closing += viewModel.OnWindowClosing;
            
            viewModel.Init();
            DataContext = viewModel;
            
            //Uri iconUri = new Uri("pack://omega:/icon1.ico", UriKind.RelativeOrAbsolute);

            //var uriSource = new Uri(@"pack://application:,,,/Omega;component/Resources/Icon1.ico");
            //this.Icon = BitmapFrame.Create(uriSource);
            //this.Icon = BitmapFrame.Create(new Uri(@"D:\AirNav\Aran\Omega\Trunk\Resources\panda.ico"));

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            GlobalParams.UI.SelectFillSymbol(viewModel.AvailableSurfaceList[0].SurfaceBase.Geo);
        }

    }
}
