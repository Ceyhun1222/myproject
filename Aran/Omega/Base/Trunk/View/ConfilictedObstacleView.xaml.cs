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
using Aran.Omega.ViewModels;
using MahApps.Metro.Controls;

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for ConfilictedObstacleView.xaml
    /// </summary>
    public partial class ConfilictedObstacleView : MetroWindow
    {
        private ConfilictedObstacleViewModel _vModel;

        public ConfilictedObstacleView(List<Models.IgnoredObstaclePair> obstaclePairList)
        {
            InitializeComponent();
            _vModel = new ViewModels.ConfilictedObstacleViewModel(GlobalParams.OlsViewModel,obstaclePairList);
            this.DataContext = _vModel;
            Closing += _vModel.OnWindowClosing;
            _vModel.RequestClose += () => this.Close();
            ChangeGridHeaders();
            IsClosed = false;
            
        }

        private void ChangeGridHeaders()
        {
            reportGrid.Columns[2].Header = "Hacc (" + InitOmega.HeightConverter.Unit + " )";
            reportGrid.Columns[3].Header = "Vacc (" + InitOmega.HeightConverter.Unit + " )";

            reportGrid.Columns[7].Header = "Hacc (" + InitOmega.HeightConverter.Unit + " )";
            reportGrid.Columns[8].Header = "Vacc (" + InitOmega.HeightConverter.Unit + " )";
        }

        public bool IsClosed { get; set; }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsClosed = true;
            Closing -= _vModel.OnWindowClosing;


            foreach (var obstacleReport in _vModel.IgnoredObstacleList)
            {
                foreach (var drawingSurface in GlobalParams.OlsViewModel.AvailableSurfaceList)
                {
                    var deletedReport = drawingSurface.SurfaceBase.GetReport.FirstOrDefault(rep => rep.Obstacle.Identifier.Equals(obstacleReport.Obstacle.Identifier));

                    if (deletedReport != null)
                        drawingSurface.SurfaceBase.GetReport.Remove(deletedReport);
                }
            }

        }

        public HashSet<ObstacleReport> IgnoredObstacleList => _vModel.IgnoredObstacleList;
    }
}
