using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;
using Aran.Temporality.Common.ArcGis;
using ESRI.ArcGIS.Geometry;
using System.Reflection;
using ESRI.ArcGIS.esriSystem;
using ArenaToolBox;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using Aran.Converters;
using Aran.Geometries;
using Aran.Aim;

namespace ARENA
{
    public class PDM_AIM_Converter
    {

       private static Dictionary<Type, Type> elementTypes = new Dictionary<Type, Type>();

       static PDM_AIM_Converter()
       {
           elementTypes.Add(typeof(PDM.AirportHeliport), typeof(PDM_AIRPORT_AIM_Converter));
           elementTypes.Add(typeof(PDM.NavaidSystem), typeof(PDM_NAVAID_AIM_Converter));
           elementTypes.Add(typeof(PDM.DME), typeof(PDM_DME_AIM_Converter));
           elementTypes.Add(typeof(PDM.VOR), typeof(PDM_VOR_AIM_Converter));
           elementTypes.Add(typeof(PDM.NDB), typeof(PDM_NDB_AIM_Converter));
           elementTypes.Add(typeof(PDM.TACAN), typeof(PDM_TACAN_AIM_Converter));
           elementTypes.Add(typeof(PDM.Marker), typeof(PDM_MARKER_AIM_Converter));
           elementTypes.Add(typeof(PDM.Localizer), typeof(PDM_LOCALIZER_AIM_Converter));
           elementTypes.Add(typeof(PDM.GlidePath), typeof(PDM_GLIDEPATH_AIM_Converter));
           elementTypes.Add(typeof(PDM.Airspace), typeof(PDM_AIRSPACE_AIM_Converter));
           elementTypes.Add(typeof(PDM.Enroute), typeof(PDM_ENROUTE_AIM_Converter));
           elementTypes.Add(typeof(PDM.RouteSegment), typeof(PDM_ROUTESEGMENT_AIM_Converter));

       }

        public static List<Aran.Aim.Features.Feature> PDM_Object_Convert(PDMObject pdmObject)
        {
            if (elementTypes.ContainsKey(pdmObject.GetType()))
            {

                Type t = elementTypes[pdmObject.GetType()];
                PDM_IAIM_Converter result = (PDM_IAIM_Converter)Activator.CreateInstance(t);
                List<Aran.Aim.Features.Feature> objlist = result.Convert_PDM_Object(pdmObject, null);
                return objlist;
            }
            else
                return null;
        }

        public static ElevatedPoint getElevatedPoint(PDM.PDMObject obj_PDM)
        {
            if (obj_PDM.Geo == null) obj_PDM.RebuildGeo();
            if (obj_PDM.Geo == null) return null;

            ElevatedPoint _ElevPnt = new ElevatedPoint();
            Aran.Geometries.Point pnt = new Aran.Geometries.Point();
            pnt.X = ((IPoint)obj_PDM.Geo).X;
            pnt.Y = ((IPoint)obj_PDM.Geo).Y;
            _ElevPnt.Geo.Assign(pnt);

            UomDistanceVertical udv;

            if ((obj_PDM.Elev.HasValue) && Enum.TryParse<UomDistanceVertical>(obj_PDM.Elev_UOM.ToString(), true, out udv))
            {
                _ElevPnt.Elevation = new ValDistanceVertical(obj_PDM.Elev.Value, udv);

            }


            return _ElevPnt;


        }

        public static ValDistanceVertical GetVerticalDistance(double? _vertDistValue,string _uomDistVert)
        {
            ValDistanceVertical res = new ValDistanceVertical();
            

            UomDistanceVertical udv;
            if (_vertDistValue.HasValue && Enum.TryParse<UomDistanceVertical>(_uomDistVert, true, out udv))
            {
                res = new ValDistanceVertical(_vertDistValue.Value, udv);
            }

            return res;
        }

        public static ValDistance GetDistance(double? _distValue, string _uomDist)
        {
            ValDistance res = new ValDistance();
            UomDistance udv;
            if (_distValue.HasValue && Enum.TryParse<UomDistance>(_uomDist, true, out udv))
            {
                res = new ValDistance(_distValue.Value, udv);
            }

            return res;
        }
 
  
        public interface PDM_IAIM_Converter
        {
            List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef);
        }

