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
    /// Interaction logic for CreateNotam.xaml
    /// </summary>
    public partial class CreateNotam : Window
    {
        private ViewModels.CreateNotamViewModel _notamViewModel;
        public CreateNotam()
        {
            InitializeComponent();
            _notamViewModel = new ViewModels.CreateNotamViewModel();
            this.DataContext = _notamViewModel;
        }
    }
}
