using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using ARINC_Types;
using System.Xml.Serialization;
//using Process.Loader;
using AreaManager;
using Microsoft.Win32;
using System.Collections;
using System.IO.Compression;
using System.ComponentModel;
namespace ARINC_DECODER_CORE
{
    public static class Static_Proc
    {
        public static bool ShowProgressBar = false;
        public static string PathToSpecificationFile;
        //delegate void ProgressDelegate(string sMessage);

        public class SectionCounter
        {
            private string _section;

            public string Section
            {
                get { return _section; }
                set { _section = value; }
            }

            private int _sectionCount;

            public int SectionCount
            {
                get { return _sectionCount; }
                set { _sectionCount = value; }
            }

            private string _desc;

            public string Desc
            {
                get { return _desc; }
                set { _desc = value; }
            }

            public SectionCounter(string sec, string description)
            {
                this.Section = sec;
                this.SectionCount = 1;
                this.Desc = description;
            }
        }
  
        public static List<ARINC_OBJECT> DecodeARINCFile(string[] ARINC_FILE, List<string> ListOfSectionsType, string ADHP_ICAO_CODE)
        {
            ARINC_OBJECT ArincOBJ = null;
            List<ARINC_OBJECT> ARINC_OBJECT_LIST = new List<ARINC_OBJECT>();

            FileStream ArincSpecificationDoc_FILE = new FileStream(PathToSpecificationFile, FileMode.Open);

            XmlDocument ArincSpecificationDoc_XML = new XmlDocument();
            ArincSpecificationDoc_XML.Load(ArincSpecificationDoc_FILE);

            //System.Diagnostics.Debug.WriteLine("DecodeARINCFile GetArea");

            AreaInfo Area = new AreaInfo();
            Area = AreaManager.AreaUtils.GetArea(GetPathToAreaFile());


            ARINC_FILE = (from line in ARINC_FILE.ToList() where line.StartsWith("S" + Area.Region) select line).ToArray();

            //string ARINC_Line = "";
            char[] arr;
            int cntr = 0;
            double LineCount = (ARINC_FILE.Length);
            int ContinuesRecordN = 0;
            int PrevContinuesRecordN = -1;

            //ProgressHandler.IsActive = ShowProgressBar;
            //ProgressHandler.Run("Please wait...", 0);


            #region debug version
            //string[] ADHP_LIST = new string[18] { "EKVG", "ENBR", "ENCN", "ENDU", "ENEV", "ENFG", "ENGM", "ENHD", "ENKR", "ENLI", "ENML", "ENNA", "ENOL", "ENRO", "ENRY", "ENTO", "ENVA", "ENZV" };
           
            #endregion

            Dictionary<string, List<SectionCounter>> SectionCounterDictionary = new Dictionary<string, List<SectionCounter>>();

            foreach (string ARINC_Line in ARINC_FILE)
            {
                try
                {
                    cntr++;

                    int Proc = Convert.ToInt32(cntr * 100 / LineCount);

                    //ProgressHandler.SetState("Please wait...", Proc);

                    arr = ARINC_Line.ToCharArray(0, ARINC_Line.Length - 1);


                    string CustomArea = ARINC_Line.Substring(ConstantValue.CustomArea - 1, ConstantValue.CustomArea_length);
                    if ((CustomArea.Trim().Length != 0) && (CustomArea.CompareTo(Area.Region) != 0)) continue;

                    string SectionType = arr[ConstantValue.SectionCodeColumn_Position - 1].ToString();
                    string SubSectionType = arr[ConstantValue.SubSectionCodeColumn_Position - 1].ToString();
                    if ((SectionType.CompareTo("P") == 0 && (SubSectionType.CompareTo(" ") == 0)))
                        SubSectionType = arr[ConstantValue.SubSectionCodeColumn_P_Position - 1].ToString();

                    if (SubSectionType.CompareTo(" ") == 0) SubSectionType = ".";

                    if ((ListOfSectionsType != null) && (!ListOfSectionsType.Contains(SectionType + SubSectionType))) continue;

                    XmlNodeList SectionsList = ArincSpecificationDoc_XML.SelectNodes("//section[@code='" + SectionType + "' and @subsection='" + SubSectionType + "']");

                    XmlNode NameNode = ArincSpecificationDoc_XML.SelectSingleNode("//section[@code='" + SectionType + "' and @subsection='" + SubSectionType + "']/name");
                    XmlNode TypeNode = ArincSpecificationDoc_XML.SelectSingleNode("//section[@code='" + SectionType + "' and @subsection='" + SubSectionType + "']/type");


                    for (int i = 0; i < SectionsList.Count; i++)
                    {

                        XmlNode MyNode = SectionsList[i];
                        if (MyNode != null)
                        {
                            int cNum = defineRecPosition(SectionType + SubSectionType);
                            ContinuesRecordN = Convert.ToInt32(ARINC_Line.Substring(cNum, 1));

                            if (((PrevContinuesRecordN > ContinuesRecordN) || (ContinuesRecordN == 0)) && (ArincOBJ != null))
                            {
                                string AirportIdentifier = Static_Proc.GetObjectValue(ArincOBJ, "Airport_ICAO_Identifier");

                                if ((AirportIdentifier == null) || (AirportIdentifier.Trim().Length == 0))
                                    AirportIdentifier = Static_Proc.GetObjectValue(ArincOBJ, "Airport_Identifier");


                                if ((AirportIdentifier == null) || (AirportIdentifier.Trim().Length == 0))
                                {
                                    ARINC_OBJECT_LIST.Add(ArincOBJ);
                                }
                                else
                                {
                                    if (AirportIdentifier.StartsWith(Area.FirstLetter))
                                    {
                                        ARINC_OBJECT_LIST.Add(ArincOBJ);

                                    }
                                }

                                //ARINC_OBJECT_LIST.Add(ArincOBJ);


                                #region debug
                                try
                                {
                                    if (AirportIdentifier == null) AirportIdentifier = Area.FirstLetter;

                                    if ((AirportIdentifier!=null)&& (SectionCounterDictionary.ContainsKey(AirportIdentifier)))
                                    {
                                        List<SectionCounter> sections = SectionCounterDictionary[AirportIdentifier];
                                        bool flag = false;
                                        foreach (SectionCounter sec in sections)
                                        {
                                            if (sec.Section.CompareTo(SectionType + SubSectionType) == 0)
                                            {
                                                sec.SectionCount++;
                                                flag = true;
                                                break;
                                            }
                                        }
                                        if (!flag)
                                        {
                                            SectionCounter sec = new SectionCounter(SectionType + SubSectionType, NameNode.InnerText);
                                            sections.Add(sec);
                                        }

                                    }
                                    else
                                    {
                                        List<SectionCounter> sections = new List<SectionCounter>();
                                        SectionCounter sec = new SectionCounter(SectionType + SubSectionType,NameNode.InnerText);
                                        sections.Add(sec);
                                        SectionCounterDictionary.Add(AirportIdentifier, sections);
                                    }
                                }

                                catch (Exception ex)
                                {
                                    //System.Diagnostics.Debug.WriteLine(ex.Message);
                                }

                                #endregion

                                if (ArincOBJ is ARINC_Airport_Primary_Record)
                                {
                                    ARINC_Airport_Primary_Record adhp = (ArincOBJ as ARINC_Airport_Primary_Record);
                                    if (adhp.Airport_ICAO_Identifier.StartsWith(ADHP_ICAO_CODE))
                                    {
                                        Area.Reference_ADHP.ICAO_CODE = ADHP_ICAO_CODE;
                                        Area.Reference_ADHP.Lat = adhp.Airport_Reference_Pt_Latitude;
                                        Area.Reference_ADHP.Lon = adhp.Airport_Reference_Pt_Longitude;

                                        AreaManager.AreaUtils.WriteToAreaFile(GetPathToRegionsFile(), GetPathToSpecificationFile(), Area);
                                    }
                                }

                            }
                            PrevContinuesRecordN = ContinuesRecordN;

                            if (ContinuesRecordN <= 1)// обработка основной записи
                            {
                                ArincOBJ = CreateArincObject(SectionType + SubSectionType);
                                ArincOBJ.Name = NameNode.InnerXml.ToString();
                                ArincOBJ.Object_Type = TypeNode.InnerXml.ToString();

                                if (ArincOBJ == null) continue;


                                ArincOBJ.CustomerArea = ARINC_Line.Substring(ConstantValue.AreaCodeColumn_Position - 1, ConstantValue.AreaCodeColumn_Length);

                                XmlNodeList RecordsList = MyNode.SelectNodes("//section[name='" + NameNode.InnerText + "' and type='" + TypeNode.InnerText + "']/specification/Primary/Record");

                                if ((RecordsList == null) || (RecordsList.Count == 0)) continue;

                                GetValueFromLine(RecordsList, ArincOBJ, ARINC_Line);

                            }
                            else // обработка "Continuation Records"
                            {
                                /* Continuation Records могут быть нескольких видов.
                                 * Вид лпределяется по значению arr[APPLICATION_TYPE]
                                 * если arr[APPLICATION_TYPE] =" " значит это просто Continuation Records
                                 * если arr[APPLICATION_TYPE] ="P" значит это Flight Planning
                                 * если arr[APPLICATION_TYPE] ="S" значит это Simulation
                                 * если arr[APPLICATION_TYPE] ="L" значит это Limitation*/
                                int apNum = defineAplicationTypeRecPosition(SectionType + SubSectionType);
                                string AplicationType = arr[apNum].ToString();

                                #region Continuation Records

                                if (AplicationType.ToString().CompareTo(" ") == 0)
                                {
                                    XmlNodeList ContinuationRecordsList = MyNode.SelectNodes("//section[name='" + NameNode.InnerText + "' and type='" + TypeNode.InnerText + "']/specification/Continuation/Record");
                                    if ((ContinuationRecordsList == null) || (ContinuationRecordsList.Count == 0)) continue;

                                    GetValueFromLine(ContinuationRecordsList, DefineRecordType(SectionType, SubSectionType, AplicationType, ArincOBJ), ARINC_Line);

                                }

                                #endregion

                                #region Simulation

                                else if (AplicationType.ToString().CompareTo("S") == 0)
                                {
                                    XmlNodeList SimulationContinuation = MyNode.SelectNodes("//section[name='" + NameNode.InnerText + "' and type='" + TypeNode.InnerText + "']/specification/SimulationContinuation/Record");
                                    if ((SimulationContinuation == null) || (SimulationContinuation.Count == 0)) continue;

                                    GetValueFromLine(SimulationContinuation, DefineRecordType(SectionType, SubSectionType, AplicationType, ArincOBJ), ARINC_Line);
                                }

                                #endregion

                                #region Flight Planning

                                else if (AplicationType.ToString().CompareTo("P") == 0)
                                {
                                    XmlNodeList SimulationContinuation = MyNode.SelectNodes("//section[name='" + NameNode.InnerText + "' and type='" + TypeNode.InnerText + "']/specification/FlightPlaningContinuation/Record");
                                    if ((SimulationContinuation == null) || (SimulationContinuation.Count == 0)) continue;

                                    GetValueFromLine(SimulationContinuation, DefineRecordType(SectionType, SubSectionType, AplicationType, ArincOBJ), ARINC_Line);
                                }

                                #endregion

                                #region Limitation

                                else if (AplicationType.ToString().CompareTo("L") == 0)
                                {
                                    XmlNodeList SimulationContinuation = MyNode.SelectNodes("//section[name='" + NameNode.InnerText + "' and type='" + TypeNode.InnerText + "']/specification/LimitationContinuation/Record");
                                    if ((SimulationContinuation == null) || (SimulationContinuation.Count == 0)) continue;

                                    GetValueFromLine(SimulationContinuation, DefineRecordType(SectionType, SubSectionType, AplicationType, ArincOBJ), ARINC_Line);
                                }
                                else continue;

                                #endregion

                            }


                        }
                    }

                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("ERROR");
                    ////System.Diagnostics.Debug.WriteLine(cntr.ToString());
                    continue;
                }

            }////



            ArincSpecificationDoc_FILE.Flush();
            ArincSpecificationDoc_FILE.Close();

            //ProgressHandler.Stop();



            if (SectionCounterDictionary.Count > 0)
            {
                List<string> tmp = new List<string>();

                tmp.Add("Country"+ (char)9+"Airport" + (char)9 +"Section decription" + (char)9 + "Section"+ (char)9 + "Count");
                foreach (KeyValuePair<string, List<SectionCounter>> pair in SectionCounterDictionary)
                {
                    //tmp.Add(pair.Key);
                    List<SectionCounter> val = pair.Value;
                    foreach (SectionCounter sec in val) tmp.Add(pair.Key.Substring(0,2)+ (char)9+ pair.Key + (char)9 + sec.Desc + (char)9 + sec.Section + (char)9 + sec.SectionCount);
                }

                string filePath = GetPathToARINCSpecificationFile() + @"\ARINC_ResultsInfo.txt";

                System.IO.File.WriteAllLines(filePath, tmp.ToArray());
            }


           
            return ARINC_OBJECT_LIST;
        }

