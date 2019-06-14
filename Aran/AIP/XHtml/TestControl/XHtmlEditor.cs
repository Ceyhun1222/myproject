using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.RichTextEditor.UI;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents.FormatProviders.Html;
using Telerik.WinForms.Documents.FormatProviders.Rtf;
using Telerik.WinForms.Documents.FormatProviders.Xaml;
using Telerik.WinForms.Documents.Model;
using Telerik.WinForms.Documents.RichTextBoxCommands;
using Telerik.WinForms.Documents.TextSearch;
using Telerik.WinForms.RichTextEditor;
using Telerik.WinForms.RichTextEditor.RichTextBoxUI.Menus;

namespace XHtmlEditor
{
    public partial class XHtmlEditor : UserControl
    {
        bool suspendToggleEvents = false;
        
        public XHtmlEditor()
        {
            InitializeComponent();

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
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
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
            //if (!this.Editor.Document.Selection.IsEmpty)
            //{
            //    RadMenuItem menuItem = new RadMenuItem()
            //    {
            //        Text = "Change selection foreground"
            //    };
            //    menuItem.Click += this.OnChangeSelectionForeground;
            //    ContextMenuGroup contextMenuGroup = new ContextMenuGroup();
            //    contextMenuGroup.Add(menuItem);
            //    e.ContextMenuGroupCollection.Add(contextMenuGroup);

            //}

            // Removing Font style and Paragraph indents
            e.ContextMenuGroupCollection?.Where(n => n.Type == ContextMenuGroupType.TextEditCommands)?.FirstOrDefault()?.ToList()?.ForEach(n => n.Visibility = Telerik.WinControls.ElementVisibility.Collapsed);

        }
        private void OnChangeSelectionForeground(object sender, EventArgs e)
        {
            this.Editor.ChangeTextForeColor(Colors.Red);
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
            else if (sender.GetType().Name.Equals("ToggleBulletsCommand"))
            {
                btn_ul.ToggleState = e.NewValue ? ToggleState.On : ToggleState.Off;
            }
            else if (sender.GetType().Name.Equals("ToggleNumberedCommand"))
            {
                btn_ol.ToggleState = e.NewValue ? ToggleState.On : ToggleState.Off;
            }



            suspendToggleEvents = false;
        }


        private void radRichTextEditor1_TextChanged(object sender, EventArgs e)
        {



            radTextBox2.Text = ConvertHTMLToAIPXML(ConvertRichTextToHTML(Editor.Document));
            //TextReader sr = new StringReader(radTextBox2.Text);
            //radTextBox1.Text = FromHtml(sr).OuterXml;
            wb.DocumentText = radTextBox2.Text;
            Editor.Focus();
        }

        private string ConvertRichTextToHTML(RadDocument richtext)
        {
            string output = "";
            HtmlFormatProvider provider2 = new HtmlFormatProvider();
            provider2.ExportSettings = UseHTMLExportOptions();
            output = provider2.Export(richtext);
            output = $@"<span>{output}</span>";
            return output;
        }

        private string ConvertHTMLToAIPXML(string HTML)
        {
            string output = "";

            output = FormatXml(HTML.Replace("<p>&nbsp;</p>", "<br />").Replace("<span>&nbsp;</span>", "<br />"));//.Replace("<", "<x:").Replace("<x:/", "</x:");
            //output = HTML.Replace("<p>&nbsp;</p>", "<br />").Replace("<span>&nbsp;</span>", "<br />").Replace("<", "<x:").Replace("<x:/", "</x:");
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
            HtmlExportSettings set = new HtmlExportSettings();

            set.ExportBoldAsStrong = true;
            set.ExportItalicAsEm = true;
            set.SpanExportMode = SpanExportMode.DefaultBehavior;
            set.DocumentExportLevel = DocumentExportLevel.Fragment;
            set.ExportEmptyDocumentAsEmptyString = true;
            set.ExportFontStylesAsTags = true;
            set.ExportHeadingsAsTags = true;
            set.ExportStyleMetadata = false;
            set.StyleRepositoryExportMode = StyleRepositoryExportMode.DontExportStyles;
            set.StylesExportMode = StylesExportMode.Inline;
            set.PropertiesToIgnore["span"].AddRange(new string[] { "color", "font-family", "font-size", "dir" });
            set.PropertiesToIgnore["p"].AddRange(new string[] { "margin-top", "margin-bottom", "margin-left", "margin-right", "line-height", "text-indent", "text-align", "direction" });
            //set.PropertiesToIgnore["table"].AddRange(new string[] { "border-top", "border-bottom", "border-left", "border-right", "table-layout", "margin-left", "border-spacing" });
            //set.PropertiesToIgnore["td"].AddRange(new string[] { "border-top", "border-bottom", "border-left", "border-right", "padding", "vertical-align" });

            return set;
        }

        private static HtmlImportSettings UseHTMLImportOptions()
        {
            HtmlImportSettings set = new HtmlImportSettings();
            return set;
        }

        //XmlDocument FromHtml(TextReader reader)
        //{
        //    // setup SgmlReader
        //    Sgml.SgmlReader s = new Sgml.SgmlReader();
        //    s.DocType = "HTML";
        //    s.WhitespaceHandling = WhitespaceHandling.All;
        //    s.CaseFolding = Sgml.CaseFolding.ToLower;
        //    s.InputStream = reader;

        //    // create document
        //    XmlDocument doc = new XmlDocument();
        //    doc.PreserveWhitespace = true;
        //    doc.XmlResolver = null;
        //    doc.Load(s);
        //    return doc;
        //}


        private void RadForm1_Load(object sender, EventArgs e)
        {
           // this.WindowState = FormWindowState.Maximized;
            //string text = Properties.Resources.Init.Replace("<x:", "<").Replace("</x:", "</");
            HtmlFormatProvider provider2 = new HtmlFormatProvider();
            provider2.ImportSettings = UseHTMLImportOptions();
            //Editor.Document = provider2.Import(text);

            radRichTextEditor1_TextChanged(null, null);

        }


        private void ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs e)
        {
            if (suspendToggleEvents)
                return;


            if ((sender as CommandBarToggleButton).Name.EndsWith("Bold"))
                Editor.ToggleBold();
            else if ((sender as CommandBarToggleButton).Name.EndsWith("Italic"))
                Editor.ToggleItalic();
            else if ((sender as CommandBarToggleButton).Name.EndsWith("_ol"))
            {
                ToggleNumberedCommand command = new ToggleNumberedCommand(Editor.RichTextBoxElement.ActiveEditor);
                command.Execute();
            }
            else if ((sender as CommandBarToggleButton).Name.EndsWith("_ul"))
            {
                ToggleBulletsCommand command = new ToggleBulletsCommand(Editor.RichTextBoxElement.ActiveEditor);
                command.Execute();
            }

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

        }

    }

}