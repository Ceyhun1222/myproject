using System.Collections.Generic;
using System.Windows.Forms;
using ARENA.Environment;
using ARENA.Util;
using PDM;
using PDM.PropertyExtension;
using ESRI.ArcGIS.Controls;
using ARENA.Enums_Const;

namespace ARENA.Project
{
    public abstract class AbstractProject
    {
        protected AbstractProject(Environment.Environment environment)
        {
            Environment = environment;
        }

        public Environment.Environment Environment { get; set; }

        public abstract ArenaProjectType ProjectType { get; }
        public static string CurProjectName { get; set; }

        public abstract void FillObjectsTree();

        #region Helper methods

        public TreeView FeatureTreeView { 
            get {
                return Environment.FeatureTreeView;
            }
        }

        public ReadOnlyPropertyGrid ReadOnlyPropertyGrid
        {
            get { return Environment.ReadOnlyPropertyGrid; }
        }

        public List<PDMObject> PdmObjectList { get { return Environment.Data.PdmObjectList; } }

        public ToolStrip EnvironmentToolStrip { get { return Environment.EnvironmentToolStrip; } }

        #endregion

        #region Event Handler

        public virtual void OnCreate()
        {
        }

        public virtual void OnLoad()
        {
        }

        public virtual void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        public virtual void TreeViewAfterCheck(object sender, TreeViewEventArgs e)
        {
        }

        //public virtual void MapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        //{
        //    if (e.button == 2) Environment.mapControlContextMenuStrip.Show(Environment.MapControl, e.x, e.y);
        //}



        #endregion
    }
}
