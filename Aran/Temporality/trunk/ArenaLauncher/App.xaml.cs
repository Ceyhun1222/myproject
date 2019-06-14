using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ARENA.Project;
using ARENA.Util;
using Aran.Aim;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using ArenaLauncher.Converter;
using ArenaLauncher.Properties;
using ESRI.ArcGIS.esriSystem;
using MvvmCore;
using NotamViewer.View;
using NotamViewer.ViewModel;
using PDM;
using ESRI.ArcGIS;

namespace ArenaLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private static void ProcessFeature(AimFeature aimAirspace, DateTime actualDate)
        {
            //deserialize esri geo
            GeometryFormatter.PrepareGeometry(aimAirspace, true);
            var geoDataList = aimAirspace.PropertyExtensions.OfType<EsriPropertyExtension>().ToList();

            if (geoDataList.Count == 0) return;

            //convert airspace here
			var pdmAirspace = PdmConverter.ConvertFromAim ( ( Aran.Aim.Features.Airspace ) aimAirspace.Feature, actualDate );
        

            //convert geo
            pdmAirspace.AirspaceVolumeList = new List<AirspaceVolume>();
            for (var index = 0; index < ((Aran.Aim.Features.Airspace)aimAirspace.Feature).GeometryComponent.Count;
                 index++)
            {
                var aimVolume = ((Aran.Aim.Features.Airspace)aimAirspace.Feature).GeometryComponent[index].TheAirspaceVolume;

                //var correspondingGeo;
                var pdmVolume = PdmConverter.ConvertFromAim(aimVolume);
                var geometry = geoDataList[index].EsriObject;

                //add geo here
                pdmVolume.Geo = geometry;
                pdmAirspace.AirspaceVolumeList.Add(pdmVolume);
            }


            pdmAirspace.Save();
        }

        private static void OnMapCommand(NotamViewerViewModel viewModel)
        {
            viewModel.BlockerModel.Block();

            var s = new Stopwatch();
            s.Start();

            try
            {
                viewModel.ProgressVisible = Visibility.Visible;
                var keyDates=viewModel.KeyDates;

                var maximumFeaturesPerDate = 1;
                var totalFeaturesProcessed = 0;
                var totalDatesProcessed = 0;

                Application.Current.Dispatcher.Invoke(
                               DispatcherPriority.Background,
                               (Action)(() =>
                               {
                                   viewModel.ProgressValue = 0;
                                   viewModel.ProgressMinimum = 0;
                               }));

                foreach (var dateTime in keyDates)
                {
                    //process current Date
					var aimAirspaces = CurrentDataContext.CurrentService.GetActualDataByDate (
										  new FeatureId
										  {
											  FeatureTypeId =
												  ( int )
												  FeatureType.Airspace
										  },
										  false, dateTime ).Select ( t => t.Data ).ToList ( );
                    totalDatesProcessed++;//date was processed

                    //calculate maximum
                    if (maximumFeaturesPerDate < aimAirspaces.Count)
                    {
                        maximumFeaturesPerDate = aimAirspaces.Count;
                    }

                    var totalFeaturesEstimated = totalFeaturesProcessed + //processed earlier
                        + aimAirspaces.Count + //to be processed in current date
                        (keyDates.Count - totalDatesProcessed) * maximumFeaturesPerDate;//to be processed for other dates

                    //reset new calculated maximum
                    Application.Current.Dispatcher.Invoke(
                               DispatcherPriority.Background,
                               (Action)(() =>
                               {
                                   viewModel.ProgressMaximum = totalFeaturesEstimated;
                               }));

                    //process current date data
                    foreach (var aimAirspace in aimAirspaces)
                    {
                        ProcessFeature(aimAirspace, dateTime);
                       totalFeaturesProcessed++;

                       Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Background,
                                (Action)(() => viewModel.ProgressValue++));
                    }

                 
                }



            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                viewModel.ProgressVisible = Visibility.Hidden;
                viewModel.BlockerModel.Unblock();
            }

            s.Stop();
            //MessageBox.Show("Total converting time: " + s.ElapsedMilliseconds + "ms");
        }

        private static Window _notamViewerWindow;

      

        private static void OpenNotamWindow()
        {  
            if (_notamViewerWindow==null)
            {
                var notamViewer = new NotamViewerView();
                _notamViewerWindow = new NotClosingWindow
                                         {
                                             Content = notamViewer,
                                             Title = "NOTAM Preview",
                                             WindowStartupLocation = WindowStartupLocation.CenterOwner,
                                             SnapsToDevicePixels = true,
                                             WindowState = WindowState.Maximized,
                                             Icon = new BitmapImage(new Uri("pack://application:,,,/ArenaLauncher;component/Resources/Images/stopwatch.ico"))
                                         };


                var viewModel = notamViewer.DataContext as NotamViewerViewModel;
                if (viewModel != null)
                {
                    viewModel.OnMapCommand = new RelayCommand(
                        t =>
                        {
                            OnMapCommand(viewModel);
                            _notamViewerWindow.Hide();
                        },
                        t => viewModel.IsOnCommandEnable());
                }
            }

            _notamViewerWindow.ShowDialog();
        }

        public App()
        {
			if ( !RuntimeManager.Bind ( ProductCode.Desktop ) )
			{
				if ( !RuntimeManager.Bind ( ProductCode.Desktop ) )
				{
					MessageBox.Show ( "Unable to bind to ArcGIS runtime. Application will be shut down." );
					return;
				}
			}

            MainAction = () =>
                             {
                                 NotamProject.RunNotamConfig = (mainform) => OpenNotamWindow();
                                 var arenaForm=new ARENA.MainForm();
                                 arenaForm.ShowDialog();
                             };
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
