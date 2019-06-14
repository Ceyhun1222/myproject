using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Aran.AranEnvironment
{
    public interface IScreenCapture
    {
        byte[] Commit(Guid uuid);
        void Rollback();
        void Delete();
        void Delete(Guid uuid);
        void Save(Guid uuid, IntPtr hWnd);
        void Save(Guid uuid, Form form);
        void Save(Guid uuid);
        void Save(IntPtr hWnd);
        void Save(Form form);
        void Save();
        List<Capture> Get(Guid uuid);
    }

    [XmlRoot("ScreenCapture")]
    public class Captures
    {

        public Captures()
        {
            CaptureList = new List<Capture>();
        }

        [XmlElement("Capture")]
        public List<Capture> CaptureList { get; set; }
    }

    public class Capture
    {
        public Capture()
        {
            Images = new List<string>();
        }

        [XmlElement("uuid")]
        public string uuid;

        [XmlElement("date")]
        public DateTime date;

        [XmlElement("Images")]
        public List<string> Images { get; set; }

        [XmlIgnore]
        public byte[] Zip { get; set; }
    }
}