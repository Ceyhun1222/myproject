using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Delib.Classes.Features.AirportHeliport;

namespace Holding.HoldingSave
{
    public class DesignatedPointWizardPage : IWizardPage
    {
        public DesignatedPointWizardPage(Control control,AirportHeliport adhp,ARAN.GeometryClasses.Point pt)
        {
            WizarControl = control;
            _dPoint = new Delib.Classes.Features.Navaid.DesignatedPoint();
            _ahdp = adhp;
            AdhpName = _ahdp.designator;
            Latitude = pt.X;
            LongTitude = pt.Y;
            IsValidated = false;

        }

        public System.Windows.Forms.Control WizarControl { get; set; }

        public bool IsComplete { get; set; }
    
        public bool IsValidated { get; set; }
      
        public int PageIndex { get; set; }

        public bool IsFeature
        {
            get { return true; }
        }
      
        public void Save()
        {
            if (IsComplete)
            {
                GlobalParams.Database.HoldingQpi.Store(_dPoint);
            }
        }        

        #region :>Property
        public string Designator
        {
            get { return _designator; }
            set
            {
                if (value.Length != 5)
                {
                    IsComplete = false;
                    MessageBox.Show("You must write only five words");
                    throw new Exception("length of desinator is long than 5");

                }
                _designator = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Designator"));
            }
        }

        public List<Delib.Classes.Codes.DesignatedPointType> TypeList { get; private set; }

        public Delib.Classes.Codes.DesignatedPointType Type
        {
            get { return _curType; }
            set 
            {
                _curType = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Name"));
            }
        }

        public double Latitude { get; private set; }

        public double  LongTitude { get; private set; }     

        public string AdhpName { get; private set; }

        public Delib.Classes.Features.Navaid.DesignatedPoint DesignatedPoint { get; set; }

        #endregion
        
        private Delib.Classes.Features.Navaid.DesignatedPoint _dPoint;
        private Delib.Classes.Codes.DesignatedPointType _curType;
        private Delib.Classes.Features.AirportHeliport.AirportHeliport _ahdp;
        private string _name,_designator;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;





       
    }
}
