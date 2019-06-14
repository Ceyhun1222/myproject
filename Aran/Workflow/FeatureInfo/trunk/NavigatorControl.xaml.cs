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

namespace Aran.Aim.FeatureInfo
{
    /// <summary>
    /// Interaction logic for FeatNavigatorControl.xaml
    /// </summary>
    internal partial class NavigatorControl : UserControl
    {
        public event EventHandler NavItemClicked;

        public NavigatorControl ()
        {
            InitializeComponent ();
        }

        private void NavItem_Click (object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            NavInfo navInfo = hyperlink.DataContext as NavInfo;

            if (NavItemClicked != null)
                NavItemClicked (navInfo, e);
        }
    }
}
