using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace PVT.Model
{
    public class Screenshot
    {
        public DateTime Date { get; set; }
        public string uuid { get; set; }
        public List<ScreenImage> Images { get; set; }
    }

    public class ScreenImage
    {
        public Image Image { get; set; }
        public BitmapImage BitmapImage { get; set; }
    }
}
