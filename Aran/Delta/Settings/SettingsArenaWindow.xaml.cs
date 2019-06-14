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

namespace Aran.Delta.Settings
{
    /// <summary>
    /// Interaction logic for SettingsArenaWindow.xaml
    /// </summary>
    public partial class SettingsArenaWindow : Window
    {
        public SettingsArenaWindow(DeltaSettings settings)
        {
            InitializeComponent();
            Globals.Settings = settings;
            var stViewModel = StFrom.DataContext as SettingsViewModel;
            if (stViewModel != null) stViewModel.SetAppType(true);

            var tabControl = StFrom.SetTabControl;
            StFrom.LoadAll();
            
            TabItem tabItem = tabControl.Items[0] as TabItem;
            if (tabItem != null) tabItem.Visibility =Visibility.Collapsed;
            
            tabControl.SelectedIndex = 1;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StFrom_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
