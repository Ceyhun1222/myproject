using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Process.Loader
{
    public class MockData : INotifyPropertyChanged
    {
        private String _text;

        private int _percentage;

        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if ( PropertyChanged != null )
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }

        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                if ( PropertyChanged != null )
                    PropertyChanged(this, new PropertyChangedEventArgs("Percentage"));
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler  PropertyChanged;

        #endregion
    }
}
