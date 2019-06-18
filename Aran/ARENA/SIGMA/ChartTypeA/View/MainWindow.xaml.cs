﻿using System;
using System.Collections.Generic;
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

namespace ChartTypeA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vModel = new ViewModels.MainViewModel();
            this.DataContext = vModel;
            vModel.RequestClose += () => this.Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var vModel = this.DataContext as ViewModels.ViewModel;
            if (vModel!=null)
                vModel.Clear();
        }
    }
}