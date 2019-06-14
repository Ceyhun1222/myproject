using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Metadata.Geo
{
    public static class GeoMetadata
    {
        static GeoMetadata ()
        {
            _aimClassInfoPropDict = new Dictionary<AimClassInfo, GeoClassInfo> ();
        }

        public static List<GeoClassInfo> GeoClassInfoList
        {
            get
            {
                if (_geoClassInfoList == null)
                    LoadGeoClassInfoList ();

                return _geoClassInfoList;
            }
        }

        public static GeoClassInfo GetGeoInfoByAimInfo (AimClassInfo aimInfo)
        {
            if (_geoClassInfoList == null)
                LoadGeoClassInfoList ();

            GeoClassInfo geoClassInfo;
            _aimClassInfoPropDict.TryGetValue (aimInfo, out geoClassInfo);
            return geoClassInfo;
        }

        
        private static void LoadGeoClassInfoList ()
        {
            AimGeomType [] geomTypes = new AimGeomType [] { AimGeomType.Point, AimGeomType.Curve, AimGeomType.Surface };
            
            AimClassInfo [] geomClassInfos = new AimClassInfo [] {
                AimMetadata.GetClassInfoByIndex ((int) AimFieldType.GeoPoint),
                AimMetadata.GetClassInfoByIndex ((int) AimFieldType.GeoPolyline),
                AimMetadata.GetClassInfoByIndex ((int) AimFieldType.GeoPolygon) };

            foreach (AimClassInfo aci in AimMetadata.AimClassInfoList)
            {
                if (aci.AimObjectType == AimObjectType.Object ||
                    aci.AimObjectType == AimObjectType.Feature)
                {
                    GeoClassInfo gci = new GeoClassInfo ();
                    gci.AimClassInfo = aci;
                    gci.PointProps = new List<List<AimPropInfo>> ();
                    gci.CurveProps = new List<List<AimPropInfo>> ();
                    gci.SurfaceProps = new List<List<AimPropInfo>> ();
                    _aimClassInfoPropDict.Add (aci, gci);
                }
            }

            int i = 0;
            foreach (AimClassInfo aci in geomClassInfos)
            {
                GeoClassInfo gci = new GeoClassInfo ();
                gci.AimClassInfo = aci;
                gci.PointProps = new List<List<AimPropInfo>> ();
                gci.CurveProps = new List<List<AimPropInfo>> ();
                gci.SurfaceProps = new List<List<AimPropInfo>> ();
                _aimClassInfoPropDict.Add (aci, gci);

                FillGeoInfo (aci, geomTypes [i++]);
            }

            foreach (AimClassInfo aci in AimMetadata.AimClassInfoList)
            {
                if (aci.AimObjectType == AimObjectType.Object)
                {
                    _aimClassInfoPropDict.Remove (aci);
                }
                else if (aci.AimObjectType == AimObjectType.Feature)
                {
                    GeoClassInfo gci = _aimClassInfoPropDict [aci];
                    
                    if (gci.PointProps.Count == 0 &&
                        gci.CurveProps.Count == 0 &&
                        gci.SurfaceProps.Count == 0)
                    {
                        _aimClassInfoPropDict.Remove (aci);
                    }
                }
            }
            foreach (AimClassInfo aci in geomClassInfos)
            {
                _aimClassInfoPropDict.Remove (aci);
            }

            _geoClassInfoList = new List<GeoClassInfo> ();
            _geoClassInfoList.AddRange (_aimClassInfoPropDict.Values);
        }

        private static void FillGeoInfo (AimClassInfo propTypeClassInfo, AimGeomType geomType)
        {
            List<AimClassInfo> propTypeACIList = new List<AimClassInfo> ();
            propTypeACIList.Add (propTypeClassInfo);

            List<AimClassInfo> allAdded = new List<AimClassInfo> ();
            
            while (propTypeACIList.Count > 0)
            {
                List<AimClassInfo> newPropTypeACIList = new List<AimClassInfo> ();

                foreach (AimClassInfo aci in AimMetadata.AimClassInfoList)
                {
                    if (aci.AimObjectType != AimObjectType.Object &&
                        aci.AimObjectType != AimObjectType.Feature)
                    {
                        continue;
                    }

                    foreach (AimClassInfo propTypeACI in propTypeACIList)
                    {
                        foreach (AimPropInfo aciPropInfo in aci.Properties)
                        {
                            if (aciPropInfo.PropType == propTypeACI)
                            {
                                GeoClassInfo gci = _aimClassInfoPropDict [aci];
                                GeoClassInfo propGCI = _aimClassInfoPropDict [propTypeACI];

                                List<List<AimPropInfo>> props = propGCI.GetProps (geomType);

                                if (props.Count > 0)
                                {
                                    foreach (List<AimPropInfo> piList in props)
                                    {
                                        List<AimPropInfo> newPIList = new List<AimPropInfo> ();
                                        newPIList.Add (aciPropInfo);
                                        newPIList.AddRange (piList);
                                        gci.GetProps (geomType).Add (newPIList);
                                    }
                                }
                                else
                                {
                                    List<AimPropInfo> newPIList = new List<AimPropInfo> ();
                                    newPIList.Add (aciPropInfo);
                                    gci.GetProps (geomType).Add (newPIList);
                                }

                                if (!allAdded.Contains (aci))
                                {
                                    newPropTypeACIList.Add (aci);
                                    allAdded.Add (aci);
                                }
                            }
                        }
                    }
                }

                propTypeACIList = newPropTypeACIList;
            }
        }

        private static List<GeoClassInfo> _geoClassInfoList;
        private static Dictionary<AimClassInfo, GeoClassInfo> _aimClassInfoPropDict;
    }

    public enum AimGeomType
    {
        Point = ObjectType.AixmPoint,
        Curve = ObjectType.Curve,
        Surface = ObjectType.Surface
    }
}