        private static int defineRecPosition(string SectionType)
        {
            int res = ConstantValue.Continuation_Record_No - 1;

            switch (SectionType)
            {
                case ("PD"):
                case ("PE"):
                case ("PF"):
                case ("ER"):
                    res = ConstantValue.Proc_Continuation_Record_No - 1;
                    break;
                case ("UF"):
                    res = ConstantValue.FIR_Continuation_Record_No - 1;
                    break;
                case ("UR"):
                case ("UC"):
                    res = ConstantValue.Restrictive_Controlled_Continuation_Record_No - 1;
                    break;
                default:
                    res = ConstantValue.Continuation_Record_No - 1;
                    break;
            }

            return res;
        }

        private static int defineAplicationTypeRecPosition(string SectionType)
        {
            int res = ConstantValue.APPLICATION_TYPE - 1;

            switch (SectionType)
            {
                case ("PD"):
                case ("PE"):
                case ("PF"):
                case ("ER"):
                    res = ConstantValue.Proc_APPLICATION_TYPE - 1;
                    break;
                
                default:
                    res = ConstantValue.APPLICATION_TYPE - 1;
                    break;
            }

            return res;
        }

        private static ARINC_OBJECT CreateArincObject(string SectionType)
        {
            ARINC_OBJECT res = null;

            switch (SectionType)
            {
                case ("D."): 
                    res = new ARINC_Navaid_VHF_Primary_Record();
                    break;
                case ("DB"):
                case ("PN"): 
                    res = new ARINC_Navaid_NDB_Primary_Record();
                    break;
                case ("EA"):
                case ("PC"):
                    res = new ARINC_WayPoint_Primary_Record();
                    break;
                case ("PA"):
                    res = new ARINC_Airport_Primary_Record();
                    break;
                case ("PG"):
                    res = new ARINC_Runway_Primary_Records();
                    break;
                case ("PI"):
                    res = new ARINC_LocalizerGlideSlope_Primary_Record();
                    break;
                case ("PD"):
                case ("PE"):
                case ("PF"):
                    res = new ARINC_Terminal_Procedure_Primary_Record();
                    break;
                case ("ER"):
                    res = new ARINC_Enroute_Airways_Primary_Record();
                    break;
                case ("UC"):
                    res = new ARINC_Controlled_Airspace_Primary_Records();
                    break;
                case ("UF"):
                    res = new ARINC_FIR_UIR_Primary_Records();
                    break;
                case ("UR"):
                    res = new ARINC_Restrictive_Airspace_Primary_Records();
                    break;
                case ("PM"):
                    res = new ARINC_Airport_Marker();
                    break;
                default:
                    res = new ARINC_OBJECT();
                    break;
            }

            return res;
        }

