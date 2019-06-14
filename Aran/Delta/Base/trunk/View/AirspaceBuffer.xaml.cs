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
    public partial class AirspaceBuffer : MetroWindow
    {
       private  AirspaceBufferViewModel _airspaceBufferViewModel;
        public AirspaceBuffer()
        {
            InitializeComponent();
            _airspaceBufferViewModel = new AirspaceBufferViewModel();
            _airspaceBufferViewModel.RequestClose += () => Close();
            //GlobalParams.RouteBuffer = _airspaceBufferViewModel;
            this.DataContext = _airspaceBufferViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _airspaceBufferViewModel.Clear();
        }
    }
}
