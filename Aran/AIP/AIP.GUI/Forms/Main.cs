using AIP.DB;
using AIP.XML;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Package;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using AIP.GUI.Forms;
using System.Data.SqlClient;
using System.Transactions;
using Telerik.WinControls.UI;
using AIP.GUI.Properties;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Collections;
using System.Configuration;
using Telerik.WinControls.UI.Docking;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;
using Telerik.WinForms.Documents.Model;
using System.Xml.Linq;
using AIP.GUI.Classes;
using System.Timers;
using System.Globalization;
using InteractivePreGeneratedViews;
using System.Data.Entity.Infrastructure;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using AIP.BaseLib.Class;
using AIP.DataSet.Classes;
using AIP.DB.Entities;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.Internal.Remote.ClientServer;
using EntityFramework.Extensions;
using HtmlAgilityPack;
using iTextSharp.text.pdf;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using WinSCP;
using XHTML_WPF;
using XHTML_WPF.ViewModel;
using AirportHeliport = AIP.DB.AirportHeliport;
using Application = System.Windows.Forms.Application;
using eAICreference = AIS.XML.eAICreference;
using eAICs = AIS.XML.eAICs;
using eSUPreference = AIS.XML.eSUPreference;
using eSUPs = AIS.XML.eSUPs;
using Expression = System.Linq.Expressions.Expression;
using Filter = Aran.Aim.Data.Filters.Filter;
using FontStyle = System.Drawing.FontStyle;
using Languageversion = AIS.XML.Languageversion;
using MessageBox = System.Windows.Forms.MessageBox;
using Size = System.Drawing.Size;

namespace AIP.GUI
{
    public partial class Main : RadForm
    {
        #region AIP Declaration

        public eAIPContext db = new eAIPContext();

        ProgressForm progress;
        private string GenMessage = "", OutputFile = "", Arguments = "";
        private string DefaultArguments, DefaultInputFile, DefaultOutputFile, DefaultPdfOutputFile;
        private List<string> DefaultPDFArguments = new List<string>();
        private string AIP_GENERATE = "";
        private string AIP_FILL = "";
        private static WebBrowser wb;
        //private static Awesomium.Windows.Forms.WebControl wb;
        //List<string> ActivatedMenus = new List<string>() { "GEN01", "GEN02", "GEN11", "GEN12", "GEN13", "GEN14", "GEN15", "GEN16", "GEN17", "GEN22", "GEN24", "GEN26", "GEN27", "GEN41", "GEN42", "ENR11", "ENR12", "ENR13", "ENR14", "ENR15", "ENR16", "ENR17", "ENR18", "ENR19", "ENR110", "ENR111", "ENR112", "ENR113", "ENR114", "ENR21", "ENR22", "ENR31", "ENR32", "ENR33", "ENR34", "ENR35", "ENR36", "ENR41", " ENR42", "ENR43", "ENR44", "ENR45", "ENR51", "ENR54", "ENR56", "AD11", "AD12", "AD13", "AD14" };

        private static AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
        private static AutoResetEvent errorWaitHandle = new AutoResetEvent(false);
        private BackgroundWorker WaitMenu = new BackgroundWorker();
        private static bool IsRunning = false;
        private static bool IsAllRunning = false;
        private static bool IsUserTerminated = false;
        private static bool RefreshSection = false;
        private static bool IsPublishProcess = false;
        Dictionary<string, object> ComplPropList = new Dictionary<string, object>();
        private static System.Windows.Application XhtmlControl;
        RadContextMenu SupAicMenu = new RadContextMenu();
        private SupAicStates SupAicStates = SupAicStates.None;
        private bool clicked = false;
        #endregion

