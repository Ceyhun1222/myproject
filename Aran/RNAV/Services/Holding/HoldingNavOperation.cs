using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Holding
{
    public class HoldingNavOperation : INotifyPropertyChanged
    {
        #region :>Constructor
        
        public HoldingNavOperation()
        {
            HoldingNavList = new BindingList<HoldingNavaid>();
            DmeCovType = DmeCoverageType.twoDme;
            HoldingNavList = new BindingList<HoldingNavaid>();
            HoldingNavList.AllowEdit = false;
            HoldingNavList.AllowNew = false;
            HoldingNavList.AllowRemove = false;
        }
        #endregion

        #region :>Property

        public int CheckedNavCount { get; set; }
        public int CurCheckedNavIndex { get; set; }
        public BindingList<HoldingNavaid> HoldingNavList { get; set; }
        public DmeCoverageType DmeCovType { get; set; }
        public HoldingNavaid CurrentNav { get { return HoldingNavList[CurCheckedNavIndex]; } }
        
        private bool _drawDME;
        public bool DrawDME
        {
            get{return _drawDME;}
            set
            {
                if (value == _drawDME)
                    return;
                _drawDME = value;
             
                if (DmeCoverageChanged != null)
                    DmeCoverageChanged(this, new DMECoverageDrawChangedEventArgs(_drawDME));
                
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DrawDME"));

            }
        }
       
        public bool TwoDmeCovTypeIsChecked
        {
            get { return Equals(DmeCovType, DmeCoverageType.twoDme); }
            set
            {
                if (value)
                {
                                        
                    DmeCovType = DmeCoverageType.twoDme;

                    if (DmeCoveTypeCheckChanged != null)
                        DmeCoveTypeCheckChanged(this, new EventArgs());

                    if (DmeCoverageChanged != null)
                        DmeCoverageChanged(this, new DMECoverageDrawChangedEventArgs(DrawDME));
  
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DmeCovType"));
                }

            }
        }

        public bool ThreeDmeCovTypeIsChecked
        {
            get { return Equals(DmeCovType, DmeCoverageType.threeDme); }
            set
            {
                if (value)
                {
                    if (CheckedNavCount > 2)
                        DmeCovType = DmeCoverageType.threeDme;

                    if (DmeCoveTypeCheckChanged != null)
                        DmeCoveTypeCheckChanged(this, new EventArgs());

                    if (DmeCoverageChanged != null)
                        DmeCoverageChanged(this, new DMECoverageDrawChangedEventArgs(DrawDME));
                   
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("DmeCovType"));
                }
            }
        }

        private bool _dmeConverageIsEnabled;
        public bool DmeCoverageIsEnabled 
        {
            get{return _dmeConverageIsEnabled;}
            set
            {
                if (_dmeConverageIsEnabled == value)
                    return;
                _dmeConverageIsEnabled = value;
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("DmeCoverageIsEnabled"));
            }
        }

        public bool  DmeCoverageChooseIsEnabled { get; set; }

        public List<HoldingNavaid> CheckedNavaidList
        {
            get 
            {
                return HoldingNavList.Where(hNavaid => hNavaid.Checked).Select(s => s).ToList<HoldingNavaid>();
            }
        }

        public void Dispose() 
        {
            foreach (var holdingNavaid in HoldingNavList)
            {
                holdingNavaid.Dispose();
            }
            HoldingNavList.Clear();
        }
        
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler DmeCoveTypeCheckChanged;
        public event DMECoverageDrawChangedEventHandler DmeCoverageChanged;


        #endregion
    }
}