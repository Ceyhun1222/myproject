using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Controls;

namespace PVT.UI.View.UserControls
{
    /// <summary>
    /// Interaction logic for HoldingViewGrid.xaml
    /// </summary>
    public partial class HoldingViewGrid 
    {
        public HoldingViewGrid()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty OpenCommandProperty = DependencyProperty.Register("OpenCommand", typeof(RelayCommand<RoutedEventArgs>), typeof(HoldingViewGrid), new UIPropertyMetadata(null));
        public RelayCommand<RoutedEventArgs> OpenCommand
        {
            get => (RelayCommand<RoutedEventArgs>)GetValue(OpenCommandProperty);
            set => SetValue(OpenCommandProperty, value);
        }


        public static readonly DependencyProperty SelectCommandProperty = DependencyProperty.Register("SelectCommand", typeof(RelayCommand<SelectionChangedEventArgs>), typeof(HoldingViewGrid), new UIPropertyMetadata(null));
        public RelayCommand<SelectionChangedEventArgs> SelectCommand
        {
            get => (RelayCommand<SelectionChangedEventArgs>)GetValue(SelectCommandProperty);
            set => SetValue(SelectCommandProperty, value);
        }

        public static readonly DependencyProperty ClearProperty = DependencyProperty.Register("Clear", typeof(bool), typeof(HoldingViewGrid), new UIPropertyMetadata(false, ClearPropertyChanged));
        public bool Clear
        {
            get => (bool)GetValue(ClearProperty);
            set => SetValue(ClearProperty, value);
        }

        public static void ClearPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                ((DataGrid)d).UnselectAll();
            }
        }
    }
}
