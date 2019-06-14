using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Panda.Constants;
using Aran.Panda.Common;
using System.ComponentModel;

namespace Aran.Omega.TypeBEsri
{
    public class RwyClassification:INotifyPropertyChanged
    {
        private string _curClassification;
        public RwyClassification()
        {
            RwyClassifationList = new EnumArray<string, RunwayClassificationType>();
            RwyClassifationList[RunwayClassificationType.NonInstrument] = "Non-instrument";
            RwyClassifationList[RunwayClassificationType.NonPrecisionApproach] = "Non-precision approach";
            RwyClassifationList[RunwayClassificationType.PrecisionApproach] = "Precision approach";
            _curClassification = RwyClassifationList[0];
        }

        public  EnumArray<string, RunwayClassificationType> RwyClassifationList { get; private set; }
        

        public string CurClassification 
        {
            get { return _curClassification; }
            set
            {
                _curClassification = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurClassification"));
            } 
        }




        public event PropertyChangedEventHandler PropertyChanged;
    }
}
