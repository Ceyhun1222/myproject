using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Aran.Aim;

namespace TOSSM.View.Control
{
    /// <summary>
    /// Interaction logic for FeatureTypeSelector.xaml
    /// </summary>
    public partial class FeatureTypeSelector : UserControl
    {
        public static readonly DependencyProperty SelectedItemProperty = 
            DependencyProperty.Register("SelectedItem", 
            typeof (FeatureType?), 
            typeof (FeatureTypeSelector),
            new PropertyMetadata(default(FeatureType?), OnSelectedItemChanged));

        public static readonly DependencyProperty MySelectedItemProperty =
            DependencyProperty.Register("MySelectedItem",
            typeof(FeatureType?),
            typeof(FeatureTypeSelector));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector=d as FeatureTypeSelector;
            if (selector != null && e.NewValue is FeatureType)
            {
                selector.MySelectedItem = (FeatureType) e.NewValue;
            }
        }

        public FeatureTypeSelector()
        {
            InitializeComponent();
            var dispatcherTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 50)};
            dispatcherTimer.Tick += (a, b) =>
            {
                if (!FeatureTypeComboBox.IsDropDownOpen && MySelectedItem != SelectedItem)
                {
                    SelectedItem = MySelectedItem;
                }
            };
            dispatcherTimer.Start();
        }

        public FeatureType? MySelectedItem
        {
            get => (FeatureType?)GetValue(MySelectedItemProperty);
            set
            {
                SetValue(MySelectedItemProperty, value);
                OnPropertyChanged(new DependencyPropertyChangedEventArgs(MySelectedItemProperty, null, value));
            }
        }

        public FeatureType? SelectedItem
        {
            get => (FeatureType?) GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }


        #region FeatureList


        private List<FeatureType> _featureList;
        public List<FeatureType> FeatureList
        {
            get
            {
                if (_featureList == null)
                {
                    _featureList = new List<FeatureType>();
                    foreach (FeatureType ft in Enum.GetValues(typeof(FeatureType)))
                    {
                        _featureList.Add(ft);
                    }
                    _featureList = new List<FeatureType>(_featureList.OrderBy(t => t.ToString()));
                }
                return _featureList;
            }
        }

        #endregion


        private void FeatureTypeComboBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!FeatureTypeComboBox.IsDropDownOpen && (e.Key == Key.Up || e.Key == Key.Down))
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                if (!FeatureTypeComboBox.IsDropDownOpen && MySelectedItem!=null)
                {
                    SelectedItem = MySelectedItem;
                }
                FeatureTypeComboBox.IsDropDownOpen = false;
            }
            else
            {
               FeatureTypeComboBox.IsDropDownOpen = true;
            }
        }
    }
}
