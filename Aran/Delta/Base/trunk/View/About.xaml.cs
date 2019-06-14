using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            tblVersion.Text = "Version :  " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            tblVersionDate.Text = "Version Date : "+ Functions.RetrieveLinkerTimestamp().ToString("MM.dd.yyyy");
             IsClosed = false;
        }

        public bool IsClosed { get; set; }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }
    }
}
