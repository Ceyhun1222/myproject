namespace PVT.Model.Plot
{
    public class Margin
    {
        public Margin()
        {

        }

        public Margin(int margin)
        {
            Bottom = Top = Left = Right = margin;
        }

        public Margin(int vmargin, int hmargin)
        {
            Bottom = Top = vmargin;
            Left = Right = hmargin;
        }

        public int Bottom { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }
}
