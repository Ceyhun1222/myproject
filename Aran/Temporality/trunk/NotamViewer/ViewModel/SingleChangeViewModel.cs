using System;
using System.Collections.Generic;
using System.Windows.Media;
using Aran.Aim.Features;
using MvvmCore;

namespace NotamViewer.ViewModel
{
    public class ChangeViewModel : ViewModelBase
    {
        public string ChangedProperty { get; set; }
        public string OldStringValue { get; set; }
        public string NewStringValue { get; set; }
    }

    public class SingleChangeViewModel:ViewModelBase
    {
        public Feature Change { get; set; }

        public SingleChangeViewModel(SingleFeatureHistoryViewModel parent)
        {
            ParentModel = parent;
        }

        public SingleFeatureHistoryViewModel ParentModel { get; set; }


        private List<ChangeViewModel> _changeList;
        public List<ChangeViewModel> ChangeList
        {
            get { return _changeList; }
            set
            {
                _changeList = value;
                OnPropertyChanged("ChangeList");
            }
        }

        private bool _popupIsOpen;
        public bool PopupIsOpen
        {
            get { return _popupIsOpen; }
            set
            {
                _popupIsOpen = value;
                OnPropertyChanged("PopupIsOpen");
                if (PopupIsOpen)
                {
                    ParentModel.SelectedChild = this;
                }
            }
        }

        private string _controlToolTip;
        public string ControlToolTip
        {
            get { return _controlToolTip; }
            set
            {
                _controlToolTip = value;
                OnPropertyChanged("ControlToolTip");
            }
        }

        private int _rowPosition;
        public int RowPosition
        {
            get { return _rowPosition; }
            set
            {
                _rowPosition = value;
                OnPropertyChanged("RowPosition");
            }
        }

        private double _controlMargin;
        public double ControlMargin
        {
            get { return _controlMargin; }
            set
            {
                _controlMargin = value;
                OnPropertyChanged("ControlMargin");
            }
        }

        private void RecalculateSize()
        {
            ControlWidth = Config.DayInPixel * (EndDate - StartDate).TotalDays;
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
                RecalculateSize();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
                RecalculateSize();
            }
        }

        private Brush _controlBackground;
        public Brush ControlBackground
        {
            get { return _controlBackground; }
            set
            {
                _controlBackground = value;
                OnPropertyChanged("ControlBackground");
            }
        }

        private double _controlWidth;
        public double ControlWidth
        {
            get { return _controlWidth; }
            set
            {
                _controlWidth = value;
                OnPropertyChanged("ControlWidth");
            }
        }
    }
}
