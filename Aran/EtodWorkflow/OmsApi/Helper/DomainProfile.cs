using AutoMapper;
using GeoAPI.Geometries;
using GeoJSON.Net;
using GeoJSON.Net.Converters;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using OmsApi.Dto;
using OmsApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmsApi.Configuration
{

    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<CompanyDto, Company>().ReverseMap();
            CreateMap<UserDto, ApplicationUser>()
                .ForMember(k => k.Id, opt => opt.Ignore())
                .ForMember(k=>k.Status,opt=>opt.Ignore())
                .ReverseMap();
            CreateMap<UserRegistration, ApplicationUser>();
            CreateMap<ApplicationUser, UserBasicInfoDto>()
                .ForMember(k => k.Fullname, opt => opt.MapFrom(o => $"{o.Firstname} {o.Lastname}"));
            CreateMap<PrivateSlotDto, PrivateSlot>()
                .ForMember(k => k.TossId, opt => opt.MapFrom(o => o.Id))
                .ReverseMap()
                .ForMember(k => k.Id, opt => opt.MapFrom(o => o.TossId));
            CreateMap<SlotDto, Slot>()
                .ForMember(k => k.TossId, opt => opt.MapFrom(o => o.Id))
                .ForMember(k => k.Id, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<RequestRegistrationDto, Request>()
                .ForMember(k => k.Geometry, opt => opt.MapFrom<GeometryResolver>())
                .ForMember(k=>k.Id,opt=>opt.Ignore())
                .ForMember(k => k.Attachment, opt => opt.MapFrom(o => Encoding.ASCII.GetBytes(o.Attachment)))
                .ForMember(k=>k.Elevation, opt=>opt.MapFrom(o=>o.Elevation + o.Height))
                .ForMember(k=>k.Submitted, opt=>opt.Ignore())
                .ReverseMap()   
                .ForMember(k=>k.Elevation, opt=>opt.MapFrom(o=>o.Elevation-o.Height))
                .ForMember(k => k.Coordinates, opt => opt.MapFrom<BackwardGeometryResolver>())      
                .ForMember(k=>k.Attachment, opt=>opt.Ignore())
                .ForMember(k => k.GeometryType, opt => opt.MapFrom<BackwardGeometryTypeResolver>());
            CreateMap<RequestReportDto, RequestReport>().ReverseMap();
            CreateMap<TossRunwayDto, RunwayDto>();
            CreateMap<Request, TossRequestSubmit2AixmDto>()
                //.ForMember(k => k.IsTemporary, opt => opt.MapFrom(t => t.Duration == Duration.Temporary))
                .ForMember(k => k.Geometry, opt => opt.MapFrom(t=> JsonConvert.DeserializeObject<GeoJSONObject>(t.Geometry, new GeoJsonConverter())))
                .ForMember(k => k.BeginEffectiveDate, opt => opt.MapFrom(t => t.BeginningDate));
            CreateMap<Request, RequestDto4Client>()
                .ForMember(k=>k.Attachment,opt=>opt.Ignore())
                .ForMember(k=>k.Elevation, opt=>opt.MapFrom(t=>t.Elevation-t.Height))
                .ForMember(k => k.Coordinates, opt => opt.MapFrom<BackwardGeometryResolver>())
                .ForMember(k => k.GeometryType, opt => opt.MapFrom<BackwardGeometryTypeResolver>());
            CreateMap<Request, RequestDto4Admin>()
                .ForMember(k => k.Attachment, opt => opt.Ignore())
                .ForMember(k => k.UserFullname, opt => opt.MapFrom(t => $"{t.CreatedBy.Firstname} {t.CreatedBy.Lastname}"))
                .ForMember(k => k.Elevation, opt => opt.MapFrom(t => t.Elevation - t.Height))
                .ForMember(k => k.UserId, opt => opt.MapFrom(t => t.CreatedBy.Id))
                .ForMember(k => k.Coordinates, opt => opt.MapFrom<BackwardGeometryResolver>())
                .ForMember(k => k.GeometryType, opt => opt.MapFrom<BackwardGeometryTypeResolver>());

        }
    }

    public class GeometryResolver:IValueResolver<RequestRegistrationDto,Request, string>
    {
        public string Resolve(RequestRegistrationDto requestRegistrationDto, Request request, string geoJSONObject, ResolutionContext context)
        {
            var writer = new GeoJsonWriter();
            string res;
            switch (requestRegistrationDto.GeometryType)
            {
                case GeometryType.Point:
                    res = writer.Write(new Point(requestRegistrationDto.Coordinates[0].Longitude, requestRegistrationDto.Coordinates[0].Latitude));
                    break;
                case GeometryType.LineString:
                    var coords = new Coordinate[requestRegistrationDto.Coordinates.Count];
                    for (int i = 0; i < requestRegistrationDto.Coordinates.Count; i++)
                    {
                        var item = requestRegistrationDto.Coordinates[i];
                        coords[i] = new Coordinate(item.Longitude, item.Latitude);
                    }
                    var lnString = new LineString(coords);
                    res = writer.Write(lnString);
                    break;
                case GeometryType.Polygon:
                    coords = new Coordinate[requestRegistrationDto.Coordinates.Count];
                    for (int i = 0; i < requestRegistrationDto.Coordinates.Count; i++)
                    {
                        var item = requestRegistrationDto.Coordinates[i];
                        coords[i] = new Coordinate(item.Longitude, item.Latitude);
                    }
                    var lnRing = new LinearRing(coords);
                    res = writer.Write(new Polygon(lnRing));
                    break;
                default:
                    throw new NotImplementedException("Not found geometry type");
            }
            return res;
        }
    }

    public class BackwardGeometryResolver : IValueResolver<Request, RequestRegistrationDto, IList<MyPoint>>
    {
        public IList<MyPoint> Resolve(Request request, RequestRegistrationDto requestRegistrationDto, IList<MyPoint> coordinates, ResolutionContext context)
        {
            var reader = new GeoJsonReader();
            Geometry geom = new GeoJsonReader().Read<Geometry>(request.Geometry);
            var result = new List<MyPoint>();
            foreach (var coord in geom.Coordinates)
            {
                result.Add(new MyPoint() { Latitude = coord.Y, Longitude = coord.X });
            }
            return result;
        }
    }
    
    public class BackwardGeometryTypeResolver : IValueResolver<Request, RequestRegistrationDto, GeometryType>
    {
        public GeometryType Resolve(Request request, RequestRegistrationDto requestRegistrationDto, GeometryType geometryType, ResolutionContext context)
        {
            var reader = new GeoJsonReader();
            Geometry geom = new GeoJsonReader().Read<Geometry>(request.Geometry);
            if (Enum.TryParse<GeometryType>(geom.GeometryType, out var result))
                return result;
            throw new NotImplementedException("Not found geometry type");
        }
    }
}