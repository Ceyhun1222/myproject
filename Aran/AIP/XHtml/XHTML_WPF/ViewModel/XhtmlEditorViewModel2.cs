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

namespace XHTML_WPF.ViewModel
{
    public class XhtmlEditorViewModel2 : ViewModelBase, INotifyPropertyChanged
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
            get => _htmlText; // Import
            set
            {
                
                _htmlText = value; // Export
                //_htmlText = value; // Export
                NotifyPropertyChanged(nameof(HtmlText));
                NotifyPropertyChanged(nameof(HtmlTextTelerik));
                NotifyPropertyChanged(nameof(HtmlTextAIP));
            }
        }

        private string _htmlTextTelerik;
        public string HtmlTextTelerik
        {
            get => _htmlText; 
        }

        private string _htmlTextAIP;
        public string HtmlTextAIP
        {
            get => ConvertHtmltoAipXml(_htmlText);
        }

        private string ImportFromXhtml(string htmlText)
        {
            MatchCollection forEditorContentHeading4 = Regex.Matches(htmlText, "<h4 class=\"Title\">(.*?)</h4>");

            foreach (Match item in forEditorContentHeading4)
            {
                htmlText = htmlText.Replace("<h4 class=\"Title\">" + item.Groups[1].Value + "</h4>", "<p class=\"AIP_H4 \">" + item.Groups[1].Value + "</p>");
            }

            MatchCollection forEditorContentHeading5 = Regex.Matches(htmlText, "<h5 class=\"Sub-title\">(.*?)</h5>");

            foreach (Match item in forEditorContentHeading5)
            {
                htmlText = htmlText.Replace("<h5 class=\"Sub-title\">" + item.Groups[1].Value + "</h5>", "<p class=\"AIP_H5 \">" + item.Groups[1].Value + "</p>");
            }

            return htmlText;
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
                int warnSize = 512 * 1024; // 512 kb show warn
                int maxSize = 1024 * 1024; // 1 Mb show error and prevent to save
                int htmlSize = Encoding.Unicode.GetByteCount(HtmlText);
                string curSize = (htmlSize / 1024).ToString();


                if (htmlSize > maxSize)
                {
                    ErrorLog.ShowWarning($@"RichText content can`t be bigger than 1024 Kb. Current size is {curSize} Kb. Please change content by reinserting images with smaller size. You can decrease image size by any image editor. ");
                    IsSaved = false;
                }
                else if (htmlSize > warnSize)
                {
                    if (ErrorLog.ShowYesNoMessage(
                        $@"RichText content is recommending to be smaller than 512 kb. Current size is {
                                curSize
                            } Kb. Are you sure you want to continue saving?")
                    )
                    {
                        IsSaved = true;
                        CloseAction?.Invoke();
                    }
                    else
                    {
                        IsSaved = false;
                    }
                }
                else if (!TryParseXML(HtmlTextAIP, out string errorMessage))
                {
                    ErrorLog.ShowWarning($@"RichText can`t be converted into xhtml format. The following error occured:{Environment.NewLine}{errorMessage}");
                    IsSaved = false;
                }
                else
                {
                    IsSaved = true;
                    CloseAction?.Invoke();
                }

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

                // Decoding html entities
                output = WebUtility.HtmlDecode(output);

                // Convert to EC Xhtml
                //string fileContent1 = output;
                //MatchCollection headingContent = Regex.Matches(fileContent1, "<p(.*?)</p>");

                //foreach (Match item in headingContent)
                //{
                //    if (item.Groups[1].Value.Contains("class=\"AIP_H4 \""))
                //    {
                //        MatchCollection content = Regex.Matches(item.Groups[1].Value + "</p>", ">(.*?)</p>");
                //        fileContent1 = fileContent1.Replace("<p" + item.Groups[1].Value + "</p>", "<h4 class=\"Title\">" + content[0].Groups[1].Value + "</h4>");
                //    }

                //    if (item.Groups[1].Value.Contains("class=\"AIP_H5 \""))
                //    {
                //        MatchCollection content = Regex.Matches(item.Groups[1].Value + "</p>", ">(.*?)</p>");
                //        fileContent1 = fileContent1.Replace("<p" + item.Groups[1].Value + "</p>", "<h5 class=\"Sub-title\">" + content[0].Groups[1].Value + "</h5>");
                //    }
                //}

                //output = fileContent1;
                // end

                


                // Trying to parse
                //output = FormatXml(output);
            }
            return output;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($@"Error in the {ex.TargetSite?.Name}");
            //    return null;
            //}
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

        bool TryParseXML(string xmlCode, out string errorMessage)
        {
            var isWellFormedXml = true;
            errorMessage = String.Empty;
            try
            {
                using (var stream = GenerateStreamFromString(xmlCode))
                {
                    using (var reader = XmlReader.Create(stream))
                    {
                        while (reader.Read()) { }
                    }
                }
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
