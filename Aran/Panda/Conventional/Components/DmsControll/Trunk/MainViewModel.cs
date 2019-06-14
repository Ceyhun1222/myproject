using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DmsControll
{
    public class MainViewModel:INotifyPropertyChanged
    {
        private double _latDeg;
        private double _latMin;
        private double _latSec;

        public MainViewModel()
        {
        }

        public static readonly DependencyProperty LatitudeProperty = DependencyProperty.Register("Latitude", typeof(double), typeof(MainViewModel),
          new UIPropertyMetadata(new Double(), new PropertyChangedCallback(Latitude_Changed)));


        public static readonly DependencyProperty LongtitudeProperty = DependencyProperty.Register("Longtitude", typeof(double), typeof(MainViewModel),
             new UIPropertyMetadata(new Double(), new PropertyChangedCallback(Longtitude_Changed)));

        public static readonly DependencyProperty AccuracyProperty = DependencyProperty.Register("Accuracy", typeof(double), typeof(MainViewModel),
          new UIPropertyMetadata(new Double(), new PropertyChangedCallback(Accuracy_Changed)));

       

        private static void Longtitude_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as MainWindow;
            double xDeg, xMin, xSec;
            Functions.DD2DMS((double)e.NewValue, out xDeg, out xMin, out xSec, 1);
            if (sender != null)
            {
                sender.LongDeg = xDeg;
                sender.LongMin = xMin;
                sender.LongSec = xSec;
            }
        }

        private static void Latitude_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as MainWindow;
            double xDeg, xMin, xSec;
            Functions.DD2DMS((double)e.NewValue, out xDeg, out xMin, out xSec, 1);
            if (sender != null)
            {
                sender.LatDeg = xDeg;
                sender.LatMin = xMin;
                sender.LatSec = xSec;
            }
        }

        private static void Accuracy_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var t = "";
        }

        public List<string> LatDirList { get; set; }
        public List<string> LongDirList { get; set; }

        public double LatDeg
        {
            get { return _latDeg; }
            set
            {
                _latDeg = value;
                NotifyEvent("LatDeg");
            }
        }

        public double LatMin
        {
            get { return _latMin; }
            set
            {
                _latMin = value;
                NotifyEvent("LatMin");
            }
        }

        public double LatSec
        {
            get { return _latSec; }
            set
            {
                _latSec = value;
                NotifyEvent("LatSec");
            }
        }

        public double LongDeg
        {
            get { return _longDeg; }
            set
            {
                _longDeg = value;
                NotifyEvent("LongDeg");
            }
        }

        public double LongMin
        {
            get { return _longMin; }
            set
            {
                _longMin = value;
                NotifyEvent("LongMin");
            }
        }

        public double LongSec
        {
            get { return _longSec; }
            set
            {
                _longSec = value;
                NotifyEvent("LongSec");
            }
        }

        public double Latitude
        {
            get { return (double)GetValue(LatitudeProperty); }
            set { SetValue(LatitudeProperty, value); }
        }

        public double Longtitude
        {
            get { return (double)GetValue(LongtitudeProperty); }
            set { SetValue(LongtitudeProperty, value); }
        }

        public int Accuracy
        {
            get { return (int)GetValue(AccuracyProperty); }
            set { SetValue(AccuracyProperty, value); }
        }

        private void NotifyEvent(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private double _longDeg;
        private double _longMin;
        private double _longSec;
    }
}
