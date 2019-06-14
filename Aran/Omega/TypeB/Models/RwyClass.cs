using System;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Omega.TypeB.Enums;
using Aran.Omega.TypeB.ViewModels;
using Aran.Panda.Common;
using System.Collections.ObjectModel;

namespace Aran.Omega.TypeB.Models
{
    public enum CodeLetter
    { 
        A,
        B,
        C,
        D,
        E,
        F
    }

    public class RwyClass:ViewModel
    {
        private Runway _rwy;

        public RwyClass(Runway rwy)
        {
            _rwy = rwy;
            Identifier = rwy.Identifier;
            Height = 0;
            Name = _rwy.Designator;
            
            RwyDirClassList = new ObservableCollection<RwyDirClass>();
            var rwyDirList = GlobalParams.Database.GetRunwayDirections(rwy.Identifier);
            if (rwyDirList.Count == 0)
                throw new Exception(rwy.Designator + " runways has not any runway direction!");
            
            foreach (var rwyDir in rwyDirList)
            {
                try
                {
                    var rwyDirClass= new RwyDirClass(rwyDir,_rwy);
                    RwyDirClassList.Add(rwyDirClass);
                }
                catch (OmegaException e)
                {
                    if (e.ExType != ExceptionType.Critical) continue;
                    
                    GlobalParams.Logs += "Runway " + rwy.Designator + "'s RunwayDirecton" + rwyDir.Designator + " there are not any threshold!";
                    throw new Exception();
                }
                
            }
            SelectedRwyDirection = RwyDirClassList[0];

            CodeNumber = 1;
            if (_rwy.NominalLength != null)
                _length = Math.Round(Aran.Converters.ConverterToSI.Convert(rwy.NominalLength, 0),1);
            else
                _length = Math.Round(SelectedRwyDirection.Length,1);

            if (_length < 800)
                CodeNumber = 1;
            else if (_length < 1200)
                CodeNumber = 2;
            else if (_length < 1800)
                CodeNumber = 3;
            else
                CodeNumber = 4;
        }

        public ObservableCollection<RwyDirClass> RwyDirClassList { get; set; }

        public Guid Identifier { get; set; }
        public string Name { get; set; }
       
        public int CodeNumber { get; private set; }

        private double _length;
        public double Length
        {
            get { return Common.ConvertDistance(_length,RoundType.ToNearest); }
        }
        
        public double Height { get; private set; }

        private bool _checked;
        public bool Checked 
        {
            get{return _checked;}
            set 
            {
                if (_checked == value)
                    return;
                _checked = value;
                if (RwyCheckedIsChanged != null)
                    RwyCheckedIsChanged(this, new EventArgs());
                NotifyPropertyChanged("Checked");
            }
        }

        public void ChangeChecked(bool isChecked)
        {
            _checked = isChecked;
            NotifyPropertyChanged("Checked");
        }

        public RwyDirClass SelectedRwyDirection { get; set; }

        public Runway Runway { get { return _rwy; } }

        public event EventHandler RwyCheckedIsChanged;
    }

    public class RunwayComparer : IComparer<RunwayCentrelinePoint>
    {
        private Aran.Geometries.Point _startPt;

        public RunwayComparer(RunwayCentrelinePoint startCntlnPt)
        {
            _startPt = GlobalParams.SpatialRefOperation.ToPrj(startCntlnPt.Location.Geo);
        }

        public int Compare(RunwayCentrelinePoint pt1, RunwayCentrelinePoint pt2)
        {
            double distance1 = ARANFunctions.ReturnDistanceInMeters(_startPt, GlobalParams.SpatialRefOperation.ToPrj(pt1.Location.Geo));
            double distance2 = ARANFunctions.ReturnDistanceInMeters(_startPt, GlobalParams.SpatialRefOperation.ToPrj(pt2.Location.Geo));
            if (distance1 > distance2)
                return 1;
            else if (distance1 < distance2)
                return -1;
            else
                return 0;
        }
    }
}
