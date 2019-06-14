using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using MvvmCore;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.RichTextBoxCommands;
using XHTML_WPF.Classes;
using ViewModelBase = MvvmCore.ViewModelBase;

namespace XHTML_WPF.ViewModel
{
    public class XhtmlViewerViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _htmlText;
        public string HtmlText
        {
            get => _htmlText;
            set
            {
                _htmlText = ConvertHtmltoAipXml(value);
                NotifyPropertyChanged(nameof(HtmlText));
            }
        }

        #region INotifyPropertyChanged Implementation

        public new event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion
        

        #region Functions
        private string ConvertHtmltoAipXml(string html)
        {
            //try
            //{
            string output = "";
            if (html != null)
            {
                // Checking for root element
                output = !html.StartsWith("<div>") ? $@"<div>{html}</div>" : html;

                // Cleaning xhtml
                output = output
                    .Replace("<p>&nbsp;</p>", "<br />")
                    .Replace("<span>&nbsp;</span>", "<br />")
                    .Replace("<br>", "<br />")
                    .Replace("&nbsp;", " ");

                // Trying to parse
                output = FormatXml(output);
            }
            return output;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($@"Error in the {ex.TargetSite?.Name}");
            //    return null;
            //}
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
                //ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return xml;
            }
        }

        #endregion
        
    }
}
