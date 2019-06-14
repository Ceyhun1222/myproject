namespace PVT.Model.Plot
{
    public class Box
    {
        public Box(PlotBase plot)
        {
            Parent = plot;
        }
        public PlotBase Parent { get; }

        public double Bottom { get { return Parent.Height - Parent.Margin.Bottom; } }
        public double Top { get { return  Parent.Margin.Top; } }
        public double Left { get { return Parent.Margin.Left; } }
        public double Right { get { return Parent.Width - Parent.Margin.Right; } }
    }
}
