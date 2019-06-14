using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Aran.Controls;
using Aran.Aim.InputForm;
using Aran.AranEnvironment;
using Aran.Package;

namespace MapEnv
{
    internal class Settings
    {
        #region ctor
        public Settings()
        {
            SetDefaults();
        }
        #endregion

        #region props
        public List<string> RecentProjectFiles { get; private set; }

        public bool ShowFeatureLoaderError { get; set; }

        public CoordinateFormat CoordinateFormat { get; set; }

        public int CoordinateFormatRound { get; set; }

        public void AddRecentProject(string fileName)
        {
            RecentProjectFiles.Remove(fileName);
            RecentProjectFiles.Insert(0, fileName);

            while (RecentProjectFiles.Count > 9)
                RecentProjectFiles.RemoveAt(RecentProjectFiles.Count - 1);
        }

        public Connection DbConnection { get; set; }

        public string[] UserNamePassword { get; private set; }

        public bool IsUserNamePasswordSaved
        {
            get { return (UserNamePassword != null); }
        }

        public string UserName
        {
            get { return UserNamePassword[0]; }
        }

        public string Password
        {
            get { return UserNamePassword[1]; }
        }

        public void SetUserNamePassword(string userName, string passwod)
        {
            if (userName == null)
                UserNamePassword = null;
            else
            {
                UserNamePassword = new string[2];
                UserNamePassword[0] = userName;
                UserNamePassword[1] = passwod;
            }
        }

        public string AirportRasterPath
        {
            get;
            set;
        }

        public FeatureInfoMode FeatureInfoMode { get; set; }

        public string MapEnvDocumentsDir { get; set; }

        public string ArcMapFileName { get; set; }

        public LogLevel LogLevel { get; set; }

        #endregion

        #region fields
        private XmlDocument _xmlDoc;
        private string _fileName;
        #endregion

        #region methods
        public void SetDefaults()
        {
            ShowFeatureLoaderError = true;
            RecentProjectFiles = new List<string>();
            CoordinateFormat = CoordinateFormat.DMS;
            CoordinateFormatRound = 4;
            FeatureInfoMode = FeatureInfoMode.Window;
            UserNamePassword = null;

            MapEnvDocumentsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\IAIM Documents";

            var s = Globals.ProgramFilesx86();
            ArcMapFileName = s + @"\ArcGIS\Desktop10.2\bin\ArcMap.exe";
            LogLevel = Aran.AranEnvironment.LogLevel.Debug;
        }

        public void Load(string fileName)
        {
            _fileName = fileName;
            _xmlDoc = new XmlDocument();

            if (File.Exists(fileName))
            {
                _xmlDoc.Load(fileName);
            }
            else
            {
                _xmlDoc.LoadXml(
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                    "<MapEnv>\n" +
                    "</MapEnv>");
            }

            LoadValues();
        }

        public void Save(string fileName = null)
        {
            if (fileName != null)
                _fileName = fileName;
            else if (_fileName == null)
                return;

            SetValues();
            _xmlDoc.Save(_fileName);
        }

        public void LoadValues()
        {
            XmlElement docElem = _xmlDoc.DocumentElement;
            XmlElement elem;

            #region OtherValues
            elem = docElem["OtherValues"];
            if (elem != null)
            {
                try
                {
                    string s = elem.Attributes["ShowFeatureLoaderError"].Value;
                    ShowFeatureLoaderError = bool.Parse(s);
                }
                catch { }

                var attr = elem.Attributes["CoordinateFormat"];
                if (attr != null)
                {
                    var sa = attr.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        CoordinateFormat = (CoordinateFormat)int.Parse(sa[0]);
                        CoordinateFormatRound = int.Parse(sa[1]);
                    }
                    catch { }
                }

                attr = elem.Attributes["FeatureInfoMode"];
                if (attr != null)
                {
                    try { FeatureInfoMode = (FeatureInfoMode)int.Parse(attr.Value); }
                    catch { }
                }

                attr = elem.Attributes["UserName"];
                var attr1 = elem.Attributes["Password"];
                if (attr != null && attr1 != null && attr.Value != "" && attr1.Value != "")
                {
                    SetUserNamePassword(attr.Value, attr1.Value);
                }

                attr = elem.Attributes["ArcMapFileName"];
                if (attr != null)
                {
                    try { ArcMapFileName = attr.Value; }
                    catch { }
                }

                attr = elem.Attributes["LogLevel"];
                if (attr != null)
                {
                    LogLevel ll;
                    if (Enum.TryParse<LogLevel>(attr.Value, out ll))
                        LogLevel = ll;
                }
            }

