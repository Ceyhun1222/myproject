using ChartManager;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using System;
using System.Runtime.InteropServices;

namespace ChartManagerTools
{
    [Guid("9E6C12E9-A525-40ED-8070-2A0AC861FD8F")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ChartManagerTools.ChartManagerExtension")]
    public class ChartManagerExtension : IExtension, IPersistVariant
    {
        #region COM Registration Function(s)

        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);
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

        private IApplication _app;

        //private ESRI.ArcGIS.Carto.IActiveViewEvents_ViewRefreshedEventHandler m_ActiveViewEventsViewRefreshed;
        IDocumentEvents_Event m_docEvents;
        private readonly int _extensionVersion = 2;

        #region IExtension Members

        public void Shutdown()
        {
            _app = null;
            GlobalController.CloseConnection();
        }

        public void Startup(ref object initializationData)
        {
            _app = initializationData as IApplication;
            if (_app == null)
                return;
            m_docEvents = _app.Document as IDocumentEvents_Event;
            m_docEvents.CloseDocument += OnCloseDocument;
        }

        /// <summary>
        /// Name of extension. Do not exceed 31 characters
        /// </summary>
        public string Name => nameof(ChartManagerExtension);

        public void Load(IVariantStream Stream)
        {
            var extensionVersion = (int)Stream.Read();
            EsriExtensionData.Id = new Guid(Stream.Read().ToString());
            EsriExtensionData.EffectiveDate = (DateTime)Stream.Read();
            EsriExtensionData.ChartVersion = (int)Stream.Read();
            EsriExtensionData.IsReadOnly = (bool)Stream.Read();
            if (extensionVersion >= 2)
            {
                var hasUpdate = (bool)Stream.Read();
                if (hasUpdate)
                    EsriExtensionData.UpdateId = (int)Stream.Read();
            }
            Marshal.ReleaseComObject(Stream);
        }

        public void Save(IVariantStream Stream)
        {
            Stream.Write(_extensionVersion);
            Stream.Write(EsriExtensionData.Id.ToString());
            Stream.Write(EsriExtensionData.EffectiveDate);
            Stream.Write(EsriExtensionData.ChartVersion);
            Stream.Write(EsriExtensionData.IsReadOnly);
            var hasUpdate = EsriExtensionData.HasUpdate;
            Stream.Write(hasUpdate);
            if (hasUpdate)
                Stream.Write(EsriExtensionData.UpdateId);
            Marshal.ReleaseComObject(Stream);
        }

        public UID ID
        {
            get
            {
                UID typeId = new UIDClass();
                typeId.Value = GetType().GUID.ToString("B");
                return typeId;
            }
        }

        #endregion

        void OnCloseDocument()
        {
            if (!EsriExtensionData.HasUpdate)
                EsriExtensionData.Clear();
        }
    }
}