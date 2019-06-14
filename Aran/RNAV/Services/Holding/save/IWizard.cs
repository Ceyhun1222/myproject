using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;


namespace Holding.HoldingSave
{
    public interface IWizardPage:INotifyPropertyChanged
    {
        Control WizarControl { get; set; }
        
        bool IsComplete { get; set; }

        bool IsValidated { get; set; }

        bool IsFeature { get; }
        
        int PageIndex { get; set; }
        
        void Save();

     }

    public  class Wizard
    {
        #region :>Fields
        #endregion

        #region :>Ctor

        public Wizard(List<IWizardPage> wizardPageList)
        {
            WizardPageList = wizardPageList;

        }

        #endregion

        #region :>Property

        public List<IWizardPage> WizardPageList { get; set; }

        public bool CanMoveNext { get; set; }

        public bool CanMovePrevious { get; set; }

        public bool CanFinish { get; set; }

        public bool CanCancel { get; set; }

        public int CurPageIndex { get; set; }

        public IWizardPage CurPage { get; set; }
        
        #endregion

        #region :>Methods
        public void MoveNext()
        {
            if (CanMoveNext)
            {
                CurPageIndex++;
                CurPage = WizardPageList[CurPageIndex];
            }
        }

        public void MovePrevious()
        {
            if (CanMovePrevious)
            {
                CurPageIndex--;
                CurPage = WizardPageList[CurPageIndex];
            }

        }

        public void Cancel()
        {
            if (CanCancel)
            { 
                
            }

        }

        public void Save()
        {
            if (CanFinish)
            {
                foreach (IWizardPage wizardPage in WizardPageList)
                {
                    wizardPage.Save();
                }
            }
        
        }

        
        #endregion
    
    }
       

}
