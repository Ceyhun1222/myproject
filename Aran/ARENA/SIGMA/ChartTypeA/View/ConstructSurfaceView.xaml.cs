using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ChartTypeA.View
{
    /// <summary>
    /// Interaction logic for ConstructSurfaceView.xaml
    /// </summary>
    public partial class ConstructSurfaceView : UserControl
    {
        public ConstructSurfaceView()
        {
            InitializeComponent();
        }

        private void ConstructSurfaceView_OnLoaded(object sender, RoutedEventArgs e)
        {
            PenetrateDataGrid.Columns[2].Header += "(" + InitChartTypeA.DistanceConverter.Unit+")";
            PenetrateDataGrid.Columns[3].Header += "(" + InitChartTypeA.HeightConverter.Unit+ " )";
            PenetrateDataGrid.Columns[4].Header += "(" + InitChartTypeA.HeightConverter.Unit + " )";

            ObstacleInChartDataGrid.Columns[2].Header += "(" + InitChartTypeA.DistanceConverter.Unit  + " )";
            ObstacleInChartDataGrid.Columns[3].Header += "(" + InitChartTypeA.HeightConverter.Unit + " )";
            ObstacleInChartDataGrid.Columns[4].Header += "(" + InitChartTypeA.HeightConverter.Unit + " )";
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TxtLatDeg_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text) && Convert.ToDouble(e.Text) > 0 && Convert.ToDouble(e.Text)<180;
        }
    }
}
