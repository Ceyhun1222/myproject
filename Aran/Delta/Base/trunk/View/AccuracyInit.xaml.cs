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

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for Accuracy.xaml
    /// </summary>
    public partial class AccuracyInit : MetroWindow
    {
        //private ViewModels.AccuracyInitViewModel _accuracyInitViewModel;

        public AccuracyInit()
        {
            InitializeComponent();
            //_accuracyInitViewModel = new ViewModels.AccuracyInitViewModel();
            //_accuracyInitViewModel.RequestClose += () => this.Close();
            //this.DataContext = _accuracyInitViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void ContinueCommand_Click(object sender, RoutedEventArgs e)
        {
            var result = double.TryParse(Accuracy.Text, out var accuracy);
            if (result && (2000 > accuracy && accuracy > 0))
            {
                GlobalParams.Accuracy = accuracy;
                this.DialogResult = true;
            }
            else
            {
                Model.Messages.Error("Accuracy should be in range: 0m - 2000m");
            }
        }
    }
}
