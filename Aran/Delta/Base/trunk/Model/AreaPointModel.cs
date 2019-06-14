using Aran.PANDA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Delta.Model
{
    public class AreaPointModel:ViewModels.ViewModel
    {
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value; 
                NotifyPropertyChanged("Index");
            }
        }

        private double _latitude;

        public Aran.Geometries.Point PrjPoint { get; set; }

        public double Latitude
        {
            get { return Math.Round(_latitude,Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision)); }
            set 
            {
                _latitude = value;
                if (!double.IsNaN(_latitude))
                {
                 //   LatStr = ARANFunctions.Degree2String(_latitude, Degree2StringMode.DMSLat,
                   //     Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));
                    Aran.PANDA.Common.ARANFunctions.Dd2DmsStr(_longtitude, _latitude, ".", "E", "N",1, Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision), out _longStr, out _latStr);
                }
                NotifyPropertyChanged("");
            }
        }

        private double _longtitude;
        public double Longtitude
        {
            get { return  Math.Round(_longtitude,Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision)); }
            set 
            {
                _longtitude = value;
                if (!double.IsNaN(_longtitude))
                {
                 //   LongStr = ARANFunctions.Degree2String(_longtitude, Degree2StringMode.DMSLon,
                   //     Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision));


                    Aran.PANDA.Common.ARANFunctions.Dd2DmsStr(_longtitude, _latitude, ".", "E", "N", 1,
                        Convert.ToInt32(GlobalParams.Settings.DeltaInterface.CoordinatePrecision), out _longStr,
                        out _latStr);
                }
                NotifyPropertyChanged("");
            }
        }

        private int _index;

        private string _longStr;
        public string LongStr
        {
            get { return _longStr; }
            set 
            {
                _longStr = value;
                NotifyPropertyChanged("LongStr");
            }
        }

        private string _latStr;
        public string LatStr
        {
            get { return _latStr; }
            set 
            {
                _latStr = value;
                NotifyPropertyChanged("LatStr");
            }
        }

        public double GetLatitude()
        {
            return _latitude;
        }

        public double GetLongtitude()
        {
            return _longtitude;
        }

        public string Type { get; set; }
        
        public double Resolution { get; set; }

        private double _accuracy;
        public double Accuracy
        {
            get => _accuracy;
            set
            {
                var lg10 = Math.Log10(value);
                var lg10floor = Math.Floor(lg10);
                if (lg10 != lg10floor)
                    lg10floor++;
                _accuracy = Math.Pow(10, lg10floor);
            }
        }
    }
}
