using System;

namespace TOSSM.ViewModel.Tool.PropertyPrecision.Single
{
    public class DoublePropertyPrecisionViewModel : SinglePropertyPrecisionViewModel
    {
        public ValPropertyPrecisionViewModel ValPropertyPrecisionViewModel { get; set; }

        private Enum _enum;
        public Enum Enum
        {
            get => _enum;
            set
            {
                _enum = value;
                OnPropertyChanged("Enum");
            }
        }

        private int _precisionFormat;
        private string _enumDescription;

        public int PrecisionFormat
        {
            get => _precisionFormat;
            set
            {
                _precisionFormat = value;
                OnPropertyChanged("PrecisionFormat");
                if (ParentModel != null)
                {
                    ParentModel.FormatChanged(this);
                }
            }
        }

        public string EnumDescription
        {
            get => _enumDescription;
            set
            {
                _enumDescription = value;
                OnPropertyChanged("EnumDescription");
            }
        }
    }
}
