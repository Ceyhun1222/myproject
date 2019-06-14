using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MvvmCore;

namespace TOSSM.View.Control
{
    public class TraditionalPropertyPrecisionViewModel : ViewModelBase
    {
        private int _integerPart=-1;
        public int IntegerPart
        {
            get => _integerPart;
            set
            {
                if (_integerPart == value) return;
                var v = value;
                if (v < -1) v = -1;
                if (v > 16) v = 16;
                if (_integerPart == v) return;
                _integerPart= v;
                UpdateValue();
                OnPropertyChanged("IntegerPart");
            }
        }

        private int _fractionalPart=-1;
      
        public int FractionalPart
        {
            get => _fractionalPart;
            set
            {
                if (_fractionalPart == value) return;
                var v = value;
                if (v < -1) v = -1;
                if (v > 16) v = 16;
                if (_fractionalPart == v) return;
                _fractionalPart= v;
                UpdateValue();
                OnPropertyChanged("FractionalPart");
            }
        }

        private void UpdateValue()
        {
            Value = (IntegerPart + 1) * 18 + FractionalPart+1;
        }


        private string _toolTip;
        public string ToolTip
        {
            get => _toolTip;
            set
            {
                _toolTip = value;
                OnPropertyChanged("ToolTip");
            }
        }

        public Action<int> OnValueChanged;
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (_value==value) return;
                _value = value;

                _integerPart = (Value / 18) - 1;
                _fractionalPart = (Value % 18) - 1;

                OnPropertyChanged("IntegerPart");
                OnPropertyChanged("FractionalPart");

                if (OnValueChanged != null)
                {
                    OnValueChanged(Value);
                }

                if (IntegerPart == -1 && FractionalPart == -1)
                {
                    ToolTip = "No format is applied, value is shown as is.";
                }
                else if (IntegerPart == -1)
                {
                    ToolTip = "Value is formatted to show exactly " +
                                FractionalPart + " digits after comma.";
                }
                else if (FractionalPart == -1)
                {
                    ToolTip = "Value is formatted to show at least " + IntegerPart + " digits before comma.";
                }
                else
                {
                    ToolTip = "Value is formatted to show at least " + IntegerPart + " digits before and exactly " +
                                FractionalPart + " digits after comma.";
                }

              
            }
        }

        private Enum _enumValue;
        public Enum EnumValue
        {
            get => _enumValue;
            set
            {
                _enumValue = value;
                OnPropertyChanged("EnumValue");
            }
        }

        private string _enumValueDescription;
        public string EnumValueDescription
        {
            get => _enumValueDescription;
            set
            {
                _enumValueDescription = value;
                OnPropertyChanged("EnumValueDescription");
            }
        }
    }

    /// <summary>
    /// Interaction logic for TraditionalPropertyPrecisionControl.xaml
    /// </summary>
    public partial class TraditionalPropertyPrecisionControl 
    {
        public TraditionalPropertyPrecisionControl()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                ViewModel = new TraditionalPropertyPrecisionViewModel
                {
                    OnValueChanged= v => { Value = v; },
                    Value=Value,
                    EnumValue = EnumValue,
                    EnumValueDescription = EnumValueDescription
                };
            };
        }

        #region ViewModel dependency property

        public static readonly DependencyProperty ViewModelProperty =
         DependencyProperty.Register("ViewModel",
         typeof(TraditionalPropertyPrecisionViewModel),
         typeof(TraditionalPropertyPrecisionControl),
         new UIPropertyMetadata(null));

        public TraditionalPropertyPrecisionViewModel ViewModel
        {
            get => (TraditionalPropertyPrecisionViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        #endregion

        #region Value Dependency Property

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register("Value", 
            typeof(int), 
            typeof(TraditionalPropertyPrecisionControl),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValuePropertyChanged));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TraditionalPropertyPrecisionControl)d;
            var newPropertyValue = (int)e.NewValue;

            if (instance.ViewModel != null)
            {
                instance.ViewModel.Value=newPropertyValue;
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
            typeof(TraditionalPropertyPrecisionControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnEnumValuePropertyChanged));

        private static void OnEnumValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TraditionalPropertyPrecisionControl)d;
            if (instance.ViewModel != null)
            {
                var newPropertyValue = (Enum)e.NewValue;
                instance.ViewModel.EnumValue = newPropertyValue;
            }
        }

        #endregion

        #region Enum Description Dependency Property

        public string EnumValueDescription
        {
            get => (string)GetValue(EnumValueDescriptionProperty);
            set => SetValue(EnumValueDescriptionProperty, value);
        }

        public static readonly DependencyProperty EnumValueDescriptionProperty =
           DependencyProperty.Register("EnumValueDescription",
           typeof(string),
           typeof(TraditionalPropertyPrecisionControl),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnEnumValueDescriptionPropertyChanged));

        private static void OnEnumValueDescriptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TraditionalPropertyPrecisionControl)d;
            if (instance.ViewModel != null)
            {
                var newPropertyValue = (string)e.NewValue;
                instance.ViewModel.EnumValueDescription = newPropertyValue;
            }
        }

        #endregion

        private void TraditionalPropertyPrecisionControl_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
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
