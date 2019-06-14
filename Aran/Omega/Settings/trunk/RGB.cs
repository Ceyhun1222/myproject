namespace Aran.Omega.SettingsUI
{
    public class RGB
    {
        public RGB(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static RGB ToRGB(int color)
        {
            int Blue = color & 255;
            int Green = (color >> 8) & 255;
            int Red = (color >> 16) & 255;
            return new RGB(Blue, Green, Red);
        }

        public static System.Windows.Media.Color ToWindowsColor(int color)
        {
            RGB rgb = RGB.ToRGB(color);
            return System.Windows.Media.Color.FromRgb((byte)rgb.R, (byte)rgb.G, (byte)rgb.B);
        }

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}