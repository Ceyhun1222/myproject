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

namespace Aran.Omega.View
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
            tblVersionDate.Text = "Version Date : " + CommonFunctions.RetrieveLinkerTimestamp().ToString("yyyy.MM.dd");

            tblModules.Text = "Modules:  Annex 14";
#if Etod
            {
                tblModules.Text += ",Annex 15";
            }
#endif
            IsClosed = false;
        }

        public bool IsClosed { get; set; }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }
    }
}
