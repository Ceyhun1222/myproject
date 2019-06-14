using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Aran.Controls;
using Aran.Aim.Package;
using Aran.AranEnvironment;

namespace Aran.Aim.InputForm
{
    internal class Settings
    {
        public Settings ()
        {
            SetDefaults ();
        }

        public void SetDefaults ()
        {
            EffectiveDate = DateTime.Now;
        }

        public void Load (string fileName)
        {
            _fileName = fileName;
            _xmlDoc = new XmlDocument ();

            if (File.Exists (fileName))
            {
                _xmlDoc.Load (fileName);
            }
            else
            {
                _xmlDoc.LoadXml (
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                    "<MapEnv>\n" +
                    "</MapEnv>");
            }

            LoadValues ();
        }

        public void Save (string fileName = null)
        {
            if (fileName != null)
                _fileName = fileName;
            else if (_fileName == null)
                return;

            SetValues ();
            _xmlDoc.Save (_fileName);
        }


        public void LoadValues ()
        {
            string connectionData = string.Empty;

            XmlElement docElem = _xmlDoc.DocumentElement;
            XmlElement elem;

            #region OtherValues
            elem = docElem ["OtherValues"];
            if (elem != null)
            {
                try
                {
                    connectionData = elem.Attributes ["ConnectionData"].Value;
                    EffectiveDate = DateTime.Parse(elem.Attributes["EffectiveDate"].Value);
                }
                catch { }
            }
            #endregion

            if (!string.IsNullOrWhiteSpace(connectionData))
                Connection = StringToConnection(connectionData);

            #region FormSettings
            elem = docElem["FormSettings"];
            if (elem != null)
            {
                try
                {
                    if (elem["Name"] != null)
                        FormSettings.Name = elem["Name"].InnerText;
                    if (elem["Height"] != null)
                        FormSettings.Height = Int32.Parse(elem["Height"].InnerText);
                    if (elem["Width"] != null)
                        FormSettings.Width = Int32.Parse(elem["Width"].InnerText);
                }
                catch { }
            }
            #endregion


        }

        public void SetValues ()
        {
            var connectionData = string.Empty;
            if (Connection != null)
                connectionData = ConnectionToString (Connection);

            XmlElement docElem = _xmlDoc.DocumentElement;
            XmlElement elem;

            #region OtherValues
            
            elem = GetOrCreateElement ("OtherValues");
            elem.SetAttribute ("ConnectionData", connectionData);
            elem.SetAttribute("EffectiveDate", EffectiveDate.ToString("yyyy-MM-dd"));
            
            elem = GetOrCreateElement("FormSettings");
            GetOrCreateElement("Name", elem).InnerText = FormSettings.Name;
            GetOrCreateElement("Height", elem).InnerText = FormSettings.Height.ToString();
            GetOrCreateElement("Width", elem).InnerText = FormSettings.Width.ToString();

            #endregion
        }
        

        public Connection Connection { get; set; }

        public DateTime EffectiveDate { get; set; }
        public FormSettings FormSettings { get; set; } = new FormSettings();

        
        private static string ConnectionToString (Connection conn)
        {
            MemoryStream ms = new MemoryStream ();
            AranPackageWriter apw = new AranPackageWriter (ms);
            conn.Pack (apw);

            var buffer = ms.ToArray ();
            string s = UTF8Encoding.UTF8.GetString (buffer);

            ms.Close ();
            ms.Dispose ();

            return s;
        }

        private static Connection StringToConnection (string text)
        {
            var buffer = UTF8Encoding.UTF8.GetBytes (text);
            AranPackageReader apr = new AranPackageReader (buffer);

            Connection conn = new Connection ();
            conn.Unpack (apr);

            apr.Dispose ();

            return conn;
        }


        private XmlElement GetOrCreateElement (string elemName, XmlElement parentElem = null)
        {
            if (parentElem == null)
                parentElem = _xmlDoc.DocumentElement;

            var elem = parentElem [elemName];
            if (elem != null)
                return elem;

            elem = _xmlDoc.CreateElement (elemName);
            parentElem.AppendChild (elem);
            return elem;
        }

        private XmlDocument _xmlDoc;
        private string _fileName;
    }

    public enum CoordinateFormat
    {
        DMS,
        DD
    }

    internal class FormSettings
    {
        public string Name { get; set; } 
        public int Height { get; set; }
        public int Width { get; set; }

    }
}

