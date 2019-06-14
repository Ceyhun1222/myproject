using PVT.Model.Drawing;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PVT.UI.View.UserControls
{
    /// <summary>
    /// Interaction logic for PointDrawComboBox.xaml
    /// </summary>
    public partial class PointDrawingComboBox
    {
        public PointDrawingComboBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(string), typeof(PointDrawingComboBox), new UIPropertyMetadata("Advanced"));
        public Brush Color
        {
            get
            {
                return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public static readonly DependencyProperty PointGeometriesProperty = DependencyProperty.Register("PointGeometries", typeof(ObservableCollection<PointGeometry>), typeof(PointDrawingComboBox), new UIPropertyMetadata(CreatePointGeometries()));
        public ObservableCollection<PointGeometry> PointGeometries
        {
            get
            {
                return (ObservableCollection<PointGeometry>)GetValue(PointGeometriesProperty);
            }
            set
            {
                SetValue(PointGeometriesProperty, value);
            }
        }

        public static ObservableCollection<PointGeometry> CreatePointGeometries()
        {
            var PointGeometrys = new ObservableCollection<PointGeometry>();
            PointGeometrys.Add(PointGeometry.Circle);
            PointGeometrys.Add(PointGeometry.Diamond);
            PointGeometrys.Add(PointGeometry.Square);
            PointGeometrys.Add(PointGeometry.Cross);
            PointGeometrys.Add(PointGeometry.X);
            return PointGeometrys;
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
