using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Collections;
using System.IO.Compression;
using System.ComponentModel;
using ANCOR.MapCore;
using ESRI.ArcGIS.ArcMapUI;
//using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using ArenaINIManager;
using System.Diagnostics;
using AranSupport;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
//using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.DataSourcesGDB;

namespace ArenaStatic
{

    public static class ArenaStaticProc
    {
        public static bool ShowProgressBar = false;
        public static string PathToSpecificationFile;
        //delegate void ProgressDelegate(string sMessage);


        public static int getObjectSize(Object obj)
        {
            XmlSerializer xmlSer = null;
            MemoryStream strmMemSer = null;
            try
            {
                xmlSer = new XmlSerializer(obj.GetType());
                strmMemSer = new MemoryStream();
                xmlSer.Serialize(strmMemSer, obj);

                byte[] byteArr = new byte[strmMemSer.Length];


                return byteArr.Length;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.InnerException);
                //System.Diagnostics.Debug.WriteLine(ex.Data);
                return -1;
            }
            finally
            {
                strmMemSer.Close();
                strmMemSer.Dispose();

                xmlSer = null;

                strmMemSer.Flush();
                strmMemSer.Close();
                strmMemSer.Dispose();
            }
        }

        public static bool Serialize(Object obj, string fileName)
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
                System.Diagnostics.Debug.WriteLine("Serialization failed: " + ex.Message);
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

        public static bool checkGuid(string GuigString)
        {
            Guid GuidID;
            return Guid.TryParse(GuigString, out GuidID);
        }

        public static void WriteExceptionInfo(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Message: {0}", ex.Message);
            System.Diagnostics.Debug.WriteLine("Exception Type: {0}", ex.GetType().FullName);
            System.Diagnostics.Debug.WriteLine("Source: {0}", ex.Source);
            System.Diagnostics.Debug.WriteLine("StrackTrace: {0}", ex.StackTrace);
            System.Diagnostics.Debug.WriteLine("TargetSite: {0}", ex.TargetSite);
        }


