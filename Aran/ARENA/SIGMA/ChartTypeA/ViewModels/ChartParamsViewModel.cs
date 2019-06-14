using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChartTypeA.Models;
using ESRI.ArcGIS.Geometry;
using PDM;

namespace ChartTypeA.ViewModels
{
    public class ChartParamsViewModel:ViewModel
    {
        private string _paperSize;
        private string _selectedTemplate;
        private double _mapChartWidthInM,_mapChartHeightInM;
        private double _horScale;
        private double _verScale;
        private double _mapChartWidth;
        private double _horGridInterval;
        private double _mapChartHeight;
        private double _verGridInterval;
        private readonly List<RunwayCenterLinePoint> _centerLinePointsList;
        private double _centerLineSlope;
        private ProfileCenterLine _selecteProfileCenterLine;

        public ChartParamsViewModel(List<RunwayCenterLinePoint> sortedCenterLinePoints ,double mapChartWidth,double mapChartHeight)
        {
          
            Header = "ICAO Chart Type A (Chart Parametrs )";
            HorGridInterval = InitChartTypeA.DistanceConverter.Unit == "M" ? 300 : 1000;

            VerGridInterval = InitChartTypeA.HeightConverter.Unit == "M" ? 30 : 100;

            _mapChartWidthInM = mapChartWidth;
            _mapChartHeightInM = mapChartHeight;

            HorScale = 20000;
            _centerLinePointsList = sortedCenterLinePoints;

            ProfileCenterLines = new ObservableCollection<ProfileCenterLine>();

            CenterLineSlope = 0.5;
        
            
            //TemplateList = new List<string>();

            //TemplateList = Directory.GetFiles(@"Templates", "*.mxd").Select(Path.GetFileName).ToList();
            //if (TemplateList.Count > 0)
            //    SelectedTemplate = TemplateList[0];
        }

        public ObservableCollection<ProfileCenterLine> ProfileCenterLines { get; set; }

        public double HorScale
        {
            get { return _horScale; }
            set
            {
                _horScale = value;

                VerScale = _horScale/10;
                double paperWidth =_mapChartWidthInM*100/_horScale;
                if (paperWidth < 23)
                    PaperSize = "A4 : ";
                else if (paperWidth < 33)
                    PaperSize = "A3 : ";
                else if (paperWidth < 49)
                    PaperSize = "A2 : ";
                else if (paperWidth < 69)
                    PaperSize = "A1 : ";
                else
                    PaperSize = "A0 : ";
                PaperSize +=Math.Ceiling(paperWidth) + " sm";


                MapChartWidth = _mapChartWidthInM *100/_horScale;
                MapChartHeight = _mapChartHeightInM*100/_horScale;

                NotifyPropertyChanged("HorScale");
            }
        }

        public double VerScale
        {
            get { return _verScale; }
            set
            {
                _verScale = value;
                NotifyPropertyChanged("VerScale");
            }
        }

        public double HorGridInterval
        {
            get { return _horGridInterval; }
            set
            {
                _horGridInterval = value;
                VerGridInterval =Common.ConvertHeight(Common.DeConvertDistance(_horGridInterval)/10,roundType.toNearest);
                NotifyPropertyChanged("HorGridInterval");
            }
        }

        public double VerGridInterval
        {
            get => _verGridInterval;
            set
            {
                _verGridInterval = value;
                NotifyPropertyChanged("VerGridInterval");
            }
        }

        public double MapChartWidth
        {
            get { return Math.Ceiling(_mapChartWidth); }
            set
            {
                _mapChartWidth =value;
                NotifyPropertyChanged("MapChartWidth");
            }
        }

        public double MapChartHeight
        {
            get { return Math.Ceiling(_mapChartHeight); }
            set
            {
                _mapChartHeight =value;
                NotifyPropertyChanged("MapChartHeight");
            }
        }

        public string PaperSize
        {
            get { return _paperSize; }
            set
            {
                _paperSize = value;
                NotifyPropertyChanged("PaperSize");
            }
        }

        public string Unit { get; } = InitChartTypeA.HeightConverter.Unit;

        public double CenterLineSlope
        {
            get { return _centerLineSlope; }
            set
            {
                _centerLineSlope = value;
                Calculate();
            }
        }

        public ProfileCenterLine SelecteProfileCenterLine
        {
            get { return _selecteProfileCenterLine; }
            set
            {
                if (_selecteProfileCenterLine != null)
                    _selecteProfileCenterLine.Clear();

                _selecteProfileCenterLine = value;
                if (_selecteProfileCenterLine != null)
                    _selecteProfileCenterLine.Draw();

                NotifyPropertyChanged("SelecteProfileCenterLine");
            }
        }

        public override void Clear()
        {
            
        }

        private void Calculate()
        {
            ProfileCenterLines.Clear();

            var slope = _centerLineSlope/100;
            var startCnt = _centerLinePointsList[0];
            int n = 0;
            for (int i = 1; i < _centerLinePointsList.Count; i++)
            {
                double? startElev = startCnt.ConvertValueToMeter(startCnt.Elev, startCnt.Elev_UOM.ToString());
                var endCnt = _centerLinePointsList[i];
                var endElev = endCnt.ConvertValueToMeter(endCnt.Elev, endCnt.Elev_UOM.ToString());

                if (startElev.HasValue && endElev.HasValue)
                {
                    var length = EsriFunctions.ReturnGeodesicDistance((IPoint)startCnt.Geo, (IPoint)endCnt.Geo);
                        
                    var percent = Math.Abs((endElev.Value - startElev.Value) / length);
                    if (percent > slope)
                    {
                        n++;
                        var profileCenterLine = new ProfileCenterLine();
                        profileCenterLine.Index = n;
                        profileCenterLine.Start = startCnt;
                        profileCenterLine.End = endCnt;
                        profileCenterLine.Percent =Math.Round(100 *  percent,4);

                        profileCenterLine.Length = EsriFunctions.ReturnGeodesicDistance((IPoint)startCnt.Geo,(IPoint)endCnt.Geo);
                        ProfileCenterLines.Add(profileCenterLine);
                    }
                }
                startCnt = endCnt;
            }

        }

        public string DistanceUnit => InitChartTypeA.DistanceConverter.Unit;
    }
}
