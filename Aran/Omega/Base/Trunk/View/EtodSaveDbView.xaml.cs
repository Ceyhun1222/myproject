using MahApps.Metro.Controls;
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

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for EtodSaveDbView.xaml
    /// </summary>
    public partial class EtodSaveDbView : MetroWindow
    {
        public EtodSaveDbView()
        {
            InitializeComponent();
        }

        public EtodSaveDbView(List<Models.DrawingSurface> surfaceList):this()
        {
            var vm = new ViewModels.EtodSaveDbViewModel(surfaceList);
            DataContext = vm;
            vm.RequestClose += () => Close();
        }
        
    }
}
