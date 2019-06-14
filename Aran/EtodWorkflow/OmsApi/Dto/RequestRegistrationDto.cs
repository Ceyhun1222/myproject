using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using OmsApi.Entity;
using System;
using System.Collections.Generic;

namespace OmsApi.Dto
{
    public class RequestRegistrationDto : BaseRequestDto
    {
        public GeometryType GeometryType { get; set; }

        public IList<MyPoint> Coordinates { get; set; }
        
        public string Attachment { get; set; }
    }

    public enum GeometryType
    {
        Point,
        LineString,
        Polygon
    }    

    public class MyPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //public double Z { get; set; }
    }
}