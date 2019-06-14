using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using Aran.Aim.AixmMessage;

namespace AIP.DataSet
{
    internal static class XmlFileConnection
    {
        public static ConnectionType? connType = null;
        public static DbProvider dbPro;
        public static Connection conn;
        public static string fileName;
        public static Dictionary<string, int> FeaturesCount;

        //static XmlFileConnection()
        //{
        //    try
        //    {
        //        //InitializeConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }

        //}

        public static void Init()
        {
            try
            {
                if (fileName == null || !File.Exists(fileName))
                {
                    ErrorLog.ShowException($@"No such file available: {fileName}");
                    return;
                }
                else
                {
                    CollectFeaturesCount();
                    if (!FeaturesCount.Any())
                    {
                        ErrorLog.ShowWarning($@"No any feature available in the file {fileName}");
                        return;
                    }
                    conn = new Connection
                    {
                        ConnectionType = ConnectionType.XmlFile,
                        XmlFileName = fileName
                    };
                }


                dbPro = DbProviderFactory.Create("Aran.Aim.Data.XmlProvider");
                dbPro.Open(conn.GetConnectionString());
                if (!dbPro.Login(conn.UserName, DbUtility.GetMd5Hash(conn.Password)))
                {
                    dbPro.Close();
                    throw new Exception("Invalid User Name or Password!");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }



        private static void CollectFeaturesCount()
        {
            
            try
            {
                // Long time for Latvia 13.5 sec
                // TODO: rewrite for better results
                //FeatureTypeList = new SortedList<Guid, FeatureType>();
                //using (var xmlReader = XmlReader.Create(fileName))
                //{
                //    var aixmBasicMess1 = new AixmBasicMessage(MessageReceiverType.Panda);
                //    aixmBasicMess1.ReadXmlAndNotify(xmlReader, (afl, collection) =>
                //    {
                //        foreach (var feature in afl)
                //        {
                //            if (!FeatureTypeList.ContainsKey(feature.Identifier))
                //            {
                //                FeatureTypeList.Add(feature.Identifier, feature.FeatureType);
                //            }
                //        }
                //        collection.Clear(); //do not keep in memory 
                //    });
                //}

                // rewritten 1.5 sec for Latvia
                XNamespace aixmMessageNamespace = @"http://www.aixm.aero/schema/5.1/message";
                var xDoc = XDocument.Load(fileName);
                if (xDoc.Root != null)
                {
                    FeaturesCount = xDoc.Root
                        .Elements(aixmMessageNamespace + "hasMember")
                        .Elements()
                        .GroupBy(x=>x.Name.LocalName)
                        .OrderBy(x => x.Key)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                
            }
            catch (Exception ex)
            {
                //ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        

        private static void CompactLoh()
        {
            try
            {
                var piLohcm = typeof(GCSettings).GetProperty("LargeObjectHeapCompactionMode", BindingFlags.Static | BindingFlags.Public);

                if (null != piLohcm)
                {
                    var miSetter = piLohcm.GetSetMethod();
                    miSetter.Invoke(null, new object[] {/* GCLargeObjectHeapCompactionMode.CompactOnce */ 2 });
                }
                GC.Collect(); // This will cause the LOH to be compacted (once).
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static List<Feature> GetFeaturesByED(FeatureType featType, Guid identifier = default(Guid))
        {
            try
            {
                // Checking, is value in the cache available
                if (Cache.Contains(featType)) return Cache.Get(featType);

                lock (XmlFileConnection.dbPro)
                {

                    var propList = new List<string>();
                    propList.Add("<LOAD_ALL_FOR_EXPORT>");
                    TimeSliceFilter tsf = new TimeSliceFilter(AIP.EffectiveDate);
                    GettingResult result = dbPro.GetVersionsOf(
                                featType,
                                TimeSliceInterpretationType.BASELINE,
                                identifier,
                                true,
                                tsf,
                                propList,
                                null);

                    if (result.IsSucceed)
                    {
                        List<Feature> lst = result.GetListAs<Feature>();
                        var orderLst = lst.OrderBy(x => x.Identifier).ToList();
                        Cache.Add(featType, orderLst);
                        return orderLst;
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

    }
}
