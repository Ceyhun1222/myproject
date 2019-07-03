using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;

namespace CommonUtils
{
    public static class Config
    {
        static Config()
        {
            FileName = "config.xml";
        }

        public static T ReadConfig<T>(string folder, string name, T defaultValue)
        {
            var dir1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fileName = Path.Combine(dir1, folder, FileName);
            
            if (!File.Exists(fileName))
                return defaultValue;

            try
            {
                var doc = new XmlDocument();
                doc.Load(fileName);

                var elem = GetElement(doc, name);

                if (elem == null)
                    return defaultValue;

                if (elem.InnerText == null)
                    return defaultValue;

                var retValObj = Convert.ChangeType(elem.InnerText.Trim(), typeof(T));
                if (retValObj == null)
                    return defaultValue;

                return (T)retValObj;
            }
            catch { return defaultValue; }
        }

        //public static T ReadConfig<T>(string folder, string name, T defaultValue)
        //{
        //    string fileName = null;

        //    var dir1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    var fileName1 = Path.Combine(dir1, folder, FileName);

        //    var dir2 = Path.GetDirectoryName(typeof(Config).Assembly.Location);
        //    var fileName2 = Path.Combine(dir2, folder, FileName);

        //    if (!File.Exists(fileName1))
        //    {
        //        if (!File.Exists(fileName2))
        //            return defaultValue;

        //        fileName = fileName2;
        //    }
        //    else
        //    {
        //        if (!File.Exists(fileName2))
        //        {
        //            fileName = fileName1;
        //        }
        //        else
        //        {
        //            var dt1 = (new FileInfo(fileName1)).LastWriteTime;
        //            var dt2 = (new FileInfo(fileName2)).LastWriteTime;
        //            fileName = (dt1 > dt2 ? fileName1 : fileName2);
        //        }
        //    }

        //    var doc = new XmlDocument();
        //    doc.Load(fileName);

        //    var elem = GetElement(doc, name);

        //    if (elem == null)
        //        return defaultValue;

        //    if (elem.InnerText == null)
        //        return defaultValue;

        //    var retValObj = Convert.ChangeType(elem.InnerText.Trim(), typeof(T));
        //    if (retValObj == null)
        //        return defaultValue;

        //    return (T)retValObj;
        //}

        public static void WriteConfig(string folder, string name, object value)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //var dir = Environment.GetEnvironmentVariable("appdata");
            var fileName = Path.Combine(dir, folder, FileName);

            var doc = new XmlDocument();
            if (File.Exists(fileName))
            {
                doc.Load(fileName);
            }
            else
            {
                doc.LoadXml(
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                    "<Config>\n" +
                    "</Config>");
            }

            var elem = GetOrAddElement(doc, name);
            elem.InnerText = value.ToString();

            if (!File.Exists(fileName))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            doc.Save(fileName);
        }

        public static void WriteConfigToFolder(string filePath, string name, object value)
        {
            var doc = new XmlDocument();
            if (File.Exists(filePath))
            {
                doc.Load(filePath);
            }
            else
            {
                doc.LoadXml(
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                    "<Config>\n" +
                    "</Config>");
            }

            var elem = GetOrAddElement(doc, name);
            elem.InnerText = value.ToString();

            if (!File.Exists(filePath))
            {
                var dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }

            doc.Save(filePath);
        }

        public static string FileName { get; set; }



        private static XmlElement GetElement(XmlDocument doc, string path)
        {
            var sa = path.Split("/\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var elem = doc.DocumentElement;

            foreach (var s in sa)
            {
                var childElem = elem[s];
                if (childElem == null)
                    return null;
                elem = childElem;
            }

            return elem;
        }

        private static XmlElement GetOrAddElement(XmlDocument doc, string path)
        {
            var sa = path.Split("/\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var elem = doc.DocumentElement;

            foreach (var s in sa)
            {
                var childElem = elem[s];
                if (childElem == null)
                {
                    childElem = doc.CreateElement(s);
                    elem.AppendChild(childElem);
                }
                elem = childElem;
            }

            return elem;
        }
    }
	
}