using GalaSoft.MvvmLight;
using PVT.Utils;

namespace PVT.Model.Drawing
{
    public class Color: ObservableObject
    {
        public Color(System.Windows.Media.Color color)
        {
            _color = color;
        }

        private System.Windows.Media.Color _color;
        public System.Windows.Media.Color SystemColor
        {
            get
            {
                return _color;
            }
            set
            {
                Set(() => SystemColor, ref _color, value);
            }
        }

        public int ToRGB()
        {
            return _color.ToEsriRGB();
        }
    }
}
