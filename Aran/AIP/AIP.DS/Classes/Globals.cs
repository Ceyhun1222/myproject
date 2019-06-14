using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.CommonUtil.Context;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using Aran.Aim.AixmMessage;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.CommonUtil.Util;
using AimDbNamespaces = Aran.Aim.AixmMessage.AimDbNamespaces;
using Filter = AIP.DataSet.Classes.Filter;

namespace AIP.DataSet
{
    internal static class Globals
    {
        internal static ConnectionType? connType = null;
        internal static DbProvider dbPro;
        internal static Connection conn;
        internal static string ConnectionString;
        internal static bool IsAMDTPreview;
        internal static ITemporalityService<AimFeature> temporalityService;
        internal static PrivateSlot ActivePrivateSlot = null;
        internal static bool isDeactivated = false;



        // TODO: change to false
        internal static bool DirectTOSSMConnection = true;

        static Globals()
        {
            try
            {
                InitializeConnection();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }

        public static void InitializeConnection()
        {
            try
            {
                conn = new Connection();
                conn.ConnectionType = ConnectionType.XmlFile;
                if (connType == null) connType = ConnectionType.TDB;//Properties.Settings.Default.ConnectionType;
                ConnectionString = "";//System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;



                if (connType == ConnectionType.TDB)
                {
                    if (DirectTOSSMConnection)
                    {
                        ConnectionProvider.Open();
                        temporalityService = CurrentDataContext.CurrentService;
                        dbPro = DbProviderFactory.Create("Aran.Temporality.Provider");
                        var connectionStrings = dbPro.GetConnectionStrings();

                    dbPro.Open(connectionStrings.FirstOrDefault());

                        if (!dbPro.Login(conn.UserName, DbUtility.GetMd5Hash(conn.Password)))
                        {
                            dbPro.Close();
                            throw new Exception("Invalid User Name or Password!");
                        }
                        //var tossmProblem = dbPro.GetAllFeatuers(FeatureType.AirportHeliport);
                        //dbPro.DefaultEffectiveDate = Lib.CurrentAIP.Effectivedate;
                        return;
                    }
                    dbPro = DbProviderFactory.Create("Aran.Temporality.Provider");
                    dbPro.Open(null);
                    //try
                    //{
                    //    //TODO: Detect why TOSSM return error in first connection
                    //    // This is test only
                    //    var tossmProblem = dbPro.GetAllFeatuers(FeatureType.AirportHeliport);
                    //    List<AirportHeliport> ft = tossmProblem.OfType<AirportHeliport>().ToList();
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine("Error when first connect");
                    //}

                    return;
                }
                else if (connType == ConnectionType.Aran)
                {
                    dbPro = DbProviderFactory.Create("Aran.Aim.Data.PgDbProviderComplex");
                    conn.ConnectionType = ConnectionType.Aran;
                    conn.Database = "AIP_QATAR";
                    conn.Port = 5432;
                    conn.Server = "127.0.0.1";
                    conn.UserName = "administrator";
                    conn.Password = "aim_administrator";
                    //conn.UserName = "aran";
                    //conn.Password = "airnav2012";
                }
                else if (connType == ConnectionType.ComSoft)
                {
                    throw new Exception("Not implemented");
                    //dbPro = DbProviderFactory.Create("Aran.Aim.CawProvider");
                }
                else if (connType == ConnectionType.XmlFile)
                {
                    //throw new Exception("Not implemented");
                    dbPro = DbProviderFactory.Create("Aran.Aim.Data.XmlProvider");
                    conn.XmlFileName = @"C:\CurrentProjects\Panda\AirNav\bin\Debug\Data\AIPDataSet\2018-06-21-AIRAC\2018-06-21-AIRAC-Unpublished-Latvia_APR-Lat_v5\EV_AIP_DataSet_2018-06-21-AIRAC-Unpublished-Latvia_APR-Lat.xml";
                }

                dbPro.Open(conn.GetConnectionString());

                if (!dbPro.Login(conn.UserName, DbUtility.GetMd5Hash(conn.Password)))
                {
                    dbPro.Close();
                    throw new Exception("Invalid User Name or Password!");
                }
                //dbPro.DefaultEffectiveDate = Lib.CurrentAIP.Effectivedate;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static string Server()
        {
            try
            {
                return CurrentDataContext.ServiceHost;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string DB()
        {
            try
            {
                return CurrentDataContext.StorageName;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string User()
        {
            try
            {
                return CurrentDataContext.CurrentUserName;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string TOSSM_Version()
        {
            try
            {
                string tossmFile = Path.Combine(Common.CurrentDirFunc(), "TOSSM.exe"); ;
                //return CurrentDataContext.Version; // no, it is current application version return
                if (File.Exists(tossmFile))
                {
                    FileVersionInfo fi = FileVersionInfo.GetVersionInfo(tossmFile);
                    return fi.ProductVersion;
                }
                return "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
        public static List<Feature> GetFeaturesByED(FeatureType featType, Guid identifier = default(Guid))
        {
            try
            {
                // Checking, is value in the cache available
                if (Cache.Contains(featType))
                {
                    return (identifier == default(Guid)) ? Cache.Get(featType) : Cache.Get(featType).Where(x=>x.Identifier == identifier).ToList();
                }

                //bool isActive = IsAnyActiveSlot();
                if (DirectTOSSMConnection)
                {
                    /* WorkPackage values 
                     -1 - only published data
                     0 - unpublished data  (include active slot)
                     */
                    int WorkPackage = AIP.IsUnpublished ? 0 : -1;
                    var mask = new FeatureId() { FeatureTypeId = (int)featType, WorkPackage = WorkPackage };
                    if (identifier != default(Guid))
                        mask.Guid = identifier;
                    IList<AbstractState<AimFeature>> result = null;
                    var lst = new List<Feature>();
                    switch (AIP.Interpretation)
                    {
                        //case InterpretationTypes.Baseline:
                        //    result = temporalityService.GetActualDataByDate(mask, false, AIP.EffectiveDate, Interpretation.BaseLine);
                        //    break;
                        case InterpretationTypes.Snapshot:
                            result = temporalityService.GetActualDataByDate(mask, false, AIP.EffectiveDate, Interpretation.Snapshot);
                            break;
                        //case InterpretationTypes.BaselineUpdate:
                        //    IList<AbstractEvent<AimFeature>> res = temporalityService.GetChangesInInterval(mask, 
                        //        AIP.EffectiveDate, AIP.CancelDate, false);
                        //    foreach (var state in res)
                        //    {
                        //        lst.Add(state.Data.Feature);
                        //    }

                        //    var order2Lst = lst.OrderBy(x => x.Identifier).ToList();
                        //    break;
                        case InterpretationTypes.AllStatesInRange:
                            result = temporalityService.GetStatesInRangeByInterpretation(mask, false,
                                AIP.EffectiveDate, AIP.CancelDate, Interpretation.Snapshot);
                            //.Where(x=>x.Data.Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA)
                            //.ToList();
                            break;
                        //case InterpretationTypes.BaseLineStatesInRange:
                        //    result = temporalityService.GetStatesInRangeByInterpretation(mask, false,
                        //        AIP.EffectiveDate, AIP.CancelDate, Interpretation.Snapshot);
                        //    break;
                        case InterpretationTypes.TempDeltaStatesInRange:
                            result = temporalityService.GetStatesInRangeByInterpretation(mask, false,
                                AIP.EffectiveDate, AIP.CancelDate, Interpretation.Snapshot);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    foreach (var state in result)
                    {
                        lst.Add(state.Data.Feature);
                    }

                    var orderLst = lst.OrderBy(x => x.Identifier).ToList();
                    // Filter only required data before adding into cache, 
                    // to use filtered data in future
                    orderLst = FilterData(featType, orderLst);
                    Cache.Add(featType, orderLst);
                    return orderLst;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        public static List<Feature> GetFeaturesByED2(FeatureType featType, Guid identifier = default(Guid))
        {
            try
            {
                // Checking, is value in the cache available
                if (Cache.Contains(featType))
                {
                    return (identifier == default(Guid)) ? Cache.Get(featType) : Cache.Get(featType).Where(x => x.Identifier == identifier).ToList();
                }
                //bool isActive = IsAnyActiveSlot();
                if (DirectTOSSMConnection)
                {
                    /* WorkPackage values 
                     -1 - only published data
                     0 - unpublished data  (include active slot)
                     */
                    int WorkPackage = AIP.IsUnpublished ? 0 : -1;
                    var mask = new FeatureId() { FeatureTypeId = (int)featType, WorkPackage = WorkPackage };
                    if (identifier != default(Guid))
                        mask.Guid = identifier;
                    IList<AbstractState<AimFeature>> result = temporalityService.GetStatesInRangeByInterpretation(mask, false, DateTime.Now.AddDays(-600),
                        DateTime.Now.AddDays(600), Interpretation.Snapshot);
                    //var result = temporalityService.GetActualDataByDate(mask, false, AIP.EffectiveDate, AIP.Interpretation);
                    var lst = new List<Feature>();
                    foreach (var state in result)
                    {
                        if (state.Data.Feature.TimeSlice.Interpretation == TimeSliceInterpretationType.TEMPDELTA)
                        {
                            lst.Add(state.Data.Feature);
                        }
                        
                    }

                    var orderLst = lst.OrderBy(x => x.Identifier).ToList();
                    // Filter only required data before adding into cache, 
                    // to use filtered data in future
                    orderLst = FilterData(featType, orderLst);
                    Cache.Add(featType, orderLst);
                    return orderLst;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
        public static List<Feature> FilterData(FeatureType featType, List<Feature> featList)
        {
            try
            {
                // ToDo: Not Specified in the EC Specification. Added by Teodor request, to filter only EV Airspaces
                if (featType == FeatureType.Airspace && !string.IsNullOrEmpty(AIP.Country))
                {
                    return featList.Cast<Airspace>().Where(x => x.Designator?.StartsWith(AIP.Country) == true).Cast<Feature>().ToList();
                }
                // ToDo: Not Specified in the EC Specification. Added by Teodor/Didzis request, to filter only ICAO types DP
                else if (featType == FeatureType.DesignatedPoint)
                {
                    return featList.Cast<DesignatedPoint>().Where(x => x.Type == CodeDesignatedPoint.ICAO).Cast<Feature>().ToList();
                }
                else if (featType == FeatureType.SignificantPointInAirspace)
                {
                    // ToDo: Temporary fix to prevent collect SignificantPointInAirspace without Airspace or DesignatedPoint.
                    // Both are must be included
                    // Teodor request
                    List<Guid> airGuid = GetFeaturesByED(FeatureType.Airspace)
                        .Select(x => x.Identifier)
                        .ToList();
                    List<Guid> dpGuid = GetFeaturesByED(FeatureType.DesignatedPoint)
                        .Select(x => x.Identifier)
                        .ToList();
                    return featList.Cast<SignificantPointInAirspace>()
                        .Where(x => airGuid.Contains(x.ContainingAirspace.Identifier) &&
                                    dpGuid.Contains(x.Location.FixDesignatedPoint.Identifier)
                        ).Cast<Feature>().ToList();
                }
                return featList;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        //public static List<Feature> FilterData(FeatureType featType, List<Feature> featList)
        //{
        //    try
        //    {
        //        ClassFilter classFilter = AIP.FilterList.FirstOrDefault(x => x.FeatureType == featType);
        //        if (classFilter != null)
        //        {
        //            Type type = Type.GetType("Aran.Aim.Features." + classFilter.FeatureType + ",Aran.Aim");
        //            return ExpressionBuilder.Run(featList.OfType<Airspace>().ToList(), classFilter.Filter).OfType<Feature>().ToList();
        //        }
        //        return featList;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return null;
        //    }
        //}



        public static bool IsAnyActiveSlot()
        {
            try
            {
                if (DirectTOSSMConnection)
                {
                    var slots = CurrentDataContext.CurrentNoAixmDataService.GetPublicSlots();

                    foreach (PublicSlot publicSlot in slots)
                    {

                        foreach (PrivateSlot priv in CommonDataProvider.GetPrivateSlots(publicSlot.Id))
                        {
                            Console.WriteLine(publicSlot.Name);
                            Console.WriteLine("> " + priv.Name);
                        }
                    }

                    PrivateSlot act = CommonDataProvider.GetActivePrivateSlot();
                    if (act != null)
                    {
                        DialogResult dialogResult = MessageBox.Show(@"There is activated slot which isn`t yet published. Do you want to authomatically deactivate it while AIP Data Set is running?",
                            @"Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            CommonDataProvider.DeactivatePrivateSlot(act.Id);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static void DeactivatePrivateSlot()
        {
            try
            {
                ActivePrivateSlot = CommonDataProvider.GetActivePrivateSlot();
                AIP.ProjectName = AIP.SpaceName = "";
                if (ActivePrivateSlot != null)
                {
                    if (!AIP.IsUnpublished)
                    {
                        CommonDataProvider.DeactivatePrivateSlot(ActivePrivateSlot.Id);
                        isDeactivated = true;
                    }
                    else
                    {
                        AIP.ProjectName = ActivePrivateSlot?.PublicSlot?.Name;
                        AIP.SpaceName = ActivePrivateSlot?.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static string ActiveSlotMessage()
        {
            try
            {
                PrivateSlot ActivePrivateSlot = CommonDataProvider.GetActivePrivateSlot();
                string output = "";
                if (ActivePrivateSlot != null)
                {
                    output = $@"Project name: {ActivePrivateSlot?.PublicSlot?.Name}{Environment.NewLine}Active user space: {ActivePrivateSlot?.Name}{Environment.NewLine}";
                }
                else
                {
                    output = "No active user space available";
                }
                return output;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public static void ActivatePrivateSlot()
        {
            try
            {
                if (isDeactivated && ActivePrivateSlot != null)
                {
                    CommonDataProvider.ActivatePrivateSlot(ActivePrivateSlot.Id);
                    isDeactivated = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static void FillActiveSlotAIPData()
        {
            try
            {
                AIP.ProjectName = AIP.SpaceName = "";
                if (AIP.IsUnpublished)
                {
                    PrivateSlot activePrivateSlot = CommonDataProvider.GetActivePrivateSlot();
                    if (activePrivateSlot != null)
                    {
                        AIP.ProjectName = activePrivateSlot?.PublicSlot?.Name;
                        AIP.SpaceName = activePrivateSlot?.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        //public static List<Feature> GetFeaturesByED(FeatureType featType, Filter filter)
        //{
        //    try
        //    {
        //        lock (Globals.dbPro)
        //        {
        //            if (AIP.EffectiveDate == null)
        //            {
        //                ErrorLog.ShowMessage("No AIP selected");
        //                return null;
        //            }
        //            // Checking, is value in the cache available
        //            if (Cache.Contains(featType)) return Cache.Get(featType);

        //            var propList = new List<string>();
        //            propList.Add("<LOAD_ALL_FOR_EXPORT>");
        //            TimeSliceFilter tsf = new TimeSliceFilter(AIP.EffectiveDate);
        //            GettingResult result = dbPro.GetVersionsOf(
        //                featType,
        //                Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE,
        //                Guid.Empty,
        //                true,
        //                tsf,
        //                null,
        //                filter);

        //            if (result.IsSucceed)
        //            {
        //                List<Feature> lst = result.GetListAs<Feature>();
        //                Cache.Add(featType, lst);
        //                return lst;
        //            }
        //            else
        //                throw new Exception(result.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return null;
        //    }
        //}

    }
}