using AIP.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AIP.BaseLib.Class;
using AIP.DB.Entities;
using AIP.GUI.Classes;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Temporality.CommonUtil.Context;
using HtmlAgilityPack;
using Npgsql;
using Org.BouncyCastle.Utilities;
using Telerik.WinControls;
using Telerik.WinControls.TextPrimitiveUtils;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents.FormatProviders.Html.Parsing;
using WinSCP;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace AIP.GUI.Forms
{
    public partial class Settings : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db = new eAIPContext();
        public bool ResetLayout = false;

        public Settings()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e)
        {
            try
            {
                gbx_Fixes.Visible = Properties.Settings.Default.PCS == "setup";
                tab_Supplement.Item.Visibility = Permissions.Is_Admin() ?  ElementVisibility.Visible : ElementVisibility.Collapsed;
                tab_Tests.Item.Visibility = Permissions.Is_Admin() ? ElementVisibility.Visible : ElementVisibility.Collapsed;
                tab_Transfer.Item.Visibility = Permissions.Is_Admin() ? ElementVisibility.Visible : ElementVisibility.Collapsed;

                Fill_TransferProtocol();
                Fill_OrganizationAuthority();
                Fill_DateFormat();

                chk_IsPreviewExternal.Checked = Properties.Settings.Default.IsPreviewExternal;
                chk_CheckSectionForFilling.Checked = Properties.Settings.Default.CheckSectionForFilling;
                chk_AskReFillSection.Checked = Properties.Settings.Default.AskReFillSection;
                chk_MakeAIPDebug.Checked = Properties.Settings.Default.MakeAIPDebug;
                chk_GeneratePDF.Checked = Properties.Settings.Default.GeneratePDF;
                chk_NilSections.Checked = Properties.Settings.Default.NilSections;
                chk_ShowErrors.Checked = Properties.Settings.Default.ShowErrors;
                chk_ShowWarnings.Checked = Properties.Settings.Default.ShowWarnings;
                chk_ShowOutput.Checked = Properties.Settings.Default.ShowOutput;
                chk_ShowLinks.Checked = Properties.Settings.Default.ShowLinks;
                chk_CreateReport.Checked = Properties.Settings.Default.CreateReport;
                chk_UseCache.Checked = Properties.Settings.Default.UseCache;
                chk_GetBeforePreview.Checked = Properties.Settings.Default.GetBeforePreview;
                chk_PendingData.Checked = Properties.Settings.Default.PendingData;
                chk_PreviewWOAmdt.Checked = Properties.Settings.Default.PreviewWOAmdt;
                tbx_MaxLogMessages.Value = Properties.Settings.Default.MaxLogMessages;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void Fill_TransferProtocol()
        {
            try
            {
                cbx_TransferProtocol.DataSource = Enum.GetValues(typeof(Protocol));
                cbx_TransferProtocol.SelectedValue = db.GetDBConfiguration<string>(Cfg.TransferProtocol);
                tbx_Hostname.Text = db.GetDBConfiguration<string>(Cfg.TransferHost);
                tbx_TransferRemoteFolder.Text = db.GetDBConfiguration<string>(Cfg.TransferRemoteFolder);
                tbx_Username.Text = db.GetDBConfiguration<string>(Cfg.TransferUser);
                tbx_Password.Text = db.GetDBConfiguration<string>(Cfg.TransferPass);
                tbx_Hostkey.Text = db.GetDBConfiguration<string>(Cfg.TransferHostKey);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        //private void Fill_Languages()
        //{
        //    try
        //    {
        //        cbx_eAIPLanguage.ValueMember = "Key";
        //        cbx_eAIPLanguage.DisplayMember = "Value";
        //        List<KeyValuePair<string, string>> ds = db.LanguageReference.AsEnumerable()
        //            .Select(x => new KeyValuePair<string, string>(x.Value, x.Name)).ToList();
        //        cbx_eAIPLanguage.DataSource = ds;
        //        cbx_eAIPLanguage.SelectedValue = ds?.FirstOrDefault(x => x.Key == Properties.Settings.Default.eAIPLanguage).Key;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }
        //}

        private void Fill_OrganizationAuthority()
        {
            try
            {
                if (Lib.CurrentAIP != null)
                {
                    cbx_OrganizationAuthority.ValueMember = "Key";
                    cbx_OrganizationAuthority.DisplayMember = "Value";
                    List<KeyValuePair<string, string>> ds = Globals.GetFeaturesByED(FeatureType.OrganisationAuthority)
                        .OfType<OrganisationAuthority>()
                        .Select(x => new KeyValuePair<string, string>(x.Identifier.ToString(), x.Name)).ToList();
                    cbx_OrganizationAuthority.DataSource = ds;
                    cbx_OrganizationAuthority.SelectedValue =
                        db.GetDBConfiguration<string>(Cfg.OrganizationAuthorityIdentifier);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void Fill_DateFormat()
        {
            try
            {
                cbx_eAIPDateFormat.ValueMember = "Key";
                cbx_eAIPDateFormat.DisplayMember = "Value";
                CultureInfo ci = new CultureInfo("en-US");
                List<string> formats = new List<string>()
                {
                    "dd MMM yyyy", "dd-MMM-yyyy", "yyyy-MM-dd","MM-dd-yyyy", "dd-MM-yyyy",
                    "dd/MMM/yyyy", "yyyy/MM/dd","MM/dd/yyyy", "dd/MM/yyyy",
                    "dd.MMM.yyyy", "yyyy.MM.dd","MM.dd.yyyy", "dd.MM.yyyy"

                };
                List<KeyValuePair<string, string>> dateFormats = new List<KeyValuePair<string, string>>();
                DateTime dt = DateTime.Now;
                foreach (var format in formats)
                {
                    dateFormats.Add(new KeyValuePair<string, string>(format, dt.ToString(format, ci).ToUpperInvariant()));
                }
                cbx_eAIPDateFormat.DataSource = dateFormats;
                cbx_eAIPDateFormat.SelectedValue = Properties.Settings.Default.eAIPDateFormat;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (Lib.CurrentAIP != null && cbx_OrganizationAuthority.SelectedValue != null && cbx_Contact.SelectedValue != null)
                {
                    if ((string)cbx_OrganizationAuthority.SelectedValue != db.GetDBConfiguration<string>(Cfg.ContactName))
                    {
                        db.SetDBConfiguration(Cfg.OrganizationAuthorityIdentifier, cbx_OrganizationAuthority.SelectedValue);
                        db.SetDBConfiguration(Cfg.ContactName, cbx_Contact.SelectedValue);
                        db.SaveChanges();
                    }
                }
                if (cbx_TransferProtocol.SelectedValue != null)
                {
                    db.SetDBConfiguration(Cfg.TransferProtocol, cbx_TransferProtocol.SelectedValue);
                    db.SetDBConfiguration(Cfg.TransferHost, tbx_Hostname.Text);
                    db.SetDBConfiguration(Cfg.TransferRemoteFolder, tbx_TransferRemoteFolder.Text);
                    db.SetDBConfiguration(Cfg.TransferUser, tbx_Username.Text);
                    db.SetDBConfiguration(Cfg.TransferPass, tbx_Password.Text);
                    db.SetDBConfiguration(Cfg.TransferHostKey, tbx_Hostkey.Text);
                    db.SaveChanges();
                }
                //Properties.Settings.Default.ConnectionType = (ConnectionType)cbx_ConnectionType.SelectedValue;
                //Properties.Settings.Default.eAIPLanguage = cbx_eAIPLanguage.SelectedValue.ToString();
                Properties.Settings.Default.eAIPDateFormat = cbx_eAIPDateFormat.SelectedValue.ToString();
                Properties.Settings.Default.IsPreviewExternal = chk_IsPreviewExternal.Checked;
                Properties.Settings.Default.CheckSectionForFilling = chk_CheckSectionForFilling.Checked;
                Properties.Settings.Default.AskReFillSection = chk_AskReFillSection.Checked;
                Properties.Settings.Default.MakeAIPDebug = chk_MakeAIPDebug.Checked;
                Properties.Settings.Default.GeneratePDF = chk_GeneratePDF.Checked;
                Properties.Settings.Default.NilSections = chk_NilSections.Checked;
                Properties.Settings.Default.ShowErrors = chk_ShowErrors.Checked;
                Properties.Settings.Default.ShowWarnings = chk_ShowWarnings.Checked;
                Properties.Settings.Default.ShowOutput = chk_ShowOutput.Checked;
                Properties.Settings.Default.ShowLinks = chk_ShowLinks.Checked;
                Properties.Settings.Default.CreateReport = chk_CreateReport.Checked;
                Properties.Settings.Default.UseCache = chk_UseCache.Checked;
                Properties.Settings.Default.GetBeforePreview = chk_GetBeforePreview.Checked;
                Properties.Settings.Default.PendingData = chk_PendingData.Checked;
                Properties.Settings.Default.PreviewWOAmdt = chk_PreviewWOAmdt.Checked;
                Cache.Enable(chk_UseCache.Checked);
                Properties.Settings.Default.MaxLogMessages = (int)tbx_MaxLogMessages.Value;
                //Opt.Set("MaxLogMessages", tbx_MaxLogMessages.Text.ToInt());
                //Properties.Settings.Default.MaxLogMessages = tbx_MaxLogMessages.Text.ToInt();
                Properties.Settings.Default.Save();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void CancelClick(object sender, EventArgs e)
        {
            try
            {
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_ResetLayout_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmResult = MessageBox.Show(@"Are you sure to reset Layout?",
                        @"Confirm Reset",
                        MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    if (File.Exists(Lib.LayoutFile)) File.Delete(Lib.LayoutFile);
                    ResetLayout = true;
                    ErrorLog.ShowInfo("Layout has been deleted");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void cbx_OrganizationAuthority_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                if (Lib.CurrentAIP != null && cbx_OrganizationAuthority.SelectedValue != null)
                {
                    OrganisationAuthority oa = Globals.GetFeaturesByED(FeatureType.OrganisationAuthority)
                        .OfType<OrganisationAuthority>()
                        .FirstOrDefault(x => x.Identifier.ToString() == cbx_OrganizationAuthority.SelectedValue.ToString());

                    if (oa != null && oa.Contact.Any())
                    {
                        cbx_Contact.ValueMember = "Key";
                        cbx_Contact.DisplayMember = "Value";
                        List<KeyValuePair<string, string>> ds = oa.Contact
                            .Select(x => new KeyValuePair<string, string>(x.Name, x.Name)).ToList();
                        cbx_Contact.DataSource = ds;
                        cbx_Contact.SelectedValue = db.GetDBConfiguration<string>(Cfg.ContactName);
                    }

                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void chk_ShowPass_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (sender is RadCheckBox)
            {
                RadCheckBox chk = (RadCheckBox)sender;
                tbx_Password.PasswordChar = (chk.Checked) ? new char() : '*';
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var badAip = db.eAIP.Where(x => x.Amendment == null).ToList();
                if (badAip.Any())
                {
                    List<int?> eAIP_IDs = badAip.Select(x => x?.id).ToList();
                    var badSection = db.AIPSection
                        .Where(x => eAIP_IDs.Contains(x.eAIPID)).ToList();
                    db.AIPSection.RemoveRange(badSection);
                    db.eAIP.RemoveRange(badAip);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                MongoDBGridFS mongo = new MongoDBGridFS();
                SetTestResult(lbl_mongo, mongo.TestConnection());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            try
            {
                SetTestResult(lbl_aipdb, IsSQLServerConnected());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public bool IsSQLServerConnected()
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(DB.SingleConnection.String))
                {
                    try
                    {
                        conn.Open();
                        var command = new NpgsqlCommand("SELECT 1", conn);
                        command.ExecuteScalar();
                        conn.Close();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            try
            {
                SetTestResult(lbl_toss, Globals.IsConnected());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                Enum.TryParse(db.GetDBConfiguration<string>(Cfg.TransferProtocol), out Protocol protocol);
                Transfer.TransferParams = new TransferParams()
                {
                    HostName = db.GetDBConfiguration<string>(Cfg.TransferHost),
                    UserName = db.GetDBConfiguration<string>(Cfg.TransferUser),
                    Password = db.GetDBConfiguration<string>(Cfg.TransferPass),
                    Protocol = protocol
                };
                Transfer.Initialize();
                SetTestResult(lbl_transfer, Transfer.IsConnected());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void SetTestResult(RadLabel lbl, bool isConnected)
        {
            try
            {
                lbl.Text = isConnected
                    ? @"<html><span style=""color:green"">Connection successful</span>"
                    : @"<html><span style=""color:red"">Connection failed</span>";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            try
            {
                bool compareAsHtml = true;
                string file1 = @"C:\Users\emins\Desktop\Old.html";
                var doc = new HtmlAgilityPack.HtmlDocument()
                {
                    OptionOutputOriginalCase = true,
                    OptionCheckSyntax = false,
                    OptionOutputAsXml = false,
                    OptionFixNestedTags = true,
                    OptionWriteEmptyNodes = true,
                    OptionAutoCloseOnEnd = true
                };
                var from = "";
                var to = "";
                string text = "";
                if (compareAsHtml)
                {
                    text = File.ReadAllText(file1);
                    doc.LoadHtml(text.Replace("<br>", "<br />"));
                    from = doc.DocumentNode.SelectSingleNode("//body").InnerHtml;
                }
                else
                {
                    from = HtmlFileToText(file1);
                }
                string file2 = @"C:\Users\emins\Desktop\New.html";
                if (compareAsHtml)
                {
                    text = File.ReadAllText(file2);
                    doc.LoadHtml(text.Replace("<br>", "<br />"));
                    to = doc.DocumentNode.SelectSingleNode("//body").InnerHtml;
                }
                else
                {
                    to = HtmlFileToText(file2);
                }

                string template = @"
<div>
<style>
ins {
	text-decoration: none; 
    background-color: #ffcccc !important;
}
del {
    background-color: #cc9999 !important;
}
</style>

{CONTENT}

</div>
";

                HtmlDiff.HtmlDiff diff = new HtmlDiff.HtmlDiff("<div>" + @from + "</div>", "<div>" + to + "</div>");
                diff.InsertTagValue = "ins";
                diff.DeleteTagValue = "del";
                diff.IgnoreWhitespaceDifferences = true;
                diff.GenerateID = true;
                diff.IDName = "test";
                diff.Ref = "1";

                string content = template.Replace("{CONTENT}", diff.Build());
                // Lets add a block expression to group blocks we care about (such as dates)
                // diffHelper.AddBlockExpression(new Regex(@"[\d]{1,2}[\s]*(Jan|Feb)[\s]*[\d]{4}", RegexOptions.IgnoreCase));
                File.WriteAllText(@"C:\Users\emins\Desktop\Html.html", content);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public string HtmlFileToText(string filePath)
        {
            try
            {
                using (var browser = new WebBrowser())
                {
                    string text = File.ReadAllText(filePath);
                    browser.ScriptErrorsSuppressed = true;
                    browser.Navigate("about:blank");
                    browser?.Document?.OpenNew(false);
                    browser?.Document?.Write(text);
                    return browser.Document?.Body?.InnerText.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        public string Convert(string path)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(path);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public string ConvertHtml(string html)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }


        private void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            Program.ClearEntityFrameworkCache();
        }
    }
}
