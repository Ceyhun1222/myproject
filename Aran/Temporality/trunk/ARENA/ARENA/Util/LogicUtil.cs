using System;
using PDM;
using System.Collections.Generic;
using System.Data;

namespace ARENA.Util
{
    public class LogicUtil
    {
        public static string GetPdmObjectLinkedWithLayer(string layerName)
        {
            var res = "";
            switch (layerName)
            {
                case ("AirportHeliport"):
                    res = "AirportHeliport";
                    break;
                case ("RunwayDirection"):
                    res = "RunwayDirection";
                    break;
                case ("GlidePath"):
                    res = "GlidePath";
                    break;
                case ("Localizer"):
                    res = "Localizer";
                    break;
                case ("VOR"):
                    res = "VOR";
                    break;
                case ("DME"):
                    res = "DME";
                    break;
                case ("NDB"):
                    res = "NDB";
                    break;
                case ("TACAN"):
                    res = "TACAN";
                    break;
                case ("WayPoint"):
                    res = "WayPoint";
                    break;
                case ("Enroute"):
                case ("RouteSegment"):
                    res = "RouteSegment";
                    break;
                case ("AirspaceVolume"):
                    res = "AirspaceVolume";
                    break;
            }

            return res;
        }

        public static void FillGeo(PDMObject sellObj)
        {
            try
            {
                var tp = sellObj.GetType().Name;
                switch (tp)
                {
                    case ("AirportHeliport"):
                    case ("RunwayDirection"):
                    case ("VOR"):
                    case ("DME"):
                    case ("NDB"):
                    case ("TACAN"):
                    case ("Localizer"):
                    case ("GlidePath"):
                    case ("RouteSegment"):
                    case ("WayPoint"):
                    case ("AirspaceVolume"):
                    case ("RunwayCenterLinePoint"):
                    case ("VerticalStructurePart"):
                    case ("Marker"):
                        sellObj.RebuildGeo();
                        break;
                    case("NavaidSystem"):
                        foreach (PDMObject comp in ((NavaidSystem) sellObj).Components)
                        {
                            comp.RebuildGeo();
                        }

                        break;
                    case("Enroute"):
                        foreach (var rte in ((Enroute) sellObj).Routes)
                        {
                            rte.RebuildGeo();
                        }
                        break;
                    //case("Airspace"):
                    //    foreach (var vol in ((Airspace) sellObj).AirspaceVolumeList)
                    //    {
                    //        if (vol.Geo == null) vol.RebuildGeo();
                    //    }
                    //    break;
                    case ("Airspace"):
                        foreach (var vol in ((Airspace)sellObj).AirspaceVolumeList)
                        {
                            if (vol.Geo == null) vol.Geo = ARENA_DataReader.GetObjectGeometry(vol);
                        }
                        break;
                    case ("VerticalStructure"):
                        foreach (var prt in ((VerticalStructure)sellObj).Parts)
                        {
                            //if (prt.Geo == null) prt.RebuildGeo();
                            if (prt.Geo == null) prt.Geo = ARENA_DataReader.GetObjectGeometry(prt);
                        }
                        break;

                    case ("InstrumentApproachProcedure"):
                    case ("StandardInstrumentArrival"):
                    case ("StandardInstrumentDeparture"):
                        foreach (var _trans in ((Procedure)sellObj).Transitions)
                        {
                            foreach (var _leg in _trans.Legs)
                            {
                                //_leg.RebuildGeo();
                                if (_leg.Geo == null) _leg.Geo = ARENA_DataReader.GetObjectGeometry(_leg);
                            }
                        }

                        break;

                    case ("ProcedureTransitions"):
                        foreach (var _leg in ((ProcedureTransitions)sellObj).Legs)
                        {
                           // _leg.RebuildGeo();
                            if (_leg.Geo == null) _leg.Geo = ARENA_DataReader.GetObjectGeometry(_leg);
                        }
                        break;

                    case("FinalLeg"):
                    case ("ProcedureLeg"):
                    case ("MissaedApproachLeg"):
                    case("FacilityMakeUp"):
                        //sellObj.RebuildGeo();
                        if (sellObj.Geo == null) sellObj.Geo = ARENA_DataReader.GetObjectGeometry(sellObj);
                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void RemoveFeature(PDMObject sellObj)
        {
            try
            {
                ARENA_DataReader.DeleteObject(ARENA.MainForm.Instance.Environment.Data.TableDictionary[sellObj.GetType()], sellObj);
                var tp = sellObj.GetType().Name;
                switch (tp)
                {
                    case ("InstrunentApproachProcedure"):
                    case ("StandardInstrumentArrival"):
                    case ("StandardInstrumentDeparture"):
                        ARENA_DataReader.DeleteObject(ARENA.MainForm.Instance.Environment.Data.TableDictionary[typeof(Procedure)], sellObj);

                        break;
                }

            }
            catch (Exception)
            {
                throw;
            }

             

        }

        public static void RemoveFeature(string tblName, string keyField, string keyValue)
        {
            try
            {
                ARENA_DataReader.DeleteObject(tblName, keyField + " = '" +keyValue+"'");

            }
            catch (Exception)
            {
                throw;
            }



        }

    }
}
