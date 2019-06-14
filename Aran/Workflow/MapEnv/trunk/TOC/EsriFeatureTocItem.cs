using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MapEnv.Toc
{
    public class EsriFeatureTocItem : TocItem
    {
        public EsriFeatureTocItem () : 
            this (TocItemType.EsriFeature)
        {
        }

        public EsriFeatureTocItem (TocItemType tocType)
        {
            _tocType = tocType;
        }

        public override TocItemType TocType
        {
            get { return _tocType; }
        }

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


        private BitmapImage _symbolImage;
        private TocItemType _tocType;
    }
}