        private static void GetValueFromLine(XmlNodeList RecordsList, ARINC_OBJECT ArincOBJ, string ARINC_LINE)
        {

            foreach (XmlNode RecordNode in RecordsList)
            {
                
                int Position = -1;
                int Len = -1;

                Int32.TryParse(RecordNode.Attributes["position"].Value, out Position);
                Int32.TryParse(RecordNode.Attributes["length"].Value, out Len);

                if ((Position == 0) || (Len == 0)) continue;


                string StoredValue = ARINC_LINE.Substring(Position - 1, Len);

                //if (RecordNode.SelectSingleNode("name").InnerXml.ToString().CompareTo("Airport_ICAO_Identifier") == 0)
                //{
                //    if (StoredValue.StartsWith("LF"))
                //    //System.Diagnostics.Debug.WriteLine("");
                //}

                //Static_Proc.SetObjectValue(ArincOBJ, RecordNode.SelectSingleNode("name").InnerXml.ToString(), StoredValue.Trim());

               
                Static_Proc.SetObjectValue(ArincOBJ, RecordNode.SelectSingleNode("name").InnerXml.ToString(), StoredValue);

                //StoredValue = RecordNode.SelectSingleNode("name").InnerXml.ToString() + " = " + StoredValue;

            }
        }

        private static ARINC_OBJECT DefineRecordType(string Section, string Subsection, string ApplicationType, ARINC_OBJECT ArincObj)
        {
            ARINC_OBJECT Res = ArincObj;

            switch (Section+Subsection+ApplicationType)
            {
                #region Navaid VHF

                    case ("D. "):
                        ARINC_Navaid_VHF_Continuation_Record VHF_CR = new ARINC_Navaid_VHF_Continuation_Record();
                        ((ARINC_Navaid_VHF_Primary_Record)ArincObj).Continuation_Record.Add(VHF_CR);
                        Res = VHF_CR;
                        break;
                
                    case ("D.S"):
                        ARINC_Navaid_VHF_Simulation_Continuation_Record VHF_SR = new ARINC_Navaid_VHF_Simulation_Continuation_Record();
                        ((ARINC_Navaid_VHF_Primary_Record)ArincObj).Simulation_Continuation_Record.Add(VHF_SR);
                        Res= VHF_SR;
                        break;

                    case ("D.P"):
                        ARINC_Navaid_VHF_Flight_Planing_Continuation_Record VHF_PR = new ARINC_Navaid_VHF_Flight_Planing_Continuation_Record();
                        ((ARINC_Navaid_VHF_Primary_Record)ArincObj).Flight_Planing_Continuation_Record.Add(VHF_PR);
                        Res = VHF_PR;
                        break;

                    case ("D.L"):
                        ARINC_Navaid_VHF_Limitation_Continuation_Record VHF_LR = new ARINC_Navaid_VHF_Limitation_Continuation_Record();
                        ((ARINC_Navaid_VHF_Primary_Record)ArincObj).Limitation_Continuation_Record.Add(VHF_LR);
                        Res = VHF_LR;
                        break;

                #endregion

                #region Navaid NDB

                    case ("DB "):
                    case ("PN "):
                        ARINC_Navaid_NDB_Continuation_Record NDB_CR = new ARINC_Navaid_NDB_Continuation_Record();
                        ((ARINC_Navaid_NDB_Primary_Record)ArincObj).Continuation_Record.Add(NDB_CR);
                        Res = NDB_CR;
                        break;

                    case ("DBS"):
                    case ("PNS"):
                        ARINC_Navaid_NDB_Simulation_Continuation_Record NDB_SR = new ARINC_Navaid_NDB_Simulation_Continuation_Record();
                        ((ARINC_Navaid_NDB_Primary_Record)ArincObj).Simulation_Continuation_Record.Add(NDB_SR);
                        Res = NDB_SR;
                        break;

                    case ("DBP"):
                    case ("PNP"):
                        ARINC_Navaid_NDB_Flight_Planing_Continuation_Record NDB_PR = new ARINC_Navaid_NDB_Flight_Planing_Continuation_Record();
                        ((ARINC_Navaid_NDB_Primary_Record)ArincObj).Flight_Planing_Continuation_Record.Add(NDB_PR);
                        Res = NDB_PR;
                        break;

                    case ("DBL"):
                    case ("PNL"):
                        ARINC_Navaid_NDB_Limitation_Continuation_Record NDB_LR = new ARINC_Navaid_NDB_Limitation_Continuation_Record();
                        ((ARINC_Navaid_NDB_Primary_Record)ArincObj).Limitation_Continuation_Record.Add(NDB_LR);
                        Res = NDB_LR;
                        break;

                #endregion

                #region Airport

                case ("PA "):
                    ARINC_Airport_Continuation_Record ARP_CR = new ARINC_Airport_Continuation_Record();
                    ((ARINC_Airport_Primary_Record)ArincObj).ARINC_Airport_Continuation_Record.Add(ARP_CR);
                    Res = ARP_CR;
                    break;
                case ("PAP"):
                    ARINC_Airport_Flight_Planning_Continuation_Records ARP_PR = new ARINC_Airport_Flight_Planning_Continuation_Records();
                    ((ARINC_Airport_Primary_Record)ArincObj).ARINC_Airport_Flight_Planning_Continuation_Records.Add(ARP_PR);
                    Res = ARP_PR;
                    break;

                #endregion

                #region WayPoint
                    case ("EA "):
                    case ("PC "):
                    ARINC_WayPoint_Continuation_Record WYP_CR = new ARINC_WayPoint_Continuation_Record();
                    ((ARINC_WayPoint_Primary_Record)ArincObj).Continuation_Record.Add(WYP_CR);
                    Res = WYP_CR;
                        break;

                    case ("EAP"):
                    case ("PCP"):
                        ARINC_WayPoint_Flight_Planing_Continuation_Record WYP_PR = new ARINC_WayPoint_Flight_Planing_Continuation_Record();
                        ((ARINC_WayPoint_Primary_Record)ArincObj).Flight_Planing_Continuation_Record.Add(WYP_PR);
                        Res = WYP_PR;
                        break;

                #endregion

                #region Runway

                    case ("PG "):
                        ARINC_Runway_Continuation_Records RWY_CR = new ARINC_Runway_Continuation_Records();
                        ((ARINC_Runway_Primary_Records)ArincObj).Runway_Continuation_Records.Add(RWY_CR);
                        Res = RWY_CR;
                        break;
                    case ("PAS"):
                        ARINC_Runway_Simulation_Continuation_Records ARP_SR = new ARINC_Runway_Simulation_Continuation_Records();
                        ((ARINC_Runway_Primary_Records)ArincObj).Runway_Simulation_Continuation_Records.Add(ARP_SR);
                        Res = ARP_SR;
                        break;

                #endregion

                default:
                    Res = ArincObj;
                    break;
            }
             return Res;

        }

