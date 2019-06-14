using System;
using System.Collections.Generic;
using PDM;
using System.IO;
using System.Xml.Serialization;
using ARENA.Enums_Const;
using ArenaStatic;
using System.Data.OleDb;
using ARENA;
using ArenaLogManager;

namespace DataModule
{
    public static class ArenaDataModule
    {
        public static List<PDMObject> GetObjectsFromPdmFile(string fileName, ref ArenaProjectType ProjectType)
        {
            string vInfo = "";
            List<PDMObject> res = new List<PDMObject>();
            string[] FN = Directory.GetFiles(fileName, "*.pdm");
            if (FN.Length == 0) FN = Directory.GetFiles(fileName, "*.obj");
            for (int i = FN.Length; i > 0; i--)
            {
                string _file = FN.Length>0 ?  FN[i - 1] : FN[0];
                var fs = new System.IO.FileStream(_file, FileMode.Open);
                var byteArr = new byte[fs.Length];
                fs.Position = 0;
                var count = fs.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    fs.Close();
                    Console.WriteLine(@"Test Failed: Unable to read data from file");
                }


                var strmMemSer = new MemoryStream();
                strmMemSer.Write(byteArr, 0, byteArr.Length);
                strmMemSer.Position = 0;

                try
                {

                
                var xmlSer = new XmlSerializer(typeof(PDM_ObjectsList));

                var prj = (PDM_ObjectsList)xmlSer.Deserialize(strmMemSer);
                if ((prj.PDMObject_list != null) && (prj.PDMObject_list.Count > 0)) res.AddRange(prj.PDMObject_list);

                

                if ((prj.ProjectType != null) && (prj.ProjectType.Length > 0)) ProjectType = (ArenaProjectType)Enum.Parse(typeof(ArenaProjectType), prj.ProjectType, true);
                if ((prj.VersionInfo != null) && (prj.VersionInfo.Length > 0))
                    vInfo = prj.VersionInfo;

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                fs.Close();
                strmMemSer.Close();
                fs.Dispose();
                strmMemSer.Dispose();
            }

            if (FN.Length >0) ArenaStaticProc.SetTargetDB(fileName);


            if (vInfo != string.Empty && System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().CompareTo(vInfo) != 0)
            {
                ArenaMessageForm msgFrm = new ArenaMessageForm("ARENA", "The downloaded file has an older version", null);
                msgFrm.checkBox1.Visible = false;
                msgFrm.TopLevel = true;
                //msgFrm.ShowDialog();
            }
            return res;
        }


