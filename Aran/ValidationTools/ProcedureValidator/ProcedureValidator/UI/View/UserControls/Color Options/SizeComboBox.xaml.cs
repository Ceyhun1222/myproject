using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PVT.UI.View.UserControls
{
    /// <summary>
    /// Interaction logic for SizeComboBox.xaml
    /// </summary>
    public partial class SizeComboBox
    {
        public SizeComboBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SizesProperty = DependencyProperty.Register("Sizes", typeof(ObservableCollection<int>), typeof(SizeComboBox), new UIPropertyMetadata(CreateSizes()));
        public ObservableCollection<int> Sizes
        {
            get
            {
                return (ObservableCollection<int>)GetValue(SizesProperty);
            }
            set
            {
                SetValue(SizesProperty, value);
            }
        }

        public static ObservableCollection<int> CreateSizes()
        {
            var PointStyles = new ObservableCollection<int>();
            PointStyles.Add(1);
            PointStyles.Add(2);
            PointStyles.Add(3);
            PointStyles.Add(4);
            return PointStyles;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Loaded += AdjustWidth;
        }

        private double GetParentHeight()
        {
            if (Parent is Grid)
            {
                var grid = Parent as Grid;
                var col = (int)GetValue(Grid.RowProperty);

                return grid.RowDefinitions[col].ActualHeight - Margin.Top - Margin.Bottom;
            }

            var panel = Parent as Panel;
            return panel != null ? panel.ActualHeight - Margin.Top - Margin.Bottom : 0;
        }


        private void AdjustWidth(object sender, EventArgs e)
        {
            var parentHeight = GetParentHeight();
            Height = parentHeight;
            MaxHeight = parentHeight;
        }
    }
}
