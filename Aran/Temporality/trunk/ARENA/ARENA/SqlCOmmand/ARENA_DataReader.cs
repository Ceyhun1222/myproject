using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.Data.SqlClient;
using System.Data.OleDb;
using System.Windows.Forms;
using PDM;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;

namespace ARENA
{
    public static class ARENA_DataReader
    {
        public static string PathToPDM_DB;


        #region  Airport

            public static DataTable Table_GetAirport(string ICAO_CODE)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AirportHeliport WHERE designator = '"+ICAO_CODE+"'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
   
            public static DataTable Table_GetAirportList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AirportHeliport WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetAirportList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AirportHeliport", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static int Table_DeleteAirport(string ID_Airport)
            {
                int res = -1;
                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM AirportHeliport WHERE FeatureGUID = '" + ID_Airport + "'", Connection);

                try
                {
                    Connection.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return res;
            }

            public static AirportHeliport GetAirport()
            {
                var res = (from element in ARENA.MainForm.Instance.Environment.Data.CurrentProject.PdmObjectList where (element != null) && (element is AirportHeliport) select element).FirstOrDefault();
                if (res.Geo == null) res.RebuildGeo();
                return (AirportHeliport)res;
            }

            public static AirportHeliport GetAirport(string AirportID)
            {
                var res = (from element in ARENA.MainForm.Instance.Environment.Data.CurrentProject.PdmObjectList where (element != null) && (element is AirportHeliport)  && (element.ID.CompareTo(AirportID) == 0) select element).FirstOrDefault();
                if (res.Geo == null) res.RebuildGeo();
                return (AirportHeliport)res;
            }

            public static List<AirportHeliport> GetAirportlist()
            {
                var lst = (from element in ARENA.MainForm.Instance.Environment.Data.CurrentProject.PdmObjectList where (element != null) && (element is AirportHeliport)  select element).ToList();
                List<AirportHeliport> res = new List<AirportHeliport>();
                foreach (var item in lst)
                {
                    if (item.Geo == null) item.RebuildGeo();
                    res.Add((AirportHeliport)item);
                }
                return res;
            }

        #endregion

        #region  Runway

            public static DataTable Table_GetRunway(string Designator)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Runway WHERE designator = '" + Designator + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static DataTable Table_GetRunwayList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Runway WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static DataTable Table_GetRunwayList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Runway", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

        #endregion

        #region  RunwayDirection

            public static DataTable Table_GetRunwayDirection(string Designator)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM RunwayDirection WHERE designator = '" + Designator + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static DataTable Table_GetRunwayDirectionList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM RunwayDirection WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static DataTable Table_GetRunwayDirectionList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM RunwayDirection", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

        #endregion

        #region  NavaidSystem

            public static DataTable Table_GetNavaidSystem(string Name)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM NavaidSystem WHERE name = '" + Name + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static DataTable Table_GetNavaidSystemList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM NavaidSystem WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static DataTable Table_GetNavaidSystemList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM NavaidSystem", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static List<DataTable> Table_GetNavaidEquipment(string ID_NavaidSystem)
            {
                List<DataTable> res = new List<DataTable>();

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");

                try
                {
                    Connection.Open();


                    #region VOR

                    DataTable dt = new DataTable();
                    OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VOR WHERE ID_NavaidSystem = '" + ID_NavaidSystem + "'", Connection);


                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "VOR";
                         if (dt.Columns.IndexOf("SHAPE")>=0) dt.Columns.Remove("SHAPE");
                        res.Add(dt);
                    }

                    #endregion

                    #region DME

                    dt = new DataTable();
                    cmd = new OleDbCommand(" SELECT * FROM DME WHERE ID_NavaidSystem = '" + ID_NavaidSystem + "'", Connection);


                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "DME";
                         if (dt.Columns.IndexOf("SHAPE")>=0) dt.Columns.Remove("SHAPE");
                        res.Add(dt);
                    }

                    #endregion

                    #region TACAN

                    dt = new DataTable();
                    cmd = new OleDbCommand(" SELECT * FROM TACAN WHERE ID_NavaidSystem = '" + ID_NavaidSystem + "'", Connection);


                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "TACAN";
                         if (dt.Columns.IndexOf("SHAPE")>=0) dt.Columns.Remove("SHAPE");
                        res.Add(dt);
                    }


                    #endregion

                    #region NDB

