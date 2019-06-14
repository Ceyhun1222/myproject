using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using AIP.BaseLib.Xhtml;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.RichTextEditor.UI;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents.FormatProviders.Html;
using Telerik.WinForms.Documents.Layout;
using Telerik.WinForms.Documents.Model;
using Telerik.WinForms.Documents.RichTextBoxCommands;
using Telerik.WinForms.RichTextEditor.RichTextBoxUI.Menus;

namespace AIP.BaseLib
{
    public partial class XHtmlEditor : UserControl
    {
        bool suspendToggleEvents = false;
        private string _value;
        private FontFamily fnt;
        HtmlFormatProvider provider = new HtmlFormatProvider();

        public string Value
        {
            get => ConvertHTMLToAIPXML(ConvertRichTextToHTML(Editor.Document));
            set
            {
                provider.ImportSettings = UseHTMLImportOptions();
                Editor.Document = provider.Import(value);
                ChangeFont();
            }
        }

        public XHtmlEditor()
        {
            InitializeComponent();
            //Editor.RichTextBoxElement.InsertSymbolWindow = new InsertSymbolDialog();
        }
        // Always paste as Plain text to prevent any problems with visualization
        // All should be made manually using controls on the form
        void Editor_CommandExecuting(object sender, CommandExecutingEventArgs e)
        {
            if (e.Command is PasteCommand)
            {
                e.Cancel = true;
                this.Editor.Insert(Clipboard.GetText());
            }
        }

        private void ContextMenu_Showing(object sender, Telerik.WinForms.RichTextEditor.RichTextBoxUI.Menus.ContextMenuEventArgs e)
        {
            e.Cancel = true;
        }

        private void Command_ToggleStateChanged(object sender, StylePropertyChangedEventArgs<bool> e)
        {
            suspendToggleEvents = true;

            if (sender.GetType().Name.Equals("ToggleBoldCommand"))
            {
                btn_Bold.ToggleState = e.NewValue ? ToggleState.On : ToggleState.Off;
            }
            else if (sender.GetType().Name.Equals("ToggleItalicCommand"))
            {
                btn_Italic.ToggleState = e.NewValue ? ToggleState.On : ToggleState.Off;
            }
            suspendToggleEvents = false;
        }


        private void radRichTextEditor1_TextChanged(object sender, EventArgs e)
        {
            Editor.Focus();
        }

        private string ConvertRichTextToHTML(RadDocument richtext)
        {
            string output = "";
            HtmlFormatProvider provider2 = new HtmlFormatProvider();
            provider2.ExportSettings = UseHTMLExportOptions();
            output = provider2.Export(richtext);
            //output = $@"<span>{output}</span>";
            return System.Net.WebUtility.HtmlDecode(output);
        }

        private string ConvertHTMLToAIPXML(string HTML)
        {
            string output = "";

            output = FormatXml(HTML.Replace("<p>&nbsp;</p>", "<br />").Replace("<span>&nbsp;</span>", "<br />"));
            return output;
        }

        string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception ex)
            {
                return xml;
            }
        }

        private static HtmlExportSettings UseHTMLExportOptions()
        {
            HtmlExportSettings set = new HtmlExportSettings
            {
                ExportBoldAsStrong = true,
                ExportItalicAsEm = true,
                SpanExportMode = SpanExportMode.DefaultBehavior,
                DocumentExportLevel = DocumentExportLevel.Fragment,
                ExportEmptyDocumentAsEmptyString = true,
                ExportFontStylesAsTags = true,
                ExportHeadingsAsTags = true,
                ExportStyleMetadata = false,
                StyleRepositoryExportMode = StyleRepositoryExportMode.DontExportStyles,
                StylesExportMode = StylesExportMode.Inline
                
            };

            set.PropertiesToIgnore["span"].AddRange(new string[] { "color", "text-decoration", "font-family", "font-size", "dir" });
            set.PropertiesToIgnore["p"].AddRange(new string[] { "margin-top", "margin-bottom", "margin-left", "margin-right", "line-height", "text-indent", "text-align", "direction" });
            set.PropertiesToIgnore["table"].AddRange(new string[] { "border-top", "border-bottom", "border-left", "border-right", "table-layout", "margin-left", "border-spacing" });
            set.PropertiesToIgnore["td"].AddRange(new string[] { "border-top", "border-bottom", "border-left", "border-right", "padding", "vertical-align" });

            return set;
        }

        private static HtmlImportSettings UseHTMLImportOptions()
        {
            HtmlImportSettings set = new HtmlImportSettings();
            return set;
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {

            Editor.Commands.ToggleBoldCommand.ToggleStateChanged += new EventHandler<StylePropertyChangedEventArgs<bool>>(Command_ToggleStateChanged);
            Editor.Commands.ToggleItalicCommand.ToggleStateChanged += new EventHandler<StylePropertyChangedEventArgs<bool>>(Command_ToggleStateChanged);
            Editor.Commands.ToggleBulletsCommand.ToggleStateChanged += new EventHandler<StylePropertyChangedEventArgs<bool>>(Command_ToggleStateChanged);
            Editor.Commands.ToggleNumberedCommand.ToggleStateChanged += new EventHandler<StylePropertyChangedEventArgs<bool>>(Command_ToggleStateChanged);

            Editor.IsContextMenuEnabled = true;
            Editor.IsSelectionMiniToolBarEnabled = false;
            Telerik.WinControls.RichTextEditor.UI.ContextMenu contextMenu = (Telerik.WinControls.RichTextEditor.UI.ContextMenu)Editor.RichTextBoxElement.ContextMenu;
            contextMenu.Showing += this.ContextMenu_Showing;
            Editor.CommandExecuting += Editor_CommandExecuting;
            Editor.InputHandler = new StrictInputBehavior(Editor.RichTextBoxElement);

            //ChangeFont();

        }

        private void ChangeFont()
        {
            Editor.Document.Selection.SelectAll();
            var fonts = FontManager.GetRegisteredFonts();
            fnt = fonts.FirstOrDefault(x => x.DisplayName == "Arial Unicode MS") ?? fonts.FirstOrDefault(x => x.DisplayName == "Arial") ?? fonts.FirstOrDefault(x => x.DisplayName == "Helvetica") ?? fonts.FirstOrDefault(x => x.DisplayName == "Microsoft Sans Serif");
            Editor.ChangeFontFamily(fnt);
            //Editor.ChangeFontSize(13);
            Editor.Document.Selection.Clear();
            Editor.Document.History.Clear();
            
        }

        private void ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs e)
        {
            if (suspendToggleEvents)
                return;
            if ((sender as CommandBarToggleButton).Name.EndsWith("Bold"))
                Editor.ToggleBold();
            else if ((sender as CommandBarToggleButton).Name.EndsWith("Italic"))
                Editor.ToggleItalic();
            Editor.Focus();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if ((sender as CommandBarButton).Name.EndsWith("Link"))
            {
                Editor.ShowInsertHyperlinkDialog();
            }
            else if ((sender as CommandBarButton).Name.EndsWith("Table"))
            {
                Editor.ShowInsertTableDialog();
            }
            else if ((sender as CommandBarButton).Name.EndsWith("Symbols"))
            {
                Editor.ShowInsertSymbolWindow();
            }
        }
    }

}