// Copyright 2008 ESRI
// 
// All rights reserved under the copyright laws of the United States
// and applicable international laws, treaties, and conventions.
// 
// You may freely redistribute and use this sample code, with or
// without modification, provided you include the original copyright
// notice and use restrictions.
// 
// See use restrictions at <your ArcGIS install location>/developerkit/userestrictions.txt.
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Framework;
using Microsoft.Win32;
using System.IO;

namespace ARENA
{
    [Guid("8e5372ed-a185-4b52-9a2a-76f359107c58")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RecentFilesCommandsCS.MultiItemRecentFiles")]
    public class MultiItemRecentFiles : IMultiItem
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
            GMxCommands.Register(regKey);
            MxCommands.Register(regKey);
            SxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GMxCommands.Unregister(regKey);
            MxCommands.Unregister(regKey);
            SxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IApplication m_application;
        private string[] m_filePaths = null;

        #region "IMultiItem Implementations"
        public string Caption
        {
            get
            {
                return "Recent Arena Files MultiItem";
            }
        }

        public int HelpContextID
        {
            get
            {
                return default(int);
            }
        }

        public string HelpFile
        {
            get
            {
                return default(string);
            }
        }

        public string Message
        {
            get
            {
                return default(string);
            }
        }

        public string Name
        {
            get
            {
                return "RecentArenaFilesMultiItem";
            }
        }

        public void OnItemClick(int index)
        {
            ArenaProjectOpener.OpenSelectedFile(m_filePaths[index], m_application);
            //m_application.OpenDocument(m_filePaths[index]);
        }

        public int OnPopup(object hook)
        {
            //The incoming hook is the application, determine the number of
            //items by getting the recent files from the registry
            m_application = hook as IApplication;
            m_filePaths = RecentFilesHelper.GetRecentFiles_Pdm();//RecentFilesHelper.GetRecentFiles_Mxd(m_application);
            return m_filePaths.Length;
        }

        public int get_ItemBitmap(int index)
        {
            return default(int);
        }

        public string get_ItemCaption(int index)
        {
            //Caption of item is "#. <File path>"
            return string.Format("&{0}. {1}", index + 1, m_filePaths[index]);
        }

        public bool get_ItemChecked(int index)
        {
            return false;
        }

        public bool get_ItemEnabled(int index)
        {
            //check if file exists
            return File.Exists(m_filePaths[index]);
        }
        #endregion

    }
}
