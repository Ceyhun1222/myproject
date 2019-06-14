using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.Aim.Data;
using Aran.AranEnvironment;
using Aran.Delta.Builders;
using Aran.Delta.Model;
using Aran.Delta.View;
using Aran.Delta.ViewModels;
using Aran.Queries.DeltaQPI;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using SpatialReferenceOperation = Aran.PANDA.Common.SpatialReferenceOperation;

namespace Aran.Delta
{
    class ContainerBootstrapperForAran
    {
        public static void RegisterTypesForCreateRoutes(IUnityContainer container, IAranEnvironment aranEnv)
        {
            try
            {
                Debug.WriteLine(string.Format("Called RegisterTypes in ContainerBootstrapper",
                    "Unity"));

                bool isArena = aranEnv == null;
                var typeDbModule = typeof(IDBModule);

                if (!isArena)
                {
                    container.RegisterInstance<ILogger>(aranEnv.GetLogger("DELTA"));
                    container.RegisterInstance(new Aran.Delta.Model.SpatialReferenceOperation(aranEnv.Graphics.ViewProjection));

                    container.RegisterType<IPointModel, PointModel>(
                        new InjectionConstructor(typeof(Aran.Delta.Model.SpatialReferenceOperation)));

                    var deltaQpi = DeltaQpiFactory.Create();
                    var dbProvider = aranEnv.DbProvider as DbProvider;
                    deltaQpi.Open(dbProvider);

                    container.RegisterInstance(deltaQpi);

                    var settings = new Aran.Delta.Settings.DeltaSettings();
                    settings.Load(aranEnv);
                    container.RegisterInstance(settings);

                    container.RegisterType<IDBModule, DbModule>(new InjectionConstructor(typeof(IDeltaQPI),
                        typeof(Aran.Delta.Settings.DeltaSettings)));

                }
                else
                    container.RegisterType<IDBModule, ArenaDBModule>(new InjectionConstructor(typeof(IDeltaQPI)));

                container.RegisterType<ISignificantPointBuilder, SignificantPointBuilder>(
                    new ContainerControlledLifetimeManager());

                container.RegisterType<ICreateRouteViewModel, CreateRouteViewModel>(new InjectionConstructor(
                    typeof(ILogger),
                    typeDbModule, typeof(ISignificantPointBuilder), isArena));

                container.RegisterInstance(new CreateRoute(container.Resolve<ICreateRouteViewModel>()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
