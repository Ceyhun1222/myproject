using Aran.PANDA.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.Models
{
    public class EtodSaveItem
    {
        public EtodSaveItem(DrawingSurface surface)
        {
            Item = surface;
            SurfaceType = Item.SurfaceBase.EtodSurfaceType;
            ViewCaption = Item.ViewCaption;
        }

        public EtodSaveItem(string header)
        {
            ViewCaption = header;
        }

        public string ViewCaption { get; set; }
        public DrawingSurface Item { get; set; }
        public bool IsChecked { get; set; }
        public EtodSurfaceType SurfaceType { get; set; }
    }
}