        public static void ClearRelations_Indexes(string PathToPDM_DB)
        {

            string[] OleDbCommands = {  "ALTER TABLE Runway DROP CONSTRAINT AirportHeliportRunway",
                                        "ALTER TABLE Procedures DROP CONSTRAINT AirportHeliportProcedures",
                                        "ALTER TABLE AirspaceVolume DROP CONSTRAINT AirspaceAirspaceVolume",
                                        "ALTER TABLE RoutePortion DROP CONSTRAINT EnrouteRoutePortion",
                                        "ALTER TABLE RouteSegment DROP CONSTRAINT EnrouteRouteSegment",
                                        "ALTER TABLE AngleIndication DROP CONSTRAINT FacilityMakeUpAngleIndication",
                                        "ALTER TABLE DistanceIndication DROP CONSTRAINT FacilityMakeUpDistanceIndication",
                                        "ALTER TABLE ApproachCondition_Minima DROP CONSTRAINT FinalLegApproachCondition_Minima",
                                        "ALTER TABLE DME DROP CONSTRAINT NavaidSystemDME",
                                        "ALTER TABLE GlidePath DROP CONSTRAINT NavaidSystemGlidePath",
                                        "ALTER TABLE Localizer DROP CONSTRAINT NavaidSystemLocalizer",
                                        "ALTER TABLE MarkerBeacon DROP CONSTRAINT NavaidSystemMarkerBeacon",
                                        "ALTER TABLE NDB DROP CONSTRAINT NavaidSystemNDB",
                                        "ALTER TABLE TACAN DROP CONSTRAINT NavaidSystemTACAN",
                                        "ALTER TABLE VOR DROP CONSTRAINT NavaidSystemVOR",
                                        "ALTER TABLE Obstruction DROP CONSTRAINT ObstacleAssessmentAreaObstruction",
                                        "ALTER TABLE ObstacleAssessmentArea DROP CONSTRAINT ProcedureLegsObstacleAssessmentArea",
                                        "ALTER TABLE ProcedureTransitions DROP CONSTRAINT ProceduresProcedureTransitions",
                                        "ALTER TABLE ProcedureLegs DROP CONSTRAINT ProcedureTransitionsProcedureLegs",
                                        "ALTER TABLE ChangeOverPoint DROP CONSTRAINT RoutePortionChangeOverPoint",
                                        "ALTER TABLE RunwayDirectionCenterLinePoint DROP CONSTRAINT RunwayDirectionRunwayDirectionCenterLinePoint",
                                        "ALTER TABLE RunwayDirection DROP CONSTRAINT RunwayRunwayDirection",
                                        "ALTER TABLE VerticalStructure_Curve DROP CONSTRAINT VerticalStructurePartVerticalStructure_Curve",
                                        "ALTER TABLE VerticalStructure_Point DROP CONSTRAINT VerticalStructurePartVerticalStructure_Point",
                                        "ALTER TABLE VerticalStructure_Surface DROP CONSTRAINT VerticalStructurePartVerticalStructure_Surface",
                                        "ALTER TABLE VerticalStructurePart DROP CONSTRAINT VerticalStructureVerticalStructurePart",
                                        "ALTER TABLE SafeAreaSector DROP CONSTRAINT SafeAreaSafeAreaSector",
                                        "ALTER TABLE GroundLightSystem DROP CONSTRAINT RunwayDirectionGroundLightSystem",
                                        "ALTER TABLE LightElement DROP CONSTRAINT GroundLightSystemLightElement",
                                        //"ALTER TABLE LightElement DROP CONSTRAINT LightSystemLightElement",
                                        "DROP INDEX FeatureGUID ON AirportHeliport",
                                        "DROP INDEX FeatureGUID ON Airspace",
                                        "DROP INDEX FeatureGUID ON AirspaceVolume",
                                        //"DROP INDEX FeatureGUID ON AMEA",
                                        "DROP INDEX FeatureGUID ON AngleIndication",
                                        //"DROP INDEX FeatureGUID ON Area",
                                        "DROP INDEX FeatureGUID ON ChangeOverPoint",
                                        " DROP INDEX FeatureGUID ON DistanceIndication",
                                        " DROP INDEX FeatureGUID ON DME",
                                        " DROP INDEX FeatureGUID ON Enroute",
                                        " DROP INDEX FeatureGUID ON FacilityMakeUp",
                                        " DROP INDEX FeatureGUID ON FinalLeg",
                                        " DROP INDEX FeatureGUID ON GlidePath",
                                        //" DROP INDEX FeatureGUID ON HoldingPattern",
                                        " DROP INDEX FeatureGUID ON InstrumentApproachProcedure ",
                                        " DROP INDEX FeatureGUID ON Localizer",
                                        " DROP INDEX FeatureGUID ON MarkerBeacon",
                                        " DROP INDEX FeatureGUID ON MissaedApproachLeg",
                                        " DROP INDEX FeatureGUID ON NavaidSystem",
                                        " DROP INDEX FeatureGUID ON NDB",
                                        " DROP INDEX FeatureGUID ON ObstacleAssessmentArea",
                                        " DROP INDEX FeatureGUID ON Obstruction",
                                        " DROP INDEX FeatureGUID ON ProcedureLegs",
                                        " DROP INDEX FeatureGUID ON Procedures",
                                        " DROP INDEX FeatureGUID ON ProcedureTransitions",
                                        " DROP INDEX FeatureGUID ON RoutePortion",
                                        " DROP INDEX FeatureGUID ON RouteSegment",
                                        " DROP INDEX FeatureGUID ON Runway",
                                        " DROP INDEX FeatureGUID ON RunwayDirection",
                                        " DROP INDEX FeatureGUID ON RunwayDirectionCenterLinePoint",
                                        " DROP INDEX FeatureGUID ON SegmentPoint",
                                        " DROP INDEX FeatureGUID ON StandardInstrumentArrival",
                                        " DROP INDEX FeatureGUID ON StandardInstrumentDeparture",
                                        " DROP INDEX FeatureGUID ON TACAN",
                                        " DROP INDEX FeatureGUID ON VerticalStructure",
                                        " DROP INDEX FeatureGUID ON VerticalStructure_Curve",
                                        " DROP INDEX FeatureGUID ON VerticalStructure_Point",
                                        " DROP INDEX FeatureGUID ON VerticalStructure_Surface",
                                        " DROP INDEX FeatureGUID ON VerticalStructurePart",
                                        " DROP INDEX FeatureGUID ON VOR",
                                        " DROP INDEX FeatureGUID ON WayPoint",
                                        " DROP INDEX FeatureGUID ON SafeArea",
                                        " DROP INDEX FeatureGUID ON SafeAreaSector",
                                        " DROP INDEX FeatureGUID ON GroundLightSystem",
                                        " DROP INDEX FeatureGUID ON LightElement",
                                     };

            
            OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");

            try
            {
                Connection.Open(); 
                foreach (var cmds_item in OleDbCommands)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine(cmds_item);
                        OleDbCommand cmd = new OleDbCommand(cmds_item, Connection);
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "OleDbCommand Error. commandText: " + cmds_item + " connection: " + Connection);
                        continue; 
                    }
                }
               

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "OleDbConnection Error. connection: " + Connection);
            }
            finally
            {
                Connection.Close();
            }



        }


        public static void ExecuteSqlCommand(string PathToPDM_DB, string _CommandText)
        {

            OleDbConnection Connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathToPDM_DB + ";Persist Security Info=False");

            try
            {
                Connection.Open();

                try
                {
                    //System.Diagnostics.Debug.WriteLine(_CommandText);
                    OleDbCommand cmd = new OleDbCommand(_CommandText, Connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger(ex.TargetSite.Name)
                .Error(ex, "OleDbCommand Error. commandText: " + _CommandText + " connection: " + Connection);
                }



            }
            catch (Exception ex)
            {
                LogManager.GetLogger(ex.TargetSite.Name)
                    .Error(ex, "OleDbConnection Error. connection: " + Connection);
            }
            finally
            {
                Connection.Close();
            }



        }



    }
}
