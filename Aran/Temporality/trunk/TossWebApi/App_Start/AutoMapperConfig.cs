using System;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Temporality.Common.Entity;
using AutoMapper;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Aran.Aim.DataTypes;
using TossWebApi.Common;
using TossWebApi.Models.DTO;
using Aran.Converters;

namespace TossWebApi
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PublicSlot, PublicSlotDto>();

                cfg.CreateMap<PublicSlotDto, PublicSlot>().ForAllMembers(opt => opt.Ignore());
                cfg.CreateMap<PublicSlotDto, PublicSlot>().ConstructUsing(
                    dto =>
                    {
                        var publicSlot = new PublicSlot
                        {
                            Name = dto.Name,
                            EffectiveDate = dto.EffectiveDate,
                            PlannedCommitDate = dto.PlannedCommitDate,
                            SlotType = (int) dto.SlotType,
                            Status = dto.Status 
                        };

                        return publicSlot;
                    });

                cfg.CreateMap<PrivateSlot, PrivateSlotDto>();

                cfg.CreateMap<PrivateSlotDto, PrivateSlot>().ForAllMembers(opt => opt.Ignore());
                cfg.CreateMap<PrivateSlotDto, PrivateSlot>().ConstructUsing(
                    dto =>
                    {
                        var privateSlot = new PrivateSlot
                        {
                            Name = dto.Name,
                            Status = dto.Status,
                        };

                        return privateSlot;
                    });

                cfg.CreateMap<ObstacleArea, ObstacleAreaDto>().
                    ForMember("Geo", opt => opt.ResolveUsing(c => GeoJsonConverter.GetMultiPolygon(c.SurfaceExtent?.Geo)));

                cfg.CreateMap<AirportHeliport, AirportHeliportDto>().
                    ForMember("Geo", opt => opt.ResolveUsing(ah => GeoJsonConverter.GetPoint(ah.ARP?.Geo))).
                    ForMember("Elevation", opt => opt.ResolveUsing(ah => {

                        if(ah.ARP?.Elevation == null)
                            return null;

                        return ah.ARP?.Elevation.Value.ToString() + " " + (UomDistanceVertical)ah.ARP?.Elevation.Uom;
                    }));

                cfg.CreateMap<Runway, RunwayDto>();

                cfg.CreateMap<RunwayDirection, RunwayDirectionDto>();

                cfg.CreateMap<RunwayCentrelinePoint, RunwayCenterlinePointDto>().
                    ForMember("Geo", opt => opt.ResolveUsing(c => GeoJsonConverter.GetPoint(c.Location?.Geo))).
                    ForMember("Elevation", opt => opt.ResolveUsing(rc =>
                    {
                        if (rc.Location?.Elevation == null)
                            return null;

                        return rc.Location?.Elevation.Value.ToString() + " " + (UomDistanceVertical)rc.Location?.Elevation.Uom;
                    }));

                cfg.CreateMap<VerticalStructure, IEnumerable<VerticalStructureDto>>().
                    ConstructUsing(
                        verticalStructure =>
                        {
                            var verticalStructureDtos = new List<VerticalStructureDto>();
                            foreach (var verticalStructurePart in verticalStructure.Part)
                            {
                                if (verticalStructurePart.HorizontalProjection == null)
                                    continue;

                                GeoJSONObject geo = null;
                                var refType = verticalStructurePart.HorizontalProjection.Choice;

                                if (refType == VerticalStructurePartGeometryChoice.ElevatedPoint)
                                {
                                    geo = GeoJsonConverter.GetPoint(verticalStructurePart.HorizontalProjection.Location.Geo);
                                }
                                else if (refType == VerticalStructurePartGeometryChoice.ElevatedCurve)
                                {
                                    geo = GeoJsonConverter.GetMultiLineString(verticalStructurePart.HorizontalProjection.LinearExtent.Geo);
                                }
                                else if (refType == VerticalStructurePartGeometryChoice.ElevatedSurface)
                                {
                                    geo = GeoJsonConverter.GetMultiPolygon(verticalStructurePart.HorizontalProjection.SurfaceExtent.Geo);
                                }

                                var verticalStructureDto = new VerticalStructureDto
                                {
                                    Identifier = verticalStructure.Identifier,
                                    Name = verticalStructure.Name,
                                    Type = verticalStructure.Type,
                                    Geo = geo,
                                };

                                if (refType == VerticalStructurePartGeometryChoice.ElevatedPoint)
                                {
                                    verticalStructureDto.HorizontalAccuracy = ConverterToSI.Convert(verticalStructurePart.HorizontalProjection?.Location?.HorizontalAccuracy, 0);
                                    verticalStructureDto.VerticalAccuracy = ConverterToSI.Convert(verticalStructurePart.HorizontalProjection?.Location?.VerticalAccuracy, 0);
                                    verticalStructureDto.Elevation = ConverterToSI.Convert(verticalStructurePart.HorizontalProjection?.Location?.Elevation, 0);
                                }

                                verticalStructureDtos.Add(verticalStructureDto);
                            }

                            return verticalStructureDtos;
                        }
                    );

                cfg.CreateMap<VerticalStructureCreateDto, VerticalStructure>().ConstructUsing(
                    dto =>
                    {
                        var verticalStructure = new VerticalStructure
                        {
                            Identifier = dto.Identifier,
                            TimeSlice = new TimeSlice
                            {
                                Interpretation = TimeSliceInterpretationType.PERMDELTA,
                                ValidTime = new TimePeriod(dto.BeginEffectiveDate),
                                FeatureLifetime = new TimePeriod(dto.BeginEffectiveDate)
                            },
                            Type = (CodeVerticalStructure)Enum.Parse(typeof(CodeVerticalStructure), dto.Type.ToString(), true),
                        };

                        var verticalStructurePart = new VerticalStructurePart
                        {
                            Type = (CodeVerticalStructure)Enum.Parse(typeof(CodeVerticalStructure), dto.Type.ToString(), true),
                            VerticalExtentAccuracy = new ValDistance(dto.VerticalAccuracy, UomDistance.M),
                            ConstructionStatus = dto.SubmitConstructionType == Submit2AixmAs.Test ? CodeStatusConstruction.IN_CONSTRUCTION : CodeStatusConstruction.COMPLETED,
                            VerticalExtent = new ValDistance(dto.Height, UomDistance.M),
                            HorizontalProjection = new VerticalStructurePartGeometry()
                        };

                        switch (dto.Geometry)
                        {
                            case Point point:
                            {
                                verticalStructurePart.HorizontalProjection.Location = new ElevatedPoint
                                {
                                    VerticalAccuracy = verticalStructurePart.VerticalExtentAccuracy,
                                    Elevation = new ValDistanceVertical(dto.Elevation, UomDistanceVertical.M),
                                    HorizontalAccuracy = new ValDistance(dto.HorizontalAccuracy, UomDistance.M)
                                };

                                var aranPoint = GeoJsonConverter.GetAranPoint(point);

                                verticalStructurePart.HorizontalProjection.Location.Geo.Assign(aranPoint);
                                break;
                            }
                            case MultiLineString multiLineString:
                            {
                                verticalStructurePart.HorizontalProjection.LinearExtent = new ElevatedCurve
                                {
                                    VerticalAccuracy = verticalStructurePart.VerticalExtentAccuracy,
                                    Elevation = new ValDistanceVertical(dto.Elevation, UomDistanceVertical.M),
                                    HorizontalAccuracy = new ValDistance(dto.HorizontalAccuracy, UomDistance.M)
                                };

                                var aranMultiLineString = GeoJsonConverter.GetAranMultiLineString(multiLineString);

                                verticalStructurePart.HorizontalProjection.LinearExtent.Geo.Assign(aranMultiLineString);
                                break;
                            }
                            case MultiPolygon multiPolygon:
                            {
                                verticalStructurePart.HorizontalProjection.SurfaceExtent = new ElevatedSurface
                                {
                                    VerticalAccuracy = verticalStructurePart.VerticalExtentAccuracy,
                                    Elevation = new ValDistanceVertical(dto.Elevation, UomDistanceVertical.M),
                                    HorizontalAccuracy = new ValDistance(dto.HorizontalAccuracy, UomDistance.M)
                                };

                                var aranMultiPolygon = GeoJsonConverter.GetAranMultiPolygon(multiPolygon);

                                verticalStructurePart.HorizontalProjection.SurfaceExtent.Geo.Assign(aranMultiPolygon);
                                break;
                            }
                        }

                        verticalStructure.Part.Add(verticalStructurePart);

                        return verticalStructure;
                    }
                );

                cfg.CreateMap<AirportHeliport, GeoJSON.Net.Feature.Feature>().
                    ConstructUsing(
                        m =>
                        {
                            var id = m.Identifier.ToString();
                            var geometry = GeoJsonConverter.GetPoint(m.ARP?.Geo);
                            var properties = new Dictionary<string, object>
                            {
                                { "Name", m.Name },
                                { "Designator", m.Designator }
                            };

                            if (m.ARP?.Elevation != null)
                                properties.Add("Elevation",
                                    m.ARP?.Elevation.Value.ToString() + " " + (UomDistanceVertical)m.ARP?.Elevation.Uom);

                            return new GeoJSON.Net.Feature.Feature(geometry, properties, id);
                        }
                    );

                cfg.CreateMap<ObstacleArea, GeoJSON.Net.Feature.Feature>().
                    ConstructUsing(
                        m =>
                        {
                            var id = m.Identifier.ToString();
                            var geometry = GeoJsonConverter.GetMultiPolygon(m.SurfaceExtent?.Geo);
                            var properties = new Dictionary<string, object>
                            {
                                { "Type", m.Type }
                            };

                            return new GeoJSON.Net.Feature.Feature(geometry, properties, id);
                        }
                    );

                cfg.CreateMap<RunwayCentrelinePoint, GeoJSON.Net.Feature.Feature>().
                    ConstructUsing(
                        m =>
                        {
                            
                            var id = m.Identifier.ToString();
                            var geometry = GeoJsonConverter.GetPoint(m.Location?.Geo);
                            var properties = new Dictionary<string, object>
                            {
                                { "Role", m.Role?.ToString() }
                            };

                            if (m.Location?.Elevation != null)
                                properties.Add("Elevation", m.Location?.Elevation.Value.ToString() + " " + (UomDistanceVertical)m.Location?.Elevation.Uom);

                            return new GeoJSON.Net.Feature.Feature(geometry, properties, id);
                        }
                    );

                cfg.CreateMap<VerticalStructure, IEnumerable<GeoJSON.Net.Feature.Feature>>().
                    ConstructUsing(
                        verticalStructure =>
                        {
                            var verticalStructureFatures = new List<GeoJSON.Net.Feature.Feature>();
                            foreach (var verticalStrucutrePart in verticalStructure.Part)
                            {
                                if (verticalStrucutrePart.HorizontalProjection == null)
                                    continue;

                                GeoJSONObject geo = null;
                                var refType = verticalStrucutrePart.HorizontalProjection.Choice;

                                if (refType == VerticalStructurePartGeometryChoice.ElevatedPoint)
                                {
                                    geo = GeoJsonConverter.GetPoint(verticalStrucutrePart.HorizontalProjection.Location.Geo);
                                }
                                else if (refType == VerticalStructurePartGeometryChoice.ElevatedCurve)
                                {
                                    geo = GeoJsonConverter.GetMultiLineString(verticalStrucutrePart.HorizontalProjection.LinearExtent.Geo);
                                }
                                else if (refType == VerticalStructurePartGeometryChoice.ElevatedSurface)
                                {
                                    geo = GeoJsonConverter.GetMultiPolygon(verticalStrucutrePart.HorizontalProjection.SurfaceExtent.Geo);
                                }

                                var id = verticalStructure.Identifier.ToString();
                                var geometry = geo;
                                var properties = new Dictionary<string, object>
                                {
                                    { "Name", verticalStructure.Name},
                                    { "Type", verticalStructure.Type.ToString() }
                                };

                                if (refType == VerticalStructurePartGeometryChoice.ElevatedPoint)
                                {
                                    properties.Add("Horizontal Accuracy", ConverterToSI.Convert(verticalStrucutrePart.HorizontalProjection?.Location?.HorizontalAccuracy, 0));
                                    properties.Add("Vertical Accuracy", ConverterToSI.Convert(verticalStrucutrePart.HorizontalProjection?.Location?.VerticalAccuracy, 0));
                                    properties.Add("Elevation", ConverterToSI.Convert(verticalStrucutrePart.HorizontalProjection?.Location?.Elevation, 0));
                                }

                                verticalStructureFatures.Add(new GeoJSON.Net.Feature.Feature((IGeometryObject)geometry, properties, id));
                            }

                            return verticalStructureFatures;
                        }
                    );
            });
        }
    }
}
