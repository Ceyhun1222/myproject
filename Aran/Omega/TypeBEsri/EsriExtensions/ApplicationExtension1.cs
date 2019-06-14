using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;

namespace Aran.Omega.TypeBEsri
{
    [Guid("f348e869-a690-4b01-8ba7-c021e0af2c6c")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Omega.TypeBEsri.ApplicationExtension1")]
    public class ApplicationExtension1 : IExtension, IExtensionConfig, IPersistVariant
    {
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
        private IApplication m_application;
        private esriExtensionState m_enableState;
        private string m_value1;
        private int m_value2;

        #region IExtension Members

        /// <summary>
        /// Name of extension. Do not exceed 31 characters
        /// </summary>
        public string Name
        {
            get
            {
                //TODO: Modify string to uniquely identify extension
                return "HowToExtension";
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

            //TODO: Add code to initialize the extension
        }

        #endregion

        #region IExtensionConfig Members

        public string Description
        {
            get
            {
                //TODO: Replace description (use \r\n for line break)
                return "ApplicationExtension1\r\n" +
                    "Copyright © Microsoft 2014\r\n\r\n" +
                    "This extension is created from a .Net template.";
            }
        }

        /// <summary>
        /// Friendly name shown in the Extension dialog
        /// </summary>
        public string ProductName
        {
            get
            {
                //TODO: Replace
                return "How to extension";
            }
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
                    {
                        throw new COMException("Cannot enable extension");
                    }

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

        #endregion

        /// <summary>
        /// Determine extension state 
        /// </summary>
        /// <param name="requestEnable">true if to enable; false to disable</param>
        private esriExtensionState StateCheck(bool requestEnable)
        {
            //TODO: Replace with advanced extension state checking if needed
            //Turn on or off extension directly
            if (requestEnable)
                return esriExtensionState.esriESEnabled;
            else
                return esriExtensionState.esriESDisabled;
        }

        #region IPersistVariant Members

        public UID ID
        {
            get
            {
                UID typeID = new UIDClass();
                typeID.Value = GetType().GUID.ToString("Bad");
                return typeID;
            }
        }

        public void Load(IVariantStream Stream)
        {
            //TODO: Load persisted data from document stream
            m_value1 = Convert.ToString(Stream.Read());
            m_value2 = Convert.ToInt32(Stream.Read());
            Marshal.ReleaseComObject(Stream);
        }

        public void Save(IVariantStream Stream)
        {
            //TODO: Save extension related data to document stream
            m_value1 = "asdfasdf";
            m_value2 = 5;

            Stream.Write(m_value1);
            Stream.Write(m_value2);
            Marshal.ReleaseComObject(Stream);
        }

        #endregion
    }
}