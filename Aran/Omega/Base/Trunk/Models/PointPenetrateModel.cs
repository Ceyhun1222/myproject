using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Aran.Omega.Models
{
    public class PointPenetrateModel
    {
        private SolidColorBrush _mySolidColorBrush;
        public PointPenetrateModel()
        {
            _mySolidColorBrush = new SolidColorBrush();
            _mySolidColorBrush.Color = Color.FromRgb(255, 0, 0);
        }

        public double Elevation { get; set; }
        public double Penetration { get; set; }
        public string Surface { get; set; }
        public string Plane { get; set; }

        public Brush CellColor
        {
            get
            {
                if (Penetration > 0)
                    return _mySolidColorBrush;
                return Brushes.Black;
            }

        }
    }
}
