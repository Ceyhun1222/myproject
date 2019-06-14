namespace PVT.Model.Plot
{
    public class Axis:PlotElement
    {
        public AxisPostion Position { get; set; }
        public string Name { get; set; }
    }

    public enum AxisPostion
    {
       Horizontal,
       Vertical
    }
}
