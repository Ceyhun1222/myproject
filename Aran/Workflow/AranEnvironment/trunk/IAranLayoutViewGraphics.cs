using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.AranEnvironment
{
    public interface IAranLayoutViewGraphics
    {
        bool Visible { get; set; }
        List<String> Text { get; set; }
        int Scale { get; set; }
        void Draw();
        void Refresh();
        void PrepareLayout();
        void Clean();
    }
}
