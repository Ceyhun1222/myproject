using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.AranEnvironment
{
    public interface IAranUI
    {
        void AddMenuItem (AranMapMenu mapMenu, ToolStripMenuItem menuItem, string otherName = null);
        void AddMapTool (AranTool toolStripMenuItem);
        void SetCurrentTool (AranTool toolStripMenuItem);
        void SetPanTool ();
        void AddSettingsPage (Guid[] baseOnPlugin, ISettingsPage page);

        ToolStripMenuItem GetApplicationMenuItem { get; }
    }

	public enum AranMapMenu
	{
		File,
		View,
		Map, 
		AIM,
		Applications,
		Plugins,
		Tools,
		Help,
        Other
	}
}
