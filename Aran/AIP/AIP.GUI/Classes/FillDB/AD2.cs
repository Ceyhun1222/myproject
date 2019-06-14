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
using CodeNotePurpose = Aran.Aim.Enums.CodeNotePurpose;
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
        #region AD2
        public static void Fill_AD20(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    //SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD21(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    //SendOutput("Saving data...", Percent: 90);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD224(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                Fill_FileSection(ent, featureList, caip, air);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void Fill_AD22(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD22 Data = new ForAD22();
                string output = "";
                List<OrganisationAuthority> oa = Globals.GetFeaturesByED(FeatureType.OrganisationAuthority)?.Cast<Aran.Aim.Features.OrganisationAuthority>().ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    Data.ARP = air.ARP?.Geo?.ToPointString();
                    Data.ADsite = Lib.AIXM_GetNotesByPropertyName(air.Annotation, "referencePointDescription");
                    Data.Direction = Lib.AIXM_GetNotesByPropertyName(air.Annotation, "locationDescription");
                    Data.Elevation = air.FieldElevation?.StringValue;
                    Data.RefTemperature = air.ReferenceTemperature?.StringValue;
                    Data.Geoid = air.ARP?.GeoidUndulation?.StringValue;
                    Data.MagneticVariation = air.MagneticVariation?.ToString();
                    Data.DateMagneticVariation = air.DateMagneticVariation;
                    Data.MagneticVariationChange = air.MagneticVariationChange?.ToString();
                    var tmp1 = oa?.Where(n => n.Identifier == air.ResponsibleOrganisation.TheOrganisationAuthority?.Identifier).FirstOrDefault()?
                        .Contact?.FirstOrDefault()?.PhoneFax?.FirstOrDefault();
                    OnlineContact tmp2 = oa?.Where(n => n.Identifier == air.ResponsibleOrganisation.TheOrganisationAuthority?.Identifier).FirstOrDefault()?
                        .Contact?.SelectManyNullSafe(x => x.NetworkNode).FirstOrDefault(x => x.Network == CodeTelecomNetwork.INTERNET);

                    Data.NameOfAdministration_linkage = tmp2?.Linkage;
                    Data.NameOfAdministration_email = tmp2?.eMail;
                    Data.NameOfAdministration_fax = tmp1?.Facsimile;
                    Data.NameOfAdministration_voice = tmp1?.Voice;

                    Data.TypeTraffic = "";
                    foreach (AirportHeliportAvailability ahav in air.Availability)
                    {
                        foreach (AirportHeliportUsage ahu in ahav.Usage)
                        {
                            foreach (FlightCharacteristic fc in ahu.Selection.Flight)
                            {
                                Data.TypeTraffic += fc.Rule + ",";
                            }
                        }
                    }
                    Data.TypeTraffic = CommaStringToUniqueString(Data.TypeTraffic);

                    Data.Remark = Lib.AIXM_GetNotes(air.Annotation);

                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD23(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD23 Data = new ForAD23();
                string output = "";

                List<PassengerService> ps = Globals.GetFeaturesByED(FeatureType.PassengerService)?.Cast<Aran.Aim.Features.PassengerService>()
                    .ToList();
                List<Unit> unit = Globals.GetFeaturesByED(FeatureType.Unit)?.Cast<Aran.Aim.Features.Unit>()
                    .ToList();
                List<AirTrafficControlService> ats = Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)?.Cast<Aran.Aim.Features.AirTrafficControlService>()
                    .ToList();
                List<AirportSuppliesService> ass = Globals.GetFeaturesByED(FeatureType.AirportSuppliesService)?.Cast<Aran.Aim.Features.AirportSuppliesService>()
                    .ToList();
                List<AircraftGroundService> ags = Globals.GetFeaturesByED(FeatureType.AircraftGroundService)?.Cast<Aran.Aim.Features.AircraftGroundService>()
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    Data.ADAdministration = (air.ResponsibleOrganisation.Role == CodeAuthorityRole.OPERATE) ?
                        Lib.GetHoursOfOperations(air.ResponsibleOrganisation.TimeInterval) : "";
                    Data.ADAdministration = Data.ADAdministration.NilIfEmpty();

                    Data.CustomAndImmigration = ps?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                    && n.Type == CodePassengerService.CUST)
                    .SelectManyNullSafe(x => x.Availability.Select(y => y.OperationalStatus.ToString()))
                    .ToList()
                    .ToSeparatedValues().NilIfEmpty();

                    Data.HealthAndSanitation = ps?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                    && n.Type == CodePassengerService.SAN)
                    .SelectManyNullSafe(x => x.Availability.Select(y => y.OperationalStatus.ToString()))
                    .ToList()
                    .ToSeparatedValues().NilIfEmpty();

                    Data.AISBriefingOffice = Lib.GetHoursOfOperations(unit
                        ?.Where(n => n.AirportLocation?.Identifier == air.Identifier && n.Type == CodeUnit.BOF)
                        .SelectManyNullSafe(x => x.Availability
                        .SelectManyNullSafe(y => y.TimeInterval)).ToList())
                        .NilIfEmpty();
                    Data.ATSReportingOffice = Lib.GetHoursOfOperations(unit
                        ?.Where(n => n.AirportLocation?.Identifier == air.Identifier && n.Type == CodeUnit.ARO)
                        .SelectManyNullSafe(x => x.Availability
                            .SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();
                    Data.METBriefingOffice = Lib.GetHoursOfOperations(unit
                        ?.Where(n => n.AirportLocation?.Identifier == air.Identifier && n.Type == CodeUnit.MET)
                        .SelectManyNullSafe(x => x.Availability
                            .SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();
                    Data.ATS = Lib.GetHoursOfOperations(ats?.Where(n => n.ClientAirport.Select(x => x.Feature.Identifier)
                    .Contains(air.Identifier))
                        .SelectManyNullSafe(
                        x => x.Availability
                        .SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();
                    Data.Fuelling = Lib.GetHoursOfOperations(ass?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                            .Contains(air.Identifier))
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();

                    Data.Handling = Lib.GetHoursOfOperations(ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                            .Contains(air.Identifier) && n.Type == CodeAircraftGroundService.HAND)
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();
                    Data.Security = Lib.GetHoursOfOperations(ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                                                                                 .Contains(air.Identifier) && n.Type == CodeAircraftGroundService.OTHER)
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();
                    Data.Deicing = Lib.GetHoursOfOperations(ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                                                                                 .Contains(air.Identifier) && n.Type == CodeAircraftGroundService.DEICE)
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();


                    Data.Remarks = Lib.AIXM_GetNotes(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                        .Contains(air.Identifier))
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.AHAvailability = Lib.GetHoursOfOperations(air.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList()).NilIfEmpty();
                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD24(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD24 Data = new ForAD24();
                string output = "";

                List<AircraftGroundService> ags = Globals.GetFeaturesByED(FeatureType.AircraftGroundService)?.Cast<Aran.Aim.Features.AircraftGroundService>()
                    .ToList();
                List<AirportSuppliesService> ass = Globals.GetFeaturesByED(FeatureType.AirportSuppliesService)?.Cast<Aran.Aim.Features.AirportSuppliesService>()
                    .ToList();
                List<ApronElement> apron = Globals.GetFeaturesByED(FeatureType.ApronElement)?
                    .Cast<Aran.Aim.Features.ApronElement>()
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    Data.CargoHandling = Lib.AIXM_GetNotesByPurpose(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                    && n.Type == CodeAircraftGroundService.HAND)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList(), CodeNotePurpose.DESCRIPTION).NilIfEmpty();
                    List<AirportSuppliesService> assForAH = ass?.Where(n => n.AirportHeliport
                        .Select(x => x.Feature.Identifier)
                        .Contains(air.Identifier))
                        .ToList();
                    Data.FuelTypes = Lib.AIXM_GetNotes(assForAH
                        .SelectManyNullSafe(x => x.FuelSupply)
                        .SelectManyNullSafe(i => i.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.OilTypes = Lib.AIXM_GetNotes(assForAH
                        .SelectManyNullSafe(x => x.OilSupply)
                        .SelectManyNullSafe(i => i.Annotation)
                        .ToList()).NilIfEmpty();

                    var tmp = apron?
                        .Where(b => b.Type == CodeApronElement.FUEL)
                        .SelectManyNullSafe(x => x.SupplyService).Select(u => u.Feature.Identifier);
                    if (tmp != null)
                    {
                        Data.FuellingFacilities = Lib.AIXM_GetNotesByPurpose(assForAH
                            .Where(x => tmp.Contains(x.Identifier))?
                            .SelectManyNullSafe(x => x.FuelSupply)
                            .SelectManyNullSafe(i => i.Annotation)
                            .ToList(), CodeNotePurpose.DESCRIPTION);
                    }
                    Data.FuellingFacilities = Data.FuellingFacilities.NilIfEmpty();

                    Data.DeicingFacilities = Lib.AIXM_GetNotes(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                        && n.Type == CodeAircraftGroundService.DEICE)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.HangarSpace = Lib.AIXM_GetNotes(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                        && n.Type == CodeAircraftGroundService.HANGAR)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.RepairFacilities = Lib.AIXM_GetNotes(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                        && n.Type == CodeAircraftGroundService.REPAIR)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.Remarks = Lib.AIXM_GetNotesByPurpose(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                        .Contains(air.Identifier))
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList(), CodeNotePurpose.REMARK).NilIfEmpty();
                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD25(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD25 Data = new ForAD25();
                string output = "";

                List<PassengerService> ps = Globals.GetFeaturesByED(FeatureType.PassengerService)?.Cast<Aran.Aim.Features.PassengerService>()
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    Data.Hotels = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.HOTEL)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.Restaurants = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.REST)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();

                    Data.Transportation = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.TRANSPORT)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.MedicalFacilities = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.MEDIC)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.BankPostOffice = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                    && (n.Type == CodePassengerService.BANK || n.Type == CodePassengerService.POST))
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.TouristOffice = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.INFO)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();

                    Data.Remarks = Lib.AIXM_GetNotesByPurpose(
                        ps?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                        .Contains(air.Identifier))
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList(), CodeNotePurpose.OTHER_PAX_FAC_FURTHER_DETAILS).NilIfEmpty();

                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD26(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD26 Data = new ForAD26();
                string output = "";

                List<FireFightingService> ffs = Globals.GetFeaturesByED(FeatureType.FireFightingService)?.Cast<Aran.Aim.Features.FireFightingService>()
                    .ToList();
                List<AircraftGroundService> ags = Globals.GetFeaturesByED(FeatureType.AircraftGroundService)?.Cast<Aran.Aim.Features.AircraftGroundService>()
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    //Data.ADCategory = Lib.AIXM_GetNotesByCategory(ffs?
                    //    .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier))
                    //    .Select(x => new CategoryWithNotes
                    //    {
                    //        Category = x.Category.ToString(),
                    //        Annotation = x.Annotation
                    //    }).ToList())
                    //    .NilIfEmpty();
                    
                    var catList = ffs?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier))
                        .Select(x => x.Category.ToString()).ToList();
                    Data.ADCategory = catList?.Count > 0 ? String.Join("<br />", catList).NilIfEmpty() : StringExtensions.Nil();

                    Data.RescueEquipment = Lib.AIXM_GetNotesByPurpose(ffs?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier))
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList(), new List<CodeNotePurpose?>
                            {
                                CodeNotePurpose.OTHER_FIRE_SERVICE_DESC,
                                CodeNotePurpose.OTHER_FIRE_EQPT_DESC,
                                CodeNotePurpose.OTHER_RESCUE_SERVICE_DESC,
                                CodeNotePurpose.OTHER_RESCUE_EQPT_DESC
                            })
                            .NilIfEmpty();

                    Data.CapabilityForRemoval = Lib.AIXM_GetNotesByPurpose(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier).Contains(air.Identifier)
                                        && n.Type == CodeAircraftGroundService.REMOVE)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList(), CodeNotePurpose.DESCRIPTION)
                            .NilIfEmpty();

                    Data.Remarks = Lib.AIXM_GetNotesByPurpose(
                        ffs?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                        .Contains(air.Identifier))
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList(), CodeNotePurpose.REMARK)
                        .NilIfEmpty();

                    //Data.Remarks += Lib.AIXM_GetNotes(
                    //    ags?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                    //            .Contains(air.Identifier))
                    //        .SelectManyNullSafe(x => x.Annotation)
                    //        .ToList());
                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD27(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD27 Data = new ForAD27();
                string output = "";

                //List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                //{
                //    CodeRuleProcedureTitle.OTHER_AIP_SNOW_CLEARANCE_EQUIPMENT,
                //    CodeRuleProcedureTitle.OTHER_AIP_CLEARANCE_PRIORITIES
                //};
                //List<RulesProcedures> rp = Globals.GetFeaturesByED(FeatureType.RulesProcedures).Cast<RulesProcedures>().Where(n => ParamsOrdered.Contains(n.Title) && IsFIRAirspace(n.AffectedArea) && Lib.IsLanguageNote(n.Annotation)).ToList();

                List<AirportClearanceService> acs = Globals.GetFeaturesByED(FeatureType.AirportClearanceService)?.Cast<Aran.Aim.Features.AirportClearanceService>()
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    //Data.TypesOfClearingEquipment = rp
                    //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_SNOW_CLEARANCE_EQUIPMENT)?
                    //    .Content
                    //    .ToXhtml()
                    //    .NilIfEmpty();

                    //Data.ClearancePriorities = rp
                    //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_CLEARANCE_PRIORITIES)?
                    //    .Content
                    //    .ToXhtml()
                    //    .NilIfEmpty();

                    Data.Remarks = Lib.AIXM_GetNotesByPurpose(
                        acs?.Where(n => n.AirportHeliport.Select(x => x.Feature.Identifier)
                        .Contains(air.Identifier))
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList(), CodeNotePurpose.OTHER_SEASONAL_AVAILABILITY)
                        .NilIfEmpty();

                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD28(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD28 Data = new ForAD28();
                string output = "";


                List<Apron> apron = Globals.GetFeaturesByED(FeatureType.Apron)?
                    .Cast<Aran.Aim.Features.Apron>()
                    .OrderBy(x=>x.Name)
                    .ToList();
                List<Taxiway> tax = Globals.GetFeaturesByED(FeatureType.Taxiway)?
                    .Cast<Aran.Aim.Features.Taxiway>()
                    .OrderBy(x=>x.Designator)
                    .ToList();
                List<CheckpointVOR> cpv = Globals.GetFeaturesByED(FeatureType.CheckpointVOR)?
                    .Cast<Aran.Aim.Features.CheckpointVOR>()
                    .OrderBy(x=>x.Identifier)
                    .ToList();
                List<CheckpointINS> cpi = Globals.GetFeaturesByED(FeatureType.CheckpointINS)?
                    .Cast<Aran.Aim.Features.CheckpointINS>()
                    .OrderBy(x=>x.Identifier)
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    List<Apron> aprList = apron?.Where(n => n.AssociatedAirportHeliport.Identifier == air.Identifier).ToList();
                    if (aprList != null)
                        foreach (Apron apr in aprList)
                        {
                            Dictionary<string, string> apronDic = new Dictionary<string, string>();
                            apronDic.Add("Name", apr.Name);
                            apronDic.Add("Composition", apr.SurfaceProperties?.Composition?.ToString());

                            apronDic.Add("SurfaceProperties", apr.SurfaceProperties?.ToSurfacePropertiesList());
                            /*
                            apronDic.Add("SurfaceCondition", apr.SurfaceProperties?.SurfaceCondition?.ToString());
                            apronDic.Add("ClassPCN", apr.SurfaceProperties?.ClassPCN?.ToString());
                            apronDic.Add("PavementTypePCN", apr.SurfaceProperties?.PavementTypePCN?.ToString().FirstOrEmpty());
                            apronDic.Add("PavementSubgradePCN", apr.SurfaceProperties?.PavementSubgradePCN?.ToString().FirstOrEmpty());
                            apronDic.Add("MaxTyrePressurePCN", apr.SurfaceProperties?.MaxTyrePressurePCN?.ToString().FirstOrEmpty());
                            apronDic.Add("EvaluationMethodPCN", apr.SurfaceProperties?.EvaluationMethodPCN?.ToString().FirstOrEmpty());
                            */
                            Data.ApronData.Add(apronDic);
                        }

                    List<Taxiway> taxList = tax?.Where(n => n.AssociatedAirportHeliport.Identifier == air.Identifier).ToList();
                    if (taxList != null)
                        foreach (Taxiway tx in taxList)
                        {
                            Dictionary<string, string> taxDic = new Dictionary<string, string>();
                            taxDic.Add("Name", tx.Designator);
                            taxDic.Add("Width", tx.Width?.StringValue);
                            taxDic.Add("Composition", tx.SurfaceProperties?.Composition?.ToString());
                            taxDic.Add("SurfaceProperties", tx.SurfaceProperties?.ToSurfacePropertiesList());
                            /*
                            apronDic.Add("SurfaceCondition", apr.SurfaceProperties?.SurfaceCondition?.ToString());
                            apronDic.Add("ClassPCN", apr.SurfaceProperties?.ClassPCN?.ToString());
                            apronDic.Add("PavementTypePCN", apr.SurfaceProperties?.PavementTypePCN?.ToString().FirstOrEmpty());
                            apronDic.Add("PavementSubgradePCN", apr.SurfaceProperties?.PavementSubgradePCN?.ToString().FirstOrEmpty());
                            apronDic.Add("MaxTyrePressurePCN", apr.SurfaceProperties?.MaxTyrePressurePCN?.ToString().FirstOrEmpty());
                            apronDic.Add("EvaluationMethodPCN", apr.SurfaceProperties?.EvaluationMethodPCN?.ToString().FirstOrEmpty());
                            */
                            Data.TaxiwayData.Add(taxDic);
                        }

                    Data.Altimeter = air.AltimeterCheckLocation.ToYesNoNil();

                    //Data.VORCheckpoints = Lib.AIXM_GetNotes(cpv?.Where(n => n.AirportHeliport.Identifier == air.Identifier)
                    //    .SelectManyNullSafe(x => x.Annotation)
                    //    .ToList())
                    //    .NilIfEmpty();
                    Data.VORCheckpoints = cpv?.Where(n => n.AirportHeliport.Identifier == air.Identifier)
                            .Select(x => x.Position.Geo)
                        .ToList().ToPointString();

                    Data.INSCheckpoints = cpi?.Where(n => n.AirportHeliport.Identifier == air.Identifier)
                            .Select(x => x.Position.Geo)
                            .ToList().ToPointString();



                    Data.Remarks = Lib.AIXM_GetNotes(
                            apron?.Where(n => n.AssociatedAirportHeliport.Identifier == air.Identifier)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList());
                    Data.Remarks += Lib.AIXM_GetNotes(
                            tax?.Where(n => n.AssociatedAirportHeliport.Identifier == air.Identifier)
                                .SelectManyNullSafe(x => x.Annotation)
                                .ToList())
                                .NilIfEmpty();
                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD29(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD29 Data = new ForAD29();
                string output = "";

                List<Apron> apron = Globals.GetFeaturesByED(FeatureType.Apron)?.Cast<Aran.Aim.Features.Apron>()
                    .Where(n => n.AssociatedAirportHeliport.Identifier == air.Identifier)
                    .ToList();
                var apronGuid = apron?
                    .Select(x => x.Identifier)
                    .ToList();

                List<ApronElement> apronElem = Globals.GetFeaturesByED(FeatureType.ApronElement)?.Cast<Aran.Aim.Features.ApronElement>()
                    .Where(n => apronGuid != null && apronGuid.Contains(n.AssociatedApron.Identifier))
                    .ToList();
                List<Guid?> apronElemGuid = apronElem?
                    .Select(x => x?.Identifier)
                    .ToList();

                List<Taxiway> taxiway = Globals.GetFeaturesByED(FeatureType.Taxiway)?.Cast<Aran.Aim.Features.Taxiway>()
                    .Where(n => n.AssociatedAirportHeliport.Identifier == air.Identifier)
                    .ToList();
                List<Guid?> taxiwayGuid = taxiway?
                    .Select(x => x?.Identifier)
                    .ToList();

                List<TaxiwayMarking> taxiwayMarking = Globals.GetFeaturesByED(FeatureType.TaxiwayMarking)?.Cast<Aran.Aim.Features.TaxiwayMarking>()
                    .Where(n => taxiwayGuid.ContainsValue(n.MarkedTaxiway?.Identifier))
                    .ToList();

                List<AircraftStand> aircraftStand = Globals.GetFeaturesByED(FeatureType.AircraftStand)?.Cast<Aran.Aim.Features.AircraftStand>()
                    .Where(n => apronElemGuid.ContainsValue(n.ApronLocation?.Identifier))
                    .ToList();
                var aircraftGuid = aircraftStand?
                    .Select(x => x.Identifier)
                    .ToList();
                List<StandMarking> standMarking = Globals.GetFeaturesByED(FeatureType.StandMarking)?.Cast<Aran.Aim.Features.StandMarking>()
                    .Where(n => aircraftGuid != null && n.MarkedStand != null && aircraftGuid.Contains(n.MarkedStand.Identifier))
                    .ToList();

                List<Runway> runway = Globals.GetFeaturesByED(FeatureType.Runway)?.Cast<Aran.Aim.Features.Runway>()
                    .Where(n => n.AssociatedAirportHeliport.Identifier == air.Identifier)
                    .ToList();
                var runwayGuid = runway?
                    .Select(x => x.Identifier)
                    .ToList();
                List<RunwayMarking> runwayMarking = Globals.GetFeaturesByED(FeatureType.RunwayMarking)?.Cast<Aran.Aim.Features.RunwayMarking>()
                    .Where(n => runwayGuid != null && runwayGuid.Contains(n.MarkedRunway.Identifier))
                    .ToList();


                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    if (aircraftStand != null)
                        foreach (AircraftStand acs in aircraftStand)
                        {
                            Dictionary<string, string> acsDic = new Dictionary<string, string>();
                            acsDic.Add("Name", acs.Designator);
                            StandMarking sm =
                                standMarking?.FirstOrDefault(x => x.MarkedStand.Identifier == acs.Identifier);
                            acsDic.Add("MarkingICAOStandard", sm?.MarkingICAOStandard.ToYesNoNil());
                            acsDic.Add("Condition", sm?.Condition.ToString());
                            acsDic.Add("Remarks", Lib.AIXM_GetNotes(sm?.Annotation));
                            Data.UseAircraft.Add(acsDic);
                        }

                    if (runway != null)
                    {
                        foreach (Runway rn in runway)
                        {
                            Dictionary<string, string> runDic = new Dictionary<string, string>();
                            runDic.Add("Name", rn.Designator);
                            RunwayMarking sm =
                                runwayMarking?.FirstOrDefault(x => x.MarkedRunway.Identifier == rn.Identifier);
                            runDic.Add("MarkingICAOStandard", sm?.MarkingICAOStandard.ToYesNoNil());
                            runDic.Add("Condition", sm?.Condition.ToString());
                            runDic.Add("Remarks", Lib.AIXM_GetNotes(rn?.Annotation)); //?
                            //runDic.Add("Remarks", Lib.AIXM_GetNotes(sm?.Annotation));
                            Data.RunwayMarking.Add(runDic);
                        }
                    }


                    if (taxiway != null)
                    {
                        foreach (Taxiway tx in taxiway)
                        {
                            Dictionary<string, string> txDic = new Dictionary<string, string>();
                            txDic.Add("Name", tx.Designator);
                            TaxiwayMarking tm =
                                taxiwayMarking?.FirstOrDefault(x => x.MarkedTaxiway.Identifier == tx.Identifier);
                            txDic.Add("MarkingICAOStandard", tm?.MarkingICAOStandard.ToYesNoNil());
                            txDic.Add("Condition", tm?.Condition.ToString());
                            txDic.Add("Remarks", Lib.AIXM_GetNotes(tx?.Annotation)); //?
                            //runDic.Add("Remarks", Lib.AIXM_GetNotes(sm?.Annotation));
                            Data.TaxiwayMarking.Add(txDic);
                        }

                        // Stop bars (if any)
                        //var guidanceLineGuid = Globals.GetFeaturesByED(FeatureType.GuidanceLine)?.Cast<Aran.Aim.Features.GuidanceLine>()
                        //    .SelectManyNullSafe(n => n.ConnectedTaxiway.Select(x=>x.Feature.Identifier).Intersect(taxiwayGuid))
                        //    .ToList();

                        //List<TaxiHoldingPosition> taxiHoldingPosition = Globals.GetFeaturesByED(FeatureType.TaxiHoldingPosition)?.Cast<Aran.Aim.Features.TaxiHoldingPosition>()
                        //    .Where(n => guidanceLineGuid != null && guidanceLineGuid.Contains(n.AssociatedGuidanceLine.Identifier))
                        //    .ToList();
                        //var taxiHoldingPositionGuid = taxiHoldingPosition?
                        //    .Select(x => x.Identifier)
                        //    .ToList();
                        //List<TaxiHoldingPositionLightSystem> taxiHoldingPositionLightSystem = Globals.GetFeaturesByED(FeatureType.TaxiHoldingPositionLightSystem)?
                        //    .Cast<Aran.Aim.Features.TaxiHoldingPositionLightSystem>()
                        //    .Where(n => taxiHoldingPositionGuid != null 
                        //            && taxiHoldingPositionGuid.Contains(n.TaxiHolding.Identifier)
                        //            && n.Type == CodeLightHoldingPosition.STOP_BAR)
                        //    .ToList();

                        ////
                        ////
                        var refTaxiHoldingPositionGuid = Globals.GetFeaturesByED(FeatureType.TaxiHoldingPositionLightSystem)?
                            .Cast<Aran.Aim.Features.TaxiHoldingPositionLightSystem>()
                            .Where(n => n.Type == CodeLightHoldingPosition.STOP_BAR)
                            .Select(x => x.TaxiHolding.Identifier)
                            .ToList();

                        if (refTaxiHoldingPositionGuid != null)
                        {
                            var refGuidanceLineGuid = Globals.GetFeaturesByED(FeatureType.TaxiHoldingPosition)?
                                .Cast<Aran.Aim.Features.TaxiHoldingPosition>()
                                .Where(n => refTaxiHoldingPositionGuid.Contains(n.Identifier))
                                .Select(x => x.AssociatedGuidanceLine.Identifier)
                                .ToList();

                            if (refGuidanceLineGuid != null)
                            {
                                List<string> taxiwayWithStopBars = taxiway
                                    .Where(n => refGuidanceLineGuid.Contains(n.Identifier))
                                    .Select(x => x.Designator)
                                    .ToList();


                                if (taxiwayWithStopBars != null)
                                {
                                    Data.StopBars = $@"Stop bars installed on Taxiway: {taxiwayWithStopBars.ToSeparatedValues()}";
                                }
                            }
                        }


                    }
                    Data.StopBars.NilIfEmpty();



                    Data.Remarks = Lib.AIXM_GetNotes(
                        taxiway?
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList());
                    Data.Remarks += Lib.AIXM_GetNotes(
                            runway?
                                .SelectManyNullSafe(x => x.Annotation)
                                .ToList())
                        .NilIfEmpty();

                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void Fill_AD210(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD210 Data = new ForAD210();
                string output = "";

                List<ObstacleArea> oa = Globals.GetFeaturesByED(FeatureType.ObstacleArea)?
                    .Cast<Aran.Aim.Features.ObstacleArea>()
                    .Where(n => n.Reference?.OwnerAirport?.Identifier == air.Identifier)
                    .ToList();
                var vsGuid2 = oa?.Where(n => n.Type == CodeObstacleArea.AREA2).SelectManyNullSafe(n => n.Obstacle).Select(x => x.Feature.Identifier).ToList();
                var vsGuid3 = oa?.Where(n => n.Type == CodeObstacleArea.AREA3).SelectManyNullSafe(n => n.Obstacle).Select(x => x.Feature.Identifier).ToList();
                List<VerticalStructure> vs2 = Globals.GetFeaturesByED(FeatureType.VerticalStructure)?
                    .Cast<Aran.Aim.Features.VerticalStructure>()
                    .Where(n => vsGuid2.ContainsValue(n.Identifier))
                    .ToList();
                List<VerticalStructure> vs3 = Globals.GetFeaturesByED(FeatureType.VerticalStructure)?
                    .Cast<Aran.Aim.Features.VerticalStructure>()
                    .Where(n => vsGuid3.ContainsValue(n.Identifier))
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 0
                    };

                    // obstacles in area 2

                    if (vs2 != null)
                    {
                        foreach (VerticalStructure vs in vs2)
                        {
                            Dictionary<string, string> vsDic = new Dictionary<string, string>();
                            vsDic.Add("Name", vs.Name);
                            vsDic.Add("Type", vs.Type.ToString());
                            vsDic.Add("Height",
                                vs.Part.Select(x => x.VerticalExtent?.StringValue).ToList().ToSeparatedValues());

                            vsDic.Add("Coordinates",
                                vs
                                .Part
                                .Select(x =>
                                (x.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                                 ? x.HorizontalProjection?.Location.Geo.ToPointString() : "")
                                 .ToList()
                                 .ToSeparatedValues());
                            vsDic.Add("Elevation",
                                vs
                                    .Part
                                    .Select(x =>
                                        (x.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                                            ? x.HorizontalProjection?.Location?.Elevation?.StringValue : "")
                                    .ToList()
                                    .ToSeparatedValues());
                            vsDic.Add("MarkingPattern",
                                vs.Part.Select(x => x.MarkingPattern?.ToString()).ToList().ToSeparatedValues());
                            vsDic.Add("MarkingFirstColour",
                                vs.Part.Select(x => x.MarkingFirstColour?.ToString()).ToList().ToSeparatedValues());
                            vsDic.Add("MarkingSecondColour",
                                vs.Part.Select(x => x.MarkingSecondColour?.ToString()).ToList().ToSeparatedValues());

                            vsDic.Add("Lighted", vs.Lighted.ToYesNoNil());
                            vsDic.Add("LightElementColour",
                                vs.Part.SelectManyNullSafe(x => x.Lighting?.Select(n => n.Colour?.ToString())).ToList()
                                    .ToSeparatedValues());
                            vsDic.Add("LightElementType",
                                vs.Part.SelectManyNullSafe(x => x.Lighting?.Select(n => n.Type?.ToString())).ToList()
                                    .ToSeparatedValues());
                            vsDic.Add("Remarks", Lib.AIXM_GetNotes(vs.Annotation));
                            Data.AREA2.Add(vsDic);
                        }
                    }

                    if (vs3 != null)
                    {
                        foreach (VerticalStructure vs in vs3)
                        {
                            Dictionary<string, string> vsDic = new Dictionary<string, string>();
                            vsDic.Add("Name", vs.Name);
                            vsDic.Add("Type", vs.Type.ToString());
                            vsDic.Add("Height",
                                vs.Part.Select(x => x.VerticalExtent?.StringValue).ToList().ToSeparatedValues());
                            vsDic.Add("Coordinates",
                                vs
                                    .Part
                                    .Select(x =>
                                        (x.HorizontalProjection.Choice == VerticalStructurePartGeometryChoice.ElevatedPoint)
                                            ? x.HorizontalProjection?.Location.Geo.ToPointString() : "")
                                    .ToList()
                                    .ToSeparatedValues());
                            vsDic.Add("MarkingPattern",
                                vs.Part.Select(x => x.MarkingPattern?.ToString()).ToList().ToSeparatedValues());
                            vsDic.Add("MarkingFirstColour",
                                vs.Part.Select(x => x.MarkingFirstColour?.ToString()).ToList().ToSeparatedValues());
                            vsDic.Add("MarkingSecondColour",
                                vs.Part.Select(x => x.MarkingSecondColour?.ToString()).ToList().ToSeparatedValues());

                            vsDic.Add("Lighted", vs.Lighted.ToYesNoNil());
                            vsDic.Add("LightElementColour",
                                vs.Part.Select(x => x.Lighting?.Select(n => n.Colour).ToString()).ToList()
                                    .ToSeparatedValues());
                            vsDic.Add("LightElementType",
                                vs.Part.Select(x => x.Lighting?.Select(n => n.Type).ToString()).ToList()
                                    .ToSeparatedValues());
                            vsDic.Add("Remarks", Lib.AIXM_GetNotes(vs.Annotation));
                            Data.AREA3.Add(vsDic);
                        }
                    }

                    Data.Area2Remarks = Lib.AIXM_GetNotesByPurpose(
                            air?.Annotation,
                            CodeNotePurpose.OTHER_OBS_AREA2_ELIST_AVAILABILITY)
                        .NilIfEmpty();
                    Data.Area3Remarks = Lib.AIXM_GetNotesByPurpose(
                            air?.Annotation,
                            CodeNotePurpose.OTHER_OBS_AREA3_ELIST_AVAILABILITY)
                        .NilIfEmpty();

                    if (!Lib.IsNull(Data))
                    {
                        output = Razor.Run(Data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD211(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD211 Data = new ForAD211();
                string output = "";

                List<InformationService> ins = Globals.GetFeaturesByED(FeatureType.InformationService)?
                    .Cast<Aran.Aim.Features.InformationService>()
                    .Where(n => n.Type == CodeServiceInformation.TAF &&
                            n.ClientAirport.Any(x => x.Feature?.Identifier == air.Identifier))
                    .ToList();
                Guid? unitGuid = ins?.Select(x => x.ServiceProvider?.Identifier).FirstOrDefault();

                Unit unit = Globals.GetFeaturesByED(FeatureType.Unit)?
                    .Cast<Unit>()
                    .FirstOrDefault(n => unitGuid != null && unitGuid == n.Identifier);
                //List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                //{
                //    CodeRuleProcedureTitle.OTHER_AIP_FLIGHT_DOCUMENTATION,
                //    CodeRuleProcedureTitle.OTHER_AIP_CHARTS_OTHER_INFO_FOR_BRIEFING,
                //    CodeRuleProcedureTitle.OTHER_AIP_AHP_SUPP_MET_EQPT,
                //};
                //List<RulesProcedures> rp = Globals.GetFeaturesByED(FeatureType.RulesProcedures).Cast<RulesProcedures>().Where(n => ParamsOrdered.Contains(n.Title) && ForAirportHeliport(n.AffectedLocation, air) && Lib.IsLanguageNote(n.Annotation)).ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 0
                    };

                    if (unit != null)
                    {
                        Data.AssociatedMETOffice = unit.Name;

                        Data.HoursOfService = Lib.GetHoursOfOperations(
                                                    ins?.SelectManyNullSafe(x => x.Availability)
                                                        .SelectManyNullSafe(v => v.TimeInterval)
                                                        .ToList())
                                                        .NilIfEmpty();
                        Data.METOfficeOutsideHours = Lib.AIXM_GetNotes(
                                ins?.SelectManyNullSafe(x => x.Annotation).ToList())
                            .NilIfEmpty();

                        Data.OfficeResponsible = unit.Name;// ???

                        //Data.PeriodsOfValidity = Lib.GetHoursOfOperations(unit.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList()).NilIfEmpty();// Not described in the map, maped by my IMHO. Must be approved.

                        //Data.TrendForecast = "".NilIfEmpty();
                        //Data.IntervalOfIssuance2 = "".NilIfEmpty();

                        Data.BriefingConsultation = Lib.AIXM_GetNotesByPurpose(
                            ins?.SelectManyNullSafe(x => x.Annotation).ToList(),
                            CodeNotePurpose.OTHER_BRIEFING_CONSULTATION_METHOD);


                        //Data.FlightDocumentation = rp
                        //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_FLIGHT_DOCUMENTATION)?
                        //    .Content
                        //    .ToXhtml()
                        //    .NilIfEmpty();

                        //Data.Charts = rp
                        //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_CHARTS_OTHER_INFO_FOR_BRIEFING)?
                        //    .Content
                        //    .ToXhtml()
                        //    .NilIfEmpty();

                        //Data.SupplementaryEquipment = rp
                        //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_AHP_SUPP_MET_EQPT)?
                        //    .Content
                        //    .ToXhtml()
                        //    .NilIfEmpty();

                        //Data.ATSUnits = "".NilIfEmpty();

                        Data.AdditionalInformation = Lib.AIXM_GetNotesByPurpose(
                            ins?.SelectManyNullSafe(x => x.Annotation).ToList(),
                            CodeNotePurpose.OTHER_BRIEFING_CONSULTATION_METHOD,
                            true);
                        if (!Lib.IsNull(Data))
                        {
                            output = Razor.Run(Data);
                            if (output == null)
                            {
                                SendOutput("Error in generating template ", Percent: 80);
                                return;
                            }
                            ss.Content = output;
                            db.Subsection.Add(ss);
                        }
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void Fill_AD212(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD212> dataList = new List<ForAD212>();
                string output = "";


                List<Runway> runway = Globals.GetFeaturesByED(FeatureType.Runway)?
                                        .Cast<Aran.Aim.Features.Runway>()
                                        .Where(x => x.AssociatedAirportHeliport?.Identifier == air.Identifier)
                                        .ToList();
                List<Guid?> runwayGuid = runway.ToGuidList();

                List<RunwayDirection> runwayDirection = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                    .Cast<Aran.Aim.Features.RunwayDirection>()
                    .Where(x => runwayGuid.ContainsValue(x.UsedRunway?.Identifier))
                    .ToList();
                List<Guid?> runwayDirectionGuid = runwayDirection.ToGuidList();

                List<RunwayProtectArea> runwayProtectAreas = Globals.GetFeaturesByED(FeatureType.RunwayProtectArea)?
                    .Cast<Aran.Aim.Features.RunwayProtectArea>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.ProtectedRunwayDirection?.Identifier))
                    .ToList();
                //List<RunwayCentrelinePoint> runwayCenterlinePointList = Globals.GetFeaturesByED(FeatureType.RunwayCentrelinePoint)?
                //    .Cast<Aran.Aim.Features.RunwayCentrelinePoint>()
                //    .Where(x => runwayDirectionGuid != null && 
                //                (x.Role == CodeRunwayPointRole.THR || x.Role == CodeRunwayPointRole.END || x.Role == CodeRunwayPointRole.TDZ ) &&
                //                runwayDirectionGuid.Contains(x.OnRunway.Identifier))
                //    .ToList();



                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    foreach (Runway rw in runway)
                    {

                        List<RunwayDirection> rwdList =
                            runwayDirection?.Where(x => x.UsedRunway?.Identifier == rw.Identifier).ToList();
                        List<Guid?> rwdGuid =
                            rwdList.ToGuidList();

                        bool? rpaOFZ = runwayProtectAreas?
                            .Any(x => x.Type == CodeRunwayProtectionArea.OFZ &&
                                        rwdGuid.ContainsValue(x.ProtectedRunwayDirection?.Identifier));

                        //var rwdStopWayList = runwayDirection?.Where(n => 
                        //                                        rpaList != null && 
                        //                                        rpaList
                        //                                        .Select(x => x.ProtectedRunwayDirection.Identifier)
                        //                                        .Contains(n.Identifier))
                        //                                        .Select(i=>i.Identifier)
                        //                        .ToList();



                        if (rwdList != null)
                        {
                            foreach (RunwayDirection rwd in rwdList)
                            {

                                ForAD212 Data = new ForAD212();
                                Data.Dimensions = ((rw?.NominalLength?.StringValue + rw?.NominalWidth?.StringValue).Length > 0)
                                    ? rw?.NominalLength?.StringValue + "x" + rw?.NominalWidth?.StringValue
                                    : "".NilIfEmpty();

                                Data.RWYSurface = rw?.SurfaceProperties?.ToSurfacePropertiesList();

                                Data.StripDimensions = ((rw?.LengthStrip?.StringValue + rw?.WidthStrip?.StringValue).Length > 0)
                                    ? rw?.LengthStrip?.StringValue + "x" + rw?.WidthStrip?.StringValue
                                    : "".NilIfEmpty();
                                Data.OFZ = rpaOFZ.ToYesNoNil();

                                Data.Designator = rwd.Designator;
                                Data.TrueBearing = rwd.TrueBearing.ToDegree();

                                Data.Remarks = Lib.AIXM_GetNotes(rwd.Annotation);

                                List<RunwayProtectArea> rpaStopWayCWYList = runwayProtectAreas?
                                    .Where(x => (x.Type == CodeRunwayProtectionArea.CWY ||
                                                 x.Type == CodeRunwayProtectionArea.STOPWAY) &&
                                                x.ProtectedRunwayDirection?.Identifier == rwd.Identifier)
                                    .ToList();
                                if (rpaStopWayCWYList != null)
                                {
                                    foreach (RunwayProtectArea rpa in rpaStopWayCWYList)
                                    {
                                        if (rpa.Type == CodeRunwayProtectionArea.STOPWAY)
                                        {
                                            Data.SWYDimensions = ((rpa.Length?.StringValue + rpa.Width?.StringValue).Length > 0)
                                                ? rpa.Length?.StringValue + "x" + rpa.Width?.StringValue
                                                : "".NilIfEmpty();
                                            Data.SWYSurface = rpa.SurfaceProperties.ToSurfacePropertiesList();
                                            Data.SlopeTDZ = rwd.SlopeTDZ.ToString();
                                        }
                                        if (rpa.Type == CodeRunwayProtectionArea.CWY)
                                        {
                                            Data.CWYDimensions = ((rpa.Length?.StringValue + rpa.Width?.StringValue).Length > 0)
                                                ? rpa.Length?.StringValue + "x" + rpa.Width?.StringValue
                                                : "".NilIfEmpty();
                                        }
                                    }

                                }

                                List<RunwayCentrelinePoint> runwayCenterlinePointList = Globals.GetFeaturesByED(FeatureType.RunwayCentrelinePoint)?
                                    .Cast<Aran.Aim.Features.RunwayCentrelinePoint>()
                                    .Where(x =>
                                                (x.Role == CodeRunwayPointRole.THR || 
                                                //x.Role == CodeRunwayPointRole.END || 
                                                x.Role == CodeRunwayPointRole.TDZ) &&
                                                x.OnRunway?.Identifier == rwd.Identifier)
                                    .ToList();
                                if (runwayCenterlinePointList != null)
                                {
                                    foreach (RunwayCentrelinePoint rcpl in runwayCenterlinePointList)
                                    {
                                        if (rcpl.Role == CodeRunwayPointRole.THR 
                                            //|| rcpl.Role == CodeRunwayPointRole.END
                                            )
                                        {
                                            Data.GeoCoords = rcpl.Location?.Geo?.ToPointString();
                                        }
                                        if (rcpl.Role == CodeRunwayPointRole.THR)
                                        {
                                            Data.GeoidUndulation = rcpl.Location?.GeoidUndulation?.StringValue;
                                        }
                                        if (rcpl.Role == CodeRunwayPointRole.THR || rcpl.Role == CodeRunwayPointRole.TDZ)
                                        {
                                            Data.Elevation = rcpl.Location?.Elevation?.StringValue;
                                        }

                                    }
                                }

                                dataList.Add(Data);
                            }

                        }


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
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD213(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD213> dataList = new List<ForAD213>();
                string output = "";


                List<Runway> runway = Globals.GetFeaturesByED(FeatureType.Runway)?
                                        .Cast<Aran.Aim.Features.Runway>()
                                        .Where(x => x.AssociatedAirportHeliport?.Identifier == air.Identifier)
                                        .ToList();
                List<Guid?> runwayGuid = runway.ToGuidList();

                List<RunwayDirection> runwayDirection = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                    .Cast<Aran.Aim.Features.RunwayDirection>()
                    .Where(x => runwayGuid.ContainsValue(x.UsedRunway?.Identifier))
                    .ToList();
                List<Guid?> runwayDirectionGuid = runwayDirection.ToGuidList();

                List<RunwayCentrelinePoint> runwayCenterlinePointList = Globals.GetFeaturesByED(FeatureType.RunwayCentrelinePoint)?
                    .Cast<Aran.Aim.Features.RunwayCentrelinePoint>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.OnRunway.Identifier))
                    .ToList();
                List<Guid?> runwayCenterlinePointGuid = runwayCenterlinePointList.ToGuidList();

                //List <RunwayDirection> runwayDirection = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                //    .Cast<Aran.Aim.Features.RunwayDirection>()
                //    .Where(x => runwayGuid != null && runwayGuid.Contains(x.UsedRunway.Identifier))
                //    .ToList();



                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    foreach (Runway rw in runway)
                    {

                        List<RunwayDirection> rwdList =
                            runwayDirection?.Where(x => x.UsedRunway?.Identifier == rw.Identifier).ToList();
                        List<Guid?> rwdGuid =
                            rwdList.ToGuidList();

                        if (rwdList != null)
                        {
                            foreach (RunwayDirection rwd in rwdList)
                            {
                                List<RunwayCentrelinePoint> runwayCenterlinePointList2 =
                                    runwayCenterlinePointList?.Where(x => x.OnRunway?.Identifier == rwd.Identifier)
                                    .ToList();
                                ForAD213 data = new ForAD213();
                                data.Designator = rw.Designator;
                                if (runwayCenterlinePointList != null)
                                {
                                    foreach (RunwayCentrelinePoint rcpl in runwayCenterlinePointList2)
                                    {
                                        foreach (RunwayDeclaredDistance rdd in rcpl.AssociatedDeclaredDistance)
                                        {


                                            foreach (RunwayDeclaredDistanceValue rddv in rdd.DeclaredValue)
                                            {
                                                if (rdd.Type == CodeDeclaredDistance.TORA)
                                                {
                                                    data.TORADistance = rddv.Distance?.StringValue;
                                                }
                                                if (rdd.Type == CodeDeclaredDistance.TODA)
                                                {
                                                    data.TODADistance = rddv.Distance?.StringValue;
                                                }
                                                if (rdd.Type == CodeDeclaredDistance.ASDA)
                                                {
                                                    data.ASDADistance = rddv.Distance?.StringValue;
                                                }
                                                if (rdd.Type == CodeDeclaredDistance.LDA)
                                                {
                                                    data.LDADistance = rddv.Distance?.StringValue;
                                                }
                                            }
                                            data.Remarks += Lib.AIXM_GetNotes(rdd.Annotation).NilIfEmpty();

                                        }
                                    }
                                }
                                dataList.Add(data);
                            }
                        }
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
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD214(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD214> dataList = new List<ForAD214>();
                string output = "";


                List<Runway> runway = Globals.GetFeaturesByED(FeatureType.Runway)?
                                        .Cast<Aran.Aim.Features.Runway>()
                                        .Where(x => x.AssociatedAirportHeliport?.Identifier == air.Identifier)
                                        .ToList();
                List<Guid?> runwayGuid = runway.ToGuidList();

                List<RunwayDirection> runwayDirection = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                    .Cast<Aran.Aim.Features.RunwayDirection>()
                    .Where(x => runwayGuid.ContainsValue(x.UsedRunway?.Identifier))
                    .ToList();
                var runwayDirectionGuid = runwayDirection.ToGuidList();

                List<ApproachLightingSystem> approachLightingSystem = Globals.GetFeaturesByED(FeatureType.ApproachLightingSystem)?
                    .Cast<Aran.Aim.Features.ApproachLightingSystem>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.ServedRunwayDirection?.Identifier))
                    .ToList();

                List<RunwayDirectionLightSystem> runwayDirectionLightSystem = Globals.GetFeaturesByED(FeatureType.RunwayDirectionLightSystem)?
                    .Cast<Aran.Aim.Features.RunwayDirectionLightSystem>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.AssociatedRunwayDirection?.Identifier))
                    .ToList();

                List<VisualGlideSlopeIndicator> visualGlideSlopeIndicator = Globals.GetFeaturesByED(FeatureType.VisualGlideSlopeIndicator)?
                    .Cast<Aran.Aim.Features.VisualGlideSlopeIndicator>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.RunwayDirection?.Identifier))
                    .ToList();

                List<RunwayProtectArea> runwayProtectArea = Globals.GetFeaturesByED(FeatureType.RunwayProtectArea)?
                    .Cast<Aran.Aim.Features.RunwayProtectArea>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.ProtectedRunwayDirection?.Identifier))
                    .ToList();
                var runwayProtectAreaGuid = runwayProtectArea.ToGuidList();

                List<RunwayProtectAreaLightSystem> runwayProtectAreaLightSystem = Globals.GetFeaturesByED(FeatureType.RunwayProtectAreaLightSystem)?
                    .Cast<Aran.Aim.Features.RunwayProtectAreaLightSystem>()
                    .Where(x => runwayProtectAreaGuid.ContainsValue(x.LightedArea?.Identifier))
                    .ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    foreach (Runway rw in runway)
                    {

                        List<RunwayDirection> rwdList =
                            runwayDirection?.Where(x => x.UsedRunway?.Identifier == rw.Identifier).ToList();
                        var rwdGuid =
                            rwdList.ToGuidList();
                        if (rwdList != null)
                        {
                            foreach (RunwayDirection rwd in rwdList)
                            {
                                ForAD214 data = new ForAD214();
                                data.Designator = rw.Designator;

                                List<ApproachLightingSystem> approachLightingSystemList =
                                    approachLightingSystem?.Where(x => x.ServedRunwayDirection?.Identifier == rwd.Identifier)
                                    .ToList();

                                if (approachLightingSystemList != null)
                                {
                                    foreach (ApproachLightingSystem als in approachLightingSystemList)
                                    {
                                        data.ALSIntensityLevel = als.IntensityLevel?.ToString();
                                        data.ALSType = als.Type?.ToString();
                                        data.ALSLength = als.Length?.ToString();
                                    }
                                }

                                List<RunwayDirectionLightSystem> runwayDirectionLightSystemList =
                                    runwayDirectionLightSystem?.Where(x => x.AssociatedRunwayDirection?.Identifier == rwd.Identifier)
                                        .ToList();

                                if (runwayDirectionLightSystemList != null)
                                {
                                    foreach (RunwayDirectionLightSystem rdls in runwayDirectionLightSystemList)
                                    {
                                        if (rdls.Position == CodeRunwaySection.THR)
                                        {
                                            data.THRColour = rdls.Colour?.ToString();
                                        }
                                        if (rdls.Position == CodeRunwaySection.TDZ)
                                        {
                                            data.TDZColour = rdls.Colour?.ToString();
                                        }
                                        if (rdls.Position == CodeRunwaySection.CL)
                                        {
                                            data.CLColour = rdls.Colour?.ToString();
                                            data.CLIntensity = rdls.IntensityLevel?.ToString();
                                        }
                                        if (rdls.Position == CodeRunwaySection.EDGE)
                                        {
                                            data.EDGEColour = rdls.Colour?.ToString();
                                            data.EDGEIntensity = rdls.IntensityLevel?.ToString();
                                        }
                                        if (rdls.Position == CodeRunwaySection.END)
                                        {
                                            data.WBARColour = rdls.Colour?.ToString();
                                            data.WBARDesc = Lib.AIXM_GetNotesByPurpose(rdls.Annotation, CodeNotePurpose.OTHER_WING_BAR_DESC);
                                        }
                                        data.Remarks += Lib.AIXM_GetNotes(rdls.Annotation);
                                    }
                                }

                                List<VisualGlideSlopeIndicator> visualGlideSlopeIndicatorList =
                                    visualGlideSlopeIndicator?.Where(x => x.RunwayDirection?.Identifier == rwd.Identifier)
                                        .ToList();

                                if (visualGlideSlopeIndicatorList != null)
                                {
                                    foreach (VisualGlideSlopeIndicator vgsi in visualGlideSlopeIndicatorList)
                                    {
                                        data.VASIS = vgsi.Type?.ToString();
                                    }
                                }

                                List<RunwayProtectArea> runwayProtectAreaList =
                                    runwayProtectArea?.Where(x => x.ProtectedRunwayDirection?.Identifier == rwd.Identifier)
                                        .ToList();

                                if (runwayProtectAreaList != null)
                                {
                                    foreach (RunwayProtectArea rpa in runwayProtectAreaList)
                                    {
                                        List<RunwayProtectAreaLightSystem> runwayProtectAreaLightSystemList =
                                            runwayProtectAreaLightSystem?.Where(x => x.LightedArea?.Identifier == rpa.Identifier)
                                                .ToList();

                                        if (runwayProtectAreaLightSystemList != null)
                                        {
                                            foreach (RunwayProtectAreaLightSystem rpals in runwayProtectAreaLightSystemList)
                                            {
                                                data.SWYColour = rpals.Colour?.ToString();
                                            }
                                        }
                                    }
                                }

                                dataList.Add(data);
                            }
                        }
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
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }



        public static void Fill_AD215(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD215 data = new ForAD215();
                string output = "";

                List<AeronauticalGroundLight> agl = Globals.GetFeaturesByED(FeatureType.AeronauticalGroundLight)?
                    .Cast<Aran.Aim.Features.AeronauticalGroundLight>()
                    .Where(n => n.AerodromeBeacon?.Identifier == air.Identifier)
                    .ToList();

                List<Taxiway> tax = Globals.GetFeaturesByED(FeatureType.Taxiway)?
                    .Cast<Aran.Aim.Features.Taxiway>()
                    .Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier)
                    .ToList();

                var taxGuid = tax.ToGuidList();

                List<TaxiwayLightSystem> tls = Globals.GetFeaturesByED(FeatureType.TaxiwayLightSystem)?
                    .Cast<Aran.Aim.Features.TaxiwayLightSystem>()
                    .Where(n => n.LightedTaxiway?.Identifier == air.Identifier)
                    .ToList();


                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 0
                    };

                    data.Location = agl?.Where(x => x.Type == CodeGroundLighting.ABN)
                                        .Select(n => n.Location.Geo)
                                        .ToList()
                                        .ToPointString();

                    foreach (AeronauticalGroundLight item in agl)
                    {
                        Dictionary<string, string> chr = new Dictionary<string, string>();
                        chr.Add("Type", item.Type?.ToString());
                        chr.Add("Colour", item.Colour?.ToString());
                        chr.Add("Flashing", item.Flashing.ToYesNoNil());
                        data.Characteristics.Add(chr);
                    }

                    if (air.WindDirectionIndicator != null) // YES|NO
                    {
                        data.WDILocation = Lib.AIXM_GetNotesByPropertyNameAndPurpose(air.Annotation,
                            "windDirectionIndicator", new List<CodeNotePurpose?> { CodeNotePurpose.DESCRIPTION });
                    }
                    if (air.LandingDirectionIndicator != null) // YES|NO
                    {
                        data.LDILocation = Lib.AIXM_GetNotesByPropertyNameAndPurpose(air.Annotation,
                            "landingDirectionIndicator", new List<CodeNotePurpose?> { CodeNotePurpose.DESCRIPTION });
                    }

                    data.TWYEdgeCl = tls != null && tls.Any(x => x.Position == CodeTaxiwaySection.EDGE) ? "Edge lights" : "";
                    string concat = data.TWYEdgeCl != "" ? ", " : "";
                    data.TWYEdgeCl += tls != null && tls.Any(x => x.Position == CodeTaxiwaySection.CL) ? $@"{concat}Centre line lights" : "";
                    data.TWYEdgeCl = data.TWYEdgeCl.NilIfEmpty();

                    if (air.SecondaryPowerSupply != null) // YES|NO
                    {
                        data.SecondaryPowerSupply = Lib.AIXM_GetNotesByPropertyNameAndPurpose(air.Annotation,
                            "secondaryPowerSupply", new List<CodeNotePurpose?> { CodeNotePurpose.DESCRIPTION });
                    }

                    data.Remarks = "".NilIfEmpty(); // Because already specified above
                    if (!Lib.IsNull(data))
                    {
                        output = Razor.Run(data);
                        if (output == null)
                        {
                            SendOutput("Error in generating template ", Percent: 80);
                            return;
                        }
                        ss.Content = output;
                        db.Subsection.Add(ss);
                    }
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static void Fill_AD216(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD216> dataList = new List<ForAD216>();
                string output = "";

                List<TouchDownLiftOff> tlof = Globals.GetFeaturesByED(FeatureType.TouchDownLiftOff)?
                    .Cast<Aran.Aim.Features.TouchDownLiftOff>()
                    .Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier)
                    .ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 0
                    };

                    if (tlof != null)
                    {
                        foreach (TouchDownLiftOff item in tlof)
                        {
                            ForAD216 data = new ForAD216();
                            data.Designator = item.Designator;
                            data.Coords = item.Extent?.Geo.ToMultiPoint().ToList().ToPointString();
                            data.GeoidUndulation = item.Extent?.GeoidUndulation?.StringValue;
                            data.Elevation = item.Extent?.Elevation?.StringValue;
                            if (item.Length != null && item.Width != null)
                                data.Dimension = item.Length?.StringValue + "x" + item.Width?.StringValue;
                            else
                                data.Dimension = "".NilIfEmpty();
                            data.Composition = item.SurfaceProperties?.Composition.ToString();

                            data.SurfaceProperties = item.SurfaceProperties?.ToSurfacePropertiesList(SectionName.AD216);


                            data.TrueBearing = "".NilIfEmpty();
                            data.DeclaredDistance = "".NilIfEmpty();
                            data.ApproachLighting = "".NilIfEmpty();
                            if (item.ApproachTakeOffArea?.Identifier != null)
                            {
                                Runway runway = (Globals.GetFeaturesByED(FeatureType.Runway)?.Cast<Runway>())?
                                .FirstOrDefault(x => x.Identifier == item.ApproachTakeOffArea?.Identifier);
                                if (runway != null)
                                {
                                    List<RunwayDirection> runwayDirection = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                                        .Cast<Aran.Aim.Features.RunwayDirection>()
                                        .Where(x => x.UsedRunway?.Identifier == runway.Identifier)
                                        .ToList();
                                    if (runwayDirection != null)
                                    {
                                        foreach (RunwayDirection rwd in runwayDirection)
                                        {
                                            if (rwd.TrueBearing != null)
                                            {
                                                data.TrueBearing = rwd.Designator + ": " + rwd.TrueBearing.ToString();
                                            }
                                            List<RunwayCentrelinePoint> runwayCentrelinePoint = Globals
                                                .GetFeaturesByED(FeatureType.RunwayCentrelinePoint)?
                                                .Cast<Aran.Aim.Features.RunwayCentrelinePoint>()
                                                .Where(x => x.OnRunway?.Identifier == rwd.Identifier)
                                                .ToList();
                                            if (runwayCentrelinePoint != null)
                                            {
                                                foreach (RunwayCentrelinePoint rcp in runwayCentrelinePoint)
                                                {
                                                    foreach (RunwayDeclaredDistance rdd in rcp.AssociatedDeclaredDistance)
                                                    {
                                                        data.DeclaredDistance = rwd.Designator + ": " + rdd.Type?.ToString();
                                                    }
                                                }
                                            }
                                            List<ApproachLightingSystem> approachLightingSystem = Globals
                                                .GetFeaturesByED(FeatureType.ApproachLightingSystem)?
                                                .Cast<Aran.Aim.Features.ApproachLightingSystem>()
                                                .Where(x => x.ServedRunwayDirection?.Identifier == rwd.Identifier)
                                                .ToList();
                                            if (approachLightingSystem != null)
                                            {
                                                foreach (ApproachLightingSystem als in approachLightingSystem)
                                                {
                                                    data.ApproachLighting = rwd.Designator + ": "
                                                        + als.IntensityLevel?.ToString() + ", "
                                                        + als.Type?.ToString() + ", "
                                                        + als.Length?.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            dataList.Add(data);
                        }
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
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD217(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD217 data = new ForAD217();
                string output = "";
                Airspace ase = null;

                List<AirTrafficControlService> atcsList = Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)?
                    .Cast<Aran.Aim.Features.AirTrafficControlService>()
                    .ToList();
                AirTrafficControlService atcs = atcsList?.FirstOrDefault(n =>
                    n.ClientAirport.Any(x => x.Feature?.Identifier == air.Identifier) &&
                    n.ClientAirspace != null);
                Guid? airspaceGuid = atcs?.ClientAirspace.FirstOrDefault()?.Feature?.Identifier;

                // If airspace contains in the AirTrafficControlService
                if (airspaceGuid != null)
                {
                    ase = Globals.GetFeaturesByED(FeatureType.Airspace)?
                            .Cast<Aran.Aim.Features.Airspace>().FirstOrDefault(x => x.Identifier == airspaceGuid);
                }
                else // Else trying to get Airspace from InformationService
                {
                    List<InformationService> insList = Globals.GetFeaturesByED(FeatureType.InformationService)?
                        .Cast<Aran.Aim.Features.InformationService>()
                        .ToList();
                    InformationService ins = insList?.FirstOrDefault(n =>
                        n.ClientAirport.Any(x => x.Feature?.Identifier == air.Identifier) &&
                        n.ClientAirspace != null);
                    if (ins != null)
                    {
                        var clAir = ins.ClientAirspace;
                        if (clAir?.Any() == true && clAir.FirstOrDefault()?.Feature?.Identifier != null)
                        {
                            ase = Globals.GetFeaturesByED(FeatureType.Airspace)?
                                .Cast<Aran.Aim.Features.Airspace>().FirstOrDefault(x => x.Identifier == clAir.FirstOrDefault()?.Feature?.Identifier);
                        }
                    }
                }

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 0
                    };

                    if (ase != null)
                    {
                        data.Designator = ase.Designator;
                        data.Name = ase.Name.Replace(ase.Type.ToString(), "").Replace("/", "");
                        var notePoints = ase.GetHorizontalProjectionAnnotation();
                        data.Coords = notePoints ?? ase.GeometryComponent
                            .SelectManyNullSafe(x => x.TheAirspaceVolume?.HorizontalProjection?.Geo?.ToMultiPoint())
                            .ToList()
                            .ToPointString();
                        data.VerticalLimits = ase.GeometryComponent
                            .Select(x =>
                                x.TheAirspaceVolume?.UpperLimit?.StringValue + " " +
                                x.TheAirspaceVolume?.LowerLimit?.StringValue)
                            .ToList()
                            .ToSeparatedValues();
                        data.AispaceClassification = ase.Class.Select(x => x.Classification.ToString()).ToList()
                            .ToSeparatedValues();

                        data.CallsignDetails = atcs?.CallSign?.Select(x => x.CallSign).ToList().ToSeparatedValues();
                        data.Language = atcs?.CallSign?.Select(x => x.Language).ToList().ToSeparatedValues();

                        data.TransitionAltitude = air?.TransitionAltitude?.StringValue;
                        data.HoursApplicability = Lib.GetHoursOfOperations(atcs?.Availability?.SelectManyNullSafe(x => x.TimeInterval).ToList());
                        data.Remarks = Lib.AIXM_GetNotes(ase.Annotation);

                        if (!Lib.IsNull(data))
                        {
                            output = Razor.Run(data);
                            if (output == null)
                            {
                                SendOutput("Error in generating template ", Percent: 80);
                                return;
                            }
                            ss.Content = output;
                            db.Subsection.Add(ss);
                        }
                    }
                    
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD218(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD218> dataList = new List<ForAD218>();
                string output = "";


                List<InformationService> insList = Globals.GetFeaturesByED(FeatureType.InformationService)?
                    .Cast<Aran.Aim.Features.InformationService>()
                    .Where(n => n.ClientAirport.Select(x => x.Feature?.Identifier)
                        .Contains(air.Identifier))
                    .ToList();
                List<AirTrafficControlService> atcsList = Globals.GetFeaturesByED(FeatureType.AirTrafficControlService)?
                    .Cast<Aran.Aim.Features.AirTrafficControlService>()
                    .Where(n => n.ClientAirport.Select(x => x.Feature?.Identifier)
                        .Contains(air.Identifier))
                    .ToList();
                List<RadioCommunicationChannel> rccList = Globals.GetFeaturesByED(FeatureType.RadioCommunicationChannel)?
                    .Cast<Aran.Aim.Features.RadioCommunicationChannel>()
                    .ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 0
                    };
                    if (insList != null && insList?.Count > 0 ||
                        atcsList != null && atcsList?.Count > 0)
                    {
                        List<Service> srvList = atcsList?.Count > 0 ?
                            new List<Service>(atcsList) :
                            new List<Service>(insList);

                        foreach (Service ins in srvList)
                        {
                            ForAD218 data = new ForAD218();
                            data.ServiceDesignation = ins.Name;
                            data.CallSign = ins.CallSign.Select(x => x.CallSign).ToList().ToSeparatedValues();
                            List<Guid?> radGuids = ins.RadioCommunication.Select(x => x.Feature?.Identifier).ToList();
                            List<RadioCommunicationChannel> radList = rccList?.Where(x => radGuids.ContainsValue(x?.Identifier)).ToList();
                            foreach (RadioCommunicationChannel rad in radList)
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                dic.Add("FrequencyTransmission", rad.FrequencyTransmission?.StringValue);
                                dic.Add("Logon", rad.Logon);
                                data.RadioCommunicationChannel.Add(dic);
                            }
                            data.ServiceOperationalHours = Lib.GetHoursOfOperations(ins.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());
                            data.Remarks = Lib.AIXM_GetNotes(radList.SelectManyNullSafe(x => x.Annotation).ToList());
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
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD219(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD219> dataList = new List<ForAD219>();
                string output = "";


                List<Runway> runway = Globals.GetFeaturesByED(FeatureType.Runway)?
                    .Cast<Aran.Aim.Features.Runway>()
                    .Where(x => x.AssociatedAirportHeliport?.Identifier == air.Identifier)
                    .ToList();
                var runwayGuid = runway.ToGuidList();

                List<RunwayDirection> runwayDirection = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                    .Cast<Aran.Aim.Features.RunwayDirection>()
                    .Where(x => runwayGuid.ContainsValue(x.UsedRunway?.Identifier))
                    .ToList();
                var runwayDirectionGuid = runwayDirection.ToGuidList();

                Dictionary<string, List<Feature>> navaidEquipment = new Dictionary<string, List<Feature>>();
                List<FeatureType> navaidEquipmentType = new List<FeatureType>()
                {
                    FeatureType.VOR,
                    FeatureType.DME,
                    FeatureType.MarkerBeacon,
                    FeatureType.NDB,
                    FeatureType.TACAN,
                    FeatureType.SDF,
                    FeatureType.Localizer,
                    FeatureType.Glidepath,
                    FeatureType.Elevation,
                    FeatureType.DirectionFinder,
                    FeatureType.Azimuth
                };
                foreach (FeatureType type in navaidEquipmentType)
                {
                    navaidEquipment.Add(type.ToString(), Globals.GetFeaturesByED(type));
                }


                List<CodeNavaidService?> allowedTypes = new List<CodeNavaidService?>()
                {
                    CodeNavaidService.VOR,
                    CodeNavaidService.DME,
                    CodeNavaidService.MKR,
                    CodeNavaidService.ILS,
                    CodeNavaidService.ILS_DME,
                    CodeNavaidService.MLS,
                    CodeNavaidService.MLS_DME,
                    CodeNavaidService.VOR_DME,
                    CodeNavaidService.TLS,
                    CodeNavaidService.LOC,
                    CodeNavaidService.LOC_DME
                };
                List<Aran.Aim.Features.Navaid> navaidList = Globals.GetFeaturesByED(FeatureType.Navaid)?
                    .Cast<Aran.Aim.Features.Navaid>()
                    .Where(x => runwayDirectionGuid != null &&
                           x.RunwayDirection.Any(n => runwayDirectionGuid.ContainsValue(n.Feature?.Identifier)) &&
                           allowedTypes.Contains(x.Type)
                           )
                    .ToList();

                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection
                    {
                        AIPSection = ent,
                        eAIP = caip,
                        SubClassType = SubClassType.Subsection,
                        OrderNumber = 0
                    };
                    if (navaidList != null && navaidList?.Count > 0)
                    {
                        //RulesProcedures rp = Globals.GetFeaturesByED(FeatureType.RulesProcedures)?
                        //    .Cast<Aran.Aim.Features.RulesProcedures>()
                        //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_OPS_TYPES_ILS_MLS_GNASS_SBAS_GBAS &&
                        //        ForAirportHeliport(x.AffectedLocation, air) && Lib.IsLanguageNote(x.Annotation));

                        foreach (Aran.Aim.Features.Navaid navaid in navaidList)
                        {
                            ForAD219 data = new ForAD219();
                            data.Type = navaid.Type?.ToString().Replace("_", "/");
                            data.Designator = navaid.Designator;
                            //data.TypeSupportedOperation = rp?.Content.ToXhtml();
                            data.HoursOfOperations = Lib.GetHoursOfOperations(navaid.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());

                            List<AbstractNavaidEquipmentRef> navaidEquipmentList = navaid.NavaidEquipment.Select(x => x.TheNavaidEquipment).ToList();
                            foreach (AbstractNavaidEquipmentRef navEq in navaidEquipmentList)
                            {
                                Feature tmp = navaidEquipment.ContainsKey(navEq.Type.ToString())
                                    ? navaidEquipment[navEq.Type.ToString()]
                                        .FirstOrDefault(x => x.Identifier == navEq.Identifier) : null;
                                if (tmp != null)
                                {
                                    Dictionary<string, string> dic = new Dictionary<string, string>();
                                    NavaidEquipment nav = (NavaidEquipment)tmp;

                                    dic.Add("Frequency", (tmp is VOR) ? ((VOR)tmp).Frequency?.StringValue : String.Empty);
                                    dic.Add("MagneticVariation", nav.MagneticVariation?.ToString());
                                    dic.Add("PositionOfTransmittingAntenna", nav.Location?.Geo?.ToPointString());
                                    dic.Add("ElevationOfDME", (tmp is DME) ? ((DME)tmp).Location?.Elevation?.StringValue : String.Empty);
                                    data.NavaidEquipment.Add(dic);
                                }
                            }

                            data.Remarks = Lib.AIXM_GetNotes(navaid.Annotation);
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
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD220(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {

                List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                {
                    CodeRuleProcedureTitle.OTHER_LOCAL_AERODROME_REGULATIONS
                };
                List<RulesProcedures> ft = Globals.GetFeaturesByED(FeatureType.RulesProcedures)
                                            .Cast<RulesProcedures>()
                                            .Where(n => ParamsOrdered.Contains(n.Title) &&
                                                        ForAirportHeliport(n.AffectedLocation, air) &&
                                                        Lib.IsLanguageNote(n.Annotation))
                                            .ToList();

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_LOCAL_AERODROME_REGULATIONS);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        //ss.Title = Lib.GetText(TitleLang);// rp.Title.ToString().Replace("OTHER_", "").Replace("_", " ");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();
                        db.Subsection.Add(ss);
                    }
                    
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD221(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {

                List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                {
                    CodeRuleProcedureTitle.NOISE_ABATEMENT_PROCEDURES
                };
                List<RulesProcedures> ft = Globals.GetFeaturesByED(FeatureType.RulesProcedures)
                                            .Cast<RulesProcedures>()
                                            .Where(n => ParamsOrdered.Contains(n.Title) &&
                                                        ForAirportHeliport(n.AffectedLocation, air) &&
                                                        Lib.IsLanguageNote(n.Annotation))
                                            .ToList();

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.NOISE_ABATEMENT_PROCEDURES);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        //ss.Title = Lib.GetText(TitleLang);// rp.Title.ToString().Replace("OTHER_", "").Replace("_", " ");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD222(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {

                List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                {
                    CodeRuleProcedureTitle.OTHER_FLIGHT_PROCEDURES
                };
                List<RulesProcedures> ft = Globals.GetFeaturesByED(FeatureType.RulesProcedures)
                                            .Cast<RulesProcedures>()
                                            .Where(n => ParamsOrdered.Contains(n.Title) &&
                                                        ForAirportHeliport(n.AffectedLocation, air) &&
                                                        Lib.IsLanguageNote(n.Annotation))
                                            .ToList();

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_FLIGHT_PROCEDURES);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        //ss.Title = Lib.GetText(TitleLang);// rp.Title.ToString().Replace("OTHER_", "").Replace("_", " ");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();
                        db.Subsection.Add(ss);
                    }
                    
                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void Fill_AD223(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {

                List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                {
                    CodeRuleProcedureTitle.OTHER_ADDITIONAL_INFORMATION
                };
                List<RulesProcedures> ft = Globals.GetFeaturesByED(FeatureType.RulesProcedures)
                                            .Cast<RulesProcedures>()
                                            .Where(n => ParamsOrdered.Contains(n.Title) &&
                                                        ForAirportHeliport(n.AffectedLocation, air) &&
                                                        Lib.IsLanguageNote(n.Annotation))
                                            .ToList();

                AIP.DB.Subsection ss;
                RulesProcedures rp = null;

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_ADDITIONAL_INFORMATION);
                    if (rp != null)
                    {
                        ss.AIPSection = ent;
                        //ss.Title = Lib.GetText(TitleLang);// rp.Title.ToString().Replace("OTHER_", "").Replace("_", " ");
                        ss.eAIP = caip;
                        ss.SubClassType = SubClassType.Subsection;
                        ss.OrderNumber = ParamsOrdered.IndexOf(rp.Title);
                        ss.Content += rp.Content.ToXhtml();
                        db.Subsection.Add(ss);
                    }

                    ent.SectionStatus = SectionStatusEnum.Filled;
                    SendSaveOutput(ent, air);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        

        #endregion


    }

}