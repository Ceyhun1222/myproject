using System.Drawing;

namespace AIP.BaseLib.Struct
{
    /// <summary>
    /// Struct for output messages into Main form
    /// </summary>
    public struct Output
    {
        public string Message { get; set; }
        public int? Percent { get; set; }
        public Color Color { get; set; }
    }
}