                    dt = new DataTable();
                    cmd = new OleDbCommand(" SELECT * FROM NDB WHERE ID_NavaidSystem = '" + ID_NavaidSystem + "'", Connection);


                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "NDB";
                         if (dt.Columns.IndexOf("SHAPE")>=0) dt.Columns.Remove("SHAPE");
                        res.Add(dt);
                    }


                    #endregion

                    #region LLZ

                    dt = new DataTable();
                    cmd = new OleDbCommand(" SELECT * FROM Localizer WHERE ID_NavaidSystem = '" + ID_NavaidSystem + "'", Connection);


                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "Localizer";
                         if (dt.Columns.IndexOf("SHAPE")>=0) dt.Columns.Remove("SHAPE");
                        res.Add(dt);
                    }


                    #endregion

                    #region GP

                    dt = new DataTable();
                    cmd = new OleDbCommand(" SELECT * FROM GlidePath WHERE ID_NavaidSystem = '" + ID_NavaidSystem + "'", Connection);


                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "GlidePath";
                         if (dt.Columns.IndexOf("SHAPE")>=0) dt.Columns.Remove("SHAPE");
                        res.Add(dt);
                    }


                    #endregion

                    #region Marker

                    dt = new DataTable();
                    cmd = new OleDbCommand(" SELECT * FROM MarkerBeacon WHERE ID_NavaidSystem = '" + ID_NavaidSystem + "'", Connection);


                    dt.Load(cmd.ExecuteReader());
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "MarkerBeacon";
                        if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                        res.Add(dt);
                    }


                    #endregion
                }
                catch (Exception)
                {
                   
                }
                finally
                {
                    Connection.Close();
                }


                return res;
                

            }

            #region VOR

                public static DataTable GetVORList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VOR", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            #endregion

            #region DME

                public static DataTable GetDMEList()
                {

                    OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                    DataTable dt = new DataTable();
                    OleDbCommand cmd = new OleDbCommand(" SELECT * FROM DME", Connection);

                    try
                    {
                        Connection.Open();
                        dt.Load(cmd.ExecuteReader());
                        if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                    }
                    catch (Exception)
                    {
                        dt = null;
                    }
                    finally
                    {
                        Connection.Close();
                    }


                    return dt;

                }
            
            #endregion

            #region TACAN

                public static DataTable GetTACANList()
                {

                    OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                    DataTable dt = new DataTable();
                    OleDbCommand cmd = new OleDbCommand(" SELECT * FROM TACAN", Connection);

                    try
                    {
                        Connection.Open();
                        dt.Load(cmd.ExecuteReader());
                        if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                    }
                    catch (Exception)
                    {
                        dt = null;
                    }
                    finally
                    {
                        Connection.Close();
                    }


                    return dt;

                }
            
            #endregion

            #region LLZ
            #endregion

            #region GP
            #endregion

            #region NDB 

                public static DataTable GetNDBList()
                {

                    OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                    DataTable dt = new DataTable();
                    OleDbCommand cmd = new OleDbCommand(" SELECT * FROM NDB", Connection);

                    try
                    {
                        Connection.Open();
                        dt.Load(cmd.ExecuteReader());
                        if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                    }
                    catch (Exception)
                    {
                        dt = null;
                    }
                    finally
                    {
                        Connection.Close();
                    }


                    return dt;

                }

            #endregion

            #region MarkerBeacon

                public static DataTable GetMarkerBeaconList()
                {

                    OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                    DataTable dt = new DataTable();
                    OleDbCommand cmd = new OleDbCommand(" SELECT * FROM MarkerBeacon", Connection);

                    try
                    {
                        Connection.Open();
                        dt.Load(cmd.ExecuteReader());
                        if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                    }
                    catch (Exception)
                    {
                        dt = null;
                    }
                    finally
                    {
                        Connection.Close();
                    }


                    return dt;

                }

            #endregion

        #endregion

        #region  Waypoints

            public static DataTable Table_GetWaypoint(string Designator)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM WayPoint WHERE designator = '" + Designator + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetWaypointList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM WayPoint WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetWaypointList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM WayPoint", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

       #endregion

        #region Routes

            public static DataTable Table_GetRoute(string RouteName)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Enroute WHERE designator = '" + RouteName + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetRouteList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Enroute WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetRouteList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Enroute", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static int Table_DeleteRoute(string RouteID)
            {
                int res = -1;
                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM Enroute WHERE FeatureGUID = '" + RouteID + "'", Connection);

                try
                {
                    Connection.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return res;
            }


        #endregion

        #region RouteSegment

            public static DataTable Table_GetRouteSegment(string RouteSegmentName)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM RouteSegment WHERE designator = '" + RouteSegmentName + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetRouteSegmentList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM RouteSegment WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetRouteSegmentList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM RouteSegment", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static int Table_DeleteRouteSegment(string RouteSegmentID)
            {
                int res = -1;
                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM RouteSegment WHERE FeatureGUID = '" + RouteSegmentID + "'", Connection);

                try
                {
                    Connection.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return res;
            }

            public static DataTable Table_GetRoutesStartEndPointsList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM SegmentPoint where role = 'ENRT'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }


        #endregion

        #region Airspace

            public static DataTable Table_GetAirspace(string codeId)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Airspace WHERE codeId = '" + codeId + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

               return dt;

            }

            public static DataTable Table_GetAirspaceList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Airspace WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static DataTable Table_GetAirspaceList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Airspace", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static int Table_DeleteAirspace(string ID_Airspace)
            {
                int res = -1;
                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM Airspace WHERE FeatureGUID = '" + ID_Airspace + "'", Connection);

                try
                {
                    Connection.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return res;
            }

        #endregion

        #region AirspaceVolume

            public static DataTable Table_GetAirspaceVolume(string codeId)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AirspaceVolume WHERE codeId = '" + codeId + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetAirspaceVolumeList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AirspaceVolume WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetAirspaceVolumeList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AirspaceVolume", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static int Table_DeleteAirspaceVolume(string ID_Airspace)
            {
                int res = -1;
                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM AirspaceVolume WHERE FeatureGUID = '" + ID_Airspace + "'", Connection);

                try
                {
                    Connection.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return res;
            }

       #endregion

        #region Procedures
            //Procedure
            public static DataTable Table_GetProcedure(string ProcedureIdentifier)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Procedures WHERE ProcedureIdentifier = '" + ProcedureIdentifier + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Procedures WHERE " + clause + " ORDER BY ProcedureType", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM Procedures", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
            public static DataTable Table_GetProcedureList(PROC_TYPE_code ProcType)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM IAP_View", Connection);

                if (ProcType == PROC_TYPE_code.SID) cmd = new OleDbCommand(" SELECT * FROM SID_View", Connection);
                if (ProcType== PROC_TYPE_code.STAR) cmd = new OleDbCommand(" SELECT * FROM STAR_View", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }


            //ProcedureTransitions
            public static DataTable Table_GetProcedureTransitions(string transitionID)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM ProcedureTransitions WHERE transitionID = '" + transitionID + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureTransitionsList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM ProcedureTransitions WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureTransitionsList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM ProcedureTransitions", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
            public static DataTable Table_GetProcedureTransitionsList(PROC_TYPE_code ProcType)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM IAP_Transition", Connection);
                if (ProcType == PROC_TYPE_code.SID) cmd = new OleDbCommand(" SELECT * FROM SID_Transition", Connection);
                if (ProcType == PROC_TYPE_code.STAR) cmd = new OleDbCommand(" SELECT * FROM STAR_Transition", Connection);



                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            //GetProcedureLeg
            public static DataTable Table_GetProcedureLeg(string FeatureGUID)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM ProcedureLegs WHERE FeatureGUID = '" + FeatureGUID + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureLegsList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM ProcedureLegs WHERE " + clause + " ORDER BY seqNumberARINC", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureLegsList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM ProcedureLegs ORDER BY seqNumberARINC", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
            public static DataTable Table_GetProcedureLegsList(PROC_TYPE_code ProcType)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM IAP_ProcedureLeg_View ORDER BY seqNumberARINC", Connection);
                if (ProcType == PROC_TYPE_code.SID) cmd = new OleDbCommand(" SELECT * FROM SID_ProcedureLeg_View", Connection);
                if (ProcType== PROC_TYPE_code.STAR) cmd = new OleDbCommand(" SELECT * FROM STAR_ProcedureLeg_View", Connection);


                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
 

            //SegmentPoint
            public static DataTable Table_GetSegmentPoints(string FeatureGUID)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM SegmentPoint WHERE FeatureGUID = '" + FeatureGUID + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureSegmentPointList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM SegmentPoint WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureSegmentPointList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM SegmentPoint", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
            public static DataTable Table_GetProcedureSegmentPointList(PROC_TYPE_code ProcType)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM IAP_SegmentPoint", Connection);
                if (ProcType == PROC_TYPE_code.SID) cmd = new OleDbCommand(" SELECT * FROM SID_SegmentPoint", Connection);
                if (ProcType == PROC_TYPE_code.STAR) cmd = new OleDbCommand(" SELECT * FROM STAR_SegmentPoint", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }


            //FacilityMakeUp
            public static DataTable Table_GetFacilityMakeUp(string FeatureGUID)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM FacilityMakeUp WHERE FeatureGUID = '" + FeatureGUID + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureFacilityMakeUpList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM FacilityMakeUp WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureFacilityMakeUptList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM FacilityMakeUp", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
            public static DataTable Table_GetProcedureFacilityMakeUptList(PROC_TYPE_code ProcType)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM IAP_FacilityMakeUp", Connection);
                if (ProcType== PROC_TYPE_code.SID) cmd = new OleDbCommand(" SELECT * FROM SID_FacilityMakeUp", Connection);
                if (ProcType == PROC_TYPE_code.STAR) cmd = new OleDbCommand(" SELECT * FROM STAR_FacilityMakeUp", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }


            //FacilityAngle
            public static DataTable Table_GetProcedureFacilityAngleList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AngleIndication WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureFacilityAngleList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM AngleIndication", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
            public static DataTable Table_GetProcedureFacilityAngleList(PROC_TYPE_code ProcType)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM IAP_AngleIndication", Connection);
                if (ProcType == PROC_TYPE_code.SID) cmd = new OleDbCommand(" SELECT * FROM SID_AngleIndication", Connection);
                if (ProcType == PROC_TYPE_code.STAR) cmd = new OleDbCommand(" SELECT * FROM STAR_AngleIndication", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            //FacilityDistamce
            public static DataTable Table_GetProcedureFacilityDistanceList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM DistanceIndication WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }
            public static DataTable Table_GetProcedureFacilityDistanceList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM DistanceIndication", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }
            public static DataTable Table_GetProcedureFacilityDistanceList(PROC_TYPE_code ProcType)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM IAP_DistInd", Connection);
                if (ProcType == PROC_TYPE_code.SID) cmd = new OleDbCommand(" SELECT * FROM SID_DistInd", Connection);
                if (ProcType == PROC_TYPE_code.STAR) cmd = new OleDbCommand(" SELECT * FROM STAR_DistInd", Connection);
                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }


            #endregion

        #region Obstacles

            public static DataTable Table_GetVerticalStructure(string FeatureGUID)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VerticalStructure WHERE FeatureGUID = '" + FeatureGUID + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetVerticalStructureList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VerticalStructure WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetVerticalStructureList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VerticalStructure", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static int Table_DeleteVerticalStructure(string FeatureGUID)
            {
                int res = -1;
                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM VerticalStructure WHERE FeatureGUID = '" + FeatureGUID + "'", Connection);

                try
                {
                    Connection.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return res;
            }


            public static DataTable Table_GetVerticalStructurePart(string FeatureGUID)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VerticalStructurePart WHERE FeatureGUID = '" + FeatureGUID + "'", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetVerticalStructurePartList(string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VerticalStructurePart WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }



                return dt;

            }

            public static DataTable Table_GetVerticalStructurePartList()
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM VerticalStructurePart", Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return dt;

            }

            public static int Table_DeleteVerticalStructurePart(string FeatureGUID)
            {
                int res = -1;
                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM VerticalStructurePart WHERE FeatureGUID = '" + FeatureGUID + "'", Connection);

                try
                {
                    Connection.Open();
                    res = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }

                return res;
            }

        #endregion


            public static List<PDMObject> GetObjectsWithinPolygon(IPolygon Poly, Type pdmObjectType )
            {
                List<PDMObject> res = new List<PDMObject>();
                AranSupport.Utilitys util = new AranSupport.Utilitys();

                var pdmList = (from element in ARENA.MainForm.Instance.Environment.Data.CurrentProject.PdmObjectList where (element != null) && (element.GetType().Equals(pdmObjectType)) select element).ToList();

                foreach (var item in pdmList)
                {

                    if (pdmObjectType.Equals(typeof(NavaidSystem)))
                    {
                        foreach (var navEq in ((NavaidSystem)item).Components)
                        {
                            if (navEq.Geo == null) navEq.RebuildGeo();
                            if (util.WithinPolygon(Poly, navEq.Geo))
                            {
                                res.Add(item);
                                break;
                            }
                        }
                    }
                    else if (pdmObjectType.Equals(typeof(VerticalStructure)))
                    {
                        foreach (var part in ((VerticalStructure)item).Parts)
                        {
                            if (part.Geo == null) part.RebuildGeo();
                            if (util.WithinPolygon(Poly, part.Geo))
                            {
                                res.Add(item);
                                break;
                            }
                        }
                    }
                    else
                    {
                       if (item.Geo == null) item.RebuildGeo();
                       if (util.WithinPolygon(Poly, item.Geo)) res.Add(item);
                    }
                        
                }
                
                return res;
            }

            public static DataTable GetTable(string _sqlCommand)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(_sqlCommand, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());

                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");

                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static DataTable GetTableByName(string _tableName)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" SELECT * FROM " + _tableName, Connection);

                try
                {
                    Connection.Open();
                    dt.Load(cmd.ExecuteReader());
                    if (dt.Columns.IndexOf("SHAPE") >= 0) dt.Columns.Remove("SHAPE");
                }
                catch (Exception)
                {
                    dt = null;
                }
                finally
                {
                    Connection.Close();
                }


                return dt;

            }

            public static void hideColumns(DataGridView dgw)
            {

                string[] colnames = { "OBJECTID", "FeatureGUID", "ID_Runway", "ID_NavaidSystem", "ID_AirportHeliport", "ID_RunwayDirection", "ID_NavaidComponent", "ID_Enroute", "SHAPE_Length", "AirspaceID", "SHAPE_Area", "AirportIdentifier", 
                                    "ID_procedure", "transitionID", "ID_Transition", "ProcedureLegID", "SegmentPointID", "facilityMakeUp_ID", "VerticalStructure_ID", "VisibilityFlag","StartSegmentPointID","EndSegmentPointID","Route_LEG_ID",
                                    "valDistVerLower_M","valDistVerUpper_M","LatCoord","LonCoord"};


                foreach (DataGridViewColumn clmn in dgw.Columns)
                {
                    if (Array.IndexOf(colnames, clmn.Name) >= 0) clmn.Visible = false;
                    if (clmn.Name.Contains("FeatureGUID")) clmn.Visible = false;
                    if (clmn.Name.Contains("ID")) clmn.Visible = false;
                }

            }

            public static void showColumns(DataGridView dgw, string[] colnames)
            {

                foreach (DataGridViewColumn clmn in dgw.Columns)
                {
                    clmn.Visible = false;
                    if (Array.IndexOf(colnames, clmn.Name) >= 0) clmn.Visible = true;
                    
                }

            }

            public static IGeometry GetObjectGeometry(PDMObject objPDM)
            {
                IGeometry res = null;
                IFeatureCursor cursor = null; ;

                ILayer lyr = ARENA.MainForm.Instance.Environment.Data.GetLinkedLayer(objPDM);
                if (lyr != null)
                {
                    IFeatureClass _fClass = (lyr as IFeatureLayer).FeatureClass;

                    ISpatialFilter filter = new SpatialFilterClass();

                    filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    filter.WhereClause = "FeatureGUID = '" + objPDM.ID + "'";

                    cursor = _fClass.Search(filter, true);

                        IFeature selFeature = cursor.NextFeature();


                        if (selFeature != null)
                        {
                            res = selFeature.Shape;
                        }
                }


                Marshal.ReleaseComObject(cursor);
                return res;
            }

            public static void DeleteObject(ITable Tbl, PDMObject objPDM)
            {
                if (Tbl != null)
                {
                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "FeatureGUID = '" + objPDM.ID + "'";

                    // Create and manage a cursor.
                    ICursor updateCursor = Tbl.Update(queryFilter, false);

                    // Delete the retrieved features.
                    IRow feature = null;
                    while ((feature = updateCursor.NextRow()) != null)
                    {
                        updateCursor.DeleteRow();
                    }


                    Marshal.ReleaseComObject(updateCursor);
                }


            }

            public static void DeleteObject(string Tblname, string clause)
            {

                OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");


                DataTable dt = new DataTable();
                OleDbCommand cmd = new OleDbCommand(" DELETE * FROM " + Tblname + " WHERE " + clause, Connection);

                try
                {
                    Connection.Open();
                    cmd.ExecuteNonQuery();
                   
                }
                catch (Exception)
                {
                    
                }
                finally
                {
                    Connection.Close();
                }



            }

        


        

    }
}
