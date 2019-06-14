using Aerodrome.Enums;
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

namespace WpfEnvelope.Crud.UserControls
{
    /// <summary>
    /// Interaction logic for NullableDateTimeControl.xaml
    /// </summary>
    public partial class NullableDateTimeControl : UserControl
    {
        public NullableDateTimeControl()
        {
            InitializeComponent();
            var sourceList= typeof(NilReason).GetEnumValues().Cast<Enum>().ToList();
            sourceList.Remove(NilReason.Unknown);
            nilReasonCbx.ItemsSource = sourceList;
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

                    valueDatePicker.Visibility = Visibility.Visible;
                    nullOrNotCheckbox.IsChecked = false;
                    nilReasonCbx.Visibility = Visibility.Collapsed;
                    if (((DateTime)instanceProps[1].GetValue(Source)).Equals(default(DateTime)))
                        valueDatePicker.SelectedDate = DateTime.Now;
                    else
                        valueDatePicker.SelectedDate = (DateTime)instanceProps[1].GetValue(Source);
                }
                else
                {
                    nullOrNotCheckbox.IsChecked = true;
                    nilReasonCbx.SelectedItem = instanceProps[0].GetValue(value);
                }

            }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(NullableDateTimeControl), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(NullableDateTimeControl.OnSourceChanged)));


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
            DependencyProperty.Register("PropertyType", typeof(Type), typeof(NullableDateTimeControl), null);


        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(NullableDateTimeControl), null);


        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NullableDateTimeControl control = (NullableDateTimeControl)d;

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
                valueDatePicker.Visibility = Visibility.Collapsed;
                nilReasonCbx.Visibility = Visibility.Visible;
                nilReasonCbx.SelectedItem = NilReason.NotEntered;
                Value = null;
                ValueChanged?.Invoke(null, null);
            }
            else
            {
                nilReasonCbx.Visibility = Visibility.Collapsed;
                valueDatePicker.Visibility = Visibility.Visible;
                Value = valueDatePicker.SelectedDate;
                ValueChanged?.Invoke(null, null);
            }
        }

        private void valueDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Value = valueDatePicker.SelectedDate;
            ValueChanged?.Invoke(null, null);
        }
    }
}
