using System;
using Aran.Omega.Enums;
using System.ComponentModel;
using Aran.Omega.ViewModels;

namespace Europe_ICAO015
{
    public class CommonParam : ViewModel, IDataErrorInfo
    {

        private double _obstacleDistanceFromAdhp;
        private bool _isObstacleDistanError;

        public CommonParam(double obstacleDistance)
        {
            _isObstacleDistanError = false;
            // ObstacleDistanceFromAdhp = Common.ConvertDistance(obstacleDistance,RoundType.ToDown).ToString();
        }

        public string ObstacleDistanceFromAdhp
        {
            get { return Common.ConvertDistance(_obstacleDistanceFromAdhp, RoundType.ToDown).ToString(); }
            set
            {
                _isObstacleDistanError = false;
                double tmp;
                if (!string.IsNullOrEmpty(value) && Double.TryParse(value, out tmp))
                    _obstacleDistanceFromAdhp = Common.DeConvertDistance(Convert.ToDouble(value));
                else
                    _isObstacleDistanError = true;

                GlobalParams.ObstacleList = GlobalParams.Database.GetVerticalStructureList(GlobalParams.Database.AirportHeliport.ARP.Geo, _obstacleDistanceFromAdhp);
                //GlobalParams.ObstacleList = GlobalParams.Database.GetVerticalStructureList();

                NotifyPropertyChanged("ObstacleDistanceFromAdhp");
            }
        }

        public bool IsCodeLetterF { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "ObstacleDistanceFromAdhp")
                {
                    if (_isObstacleDistanError)
                        return "Please enter distance correctly";
                }
                return "";
            }
        }
    }
}
