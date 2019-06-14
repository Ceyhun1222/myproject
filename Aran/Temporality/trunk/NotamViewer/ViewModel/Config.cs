using System.Windows.Media;

namespace NotamViewer.ViewModel
{
    public class Config
    {
        public static Brush AiracBrush = Brushes.LightGray;
        public static Brush PermBrush = Brushes.Blue;
        public static Brush TempBrush = Brushes.Red;

        public static Brush TemporaryChangeBrush = Brushes.MistyRose;
        public static Brush PermanentChangeBrush = Brushes.Lavender;
        
        public static double DayInPixel = 3;

        public static double DefaultRowHeight = 42;
        public static double DefaultStrokeThickness=1;
    }
}