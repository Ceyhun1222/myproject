using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using Aran.Aim;
using Aran.Aim.AixmMessage;
using Aran.Aim.Features;
using Aran.Aim.Utilities;
using Aran.PANDA.Common;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;

namespace AIP.DataSet
{
    class Common
    {
        private static StringWriterWithEncoding stringBuilder;
        private static XmlWriter writer;
        internal static List<Feature> DataSet = new List<Feature>();
        internal static Queue<Output> OutputQueue = new Queue<Output>();
        internal static ErrorLevel Errors;
        internal static Dictionary<ErrorLevel, int> ErrorNumber = new Dictionary<ErrorLevel, int>();
        internal static Dictionary<FeatureType, List<FeatureType>> FeatureRefTo = new Dictionary<FeatureType, List<FeatureType>>();
        internal static Dictionary<FeatureType, List<FeatureType>> FeatureRefMe = new Dictionary<FeatureType, List<FeatureType>>();
        private static FeatureType? oldType = null;
        private static FeatureType? curType = null;
        private static int cnt = 0;
        private static int totalCnt = 0;

        public struct Output
        {
            internal string Message;
            internal Color? Color;
            internal FontStyle? Style;
        }

        [Flags]
        public enum ErrorLevel
        {
            Unknown = 0x0,
            Success = 0x1,
            SuppressErrors = 0x2,
            RefLink = 0x4,
            SubLink = 0x8,
        }

