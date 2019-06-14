using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.RichTextEditor.UI;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents.Layout;
using Telerik.WinForms.Documents.UI.Extensibility;
using Telerik.WinForms.RichTextEditor;

namespace AIP.BaseLib.Xhtml
{
    public partial class InsertSymbolDialog : RadForm, IInsertSymbolWindow
    {
        RadRichTextBox richTextBox;
        FontFamily initialFont;
        bool isOpen;

        public void Show(Action<char, FontFamily> insertSymbolCallback, FontFamily initialFont, RadRichTextBox owner)
        {
            //this.Owner = richTextBox.InsertSymbolWindow.;
            this.richTextBox = owner;
            var fonts = FontManager.GetRegisteredFonts();
            var fnt = fonts.FirstOrDefault(x => x.DisplayName == "Arial Unicode MS") ?? fonts.FirstOrDefault(x => x.DisplayName == "Arial") ?? fonts.FirstOrDefault(x => x.DisplayName == "Helvetica") ?? fonts.FirstOrDefault(x => x.DisplayName == "Microsoft Sans Serif");
            this.initialFont = fnt;
            this.Show();
        }

        public bool IsOpen {
            get
            {
                return this.isOpen;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            this.isOpen = true;
            base.OnShown(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            this.isOpen = false;
            base.OnActivated(e);
        }
    }
}
