using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Aran.Panda.RadarMA.Models;
using Aran.Panda.RadarMA.ViewModels;
using Aran.PANDA.Common;
using ESRI.ArcGIS.Geometry;

namespace Aran.Panda.RadarMA.View
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private readonly ReportViewModel rpViewModel;
        public Report(ObservableCollection<Sector> sectorList,List<VerticalStructure> vsList,
            ElevationCalculator.ElavationCalculatorFacade elevatCalculatorFacade,UnitConverter unitConverter,IGeometry radarVectoringArea)
        {
            InitializeComponent();
            rpViewModel = new ReportViewModel(sectorList, vsList, elevatCalculatorFacade, unitConverter,radarVectoringArea);
            this.DataContext = rpViewModel;
        }


        private void Report_OnClosing(object sender, CancelEventArgs e)
        {
            rpViewModel?.Clear();
        }

    }
}
