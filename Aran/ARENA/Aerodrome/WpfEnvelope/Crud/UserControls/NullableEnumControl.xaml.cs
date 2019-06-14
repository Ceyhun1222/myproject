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
    /// Interaction logic for NullableControl.xaml
    /// </summary>
    public partial class NullableEnumControl : UserControl
    {
        public NullableEnumControl()
        {
            InitializeComponent();

            nilReasonCbx.ItemsSource=typeof(NilReason).GetEnumValues();
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

                    valueCbx.Visibility = Visibility.Visible;
                    nullOrNotCheckbox.IsChecked = false;
                    nilReasonCbx.Visibility = Visibility.Collapsed;
                    valueCbx.SelectedItem = (Enum)instanceProps[1].GetValue(value);
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
            DependencyProperty.Register("Source", typeof(object), typeof(NullableEnumControl), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(NullableEnumControl.OnSourceChanged)));


        public Type PropertyType
        {
            get { return (Type)GetValue(PropertyTypeProperty); }
            set
            {
                SetValue(PropertyTypeProperty, value);
                valueCbx.ItemsSource = value.GetGenericArguments()[0].GetEnumValues();
            }
        }

        // Using a DependencyProperty as the backing store for PropertyType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyTypeProperty =
            DependencyProperty.Register("PropertyType", typeof(Type), typeof(NullableEnumControl), null);


        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(NullableEnumControl), null);


        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NullableEnumControl control = (NullableEnumControl)d;
           
            control.ValueChanged?.Invoke(null, null);
        }

        public SelectionChangedEventHandler ValueChanged;


        private void valueCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Value = valueCbx.SelectedItem;
            ValueChanged?.Invoke(null, null);
        }

        private void nilReasonCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Value = null;
            ValueChanged?.Invoke(null, null);
        }

        private void NullOrNotCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (nullOrNotCheckbox.IsChecked.Value)
            {
                valueCbx.Visibility = Visibility.Collapsed;
                nilReasonCbx.Visibility = Visibility.Visible;
                nilReasonCbx.SelectedItem = NilReason.NotEntered;
                Value = null;
                ValueChanged?.Invoke(null, null);
            }
            else
            {
                nilReasonCbx.Visibility = Visibility.Collapsed;
                valueCbx.Visibility = Visibility.Visible;
                Value = valueCbx.SelectedItem;
                ValueChanged?.Invoke(null, null);
            }
        }
    }
}
