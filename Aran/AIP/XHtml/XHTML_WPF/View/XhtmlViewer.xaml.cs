using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Layout;
using XHTML_WPF.Classes;
using XHTML_WPF.ViewModel;
using ContextMenu = Telerik.Windows.Controls.RichTextBoxUI.ContextMenu;

namespace XHTML_WPF.View
{
    /// <summary>
    /// Interaction logic for XhtmlViewer.xaml
    /// </summary>
    public partial class XhtmlViewer : UserControl
    {
        public XhtmlViewer()
        {
            InitializeComponent();
        }
        private void XhtmlViewer_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetDefaultFont();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                HtmlFormatProvider provider = new HtmlFormatProvider();
                Editor.Document = provider.Import(value);
            }
        }

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text",typeof(string),typeof(XhtmlViewer),new PropertyMetadata(null));

        private void SetDefaultFont()
        {
            try
            {
                Editor.Document.Selection.SelectAll();
                var fonts = FontManager.GetRegisteredFonts();
                var fnt = fonts.FirstOrDefault(x => x.DisplayName == "Arial Unicode MS") ?? fonts.FirstOrDefault(x => x.DisplayName == "Arial") ?? fonts.FirstOrDefault(x => x.DisplayName == "Helvetica") ?? fonts.FirstOrDefault(x => x.DisplayName == "Microsoft Sans Serif");
                Editor.ChangeFontFamily(fnt);
                Editor.Document.Selection.Clear();
                Editor.Document.History.Clear(); //  to prevent undo set default font
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

    }
}
