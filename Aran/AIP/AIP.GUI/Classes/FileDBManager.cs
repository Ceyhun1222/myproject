using AIP.DB;
using AIP.XML;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using AIP.BaseLib.Class;
using Aran.Temporality.CommonUtil.Context;
using MongoDB.Bson;
using AIP = AIP.XML.AIP;

namespace AIP.GUI.Classes
{
    internal static class FileDBManager
    {
        /// <summary>
        /// Object collect all messages to send into Main form for the output with the AddOutput method
        /// </summary>
        internal static Queue<BaseLib.Struct.Output> OutputQueue = new Queue<BaseLib.Struct.Output>();
        
        internal static void Upload(List<Folder> folderList)
        {
            try
            {
                foreach (var folder in folderList)
                {
                    SaveCollection(folder);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        internal static void Download(List<Folder> folderList)
        {
            try
            {
                foreach (var folder in folderList)
                {
                    GetCollection(folder);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void SendOutput(string Message, Color? Color = null, int? Percent = null)
        {
            try
            {
                BaseLib.Struct.Output output = new BaseLib.Struct.Output();
                output.Message = Message;
                output.Color = Color ?? System.Drawing.Color.Black;
                output.Percent = Percent;
                OutputQueue.Enqueue(output);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void SaveCollection(Folder folder)
        {
            try
            {
                SendOutput($@"Saving {folder.collectionName} collection in the Database");
                using (var mongo = new BaseLib.Class.MongoDBGridFS("eAIPProduction", folder.collectionName))
                {
                    GridFSMetaData metadata = new GridFSMetaData()
                    {
                        Username = CurrentDataContext.CurrentUser?.Name
                    };
                    if (folder.isFile)
                        mongo.UploadFileAsync(folder.folderPath, metadata.ToBsonDocument()).Wait();
                    else
                        mongo.UploadFolderAsync(folder.folderPath, metadata.ToBsonDocument()).Wait();
                    SendOutput($@"{folder.collectionName} collection has been successfully saved");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void GetCollection(Folder folder)
        {
            try
            {
                SendOutput($@"Receiving {folder.collectionName} collection from the Database");
                using (var mongo = new BaseLib.Class.MongoDBGridFS("eAIPProduction", folder.collectionName))
                {
                    mongo.DownloadFolderAsync(folder.folderPath, folder.savePath).Wait();
                    SendOutput($@"{folder.collectionName} collection has been successfully received");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public class Folder
        {
            public Folder(string folder, string collection, bool File=false, string saveFolder = null)
            {
                folderPath = folder;
                collectionName = collection;
                isFile = File;
                savePath = saveFolder;
            }
            public string folderPath;
            public string collectionName;
            public bool isFile;
            public string savePath;
        }
    }
}
