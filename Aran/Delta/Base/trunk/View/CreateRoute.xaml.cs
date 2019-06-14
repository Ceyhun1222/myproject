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
using System.Windows.Shapes;
using Aran.AranEnvironment;
using Aran.Delta.Builders;
using Aran.Delta.ViewModels;
using MahApps.Metro.Controls;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for CreateRoute.xaml
    /// </summary>
    public partial class CreateRoute : MetroWindow
    {
        public CreateRoute(ICreateRouteViewModel createRouteViewModel)
        {
            InitializeComponent();
            var vModel = createRouteViewModel;
            this.DataContext = vModel;
            var viewModel = vModel as ViewModel;
            if (viewModel != null) viewModel.RequestClose += this.Close;

            if (GlobalParams.AranEnv==null)
                DGrid.Columns[4].Visibility =Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var createRouteViewModel = this.DataContext as ViewModels.CreateRouteViewModel;
            if (createRouteViewModel != null)
                createRouteViewModel.Clear();
        }
    }
}
