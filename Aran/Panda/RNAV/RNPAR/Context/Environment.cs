using Aran.Aim.Data;
using Aran.AranEnvironment;
using Aran.Panda.RNAV.RNPAR.Properties;
using Aran.PANDA.Common;
using System;

namespace Aran.Panda.RNAV.RNPAR.Context
{
    class AppEnvironment
    {
        public static AppEnvironment Current { get; private set; }

        public static void Init(IAranEnvironment aranEnv)
        {
            Current = new AppEnvironment(aranEnv);
        }

        public RNPContext RNPContext { get; }
        public SystemContext SystemContext { get;}
        public ApplicationContext ApplicationContext { get; }
        public UnitContext UnitContext { get; }
        public SpatialContext SpatialContext { get;}
        public DataContext DataContext { get; }
        public IAranEnvironment AranEnv { get; }
        public PANDA.Common.Settings Settings { get; }
        public IAranGraphics AranGraphics => AranEnv?.Graphics;


        private AppEnvironment(IAranEnvironment aranEnv)
        {

            AranEnv = aranEnv;
            Settings = new PANDA.Common.Settings();
            Settings.Load(aranEnv);
            SystemContext = new SystemContext(this);
            ApplicationContext = new ApplicationContext(this);
            UnitContext = new UnitContext(this);
            SpatialContext = new SpatialContext(this);
            DataContext = new DataContext(this);
            RNPContext = new RNPContext();
            DataContext.Init();
        }
       
        public ILogger GetLogger(string name)
        {
            return AranEnv.GetLogger(name);
        }

        public ILogger GetLogger(Type type)
        {
            return AranEnv.GetLogger(type.FullName);
        }
    }
}
