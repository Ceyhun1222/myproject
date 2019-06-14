using Aran.AranEnvironment.Symbols;
using Aran.Panda.Constants;

namespace Aran.Omega.TypeB.Settings
{
    public class SurfaceModel : SettingsModel
    {
        public SurfaceModel()
        {
            Type = MenuType.Surface;
        }
        public FillSymbol Symbol { get; set; }
        public FillSymbol SelectedSymbol { get; set; }
        public SurfaceType Surface { get; set; }
    }
}