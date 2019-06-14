using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace AIP.BaseLib.Class
{
    public class MongoDBGridFS : IDisposable
    {
        private MongoClient client;
        private IMongoDatabase db;
        private IGridFSBucket gridFS;
        private string CurrentPath;
        private List<string> FileCollection = new List<string>();
        

        public MongoDBGridFS(string databaseName = "eAIPProduction", string tableName = null)
        {
            var connection = SingleConnection.String;
            client = new MongoClient(connection);
            db = client.GetDatabase(databaseName);
            var table = new GridFSBucketOptions()
            {
                BucketName = string.IsNullOrEmpty(tableName) ? "tmp" : tableName,
                //ChunkSizeBytes = 1048576, // 1MB
                //WriteConcern = WriteConcern.WMajority,
                //ReadPreference = ReadPreference.Secondary
            };
            gridFS = new GridFSBucket(db, table);
        }

        public bool TestConnection(string databaseName = "eAIPProduction")
        {
            try
            {
                var connection = SingleConnection.String;
                client = new MongoClient(connection);
                db = client.GetDatabase(databaseName);
                return db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                return false;
            }
        }

        public async Task UploadFileAsync(string file, BsonDocument BsonDocument = null)
        {
            try
            {
                var dbfile = Path.GetFileName(file);
                using (Stream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    ObjectId id = await gridFS.UploadFromStreamAsync(dbfile, fs,
                        new GridFSUploadOptions
                        {
                            Metadata = BsonDocument
                        }
                        ).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }
        

        public async Task UploadFolderAsync(string uploadFolder, BsonDocument BsonDocument = null)
        {
            try
            {
                FileCollection.Clear();
                FillFileCollection(uploadFolder);
                var topFolder = Path.GetDirectoryName(uploadFolder) + @"\";
                var lastFolder = uploadFolder.Replace(topFolder, "");
                // Remove old entries
                await RemoveFilesAsync(Builders<GridFSFileInfo>.Filter.Where(f => f.Filename.StartsWith(lastFolder)));
                

                foreach (var file in FileCollection)
                {
                    var dbfile = file.Replace(topFolder, "");
                    using (Stream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        ObjectId id = await gridFS.UploadFromStreamAsync(dbfile, fs,
                            new GridFSUploadOptions
                            {
                                Metadata = BsonDocument
                            }
                        ).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }

        public async Task FindFilesAsync(FilterDefinition<GridFSFileInfo> filter)
        {
            try
            {
                await gridFS.FindAsync(filter).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }


        public async Task RemoveFilesAsync(FilterDefinition<GridFSFileInfo> filter)
        {
            try
            {
                List<GridFSFileInfo> files = gridFS.FindAsync(filter)?.Result?.ToList();
                if (files != null)
                    foreach (var file in files)
                    {
                        await gridFS.DeleteAsync(file.Id);
                    }
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }

        // CurrentPath/eAIP-output/{EDString}/html/eAIP/{fileName}
        // CurrentPath/eAIP-source/{EDString}/html/eAIP/{fileName}
        public async Task DownloadFolderAsync(string downloadFolder, string saveFolder = null)
        {
            try
            {
                var topFolder = Path.GetDirectoryName(downloadFolder) + @"\";
                var lastFolder = downloadFolder.Replace(topFolder, "");
                var filter = Builders<GridFSFileInfo>.Filter.Where(f => f.Filename.StartsWith(lastFolder));
                var fileInfos = await gridFS.FindAsync(filter).ConfigureAwait(false);
                var fileInfo = fileInfos.ToList().Where(x => !x.Filename.Contains(".hash"));
                var destFolder = saveFolder ?? topFolder;
                foreach (var element in fileInfo)
                {
                    string path = Path.Combine(destFolder, element.Filename);
                    string dir = Path.GetDirectoryName(path);
                    if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    using (Stream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        await gridFS.DownloadToStreamAsync(element.Id, fs).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }
        
        private void FillFileCollection(string dir)
        {
            try
            {
                foreach (string f in Directory.GetFiles(dir)) FileCollection.Add(f);
                foreach (string d in Directory.GetDirectories(dir)) FillFileCollection(d);
            }
            catch (System.Exception ex)
            {
                Report.Write($@"Error in the {ex.TargetSite?.Name}{Environment.NewLine}{ex.GetBaseException()}");
                throw;
            }
        }

        public void Dispose()
        {
            client = null;
            db = null;
            gridFS = null;
            FileCollection.Clear();
            FileCollection = null;
        }
    }
}
