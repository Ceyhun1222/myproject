using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Metadata
{
    public class ExtensionDataProvider
    {
        private readonly string _path;

        string _filename;

        public ExtensionDataProvider(string path, string filename)
        {
            this._path = path;
            this._filename = filename;
        }

        public ExtensionData GetExtensionData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            if(!File.Exists(Path.GetFullPath(this._path) + "\\" + _filename))
            {
                ExtensionData res = new ExtensionData();
                res.Initialize();
                return res;
            }
            string textReader = File.ReadAllText(Path.GetFullPath(this._path) + "\\" + _filename);
            ExtensionData result = JsonConvert.DeserializeObject<ExtensionData>(textReader, settings);

            return result;
        }

        public void CommitExtensionData(ExtensionData extensionData)
        {
            if (!Directory.Exists(this._path))
                Directory.CreateDirectory(this._path);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
          
            string json = JsonConvert.SerializeObject(extensionData, Formatting.Indented, settings);
            File.WriteAllText(Path.GetFullPath(this._path) + "\\" + _filename, json);

        }
    }
}
