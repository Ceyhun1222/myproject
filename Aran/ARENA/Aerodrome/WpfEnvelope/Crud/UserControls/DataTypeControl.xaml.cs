using Aerodrome.Features;
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
using Aerodrome.DataType;
using Aerodrome.Enums;
using System.Text.RegularExpressions;

namespace WpfEnvelope.Crud.UserControls
{
    /// <summary>
    /// Interaction logic for DataTypeControl.xaml
    /// </summary>
    public partial class DataTypeControl : UserControl
    {
        public DataTypeControl()
        {
            InitializeComponent();
           
        }


        public static readonly DependencyProperty SourceProperty =
             DependencyProperty.Register("Source", typeof(DataType<Enum>), typeof(DataTypeControl), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(DataTypeControl.OnSourceChanged)));

        public static readonly DependencyProperty SelectedProperty =
             DependencyProperty.Register("Selected", typeof(DataType<Enum>), typeof(DataTypeControl), new FrameworkPropertyMetadata(null,null));

        public static readonly DependencyProperty ComboSourceProperty =
             DependencyProperty.Register("ComboSource", typeof(Array), typeof(DataTypeControl), new FrameworkPropertyMetadata(null, null));


        public DataType<Enum> Source
        {
            get { return (DataType<Enum>)GetValue(SourceProperty); }
            set
            {
                SetValue(SourceProperty, value);
                comboBoxUnit.SelectedItem = Source.Uom;
                textBoxValue.Text = Source.Value.ToString();
            }
        }

        public DataType<Enum> Selected
        {
            get { return (DataType<Enum>)GetValue(SelectedProperty); }
            set
            {
                SetValue(SelectedProperty, value);              
            }
        }

        public Array ComboSource
        {
            get { return (Array)GetValue(ComboSourceProperty); }
            set
            {
                SetValue(ComboSourceProperty, value);
                comboBoxUnit.ItemsSource = ComboSource;//Source.Uom.GetType().GetEnumValues();
            }
        }

        public SelectionChangedEventHandler ValueChanged;
     


        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataTypeControl control = (DataTypeControl)d;
            
            control.ValueChanged?.Invoke(null, null);
        }

        private void textBoxValue_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (textBoxValue.Text.Equals(string.Empty))
            {
                Selected = null;
                ValueChanged?.Invoke(null, null);
                return;
            }
            //Set selected Value
            double val = 0;
            if (!Double.TryParse(textBoxValue.Text, out val))
            {
                textBoxValue.Text = String.Empty;
                //Selected.Value = 0;
                return;
            }
            if (Selected == null) Selected = new DataType<Enum>();
            Selected.Value = val;
            Selected.Uom = (Enum)comboBoxUnit.SelectedValue;
            ValueChanged?.Invoke(null, null);
        }

        private void comboBoxUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Set selected Value
            if (Selected == null) return;
            Selected.Uom = (Enum)comboBoxUnit.SelectedValue;
            ValueChanged?.Invoke(null, null);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {            
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }       
    }
}
