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
using RazorEngine.Compilation.ImpromptuInterface;
using AirportHeliport = Aran.Aim.Features.AirportHeliport;
using InformationService = Aran.Aim.Features.InformationService;
using Navaid = Aran.Aim.Features.Navaid;
using Point = Aran.Geometries.Point;

namespace AIP.GUI.Classes
{
    /// <summary>
    /// Class to transfer Data from AIXM to AIP DB
    /// GEN section
    /// Other sections are the part of FillDB
    /// </summary>
    internal static partial class FillDB
    {
        public static void Fill_GEN02(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
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




        public static void Fill_GEN03(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    ent.Title = GetSectionTitle(ent.SectionName.ToString());
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

        public static void Fill_GEN11(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {

                // Order is important here:
                List<CodeOrganisation?> ParamsOrdered = new List<CodeOrganisation?>()
                {
                    CodeOrganisation.STATE,
                    CodeOrganisation.STATE_GROUP,
                    CodeOrganisation.ORG,
                    CodeOrganisation.NTL_AUTH,
                    CodeOrganisation.ATS
                };
                int cnt = 0;
                List<OrganisationAuthority> ft = Globals.GetFeaturesByED(FeatureType.OrganisationAuthority)
                    .Cast<OrganisationAuthority>().Where(n => ParamsOrdered.Contains(n.Type) && Lib.IsLanguageNote(n.Annotation)).OrderBy(x=>x.Type).ThenBy(x=>x.Name).ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (Aran.Aim.Features.OrganisationAuthority orgAuth in ft)
                    {
                        string output = "";
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = orgAuth.Name.ToString();
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(orgAuth.Type)*100 + cnt++;
                        foreach (ContactInformation adr in orgAuth.Contact)
                        {
                            output = ContactInformationToXHTML(adr);
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
                return;
            }
        }

        public static void Fill_GEN21(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);

                List<SpecialDate> sdList = Globals.GetFeaturesByED(FeatureType.SpecialDate)
                    .Cast<SpecialDate>().Where(n => n.Type == CodeSpecialDate.HOL)
                    .ToList();

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();

                    // TODO: when will be fixed error
                    // 2.1.1
                     InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_UNITS_OF_MEASUREMENT, 1, ref ss);
                    // 2.1.2
                     InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_TEMPORAL_REFERENCE_SYSTEM, 2, ref ss);
                    // 2.1.3
                     InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_HORIZONTAL_REFERENCE_SYSTEM, 3, ref ss);
                    // 2.1.4
                     InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_VERTICAL_REFERENCE_SYSTEM, 4, ref ss);
                    // 2.1.5
                     InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_AIRCRAFT_NATIONALITY_AND_REGISTRATION_MARKS, 5, ref ss);
                    // 3.5.7
                    List<ForGEN21_6> dataList = new List<ForGEN21_6>();
                    foreach (SpecialDate sd in sdList.OrderBy(x=>x.Name))
                    {
                        ForGEN21_6 data = new ForGEN21_6();
                        data.Name = sd.Name;
                        data.Date = sd.DateDay + " " + sd.DateYear;
                        dataList.Add(data);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN216");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 6;

                        string output = Razor.Run(dataList);
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
                return;
            }
        }

        public static void Fill_GEN24(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                    LocationTable l1 = new LocationTable();
                    List<LocationTable> l1_List = new List<LocationTable>();

                    List<LocationDefinition> l2_List = new List<LocationDefinition>();
                    l1.AIPSection = ent;
                    l1.eAIP = caip;
                    l1.SubClassType = SubClassType.LocationTable;
                    foreach (AirportHeliport airHel in Lib.GetAIXMAirportHeliport())
                    {
                        LocationDefinition l2 = new LocationDefinition();
                        l2.LocationIdent = airHel.LocationIndicatorICAO;
                        l2.LocationName = airHel.Name;
                        l2.LocationDefinitionType = (airHel.CertifiedICAO == true)
                            ? LocationDefinitionType.ICAO
                            : LocationDefinitionType.Nonstandard;
                        l2.LocationDefinitionAFS = (airHel?.Contact?
                            .SelectManyNullSafe(x => x.NetworkNode?.Select(n => n.Network == CodeTelecomNetwork.AFTN))?.Count() > 0) ? YesNo.No : YesNo.Yes;
                        l2.LocationTable = l1;
                        l2.eAIP = caip;
                        l2_List.Add(l2);

                    }
                    // db.Database.Log = Console.Write;
                    l1_List.Add(l1);
                    //ent.Children = l1_List.ToArray();
                    db.LocationTable.Add(l1);
                    db.LocationDefinition.AddRange(l2_List);
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return;
            }
        }

        public static void Fill_GEN25(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<Navaid> navaidList = Globals.GetFeaturesByED(FeatureType.Navaid)
                    .Cast<Navaid>()
                    .ToList();
                var VOR = Globals.GetFeaturesByED(FeatureType.VOR).Cast<VOR>().ToList();
                var DME = Globals.GetFeaturesByED(FeatureType.DME).Cast<DME>().ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    List<ForGEN25> dataList = new List<ForGEN25>();
                    foreach (Navaid nav in navaidList.OrderBy(x => x.Designator))
                    {
                        ForGEN25 data = new ForGEN25();

                        var eqs = nav.NavaidEquipment.Select(n => n.TheNavaidEquipment?.Identifier).ToList();
                        IEnumerable<VOR> vors = VOR.Where(n => eqs.Contains(n.Identifier));
                        IEnumerable<DME> dmes = DME.Where(n => eqs.Contains(n.Identifier));
                        List<string> types = new List<string>();
                        foreach (VOR item in vors)
                        {
                            types.Add((item.Type != null) ? item.Type.ToString() : item.NavaidEquipmentType.ToString());
                        }
                        foreach (DME item in dmes)
                        {
                            types.Add((item.Type != null) ? item.Type.ToString() : item.NavaidEquipmentType.ToString());
                        }
                        data.Id = nav.Designator;
                        data.StationName = nav.Name;
                        data.Facility = nav.Type == CodeNavaidService.VOR_DME ? string.Join("/", types) : nav.Type.ToString().Replace("_","/");
                        data.Purpose = nav.Purpose?.ToString().Substring(0,1);
                        dataList.Add(data);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN25");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 1;

                        string output = Razor.Run(dataList);
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
                return;
            }
        }

        public static void Fill_GEN31(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                {
                    CodeRuleProcedureTitle.OTHER_RESPONSIBLE_SERVICE,
                    CodeRuleProcedureTitle.OTHER_GEN_3_1_2_AREA_OF_RESPONSIBILITY,
                    CodeRuleProcedureTitle.OTHER_AERONAUTICAL_PUBLICATIONS,
                    CodeRuleProcedureTitle.OTHER_AIRAC_SYSTEM,
                    CodeRuleProcedureTitle.OTHER_PRE_FLIGHT_INFORMATION_SERVICE_AT_AERODROMES_HELIPORTS
                };
                List<RulesProcedures> ft = Globals.GetFeaturesByED(FeatureType.RulesProcedures).Cast<RulesProcedures>().Where(n => ParamsOrdered.Contains(n.Title) && IsFIRAirspace(n.AffectedArea) && Lib.IsLanguageNote(n.Annotation)).ToList();

                List<InformationService> isl = Globals.GetFeaturesByED(FeatureType.InformationService).Cast<InformationService>().Where(n => n.Type == CodeServiceInformation.AIS && Lib.IsLanguageNote(n.Annotation)).ToList();

                var isl_Guid = isl?.Select(x => x.ServiceProvider?.Identifier).ToList();

                List<Unit> unitl = Globals.GetFeaturesByED(FeatureType.Unit).Cast<Unit>().Where(n => n.Type == CodeUnit.AOF && Lib.IsLanguageNote(n.Annotation) && isl_Guid.ContainsValue(n.Identifier)).ToList();


                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // 3.1.1 PreText
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_RESPONSIBLE_SERVICE);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        ss.Title = Lib.HeaderWithNumbering("GEN311");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();
                    }


                    // 3.1.1 Continue with post addresses
                    string timesheet = "";
                    foreach (Unit unit in unitl)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        //ss.Title = unit.Name;
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        foreach (ContactInformation adr in unit.Contact)
                        {
                            ss.Content += ContactInformationToXHTML(adr);
                        }
                        timesheet += Lib.GetHoursOfOperations(unit.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());
                        //                       db.Subsection.Add(ss);
                    }
                    
                    // 3.1.1 - availability goes here
                    ss.Content += timesheet;

                    // then first subsection complete
                    db.Subsection.Add(ss);

                    // 3.1.2
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_GEN_3_1_2_AREA_OF_RESPONSIBILITY);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        ss.Title = Lib.HeaderWithNumbering("GEN312");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();
                        db.Subsection.Add(ss);
                    }

                    // 3.1.3
                    ss = new DB.Subsection();
                    foreach (Aran.Aim.Features.RulesProcedures rpl in ft.Where(n =>
                    n.Title == CodeRuleProcedureTitle.OTHER_AERONAUTICAL_PUBLICATIONS))
                    {
                        ss.AIPSection = ent;
                        ss.Title = Lib.HeaderWithNumbering("GEN313");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rpl.Title);
                        ss.Content += rpl.Content.ToXhtml();
                    }
                    db.Subsection.Add(ss);

                    // 3.1.4
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_AIRAC_SYSTEM);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        ss.Title = Lib.HeaderWithNumbering("GEN314");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();
                        db.Subsection.Add(ss);
                    }

                    // 3.1.5
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_PRE_FLIGHT_INFORMATION_SERVICE_AT_AERODROMES_HELIPORTS);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        ss.Title = Lib.HeaderWithNumbering("GEN315");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();

                    }
                    db.Subsection.Add(ss);

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
        

        public static void Fill_GEN32(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);

                List<InformationService> isl = Globals.GetFeaturesByED(FeatureType.InformationService).Cast<InformationService>().Where(n => n.Type == CodeServiceInformation.OTHER_CARTO_SERVICE && Lib.IsLanguageNote(n.Annotation)).ToList();
                var isl_Guid = isl?.Select(x => x.ServiceProvider?.Identifier).ToList();

                List<Unit> unitl = Globals.GetFeaturesByED(FeatureType.Unit).Cast<Unit>().Where(n => n.Type == CodeUnit.OTHER_CARTO && Lib.IsLanguageNote(n.Annotation) && isl_Guid.ContainsValue(n.Identifier)).ToList();

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    // 3.2.1 post addresses
                    foreach (Unit unit in unitl)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.HeaderWithNumbering("GEN321");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        foreach (ContactInformation adr in unit.Contact)
                        {
                            ss.Content += ContactInformationToXHTML(adr);
                        }

                        ss.Content += Lib.AIXM_GetNotesByPurpose(isl.SelectManyNullSafe(x => x.Annotation).ToList(), CodeNotePurpose.OTHER_ICAO_DOC_BASIS);
                        ss.Content += Lib.GetHoursOfOperations(unit.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());
                        ss.OrderNumber = 1;
                        db.Subsection.Add(ss);
                    }

                    // 3.2.2 OTHER_AIP_AERO_CHART_REVISION_AMENDMENT
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_MAINTENANCE_OF_CHARTS, 2, ref ss);

                    // 3.2.3 CodeUnit.OTHER_AERO_CHART_SALES_UNIT
                    InsertUnit(ent, caip, CodeUnit.OTHER_AERO_CHART_SALES_UNIT, 3);

                    // 3.2.4 OTHER_AIP_AERO_CHART_SERIES_AVAILABILITY
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_AERONAUTICAL_CHART_SERIES_AVAILABLE, 4, ref ss);

                    // 3.2.5 OTHER_AIP_AERO_CHART_AVAILABILITY_LIST
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_LIST_OF_AERONAUTICAL_CHARTS_AVAILABLE, 5, ref ss);

                    // 3.2.6 OTHER_WAC_INDEX
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_INDEX_TO_THE_WORLD_AERONAUTICAL_CHART_WAC, 6, ref ss);

                    // 3.2.7 CodeUnit.OTHER_AERO_CHART_SALES_UNIT
                    InsertUnit(ent, caip, CodeUnit.OTHER_TOPO_CHART_SALES_UNIT, 7);

                    // 3.2.8 OTHER_WAC_INDEX
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_CORRECTIONS_TO_CHARTS_NOT_CONTAINED_IN_THE_AIP, 8, ref ss);

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


        public static void Fill_GEN33(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<RPHelper> RPHelperList = RPSection.FirstOrDefault(x=>x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);
                List<CodeServiceATC?> CodeServiceList = new List<CodeServiceATC?>()
                {
                    CodeServiceATC.ACS,
                    CodeServiceATC.UAC,
                    CodeServiceATC.OACS,
                    CodeServiceATC.EFAS
                };
                List<CodeUnit?> CodeUnitList = new List<CodeUnit?>()
                {
                    CodeUnit.ATSU,
                    CodeUnit.ATCC,
                    CodeUnit.ATMU,
                    CodeUnit.ATFMU
                };

                List<OrganisationAuthority> oaList = Globals.GetFeaturesByED(FeatureType.OrganisationAuthority)
                    .Cast<OrganisationAuthority>()
                    .ToList();

                List<AirTrafficControlService> atcs = Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)
                    .Cast<AirTrafficControlService>()
                    .Where(n => CodeServiceList.Contains(n.Type) && Lib.IsLanguageNote(n.Annotation))
                    .OrderBy(x=>x.Name)
                    .ToList();
                var atcs_Guid = atcs?.Select(x => x.ServiceProvider?.Identifier).ToList();

                List<Unit> unitAll = Globals.GetFeaturesByED(FeatureType.Unit)
                    .Cast<Unit>()
                    .Where(n => Lib.IsLanguageNote(n.Annotation) && atcs_Guid.ContainsValue(n.Identifier))
                    .ToList();
                List<Unit> unit1 = unitAll
                    .Where(n => CodeUnitList.Contains(n.Type))
                    .OrderBy(x=>x.Name)
                    .ToList();
                

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    // 3.3.1 post addresses
                    foreach (Unit unit in unit1)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN331");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        foreach (ContactInformation adr in unit.Contact)
                        {
                            ss.Content += ContactInformationToXHTML(adr);
                        }

                        ss.Content += Lib.AIXM_GetNotesByPurpose(atcs.SelectManyNullSafe(x => x.Annotation).ToList(), CodeNotePurpose.OTHER_ICAO_DOC_BASIS);
                        ss.Content += Lib.GetHoursOfOperations(unit.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());
                        ss.OrderNumber = 1;
                        db.Subsection.Add(ss);
                    }

                    // 3.3.2 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_3_2_AREA_OF_RESPONSIBILITY, 2, ref ss);

                    // 3.3.3 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_3_3_TYPES_OF_SERVICES, 3, ref ss);

                    // 3.3.4 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_COORDINATION_BETWEEN_THE_OPERATOR_AND_ATS, 4, ref ss);

                    // 3.3.5 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_MINIMUM_FLIGHT_ALTITUDE, 5, ref ss);
                    
                    // 3.3.6 List of Unit
                    List<ForGEN33_6> dataList = new List<ForGEN33_6>();
                    foreach (AirTrafficControlService atcsItem in atcs)
                    {
                        ForGEN33_6 data = new ForGEN33_6();

                        Unit unit = unitAll.FirstOrDefault(x => x.Identifier == atcsItem.ServiceProvider?.Identifier);
                        if (unit != null)
                        {
                            
                            data.UnitName = atcsItem.Name;
                            OrganisationAuthority oa = oaList.FirstOrDefault(x => x.Identifier == unit?.OwnerOrganisation?.Identifier);
                            if (oa != null)
                            {
                                var postalAddress = oa.Contact?.SelectManyNullSafe(x => x.Address).FirstOrDefault();
                                data.AdministrativeArea = postalAddress?.AdministrativeArea;
                                data.City = postalAddress?.City;
                                data.Country = postalAddress?.Country;
                                data.PostalCode = postalAddress?.PostalCode;

                                data.Phone = oa.Contact?.SelectManyNullSafe(x => x.PhoneFax).ToList()?.Select(x => x.Voice)
                                    .ToList().ToSeparatedValues().NilIfEmpty();
                                data.Fax = oa.Contact?.SelectManyNullSafe(x => x.PhoneFax).ToList()?.Select(x => x.Facsimile)
                                    .ToList().ToSeparatedValues().NilIfEmpty();
                                data.Email = oa.Contact?.SelectManyNullSafe(x => x.NetworkNode).ToList()?.Where(x => x.Protocol == "INTERNET").Select(x=>x.eMail).ToList().ToSeparatedValues().NilIfEmpty();
                                data.URL = oa.Contact?.SelectManyNullSafe(x => x.NetworkNode).ToList()?.Where(x => x.Protocol == "INTERNET").Select(x => x.Linkage).ToList().ToSeparatedValues().NilIfEmpty();
                                data.AFSAddress = oa.Contact?.SelectManyNullSafe(x => x.NetworkNode).ToList()?.Where(x => x.Protocol == "AFTN").Select(x => x.Linkage).ToList().ToSeparatedValues().NilIfEmpty();
                            }
                        }
                        dataList.Add(data);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN336");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 6;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }
                    ////


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

        public static void Fill_GEN34(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);

                List<OrganisationAuthority> oaList = Globals.GetFeaturesByED(FeatureType.OrganisationAuthority)
                    .Cast<OrganisationAuthority>().Where(n => n.Type == CodeOrganisation.OTHER_TELECOM_NAV_REGULATOR)
                    .ToList();

                var oaList_Guid = oaList.ToGuidList();

                List<Unit> unitl = Globals.GetFeaturesByED(FeatureType.Unit).Cast<Unit>().Where(n => n.Type == CodeUnit.COM && Lib.IsLanguageNote(n.Annotation) && oaList_Guid.ContainsValue(n.OwnerOrganisation?.Identifier)).ToList();

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    // 3.4.1 post addresses
                    foreach (OrganisationAuthority oa in oaList)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN341");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        foreach (ContactInformation adr in oa.Contact)
                        {
                            ss.Content += ContactInformationToXHTML(adr);
                        }

                        ss.Content += Lib.AIXM_GetNotesByPurpose(oaList.SelectManyNullSafe(x => x.Annotation).ToList(), CodeNotePurpose.OTHER_ICAO_DOC_BASIS);
                        ss.Content += Lib.AIXM_GetNotesByPurpose(oaList.SelectManyNullSafe(x=>x.Annotation).ToList(), CodeNotePurpose.OTHER_HOURS_OF_SERVICE);
                        ss.OrderNumber = 1;
                        db.Subsection.Add(ss);
                    }

                    // 3.4.2 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_4_2_AREA_OF_RESPONSIBILITY, 2, ref ss);

                    // 3.4.3 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_4_3_TYPES_OF_SERVICES, 3, ref ss);

                    // 3.4.4 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_REQUIREMENTS_AND_CONDITIONS, 4, ref ss);

                    // 3.4.5 
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_MISCELLANEOUS, 5, ref ss);

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

        public static void Fill_GEN35(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);


                List<InformationService> isList = Globals.GetFeaturesByED(FeatureType.InformationService)
                    .Cast<InformationService>().Where(n => n.Type == CodeServiceInformation.RAF)
                    .ToList();
                var isList_Guid = isList?.Select(x => x.ServiceProvider?.Identifier).ToList();
                List<Unit> unit1 = Globals.GetFeaturesByED(FeatureType.Unit).Cast<Unit>().Where(n => n.Type == CodeUnit.MET && Lib.IsLanguageNote(n.Annotation) && isList_Guid.ContainsValue(n.Identifier)).ToList();

                List<InformationService> is7List = Globals.GetFeaturesByED(FeatureType.InformationService)
                    .Cast<InformationService>().Where(n => n.Type == CodeServiceInformation.VOLMET)
                    .ToList();
                //var is7List_Guid = is7List?.Select(x => x.ServiceProvider.Identifier).ToList();
                List<Unit> unitAll = Globals.GetFeaturesByED(FeatureType.Unit).Cast<Unit>().Where(n => Lib.IsLanguageNote(n.Annotation)).ToList();
                List<RadioCommunicationChannel> rccAll = Globals.GetFeaturesByED(FeatureType.RadioCommunicationChannel)
                    .Cast<RadioCommunicationChannel>()
                    .ToList();
                List<AirportHeliport> ahList = Globals.GetFeaturesByED(FeatureType.AirportHeliport)
                    .Cast<AirportHeliport>()
                    .ToList();

                List<InformationService> is8List = Globals.GetFeaturesByED(FeatureType.InformationService)
                    .Cast<InformationService>()
                    .ToList();
                var is8List_Guid = is8List?
                    .Where(x => x.ServiceProvider?.Identifier != null)
                    .Select(x => x.ServiceProvider.Identifier)
                    .ToList();
                

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    // 3.5.1 post addresses
                    foreach (Unit unit in unit1)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN351");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        foreach (ContactInformation adr in unit.Contact)
                        {
                            ss.Content += ContactInformationToXHTML(adr);
                        }

                        ss.Content += Lib.AIXM_GetNotesByPurpose(isList.SelectManyNullSafe(x => x.Annotation).ToList(), CodeNotePurpose.OTHER_ICAO_DOC_BASIS);
                        ss.Content += Lib.GetHoursOfOperations(unit.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());
                        ss.OrderNumber = 1;
                        db.Subsection.Add(ss);
                    }

                    // 3.5.2 OTHER_AIP_MET_AREA_OF_RESPONSIBILITY
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_5_2_AREA_OF_RESPONSIBILITY, 2, ref ss);

                    // 3.5.3 OTHER_AIP_MET_OBSERVATIONS_AND_REPORTS
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_METEOROLOGICAL_OBSERVATIONS_AND_REPORTS, 3, ref ss);

                    // 3.5.4 OTHER_AIP_MET_SERVICE_TYPES
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_5_4_TYPES_OF_SERVICES, 4, ref ss);

                    // 3.5.5 OTHER_AIP_MET_OPER_NOTIFICATION_REQUIRED
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_NOTIFICATION_REQUIRED_FROM_OPERATORS, 5, ref ss);

                    // 3.5.6 OTHER_AIP_MET_AIRCRAFT_REPORTS
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_AIRCRAFT_REPORTS, 6, ref ss);

                    // 3.5.7
                    List<ForGEN35_7> dataList = new List<ForGEN35_7>();
                    foreach (InformationService is7Item in is7List)
                    {
                        ForGEN35_7 data = new ForGEN35_7();

                        data.CallSign = is7Item.CallSign.Select(x => x.CallSign).ToList().ToSeparatedValues();
                        var RCCGuidList = is7Item.RadioCommunication.Select(x => x.Feature?.Identifier).ToList();
                        List<RadioCommunicationChannel> RCCList = rccAll.Where(x => RCCGuidList.ContainsValue(x.Identifier)).ToList();
                        
                        data.Frequency = RCCList?.Select(x => x.FrequencyTransmission?.StringValue).ToList().ToSeparatedValues();
                        data.BroadcastPeriod = "".NilIfEmpty();
                        data.HoursOfService =
                            Lib.GetHoursOfOperations(is7Item.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());

                        var ahGuidList = is7Item.ClientAirport.Select(x => x.Feature?.Identifier).ToList();
                        data.AHIncluded = ahList.Where(x => ahGuidList.ContainsValue(x.Identifier)).Select(x => x.Name).ToList().ToSeparatedValues("<br />");
                        data.ContentsFormatRemarks = Lib.AIXM_GetNotes(is7Item.Annotation);

                        Unit unit = unitAll.FirstOrDefault(x => x.Identifier == is7Item?.ServiceProvider?.Identifier);
                        if (unit != null){

                            data.Name = is7Item.Name;
                        }
                        dataList.Add(data);
                    }

                    if (dataList.Count > 0)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN357");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = 7;

                        string output = Razor.Run(dataList);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }
                    //

                    // 3.5.8
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_SIGMET_AND_AIRMET_SERVICE, 8, ref ss);

                    //List<ForGEN35_8> data8List = new List<ForGEN35_8>();
                    //foreach (InformationService is8Item in is8List)
                    //{
                    //    ForGEN35_8 data8 = new ForGEN35_8();
                    //    var ah8GuidList = is8Item.ClientAirport.Select(x => x.Feature?.Identifier).ToList();
                    //    data8.FIRorCTA = ahList.Where(x => ah8GuidList.ContainsValue(x.Identifier)).Select(x => x.Name).ToList().ToSeparatedValues("<br />");
                    //    data8.SigmetValidity = Lib.AIXM_GetNotesByPurpose(is8Item.Annotation, CodeNotePurpose.OTHER_SIGMET_VALIDITY_PERIODS);
                    //    Unit unit = unitAll.FirstOrDefault(x => x.Type == CodeUnit.MWO && x.Identifier == is8Item?.ServiceProvider?.Identifier && Lib.IsLanguageNote(x.Annotation));
                    //    if (unit != null)
                    //    {
                    //        data8.Name = unit.Name;
                    //        data8.Name += " " + unit.Contact
                    //            .SelectManyNullSafe(x =>
                    //                x.NetworkNode.Where(n => n.Network == CodeTelecomNetwork.AFTN)
                    //                    .Select(i => i.Linkage)).ToList().ToSeparatedValues();
                    //        data8.Hours = Lib.GetHoursOfOperations(unit.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());
                    //    }
                    //    //data8.SpecificProcedures =
                    //    //    ft.FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_SIGMET_PROCS)?.Content.ToXhtml();
                    //    //data8.SpecificProcedures +=
                    //    //    ft.FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_AIRMET_PROCS)?.Content.ToXhtml();
                    //    data8.ATSUnitServed = "".NilIfEmpty(); // No mapping
                    //    data8.AdditionalInfo = Lib.AIXM_GetNotes(is8Item.Annotation);
                    //    data8List.Add(data8);
                    //}

                    //if (data8List.Count > 0)
                    //{
                    //    ss = new DB.Subsection();
                    //    ss.AIPSection = ent;
                    //    ss.Title = Lib.GetText("GEN358");
                    //    ss.eAIP = caip;
                    //    ss.SubClassType = SubClassType.Subsection;
                    //    ss.OrderNumber = 8;

                    //    string output = Razor.Run(data8List);
                    //    if (output == null)
                    //    {
                    //        SendOutput("Error in generating template ", Percent: 80);
                    //        return;
                    //    }
                    //    ss.Content = output;
                    //    db.Subsection.Add(ss);
                    //}
                    ////

                    // 3.5.9 OTHER_AIP_MET_OTHER_SERVICES
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_OTHER_AUTOMATED_METEOROLOGICAL_SERVICES, 9, ref ss);

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


        public static void Fill_GEN36(AIPSection ent, List<Feature> featureList, DB.eAIP caip)
        {
            try
            {
                List<RPHelper> RPHelperList =
                    RPSection.FirstOrDefault(x => x.Title == ent.SectionName.ToString())?.RPHelperList;
                List<RulesProcedures> ft = GetRP(RPHelperList);
                List<CodeServiceSAR?> srsCodeList = new List<CodeServiceSAR?>()
                {
                    CodeServiceSAR.SAR,
                    CodeServiceSAR.RCC,
                    CodeServiceSAR.ALRS
                };

                List<SearchRescueService> srsList = Globals.GetFeaturesByED(FeatureType.SearchRescueService)
                    .Cast<SearchRescueService>().Where(n => srsCodeList.Contains(n.Type))
                    .ToList();
                
                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    // 3.6.1 post addresses
                    foreach (SearchRescueService srs in srsList)
                    {
                        ss = new DB.Subsection();
                        ss.AIPSection = ent;
                        ss.Title = Lib.GetText("GEN341");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        foreach (ContactInformation adr in srs.GroundCommunication)
                        {
                            ss.Content += ContactInformationToXHTML(adr, srs.Name);
                        }
                        ss.Content += Lib.AIXM_GetNotesByPurpose(srs.Annotation, CodeNotePurpose.OTHER_ICAO_DOC_BASIS);
                        ss.OrderNumber = 1;
                        db.Subsection.Add(ss);
                    }

                    // 3.6.2 OTHER_AIP_SAR_AREA_OF_RESPONSIBILITY
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_6_2_AREA_OF_RESPONSIBILITY, 2, ref ss);

                    // 3.6.3 OTHER_AIP_SAR_SERVICE_TYPES
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_GEN_3_6_3_TYPES_OF_SERVICES, 3, ref ss);

                    // 3.6.4 OTHER_AIP_SAR_AGREEMENTS
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_SEARCH_AND_RESCUE_AGREEMENTS, 4, ref ss);

                    // 3.6.5 OTHER_AIP_SAR_AVAILABILITY_CONDITIONS
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_CONDITIONS_OF_AVAILABILITY, 5, ref ss);

                    // 3.6.6 OTHER_AIP_SAR_PROCEDURES_AND_SIGNALS
                    InsertRP(ent, caip, ft, CodeRuleProcedureTitle.OTHER_PROCEDURES_AND_SIGNALS_USED, 6, ref ss);

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

        private static void InsertUnit(AIPSection ent, eAIP caip, CodeUnit type, int SubsectionNumber)
        {
            try
            {
                List<Unit> unitList = Lib.GetUnitByType(type);
                foreach (Unit unit in unitList)
                {
                    DB.Subsection ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.Title = Lib.GetText(ent.SectionName.ToString() + SubsectionNumber);
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    foreach (ContactInformation adr in unit.Contact)
                    {
                        ss.Content += ContactInformationToXHTML(adr);
                    }
                    ss.OrderNumber = SubsectionNumber;
                    db.Subsection.Add(ss);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void InsertRP(AIPSection ent, eAIP caip, List<RulesProcedures> ft, CodeRuleProcedureTitle CRPT, int SubsectionNumber, ref Subsection ss)
        {
            try
            {
                RulesProcedures rp = ft.FirstOrDefault(x => x.Title == CRPT);
                if (rp != null)
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.Title = Lib.HeaderWithNumbering(ent.SectionName.ToString() + SubsectionNumber);
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = SubsectionNumber;
                    ss.Content += rp.Content.ToXhtml();
                    db.Subsection.Add(ss);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }

}