using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.AranEnvironment;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Forms.Integration;
using System.Windows;
using Aran.Omega.TypeBEsri.View;
using Aran.Aim;


namespace Aran.Omega.TypeBEsri
{
    public class OmegaService //: IAranPlugin
    {
        public static AranTool ByClickToolButton;
        //public void Startup(IAranEnvironment aranEnv)
        //{
        //    var menuItem = new ToolStripMenuItem();
        //    menuItem.Text = "Omega";

        //    var olsMenuItem = new ToolStripMenuItem {Text = "Type B "};
        //    olsMenuItem.Click += new EventHandler(OlsMenuItem_Click);
        //    menuItem.DropDownItems.Add(olsMenuItem);

        //    bool isHave = false;
        //    foreach (ToolStripMenuItem apps in aranEnv.AranUI.GetApplicationMenuItem.DropDownItems)
        //    {
        //        if (apps.Text == "Omega")
        //        {
        //            apps.DropDownItems.Add(olsMenuItem);
        //            isHave = true;
        //        }
        //    }
        //    if (!isHave)
        //        aranEnv.AranUI.AddMenuItem(AranMapMenu.Applications, menuItem);

        //    var win32Window = new Win32Windows(aranEnv.Win32Window.Handle.ToInt32());
        //    GlobalParams.AranEnvironment = aranEnv;
        //    GlobalParams.AranExtension = aranEnv.PandaAranExt;
        //}

        //void OlsMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (InitOmega.InitCommand())
        //    {
        //        AranTool aranToolItem = new AranTool();
        //        aranToolItem.Visible = false;
        //        aranToolItem.Cursor = Cursors.Cross;
        //        ByClickToolButton = aranToolItem;
        //        var parentHandle = GlobalParams.AranEnvironment.Win32Window.Handle; // the ArcMap window handle

        //        try
        //        {
        //            GlobalParams.TypeBView = new TypeBView();
        //            TypeBView window = GlobalParams.TypeBView;

        //            if (GlobalParams.Logs.Length > 0)
        //                System.Windows.MessageBox.Show(GlobalParams.Logs, "Omega", MessageBoxButton.OK, MessageBoxImage.Warning);

        //            var helper = new WindowInteropHelper(window);
        //            helper.Owner = parentHandle;
        //            ElementHost.EnableModelessKeyboardInterop(window);
        //            GlobalParams.HWND = parentHandle.ToInt32();
        //            window.ShowInTaskbar = false; // hide from taskbar and alt-tab list
        //            window.Show();
                    
        //        }
        //        catch (Exception ex)
        //        {   
        //            System.Windows.MessageBox.Show(ex.Message, "Omega", MessageBoxButton.OK, MessageBoxImage.Error);   
        //        }
        //    }
        //}


        //public string Name
        //{
        //    get { return "TypeB"; }
        //}

        //public void AddChildSubMenu(List<string> hierarcy)
        //{
        //    throw new NotImplementedException();
        //}

        //public Guid Id
        //{
        //    get { return new Guid("b1e84b5f-d999-43b8-84fb-308f373d6c15"); }
        //}

        //public List<Aim.FeatureType> GetLayerFeatureTypes()
        //{
        //    var list = new List<FeatureType>();

        //    list.Add(FeatureType.AirportHeliport);
        //    list.Add(FeatureType.RunwayCentrelinePoint);
        //    list.Add(FeatureType.VerticalStructure);
        //    list.Add(FeatureType.ObstacleArea);

        //    return list;
        //}
    }
}
