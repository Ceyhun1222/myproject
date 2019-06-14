using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace MapEnv
{
    public abstract class NotifiableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged (string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged (this, new PropertyChangedEventArgs (propName));
        }
    }
}

namespace MapEnv.Toc
{
    public abstract class TocItem : NotifiableObject
    {
        public TocItem ()
        {
            _isExpanded = false;
            _isLoading = false;
        }

        public abstract TocItemType TocType { get; }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;
                OnPropertyChanged ("IsVisible");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                OnPropertyChanged ("Name");
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                    return;

                _isExpanded = value;
                OnPropertyChanged ("IsExpanded");
            }
        }

        public bool IsDragDropLineVisible
        {
            get { return _isDragDropLineVisible; }
            set
            {
                if (_isDragDropLineVisible == value)
                    return;

                _isDragDropLineVisible = value;
                OnPropertyChanged ("IsDragDropLineVisible");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading == value)
                    return;

                _isLoading = value;
                OnPropertyChanged ("IsLoading");
            }
        }

        private bool _isVisible;
        private string _name;
        private bool _isExpanded;
        private bool _isDragDropLineVisible;
        private bool _isLoading;
    }

    public enum TocItemType
    {
        AimSimple, AimComplex, EsriFeature, EsriRaster
    }
}