        public static void SerializeANDZip(Object obj, string fileName)
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
                    return;
                }


                MemoryStream strmMemCompressed = new MemoryStream();
                GZipStream strmZip = new GZipStream(strmMemCompressed, CompressionMode.Compress, true);
                strmZip.Write(byteArr, 0, byteArr.Length);
                strmZip.Close();
                if (File.Exists(fileName)) File.Delete(fileName);

                System.IO.FileStream strmFileCompressed = new System.IO.FileStream(fileName, FileMode.CreateNew);
                strmMemCompressed.WriteTo(strmFileCompressed);
                strmMemCompressed.Close();
                strmFileCompressed.Close();

                strmMemCompressed.Dispose();



            }
            catch (Exception exp)
            {
                throw new Exception("Serialization failed: " + exp.InnerException.Message);
                //System.Windows.Forms.MessageBox.Show("Serialization failed: "+exp.InnerException.Message);
            }
        }


        public static void SetObjectValue(object EditedObject, string PropertyName, object Value)
        {

            object objVal2 = EditedObject;

            string[] sa = PropertyName.Split('.');
            if (sa.Length == 0) return;

            foreach (string s in sa)
            {
                if (EditedObject == null) continue;
                PropertyInfo propInfo = EditedObject.GetType().GetProperty(s);

                if (propInfo == null) continue;
                try
                {
                    int tpInd = CheckType(propInfo.PropertyType);

                    switch (tpInd)
                    {
                      case  2 :
                            propInfo.SetValue(EditedObject, Convert.ToDouble(Value), null);
                            break;
                        case 3:
                            propInfo.SetValue(EditedObject, Convert.ToInt32(Value), null);
                            break;
                        case 4:
                            propInfo.SetValue(EditedObject, Value, null);
                            break;
                        default:
                            propInfo.SetValue(EditedObject, Value, null);
                            break;
                    }

                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    continue;
                }

            }




        }

        private static bool IsTypeADoubleType(Type typeToCheck)
        {
            var typeCode = Type.GetTypeCode(GetUnderlyingType(typeToCheck));

            switch (typeCode)
            {
                //case TypeCode.Boolean:
                //case TypeCode.Byte:
                //case TypeCode.Char:
                //case TypeCode.DateTime:
                //case TypeCode.Decimal:
                case TypeCode.Double:
                //case TypeCode.Int16:
                //case TypeCode.Int32:
                //case TypeCode.Int64:
                //case TypeCode.SByte:
                //case TypeCode.Single:
                //case TypeCode.String:
                //case TypeCode.UInt16:
                //case TypeCode.UInt32:
                //case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }
        private static bool IsTypeAStringType(Type typeToCheck)
        {
            var typeCode = Type.GetTypeCode(GetUnderlyingType(typeToCheck));

            switch (typeCode)
            {
                //case TypeCode.Boolean:
                //case TypeCode.Byte:
                //case TypeCode.Char:
                //case TypeCode.DateTime:
                //case TypeCode.Decimal:
                //case TypeCode.Double:
                //case TypeCode.Int16:
                //case TypeCode.Int32:
                //case TypeCode.Int64:
                //case TypeCode.SByte:
                //case TypeCode.Single:
                case TypeCode.String:
                    //case TypeCode.UInt16:
                    //case TypeCode.UInt32:
                    //case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        private static int CheckType(Type typeToCheck)
        {
            var typeCode = Type.GetTypeCode(GetUnderlyingType(typeToCheck));

            switch (typeCode)
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Char:
                    return 0;
                case TypeCode.DateTime:
                    return 1;
                case TypeCode.Decimal:
                case TypeCode.Double:
                    return 2;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return 3;
                case TypeCode.String:
                    return 4;
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return 0;
                default:
                    return 0;
            }
        }


        private static Type GetUnderlyingType(Type typeToCheck)
        {
            if (typeToCheck.IsGenericType &&
                typeToCheck.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(typeToCheck);
            }
            else
            {
                return typeToCheck;
            }
        }

        public static string GetObjectValueAsString(object obj, string propName)
        {

            object objVal = obj;

            string[] sa = propName.Split('.');
            if (sa.Length == 0)
                return null;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = objVal.GetType().GetProperty(s);

                if (propInfo == null)
                    return null;

                object objPropVal = propInfo.GetValue(objVal, null);


                if (objPropVal is IList && (objPropVal as IList).Count > 0)
                {
                    objPropVal = (objPropVal as IList)[0];
                }
                else if (objPropVal is IList && (objPropVal as IList).Count <= 0)
                {
                    objPropVal = null;
                }

                objVal = objPropVal;

                if (objVal == null)
                    return null;
            }

            ////System.Diagnostics.Debug.WriteLine(propName + " - " + objVal.GetType().ToString());

            return (objVal == null ? "<null>" : objVal.ToString());
        }

        public static List<string> GetObjectProperties(object obj)
        {
            List<string> res = null;
            object objVal = obj;

            var propInfo = objVal.GetType().GetRuntimeProperties();

            if (propInfo == null)
                return null;

            foreach (var objPropVal in propInfo)
            {
                if (objPropVal is IList) continue;
                if (res == null) res = new List<string>();
                res.Add(objPropVal.Name);
            }


            return res;
        }


        public static string GetObjectUomString(object obj, string propName)
        {

            object objVal = obj;

            string[] sa = propName.Split('.');
            if (sa.Length == 0) return null;

            foreach (PropertyInfo propInfo in objVal.GetType().GetProperties())
            {

                if (propInfo == null) continue;
                if (propInfo.Name.ToUpper().Contains(propName.ToUpper()) && propInfo.Name.ToUpper().Contains("UOM"))
                {
                    object objPropVal = propInfo.GetValue(objVal, null);

                    objVal = objPropVal;

                    //if (objVal == null) return null;
                    break;
                }

            }

            return (objVal == null ? "<null>" : objVal.ToString());
        }


        public static bool HasProperty(object obj, string propName)
        {
            object objVal = obj;

            PropertyInfo propInfo = objVal.GetType().GetProperty(propName);
            return propInfo != null;

        }


        public static object GetObjectValue(object obj, string propName, bool ReturnFirstFromList = true)
        {
            object objVal = obj;

            string[] sa = propName.Split('.');
            if (sa.Length == 0)
                return null;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = objVal.GetType().GetProperty(s);

                if (propInfo == null)
                    return null;

                object objPropVal = propInfo.GetValue(objVal, null);


                if (objPropVal is IList && ReturnFirstFromList)
                {
                    objPropVal = (objPropVal as IList)[0];
                }

                objVal = objPropVal;

                if (objVal == null)
                    return null;
            }

            return (objVal == null ? null : objVal);
        }

        public static string GetObjectPropertyDescription(object obj, string propName)
        {

            object objVal = obj;
            string _description = "";

            string[] sa = propName.Split('.');
            if (sa.Length == 0)
                return null;

            foreach (string s in sa)
            {
                PropertyInfo propInfo = objVal.GetType().GetProperty(s);

                if (propInfo == null)
                    return null;

                ////////////////////////////////////////////////
                DescriptionAttribute _DescriptionAttribute = DescriptionAttribute.GetCustomAttribute(propInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (_DescriptionAttribute != null)
                {
                    _description = _DescriptionAttribute.Description;
                }
                ////////////////////////////////////////////////


            }

            return _description;

        }

        public static string airspaceCodeType_to_AirspaceType(string _CodeType)
        {
            string[] aixmenum = { "NAS","FIR","FIR_P","UIR","UIR_P","CTA","CTA_P","OCA_P","OCA","UTA","UTA_P",
                                  "TMA","TMA_P","CTR","CTR_P","OTA","SECTOR","SECTOR_C","TSA","CBA","RCA","RAS",
                                  "AWY","MTR","P","R","D","ADIZ","NO_FIR","PART","CLASS","POLITICAL","D_OTHER",
                                  "TRA","A","W","PROTECT","AMA","ASR","ADV","UADV","ATZ","ATZ_P","HTZ","NAS_P","OTHER"};

            if (aixmenum.Contains(_CodeType)) return _CodeType;
            else return "OTHER";


        }

        public static double UomTransformation(string UOM_From, string UOM_To, double val, int rounder = 0)
        {
            double res = val;

            switch (UOM_From)
            {
                case "M":
                    switch (UOM_To)
                    {
                        #region 

                        case "M":
                            res = res * 1;
                            break;
                        case "KM":
                            res = res * 0.001;
                            break;
                        case "FT":
                            res = res * 3.28083;
                            break;
                        case "NM":
                            res = res * 0.0005399568;
                            break;
                        default:
                            res = res * 1;
                            break;

                            #endregion
                    }
                    break;
                case "KM":
                    switch (UOM_To)
                    {
                        #region 

                        case "M":
                            res = res * 1000;
                            break;
                        case "KM":
                            res = res * 1;
                            break;
                        case "FT":
                            res = res * 3280.83;
                            break;
                        case "NM":
                            res = res * 0.5399555;
                            break;
                        default:
                            res = res * 1;
                            break;

                            #endregion
                    }
                    break;
                case "FT":
                    switch (UOM_To)
                    {
                        #region 
                        case "M":
                            res = res * 0.3048;
                            break;
                        case "KM":
                            res = res * 0.0003048009;
                            break;
                        case "FT":
                            res = res * 1;
                            break;
                        case "NM":
                            res = res * 0.0001645784;
                            break;
                        default:
                            res = res * 1;
                            break;
                            #endregion
                    }
                    break;
                case "NM":
                    switch (UOM_To)
                    {
                        #region 

                        case "M":
                            res = res * 1852;
                            break;
                        case "KM":
                            res = res * 1.852;
                            break;
                        case "FT":
                            res = res * 6076.131;
                            break;
                        case "NM":
                            res = res * 1;
                            break;
                        default:
                            res = res * 1;
                            break;

                            #endregion
                    }
                    break;
                default:
                    res = res * 1;
                    break;
            }

            return Math.Round(res, rounder);
        }

        public static void CloseProgressBar()
        {
            //ProgressHandler.Close();
        }

        public static void CompressDirectory(string sInDir, string sOutFile, string[] userFileExtensions = null)
        {

            string[] fileExtensions = { ".mxd", ".mdb", ".pdm" , ".txt"};
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

        public static void CompressAllDirectory(string sInDir, string sOutFile)
        {
            string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
            int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

            using (System.IO.FileStream outFile = new System.IO.FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
                foreach (string sFilePath in sFiles)
                {
                    string sRelativePath = sFilePath.Substring(iDirLen);
                    CompressFile(sInDir, sRelativePath, str);
                }
        }


        public static bool DecompressToDirectory(string sCompressedFile, string sDir)
        {
            bool res = true;
            try
            {
                AlertForm alrtForm = new AlertForm();
                alrtForm.FormBorderStyle = FormBorderStyle.None;
                alrtForm.Opacity = 0.5;
                alrtForm.BackgroundImage = ArenaStatic.Properties.Resources.ArenaSplash;
                alrtForm.TopMost = true;
                alrtForm.progressBar1.Visible = true;
                alrtForm.progressBar1.Value = 0;

                //string[] F = System.IO.Directory.GetFiles(sDir, "*.*");
                alrtForm.progressBar1.Maximum = 100;

                alrtForm.label1.Visible = false;
                if (!System.Diagnostics.Debugger.IsAttached) alrtForm.Show();

                using (System.IO.FileStream inFile = new System.IO.FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
                using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
                    while (DecompressFile(sDir, zipStream))
                    {
                        alrtForm.progressBar1.Value++;
                        if (alrtForm.progressBar1.Value  == alrtForm.progressBar1.Maximum)
                            alrtForm.progressBar1.Maximum += 100;

                        System.Diagnostics.Debug.WriteLine(alrtForm.progressBar1.Value.ToString());
                    }


                alrtForm.Close();
            }
            catch { res = false; }

            return res;
        }

        public static bool _DecompressToDirectory(string sCompressedFile, string sDir)
        {
            bool res = true;
            try
            {
                System.IO.FileStream inFile = new System.IO.FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None);
                GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true);
                while (DecompressFile(sDir, zipStream)) System.Threading.Thread.Sleep(1);

                zipStream.Close();
                inFile.Close();

                zipStream.Dispose();
                inFile.Dispose();
            }
            catch { res = false; }

            return res;
        }

        public static void CompressFile(string archivFileName, string pathToFile)
        {

            string anyString = File.ReadAllText(pathToFile);
            // A.
            // Write string to temporary file.
            string temp = Path.GetTempFileName();
            File.WriteAllText(temp, anyString);

            // B.
            // Read file into byte array buffer.
            byte[] b;
            using (System.IO.FileStream f = new System.IO.FileStream(temp, FileMode.Open))
            {
                b = new byte[f.Length];
                f.Read(b, 0, (int)f.Length);
            }

            // C.
            // Use GZipStream to write compressed bytes to target file.
            using (System.IO.FileStream f2 = new System.IO.FileStream(archivFileName, FileMode.Create))
            using (GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
            {
                gz.Write(b, 0, b.Length);
            }
        }


        public static void Decompress(FileInfo fileToDecompress, string newFileName)
        {
            using (System.IO.FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;

                using (System.IO.FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        //Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
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

        public static void DeleteFilesFromDirectory(string DirName)
        {
            //if (System.Diagnostics.Debugger.IsAttached) MessageBox.Show("DeleteFilesFromDirectory " + DirName);

            //string[] FN = Directory.GetFiles(System.IO.Path.GetDirectoryName(DirName), "*.*");
            //foreach (var fl in FN)
            //{
            //    File.Delete(DirName + @"\" + System.IO.Path.GetFileName(fl));
            //}

        }

        public static string SendMapToArchive(string selectedChart)
        {

            try
            {
                ////Archive Copy
                string Dirname = System.IO.Path.GetDirectoryName(selectedChart);
                string ArchiveName = System.IO.Path.Combine(ArenaStaticProc.GetMapArchiveFolder(), System.IO.Path.GetFileNameWithoutExtension(selectedChart) + "_Archive_" + DateTime.Now.Ticks.ToString()) + ".SigmaZip";


                if (System.IO.File.Exists(ArchiveName)) System.IO.File.Delete(ArchiveName);

                List<string> tmp = new List<string>();

                tmp.Add(System.IO.Path.GetFileName(selectedChart));
                tmp.Add(DateTime.Now.ToLongDateString());
                tmp.Add(DateTime.Now.ToLongTimeString());


                System.IO.File.WriteAllLines(System.IO.Path.Combine(Dirname, "info.txt"), tmp.ToArray());


                string[] fileEx = new string[] { ".mxd", ".mdb", ".obj", ".txt" };


                ArenaStaticProc.CompressDirectory(Dirname, ArchiveName, fileEx);



                return ArchiveName;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                return "";
            }

        }

        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return;
            //if (path.Length > 5 && !path.ToUpper().Contains("TEMP")) return;

            if (System.Diagnostics.Debugger.IsAttached) MessageBox.Show("Folder deleting");

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

        public static string LonToDDMMSS(string X_Lon, coordtype coorT)
        {
            string res = "";

            try
            {
                AranSupport.Utilitys ut = new AranSupport.Utilitys();
                double Coord = IsNumeric(X_Lon) ? Convert.ToDouble(X_Lon) : ut.GetLONGITUDEFromAIXMString(X_Lon);
                //double Coord = ut.GetLONGITUDEFromAIXMString(X_Lon);
                if (Coord == 0) Coord = Convert.ToDouble(X_Lon);
                string sign = "E";
                if (Coord < 0)
                {
                    sign = "W";
                    Coord = Math.Abs(Coord);
                }

                double X = Math.Round(Coord, 10);

                int deg = (int)X;
                double delta = Math.Round((X - deg) * 60, 8);

                int min = (int)delta;
                int rounder = coorT.ToString().Contains("SS_SS") ? 2 : 0;
                if (rounder == 0)
                {
                    rounder = coorT.ToString().Contains("SS_S") ? 1 : 0;
                }
                delta = Math.Round((delta - min) * 60, rounder);

                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                degSTR = deg < 10 ? "0" + degSTR : "0";
                degSTR = deg < 100 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                switch (coorT)
                {
                    case coordtype.DDMMSSN_1:
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_1:
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.DDMMSS_2:
                        res = degSTR + minSTR + secSTR + sign;
                        break;
                    case coordtype.NDDMMSS_2:
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;

                    ////////////////////////////////


                    case coordtype.DDMMSS_SSN_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_SS_3:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.NDDMMSS_SS_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;
                    case coordtype.DDMMSS_SS_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = degSTR + minSTR + secSTR + sign;
                        break;

                    ////////////////////////////////

                    case coordtype.DDMMSS_SN_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_S_3:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.NDDMMSS_S_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;
                    case coordtype.DDMMSS_S_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = degSTR + minSTR + secSTR + sign;
                        break;
                    default:
                        break;

                }

                //res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;

            }
            catch (Exception ex) { }

            return res;
        }

        public static string LatToDDMMSS(string Y_Lat, coordtype coorT)
        {
            string res = "";

            try
            {
                AranSupport.Utilitys ut = new AranSupport.Utilitys();
                double Coord = IsNumeric(Y_Lat) ? Convert.ToDouble(Y_Lat) : ut.GetLATITUDEFromAIXMString(Y_Lat);
                if (Coord == 0) Coord = Convert.ToDouble(Y_Lat);

                //double Coord = Convert.ToDouble(Y_Lat);

                string sign = "N";
                if (Coord < 0)
                {
                    sign = "S";
                    Coord = Math.Abs(Coord);
                }

                double Y = Math.Round(Coord, 10);
                //X = RealMode(X, 360);

                int deg = (int)Y;
                double delta = Math.Round((Y - deg) * 60, 8);

                int min = (int)delta;
                int rounder = coorT.ToString().Contains("SS_SS") ? 2 : 0;
                if (rounder == 0)
                {
                    rounder = coorT.ToString().Contains("SS_S") ? 1 : 0;
                }
                delta = Math.Round((delta - min) * 60, rounder);

                //delta = Math.Round(delta, 0);/////?????????Math.Round(


                string degSTR = "0";
                string minSTR = "0";
                string secSTR = "0";

                if (delta == 60) { min++; delta = 0; }
                if (min == 60) { deg++; min = 0; }

                degSTR = deg < 10 ? degSTR + deg.ToString() : deg.ToString();
                minSTR = min < 10 ? minSTR + min.ToString() : min.ToString();
                secSTR = delta < 10 ? secSTR + delta.ToString() : delta.ToString();

                switch (coorT)
                {
                    case coordtype.DDMMSSN_1:
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_1:
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.DDMMSS_2:
                        res = degSTR + minSTR + secSTR + sign;
                        break;
                    case coordtype.NDDMMSS_2:
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;

                    ////////////////////////////////

                    case coordtype.DDMMSS_SSN_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_SS_3:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.NDDMMSS_SS_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;
                    case coordtype.DDMMSS_SS_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 5)
                            secSTR = secSTR + "0";
                        res = degSTR + minSTR + secSTR + sign;
                        break;

                    ////////////////////////////////

                    case coordtype.DDMMSS_SN_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = degSTR + "°" + minSTR + "'" + secSTR + "\"" + sign;
                        break;
                    case coordtype.NDDMMSS_S_3:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + "°" + minSTR + "'" + secSTR + "\"";
                        break;
                    case coordtype.NDDMMSS_S_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = sign + " " + degSTR + minSTR + secSTR;
                        break;
                    case coordtype.DDMMSS_S_2:
                        if (!secSTR.Contains(".")) secSTR = secSTR + ".";
                        while (secSTR.Length < 4)
                            secSTR = secSTR + "0";
                        res = degSTR + minSTR + secSTR + sign;
                        break;
                    default:
                        break;
                }

                //res = degSTR + "°" + minSTR + "'" + secSTR + "'" + "'" + sign;
            }
            catch (Exception ex) { }

            return res;
        }

        public static bool IsNumeric(string anyString)
        {
            if (anyString == null)
            {
                anyString = "";
            }
            if (anyString.Length > 0)
            {
                double dummyOut = new double();
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", true);
                return Double.TryParse(anyString, System.Globalization.NumberStyles.Any, cultureInfo.NumberFormat, out dummyOut);
            }
            else
            {
                return false;
            }
        }


        public static void BringToFrontToc(ESRI.ArcGIS.ArcMapUI.IMxDocument mxDocument, string TocName, bool MandatoryFlag = false)
        {
            if (mxDocument.CurrentContentsView.Name.StartsWith(TocName) && !MandatoryFlag) return;

            for (int i = 0; i < mxDocument.ContentsViewCount; i++)
            {
                IContentsView cnts = mxDocument.get_ContentsView(i);

                string cntxtName = mxDocument.ContentsView[i].Name;

                if (cntxtName.StartsWith(TocName))
                {
                    mxDocument.CurrentContentsView = cnts;
                    mxDocument.ContentsView[i].Refresh(cntxtName);
                }
            }

        }

        public static void AutoSaveArenaProject(IApplication _application)
        {
            UID menuID = new UIDClass();

            menuID.Value = "{da3b537a-ce02-44bf-be27-98194abc713a}";

            ICommandItem pCmdItem = _application.Document.CommandBars.Find(menuID);
            pCmdItem.Execute();
            Marshal.ReleaseComObject(pCmdItem);
            Marshal.ReleaseComObject(menuID);

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        }

        public static bool CreateArenaIniFile()
        {
            try
            {
                var ArenaIni = new IniFile(GetMainFolder());
                ArenaIni.CreateArenaIni(GetArenaVersion());


                string modelFolder = GetProgrammFolder() + @"\Model";
                string new_modelFolder = GetMainFolder() + @"\Model";
                string archiveFldr = GetMapArchiveFolder();

                if (!System.IO.Directory.Exists(new_modelFolder))
                {
                    DirectoryCopy(modelFolder, new_modelFolder, true);

                    if (!System.IO.Directory.Exists(archiveFldr))
                        System.IO.Directory.CreateDirectory(archiveFldr);

                    return true;

                }



                #region new  Version 2.2.7.151 Release 1

                /// 1. обновляем файлы 
                DirectoryInfo dir = new DirectoryInfo(modelFolder);

                DirectoryInfo[] dirs = dir.GetDirectories();

                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Name.CompareTo("aerodrome.mdb") == 0) file.CopyTo(new_modelFolder + @"\" + file.Name, true);
                    if (file.Name.CompareTo("aerodrome.mxd") == 0) file.CopyTo(new_modelFolder + @"\" + file.Name, true);
                    if (file.Name.CompareTo("arena_PDM.mxd") == 0) file.CopyTo(new_modelFolder + @"\" + file.Name, true);
                    if (file.Name.CompareTo("pdm.mdb") == 0) file.CopyTo(new_modelFolder + @"\" + file.Name, true);
                    //if (file.Name.CompareTo("SIGMA_manual.chm") == 0) file.CopyTo(new_modelFolder + @"\" + file.Name, true);
                   // if (file.Name.CompareTo("WMM.CNF") == 0) file.CopyTo(new_modelFolder + @"\" + file.Name, true);
                    if (file.Name.CompareTo("WMM.CNF") == 0) file.CopyTo(new_modelFolder + @"\" + file.Name, true);

                }


                /// 2. перезаписываем папки ARINC & Fonts
                if (System.IO.Directory.Exists(new_modelFolder + @"\ARINC"))
                {
                    System.IO.Directory.Delete(new_modelFolder + @"\ARINC", true);
                }
                DirectoryCopy(modelFolder + @"\ARINC", new_modelFolder + @"\ARINC", true);


                // 3. перезаписываем файлы внутри папок - шаблонов карт
                foreach (var dr in dirs)
                {
                    if (dr.Name.StartsWith("SIGMA"))
                    {
                        DirectoryInfo[] Sigma_dirs = dr.GetDirectories();

                        foreach (var tmp_folder in Sigma_dirs)
                        {
                            if (tmp_folder.Name.StartsWith("Templates"))
                            {
                                DirectoryInfo[] temp_dirs = tmp_folder.GetDirectories();
                                foreach (var item in temp_dirs)
                                {
                                    if (item.Name.StartsWith("ChartTypeA")) continue;

                                    files = item.GetFiles();

                                    if (!Directory.Exists(new_modelFolder + @"\SIGMA\Templates\" + item.Name))
                                        Directory.CreateDirectory(new_modelFolder + @"\SIGMA\Templates\" + item.Name);

                                    foreach (FileInfo file in files)
                                    {
                                        if (file.Extension.EndsWith("xls")) file.CopyTo(new_modelFolder + @"\SIGMA\Templates\" + item.Name + @"\" + file.Name, true);
                                        if (file.Extension.EndsWith("sce")) file.CopyTo(new_modelFolder + @"\SIGMA\Templates\" + item.Name + @"\" + file.Name, true);
                                        if (file.Name.StartsWith("sigma")) file.CopyTo(new_modelFolder + @"\SIGMA\Templates\" + item.Name + @"\" + file.Name, true);
                                    }
                                }


                                break;
                            }

                        }

                        break;
                    }
                }


                #endregion

                #region new Version 2.2.7.151 Release 1

                if (!System.IO.Directory.Exists(archiveFldr))
                {
                    System.IO.Directory.CreateDirectory(archiveFldr);
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

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

        public static string GetMainFolder()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RISK\ARENA";
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

        public static string GetProgrammFolder2()
        {
            // возвращает путь к папке, где расположена активная Dll
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string _path = System.IO.Path.GetDirectoryName(path);
            return System.IO.Path.GetDirectoryName(_path);
        }

        public static string GetExecutingFolder()
        {
            // возвращает путь к папке, где расположена активная Dll
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string _path = System.IO.Path.GetDirectoryName(path);
            return _path;
        }

        public static string GetPathToAreaFile()
        {

            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();


            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            return ArenaIni.Read("AreaFile", "ARINC");

          
        }

        public static string GetPathToRegionsFile()
        {
            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            return ArenaIni.Read("Regions", "ARINC");
            
        }

        public static string GetPathToSpecificationFile()
        {

            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            return ArenaIni.Read("AreaFile", "ARINC");

        }

        public static string GetPathToARINCSpecificationFile()
        {

            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            return ArenaIni.Read("SpecificationPath", "ARINC");

           
        }

        public static string GetPathToTemplateFile()
        {
            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            return ArenaIni.Read("PdmFile", "Arena");

           
        }

        public static string GetPathToTemplate()
        {
            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            return ArenaIni.Read("templateFolder", "SIGMA");

           
        }

        public static void SetTargetDB(string targetDBpath)
        {
            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            ArenaIni.Write("TargetDB", targetDBpath, "Arena");

            return;

        }

        public static string GetTargetDB()
        {
            string pathToMdb = GetTargetDBFolder() + @"\pdm.mdb";
            if (!File.Exists(pathToMdb))
            {
                string dp = System.IO.Path.GetDirectoryName(pathToMdb);
                pathToMdb = System.IO.Path.Combine(dp, "aerodrome.mdb");
            }
            return pathToMdb;

          
        }

        public static string GetTargetDBFolder()
        {
            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            return ArenaIni.Read("TargetDB", "Arena");
        }

        public static string GetTargetDB_Aerodrome()
        {
            return GetTargetDBFolder() + @"\aerodrome.mdb";

        }

        public static string GetArenaVersion()
        {
            string pF = GetProgrammFolder() + @"\BIN";

            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(pF + @"\ArenaToolBox.dll");
            return myFileVersionInfo.FileVersion;
        }

		public static string GetArenaVersion2()
		{
			string pF = GetProgrammFolder2() ;

			FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(pF + @"\ArenaToolBox.dll");
			return myFileVersionInfo.FileVersion;
		}


		private static bool IniSuccess()
        {
            bool res = File.Exists(ArenaStatic.ArenaStaticProc.GetMainFolder() + @"\ArenaSettings.ini");
            if (res)
            {
                var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");

                if (ArenaIni.Read("version", "Arena").CompareTo(GetArenaVersion()) != 0) return false;


                string tarDB = ArenaIni.Read("PdmFile", "Arena");

                tarDB = System.IO.Path.Combine(tarDB, "pdm.mdb");

                res = File.Exists(tarDB);
            }
            return res;
        }

        public static string GetPathToMapFolder()
        {

            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            string res1 = ArenaIni.Read("mapFolder", "SIGMA");
            if (res1.CompareTo(@"C:\") == 0)
                res1 = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            return res1;

          
        }

        public static string GetMapArchiveFolder()
        {

            //if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            string res1 = ArenaIni.Read("archiveFolder", "SIGMA");
            if (res1.CompareTo(@"C:\") == 0)
                res1 = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            return res1;

           
        }

        public static void SetPathToMapFolder(string Path)
        {
            if (!IniSuccess()) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");
            ArenaIni.Write("mapFolder", Path, "SIGMA");

            return;

        }

        public static void SaveRecentFileName(string Filename)
        {
            if (!File.Exists(ArenaStatic.ArenaStaticProc.GetMainFolder() + @"\ArenaSettings.ini")) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(GetMainFolder() + @"\ArenaSettings.ini");

            string recentList = ArenaIni.Read("RecentFiles", "ARENA");
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

            ArenaIni.Write("RecentFiles", recentList, "ARENA");

        }


        public static string[] GetRecentFiles_Pdm()
        {
            if (!File.Exists(ArenaStatic.ArenaStaticProc.GetMainFolder() + @"\ArenaSettings.ini")) ArenaStatic.ArenaStaticProc.CreateArenaIniFile();
            var ArenaIni = new IniFile(ArenaStatic.ArenaStaticProc.GetMainFolder() + @"\ArenaSettings.ini");

            string recentList = ArenaIni.Read("RecentFiles", "ARENA");
            if (recentList.EndsWith("?")) recentList = recentList.TrimEnd('?');

            string[] recentFilePaths = recentList.Split('?');



            return recentFilePaths;
        }

        public static bool CheckPermission(string directoryFullName)
        {
            bool res = true;
            string directory = directoryFullName;

            DirectoryInfo di = new DirectoryInfo(directory);

            DirectorySecurity ds = di.GetAccessControl();

            foreach (AccessRule rule in ds.GetAccessRules(true, true, typeof(NTAccount)))
            {
                //Console.WriteLine("Identity = {0}; Access = {1}",
                //              rule.IdentityReference.Value, rule.AccessControlType);

                res = res && (rule.AccessControlType == AccessControlType.Allow);
            }

            return res;

        }

        public static void SetEnvironmentPath()
        {

            var oldPath = System.Environment.GetEnvironmentVariable("Path");

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string executedPath = System.IO.Path.GetDirectoryName(path);

            System.Environment.SetEnvironmentVariable("Path", executedPath + ";" + oldPath);
        }

        public static string GetZippedChartPath(string documentFileName)
        {
            string tmpfolder = System.IO.Path.GetTempPath();

            System.IO.DirectoryInfo inf = System.IO.Directory.CreateDirectory(tmpfolder + @"\" + DateTime.Now.Ticks.ToString());

            ArenaStatic.ArenaStaticProc.DirectoryCopy(documentFileName, inf.FullName, false);

            string mapName = System.IO.Path.Combine(inf.FullName, System.IO.Path.GetFileName(documentFileName));

            mapName = ArenaStatic.ArenaStaticProc.SendMapToArchive(mapName);

            ArenaStatic.ArenaStaticProc.DeleteDirectory(inf.FullName);
            return mapName;
        }
    }

}
