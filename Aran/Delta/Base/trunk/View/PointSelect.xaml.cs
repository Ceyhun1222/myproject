using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for PointSelect.xaml
    /// </summary>
    public partial class PointSelect : UserControl
    {
        public PointSelect()
        {
            InitializeComponent();
            RenderOptions.ProcessRenderMode =RenderMode.SoftwareOnly;
           
           // var coordController = (WinWrapper1.Child as ChoosePointNS.CoordinateControl);
        }

        void OnLeftLatitudeChanged(object sender, EventArgs e)
        {
            var dataContext = this.DataContext as ViewModels.PointSelectViewModel ;
            dataContext.LeftLatitude = Convert.ToDouble(sender);
        }

        void OnLeftLongtitudeChanged(object sender, EventArgs e)
        {
            var dataContext = this.DataContext as ViewModels.PointSelectViewModel;
            dataContext.LeftLongtitude = Convert.ToDouble(sender);
        }

        void OnRightLatitudeChanged(object sender, EventArgs e)
        {
            var dataContext = this.DataContext as ViewModels.PointSelectViewModel;
            dataContext.RightLatitude = Convert.ToDouble(sender); ;
        }

        void OnRightLongtitudeChanged(object sender, EventArgs e)
        {
            var dataContext = this.DataContext as ViewModels.PointSelectViewModel;
            dataContext.RightLongtitude = Convert.ToDouble(sender); 
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
           // WinWrapper1_OnLostFocus(null, null);
            //WinWrapper2_OnLostFocus(null, null);
        }

        private void WinWrapper1_OnLostFocus(object sender, RoutedEventArgs e)
        {
         //   var leftCoordController = (WinWrapper1.Child as ChoosePointNS.CoordinateControl);
           // leftCoordController.Refresh();//
        }

        private void WinWrapper2_OnLostFocus(object sender, RoutedEventArgs e)
        {
           // var rightCoordController = (WinWrapper2.Child as ChoosePointNS.CoordinateControl);
            //rightCoordController.Refresh();
        }
    }
}
