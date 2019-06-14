using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aran.AranEnvironment;
using Aran.PANDA.Common;
using DataImporter.Export;
using DataImporter.Factories;
using DataImporter.Factories.ObstacleLoader;
using DataImporter.Factories.RunwayLoader;
using DataImporter.Models;
using DataImporter.Repository;
using DataImporter.View;
using DataImporter.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace DataImporter
{
    public class Service: AranPlugin
    {
        private IAranEnvironment _aranEnv;
        public override Guid Id => new Guid("0eaf1d31-921e-4246-a6aa-5f55a9489163");

        public override void Startup(IAranEnvironment aranEnv)
        {
            _aranEnv = aranEnv;
            var menuItem = new ToolStripMenuItem("Import Data From Excel");
            menuItem.Click += new EventHandler(ImportItem);

            aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);
        }

        private void ImportItem(object sender, EventArgs e)
        {
            var unityContainer = new Unity.UnityContainer();
            unityContainer.RegisterInstance<ILogger>(_aranEnv.GetLogger("Data Importer"));

            unityContainer.RegisterInstance<IDialogCoordinator>(DialogCoordinator.Instance);
            unityContainer.RegisterInstance<IAranEnvironment>(_aranEnv);

            unityContainer.RegisterInstance<SpatialReferenceOperation>(new SpatialReferenceOperation(unityContainer.Resolve<IAranEnvironment>()));

            unityContainer.RegisterSingleton<IRepository,ArandDbRepository>(new InjectionConstructor(
                unityContainer.Resolve<IAranEnvironment>()));

            unityContainer.RegisterInstance<IAranGraphics>(_aranEnv.Graphics);

            unityContainer.RegisterType<IRunwayLoaderFactory, RunwayLoaderFactory>(
                new InjectionConstructor(unityContainer.Resolve<ILogger>(),
                                         unityContainer.Resolve<SpatialReferenceOperation>()   
                ));

            unityContainer.RegisterSingleton<ICommonObject, CommonObject>(
                new InjectionConstructor(unityContainer.Resolve<ILogger>(), 
                                        unityContainer.Resolve<IDialogCoordinator>()
                ));

            unityContainer.RegisterSingleton<IRunwayCntDbSaver, RunwayCntDbSaver>(
                new InjectionConstructor(unityContainer.Resolve<IRepository>()
                ));

            unityContainer.RegisterType<IImportPageVM,RwyCntPointViewModel>("RwyCntVM",
                new InjectionConstructor(unityContainer.Resolve<IRepository>(),
                                        unityContainer.Resolve<IAranGraphics>(),
                                        unityContainer.Resolve<IRunwayLoaderFactory>(),
                                        unityContainer.Resolve<IRunwayCntDbSaver>(),
                                        unityContainer.Resolve<ICommonObject>())
                                        );

            unityContainer.RegisterType<IObstacleDbSaver, ObstacleDbSaver>(new InjectionConstructor(
                unityContainer.Resolve<IRepository>()));

            unityContainer.RegisterType<IObstacleLoaderFactory, ObstacleLoaderFactory>(
                new InjectionConstructor(unityContainer.Resolve<ILogger>(),
                                        unityContainer.Resolve<SpatialReferenceOperation>())
                                        );

            unityContainer.RegisterType<IImportPageVM, ObstacleViewModel>("ObstacleVM",
                new InjectionConstructor(unityContainer.Resolve<IObstacleDbSaver>(),
                    unityContainer.Resolve<IAranGraphics>(),
                    unityContainer.Resolve<IObstacleLoaderFactory>(),
                    unityContainer.Resolve<ICommonObject>()
                    ));

            unityContainer.RegisterSingleton<EsriObstacleExporter>();


            IEnumerable<IImportPageVM> pageList = unityContainer.ResolveAll<IImportPageVM>();
            var listOfPages = pageList.ToList();

            unityContainer.RegisterType<IImportPageVM, MainViewModel>("MainViewModel",
                new InjectionConstructor(listOfPages,unityContainer.Resolve<IDialogCoordinator>(),
                    unityContainer.Resolve<SpatialReferenceOperation>(),
                    unityContainer.Resolve<EsriObstacleExporter>()));


            unityContainer.RegisterInstance(new MainWindow(unityContainer.Resolve<IImportPageVM>("MainViewModel")));

            var window = unityContainer.Resolve<MainWindow>();
            var helper = new WindowInteropHelper(window) { Owner = _aranEnv.Win32Window.Handle };
            ElementHost.EnableModelessKeyboardInterop(window);
            window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
            window.Show();
        }

        public override string Name => "Excel Data Importer";
    }
}
