using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace Aran.Delta.Classes
{
    /// <summary>
    /// Summary description for DeltaMenu.
    /// </summary>
    [Guid("dc2ab9c6-c950-407a-85f6-8f273266a738")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aran.Delta.Classes.DeltaMenu")]
    public sealed class DeltaMenu : BaseMenu
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

        public DeltaMenu()
        {
            //
            // TODO: Define your menu here by adding items
            //
            //AddItem("esriArcMapUI.ZoomInFixedCommand");
            //BeginGroup(); //Separator
            //AddItem("{FBF8C3FB-0480-11D2-8D21-080009EE4E51}", 1); //undo command
            //AddItem(new Guid("FBF8C3FB-0480-11D2-8D21-080009EE4E51"), 2); //redo command

            AddItem("Aran.Delta.EsriClasses.Point_Creation");
            AddItem("Aran.Delta.EsriClasses.Buffer");
            AddItem("Aran.Delta.Create");
            AddItem("Aran.Delta.Classes.IntersectionMenu");
            AddItem("Aran.Delta.Classes.VisibilityClass");
            AddItem("Aran.Delta.Classes.ArenaSettings");
            AddItem("Aran.Delta.Classes.LogCommand");
            AddItem("Aran.Delta.Classes.HelpCommand");
            AddItem("Aran.Delta.EsriClasses.About");
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "Delta";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "DeltaMenu";
            }
        }
    }
}