using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Aran.Aim;
using Aran.Aim.Enums;

namespace TOSSM.View.Control
{
    /// <summary>
    /// Interaction logic for FeatureTypeSelector.xaml
    /// </summary>
    public partial class LanguageSelector : UserControl
    {
        public static readonly DependencyProperty SelectedItemProperty = 
            DependencyProperty.Register("SelectedItem", 
            typeof (language?), 
            typeof (LanguageSelector),
            new PropertyMetadata(default(language?), OnSelectedItemChanged));

        public static readonly DependencyProperty MySelectedItemProperty =
            DependencyProperty.Register("MySelectedItem",
            typeof(language?),
            typeof(LanguageSelector));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector=d as LanguageSelector;
            if (selector != null && e.NewValue is language)
            {
                selector.MySelectedItem = (language) e.NewValue;
            }
        }

        public LanguageSelector()
        {
            InitializeComponent();
            var dispatcherTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 50)};
            dispatcherTimer.Tick += (a, b) =>
            {
                if (!LanguageSelectorComboBox.IsDropDownOpen && MySelectedItem != SelectedItem)
                {
                    SelectedItem = MySelectedItem;
                }
            };
            dispatcherTimer.Start();
        }

        public language? MySelectedItem
        {
            get => (language?)GetValue(MySelectedItemProperty);
            set
            {
                SetValue(MySelectedItemProperty, value);
                OnPropertyChanged(new DependencyPropertyChangedEventArgs(MySelectedItemProperty, null, value));
            }
        }

        public language? SelectedItem
        {
            get => (language?) GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }


        #region FeatureList


        private List<language> _languageList;
        public List<language> LanguageList
        {
            get
            {
                if (_languageList == null)
                {
                    _languageList = new List<language>();
                    foreach (language ft in Enum.GetValues(typeof(language)))
                    {
                        _languageList.Add(ft);
                    }
                    _languageList = new List<language>(_languageList.OrderBy(t => t.ToString()));
                }
                return _languageList;
            }
        }

        #endregion


        private void LanguageSelectorComboBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!LanguageSelectorComboBox.IsDropDownOpen && (e.Key == Key.Up || e.Key == Key.Down))
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                if (!LanguageSelectorComboBox.IsDropDownOpen && MySelectedItem!=null)
                {
                    SelectedItem = MySelectedItem;
                }
                LanguageSelectorComboBox.IsDropDownOpen = false;
            }
            else
            {
                LanguageSelectorComboBox.IsDropDownOpen = true;
            }
        }
    }
}
