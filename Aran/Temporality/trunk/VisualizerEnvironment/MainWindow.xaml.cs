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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualizerEnvironment.Properties;

namespace VisualizerEnvironment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                Top = Settings.Default.Top;
                Left = Settings.Default.Left;
                Height = Settings.Default.Height;
                Width = Settings.Default.Width;
                WindowState = Settings.Default.IsMaximized ? WindowState.Maximized : WindowState.Normal;
            };

            Closing += (a, b) =>
            {
                if (WindowState == WindowState.Maximized)
                {
                    Settings.Default.Top = RestoreBounds.Top;
                    Settings.Default.Left = RestoreBounds.Left;
                    Settings.Default.Height = RestoreBounds.Height;
                    Settings.Default.Width = RestoreBounds.Width;
                    Settings.Default.IsMaximized = true;
                }
                else
                {
                    Settings.Default.Top = Top;
                    Settings.Default.Left = Left;
                    Settings.Default.Height = Height;
                    Settings.Default.Width = Width;
                    Settings.Default.IsMaximized = false;
                }
                Properties.Settings.Default.Save();
            };
        }
    }
}
