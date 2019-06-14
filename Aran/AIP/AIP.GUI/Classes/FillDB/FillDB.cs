using AIP.DB;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using AIP.GUI.Templates;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;
using AirportHeliport = Aran.Aim.Features.AirportHeliport;
using Navaid = Aran.Aim.Features.Navaid;
using Point = Aran.Geometries.Point;

namespace AIP.GUI.Classes
{
    /// <summary>
    /// Class to transfer Data from AIXM to AIP DB
    /// </summary>
    internal static partial class FillDB
    {
        /// <summary>
        /// Inherits from Main form
        /// </summary>
        internal static eAIPContext db;

        /// <summary>
        /// Contains list of the checked AIP id
        /// </summary>
        //internal static List<int> CheckedAIPCache = new List<int>();

        /// <summary>
        /// Object collect all messages in the FillDataset to send into Main form for the output with the AddOutput method
        /// </summary>
        internal static Queue<BaseLib.Struct.Output> OutputQueue = new Queue<BaseLib.Struct.Output>();

        /// <summary>
        /// Contains current section
        /// </summary>
        internal static string CurrentSection = "";

        //private static Dictionary<string, List<RPHelper>> RPSection;
        private static List<RP> RPSection;

        /// <summary>
        /// Fill ALL sections with subsections
        /// Contains GEN, ENR, AD sections
        /// 
        /// </summary>
        private static void Init_RPHelper()
        {
            try
            {
                if (RPSection == null)
                {
                    RPSection = new List<RP>();
                    List<CodeRuleProcedure?> Not_ICAO_DIFF = new List<CodeRuleProcedure?>()
                    {
                        CodeRuleProcedure.LAW,
                        CodeRuleProcedure.PRACTICE,
                        CodeRuleProcedure.PROCEDURE,
                        CodeRuleProcedure.RULE
                    };
                    List<CodeRuleProcedure?> PROCEDURE = new List<CodeRuleProcedure?>()
                    {
                        CodeRuleProcedure.PROCEDURE
                    };
                    List<CodeRuleProcedure?> RULE = new List<CodeRuleProcedure?>()
                    {
                        CodeRuleProcedure.RULE
                    };


                    RPSection.Add(new RP("GEN01",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_PREFACE, Not_ICAO_DIFF)
                        }));

                    RPSection.Add(new RP("GEN05",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_LIST_OF_HAND_AMENDMENTS_TO_THE_AIP, Not_ICAO_DIFF)
                        }));

                    RPSection.Add(new RP("ENR01", null));

