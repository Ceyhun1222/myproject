using Aerodrome.Features;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Windows.Shapes;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for ARPInputWindow.xaml
    /// </summary>
    public partial class ARPInputWindow : Window
    {
        public AM_AerodromeReferencePoint ARP { get; set; }
        public ARPInputWindow()
        {
            InitializeComponent();
            ARP = new AM_AerodromeReferencePoint();
            ARP.featureID = Guid.NewGuid().ToString();
            this.DataContext = ARP;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Validator.IsValid(this)) // is valid
                return;

            double longDeg, longMin, longSec, latDeg, latMin, latSec;
            if(!Double.TryParse(longDegTbx.Text,out longDeg) || !Double.TryParse(longMinTbx.Text, out longMin) 
                || !Double.TryParse(longSecTbx.Text, out longSec) || !Double.TryParse(latgDegTbx.Text, out latDeg) 
                || !Double.TryParse(latMinTbx.Text, out latMin) || !Double.TryParse(latSecTbx.Text, out latSec))
            {
                MessageBox.Show("Uncorrect coordinate value", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string selectedLong = longCbx.Text.ToString();
            string selectedLat = latCbx.Text.ToString();
            int longSign, latSign;
            if (selectedLong.Equals("W"))
                longSign = -1;
            else
                longSign = 1;

            if (selectedLat.Equals("S"))
                latSign = -1;
            else
                latSign = 1;

            ARP.geopnt = new PointClass()
            {
                X = longSign*(longDeg + longMin / 60 + longSec / 3600),
                Y = latSign*(latDeg + latMin / 60 + latSec / 3600)
            };
            //MessageBox.Show(longSign * (longDeg + longMin / 60 + longSec / 3600)+ "||||" + latSign * (latDeg + latMin / 60 + latSec / 3600),"", MessageBoxButton.OK, MessageBoxImage.Error);
            DialogResult = true;

        }

       
    }
   
}
