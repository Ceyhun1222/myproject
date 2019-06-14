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
using Aran.Delta.ViewModels;
using MahApps.Metro.Controls;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for RouteBuffer.xaml
    /// </summary>
    public partial class RouteBuffer : MetroWindow
    {
        private RouteBufferViewModel _routeBufferViewModel;
        public RouteBuffer()
        {
            InitializeComponent();
            _routeBufferViewModel = new RouteBufferViewModel();
            _routeBufferViewModel.RequestClose += () => this.Close();
            GlobalParams.RouteBuffer = _routeBufferViewModel;
            this.DataContext = _routeBufferViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _routeBufferViewModel.Clear();
            _routeBufferViewModel.RequestClose -= null;
            _routeBufferViewModel.Dispose();
            GlobalParams.RouteBuffer = null;
            GlobalParams.Settings = null;
            GlobalParams.SpatialRefOperation = null;
            GlobalParams.UI = null;
            GlobalParams.DesigningAreaReader = null;

//            _routeBufferViewModel = null;
        }
    }
}
