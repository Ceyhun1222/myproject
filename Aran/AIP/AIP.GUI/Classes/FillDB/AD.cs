using AIP.DB;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Windows.Forms;
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
    /// AD2 section
    /// Very similar to AD3
    /// </summary>
    internal static partial class FillDB
    {

        public static void Fill_AD13(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                string output = "";
                List<ForAD13> dataList = new List<ForAD13>();
                List<Dictionary<string, string>> ObjList = new List<Dictionary<string, string>>();

                var ftgroup = Globals.GetFeaturesByED(FeatureType.AirportHeliport)?.Cast<Aran.Aim.Features.AirportHeliport>()
                    .ToList()
                    .OrderBy(x=>x.Name)
                    .GroupBy(n => n.Type);

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    if (ftgroup != null)
                        foreach (var ft in ftgroup)
                        {
                            ForAD13 data = new ForAD13();
                            ObjList = new List<Dictionary<string, string>>();
                            data.type = Lib.GetEnumText(ft.Key);
                            // AD 2 or AD 3
                            string num = (ft.Key == CodeAirportHeliport.AD || ft.Key == CodeAirportHeliport.AH)
                                ? "2"
                                : "3";

                            foreach (Aran.Aim.Features.AirportHeliport ais in ft)
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();

                                dic["Name"] = ais.Name;
                                dic["LocationIndicatorICAO"] = ais.LocationIndicatorICAO;
                                dic["LocationIndicatorICAO_URL"] =
                                    $@"<a href = ""{Lib.CurrentAIS?.ICAOcountrycode?.ToUpperInvariant()}-AD-{num}.{
                                            ais.LocationIndicatorICAO
                                        }-{Lib.AIPLanguage}.html"" class=""Cell-body"">AD {num} {
                                            ais.LocationIndicatorICAO
                                        }</a>";

                                dic["TrafficType_NTL"] =
                                    dic["TrafficType_IFR"] = dic["TrafficType_Sch"] = "";
                                foreach (AirportHeliportAvailability ahav in ais.Availability)
                                {
                                    foreach (AirportHeliportUsage ahu in ahav.Usage)
                                    {
                                        foreach (FlightCharacteristic fc in ahu.Selection.Flight)
                                        {
                                            dic["TrafficType_NTL"] += fc.Origin + ",";
                                            dic["TrafficType_IFR"] += fc.Rule + ",";
                                            dic["TrafficType_Sch"] += fc.Purpose + ",";
                                        }
                                    }
                                }
                                dic["TrafficType_NTL"] = CommaStringToUniqueString(dic["TrafficType_NTL"]);
                                dic["TrafficType_IFR"] = CommaStringToUniqueString(dic["TrafficType_IFR"]);
                                dic["TrafficType_Sch"] = CommaStringToUniqueString(dic["TrafficType_Sch"]);


                                ObjList.Add(dic);
                            }
                            data.Row = ObjList;
                            dataList.Add(data);
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
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD15(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                string output = "";
                List<ForAD15> dataList = new List<ForAD15>();
                List<Dictionary<string, string>> ObjList = new List<Dictionary<string, string>>();

                var ftgroup = Globals.GetFeaturesByED(FeatureType.AirportHeliport)?
                    .Cast<Aran.Aim.Features.AirportHeliport>()
                    .Where(x=>x.CertifiedICAO == true)
                    .ToList()
                    .OrderBy(x=>x.Name)
                    .GroupBy(n => n.Type);

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    if (ftgroup != null)
                        foreach (var ft in ftgroup.OrderBy(x=>x.Key))
                        {
                            ForAD15 data = new ForAD15();
                            ObjList = new List<Dictionary<string, string>>();
                            data.type = Lib.GetEnumText(ft.Key);
                            data.TitleName = (ft.Key == CodeAirportHeliport.AD || ft.Key == CodeAirportHeliport.AH)
                                ? Tpl.Text("AD15_AD") : Tpl.Text("AD15_HP");
                            foreach (Aran.Aim.Features.AirportHeliport ais in ft)
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                dic["Name"] = ais.Name;
                                dic["LocationIndicatorICAO"] = ais.LocationIndicatorICAO;
                                dic["CertificationDate"] = ais.CertificationDate?.ToShortDateString();
                                dic["CertificationValidity"] = ais.CertificationExpirationDate?.ToShortDateString() ?? "Unlimited";
                                dic["Remarks"] = Lib.AIXM_GetNotesByPropertyName(ais.Annotation, new List<string>
                                {
                                    "CertificationDate",
                                    "CertificationExpirationDate"
                                }); 
                                ObjList.Add(dic);
                            }
                            data.Row = ObjList;
                            dataList.Add(data);
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
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }

}