using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DmsControll
{
    public class TestViewModel:INotifyPropertyChanged
    {
        private double _latitude;
        private double _longtitude;
        public event PropertyChangedEventHandler PropertyChanged;

        public TestViewModel()
        {
            LongtitudeProp = 28.5;
            LatitudeProp = 55.4;
        }


        
    }
}