                    RPSection.Add(new RP("GEN12",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_ENTRY_TRANSIT_AND_DEPARTURE_OF_AIRCRAFT, Not_ICAO_DIFF)
                        }));

                    RPSection.Add(new RP("GEN13",
                         new List<RPHelper>(){
                            new RPHelper(CodeRuleProcedureTitle.OTHER_ENTRY_TRANSIT_AND_DEPARTURE_OF_PASSENGERS_AND_CREW, Not_ICAO_DIFF)}));

                    RPSection.Add(new RP("GEN14",
                        new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_ENTRY_TRANSIT_AND_DEPARTURE_OF_CARGO)
                        }));

                    RPSection.Add(new RP("GEN15",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_AIRCRAFT_INSTRUMENTS_EQUIPMENT_AND_FLIGHT_DOCUMENTS)}));

                    RPSection.Add(new RP("GEN16",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_SUMMARY_OF_NATIONAL_REGULATIONS_AND_INTERNATIONAL_AGREEMENTS_CONVENTIONS)}));
                    RPSection.Add(new RP("GEN17",
                                            new List<RPHelper>() {
                   new RPHelper(CodeRuleProcedureTitle.OTHER_DIFFERENCES_FROM_ICAO_STANDARDS_RECOMMENDED_PRACTICES_AND_PROCEDURES)}));


                    RPSection.Add(new RP("GEN21",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_UNITS_OF_MEASUREMENT),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_TEMPORAL_REFERENCE_SYSTEM),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_HORIZONTAL_REFERENCE_SYSTEM),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_VERTICAL_REFERENCE_SYSTEM),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_AIRCRAFT_NATIONALITY_AND_REGISTRATION_MARKS
                    )}, 
                                            false));

                    RPSection.Add(new RP("GEN22",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_ABBREVIATIONS_USED_IN_AIS_PUBLICATIONS)}));

                    RPSection.Add(new RP("GEN23",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_CHART_SYMBOLS)}));

                    RPSection.Add(new RP("GEN26",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_CONVERSION_OF_UNITS_OF_MEASUREMENT)}));

                    RPSection.Add(new RP("GEN27",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_SUNRISE_SUNSET)}));

                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("GEN32",
                        new List<RPHelper>(){
                            new RPHelper(CodeRuleProcedureTitle.OTHER_MAINTENANCE_OF_CHARTS, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_AERONAUTICAL_CHART_SERIES_AVAILABLE, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_LIST_OF_AERONAUTICAL_CHARTS_AVAILABLE,Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_INDEX_TO_THE_WORLD_AERONAUTICAL_CHART_WAC,Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_CORRECTIONS_TO_CHARTS_NOT_CONTAINED_IN_THE_AIP,Not_ICAO_DIFF)
                        },
                        false));

                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("GEN33",
                        new List<RPHelper>(){
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_3_2_AREA_OF_RESPONSIBILITY, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_3_3_TYPES_OF_SERVICES, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_COORDINATION_BETWEEN_THE_OPERATOR_AND_ATS,Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_MINIMUM_FLIGHT_ALTITUDE,Not_ICAO_DIFF)
                        },
                        false));

                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("GEN34",
                        new List<RPHelper>(){
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_4_2_AREA_OF_RESPONSIBILITY, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_4_3_TYPES_OF_SERVICES, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_REQUIREMENTS_AND_CONDITIONS,Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_MISCELLANEOUS,Not_ICAO_DIFF)
                        },
                        false));

                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("GEN35",
                        new List<RPHelper>(){
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_5_2_AREA_OF_RESPONSIBILITY, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_METEOROLOGICAL_OBSERVATIONS_AND_REPORTS, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_5_4_TYPES_OF_SERVICES, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_NOTIFICATION_REQUIRED_FROM_OPERATORS, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_AIRCRAFT_REPORTS, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_SIGMET_AND_AIRMET_SERVICE, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_OTHER_AUTOMATED_METEOROLOGICAL_SERVICES, PROCEDURE)
                        },
                        false));

                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("GEN36",
                        new List<RPHelper>(){
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_6_2_AREA_OF_RESPONSIBILITY, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GEN_3_6_3_TYPES_OF_SERVICES, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_SEARCH_AND_RESCUE_AGREEMENTS, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_CONDITIONS_OF_AVAILABILITY, Not_ICAO_DIFF),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_PROCEDURES_AND_SIGNALS_USED, Not_ICAO_DIFF)
                        },
                        false));

                    RPSection.Add(new RP("GEN41",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_AERODROME_HELIPORT_CHARGES)}));

                    RPSection.Add(new RP("GEN42",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.AIR_NAVIGATION_SERVICES_CHARGES)}));

                    // ENR
                    RPSection.Add(new RP("ENR11",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_GENERAL_RULES)}));
                    RPSection.Add(new RP("ENR12",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.VISUAL_FLIGHT_RULES)}));
                    RPSection.Add(new RP("ENR13",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.INSTRUMENT_FLIGHT_RULES)}));
                    RPSection.Add(new RP("ENR14",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.ATS_AIRSPACE_CLASSIFICATION),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_ATS_AIRSPACE_DESCRIPTION)

                                            }));
                    RPSection.Add(new RP("ENR15",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_GENERAL),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_ARRIVING_FLIGHTS),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_DEPARTING_FLIGHTS),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_ENR_1_5_4_OTHER_RELEVANT_INFORMATION_AND_PROCEDURES)
                                                
                                            }));
                    RPSection.Add(new RP("ENR16",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_PRIMARY_RADAR),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_SECONDARY_SURVEILLANCE_RADAR_SSR),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_AUTOMATIC_DEPENDENT_SURVEILLANCE_BROADCAST_ADS_B),
                    new RPHelper(CodeRuleProcedureTitle.OTHER_ENR_1_6_4_OTHER_RELEVANT_INFORMATION_AND_PROCEDURES)

                                            }));
                    RPSection.Add(new RP("ENR17",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.ALTIMETER_SETTING_PROCEDURES)}));
                    RPSection.Add(new RP("ENR18",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.REGIONAL_SUPPLEMENTARY_PROCEDURES)}));

                    RPSection.Add(new RP("ENR19",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_AIR_TRAFFIC_FLOW_MANAGEMENT_AND_AIRSPACE_MANAGEMENT)}));

                    RPSection.Add(new RP("ENR110",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.FLIGHT_PLANNING)}));
                    RPSection.Add(new RP("ENR111",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_ADRESSING_OF_FLIGHT_PLAN_MESSAGES)}));
                    RPSection.Add(new RP("ENR112",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_INTERCEPTION_OF_CIVIL_AIRCRAFT)}));
                    RPSection.Add(new RP("ENR113",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.UNLAWFUL_INTERFERENCE)}));
                    RPSection.Add(new RP("ENR114",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.AIR_TRAFFIC_INCIDENTS)}));

                    RPSection.Add(new RP("ENR22",
                                            new List<RPHelper>() {
                    new RPHelper(CodeRuleProcedureTitle.OTHER_OTHER_REGULATED_AIRSPACE)
                                            }));
                    RPSection.Add(new RP("ENR35",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_OTHER_ROUTES)
                        }));

                    RPSection.Add(new RP("ENR43",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GLOBAL_NAVIGATION_SATELLITE_SYSTEM_GNSS)
                        }));
                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("ENR52",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_MILITARY_EXERCISE_AND_TRAINING_AREAS_AND_ADIZ, Not_ICAO_DIFF)
                        }, false));
                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("ENR53",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_OTHER_POTENTIAL_HAZARDS, Not_ICAO_DIFF)
                        }, false));

                    // Mixed section, RulesProcedure mix with mapping. Last param is false.
                    RPSection.Add(new RP("ENR55",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_AERIAL_SPORTING_AND_RECREATIONAL_ACTIVITIES, Not_ICAO_DIFF)
                        }, false));

                    RPSection.Add(new RP("ENR56",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_BIRD_MIGRATION_AND_AREAS_WITH_SENSITIVE_FAUNA)
                        }));
                    RPSection.Add(new RP("AD11",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GENERAL_CONDITIONS),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_USE_OF_MILITARY_AIR_BASES),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_LOW_VISIBILITY_PROCEDURES_LVP),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_AERODROME_OPERATING_MINIMA),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_OTHER_INFORMATION)
                        }));
                    RPSection.Add(new RP("AD12",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_RESCUE_AND_FIRE_FIGHTING_SERVICES),
                            new RPHelper(CodeRuleProcedureTitle.OTHER_SNOW_PLAN)
                        }));

                    RPSection.Add(new RP("AD14",
                        new List<RPHelper>() {
                            new RPHelper(CodeRuleProcedureTitle.OTHER_GROUPING_OF_AERODROMES_HELIPORTS)
                        }));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        
        /// <summary>
        /// Fill Dataset with data from AIXM 5.1
        /// </summary>
        /// <param name="featureList">
        /// 
        /// </param>
        public static void FillAIPDatasetSection(string section, List<Feature> featureList, DB.AirportHeliport airHel = null)
        {
            try
            {
                Init_RPHelper();
                DB.eAIP caip = db.eAIP.FirstOrDefault(n => n.id == Lib.CurrentAIP.id);
                CurrentSection = section;
                AIPSection ent = null;

                ent = CheckSection(caip, section, airHel);

                if (ent == null)
                    return;

                if (RPSection.Any(x => x.Title == section && x.AutoGenerate == true))
                    Fill_RPHelper(ent, featureList, caip, RPSection.FirstOrDefault(x => x.Title == section)?.RPHelperList);
                else
                {
                    if (airHel != null) // AD2 or AD3
                    {
                        Aran.Aim.Features.AirportHeliport air = Lib.GetAIXMAirportHeliport()
                            .FirstOrDefault(x => x.LocationIndicatorICAO == airHel.LocationIndicatorICAO);
                        if (air == null) return;
                        RunMethod($@"Fill_{section}", new object[] { ent, featureList, caip, air });
                    }
                    else
                        RunMethod($@"Fill_{section}", new object[] { ent, featureList, caip });
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        /// <summary>
        /// Fill Dataset with data from AIXM 5.1
        /// </summary>
        /// <param name="featureList">
        /// 
        /// </param>
        public static void FillAirportHeliport(string section, List<Feature> featureList, DB.AirportHeliport airHel = null)
        {
            try
            {
                Aran.Aim.Features.AirportHeliport air = Lib.GetAIXMAirportHeliport().FirstOrDefault(x => x.LocationIndicatorICAO == airHel?.LocationIndicatorICAO);
                if (air == null) return;

                DB.eAIP caip = db.eAIP.FirstOrDefault(n => n.id == Lib.CurrentAIP.id);
                CurrentSection = section;
                List<string> sectionList = new List<string>();
                // Number of sections
                // AD2 - 24, AD3 - 23
                int maxNum = section.StartsWith("AD2") ? 24 : 23;
                if (Lib.TempAdNum < maxNum) maxNum = Lib.TempAdNum; // temporary to test first X sections
                for (int i = 0; i <= maxNum; i++) // For AirportHeliport - each from AD21 to AD224, from AD31 to AD323
                {
                    sectionList.Add(section + i); // AD2 + (0-24)
                }
                Dictionary<string, AIPSection> entList = CheckAHSection(caip, sectionList, airHel);
                if (entList == null)
                    return;

                foreach (KeyValuePair<string, AIPSection> entPair in entList)
                {
                    RunMethod($@"Fill_{entPair.Key}", new object[] { entPair.Value, featureList, caip, air }); // AD2 + (0-24)
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static bool RunMethod(string MethodName, object[] Parameters)
        {
            try
            {
                MethodInfo BuildSectionMethod = typeof(AIP.GUI.Classes.FillDB).GetMethod(MethodName);
                if (BuildSectionMethod == null)
                {
                    SendOutput($@"No such method {MethodName} found", Color.Red);
                    ErrorLog.Add($@"No such method {MethodName} found");
                    return false;
                }
                BuildSectionMethod.Invoke(null, Parameters);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        private static void SendOutput(string Message, Color? Color = null, int? Percent = null)
        {
            try
            {
                BaseLib.Struct.Output output = new BaseLib.Struct.Output();
                output.Message = Message;
                output.Color = Color ?? System.Drawing.Color.Black;
                output.Percent = Percent;
                OutputQueue.Enqueue(output);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static AIPSection CheckSection(DB.eAIP caip, string sectionT, DB.AirportHeliport airHel = null)
        {
            try
            {
                //SectionStatusEnum airHelStatus = SectionStatusEnum.None;
                SectionName enumSection = Lib.GetSectionName(sectionT);
                AIPSection ent = null;
                if (airHel != null)
                    ent = db.AIPSection.FirstOrDefault(n => n.eAIPID == Lib.CurrentAIP.id && n.SectionName == enumSection
                    && n.AirportHeliportID == airHel.id);
                else
                    ent = db.AIPSection.FirstOrDefault(n => n.eAIPID == Lib.CurrentAIP.id && n.SectionName == enumSection);

                if (ent == null)
                    return CreateSection(caip, sectionT, airHel);
                else if (ent.SectionStatus == SectionStatusEnum.Filled && Properties.Settings.Default.CheckSectionForFilling)
                {
                    SendOutput("Section already filled", Color.DarkOrange);
                    return null;
                }
                else if (ent.SectionStatus == SectionStatusEnum.Work || ent.SectionStatus == SectionStatusEnum.Filled)
                {
                    bool isContinue = false;
                    if (Properties.Settings.Default.AskReFillSection)
                    {
                        DialogResult dialogResult = MessageBox.Show("Do you want to delete all received from AIXM data and received them again?", "Question", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes) isContinue = true;
                    }
                    else
                        isContinue = true;


                    if (isContinue)
                    {
                        List<string> Subsection_sections = Lib
                            .SectionByAttribute(SectionParameter.FillSubsection)
                            .ToStringList();

                        //new List<string>() { "GEN01", "GEN11", "GEN12", "GEN13", "GEN14", "GEN15", "GEN16", "GEN17", "GEN22", "GEN26", "GEN27", "GEN31", "GEN41", "GEN42", "ENR11", "ENR12", "ENR13", "ENR14", "ENR15", "ENR16", "ENR17", "ENR18", "ENR19", "ENR110", "ENR111", "ENR112", "ENR113", "ENR114", "ENR21", "ENR22", "ENR23", "ENR35", "ENR42", "ENR43", "ENR45", "ENR51", "ENR54", "ENR56", "ENR6", "AD11", "AD12", "AD13", "AD14", "AD22", "AD23", "AD24", "AD25", "AD26", "AD27", "AD28", "AD29", "AD210", "AD211", "AD212", "AD213", "AD214", "AD215", "AD216", "AD217", "AD218", "AD219", "AD220", "AD221", "AD222", "AD223", "AD224", "AD32", "AD33", "AD34", "AD35", "AD36", "AD37", "AD38", "AD39", "AD310", "AD311", "AD312", "AD313", "AD314", "AD315", "AD316", "AD317", "AD318", "AD319", "AD320", "AD321", "AD322", "AD323", "AD324" };

                        using (var transaction = new TransactionScope())
                        {
                            string Remove_Section = sectionT;
                            if (sectionT == "ENR31" || sectionT == "ENR32" || sectionT == "ENR33" || sectionT == "ENR34")
                                Remove_Section = "ENR31_34";
                            if (Subsection_sections.Contains(sectionT))
                                Remove_Section = "Subsection";

                            if (enumSection.HasParameterFlag(SectionParameter.Fill) && RunMethod($@"Remove_{Remove_Section}", new object[] { ent }) == false)
                                return null;
                            else
                            {
                                ent.SectionStatus = SectionStatusEnum.None;
                                ent.Title = GetSectionTitle(sectionT);
                                db.Entry(ent).State = EntityState.Modified;
                                db.SaveChanges();
                                transaction.Complete();
                                return ent;
                            }
                        }
                    }
                    else
                    {
                        SendOutput("Operation terminated by user. Exiting from action.", Color.Red, 100);
                        return null;
                    }
                }
                else
                    return ent;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                SendOutput("Unknown error occured in the CheckSection", Color.Red, 100);
                return null;
            }
        }

        private static Dictionary<string, AIPSection> CheckAHSection(DB.eAIP caip, List<string> sectionList, DB.AirportHeliport airHel)
        {
            try
            {
                AIPSection ent = null;
                Dictionary<string, AIPSection> aipHelSectionList = new Dictionary<string, AIPSection>();
                for (var i = 0; i < sectionList.Count; i++)
                {
                    string sectionT = sectionList[i];
                    SectionName enumSection = Lib.GetSectionName(sectionT);
                    ent = db.AIPSection.FirstOrDefault(n =>
                        n.eAIPID == Lib.CurrentAIP.id && n.SectionName == enumSection
                        && n.AirportHeliportID == airHel.id);

                    if (ent == null)
                    {
                        ent = CreateSection(caip, sectionT, airHel);
                    }
                    // If it is first level of the AD2 or AD3
                    // Then we check for filling and must make logic for all pseudo-children
                    // This logic is implemented to prevent setting Work status for all subsections of the AirportHeliport
                    else if (i == 0)
                    {
                        if (ent.SectionStatus == SectionStatusEnum.Filled && Properties.Settings.Default.CheckSectionForFilling)
                        {
                            SendOutput("Section already filled", Color.DarkOrange);
                            return null;
                        }
                        else if (ent.SectionStatus == SectionStatusEnum.Work || ent.SectionStatus == SectionStatusEnum.Filled)
                        {
                            bool isContinue = false;
                            if (Properties.Settings.Default.AskReFillSection)
                            {
                                DialogResult dialogResult = MessageBox.Show("Do you want to delete all received from AIXM data and received them again?", "Question", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.Yes) isContinue = true;
                            }
                            else
                                isContinue = true;


                            if (isContinue)
                            {
                                using (var transaction = new TransactionScope())
                                {
                                    // Removing all children subsections
                                    for (var j = 0; j < sectionList.Count; j++)
                                    {
                                        string sectionRemove = sectionList[j];
                                        SectionName enumRemoveSection = Lib.GetSectionName(sectionRemove);
                                        var entRemove = db.AIPSection.FirstOrDefault(n =>
                                            n.eAIPID == Lib.CurrentAIP.id && n.SectionName == enumRemoveSection
                                            && n.AirportHeliportID == airHel.id);

                                        if (entRemove != null)
                                        {
                                            int parentId = entRemove.id;
                                            db.Subsection.RemoveRange(db.Subsection.Where(n => n.AIPSectionID == parentId));
                                            entRemove.SectionStatus = SectionStatusEnum.None;
                                            entRemove.Title = GetSectionTitle(sectionRemove);
                                            db.Entry(entRemove).State = EntityState.Modified;
                                        }
                                    }
                                    db.SaveChanges();
                                    transaction.Complete();
                                    //return ent; // Adding into List at end of function
                                }
                            }
                            else
                            {
                                SendOutput("Operation terminated by user. Exiting from action.", Color.Red, 100);
                                return null;
                            }
                        }
                    }
                    aipHelSectionList.Add(sectionT, ent);
                }
                return aipHelSectionList;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                SendOutput("Unknown error occured in the CheckSection", Color.Red, 100);
                return null;
            }
        }

        private static AIPSection CreateSection(eAIP caip, string sectionT, DB.AirportHeliport airHel = null)
        {
            try
            {
                AIPSection newSection = null;
                newSection = new AIPSection();
                newSection.SectionName = Lib.GetSectionName(sectionT);
                newSection.SectionStatus = SectionStatusEnum.None;
                newSection.eAIPID = caip.id;
                newSection.eAIP = caip;
                newSection.Title = GetSectionTitle(sectionT);
                if (airHel != null) // AD2.**** or AD3.****
                {
                    newSection.AirportHeliport = airHel;
                    newSection.AirportHeliportID = airHel.id;
                }
                db.AIPSection.Add(newSection);
                db.SaveChanges();
                return newSection;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        #region Remove Sections

        /// <summary>
        /// For all section where root is Subsection (Gen 0.1, GEN 1.1, GEN 1.2, etc)
        /// </summary>
        /// <param name="ent"></param>
        public static void Remove_Subsection(ref dynamic ent)
        {
            try
            {
                int parent_id = ent.id;
                db.Subsection.RemoveRange(db.Subsection.Where(n => n.AIPSectionID == parent_id));
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Remove_GEN24(ref AIPSection ent)
        {
            try
            {
                List<LocationDefinition> tmp = ((AIPSection)ent).Children.OfType<LocationTable>().SelectManyNullSafe(n => n.LocationDefinition).ToList();
                db.LocationDefinition.RemoveRange(tmp);
                var tmp2 = ((AIPSection)ent).Children.OfType<LocationTable>().FirstOrDefault();
                if (tmp2 != null) db.LocationTable.Remove(tmp2);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }
        public static void Remove_ENR31_34(ref dynamic ent)
        {
            try
            {
                int parent_id = ent.id;

                // 1-st level getting ids from section
                List<int> rt_ids = db.Route.Where(m => m.AIPSectionID == parent_id).Select(n => n.id).ToList();

                // 2-nd level getting ids from 1-st ids
                List<int> rs_ids = db.Routesegment.Where(n => rt_ids.Contains(n.Parent.id)).Select(n => n.id).ToList();
                List<int> spr_ids = db.Significantpointreference.Where(n => rt_ids.Contains(n.Parent.id)).Select(n => n.id).ToList();

                // 3-rd level removing by 2-nd level ids
                db.Routesegmentusagereference.RemoveRange(db.Routesegmentusagereference.Where(x => rs_ids.Contains(x.Routesegmentid ?? -1)));
                db.Navaidindication.RemoveRange(db.Navaidindication.Where(x => spr_ids.Contains(x.Significantpointreferenceid ?? -1)));

                // 2-nd level removing by own ids
                db.Routesegment.RemoveRange(db.Routesegment.Where(n => rs_ids.Contains(n.id)));
                db.Significantpointreference.RemoveRange(db.Significantpointreference.Where(n => spr_ids.Contains(n.id)));

                // 1-st level removing by own ids
                db.Route.RemoveRange(db.Route.Where(n => rt_ids.Contains(n.id)));
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Remove_ENR36(ref AIPSection ent)
        {
            try
            {
                db.Sec36Table2.RemoveRange(((AIPSection)ent).Children.OfType<Sec36Table>().SelectManyNullSafe(n => n.Sec36Table2));
                db.Sec36Table.RemoveRange(ent.Children.OfType<Sec36Table>());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Remove_ENR41(ref AIPSection ent)
        {
            try
            {
                db.Navaid.RemoveRange(ent.Children.OfType<DB.Navaid>());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Remove_ENR44(ref AIPSection ent)
        {
            try
            {
                db.Designatedpoint.RemoveRange(ent.Children.OfType<DB.Designatedpoint>());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }


        #endregion


        private static string GetSectionTitle(string Section = null)
        {
            try
            {
                if (Section == null)
                    Section = CurrentSection;

                return
                    Tpl.MenuText(Section);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        #region Fill Sections


        public static bool IsFIRAirspace(List<Aran.Aim.Objects.FeatureRefObject> AffectedArea)
        {
            try
            {
                return true;// ToDo: temporary added
                // Fill static variable just once
                if (Lib.Airspace_FIR_Guid_List == null)
                {
                    Lib.Airspace_FIR_Guid_List = Globals.GetFeaturesByED(FeatureType.Airspace).
                        Cast<Airspace>().
                        Where(n => n.Type == CodeAirspace.FIR || n.Type == CodeAirspace.FIR_P).
                        Select(x => x.Identifier).
                        ToList();
                }
                if (AffectedArea?.Any(x => Lib.Airspace_FIR_Guid_List.Contains(x.Feature.Identifier)) == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static bool ForAirportHeliport(List<Aran.Aim.Objects.FeatureRefObject> affectedLocation, AirportHeliport airHel)
        {
            try
            {
                return affectedLocation?.Any(x => x.Feature.Identifier == airHel.Identifier) == true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }



        private static void SendSaveOutput(AIPSection ent, AirportHeliport air = null)
        {
            try
            {
                string section = air == null ? ent.SectionName.ToString() : $@"{air.LocationIndicatorICAO} {ent.SectionName.ToString().Replace("AD2", "AD 2.").Replace("AD3", "AD 3.")}";
                SendOutput($@"Saving {section} data...", Percent: 90);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_FileSection(AIPSection ent, List<Feature> featureList, DB.eAIP caip, AirportHeliport airHel = null)
        {
            try
            {
                string output = "";
                List<ForFileSection> dataList = new List<ForFileSection>();
                //db.Database.Log = Console.Write;
                // Getting info from AIP DB
                int langId = Lib.GetLangIdByValue(caip.lang) ?? 0;
                if (langId == 0) return;
                List<AIPFile> aipFiles = new List<AIPFile>();
                Expression<Func<AIPFile, bool>> isAirHel = _ => true;
                if (airHel != null) isAirHel = x => x.AirportHeliport == airHel.LocationIndicatorICAO;

                aipFiles = db.AIPFile
                    .Include(x => x.ChartNumber.Name)
                    .AsNoTracking()
                    .Where(x => x.IsCanceled != true
                                && x.SectionName == ent.SectionName
                                && (x.LanguageReferenceId == langId || x.LanguageReferenceId == null)
                                && (caip.Effectivedate >= x.EffectivedateFrom && (x.EffectivedateTo == null || caip.Effectivedate <= x.EffectivedateTo))
                    )
                    .Where(isAirHel)
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .OrderBy(x => x.Order)
                    //.ThenBy(x => x.Title)
                    .ThenBy(x => x.ChartNumber.Name)
                    .Include(x => x.AIPFileData.AIPFileDataHash)
                    .ToList();

                if (aipFiles.Any())
                {
                    AIP.DB.Subsection ss;
                    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 0;

                        foreach (AIPFile aipFile in aipFiles)
                        {
                            ForFileSection Data = new ForFileSection();
                            Data.Title = aipFile.Title;
                            Data.Description = aipFile.Description;
                            Data.FileName = Lib.AIPFileName(aipFile);
                            Data.FileType = Lib.GetFileType(aipFile.FileName);
                            // Save file to disk
                            string path = Lib.SaveAIPFile(aipFile);
                            Data.GraphicFiles = PDFManager.GetGraphicFiles(aipFile, path, false);
                            if (path != "" && File.Exists(path))
                            {
                                SendOutput($@"Saving file on the disk: {Data.FileName}", Percent: 85);
                                string storedHash = aipFile.AIPFileData.AIPFileDataHash.Hash;
                                if (storedHash == Lib.SHA1(path))
                                    SendOutput($@"File hash is equal to original: {storedHash}", Percent: 88);
                                else
                                    SendOutput($@"File hash is not equal to original: {storedHash}", Percent: 88);
                            }
                            else
                            {
                                SendOutput($@"Error in saving file: {Data.FileName}", Percent: 85);
                            }

                            dataList.Add(Data);
                        }

                        if (!Lib.IsNull(dataList))
                        {
                            output = Razor.Run(dataList);
                            if (output == null)
                            {
                                SendOutput("Error in generating template ", Percent: 80);
                                return;
                            }
                            ss.Content = output;
                            db.Subsection.Add(ss);
                        }

                        ent.SectionStatus = SectionStatusEnum.Filled;
                        SendSaveOutput(ent, airHel);
                        db.SaveChanges();
                        transaction.Complete();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        private static string ContactInformationToXHTML(ContactInformation adr, string Name = null)
        {
            try
            {
                AddressContact acon = new AddressContact();
                acon.Name = Name ?? adr.Name ?? "";
                acon.Title = adr.Title ?? "";
                acon.DeliveryPoint = adr.Address.FirstOrDefault()?.DeliveryPoint ?? "";
                acon.City = adr.Address.FirstOrDefault()?.City ?? "";
                acon.PostalCode = adr.Address.FirstOrDefault()?.PostalCode ?? "";
                acon.Country = adr.Address.FirstOrDefault()?.Country ?? "";
                acon.Voice = adr.PhoneFax.FirstOrDefault()?.Voice ?? "".NilIfEmpty();
                acon.Facsimile = adr.PhoneFax.FirstOrDefault()?.Facsimile ?? "".NilIfEmpty();
                acon.AFS = adr.NetworkNode.FirstOrDefault(n => n.Network == CodeTelecomNetwork.AFTN)?.Linkage ?? "".NilIfEmpty(); //?
                acon.Email = adr.NetworkNode.FirstOrDefault()?.eMail ?? "".NilIfEmpty();
                acon.Linkage = adr.NetworkNode.FirstOrDefault()?.Linkage ?? "".NilIfEmpty();
                
                string output = "";
                output += $@"<e:Address class = ""Body""><e:Address-part Type=""Post"">";
                output += $@"<strong>{acon.Name}<br/></strong>";
                output += $@"{acon.Title}<br/>";
                output += $@"{acon.DeliveryPoint}<br/>";
                output += $@"{acon.City},";
                output += $@"{acon.PostalCode}, ";
                output += $@"{acon.Country}";
                output += $@"</e:Address-part>";
                output += $@"<e:Address-part Type=""Phone"" class=""Address-part-Phone"">{acon.Voice}</e:Address-part>";
                output += $@"<e:Address-part Type=""Fax"" class=""Address-part-Fax"">{acon.Facsimile}</e:Address-part>";

                output += $@"<e:Address-part Type=""AFS"" class=""Address-part-AFS"">{acon.AFS}</e:Address-part>";
                output += $@"<e:Address-part Type=""Email"" class=""Address-part-Email"">{acon.Email}</e:Address-part>";
                output += $@"<e:Address-part Type=""URL"" Updated=""No"" class=""Address-part-URL"">{acon.Linkage}</e:Address-part>";
                output += $@"</e:Address>";

                return output;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        struct AddressContact
        {
            public string Name;
            public string Title;
            public string DeliveryPoint;
            public string City;
            public string PostalCode;
            public string Country;
            public string Voice;
            public string Facsimile;
            public string AFS;
            public string Email;
            public string Linkage;
        }
        private static AIP.XML.Address ContactInformationToXMLAIP(ContactInformation adr)
        {
            try
            {
                AddressContact acon = new AddressContact();
                acon.Name = adr.Name ?? "";
                acon.Title = adr.Title ?? "";
                acon.DeliveryPoint = adr.Address.FirstOrDefault()?.DeliveryPoint ?? "";
                acon.City = adr.Address.FirstOrDefault()?.City ?? "";
                acon.PostalCode = adr.Address.FirstOrDefault()?.PostalCode ?? "";
                acon.Country = adr.Address.FirstOrDefault()?.Country ?? "";
                acon.Voice = adr.PhoneFax.FirstOrDefault()?.Voice ?? "";
                acon.Facsimile = adr.PhoneFax.FirstOrDefault()?.Facsimile ?? "";
                acon.AFS = adr.NetworkNode.FirstOrDefault()?.Linkage ?? ""; //?
                acon.Email = adr.NetworkNode.FirstOrDefault()?.eMail ?? "";
                acon.Linkage = adr.NetworkNode.FirstOrDefault()?.Linkage ?? "";


                List<AIP.XML.Addresspart> address_lst = new List<XML.Addresspart>();
                AIP.XML.Address address = new XML.Address() { @class = "Body" };
                AIP.XML.Addresspart address_part = new XML.Addresspart() { Type = XML.AddresspartType.Post };
                XML.strong title = new XML.strong() { Items = new object[] { acon.Name, new XML.br() } };
                address_part.Items = new object[] { title, acon.Title, new XML.br(),
                                                           acon.DeliveryPoint, new XML.br(),
                                                           acon.City, new XML.br(),
                                                           acon.PostalCode, new XML.br(),
                                                           acon.Country, new XML.br()
                };
                address_lst.Add(address_part);
                address_lst.Add(new XML.Addresspart() { Type = XML.AddresspartType.Phone, Items = new object[] { acon.Voice } });
                address_lst.Add(new XML.Addresspart() { Type = XML.AddresspartType.Fax, Items = new object[] { acon.Facsimile } });
                address_lst.Add(new XML.Addresspart() { Type = XML.AddresspartType.AFS, Items = new object[] { acon.Linkage } });
                address_lst.Add(new XML.Addresspart() { Type = XML.AddresspartType.Email, Items = new object[] { acon.Email } });
                address_lst.Add(new XML.Addresspart() { Type = XML.AddresspartType.URL, Items = new object[] { acon.Linkage } });

                address.Addresspart = address_lst.ToArray();

                return address;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static Func<RulesProcedures, bool> IsRP(List<RPHelper> RPHelper)
        {
            Func<RulesProcedures, bool> isRP = _ => false;
            try
            {
                Dictionary<CodeRuleProcedureTitle?, List<CodeRuleProcedure?>> dic = RPHelper.ToDictionary(x => x.RPTitle, x => x.RPCategory);
                if (dic.Count > 0)
                {
                    isRP = x => x.Title != null && dic.ContainsKey(x.Title) && (dic[x.Title] == null || dic[x.Title].Contains(x.Category));
                }
                return isRP;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return isRP;
            }
        }

        public static List<RulesProcedures> GetRP(List<RPHelper> RPHelperList)
        {
            try
            {
                return Globals.GetFeaturesByED(FeatureType.RulesProcedures)
                    .Cast<RulesProcedures>()
                    .Where(IsRP(RPHelperList))
                    .Where(n => IsFIRAirspace(n.AffectedArea) &&
                                Lib.IsLanguageNote(n.Annotation))
                    .ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        public static void Fill_RPHelper(AIPSection ent, List<Feature> featureList, DB.eAIP caip, List<RPHelper> RPHelperList)
        {
            try
            {
                List<CodeRuleProcedureTitle?> ParamsOrdered = RPHelperList.Select(x => x.RPTitle).ToList();
                List<RulesProcedures> ft = GetRP(RPHelperList);
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (Aran.Aim.Features.RulesProcedures rproc in ft)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rproc.Title);
                        string TitleLang = RPHelperList.FirstOrDefault(n => n.RPTitle == rproc.Title).Title;
                        if (string.IsNullOrEmpty(TitleLang))
                            TitleLang = ent.SectionName.ToString() + (ss.OrderNumber + 1).ToString();
                        ss.Title = Lib.GetText(TitleLang);//rproc.Title.ToString().Replace("OTHER_", "").Replace("_", " ");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.Content = rproc.Content.ToXhtml().ToDataTag(rproc, "Content"); //
                        db.Subsection.Add(ss);
                    }
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        /// <summary>
        /// Convert string of values to unique string of values
        /// </summary>
        /// <param name="subdata"></param>
        /// <param name="InputDelimiter"></param>
        /// <param name="OutputDelimiter"></param>
        /// <returns></returns>
        private static string CommaStringToUniqueString(string subdata, string InputDelimiter = ",", string OutputDelimiter = "-")
        {
            try
            {
                char[] InputDelimiterCA = InputDelimiter.ToCharArray();
                string[] tmp = subdata.TrimEnd(InputDelimiterCA).Split(InputDelimiterCA);
                HashSet<string> hset = new HashSet<string>(tmp);
                return string.Join(OutputDelimiter, hset);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        #endregion




        //public struct RPHelper
        //{
        //    public Dictionary<CodeRuleProcedureTitle?, List<CodeRuleProcedure?>> RpTitleDictionary;
        //    public string Title;

        //    public RPHelper(Dictionary<CodeRuleProcedureTitle?, List<CodeRuleProcedure?>> p1, string p3 = null)
        //    {
        //        RpTitleDictionary = p1;
        //        Title = p3;
        //    }
        //}
    }



}