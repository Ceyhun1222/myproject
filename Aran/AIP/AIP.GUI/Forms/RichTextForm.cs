using AIP.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AIP.GUI.Classes;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents;
using Telerik.WinForms.Documents.FormatProviders.Html;
using Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx;
using Telerik.WinForms.Documents.FormatProviders.Pdf;
using Telerik.WinForms.Documents.FormatProviders.Rtf;
using Telerik.WinForms.Documents.Model;
using Telerik.WinForms.Documents.TextSearch;

namespace AIP.GUI.Forms
{
    public partial class RichTextForm : Telerik.WinControls.UI.RadForm
    {
        /// <summary>
        /// DB object imported from Main form
        /// </summary>
        private eAIPContext db;

        /// <summary>
        /// DB AIP Page
        /// </summary>
        private AIPPage page;

        /// <summary>
        /// DB AIP Page
        /// </summary>
        private RichTextLib richTextLib;

        public RichTextForm()
        {
            InitializeComponent();
        }

        public RichTextForm(PageType _pageType, DocType _docType, eAIPContext _db , bool regenerate) : this()
        {
            richTextLib = new RichTextLib(_pageType, _docType, _db);
            if (regenerate)
            {
                richTextLib.ResetTemplate();
            }
            
        }

        private void RichTextForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeDocument();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                richTextLib.Save(Editor.Document, page);

                // Removing tmp file
                //if (File.Exists(targetFile)) File.Delete(targetFile);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Preview_Click(object sender, EventArgs e)
        {
            try
            {
               // Editor.Document =  richTextLib.Preview(Editor.Document);
                richTextLib.Preview(Editor.Document);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                //if (File.Exists(targetFile)) File.Delete(targetFile);
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void InitializeDocument()
        {
            try
            {
                Editor.Document = richTextLib.InitializeSource(Editor.Document, ref page);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }
}
