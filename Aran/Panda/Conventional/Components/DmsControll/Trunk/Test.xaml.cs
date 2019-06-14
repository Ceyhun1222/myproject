using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DmsControll
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window,INotifyPropertyChanged
    {
        public Test()
        {
            InitializeComponent();
            Accuracy = 3;
            LatitudeProp = 55.3;
            LongtitudeProp = 24.32;
            DataContext = this;
        }

        public double LatitudeProp
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LatitudeProp"));
            }
        }

        public double LongtitudeProp
        {
            get { return _longtitude; }
            set
            {
                _longtitude = value;
                TxtAydin = LongtitudeProp.ToString();
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LongtitudeProp"));
            }
        }

        public int Accuracy
        {
            get { return _accuracy; }
            set
            {
                _accuracy = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Accuracy"));
            }
        }

        public string TxtAydin
        {
            get { return _txtAydin; }
            set
            {
                _txtAydin = value;
               // LatitudeProp =Convert.ToDouble(_txtAydin);
                if (PropertyChanged!=null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TxtAydin"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private double _latitude;
        private double _longtitude;
        private string _txtAydin;
        private int _accuracy;
    }
}   
