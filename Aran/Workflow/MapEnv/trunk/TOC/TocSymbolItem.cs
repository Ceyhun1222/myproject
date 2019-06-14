using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MapEnv.Toc
{
    public class TocSymbolItem : NotifiableObject
    {
        public BitmapImage SymbolImage
        {
            get { return _symbolImage; }
            set
            {
                if (_symbolImage == value)
                    return;

                _symbolImage = value;
                OnPropertyChanged ("SymbolImage");
            }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                if (_propertyName == value)
                    return;
                _propertyName = value;
                OnPropertyChanged ("PropertyName");
            }
        }

        private BitmapImage _symbolImage;
        private string _propertyName;

    }
}
