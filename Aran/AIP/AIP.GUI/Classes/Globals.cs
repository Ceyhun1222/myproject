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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AIP.BaseLib.Class;
using AIP.DB;
using AIP.GUI.Classes;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.CommonUtil.Util;
using AirportHeliport = Aran.Aim.Features.AirportHeliport;
using Application = System.Windows.Forms.Application;

namespace AIP.GUI
{
    internal static class Globals
    {
        internal static ConnectionType connType;
        internal static DbProvider dbPro;
        internal static Connection conn;
        // internal static string ConnectionString;
        internal static bool IsAMDTPreview;
        internal static ITemporalityService<AimFeature> temporalityService;
        internal static DB.User CurrentUser;


        // TODO: change to false
        internal static bool DirectTOSSMConnection = true;

        static Globals()
        {
            try
            {
                conn = new Connection();
                connType = Properties.Settings.Default.ConnectionType;
                //ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

                if (DirectTOSSMConnection)
                {
                    //CommonDataProvider.CommitAsCorrection();
                    dbPro = DbProviderFactory.Create("Aran.Temporality.Provider");
                    var connectionStrings = dbPro.GetConnectionStrings();

                    try
                    {
                        Program.HideSplash();
                        dbPro.Open(connectionStrings.FirstOrDefault());
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                    }
                    finally
                    {
                        Program.ShowSplash();
                    }
                    if (CurrentDataContext.CurrentUser == null)
                    {
                        Program.HideSplash();
                        ErrorLog.ShowWarning($@"Login into TOSS service was unsuccessful. Please check connection to server, login or password and try again!");
                        Application.Exit();
                        return;
                    }
                    else
                    {
                        TossUser.Init();

                    }
                    //var time = CurrentDataContext.CurrentService.GetServerTime();

                    if (!dbPro.Login(conn.UserName, DbUtility.GetMd5Hash(conn.Password)))
                    {
                        dbPro.Close();
                        throw new Exception("Invalid User Name or Password!");
                    }
                    if (Lib.CurrentAIP != null) dbPro.DefaultEffectiveDate = Lib.CurrentAIP.Effectivedate;
                    temporalityService = CurrentDataContext.CurrentService;
                    if (CurrentDataContext.CurrentUser?.Id > 0 && CurrentDataContext.CurrentUser?.Name != "")
                    {
                        using (eAIPContext db = new eAIPContext())
                        {
                            var dbUser = db.User.FirstOrDefault(x => x.TossId == CurrentDataContext.CurrentUser.Id);
                            if (dbUser.IsNull())
                            {
                                var user = new DB.User
                                {
                                    TossId = CurrentDataContext.CurrentUser.Id,
                                    Name = CurrentDataContext.CurrentUser.Name
                                };
                                db.User.Add(user);
                                db.SaveChanges();
                                CurrentUser = user;
                            }
                            else
                            {
                                CurrentUser = dbUser;
                            }
                        }
                    }
                    else
                    {
                        //ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                        Application.Exit();
                    }
                    //time = CurrentDataContext.CurrentService.GetServerTime();
                    return;
                }

                if (connType == ConnectionType.TDB)
                {
                    dbPro = DbProviderFactory.Create("Aran.Temporality.Provider");
                    dbPro.Open(null);
                    try
                    {
                        //TODO: Detect why TOSSM return error in first connection
                        // This is test only
                        var tossmProblem = dbPro.GetAllFeatuers(FeatureType.AirportHeliport);
                        List<AirportHeliport> ft = tossmProblem.OfType<AirportHeliport>().ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error when first connect");
                    }

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
                    throw new Exception("Not implemented");
                    //dbPro = DbProviderFactory.Create("Aran.Aim.Data.XmlProvider");
                }

                dbPro.Open(conn.GetConnectionString());

                if (!dbPro.Login(conn.UserName, DbUtility.GetMd5Hash(conn.Password)))
                {
                    dbPro.Close();
                    throw new Exception("Invalid User Name or Password!");
                }
                dbPro.DefaultEffectiveDate = Lib.CurrentAIP.Effectivedate;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);

            }

        }

