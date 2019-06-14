using PVT.Engine.Common.Converters;
using PVT.Engine.Common.Database;
using PVT.Engine.Common.Geometry;
using PVT.Engine.Common.Logging;
using PVT.Engine.Common.Utils;
using PVT.Engine.Graphics;
using System;
using System.Windows.Forms;

namespace PVT.Engine

{
    public abstract class Environment
    {
        public static readonly string Name = "Procedure Validation";
        public static Environment Current { get; set; }

        public int CurrentCMD { get;  set; }
        public int LangCode { get; protected set; }
        public Guid CurrentAeroport { get; protected set; }
        public DateTime EffectiveDate { get; protected set; }


        public IDbProvider DbProvider { get { return DbManager.DbProvider; } }
        public IGraphics Graphics { get { return GraphicsManager.Graphics; } }
        public IGeometry Geometry { get { return GeometryManager.Geometry; } }
        public IConverters Converters { get { return ConvertersManager.Converters; } }
        public ILogger Logger { get { return LogManager.Logger; } }
        public IUtils Utils { get { return UtilsManager.Utils; } }

        public Environments Value { get; protected set; }


        public static void Init()
        {
            Current?.Initialize();
        }


        public abstract void Initialize();
        public abstract void Delete();

        public virtual IntPtr EnvWin32Window
        {
            get { return IntPtr.Zero; }
        }

        public virtual IntPtr Win32Window
        {
            get { return IntPtr.Zero; }
        }

    }
}
