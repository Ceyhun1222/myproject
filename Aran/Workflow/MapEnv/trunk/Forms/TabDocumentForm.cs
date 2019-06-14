using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Controls;

namespace MapEnv
{
    public partial class TabDocumentForm : Form
    {
        public const Int32 IDM_DOCK = 1000;

        public event TabDocumentEventHandler TabDocumentDockClicked;

        public TabDocumentForm ()
        {
            InitializeComponent ();
        }

        public void AddPage (TabDocument page)
        {
            ui_tdiControl.AddPage (page);
        }

        public TDIControl TDIControl
        {
            get { return ui_tdiControl; }
        }


        private void TabDocumentForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                TabDocument [] pageArr = ui_tdiControl.PageDocuments;
                foreach (TabDocument page in pageArr)
                {
                    page.ClosePage ();
                }

                e.Cancel = true;
                Visible = false;
            }
        }

        //public void ShowPage (TabDocument tabDocument, IWin32Window owner)
        //{
        //    ui_tdiControl.ButtonBarVisible = false;
        //    Text = tabDocument.Text;
            
        //    tabDocument.InForm = true;
        //    AddPage (tabDocument);

        //    FormClosing -= new FormClosingEventHandler (TabDocumentForm_FormClosing);

        //    //#region Add Dock menu item to system menu.
        //    //IntPtr sysMenuHandle = WindowsAPI.GetSystemMenu (Handle, false);
        //    //WindowsAPI.InsertMenu (sysMenuHandle, 5, WindowsAPI.MF_BYPOSITION | WindowsAPI.MF_SEPARATOR, 0, string.Empty);
        //    //WindowsAPI.InsertMenu (sysMenuHandle, 6, WindowsAPI.MF_BYPOSITION, IDM_DOCK, "Dock Window");
        //    //#endregion

        //    Show (owner);
        //}

        private void uiEvents_tdiControl_AllDocumentsClosed (object sender, EventArgs e)
        {
            Close ();
        }

        private void uiEvents_tdiControl_TabDocumentDockedClicked (object sender, TabDocumentEventArgs e)
        {
            if (TabDocumentDockClicked != null)
                TabDocumentDockClicked (sender, e);
        }

        private void uiEvents_dockMenuItem_Clicked ()
        {
            if (ui_tdiControl.CurrentTabDocument == null)
                return;

            if (TabDocumentDockClicked != null)
                TabDocumentDockClicked (this, new TabDocumentEventArgs (ui_tdiControl.CurrentTabDocument));
        }

        private void TdiControl_TabdocumentClosed (object sender, TabDocumentEventArgs e)
        {
            if (e.TabDocument.WorkArea is IAttributePageControl)
            {
                (e.TabDocument.WorkArea as IAttributePageControl).OnClose ();
                Globals.MainForm.ActiveView.Refresh ();
            }
        }

        //protected override void WndProc (ref Message m)
        //{
        //    if (m.Msg == WindowsAPI.WM_SYSCOMMAND)
        //    {
        //        switch (m.WParam.ToInt32 ())
        //        {
        //            case IDM_DOCK:
        //                uiEvents_dockMenuItem_Clicked ();
        //                return;
        //            default:
        //                break;
        //        }
        //    }

        //    base.WndProc (ref m);
        //}
    }
}
