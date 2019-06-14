using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Holding.Models
{
  public  class ModelWizardChange : INotifyPropertyChanged
  {
        #region :> Fields
        private List<Control> _container;
        private Aran.Geometries.Point _curPoint;
        private int _curWizard = 0;
      #endregion

        public ModelWizardChange(List<Control> container)
        {
            CountWizard = container.Count;
            _container = container;
         }

        public int CountWizard { get; private set; }
            
        public int CurrentWizard
        {
            get { return _curWizard; }
            set
            {
                _curWizard = value;
                PointChoice.WizardNumber = _curWizard;
                OnPropertyChanged("CurrentWizard",_curPoint);
            }
        }

        public ModelPointChoise PointChoice { get; set; }

        public Control CurContainer
        {
            get { return _container[_curWizard]; }
        }

        public bool NextAvailable { get; set; }

        public bool PreviusAvailable { get;set; }

        public bool CurrentPointChange(Aran.Geometries.Point newPoint)
        {
            if ((_curPoint == null && newPoint != null) || (_curPoint != null && newPoint == null))
            {
                _curPoint = newPoint;
                OnPropertyChanged("CurPoint", newPoint);
                return true;
            }
            return false;
        }
              
        public void OnPropertyChanged(string propName,Aran.Geometries.Point curPoint)
        {
                if (CurrentWizard > 0 && CurrentWizard < CountWizard - 1)
                {
                    PreviusAvailable = true;
                    NextAvailable = true;
                }
                else if (CurrentWizard == 0 && curPoint != null)
                {
                    PreviusAvailable = false;
                    NextAvailable = true;
                }
                else if (CurrentWizard == 0 && curPoint == null)
                {
                    PreviusAvailable = false;
                    NextAvailable = false;
                }
                else if (CurrentWizard == CountWizard - 1)
                {
                    PreviusAvailable = true;
                    NextAvailable = false;
                }

                _curPoint = curPoint;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void SetCurPoint(Aran.Geometries.Point curPt)
        {
            OnPropertyChanged("CurPoint",curPt);
        }
      
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
