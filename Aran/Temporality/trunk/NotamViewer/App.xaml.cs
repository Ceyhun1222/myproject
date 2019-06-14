using System;
using System.Windows;
using System.Windows.Controls;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.CommonUtil.Util;
using ESRI.ArcGIS.esriSystem;
using NotamViewer.Properties;

namespace NotamViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            MainAction = () => new MainWindow().ShowDialog();
        }

        #region Resources init/release procedures

        public override void Init()
        {

            //init esri license in the same thread
            LicenseInitializer.Instance.InitializeApplication(
               new[] { esriLicenseProductCode.esriLicenseProductCodeBasic },
               new esriLicenseExtensionCode[] { });


            ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject),
                                                                          new FrameworkPropertyMetadata(0));
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject),
                                                                new FrameworkPropertyMetadata(Int32.MaxValue));
            ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof(DependencyObject),
                                                                 new FrameworkPropertyMetadata(true));

            base.Init();
        }

        public override void Release()
        {
            base.Release();

            UiHelperMetadata.SaveBusinessRules();
			UiHelperMetadata.SaveLinkProblemColumns ( );

            //ESRI License Initializer generated code.
            //Do not make any call to ArcObjects after ShutDownApplication()
            LicenseInitializer.Instance.ShutdownApplication();

            Settings.Default.Save();
        }

        #endregion
    }
}
