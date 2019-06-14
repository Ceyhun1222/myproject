using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using TOSSM.Properties;
using TOSSM.Util;
using TOSSM.View;
using TOSSM.ViewModel;
using Application = System.Windows.Forms.Application;
using Clipboard = System.Windows.Forms.Clipboard;
using IDataObject = System.Windows.Forms.IDataObject;

namespace TOSSM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static SecuredApplication Instance; 

        /// <summary>  
        /// Loads the CSM.dll from ArcGIS BIN path in case the process wants to load the Microsoft library of same name.  
        /// Call this method as early as possible to be effective (e.g. main form OnLoad, program's entry point method).  
        /// </summary>  
        private static void PreLoadEsriCsm(bool debugOnly)
        {
            if (!debugOnly || System.Diagnostics.Debugger.IsAttached)
            {
                string path = System.IO.Path.Combine(ESRI.ArcGIS.RuntimeManager.ActiveRuntime.Path, "Bin\\CSM.dll");
                if (System.IO.File.Exists(path))
                {
                    bool isConflict = false;
                    bool isMatch = false;
                    using (var p = System.Diagnostics.Process.GetCurrentProcess())
                    {
                        foreach (System.Diagnostics.ProcessModule m in p.Modules)
                        {
                            if (m.ModuleName.ToLower() == "csm.dll")
                            {
                                if (path.ToLower() != m.FileName.ToLower())
                                    isConflict = true;
                                else
                                {
                                    isMatch = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isConflict && !isMatch)
                    {
                        System.Diagnostics.Debug.WriteLine("It may be necessary to call this method earlier to be effective.");
                    }
                    if (!isMatch)
                        LoadLibrary(path);
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllname);




        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin=false;
            try
            {
                //get the currently logged in user
                var user = WindowsIdentity.GetCurrent();
                if (user != null)
                {
                    var principal = new WindowsPrincipal(user);
                    isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }


        private static BackgroundWorker _serverWorker;
        private void StartServer()
        {
            #if LOCAL_SERVER

            if (!IsUserAdministrator())
            {
                MessageBoxHelper.Show("You should be system administrator to start server.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
            }

            _serverWorker = new BackgroundWorker();
            var serverStarted = false;
            _serverWorker.DoWork += (t, e) =>
                                 {
                                     try
                                     {
                                         Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                                         Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                                         TemporalityServer.StartServer();
                                         Console.WriteLine(@"Service is available. Press <ENTER> to exit.");
                                         Console.ReadLine();
                                     }
                                     catch (Exception exception)
                                     {
                                         MessageBoxHelper.Show(exception.Message, "Can not start server", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                     }
                                     serverStarted = true;
                                 };

            _serverWorker.RunWorkerAsync();

            int i = 0;
            while (!serverStarted && i++<50)
            {
                Thread.Sleep(100);
            }

            Thread.Sleep(2000);

            #endif
        }

        private void StopServer()
        {
            #if LOCAL_SERVER
            TemporalityServer.StopServer();
            Release();
            Environment.Exit(0);
            #endif
        }


        public App()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LogManager.Configure("tossmlogs.log", "tossmerrorlogs.log", LogLevel.Info);
            LogManager.GetLogger(this).Info("TOSSM is starting.");

            try
            {
            
                StartServer();

                MainAction = () =>
                {
                    //init esri license in the same thread

                    esriLicenseProductCode esriLicenseProductCode;
                    if (CurrentDataContext.CurrentLicense == EsriLicense.Basic)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeBasic;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Standard)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeStandard;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Enginegeodb)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Advanced)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeAdvanced;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Arcserver)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeArcServer;
                    else if (CurrentDataContext.CurrentLicense == EsriLicense.Engine)
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeEngine;
                    else
                        esriLicenseProductCode = esriLicenseProductCode.esriLicenseProductCodeBasic;

                    var initializeIsSuccess = LicenseInitializer.Instance.InitializeApplication(new[] { esriLicenseProductCode }, new esriLicenseExtensionCode[] { });

                    if (CurrentDataContext.CurrentLicense != EsriLicense.Missing)
                    {
                        if (!initializeIsSuccess)
                        {
                            throw new Exception("Error initializing ESRI License.");
                        }
                    }

                    PreLoadEsriCsm(true);

                    LoadDataFromClipBoard();
                    //CurrentDataContext.GetBusinessRules();
                    new MainManagerWindow().ShowDialog();

                    MainManagerModel.Instance.SlotSelectorToolViewModel.IsTerminated = true;
                    try
                    {
                        ConnectionProvider.Close();
                        StopServer();
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(App)).Error(ex, "Error on application closing");
                    }

                };
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(App)).Error(ex, "Error on application starting");
                throw;
            }

        }

        #region esriConstants

        public const string esriIACFeatureLayer = "{AD88322D-533D-4E36-A5C9-1B109AF7A346}";
        public const string esriIACLayer = "{74E45211-DFE6-11D3-9FF7-00C04F6BC6A5}";
        public const string esriIACImageLayer = "{495C0E2C-D51D-4ED4-9FC1-FA04AB93568D}";
        public const string esriIACAcetateLayer = "{65BD02AC-1CAD-462A-A524-3F17E9D85432}";
        public const string esriIAnnotationLayer = "{4AEDC069-B599-424B-A374-49602ABAD308}";
        public const string esriIAnnotationSublayer = "{DBCA59AC-6771-4408-8F48-C7D53389440C}";
        public const string esriICadLayer = "{E299ADBC-A5C3-11D2-9B10-00C04FA33299}";
        public const string esriICadastralFabricLayer = "{7F1AB670-5CA9-44D1-B42D-12AA868FC757}";
        public const string esriICompositeLayer = "{BA119BC4-939A-11D2-A2F4-080009B6F22B}";
        public const string esriICompositeGraphicsLayer = "{9646BB82-9512-11D2-A2F6-080009B6F22B}";
        public const string esriICoverageAnnotationLayer = "{0C22A4C7-DAFD-11D2-9F46-00C04F6BC78E}";
        public const string esriIDataLayer = "{6CA416B1-E160-11D2-9F4E-00C04F6BC78E}";
        public const string esriIDimensionLayer = "{0737082E-958E-11D4-80ED-00C04F601565}";
        public const string esriIFDOGraphicsLayer = "{48E56B3F-EC3A-11D2-9F5C-00C04F6BC6A5}";
        public const string esriIFeatureLayer = "{40A9E885-5533-11D0-98BE-00805F7CED21}";
        public const string esriIGdbRasterCatalogLayer = "{605BC37A-15E9-40A0-90FB-DE4CC376838C}";
        public const string esriIGeoFeatureLayer = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
        public const string esriIGraphicsLayer = "{34B2EF81-F4AC-11D1-A245-080009B6F22B}";
        public const string esriIGroupLayer = "{EDAD6644-1810-11D1-86AE-0000F8751720}";
        public const string esriIIMSSubLayer = "{D090AA89-C2F1-11D3-9FEF-00C04F6BC6A5}";
        public const string esriIIMAMapLayer = "{DC8505FF-D521-11D3-9FF4-00C04F6BC6A5}";
        public const string esriILayer = "{34C20002-4D3C-11D0-92D8-00805F7C28B0}";
        public const string esriIMapServerLayer = "{E9B56157-7EB7-4DB3-9958-AFBF3B5E1470}";
        public const string esriIMapServerSublayer = "{B059B902-5C7A-4287-982E-EF0BC77C6AAB}";
        public const string esriINetworkLayer = "{82870538-E09E-42C0-9228-CBCB244B91BA}";
        public const string esriIRasterLayer = "{D02371C7-35F7-11D2-B1F2-00C04F8EDEFF}";
        public const string esriIRasterCatalogLayer = "{AF9930F0-F61E-11D3-8D6C-00C04F5B87B2}";
        public const string esriITemporaryLayer = "{FCEFF094-8E6A-4972-9BB4-429C71B07289}";
        public const string esriITerrainLayer = "{5A0F220D-614F-4C72-AFF2-7EA0BE2C8513}";
        public const string esriITinLayer = "{FE308F36-BDCA-11D1-A523-0000F8774F0F}";
        public const string esriITopologyLayer = "{FB6337E3-610A-4BC2-9142-760D954C22EB}";
        public const string esriIWMSLayer = "{005F592A-327B-44A4-AEEB-409D2F866F47}";
        public const string esriIWMSGroupLayer = "{D43D9A73-FF6C-4A19-B36A-D7ECBE61962A}";
        public const string esriIWMSMapLayer = "{8C19B114-1168-41A3-9E14-FC30CA5A4E9D}";

        #endregion

        //Command	Paste	Edit_Paste	Edit	{A33D9407-7ED5-11D0-8D7C-0080C7A4557D} esriArcMapUI.EditPasteCommand	
        //none	Edit_Menu	Paste the clipboard contents into your map

        void LoadDataFromClipBoard()
        {
            //Get the Data from the Clipboard  
            IDataObject clipBoardDataObject = Clipboard.GetDataObject();
            if (clipBoardDataObject == null) return;

            var geomStream = clipBoardDataObject.GetData("ESRI Graphics List") as MemoryStream;
            if (geomStream == null) return;
            byte[] bytes = geomStream.ToArray();


           // var f = new FileStream("d:\\clip.dat",FileMode.CreateNew);
           //f.Write(bytes,0,bytes.Length);
           //f.Close();

            IMemoryBlobStreamVariant memoryBlobStreamVariant = new MemoryBlobStreamClass();
            memoryBlobStreamVariant.ImportFromVariant(bytes);
            IMemoryBlobStream2 memoryBlobStream = memoryBlobStreamVariant as IMemoryBlobStream2;
            IStream stream = memoryBlobStream as IStream;

            IObjectStream objectStream = new ObjectStreamClass();
            objectStream.Stream = stream;

            byte pv;
            uint cb = sizeof(int);
            uint pcbRead;

            objectStream.RemoteRead(out pv, cb, out pcbRead);
            int count = Convert.ToInt32(pv);

            // Define Guids
            Guid guidLayer = new Guid("34B2EF81-F4AC-11D1-A245-080009B6F22B");

            var list = new ArrayList(bytes);
            var s=list.IndexOf((byte)194);


            // Get Dropped Layers
            for (int i = 0; i < count; i++)
            {
                object o = objectStream.LoadObject(ref guidLayer, null);
                IGraphicsLayer layer = (IGraphicsLayer)o;
            }

        }

        #region Resources init/release procedures

        private void InitializeEngineLicense()
        {
            AoInitialize aoi = new AoInitializeClass();

            //more license choices could be included here
            esriLicenseProductCode productCode = esriLicenseProductCode.esriLicenseProductCodeEngine;
            if (aoi.IsProductCodeAvailable(productCode) == esriLicenseStatus.esriLicenseAvailable)
            {
                aoi.Initialize(productCode);
            }
        }
        public override void Init()
        {
            Instance = this;

            //Process proc = Process.GetCurrentProcess();
            //int count = Process.GetProcesses().Count(p => p.ProcessName == proc.ProcessName);
            //if (count > 1)
            //{

            //    try
            //    {
            //        MessageBoxHelper.Show("TOSSM instance arleady is running...");
            //        App.Current.Shutdown();
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    Environment.Exit(1);
            //}

            //Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            //if (processes.Length > 1)
            //{
            //    try
            //    {
            //        var client = new NamedPipeClientStream("TossmPipe");
            //        client.Connect();
            //        StreamWriter writer = new StreamWriter(client);
            //        writer.WriteLine(String.Join("|", StartupArgs));
            //        writer.Flush();
            //        client.Close();
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    Environment.Exit(1);
            //}

            var d = Aran.Aim.Metadata.UI.UIMetadata.Instance.ClassInfoList;
            if (d.Count == 0)
            {
            }

            base.Init();
        }

        public override void Release()
        {

            LogManager.GetLogger(this).Info("TOSSM is closing.");

            base.Release();

            UiHelperMetadata.SaveBusinessRules();
			UiHelperMetadata.SaveLinkProblemColumns ( );

            //ESRI License Initializer generated code.
            //Do not make any call to ArcObjects after ShutDownApplication()
            if (ConfigUtil.UseEsri)
            {
                LicenseInitializer.Instance.ShutdownApplication();
            }

            Settings.Default.Save();

            Environment.Exit(0);
        }

        #endregion
    }
}
