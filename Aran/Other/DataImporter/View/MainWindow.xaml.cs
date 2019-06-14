using System;
using Aran.AranEnvironment;
using DataImporter.ViewModels;
using MahApps.Metro.Controls;

namespace DataImporter.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainViewModel _vModel;
        
        public MainWindow(IImportPageVM vModel)
        {
            InitializeComponent();
            this.DataContext = vModel;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