        public static DateTime? GetServerDate()
        {
            try
            {
                return CurrentDataContext.CurrentService?.GetServerTime();
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
                if (Lib.CurrentAIP == null)
                {
                    ErrorLog.ShowMessage("No AIP selected");
                    return null;
                }
                // Checking, is value in the cache available
                if (Cache.Contains(featType) && identifier == default(Guid)) return Cache.Get(featType);

                if (DirectTOSSMConnection)
                {
                    int WorkPackage = Properties.Settings.Default.PendingData ? 0 : -1;
                    var mask = new FeatureId() { FeatureTypeId = (int)featType, WorkPackage = WorkPackage };
                    if (identifier != default(Guid))
                        mask.Guid = identifier;

                    //var result = temporalityService.GetActualDataByDate(mask, false, Lib.CurrentAIP.Effectivedate);
                    //var lst = new List<Feature>();
                    //foreach (var state in result)
                    //{
                    //    lst.Add(state.Data.Feature);
                    //}


                    List<Feature> lst = temporalityService
                        .GetActualDataByDate(mask, false, Lib.CurrentAIP.Effectivedate, Interpretation.BaseLine)
                        .Select(x => x.Data.Feature)
                        .OrderBy(x => x.Identifier)
                        .ToList();

                    if (identifier == default(Guid)) Cache.Add(featType, lst);
                    return lst;
                }
                else
                {
                    return null;
                }
                //// test 2
                //lock (Globals.dbPro)
                //{

                //    var propList = new List<string>();
                //    propList.Add("<LOAD_ALL_FOR_EXPORT>");
                //    TimeSliceFilter tsf = new TimeSliceFilter(Lib.CurrentAIP.Effectivedate);
                //    GettingResult result = dbPro.GetVersionsOf(
                //                featType,
                //                TimeSliceInterpretationType.BASELINE,
                //                identifier,
                //                true,
                //                tsf,
                //                propList,
                //                null);

                //    if (result.IsSucceed)
                //    {
                //        List<Feature> lst = result.GetListAs<Feature>()
                //            .OrderBy(x => x.Identifier)
                //            .ToList();
                //        Cache.Add(featType, lst);
                //        return lst;
                //    }
                //    else
                //        throw new Exception(result.Message);
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }


        public static List<Feature> GetTempFeaturesByED(FeatureType featType, Guid identifier = default(Guid))
        {
            try
            {
                if (Lib.CurrentAIP == null)
                {
                    ErrorLog.ShowMessage("No AIP selected or previous AIP is not available");
                    return null;
                }

                if (DirectTOSSMConnection)
                {
                    int WorkPackage = Properties.Settings.Default.PendingData ? 0 : -1;
                    var mask = new FeatureId() { FeatureTypeId = (int)featType, WorkPackage = WorkPackage };
                    if (identifier != default(Guid))
                        mask.Guid = identifier;


                    //List<Feature> lst = temporalityService
                    //    .GetChangesInInterval(mask, Lib.PrevousAIP.Effectivedate, Lib.CurrentAIP.Effectivedate,  false)
                    //    .Select(x => x.Data.Feature)
                    //    .OrderBy(x => x.Identifier)
                    //    .ToList();

                    //List<Feature> lst = temporalityService
                    //    .GetActualDataByDate(mask, false, Lib.CurrentAIP.Effectivedate, Interpretation.TempDelta)
                    //    .Select(x => x.Data.Feature)
                    //    .OrderBy(x => x.Identifier)
                    //    .ToList();

                    List<Feature> lst = temporalityService
                        .GetEventsByDate(mask, false, Lib.CurrentAIP.Effectivedate, Lib.CurrentAIP.Effectivedate.AddYears(5), Interpretation.TempDelta)
                        .Select(x => x.Data.Feature)
                        .OrderBy(x => x.Identifier)
                        .ToList();

                    //Cache.Add(featType, lst);
                    return lst;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="featType"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Feature> GetFeaturesByED(FeatureType featType, Filter filter)
        {
            try
            {
                lock (Globals.dbPro)
                {
                    if (Lib.CurrentAIP == null)
                    {
                        ErrorLog.ShowMessage("No AIP selected");
                        return null;
                    }
                    // Checking, is value in the cache available
                    if (Cache.Contains(featType)) return Cache.Get(featType);

                    var propList = new List<string>();
                    propList.Add("<LOAD_ALL_FOR_EXPORT>");
                    TimeSliceFilter tsf = new TimeSliceFilter(Lib.CurrentAIP.Effectivedate);
                    GettingResult result = dbPro.GetVersionsOf(
                        featType,
                        Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE,
                        Guid.Empty,
                        true,
                        tsf,
                        null,
                        filter);

                    if (result.IsSucceed)
                    {
                        List<Feature> lst = result.GetListAs<Feature>();
                        Cache.Add(featType, lst);
                        return lst;
                    }
                    else
                        throw new Exception(result.Message);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static bool IsConnected()
        {
            try
            {
                CurrentDataContext.CurrentService.GetServerTime();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }

}
