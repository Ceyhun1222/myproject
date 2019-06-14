using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AranUpdateManager
{
    class Settings
    {
        private string _fileName;

        public Settings()
        {

        }

        public void Load(string fileName)
        {
            _fileName = fileName;

            if (!File.Exists(fileName))
                return;

            try
            {
                var lines = File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    if (line.ToLower().StartsWith("server:"))
                        Server = line.Substring(7).Trim();
                    else if (line.ToLower().StartsWith("port:"))
                        Port = int.Parse(line.Substring(5).Trim());
                }
            }
            catch { }
        }

        public void Save(string fileName = null)
        {
            if (fileName == null)
                fileName = _fileName;

            var dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(fileName, string.Format("server: {0}\r\nport: {1}", Server, Port));

        }

        public string Server { get; set; }

        public int Port { get; set; }
    }
}
