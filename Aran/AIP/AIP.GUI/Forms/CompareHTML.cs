using AIP.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using AIP.BaseLib;
using AIP.GUI.Classes;
using Aran.Aim.Enums;
using EntityFramework.Extensions;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace AIP.GUI.Forms
{
    public partial class CompareHTML : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db;
        private RadContextMenu contextMenu;
        private bool FormLoaded = false;
        private string ClientDirectory ="";
        private List<Compare> list = new List<Compare>();
        private string CurrentAipHtmlDirectory = "";
        private string CurrentAipResultsDirectory = "";
        public CompareHTML()
        {
            InitializeComponent();

            string txt_airac = Lib.IsAIRAC(Lib.CurrentAIP.Effectivedate) ? "-AIRAC" : "";
            string dateAip = Lib.CurrentAIP.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;

            CurrentAipHtmlDirectory = Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", "eAIP");
            CurrentAipResultsDirectory = Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", "results");

            

            string path = Properties.Settings.Default.PathToCompareFolder;

            if (!string.IsNullOrEmpty(path))
            {
                ClientDirectory = path;
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            ChoosenFIlesPath.Text = $@"<html><span style=""color:black"">Selected comparing eAIP folder: <b>" + ClientDirectory + "</b><br/>Current eAIP folder: <b>" + CurrentAipHtmlDirectory + "</b></span></html>";
        }

        private void LanguageManager_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            contextMenu = new RadContextMenu();
            RadMenuItem menuItem1 = new RadMenuItem("Open Left Comparing File");
            menuItem1.Tag = 0;
            menuItem1.Click += contextMenu_Click;
            contextMenu.Items.Add(menuItem1);
            RadMenuItem menuItem2 = new RadMenuItem("Open Right Comparing File");
            menuItem2.Tag = 1;
            menuItem2.Click += contextMenu_Click;
            contextMenu.Items.Add(menuItem2);
            RadMenuItem menuItem3 = new RadMenuItem("Open a Compared File");
            menuItem3.Tag = 2;
            menuItem3.Click += contextMenu_Click;
            contextMenu.Items.Add(menuItem3);
            FormLoaded = true;
        }

        private void contextMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if(sender is RadMenuItem && ((int)((RadMenuItem)sender).Tag) == 0)
                if (fm_rgv.CurrentRow?.DataBoundItem == null) return;
                if (fm_rgv.CurrentRow.DataBoundItem is Compare)
                {
                    int colFile = (int) ((RadMenuItem) sender).Tag;
                    Compare cmp = ((Compare) fm_rgv.CurrentRow.DataBoundItem);
                    string file = colFile == 0 && cmp.OtherFile != "" ? cmp.OtherFile : cmp.AIPProdFile;

                    if (!string.IsNullOrEmpty(file))
                    {
                        string txt_airac = Lib.IsAIRAC(Lib.CurrentAIP.Effectivedate) ? "-AIRAC" : "";
                        string dateAip = Lib.CurrentAIP.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                        
                        string dir = "";
                        if (colFile == 0)
                        {
                            dir = ClientDirectory;
                        }
                        else if(colFile == 1)
                        {
                            dir = CurrentAipHtmlDirectory;
                        }
                        else
                            dir = Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", "results");

                        if (File.Exists(Path.Combine(dir, file)))
                            System.Diagnostics.Process.Start(Path.Combine(dir, file));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public delegate void UpdateDataGridViewDelegate();
        private async Task UpdateDataGridView()
        {
            //PreLongAction();
            try
            {

                //if (InvokeRequired)
                //{
                //    Invoke(new UpdateDataGridViewDelegate(UpdateDataGridView));
                //}
                //else
                //{
                    
                    //return;
                    List<string> lst = Lib.TextSectionList();
                    List<string> files = new List<string>();
                    foreach (var item in lst)
                    {
                        string file = Lib.CurrentAIS.ICAOcountrycode.ToUpperInvariant() + "-" +
                                      Lib.AIPClassToSection(item) + "-" + Lib.CurrentAIS.lang + @".html";
                        files.Add(file);
                    }

                    string AIP_GENERATE = "AD2";

                    List<DB.AirportHeliport> airportHeliportList = db.AirportHeliport
                        .AsNoTracking()
                        .Where(n => n.eAIPID == Lib.CurrentAIP.id)
                        .OrderBy(x => x.LocationIndicatorICAO)
                        .ToList();

                    Func<DB.AirportHeliport, bool> ahQuery = n =>
                        n.Type == CodeAirportHeliport.AD || n.Type == CodeAirportHeliport.AH;
                    foreach (DB.AirportHeliport air in airportHeliportList.Where(ahQuery))
                    {
                        string file = Lib.CurrentAIS.ICAOcountrycode.ToUpperInvariant() + "-AD-2." +
                                      air.LocationIndicatorICAO + "-" + Lib.CurrentAIS.lang + @".html";
                        files.Add(file);
                    }

                    Func<DB.AirportHeliport, bool> ahQueryAD3 = n => n.Type == CodeAirportHeliport.HP;
                    foreach (DB.AirportHeliport air in airportHeliportList.Where(ahQueryAD3))
                    {
                        string file = Lib.CurrentAIS.ICAOcountrycode.ToUpperInvariant() + "-AD-3." +
                                      air.LocationIndicatorICAO + "-" + Lib.CurrentAIS.lang + @".html";
                        files.Add(file);
                    }

                    
                    bool issetAipProdFile = true;
                    bool issetOtherFile = true;

                    if (!Directory.Exists(CurrentAipResultsDirectory))
                    {
                        Directory.CreateDirectory(CurrentAipResultsDirectory);
                    }
                    else
                    {
                        DirectoryInfo dir = new DirectoryInfo(CurrentAipResultsDirectory);

                        foreach (FileInfo fi in dir.GetFiles())
                        {
                            fi.Delete();
                        }
                    }

                    foreach (var file in files)
                    {
                        Compare compareHtml = new Compare();
                        var CurrentAipFileDirectory = Path.Combine(CurrentAipHtmlDirectory, file);
                        var ClientAipFileDirectory = Path.Combine(ClientDirectory, file);

                        if (File.Exists(ClientAipFileDirectory) && File.Exists(CurrentAipFileDirectory))
                        {
                            compareHtml.AIPProdFile = file;
                            compareHtml.OtherFile = file;

                            var task = Compare(ClientAipFileDirectory, CurrentAipFileDirectory,
                                CurrentAipResultsDirectory, file);
                            await task;
                            var result = task.Result;

                            compareHtml.Result = !result ? "Different" : "";

                            list.Add(compareHtml);
                        }
                        else if (File.Exists(ClientAipFileDirectory) && !File.Exists(CurrentAipFileDirectory))
                        {
                            compareHtml.AIPProdFile = "";
                            compareHtml.OtherFile = file;
                            compareHtml.Result = "Right file doesn't exist";
                            list.Add(compareHtml);
                        }
                        else if (!File.Exists(ClientAipFileDirectory) && File.Exists(CurrentAipFileDirectory))
                        {
                            compareHtml.AIPProdFile = file;
                            compareHtml.OtherFile = "";
                            compareHtml.Result = "Left file doesn't exist";
                            list.Add(compareHtml);
                        }

                    }
                    
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            //finally
            //{
            //    PostLongAction();
            //}
        }

        public delegate bool CompareDelegate(string ClientAipFileDirectory, string CurrentAipFileDirectory, string CurrentAipResultsDirectory, string file);
        private async Task<bool> Compare(string ClientAipFileDirectory, string CurrentAipFileDirectory, string CurrentAipResultsDirectory, string file)
        {
            try
            {
                //if (InvokeRequired)
                //{
                //    return (bool)Invoke(new CompareDelegate(Compare),
                //        new object[] { ClientAipFileDirectory , CurrentAipFileDirectory, CurrentAipResultsDirectory, file });
                //}
                //else
                //{
                    bool compareAsHtml = false;

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
                        text = File.ReadAllText(ClientAipFileDirectory);
                        doc.LoadHtml(text.Replace("<br>", "<br />"));
                        from = doc.DocumentNode.SelectSingleNode("//body").InnerHtml;
                    }
                    else
                    {
                        from = HtmlFileToText(ClientAipFileDirectory);
                    }

                    if (compareAsHtml)
                    {
                        text = File.ReadAllText(CurrentAipFileDirectory);
                        doc.LoadHtml(text.Replace("<br>", "<br />"));
                        to = doc.DocumentNode.SelectSingleNode("//body").InnerHtml;
                    }
                    else
                    {
                        to = HtmlFileToText(CurrentAipFileDirectory);
                    }


                    HtmlDiff.HtmlDiff diff = new HtmlDiff.HtmlDiff("<div>" + @from + "</div>", "<div>" + to + "</div>");
                    diff.InsertTagValue = "ins";
                    diff.DeleteTagValue = "del";
                    diff.IgnoreWhitespaceDifferences = true;
                    diff.GenerateID = true;
                    diff.IDName = "test";
                    diff.Ref = "1";


                    var ResultTemplateDirectory =
                        Path.Combine(Lib.MakeAIPWorkingDir, "html", "template-for-result.html");
                Application.DoEvents();
                var diffResult = diff.Build();
                    Application.DoEvents();
                    string content = File.ReadAllText(ResultTemplateDirectory).Replace("{{CONTENT}}", diffResult);

                    var FileWithChangesOnlyText = Path.Combine(CurrentAipResultsDirectory, file + ".1");
                    var CurrentFileOnlyText = Path.Combine(CurrentAipResultsDirectory, file + ".2");
                    var FileWithChanges = Path.Combine(CurrentAipResultsDirectory, file);

                    File.Create(FileWithChangesOnlyText).Dispose();
                    File.WriteAllText(FileWithChangesOnlyText,
                        diffResult.Trim().Replace("\t", "").Replace("\r\n", "").Replace(" ", string.Empty));

                    File.Create(CurrentFileOnlyText).Dispose();
                    File.WriteAllText(CurrentFileOnlyText,
                        ("<?xml version=\"1.0\" encoding=\"utf-8\"?><div>" + to + "</div>").Trim().Replace("\t", "")
                        .Replace("\r\n", "").Replace(" ", string.Empty));

                    var result = true;
                    if (Lib.FileLength(FileWithChangesOnlyText) != Lib.FileLength(CurrentFileOnlyText))
                    {
                        File.Create(FileWithChanges).Dispose();
                        File.WriteAllText(FileWithChanges, content);

                        result = false;
                    }

                    File.Delete(FileWithChangesOnlyText);
                    File.Delete(CurrentFileOnlyText);

                    return result;
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        delegate string HtmlFileToTextDelegate(string filePath);
        private string HtmlFileToText(string filePath)
        {
            try
            {
                if (InvokeRequired)
                {
                    return (string)Invoke(new HtmlFileToTextDelegate(HtmlFileToText),
                        new object[] { filePath });
                }
                else
                {
                    Application.DoEvents();
                    using (var browser = new WebBrowser())
                    {
                        string text = File.ReadAllText(filePath);
                        browser.ScriptErrorsSuppressed = true;
                        browser.Navigate("about:blank");
                        browser?.Document?.OpenNew(false);
                        browser?.Document?.Write(text);
                        Application.DoEvents();
                        return browser.Document?.Body?.InnerText.Replace(Environment.NewLine, "<br />");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }


        private void fm_rgv_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (fm_rgv.CurrentRow.DataBoundItem is Compare)
                {
                    string file = ((Compare)fm_rgv.CurrentRow.DataBoundItem).OtherFile;

                    if (!string.IsNullOrEmpty(file))
                    {
                        string txt_airac = Lib.IsAIRAC(Lib.CurrentAIP.Effectivedate) ? "-AIRAC" : "";
                        string dateAip = Lib.CurrentAIP.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                        var CurrentAipResultsDirectory = Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", "results");

                        if(File.Exists(Path.Combine(CurrentAipResultsDirectory, file)))
                            System.Diagnostics.Process.Start(Path.Combine(CurrentAipResultsDirectory , file));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (fm_rgv.CurrentRow?.DataBoundItem == null) return;
                if (fm_rgv.CurrentRow.DataBoundItem is Compare)
                {
                    string file = ((Compare)fm_rgv.CurrentRow.DataBoundItem).OtherFile;

                    if (!string.IsNullOrEmpty(file))
                    {
                        string txt_airac = Lib.IsAIRAC(Lib.CurrentAIP.Effectivedate) ? "-AIRAC" : "";
                        string dateAip = Lib.CurrentAIP.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                        var CurrentAipResultsDirectory = Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", "results");

                        if (File.Exists(Path.Combine(CurrentAipResultsDirectory, file)))
                            System.Diagnostics.Process.Start(Path.Combine(CurrentAipResultsDirectory, file));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Choose_Click(object sender, EventArgs e)
        {

            try
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();

                string path = Properties.Settings.Default.PathToCompareFolder;

                if (!string.IsNullOrEmpty(path))
                {
                    folderDlg.SelectedPath = path;
                }
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ClientDirectory = folderDlg.SelectedPath;

                    UpdateTitle();

                    Properties.Settings.Default.PathToCompareFolder = folderDlg.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private async void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ClientDirectory))
                {
                    MessageBox.Show($@"Please choose folder for comparing. It is must be an eAIP/ folder with html files.");
                    return;
                }
                list.Clear();
                fm_rgv.DataSource = null;
                await Task.Factory.StartNew(PreLongAction);
                await UpdateDataGridView();
                
                fm_rgv.DataSource = list;

                foreach (GridViewDataColumn col in fm_rgv.Columns)
                {
                    col.ReadOnly = true;
                }

                fm_rgv.Refresh();

                int i = 0;
                foreach (var item in list)
                {
                    if (item.Result.Equals("Different"))
                    {
                        fm_rgv.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                    }
                    else if (item.Result.Contains("Left") || item.Result.Contains("Right"))
                    {
                        fm_rgv.Rows[i].Cells[2].Style.ForeColor =
                            System.Drawing.ColorTranslator.FromHtml("#f27676");
                    }

                    i++;
                }
                await Task.Run(() => PostLongAction());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public delegate void PreLongActionDelegate();
        private void PreLongAction()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new PreLongActionDelegate(PreLongAction), new object[] { });
                }
                else
                {
                    btn_Update.Enabled = false;
                    btn_Choose.Enabled = false;
                    btn_Save.Enabled = false;
                    StartProgress();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public delegate void PostLongActionDelegate();
        private void PostLongAction()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new PostLongActionDelegate(PostLongAction), new object[] { });
                }
                else
                {
                    btn_Update.Enabled = true;
                    btn_Choose.Enabled = true;
                    btn_Save.Enabled = true;
                    StopProgress();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        delegate void StartProgressCallback();
        private void StartProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new StartProgressCallback(StartProgress), null);
            }
            else
            {
                //if (TreeView.SelectedNode != null) // May be treeview has been unselected
                //{
                radWaitingBar1.Visible = true;
                radWaitingBar1.StartWaiting();
                //}
            }
        }

        delegate void StopProgressCallback();
        private void StopProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new StopProgressCallback(StopProgress), null);
            }
            else
            {
                radWaitingBar1.StopWaiting();
                radWaitingBar1.Visible = false;
            }
        }

        private void fm_rgv_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = contextMenu.DropDown;
        }
    }

}
