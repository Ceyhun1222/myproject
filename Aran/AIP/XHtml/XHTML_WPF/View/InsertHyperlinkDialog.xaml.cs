using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RichTextBoxUI.Dialogs;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.UI.Extensibility;
using XHTML_WPF.Classes;

//using Telerik.Windows.Documents.Model;
//using Telerik.Windows.Documents.UI.Extensibility;

namespace XHTML_WPF
{
    /// <summary>
    /// Represents dialog for inserting hyperlinks.
    /// </summary>
    [CustomInsertHyperlink]
    public partial class InsertHyperlinkDialog : RadRichTextBoxWindow, IInsertHyperlinkDialog
    {
        private static readonly string httpProtocol = "http://";

        private bool callbackCalled;
        private Action<string, HyperlinkInfo> insertHyperlinkCallback;
        private Action cancelCallback;

        private string hyperlinkPattern = @"^(((http|https|ftp|file)://)|(mailto:)|(\\)|(onenote:)|(www\.))(\S+)$";

        public string HyperlinkPattern
        {
            get { return hyperlinkPattern; }
            set { hyperlinkPattern = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertHyperlinkDialog"/> class.
        /// </summary>
        public InsertHyperlinkDialog()
        {
            InitializeComponent();
            comboBookmarks.EmptyText = LocalizationManager.GetString("Documents_InsertHyperlinkDialog_SelectBookmark");
            Loaded += (s, a) =>
            {
                
                {
                    if (txtAddress.Text.StartsWith("eaippro://"))
                    {
                        buttonOK.IsEnabled = false;
                        TopGrid.IsEnabled = false;
                    }
                    if (txtAddress.Visibility == Visibility.Visible)
                    {
                        txtAddress.Focus();
                        txtAddress.SelectAll();
                    }
                    else
                    {
                        txtText.Focus();
                        txtText.SelectAll();
                    }
                }
            };

            buttonOK.IsDefault = true;
            buttonCancel.IsCancel = true;

        }
        /// <summary>
        /// Shows the dialog for inserting hyperlinks.
        /// </summary>
        /// <param name="text">The text of the hyperlink.</param>
        /// <param name="currentHyperlinkInfo">The current hyperlink info. Null if we are not in edit mode.</param>
        /// <param name="bookmarkNames">Names of all existing bookmarks.</param>
        /// <param name="insertHyperlinkCallback">The callback that will be called on confirmation to insert the hyperlink.</param>
        /// <param name="cancelCallback">The callback that will be called on cancelation.</param>
        /// <param name="owner">The owner of the dialog.</param>
        public void ShowDialog(string text, HyperlinkInfo currentHyperlinkInfo, IEnumerable<string> bookmarkNames, Action<string, HyperlinkInfo> insertHyperlinkCallback, Action cancelCallback, RadRichTextBox owner)
        {
            ShowDialogInternal(text, currentHyperlinkInfo, bookmarkNames, insertHyperlinkCallback, cancelCallback, owner);
        }
        private void ShowDialogInternal(string text, HyperlinkInfo currentHyperlinkInfo, IEnumerable<string> bookmarkNames, Action<string, HyperlinkInfo> insertHyperlinkCallback, Action cancelCallback, RadRichTextBox owner)
        {
            ResetDialog();

            comboBookmarks.DataContext = bookmarkNames;
            comboURL.DataContext = new List<string> { "GEN 0.1", "GEN 0.2", "GEN 0.3" };
            this.insertHyperlinkCallback = insertHyperlinkCallback;
            this.cancelCallback = cancelCallback;
            callbackCalled = false;
            SetOwner(owner);

            if (text == null)
            {
                txtText.IsEnabled = false;
                txtText.Text = LocalizationManager.GetString("Documents_InsertHyperlinkDialog_SelectionInDocument");
            }
            else
            {
                txtText.IsEnabled = true;
                txtText.Text = text;
            }

            //if (!Lib.IsAIPDBExists && currentHyperlinkInfo == null)
            //{
            //    rbAIP.Visibility = Visibility.Collapsed;
            //    rbURL.IsChecked = true;
            //    ChangeUriUIVisibility(false);
            //}

            if (currentHyperlinkInfo != null)
            {
                PreselectTarget(currentHyperlinkInfo.Target);

                if (!currentHyperlinkInfo.IsAnchor)
                {
                    txtAddress.Text = (currentHyperlinkInfo.NavigateUri != null) ? currentHyperlinkInfo.NavigateUri : string.Empty;
                }
                else
                {
                    //if (currentHyperlinkInfo.NavigateUri.StartsWith("http"))
                    //{
                    PreselectBookmark(currentHyperlinkInfo.NavigateUri);
                    rbBookmark.IsChecked = true;
                    ChangeUriUIVisibility(true);
                    //}
                    //else
                    //{
                    //    PreselectBookmark(currentHyperlinkInfo.NavigateUri);
                    //    rbAIP.IsChecked = true;
                    //    ChangeUriUIVisibility(null);
                    //}
                }
            }
            //else
            //{
            //    if (!Lib.IsAIPDBExists)
            //    {
            //        rbBookmark.IsChecked = true;
            //        ChangeUriUIVisibility(true);
            //    }
            //    else
            //    {
            //        rbAIP.IsChecked = true;
            //        ChangeUriUIVisibility(null);
            //    }
            //}

            ShowDialog();
        }

        private void PreselectTarget(HyperlinkTargets target)
        {
            string targetAsString = target.ToString();
            foreach (RadComboBoxItem item in comboTarget.Items)
            {
                if (targetAsString.Equals(item.Tag))
                {
                    comboTarget.SelectedItem = item;
                    break;
                }
            }
        }

        private void PreselectBookmark(string bookmarkName)
        {
            if (comboBookmarks.Items.Contains(bookmarkName))
            {
                comboBookmarks.SelectedValue = bookmarkName;
            }
            else
            {
                comboBookmarks.SelectedValue = null;
            }
        }

        private void ChangeUriUIVisibility(bool? showBookmarkCombo = null)
        {
            //if (showBookmarkCombo == null)
            //{
            //    comboURL.Visibility = Visibility.Visible;
            //    comboBookmarks.Visibility = Visibility.Collapsed;
            //    txtAddress.Visibility = Visibility.Collapsed;
            //}
            //else 
            if (showBookmarkCombo == true)
            {
                comboBookmarks.Visibility = Visibility.Visible;
                txtAddress.Visibility = Visibility.Collapsed;
                comboURL.Visibility = Visibility.Collapsed;
            }
            else
            {
                comboURL.Visibility = Visibility.Collapsed;
                comboBookmarks.Visibility = Visibility.Collapsed;
                txtAddress.Visibility = Visibility.Visible;
            }
        }

        private void ResetDialog()
        {
            comboTarget.SelectedIndex = 0;
            //this.comboBookmarks.SelectedIndex = 0;
            comboBookmarks.SelectedValue = null;
            txtAddress.Text = string.Empty;
            txtText.Text = string.Empty;
            txtValidation.Text = string.Empty;
            //if (Lib.IsAIPDBExists)
            //{
            //    rbAIP.IsChecked = true;
            //    comboURL.Visibility = Visibility.Visible;
            //    txtAddress.Visibility = Visibility.Collapsed;
            //    comboBookmarks.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
                rbURL.IsChecked = true;
                //rbAIP.Visibility = Visibility.Collapsed;
                comboURL.Visibility = Visibility.Collapsed;
                txtAddress.Visibility = Visibility.Visible;
                comboBookmarks.Visibility = Visibility.Collapsed;
            //}

            
        }

        private void OnOkClicked()
        {
            txtValidation.Text = string.Empty;

            if (txtText.IsEnabled)
            {
                if (string.IsNullOrEmpty(txtText.Text))
                {
                    txtValidation.Text = LocalizationManager.GetString("Documents_InsertHyperlinkDialog_InvalidText");
                    return;
                }
            }

            bool isUrl = (rbURL.IsChecked == true);

            if (isUrl)
            {
                if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
                {
                    txtValidation.Text = LocalizationManager.GetString("Documents_InsertHyperlinkDialog_InvalidAddress");
                    return;
                }
            }
            else
            {
                if (comboBookmarks.SelectedValue == null)
                {
                    txtValidation.Text = LocalizationManager.GetString("Documents_InsertHyperlinkDialog_InvalidBookmark");
                    return;
                }
            }

            HyperlinkInfo hyperlinkInfo = new HyperlinkInfo();

            if (isUrl)
            {
                string navigateUri = txtAddress.Text.Trim();
                if (!Regex.IsMatch(navigateUri, HyperlinkPattern))
                {
                    //navigateUri = InsertHyperlinkDialog.httpProtocol + navigateUri;
                    // navigateUri = InsertHyperlinkDialog.httpProtocol + navigateUri;
                }
                hyperlinkInfo.NavigateUri = navigateUri;
            }
            else
            {
                hyperlinkInfo.NavigateUri = (string)comboBookmarks.SelectedValue;
                hyperlinkInfo.IsAnchor = true;
            }

            string targetStr = ((RadComboBoxItem)comboTarget.SelectedItem).Tag.ToString();
            hyperlinkInfo.Target = (HyperlinkTargets)Enum.Parse(typeof(HyperlinkTargets), targetStr, false);

            string hyperlinkText = txtText.IsEnabled ? txtText.Text : string.Empty;
            if (insertHyperlinkCallback != null)
            {
                insertHyperlinkCallback(hyperlinkText, hyperlinkInfo);
            }
            callbackCalled = true;
            Close();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            OnOkClicked();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            if (!callbackCalled && cancelCallback != null)
            {
                cancelCallback();
            }

            insertHyperlinkCallback = null;
            cancelCallback = null;
            callbackCalled = true;
            Owner = null;
        }

        private void RadRadioButton_Click(object sender, RoutedEventArgs e)
        {
            //if (rbAIP.IsChecked == true) ChangeUriUIVisibility(null);
            //else
                ChangeUriUIVisibility(rbBookmark.IsChecked == true);
        }

    }
}
