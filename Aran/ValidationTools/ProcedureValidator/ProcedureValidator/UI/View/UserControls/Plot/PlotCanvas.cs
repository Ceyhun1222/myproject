using PVT.Model.Plot;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PVT.UI.View.UserControls
{
    public class PlotCanvasBase<T> : Canvas where T : Model.Geometry.Geometry
    {
        public PlotCanvasBase()
        {
            SizeChanged += PlotCanvas_SizeChanged;
        }

        public static readonly DependencyProperty PlotModelProperty =
            DependencyProperty.Register("PlotModel", typeof(PlotModel<T>), typeof(PlotCanvasBase<T>), new UIPropertyMetadata(null, new PropertyChangedCallback(PlotModelChanged)));

        public PlotModel<T> PlotModel
        {
            get { return (PlotModel<T>)GetValue(PlotModelProperty); }
            set { SetValue(PlotModelProperty, value); }
        }

        private void OnPlotModelChanged()
        {
            draw();
        }

        private void PlotCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PlotModel != null)
                PlotModel.PlotSizeChanged(e);
        }

        private static void PlotModelChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            var canv = depObj as PlotCanvasBase<T>;
            if (canv != null)
            {
                if (args.OldValue != null)
                {
                    (args.OldValue as PlotModel<T>).PlotModelChanged -= canv.OnPlotModelChanged;
                }
                canv.PlotModel.SetSize(canv.ActualWidth, canv.ActualHeight);
                canv.PlotModel.PlotModelChanged += canv.OnPlotModelChanged;
                canv.draw();
            }
        }

        private void draw()
        {
            Children.Clear();
            for (var i = 0; i < PlotModel.DrawGeometries.Count; i++)
            {
                var path = new Path();
                path.Data = PlotModel.DrawGeometries[i].Geometry.Convert();
                path.StrokeThickness = PlotModel.DrawGeometries[i].Thickness;
                path.Stroke = new SolidColorBrush(PlotModel.DrawGeometries[i].Color);
                Children.Add(path);
            }
        }
    }
}
