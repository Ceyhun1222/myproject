using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace Aran.Controls
{
    public partial class TDIControl : UserControl
    {
        private TabDocument _currentTabDocument;
        private List<ButtonPagePair> _pageList;
        private List<TabDocument> _pageDocuments;

        public event TabDocumentEventHandler TabDocumentFloatClicked;
        public event TabDocumentEventHandler TabDocumentFloatWithClicked;
        public event TabDocumentEventHandler TabDocumentDockedClicked;
        public event TabDocumentEventHandler TabdocumentClosed;
        public event EventHandler AllDocumentsClosed;

        public TDIControl ()
        {
            InitializeComponent ();

            _pageList = new List<ButtonPagePair> ();
            _pageDocuments = new List<TabDocument> ();
        }

        public void AddPage (TabDocument page)
        {
            PageButton button = new PageButton ();
            button.CheckedChanged += new EventHandler (PageButton_CheckedChanged);
            button.CloseClicked += new EventHandler (Button_CloseClicked);
            button.FloatClicked += new EventHandler (Button_FloatClicked);
            button.FloatWithClicked += new EventHandler (Button_FloatWithClicked);
            button.DockedClicked += new EventHandler (Button_DockedClicked);
            button.Text = page.Text;
            button.VisibleContextStrip = page.VisibleContextStrip;
            button.InForm = page.InForm;

            page.WorkArea.Visible = false;

            _pageList.Add (new ButtonPagePair (page, button));
            _pageDocuments.Add (page);
            ui_pageBarFlowLayoutPanel.Controls.Add (button);
            toolTip1.SetToolTip (button, button.Text);
            page.WorkArea.Dock = DockStyle.Fill;
            ui_workAreaPanel.Controls.Add (page.WorkArea);

            button.Checked = true;

            page.PropertyChanged += new PropertyChangedEventHandler (TabDocument_PropertyChanged);
            page.PageClosed += new EventHandler (Page_PageClosed);
        }

        public TabDocument [] PageDocuments
        {
            get { return _pageDocuments.ToArray (); }
        }

        public TabDocument CurrentTabDocument
        {
            get
            {
                return _currentTabDocument;
            }
            set
            {
                if (_currentTabDocument != value)
                {
                    _currentTabDocument = value;

                    PageButton button = GetButton (_currentTabDocument);
                    button.Checked = true;
                }
            }
        }

        public bool ButtonBarVisible
        {
            get { return ui_buttonBarPanel.Visible; }
            set { ui_buttonBarPanel.Visible = value; }
        }

        
        private void Button_CloseClicked (object sender, EventArgs e)
        {
            PageButton button = (PageButton) sender;
            if (button != _pageList [0].Button)
                _pageList [0].Button.Checked = true;
            else if (_pageList.Count > 1)
                _pageList [1].Button.Checked = true;

            for (int i = 0; i < _pageList.Count; i++)
            {
                ButtonPagePair pair = _pageList [i];

                if (TabdocumentClosed != null)
                    TabdocumentClosed (this, new TabDocumentEventArgs (pair.Page));

                if (pair.Button.Equals (button))
                {
                    ui_pageBarFlowLayoutPanel.Controls.Remove (button);
                    ui_workAreaPanel.Controls.Remove (pair.Page.WorkArea);

                    _pageDocuments.RemoveAt (i);
                    _pageList.RemoveAt (i);

                    pair.Page.PageClosed -= new EventHandler (Page_PageClosed);
                    pair.Page.PropertyChanged -= new PropertyChangedEventHandler (TabDocument_PropertyChanged);
                    break;
                }
            }

            if (AllDocumentsClosed != null && _pageDocuments.Count == 0)
                AllDocumentsClosed (this, e);
        }

        private void Button_FloatClicked (object sender, EventArgs e)
        {
            if (TabDocumentFloatClicked == null)
                return;

            PageButton button = (PageButton) sender;
            TabDocument tabDocument = GetPage (button);
            TabDocumentFloatClicked (this, new TabDocumentEventArgs (tabDocument));
        }

        private void Button_FloatWithClicked (object sender, EventArgs e)
        {
            if (TabDocumentFloatWithClicked == null)
                return;

            PageButton button = (PageButton) sender;
            TabDocument tabDocument = GetPage (button);
            TabDocumentFloatWithClicked (this, new TabDocumentEventArgs (tabDocument));
        }

        private void Button_DockedClicked (object sender, EventArgs e)
        {
            if (TabDocumentDockedClicked == null)
                return;

            PageButton button = (PageButton) sender;
            TabDocument tabDocument = GetPage (button);
            TabDocumentDockedClicked (this, new TabDocumentEventArgs (tabDocument));
        }

        private void Page_PageClosed (object sender, EventArgs e)
        {
            TabDocument page = sender as TabDocument;
            PageButton button = GetButton (page);
            Button_CloseClicked (button, e);
        }

        private void TabDocument_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            TabDocument tabDocument = (TabDocument) sender;
        }

        private TabDocument GetPage (PageButton button)
        {
            foreach (ButtonPagePair pair in _pageList)
            {
                if (pair.Button.Equals (button))
                    return pair.Page;
            }
            return null;
        }

        private PageButton GetButton (TabDocument page)
        {
            foreach (ButtonPagePair pair in _pageList)
            {
                if (pair.Page.Equals (page))
                    return pair.Button;
            }
            return null;
        }

        private void PageButton_CheckedChanged (object sender, EventArgs e)
        {
            PageButton button = (PageButton) sender;

            if (!button.Checked)
                return;

            foreach (ButtonPagePair pair in _pageList)
            {
                pair.Page.WorkArea.Visible = (pair.Button.Equals (button));
            }

            _currentTabDocument = GetPage (button);
        }
    }

    internal class ButtonPagePair
    {
        public ButtonPagePair (TabDocument page, PageButton button)
        {
            Page = page;
            Button = button;
        }

        public TabDocument Page { get; set; }
        public PageButton Button { get; set; }
    }

    public class TabDocumentEventArgs : EventArgs 
    {
        public TabDocumentEventArgs (TabDocument tabDocument)
        {
            TabDocument = tabDocument;
        }

        public TabDocument TabDocument { get; private set; }
    }

    public delegate void TabDocumentEventHandler (object sender, TabDocumentEventArgs e);
}
