using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using MvvmCore;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.RichTextBoxCommands;
using XHTML_WPF.Classes;
using ViewModelBase = MvvmCore.ViewModelBase;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Telerik.Windows.Controls.Primitives;

namespace XHTML_WPF.ViewModel
{
    public class XhtmlEditorViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public bool IsSaved { get; private set; } = false;

        private bool _editable;
        public bool Editable
        {
            get => _editable;
            set
            {
                _editable = value;
                NotifyPropertyChanged(nameof(Editable));
                NotifyPropertyChanged(nameof(ReadOnly));
                NotifyPropertyChanged(nameof(Mode));
                NotifyPropertyChanged(nameof(Title));
            }
        }

        public bool ReadOnly => !Editable;

        private string Mode => ReadOnly ? ", Read-only mode" : ", Edit mode";

        public string Title => $@"XHTML Editor - {Assembly.GetExecutingAssembly().GetName().Version.ToTitleString()}{Mode}";

        private bool _aipExists;
        public bool AipExists
        {
            get => _aipExists;
            set
            {
                _aipExists = value;
                NotifyPropertyChanged(nameof(AipExists));
            }
        }

        private string _htmlText;
        public string HtmlText
        {
            get => _htmlText;
            set
            {
                _htmlText = value;
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
        #region CancelCommand
        RelayCommand _cancelCommand;
        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel, CanCancel));

        public bool CanCancel(object parameter) { return true; }

        public void OnCancel(object parameter)
        {
            _htmlText = null;
            CloseAction?.Invoke();
        }
        #endregion
        #region SaveCommand
        RelayCommand _saveCommand;
        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(OnSave, CanSave));

        public bool CanSave(object parameter)
        {
            return true;
        }

        public void OnSave(object parameter)
        {
            try
            {
                IsSaved = true;
                HtmlText = ConvertHtmltoAipXml(HtmlText);
                
                int warnSize = 2048 * 1024; // 2048 kb show warn, that size is too big
                int maxSize = 8192 * 1024; // 8192 kb show error and prevent to save
                int htmlSize = Encoding.UTF8.GetByteCount(HtmlText);
                string curSize = (htmlSize / 1024).ToString();

                // Checking that XML is correct
                if (!TryParseXML(HtmlText, out string errorMessage))
                {
                    ErrorLog.ShowWarning($@"RichText can`t be converted into xhtml format. The following error occured:{Environment.NewLine}{errorMessage}");
                    IsSaved = false;
                    return;
                }

                // Checking that size not bigger than maxSize
                if (htmlSize > maxSize)
                {
                    ErrorLog.ShowWarning($@"RichText content can`t be bigger than {maxSize / 1024} Kb. Current size is {curSize} Kb. Please change content by reinserting images with smaller size. You can decrease image size by any image editor. ");
                    IsSaved = false;
                    return;
                }

                // Checking that size not bigger than warnSize
                if (htmlSize > warnSize)
                {
                    IsSaved = ErrorLog.ShowYesNoMessage($@"RichText content is recommending to be smaller than {warnSize / 1024} kb. Current size is {curSize} Kb. Are you sure you want to continue saving?");
                }

                if (IsSaved) CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        #endregion

        #region ShowInsertAipSectionDialog
        RelayCommand _ShowInsertAipSectionDialog;
        public RelayCommand ShowInsertAipSectionDialog => _ShowInsertAipSectionDialog ?? (_ShowInsertAipSectionDialog = new RelayCommand(OnShowInsertAipSectionDialog, CanShowInsertAipSectionDialog));

        public bool CanShowInsertAipSectionDialog(object parameter) { return true; }

        public void OnShowInsertAipSectionDialog(object parameter)
        {
            try
            {
                //InsertAipSectionDialog window = new InsertAipSectionDialog();
                //window.ShowDialog();
                //if (window.outputValue != null)
                //{
                //    DocumentPosition start = new DocumentPosition(HtmlText.Document);
                //    DocumentPosition end = new DocumentPosition(this.editor.Document);

                //    this.editor.InsertHyperlink(window.outputValue);
                //}
                //CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
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
                //output = !html.StartsWith("<div ") ? $@"<div>{html}</div>" : html;
                output = !html.StartsWith(@"<div xmlns=""http://www.w3.org/1999/xhtml"">") ?
                    $@"<div xmlns=""http://www.w3.org/1999/xhtml"">{html}</div>" :
                    html;
                // Cleaning xhtml
                output = output
                    .Replace("<p>&nbsp;</p>", "<br />")
                    .Replace("<span>&nbsp;</span>", "<br />")
                    .Replace("<br>", "<br />")
                    .Replace("&nbsp;", " ");

                // Entity converting to Unicode 
                // to prevent unknown entity xml error
                // output = EntityToUnicode(output); // Not optimal method for next EC Toolbox generating

                // Alternate method
                // Decoding html entities
                output = WebUtility.HtmlDecode(output);
                // Replacing all `&` to `&amp;` to fix issues with possible errors like:
                // index.php?lang=2&cPath=bla-bla
                // return error = is unexpected token. Expected token is ; (tried to identify &cPath= as entity)
                output = output.Replace("&", "&amp;");
            }
            return output;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($@"Error in the {ex.TargetSite?.Name}");
            //    return null;
            //}
        }

        public string EntityToUnicode(string html)
        {
            var replacements = new Dictionary<string, string>(StringComparer.InvariantCulture);
            var regex = new Regex("(&[a-zA-Z]{2,11};)");
            foreach (Match match in regex.Matches(html))
            {
                if (!replacements.ContainsKey(match.Value))
                {
                    var unicode = WebUtility.HtmlDecode(match.Value);
                    if (unicode.Length == 1)
                    {
                        replacements.Add(match.Value, string.Concat("&#", Convert.ToInt32(unicode[0]), ";"));
                    }
                }
            }
            foreach (var replacement in replacements)
            {
                html = html.Replace(replacement.Key, replacement.Value);
            }
            return html;
        }

        // Old function, use TryParseXml
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

        /// <summary>
        /// Trying to parse xhtml
        /// </summary>
        /// <param name="xmlCode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool TryParseXML(string xmlCode, out string errorMessage)
        {
            var isWellFormedXml = true;
            errorMessage = String.Empty;
            try
            {
                // First method to check XML
                // XmlReader
                using (var stream = GenerateStreamFromString(xmlCode))
                {
                    using (var reader = XmlReader.Create(stream))
                    {
                        while (reader.Read()) { }
                    }
                }

                // Second method to check XML
                // XmlDocument
                var doc = new XmlDocument();
                doc.LoadXml(xmlCode);

            }
            catch (Exception ex)
            {
                isWellFormedXml = false;
                errorMessage = ex.Message;
            }
            return isWellFormedXml;
        }


        public static Stream GenerateStreamFromString(string s)
        {
            try
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
        #endregion

        public Action CloseAction { get; set; }
    }
}
