using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;
using System.IO;
using Aran.Package;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace Aran.Omega.TypeBEsri
{
    [Guid("ab2e5510-995a-435d-ab74-6176981fb1dd")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Omega.TypeBEsri.TybebExtension")]
    public class TybeBExtension : IExtension, IPersistVariant, IExtensionConfig
    {
        public TybeBExtension()
        {
            dictionary = new Dictionary<string, Stream>();
        }

        private IApplication m_application;
        private IDocumentEvents_Event m_docEvents;
        private MxDocument m_appDocument;
        private esriExtensionState m_enableState;
        private Dictionary<string, Stream> dictionary;
        //private MemoryStream ms = new MemoryStream();
        private int PersistenceVersion = 0;

        private const int persistenceVersion1 = 1; // if anything changes concerning persistence, increment this attribute
        private const int persistenceVersion2 = 2;
        private const int CurrentPersistenceVersion = persistenceVersion1;

        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Unregister(regKey);

        }

        #endregion
        #endregion

        #region IExtension Members
        #region IExtension Members

        /// <summary>
        /// Name of extension. Do not exceed 31 characters
        /// </summary>
        public string Name
        {
            get
            {
                //TODO: Modify string to uniquely identify extension
                return "ApplicationExtension1";
            }
        }

        public void Shutdown()
        {
            //TODO: Clean up resources

            m_application = null;
        }

        public void Startup(ref object initializationData)
        {
            m_application = initializationData as IApplication;
            if (m_application == null)
                return;

            m_appDocument = m_application.Document as MxDocument;
            SetUpDocumentEvent(m_appDocument);
            //TODO: Add code to initialize the extension
        }

        private void SetUpDocumentEvent(IDocument myDocument)
        {
            m_docEvents = myDocument as IDocumentEvents_Event;
            m_docEvents.NewDocument += delegate() { ArcMap_NewDocument(); };
            //m_docEvents.NewDocument += delegate() { OnNewDocument(); }; 
            m_docEvents.OpenDocument += delegate { ArcMap_OpenDocument(); };
            m_docEvents.CloseDocument += delegate { ArcMap_CloseDocument(); };
        }

        #endregion

        public UID ID
        {
            get
            {
                UID typeID = new UIDClass();
                typeID.Value = GetType().GUID.ToString("test");
                return typeID;
            }
        }

        private void LoadProjectDataV1(IVariantStream Stream)
        {
            int i, count = Convert.ToInt32(Stream.Read());

            for (i = 0; i < count; i++)
            {
                string key = Convert.ToString(Stream.Read());

                int l = (int)Stream.Read();

                byte[] bArray = new byte[l];
                for (l = 0; l < bArray.Length; l++)
                    bArray[l] = (byte)Stream.Read();

                MemoryStream ms = new MemoryStream(bArray);
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key].Dispose();
                    dictionary[key] = ms;
                }
                else
                    dictionary.Add(key, ms);
            }
        }

        public void Load(IVariantStream Stream)
        {
            ESRI.ArcGIS.Framework.IMessageDialog msgBox = new ESRI.ArcGIS.Framework.MessageDialogClass();
            msgBox.DoModal("BeforeCloseDocument Event", "Abort closing?", "Yes", "No", m_application.hWnd);

            int PersistenceVersion = Convert.ToInt32(Stream.Read());

            if (PersistenceVersion == persistenceVersion1)
                LoadProjectDataV1(Stream);

            Marshal.ReleaseComObject(Stream);

            if (dictionary.ContainsKey("Project"))
            {
                var data = new Data();

                //Aran.Settings.Settings settings = new Aran.Settings.Settings(DateTime.Now);
                IPackable settingsPackable = (data as IPackable);
                GetData("Project", ref settingsPackable);
                data = settingsPackable as Data;
            }
        }

        public void GetData(string key, ref IPackable value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Position = 0;
                PackageReader reader = new BinaryPackageReader(dictionary[key]);
                value.Unpack(reader);
            }
        }

        private void SaveProjectData(IVariantStream Stream)
        {
            Stream.Write(dictionary.Count);

            foreach (KeyValuePair<string, Stream> kvp in dictionary)
            {
                Stream.Write(kvp.Key);

                byte[] bArray = new byte[kvp.Value.Length];
                kvp.Value.Position = 0;
                kvp.Value.Read(bArray, 0, bArray.Length);

                Stream.Write(bArray.Length);
                for (int i = 0; i < bArray.Length; i++)
                    Stream.Write(bArray[i]);
            }
        }

        public void Save(IVariantStream Stream)
        {
            PersistenceVersion = 1;
            if (PersistenceVersion > 0)
            {
                Stream.Write(PersistenceVersion);
                SaveProjectData(Stream);
            }

            Marshal.ReleaseComObject(Stream);
        }


        private void ArcMap_BeforeCloseDocument()
        {
            //throw new NotImplementedException();
        }

        private void ArcMap_CloseDocument()
        {
            if (dictionary.ContainsKey("Project"))
            {
                ESRI.ArcGIS.Carto.IMap map = m_appDocument as IMap;//ArcMap.Document.FocusMap;
                IFeatureCursor featureCursor;
                IFeature feature;
                for (int i = 0; i < map.LayerCount; i++)
                {
                    featureCursor = (map.Layer[i] as IFeatureLayer).FeatureClass.Search(null, false);
                    while ((feature = featureCursor.NextFeature()) != null)
                    {
                        feature.Delete();
                    }
                }
            }
        }

        private void ArcMap_OpenDocument()
        {
            MessageBox.Show("Open document");

            ICommandItem pCmdItem;
            ICommandBar pMenuBar;

            UID pMenuBarUID = new UID();
            UID pPANDANewUID = new UID();
            UID pPANDAPropertiesUID = new UID();
            pMenuBar = null;

            try
            {
                pMenuBarUID.Value = "esriArcMapUI.MxFileMenu";
                pPANDANewUID.Value = "Aran.Settings.NewButton";
                pPANDAPropertiesUID.Value = "Aran.Settings.PropButton";
                pMenuBar = m_application.Document.CommandBars.Find(pMenuBarUID, false, false) as ICommandBar;
            }
            catch
            {
            }

            if (pMenuBar != null)
            {
                object index;
                pCmdItem = pMenuBar.Find(pPANDANewUID, false);
                UID uid = new UID();
                uid.Value = "esriArcMapUI.MxFileMenuItem";
                ICommandItem cmdItem;
                if (pCmdItem == null)
                {
                    uid.SubType = 1;
                    cmdItem = m_application.Document.CommandBars.Find(uid, false, false) as ICommandItem;
                    index = cmdItem.Index + 1;
                    pMenuBar.Add(pPANDANewUID, ref index);
                }
                pCmdItem = pMenuBar.Find(pPANDAPropertiesUID, false);
                if (pCmdItem == null)
                {
                    uid.SubType = 8;
                    cmdItem = m_application.Document.CommandBars.Find(uid, false, false) as ICommandItem;
                    index = cmdItem.Index + 1;
                    pMenuBar.Add(pPANDAPropertiesUID, ref index);
                }
            }
            pMenuBarUID = null;
            pPANDANewUID = null;
        }

        void ArcMap_NewDocument()
        {
            MessageBox.Show("New typeb");
            ICommandItem pCmdItem;
            ICommandBar pMenuBar;

            // dictionary.Clear();
            UID pMenuBarUID = new UID();
            UID pPANDANewUID = new UID();
            UID pPANDAPropertiesUID = new UID();
            pMenuBar = null;
            try
            {
                pMenuBarUID.Value = "esriArcMapUI.MxFileMenu";
                pPANDANewUID.Value = "Aran.Settings.NewButton";
                pPANDAPropertiesUID.Value = "Aran.Settings.PropButton";
                pMenuBar = m_application.Document.CommandBars.Find(pMenuBarUID, false, false) as ICommandBar;
            }
            catch
            {
            }

            if (pMenuBar != null)
            {
                object index;
                pCmdItem = pMenuBar.Find(pPANDANewUID, false);
                UID uid = new UID();
                uid.Value = "esriArcMapUI.MxFileMenuItem";
                ICommandItem cmdItem;
                if (pCmdItem == null)
                {
                    uid.SubType = 1;
                    cmdItem = m_application.Document.CommandBars.Find(uid, false, false) as ICommandItem;
                    index = cmdItem.Index + 1;
                    pMenuBar.Add(pPANDANewUID, ref index);
                }
                pCmdItem = pMenuBar.Find(pPANDAPropertiesUID, false);
                if (pCmdItem == null)
                {
                    uid.SubType = 8;
                    cmdItem = m_application.Document.CommandBars.Find(uid, false, false) as ICommandItem;
                    index = cmdItem.Index + 1;
                    pMenuBar.Add(pPANDAPropertiesUID, ref index);
                }
            }
            pMenuBarUID = null;
            pPANDANewUID = null;
        }
        #endregion

        public string Description
        {
            get { return "TypeB"; }
        }

        public string ProductName
        {
            get { return "ChartTypeB"; }
        }

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
        private esriExtensionState StateCheck(bool requestEnable)
        {
            if (requestEnable)
                return esriExtensionState.esriESEnabled;
            else
                return esriExtensionState.esriESDisabled;
        }
    }

}