        static Common()
        {
            try
            {
                FillFeatureRelation();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public static List<T> LoadFeatures<T>() where T : Feature, new()
        {
            try
            {
                return XmlFileConnection.GetFeaturesByED(new T().FeatureType).OfType<T>().ToList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static DataTable LoadDataTableFeatures<T>() where T : Feature, new()
        {
            try
            {
                List<T> data = XmlFileConnection.GetFeaturesByED(new T().FeatureType).OfType<T>().ToList();

                Type type = typeof(T);
                var properties = type.GetProperties();

                DataTable dataTable = new DataTable();
                foreach (PropertyInfo info in properties)
                {
                    dataTable.Columns.Add(new DataColumn(info.Name, typeof(String)));
                }

                foreach (T entity in data)
                {
                    object[] values = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        // If List, just count the length of List
                        if (properties[i].PropertyType.Name.Contains("List`1"))
                            values[i] = "Count: "+(properties[i].GetValue(entity) as IEnumerable<object>)?.Count();
                        else if (properties[i]?.GetValue(entity)?.ToString().Contains("Aran.Aim.") != false)
                            values[i] = properties[i].GetValue(entity) == null ? "" : 
                                properties[i].GetValue(entity).ToString()
                                .Replace("Aran.Aim.","")
                                ;
                        else
                            values[i] = properties[i].GetValue(entity);
                    }

                    dataTable.Rows.Add(values);
                }
                return dataTable;
                
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
        


        private static void FillFeatureRelation()
        {
            try
            {
                var xlist = Enum.GetValues(typeof(FeatureType));

                foreach (FeatureType ftype in xlist.OfType<FeatureType>().OrderBy(x => x.ToString()))
                {
                    AimClassInfo ci = AimMetadata.GetClassInfoByIndex((int)ftype);
                    List<FeatureType> pi = ci.Properties.Where(x => x.IsFeatureReference && x.ReferenceFeature != 0)?.Select(x => x.ReferenceFeature).ToList();
                    FeatureRefTo.Add(ftype, pi);
                    FeatureRefMe.Add(ftype, new List<FeatureType>());
                }

                foreach (KeyValuePair<FeatureType, List<FeatureType>> toMe in FeatureRefTo)
                {
                    foreach (FeatureType ftype in toMe.Value)
                    {
                        FeatureRefMe[ftype].Add(toMe.Key);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void OpenXmlWriter()
        {
            writer = CreateXmlWriter(out stringBuilder);
        }

        private static void WriteFeature(Feature feature)
        {
            try
            {
                Dictionary<FeatureType, List<string>> IgnoredProperties = new Dictionary<FeatureType, List<string>>();
                IgnoredProperties.Add(
                    FeatureType.AirTrafficControlService,
                    new List<string>()
                    {
                        "ClientAirport", "ClientProcedure", "ClientAerialRefuelling", "AircraftLocator"
                    });
                IgnoredProperties.Add(
                    FeatureType.InformationService,
                    new List<string>()
                    {
                        "ClientAirport", "ClientProcedure", "ClientAerialRefuelling"
                    });

                writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                {
                    var afl = new AixmFeatureList { feature };
                    afl.WriteDataSetXml(writer, IgnoredProperties);
                }
                writer.WriteEndElement();
                //if (feature.FeatureType == FeatureType.Airspace)
                //{
                //    AddOutput($@"Feature {feature.FeatureType} {((Airspace)feature).Name} has been added. ", Color.Blue);
                //}
            }
            catch (Exception ex)
            {
                //   ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void WriteFeature(List<Feature> features)
        {
            try
            {
                Dictionary<FeatureType, List<string>> IgnoredProperties = new Dictionary<FeatureType, List<string>>();
                IgnoredProperties.Add(
                    FeatureType.AirTrafficControlService,
                    new List<string>()
                    {
                        "ClientAirport", "ClientProcedure", "ClientAerialRefuelling", "AircraftLocator"
                    });
                IgnoredProperties.Add(
                    FeatureType.InformationService,
                    new List<string>()
                    {
                        "ClientAirport", "ClientProcedure", "ClientAerialRefuelling"
                    });

                writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                {
                    var afl = new AixmFeatureList();
                    afl.AddRange(features);
                    afl.WriteDataSetXml(writer, IgnoredProperties);
                }
                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                //   ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        //private static void WriteFeatureWithReference(Feature feature, SortedSet<Guid> processedIds)
        //{
        //    try
        //    {
        //        //add feature itself
        //        WriteFeature(feature);

        //        //get links
        //        var links = new List<RefFeatureProp>();
        //        AimMetadataUtility.GetReferencesFeatures(feature, links);


        //        //add links
        //        foreach (var link in links)
        //        {
        //            var id = link.RefIdentifier;
        //            if (!processedIds.Add(id)) continue;

        //            //var linkedFeatures = CurrentDataContext.CurrentService.GetActualDataByDate(new FeatureId { FeatureTypeId = (int)link.FeatureType, Guid = id }, false, AIP.EffectiveDate, AIP.Interpretation);
        //            //if (linkedFeatures != null && linkedFeatures.Count > 0)
        //            //{
        //            //    WriteFeatureWithReference(linkedFeatures.First().Data.Feature, processedIds);
        //            //}
        //            var linkedFeatures = Globals.GetFeaturesByED(feature.FeatureType, id);
        //            if (linkedFeatures != null && linkedFeatures.Count > 0)
        //            {
        //                WriteFeatureWithReference(linkedFeatures.First(), processedIds);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }
        //}



        public static XmlWriter CreateXmlWriter(out StringWriterWithEncoding stringBuilder)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                stringBuilder = new StringWriterWithEncoding(Encoding.UTF8);

                //stringBuilder = new StringBuilder();
                //var writer = XmlWriter.Create(fileName, settings);
                var writer = XmlWriter.Create(stringBuilder, settings);

                writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "AIXMBasicMessage");
                writer.WriteAttributeString(AimDbNamespaces.XSI, "schemaLocation",
                    "http://www.aixm.aero/schema/5.1/message http://www.aixm.aero/schema/5.1/message/AIXM_BasicMessage.xsd");
                writer.WriteAttributeString(AimDbNamespaces.GML, "id", CommonXmlWriter.GenerateNewGmlId());
                writer.WriteAttributeString("xmlns", AimDbNamespaces.AIXM51.Prefix, null, AimDbNamespaces.AIXM51.Namespace);
                if (false) //TODO:add metadata
                {
                    writer.WriteStartElement(AimDbNamespaces.AIXM51, "messageMetadata");
                    {
                    }
                    writer.WriteEndElement();
                }
                return writer;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                stringBuilder = new StringWriterWithEncoding();
                return null;
            }
        }

        public static void CloseXmlWriter()
        {
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        private static IList<AbstractState<AimFeature>> RemoveDecomissioned(IList<AbstractState<AimFeature>> data, DateTime endDateTime)
        {
            try
            {
                var errorData = data.Where(f => f.Data.Feature.TimeSlice.FeatureLifetime == null);
                var errorList = errorData as IList<AbstractState<AimFeature>> ?? errorData.ToList();
                //if (errorList.IsNotEmpty())
                //{
                //    _featureErrors.AddRange(errorList.Select(t => new FeatureError
                //    {
                //        Message = "Feature has a null feature life time.",
                //        Type = ErrorType.NullReference,
                //        FeatureType = t.Data.FeatureType,
                //        Data = { t.Data.Feature }
                //    }));
                //}


                data =
                    data.Where(
                        f => f.Data.Feature.TimeSlice.FeatureLifetime?.EndPosition == null || f.Data.Feature.TimeSlice.FeatureLifetime.EndPosition.Value > endDateTime).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
        
        public static void CollectAipDataSet(List<Feature> MainFeatures, List<Feature> SubFeatures)
        {
            try
            {
                // Recursively getting reference features
                CollectFeatureWithReference(MainFeatures);



                // Collecting SubFeatures
                CollectFeatureWithReference(SubFeatures);

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }

        private static void CollectFeatureWithReference(List<Feature> Features)
        {
            try
            {
                if (Features == null || Features.Count == 0) return;
                //add feature itself
                foreach (Feature feature in Features)
                {
#if DEBUG
                    //if (feature.Identifier.ToString().Contains("469b1f40-ac45-4f1d-9f6f-e786deee103a"))
                    //    System.Diagnostics.Debugger.Break();
#endif
                    if(DataSet.Any(x => 
                    x.Identifier == feature.Identifier && 
                    x.TimeSlice.SequenceNumber == feature.TimeSlice.SequenceNumber && 
                    x.TimeSlice.CorrectionNumber == feature.TimeSlice.CorrectionNumber &&
                    x.TimeSlice.Interpretation == feature.TimeSlice.Interpretation
                    ) == false)
                        DataSet.Add(feature);

                    // Getting reference links
                    var links = new List<RefFeatureProp>();
                    AimMetadataUtility.GetReferencesFeatures(feature, links);
                    if (links?.Count > 0)
                    {
                        // temp inject begin
                        //if (feature is AirTrafficControlService)
                        //{
                        //    if (true) // current is airspace
                        //    {
                        //        links.RemoveAll(x => x.PropInfoList != null && 
                        //        x.PropInfoList.FirstOrDefault() != null &&
                        //        (x.PropInfoList.FirstOrDefault().Name.Contains("ClientAirport")
                        //        || x.PropInfoList.FirstOrDefault().Name.Contains("ClientAirspace")
                        //        ));
                        //    }
                        //}
                        // temp inject end

                        List<Feature> feat = GetFeatureByRef(feature, links);
                        if (feat != null)
                        {
                            CollectFeatureWithReference(feat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void CollectSubFeatureWithReference(List<Feature> Features)
        {
            try
            {
                //add feature itself
                foreach (Feature feature in Features)
                {
                    DataSet.Add(feature);
                    //get links
                    var links = new List<RefFeatureProp>();
                    AimMetadataUtility.GetReferencesFeatures(feature, links);
                    if (links?.Count > 0)
                    {

                        List<Feature> feat = GetFeatureByRef(feature, links);
                        if (feat != null)
                        {
                            CollectFeatureWithReference(feat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static List<Feature> GetFeatureByRef(Feature ParentFeature, List<RefFeatureProp> links)
        {
            try
            {
                List<Feature> refFeat = new List<Feature>();
                foreach (RefFeatureProp link in links)
                {
                    Feature ft = Cache.GetById(link.FeatureType, link.RefIdentifier);
                    if (ft != null) refFeat.Add(ft);
                    else if (!Errors.HasFlag(ErrorLevel.SuppressErrors))
                    {
                        Errors |= ErrorLevel.RefLink;

                        string linkId = "";
                        if (Properties.Settings.Default.UseLinks)
                        {
                            linkId = $@", file:///{ParentFeature.FeatureType}/{ParentFeature.Identifier}";
                        }

                        AddErrorNumber(ErrorLevel.RefLink);
                        AddOutput($@"Can`t find reference feature for {ParentFeature.FeatureType} with Guid: {ParentFeature.Identifier}{linkId}. {Environment.NewLine}Reference feature is {link.FeatureType.ToString()} with Guid: {link.RefIdentifier}{Environment.NewLine}",
                            Color.Red);
                    }
                }
                return refFeat;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static void AddErrorNumber(ErrorLevel error)
        {
            try
            {
                if (ErrorNumber.ContainsKey(error)) ErrorNumber[error]++;
                else ErrorNumber.Add(error, 1);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static SortedSet<Guid> GetGuidList(List<Feature> ft)
        {
            try
            {
                List<Guid> guidList = ft.Select(x => x.Identifier).ToList();
                return new SortedSet<Guid>(guidList);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private static void AddOutput(string Action, Color? color = null, FontStyle? style = null)
        {
            try
            {
                if (color == null) color = Color.Black;
                OutputQueue.Enqueue(new Output() { Message = Action, Color = color, Style = style });
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        

        public static void WriteAipDataSet()
        {
            try
            {
                AddOutput($@"{Environment.NewLine}3. Writing Data and Creating MD5 Hash{Environment.NewLine}", Color.Black, FontStyle.Bold);

                OpenXmlWriter();
                var groupedFeatures = DataSet.GroupBy(x => x.Identifier).ToList();
                foreach (var feat in groupedFeatures)
                {
                    var featList = feat.Select(x => x).ToList();
                    WriteFeature(featList);
                }
                CloseXmlWriter();

                string xmlString = stringBuilder.ToString();
                string crcText = CRC32.CalcCRC32(xmlString);
                string PublishInfo = "";
                string DateInfo = "";
                if (AIP.IsUnpublished)
                {
                    PublishInfo = $"<!-- Attention: Contains Unpublished Data -->\n";
                    if (!string.IsNullOrEmpty(AIP.ProjectName)) PublishInfo += $"<!-- Project name: {AIP.ProjectName} -->\n";
                    if (!string.IsNullOrEmpty(AIP.SpaceName)) PublishInfo += $"<!-- Space name: {AIP.SpaceName} -->\n";
                }
                if ((int)AIP.Interpretation >= (int)InterpretationTypes.AllStatesInRange)
                {
                    DateInfo = $"<!-- Date/Time from (UTC): {AIP.EffectiveDate.ToLongFormatString()} -->\n";
                    DateInfo += $"<!-- Date/Time to (UTC): {AIP.CancelDate.ToLongFormatString()} -->\n";
                    DateInfo += $"<!-- CRC32: {crcText} -->\n";
                }
                else
                {
                    DateInfo = (AIP.IsAIRAC ? $"<!-- Effective Date: {AIP.EffectiveDate.ToShortFormatString()}" : $"<!-- Effective Date/Time (UTC): {AIP.EffectiveDate.ToLongFormatString()}") + $" CRC32: {crcText} -->\n";
                }
                string comment =
                    $"<!-- {TitleVersion()} -->\n" +
                    $"<!-- AIXM 5.1 AIP DataSet for {AIP.Country} -->\n" +
                    $"<!-- AIP Data Set Type: {AIP.Interpretation.GetEnumDescription()} -->\n" +
                    DateInfo +
                    PublishInfo;
                int index = xmlString.IndexOf("<aixm-message-5.1:AIXMBasicMessage",
                    StringComparison.Ordinal);
                xmlString = xmlString.Insert(index, comment);
                //string airac = AIP.IsAIRAC ? "AIRAC_" : "";

                //string fileName = $@"{AIP.Country}_AIP_DataSet_{airac}{AIP.EffectiveDate:yyyy-MM-dd}.xml";


                //AIP.DataSetFile = fileName;
                System.IO.StreamWriter file = new System.IO.StreamWriter(AIP.DataSetFileFullPath);
                file.WriteLine(xmlString);
                file.Close();
                AddOutput($@"Data has been successfully written into file: {AIP.DataSetFile}", Color.DarkGreen, FontStyle.Bold);
                CreateMD5File(AIP.DataSetFileFullPath);
                AddOutput($@"MD5 Hash file has been successfully created: {AIP.DataSetFile}.md5", Color.DarkGreen, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        
        public static string TitleVersion(bool shortVersion = false)
        {
            try
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                string server = Globals.Server();
                string user = Globals.User();
                string db = Globals.DB();
                string tossmVersion = Globals.TOSSM_Version();

                return shortVersion ? 
                    $@"AIXM 5.1 driven AIP Data Set v. {v.Major}.{v.Minor}.{v.Build}, Server: {server}" : 
                    $@"AIXM 5.1 driven AIP Data Set v. {v.Major}.{v.Minor}.{v.Build}, TOSSM v.: {tossmVersion}, Server: {server}, DB: {db}, User: {user}";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return String.Empty;
            }
        }
        public static void CreateZipFile()
        {
            try
            {
                Application.DoEvents();
                string startPath = AIP.DataSetFolder;
                string zipPath = Path.Combine(AIP.DataSetFolder, "../", $@"{AIP.DataSetFile}.zip");
                ZipFile.CreateFromDirectory(startPath, zipPath);
                File.Move(zipPath, $@"{AIP.DataSetFileFullPath}.zip");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        internal static void CreateMD5File(string FilePath)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string hash = GetMD5HashFromFile(FilePath);
                    File.WriteAllText($@"{FilePath}.md5", $@"{hash} *{Path.GetFileName(FilePath)}");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static string GetMD5HashFromFile(string filePath)
        {
            try
            {
                using (var stream = new BufferedStream(File.OpenRead(filePath), 32768))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] checksum = md5.ComputeHash(stream);
                    return BitConverter.ToString(checksum).Replace("-", String.Empty);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static string CurrentDirFunc()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendColorText(this RichTextBox box, string text, Color? color, FontStyle? style)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            if (color != null) box.SelectionColor = (Color)color;
            if (style != null) box.SelectionFont = new Font(box.Font, (FontStyle)style);
            box.AppendText(text);
            if (color != null) box.SelectionColor = box.ForeColor;
            if (style != null) box.SelectionFont = new Font(box.Font, box.Font.Style);
        }
    }
}
