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

namespace Aran.Delta
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class CourseDistance : Window
    {
        private ViewModels.CalculatorViewModel _calculatorViewModel;
        public CourseDistance()
        {
            InitializeComponent();
            _calculatorViewModel = new ViewModels.CalculatorViewModel(Enums.CalcultaionType.CourseDistance);
            _calculatorViewModel.RequestClose +=()=> this.Close();
            this.DataContext = _calculatorViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _calculatorViewModel.Clear();
        }
    }
}
