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
using Aran.Panda.RadarMA.Models;
using Aran.Panda.RadarMA.ViewModels;

namespace Aran.Panda.RadarMA.View
{
    /// <summary>
    /// Interaction logic for ObstacleReportView.xaml
    /// </summary>
    public partial class ObstacleReportView : Window
    {
        private ObstacleReportViewModel _obstacleReportViewModel;

        public ObstacleReportView(List<ObstacleReport> reports)
        {
            InitializeComponent();
            _obstacleReportViewModel = new ViewModels.ObstacleReportViewModel(reports);
            this.DataContext = _obstacleReportViewModel;
        }

        private void ObstacleReportView_OnClosed(object sender, EventArgs e)
        {
            _obstacleReportViewModel?.Clear();
        }
    }
}
