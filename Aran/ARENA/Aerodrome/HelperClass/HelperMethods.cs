
using Aerodrome.DB;
using Aerodrome.Enums;
using Aerodrome.Metadata;
using Aerodrome.Features;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;

using Framework.Stasy.Context;
using Framework.Stasy.Core;
using Framework.Stasy.Helper;
using Framework.Stasy.SyncProvider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using HelperDialog;
using System.Windows.Interop;

namespace Aerodrome.Metadata
{
    public static class HelperMethods
    {
        public static int GetObjectSize(System.Object obj)
        {
           
            try
            {
                byte[] bytes = SetObjectToBlob(obj, "");

                return bytes.Length;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                //System.Diagnostics.Debug.WriteLine(ex.Data);
                return -1;
            }
          
        }
    
        public static string GetMainFolder()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RISK\AMDB";
            if (!System.IO.Directory.Exists(appData)) System.IO.Directory.CreateDirectory(appData);
            return appData;

        }

        public static string GetProgrammFolder()
        {
            // возвращает путь к папке, где расположена активная Dll
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string _path = System.IO.Path.GetDirectoryName(path);
            return System.IO.Path.GetDirectoryName(_path);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static string GetAerodromeVersion()
        {
            string pF = GetProgrammFolder();

            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(pF + @"\AerodromeToolBox.dll");
            return myFileVersionInfo.FileVersion;
        }

        public static string GetPathToTemplateFile()
        {
            if (!IniSuccess()) CreateAerodromeIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\AMDBSettings.ini");
            return ArenaIni.Read("AmdbFile", "AERODROME");


        }

        public static string GetTargetDB()
        {
            if (!IniSuccess()) CreateAerodromeIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\AMDBSettings.ini");
            return ArenaIni.Read("TargetDB", "AERODROME") + @"\AMDB.mdb";

        }

        public static void SetTargetDB(string targetDBpath)
        {
            if (!IniSuccess()) CreateAerodromeIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\AMDBSettings.ini");
            ArenaIni.Write("TargetDB", targetDBpath, "AERODROME");

            return;

        }

        public static bool CreateAerodromeIniFile()
        {
            try
            {
                var ArenaIni = new IniFile(GetMainFolder());
                ArenaIni.CreateAerodromeIni("1.0");


                //string modelFolder = GetProgrammFolder() + @"\Model";
                //string new_modelFolder = GetMainFolder() + @"\Model";

                //if (!System.IO.Directory.Exists(new_modelFolder))
                //{
                //    DirectoryCopy(modelFolder, new_modelFolder, true);

                //    return true;

                //}
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

        }

        private static bool IniSuccess()
        {
            bool res = File.Exists(GetMainFolder() + @"\AMDBSettings.ini");
            if (res)
            {
                var ArenaIni = new IniFile(GetMainFolder() + @"\AMDBSettings.ini");

                //if (ArenaIni.Read("version", "AERODROME").CompareTo(GetAerodromeVersion()) != 0) return false;


                string tarDB = ArenaIni.Read("AmdbFile", "AERODROME");

                tarDB = System.IO.Path.Combine(tarDB, "AMDB.mdb");

                res = File.Exists(tarDB);
            }
            return res;
        }


        public static void SaveRecentFileName(string Filename)
        {
            if (!File.Exists(GetMainFolder() + @"\AMDBSettings.ini")) CreateAerodromeIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\AMDBSettings.ini");

            string recentList = ArenaIni.Read("RecentFiles", "AERODROME");
            string[] lst = recentList.Split('?');

            List<string> newLst = new List<string>();
            newLst.Add(Filename);

            for (int i = 0; i <= lst.Length - 1; i++)
            {
                if (lst[i].Length < 3) continue;

                if (!System.IO.File.Exists(lst[i])) continue;

                if (newLst.IndexOf(lst[i]) < 0) newLst.Add(lst[i]);

                if (i > 10) break;
            }

            recentList = "";
            foreach (var item in newLst)
            {
                recentList = recentList + item + "?";
            }
            //recentList = recentList.Length > 0 ? recentList + "?" + Filename : Filename;

            ArenaIni.Write("RecentFiles", recentList, "AERODROME");

        }

        public static string[] GetRecentFilesAmdm()
        {
            if (!File.Exists(GetMainFolder() + @"\AMDBSettings.ini")) CreateAerodromeIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\AMDBSettings.ini");

            string recentList = ArenaIni.Read("RecentFiles", "AERODROME");
            if (recentList.EndsWith("?")) recentList = recentList.TrimEnd('?');

            string[] recentFilePaths = recentList.Split('?');



            return recentFilePaths;
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

            using (System.IO.FileStream outFile = new System.IO.FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                outFile.Write(bytes, 0, iFileLen);

            return true;
        }

        public static bool DecompressToDirectory(string sCompressedFile, string sDir)
        {
            bool res = true;
            try
            {
                using (System.IO.FileStream inFile = new System.IO.FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
                using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                    while (DecompressFile(sDir, zipStream)) ;


            }
            catch { res = false; }

            return res;
        }

        public static void DeleteFilesFromDirectory(string DirName)
        {
            string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(DirName), "*.*");
            foreach (var fl in FN)
            {
                File.Delete(DirName + @"\" + System.IO.Path.GetFileName(fl));
            }

        }

        public static void CompressDirectory(string sInDir, string sOutFile, string[] userFileExtensions = null)
        {

            string[] fileExtensions = { ".mxd", ".mdb", ".amdb" };
            if (userFileExtensions != null)
            {
                int n = userFileExtensions.Length;
                if (n > 0)
                {
                    System.Array.Resize<string>(ref fileExtensions, n + 3);
                    System.Array.Copy(userFileExtensions, 0, fileExtensions, 3, n);
                }
            }

            string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
            int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (System.IO.FileStream outFile = new System.IO.FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
                foreach (string sFilePath in sFiles)
                {
                    string sRelativePath = sFilePath.Substring(iDirLen);

                    string fileExt = (Path.GetExtension(sRelativePath));
                    // Compress all files
                    //if (fileExtensions.ToList().IndexOf(fileExt) >= 0)
                    CompressFile(sInDir, sRelativePath, str);
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

        public static void CompactDataBase(string ConString)
        {
            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();

            IWorkspace workspace = workspaceFactory.OpenFromFile(ConString, 0);

            IDatabaseCompact cmpkt = (IDatabaseCompact)workspace;
            if (cmpkt.CanCompact()) cmpkt.Compact();

            workspace = null;
        }

        //public static List<AM_AbstractFeature> GetObjectsFromAmdbFile(string fileName)
        //{
           
        //    List<AM_AbstractFeature> res = new List<AM_AbstractFeature>();
        //    string[] FN = Directory.GetFiles(fileName, "*.amdb");
        //    if (FN.Length == 0) FN = Directory.GetFiles(fileName, "*.obj");
        //    for (int i = FN.Length; i > 0; i--)
        //    {
        //        string _file = FN[i - 1];
               
        //        JsonSerializerSettings settings = new JsonSerializerSettings
        //        {
        //            TypeNameHandling = TypeNameHandling.All
        //        };
             
        //        var prj = JsonConvert.DeserializeObject<AmdbObjectContainer>(File.ReadAllText(_file), settings);
        //        if ((prj.ObjectList != null) && (prj.ObjectList.Count > 0)) res.AddRange(prj.ObjectList);

        //    }

        //    if (FN.Length > 0) SetTargetDB(fileName);

        //    return res;
        //}

        public static byte[] SetObjectToBlob(object SHP, string propertyName)
        {

            // вначале переведем IGeometry к типу IMemoryBlobStream 
            IMemoryBlobStream memBlb = new MemoryBlobStream();
            IObjectStream objStr = new ObjectStream();
            objStr.Stream = memBlb;
            ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            IPersistStream perStr = (IPersistStream)propertySet;
            propertySet.SetProperty(propertyName, SHP);
            perStr.Save(objStr, 0);

            ////затем полученный IMemoryBlobStream представим в виде массива байтов
            object o;
            ((IMemoryBlobStreamVariant)memBlb).ExportToVariant(out o);

            byte[] bytes = (byte[])o;


            return bytes;
        }


        public static object GetObjectFromBlob(object anObject, string propName)
        {

            try
            {         
                byte[] bytes = (byte[])anObject;
                // сконвертируем его в геометрию 
                IMemoryBlobStream memBlobStream = new MemoryBlobStream();

                IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                IObjectStream anObjectStream = new ObjectStreamClass();
                anObjectStream.Stream = memBlobStream;

                IPropertySet aPropSet = new PropertySetClass();

                IPersistStream aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                object result = aPropSet.GetProperty(propName);

                return result;


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static bool Serialize(System.Object obj, string fileName)
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
                    return false;
                }
                byteArr = null;


                //if (File.Exists(fileName)) File.Delete(fileName);
                System.IO.FileStream strmFile = new System.IO.FileStream(fileName, FileMode.Create);
                strmMemSer.WriteTo(strmFile);
                strmMemSer.Close();
                strmMemSer.Dispose();

                xmlSer = null;

                strmFile.Flush();
                strmFile.Close();
                strmFile.Dispose();

                strmMemSer.Flush();
                strmMemSer.Close();
                strmMemSer.Dispose();

                return true;

            }
            catch (Exception ex)
            {
                //throw new Exception("Serialization failed: " + exp.InnerException.Message);
                System.Diagnostics.Debug.WriteLine("Serialization failed: " + ex.Message + ex.InnerException);
                //System.Diagnostics.Debug.WriteLine("--------- Outer Exception Data ---------");
                //WriteExceptionInfo(ex);
                //ex = ex.InnerException;
                //if (null != ex)
                //{
                //    Console.WriteLine("--------- Inner Exception Data ---------");
                //    if (ex.InnerException !=null) WriteExceptionInfo(ex.InnerException);
                //    ex = ex.InnerException;
                //}


                return false;
            }
        }

        public static object XmlDeSerializer(string xmlPathFile)
        {
            // Create xmlStream and load in the .XML file
            IXMLStream xmlStreamCls = new XMLStream();
            xmlStreamCls.LoadFromFile(xmlPathFile);


            // Create xmlReader and read the XML stream
            IXMLReader xmlReaderCls = new XMLReader();
            xmlReaderCls.ReadFrom((IStream)xmlStreamCls); // Explicit Cast


            // Create a serializer
            IXMLSerializer xmlSerializerCls = new XMLSerializer();


            // Return the XML contents
            return xmlSerializerCls.ReadObject(xmlReaderCls, null, null);
        }

        public static void XmlSerializer(System.String xmlPathFile, System.Object xmlObject, System.String xmlNodeName)
        {
            System.String elementURI = "http://www.esri.com/schemas/ArcGIS/9.2";


            // Create xml writer
            IXMLWriter xmlWriterCls = new XMLWriterClass();


            // Create xml stream
            IXMLStream xmlStreamCls = new XMLStreamClass();


            // Explicit Cast for IStream and then write to stream 
            xmlWriterCls.WriteTo((IStream)xmlStreamCls);


            // Serialize 
            IXMLSerializer xmlSerializerCls = new XMLSerializerClass();
            xmlSerializerCls.WriteObject(xmlWriterCls, null, null, xmlNodeName, elementURI, xmlObject);


            // Save the xmlObject to an xml file. When using xmlstream the cpu keeps data in memory until it is written to file. 
            xmlStreamCls.SaveToFile(@xmlPathFile);
        }

        public static bool CreateAmdbProject(IApplication m_application, AM_AerodromeReferencePoint arp = null)
        {
            
            var pathToTemplateFile = HelperMethods.GetPathToTemplateFile();
            try
            {
                if (Directory.Exists(pathToTemplateFile))
                {
                    var tempDirName = System.IO.Path.GetTempPath();
                    var dInf = Directory.CreateDirectory(tempDirName + @"\Aerodrome\" + Guid.NewGuid().ToString() + @"\");
                    tempDirName = dInf.FullName;

                    string pathToTemplateFileMxdSource = System.IO.Path.Combine(HelperMethods.GetMainFolder(), "AMDB.mxd");
                    string pathToTemplateFileMxdDest = System.IO.Path.Combine(tempDirName, "AMDB.mxd");

                    string pathToTemplateFileGdbSource = System.IO.Path.Combine(HelperMethods.GetMainFolder(), "AMDB.mdb");
                    string pathToTemplateFileGdbDest = System.IO.Path.Combine(tempDirName, "AMDB.mdb");

                    if (!File.Exists(pathToTemplateFileGdbSource))
                    {
                        CreateAmdbTemplate(HelperMethods.GetMainFolder(), "AMDB");
                    }

                    File.Copy(pathToTemplateFileMxdSource, pathToTemplateFileMxdDest, true);
                    File.Copy(pathToTemplateFileGdbSource, pathToTemplateFileGdbDest, true);

                    while (Directory.GetFiles(tempDirName).Length < 2) System.Threading.Thread.Sleep(1);

                    m_application.NewDocument(false, pathToTemplateFileMxdDest);
                    Application.DoEvents();
                    Application.DoEvents();

                    IMxDocument pNewDocument = (IMxDocument)m_application.Document;
                    Application.DoEvents();

                    var provider = new ListSyncProvider(System.IO.Path.GetDirectoryName(tempDirName));

                    // the data context
                    AerodromeDataCash.ProjectEnvironment = new AerodromeEnvironment { mxApplication = m_application, pMap = ((IMxDocument)m_application.Document).ActiveView.FocusMap };

                    AerodromeDataCash.ProjectEnvironment.Context = new Framework.Stasy.Context.ApplicationContext(provider, null);
                    AerodromeDataCash.ProjectEnvironment.Context.Initialize();
                    AerodromeDataCash.ProjectEnvironment.GeoDbProvider = new GeoDbSyncProvider();

                    AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath = tempDirName;
                    ExtensionDataCash.ProjectxtensionContext = new ExtensionDataContext(System.IO.Path.GetDirectoryName(tempDirName));
                    ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Initialize();



                    if (arp != null)
                    {
                        ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Organization = arp.Organization;
                        ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.ADHP = arp.idarpt?.Value;
                        ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Name = arp.name;

                        AerodromeDataCash.ProjectEnvironment.pMap.SpatialReference = EsriUtils.CreateSpatialReferenceByMeridian(arp.geopnt.X);

                        AerodromeDataCash.ProjectEnvironment.Context.PrepareEntity<AM_AerodromeReferencePoint>(arp, false);
                        AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)].Add(arp);

                        var stateChangedList = AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.GetEntitiesWithState();

                        var insertedList = stateChangedList.Where<KeyValuePair<SynchronizationOperation, object>>((Func<KeyValuePair<SynchronizationOperation, object>, bool>)(kvp => kvp.Key == SynchronizationOperation.Inserted)).Select<KeyValuePair<SynchronizationOperation, object>, object>((Func<KeyValuePair<SynchronizationOperation, object>, object>)(kvp => kvp.Value));

                        //Add to Entity List
                        AerodromeDataCash.ProjectEnvironment.Context._syncProvider.Insert(insertedList);
                        
                        AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                        AerodromeDataCash.ProjectEnvironment.GeoDbProvider.Insert(AerodromeDataCash.ProjectEnvironment.TableDictionary, (AM_AbstractFeature)arp);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }

        }

        public static void CreateAmdbTemplate(string path, string dbName)
        {
            LocalGdbGenerator dbGenerator = new LocalGdbGenerator();
            var dbWorkspace = dbGenerator.CreateAccessWorkspace(path, dbName);

            Assembly asm = typeof(AM_AerodromeReferencePoint).Assembly;

            #region Spatial Reference
            
            ESRI.ArcGIS.Geometry.ISpatialReferenceFactory spatialReferenceFactory = new ESRI.ArcGIS.Geometry.SpatialReferenceEnvironmentClass();

            ESRI.ArcGIS.Geometry.ISpatialReference spatialReference =

            spatialReferenceFactory.CreateGeographicCoordinateSystem((int)ESRI.ArcGIS.Geometry.esriSRGeoCSType.esriSRGeoCS_WGS1984);

            ESRI.ArcGIS.Geometry.ISpatialReferenceResolution spatialReferenceResolution = (ESRI.ArcGIS.Geometry.ISpatialReferenceResolution)spatialReference;

            spatialReferenceResolution.ConstructFromHorizon();

            ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance spatialReferenceTolerance = (ESRI.ArcGIS.Geometry.ISpatialReferenceTolerance)spatialReference;

            spatialReferenceTolerance.SetDefaultXYTolerance();
            #endregion

            foreach (var amObj in Enum.GetValues(typeof(Feat_Type)))
            {
                string featName = amObj.ToString().Replace("_", String.Empty);
                Type amFeature = asm.GetType(asm.GetName().Name + ".AM_" + featName);

                dbGenerator.CreateFeatureClassFromType(featName, amFeature, (IFeatureWorkspace)dbWorkspace, spatialReference);

            }
        }

        public static void OpenAmdbProject(string _FileName, IApplication m_application, bool showSplash = true)
        {
            FilePathProvider fp = new FilePathProvider(HelperMethods.GetMainFolder());

            //if (fp.IsExistTargetPath(_FileName)&& AerodromeDataCash.ProjectEnvironment?.CurProjectName != _FileName)
            //{
            //    System.Windows.MessageBox.Show("The project is already open", "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            //    return;
            //}

            if (AerodromeDataCash.ProjectEnvironment != null)
            {
               
                m_application.SaveDocument(System.IO.Path.Combine(AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath, "AMDB.mxd"));

                if (AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved == true)
                {                    
                    System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Save changes to the AMDB project?", "Aerodrome", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);
                    if (result == System.Windows.MessageBoxResult.Yes)
                    {                      
                        HelperMethods.SaveAmdbProject(m_application, showSplash: true);                        
                    }                     
                    else if (result == System.Windows.MessageBoxResult.Cancel)
                    {
                        return;
                    }

                    AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = false;
                }
            }

            if (showSplash)
            {
                Splasher.Splash = new SplashScreen();
                var parentHandle = new IntPtr(m_application.hWnd);
                var helper = new WindowInteropHelper(Splasher.Splash) { Owner = parentHandle };
                MessageListener.Instance.ReceiveMessage("Opening project...");
                Splasher.ShowSplash();
            }
            var tempDirName = System.IO.Path.GetTempPath();
            var dInf = Directory.CreateDirectory(tempDirName + @"\Aerodrome\" + System.IO.Path.GetFileNameWithoutExtension(_FileName) + "_" + Guid.NewGuid().ToString());
            tempDirName = dInf.FullName;
            string currTempPath = tempDirName;
            string currentFileName = _FileName;
            var tempPdmFilename = System.IO.Path.Combine(tempDirName, "aerodrome.amdb");
            var tempMxdFilename = System.IO.Path.Combine(tempDirName, "AMDB.mxd");
            var tempMdbFileName = System.IO.Path.Combine(tempDirName, "AMDB.mdb");

            try
            {
                //Распаковка файлов
                HelperMethods.DecompressToDirectory(_FileName, tempDirName);

                Application.DoEvents();

                m_application.Caption = System.IO.Path.GetFileNameWithoutExtension(_FileName);

                string ConString = System.IO.Path.Combine(tempDirName, "AMDB.mdb");
                if (File.Exists(ConString))
                {
                    HelperMethods.CompactDataBase(ConString);
                    tempMxdFilename = System.IO.Path.Combine(tempDirName, "AMDB.mxd");
                    m_application.NewDocument(false, tempMxdFilename);
                }
                
                //HelperMethods.SaveRecentFileName(_FileName);

                Application.DoEvents();

                tempDirName = System.IO.Path.GetDirectoryName(currTempPath);
                if (!System.IO.Directory.Exists(tempDirName) || (tempDirName.CompareTo("\\") == 0)) return;
                _FileName = System.IO.Path.GetFileName(tempDirName);

                var provider = new ListSyncProvider(currTempPath);
                AerodromeDataCash.ProjectEnvironment = new AerodromeEnvironment { mxApplication = m_application, pMap = ((IMxDocument)m_application.Document).ActiveView.FocusMap };
                AerodromeDataCash.ProjectEnvironment.CurProjectName = currentFileName;
                AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath = currTempPath;
                AerodromeDataCash.ProjectEnvironment.MapDocumentName = System.IO.Path.GetFileNameWithoutExtension(_FileName);

                AerodromeDataCash.ProjectEnvironment.Context = new Framework.Stasy.Context.ApplicationContext(provider, null);
                AerodromeDataCash.ProjectEnvironment.Context.Initialize();
                AerodromeDataCash.ProjectEnvironment.GeoDbProvider = new GeoDbSyncProvider();
               
                AerodromeDataCash.ProjectEnvironment.Context._entityContextManager.ResetEntitiesState();

                ExtensionDataCash.ProjectxtensionContext = new ExtensionDataContext(currTempPath);
                ExtensionDataCash.ProjectxtensionContext.LoadExtensionData();

                var arpColl = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)];

                if (arpColl?.Count() > 0)
                {
                    var arp = arpColl.FirstOrDefault();
                    var meridian = ((ESRI.ArcGIS.Geometry.IPoint)arp.geopnt).X;
                    AerodromeDataCash.ProjectEnvironment.pMap.SpatialReference = EsriUtils.CreateSpatialReferenceByMeridian(meridian);
                }

                //FilePathProvider fp = new FilePathProvider(HelperMethods.GetMainFolder());
                fp.Add(AerodromeDataCash.ProjectEnvironment.CurProjectName);

                if (showSplash)
                {
                    Splasher.CloseSplash();
                    MessageScreen messageScreen = new MessageScreen();
                    var parentHandle = new IntPtr(m_application.hWnd);
                    var messageScreeenHelper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
                    messageScreen.MessageText = "Succesfully opened";
                    messageScreen.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                Splasher.CloseSplash();
                AerodromeDataCash.ProjectEnvironment = null;
                System.Windows.MessageBox.Show(ex.Message, "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

        }

        public static void SaveAmdbProject(IApplication m_application, bool isCommitFeatures = true, bool isCommitMetadata = true, bool showSplash = false)
        {

            if (AerodromeDataCash.ProjectEnvironment is null)
            {
                System.Windows.MessageBox.Show("Empty project", "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            string ProjectName = "";
            bool NewProjectFlag = false;
            if (AerodromeDataCash.ProjectEnvironment.CurProjectName == null || !Directory.Exists(Path.GetDirectoryName(AerodromeDataCash.ProjectEnvironment.CurProjectName)))
            {
                var saveFileDialog1 = new SaveFileDialog
                {
                    Filter = @"Aerodrome type files (*.amdb)|*.amdb",
                    RestoreDirectory = true
                };

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
                ProjectName = saveFileDialog1.FileName;
                m_application.Caption = System.IO.Path.GetFileNameWithoutExtension(ProjectName);
                NewProjectFlag = true;
                AerodromeDataCash.ProjectEnvironment.CurProjectName = ProjectName;
            }
            else
                ProjectName = AerodromeDataCash.ProjectEnvironment.CurProjectName; //m_application.Caption;

            if (showSplash)
            {
                Splasher.Splash = new SplashScreen();
                var parentHandle = new IntPtr(m_application.hWnd);
                var helper = new WindowInteropHelper(Splasher.Splash) { Owner = parentHandle };
                MessageListener.Instance.ReceiveMessage("Saving project...");
                Splasher.ShowSplash();
            }

            try
            {
                string targetDB = AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath;
                string mxdName = System.IO.Path.Combine(targetDB, "AMDB.mxd");
                string projName = targetDB + @"\aerodrome.amdb";
                string mdbName = targetDB + @"\AMDB.mdb";

                var tempDirName = targetDB + @"\amdbData";

                DirectoryInfo dInf = System.IO.Directory.CreateDirectory(tempDirName);

                string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.amdb");
                foreach (var fl in FN)
                {
                    System.IO.File.Delete(fl);
                }
                if (isCommitFeatures)
                    AerodromeDataCash.ProjectEnvironment.Context.Commit();

                if (isCommitMetadata)
                    ExtensionDataCash.ProjectxtensionContext.CommitExtensionData();

                #region MXD File

                IMxDocument pNewDocument = (IMxDocument)m_application.Document;
                pNewDocument.RelativePaths = true;
                try
                {
                    m_application.SaveDocument(mxdName);
                }
                catch (Exception ex)
                {
                   //TODO: Записать лог при неудачном сохранении mxd 
                }
                
                Application.DoEvents();
                System.IO.File.Copy(mxdName, System.IO.Path.Combine(tempDirName, "AMDB.mxd"), true);
                Application.DoEvents();

                #endregion

                FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(projName), "*.obj");
                foreach (var fl in FN)
                {
                    System.IO.File.Copy(fl, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(fl)), true);
                }

                System.IO.File.Copy(mdbName, System.IO.Path.Combine(tempDirName, System.IO.Path.GetFileName(mdbName)), true);

                string sCompressedFile = ProjectName; /////

                if (!NewProjectFlag)
                {
                    if (File.Exists(sCompressedFile))
                    {
                        System.IO.File.Delete(sCompressedFile);
                        Application.DoEvents();
                    }
                }

                AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = false;

                HelperMethods.CompressDirectory(tempDirName, sCompressedFile);

                HelperMethods.DeleteFilesFromDirectory(tempDirName + @"\");
                Directory.Delete(tempDirName);

                //HelperMethods.SaveRecentFileName(ProjectName); /////////
                FilePathProvider fp = new FilePathProvider(HelperMethods.GetMainFolder());
                fp.Add(AerodromeDataCash.ProjectEnvironment.CurProjectName);

                if (showSplash)
                {
                    Splasher.CloseSplash();
                }
            }
            catch (Exception ex)
            {
                Splasher.CloseSplash();
                System.Windows.MessageBox.Show(ex.Message, "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

        }
    }
}
