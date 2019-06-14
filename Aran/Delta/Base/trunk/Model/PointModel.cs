using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Aran.Delta.Model
{
    public interface IPointModel
    {
        Aran.Geometries.Point Geo { get; set; }
        Guid Identifier { get; set; }
        Aran.Geometries.Point GeoPrj { get; set; }
        string Name { get; set; }
        Enums.PointChoiceType ObjectType { get; set; }
        Aran.Aim.Features.Feature Feat { get; set; }
        bool IsError { get; set; }
        double Accuracy { get; set; }
        string Type { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }

    public class PointModel:ViewModels.ViewModel, IPointModel
    {
        private SpatialReferenceOperation _spOperation;

        public PointModel(SpatialReferenceOperation spOperation)
        {
            ObjectType = Enums.PointChoiceType.Point;
            _spOperation = spOperation;
        }
        private Aran.Geometries.Point _geo;
        public Aran.Geometries.Point Geo
        { 
            get { return _geo; }
            set
            {
                _geo = value;
                if (_geo != null)
                   GeoPrj = _spOperation?.ToPrj(_geo);
                NotifyPropertyChanged("Geo");
            }
        }
        

        public Guid Identifier { get; set; }
        public Aran.Geometries.Point GeoPrj { get; set; }
        public string Name { get; set; }
        public Enums.PointChoiceType ObjectType { get; set; }

        private Aran.Aim.Features.Feature _feat;
        public Aran.Aim.Features.Feature Feat
        {
            get { return _feat; }
            set 
            {
                _feat = value;
                Accuracy = 0;
                IsError = true;
                if (_feat != null)
                {
                    dynamic dynamicFeat = _feat as dynamic;
                    if (dynamicFeat.Location?.HorizontalAccuracy != null)
                        Accuracy = Converters.ConverterToSI.Convert(dynamicFeat.Location.HorizontalAccuracy, 0);
                    else
                        Accuracy = CalcDefaultAccuracy(dynamicFeat.Location.Geo.Y, dynamicFeat.Location.Geo.X);
                 
                    //}
                    //else if (_feat is Aran.Aim.Features.NavaidEquipment) 
                    //{
                    //    var equipment = _feat as Aran.Aim.Features.NavaidEquipment;
                    //    if (equipment.Location.HorizontalAccuracy != null)
                    //    {
                    //        Accuracy = Converters.ConverterToSI.Convert(equipment.Location.HorizontalAccuracy, 0);
                    //        //IsError = false;
                    //    }
                    //    else
                    //    {
                    //        Accuracy = CalcDefaultAccuracy(equipment.Location.Geo.Y, equipment.Location.Geo.X);
                    //    }
                    //}
                    //else if (_feat is Aran.Aim.Features.Navaid)
                    //{
                    //    var navaid = _feat as Aran.Aim.Features.Navaid;
                    //    if (navaid.Location.HorizontalAccuracy != null)
                    //    {
                    //        Accuracy = Converters.ConverterToSI.Convert(navaid.Location.HorizontalAccuracy, 0);
                    //        //IsError = false;
                    //    }
                    //    else
                    //    {
                    //        Accuracy = CalcDefaultAccuracy(navaid.Location.Geo.Y, navaid.Location.Geo.X);
                    //    }
                    //}
                }
            }
        }

        public bool IsError { get; set; }

        public double Accuracy { get; set; }

        public string Type { get; set; }

        private double CalcDefaultAccuracy(double lat,double longtitude)
        {
            double latDeg, latMin, latSec;
            double longDeg, longMin, longSec;

            Functions.DD2DMS(lat, out latDeg, out latMin, out latSec, 1);
            Functions.DD2DMS(longtitude, out longDeg, out longMin, out longSec, 1);

            int coordPrecision = Convert.ToInt32(GlobalParams.Settings?.DeltaInterface.CoordinatePrecision);
            var latSecNew = latSec + 1;// Math.Pow(0.1, coordPrecision);

            var longSecNew = longSec + 1;// Math.Pow(0.1, coordPrecision);

            var latNew = Functions.DMS2DD(latDeg, latMin, latSecNew, 1);
            var longNew = Functions.DMS2DD(longDeg, longMin, longSecNew, 1);

            return PANDA.Common.NativeMethods.ReturnGeodesicDistance(longtitude, lat, longNew, latNew);

        }
    }
}
