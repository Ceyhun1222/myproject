using System;
using System.Collections.Generic;
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

namespace FAS
{
    /// <summary>
    /// Interaction logic for LatLonControl.xaml
    /// </summary>
    public partial class LatLonControl : UserControl
    {
        private bool _isLatitude;

        public LatLonControl()
        {
            InitializeComponent();

            IsLatitude = true;
        }

        public bool IsLatitude
        {
            get { return _isLatitude; }
            set
            {
                if (_isLatitude == value)
                    return;

                _isLatitude = value;

                ui_cb.Items.Clear();

                if (IsLatitude)
                {
                    ui_cb.Items.Add("N");
                    ui_cb.Items.Add("S");
                }
                else
                {
                    ui_cb.Items.Add("W");
                    ui_cb.Items.Add("E");
                }

                ui_cb.SelectedIndex = 0;
            }
        }


        #region Static

        public static readonly DependencyProperty ValueProperty;

        static LatLonControl()
        {
            ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(double), typeof(LatLonControl));
        }

        public static double GetValue(DependencyObject obj)
        {
            return (double)obj.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject obj, double value)
        {
            obj.SetValue(ValueProperty, value);
        }

        #endregion


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Global.IsTextAllowed(e.Text, ui_second.Equals(sender));
            //e.Handled = new Regex("[^0-9.-]+").IsMatch(e.Text);
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!Global.IsTextAllowed(text, ui_second.Equals(sender)))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //double val = ;
        }

        
    }
}
