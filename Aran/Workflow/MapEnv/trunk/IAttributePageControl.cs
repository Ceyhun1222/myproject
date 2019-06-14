using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;

namespace MapEnv
{
    public interface IAttributePageControl
    {
        event ZoomToEventHandler ZoomToClicked;

        void OpenLayer (ILayer layer);

        void OnClose ();

        string Text { get; }

        ILayer Layer { get; }
    }
}
