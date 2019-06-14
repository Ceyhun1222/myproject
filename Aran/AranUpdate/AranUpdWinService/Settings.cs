using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AranUpdWinService
{
    public class Settings
    {
        private string _openedFileName;
        private string[] _keys;

        public Settings()
        {
            _keys = new string[] { "server", "port" };
            
            Server = string.Empty;
            Port = 0;
        }

        public void Open(string fileName)
        {
            _openedFileName = fileName;

            try
            {
                using (var sr = File.OpenText(fileName))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        for (int i = 0; i < _keys.Length; i++)
                        {
                            var s = _keys[i];
                            if (line.StartsWith(s + ":"))
                            {
                                var value = line.Substring(s.Length + 1).Trim();

                                switch (i)
                                {
                                    case 0:
                                        Server = value;
                                        break;
                                    case 1:
                                        {
                                            int x;
                                            if (int.TryParse(value, out x))
                                                Port = x;
                                            break;
                                        }
                                }

                                break;
                            }
                        }
                    }
                    sr.Close();
                }

                

            }
            catch { }
        }

        public void Save(string fileName = null)
        {
            if (fileName == null)
                fileName = _openedFileName;

            if (fileName == null)
                throw new Exception("File is not open!");

            var sb = new StringBuilder();
            sb.AppendFormat("{0}: {1}\r\n", _keys[0], Server);
            sb.AppendFormat("{0}: {1}\r\n", _keys[1], Port);

            File.WriteAllText(fileName, sb.ToString());
        }

        public string Server { get; set; }

        public int Port { get; set; }
    }
}
