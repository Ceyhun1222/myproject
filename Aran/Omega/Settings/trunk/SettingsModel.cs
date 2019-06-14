using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Omega.SettingsUI
{
    public abstract class SettingsModel
    {
        public SettingsModel()
        {
            Type = MenuType.Surface;
        }
        public string ViewCaption { get; set; }
        public bool IsSelected { get; set; }
       
        public MenuType Type { get; set; }
    }
}
