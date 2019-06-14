using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;
using Encryptor;

namespace ARENA
{
    /// <summary>
    /// Summary description for ArenaToolbar.
    /// </summary>
    [Guid("18f3ade5-efbd-439a-a9c2-7ae9e2e220b9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Arena.ArenaToolbar")]
    public sealed class ArenaToolbar : BaseToolbar
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
            MxCommandBars.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommandBars.Unregister(regKey);
        }

        #endregion
        #endregion

        public ArenaToolbar()
        {
            //
            // TODO: Define your toolbar here by adding items
            //
            if (DateTime.Now > SigmaEncryptor.GetEncryptedDate()) return;


            AddItem("Arena.ToolbarComandsMenu.ArenaProjectMenu");
            AddItem("Arena.ToolbarComandsMenu.ArenaDataMenu");
            BeginGroup(); //Separator
            AddItem("{4e0552c8-ee58-4dda-b6be-c4eb6b9dd690}");
            //AddItem("{FBF8C3FB-0480-11D2-8D21-080009EE4E51}", 1); //undo command
            //AddItem(new Guid("FBF8C3FB-0480-11D2-8D21-080009EE4E51"), 2); //redo command
            //ArenaTag = "ff";
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Arena toolbar";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "ArenaToolbar";
            }
        }


    }
}