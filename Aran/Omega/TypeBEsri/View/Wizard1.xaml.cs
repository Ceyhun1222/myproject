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
using Omega.ViewModels;

namespace Omega
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class OmegaMainForm : Window
    {
        public OmegaMainForm()
        {
            InitializeComponent();
            var olsViewModelviewModel = new ViewModels.OLSViewModel();
            if (olsViewModelviewModel.CloseAction == null)
                olsViewModelviewModel.CloseAction += new Action(this.Close);
            Closing += olsViewModelviewModel.OnWindowClosing;
            
            olsViewModelviewModel.Init();
            DataContext = olsViewModelviewModel;

            lblElevationDatum.Content = "Elevation Datum (" + InitOmega.HeightConverter.Unit+" )";
            dGridRwy.Columns[3].Header = "Length (" + InitOmega.DistanceConverter.Unit + " )";
            dGridRwy.Columns[4].Header = "HEIGHT (" + InitOmega.HeightConverter.Unit + " )";
            //Uri iconUri = new Uri("pack://omega:/icon1.ico", UriKind.RelativeOrAbsolute);

            //var uriSource = new Uri(@"pack://application:,,,/Omega;component/Resources/Icon1.ico");
            //this.Icon = BitmapFrame.Create(uriSource);
            //this.Icon = BitmapFrame.Create(new Uri(@"D:\AirNav\Aran\Omega\Trunk\Resources\panda.ico"));

        }

    }
}