        public class PDM_AIRPORT_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {

                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.AirportHeliport obj_AIM;
                PDM.AirportHeliport obj_PDM = (PDM.AirportHeliport)pdmObject;
                try
                {
                    //double dbl = 0;

                    obj_AIM = new Aran.Aim.Features.AirportHeliport
                    {
                        Identifier = new Guid( pdmObject.ID),
                        Designator = obj_PDM.Designator !=null ? obj_PDM.Designator : "",
                        Name = obj_PDM.Name != null ? obj_PDM.Name : "",
                        DesignatorIATA = obj_PDM.DesignatorIATA != null ? obj_PDM.DesignatorIATA : "",
                        MagneticVariation = obj_PDM.MagneticVariation,
                        Type  = Aran.Aim.Enums.CodeAirportHeliport.AD,
                        ARP = getElevatedPoint(obj_PDM),
                        TransitionAltitude = GetVerticalDistance(obj_PDM.TransitionAltitude.Value, obj_PDM.TransitionAltitudeUOM.ToString()),
                       
                    };

                    res.Add(obj_AIM);

                    if ((obj_PDM.RunwayList != null) && (obj_PDM.RunwayList.Count > 0))
                    {
                        FeatureRef f_ref = new FeatureRef(obj_AIM.Identifier);

                        foreach (var rwy in obj_PDM.RunwayList)
                        {
                            PDM_RUNWAY_AIM_Converter rwyConverter = new PDM_RUNWAY_AIM_Converter();
                            res.AddRange(rwyConverter.Convert_PDM_Object(rwy, f_ref));
                        }
                    }
                   
                }
                catch
                {
                    return null;
                }
                return res;
            }

            
        }

        public class PDM_RUNWAY_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.Runway obj_AIM;
                PDM.Runway obj_PDM = (PDM.Runway)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.Runway
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator,
                        NominalLength = GetDistance(obj_PDM.Length, obj_PDM.Uom.ToString()),
                        NominalWidth = GetDistance(obj_PDM.Width, obj_PDM.Uom.ToString()),
                        AssociatedAirportHeliport = aimRef,
                    };

                    res.Add(obj_AIM);

                    if ((obj_PDM.RunwayDirectionList != null) && (obj_PDM.RunwayDirectionList.Count > 0))
                    {
                        FeatureRef f_ref = new FeatureRef(obj_AIM.Identifier);

                        foreach (var rdn in obj_PDM.RunwayDirectionList)
                        {
                            PDM_THR_AIM_Converter thrConverter = new PDM_THR_AIM_Converter();
                            res.AddRange(thrConverter.Convert_PDM_Object(rdn, f_ref));
                        }
                    }
                }
                catch
                {
                    return null;
                }

                return res;
            }

        }

        public class PDM_THR_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.RunwayDirection obj_AIM;
                PDM.RunwayDirection obj_PDM = (PDM.RunwayDirection)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.RunwayDirection
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator,
                        MagneticBearing = obj_PDM.MagBearing,
                        TrueBearing = obj_PDM.TrueBearing,
                        ElevationTDZ = GetVerticalDistance(obj_PDM.Elev, obj_PDM.Elev_UOM.ToString()),
                        UsedRunway = aimRef,
                    };

                    res.Add(obj_AIM);

                    if ((obj_PDM.CenterLinePoints != null) && (obj_PDM.CenterLinePoints.Count > 0))
                    {
                        foreach (var clp in obj_PDM.CenterLinePoints)
                        {
                            PDM_CLP_AIM_Converter clpConverter = new PDM_CLP_AIM_Converter();
                            res.AddRange(clpConverter.Convert_PDM_Object(clp, aimRef));
                        }
                    }

                    if ((obj_PDM.Related_NavaidSystem != null) && (obj_PDM.Related_NavaidSystem.Count > 0))
                    {
                        FeatureRef f_ref = new FeatureRef(obj_AIM.Identifier);

                        foreach (var nvd in obj_PDM.Related_NavaidSystem)
                        {
                            PDM_NAVAID_AIM_Converter nvdConverter = new PDM_NAVAID_AIM_Converter();
                            res.AddRange(nvdConverter.Convert_PDM_Object(nvd, f_ref));
                        }
                    }
                    
                }
                catch
                {
                    return null;
                }

                return res;
            }

           
        }

        public class PDM_CLP_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.RunwayCentrelinePoint obj_AIM;
                PDM.RunwayCenterLinePoint obj_PDM = (PDM.RunwayCenterLinePoint)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.RunwayCentrelinePoint
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Role = getRole(obj_PDM),
                        Location = getElevatedPoint(obj_PDM),
                        OnRunway = aimRef,
                    };

                    res.Add(obj_AIM);
                }

                catch
                {
                    return null;
                }

                return res;
            }

            private CodeRunwayPointRole getRole(RunwayCenterLinePoint obj_PDM)
            {
                Aran.Aim.Enums.CodeRunwayPointRole uom_role;
                if (Enum.TryParse<Aran.Aim.Enums.CodeRunwayPointRole>(obj_PDM.Role.ToString(), out uom_role))
                    return uom_role;
                else return Aran.Aim.Enums.CodeRunwayPointRole.MID;
            }


        }

        public class PDM_NAVAID_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.Navaid obj_AIM;
                PDM.NavaidSystem obj_PDM = (PDM.NavaidSystem)pdmObject;

                
                try
                {
                    obj_AIM = new Aran.Aim.Features.Navaid
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator,
                        Name = obj_PDM.Name,
                        Type = GetNavaidSystemType(obj_PDM),
                        Location = obj_PDM.Components != null && obj_PDM.Components.Count>0 ? getElevatedPoint(obj_PDM.Components[0]) : null,
                    };

                    if (aimRef != null)
                    {
                        var f_ref = new FeatureRefObject();
                        f_ref.Feature = aimRef;
                        obj_AIM.RunwayDirection.Add(f_ref);
                    }

                    if ((obj_PDM.Components != null) && (obj_PDM.Components.Count > 0))
                    {
                        foreach (var navCmnt in obj_PDM.Components)
                        {
                            Aran.Aim.Features.NavaidComponent navaidComponent = new Aran.Aim.Features.NavaidComponent();
                            navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                            navaidComponent.TheNavaidEquipment.Type = GetEquipmentType(navCmnt);
                            navaidComponent.TheNavaidEquipment.Identifier = new Guid(obj_PDM.ID);

                            ///////////////////
                            res.AddRange( PDM_AIM_Converter.PDM_Object_Convert(navCmnt));
                            ////////////////////

                            obj_AIM.NavaidEquipment.Add(navaidComponent);
                        }
                    }

                    res.Add(obj_AIM);


                }

                catch
                {
                    return null;
                }

                return res;
            }

            private Aran.Aim.NavaidEquipmentType GetEquipmentType(PDM.NavaidComponent navCmnt)
            {
                Aran.Aim.NavaidEquipmentType res = Aran.Aim.NavaidEquipmentType.DME;
                switch (navCmnt.PDM_Type)
                {
                    case PDM_ENUM.Localizer:
                        res = Aran.Aim.NavaidEquipmentType.Localizer;
                        break;
                    case PDM_ENUM.GlidePath:
                        res = Aran.Aim.NavaidEquipmentType.Glidepath;
                        break;
                    case PDM_ENUM.VOR:
                        res = Aran.Aim.NavaidEquipmentType.VOR;
                        break;
                    case PDM_ENUM.DME:
                        res = Aran.Aim.NavaidEquipmentType.DME;
                        break;
                    case PDM_ENUM.TACAN:
                        res = Aran.Aim.NavaidEquipmentType.TACAN;
                        break;
                    case PDM_ENUM.NDB:
                        res = Aran.Aim.NavaidEquipmentType.NDB;
                        break;
                    case PDM_ENUM.Marker:
                        res = Aran.Aim.NavaidEquipmentType.MarkerBeacon;
                        break;
                    default:
                        res = Aran.Aim.NavaidEquipmentType.DME;
                        break;

                }

                return res;

            }

            private CodeNavaidService GetNavaidSystemType(NavaidSystem obj_PDM)
            {
                Aran.Aim.Enums.CodeNavaidService _uom;
                if (Enum.TryParse<Aran.Aim.Enums.CodeNavaidService>(obj_PDM.CodeNavaidSystemType.ToString(), out _uom))
                    return _uom;
                else return Aran.Aim.Enums.CodeNavaidService.DME;
            }


        }

        public class PDM_DME_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.DME obj_AIM;
                PDM.DME obj_PDM = (PDM.DME)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.DME
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator!=null ? obj_PDM.Designator:"",
                        Name = obj_PDM.NavName!=null? obj_PDM.NavName:"",
                        GhostFrequency = obj_PDM.GhostFrequency >0 ? GetFrequency(obj_PDM) :  new ValFrequency(0,UomFrequency.KHZ),
                        Location = getElevatedPoint(obj_PDM),
                    };

                    res.Add(obj_AIM);

                }

                catch
                {
                    return null;
                }

                return res;
            }

            private ValFrequency GetFrequency(PDM.DME obj_PDM)
            {
                ValFrequency res = new ValFrequency();
                UomFrequency udv;
                if (Enum.TryParse<UomFrequency>(obj_PDM.GhostFrequency.ToString(), true, out udv))
                {
                    res = new ValFrequency(obj_PDM.GhostFrequency.Value, udv);
                }

                return res;
            }

  
        }

        public class PDM_VOR_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.VOR obj_AIM;
                PDM.VOR obj_PDM = (PDM.VOR)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.VOR
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator != null ? obj_PDM.Designator : "",
                        Name = obj_PDM.NavName != null ? obj_PDM.NavName : "",
                        Frequency = obj_PDM.Frequency >0 ? GetFrequency(obj_PDM) : new ValFrequency(0, UomFrequency.KHZ),
                        Location = getElevatedPoint(obj_PDM),
                        Declination = obj_PDM.StationDeclination!=null? Convert.ToDouble(obj_PDM.StationDeclination) : 0,
                    };

                    res.Add(obj_AIM);

                }

                catch
                {
                    return null;
                }

                return res;
            }

            private ValFrequency GetFrequency(PDM.VOR obj_PDM)
            {
                ValFrequency res = new ValFrequency();
                UomFrequency udv;
                if (Enum.TryParse<UomFrequency>(obj_PDM.Frequency.ToString(), true, out udv))
                {
                    res = new ValFrequency(obj_PDM.Frequency.Value, udv);
                }

                return res;
            }


        }

        public class PDM_NDB_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.NDB obj_AIM;
                PDM.NDB obj_PDM = (PDM.NDB)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.NDB
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator != null ? obj_PDM.Designator : "",
                        Name = obj_PDM.NavName != null ? obj_PDM.NavName : "",
                        Frequency = obj_PDM.Frequency >0 ? GetFrequency(obj_PDM) : new ValFrequency(0, UomFrequency.KHZ),
                        Location = getElevatedPoint(obj_PDM),
                        MagneticVariation = obj_PDM.MagVar,
                    };

                    res.Add(obj_AIM);

                }

                catch
                {
                    return null;
                }

                return res;
            }

            private ValFrequency GetFrequency(PDM.NDB obj_PDM)
            {
                ValFrequency res = new ValFrequency();
                UomFrequency udv;
                if (Enum.TryParse<UomFrequency>(obj_PDM.Frequency_UOM.ToString(), true, out udv))
                {
                    res = new ValFrequency(obj_PDM.Frequency.Value, udv);
                }

                return res;
            }


        }

        public class PDM_TACAN_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.TACAN obj_AIM;
                PDM.TACAN obj_PDM = (PDM.TACAN)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.TACAN
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator != null ? obj_PDM.Designator : "",
                        Name = obj_PDM.NavName != null ? obj_PDM.NavName : "",
                        Location = getElevatedPoint(obj_PDM),
                        MagneticVariation = obj_PDM.MagVar,
                    };

                    res.Add(obj_AIM);

                }

                catch
                {
                    return null;
                }

                return res;
            }

 

        }

        public class PDM_MARKER_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.MarkerBeacon obj_AIM;
                PDM.Marker obj_PDM = (PDM.Marker)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.MarkerBeacon
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator != null ? obj_PDM.Designator : "",
                        Name = obj_PDM.NavName != null ? obj_PDM.NavName : "",
                        Location = getElevatedPoint(obj_PDM),
                        Frequency = obj_PDM.Frequency > 0 ? GetFrequency(obj_PDM) : new ValFrequency(0, UomFrequency.KHZ),
                        MagneticVariation = obj_PDM.MagVar,
                        AxisBearing = obj_PDM.Axis_Bearing,
                    };

                    res.Add(obj_AIM);

                }

                catch
                {
                    return null;
                }

                return res;
            }

            private ValFrequency GetFrequency(PDM.Marker obj_PDM)
            {
                ValFrequency res = new ValFrequency();
                UomFrequency udv;
                if (Enum.TryParse<UomFrequency>(obj_PDM.Frequency_UOM.ToString(), true, out udv))
                {
                    res = new ValFrequency(obj_PDM.Frequency.Value, udv);
                }

                return res;
            }



        }

        public class PDM_LOCALIZER_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.Localizer obj_AIM;
                PDM.Localizer obj_PDM = (PDM.Localizer)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.Localizer
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator != null ? obj_PDM.Designator : "",
                        Name = obj_PDM.NavName != null ? obj_PDM.NavName : "",
                        Frequency = obj_PDM.Frequency > 0 ? GetFrequency(obj_PDM) : new ValFrequency(0, UomFrequency.KHZ),
                        Location = getElevatedPoint(obj_PDM),
                        MagneticBearing = obj_PDM.MagBrg,
                        TrueBearing = obj_PDM.trueBearing,
                        MagneticVariation = obj_PDM.MagVar,
                        WidthCourse = obj_PDM.Width
                    };

                    res.Add(obj_AIM);

                }

                catch
                {
                    return null;
                }

                return res;
            }

            private ValFrequency GetFrequency(PDM.Localizer obj_PDM)
            {
                ValFrequency res = new ValFrequency();
                UomFrequency udv;
                if (Enum.TryParse<UomFrequency>(obj_PDM.Frequency.ToString(), true, out udv))
                {
                    res = new ValFrequency(obj_PDM.Frequency.Value, udv);
                }

                return res;
            }


        }

        public class PDM_GLIDEPATH_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {
                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.Glidepath obj_AIM;
                PDM.GlidePath obj_PDM = (PDM.GlidePath)pdmObject;
                try
                {
                    obj_AIM = new Aran.Aim.Features.Glidepath
                    {
                        Identifier = new Guid(obj_PDM.ID),
                        Designator = obj_PDM.Designator != null ? obj_PDM.Designator : "",
                        Name = obj_PDM.NavName != null ? obj_PDM.NavName : "",
                        Location = getElevatedPoint(obj_PDM),
                        MagneticVariation = obj_PDM.MagVar,
                        Slope = obj_PDM.Angle,
                    };

                    res.Add(obj_AIM);

                }

                catch
                {
                    return null;
                }

                return res;
            }

    
        }

        public class PDM_AIRSPACE_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {

                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.Airspace obj_AIM;
                PDM.Airspace obj_PDM = (PDM.Airspace)pdmObject;
                try
                {
                    System.Diagnostics.Debug.WriteLine(obj_PDM.TxtName);
                   
                    obj_AIM = new Aran.Aim.Features.Airspace
                    {
                        Identifier = new Guid(pdmObject.ID),
                        Name = obj_PDM.TxtName!=null?  obj_PDM.TxtName : "",
                        Designator = obj_PDM.CodeID!=null? obj_PDM.CodeID :"",
                        Type = getAirspaceType(obj_PDM),
                    };

                    

                    if ((obj_PDM.AirspaceVolumeList != null) && (obj_PDM.AirspaceVolumeList.Count > 0))
                    {
                        

                        foreach (var arsps in obj_PDM.AirspaceVolumeList)
                        {
                            if (arsps.Geo == null) arsps.RebuildGeo2();
                            AirspaceGeometryComponent airspaceComponent = new AirspaceGeometryComponent();

                            airspaceComponent.TheAirspaceVolume = new Aran.Aim.Features.AirspaceVolume();
                            airspaceComponent.TheAirspaceVolume.UpperLimit = GetVerticalDistance(arsps.ValDistVerUpper, arsps.UomValDistVerUpper.ToString());
                            airspaceComponent.TheAirspaceVolume.LowerLimit = GetVerticalDistance(arsps.ValDistVerLower, arsps.UomValDistVerLower.ToString());
                            airspaceComponent.TheAirspaceVolume.MaximumLimit = GetVerticalDistance(arsps.ValDistVerMax, arsps.UomValDistVerMax.ToString());
                            airspaceComponent.TheAirspaceVolume.MinimumLimit = GetVerticalDistance(arsps.ValDistVerMnm, arsps.UomValDistVerMnm.ToString());
                            airspaceComponent.TheAirspaceVolume.UpperLimitReference = getVertivalReference(arsps.CodeDistVerUpper);
                            airspaceComponent.TheAirspaceVolume.LowerLimitReference = getVertivalReference(arsps.CodeDistVerLower);
                            airspaceComponent.TheAirspaceVolume.MinimumLimitReference = getVertivalReference(arsps.CodeDistVerMnm);
                            airspaceComponent.TheAirspaceVolume.MaximumLimitReference = getVertivalReference(arsps.CodeDistVerMax);

                            if (arsps.Geo != null)
                            {
                                airspaceComponent.TheAirspaceVolume.HorizontalProjection = new Surface();
                                Aran.Geometries.Geometry airsapceGeo = ConvertFromEsriGeom.ToGeometry(arsps.Geo, true);
                                
                                foreach (Aran.Geometries.Polygon geom in airsapceGeo)
                                    airspaceComponent.TheAirspaceVolume.HorizontalProjection.Geo.Add(geom);
                            }

                            obj_AIM.GeometryComponent.Add(airspaceComponent);
                        }
                    }

                    res.Add(obj_AIM);

                }
                catch(Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(obj_PDM.GetObjectLabel());
                    return null;
                }
                return res;
            }

            private CodeAirspace getAirspaceType(PDM.Airspace obj_PDM)
            {
                CodeAirspace res = CodeAirspace.D_OTHER;

                Enum.TryParse<CodeAirspace>(obj_PDM.CodeType.ToString(), true, out res);
                
                return res;
            }

            private Aran.Aim.Enums.CodeVerticalReference getVertivalReference(CODE_DIST_VER code)
            {
                Aran.Aim.Enums.CodeVerticalReference res = Aran.Aim.Enums.CodeVerticalReference.MSL;

                Enum.TryParse<Aran.Aim.Enums.CodeVerticalReference>(code.ToString(), true, out res);

                return res;
            }

        }

        public class PDM_ENROUTE_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {

                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.Route obj_AIM;
                PDM.Enroute obj_PDM = (PDM.Enroute)pdmObject;
                try
                {

                    obj_AIM = new Aran.Aim.Features.Route
                    {
                        Identifier = new Guid(pdmObject.ID),
                        Name = obj_PDM.TxtDesig,
                        
                       
                    };

                    if (obj_PDM.InternationalUse != PDM.CodeRouteOrigin.OTHER) obj_AIM.InternationalUse = getInternationalUseValue(obj_PDM);

                    if ((obj_PDM.Routes != null) && (obj_PDM.Routes.Count > 0))
                    {
                        foreach (var rtSeg in obj_PDM.Routes)
                        {
                            FeatureRef f_ref = new FeatureRef(obj_AIM.Identifier);

                            PDM_ROUTESEGMENT_AIM_Converter rtSegConverter = new PDM_ROUTESEGMENT_AIM_Converter();
                            res.AddRange(rtSegConverter.Convert_PDM_Object(rtSeg, f_ref));

                        }
                    }

                    res.Add(obj_AIM);

                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(obj_PDM.GetObjectLabel());
                    return null;
                }
                return res;
            }

            private Aran.Aim.Enums.CodeRouteOrigin getInternationalUseValue(PDM.Enroute obj_PDM)
            {
                Aran.Aim.Enums.CodeRouteOrigin res = Aran.Aim.Enums.CodeRouteOrigin.BOTH;

                Enum.TryParse<Aran.Aim.Enums.CodeRouteOrigin>(obj_PDM.InternationalUse.ToString(), true, out res);

                return res;
            }


        }

        public class PDM_ROUTESEGMENT_AIM_Converter : PDM_IAIM_Converter
        {
            public List<Aran.Aim.Features.Feature> Convert_PDM_Object(PDMObject pdmObject, FeatureRef aimRef)
            {

                List<Aran.Aim.Features.Feature> res = new List<Feature>();

                Aran.Aim.Features.RouteSegment obj_AIM;
                PDM.RouteSegment obj_PDM = (PDM.RouteSegment)pdmObject;
                try
                {

                    obj_AIM = new Aran.Aim.Features.RouteSegment
                    {
                        Identifier = new Guid(pdmObject.ID),
                        UpperLimit = GetVerticalDistance(obj_PDM.ValDistVerUpper,obj_PDM.UomValDistVerUpper.ToString()),
                        LowerLimit = GetVerticalDistance(obj_PDM.ValDistVerLower, obj_PDM.UomValDistVerLower.ToString()),
                        MinimumEnrouteAltitude = GetVerticalDistance(obj_PDM.ValDistVerMnm, obj_PDM.UomValDistVerMnm.ToString()),
                        Length = GetDistance(obj_PDM.ValLen,obj_PDM.UomValLen.ToString()),
                        WidthLeft = GetDistance(obj_PDM.ValWidLeft,obj_PDM.UomValWid.ToString()),
                        WidthRight = GetDistance(obj_PDM.ValWidRight, obj_PDM.UomValWid.ToString()),
                        MagneticTrack = obj_PDM.ValMagTrack,
                        ReverseMagneticTrack = obj_PDM.ValReversMagTrack,
                        TrueTrack = obj_PDM.ValTrueTrack,
                        ReverseTrueTrack = obj_PDM.ValReversTrueTrack
                    };

                    if (obj_PDM.StartPoint != null) obj_AIM.Start = ToRouteSegmentPoint(obj_PDM.StartPoint);
                    if (obj_PDM.EndPoint != null) obj_AIM.Start = ToRouteSegmentPoint(obj_PDM.EndPoint);

                    obj_AIM.CurveExtent = new Curve();
                    if (obj_PDM.Geo == null) obj_PDM.RebuildGeo();


                    if (obj_PDM.Geo != null)
                    {
                        var routeGeo = (MultiLineString)Aran.Converters.ConvertFromEsriGeom.ToGeometry(obj_PDM.Geo);
                        if (routeGeo != null)
                            obj_AIM.CurveExtent.Geo.Add(routeGeo);
                    }

                    obj_AIM.RouteFormed = aimRef;

                    res.Add(obj_AIM);


                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(obj_PDM.GetObjectLabel());
                    return null;
                }
                return res;
            }

            private Aran.Aim.Enums.CodeRouteOrigin getInternationalUseValue(PDM.Enroute obj_PDM)
            {
                Aran.Aim.Enums.CodeRouteOrigin res = Aran.Aim.Enums.CodeRouteOrigin.BOTH;

                Enum.TryParse<Aran.Aim.Enums.CodeRouteOrigin>(obj_PDM.InternationalUse.ToString(), true, out res);

                return res;
            }


            private Aran.Aim.Features.EnRouteSegmentPoint ToRouteSegmentPoint(PDM.RouteSegmentPoint pdmRouteSegmentPoint)
            {
                var result = new EnRouteSegmentPoint();
                result.PointChoice = new SignificantPoint();
                switch (pdmRouteSegmentPoint.PointChoice)
                {
                    case PDM.PointChoice.AirportHeliport:
                        result.PointChoice.AirportReferencePoint = new FeatureRef { FeatureType = FeatureType.AirportHeliport, Identifier = new Guid(pdmRouteSegmentPoint.PointChoiceID) };
                        break;
                    case PDM.PointChoice.DesignatedPoint:
                        result.PointChoice.FixDesignatedPoint = new FeatureRef { FeatureType = FeatureType.DesignatedPoint, Identifier = new Guid(pdmRouteSegmentPoint.PointChoiceID) };
                        break;
                    case PDM.PointChoice.Navaid:
                        result.PointChoice.NavaidSystem = new FeatureRef { FeatureType = FeatureType.Navaid, Identifier = new Guid(pdmRouteSegmentPoint.Route_LEG_ID) };
                        break;
                    case PDM.PointChoice.RunwayCentrelinePoint:
                        result.PointChoice.RunwayPoint = new FeatureRef { FeatureType = FeatureType.Runway, Identifier = new Guid(pdmRouteSegmentPoint.PointChoiceID) };
                        break;
                }
                return result;
            }


        }



    }
}