        public static void Serialize(Object obj, string fileName)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
                MemoryStream strmMemSer = new MemoryStream();
                xmlSer.Serialize(strmMemSer, obj);



                byte[] byteArr = new byte[strmMemSer.Length];
                strmMemSer.Position = 0;
                int count = strmMemSer.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    strmMemSer.Close();
                    Console.WriteLine("Test Failed: Unable to write data from file");
                    return;
                }


                
                //if (File.Exists(fileName)) File.Delete(fileName);
                FileStream strmFile = new FileStream(fileName, FileMode.Create);
                strmMemSer.WriteTo(strmFile);
                strmMemSer.Close();
                strmMemSer.Dispose();

                strmFile.Flush();
                strmFile.Close();
                strmFile.Dispose();

            }
            catch (Exception exp)
            {
                throw new Exception("Serialization failed: " + exp.InnerException.Message);
                //System.Windows.Forms.MessageBox.Show("Serialization failed: "+exp.InnerException.Message);
            }
        }

        public static void SerializeANDZip(Object obj, string fileName)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
                MemoryStream strmMemSer = new MemoryStream();
                xmlSer.Serialize(strmMemSer, obj);



                byte[] byteArr = new byte[strmMemSer.Length];
                strmMemSer.Position = 0;
                int count = strmMemSer.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    strmMemSer.Close();
                    Console.WriteLine("Test Failed: Unable to write data from file");
                    return;
                }


                MemoryStream strmMemCompressed = new MemoryStream();
                GZipStream strmZip = new GZipStream(strmMemCompressed, CompressionMode.Compress, true);
                strmZip.Write(byteArr, 0, byteArr.Length);
                strmZip.Close();
                if (File.Exists(fileName)) File.Delete(fileName);

                System.IO.FileStream strmFileCompressed = new System.IO.FileStream(fileName, FileMode.CreateNew);
                strmMemCompressed.WriteTo(strmFileCompressed);
                strmMemCompressed.Close();
                strmFileCompressed.Close();

                strmMemCompressed.Dispose();



            }
            catch (Exception exp)
            {
                throw new Exception("Serialization failed: " + exp.InnerException.Message);
                //System.Windows.Forms.MessageBox.Show("Serialization failed: "+exp.InnerException.Message);
            }
        }

        public static ARINC_DECODED_OBJECTS DeSerialize(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                byte[]  byteArr = new byte[fs.Length];
                fs.Position = 0;
                int count = fs.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    fs.Close();
                    Console.WriteLine("Test Failed: Unable to read data from file");
                }


                MemoryStream strmMemSer = new MemoryStream();
                strmMemSer.Write(byteArr, 0, byteArr.Length);
                strmMemSer.Position = 0;

                XmlSerializer xmlSer = new XmlSerializer(typeof(ARINC_DECODED_OBJECTS));
                ARINC_DECODED_OBJECTS ARINC_OBJECTS_LIST = (ARINC_DECODED_OBJECTS)xmlSer.Deserialize(strmMemSer);
                fs.Close();
                strmMemSer.Close();


                return ARINC_OBJECTS_LIST;


            }
            catch (Exception exp)
            {
                throw new Exception("DeSerialization failed: " + exp.InnerException.Message);
                //System.Windows.Forms.MessageBox.Show("Serialization failed: "+exp.InnerException.Message);
            }
        }

        public static void SetObjectValue(object EditedObject, string PropertyName, object Value)
        {

            object objVal2 = EditedObject;

            string[] sa = PropertyName.Split('.');
            if (sa.Length == 0) return;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = EditedObject.GetType().GetProperty(s);

                if (propInfo == null) continue;
                try
                {
                    propInfo.SetValue(EditedObject, Value, null);
                }
                catch
                {
                    continue;
                }

            }


        }

        public static string GetObjectValue(object obj, string propName)
        {

            object objVal = obj;

            string[] sa = propName.Split('.');
            if (sa.Length == 0)
                return null;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = objVal.GetType().GetProperty(s);

                if (propInfo == null)
                    return null;

                object objPropVal = propInfo.GetValue(objVal, null);


                if (objPropVal is IList)
                {
                    objPropVal = (objPropVal as IList)[0];
                }

                objVal = objPropVal;

                if (objVal == null)
                    return null;
            }

            ////System.Diagnostics.Debug.WriteLine(propName + " - " + objVal.GetType().ToString());

            return (objVal == null ? "<null>" : objVal.ToString());
        }

        public static string GetObjectPropertyDescription(object obj, string propName)
        {

            object objVal = obj;
            string _description = "";

            string[] sa = propName.Split('.');
            if (sa.Length == 0)
                return null;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = objVal.GetType().GetProperty(s);

                if (propInfo == null)
                    return null;

                ////////////////////////////////////////////////
                DescriptionAttribute _DescriptionAttribute = DescriptionAttribute.GetCustomAttribute(propInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (_DescriptionAttribute != null)
                {
                    _description = _DescriptionAttribute.Description;
                }
                ////////////////////////////////////////////////

                
            }

            return _description;

        }


        public static void CloseProgressBar()
        {
            //ProgressHandler.Close();
        }

        public static string GetPathToAreaFile()
        {

            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA\\ARINC";
            const string keyName = userRoot + "\\" + subkey;
            string Res = (string)Registry.GetValue(keyName, "AreaFile", "Area not exist");

            return Res;

        }

        public static string GetPathToRegionsFile()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA\\ARINC";
            const string keyName = userRoot + "\\" + subkey;
            //string PathToSpecificationFile = (string)Registry.GetValue(keyName, "AreaFile", "Area not exist");
            return (string)Registry.GetValue(keyName, "Regions", "Regions not exist");
        }

        public static string GetPathToSpecificationFile()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA\\ARINC";
            const string keyName = userRoot + "\\" + subkey;
            return (string)Registry.GetValue(keyName, "AreaFile", "Area not exist");
            //return (string)Registry.GetValue(keyName, "Regions", "Regions not exist");
        }

        public static string GetPathToARINCSpecificationFile()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA\\ARINC";
            const string keyName = userRoot + "\\" + subkey;
            return (string)Registry.GetValue(keyName, "SpecificationPath", "File not exist");
            //return (string)Registry.GetValue(keyName, "Regions", "Regions not exist");
        }

        public static string GetPathToTemplateFile()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA";
            const string keyName = userRoot + "\\" + subkey;
            return (string)Registry.GetValue(keyName, "PdmFile", "File not exist");
            //return (string)Registry.GetValue(keyName, "Regions", "Regions not exist");
        }

        public static void SetLanguageCode(string LangCode)
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA";
            const string keyName = userRoot + "\\" + subkey;
            Registry.SetValue(keyName, "LanguageCode", LangCode);
        }

        public static string GetLanguageCode(string LangCode)
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "Software\\RISK\\ARENA";
            const string keyName = userRoot + "\\" + subkey;
            return (string)Registry.GetValue(keyName, "LanguageCode", "1033");
        }

        public static Dictionary<string, List<string>> DecodeJeppesenOBSTCLFile(string[] obsList)
        {

            //string[] obsList = System.IO.File.ReadAllLines(FilePath);

            //ProgressHandler.IsActive = ShowProgressBar;
            //ProgressHandler.Run("Please wait...", 0);

            double LineCount = (obsList.Length);



            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

            List<string> result = new List<string>();
            string[] columns = obsList[0].Split((char)9);

            for (int i = 1; i <= obsList.Length - 1; i++)
            {
                try
                {
                    int Proc = Convert.ToInt32(i * 100 / LineCount);

                    //ProgressHandler.SetState("Please wait...", Proc);

                    string ln = obsList[i];
                    string[] data = ln.Split((char)9);

                    string cntr = data[2];

                    if (dic.ContainsKey(cntr))
                    {
                        result = dic[cntr];
                    }
                    else
                    {
                        dic.Add(cntr, new List<string>());
                        result = dic[cntr];
                    }

                    string lat = data[23];
                    string lon = data[24];

                    if (lat.StartsWith("-")) lat = lat.Remove(0, 1);
                    if (lon.StartsWith("-")) lon = lon.Remove(0, 1);


                    int SepPos = lon.LastIndexOf(".");
                    if (SepPos > 0)
                    {
                        string Coordinata = lon.Substring(0, SepPos);
                        while (Coordinata.Length < 3) Coordinata = "0" + Coordinata;
                        string ost = lon.Substring(SepPos + 1, lon.Length - SepPos - 1);
                        lon = Coordinata + "." + ost;
                    }


                    SepPos = lat.LastIndexOf(".");
                    if (SepPos > 0)
                    {
                        string Coordinata = lat.Substring(0, SepPos);
                        while (Coordinata.Length < 2) Coordinata = "0" + Coordinata;
                        string ost = lat.Substring(SepPos + 1, lat.Length - SepPos - 1);
                        lat = Coordinata + "." + ost;
                    }


                    result.Add(data[0] + (char)9 + data[1] + (char)9 + data[2] + (char)9 + data[3] + (char)9 + data[4] + (char)9 + data[5] + (char)9 + data[6] + (char)9 + data[7] + (char)9 +
                        data[9] + (char)9 + data[10] + (char)9 + data[12] + (char)9 + lat + data[15] + (char)9 + lon + data[19] + (char)9);
                }
                catch(Exception ex)
                {
                    continue;
                }
            }


            //ProgressHandler.Stop();
            //ProgressHandler.Close();

            return dic;


        }

        public static Dictionary<string, List<string>> DecodeJeppesenOBSTCLFile(string FilePath)
        {


            DateTime start = DateTime.Now;


            var filestream = new FileStream(FilePath,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite);
            var file = new StreamReader(filestream, Encoding.UTF8, true, 128);
            string ln;
            int i = 0;
            string[] columns = null;

            

            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            while ((ln = file.ReadLine()) != null)
            {
                i++;
                if (i == 1) 
                { 
                    columns = ln.Split((char)9);
                    dic.Add("COLUMNS_HEADER", columns.ToList());
                    continue; 
                }

                /////////////////////////////////////////////////////////////////////////

                int Proc = Convert.ToInt32(i);

                //ProgressHandler.SetState("Please wait...", Proc);

                try
                {

                    List<string> result = new List<string>();


                    string[] data = ln.Split((char)9);

                    string cntr = data[2];

                    if (dic.ContainsKey(cntr))
                    {
                        result = dic[cntr];
                    }
                    else
                    {
                        dic.Add(cntr, new List<string>());
                        result = dic[cntr];
                    }

                    string lat = data[23];
                    string lon = data[24];

                    if (lat.StartsWith("-")) lat = lat.Remove(0, 1);
                    if (lon.StartsWith("-")) lon = lon.Remove(0, 1);


                    int SepPos = lon.LastIndexOf(".");
                    if (SepPos > 0)
                    {
                        string Coordinata = lon.Substring(0, SepPos);
                        while (Coordinata.Length < 3) Coordinata = "0" + Coordinata;
                        string ost = lon.Substring(SepPos + 1, lon.Length - SepPos - 1);
                        lon = Coordinata + "." + ost;
                    }


                    SepPos = lat.LastIndexOf(".");
                    if (SepPos > 0)
                    {
                        string Coordinata = lat.Substring(0, SepPos);
                        while (Coordinata.Length < 2) Coordinata = "0" + Coordinata;
                        string ost = lat.Substring(SepPos + 1, lat.Length - SepPos - 1);
                        lat = Coordinata + "." + ost;
                    }


                    result.Add(data[0] + (char)9 + data[1] + (char)9 + data[2] + (char)9 + data[3] + (char)9 + data[4] + (char)9 + data[5] + (char)9 + data[6] + (char)9 + data[7] + (char)9 +
                        data[9] + (char)9 + data[10] + (char)9 + data[12] + (char)9 + lat + data[15] + (char)9 + lon + data[19] + (char)9);




                }

                catch
                {
                    continue;
                }


                ////////////////////////////////////////////////////////////////////////

            }


            //ProgressHandler.Stop();
            //ProgressHandler.Close();

            file.Close();
            filestream.Close();

            file.Dispose();
            filestream.Dispose();

            return dic;

        }

        public static void CompressDirectory(string sInDir, string sOutFile)
        {


            string[] fileExtensions = {".mxd", ".mdb", ".pdm"};
            string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
            int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
                foreach (string sFilePath in sFiles)
                {
                    string sRelativePath = sFilePath.Substring(iDirLen);
                    //if (progress != null) progress(sRelativePath);
                    string fileExt = (Path.GetExtension(sRelativePath));
                    if (fileExtensions.ToList().IndexOf(fileExt)>=0)
                    CompressFile(sInDir, sRelativePath, str);
                }
        }

        public static bool DecompressToDirectory(string sCompressedFile, string sDir)
        {
            bool res = true;
            try
            {
                using (FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
                using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                    while (DecompressFile(sDir, zipStream)) ;
                
                
            }
            catch { res = false; }

            return res;
        }

        public static bool _DecompressToDirectory(string sCompressedFile, string sDir)
        {
            bool res = true;
            try
            {
                FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None);
                GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true);
                while (DecompressFile(sDir, zipStream)) System.Threading.Thread.Sleep(1);

                zipStream.Close();
                inFile.Close();

                zipStream.Dispose();
                inFile.Dispose();
            }
            catch { res = false; }

            return res;
        }

        private static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
        {
            //Compress file name
            char[] chars = sRelativePath.ToCharArray();
            zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
            foreach (char c in chars)
                zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));

            //Compress file content
            byte[] bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
            zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
            zipStream.Write(bytes, 0, bytes.Length);
        }
  
        private static bool DecompressFile(string sDir, GZipStream zipStream)
        {
            //Decompress file name
            byte[] bytes = new byte[sizeof(int)];
            int Readed = zipStream.Read(bytes, 0, sizeof(int));
            if (Readed < sizeof(int))
                return false;

            int iNameLen = BitConverter.ToInt32(bytes, 0);
            bytes = new byte[sizeof(char)];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iNameLen; i++)
            {
                zipStream.Read(bytes, 0, sizeof(char));
                char c = BitConverter.ToChar(bytes, 0);
                sb.Append(c);
            }
            string sFileName = sb.ToString();
            //if (progress != null) progress(sFileName);

            //Decompress file content
            bytes = new byte[sizeof(int)];
            zipStream.Read(bytes, 0, sizeof(int));
            int iFileLen = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[iFileLen];
            zipStream.Read(bytes, 0, bytes.Length);

            string sFilePath = Path.Combine(sDir, sFileName);
            string sFinalDir = Path.GetDirectoryName(sFilePath);
            if (!Directory.Exists(sFinalDir))
                Directory.CreateDirectory(sFinalDir);

            using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                outFile.Write(bytes, 0, iFileLen);

            return true;
        }

        public static void DeleteFilesFromDirectory(string DirName)
        {
            string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(DirName), "*.*");
            foreach (var fl in FN)
            {
                File.Delete(DirName + @"\" + System.IO.Path.GetFileName(fl));
            }
            
        }

        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return;

            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
            
        }




    }
    
}
