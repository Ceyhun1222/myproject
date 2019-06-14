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
    /// AD3 section
    /// Very similar to AD2
    /// </summary>
    internal static partial class FillDB
    {
        #region AD3
        public static void Fill_AD30(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
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

        public static void Fill_AD31(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
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

        public static void Fill_AD323(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
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

        public static void Fill_AD32(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD32 Data = new ForAD32();
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
                    var tmp1 = oa?.Where(n => n.Identifier == air?.ResponsibleOrganisation?.TheOrganisationAuthority?.Identifier).FirstOrDefault()?
                        .Contact?.FirstOrDefault()?.PhoneFax?.FirstOrDefault();
                    OnlineContact tmp2 = oa?.Where(n => n.Identifier == air.ResponsibleOrganisation?.TheOrganisationAuthority?.Identifier).FirstOrDefault()?
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

        public static void Fill_AD33(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD33 Data = new ForAD33();
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


                    Data.ADAdministration = (air.ResponsibleOrganisation?.Role == CodeAuthorityRole.OPERATE) ?
                        Lib.GetHoursOfOperations(air.ResponsibleOrganisation?.TimeInterval) : "";
                    Data.ADAdministration = Data.ADAdministration.NilIfEmpty();

                    Data.CustomAndImmigration = ps?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                    && n.Type == CodePassengerService.CUST)
                    .SelectManyNullSafe(x => x.Availability.Select(y => y.OperationalStatus.ToString()))
                    .ToList()
                    .ToSeparatedValues().NilIfEmpty();

                    Data.HealthAndSanitation = ps?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
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
                    Data.Fuelling = Lib.GetHoursOfOperations(ass?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
                            .Contains(air.Identifier))
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();

                    Data.Handling = Lib.GetHoursOfOperations(ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
                            .Contains(air.Identifier) && n.Type == CodeAircraftGroundService.HAND)
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();
                    Data.Security = Lib.GetHoursOfOperations(ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
                                                                                 .Contains(air.Identifier) && n.Type == CodeAircraftGroundService.OTHER)
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();
                    Data.Deicing = Lib.GetHoursOfOperations(ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
                                                                                 .Contains(air.Identifier) && n.Type == CodeAircraftGroundService.DEICE)
                        .SelectManyNullSafe(
                            x => x.Availability.SelectManyNullSafe(y => y.TimeInterval)).ToList()).NilIfEmpty();


                    Data.Remarks = Lib.AIXM_GetNotes(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
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

        public static void Fill_AD34(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD34 Data = new ForAD34();
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
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                    && n.Type == CodeAircraftGroundService.HAND)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList(), CodeNotePurpose.DESCRIPTION).NilIfEmpty();
                    List<AirportSuppliesService> assForAH = ass?.Where(n => n.AirportHeliport
                        .Select(x => x.Feature?.Identifier)
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
                        .SelectManyNullSafe(x => x.SupplyService).Select(u => u.Feature?.Identifier);
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
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                        && n.Type == CodeAircraftGroundService.DEICE)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.HangarSpace = Lib.AIXM_GetNotes(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                        && n.Type == CodeAircraftGroundService.HANGAR)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.RepairFacilities = Lib.AIXM_GetNotes(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                        && n.Type == CodeAircraftGroundService.REPAIR)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.Remarks = Lib.AIXM_GetNotesByPurpose(
                        ags?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
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

        public static void Fill_AD35(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD35 Data = new ForAD35();
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
                        .Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.HOTEL)
                            .SelectManyNullSafe(x => x.Annotation)
                            .ToList()).NilIfEmpty();

                    Data.Restaurants = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.REST)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();

                    Data.Transportation = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.TRANSPORT)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.MedicalFacilities = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.MEDIC)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.BankPostOffice = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                    && (n.Type == CodePassengerService.BANK || n.Type == CodePassengerService.POST))
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();
                    Data.TouristOffice = Lib.AIXM_GetNotes(ps?
                        .Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier).Contains(air.Identifier)
                                    && n.Type == CodePassengerService.INFO)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList()).NilIfEmpty();

                    Data.Remarks = Lib.AIXM_GetNotesByPurpose(
                        ps?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
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

        public static void Fill_AD36(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD36 Data = new ForAD36();
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

        public static void Fill_AD37(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD37 Data = new ForAD37();
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
                    //    .Content.ToXhtml()
                    //    .NilIfEmpty();

                    //Data.ClearancePriorities = rp
                    //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_CLEARANCE_PRIORITIES)?
                    //    .Content.ToXhtml()
                    //    .NilIfEmpty();

                    Data.Remarks = Lib.AIXM_GetNotesByPurpose(
                        acs?.Where(n => n.AirportHeliport.Select(x => x.Feature?.Identifier)
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

        public static void Fill_AD38(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD38 Data = new ForAD38();
                string output = "";


                List<Apron> apron = Globals.GetFeaturesByED(FeatureType.Apron)?
                    .Cast<Aran.Aim.Features.Apron>()
                    .ToList();
                List<Taxiway> tax = Globals.GetFeaturesByED(FeatureType.Taxiway)?
                    .Cast<Aran.Aim.Features.Taxiway>()
                    .ToList();
                List<CheckpointVOR> cpv = Globals.GetFeaturesByED(FeatureType.CheckpointVOR)?
                    .Cast<Aran.Aim.Features.CheckpointVOR>()
                    .ToList();
                List<CheckpointINS> cpi = Globals.GetFeaturesByED(FeatureType.CheckpointINS)?
                    .Cast<Aran.Aim.Features.CheckpointINS>()
                    .ToList();


                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;


                    List<Apron> aprList = apron?.Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier).ToList();
                    if (aprList != null)
                    {
                        var apronGuid = aprList?
                            .Select(x => x.Identifier)
                            .ToList();

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

                            List<ApronElement> apronElem = Globals.GetFeaturesByED(FeatureType.ApronElement)?.Cast<Aran.Aim.Features.ApronElement>()
                                .Where(n => apronGuid.Contains(apr.Identifier))
                                .ToList();
                            List<Guid?> apronElemGuid = apronElem?
                                .Select(x => x?.Identifier)
                                .ToList();
                            List<AircraftStand> aircraftStand = Globals.GetFeaturesByED(FeatureType.AircraftStand)?.Cast<Aran.Aim.Features.AircraftStand>()
                                .Where(n => apronElemGuid.ContainsValue(n.ApronLocation?.Identifier))
                                .ToList();
                            string str = "";
                            if (aircraftStand?.Count > 0)
                            {
                                apronDic.Add("AircraftComposition", aircraftStand?.Select(x => x.SurfaceProperties?.Composition?.ToString()).ToList().ToSeparatedValues());
                            }
                            else
                            {
                                apronDic.Add("AircraftComposition", "");
                            }
                            Data.ApronData.Add(apronDic);
                        }
                    }

                    List<Taxiway> taxList = tax?.Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier).ToList();
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
                    Data.VORCheckpoints = cpv?.Where(n => n.AirportHeliport?.Identifier == air.Identifier)
                            .Select(x => x.Position.Geo)
                        .ToList().ToPointString();

                    Data.INSCheckpoints = cpi?.Where(n => n.AirportHeliport?.Identifier == air.Identifier)
                            .Select(x => x.Position.Geo)
                            .ToList().ToPointString();



                    Data.Remarks = Lib.AIXM_GetNotes(
                            apron?.Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier)
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList());
                    Data.Remarks += Lib.AIXM_GetNotes(
                            tax?.Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier)
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

        public static void Fill_AD39(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD39 Data = new ForAD39();
                string output = "";

                List<Apron> apron = Globals.GetFeaturesByED(FeatureType.Apron)?.Cast<Aran.Aim.Features.Apron>()
                    .Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier)
                    .ToList();
                var apronGuid = apron.ToGuidList();

                List<ApronElement> apronElem = Globals.GetFeaturesByED(FeatureType.ApronElement)?.Cast<Aran.Aim.Features.ApronElement>()
                    .Where(n => apronGuid.ContainsValue(n.AssociatedApron?.Identifier))
                    .ToList();
                var apronElemGuid = apronElem.ToGuidList();

                List<Taxiway> taxiway = Globals.GetFeaturesByED(FeatureType.Taxiway)?.Cast<Aran.Aim.Features.Taxiway>()
                    .Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier)
                    .ToList();
                var taxiwayGuid = taxiway?
                    .Select(x => x.Identifier)
                    .ToList();

                List<TaxiwayMarking> taxiwayMarking = Globals.GetFeaturesByED(FeatureType.TaxiwayMarking)?.Cast<Aran.Aim.Features.TaxiwayMarking>()
                    .Where(n => taxiwayGuid != null && n.MarkedTaxiway != null && taxiwayGuid.Contains(n.MarkedTaxiway.Identifier))
                    .ToList();

                List<AircraftStand> aircraftStand = Globals.GetFeaturesByED(FeatureType.AircraftStand)?.Cast<Aran.Aim.Features.AircraftStand>()
                    .Where(n => apronElemGuid != null && n.ApronLocation != null && apronElemGuid.Contains(n.ApronLocation.Identifier))
                    .ToList();
                var aircraftGuid = aircraftStand?
                    .Select(x => x.Identifier)
                    .ToList();
                List<StandMarking> standMarking = Globals.GetFeaturesByED(FeatureType.StandMarking)?.Cast<Aran.Aim.Features.StandMarking>()
                    .Where(n => aircraftGuid != null && aircraftGuid.Contains(n.MarkedStand.Identifier))
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
                    {
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
                    }

                    Data.Remarks = Lib.AIXM_GetNotes(taxiway?
                        .SelectManyNullSafe(x => x.Annotation)
                        .ToList());
                    Data.Remarks += Lib.AIXM_GetNotes(runway?
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

        public static void Fill_AD310(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD310 Data = new ForAD310();
                string output = "";

                List<ObstacleArea> oa = Globals.GetFeaturesByED(FeatureType.ObstacleArea)?
                    .Cast<Aran.Aim.Features.ObstacleArea>()
                    .Where(n => n.Reference.OwnerAirport.Identifier == air.Identifier)
                    .ToList();
                var vsGuid2 = oa?.SelectManyNullSafe(n => n.Obstacle).Select(x => x.Feature.Identifier).ToList();
                List<VerticalStructure> vs2 = Globals.GetFeaturesByED(FeatureType.VerticalStructure)?
                    .Cast<Aran.Aim.Features.VerticalStructure>()
                    .Where(n => vsGuid2 != null && vsGuid2.Contains(n.Identifier))
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

                    // obstacles 

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

                    Data.Area2Remarks = Lib.AIXM_GetNotesByPurpose(
                            air?.Annotation,
                            CodeNotePurpose.OTHER_OBS_AREA2_ELIST_AVAILABILITY)
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

        public static void Fill_AD311(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD311 Data = new ForAD311();
                string output = "";

                List<InformationService> ins = Globals.GetFeaturesByED(FeatureType.InformationService)?
                    .Cast<Aran.Aim.Features.InformationService>()
                    .Where(n => n.Type == CodeServiceInformation.TAF &&
                            n.ClientAirport.Any(x => x.Feature.Identifier == air.Identifier))
                    .ToList();
                Guid? unitGuid = ins?.Select(x => x.ServiceProvider.Identifier).FirstOrDefault();

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
                        //    .Content.ToXhtml()
                        //    .NilIfEmpty();

                        //Data.Charts = rp
                        //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_CHARTS_OTHER_INFO_FOR_BRIEFING)?
                        //    .Content.ToXhtml()
                        //    .NilIfEmpty();

                        //Data.SupplementaryEquipment = rp
                        //    .FirstOrDefault(x => x.Title == CodeRuleProcedureTitle.OTHER_AIP_AHP_SUPP_MET_EQPT)?
                        //    .Content.ToXhtml()
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

        public static void Fill_AD312(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD312 data = new ForAD312();
                string output = "";
                List<TouchDownLiftOff> tlof = Globals.GetFeaturesByED(FeatureType.TouchDownLiftOff)?
                    .Cast<Aran.Aim.Features.TouchDownLiftOff>()
                    .Where(n => n.AssociatedAirportHeliport?.Identifier == air.Identifier)
                    .ToList();

                List<Runway> rwFATO = Globals.GetFeaturesByED(FeatureType.Runway)?
                    .Cast<Aran.Aim.Features.Runway>()
                    .Where(x => x.AssociatedAirportHeliport?.Identifier == air.Identifier &&
                                x.Type == CodeRunway.FATO)
                    .ToList();
                var runwayGuid = rwFATO.ToGuidList();

                List<Runway> rwAll = Globals.GetFeaturesByED(FeatureType.Runway)?
                    .Cast<Aran.Aim.Features.Runway>()
                    .Where(x => x.AssociatedAirportHeliport?.Identifier == air.Identifier)
                    .ToList();
                var runwayAllGuid = rwAll.ToGuidList();
                List<RunwayDirection> rdAll = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                    .Cast<Aran.Aim.Features.RunwayDirection>()
                    .Where(x => runwayAllGuid.ContainsValue(x.UsedRunway?.Identifier))
                    .ToList();


                List<TouchDownLiftOff> tlofrw = Globals.GetFeaturesByED(FeatureType.TouchDownLiftOff)?
                    .Cast<Aran.Aim.Features.TouchDownLiftOff>()
                    .Where(n => runwayGuid.ContainsValue(n.ApproachTakeOffArea?.Identifier))
                    .ToList();

                List<RunwayDirection> rdFATO = Globals.GetFeaturesByED(FeatureType.RunwayDirection)?
                    .Cast<Aran.Aim.Features.RunwayDirection>()
                    .Where(x => runwayGuid.ContainsValue(x.UsedRunway?.Identifier))
                    .ToList();
                var runwayDirectionGuid = rdFATO.ToGuidList();

                List<RunwayProtectArea> rpa = Globals.GetFeaturesByED(FeatureType.RunwayProtectArea)?
                    .Cast<Aran.Aim.Features.RunwayProtectArea>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.ProtectedRunwayDirection?.Identifier))
                    .ToList();
                AIP.DB.Subsection ss;
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ss = new DB.Subsection();
                    ss.AIPSection = ent;
                    ss.eAIP = caip;
                    ss.SubClassType = SubClassType.Subsection;
                    ss.OrderNumber = 0;

                    data.HeliportType =
                        Lib.AIXM_GetNotesByPurpose(air.Annotation, CodeNotePurpose.OTHER_HELIPORT_TLOF_AREA_TYPE);

                    //if (!tlofrw.IsNullOrEmpty())
                    //    data.FATOElevation = tlofrw.FirstOrDefault().Extent.Geo.Centroid.ToPointString();
                    if (!rdFATO.IsNullOrEmpty())
                    {
                        data.FATOSlope = rdFATO?.FirstOrDefault()?.SlopeTDZ?.ToString();
                        data.FATOElevation = rdFATO?.FirstOrDefault()?.ElevationTDZ?.StringValue;
                    }
                    if (!rpa.IsNullOrEmpty())
                    {
                        var rpaFD = rpa.FirstOrDefault(x => x.Type == CodeRunwayProtectionArea.RESA);
                        data.SafetyArea = rpaFD?.Width?.StringValue + "x" + rpaFD?.Length?.StringValue;

                        rpaFD = rpa.FirstOrDefault(x => x.Type == CodeRunwayProtectionArea.CWY);
                        data.HELCWYDimensions = rpaFD?.Width?.StringValue + "x" + rpaFD?.Length?.StringValue;

                        var any = rpa?.Any(x => x.Type == CodeRunwayProtectionArea.OFZ);
                        data.ObstacleFreeSector = any.ToYesNoNil();
                    }
                    foreach (TouchDownLiftOff tl in tlof)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("Designator", tl.Designator?.NilIfEmpty());
                        dic.Add("TLOFDimensions", tl.Extent?.Elevation?.StringValue.NilIfEmpty());
                        dic.Add("SurfaceType", tl.SurfaceProperties?.Composition?.ToString().NilIfEmpty());
                        dic.Add("BRGStrength", tl.SurfaceProperties?.WeightAUW?.StringValue.NilIfEmpty());
                        dic.Add("TLOFCoordinates", tl.Extent?.Geo?.Centroid?.ToPointString().NilIfEmpty());
                        dic.Add("TLOFElevation", runwayGuid?.ContainsValue(tl.ApproachTakeOffArea?.Identifier) == true ? tl?.Extent?.Geo?.Centroid?.ToPointString().NilIfEmpty() : "".NilIfEmpty());
                        dic.Add("TLOFSlope", tl.Slope.ToString().NilIfEmpty());


                        if (tl.ApproachTakeOffArea?.Identifier != null)
                        {
                            Runway runway = (Globals.GetFeaturesByED(FeatureType.Runway)?.Cast<Runway>())?
                                .FirstOrDefault(x => x.Identifier == tl.ApproachTakeOffArea?.Identifier);
                            if (runway != null)
                            {
                                List<RunwayDirection> runwayDirection = Globals
                                    .GetFeaturesByED(FeatureType.RunwayDirection)?
                                    .Cast<Aran.Aim.Features.RunwayDirection>()
                                    .Where(x => x.UsedRunway?.Identifier == runway.Identifier)
                                    .ToList();
                                if (runwayDirection != null)
                                {
                                    foreach (RunwayDirection rwd in runwayDirection)
                                    {
                                        if (rwd.TrueBearing != null)
                                        {
                                            data.FATOTrueBearings += rwd.Designator + ": " + rwd.TrueBearing.ToString() + ",";
                                        }
                                    }
                                    data.FATOTrueBearings = data.FATOTrueBearings.TrimEnd(',').NilIfEmpty();
                                }
                            }
                        }
                    }
                    data.Remarks = Lib.AIXM_GetNotes(rdAll.SelectManyNullSafe(x => x.Annotation).ToList());
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

        public static void Fill_AD313(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD313> dataList = new List<ForAD313>();
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

                List<RunwayCentrelinePoint> runwayCenterlinePointList = Globals.GetFeaturesByED(FeatureType.RunwayCentrelinePoint)?
                    .Cast<Aran.Aim.Features.RunwayCentrelinePoint>()
                    .Where(x => runwayDirectionGuid.ContainsValue(x.OnRunway?.Identifier))
                    .ToList();
                var runwayCenterlinePointGuid = runwayCenterlinePointList.ToGuidList();


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
                                List<RunwayCentrelinePoint> runwayCenterlinePointList2 =
                                    runwayCenterlinePointList?.Where(x => x.OnRunway?.Identifier == rwd.Identifier)
                                    .ToList();
                                ForAD313 data = new ForAD313();
                                data.Designator = rw.Designator;
                                if (runwayCenterlinePointList != null)
                                {
                                    foreach (RunwayCentrelinePoint rcpl in runwayCenterlinePointList2)
                                    {
                                        foreach (RunwayDeclaredDistance rdd in rcpl.AssociatedDeclaredDistance)
                                        {


                                            foreach (RunwayDeclaredDistanceValue rddv in rdd.DeclaredValue)
                                            {
                                                if (rdd.Type == CodeDeclaredDistance.TODAH)
                                                {
                                                    data.TODAHDistance = rddv.Distance?.StringValue;
                                                }
                                                if (rdd.Type == CodeDeclaredDistance.RTODAH)
                                                {
                                                    data.RTODAHDistance = rddv.Distance?.StringValue;
                                                }
                                                if (rdd.Type == CodeDeclaredDistance.LDAH)
                                                {
                                                    data.LDAHDistance = rddv.Distance?.StringValue;
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

        public static void Fill_AD314(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD314 data = new ForAD314();
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
                List<Guid?> runwayDirectionGuid = runwayDirection.ToGuidList();
                List<TouchDownLiftOff> TouchDownLiftOff = Globals.GetFeaturesByED(FeatureType.TouchDownLiftOff)?
                    .Cast<Aran.Aim.Features.TouchDownLiftOff>()
                    .Where(x => runwayGuid.ContainsValue(x.ApproachTakeOffArea?.Identifier))
                    .ToList();
                var TouchDownLiftOffGuid = TouchDownLiftOff.ToGuidList();
                List<TouchDownLiftOffLightSystem> TouchDownLiftOffLightSystem = Globals.GetFeaturesByED(FeatureType.TouchDownLiftOffLightSystem)?
                    .Cast<Aran.Aim.Features.TouchDownLiftOffLightSystem>()
                    .Where(x => TouchDownLiftOffGuid.ContainsValue(x.LightedTouchDownLiftOff?.Identifier))
                    .ToList();



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
                List<Guid?> runwayProtectAreaGuid = runwayProtectArea.ToGuidList();

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

                    //foreach (RunwayDirection rwd in runwayDirection)
                    //{

                    //List<ApproachLightingSystem> approachLightingSystemList =
                    //    approachLightingSystem?.Where(x => x.ServedRunwayDirection.Identifier == rwd.Identifier)
                    //    .ToList();

                    if (approachLightingSystem != null)
                    {
                        data.ALSIntensityLevel = approachLightingSystem?.FirstOrDefault(x => x.IntensityLevel != null)?.IntensityLevel?.ToString();
                        data.ALSType = approachLightingSystem.FirstOrDefault(x => x.Type != null)?.Type?.ToString();
                        data.ALSLength = approachLightingSystem.FirstOrDefault(x => x.Length != null)?.Length?.ToString();
                        data.ALSAll = (data.ALSIntensityLevel + data.ALSType + data.ALSLength).Trim() == ""
                            ? $@"{data.ALSIntensityLevel}, {data.ALSType}, {data.ALSLength}"
                            : "".NilIfEmpty();
                    }

                    data.VASISType = visualGlideSlopeIndicator?.FirstOrDefault()?.Type.ToString().NilIfEmpty();
                    data.FATOCharacteristics = "";

                    foreach (var tdlo in TouchDownLiftOff)
                    {
                        List<TouchDownLiftOffLightSystem> tlofLS = Globals.GetFeaturesByED(FeatureType.TouchDownLiftOffLightSystem)?
                            .Cast<Aran.Aim.Features.TouchDownLiftOffLightSystem>()
                            .Where(x => tdlo.Identifier == x.LightedTouchDownLiftOff?.Identifier)
                            .ToList();

                        foreach (var tlofLSitem in tlofLS)
                        {
                            if (tlofLSitem.Position == CodeTLOFSection.AIM)
                            {
                                data.LocationAPL = tdlo?.Extent?.Geo?.Centroid?.ToPointString().NilIfEmpty();
                                List<string> tmp = new List<string>();
                                tmp.Add(TouchDownLiftOffLightSystem.FirstOrDefault()?.IntensityLevel.ToString());
                                tmp.Add(TouchDownLiftOffLightSystem.FirstOrDefault()?.Colour.ToString());
                                tmp.Add(TouchDownLiftOffLightSystem.FirstOrDefault()?.Position.ToString());
                                data.CharacteristicsAPL = tmp.ToSeparatedValues();
                            }
                            if (tlofLSitem.Position == CodeTLOFSection.EDGE)
                            {
                                data.LocationTLOF = tdlo?.Extent?.Geo?.Centroid?.ToPointString().NilIfEmpty();
                                List<string> tmp = new List<string>();
                                tmp.Add(TouchDownLiftOffLightSystem.FirstOrDefault()?.IntensityLevel.ToString());
                                tmp.Add(TouchDownLiftOffLightSystem.FirstOrDefault()?.Colour.ToString());
                                tmp.Add(TouchDownLiftOffLightSystem.FirstOrDefault()?.Position.ToString());
                                data.CharacteristicsTLOF = tmp.ToSeparatedValues();
                            }
                        }
                    }


                    data.Remarks = Lib.AIXM_GetNotes(runwayDirectionLightSystem.SelectManyNullSafe(x => x.Annotation).ToList());
                    //}

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

        public static void Fill_AD315(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD315 data = new ForAD315();
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

        public static void Fill_AD316(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                ForAD316 data = new ForAD316();
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
                    Guid? insGuid = ins?.ClientAirspace.FirstOrDefault()?.Feature?.Identifier;
                    if (insGuid != null)
                    {
                        ase = Globals.GetFeaturesByED(FeatureType.Airspace)?
                            .Cast<Aran.Aim.Features.Airspace>().FirstOrDefault(x => x.Identifier == insGuid);
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
                        data.Coords = ase.GeometryComponent
                            .SelectManyNullSafe(x => x.TheAirspaceVolume.HorizontalProjection.Geo.ToMultiPoint())
                            .ToList()
                            .ToPointString();
                        data.VerticalLimits = ase.GeometryComponent
                            .Select(x =>
                                x.TheAirspaceVolume.UpperLimit?.StringValue + " " +
                                x.TheAirspaceVolume.LowerLimit?.StringValue)
                            .ToList()
                            .ToSeparatedValues();
                        data.AispaceClassification = ase.Class.Select(x => x.Classification.ToString()).ToList()
                            .ToSeparatedValues();

                        data.CallsignDetails = atcs?.CallSign.Select(x => x.CallSign).ToList().ToSeparatedValues();
                        data.Language = atcs?.CallSign.Select(x => x.Language).ToList().ToSeparatedValues();

                        data.TransitionAltitude = air.TransitionAltitude?.StringValue;
                        data.HoursApplicability = Lib.GetHoursOfOperations(atcs?.Availability.SelectManyNullSafe(x => x.TimeInterval).ToList());
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

        public static void Fill_AD317(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD317> dataList = new List<ForAD317>();
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
                            ForAD317 data = new ForAD317();
                            data.ServiceDesignation = ins.Name;
                            data.CallSign = ins.CallSign.Select(x => x.CallSign).ToList().ToSeparatedValues();
                            var radGuids = ins.RadioCommunication.Select(x => x.Feature?.Identifier).ToList();
                            List<RadioCommunicationChannel> radList = rccList?.Where(x => radGuids.Contains(x.Identifier)).ToList();
                            foreach (RadioCommunicationChannel rad in radList)
                            {
                                Dictionary<string, string> dic = new Dictionary<string, string>();
                                dic.Add("FrequencyTransmission", rad.FrequencyTransmission?.StringValue);
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

        public static void Fill_AD318(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {
                List<ForAD318> dataList = new List<ForAD318>();
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

                List<Aran.Aim.Features.Navaid> navaidList = Globals.GetFeaturesByED(FeatureType.Navaid)?
                    .Cast<Aran.Aim.Features.Navaid>()
                    .Where(x => runwayDirectionGuid != null &&
                           x.RunwayDirection.Any(n => runwayDirectionGuid.Contains(n.Feature?.Identifier))
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
                        foreach (Aran.Aim.Features.Navaid navaid in navaidList)
                        {
                            ForAD318 data = new ForAD318();
                            data.Type = navaid.Type?.ToString().Replace("_", "/");
                            data.Designator = navaid.Designator;
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

        public static void Fill_AD319(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
        {
            try
            {

                List<CodeRuleProcedureTitle?> ParamsOrdered = new List<CodeRuleProcedureTitle?>()
                {
                    CodeRuleProcedureTitle.OTHER_LOCAL_HELIPORT_REGULATIONS
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
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.OTHER_LOCAL_HELIPORT_REGULATIONS);
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

        public static void Fill_AD320(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
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

        public static void Fill_AD321(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
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

                    ss = new DB.Subsection();
                    rp = ft.FirstOrDefault(n => n.Title == CodeRuleProcedureTitle.AERODROME_FLIGHT_PROCEDURES);
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

        public static void Fill_AD322(AIPSection ent, List<Feature> featureList, DB.eAIP caip, Aran.Aim.Features.AirportHeliport air)
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