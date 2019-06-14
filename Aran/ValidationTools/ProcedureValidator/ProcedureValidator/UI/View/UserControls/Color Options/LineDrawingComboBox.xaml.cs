using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PVT.UI.View.UserControls
{
    /// <summary>
    /// Interaction logic for LineDrawingComboBox.xaml
    /// </summary>
    public partial class LineDrawingComboBox
    {



        static LineDrawingComboBox()
        {

        }


        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(string), typeof(LineDrawingComboBox), new UIPropertyMetadata("Advanced"));
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

        public static readonly DependencyProperty LineGeometriesProperty = DependencyProperty.Register("LineGeometries", typeof(ObservableCollection<Model.Drawing.LineGeometry>), typeof(LineDrawingComboBox), new UIPropertyMetadata(CreateLineGeometries()));
        public ObservableCollection<Model.Drawing.LineGeometry> LineGeometries
        {
            get
            {
                return (ObservableCollection<Model.Drawing.LineGeometry>)GetValue(LineGeometriesProperty);
            }
            set
            {
                SetValue(LineGeometriesProperty, value);
            }
        }

        public static ObservableCollection<Model.Drawing.LineGeometry>  CreateLineGeometries()
        {
            var  LineStyles = new ObservableCollection<Model.Drawing.LineGeometry>();
            LineStyles.Add(Model.Drawing.LineGeometry.Solid);
            LineStyles.Add(Model.Drawing.LineGeometry.Dash);
            LineStyles.Add(Model.Drawing.LineGeometry.DashDot);
            LineStyles.Add(Model.Drawing.LineGeometry.DashDotDot);
            //LineStyles.Add(DashStyles.Dot);
            return LineStyles;
        }

        public LineDrawingComboBox()
        {
            InitializeComponent();
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
