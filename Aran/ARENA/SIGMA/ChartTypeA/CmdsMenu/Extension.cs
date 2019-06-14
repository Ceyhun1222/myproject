using Aran.Package;
using ChartTypeA.Models;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChartTypeA.CmdsMenu
{
    [Guid("4d695db5-91fc-4185-b8fa-039ac31e7aa3")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ChartTypeA.CmdsMenu.TypeAExtension")]
    public class Extension : IExtension, IExtensionConfig, IPersistVariant
    {
        private esriExtensionState m_enableState;
        private IApplication m_application;
        private IDocumentEvents_Event m_docEvents;
        private MxDocument m_appDocument;
        private const int version = 1;

        public string ProductName => "Type A extension";

        public string Description => "This is Type A extension.For aligning grids";

        public string HeightUnit { get; set; }

        public esriExtensionState State
        {
            get
            {
                return m_enableState;
            }
            set
            {
                if (m_enableState != 0 && value == m_enableState)
                    return;

                //Check if ok to enable or disable extension
                esriExtensionState requestState = value;
                if (requestState == esriExtensionState.esriESEnabled)
                {
                    //Cannot enable if it's already in unavailable state
                    if (m_enableState == esriExtensionState.esriESUnavailable)
                        throw new COMException("Cannot enable extension");

                    //Determine if state can be changed
                    esriExtensionState checkState = StateCheck(true);
                    m_enableState = checkState;
                }
                else if (requestState == 0 || requestState == esriExtensionState.esriESDisabled)
                {
                    //Determine if state can be changed
                    esriExtensionState checkState = StateCheck(false);
                    if (checkState != m_enableState)
                        m_enableState = checkState;
                }
            }
        }

        public void Load(IVariantStream Stream)
        {
            try
            {
                int persistenceVersion = Convert.ToInt32(Stream.Read());
                if (persistenceVersion == 1)
                {
                    int type = Convert.ToInt32(Stream.Read());
                    int size = Convert.ToInt32(Stream.Read());
                    var buffer = new byte[size];
                    for (int i = 0; i < size; i++)
                    {
                        var b = Stream.Read();
                        buffer[i] = (byte) b;
                    }

                    if (type == 0)
                        GlobalParams.GrCreater = new GridCreater();
                    else
                        GlobalParams.GrCreater = new GridCreaterWithOffset();

                    using (var bpr = new BinaryPackageReader(buffer))
                    {
                        GlobalParams.GrCreater.Unpack(bpr);
                    }

                    HeightUnit = Stream.Read().ToString();
                    GlobalParams.TypeAExtension = this;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               // throw;
            }

            Marshal.ReleaseComObject(Stream);
        }

        public void Save(IVariantStream Stream)
        {
            Stream.Write(version);
            if (GlobalParams.GrCreater is GridCreater)
                Stream.Write(0);
            else
                Stream.Write(1);

            using (var ms = new MemoryStream())
            {
                var bpw = new BinaryPackageWriter(ms);
                GlobalParams.GrCreater.Pack(bpw);
                var buffer = ms.ToArray();

                Stream.Write(buffer.Length);
                foreach (var b in buffer)
                    Stream.Write(b);
            }

            Stream.Write(InitChartTypeA.HeightConverter.Unit);

            Marshal.ReleaseComObject(Stream);
        }

        public UID ID
        {
            get
            {
                UID id = new UIDClass();
                id.Value = GetType().GUID.ToString("B");
                return id;
            }
        }

        public void Shutdown()
        {
            m_application = null;
            m_appDocument = null;
        }

        public void Startup(ref object initializationData)
        {
            m_application = initializationData as IApplication;
            if (m_application == null)
                return;

            m_appDocument = m_application.Document as MxDocument;
            SetUpDocumentEvent(m_appDocument);


         //   Globals.Settings = new TypeBSettings();

           // UserName = Environment.UserName;
        }

        private esriExtensionState StateCheck(bool requestEnable)
        {
            if (requestEnable)
                return esriExtensionState.esriESEnabled;
            else
                return esriExtensionState.esriESDisabled;
        }

        private void SetUpDocumentEvent(IDocument myDocument)
        {
            m_docEvents = myDocument as IDocumentEvents_Event;
             m_docEvents.NewDocument += delegate() { OnNewDocument(); };
            //m_docEvents.NewDocument += delegate() { OnNewDocument(); }; 
            m_docEvents.OpenDocument += delegate () { OnOpenDocument(); };
            //m_docEvents.CloseDocument += new IDocumentEvents_CloseDocumentEventHandler(OnCloseDocument);
        }

        private void OnNewDocument()
        {
            
        }

        private void OnOpenDocument()
        {
            
        }

        public string Name => nameof(Extension);
    }
}
