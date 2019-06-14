using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.CatalogUI;
using System.Windows.Forms;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using System.IO;
using Aerodrome.DB;
using ESRI.ArcGIS.Geodatabase;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Aerodrome.Features;
using Aerodrome.Enums;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.SyncProvider;
using Framework.Stasy.Context;
using WpfUI;
using System.Collections.Generic;
using Framework.Stasy.Core;
using System.Linq;
using ESRI.ArcGIS.DataSourcesGDB;
using Framework.Stasy.Helper;
using Framework.Attributes;
using Framework.Stuff.Extensions;
using ESRI.ArcGIS.esriSystem;
using HelperDialog;
using Aerodrome.Import;
using System.Windows.Interop;
using Aerodrome.Metadata;

namespace AerodromeToolBox.ArcMap_components
{
    [Guid("6EDA7B5E-0238-433E-88F5-71CE077B34BE")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Aerodrome.AerodromeImportMdb")]
    public sealed class AerodromeImportMdb : BaseCommand
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
            GxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GxCommands.Unregister(regKey);

        }

        #endregion
        #endregion


        //Dictionary<Type, List<FeaturePropertyMappingInfo>> ImportPropertyrelations { get; set; }
        private IApplication m_application;
        public AerodromeImportMdb()
        {
            
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Import mdb file"; //localizable text
            base.m_caption = "Based on mdb";  //localizable text
            base.m_message = "Import mdb file in AMDB format";  //localizable text 
            base.m_toolTip = "Import mdb file in AMDB format";  //localizable text 
            base.m_name = "AerodromeImportMdb";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")
            try
            {



                //ImportPropertyrelations = new Dictionary<Type, List<FeaturePropertyMappingInfo>>();
                
                //ImportPropertyrelations.Add(typeof(AM_RunwayDirection), new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeyPropertyName =nameof(AM_RunwayDirection.idrwy),

                //        ForeignKeySetters=new List<SetterPropertyInfo>
                //        {
                //            new SetterPropertyInfo
                //            {
                //                SetterProperty =typeof(AM_RunwayDirection).GetProperty(nameof(AM_RunwayDirection.AssociatedRunway)),
                //                PropertyNameForRelation=nameof(AM_Runway.Name)
                //            }
                //        }
                //    }

                //});

                //List<FeaturePropertyMappingInfo> rwyElemRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_RunwayElement).GetProperty(nameof(AM_RunwayElement.AssociatedRunway)),
                //        ForeignKeyPropertyName =nameof(AM_RunwayElement.idrwy),
                //        SetterPropertyNameForSearch =nameof(AM_Runway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_RunwayElement), rwyElemRelations);

                //List<FeaturePropertyMappingInfo> rwyDispAreaRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_RunwayDisplacedArea).GetProperty(nameof(AM_RunwayDisplacedArea.RelatedRunwayDirection)),
                //        ForeignKeyPropertyName =nameof(AM_RunwayDisplacedArea.idthr),
                //        SetterPropertyNameForSearch =nameof(AM_RunwayDirection.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_RunwayDisplacedArea), rwyDispAreaRelations);

                //List<FeaturePropertyMappingInfo> rwyInterRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_RunwayIntersection).GetProperty(nameof(AM_RunwayIntersection.AssociatedRunways)),
                //        ForeignKeyPropertyName =nameof(AM_RunwayIntersection.idrwi),
                //        SetterPropertyNameForSearch =nameof(AM_Runway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_RunwayIntersection), rwyInterRelations);

                //List<FeaturePropertyMappingInfo> rwyShoulderRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_RunwayShoulder).GetProperty(nameof(AM_RunwayShoulder.AssociatedRunway)),
                //        ForeignKeyPropertyName =nameof(AM_RunwayShoulder.idrwy),
                //        SetterPropertyNameForSearch =nameof(AM_Runway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_RunwayShoulder), rwyShoulderRelations);

                //List<FeaturePropertyMappingInfo> rwyPaintedRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_PaintedCenterline).GetProperty(nameof(AM_PaintedCenterline.AssociatedRunway)),
                //        ForeignKeyPropertyName =nameof(AM_PaintedCenterline.idrwy),
                //        SetterPropertyNameForSearch =nameof(AM_Runway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_PaintedCenterline), rwyPaintedRelations);

                //List<FeaturePropertyMappingInfo> rwyMarkingRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_RunwayMarking).GetProperty(nameof(AM_RunwayMarking.AssociatedRunway)),
                //        ForeignKeyPropertyName =nameof(AM_RunwayMarking.idrwy),
                //        SetterPropertyNameForSearch =nameof(AM_Runway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_RunwayMarking), rwyMarkingRelations);

                //List<FeaturePropertyMappingInfo> rwyExitLineRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_RunwayExitLine).GetProperty(nameof(AM_RunwayExitLine.AssociatedRunway)),
                //        ForeignKeyPropertyName =nameof(AM_RunwayExitLine.idrwy),
                //        SetterPropertyNameForSearch =nameof(AM_Runway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_RunwayExitLine), rwyExitLineRelations);

                //List<FeaturePropertyMappingInfo> touchDownRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_TouchDownLiftOffArea).GetProperty(nameof(AM_TouchDownLiftOffArea.AssociatedRunway)),
                //        ForeignKeyPropertyName =nameof(AM_TouchDownLiftOffArea.idrwy),
                //        SetterPropertyNameForSearch =nameof(AM_Runway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_TouchDownLiftOffArea), touchDownRelations);

                //List<FeaturePropertyMappingInfo> twyElemRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_TaxiwayElement).GetProperty(nameof(AM_TaxiwayElement.AssociatedTaxiway)),
                //        ForeignKeyPropertyName =nameof(AM_TaxiwayElement.idlin),
                //        SetterPropertyNameForSearch =nameof(AM_Taxiway.Name)
                //    },
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_TaxiwayElement).GetProperty(nameof(AM_TaxiwayElement.AssociatedApron)),
                //        ForeignKeyPropertyName =nameof(AM_TaxiwayElement.idapron),
                //        SetterPropertyNameForSearch =nameof(AM_Apron.Name)
                //    },
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_TaxiwayElement).GetProperty(nameof(AM_TaxiwayElement.AttachedTaxiways)),
                //        ForeignKeyPropertyName =nameof(AM_TaxiwayElement.idinter),
                //        SetterPropertyNameForSearch =nameof(AM_Taxiway.Name),
                //        IsCollection=true
                //    },
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_TaxiwayElement).GetProperty(nameof(AM_TaxiwayElement.CrossedTaxiways)),
                //        ForeignKeyPropertyName =nameof(AM_TaxiwayElement.idcross),
                //        SetterPropertyNameForSearch =nameof(AM_Taxiway.Name),
                //        IsCollection=true
                //    },
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_TaxiwayElement).GetProperty(nameof(AM_TaxiwayElement.RelatedFeature)),
                //        ForeignKeyPropertyName =nameof(AM_TaxiwayElement.idbase),
                //        SetterPropertyNameForSearch =nameof(AM_RunwayElement.idrwy)
                //    }
                //};
                //ImportPropertyrelations.Add(typeof(AM_TaxiwayElement), twyElemRelations);

                //List<FeaturePropertyMappingInfo> twyGuidanceLineRelations = new List<FeaturePropertyMappingInfo>
                //{
                //    new FeaturePropertyMappingInfo
                //    {
                //        ForeignKeySetterPropInfo =typeof(AM_TaxiwayGuidanceLine).GetProperty(nameof(AM_TaxiwayGuidanceLine.RelatedTaxiway)),
                //        ForeignKeyPropertyName =nameof(AM_TaxiwayGuidanceLine.idlin),
                //        SetterPropertyNameForSearch =nameof(AM_Taxiway.Name)}
                //};
                //ImportPropertyrelations.Add(typeof(AM_TaxiwayGuidanceLine), twyGuidanceLineRelations);


                //
                // TODO: change bitmap name if necessary
                //
                // base.m_bitmap = global::ArenaToolBox.Properties.Resources.avia_icon;//new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }


        }

        #region Overridden Class Methods


        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            m_application = hook as IApplication;

            //Disable if it is not ArcCatalog
            if (hook is IGxApplication)
                base.m_enabled = true;
            else
                base.m_enabled = false;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (AerodromeDataCash.ProjectEnvironment != null)
            {
                m_application.SaveDocument(System.IO.Path.Combine(AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath, "AMDB.mxd"));
                if (AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved == true)
                {
                    System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Save changes to the AMDB project?", "Aerodrome", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);
                    if (result == System.Windows.MessageBoxResult.Yes)
                    {                        
                        HelperMethods.SaveAmdbProject(m_application, showSplash: true);                       
                    }                   
                    else if (result == System.Windows.MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = false;
                }
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Cursor Files|*.mdb";
            openFileDialog1.Title = "Select mdb File";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .CUR file was selected, open it.  

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            
            Splasher.Splash = new SplashScreen();
            var parentHandle = new IntPtr(m_application.hWnd);
            var helper = new WindowInteropHelper(Splasher.Splash) { Owner = parentHandle };
            MessageListener.Instance.ReceiveMessage("");

            Splasher.ShowSplash();

            IMxDocument pNewDocument = (IMxDocument)m_application.Document;
            IMap pMap = pNewDocument.FocusMap;

            AerodromeEnvironment Environment = new AerodromeEnvironment { mxApplication = m_application };
            m_application.Caption = "Untitled - ArcMap";          

            #region Spatial Reference

            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();

            ISpatialReference spatialReference =

            spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

            ISpatialReferenceResolution spatialReferenceResolution = (ISpatialReferenceResolution)spatialReference;

            spatialReferenceResolution.ConstructFromHorizon();

            ISpatialReferenceTolerance spatialReferenceTolerance = (ISpatialReferenceTolerance)spatialReference;

            spatialReferenceTolerance.SetDefaultXYTolerance();
            #endregion
           
            HelperMethods.CreateAmdbProject(m_application);            

            ExtensionDataCash.ProjectxtensionContext = new ExtensionDataContext(System.IO.Path.GetDirectoryName(AerodromeDataCash.ProjectEnvironment.CurrentProjTempPath));
            ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Initialize();


            Application.DoEvents();
           
            ImportHelper importHelper = new ImportHelper();
          
            bool importresult = importHelper.ImportMdbFile(openFileDialog1.FileName);

            if(!importresult)
            {
                AerodromeDataCash.ProjectEnvironment = null;
                Splasher.CloseSplash();
                System.Windows.MessageBox.Show("AerodromeReferencePoint layer not found. Select correct mdb file", "Aerodrome", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                return;
            }

            var arpColl = (IEnumerable<dynamic>)AerodromeDataCash.ProjectEnvironment?.Context.FeatureCollections[typeof(AM_AerodromeReferencePoint)];

            if (arpColl?.Count() > 0)
            {
                var arp = arpColl.FirstOrDefault() as AM_AerodromeReferencePoint;

                ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Organization = arp.Organization;
                ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.ADHP = arp.idarpt?.Value;
                ExtensionDataCash.ProjectxtensionContext.ProjectExtensionData.Name = arp.name;

                var meridian = ((IPoint)arp.geopnt).X;
                AerodromeDataCash.ProjectEnvironment.pMap.SpatialReference = EsriUtils.CreateSpatialReferenceByMeridian(meridian);

                IMxDocument pMxDoc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;

                ESRI.ArcGIS.Geometry.IEnvelope envelopeCls = ((IPoint)arp.geopnt).Envelope;
                double dim = 1.0;
                double layerWidth = ((IPoint)arp.geopnt).Envelope.Width;
                double layerHeight = ((IPoint)arp.geopnt).Envelope.Height;
                double layerDim = System.Math.Max(layerWidth, layerHeight) * 0.05;

                if (layerDim > 0.0)
                    dim = System.Math.Min(1.0, layerDim);

                double xMin = ((IPoint)arp.geopnt).Envelope.XMin;
                double yMin = ((IPoint)arp.geopnt).Envelope.YMin;

                ESRI.ArcGIS.Geometry.IPoint pointCls = new ESRI.ArcGIS.Geometry.PointClass();
                pointCls.X = xMin;
                pointCls.Y = yMin;
                envelopeCls.Width = dim;
                envelopeCls.Height = dim;
                envelopeCls.CenterAt(pointCls);
                pMxDoc.ActiveView.Extent = envelopeCls;
                
                pMxDoc.ActiveView.FocusMap.MapScale = 15000;
                pMxDoc.ActiveView.Refresh();
            }
            AerodromeDataCash.ProjectEnvironment.ProjectNeedSaved = true;

            Application.DoEvents();
            Splasher.CloseSplash();
            MessageScreen messageScreen = new MessageScreen();
            var messageScreeenHelper = new WindowInteropHelper(messageScreen) { Owner = parentHandle };
            messageScreen.MessageText = "Import complete";
            messageScreen.ShowDialog();
        }

        #endregion
    }

}
