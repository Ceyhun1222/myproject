using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Aran.Temporality.CommonUtil.View
{
    /// <summary>
    /// Interaction logic for XmlViewerControl.xaml
    /// </summary>
    public partial class XmlViewerControl : UserControl
    {
        public static readonly DependencyProperty XmlDocumentProperty =
          DependencyProperty.Register("XmlDocument",
          typeof(XmlDocument),
          typeof(XmlViewerControl),
          new PropertyMetadata(default(XmlViewerControl), OnXmlDocumentChanged));

        private static void OnXmlDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = d as XmlViewerControl;
            if (viewer == null) return;
            viewer.XmlDocument = e.NewValue as XmlDocument;
        }


        private XmlDocument _xmldocument;
        public XmlViewerControl()
        {
            InitializeComponent();
        }

        public XmlDocument XmlDocument
        {
            get { return _xmldocument; }
            set
            {
                _xmldocument = value;
                BindXmlDocument();
            }
        }

        private void BindXmlDocument()
        {
            if (_xmldocument == null)
            {
                xmlTree.ItemsSource = null;
                return;
            }

            var provider = new XmlDataProvider { Document = _xmldocument };
            var binding = new Binding { Source = provider, XPath = "child::node()" };
            xmlTree.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        }
    }
}
