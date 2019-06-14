using Aerodrome.Enums;
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

namespace WpfEnvelope.Crud.UserControls
{
    /// <summary>
    /// Interaction logic for NullableSimpleControl.xaml
    /// </summary>
    public partial class NullableSimpleControl : UserControl
    {
        public NullableSimpleControl()
        {
            InitializeComponent();
            nilReasonCbx.ItemsSource = typeof(NilReason).GetEnumValues();
            nilReasonCbx.SelectedIndex = 0;
            nullOrNotCheckbox.IsChecked = true;
        }

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set
            {
                SetValue(SourceProperty, value);

                var instanceProps = PropertyType.GetProperties();
                //надо смотреть не сам source а NilReason. если нуль тогда заходим
                if (value is null)
                {
                    Value = null;
                    ValueChanged?.Invoke(null, null);
                    return;
                }
                if (instanceProps[0].GetValue(value) is null)
                {

                    valueTbx.Visibility = Visibility.Visible;
                    nullOrNotCheckbox.IsChecked = false;
                    nilReasonCbx.Visibility = Visibility.Collapsed;
                    valueTbx.Text = instanceProps[1].GetValue(Source)?.ToString();
                    Value = valueTbx.Text;
                    ValueChanged?.Invoke(null, null);
                }
                else
                {
                    nullOrNotCheckbox.IsChecked = true;
                    nilReasonCbx.SelectedItem = instanceProps[0].GetValue(value);
                    Value = null;
                    ValueChanged?.Invoke(null, null);
                }

            }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(NullableSimpleControl), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(NullableSimpleControl.OnSourceChanged)));


        public Type PropertyType
        {
            get { return (Type)GetValue(PropertyTypeProperty); }
            set
            {
                SetValue(PropertyTypeProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PropertyType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyTypeProperty =
            DependencyProperty.Register("PropertyType", typeof(Type), typeof(NullableSimpleControl), null);


        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(NullableSimpleControl), null);


        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NullableSimpleControl control = (NullableSimpleControl)d;

            control.ValueChanged?.Invoke(null, null);
        }

        public SelectionChangedEventHandler ValueChanged;


        private void nilReasonCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Value = null;
            ValueChanged?.Invoke(null, null);
        }

        private void NullOrNotCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (nullOrNotCheckbox.IsChecked.Value)
            {
                valueTbx.Visibility = Visibility.Collapsed;
                nilReasonCbx.Visibility = Visibility.Visible;
                nilReasonCbx.SelectedItem = NilReason.NotEntered;
                Value = null;
                ValueChanged?.Invoke(null, null);
            }
            else
            {
                nilReasonCbx.Visibility = Visibility.Collapsed;
                valueTbx.Visibility = Visibility.Visible;
                Value = valueTbx.Text;
                ValueChanged?.Invoke(null, null);
            }
        }

      
        private void valueTbx_LostFocus(object sender, RoutedEventArgs e)
        {
            Value = valueTbx.Text;
            ValueChanged?.Invoke(null, null);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var instanceProps = PropertyType.GetProperties();
            if(instanceProps[1].PropertyType.Name.Equals(typeof(double).Name))
            {
                e.Handled = !IsTextAllowed(e.Text);
            }
            
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
