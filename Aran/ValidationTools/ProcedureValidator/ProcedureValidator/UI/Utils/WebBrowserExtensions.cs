using System.Windows;
using System.Windows.Controls;

namespace PVT.UI.Extensions
{
    public static class WebBrowserExtensions
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
        "Html",
        typeof(string),
        typeof(WebBrowserExtensions),
        new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wb = d as WebBrowser;
            wb?.NavigateToString(e.NewValue as string);
        }

        public static readonly DependencyProperty PrintProperty = DependencyProperty.RegisterAttached(
        "Print",
        typeof(string),
        typeof(WebBrowserExtensions),
        new FrameworkPropertyMetadata(OnPrintChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetPrint(WebBrowser d)
        {
            return (string)d.GetValue(PrintProperty);
        }

        public static void SetPrint(WebBrowser d, string value)
        {
            d.SetValue(PrintProperty, value);
        }

        private static void OnPrintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wb = d as WebBrowser;
            var doc = wb?.Document as mshtml.IHTMLDocument2;
            doc?.execCommand("Print", true, null);
        }
    }
}
