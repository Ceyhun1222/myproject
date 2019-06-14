using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Intersection.xaml
    /// </summary>
    public partial class Intersection : Window
    {
        private ViewModels.IntersectionViewModel _intersectVM;
        public Intersection()
        {
            InitializeComponent();
            _intersectVM =  new ViewModels.IntersectionViewModel();
            _intersectVM.RequestClose += () => this.Close();
            this.DataContext = _intersectVM;
        }

        private void Intersection_OnClosing(object sender, CancelEventArgs e)
        {
            _intersectVM.Clear();
        }
    }
}
