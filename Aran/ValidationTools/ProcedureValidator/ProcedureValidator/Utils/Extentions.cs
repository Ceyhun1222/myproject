using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace PVT.Utils
{
    public static class ColorExttentionsMethods
    {
        public static void SetColor(this System.Windows.Media.Color current, Color color)
        {
            current.A = color.A;
            current.R = color.R;
            current.G = color.G;
            current.B = color.B;
        }

        public static Color GetColor(this System.Windows.Media.Color current) => Color.FromArgb((current.A << 24) | (current.R << 16) | (current.G << 8) | current.B);

        public static int ToARGB(this System.Windows.Media.Color current) => (current.A << 24) | (current.R << 16) | (current.G << 8) | current.B;

        public static int ToRGB(this System.Windows.Media.Color current) => (current.R << 16) | (current.G << 8) | current.B;

        public static int ToEsriRGB(this System.Windows.Media.Color current) => (current.B << 16) | (current.G << 8) | current.R;


        public static void FromRGB(this System.Windows.Media.Color current, byte r, byte g, byte b)
        {
            current.A = 255;
            current.R = r;
            current.B = b;
            current.A = g;
        }

        public static System.Windows.Media.Color GetColor(this Color current)
        {
            return System.Windows.Media.Color.FromArgb(current.A, current.R, current.G ,current.B);
        }


        public static int ToRGB(this Color current) => (current.R << 16) | (current.G << 8) | current.B;

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Jpeg);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
