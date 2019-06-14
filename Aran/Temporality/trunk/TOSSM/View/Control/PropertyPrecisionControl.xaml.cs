using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TOSSM.ViewModel.Control.PropertyPrecision;

namespace TOSSM.View.Control
{
    /// <summary>
    /// Interaction logic for PropertyPrecisionControl.xaml
    /// </summary>
    public partial class PropertyPrecisionControl 
    {
        #region Ctor

        public PropertyPrecisionControl()
        {
            Loaded += (a, b) =>
            {
                var model = new PropertyPrecisionControlViewModel
                {
                    UpdateValue = UpdateValue
                };
                model.OnValueChanged(Value);
                model.EnumValue = EnumValue;
                ViewModel = model;
            };

            InitializeComponent();
        }

        #endregion

        #region ViewModel dependency property

        public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", 
            typeof(PropertyPrecisionControlViewModel), 
            typeof(PropertyPrecisionControl), 
            new UIPropertyMetadata(null));

        public PropertyPrecisionControlViewModel ViewModel
        {
            get => (PropertyPrecisionControlViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        #endregion

        #region Value Dependency Property

        private void UpdateValue(int newValue)
        {
            Value = newValue;
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register("Value", 
            typeof(int), 
            typeof(PropertyPrecisionControl),
            new FrameworkPropertyMetadata(100, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValuePropertyChanged));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (PropertyPrecisionControl) d;
            var newPropertyValue = (int)e.NewValue;
            if (instance.ViewModel != null)
            {
                instance.ViewModel.OnValueChanged(newPropertyValue);
            }
        }

        #endregion

        #region Enum Dependency Property

        public Enum EnumValue
        {
            get => (Enum)GetValue(EnumValueProperty);
            set => SetValue(EnumValueProperty, value);
        }

        public static readonly DependencyProperty EnumValueProperty =
            DependencyProperty.Register("EnumValue",
            typeof(Enum),
            typeof(PropertyPrecisionControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnEnumValuePropertyChanged));

        private static void OnEnumValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (PropertyPrecisionControl)d;
            if (instance.ViewModel != null)
            {
                var newPropertyValue = (Enum)e.NewValue;
                instance.ViewModel.EnumValue = newPropertyValue;
            }
        }

        #endregion

        private void PropertyPrecisionControl_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled) return;

            var listView = sender as ListView;
            if (listView==null) return;


            var parent = listView.Parent as UIElement;

            if (parent == null) return;

            e.Handled = true;

            var eventArg = new MouseWheelEventArgs(e.MouseDevice,
                e.Timestamp, e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = parent
            };

            parent.RaiseEvent(eventArg);
        }
    }
}
