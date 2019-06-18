﻿using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AerodromeToolBox
{
    [Guid("F6B8EE64-18AD-48C9-8BFF-52391164C8F3")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeProjectMenu")]
    public sealed class AerodromeProjectMenu : BaseMenu, IRootLevelMenu
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

        public AerodromeProjectMenu()
        {
            //
            // TODO: Define your menu here by adding items
            ////

            AddItem("Aerodrome.AerodromeNewProjectMenu");
            AddItem("Aerodrome.AerodromeOpenProject");
            AddItem("Aerodrome.AerodromeSaveProject");
            AddItem("Aerodrome.AerodromeOpenUI");
            BeginGroup(); //Separator
            AddItem("Aerodrome.AerodromeGenerateNetwork");
            BeginGroup(); //Separator
            AddItem("Aerodrome.AerodromeExportMdb");
            BeginGroup(); //Separator
            AddItem("Aerodrome.AerodromeReportMenu");
            


        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Aerodrome Project";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "AerodromeProjectMenu";
            }
        }


    }
}