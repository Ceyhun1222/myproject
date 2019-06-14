//using Aerodrome.Context;

//using Aerodrome.Features.Context;
using Aerodrome.Opc;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using Framework.Stasy.SyncProvider;
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

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationContext _context;

        public MainWindow(ApplicationContext context)
        {
            RuntimeManager.Bind(ProductCode.Desktop);
            AoInitialize ao = new AoInitialize();
            ao.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);

            _context = context;
            InitializeComponent();


            CommandBindings.Add(
                new CommandBinding(ApplicationCommands.Close,
                    new ExecutedRoutedEventHandler(
                        delegate (object sender, ExecutedRoutedEventArgs args) { this.Close(); })));


            this.Content = new MasterData(_context);
        }

        private void Window_Drag(object sender, MouseButtonEventArgs e)
        {
            switch (e.ClickCount)
            {
                case 1:
                    DragMove();
                    break;
                default:
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //_context.Commit();
        }
    }
}
