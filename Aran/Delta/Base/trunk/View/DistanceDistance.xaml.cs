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
using System.Windows.Shapes;

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for DistanceTest.xaml
    /// </summary>
    public partial class DistanceDistance : Window
    {
        private ViewModels.CalculatorViewModel _calculatorViewModel;
        public DistanceDistance()
        {
            InitializeComponent();
            _calculatorViewModel = new ViewModels.CalculatorViewModel(Enums.CalcultaionType.DistanceDistance);
            _calculatorViewModel.RequestClose +=()=> this.Close();
            this.DataContext = _calculatorViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _calculatorViewModel.Clear();
        }
    }
}