            #endregion

            #region Recent Projects

            RecentProjectFiles.Clear();

            elem = docElem["RecentProjects"];
            if (elem != null)
            {
                try
                {
                    foreach (XmlNode node in elem.ChildNodes)
                    {
                        if (node.NodeType == XmlNodeType.Element)
                        {
                            XmlElement proElem = node as XmlElement;
                            if (File.Exists(proElem.InnerText))
                                RecentProjectFiles.Add(proElem.InnerText);
                        }
                    }
                }
                catch { }
            }
            #endregion

            #region Database Connection
            elem = docElem["DatabaseConnection"];
            if (elem != null)
            {
                DbConnection = new Connection();

                #region ConnectionType

#if TDB
                DbConnection.ConnectionType = ConnectionType.TDB;
#elif COMSOFTDB
                DbConnection.ConnectionType = ConnectionType.ComSoft;
#endif

                #endregion

                try
                {
                    XmlElement childElement = elem["Server"];
                    if (childElement != null)
                    {
                        DbConnection.Server = childElement.InnerText;
                    }

                    childElement = elem["Port"];
                    if (childElement != null)
                    {
                        DbConnection.Port = int.Parse(childElement.InnerText);
                    }

                    childElement = elem["DatabaseName"];
                    if (childElement != null)
                    {
                        DbConnection.Database = childElement.InnerText;
                    }
                }
                catch
                {
                }
            }
            #endregion

            elem = docElem["AerodromRasterPath"];
            if (elem != null)
            {
                AirportRasterPath = elem.InnerText;
            }
        }

        public void SetValues()
        {
            XmlElement docElem = _xmlDoc.DocumentElement;
            XmlElement elem;

            #region OtherValues

            elem = GetOrCreateElement("OtherValues");
            elem.SetAttribute("ShowFeatureLoaderError", ShowFeatureLoaderError.ToString());

            string coordFormat = (int)CoordinateFormat + ";" + CoordinateFormatRound;
            elem.SetAttribute("CoordinateFormat", coordFormat);

            elem.SetAttribute("FeatureInfoMode", ((int)FeatureInfoMode).ToString());

            elem.SetAttribute("UserName", (UserNamePassword == null ? "" : UserNamePassword[0]));
            elem.SetAttribute("Password", (UserNamePassword == null ? "" : UserNamePassword[1]));

            elem.SetAttribute("ArcMapFileName", ArcMapFileName);

            elem.SetAttribute("LogLevel", LogLevel.ToString());

            #endregion

            #region Recent Projects
            elem = GetOrCreateElement("RecentProjects");
            elem.RemoveAll();
            foreach (string fileName in RecentProjectFiles)
            {
                var childElem = _xmlDoc.CreateElement("Pro");
                childElem.InnerText = fileName;
                elem.AppendChild(childElem);
            }
            #endregion

            #region Database Connection
            if (DbConnection != null && DbConnection.ConnectionType == ConnectionType.Aran)
            {
                elem = GetOrCreateElement("DatabaseConnection");
                elem.RemoveAll();

                XmlElement childElement = GetOrCreateElement("Server", elem);
                childElement.InnerText = DbConnection.Server;

                childElement = GetOrCreateElement("Port", elem);
                childElement.InnerText = DbConnection.Port.ToString();

                childElement = GetOrCreateElement("DatabaseName", elem);
                childElement.InnerText = DbConnection.Database;
            }
            #endregion

            elem = GetOrCreateElement("AerodromRasterPath");
            elem.InnerText = AirportRasterPath;
        }

        private XmlElement GetOrCreateElement(string elemName, XmlElement parentElem = null)
        {
            if (parentElem == null)
                parentElem = _xmlDoc.DocumentElement;

            var elem = parentElem[elemName];
            if (elem != null)
                return elem;

            elem = _xmlDoc.CreateElement(elemName);
            parentElem.AppendChild(elem);
            return elem;
        }
        #endregion
    }

    public enum FeatureInfoMode { Window, Popup }
}
