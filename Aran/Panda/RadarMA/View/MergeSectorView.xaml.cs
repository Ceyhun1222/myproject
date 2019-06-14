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

namespace Aran.Panda.RadarMA.View
{
    /// <summary>
    /// Interaction logic for MergeSectorView.xaml
    /// </summary>
    public partial class MergeSectorView : Window
    {
        private MergeSectorViewModel vModel;
        public MergeSectorView(
            ObservableCollection<Sector> sectors,
            List<State> stateList,
            IEnumerable<double> mocList,
            IEnumerable<double> bufferValueList)
        {
            InitializeComponent();
            vModel = new MergeSectorViewModel(sectors,stateList,mocList,bufferValueList);
            this.DataContext = vModel;
            vModel.ClosEventHandler+=new EventHandler(CloseForm);
        }

        private void CloseForm(object sender, EventArgs e)
        {
            vModel.Clear();
          Close();
        }

        private void MergeSectorView_OnClosing(object sender, CancelEventArgs e)
        {
            vModel.Clear();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
          //  Close();
                
        }
    }
}
