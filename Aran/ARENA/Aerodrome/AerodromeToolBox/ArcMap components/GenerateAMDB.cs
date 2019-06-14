//using Aerodrome.Converter;
//using Aerodrome.Features;
//using ARENA;
//using ESRI.ArcGIS.ADF.BaseClasses;
//using ESRI.ArcGIS.ADF.CATIDs;
//using ESRI.ArcGIS.CatalogUI;
//using ESRI.ArcGIS.Framework;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace AerodromeToolBox
//{
//    [Guid("B9C95974-5FD0-4F01-941F-CF6D253253F6")]
//    [ClassInterface(ClassInterfaceType.None)]
//    [ProgId("Aerodrome.GenerateAMDB")]
//    public sealed class GenerateAMDB : BaseCommand
//    {
//        #region COM Registration Function(s)
//        [ComRegisterFunction()]
//        [ComVisible(false)]
//        static void RegisterFunction(Type registerType)
//        {
//            // Required for ArcGIS Component Category Registrar support
//            ArcGISCategoryRegistration(registerType);

//            //
//            // TODO: Add any COM registration code here
//            //
//        }

//        [ComUnregisterFunction()]
//        [ComVisible(false)]
//        static void UnregisterFunction(Type registerType)
//        {
//            // Required for ArcGIS Component Category Registrar support
//            ArcGISCategoryUnregistration(registerType);

//            //
//            // TODO: Add any COM unregistration code here
//            //
//        }

//        #region ArcGIS Component Category Registrar generated code
//        /// <summary>
//        /// Required method for ArcGIS Component Category registration -
//        /// Do not modify the contents of this method with the code editor.
//        /// </summary>
//        private static void ArcGISCategoryRegistration(Type registerType)
//        {
//            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
//            GxCommands.Register(regKey);

//        }
//        /// <summary>
//        /// Required method for ArcGIS Component Category unregistration -
//        /// Do not modify the contents of this method with the code editor.
//        /// </summary>
//        private static void ArcGISCategoryUnregistration(Type registerType)
//        {
//            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
//            GxCommands.Unregister(regKey);

//        }

//        #endregion
//        #endregion

//        private IApplication m_application;
//        public GenerateAMDB()
//        {
//            //
//            // TODO: Define values for the public properties
//            //
//            base.m_category = "Generate AMDB "; //localizable text
//            base.m_caption = "Generate AMDB";  //localizable text
//            base.m_message = "Generate AMDB";  //localizable text 
//            base.m_toolTip = "Generate AMDB";  //localizable text 
//            base.m_name = "GenerateAMDB";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
//            try
//            {
//                //
//                // TODO: change bitmap name if necessary
//                //
//                // base.m_bitmap = global::ArenaToolBox.Properties.Resources.avia_icon;//new Bitmap(GetType(), bitmapResourceName);
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
//            }


//        }

//        #region Overridden Class Methods


//        /// <summary>
//        /// Occurs when this command is created
//        /// </summary>
//        /// <param name="hook">Instance of the application</param>
//        public override void OnCreate(object hook)
//        {
//            if (hook == null)
//                return;

//            m_application = hook as IApplication;

//            //Disable if it is not ArcCatalog
//            if (hook is IGxApplication)
//                base.m_enabled = true;
//            else
//                base.m_enabled = false;

//            // TODO:  Add other initialization code
//        }

//        /// <summary>
//        /// Occurs when this command is clicked
//        /// </summary>
//        public override void OnClick()
//        {
            
//            var pdmArp = DataCash.ProjectEnvironment.Data.PdmObjectList.Where(t => t.PDM_Type == PDM_ENUM.AirportHeliport && ((AirportHeliport)t).Designator.Equals("OTHH")).FirstOrDefault();
          
//            if(pdmArp!=null)
//            {
//                var amArp = Global.FeatureGenericConverter.Convert<AirportHeliport, AM_AerodromeReferencePoint>((AirportHeliport)pdmArp);
//                Global.AmObjectList.Add(amArp);
//                #region Runway
//                foreach (var pdmRwy in ((AirportHeliport)pdmArp).RunwayList)
//                {
//                    var amRwy= Global.FeatureGenericConverter.Convert<Runway, AM_Runway>(pdmRwy);
//                    amRwy.AssociatedADHP = amArp;
//                    Global.AmObjectList.Add(amRwy);

//                    #region RunwayDirection
//                    foreach (var pdmRwyDir in pdmRwy.RunwayDirectionList)
//                    {
//                        var amRwyDir= Global.FeatureGenericConverter.Convert<RunwayDirection, AM_RunwayDirection>(pdmRwyDir);
//                        amRwyDir.AssociatedRunway = amRwy;
//                        Global.AmObjectList.Add(amRwyDir);

//                        #region RunwayCenterLinePoints

//                        if (pdmRwyDir.CenterLinePoints!=null && pdmRwyDir.CenterLinePoints.Count>0)
//                        {
//                            foreach(var pdmRwyClp in pdmRwyDir.CenterLinePoints)
//                            {
//                                var amRwyClp= Global.FeatureGenericConverter.Convert<RunwayCenterLinePoint, AM_RunwayCenterlinePoint>(pdmRwyClp);
//                                //amRwyClp.re = amRwy;
//                                Global.AmObjectList.Add(amRwyClp);
//                            }
//                        }
//                        #endregion
//                    }
//                    #endregion

//                    #region RunwayElement
//                    foreach (var pdmRwyElem in pdmRwy.RunwayElementsList)
//                    {
//                        //var amRwyElem= Global.FeatureGenericConverter.Convert<RunwayElement, AM_RunwayElement>(pdmRwyElem);
//                        //amRwyElem.idrwy = amRwy;
//                        //Global.AmObjectList.Add(amRwyElem);                       
//                    }
//                    #endregion
//                }
//                #endregion

//                #region Taxiway
//                foreach (var pdmTwy in ((AirportHeliport)pdmArp).TaxiwayList)
//                {
//                    var amTwy= Global.FeatureGenericConverter.Convert<Taxiway, AM_Taxiway>(pdmTwy);
//                    amTwy.AssociatedADHP = amArp;
//                    Global.AmObjectList.Add(amTwy);

//                    #region GuidanceLine
//                    if(pdmTwy.GuidanceLineList!=null && pdmTwy.GuidanceLineList.Count>0)
//                    {
//                        foreach(var guidLine in pdmTwy.GuidanceLineList)
//                        {
//                            var amGuidLine = Global.FeatureGenericConverter.Convert<GuidanceLine, AM_TaxiwayGuidanceLine>(guidLine);
//                        }
//                    }
//                    #endregion
//                }
//                #endregion
               
//            }





//            //StoreToDB for all classes in AM_AbstractFeature или отдельно
//            //Может лучше передать в конвертер лист объектов которые нужны для конвертации

//        }

//        #endregion
//    }
//}
