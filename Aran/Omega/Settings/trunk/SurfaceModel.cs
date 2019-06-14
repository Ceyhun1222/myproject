using Aran.AranEnvironment.Symbols;
using Aran.PANDA.Constants;

namespace Aran.Omega.SettingsUI
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