        #region Arguments processing
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_COPYDATA)
            {
                ShowMe();
                // Extract the file name
                NativeMethods.COPYDATASTRUCT copyData = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.COPYDATASTRUCT));
                int dataType = (int)copyData.dwData;
                if (dataType == 2)
                {
                    string arg = Marshal.PtrToStringAnsi(copyData.lpData);
                    OpenArguments(arg);
                }
                else
                {
                    MessageBox.Show(String.Format("Unrecognized data type = {0}.", dataType), Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Maximized;
            }
            // get our current "TopMost" value (ours will always be false though)
            bool top = TopMost;
            // make our form jump to the top of everything
            TopMost = true;
            // set it back to whatever it was
            TopMost = top;
        }

        public void OpenArguments(string arg)
        {
            try
            {
                if (arg.StartsWith("eaippro://") && arg.Contains("/OpenFile/"))
                {
                    AddOutput($@"URL Query request processing: {arg}");
                    var param = Lib.GetParams(arg);
                    if (param.ContainsKey("identifier"))
                    {
                        Guid.TryParse(param["identifier"], out Guid Identifier);
                        if (Identifier != default(Guid))
                        {
                            var file = db.AIPFile
                                .Where(x => x.Identifier == Identifier)
                                .Include(x => x.AIPFileData)
                                .OrderByDescending(x => x.Version)
                                .FirstOrDefault();
                            AddOutput($@"Opening file: {file?.FileName}");
                            var path = Lib.SaveAIPFile(file);
                            if (path != "" && File.Exists(path))
                            {
                                System.Diagnostics.Process.Start(path);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        #endregion


        public Main(string arg)
        {
            try
            {
                InitializeComponent();
                Arguments = arg;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void InitXMLMenu()
        {
            try
            {
                string defPath = Path.Combine(Lib.CurrentDir, "Settings",
                    $@"menu_{Properties.Settings.Default.eAIPLanguage}.xml");
                if (Properties.Settings.Default.eAIPLanguage != null && File.Exists(defPath))
                {
                    LoadXML(defPath);
                }
                else
                    LoadXML(Path.Combine(Lib.CurrentDir, "Settings", "menu.xml"));
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }

        private void Context_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode != null && sender is RadMenuItem) // May be treeview has been unselected
                {
                    RadTreeNode selectedNode = TreeView.SelectedNode;
                    string section = selectedNode.Name;
                    IsAllRunning = (Properties.Settings.Default.GetBeforePreview) ? true : false;
                    switch ((ContextOptions)((RadMenuItem)sender).Tag)
                    {
                        case ContextOptions.Open:
                            State.LastClick = RGVStates.TreeViewClicked;
                            if (!WaitMenu.IsBusy)
                            {
                                WaitMenu.RunWorkerAsync();
                            }
                            break;
                        case ContextOptions.Import:
                            //Regex digitsOnly = new Regex(@"[^\d]");
                            GetData(section);
                            break;
                        case ContextOptions.Generate:
                            Menu_GenerateAction(section);
                            break;
                        case ContextOptions.HtmlPreview:
                            if (IsAllRunning) GetData(section);
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                while (IsAllRunning) { }
                                PreviewSection(false);
                            }).Start();
                            break;
                        case ContextOptions.HtmlAmdtPreview:
                            if (IsAllRunning) GetData(section);
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                while (IsAllRunning) { }
                                PreviewSection(true);
                            }).Start();
                            break;
                        case ContextOptions.PdfPreview:
                            if (IsAllRunning) GetData(section);
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                while (IsAllRunning) { }
                                PreviewSection(false, false);
                            }).Start();
                            break;
                        case ContextOptions.PdfAmdtPreview:
                            if (IsAllRunning) GetData(section);
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                while (IsAllRunning) { }
                                PreviewSection(true, false);
                            }).Start();
                            break;
                        case ContextOptions.None:
                        default:
                            break;
                    }

                }


                ////selectedNode.Name;

                //GridViewRowInfo currentRow = RGV.CurrentRow;
                //GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;

                //if (current.DataBoundItem is BaseEntity)
                //{
                //    BaseEntity Entity = (BaseEntity)current.DataBoundItem;
                //    UniFrm frm = new UniFrm(Entity);
                //    if (frm.ShowDialog() == DialogResult.OK)
                //    {
                //        RGV.BeginUpdate();
                //        db.SaveChanges();
                //        RGV.EndUpdate();
                //    }
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private async void SupAicContext_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRowInfo currentRow = (SupAicStates)SupAicMenu.DropDown.Tag == SupAicStates.SupClicked ? rgv_SupChanges.CurrentRow : rgv_AIC.CurrentRow;
                GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;

                if (sender is RadMenuItem && (current?.DataBoundItem is DB.Supplement || current?.DataBoundItem is DB.Circular)
                    )
                {
                    dynamic dataBound = current.DataBoundItem;

                    switch ((ContextOptions)((RadMenuItem)sender).Tag)
                    {
                        case ContextOptions.Open:
                            if (current?.DataBoundItem is DB.Supplement)
                                await OpenSupplement(dataBound);
                            else
                                await OpenCircular(dataBound);
                            break;
                        case ContextOptions.HtmlPreview:
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                PreviewSection(false, true, dataBound);
                            }).Start();
                            break;
                        case ContextOptions.PdfPreview:
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                PreviewSection(false, false, dataBound);
                            }).Start();
                            break;
                        case ContextOptions.None:
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void GetData(string section)
        {
            try
            {
                AIP_FILL = section;
                RefreshSection = true;
                MainFillDataset(AIP_FILL);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        //private void RunOutputShownTask(Action<eAIPContext> action, Queue<BaseLib.Struct.Output> output)
        //{
        //    try
        //    {
        //        PreLongAction();
        //        Task task = Task.Factory.StartNew(() => action(db));
        //        for (; ; )
        //        {
        //            if (UserTerminated()) return;
        //            Application.DoEvents();
        //            lock (output)
        //            {
        //                while (output.Count > 0)
        //                {
        //                    BaseLib.Struct.Output msg = output.Dequeue();
        //                    AddOutput(msg.Message, msg.Percent, null, msg.Color);
        //                }
        //                if (task.IsCompleted || task.IsFaulted) break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }
        //    finally
        //    {
        //        PostLongAction();
        //    }
        //}


        private void RunOutputShownTask(Task task, Queue<BaseLib.Struct.Output> output)
        {
            try
            {
                PreLongAction();
                for (; ; )
                {
                    if (UserTerminated()) return;
                    Application.DoEvents();
                    lock (output)
                    {
                        while (output.Count > 0)
                        {
                            BaseLib.Struct.Output msg = output.Dequeue();
                            AddOutput(msg.Message, msg.Percent, null, msg.Color);
                        }
                        if (task.IsCompleted || task.IsFaulted) break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                PostLongAction();
            }
        }

        //private void RunOutputShownTask(Action action, Queue<BaseLib.Struct.Output> output)
        //{
        //    try
        //    {
        //        PreLongAction();
        //        Task task = Task.Factory.StartNew(action);
        //        for (; ; )
        //        {
        //            if (UserTerminated()) return;
        //            Application.DoEvents();
        //            lock (output)
        //            {
        //                while (output.Count > 0)
        //                {
        //                    BaseLib.Struct.Output msg = output.Dequeue();
        //                    AddOutput(msg.Message, msg.Percent, null, msg.Color);
        //                }
        //                if (task.IsCompleted || task.IsFaulted) break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }
        //    finally
        //    {
        //        PostLongAction();
        //    }
        //}

        private void PostGenerate()
        {
            try
            {
                RunOutputShownTask(Task.Factory.StartNew(() => WebGenerate.Run(db)), WebGenerate.OutputQueue);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void Upload()
        {
            try
            {
                if (!CurrentAIPSelected()) return;
                if ((int)Lib.CurrentAIS.Status < (int)Status.Published)
                {
                    ErrorLog.ShowWarning($@"Attention: Current AIS package not yet in Publish status and can`t be uploaded");
                    return;
                }
                DownloadPublication();
                //if (Lib.CurrentAIS.Status == Status.Uploaded)
                //{
                //    ErrorLog.ShowWarning($@"Attention: Current AIS package is already uploaded");
                //    return;
                //}
                Enum.TryParse(db.GetDBConfiguration<string>(Cfg.TransferProtocol), out Protocol protocol);
                Transfer.TransferParams = new TransferParams()
                {
                    HostName = db.GetDBConfiguration<string>(Cfg.TransferHost),
                    UserName = db.GetDBConfiguration<string>(Cfg.TransferUser),
                    Password = db.GetDBConfiguration<string>(Cfg.TransferPass),
                    Protocol = protocol,
                    LocalDirectory = Lib.OutputDir,
                    RemoteDirectory = Path.Combine(db.GetDBConfiguration<string>(Cfg.TransferRemoteFolder), Lib.RemoteOutputDir),
                };
                Transfer.Initialize();
                RunOutputShownTask(Task.Factory.StartNew(Transfer.Sync), Transfer.OutputQueue);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void DownloadPublication()
        {
            try
            {
                if (!CurrentAIPSelected()) return;
                //if ((int)Lib.CurrentAIS.Status < (int)Status.Published)
                //{
                //    ErrorLog.ShowWarning($@"Attention: Current AIS package not yet in Publish status and can`t be downloaded from database.");
                //    return;
                //}
                RunOutputShownTask(Task.Factory.StartNew(() => FileDBManager.Download(
                new List<FileDBManager.Folder>
                {
                            new FileDBManager.Folder(Lib.SourceDir, "Source"),
                            new FileDBManager.Folder(Lib.OutputDir, "Output")
                }
            )),
            FileDBManager.OutputQueue);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void CopyPublication()
        {
            try
            {
                if (!CurrentAIPSelected()) return;
                if ((int)Lib.CurrentAIS.Status < (int)Status.Published)
                {
                    ErrorLog.ShowWarning($@"Attention: Current AIS package not yet in Publish status and can`t be downloaded from database.");
                    return;
                }

                // Show dialog
                var SaveFolderDialog = new FolderBrowserDialog();
                DialogResult result = SaveFolderDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string saveFolder = SaveFolderDialog.SelectedPath;
                    RunOutputShownTask(Task.Factory.StartNew(() => FileDBManager.Download(
                            new List<FileDBManager.Folder>
                            {
                                new FileDBManager.Folder(Lib.OutputDir, "Output", false, saveFolder)
                            }
                        )),
                        FileDBManager.OutputQueue);
                    AddOutput($@"AIP has been successfully copied into {saveFolder}", null, null, Color.Green);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void AISContext_Click(object sender, EventArgs e)
        {
            try
            {
                RadListDataItem currentRow = RadListControl_AIS.SelectedItem;
                string path = "";

                if (currentRow?.DataBoundItem is DB.eAISpackage)
                {

                    switch ((AISContextOptions)((MenuItem)sender).Tag)
                    {
                        case AISContextOptions.New:
                            NewAIS();
                            break;
                        case AISContextOptions.Open:
                            OpenAIS();
                            break;
                        case AISContextOptions.Activate:
                            ActivateAIS();
                            break;
                        case AISContextOptions.Amdt:
                            OpenAmdt();
                            break;
                        case AISContextOptions.OpenSourceFolder:
                            path = Lib.GetPath(Lib.PathType.SourceFolder, (DB.eAISpackage)currentRow?.DataBoundItem);
                            if (Directory.Exists(path))
                            {
                                Process.Start("explorer.exe", path);
                            }
                            break;
                        case AISContextOptions.OpenOutputFolder:
                            path = Lib.GetPath(Lib.PathType.OutputFolder, (DB.eAISpackage)currentRow?.DataBoundItem);
                            if (Directory.Exists(path))
                            {
                                Process.Start("explorer.exe", path);
                            }
                            break;
                        case AISContextOptions.OpenOutputIndex:
                            path = Lib.GetPath(Lib.PathType.OutputIndex, (DB.eAISpackage)currentRow?.DataBoundItem);
                            if (File.Exists(path))
                            {
                                Process.Start(path);
                            }
                            break;
                        case AISContextOptions.None:
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void FindRecursive(RadTreeNode treeNode)
        {
            foreach (RadTreeNode tn in treeNode.Nodes)
            {
                tn.AllowDrop = false;
                FindRecursive(tn);
            }
        }

        public DataTable GetRecords(DateTime dtParameter)
        {
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * from yourTable where DateField = @dateparameter"))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@dateparameter", dtParameter);
                    SqlDataReader dr = cmd.ExecuteReader();
                    //...rest of the code
                    dt.Load(dr);
                }
            }
            return dt;
        }



        /// <summary>
        /// Updating Selected AIS information in the label
        /// </summary>
        /// <param name="ais">Active AIS</param>
        /// <param name="pais">Previous AIS</param>
        public delegate void UpdateAISTextboxDelegate(DB.eAISpackage ais, DB.eAISpackage pais = null);
        private void UpdateAISTextbox(DB.eAISpackage ais, DB.eAISpackage pais = null)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new UpdateAISTextboxDelegate(UpdateAISTextbox),
                        new object[] { ais, pais });
                }
                else
                {
                    string format = Properties.Settings.Default.eAIPDateFormat;
                    CultureInfo ci = new CultureInfo("en-US");
                    //String Language = CultureInfo.GetCultureInfo(ais.lang)?.EnglishName;
                    //var CurrentLanguage = (!String.IsNullOrEmpty(Language)) ?  Language : ais.lang;
                    var CurrentLanguage = ais.lang;
                    string color = (ais.Status == Status.Work) ? "green" : "black";
                    lbl_AipTxt.Text =
                        $@"<html><span style=""color:black"">Active eAIP Effective date: <b>{
                                ais.Effectivedate.ToString(format, ci).ToUpperInvariant()
                            }</b>, Publishing date: <b>{
                                ais.Publicationdate?.ToString(format, ci).ToUpperInvariant()
                            }</b>, Language: <b>{CurrentLanguage}</b>, Status: <b><span style=""color: {color}"">{ais.Status}</span></b></span>";
                    if (pais != null)
                        lbl_AipTxt.Text +=
                            $@"; <span style=""color:gray"">Previous eAIP Effective date: <b>{
                                    pais.Effectivedate.ToString(format, ci).ToUpperInvariant()
                                }</b>, Publishing date: <b>{
                                    pais.Publicationdate?.ToString(format, ci).ToUpperInvariant()
                                }</b>, Language: <b>{CurrentLanguage}</b>, Status: <b>{pais.Status}</b></span></html>";
                    RefreshTitle();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the UpdateAIPTextbox: {ex.Message}", true);
            }
        }


        /// <summary>
        /// Clear AIS information in the label
        /// </summary>
        private void ClearAISTextbox()
        {
            try
            {
                lbl_AipTxt.Text = "";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the UpdateAIPTextbox: {ex.Message}", true);
            }
        }

        private string TitleVersion()
        {
            try
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                return $@"{Program.Name} v. {Program.Version()}, eAIP Server: {GetSQLServerName()}";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        private string GetSQLServerName()
        {
            try
            {
                //string connectString = ConfigurationManager.ConnectionStrings[eAIPContext.DefaultConnectionName].ToString();
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
                //string DataSource = builder.DataSource;
                //string Server = (DataSource.Contains(@"\")) ? DataSource.GetBeforeOrEmpty(@"\") : DataSource;
                return eAIPContext.ServerName();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return String.Empty;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                NativeMethodsInit();
                this.Visible = false;
                this.WindowState = FormWindowState.Minimized;
                InitXMLMenu();
                InitTreeView();
                InitSupGridView();
                InitListView();
                LoadLayout();
                // Improve second time speed by caching in first time
                CreateDatabaseViews();
                


                if (!Permissions.ManagaAisPackages()) rmi_NewAIS.Visibility = ElementVisibility.Collapsed;
                if (!Permissions.CanPublishAIP()) rmi_Run_All.Visibility = ElementVisibility.Collapsed;
                if (!Permissions.CanUploadAIPtoWebsite()) rmi_Upload.Visibility = ElementVisibility.Collapsed;
                if (!Permissions.CanUploadFiles()) rmi_FileManager.Visibility = ElementVisibility.Collapsed;
                if (!Permissions.CanManageLanguageTexts()) rmi_lm.Visibility = ElementVisibility.Collapsed;
                if (!Permissions.CanManageAbbreviations()) rmi_Abbrev.Visibility = ElementVisibility.Collapsed;

                // CreateAIPMenuItems();
                //await Task.Run(()=> CreateAIPMenuItems());
                //RGV.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.FullRowSelect;
                //ShowDBLog();

                //var binding = new Binding("Text", db.eAIP.Where(n => n.AIPStatus == AIPStatus.Work && n.id == Properties.Settings.Default.CurrentAIPID).FirstOrDefault(), null);
                //binding.Format += delegate (object sentFrom, ConvertEventArgs convertEventArgs)
                //{
                //    Dataset.eAIP eaip = convertEventArgs.Value as Dataset.eAIP;
                //    convertEventArgs.Value = "eAIP Effective date: " + eaip.Effectivedate?.ToShortDateString() + Environment.NewLine + "Publishing date: " + eaip.Publicationdate?.ToShortDateString();
                //};
                //lbl_AipTxt.DataBindings.Add(binding);
                //db.BaseEntity.LoadAsync();
                //rlv_eAIPs.DataSource = db.eAIP.ToList();

                //rlv_eAIPs.DataMember = "id";



                WaitMenu.WorkerReportsProgress = true;
                WaitMenu.WorkerSupportsCancellation = true;
                WaitMenu.DoWork += new DoWorkEventHandler(WaitMenu_DoWork);
                WaitMenu.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WaitMenu_RunWorkerCompleted);

                FillDB.db = db;

                //checking for languages
                using (eAIPContext db_options = new eAIPContext())
                {
                    if (!db_options.eAIPOptions.Any())
                    {
                        eAIPOptions options = new eAIPOptions();
                        db_options.eAIPOptions.Add(options);

                        LanguageReference lref = new LanguageReference();
                        lref.Name = "English";
                        lref.Value = "en-GB";
                        lref.AIXMValue = language.ENG;
                        lref.eAIPOptions = options;

                        db_options.LanguageReference.Add(lref);
                        options.LanguageReferences.Add(lref);

                        lref = new LanguageReference();
                        lref.Name = "Latvian";
                        lref.Value = "lv-LV";
                        lref.AIXMValue = language.LV;
                        lref.eAIPOptions = options;
                        db_options.LanguageReference.Add(lref);
                        options.LanguageReferences.Add(lref);
                        db_options.SaveChanges();
                    }
                }
                ////////////////
                using (eAIPContext db_options = new eAIPContext())
                {
                    Lib.DBOptions = db_options.eAIPOptions?.Include("LanguageReferences").AsNoTracking().ToList()
                        .FirstOrDefault();
                }


                UpdateLanguages();
                if (Properties.Settings.Default.CurrentAISID != 0)
                {
                    DB.eAISpackage aisd = db.eAISpackage.Find(Properties.Settings.Default.CurrentAISID);
                    aisd = db.eAISpackage.Where(n => n.id == Properties.Settings.Default.CurrentAISID)
                        .OrderBy(x => x.Effectivedate)
                        .Include(n => n.eAIPpackage.eAIP.Amendment.Group)
                        .FirstOrDefault();

                    if (aisd != null)
                    {
                        eAIS_Activated(aisd);
                    }
                }
                GenerateWindowsMenu();
                GenerateAIPLanguageMenu();
                UpdateAISList();

                if (!String.IsNullOrEmpty(Arguments)) OpenArguments(Arguments);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                Program.CloseSplash();
                Visible = true;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void CreateDatabaseViews()
        {
            try
            {
                // Try 1
                InteractiveViews.SetViewCacheFactory(db,
                    new FileViewCacheFactory(DB.DbContextConfiguration.EFCachePath + "DBViews.xml"));
            }
            catch (Exception)
            {
                // Seems cache is conflicted
                Program.ClearEntityFrameworkCache();

                try
                {
                    // Try 2, seriously trying and detecting error
                    InteractiveViews.SetViewCacheFactory(db,
                                new FileViewCacheFactory(DB.DbContextConfiguration.EFCachePath + "DBViews.xml"));
                }
                catch (Exception ex)
                {
                    ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                }
            }
        }

        private void NativeMethodsInit()
        {
            try
            {
                NativeMethods.CHANGEFILTERSTRUCT changeFilter = new NativeMethods.CHANGEFILTERSTRUCT();
                changeFilter.size = (uint)Marshal.SizeOf(changeFilter);
                changeFilter.info = 0;
                if (!NativeMethods.ChangeWindowMessageFilterEx(this.Handle, NativeMethods.WM_COPYDATA, NativeMethods.ChangeWindowMessageFilterExAction.Allow, ref changeFilter))
                {
                    int error = Marshal.GetLastWin32Error();
                    MessageBox.Show($@"The error {error} occured.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void LoadLayout()
        {
            try
            {
                if (File.Exists(Lib.LayoutFile)) radDock1.LoadFromXml(Lib.LayoutFile);
                else radDock1.RestoreWindowsStatesAfterLoad();
                splitPanel6.Dock = DockStyle.Fill;
                splitPanel7.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void FillAIPList()
        {
            try
            {
                List<eAISpackage> pk = db.eAISpackage?.Where(n => n.lang == Lib.AIPLanguage)?.Include(n => n.eAIPpackage.eAIP.Amendment).OrderByDescending(n => n.Effectivedate).ToList();
                if (pk?.Count > 0)
                {
                    RadListControl_AIS.DataSource = pk;
                }
                else
                {
                    RadListControl_AIS.DataSource = null;
                }
                RadListControl_AIS.Rebind();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public delegate void AddOutputDelegate(string Action, int? percent = null, BackgroundWorker bgw = null, Color? color = null);
        public void AddOutput(string Message, int? percent = null, BackgroundWorker bgw = null, Color? color = null)
        {
            if (InvokeRequired)
            {
                Invoke(new AddOutputDelegate(AddOutput),
                new object[] { Message, percent, bgw, color });
            }
            else
            {
                if (String.IsNullOrEmpty(Message)) return;
                string str = Lib.CurrentDirFunc().Substring(0, 3);
                if (Properties.Settings.Default.ShowLinks && Message?.Contains(str) == true)
                {
                    Message = Message
                        .Replace("Program Files (x86)", "Program%20Files%20(x86)")
                        .Replace(@"""", @"")
                        ;
                    // C:\ or D:\
                    //string path = Message.ToStringBetween(str,".").Replace(' ', (char)160); // C:\Program files
                    //Message = Message.Replace(str+ path + ".", @"file://" + str + path + ".");
                    Message = Message.Replace(str, @"file://" + str);
                }
                if (color == null) color = Color.Black;
                if (log_output.Text.Length == 0)
                    log_output.AppendColorText(Message, color);
                else
                    log_output.AppendColorText(Environment.NewLine + Message, color);

                log_output.ScrollToCaret();
                if (Properties.Settings.Default.CreateReport) Report.Write(Message);
                //if (percent != null) radProgressBar1.Value1 = percent ?? 0;
            }
        }

        public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        private QueryInfo GetQIForAIP(string AIPSection)
        {
            try
            {
                QueryInfo qi = new QueryInfo();
                //if (Lib.NotRequiredSections.Contains(AIPSection)) return qi;//Yes, return null object, but not null

                string file = $@"{AssemblyDirectory}\Queries\{AIPSection}.qls";
                if (!File.Exists(file))
                {
                    //   ErrorLog.ShowMessage($@"File {file} doesn`t exist");
                    return qi;
                    //return null;
                }

                System.IO.FileStream fs = new System.IO.FileStream(file, FileMode.Open);
                BinaryPackageReader reader = new BinaryPackageReader(fs);
                qi.Unpack(reader);
                return qi;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the GetQIForAIP method for section {AIPSection}: {ex.Message}", true);
                return null;
            }
        }


        //private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{

        //    //string Dir = AssemblyDirectory + @"\Data";
        //    BackgroundWorker worker = sender as BackgroundWorker;
        //    worker.ReportProgress(10);
        //    GenMessage = "";
        //    OutputFile = "";
        //    AddOutput("Getting information from Database", 20, worker);


        //    QueryInfo qi = GetQIForAIP(AIP_FILL);
        //    if (qi == null)
        //    {
        //        AddOutput("Error occured", 0, worker, Color.Red); return;
        //    }
        //    string file = "", Arguments = "";

        //    // Converting int 31 to string 3.1 or int 422 to string 4.2.2
        //    string str = string.Join(".", AIP_FILL.ToString().ToCharArray());

        //    List<Feature> featList = new List<Feature>();
        //    AimComplexTable CT = new AimComplexTable();

        //    // Getting all features
        //    CT = AimFLComplexLoader.LoadQueryInfo(qi);
        //    featList = CT.GetAllFeatures();
        //    AddOutput("Writing information into Dataset", 60, worker);

        //    //GenerateAIPSection<XML.ENR44>(featList);

        //    AddOutput("Operation complete", 100, worker);
        //}

        // This event handler updates the progress.
        //private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    progress.Message = "In progress, please wait... " + e.ProgressPercentage.ToString() + "%";
        //    progress.ProgressValue = e.ProgressPercentage;
        //}

        private List<Feature> GetFeatures(FeatureType featureType, Filter filter)
        {
            var gr = Globals.dbPro.GetVersionsOf(
                featureType,
                Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE,
                Guid.Empty,
                true,
                null,
                null,
                filter);

            if (!gr.IsSucceed)
            {
                throw new Exception(gr.Message);
            }

            return gr.GetListAs<Feature>();
        }

        //// This event handler deals with the results of the background operation.
        //private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    progress.Close();
        //    DialogResult dialogResult = MessageBox.Show(Environment.NewLine + GenMessage + Environment.NewLine +
        //        "Do you want to open this file?", "Result", MessageBoxButtons.YesNo);
        //    if (dialogResult == DialogResult.Yes && OutputFile.EndsWith("html"))
        //    {
        //        System.Diagnostics.Process.Start(OutputFile);
        //    }
        //    else if (dialogResult == DialogResult.No)
        //    {
        //        //do something else
        //    }
        //}

        /// <summary>
        /// Convert Class to XML file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Class"></param>
        /// <param name="xmlFileName"></param>
        private void AIPXMLWrite<T>(T Class, string xmlFileName)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("e", "http://www.eurocontrol.int/xmlns/AIM/eAIP");
                ns.Add("x", "http://www.w3.org/1999/xhtml");
                ns.Add("xlink", "http://www.w3.org/1999/xlink");
                ConvertClassToXML(Class, xmlFileName, ns);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the AIPXMLWrite", ex, true);
                return;
            }
        }

        private void AISXMLWrite<T>(T Class, string xmlFileName)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("p", "http://www.eurocontrol.int/xmlns/AIM/eAIS-package");
                //ns.Add("x", "http://www.w3.org/1999/xhtml");
                ns.Add("xlink", "http://www.w3.org/1999/xlink");
                ConvertClassToXML(Class, xmlFileName, ns);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the AIPXMLWrite: {ex.Message}");
                return;
            }
        }

        private static void ConvertClassToXML<T>(T ais, string xmlFileName, XmlSerializerNamespaces ns, Dictionary<string, string> child = null)
        {
            var serializer = new XmlSerializer(typeof(T));
            XmlDocument xmlDocument = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();

            XmlNode docNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(docNode);


            using (MemoryStream stream = new MemoryStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                serializer.Serialize(stream, ais, ns);
                stream.Position = 0;
                xmlDocument.Load(stream);
                if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    XmlDeclaration dec = (XmlDeclaration)xmlDocument.FirstChild;
                    dec.Encoding = "UTF-8";
                }
                else
                {
                    XmlDeclaration dec = xmlDocument.CreateXmlDeclaration("1.0", null, null);
                    dec.Encoding = "UTF-8";
                    xmlDocument.InsertBefore(dec, xmlDocument.DocumentElement);
                }
                if (child != null)
                {
                    foreach (var item in child.Reverse())
                    {
                        xmlDocument.InsertAfter(xmlDocument.CreateProcessingInstruction(
                                        item.Key,
                                        item.Value), xmlDocument.FirstChild);
                    }
                }
                xmlDocument.Save(xmlFileName);

                // temporary check
                // must be improved
                File.WriteAllText(xmlFileName, File.ReadAllText(xmlFileName)?.Replace("&gt;", ">").Replace("&lt;", "<"));
                //

                stream.Close();
            }
        }

        private Guid? GetPointId(EnRouteSegmentPoint point)
        {
            if (point != null)
            {
                switch (point.PointChoice.Choice)
                {
                    case SignificantPointChoice.DesignatedPoint:
                        return point.PointChoice.FixDesignatedPoint.Identifier;
                    case SignificantPointChoice.Navaid:
                        return point.PointChoice.NavaidSystem.Identifier;
                    case SignificantPointChoice.TouchDownLiftOff:
                        return point.PointChoice.AimingPoint.Identifier;
                    case SignificantPointChoice.RunwayCentrelinePoint:
                        return point.PointChoice.RunwayPoint.Identifier;
                    case SignificantPointChoice.AirportHeliport:
                        return point.PointChoice.AirportReferencePoint.Identifier;
                    case SignificantPointChoice.AixmPoint:
                        //no id here
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return null;
        }

        private List<Aran.Aim.Features.RouteSegment> SortSegments(List<Aran.Aim.Features.RouteSegment> relatedSegments, bool ShowErrors = true)
        {
            if (relatedSegments == null || relatedSegments.Count < 2) return relatedSegments;

            //first is the one that not second
            var endIds = relatedSegments.Select(t => GetPointId(t.End)).ToList();

            var independent = relatedSegments.Where(t => !endIds.Contains(GetPointId(t.Start))).ToList();
            if (independent.Count == 0)
            {
                if (ShowErrors)
                    throw new Exception("Can not find first segment");
            }
            if (independent.Count > 1)
            {
                if (ShowErrors)
                    throw new Exception("Several first segments detected");
            }

            var result = new List<Aran.Aim.Features.RouteSegment>();
            var current = independent.First();
            while (true)
            {
                result.Add(current);
                var id = GetPointId(current.End);

                var next = relatedSegments.Where(t => GetPointId(t.Start) == id).ToList();
                if (next.Count == 0)
                {
                    break;
                }

                if (next.Count > 1)
                {
                    if (ShowErrors)
                        throw new Exception("Several next segments detected");
                }

                current = next.First();
            }

            if (result.Count != relatedSegments.Count)
            {
                if (ShowErrors)
                    throw new Exception("Can not recreate segment sequence");
            }

            return result;
        }

        public void FillAIPData(List<Feature> featureList, string Section)
        {
            try
            {

                //db = new eAIPContext();
                if (Section == "")
                {
                    ErrorLog.ShowMessage("Incorrect section number");
                    return;
                }
                if (Section.Contains(".")) // It is AD2.XXXX or AD3.XXXX
                {
                    string[] arr = Section.Split('.');
                    string airHelTxt = "";
                    if (arr.Length == 2)
                    {
                        Section = arr[0];
                        airHelTxt = arr[1];
                        DB.AirportHeliport airHel = db.AirportHeliport.FirstOrDefault(x => x.LocationIndicatorICAO == airHelTxt);
                        FillAIPDataBase(featureList, Section, airHel);// For AirportHeliport - each from AD21 to AD224
                        //for (int i = 0; i <= 2; i++) // For AirportHeliport - each from AD21 to AD224
                        //{
                        //    FillAIPDataBase(featureList, Section + i, airHel);
                        //}
                    }
                    else
                        return;
                }
                else
                    FillAIPDataBase(featureList, Section);

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException("Error in the FillAIPData", ex, true);
            }

        }


        private void FillAIPDataBase(List<Feature> featureList, string Section, DB.AirportHeliport airHel = null)
        {
            try
            {
                Task taskA = airHel == null ? Task.Factory.StartNew(() => FillDB.FillAIPDatasetSection(Section, featureList, airHel)) :
                    Task.Factory.StartNew(() => FillDB.FillAirportHeliport(Section, featureList, airHel));
                for (; ; )
                {
                    Application.DoEvents();
                    lock (FillDB.OutputQueue)
                    {
                        while (FillDB.OutputQueue.Count > 0)
                        {
                            BaseLib.Struct.Output msg = FillDB.OutputQueue.Dequeue();
                            AddOutput(msg.Message, msg.Percent, null, msg.Color);
                        }
                        if (taskA.IsCompleted || taskA.IsFaulted) break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }



        private void RefreshTitle()
        {
            try
            {
                string format = Properties.Settings.Default.eAIPDateFormat;
                CultureInfo ci = new CultureInfo("en-US");
                Text = TitleVersion();
                if (Lib.CurrentAIP != null)
                {
                    Text += @", Effective date: " + Lib.CurrentAIP?.Effectivedate.ToString(format, ci).ToUpperInvariant();
                }
                if (Lib.AIPLanguage != null)
                {
                    String Language = CultureInfo.GetCultureInfo(Lib.AIPLanguage)?.EnglishName;
                    if (!String.IsNullOrEmpty(Language)) Text += @", AIP Language: " + Language;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private void newFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AipFrm aip = new AipFrm();
            aip.Show();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            //radTreeView1.ClearSelection();
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
                rwb.Visible = true;
                rwb.StartWaiting();
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
                rwb.StopWaiting();
                rwb.Visible = false;
            }
        }



        private void radTreeView1_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            SelectedNodeChanged();
        }

        delegate void SelectedNodeChangedCallback();
        private void SelectedNodeChanged()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new SelectedNodeChangedCallback(SelectedNodeChanged), null);
                }
                else
                {
                    if (TreeView.SelectedNode != null) // May be treeview has been unselected
                    {
                        StartProgress();
                        Application.DoEvents();
                        State.LastClick = RGVStates.TreeViewClicked;
                        if (!WaitMenu.IsBusy)
                        {
                            WaitMenu.RunWorkerAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void WaitMenu_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StopProgress();
        }

        private void WaitMenu_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowChildren();
            //ShowTempChildren();
        }

        //delegate void ShowTempChildrenCallback();
        //public void ShowTempChildren()
        //{
        //    try
        //    {
        //        if (InvokeRequired)
        //        {
        //            Invoke(new ShowTempChildrenCallback(ShowTempChildren), null);
        //        }
        //        else
        //        {
        //            if (Lib.CurrentAIS == null)
        //            {
        //                ErrorLog.ShowInfo($@"No active eAIS package selected.{Environment.NewLine}Please select eAIS package from ""eAIS packages"" window and click ""Active""");
        //                return;
        //            }
        //            RGV.Visible = true;
        //            List<BaseEntityType> MyChildren = null;
        //            Nullable<Int32> ParentID = 0;
        //            string ParentClassName = "";

        //            if (State.LastClick == RGVStates.TreeViewClicked)
        //            {
        //                ClearGridView();
        //                ClearRadEntry();
        //                if (MainTabControl.Pages.Count == 0 || MainTabControl.Pages.Where(n => n.Name == "PV1").Count() == 0)
        //                {
        //                    MainTabControl.Pages.Add(PV1);
        //                }
        //                StructButton(false);
        //                RadTreeNode selectedNode = this.radTreeView1.SelectedNode;
        //                ParentClassName = selectedNode.Name;
        //                if (ParentClassName == null || ParentClassName == "")
        //                {
        //                    ClearGridView();
        //                    ClearRadEntry();
        //                    return;
        //                }
        //                Type AIPType = Type.GetType("AIP.Dataset." + ParentClassName + ",AIP.Dataset");
        //                if (AIPType == null)
        //                {
        //                    ClearGridView();
        //                    ClearRadEntry();
        //                    return;
        //                }

        //                //ParentID = GetBaseEntityIDFromTreeView(ParentClassName);
        //                //MyChildren = GetMyChildrenTypesByID(ParentID, ParentClassName);

        //                //State.ParentID = ParentID;
        //                //State.ParentClassName = ParentClassName;


        //            }


        //            DB.GEN01 ds = db.GEN01.Where(n => n.eAIPID == Lib.CurrentAIP.id).FirstOrDefault();


        //            if (State.LastClick == RGVStates.GridViewClicked) StructButton(true, ParentClassName, radDataEntry1.DataSource, RGV.DataSource);

        //            ShowTempEntity(ds);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }
        //}

        delegate void ColorRowCallback(string ColumnName);
        private void ColorRow(string ColumnName)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new ColorRowCallback(ColorRow), new object[] { ColumnName });
                }
                else
                {
                    ConditionalFormattingObject obj = new ConditionalFormattingObject("IsRowContainChildren", ConditionTypes.Greater, "0", "0", true);
                    Font fnt = new Font(RGV.Font, FontStyle.Bold);
                    obj.RowFont = fnt;
                    //obj.RowBackColor = Color.SkyBlue;
                    RGV.Columns[ColumnName].ConditionalFormattingObjectList.Add(obj);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public List<T> SelectEntity<T>(Expression<Func<T, bool>> expression, eAIPContext db)
            where T : class
        {
            return db.Set<T>().OfType<T>().Where(expression).ToList();
        }

        //public BaseEntityType GetMyEnum(string classname)
        //{
        //    return (BaseEntityType)System.Enum.Parse(typeof(BaseEntityType), classname);
        //}

        //delegate Nullable<Int32> GetBaseEntityIDFromTreeViewCallback(string classname);
        //public Nullable<Int32> GetBaseEntityIDFromTreeView(string classname)
        //{
        //    var logFile = new StreamWriter(Lib.DBLogFile);
        //    try
        //    {

        //        if (InvokeRequired)
        //        {
        //            return (Nullable<Int32>)Invoke(new GetBaseEntityIDFromTreeViewCallback(GetBaseEntityIDFromTreeView), new object[] { classname });
        //        }
        //        else
        //        {
        //            BaseEntityType bet = GetMyEnum(classname);
        //            using (eAIPContext db = new eAIPContext())
        //            {
        //                Application.DoEvents();
        //                if (Lib.ShowDBLog)
        //                    //db.Database.Log = logFile.Write;
        //                    db.Database.Log = Console.Write;
        //                else
        //                    db.Database.Log = null;
        //                //return db.BaseEntity.OfType<BaseEntity>().AsNoTracking().Where(n => n.eAIPID == 4 && n.BaseEntityType == BaseEntityType.GEN01).Select(n=>n.id)?.FirstOrDefault();//TODO: SlowOperations
        //                return db.BaseEntity.OfType<BaseEntity>().AsNoTracking().Where(n => n.eAIPID == Lib.CurrentAIP.id && n.BaseEntityType == bet).Select(n => n.id)?.FirstOrDefault();//TODO: SlowOperations
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return null;
        //    }
        //    finally
        //    {
        //        logFile.Close();
        //    }
        //}

        //public List<BaseEntityType> GetMyChildrenTypesByID(int? MyID, string classname)
        //{
        //    BaseEntityType bet = GetMyEnum(classname);
        //    using (eAIPContext db = new eAIPContext())
        //    {
        //        if (Lib.ShowDBLog)
        //            db.Database.Log = Console.Write;
        //        else
        //            db.Database.Log = null;
        //        return db.BaseEntity.OfType<BaseEntity>().AsNoTracking().Where(n => n.eAIPID == Lib.CurrentAIP.id && n.BaseEntityID == MyID).Select(n => n.BaseEntityType).Distinct().ToList();
        //    }
        //}


        public void LoadEntityFromString(eAIPContext db, string EntityName)
        {
            Type AIPType = Type.GetType("AIP.DB." + EntityName + ",AIP.DB");
            MethodInfo method = typeof(AIP.GUI.Main).GetMethod("LoadEntity");
            MethodInfo generic = method.MakeGenericMethod(AIPType);
            Object obj = generic.Invoke(this, new object[] { db });
        }

        public void LoadEntity<TEntity>(eAIPContext db)
            where TEntity : class
        {

            db.Set<TEntity>().Load();
        }

        delegate void StructButtonCallback(bool Show, string ParentName = "", object DataEntryData = null, object GridViewData = null);
        private void StructButton(bool Show, string ParentName = "", object DataEntryData = null, object GridViewData = null)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new StructButtonCallback(StructButton), new object[] { Show, ParentName, DataEntryData, GridViewData });
                }
                else
                {
                    if (Show)
                    {
                        btn_Struct.Visible = true;
                        btn_Struct.Text = "Go back";
                        btn_Struct.Click += Menu_Click;

                        if (ParentName != "")
                        {
                            RadMenuItem menu = new RadMenuItem(ParentName);
                            menu.Click += Menu_Click;
                            menu.Tag = new object[] { DataEntryData, GridViewData };
                            menu.Text = ParentName;
                            menu.Name = "index_" + btn_Struct.Items.Count;
                            btn_Struct.Items.Add(menu);
                        }
                    }
                    else
                    {
                        RemoveStructButton();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void RemoveStructButton()
        {
            btn_Struct.Visible = false;
            btn_Struct.Items.Clear();
        }

        private void Menu_Click(object sender, EventArgs e)
        {

        }

        private void RGV_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (RGV.CurrentRow.DataBoundItem is AIPSection)
                State.LastClick = RGVStates.GridViewClicked;
            else
                State.LastClick = RGVStates.GridViewSelectorClicked;

            ShowChildren();
        }

        private void ClearRadEntry()
        {
            PageViewRemoveViews();
            radDataEntry1.DataSource = null;
            radPanel1.Visible = false;
        }

        delegate void ShowEntityCallback(int? BaseEntityID, string ClassName = "");
        private void ShowEntity(int? BaseEntityID, string ClassName = "")
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new ShowEntityCallback(ShowEntity), new object[] { BaseEntityID, ClassName });
                }
                else
                {
                    if (BaseEntityID == null)
                    {
                        ErrorLog.ShowMessage($@"Parent object not available", true);
                        return;
                    }
                    ComplPropList.Clear();
                    radPanel1.Visible = true;
                    MainTabControl.Visible = true;
                    radPanel1.Text = "";
                    Application.DoEvents();
                    if (Lib.ShowDBLog)
                        //db.Database.Log = logFile.Write;
                        db.Database.Log = Console.Write;
                    else
                        db.Database.Log = null;

                    dynamic t = null;
                    if (State.LastClick == RGVStates.TreeViewClicked) // treeview - AIPSection
                    {
                        t = State.CurrentSection;
                        PV1.Text = t.SectionName.ToString();
                    }
                    else // SubClass
                    {
                        t = State.CurrentSubClass;
                        PV1.Text = t.SubClassType.ToString();
                    }

                    Application.DoEvents();
                    FillComplexPropertyCollection(t);
                    PageViewRemoveViews();


                    radDataEntry1.ShowValidationPanel = true;
                    radDataEntry1.ItemDefaultSize = new Size(520, 25);
                    radDataEntry1.ItemSpace = 8;

                    //radDataEntry1.EditorInitializing += RadDataEntry1_EditorInitializing;
                    radDataEntry1.DataSource = t;
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the ShowEntity: {ex.Message}", true);
            }


        }


        private void PageViewRemoveViews()
        {
            if (MainTabControl.Pages.Count > 1)
            {
                foreach (var item in MainTabControl.Pages)
                {
                    if (item.Name != "PV1")
                        MainTabControl.Pages.Remove(item);
                }
            }
        }
        LinkLabel control = null;
        private void RadDataEntry1_EditorInitializing(object sender, EditorInitializingEventArgs e)
        {
            if (ComplPropList.ContainsKey(e.Property.Name))
            {
                control = new LinkLabel();
                control.Click += Control_Click;
                control.Tag = ComplPropList[e.Property.Name];
                control.Name = e.Property.Name;
                e.Editor = control;
                e.Editor.Width = 300;
                e.Editor.Height = 300;
            }
            else if (e.Property.Name == "Content")
            {
                RichTextBox ct = new RichTextBox();
                ct.Name = "Content";
                ct.AutoSize = false;
                ct.Multiline = true;
                ct.Width = 600;
                ct.Height = 400;
                ct.ReadOnly = true;
                e.Editor = ct;
                e.Editor.Width = 600;
                e.Editor.Height = 400;
            }
            //else if (e.Property.Name.Equals("SectionStatus"))
            //{
            //    var radDropDownList1 = new RadDropDownList();
            //    radDropDownList1.DataSource = Enum.GetValues(typeof(SectionStatusEnum));
            //    radDropDownList1.Parent = this;
            //    e.Editor = radDropDownList1;
            //}
            else if (!object.Equals(e.Property.Category, "Edit"))
            {
                e.Editor.Enabled = false;
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RadPageViewPage page in MainTabControl.Pages)
                {
                    if (page.Name == ((Control)sender).Name) // Already created, just switch to it
                    {
                        MainTabControl.SelectedPage = page;
                        return;
                    }
                }

                RadGridView rgv = new RadGridView();
                rgv.AllowAddNewRow = false;
                rgv.AllowDeleteRow = false;
                rgv.AllowDrop = false;
                //rgv.SelectionMode = GridViewSelectionMode.FullRowSelect;
                rgv.DataBindingComplete += Rgv_DataBindingComplete;


                rgv.DataSource = ((Control)sender).Tag;
                rgv.Dock = DockStyle.Fill;
                rgv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                rgv.ThemeName = "Office2013Light";
                rgv.CellDoubleClick += Rde_CellDoubleClick;
                //rde.ReadOnly = true;


                RadPageViewPage rpvp = new RadPageViewPage();
                rpvp.Dock = DockStyle.Fill;
                rpvp.Controls.Add(rgv);
                rpvp.Text = $@"List of {((Control)sender).Name}";
                rpvp.Name = $@"{((Control)sender).Name}_lst";
                MainTabControl.Pages.Add(rpvp);
                MainTabControl.SelectedPage = rpvp;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the Control_Click: {ex.Message}", true);
            }

        }

        private void Rgv_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            try
            {
                (((RadGridView)sender).Columns)?.Where(n => n.DataType.Name.ToLowerInvariant().Contains("collection")).All(n => n.IsVisible = false);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the Rgv_DataBindingComplete: {ex.Message}", true);
            }
        }

        private int correctiveLine = 0;
        private int lineHeight = 25;
        private void Rde_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row == null || e.Row.DataBoundItem == null) return;
            correctiveLine = 0;
            RadDataEntry radDataEntryx = new RadDataEntry();
            radDataEntryx.Dock = DockStyle.Fill;
            radDataEntryx.ShowValidationPanel = true;
            radDataEntryx.ItemDefaultSize = new Size(520, 25);
            radDataEntryx.ItemSpace = 8;
            radDataEntryx.EditorInitializing += RadDataEntry1_EditorInitializing;
            radDataEntryx.ItemInitialized += RadDataEntryxOnItemInitialized;
            radDataEntryx.BindingCreating += radDataEntry1_BindingCreating;
            radDataEntryx.DataSource = e.Row.DataBoundItem;


            //InitNewDataEntry(e.Row.DataBoundItem, radDataEntryx);
            foreach (var prop in e.Row.DataBoundItem.GetType().GetProperties())
            {
                string propType = prop.GetValue(e.Row.DataBoundItem, null)?.GetType()?.ToString();
                if (propType != null && (propType.ToLowerInvariant().Contains("system.collections.generic.list`1[aip.db")
                    || propType.ToLowerInvariant().Contains("system.collections.generic.hashset")))
                {
                    if (ComplPropList.ContainsKey(prop.Name)) ComplPropList.Remove(prop.Name);
                    ComplPropList.Add(prop.Name, prop.GetValue(e.Row.DataBoundItem, null));
                }
            }
            //FillComplexPropertyCollection(e.Row.DataBoundItem);

            RadPageViewPage rpvp = new RadPageViewPage();
            rpvp.Dock = DockStyle.Fill;
            rpvp.Controls.Add(radDataEntryx);
            rpvp.Text = e.Row?.DataBoundItem?.GetType()?.BaseType?.Name;
            rpvp.Name = e.Row?.DataBoundItem?.GetType()?.BaseType?.Name;
            MainTabControl.Pages.Add(rpvp);
            MainTabControl.SelectedPage = rpvp;
        }

        private void RadDataEntryxOnItemInitialized(object sender, ItemInitializedEventArgs e)
        {
            try
            {
                e.Panel.Controls[1].Font = new System.Drawing.Font(e.Panel.Controls[1].Font.Name, 11, FontStyle.Bold);
                if (e.Panel.Controls[0].Tag is DataEntryOption)
                {
                    e.Panel.Visible = ((DataEntryOption)e.Panel.Controls[0].Tag).Visible;
                    if (!e.Panel.Visible) correctiveLine++;
                    e.Panel.Enabled = !((DataEntryOption)e.Panel.Controls[0].Tag).ReadOnly;
                }
                e.Panel.Location = new System.Drawing.Point(e.Panel.Location.X, e.Panel.Location.Y - lineHeight * correctiveLine);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        delegate void FillComplexPropertyCollectionCallback<T>(T data);
        private void FillComplexPropertyCollection<T>(T data)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new FillComplexPropertyCollectionCallback<T>(FillComplexPropertyCollection), new object[] { data });
                }
                else
                {
                    //PropertyInfo[] pi = data.GetType().GetProperties().Where(n=>n.GetValue(data,null)?.GetType()?.ToString().ToLowerInvariant().Contains);
                    PropertyInfo[] pi = data.GetType().GetProperties();
                    Application.DoEvents();
                    foreach (var prop in pi)//TODO: Slow operation
                    {
                        Console.WriteLine(prop.Name);
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        if (prop.Name.Contains("Parent") || prop.Name.Contains("ChildNumberHidden"))
                            continue;

                        string propType = prop.GetValue(data, null)?.GetType()?.ToString();
                        if (propType != null && (propType.ToLowerInvariant().Contains("system.collections.generic.list`1[aip.db")
                            || propType.ToLowerInvariant().Contains("system.collections.generic.hashset")))
                        {
                            if (ComplPropList.ContainsKey(prop.Name)) ComplPropList.Remove(prop.Name);
                            ComplPropList.Add(prop.Name, prop.GetValue(data, null));
                        }
                        watch.Stop();
                        var elapsedMs = watch.ElapsedMilliseconds;
                        Console.WriteLine(elapsedMs);
                    }
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the FillComplexPropertyCollection: {ex.Message}", true);
            }

        }

        private void InitNewDataEntry<T>(T data, RadDataEntry rde)
        {
            rde.Dock = DockStyle.Fill;
            rde.ShowValidationPanel = true;
            rde.ItemDefaultSize = new Size(520, 25);
            rde.ItemSpace = 8;
            rde.EditorInitializing += RadDataEntry1_EditorInitializing;
            rde.DataSource = data;

        }



        delegate void ShowChildrenCallback();
        public void ShowChildren()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new ShowChildrenCallback(ShowChildren), null);
                }
                else
                {
                    if (!Lib.IsAIPSelected()) return;

                    RGV.Visible = true;
                    List<SubClassType> MyChildren = null;
                    Nullable<Int32> ParentID = 0;
                    string ParentClassName = "";

                    if (State.LastClick == RGVStates.TreeViewClicked)
                    {
                        ClearGridView();
                        ClearRadEntry();
                        if (MainTabControl.Pages.Count == 0 || MainTabControl.Pages.Count(n => n.Name == "PV1") == 0)
                        {
                            MainTabControl.Pages.Add(PV1);
                        }
                        StructButton(false);
                        RadTreeNode selectedNode = TreeView.SelectedNode;
                        ParentClassName = selectedNode.Name;
                        if (string.IsNullOrEmpty(ParentClassName))
                        {
                            ClearGridView();
                            ClearRadEntry();
                            return;
                        }

                        SectionName SelectedSection = Lib.GetSectionName(ParentClassName);
                        DB.AirportHeliport airHel = Lib.GetAirportHeliport(ParentClassName);
                        if (SelectedSection == SectionName.None)
                            return;

                        State.CurrentSection =
                            airHel == null ?
                            db.AIPSection?.Where(n => n.eAIPID == Lib.CurrentAIP.id && n.SectionName == SelectedSection)?.Include(n => n.Children)?.FirstOrDefault() :
                            db.AIPSection?.Where(n => n.eAIPID == Lib.CurrentAIP.id && n.SectionName == SelectedSection && n.AirportHeliportID == airHel.id)?.Include(n => n.Children)?.FirstOrDefault();
                        if (State.CurrentSection == null)
                        {
                            ShowTabError(ParentClassName);
                            return;
                        }

                        MyChildren = State.CurrentSection?.Children?.Select(x => x.SubClassType)?.Distinct()?.ToList();
                        State.CurrentSubClass = null;
                        State.ParentID = ParentID;
                        State.ParentClassName = ParentClassName;
                    }
                    else if (State.LastClick == RGVStates.GridViewClicked ||
                            State.LastClick == RGVStates.GridViewSelectorClicked)
                    {
                        GridViewRowInfo currentRow = RGV.CurrentRow;
                        GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;
                        if (current.DataBoundItem is SubClass)
                        {
                            SubClass row = (SubClass)current.DataBoundItem;
                            ParentID = row.id;
                            if (row.SubClassType == SubClassType.None)
                            {
                                ErrorLog.ShowMessage($@"No such SubClassType available");
                                return;
                            }
                            ParentClassName = row.SubClassType.ToString();
                            // MyChildren = GetMyChildrenTypesByID(ParentID, ParentClassName); 
                            State.CurrentSection = null;
                            //State.CurrentSubClass = db.Route?.Where(n => n.eAIPID == Lib.CurrentAIP.id && n.id == ParentID)?.Include(n => n.Children)?.FirstOrDefault();
                            State.CurrentSubClass = SelectSubClassByName(ParentClassName, ParentID);



                            if (State.CurrentSubClass == null) return;
                            MyChildren = State.CurrentSubClass?.Children?.Select(x => x.SubClassType)?.Distinct()?.ToList();

                            if (MyChildren.Count >= 1)
                            {
                                StructButton(true, ParentClassName, radDataEntry1.DataSource, RGV.DataSource);
                            }
                            State.ParentID = ParentID;
                            State.ParentClassName = ParentClassName;
                        }

                        else if (current.DataBoundItem is ValueType<string>) // Selector between classes
                        {
                            ValueType<string> row = (ValueType<string>)current.DataBoundItem;
                            SubClassType type = Lib.GetSubclassName(row.ItemProperty);
                            MyChildren = new List<SubClassType>() { type };

                            ParentID = State.ParentID;
                            ParentClassName = State.ParentClassName;

                            //State.CurrentSubClass = SelectSubClassByName(ParentClassName, ParentID);
                            var objlist = State.CurrentSubClass.Children.Where(n => n.SubClassType == type);
                            RGV.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                            ShowGridView(objlist);
                        }

                    }



                    if (MyChildren.Count > 1)
                    {
                        IEnumerable<string> Entities = MyChildren.Select(n => n.ToString()).Distinct().ToList();
                        ArrayList list = new ArrayList();
                        foreach (var item in Entities)
                        {
                            list.Add(new ValueType<string>(item));
                        }
                        ShowGridView(list);
                        RGV.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                        RGV.BestFitColumns();
                        ShowEntity(ParentID);
                        return;
                    }
                    else if (MyChildren.Count == 0)
                    {
                        ShowEntity(ParentID);
                        ClearGridView();
                        return;
                    }
                    if (State.LastClick == RGVStates.GridViewClicked) StructButton(true, ParentClassName, radDataEntry1.DataSource, RGV.DataSource);

                    ShowEntity(ParentID, ParentClassName);

                    if (State.CurrentSection != null)
                    {
                        var objlist = State.CurrentSection.Children;
                        RGV.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                        ShowGridView(objlist);
                        // ColorRow("ChildNumberHidden");
                    }
                    //else if (State.CurrentSubClass != null)
                    //{
                    //    var objlist = State.CurrentSubClass.Children;
                    //    RGV.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    //    ShowGridView(objlist);
                    //}

                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        public T SelectSubClass<T>(Expression<Func<T, bool>> expression, eAIPContext db)
            where T : class
        {
            try
            {
                return db.Set<T>().OfType<T>().Where(expression).Include("Children").FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
            // State.CurrentSubClass = db.Route?.Where(n => n.eAIPID == Lib.CurrentAIP.id && n.id == ParentID)?.Include(n => n.Children)?.FirstOrDefault();
            // return db.Set<T>().OfType<T>().Where(expression).ToList();
        }

        public dynamic SelectSubClassByName(string SubClassName, int? ParentID)
        {
            try
            {
                if (ParentID == null) return null;
                Type ChildType = Type.GetType("AIP.DB." + SubClassName + ",AIP.DB");
                MethodInfo method = typeof(AIP.GUI.Main).GetMethod("SelectSubClass");
                MethodInfo generic = method.MakeGenericMethod(ChildType);

                ParameterExpression p = Expression.Parameter(ChildType);
                Expression property = Expression.Property(p, "id");
                Expression c = Expression.Constant(ParentID);
                Expression body = Expression.Equal(property, Expression.Convert(c, property.Type));
                Expression exp = Expression.Lambda(body, new ParameterExpression[] { p });

                var objlist = generic.Invoke(this, new object[] { exp, db });
                return objlist;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private void ShowTabError(string ParentClassName)
        {
            try
            {
                radPanel1.Visible = true;
                MainTabControl.Visible = false;
                radPanel1.Text = $@"<html>Section <b>{ParentClassName}</b> not yet imported from AIXM 5.1 DB";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        delegate void ClearGridViewCallBack();
        private void ClearGridView()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearGridViewCallBack(ClearGridView), null);
            }
            else
            {
                RGV.DataSource = null;
                RGV.Visible = false;
            }

        }

        delegate void ShowGridViewCallback(object DataSource);
        private void ShowGridView(object DataSource)
        {
            if (InvokeRequired)
            {
                Invoke(new ShowGridViewCallback(ShowGridView), new object[] { DataSource });
            }
            else
            {
                RGV.DataSource = DataSource;
                RGV.Visible = true;
            }

        }

        private void RGV_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {
            var column = RGV.Columns.Where(n => n.Name.EndsWith("Hidden")).FirstOrDefault();
            if (column != null)
                column.IsVisible = false;
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            //GridViewRowInfo currentRow = RGV.CurrentRow;
            //GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;

            //if (current.DataBoundItem is BaseEntity)
            //{
            //    BaseEntity Entity = (BaseEntity)current.DataBoundItem;
            //    UniFrm frm = new UniFrm(Entity);
            //    if (frm.ShowDialog() == DialogResult.OK)
            //    {
            //        RGV.BeginUpdate();
            //        db.SaveChanges();
            //        RGV.EndUpdate();
            //    }
            //}
        }

        private void CurrentLanguage()
        {
            try
            {
                Lib.AIPLanguage = Properties.Settings.Default.eAIPLanguage ?? "en-GB";
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        /// <summary>
        /// Set Lib.AIXMLanguage based on current AIP language
        /// </summary>
        private void AIXMLanguage()
        {
            try
            {
                using (eAIPContext db_options = new eAIPContext())
                {
                    Lib.AIXMLanguage = db_options.LanguageReference
                        ?.AsNoTracking().Where(n => n.Value == Lib.AIPLanguage)?.FirstOrDefault()?.AIXMValue ?? language.ENG;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        /// <summary>
        /// Update interface and static variables with selected language
        /// </summary>
        private void UpdateLanguages()
        {
            try
            {
                CurrentLanguage(); // Set Lib.AIPLanguage
                AIXMLanguage(); // Set Lib.AIXMLanguage
                Lib.GetLanguageTexts(); // Fill Lib.MenuLang and Lib.Lang lists
                FillAIPList(); // Rebind AIP List with new language
                RefreshTitle(); // Refreshing title with new language
                InitXMLMenu(); // Init XML menu with new language
                refreshSupList(); // Refreshing supplement list
                updateCircular();
                UpdatePageList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }



        /// <summary>
        /// Menu clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is RadMenuItem)
                {
                    string menu = (sender as RadMenuItem).Name?.ToLowerInvariant().Replace("rmi_", "");
                    if (menu.IsNull()) return;
                    // Small chars
                    // All menus except for Fill and Gen
                    if (!menu.StartsWith("gen_") && !menu.StartsWith("fill_") && !menu.StartsWith("run_"))
                    {
                        switch (menu)
                        {
                            case "filemanager":
                                OpenFileManager();
                                break;
                            case "abbrev":
                                OpenAbbrevList();
                                break;
                            case "compare":
                                OpenCompareManager();
                                break;
                            case "settings":
                                OpenSettingsForm();
                                break;
                            case "about":
                                OpenAbout();
                                break;
                            case "help":
                                OpenHelp();
                                break;
                            case "lm":
                                OpenLanguageManager();
                                break;
                            case "newais":
                                NewAIS();
                                break;
                            case "exit":
                                Application.Exit();
                                break;
                            case "download":
                                DownloadPublication();
                                break;
                            case "copy":
                                CopyPublication();
                                break;
                            case "upload":
                                Upload();
                                break;
                            case "web_aip":
                                PostGenerate();
                                break;
                            case "":
                                break;
                            default:
                                return;
                                break;
                        }
                    }
                    else if (menu.StartsWith("fill_"))
                    {
                        Menu_FillAction(null, new AdditionalInfo(true, null)); // Fill DB and Generate XML 
                    }
                    else if (menu.StartsWith("gen_"))
                    {
                        Menu_GenerateAction(null, new AdditionalInfo(false, null)); // Generate html/pdf
                    }
                    else if (menu.StartsWith("run_"))
                    {
                        Menu_RunAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error in MainMenu_Click: " +
                    ex.Message
                    );

            }
        }

        private void OpenAbbrevList()
        {
            try
            {
                AbbrevList stn = new AbbrevList();
                stn.db = db;
                DialogResult result = stn.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // RefreshTitle();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private void Menu_FillAction(string section = null, AdditionalInfo fillAdditionalInfo = null)
        {
            try
            {
                if (Lib.CurrentAIP == null)
                {
                    MessageBox.Show(@"No AIP selected");
                    return;
                }
                AIP_FILL = section.IsNull() ? "All" : section;
                MainFillDataset(AIP_FILL, fillAdditionalInfo);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void Menu_RunAll(string section = null)
        {
            try
            {
                var initAIS = Lib.CurrentAIS;
                if (Lib.CurrentAIP == null)
                {
                    MessageBox.Show(@"No AIP selected");
                    return;
                }
                // Check that AIP with the same ED on another language exists
                // Check that both AIP are in the Work status 
                List<KeyValuePair<string, string>> ds = db.LanguageReference
                    .AsNoTracking()
                    .AsEnumerable()
                    .Select(x => new KeyValuePair<string, string>(x.Value, x.Name))
                    .ToList();
                // Getting all AIS packages with current AIP ED
                var aisByED = db.eAISpackage
                    .Include(x => x.eAIPpackage.eAIP.Amendment)
                    .Where(x => x.Effectivedate == Lib.CurrentAIP.Effectivedate)
                    .ToList();
                // Declare all required to run AIPs (English and Latvian)
                List<DB.eAISpackage> eAISList = new List<DB.eAISpackage>();
                foreach (KeyValuePair<string, string> lang in ds)
                {
                    var ais = aisByED
                        .FirstOrDefault(x => x.lang == lang.Key);
                    // Not all language AIS package created
                    if (ais.IsNull())
                    {
                        ErrorLog.ShowWarning($@"Attention: All lingual AIS packages with Effective Date {Lib.CurrentAIP.Effectivedate:yyyy-MM-dd} must be created. {lang.Value} AIS package not yet created. Create it and set it Work status.");
                        return;
                    }
                    // Not all AIS packages is in the Work mode
                    if (ais?.Status != Status.Work)
                    {
                        ErrorLog.ShowWarning($@"Attention: All lingual AIS packages with Effective Date {Lib.CurrentAIP.Effectivedate:yyyy-MM-dd} must be in the Work status. {lang.Value} in the {ais?.Status} status");
                        return;
                    }
                    if (Properties.Settings.Default.PendingData)
                    {
                        ErrorLog.ShowWarning($@"Attention: Pending Data enabled in the settings. AIP can`t be published. You need to publish it or set setting not to use Pending Data.");
                        return;
                    }
                    eAISList.Add(ais);
                }

                if (Lib.sectionsDebug > 0) AddOutput($@"DEBUG ENABLED: Only first {Lib.sectionsDebug} will be processed", 0, null, Color.Red);

                IsUserTerminated = false; // Prepare to check user action
                                          // For each ais (Eng or LV)

                // Prepare works
                // Clear current AIP Source
                // Directory.Delete(Lib.SourceDir, true); // ToDO: check for generate order

                var thread = new Thread(() =>
                {
                    IsPublishProcess = true;
                    Thread.CurrentThread.IsBackground = false;
                    foreach (var ais in eAISList)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() => ChangeAIPLanguage(ais.lang)));
                        }
                        eAIS_Activated(ais, false);
                        ClearPDFPagesInfo().Wait();

                        IsAllRunning = true;
                        Menu_FillAction(null, new AdditionalInfo(false, null)); // fill & xml
                        while (IsAllRunning) { }
                        if (IsUserTerminated) return;

                        GenerateXmlAsync().Wait();

                        IsAllRunning = true;
                        //Menu_GenerateAction(null, new AdditionalInfo(false, null)); // html/pdf
                        Menu_GenerateAction(null, new AdditionalInfo(false, null)); // TODO: check empty sections issue
                        while (IsAllRunning) { }
                        if (IsUserTerminated) return;

                        IsAllRunning = true;
                        PostGenerate();
                        while (IsAllRunning) { }
                        if (IsUserTerminated) return;
                    }

                    // If transaction completed successfully
                    // Change AIS status to Published
                    // For each ais (Eng or LV)
                    foreach (var ais in eAISList)
                    {
                        ais.Status = Status.Published;
                        db.Entry(ais).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => eAIS_Activated(initAIS, false)));
                    }

                    // Upload files to MongoDB
                    // Copy Report file to the Output folder (or may be to the Source?)
                    if (Report.ErrorLogFileName != null)
                    {
                        Lib.SafeCopy(Report.ErrorLogFileName,
                            Path.Combine(Lib.OutputDir, Path.GetFileName(Report.ErrorLogFileName)));
                    }

                    RunOutputShownTask(Task.Factory.StartNew(() => FileDBManager.Upload(
                        new List<FileDBManager.Folder>()
                        {
                            new FileDBManager.Folder(Lib.SourceDir, "Source"),
                            new FileDBManager.Folder(Lib.OutputDir, "Output")
                        }
                        )),
                        FileDBManager.OutputQueue);

                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => ReloadAISList(initAIS)));
                    }

                    IsPublishProcess = false;
                });
                thread.Start();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void Menu_GenerateAction(string section = null, AdditionalInfo fillAdditionalInfo = null)
        {
            if (Lib.CurrentAIP == null)
            {
                MessageBox.Show(@"No AIP selected");
                return;
            }
            AIP_GENERATE = section.IsNull() ? "All" : section;

            try
            {
                if (!CurrentAIPSelected()) return;
                //rmi_Gen_AIP.Enabled = false;
                GenerateAIP(AIP_GENERATE, fillAdditionalInfo);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                //rmi_Gen_AIP.Enabled = true;
            }
        }

        private void OpenAbout()
        {
            try
            {
                // Quick Test on the my machine only
                //if (Environment.MachineName.Contains("EMINS"))
                //{
                //    string file =
                //        @"C:\CurrentProjects\Panda\AirNav\bin\Debug\Data\eAIP-source\2018-09-13-AIRAC\eAIP\EV-GEN-1.4-en-GB.xml";
                //    Lib.GetComparedFile(file + ".1", file + ".2", "GEN-1.4", "333");
                //    return;
                //}
                About stn = new About();
                DialogResult result = stn.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // RefreshTitle();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenHelp()
        {
            try
            {
                string helpFile = @"Help\eAIPProduction.chm";
                if (File.Exists(helpFile)) Help.ShowHelp(this, helpFile);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenFileManager()
        {
            try
            {
                FileManager stn = new FileManager();
                stn.db = db;
                DialogResult result = stn.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // RefreshTitle();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenCompareManager()
        {
            try
            {
                CompareHTML stn = new CompareHTML();
                stn.db = db;
                DialogResult result = stn.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // RefreshTitle();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenSettingsForm()
        {
            try
            {
                bool getBeforePreview = Properties.Settings.Default.GetBeforePreview;
                Forms.Settings stn = new Forms.Settings();
                stn.db = db;
                DialogResult result = stn.ShowDialog();
                if (stn.ResetLayout)
                {
                    LoadLayout();
                }
                if (result == DialogResult.OK)
                {
                    RefreshTitle();
                    //if (Lib.AIPLanguage != Properties.Settings.Default.eAIPLanguage)
                    //{

                    //    // Current AIP must be rewritten
                    //    // If AIP language changed, Current AIP with old language can`t be more actual
                    //    eAIS_Deactivated();
                    //    UpdateLanguages();
                    //}
                    if (getBeforePreview != Properties.Settings.Default.GetBeforePreview)
                    {
                        InitTreeView();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenLanguageManager()
        {
            try
            {
                LanguageManager stn = new LanguageManager();
                stn.db = db;
                DialogResult result = stn.ShowDialog();
                if (result == DialogResult.OK)
                {
                    RefreshTitle();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public delegate void MainFillDatasetDelegate(string section, AdditionalInfo fillAdditionalInfo);
        private void MainFillDataset(string section, AdditionalInfo fillAdditionalInfo = null)
        {
            if (InvokeRequired)
            {
                Invoke(new MainFillDatasetDelegate(MainFillDataset), section, fillAdditionalInfo);
            }
            else
            {
                if (!CurrentAIPSelected()) return;
                DockWindow dw = radDock1.DockWindows.FirstOrDefault(n => n.Name.ToLowerInvariant().Contains("output"));
                if (dw != null) radDock1.ActiveWindow = dw;

                if (bg_DatasetFill.IsBusy != true)
                {
                    try
                    {
                        AIP_FILL = section;
                        bg_DatasetFill.RunWorkerAsync(fillAdditionalInfo);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                }
            }

        }


        public delegate void GenerateAIPDelegate(string section, AdditionalInfo fillAdditionalInfo = null);
        private void GenerateAIP(string section, AdditionalInfo fillAdditionalInfo = null)
        {

            if (InvokeRequired)
            {
                Invoke(new GenerateAIPDelegate(GenerateAIP), section, fillAdditionalInfo);
            }
            else
            {
                try
                {
                    if (!CurrentAIPSelected()) return;
                    DockWindow dw =
                                radDock1.DockWindows.FirstOrDefault(n => n.Name.ToLowerInvariant().Contains("output"));
                    if (dw != null) radDock1.ActiveWindow = dw;

                    if (bg_GenerateAIP.IsBusy != true)
                    {
                        try
                        {
                            AIP_GENERATE = section;
                            bg_GenerateAIP.RunWorkerAsync(fillAdditionalInfo);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);

                }
            }
        }

        /// <summary>
        /// Do not forgive to remove Lib.LayoutFile file after changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateWindowsMenu();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void GenerateWindowsMenu()
        {
            try
            {
                rm_Windows.Items.Clear();
                foreach (DockWindow window in radDock1.DockWindows)
                {
                    if (window.Text.Equals("SUP"))
                    {
                        if (!Permissions.CanManageSupplements())
                        {
                            window.DockState = DockState.Hidden;
                            continue;
                        }
                    }
                    else if (window.Text.Equals("Other Pages"))
                    {
                        if (!Permissions.CanManageOtherPages())
                        {
                            window.DockState = DockState.Hidden;
                            continue;
                        }
                    }
                    else if (window.Text.Equals("AIC"))
                    {
                        if (!Permissions.CanManageCirculars())
                        {
                            window.DockState = DockState.Hidden;
                            continue;
                        }
                    }

                    RadMenuItem item = new RadMenuItem(window.Text);
                    if (window.DockState != DockState.Hidden)
                    {
                        item.IsChecked = true;
                    }
                    item.Click += new EventHandler(item_Click);
                    rm_Windows.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void GenerateAIPLanguageMenu()
        {
            try
            {
                rm_AIP_Language.Items.Clear();
                List<KeyValuePair<string, string>> ds = db.LanguageReference
                    .AsNoTracking()
                    .AsEnumerable()
                    .Select(x => new KeyValuePair<string, string>(x.Value, x.Name))
                    .ToList();
                foreach (KeyValuePair<string, string> lang in ds)
                {
                    RadMenuItem item = new RadMenuItem(lang.Value)
                    {
                        IsChecked = lang.Key == Properties.Settings.Default.eAIPLanguage,
                        Tag = lang.Key
                    };
                    item.Click += ItemOnClick;
                    rm_AIP_Language.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void ItemOnClick(object sender, EventArgs eventArgs)
        {
            try
            {
                RadMenuItem item = sender as RadMenuItem;
                var prevLangDate = Lib.CurrentAIP?.Effectivedate;
                var newLangKey = item?.Tag.ToString();
                // Is Item changed?
                if (!item.IsNull() && !newLangKey.IsNull() && newLangKey != Properties.Settings.Default.eAIPLanguage)
                {
                    ChangeAIPLanguage(newLangKey);
                    // Trying to find the same date for other lang and activate it
                    if (!prevLangDate.IsNull())
                    {
                        var ais = db.eAISpackage
                            .Include(x => x.eAIPpackage.eAIP.Amendment)
                            .FirstOrDefault(x => x.lang == newLangKey && x.Effectivedate == prevLangDate);
                        if (!ais.IsNull()) eAIS_Activated(ais);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }

        private void ChangeAIPLanguage(string newLangKey)
        {
            try
            {
                // Saving into config
                Properties.Settings.Default.eAIPLanguage = newLangKey;
                Properties.Settings.Default.Save();
                // Checking selected, unchecking other
                foreach (var radItem in rm_AIP_Language.Items)
                {
                    var rmItem = (RadMenuItem)radItem;
                    rmItem.IsChecked = rmItem?.Tag.ToString() == newLangKey;
                }

                // Deactivating, updating, refreshing title
                eAIS_Deactivated();
                UpdateLanguages();
                RefreshTitle();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            try
            {
                RadMenuItem item = sender as RadMenuItem;

                foreach (DockWindow window in radDock1.DockWindows)
                {
                    if (window.Text == ((RadMenuItem)sender).Text)
                    {
                        if (!item.IsChecked)
                        {
                            if (window is DocumentWindow && window.DockState != DockState.TabbedDocument)
                            {
                                window.DockState = DockState.TabbedDocument;
                                window.TabStripItem.ShowCloseButton = true;
                                radDock1.ActiveWindow = window;
                            }
                            else
                            {
                                window.DockState = DockState.Docked;
                                radDock1.ActiveWindow = window;
                            }
                        }
                        else
                        {
                            window.DockState = DockState.Hidden;
                            item.IsChecked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void rm_eAIPs_Click(object sender, EventArgs e)
        {

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Saving Layout
                radDock1.SaveToXml(Lib.LayoutFile);
                if (Lib.AIPLanguage != "" && Lib.AIPLanguage != null)
                {
                    string treeViewFile = $@"Settings/menu_{Lib.AIPLanguage}.xml";
                    if (File.Exists(treeViewFile))
                        TreeView.SaveXML(treeViewFile);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private void rlv_eAIPs_ItemDataBound(object sender, ListItemDataBoundEventArgs args)
        {
            try
            {
                string format = Properties.Settings.Default.eAIPDateFormat;
                CultureInfo ci = new CultureInfo("en-US");

                DB.eAISpackage view = (DB.eAISpackage)args.NewItem.DataBoundItem;
                String Language = CultureInfo.GetCultureInfo(view.lang)?.EnglishName;
                string color = (view.Status == Status.Published) ? "gray" : (view.Status == Status.Work) ? "green" : "black";
                string airac = Lib.IsAIRAC(view.Effectivedate) ? "AIRAC " : "";
                string amdt = String.IsNullOrEmpty(view.eAIPpackage?.eAIP?.Amendment?.Number) ? "" : $@"<br/>{airac}AIP AMDT:<b> {view.eAIPpackage?.eAIP?.Amendment.Number}/{view.eAIPpackage?.eAIP?.Amendment.Year}</b>";
                args.NewItem.Text = $@"<html>Effective Date: <b>{view.Effectivedate.ToString(format, ci).ToUpperInvariant()}</b>{amdt}<br/>{Language}<br/><i><span style=""color: {color}"">{view.Status}</span></i>";
                Console.WriteLine(args.NewItem.Text);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void rlv_eAIPs_CreatingVisualListItem(object sender, CreatingVisualListItemEventArgs args)
        {
            try
            {
                RadListVisualItem visualItem = new RadListVisualItem();
                visualItem.Padding = new Padding(5, 5, 0, 5);
                args.VisualItem = visualItem;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_AIP_Open_Click(object sender, EventArgs e)
        {
            try
            {
                OpenAIS();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenAIS()
        {
            try
            {
                RadListDataItem currentRow = RadListControl_AIS.SelectedItem;
                if (currentRow?.DataBoundItem is DB.eAISpackage)
                {
                    string oldlang = ((DB.eAISpackage)currentRow.DataBoundItem).lang;
                    eAISFrm frm = new eAISFrm((DB.eAISpackage)currentRow.DataBoundItem);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        var count = db.eAISpackage.Count(x =>
                            x.Effectivedate == frm.eais.Effectivedate &&
                            x.lang ==
                            Properties.Settings.Default.eAIPLanguage);
                        if (count > 1)
                        {
                            ErrorLog.ShowWarning($@"Document has not been saved! You can`t create duplicates. eAIS package with effective date: {frm.eais.Effectivedate.ToShortDateString()} is already exists. Please select another effective date or use existing package");
                        }
                        else
                        {
                            //frm.eais.Publicationdate = frm.eais.Effectivedate.AddDays(-42);
                            frm.eais.Publishingorganisation = Tpl.Text("PublishingOrganization");
                            frm.eais.State = Tpl.Text("State");
                            frm.eais.ChangedUserId = Globals.CurrentUser.id;
                            frm.eais.ChangedDate = Lib.GetServerDate() ?? DateTime.UtcNow;
                            CloneAISToAIP(frm, false);
                            db.SaveChanges();

                            if (frm.eais.eAIPpackage.eAIP.lang != oldlang)
                            {
                                RadListControl_AIS.Rebind();
                            }
                            else
                                FillAIPList();
                            eAIS_Activated(((DB.eAISpackage)currentRow?.DataBoundItem), true);
                            //Lib.CurrentAIP = Lib.CreateAmendment(Lib.CurrentAIP);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenAmdt()
        {
            try
            {
                RadListDataItem currentRow = RadListControl_AIS.SelectedItem;
                if (currentRow?.DataBoundItem is DB.eAISpackage)
                {
                    if (((DB.eAISpackage)currentRow?.DataBoundItem).Status != Status.Work)
                    {
                        ErrorLog.ShowMessage("Only eAISpackage in Work status amendment can be changed");
                        return;
                    }
                    string oldlang = ((DB.eAISpackage)currentRow.DataBoundItem).lang;
                    dynamic Obj = ((DB.eAISpackage)currentRow.DataBoundItem).eAIPpackage.eAIP.Amendment;
                    CommonFrm frm = new CommonFrm("Amendment form", Obj);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        db.SaveChanges();
                        ReloadAISList((DB.eAISpackage)currentRow.DataBoundItem);
                    }
                    else
                    {
                        db.Entry(Obj).Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void ReloadAISList(DB.eAISpackage selected = null)
        {
            try
            {
                FillAIPList();
                UpdateAISList(selected);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        public bool IsFillAllow<T>()
            where T : class
        {
            try
            {
                dynamic ent = db.Set<T>().OfType<AIPSection>().FirstOrDefault(n => n.eAIPID == Lib.CurrentAIP.id);
                if (ent == null)
                {
                    //FillDB.CheckNewAIPStructure(ref Lib.CurrentAIP);
                    return true;
                }
                if (ent.SectionStatus == SectionStatusEnum.Filled) return false;
                else return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public bool IsSectionFilled<T>()
            where T : class
        {
            try
            {
                dynamic ent = db.Set<T>().OfType<AIPSection>().FirstOrDefault(n => n.eAIPID == Lib.CurrentAIP.id);
                if (ent != null && ent.SectionStatus == SectionStatusEnum.Filled) return true;
                else return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        private void CheckFillData(BackgroundWorker worker, string AIP_DATASET)
        {
            try
            {
                string AIP_SectionName = "";
                string AIP_ClassName = "";
                //bool IsSectionFillAllowed = false;
                List<Feature> featList = new List<Feature>();
                //AimComplexTable CT = new AimComplexTable();

                AIP_SectionName = Lib.AIPClassToSection(AIP_DATASET);
                AIP_ClassName = AIP_DATASET;
                SectionName CurrentSection = Lib.GetSectionName(AIP_ClassName);


                AddOutput($@"Checking eAIP section {AIP_SectionName}", 25, worker, Color.Black);

                AIPSection ent = db.AIPSection.AsNoTracking().FirstOrDefault(n => n.eAIPID == Lib.CurrentAIP.id && (int)n.SectionName == (int)CurrentSection);

                if (ent != null && (ent.SectionStatus == SectionStatusEnum.Filled && Properties.Settings.Default.CheckSectionForFilling))
                {
                    AddOutput($@"eAIP section {AIP_SectionName} is already filled", 100, worker, Color.OrangeRed);
                    return;
                }

                AddOutput($@"Analyzing AIXM 5.1 data and converting into eAIP DB, section  {AIP_SectionName}", 40, worker);
                if (Properties.Settings.Default.PendingData) AddOutput(@"Attention: Pending Data will be received", 24, worker, Color.DarkOrange);

                FillAIPData(featList, AIP_DATASET);
                if (RefreshSection)
                {
                    SelectedNodeChanged();
                    RefreshSection = false;
                }
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
                    btn_StopClicked(false);
                    IsRunning = true;
                    btn_Stop.Visible = true;
                    MainTLP.Enabled = false;
                    pnl_OtherPages.Enabled = false;
                    rpv_Sup.Enabled = false;
                    tlp_Aic.Enabled = false;
                    tableLayoutPanel3.Enabled = false;
                    rm_Preview.Enabled = false;
                    rm_Publish.Enabled = false;
                    rm_AIP_Language.Enabled = false;
                    rm_File.Enabled = false;
                    rm_Tools.Enabled = false;
                    rm_Settings.Enabled = false;
                    rm_Windows.Enabled = false;
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
                    IsAllRunning = false;
                    IsRunning = false;
                    btn_Stop.Visible = false;
                    MainTLP.Enabled = true;
                    pnl_OtherPages.Enabled = true;
                    rpv_Sup.Enabled = true;
                    tlp_Aic.Enabled = true;
                    tableLayoutPanel3.Enabled = true;
                    rm_Preview.Enabled = true;
                    rm_Publish.Enabled = true;
                    rm_AIP_Language.Enabled = true;
                    rm_File.Enabled = true;
                    rm_Tools.Enabled = true;
                    rm_Settings.Enabled = true;
                    rm_Windows.Enabled = true;
                    StopProgress();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void bg_DatasetFill_DoWork(object sender, DoWorkEventArgs e)
        {
            PreLongAction();
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                worker?.ReportProgress(10);
                Cache.Clear(); // for better and stable results clear cache before import data from AIXM
                if (Lib.CurrentAIP == null) return;
                AdditionalInfo info = (AdditionalInfo)e.Argument;
                SectionName sectionEnum = Lib.GetSectionName(AIP_FILL);
                if (AIP_FILL == "All" || sectionEnum == SectionName.None)
                {
                    if (AIP_FILL.EndsWith("_")) AIP_FILL = AIP_FILL.TrimEnd('_'); // Cut _ from GEN_
                    List<SectionName> lst =
                        AIP_FILL == "All" && info?.Sections?.Count > 0 ?
                        info?.Sections :
                        AIP_FILL == "All" ?
                        Lib.SectionByAttribute(SectionParameter.Fill) :
                        Lib.SectionByAttribute(SectionParameter.Fill)
                        .Where(x => x.ToString()
                        .StartsWith(AIP_FILL))
                        .ToList();
                    List<DB.AirportHeliport> airportHeliportList = (AIP_FILL == "All" || AIP_FILL.StartsWith("AD2") || AIP_FILL.StartsWith("AD3")) ? db.AirportHeliport
                        .AsNoTracking()
                        .Where(n => n.eAIPID == Lib.CurrentAIP.id)
                        .OrderBy(x => x.LocationIndicatorICAO)
                        .ToList() : new List<AirportHeliport>();
                    // ForEach rewrite AIP_FILL - be carefull on change
                    foreach (SectionName item in lst)
                    {
                        if (UserTerminated()) return;
                        AIP_FILL = item.ToString();
                        if (AIP_FILL.StartsWith("AD2") || AIP_FILL.StartsWith("AD3"))
                        {
                            string section = AIP_FILL.StartsWith("AD2") ? "AD2" : "AD3";
                            Func<DB.AirportHeliport, bool> ahQuery =
                                n => AIP_FILL.StartsWith("AD2")
                                    ? n.Type == CodeAirportHeliport.AD || n.Type == CodeAirportHeliport.AH
                                    : n.Type == CodeAirportHeliport.HP;
                            foreach (DB.AirportHeliport air in airportHeliportList.Where(ahQuery))
                            {
                                if (UserTerminated()) return;
                                CheckFillData(worker, $@"{section}.{air.LocationIndicatorICAO}");
                                if (info?.runXmlGeneration == true)
                                {
                                    GenerateXMLSection($@"{section}.{air.LocationIndicatorICAO}");
                                }
                            }
                        }
                        else
                        {
                            CheckFillData(worker, AIP_FILL);
                            if (info?.runXmlGeneration == true)
                            {
                                GenerateXMLSection(item.ToString());
                            }
                        }
                    }
                }
                else
                {
                    CheckFillData(worker, AIP_FILL);
                    if (info?.runXmlGeneration == true)
                    {
                        GenerateXMLSection(AIP_FILL);
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                PostLongAction();
            }
        }

        private void bg_DatasetFill_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            radProgressBar1.Value1 = e.ProgressPercentage;
        }

        private void bg_DatasetFill_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AddOutput($@"AIXM 5.1 Data received for {CultureInfo.GetCultureInfo(Lib.AIPLanguage)?.EnglishName} AIP with Effective Date {Lib.CurrentAIP?.Effectivedate:yyyy-MM-dd}. Operation completed!", 0, null, Color.DarkGreen);
            //dock_Output.Hide();
        }


        private void btn_Save_Click(object sender, EventArgs e)
        {
            ((RadButton)sender).Enabled = false;
            db.SaveChanges();
            ((RadButton)sender).Enabled = true;
        }

        /// <summary>
        /// Preview AIP with or without AMDT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Preview_Click(object sender, EventArgs e)
        {
            RadButton rb = null;
            try
            {
                rb = sender as RadButton;
                rb.Enabled = false;
                bool AmPreview = (rb.Name.Contains("btn_AMDT_Preview")) ? true : false;
                if (radDataEntry1.DataSource != null && TreeView.SelectedNode != null)
                {
                    PreviewSection(AmPreview);
                }
                else
                {
                    ErrorLog.ShowInfo("To preview AIP section, please select it from left tree AIP menu");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException("Error in the btn_Preview_Click", ex, true);
            }
            finally
            {
                if (rb != null) rb.Enabled = true;
            }

        }

        private void PreviewSection(bool AmPreview, bool? _isHtml = null, DB.TemporalityEntity entity = null)
        {
            PreLongAction();
            try
            {
                if (!CurrentAIPSelected()) return;
                PreviewSectionBase(AmPreview, _isHtml, entity);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                PostLongAction();
            }
        }

        private void PreviewSectionBase(bool AmPreview, bool? _isHtml, DB.TemporalityEntity entity = null)
        {
            try
            {
                bool isHtml = _isHtml.IsNull() || _isHtml == true ? true : false;
                Globals.IsAMDTPreview = (AmPreview) ? true : false;
                RadTreeNode selectedNode = TreeView.SelectedNode;
                if (!string.IsNullOrEmpty(selectedNode?.Name) || entity != null)
                {
                    MethodInfo method = typeof(AIP.GUI.Main).GetMethod("PreviewXML");
                    var objlist = method?.Invoke(this, new object[] { selectedNode?.Name, isHtml, entity });
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private bool CurrentAIPSelected()
        {
            if (Lib.CurrentAIP == null)
            {
                MessageBox.Show(@"Please select Current AIP");
                return false;
            }
            else
                return true;
        }

        private void eAIS_Deactivated()
        {
            try
            {
                Lib.CurrentAIS = null;
                Lib.PrevousAIS = null;
                Lib.CurrentAIP = null;
                Lib.PrevousAIP = null;
                SaveSettingsLastActivatedAIP();
                ClearAISTextbox();
                Lib.TargetDir = Lib.SourceDirCat = DefaultArguments = DefaultInputFile = DefaultOutputFile = "";
                DefaultPDFArguments.Clear();
                ClearGridView();
                ClearRadEntry();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in the eAIS_Activated: {ex.Message}");
            }
        }

        public delegate void eAIS_ActivatedDelegate(DB.eAISpackage ais, bool PreGenerate = false);
        public void eAIS_Activated(DB.eAISpackage ais, bool PreGenerate = false)
        {
            if (InvokeRequired)
            {
                Invoke(new eAIS_ActivatedDelegate(eAIS_Activated), ais, PreGenerate);
            }
            else
            {
                try
                {

                    if (ais?.eAIPpackage?.eAIP == null)
                        throw new ArgumentNullException("ais.eAIPpackage.eAIP");
                    string pdfArg = "";
                    DefaultPDFArguments.Clear();
                    SaveSettingsLastActivatedAIP(ais);

                    Lib.CurrentAIS = ais;
                    Lib.PrevousAIS = db.eAISpackage
                        .Where(n => n.Effectivedate < ais.Effectivedate && n.lang == ais.lang)
                        .OrderByDescending(n => n.Effectivedate)
                        .Include(n => n.eAIPpackage.eAIP.Amendment.Group)
                        .FirstOrDefault();
                    Lib.CurrentAIP = ais?.eAIPpackage?.eAIP;
                    Lib.PrevousAIP = Lib.PrevousAIS?.eAIPpackage?.eAIP;

                    UpdateAISTextbox(ais, Lib.PrevousAIS);

                    // Is it AIRAC date, adding "-AIRAC" to end of Source and target dir
                    string txt_airac = Lib.IsAIRAC(Lib.CurrentAIS.Effectivedate) ? "-AIRAC" : "";
                    string dateAip = Lib.CurrentAIS.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                    Report.ErrorLogFileName = $@"Report_{dateAip}.log";

                    Lib.SourceDir = Lib.SourceDirTemplate.Replace("{DATE}", dateAip);
                    Lib.RemoteOutputDir = dateAip;
                    Lib.SourcePdfDir =
                        Path.Combine(Lib.SourceDir, "with-amdt-info"); // $@"{Lib.SourceDir}with-amdt-info\";
                    Lib.SourceDirCat = Lib.SourceCatDirTemplate.Replace("{DATE}", dateAip); //.Replace("{CAT}", "eAIP");
                    Lib.TargetDir = Lib.TargetDirTemplate.Replace("{DATE}", dateAip); //.Replace("{CAT}", "eAIP");

                    Lib.OutputDir = Lib.OutputDirTemplate.Replace("{DATE}", dateAip);
                    Lib.TargetPdfDir = Path.Combine(Lib.OutputDir, "pdf"); //$@"{Lib.OutputDir}pdf\";
                    DefaultArguments = "{SECTION}"
                                       + @" -d " + "\"" + Lib.SourceDirCat + "\""
                                       + @" -t " + "\"" + Lib.TargetDir + "\""
                                       + @" -l " + Lib.CurrentAIS.lang
                                       + @" -c " + Lib.CurrentAIS.ICAOcountrycode;
                    DefaultArguments = Lib.AdditionalParams(DefaultArguments);


                    DefaultPDFArguments.Add("{SECTION}" + @" -f fo -d " + "\"" + Lib.SourceDirCat + "\"" + @" -t " +
                                            "\"" + Lib.SourcePdfDir + "\"" + @" -l " + Lib.CurrentAIS.lang + @" -c " +
                                            Lib.CurrentAIS.ICAOcountrycode);
                    //DefaultPDFArguments.Add("{SECTION}" + @" -f at -s " + "\"" + Lib.SourcePdfDir + "\"" + @" -t " + "\"" + Lib.SourcePdfDir + "\"" + @" -l " + Lib.CurrentAIS.lang + @" -c " + Lib.CurrentAIS.ICAOcountrycode);
                    DefaultPDFArguments.Add("{SECTION}" + @" -f pdf -s " + "\"" + Lib.SourcePdfDir + "\"" + @" -t " +
                                            "\"" + Lib.TargetPdfDir + "\"" + @" -l " + Lib.CurrentAIS.lang + @" -c " +
                                            Lib.CurrentAIS.ICAOcountrycode);
                    DefaultPDFArguments = DefaultPDFArguments.Select(Lib.AdditionalParams).ToList();

                    DefaultInputFile = Path.Combine(Lib.SourceDirCat,
                        Lib.CurrentAIS.ICAOcountrycode.ToUpperInvariant() + "-"
                        + "{SECTION}" + @"-"
                        + Lib.CurrentAIS.lang + @".xml");
                    DefaultOutputFile = Path.Combine(Lib.TargetDir,
                        Lib.CurrentAIS.ICAOcountrycode.ToUpperInvariant() + "-"
                        + "{SECTION}" + @"-"
                        + Lib.CurrentAIS.lang + @".html");
                    DefaultPdfOutputFile = Path.Combine(Lib.TargetPdfDir,
                        Lib.CurrentAIS.ICAOcountrycode.ToUpperInvariant() + "-"
                        + "{SECTION}" + @"-"
                        + Lib.CurrentAIS.lang + @".pdf");

                    if (Lib.CurrentAIS.Status == Status.Published)
                    {
                        ClearGridView();
                        ClearRadEntry();
                        UpdateAirportHeliport();
                        UpdateAISList();
                        documentContainer1.Visible = false;
                        return;
                    }
                    else
                    {
                        documentContainer1.Visible = true;
                    }

                    WriteAISPackages();
                    WriteAISPackage();
                    foreach (PathCategory pathCat in Enum.GetValues(typeof(PathCategory)))
                    {
                        if (!Directory.Exists(Lib.SourceDirCat.WithCategory(pathCat)))
                            Directory.CreateDirectory(Lib.SourceDirCat.WithCategory(pathCat));
                    }

                    Lib.FileGenerate("eAIP", Lib.CurrentAIS, Lib.SourceDirCat.WithCategory());
                    foreach (PathCategory pathCat in Enum.GetValues(typeof(PathCategory)))
                    {
                        if (!Directory.Exists(Lib.TargetDir.WithCategory(pathCat)))
                        {
                            Directory.CreateDirectory(Lib.TargetDir.WithCategory(pathCat));

                        }
                    }

                    if (Lib.IsHashChanged(Properties.Resources.SourcePack, Path.Combine(Lib.SourceDir, "SourcePack.hash")))
                        Lib.ExtractFiles(Properties.Resources.SourcePack, Lib.SourceDir);

                    if (Lib.IsHashChanged(Properties.Resources.OutputPack,
                        Path.Combine(Lib.TargetDir, @"..\..", "OutputPack.hash")))
                        Lib.ExtractFiles(Properties.Resources.OutputPack,
                            Path.Combine(Lib.TargetDir.WithCategory(), @"..\.."));
                    if (!Directory.Exists(Lib.TargetPdfDir))
                    {
                        Directory.CreateDirectory(Lib.TargetPdfDir);
                    }


                    ClearGridView();
                    ClearRadEntry();
                    UpdateAirportHeliport();
                    UpdateAISList();


                    refreshSupList();
                    updateCircular();
                    UpdatePageList();


                    if (PreGenerate) GenerateLinkedSections();
                }
                catch (Exception ex)
                {
                    ErrorLog.ShowMessage($@"Error in the eAIS_Activated: {ex.Message}");
                }
            }
        }

        private void UpdateAISList(DB.eAISpackage selected = null)
        {
            try
            {
                if (RadListControl_AIS.Items.Count == 0) return;
                foreach (RadListDataItem ais in RadListControl_AIS.Items)
                {
                    if (ais.DataBoundItem is DB.eAISpackage)
                    {
                        if (((DB.eAISpackage)ais.DataBoundItem)?.id == Lib.CurrentAIS?.id)
                            ais.Image = Properties.Resources.on;
                        else
                            ais.Image = Properties.Resources.off;

                        if (((DB.eAISpackage)ais.DataBoundItem)?.id == selected?.id)
                            ais.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void UpdateAirportHeliport()
        {
            try
            {

                List<Aran.Aim.Features.AirportHeliport> aixm_ah_list = Lib.GetAIXMAirportHeliport();
                if (aixm_ah_list.Count > 0)
                {
                    List<DB.AirportHeliport> airportHeliportList = db.AirportHeliport.AsNoTracking().Where(n => n.eAIPID == Lib.CurrentAIP.id).OrderBy(x => x.LocationIndicatorICAO).ToList();
                    if (airportHeliportList.Count == 0)
                    {
                        foreach (Aran.Aim.Features.AirportHeliport ah in aixm_ah_list)
                        {
                            DB.AirportHeliport airportHeliport = new DB.AirportHeliport();
                            airportHeliport.eAIP = Lib.CurrentAIP;
                            airportHeliport.eAIPID = Lib.CurrentAIP.id;
                            airportHeliport.LocationIndicatorICAO = ah.LocationIndicatorICAO;
                            airportHeliport.Name = ah.Name;
                            airportHeliport.Type = ah.Type;
                            airportHeliportList.Add(airportHeliport);
                        }
                        db.AirportHeliport.AddRange(airportHeliportList);
                        db.SaveChanges();
                        airportHeliportList = db.AirportHeliport.AsNoTracking().Where(n => n.eAIPID == Lib.CurrentAIP.id).OrderBy(x => x.LocationIndicatorICAO).ToList();
                    }
                    string file = Path.Combine(Lib.CurrentDir, "Settings",
                                               $@"menu_{Properties.Settings.Default.eAIPLanguage}.xml");
                    // $@"{Lib.CurrentDir}\Settings\menu_{Properties.Settings.Default.eAIPLanguage}.xml";
                    if (File.Exists(file))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(file);
                        XmlNode userNode = xmlDoc.SelectSingleNode("//TreeView/Nodes/Nodes[@Name='AD2']");
                        if (userNode != null)
                        {
                            userNode.InnerXml = "";
                            foreach (DB.AirportHeliport item in airportHeliportList.Where(n => n.Type == CodeAirportHeliport.AD || n.Type == CodeAirportHeliport.AH))
                            {
                                XmlElement Child = xmlDoc.CreateElement("Nodes");
                                Child.SetAttribute("Name", "AD2." + item.LocationIndicatorICAO);
                                Child.SetAttribute("Text", item.LocationIndicatorICAO + " " + item.Name);
                                userNode.AppendChild(Child);
                            }
                        }
                        userNode = xmlDoc.SelectSingleNode("//TreeView/Nodes/Nodes[@Name='AD3']");
                        if (userNode != null)
                        {
                            userNode.InnerXml = "";
                            foreach (DB.AirportHeliport item in airportHeliportList.Where(n => n.Type == CodeAirportHeliport.HP))
                            {
                                XmlElement Child = xmlDoc.CreateElement("Nodes");
                                Child.SetAttribute("Name", "AD3." + item.LocationIndicatorICAO);
                                Child.SetAttribute("Text", item.LocationIndicatorICAO + " " + item.Name);
                                userNode.AppendChild(Child);
                            }
                        }
                        xmlDoc.Save(file);
                        if (File.Exists(file))
                        {
                            LoadXML(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        /// <summary>
        /// Fix to prevent RadBreadCrumb error
        /// </summary>
        /// <param name="file">TreeView file path</param>
        private void LoadXML(string file)
        {
            try
            {
                radBreadCrumb1.DefaultTreeView = null;
                TreeView.ClearSelection();
                TreeView.Nodes.Refresh();
                TreeView.LoadXML(file);
                radBreadCrumb1.DefaultTreeView = TreeView;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void SaveSettingsLastActivatedAIP(eAISpackage ais = null)
        {
            try
            {
                Properties.Settings.Default.CurrentAISID = (ais == null) ? 0 : ais.id;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void WriteAISPackages()
        {
            try
            {
                string txt_airac = "";
                List<eAISpackage> ais_lst = db.eAISpackage.Where(n => n.Status != Status.None).ToList();

                AIS.XML.eAISpackages aises = new AIS.XML.eAISpackages();
                aises.Version = "2.0";
                aises.Publicationdate = Lib.CurrentAIS.Publicationdate?.ToString("yyyy-MM-dd");

                List<AIS.XML.eAISpackagereference> ref_lst = new List<AIS.XML.eAISpackagereference>();
                foreach (eAISpackage item in ais_lst)
                {
                    txt_airac = Lib.IsAIRAC(item.Effectivedate) ? "-AIRAC" : "";

                    ref_lst.Add(new AIS.XML.eAISpackagereference() { href = item.Effectivedate.ToString("yyyy-MM-dd") + txt_airac + @"/eAIS-package.xml" });
                }
                aises.eAISpackagereference = ref_lst.ToArray();
                AIPXMLWrite(aises, Path.Combine(Lib.CurrentDir, "Data", "eAIP-source", "eAIS-packages.xml"));



                //return ais_lst;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                //return null;
            }

        }

        /// <summary>
        /// Creates eAIS-package.xml
        /// </summary>
        private void WriteAISPackage()
        {
            try
            {
                if (Lib.CurrentAIS == null)
                {
                    ErrorLog.ShowWarning("Current AIS not defined");
                    return;
                }
                string txt_airac = Lib.IsAIRAC(Lib.CurrentAIS.Effectivedate) ? "-AIRAC" : "";

                AIS.XML.eAISpackage eAISPackage = new AIS.XML.eAISpackage();
                eAISPackage.ICAOcountrycode = Lib.CurrentAIS.ICAOcountrycode;
                eAISPackage.Packagename = Lib.CurrentAIS.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                eAISPackage.Publicationdate = Lib.CurrentAIS.Publicationdate?.ToString("yyyy-MM-dd");// "2017-02-02";
                eAISPackage.Publishingorganisation = Tpl.Text("PublishingOrganization");
                eAISPackage.State = Tpl.Text("State");
                eAISPackage.eAIPpackage = new AIS.XML.eAIPpackage();
                // TODO: While the same name as AIS, must be asked
                eAISPackage.eAIPpackage.Packagename = Lib.CurrentAIS.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                eAISPackage.eAIPpackage.Languageversion = new AIS.XML.Languageversion[]
                {
                    new AIS.XML.Languageversion() {
                        href = $@"eAIP/{Lib.CurrentAIP.ICAOcountrycode.ToUpperInvariant()}-eAIP-{Lib.CurrentAIP.lang}.xml", lang = Lib.CurrentAIP.lang }
                };
                eAISPackage.Description = new AIS.XML.Description[]
                {
                    new AIS.XML.Description() {
                        Short = "Initial electronic AIP (Effective "+Lib.CurrentAIS.Effectivedate.ToString("yyyy-MM-dd")+")", lang = Lib.CurrentAIP.lang,
                        Items = new object[] {
                            new AIS.XML.p() { Items = new object[] { @"<p>This eAIS package contains:</p><ul><li>Initial issue of this electronic AIP edition</li></ul>" }  }
                        } }
                };


                // Adding eSUP and eAIC
                int langId = Lib.GetLangIdByValue(Lib.CurrentAIP.lang) ?? 0;
                var queryS = db.Supplement
                    .Where(x => x.IsCanceled == false && x.LanguageReferenceId == langId)?
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .ToList();
                var queryC = db.Circular
                    .Where(x => x.IsCanceled == false && x.LanguageReferenceId == langId)?
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .ToList();

                if (queryS.Count > 0)
                {
                    List<eSUPs> eSupList = new List<eSUPs>();
                    foreach (var sup in queryS)
                    {
                        eSUPs esups = new eSUPs();
                        esups.eSUPreference = new[] { new eSUPreference { Packagename = eAISPackage.Packagename, Languageversion = new[] { new Languageversion() { href = $@"eSUP/{Lib.CurrentAIP.ICAOcountrycode.ToUpperInvariant()}-eSUP-{sup.Year}-{sup.Number}-{Lib.CurrentAIP.lang}.xml", lang = Lib.CurrentAIP.lang } } } };
                        eSupList.Add(esups);
                    }
                    eAISPackage.eSUPs = eSupList.ToArray();
                }
                if (queryC.Count > 0)
                {
                    List<eAICs> eAicList = new List<eAICs>();
                    foreach (var aic in queryC)
                    {
                        eAICs eaics = new eAICs();
                        eaics.eAICreference = new[] { new eAICreference { Packagename = eAISPackage.Packagename, Languageversion = new[] { new Languageversion() { href = $@"eAIC/{Lib.CurrentAIP.ICAOcountrycode.ToUpperInvariant()}-eAIC-{aic.Year}-{aic.Number}-{aic.Series}-{Lib.CurrentAIP.lang}.xml", lang = Lib.CurrentAIP.lang } } } };
                        eAicList.Add(eaics);
                    }
                    eAISPackage.eAICs = eAicList.ToArray();
                }
                // End

                string folder = Lib.SourceCatDirTemplate.Replace(@"{DATE}\{CAT}", $@"{Lib.CurrentAIS.Effectivedate.ToString("yyyy-MM-dd") + txt_airac}");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                string file = Path.Combine(folder, "eAIS-package.xml");// $@"{folder}\eAIS-package.xml";

                Dictionary<string, string> child = new Dictionary<string, string>();
                child.Add("xml-stylesheet", $@"href='..\xslt\html-content.xslt' type='text/xsl'");
                child.Add("eAIP-locales", $@"locales.xml");
                //AISXMLWrite(eAISPackage, file, child);

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("p", "http://www.eurocontrol.int/xmlns/AIM/eAIS-package");
                //ns.Add("x", "http://www.w3.org/1999/xhtml");
                ns.Add("xlink", "http://www.w3.org/1999/xlink");
                ConvertClassToXML(eAISPackage, file, ns, child);

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radDataEntry1_BindingCreated(object sender, BindingCreatedEventArgs e)
        {
            if (e.DataMember == "Navaid")
            {

            }
        }

        private void radDataEntry1_BindingCreating(object sender, BindingCreatingEventArgs e)
        {
            // Stupid telerik fix
            if (e.Control is RadDropDownList)
            {
                e.PropertyName = "SelectedValue";
            }

        }

        private void radDataEntry1_ItemInitializing(object sender, ItemInitializingEventArgs e)
        {
            if (e.Panel.Controls[1].Text.EndsWith("Hidden"))
            {
                e.Cancel = true;
            }
        }

        public T SetValue<T>(string value) where T : class, new()
        {
            if (value == null) return null;
            T newprop = new T();
            PropertyInfo propInfo = newprop.GetType().GetProperty("Items");
            propInfo.SetValue(newprop, new object[] { value });
            return newprop;
        }

        private void CreateAIPMenuItems()
        {
            //try
            //{
            //    RadMenuItem newFillItem, newGenItem;
            //    Regex digitsOnly = new Regex(@"[^\d]");

            //    //foreach (string menu in Lib.SectionList.Keys)
            //    //{
            //    //    bool IsActive = ActivatedMenus.Contains(menu);
            //    //    string menuT = Lib.AIPClassToSection(menu);
            //    //    // FillDataset items
            //    //    newFillItem = new RadMenuItem(menuT);
            //    //    newFillItem.Name = $@"rmi_Fill_{menu}";
            //    //    newFillItem.Enabled = IsActive;
            //    //    newFillItem.Click += MainMenu_Click;
            //    //    if (menu.StartsWith("GEN")) rmi_AIP_GEN.Items.Add(newFillItem);
            //    //    else if (menu.StartsWith("ENR")) rmi_AIP_ENR.Items.Add(newFillItem);
            //    //    else if (menu.StartsWith("AD")) rmi_AIP_AD.Items.Add(newFillItem);

            //    //    // Generate AIP items
            //    //    newGenItem = new RadMenuItem(menuT);
            //    //    newGenItem.Name = $@"rmi_Gen_{menu}";
            //    //    newGenItem.Enabled = IsActive;
            //    //    newGenItem.Click += MainMenu_Click;
            //    //    if (menu.StartsWith("GEN")) rmi_Gen_GEN.Items.Add(newGenItem);
            //    //    else if (menu.StartsWith("ENR")) rmi_Gen_ENR.Items.Add(newGenItem);
            //    //    else if (menu.StartsWith("AD")) rmi_Gen_AD.Items.Add(newGenItem);
            //    //}

            //    List<SectionName> fillList = Lib.SectionByAttribute(SectionParameter.Fill);
            //    List<SectionName> genList = Lib.SectionByAttribute(SectionParameter.Generate);

            //    foreach (SectionName menu in fillList)
            //    {

            //        string menuT = menu.ToName();

            //        if(menu.ToString().StartsWith("AD2"))
            //        // FillDataset items
            //        newFillItem = new RadMenuItem(menuT) {Name = $@"rmi_Fill_{menu.ToString()}"};
            //        newFillItem.Click += MainMenu_Click;
            //        if (menu.ToString().StartsWith("GEN")) rmi_AIP_GEN.Items.Add(newFillItem);
            //        else if (menu.ToString().StartsWith("ENR")) rmi_AIP_ENR.Items.Add(newFillItem);
            //        else if (menu.ToString().StartsWith("AD")) rmi_AIP_AD.Items.Add(newFillItem);



            //        // Generate AIP items
            //        //newGenItem = new RadMenuItem(menuT);
            //        //newGenItem.Name = $@"rmi_Gen_{menu}";
            //        //newGenItem.Enabled = IsActive;
            //        //newGenItem.Click += MainMenu_Click;
            //        //if (menu.StartsWith("GEN")) rmi_Gen_GEN.Items.Add(newGenItem);
            //        //else if (menu.StartsWith("ENR")) rmi_Gen_ENR.Items.Add(newGenItem);
            //        //else if (menu.StartsWith("AD")) rmi_Gen_AD.Items.Add(newGenItem);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            //}
        }

        private void TestCommitAsCorrection()
        {
            try
            {
                Guid id = new Guid("8a896a1a-175a-4c4f-ba31-fdc204a2007b");
                RulesProcedures ft = Globals.GetFeaturesByED(FeatureType.RulesProcedures, id).OfType<RulesProcedures>().FirstOrDefault();
                if (ft != null)
                {
                    MessageBox.Show(ft.Content);
                    ft.Content = "Added text from AIP. " + ft.Content;
                    CommonDataProvider.CommitAsCorrection(ft);
                    RulesProcedures ft2 = Globals.GetFeaturesByED(FeatureType.RulesProcedures, id).OfType<RulesProcedures>().FirstOrDefault();
                    MessageBox.Show(ft2?.Content);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void TestPDFEditor()
        {
            try
            {
                string myPath = @"D:\AirNav\bin\Debug\Data\eAIP-output\2018-09-13-AIRAC\pdf\EV-GEN-3.4-en-GB.pdf";
                string myPath1 = @"D:\AirNav\bin\Debug\Data\eAIP-output\2018-09-13-AIRAC\pdf\EV-GEN-3.4-en-GB_new.pdf";

                PdfReader reader = new PdfReader(myPath);
                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
                iTextSharp.text.Document document = new iTextSharp.text.Document(size);

                FileStream fs = new FileStream(myPath1, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                PdfContentByte cb = writer.DirectContent;


                for (int i = 1; i <= reader.NumberOfPages; i++)
                {

                    PdfImportedPage page = writer.GetImportedPage(reader, i);

                    StringWriter output = new StringWriter();
                    output.WriteLine(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i, new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy()));

                    string content = output.ToString();



                    if ((i % 2 == 0 && content.Contains("PAGE INTENTIONALLY LEFT BLANK")))
                    {
                        document.NewPage();
                        cb.AddTemplate(page, 0, 0);
                        continue;
                    }
                    else if ((content.Contains("\r\n") && content.Length < 10))
                    {
                        continue;
                    }
                    else if ((content.Contains("PAGE INTENTIONALLY LEFT BLANK") && content.Length < 40))
                    {
                        continue;
                    }



                    document.NewPage();
                    cb.AddTemplate(page, 0, 0);

                    bool existChanges = ComparePdfPages(content, i);
                    if (!existChanges)
                    {
                        var dateFont = iTextSharp.text.FontFactory.GetFont("Arial", 10, iTextSharp.text.BaseColor.BLACK);
                        iTextSharp.text.Phrase myText = new iTextSharp.text.Phrase("30 FEB 2018", dateFont);

                        iTextSharp.text.Rectangle rectangle;
                        ColumnText ct = new ColumnText(cb);

                        if (i % 2 != 0)
                        {
                            rectangle = new iTextSharp.text.Rectangle(490, 804, 560, 819);
                            ct.SetSimpleColumn(myText, 490, 804, 554, 819, 12, iTextSharp.text.Element.ALIGN_RIGHT);
                        }
                        else
                        {
                            rectangle = new iTextSharp.text.Rectangle(40, 804, 110, 819);
                            ct.SetSimpleColumn(myText, 43, 804, 107, 819, 12, iTextSharp.text.Element.ALIGN_LEFT);
                        }

                        rectangle.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
                        cb.Rectangle(rectangle);
                        cb.Stroke();

                        ct.Go();
                    }
                }

                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private bool ComparePdfPages(string text, int pageNumber)
        {
            try
            {
                var myPath = @"D:\AirNav\bin\Debug\Data\eAIP-output\2018-09-13-AIRAC\pdf\EV-GEN-3.4-en-GB_old.pdf";

                string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Skip(2).Reverse().Skip(2).Reverse().ToArray();

                string newContent = string.Join(Environment.NewLine, lines);

                PdfReader reader = new PdfReader(myPath);

                if (reader.NumberOfPages < pageNumber)
                    return false;
                StringWriter output = new StringWriter();
                output.WriteLine(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageNumber, new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy()));

                string[] oldLines = output.ToString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Skip(2).Reverse().Skip(2).Reverse().ToArray();

                string oldContent = string.Join(Environment.NewLine, oldLines);

                if (Lib.ComparePdfdContent(newContent, oldContent))
                    return false;
                else return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        private void TestEditor()
        {
            try
            {
                Lib.LoadXhtmlControlResources();

                // test code
                //                var richTextDialog = new XhtmlEditor2();
                //                var viewModel = richTextDialog.DataContext as XhtmlEditorViewModel2;
                var richTextDialog = new XhtmlEditor();
                var viewModel = richTextDialog.DataContext as XhtmlEditorViewModel;
                if (viewModel == null) return;
                viewModel.Editable = true;
                string fileContent = File.Exists("tmp2.txt") ? File.ReadAllText("tmp2.txt") : "";
                fileContent = !fileContent.StartsWith("<div>") ? "<div>" + fileContent + "</div>" : fileContent;

                viewModel.HtmlText = fileContent;
                richTextDialog.DataContext = viewModel;
                ElementHost.EnableModelessKeyboardInterop(richTextDialog);
                richTextDialog.ShowDialog();
                if (viewModel.IsSaved)
                {
                    // Test section 
                    File.WriteAllText(@"tmp2.txt", viewModel.HtmlText);
                    //viewModel.HtmlText = Parser.Export(viewModel.HtmlTextAIP);
                    File.WriteAllText(@"tmp.txt", viewModel.HtmlText);

                    // ConvertToAIPXhtml include Parser
                    string xhtmlStr = XML.Parser.Export(File.ReadAllText("tmp.txt"));
                    xhtmlStr = Lib.ConvertToAIPXhtml(xhtmlStr, Lib.CurrentAIP, "GEN01");
                    string str = Resources.EV_GEN_0_1_en_GB;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic["{TEST_TEXT}"] = xhtmlStr;
                    foreach (string k in dic.Keys)
                    {
                        str = str.Replace(k, dic[k]);
                    }

                    string myPath = Environment.MachineName.Contains("EMINS") ?
                        @"C:\CurrentProjects\Panda\AirNav\bin\Debug" :
                        @"D:\AirNav\bin\Debug";
                    string myAIP = Environment.MachineName.Contains("EMINS") ?
                        @"2018-07-19-AIRAC" :
                        @"2018-09-13-AIRAC";

                    if (MessageBox.Show(
                           $@"Update EV-GEN-0.1-en-GB.xml ?",
                           @"Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.WriteAllText(
                            $@"{myPath}\Data\eAIP-source\{myAIP}\eAIP\EV-GEN-0.1-en-GB.xml",
                            str);
                    }



                    if (Properties.Settings.Default.IsPreviewExternal)
                    {
                        GenerateHtml($@"GEN-0.1 -d {myPath}\Data\eAIP-source\{myAIP}\eAIP -t {myPath}\Data\eAIP-output\{myAIP}\html\eAIP -l en-GB -c EV");
                    }

                    if (Properties.Settings.Default.GeneratePDF)
                    {

                        IsRunning = true;
                        GeneratePdf(new List<string>()
                            {
                                $@"GEN-0.1 -f fo -d {myPath}\Data\eAIP-source\{myAIP}\eAIP -t {myPath}\Data\eAIP-source\{myAIP}\with-amdt-info\ -l en-GB -c EV",
                                //@"GEN-0.1 -f at -s D:\AirNav\bin\Debug\Data\eAIP-source\2018-09-13-AIRAC\\with-amdt-info\ -t D:\AirNav\bin\Debug\Data\eAIP-source\2018-09-13-AIRAC\\with-amdt-info\ -l en-GB -c EV",
                                $@"GEN-0.1 -f pdf -s {myPath}\Data\eAIP-source\{myAIP}\with-amdt-info\ -t {myPath}\Data\eAIP-output\{myAIP}\pdf\ -l en-GB -c EV"
                            },
                            $@"{myPath}\Data\eAIP-output\{myAIP}\pdf\EV-GEN-0.1-en-GB.pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        static List<string> FileCol = new List<string>();
        static void DirSearch(string dir, string main)
        {
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                    FileCol.Add(f.Replace(main, ""));
                foreach (string d in Directory.GetDirectories(dir))
                {
                    DirSearch(d, main);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static MongoClient client = new MongoClient("mongodb://localhost");
        static IMongoDatabase database = client.GetDatabase("test");
        static IGridFSBucket gridFS = new GridFSBucket(database);


        private void radMenuItem6_Click_1(object sender, EventArgs e)
        {
            try
            {
                //TestCommitAsCorrection();
                //List<int?> intLIst = null;
                //if (intLIst.ContainsValue(5))
                //TestEditor();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in Test Button: {ex.Message}");

            }
        }


        private void radMenuItem7_Click_1(object sender, EventArgs e)
        {
            try
            {
                //TestCommitAsCorrection();
                //List<int?> intLIst = null;
                //if (intLIst.ContainsValue(5))

                //RichTextLib lib = new RichTextLib(PageType.Cover, DocType.PDF, db);
                //lib.GetInsertDeleted();
                //TestPDFEditor();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowMessage($@"Error in Test Button: {ex.Message}");

            }
        }

        private async Task ClearPDFPagesInfo()
        {
            try
            {
                var pages = db.PDFPage?.Where(x => x.eAIPID == Lib.CurrentAIP.id).ToList();
                if (pages?.Any() == true)
                {
                    db.PDFPage?.RemoveRange(pages);
                    await db.SaveChangesAsync();
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
                NewAIS();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void NewAIS()
        {
            try
            {
                eAISFrm frm = new eAISFrm(new DB.eAISpackage());
                string lang = Properties.Settings.Default.eAIPLanguage ?? "en-GB";
                //db.Database.Log = Console.Write;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var count = db.eAISpackage.Count(x =>
                        x.Effectivedate == frm.eais.Effectivedate &&
                        x.lang == lang);
                    if (count > 0)
                    {
                        ErrorLog.ShowWarning($@"Document has not been saved! You can`t create duplicates. eAIS package with effective date: {frm.eais.Effectivedate.ToShortDateString()} and language: {frm.eais.lang} is already exists. Please select another effective date and language, or use existing package");
                    }
                    else
                    {
                        if (frm.eais?.Effectivedate != null)
                        {
                            //frm.eais.Publicationdate = frm.eais.Effectivedate.AddDays(-42);
                            var dt = Lib.GetServerDate() ?? DateTime.UtcNow;
                            frm.eais.Publishingorganisation = Tpl.Text("PublishingOrganization");
                            frm.eais.State = Tpl.Text("State");
                            frm.eais.CreatedUserId = Globals.CurrentUser.id;
                            frm.eais.CreatedDate = dt;
                            frm.eais.ChangedUserId = Globals.CurrentUser.id;
                            frm.eais.ChangedDate = dt;
                            frm.eais.lang = lang;
                            CloneAISToAIP(frm, true);
                            db.eAISpackage.Add(frm.eais);
                        }
                        db.SaveChanges();
                        DB.eAIP aip = frm.eais?.eAIPpackage.eAIP;
                        // Create amendment
                        if (aip != null)
                        {
                            Lib.CreateAmendment(aip, db);
                        }
                        FillAIPList();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static void CloneAISToAIP(eAISFrm frm, bool isNew)
        {
            try
            {
                if (isNew)
                {
                    frm.eais.eAIPpackage = new eAIPpackage();
                    frm.eais.eAIPpackage.Languageversion = new LanguageVersion[] { new LanguageVersion() { lang = "en-GB" } };
                    frm.eais.eAIPpackage.eAIP = new DB.eAIP();
                }
                string txt_airac = Lib.IsAIRAC(frm.eais.Effectivedate) ? "-AIRAC" : "";

                frm.eais.eAIPpackage.packagename = frm.eais.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                frm.eais.eAIPpackage.eAIP.AIPStatus = frm.eais.Status;
                frm.eais.eAIPpackage.eAIP.Effectivedate = frm.eais.Effectivedate;
                frm.eais.eAIPpackage.eAIP.ICAOcountrycode = frm.eais.ICAOcountrycode;
                frm.eais.eAIPpackage.eAIP.lang = frm.eais.lang;
                frm.eais.eAIPpackage.eAIP.Publicationdate = frm.eais.Publicationdate;
                frm.eais.eAIPpackage.eAIP.Publishingorganisation = frm.eais.Publishingorganisation;
                frm.eais.eAIPpackage.eAIP.Publishingstate = frm.eais.Publishingstate;
                frm.eais.eAIPpackage.eAIP.State = frm.eais.State;
                frm.eais.eAIPpackage.eAIP.Version = frm.eais.Version;


            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }




        private object[] AddSubsectionText(string text)
        {
            return new object[] { new div() { Items = new object[] { text } } };
        }

        private void bg_GenerateAIP_DoWork(object sender, DoWorkEventArgs e)
        {
            PreLongAction();
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                worker?.ReportProgress(2);
                string originalAIP_Generate = AIP_GENERATE;
                AdditionalInfo info = (AdditionalInfo)e.Argument;
                SectionName sectionEnum = Lib.GetSectionName(AIP_GENERATE);
                if (AIP_GENERATE == "All" || sectionEnum == SectionName.None)
                {
                    if (AIP_GENERATE.EndsWith("_")) AIP_GENERATE = AIP_GENERATE.TrimEnd('_'); // Cut _ from GEN_
                    List<SectionName> lst =
                        AIP_GENERATE == "All" && info?.Sections?.Count > 0 ?
                        info?.Sections :
                        AIP_GENERATE == "All" ?
                        Lib.SectionByAttribute(SectionParameter.Generate) :
                        Lib.SectionByAttribute(SectionParameter.Generate)
                            .Where(x => x.ToString()
                            .StartsWith(AIP_GENERATE))
                            .ToList(); ;
                    List<DB.AirportHeliport> airportHeliportList = (AIP_GENERATE == "All" || AIP_GENERATE.StartsWith("AD2") || AIP_GENERATE.StartsWith("AD3")) ? db.AirportHeliport
                        .AsNoTracking()
                        .Where(n => n.eAIPID == Lib.CurrentAIP.id)
                        .OrderBy(x => x.LocationIndicatorICAO)
                        .ToList() : new List<AirportHeliport>();
                    foreach (SectionName item in lst)
                    {
                        if (UserTerminated()) return;
                        AIP_GENERATE = item.ToString();
                        if (AIP_GENERATE.StartsWith("AD2") || AIP_GENERATE.StartsWith("AD3"))
                        {
                            string section = AIP_GENERATE.StartsWith("AD2") ? "AD2" : "AD3";
                            Func<DB.AirportHeliport, bool> ahQuery =
                                n => AIP_GENERATE == "AD2"
                                    ? n.Type == CodeAirportHeliport.AD || n.Type == CodeAirportHeliport.AH
                                    : n.Type == CodeAirportHeliport.HP;
                            foreach (DB.AirportHeliport air in airportHeliportList.Where(ahQuery))
                            {
                                if (UserTerminated()) return;
                                GenerateData(worker, $@"{section}.{air.LocationIndicatorICAO}", info?.runXmlGeneration);
                            }
                        }
                        else GenerateData(worker, AIP_GENERATE, info?.runXmlGeneration);
                    }

                    // Gen eSUP and eAIC
                    if (originalAIP_Generate == "All") GenerateSupAic();

                    // Gen search
                    // TODO: Gen Search

                }
                else
                {
                    GenerateData(worker, AIP_GENERATE, info?.runXmlGeneration);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                PostLongAction();
            }
        }

        private async Task GenerateXmlAsync()
        {
            try
            {
                await Task.Run(() => PreLongAction());

                List<SectionName> lst = Lib.SectionByAttribute(SectionParameter.Generate);
                List<DB.AirportHeliport> airportHeliportList = db.AirportHeliport
                    .AsNoTracking()
                    .Where(n => n.eAIPID == Lib.CurrentAIP.id)
                    .OrderBy(x => x.LocationIndicatorICAO)
                    .ToList();
                foreach (SectionName item in lst)
                {
                    if (UserTerminated()) return;
                    AIP_GENERATE = item.ToString();
                    if (AIP_GENERATE.StartsWith("AD2") || AIP_GENERATE.StartsWith("AD3"))
                    {
                        string section = AIP_GENERATE.StartsWith("AD2") ? "AD2" : "AD3";
                        Func<DB.AirportHeliport, bool> ahQuery =
                            n => AIP_GENERATE == "AD2"
                                ? n.Type == CodeAirportHeliport.AD || n.Type == CodeAirportHeliport.AH
                                : n.Type == CodeAirportHeliport.HP;
                        foreach (DB.AirportHeliport air in airportHeliportList.Where(ahQuery))
                        {
                            if (UserTerminated()) return;
                            await Task.Run(() => GenerateXMLSection($@"{section}.{air.LocationIndicatorICAO}"));
                        }
                    }
                    else await Task.Run(() => GenerateXMLSection(AIP_GENERATE));
                }
                await Task.Run(() => PostLongAction());

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }



        private void GenerateSupAic()
        {
            try
            {
                int langId = Lib.GetLangIdByValue(Lib.CurrentAIP.lang) ?? 0;

                var queryS = db.Supplement
                    .Where(x => x.IsCanceled == false &&
                    x.LanguageReferenceId == langId &&
                    (x.EffectivedateTo == null || x.EffectivedateTo >= Lib.CurrentAIP.Effectivedate))?
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .ToList();

                var queryC = db.Circular
                    .Where(x => x.IsCanceled == false &&
                    x.LanguageReferenceId == langId &&
                    (x.EffectivedateTo == null || x.EffectivedateTo >= Lib.CurrentAIP.Effectivedate))?
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .ToList();

                foreach (var tempEntity in queryS)
                {
                    GenerateXMLSection(String.Empty, tempEntity);
                    GenerateHtmlPdfSection(String.Empty, null, tempEntity);
                }
                foreach (var tempEntity in queryC)
                {
                    GenerateXMLSection(String.Empty, tempEntity);
                    GenerateHtmlPdfSection(String.Empty, null, tempEntity);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private bool UserTerminated()
        {
            try
            {
                if (!IsRunning)
                {
                    AddOutput("Operation terminated by user. Exiting from action.", 0, null, Color.Red);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        private bool GenerateData(BackgroundWorker worker, string AIP_GENERATE, bool? runXmlGeneration = null)
        {
            try
            {
                string AIP_SectionName = "";
                AIP_SectionName = AIP_GENERATE;
                SectionName CurrentSection = Lib.GetSectionName(AIP_GENERATE);

                if (typeof(SectionName).GetCustomAttributes<SectionOptionAttribute>()
                    .Any(x => (x.ValidOn & SectionParameter.Fill) != 0))
                {
                    AddOutput($@"Checking eAIP section {AIP_SectionName}", 25, worker, Color.Black);
                    AIPSection ent = db.AIPSection.AsNoTracking().FirstOrDefault(n =>
                        n.eAIPID == Lib.CurrentAIP.id && n.SectionName == CurrentSection);

                    if (!(ent != null && ent.SectionStatus == SectionStatusEnum.Filled))
                    {
                        AddOutput($@"eAIP section {AIP_SectionName} is not filled", 100, worker, Color.OrangeRed);
                        return false;
                    }

                    AddOutput($@"Receiving information from AIP DB for {AIP_SectionName}", 10, worker);
                }
                if (CurrentSection.HasParameterFlag(SectionParameter.AMDT))
                    Globals.IsAMDTPreview = true;
                else
                    Globals.IsAMDTPreview = false;
                if (runXmlGeneration != false)
                    GenerateXMLSection(AIP_GENERATE);
                else
                    InitCurrentSection(AIP_GENERATE);// important
                GenerateHtmlPdfSection(AIP_GENERATE);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }


        private void bg_GenerateAIP_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            radProgressBar1.Value1 = e.ProgressPercentage;
        }

        private void bg_GenerateAIP_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AddOutput($@"Meta Data, HTML and PDF files generated for {CultureInfo.GetCultureInfo(Lib.AIPLanguage)?.EnglishName} AIP with Effective Date {Lib.CurrentAIP?.Effectivedate:yyyy-MM-dd}. Operation completed!", 0, null, Color.DarkGreen);
        }

        //public void GenerateXMLSection<T>() where T : class
        //{
        //    try
        //    {
        //        string section = typeof(T).Name;

        //        // From ENR31 to ENR and 31
        //        Match result = new Regex(@"([a-zA-Z]+)(\d+)").Match(section);
        //        // From ENR and 31 to "ENR-3.1"
        //        string dotNum = string.Join(".", result?.Groups[2]?.Value?.ToString().ToCharArray());
        //        string sectionT = result?.Groups[1]?.Value + "-" + dotNum;

        //        string file = DefaultInputFile.Replace("{SECTION}", sectionT);
        //        string OutputFile = DefaultOutputFile.Replace("{SECTION}", sectionT);
        //        string Arguments = DefaultArguments.Replace("{SECTION}", sectionT);
        //        List<string> PdfArguments = DefaultPDFArguments.Select(x => x.Replace("{SECTION}", sectionT)).ToList();


        //        Type T1 = Type.GetType("AIP.XML." + section + ",AIP.XML");
        //        MethodInfo method = typeof(AIP.GUI.Classes.DS2XML).GetMethod("Build");
        //        MethodInfo generic = method.MakeGenericMethod(T1);
        //        var objlist = generic.Invoke(this, new object[] { db, section, file });
        //        GenerateHtml(Arguments);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }
        //}



        private void log_clearall_Click(object sender, EventArgs e)
        {
            try
            {
                log_output.Clear();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        /// <summary>
        /// Linked sections are common sections required for other sections generating
        /// Linked sections:
        /// GEN 0.2 - required for AMDT and Effective date in the PDF header and footer
        /// GEN 2.2 - required to insert abbreviation
        /// GEN 2.4 - required for correct AD 2 and 3 html 
        /// ENR 4.1 - required for ENR 3.1-3.5
        /// ENR 4.4 - required for ENR 3.1-3.5
        /// ENR 3.3 - required for ENR 4.4
        /// </summary>
        /// <returns></returns>
        public void GenerateLinkedSections()
        {
            try
            {
                List<SectionName> initSections = new List<SectionName>
                {
                    SectionName.GEN02,SectionName.GEN22,SectionName.GEN24
                };
                MainFillDataset("All", new AdditionalInfo(true, initSections));
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public string GenerateXMLSection(string section, DB.TemporalityEntity entity = null)
        {
            try
            {
                if (!CurrentAIPSelected()) return "";

                InitCurrentSection(section, entity);

                AddOutput($@"Generating XML file: {CurrentSection.Path.Xml}");
                Type T1 = Type.GetType("AIP.XML." + CurrentSection.Class + ",AIP.XML");
                MethodInfo method = typeof(AIP.GUI.Classes.DS2XML).GetMethod("Build");
                MethodInfo generic = method?.MakeGenericMethod(T1);
                var objlist = generic?.Invoke(this, new object[] { db, section, CurrentSection.Path.Xml, entity });

                return CurrentSection.Path.Xml;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        //public string GenerateXMLSection(string section, DB.TemporalityEntity entity = null)
        //{
        //    try
        //    {
        //        if (!CurrentAIPSelected()) return "";
        //        CurrentSection.Clear();

        //        if (entity != null)
        //        {
        //            if (entity is DB.Supplement)
        //            {
        //                CurrentSection.Class = "eSUP";
        //                CurrentSection.TextName = CurrentSection.Common = $@"{CurrentSection.Class}-{((DB.Supplement)entity).Year}-{((DB.Supplement)entity).Number}";
        //                CurrentSection.PathCategory = PathCategory.eSUP;
        //            }
        //            else if (entity is DB.Circular)
        //            {
        //                CurrentSection.Class = "eAIC";
        //                CurrentSection.TextName = CurrentSection.Common = $@"{CurrentSection.Class}-{((DB.Circular)entity).Year}-{((DB.Circular)entity).Number}-{((DB.Circular)entity).Series}";
        //                CurrentSection.PathCategory = PathCategory.eAIC;
        //            }
        //        }
        //        else if (section.Contains("."))
        //        {
        //            CurrentSection.TextName = section.Replace("2.", "-").Replace("3.", "-");
        //            CurrentSection.Common = section.Replace("2.", "-2.").Replace("3.", "-3.");
        //            CurrentSection.Class = section.Contains("2.") ? "Aerodrome" : "Heliport";
        //        }
        //        else
        //        {
        //            CurrentSection.TextName = CurrentSection.Common = Lib.AIPClassToSection(section);
        //            CurrentSection.Class = section;
        //        }

        //        CurrentSection.Path.Xml = DefaultInputFile.Replace("{SECTION}", CurrentSection.TextName).WithCategory(CurrentSection.PathCategory);
        //        CurrentSection.Path.Html = DefaultOutputFile.Replace("{SECTION}", CurrentSection.Common).WithCategory(CurrentSection.PathCategory);
        //        CurrentSection.Path.Pdf = DefaultPdfOutputFile.Replace("{SECTION}", CurrentSection.Common);
        //        CurrentSection.HtmlArg = DefaultArguments.Replace("{SECTION}", CurrentSection.Common).WithCategory(CurrentSection.PathCategory);
        //        CurrentSection.PdfArg = DefaultPDFArguments.Select(x => x.Replace("{SECTION}", CurrentSection.Common).WithCategory(CurrentSection.PathCategory)).ToList();


        //        AddOutput($@"Generating XML file: {CurrentSection.Path.Xml}");
        //        Type T1 = Type.GetType("AIP.XML." + CurrentSection.Class + ",AIP.XML");
        //        MethodInfo method = typeof(AIP.GUI.Classes.DS2XML).GetMethod("Build");
        //        MethodInfo generic = method?.MakeGenericMethod(T1);
        //        var objlist = generic?.Invoke(this, new object[] { db, section, CurrentSection.Path.Xml, entity });

        //        return CurrentSection.Path.Xml;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //        return "";
        //    }
        //}

        public bool InitCurrentSection(string section, DB.TemporalityEntity entity = null)
        {
            try
            {
                if (!CurrentAIPSelected()) return false;
                CurrentSection.Clear();

                if (entity != null)
                {
                    if (entity is DB.Supplement)
                    {
                        CurrentSection.Class = "eSUP";
                        CurrentSection.TextName = CurrentSection.Common = $@"{CurrentSection.Class}-{((DB.Supplement)entity).Year}-{((DB.Supplement)entity).Number}";
                        CurrentSection.PathCategory = PathCategory.eSUP;
                    }
                    else if (entity is DB.Circular)
                    {
                        CurrentSection.Class = "eAIC";
                        CurrentSection.TextName = CurrentSection.Common = $@"{CurrentSection.Class}-{((DB.Circular)entity).Year}-{((DB.Circular)entity).Number}-{((DB.Circular)entity).Series}";
                        CurrentSection.PathCategory = PathCategory.eAIC;
                    }
                }
                else if (section.Contains("."))
                {
                    CurrentSection.TextName = section.Replace("2.", "-").Replace("3.", "-");
                    CurrentSection.Common = section.Replace("2.", "-2.").Replace("3.", "-3.");
                    CurrentSection.Class = section.Contains("2.") ? "Aerodrome" : "Heliport";
                }
                else
                {
                    CurrentSection.TextName = CurrentSection.Common = Lib.AIPClassToSection(section);
                    CurrentSection.Class = section;
                }

                CurrentSection.Path.Xml = DefaultInputFile.Replace("{SECTION}", CurrentSection.TextName).WithCategory(CurrentSection.PathCategory);
                CurrentSection.Path.Html = DefaultOutputFile.Replace("{SECTION}", CurrentSection.Common).WithCategory(CurrentSection.PathCategory);
                CurrentSection.Path.Pdf = DefaultPdfOutputFile.Replace("{SECTION}", CurrentSection.Common);
                CurrentSection.HtmlArg = DefaultArguments.Replace("{SECTION}", CurrentSection.Common).WithCategory(CurrentSection.PathCategory);
                CurrentSection.PdfArg = DefaultPDFArguments.Select(x => x.Replace("{SECTION}", CurrentSection.Common).WithCategory(CurrentSection.PathCategory)).ToList();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public string GenerateHtmlPdfSection(string section, bool? isHtml = null, DB.TemporalityEntity entity = null)
        {
            try
            {
                if (!CurrentAIPSelected()) return "";

                if (isHtml.IsNull() || isHtml == true)
                {
                    GenerateHtml(CurrentSection.HtmlArg);
                }
                if (isHtml.IsNull() || isHtml == false)
                {
                    if (section?.Contains("GEN04") == true)
                    {
                        string oldFileName = Path.GetFileName(CurrentSection.Path.Pdf);
                        string newFileName = oldFileName?
                            .Replace(Lib.CurrentAIS.lang, Lib.PdfLang(Lib.CurrentAIS.lang))
                            .Replace("-", "_")
                            .Replace(".", "_")
                            .Replace("_pdf", ".pdf");
                        CurrentSection.Path.Pdf = CurrentSection.Path.Pdf.Replace(oldFileName, newFileName);
                        PDFManager.GenerateGEN04Section(db, CurrentSection.Path.Pdf);
                        return CurrentSection.Path.Pdf;
                    }
                    if ((isHtml.IsNull() && (Properties.Settings.Default.GeneratePDF || IsPublishProcess)) || isHtml == false)
                        CurrentSection.Path.Pdf = GeneratePdf(CurrentSection.PdfArg, CurrentSection.Path.Pdf, section);
                }

                return (isHtml == true || isHtml.IsNull()) ? CurrentSection.Path.Html : CurrentSection.Path.Pdf;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        private void log_output_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText.Replace("%20", " "));
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_StopClicked(bool clicked)
        {
            try
            {
                btn_Stop.Text = clicked ? @"Stopping..." : @"Stop action";
                btn_Stop.Enabled = !clicked;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            try
            {
                IsUserTerminated = true;
                btn_StopClicked(true);
                if (IsRunning)
                {
                    IsRunning = false;
                    //btn_Stop.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void TreeView_ContextMenuOpening(object sender, TreeViewContextMenuOpeningEventArgs e)
        {
            try
            {

                RadTreeNode selectedNode = TreeView.SelectedNode;
                if (selectedNode.Name.StartsWith("AD2") || selectedNode.Name.StartsWith("AD3"))
                {
                    if (!selectedNode.Name.Contains("."))
                        e.Cancel = true;
                    return;
                }

                if (selectedNode != null)
                {

                    List<string> temp = Lib.AllSectionsWithAttributes().ToStringList();
                    if (temp.Contains(selectedNode.Name))
                    {

                    }
                    else
                    {
                        e.Cancel = true;
                    }

                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }


        private void InitTreeView()
        {
            try
            {
                TreeView.AllowDefaultContextMenu = true;
                TreeView.AllowEdit = false;
                TreeView.AllowAdd = false;
                Dictionary<ContextOptions, string> Menus = new Dictionary<ContextOptions, string>();
                RadContextMenu menu = new RadContextMenu();

                string getData = (Properties.Settings.Default.GetBeforePreview) ? "Get data and " : "";

                Menus.Add(ContextOptions.Open, "Open");
                Menus.Add(ContextOptions.Import, "Get data from AIXM 5.1");
                Menus.Add(ContextOptions.Generate, "Generate AIP section");
                //if (!Properties.Settings.Default.GetBeforePreview) Menus.Add(ContextOptions.Import, "Get data from AIXM 5.1");
                if (Properties.Settings.Default.PreviewWOAmdt)
                {
                    Menus.Add(ContextOptions.None, $@"{getData}Preview Html without AMDT");
                    Menus.Add(ContextOptions.HtmlPreview, $@"{getData}Preview Html without AMDT");
                    Menus.Add(ContextOptions.PdfPreview, $@"{getData}Preview Pdf without AMDT");
                }
                Menus.Add(ContextOptions.HtmlAmdtPreview, $@"{getData}Preview Html");
                Menus.Add(ContextOptions.PdfAmdtPreview, $@"{getData}Preview Pdf");

                foreach (var item in Menus)
                {
                    if (item.Key == ContextOptions.None)
                    {
                        RadMenuSeparatorItem menu_item = new RadMenuSeparatorItem();
                        menu.Items.Add(menu_item);
                    }
                    else
                    {
                        RadMenuItem menu_item = new RadMenuItem(item.Value);
                        menu_item.Tag = item.Key;
                        menu.Items.Add(menu_item);
                        menu_item.Click += new EventHandler(Context_Click);
                    }
                }
                TreeView.RadContextMenu = menu;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        //private void InitTreeView()
        //{
        //    try
        //    {
        //        TreeView.AllowDefaultContextMenu = true;
        //        TreeView.AllowEdit = false;
        //        TreeView.AllowAdd = false;
        //        List<Tuple<ContextOptions, string>> Menus = new List<Tuple<ContextOptions, string>>();
        //        ContextMenu menu = new ContextMenu();
        //        //Dictionary<ContextOptions, string> Menus = new Dictionary<ContextOptions, string>();
        //        //RadContextMenu menu = new RadContextMenu();

        //        string getData = (Properties.Settings.Default.GetBeforePreview) ? "Get data and " : "";

        //        Menus.Add(Tuple.Create(ContextOptions.Open, "Open"));
        //        Menus.Add(Tuple.Create(ContextOptions.Import, "Get data from AIXM 5.1"));
        //        Menus.Add(Tuple.Create(ContextOptions.Generate, "Generate AIP section"));
        //        //if (!Properties.Settings.Default.GetBeforePreview) Menus.Add(ContextOptions.Import, "Get data from AIXM 5.1");
        //        if (Properties.Settings.Default.PreviewWOAmdt)
        //        {
        //            Menus.Add(Tuple.Create(ContextOptions.None, $@"{getData}Preview Html without AMDT"));
        //            Menus.Add(Tuple.Create(ContextOptions.HtmlPreview, $@"{getData}Preview Html without AMDT"));
        //            Menus.Add(Tuple.Create(ContextOptions.PdfPreview, $@"{getData}Preview Pdf without AMDT"));
        //        }
        //        Menus.Add(Tuple.Create(ContextOptions.HtmlAmdtPreview, $@"{getData}Preview Html"));
        //        Menus.Add(Tuple.Create(ContextOptions.PdfAmdtPreview, $@"{getData}Preview Pdf"));

        //        foreach (var item in Menus)
        //        {
        //            if (item.Item1 == ContextOptions.None)
        //            {
        //                menu.MenuItems.Add("-");
        //            }
        //            else
        //            {
        //                MenuItem menu_item = new MenuItem(item.Item2);
        //                menu_item.Tag = item.Item1;
        //                menu.MenuItems.Add(menu_item);
        //                menu_item.Click += new EventHandler(Context_Click);
        //            }
        //        }
        //        TreeView.ContextMenu = menu;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
        //    }
        //}
        private void InitSupGridView()
        {
            try
            {
                Dictionary<ContextOptions, string> Menus = new Dictionary<ContextOptions, string>();

                Menus.Add(ContextOptions.Open, "Open");
                Menus.Add(ContextOptions.HtmlPreview, @"Preview Html");
                Menus.Add(ContextOptions.PdfPreview, @"Preview Pdf");

                foreach (var item in Menus)
                {
                    if (item.Key == ContextOptions.None)
                    {
                        RadMenuSeparatorItem menu_item = new RadMenuSeparatorItem();
                        SupAicMenu.Items.Add(menu_item);
                    }
                    else
                    {
                        RadMenuItem menu_item = new RadMenuItem(item.Value);
                        menu_item.Tag = item.Key;
                        SupAicMenu.Items.Add(menu_item);
                        menu_item.Click += new EventHandler(SupAicContext_Click);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void InitListView()
        {
            try
            {
                List<Tuple<AISContextOptions, string>> Menus = new List<Tuple<AISContextOptions, string>>();
                ContextMenu menu = new ContextMenu();

                Menus.Add(Tuple.Create(AISContextOptions.New, "New"));
                Menus.Add(Tuple.Create(AISContextOptions.Open, "Open"));
                Menus.Add(Tuple.Create(AISContextOptions.Activate, "Activate"));
                Menus.Add(Tuple.Create(AISContextOptions.None, "-"));
                Menus.Add(Tuple.Create(AISContextOptions.Amdt, "Amendment"));
                Menus.Add(Tuple.Create(AISContextOptions.None, "-"));
                Menus.Add(Tuple.Create(AISContextOptions.OpenSourceFolder, "Open Source"));
                Menus.Add(Tuple.Create(AISContextOptions.OpenOutputFolder, "Open Output"));
                Menus.Add(Tuple.Create(AISContextOptions.OpenOutputIndex, "Open Index File"));

                foreach (var item in Menus)
                {
                    if (item.Item1 == AISContextOptions.None)
                    {
                        menu.MenuItems.Add("-");
                    }
                    else
                    {
                        MenuItem menu_item = new MenuItem(item.Item2);
                        menu_item.Tag = item.Item1;
                        menu.MenuItems.Add(menu_item);
                        menu_item.Click += new EventHandler(AISContext_Click);
                    }
                }
                RadListControl_AIS.ContextMenu = menu;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        // reflection reference from btn_Preview_Click
        public void PreviewXML(string section, bool isHtml, DB.TemporalityEntity entity = null)
        {
            try
            {
                GenerateXMLSection(section, entity);
                string outputFile = GenerateHtmlPdfSection(section, isHtml, entity);
                if (outputFile != "")
                {
                    if (File.Exists(outputFile))
                    {
                        AddOutput($@"Opening file {Path.GetFileName(outputFile)}", 0, null, Color.Green);
                        if (isHtml == true) PreviewHTML(outputFile, section.IsNull());
                        else PreviewPdf(outputFile);
                    }
                    else
                        AddOutput($@"File {Path.GetFileName(outputFile)} has not been created", 0, null, Color.Red);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }



        private void GenerateHtml(string Arguments)
        {
            try
            {
                AddOutput("Generating HTML file...");
                //GenerateFile(Arguments);
                RunCommandLine(Arguments);
                AddOutput("HTML file generating completed");
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void RunCommandLine(string Arguments, bool isPdf = false)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => CommandLine.Run(Arguments, isPdf));
                for (; ; )
                {
                    Application.DoEvents();
                    lock (CommandLine.OutputQueue)
                    {
                        while (CommandLine.OutputQueue.Count > 0)
                        {
                            BaseLib.Struct.Output msg = CommandLine.OutputQueue.Dequeue();
                            AddOutput(msg.Message, msg.Percent, null, msg.Color);
                        }
                        if (task.IsCompleted || task.IsFaulted) break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private string GeneratePdf(List<string> Arguments, string originalPath, string sectionClass = "")
        {
            try
            {
                string newPath = "";
                AddOutput("Generating PDF file...");

                foreach (string pdfArg in Arguments)
                {
                    if (UserTerminated()) return "";
                    RunCommandLine(pdfArg, true);
                    //Task task = Task.Factory.StartNew(() => CommandLine.Run(pdfArg, Lib.ExtMakeAIPFile, Lib.ExtMakeAIPWorkingDir));
                    //for (; ; )
                    //{
                    //    Application.DoEvents();
                    //    lock (CommandLine.OutputQueue)
                    //    {
                    //        while (CommandLine.OutputQueue.Count > 0)
                    //        {
                    //            Output msg = CommandLine.OutputQueue.Dequeue();
                    //            AddOutput(msg.Message, msg.Percent, null, msg.Color);
                    //        }
                    //        if (task.IsCompleted || task.IsFaulted) break;
                    //    }
                    //}
                }

                string oldFileName = Path.GetFileName(originalPath);
                string newFileName = oldFileName?
                    .Replace(Lib.CurrentAIS.lang, Lib.PdfLang(Lib.CurrentAIS.lang))
                    .Replace("-", "_")
                    .Replace(".", "_")
                    .Replace("_pdf", ".pdf");
                if (oldFileName != null)
                {
                    newPath = originalPath.Replace(oldFileName, newFileName);
                    if (new FileInfo(newPath).IsFileLocked())
                    {
                        AddOutput($@"PDF file {Path.GetFileName(newPath)} is already open and can`t be rewritten! Please close opened file and try again.", null, null, Color.Red);
                        return null;
                    }
                    if (File.Exists(newPath)) File.Delete(newPath);
                    if (File.Exists(originalPath)) File.Copy(originalPath, newPath);
                    AddOutput($@"PDF file moved to: {newPath}");
                }



                //List<PDFPage> listGen04 = db.PDFPage
                //    .AsNoTracking()
                //    .Where(x => x.eAIPID == Lib.CurrentAIP.id) // GEN 0.4 Only for current AIP
                //    .Include(x => x.eAIP)
                //    .ToList();
                //CultureInfo ci = new CultureInfo("en-US");
                //foreach (var item in listGen04)
                //{
                //    Console.WriteLine($@"{item.Section.ToName()} - {item.Page} ... {item.eAIP.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant()}");
                //}

                // Analizing PDF page number
                // 1. Detecting Section enum from sectionClass
                SectionName section = Lib.GetSectionName(sectionClass);
                int? airportHeliportId = null;
                int? prevAhHeliportId = null;
                string ahName = section.ToString().StartsWith("AD2") || section.ToString().StartsWith("AD3") ? sectionClass.GetAfterOrEmpty() : "";
                if (section.ToString().StartsWith("AD2") || section.ToString().StartsWith("AD3"))
                {
                    airportHeliportId = db.AirportHeliport
                        .AsNoTracking()
                        .FirstOrDefault(n => n.eAIPID == Lib.CurrentAIP.id &&
                                        n.LocationIndicatorICAO == ahName)?
                                        .id;
                }

                // 2. Is this class require PDF date changes
                if (Lib.SectionByAttribute(SectionParameter.PDFPages).Contains(section))
                {
                    // 3. Getting PreviousAIP Section Pages Info, 
                    if (Lib.PrevousAIP != null)
                    {
                        prevAhHeliportId = db.AirportHeliport
                            .AsNoTracking()
                            .FirstOrDefault(n => n.eAIPID == Lib.PrevousAIP.id && n.LocationIndicatorICAO == ahName)?.id;
                        List<PDFPage> prevPdfPages = db.PDFPage
                            .Include(x => x.eAIP.Amendment)
                            .Include(x => x.PageeAIP.Amendment)
                            .Where(x => x.eAIPID == Lib.PrevousAIP.id &&
                                        x.Section == section &&
                                        x.AirportHeliportID == prevAhHeliportId)
                            .AsNoTracking()
                            .ToList();
                        if (prevPdfPages.Any())
                        {
                            //    3.2 If received, 
                            //       3.2.1 Detecting not changed numbers in the PDF 
                            List<PDFPage> NewPageList = PDFManager.GetNewPagesList(db, prevPdfPages, originalPath, newPath, airportHeliportId);
                            //       3.2.2 Inserting dates from PreviousAIP
                            //       3.2.3 DB: Remove & Insert dates to CurrentAIP

                            if (NewPageList != null)
                            {
                                var pages = db.PDFPage?
                                    .Where(x => x.eAIPID == Lib.CurrentAIP.id &&
                                                x.Section == section &&
                                                x.AirportHeliportID == airportHeliportId).ToList();
                                if (pages.Any())
                                {
                                    //Console.WriteLine(pages.Count());
                                    db.PDFPage?.RemoveRange(pages);
                                    db.SaveChanges();
                                }
                                db.PDFPage.AddRange(NewPageList);
                                db.SaveChanges();
                            }
                            else
                            {
                                Console.WriteLine("pdf numbers error: " + section);
                            }
                        }
                        //    3.1 If no Pages Info, 
                        //       3.1.1 DB: Remove & Insert CurrentAIP dates for each page, exit
                        else InsertInitialPdfDates(section, newPath, airportHeliportId);
                    }
                    //    3.1 If No PreviousAIP, 
                    //       3.1.1 DB: Remove & Insert CurrentAIP dates for each page, exit
                    else InsertInitialPdfDates(section, newPath, airportHeliportId);


                }
                if (File.Exists(originalPath)) File.Delete(originalPath);
                AddOutput("PDF file generating completed");
                return newPath;

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return "";
            }
        }

        private void InsertInitialPdfDates(SectionName section, string newPath, int? airportHeliportId = null)
        {
            try
            {
                var pages = db.PDFPage?
                    .Where(x => x.eAIPID == Lib.CurrentAIP.id &&
                                x.Section == section &&
                                x.AirportHeliportID == airportHeliportId);
                if (pages != null)
                {
                    db.PDFPage?.RemoveRange(pages);
                    db.SaveChanges();
                }
                List<PDFPage> newList = new List<PDFPage>();
                newList = NewListPDFPages(section, newPath, airportHeliportId);

                if (newList.Any())
                {
                    db.PDFPage?.AddRange(newList);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private static List<PDFPage> NewListPDFPages(SectionName section, string newPath, int? airportHeliportId = null)
        {
            try
            {
                List<PDFPage> newList = new List<PDFPage>();
                int cnt = PDFManager.GetPdfPagesNumber(newPath);
                for (int i = 0; i < cnt; i++)
                {
                    newList.Add(new PDFPage()
                    {
                        eAIPID = Lib.CurrentAIP.id,
                        Section = section,
                        Page = i + 1,
                        AirportHeliportID = airportHeliportId,
                        PageeAIPID = Lib.CurrentAIP.id
                    });
                }
                return newList;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public delegate void PreviewHTMLDelegate(string path, bool IsPreviewExternal = false);
        private void PreviewHTML(string path, bool IsPreviewExternal = false)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new PreviewHTMLDelegate(PreviewHTML), path, IsPreviewExternal);
                }
                else
                {
                    if (File.Exists(path))
                    {
                        if (IsPreviewExternal || Properties.Settings.Default.IsPreviewExternal)
                        {
                            System.Diagnostics.Process.Start(path);
                            return;
                        }
                        var htmlTab = MainTabControl.Pages.FirstOrDefault(n => n.Name == "PV1_Preview");
                        if (htmlTab == null)
                        {
                            if (wb == null)
                            {
                                wb = new WebBrowser();
                            }
                            wb.Name = "PV1_wb";
                            wb.ScriptErrorsSuppressed = true;
                            wb.WebBrowserShortcutsEnabled = false;
                            wb.Dock = DockStyle.Fill;
                            wb.Navigate(path);
                            wb.DocumentCompleted += wb_DocumentCompleted; // Inject js in the this method

                            // ToDo: remove awesomium due license problems
                            //if (wb == null)
                            //{
                            //    wb = new Awesomium.Windows.Forms.WebControl();
                            //}
                            //wb.Dock = DockStyle.Fill;
                            //wb.Source = new Uri(path);
                            //wb.Update();

                            RadPageViewPage rpvp = new RadPageViewPage();
                            rpvp.Dock = DockStyle.Fill;
                            rpvp.Controls.Add(wb);
                            rpvp.Text = "Preview: " + PV1.Text;
                            rpvp.Name = "PV1_Preview";
                            MainTabControl.Pages.Add(rpvp);
                            MainTabControl.SelectedPage = rpvp;
                        }
                        else
                        {
                            //WebBrowser wb = (WebBrowser)htmlTab.Controls.Find("PV1_wb", true).FirstOrDefault();
                            //if (wb != null)
                            //{
                            //wb.Source = new Uri(path);
                            //wb.Update();
                            wb.Navigate(path);
                            MainTabControl.SelectedPage = htmlTab;
                            //}

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (wb == null) return;

                HtmlElement body = wb.Document?.GetElementsByTagName("body")[0];
                body.Click += new HtmlElementEventHandler(thisBody_Click);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        void thisBody_Click(object sender, HtmlElementEventArgs e)
        {
            try
            {
                if (!clicked)
                {
                    clicked = true;
                    HtmlElementCollection body = wb.Document?.GetElementsByTagName("body");
                    if (body != null)
                    {
                        int i = body[0].Id.ToInt();

                        if (i == 1)
                        {
                            string data = "";
                            var tmp = body[0].GetAttribute("data-feature").Split(',');
                            if (tmp.Length == 6)
                            {
                                data = $@"Storage name: {tmp[0]}{Environment.NewLine}";
                                data += $@"Feature type: {tmp[1]}{Environment.NewLine}";
                                data += $@"Identifier: {tmp[2]}{Environment.NewLine}";
                                data += $@"Sequence: {tmp[3]}{Environment.NewLine}";
                                data += $@"Correction: {tmp[4]}{Environment.NewLine}";
                                data += $@"Field: {tmp[5]}{Environment.NewLine}";
                                DialogResult dialogResult = MessageBox.Show(
                                    $@"{data}{Environment.NewLine}{Environment.NewLine}Do you want to go to the TOSSM?",
                                    @"Question", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    string feature = body[0].GetAttribute("data-feature");
                                    if (Directory.Exists(Lib.CurrentDirFunc()))
                                    {
                                        string eaip_url = $@"eaip://OpenDocument/storage={tmp[0]}&featureType={tmp[1]}&id={tmp[2]}&sequence={tmp[3]}&correction={tmp[4]}&field={tmp[5]}";
                                        Process TOSSM = new Process();
                                        TOSSM.StartInfo.FileName = Path.Combine(Lib.CurrentDirFunc(), "TOSSM.exe");
                                        TOSSM.StartInfo.Arguments = eaip_url;
                                        TOSSM.Start();
                                        //Process.Start(Path.Combine(Lib.CurrentDirFunc(),"TOSSM.exe"));
                                    }

                                }
                                else if (dialogResult == DialogResult.No)
                                {

                                }
                                body[0].SetAttribute("id", "0");
                            }
                        }
                        else if (i == 2)
                        {
                            ErrorLog.ShowWarning(@"Information about AIXM feature not available for this section.");
                            body[0].SetAttribute("id", "0");
                        }
                    }

                    clicked = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        private void btn_GetChanges_Click(object sender, EventArgs e)
        {
            try
            {
                rgv_TempChanges.DataSource = null;
                rgv_TempChanges.GroupDescriptors.Clear();
                List<Feature> featList = new List<Feature>();
                btn_GetChanges.Enabled = false;

                AddOutput($@"Beginning to find TempDelta changes from {Lib.CurrentAIP.Effectivedate:yyyy-MM-dd}");

                foreach (FeatureType ftype in Enum.GetValues(typeof(FeatureType)))
                {
                    List<Feature> lst = Globals.GetTempFeaturesByED(ftype);

                    if (lst.Any())
                    {
                        featList.AddRange(lst);
                        AddOutput($@"{ftype.ToString()}: {lst.Count} changes");
                    }
                }

                if (featList.Any())
                {
                    AddOutput($@"Total {featList.Count} changes found. Showing differencies in the Data Grid.");
                    BindingList<SUPEntry> supList = new BindingList<SUPEntry>();
                    foreach (var feat in featList)
                    {
                        Feature permFeat = Globals.GetFeaturesByED(feat.FeatureType, feat.Identifier).FirstOrDefault();
                        var delta = SUP.GetStateDelta(permFeat, feat);

                        foreach (var var in delta)
                        {
                            supList.Add(new SUPEntry(feat.Identifier, feat.FeatureType.ToString(), var.Key, var.Value.Item1, var.Value.Item2, feat.TimeSlice.ValidTime.BeginPosition.ToString("yyyy-MM-dd"), feat.TimeSlice.ValidTime.EndPosition?.ToString("yyyy-MM-dd")));
                        }
                    }

                    GroupDescriptor groupFeature = new GroupDescriptor();
                    groupFeature.GroupNames.Add("FeatureType", ListSortDirection.Ascending);
                    GroupDescriptor groupIdentifier = new GroupDescriptor();
                    groupIdentifier.GroupNames.Add("Identifier", ListSortDirection.Ascending);
                    rgv_TempChanges.GroupDescriptors.Add(groupFeature);
                    rgv_TempChanges.GroupDescriptors.Add(groupIdentifier);

                    rgv_TempChanges.BestFitColumns();
                    rgv_TempChanges.AutoGenerateColumns = true;
                    rgv_TempChanges.DataSource = supList;
                    rgv_TempChanges.BestFitColumns();
                }
                else
                {
                    AddOutput($@"No any TempDelta changes found");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                btn_GetChanges.Enabled = true;
            }
        }


        private async void btn_SupNew_Click(object sender, EventArgs e)
        {
            try
            {
                await OpenSupplement();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private async Task OpenSupplement(DB.Supplement supplement = null)
        {
            try
            {
                int langId = Lib.GetLangIdByValue(Properties.Settings.Default.eAIPLanguage) ?? 0;
                if (langId == 0)
                {
                    ErrorLog.ShowWarning("Language not selected. Please select language.");
                    return;
                }

                SuplForm sf = supplement != null ? new SuplForm(supplement) : new SuplForm();
                sf.db = db;
                sf.ShowDialog();
                if (sf.DialogResult == DialogResult.OK)
                {
                    db.Database.Log = Console.Write;
                    // New Supplement
                    if (sf.formSource.id == 0)
                    {
                        sf.formSource.Identifier = Guid.NewGuid();
                        sf.formSource.LanguageReference = null;
                        sf.formSource.LanguageReferenceId = langId;
                    }
                    else
                    {
                        // Require to compute next version
                        // Current version number may be not last, if last one is Canceled
                        int? ver = db.Supplement?
                            .AsNoTracking()
                            .Where(x => x.Identifier == sf.formSource.Identifier)?
                            .OrderByDescending(x => x.Version)
                            .FirstOrDefault()
                            ?.Version;
                        sf.formSource.Version = ver + 1 ?? 1;
                    }

                    sf.formSource.Created = Lib.GetServerDate() ?? DateTime.UtcNow;
                    sf.formSource.UserId = Globals.CurrentUser.id;
                    db.Entry(sf.formSource).State = EntityState.Added;
                    db.Supplement.Add(sf.formSource);
                    await db.SaveChangesAsync();
                }
                refreshSupList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            refreshSupList();
        }

        private void refreshSupList()
        {
            try
            {
                if (Lib.CurrentAIS.IsNull())
                {
                    rgv_SupChanges.DataSource = null;
                    rgv_SupChanges.Refresh();
                    return;
                }
                int langId = Lib.GetLangIdByValue(Lib.CurrentAIP.lang) ?? 0;
                var query = db.Supplement
                    .Where(x => x.IsCanceled == false && x.LanguageReferenceId == langId)?
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .ToList();

                Dictionary<string, GridViewOption> fileProp = Lib.GridViewAttributes<DB.Supplement>();
                BindingSource bi = new BindingSource();
                bi.DataSource = query;
                rgv_SupChanges.DataSource = query;
                rgv_SupChanges.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //rgv_SupChanges.BestFitColumns();
                foreach (GridViewDataColumn col in rgv_SupChanges.Columns)
                {
                    if (fileProp.ContainsKey(col.Name))
                    {
                        col.IsVisible = fileProp[col.Name].Visible;
                        col.ReadOnly = fileProp[col.Name].ReadOnly;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                rgv_SupChanges.Refresh();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private async void rgv_SupAic_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (e.Row?.DataBoundItem == null) return;
                if (e.Row.DataBoundItem is DB.Supplement)
                {
                    await OpenSupplement((DB.Supplement)e.Row.DataBoundItem);
                }
                else if (e.Row.DataBoundItem is DB.Circular)
                {
                    await OpenCircular((DB.Circular)e.Row.DataBoundItem);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_GenSup_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Lib.IsAIPSelected()) return;
                int langId = Lib.GetLangIdByValue(Lib.CurrentAIP.lang) ?? 0;
                List<DB.Supplement> supplements = db.Supplement
                    .AsNoTracking()
                    .Where(x => x.IsCanceled != true
                                && x.LanguageReferenceId == langId
                                && (Lib.CurrentAIP.Effectivedate >= x.EffectivedateFrom && (x.EffectivedateTo == null || Lib.CurrentAIP.Effectivedate <= x.EffectivedateTo))
                    )
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .ToList();

                Guid oaGuid = db.GetDBConfiguration<Guid>(Cfg.OrganizationAuthorityIdentifier);
                string contactName = db.GetDBConfiguration<string>(Cfg.ContactName);

                OrganisationAuthority oa = Globals
                    .GetFeaturesByED(FeatureType.OrganisationAuthority)
                    .OfType<OrganisationAuthority>()
                    .FirstOrDefault(x => x.Identifier == db.GetDBConfiguration<Guid>(Cfg.OrganizationAuthorityIdentifier));
                ContactInformation contact = oa?.Contact
                    .Where(y => y.Name == contactName)
                    .ToList()
                    .FirstOrDefault();
                if (contact != null)
                {
                    foreach (DB.Supplement sup in supplements)
                    {
                        eSUP xmlSup = new eSUP();
                        xmlSup.ICAOcountrycode = Lib.CurrentAIP.ICAOcountrycode;
                        xmlSup.State = Lib.CurrentAIP.State;
                        xmlSup.Effectivedate = sup.EffectivedateFrom.ToString("yyyy-MM-dd");
                        xmlSup.Publishingorganisation = Lib.CurrentAIP.Publishingorganisation;
                        xmlSup.Cancel = sup.EffectivedateTo?.ToString("yyyy-MM-dd");
                        xmlSup.Publicationdate = sup.Publicationdate.ToString("yyyy-MM-dd");
                        xmlSup.Number = sup.Number;
                        xmlSup.Year = sup.Year;
                        xmlSup.lang = Lib.CurrentAIP.lang;
                        xmlSup.Version = sup.Version.ToString();
                        xmlSup.Type = Lib.IsAIRAC(sup.EffectivedateFrom) ? eSUPType.AIRAC : eSUPType.NonAIRAC;
                        xmlSup.@class = "Body";

                        int cnt = 0;
                        xmlSup.Address = new Address[]
                        {
                        new Address()
                        {
                            Addresspart = new Addresspart[]
                            {
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Phone,
                                    Items = new object[]{ contact.PhoneFax.FirstOrDefault()?.Voice }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Fax, @class = "Body",
                                    Items = new object[]{ contact.PhoneFax.FirstOrDefault()?.Facsimile }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Email, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x=>x.Network == CodeTelecomNetwork.INTERNET)?.eMail }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.AFS, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x => x.Network == CodeTelecomNetwork.AFTN)?.Linkage }},
                                new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.URL, @class = "Body",
                                    Items = new object[]{ contact.NetworkNode.FirstOrDefault(x => x.Network == CodeTelecomNetwork.INTERNET)?.Linkage }}
                            }
                        },
                        new Address()
                            {
                                Addresspart = new Addresspart[]
                                {
                                    new Addresspart(){ id = $@"eSUP{sup.id}{cnt++}", Type = AddresspartType.Post,
                                        Items = new object[]
                                        {
                                            oa.Name, new br(),
                                            contact.Name, new br(),
                                            contact.Address.FirstOrDefault()?.DeliveryPoint + ", " +
                                            contact.Address.FirstOrDefault()?.City,
                                            contact.Address.FirstOrDefault()?.AdministrativeArea + ", " +
                                            contact.Address.FirstOrDefault()?.PostalCode + ", " +
                                            contact.Address.FirstOrDefault()?.Country
                                        }}
                                }
                            }
                        };

                        //xmlSup.Title = new Title() { Items = new object[] { sup.Title } };
                        xmlSup.SUPsection = new SUPsection[]
                        {
                            new SUPsection()
                            {
                                Title = new Title(){Items = new object[]{sup.Title}},
                                Items = new object[] { new div() { Items = new object[] {
                                    Lib.ConvertToAIPXhtml(sup.Description, Lib.CurrentAIP)
                                    }
                                    }
                                }

                            }
                        };

                        string fileName = $@"{Lib.CurrentAIP.ICAOcountrycode.ToUpperInvariant()}-eSUP-{sup.Year}-{sup.Number}-{Lib.CurrentAIP.lang}.xml";
                        string path = Path.Combine(Lib.CurrentDir, "Data", "eAIP-source", "eSUP");
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        AIPXMLWrite(xmlSup, Path.Combine(path, fileName));
                    }
                    // Get all actual SUPs
                    // GenerateXMLs
                    // GeneratePackage
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void SupAic_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            SupAicMenu.DropDown.Tag = sender is RadGridView && ((RadGridView)sender).Name.Contains("Sup") ? SupAicStates.SupClicked : SupAicStates.AicClicked;
            e.ContextMenu = SupAicMenu.DropDown;
        }

        private void btnUpdateAIC_Click(object sender, EventArgs e)
        {
            updateCircular();
        }

        private void updateCircular()
        {
            try
            {
                if (Lib.CurrentAIS.IsNull())
                {
                    rgv_AIC.DataSource = null;
                    rgv_AIC.Refresh();
                    return;
                }
                int langId = Lib.GetLangIdByValue(Lib.CurrentAIP.lang) ?? 0;
                var query = db.Circular
                        .Where(x => x.IsCanceled == false && x.LanguageReferenceId == langId)?
                        .GroupBy(x => x.Identifier)
                        .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                        .ToList();

                Dictionary<string, GridViewOption> fileProp = Lib.GridViewAttributes<DB.Circular>();
                BindingSource bi = new BindingSource();
                bi.DataSource = query;
                rgv_AIC.DataSource = query;
                rgv_AIC.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //rgv_AIC.BestFitColumns();
                foreach (GridViewDataColumn col in rgv_AIC.Columns)
                {
                    if (fileProp.ContainsKey(col.Name))
                    {
                        col.IsVisible = fileProp[col.Name].Visible;
                        col.ReadOnly = fileProp[col.Name].ReadOnly;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                rgv_AIC.Refresh();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        private async void btnNewAIC_Click(object sender, EventArgs e)
        {
            try
            {
                await OpenCircular();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private async Task OpenCircular(DB.Circular circular = null)
        {
            try
            {
                int langId = Lib.GetLangIdByValue(Properties.Settings.Default.eAIPLanguage) ?? 0;
                if (langId == 0)
                {
                    ErrorLog.ShowWarning("Language not selected. Please select language.");
                    return;
                }

                AICForm sf = circular != null ? new AICForm(circular) : new AICForm();
                sf.db = db;
                sf.ShowDialog();
                if (sf.DialogResult == DialogResult.OK)
                {
                    //db.Database.Log = Console.Write;
                    // New circular
                    if (sf.formSource.id == 0)
                    {
                        sf.formSource.Identifier = Guid.NewGuid();
                        sf.formSource.LanguageReference = null;
                        sf.formSource.LanguageReferenceId = langId;
                    }
                    else
                    {
                        // Require to compute next version
                        // Current version number may be not last, if last one is Canceled
                        int? ver = db.Circular?
                            .AsNoTracking()
                            .Where(x => x.Identifier == sf.formSource.Identifier)?
                            .OrderByDescending(x => x.Version)
                            .FirstOrDefault()
                            ?.Version;
                        sf.formSource.Version = ver + 1 ?? 1;
                    }

                    sf.formSource.Created = Lib.GetServerDate() ?? DateTime.UtcNow;
                    sf.formSource.UserId = Globals.CurrentUser.id;
                    db.Entry(sf.formSource).State = EntityState.Added;
                    db.Circular.Add(sf.formSource);
                    await db.SaveChangesAsync();
                    updateCircular();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void PreviewPdf(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void rgv_Pages_ContextMenu()
        {
            try
            {
                    ContextMenu menu = new ContextMenu();

                    MenuItem menu_item = new MenuItem("Open");
                    menu_item.Tag = ContextMenuOfOtherPages.Open;
                    menu.MenuItems.Add(menu_item);
                    menu_item.Click += new EventHandler(CoverPagePdf_action);

                    MenuItem menu_item2 = new MenuItem("Reset to default template");
                    menu_item2.Tag = ContextMenuOfOtherPages.Reset;
                    menu.MenuItems.Add(menu_item2);
                    menu_item2.Click += new EventHandler(CoverPagePdf_action);

                    rgv_Pages.ContextMenu = menu;
                
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            
        }

        private void CoverPagePdf_action(object sender, EventArgs e)
        {
            try
            {
                
                    GridViewRowInfo currentRow = rgv_Pages.CurrentRow;
                    if (currentRow.DataBoundItem is Page)
                    {
                        var richTextForm = new RichTextForm();
                        switch ((ContextMenuOfOtherPages)((MenuItem)sender).Tag)
                        {
                            case ContextMenuOfOtherPages.Open:

                                richTextForm = new RichTextForm(((Page)currentRow.DataBoundItem).pageType, ((Page)currentRow.DataBoundItem).docType, db, false);

                                richTextForm.ShowDialog();
                                UpdatePageList();
                            break;
                            case ContextMenuOfOtherPages.Reset:
                                if (!Permissions.Is_Admin())
                                {
                                    ErrorLog.ShowInfo("You must to have Admin privilegies to make this action!");
                                    return;
                                }
                            var confirmResult = MessageBox.Show(@"Are you sure you want to reset changes to default values? All changes will be lost!",
                                    @"Reset Data",
                                    MessageBoxButtons.YesNo);
                                if (confirmResult == DialogResult.Yes)
                                {

                                    richTextForm = new RichTextForm(((Page)currentRow.DataBoundItem).pageType, ((Page)currentRow.DataBoundItem).docType, db, true);
                                    //richTextForm.ShowDialog();
                                    UpdatePageList();
                                ErrorLog.ShowInfo("Selected page has been successfully reset to default values!");
                                }
                                
                                break;
                            default:
                                break;
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void rgv_Pages_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                GridViewRowInfo currentRow = rgv_Pages.CurrentRow;
                if (currentRow.DataBoundItem is Page)
                {
                    var richTextForm = new RichTextForm(((Page)currentRow.DataBoundItem).pageType, ((Page)currentRow.DataBoundItem).docType, db , false);
                    richTextForm.ShowDialog();
                    UpdatePageList();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Activate_AIP_Click(object sender, EventArgs e)
        {
            try
            {
                ActivateAIS();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void ActivateAIS()
        {
            try
            {
                RadListDataItem currentRow = RadListControl_AIS.SelectedItem;
                if (currentRow?.DataBoundItem is DB.eAISpackage)
                {
                    if ((int)((DB.eAISpackage)currentRow?.DataBoundItem).Status < (int)Status.Work)
                    {
                        ErrorLog.ShowMessage("Only eAISpackage in Work status and higher can be selected as active");
                        return;
                    }
                    else
                    {
                        if (((DB.eAISpackage)currentRow?.DataBoundItem) != null)
                        {
                            eAIS_Activated(((DB.eAISpackage)currentRow?.DataBoundItem), true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        // end of main
        private void log_copy_Click(object sender, EventArgs e)
        {
            try
            {
                log_output.Copy();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private void UpdatePageList()
        {
            try
            {
                if (Lib.CurrentAIS.IsNull())
                {
                    rgv_Pages.DataSource = null;
                    rgv_Pages.Refresh();
                    return;
                }
                var dbPages = db.AIPPage
                    .Where(x => x.eAIPID == Lib.CurrentAIP.id)
                    .ToList();
                List<Page> pages = new List<Page>();
                var pageTypes = Enum.GetValues(typeof(PageType)).Cast<PageType>().Skip(1).ToList();
                var docTypes = Enum.GetValues(typeof(DocType)).Cast<DocType>().Skip(1).ToList();
                foreach (PageType pageType in pageTypes)
                {
                    foreach (DocType docType in docTypes)
                    {
                        var currentPage = dbPages.FirstOrDefault(x => x.PageType == pageType && x.DocType == docType);
                        Page page = new Page();
                        page.isCreated = dbPages.Any(x => x.PageType == pageType && x.DocType == docType);
                        page.docType = docType;
                        page.pageType = pageType;
                        page.CreatedUser = currentPage?.CreatedUser?.Name ?? "";
                        page.CreatedDate = currentPage?.CreatedDate;
                        page.ChangedUser = currentPage?.ChangedUser?.Name ?? "";
                        page.ChangedDate = currentPage?.ChangedDate;
                        pages.Add(page);
                    }
                }
                rgv_Pages_ContextMenu();

                rgv_Pages.DataSource = pages;
                rgv_Pages.BestFitColumns();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }



    internal static class State
    {
        internal static RGVStates LastClick { get; set; }
        internal static int? ParentID { get; set; }
        internal static string ParentClassName { get; set; }
        internal static AIPSection CurrentSection { get; set; }
        internal static SubClass CurrentSubClass { get; set; }

        static State()
        {
            LastClick = RGVStates.None;
        }
    }

    enum RGVStates
    {
        None,
        TreeViewClicked, // if clicked on treemenu
        GridViewClicked, // if clicked on entry
        GridViewSelectorClicked // If selecting between types of data
    }

    enum SupAicStates
    {
        None,
        SupClicked,
        AicClicked
    }

    enum ContextOptions
    {
        None,
        Open,
        Import,
        HtmlPreview,
        HtmlAmdtPreview,
        PdfPreview,
        PdfAmdtPreview,
        Generate
    }

    enum AISContextOptions
    {
        None,
        New,
        Open,
        Activate,
        Amdt,
        OpenSourceFolder,
        OpenOutputFolder,
        OpenOutputIndex,
    }

    enum ContextMenuOfOtherPages
    {
        Open,
        Reset,
    }

    public class ValueType<T>
    {
        T item;
        public ValueType() { }
        public ValueType(T item)
        {
            this.item = item;
        }

        [DisplayName("Select a type of subitem")]
        public T ItemProperty
        {
            get { return item; }
            set { item = value; }
        }
    }


}
