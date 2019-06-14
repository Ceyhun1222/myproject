using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Aran.Controls
{
    public class TabDocument : INotifyPropertyChanged
    {
        private Control _workArea;
        private string _text;

        public event PropertyChangedEventHandler PropertyChanged;
        internal event EventHandler PageClosed;

        public TabDocument ()
        {
            VisibleContextStrip = true;
            InForm = false;
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged ("Text");
                }
            }
        }

        public Control WorkArea
        {
            get { return _workArea; }
            set
            {
                if (_workArea != value)
                {
                    _workArea = value;
                    OnPropertyChanged ("WorkArea");
                }
            }
        }

        public bool VisibleContextStrip { get; set; }

        public void ClosePage ()
        {
            if (PageClosed != null)
                PageClosed (this, null);
        }

        public bool InForm { get; set; }


        private void OnPropertyChanged (string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}
