
using System.Windows.Media;

namespace PVT.Model.Plot
{
    public class PlotGeometry: PlotElement
    {
        public Color Color { get; set; }
        public int Thickness { get; set; }
        public virtual Geometry.Geometry2D Geometry {get; set; }
    }
}
