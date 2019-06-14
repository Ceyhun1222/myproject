using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DmsControll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DmsControl : UserControl
    {
        public DmsControl()
        {
            InitializeComponent();

            CmbLatDirList.Items.Add("N");
            CmbLatDirList.Items.Add("S");

            CmbLongDirList.Items.Add("E");
            CmbLongDirList.Items.Add("W");

            IsEnabledP = true;

            Longtitude = 0;
            Longtitude = 0;

            CmbLatDirList.SelectedIndex = 0;
            CmbLongDirList.SelectedIndex = 0;
        }



        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(decimal), typeof(DmsControl),
            new UIPropertyMetadata(0m));


        public static readonly DependencyProperty LatitudeStringProperty = DependencyProperty.Register("LatitudeString", typeof(string), typeof(DmsControl),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty LongtitudeStringProperty = DependencyProperty.Register("LongtitudeString", typeof(string), typeof(DmsControl),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty LatitudeProperty = DependencyProperty.Register("Latitude", typeof(double), typeof(DmsControl),
            new UIPropertyMetadata(0.1, new PropertyChangedCallback(Latitude_Changed)));

        public static readonly DependencyProperty LongtitudeProperty = DependencyProperty.Register("Longtitude", typeof(double), typeof(DmsControl),
            new UIPropertyMetadata(0.1, new PropertyChangedCallback(Longtitude_Changed)));

        public static readonly DependencyProperty AccuracyProperty = DependencyProperty.Register("Accuracy", typeof(int), typeof(DmsControl),
            new UIPropertyMetadata(new int(), new PropertyChangedCallback(Accuracy_Changed)));

        public static readonly DependencyProperty IsEnabledProperties = DependencyProperty.Register("IsEnabledP", typeof(bool), typeof(DmsControl),
            new UIPropertyMetadata(new bool(), new PropertyChangedCallback(IsEnabled_Changed)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Point), typeof(DmsControl),
            new UIPropertyMetadata(new Point(), new PropertyChangedCallback(Value_Changed)));

        public static readonly DependencyProperty IsFullDescriptionProperty = DependencyProperty.Register("IsFullDescription", typeof(bool), typeof(DmsControl),
            new UIPropertyMetadata(false, new PropertyChangedCallback(IsFullDescription_Changed)));


        private static void IsEnabled_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DmsControl sender)
            {
                sender.MainGrid.IsEnabled = (bool)e.NewValue;
            }
        }

        private static void Longtitude_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DmsControl sender))
                return;

            var newValue = (double) e.NewValue;
            //if (sender.Longtitude == newValue)
            //    return;

            Functions.DD2DMS(newValue, out double xDeg, out double xMin, out double xSec, Math.Sign((double)e.NewValue));

            sender.TxtLongDeg.Text = xDeg.ToString("00#");
            sender.TxtLongMin.Text = xMin.ToString("0#");
            sender.TxtLongSec.Text = xSec.ToString("00");

            /*var secDecimalSize = 0;
            if (xSec.ToString().IndexOf(".") > -1)
                secDecimalSize = xSec.ToString().Length - xSec.ToString().IndexOf(".") - 1;

            sender.SetResolution(sender, secDecimalSize);*/

            if (sender.Accuracy > 0)
                sender.TxtLongSec.Text = xSec.ToString("00." + new string('0', sender.Accuracy));
            else if (sender.Accuracy == -1)
                sender.TxtLongSec.IsEnabled = false;

            sender.LongtitudeString = $"{sender.TxtLongDeg.Text}.{sender.TxtLongMin.Text}.{sender.TxtLongSec.Text}";

            if (newValue >= 0)
                sender.CmbLongDirList.SelectedIndex = 0;
            else
                sender.CmbLongDirList.SelectedIndex = 1;

            sender.Value = new Point(sender.Longtitude, sender.Latitude);
        }

        private static void Latitude_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DmsControl sender))
                return;

            var newValue = (double) e.NewValue;

            //if (sender.Latitude == newValue)
            //    return;

            Functions.DD2DMS(newValue, out double xDeg, out double xMin, out double xSec, 1);

            sender.TxtLatDeg.Text = xDeg.ToString("0#");
            sender.TxtLatMin.Text = xMin.ToString("0#");
            sender.TxtLatSec.Text = xSec.ToString("00");

            /*var secDecimalSize = 0;
            if (xSec.ToString().IndexOf(".") > -1)
                secDecimalSize = xSec.ToString().Length - xSec.ToString().IndexOf(".") - 1;

            sender.SetResolution(sender, secDecimalSize);*/

            if (sender.Accuracy > 0)
                sender.TxtLatSec.Text = xSec.ToString("00." + new string('0', sender.Accuracy));
            else if(sender.Accuracy == -1)
                sender.TxtLatSec.IsEnabled = false;

            sender.LatitudeString = $"{sender.TxtLatDeg.Text}.{sender.TxtLatMin.Text}.{sender.TxtLatSec.Text}";

            if (newValue >= 0)
                sender.CmbLatDirList.SelectedIndex = 0;
            else
                sender.CmbLatDirList.SelectedIndex = 1;

            sender.Value = new Point(sender.Longtitude, sender.Latitude);
        }

        private static void Accuracy_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DmsControl sender))
                return;

            sender.Resolution = (decimal)(1 / Math.Pow(10, sender.Accuracy));
        }

        private static void Value_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DmsControl sender))
                return;

            var newValue = (Point)e.NewValue;

            if (sender.Latitude != newValue.Y)
                sender.Latitude = newValue.Y;

            if (sender.Longtitude != newValue.X)
                sender.Longtitude = newValue.X;
        }

        private static void IsFullDescription_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
 
        }

        public decimal Resolution
        {
            get { return (decimal)GetValue(ResolutionProperty); }
            set { SetValue(ResolutionProperty, value); }
        }

        public string LatitudeString
        {
            get { return (string)GetValue(LatitudeStringProperty); }
            set { SetValue(LatitudeStringProperty, value); }
        }

        public string LongtitudeString
        {
            get { return (string)GetValue(LongtitudeStringProperty); }
            set { SetValue(LongtitudeStringProperty, value); }
        }

        public double Latitude
        {
            get { return (double)GetValue(LatitudeProperty); }
            set { SetValue(LatitudeProperty, value); }
        }

        public double Longtitude
        {
            get { return (double)GetValue(LongtitudeProperty); }
            set { SetValue(LongtitudeProperty, value);  }
        }

        public Point Value
        {
            get { return (Point)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool IsFullDescription
        {
            get { return (bool)GetValue(IsFullDescriptionProperty); }
            set { SetValue(IsFullDescriptionProperty, value); }
        }

        public int Accuracy
        {
            get { return (int)GetValue(AccuracyProperty); }
            set { SetValue(AccuracyProperty, value); }
        }

        public bool IsEnabledP
        {
            get { return (bool)GetValue(IsEnabledProperties); }

            set { SetValue(IsEnabledProperties, value); }
}

        private static bool IsTextAllowed(string text)
        {
            var s = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            Regex regex = new Regex($"[^0-9{s}-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void TxtLatDeg_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TxtLatDeg_OnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                bool isLatChanged = false;

                if (sender.Equals(TxtLatDeg))
                {
                    var tmpVal = Convert.ToDouble(TxtLatDeg.Text);
                    if (tmpVal < 0)
                        tmpVal = 0;
                    else if (tmpVal > 89)
                        tmpVal = 89;

                    TxtLatDeg.Text = tmpVal.ToString("0#");
                    isLatChanged = true;
                }
                else if (sender.Equals(TxtLongDeg))
                {
                    var tmpVal = Convert.ToDouble(TxtLongDeg.Text);
                    if (tmpVal < 0)
                        tmpVal = 0;
                    else if (tmpVal > 179)
                        tmpVal = 179;

                    TxtLongDeg.Text = tmpVal.ToString("00#");
                }
                else if (sender.Equals(TxtLatMin) || sender.Equals(TxtLongMin))
                {
                    var textBox = sender as TextBox;
                    if (textBox != null)
                    {
                        var tmpVal = Convert.ToDouble(textBox.Text);

                        if (tmpVal < 0)
                            tmpVal = 0;
                        else if (tmpVal > 59)
                            tmpVal = 59;
                        textBox.Text = tmpVal.ToString("0#");
                    }
                    isLatChanged = sender.Equals(TxtLatMin);
                }
                else if (sender.Equals(TxtLatSec) || sender.Equals(TxtLongSec))
                {
                    if (sender is TextBox textBox)
                    {
                        var tmpVal = Math.Round(Convert.ToDouble(textBox.Text), Accuracy);

                        if (tmpVal < 0)
                            tmpVal = 0;
                        else if (tmpVal >= 60)
                            tmpVal = Convert.ToDouble(59.99999.ToString().Substring(0, 3 + Accuracy));

                        textBox.Text = tmpVal.ToString("00." + new string('0', Accuracy));
                    }
                    isLatChanged = sender.Equals(TxtLatSec);
                }

                if (isLatChanged)
                {
                    Latitude = Functions.DMS2DD(Convert.ToDouble(TxtLatDeg.Text), Convert.ToDouble(TxtLatMin.Text),
                        Convert.ToDouble(TxtLatSec.Text), 1);
                }
                else
                {
                    Longtitude = Functions.DMS2DD(Convert.ToDouble(TxtLongDeg.Text), Convert.ToDouble(TxtLongMin.Text),
                        Convert.ToDouble(TxtLongSec.Text), 1);
                }
            }
            catch
            {

            }
        }

        private void CmbLatDirList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbLatDirList.SelectedIndex == 0)
                Latitude = Math.Abs(Latitude);
            else
                Latitude =- Math.Abs(Latitude);
        }

        private void CmbLongDirList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbLongDirList.SelectedIndex == 0)
                Longtitude = Math.Abs(Longtitude);
            else
                Longtitude = -Math.Abs(Longtitude);
        }

        private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) textBox.SelectAll();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var ue = sender as FrameworkElement;
                if (e.Key == Key.Enter)
                {
                    if (ue.Tag != null && ue.Tag.ToString() == "IgnoreEnterKeyTraversal")
                    {
                        //ignore
                    }
                    else
                    {
                        e.Handled = true;
                        ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetResolution(DmsControl sender, int secondDecimalSize)
        {
            if (sender == null)
                return;

            if (secondDecimalSize == 0)
                sender.Resolution = 1m;

            else if (secondDecimalSize == 1 && sender.Resolution < 1m)
                sender.Resolution = 0.1m;

            else if (secondDecimalSize > 1 && sender.Resolution < 0.1m)
                sender.Resolution = Decimal.Parse("0." + new string('0', secondDecimalSize - 1) + "1m");
        }
    }
}
