using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Holding
{
    #region :>Enums
    public enum DistanceType
    {
        none,
        Time,
        Wd
    }

    public enum ProcedureType
    {
        withHoldingFunc,
        withoutHoldingFunc,
        RNP,
        None
    }
    #endregion

    public class ModelProcedureType :Changed,INotifyPropertyChanged
    {
        #region :>Fields
        private double _WD,_time, _minWD,_applyWD,_applyTime;
        private ProcedureType  _applyPropType;
        private protectionSectorType _protectionType, _applyProtectionType;
        private bool _wdCanActive;
        private DistanceType _applyDistanceType;
        #endregion


        public ModelProcedureType(double minTime,double maxTime,double currentTime)
        {
            MinTime = minTime;
            MaxTime = maxTime;
            Time = currentTime;

            DistanceTypeList = new List<DistanceType> { DistanceType.Time, DistanceType.Wd };
            HoldFuncIsActive = true;
            CurDistanceType = DistanceType.Time;
            PropType = ProcedureType.withHoldingFunc;
            _applyPropType = ProcedureType.None;
        }

        public List<DistanceType> DistanceTypeList { get; set; }

        private DistanceType _curDistanceType;
        public DistanceType CurDistanceType
        {
            get { return _curDistanceType; }
            set
            {
                if (Equals(_curDistanceType, value))
                    return;

                ChangeModelChanged(_curDistanceType, value, _applyDistanceType);
                if (value == DistanceType.Wd)
                {
                    if (WDCanActive)
                    {
                        _curDistanceType = DistanceType.Wd;
                        SectorOneIsEnabled = true;
                        SectorTwoIsEnabled = true;
                        OmnidirectionalIsEnabled = false;
                    }
                    else
                    {
                        SectorOneIsEnabled = false;
                        SectorTwoIsEnabled = false;
                    }
                }
                else
                {
                    if (value == DistanceType.Time)
                    {
                        _curDistanceType = value;

                        SectorOneIsEnabled = false;
                        SectorTwoIsEnabled = false;
                        OmnidirectionalIsEnabled = true;
                    }   
                }         
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurSelected"));
            }
        }

        public double MinTime { get; set; }

        public double MaxTime { get; set; }

        public double Time 
        {
            get{return _time;}
            set
            {
                if (_time==value)
                    return;
                ChangeModelChanged(_time,value,_applyTime);
                _time = value;
            }
        }
        
        public double WD
        {
            get { return Common.ConvertDistance(_WD,roundType.toNearest); }
            set
            {
                double tmp = Common.DeConvertDistance(value);
                if (tmp == _WD)
                    return;
                ChangeModelChanged(_WD, tmp, _applyWD);
                _WD = tmp;
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("WD"));
            }
        }

        public double MinWD
        {
            get{return Common.ConvertDistance(_minWD,roundType.toUp);} 
            set
            {
                _minWD = Common.DeConvertDistance(value);
                if (_minWD>_WD)
                {
                    _WD =  _minWD;
                }
            }
        }

        public bool WDCanActive
        {
            get{return _wdCanActive;}
            set
            {
                _wdCanActive = value;
                if (PropertyChanged!=null)
                    PropertyChanged(this,new PropertyChangedEventArgs("WDCanActive"));
            }
        }

        public ProcedureType PropType{ get; set; }
        
        public bool HoldFuncIsActive
        {
            get { return (Equals(PropType, ProcedureType.withHoldingFunc)); }
            set
            {
                if (value)
                {
                    if (DistanceTypeList.Count > 1)
                        DistanceTypeList.Remove(DistanceType.Wd);

                    ChangeModelChanged(PropType, ProcedureType.withHoldingFunc, _applyPropType);
                    PropType = ProcedureType.withHoldingFunc;
                    OmnidirectionalIsEnabled = true;
                    SectorOneIsEnabled = false;
                    SectorTwoIsEnabled = false;
                    SectorFourIsEnabled = false;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("PropType"));    
                }
                                
            }
        }
                
        public bool WithoutHoldFuncIsActive
        {
            get { return Equals(PropType, ProcedureType.withoutHoldingFunc); }
            set
            {
                if (value)
                {

                    if (DistanceTypeList.Count < 2)
                        DistanceTypeList.Add(DistanceType.Wd);
                    ChangeModelChanged(PropType, ProcedureType.withoutHoldingFunc, _applyPropType);
                    PropType = ProcedureType.withoutHoldingFunc;

                    if (CurDistanceType == DistanceType.Time)
                    {
                        OmnidirectionalIsEnabled = true;
                        SectorFourIsEnabled = false;
                        SectorTwoIsEnabled = false;
                        SectorFourIsEnabled = false;
                    }
                    else
                    {
                        SectorOneIsEnabled = true;
                        SectorTwoIsEnabled = true;
                        OmnidirectionalIsEnabled = false;
                        SectorFourIsEnabled = false;
                    }
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("PropType"));                    
                }
                              
                
            }
        }

        public bool RNPIsActive
        {
            get { return Equals(PropType, ProcedureType.RNP); }
            set
            {
                if (value)
                {
                    if (DistanceTypeList.Count > 1)
                        DistanceTypeList.Remove(DistanceType.Wd);
                    ChangeModelChanged(PropType, ProcedureType.RNP, _applyPropType);
                    PropType = ProcedureType.RNP;
                    SectorFourIsEnabled =true;
                    OmnidirectionalIsEnabled = false;
                    SectorOneIsEnabled = false;
                    SectorTwoIsEnabled = false;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("PropType"));
                }
                
               
            }
        }

        public bool TimeEnabled
        {
            get { return Equals(DistanceType.Time, _curDistanceType); }
        }

        public bool WDEnabled
        {
            get { return Equals(DistanceType.Wd, _curDistanceType); }
            set
            {
                if (!value)
                {
                    _curDistanceType = DistanceType.none;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("WDEnabled"));
                }
            }
        }

        //public bool HoldingAreaIsEnabled
        //{
        //    get { return (!IsApply && _holdingAreaIsEnabled) || (ChangedCount != 0 && _holdingAreaIsEnabled); }
        //    set
        //    {
        //        if (_holdingAreaIsEnabled == value)
        //            return;
        //        _holdingAreaIsEnabled = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs("HoldingAreaIsEnabled"));
        //    }
        //}

        //public int ChangedCount
        //{
        //    get { return _changedCount; }
        //    set
        //    {
        //        _changedCount = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs("HoldingAreaIsEnabled"));
        //    }
        //}       
        
        public protectionSectorType ProtectionType
        {
            get { return _protectionType ;}
            set
            {
                if (_protectionType == value)
                    return;
                ChangeModelChanged(_protectionType, value, _applyProtectionType);
                _protectionType = value;
            }
        }

        public bool OmnidirectionalIsEnabled { get; private set; }
        public bool SectorOneIsEnabled { get; private set; }
        public bool SectorTwoIsEnabled { get; private set; }
        public bool SectorFourIsEnabled { get; private set; }

        public bool OmnidirectionalIsChecked
        {
            get { return Equals(protectionSectorType.omnidirectional,ProtectionType);}
            set
            {
                if (value)
                {
                    ProtectionType = protectionSectorType.omnidirectional;
                    
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("OmnidirectionalIsChecked"));
                }
                else
                    ProtectionType = protectionSectorType.none;
             }
        }

        public bool SectorOneIsChecked
        {
            get { return Equals(protectionSectorType.oneSectorPermitted, ProtectionType); }
            set
            {
                if (value)
                {
                    ProtectionType = protectionSectorType.oneSectorPermitted;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SectorOneIsChecked"));                    
                }
                else
                    ProtectionType = protectionSectorType.none;
                                
            }
            
        }

        public bool SectorTwoIsChecked
        {
            get { return Equals(protectionSectorType.twoSectorPermitted, ProtectionType); }
            set
            {
                if (value)
                {
                    ProtectionType = protectionSectorType.twoSectorPermitted;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SectorOneIsChecked"));

                }
                else
                    ProtectionType = protectionSectorType.none;
                
            }

        }

        public bool SectorFourIsChecked
        {
            get { return Equals(protectionSectorType.fourSectorPermitted, ProtectionType); }
            set
            {
                if (value)
                {
                    ProtectionType = protectionSectorType.fourSectorPermitted;
                    OmnidirectionalIsEnabled = false;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SectorOneIsChecked"));

                }
                else
                    ProtectionType = protectionSectorType.none;
               
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override void SetApplyParams()
        {
            _applyTime = _time;
            _applyWD = _WD;
            _applyPropType = PropType;
            _applyProtectionType = _protectionType;
            _applyDistanceType = _curDistanceType;
        }
    }
}
