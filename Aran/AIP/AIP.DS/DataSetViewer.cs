using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIP.DataSet.Classes;
using AIP.DataSet.Lib;
using AIP.DataSet.Properties;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.CommonUtil.Util;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace AIP.DataSet
{
    public partial class DataSetViewer : Telerik.WinControls.UI.RadForm
    {
        private string currentFeature;
        private string previousFeature;
        private BackgroundWorker bw = new BackgroundWorker();

        private enum HashStatus
        {
            None,
            Unchecked,
            Incorrect,
            Correct
        }

        public DataSetViewer()
        {
            InitializeComponent();
        }

        private void DataSetManager_Load(object sender, EventArgs e)
        {
            try
            {
                pnl_Menu.PanelElement.PanelBorder.Visibility = ElementVisibility.Collapsed;
                bw.DoWork += Bw_DoWork;
                bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                EnableControls(false);
                InitializeForm();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void InitializeForm()
        {
            var task = Task.Run(() => PrepareForm());
            task.ContinueWith(
                o =>
                {
                    PrepareControls();
                    CheckHash();
                    EnableControls(true);
                }
            );
        }

        delegate void PrepareFormDelegate();

        private void PrepareForm()
        {
            try
            {
                Cache.Clear();
                XmlFileConnection.Init();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        delegate void PrepareControlsDelegate();

        private void PrepareControls()
        {
            if (InvokeRequired)
            {
                Invoke(new PrepareControlsDelegate(PrepareControls), null);
            }
            else
            {
                try
                {
                    Text += @" - " + XmlFileConnection.fileName;
                    rgv_DataSet.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                    rgv_DataSet.BestFitColumns();
                    cbx_DataSet.ValueMember = "Key";
                    cbx_DataSet.DisplayMember = "Value";
                    cbx_DataSet.DataSource = new BindingSource(LoadDataSetFeatures(), null);
                }
                catch (Exception ex)
                {
                    ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                }
            }
        }

        delegate void CheckHashDelegate();

        private void CheckHash()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new CheckHashDelegate(CheckHash), null);
                }
                else
                {
                    if (File.Exists(XmlFileConnection.fileName + ".md5"))
                    {
                        string hash = Common.GetMD5HashFromFile(XmlFileConnection.fileName);
                        var oldHashFile = File.ReadAllText($@"{XmlFileConnection.fileName}.md5");
                        var arr = oldHashFile.Split(new string[] { " *" }, StringSplitOptions.None);
                        if (arr?[1] != null && arr?[0] != null
                            && arr[0] == hash
                            && arr[1] == Path.GetFileName(XmlFileConnection.fileName))
                        {
                            SetHash(HashStatus.Correct);
                            return;
                        }
                        SetHash(HashStatus.Incorrect);
                    }
                    else
                        SetHash(HashStatus.Unchecked);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        delegate void SetHashDelegate(HashStatus hashStatus);

        private void SetHash(HashStatus hashStatus)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new SetHashDelegate(SetHash), new object[] { hashStatus });
                }
                else
                {
                    switch (hashStatus)
                    {
                        case HashStatus.None:
                            lbl_Hash.Text = @"Unknown issue";
                            break;
                        case HashStatus.Unchecked:
                            lbl_Hash.Text = @"The hash value is not available";
                            lbl_Hash.ForeColor = Color.Chocolate;
                            break;
                        case HashStatus.Incorrect:
                            lbl_Hash.Text = @"The hash value is not correct";
                            lbl_Hash.ForeColor = Color.Red;
                            break;
                        case HashStatus.Correct:
                            lbl_Hash.Text = @"The hash value is correct";
                            lbl_Hash.ForeColor = Color.Green;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        delegate void LoadFeaturesCallback(string feature);

        private void LoadFeatures(string feature)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new LoadFeaturesCallback(LoadFeatures), new object[] { feature });
                }
                else
                {
                    Type AIPType = Type.GetType("Aran.Aim.Features." + feature + ",Aran.Aim");
                    MethodInfo method = typeof(Common).GetMethod($@"LoadDataTableFeatures");
                    //MethodInfo method = typeof(Common).GetMethod($@"LoadFeatures");
                    MethodInfo BuildSectionMethod = method?.MakeGenericMethod(AIPType);
                    UpdateDataGridTemplate(feature);
                    rgv_DataSet.DataSource = BuildSectionMethod?.Invoke(null, null);
                    lbl_Count.Text = rgv_DataSet.MasterView.Rows.Count.ToString();
                    rgv_DataSet.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private Dictionary<string, string> LoadDataSetFeatures()
        {
            try
            {
                Dictionary<string, string> tmp = new Dictionary<string, string>() { { "", "" } };
                Dictionary<string, string> tmp2 = XmlFileConnection
                    .FeaturesCount
                    .ToDictionary(x => x.Key, x => x.Key + " (" + x.Value + ")");
                return tmp.Union(tmp2).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private void cbx_DataSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //await Task.Run(() => EnableControls(false));
                UpdateGridView();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void UpdateGridView()
        {
            try
            {
                if (cbx_DataSet.SelectedIndex != 0)
                {
                    var key = ((KeyValuePair<string, string>)cbx_DataSet.SelectedItem).Key;
                    EnableControls(false);
                    bw.RunWorkerAsync(key);
                    //await Task.Run(() => UpdateDataAsync(key));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {

            string param = e.Argument as string;
            UpdateData(param);
            //e.Result = null; //Set your Result of the long running task
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableControls(true);
        }

        delegate void EnableControlsCallBack(bool isEnable);

        private void EnableControls(bool isEnable)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new EnableControlsCallBack(EnableControls), new object[] { isEnable });
                }
                else
                {
                    cbx_DataSet.Enabled = isEnable;
                    rgv_DataSet.Visible = isEnable;
                    wb_bar.Visible = !isEnable;
                    if (!isEnable)
                    {
                        wb_bar.StartWaiting();
                    }
                    else
                    {
                        wb_bar.StopWaiting();
                    }
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void UpdateData(string selectedFeature)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedFeature)) LoadFeatures(selectedFeature);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private async Task UpdateDataAsync(string selectedFeature)
        {
            await Task.Run(() => UpdateData(selectedFeature));
        }

        delegate void SaveDataGridTemplateDelegate(string featureName);

        private void SaveDataGridTemplate(string featureName)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new SaveDataGridTemplateDelegate(SaveDataGridTemplate), new object[] { featureName });
                }
                else
                {
                    string d = Path.Combine(AIP.AssemblyDirectory(), "Templates", "AIPDataSet");
                    string s = Path.Combine(d, $@"{featureName}.xml");
                    if (!Directory.Exists(d)) Directory.CreateDirectory(d);
                    rgv_DataSet.SaveLayout(s);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        delegate void UpdateDataGridTemplateDelegate(string featureName);

        private void UpdateDataGridTemplate(string featureName)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new UpdateDataGridTemplateDelegate(UpdateDataGridTemplate), new object[] { featureName });
                }
                else
                {
                    previousFeature = currentFeature;
                    currentFeature = featureName;

                    if (!string.IsNullOrEmpty(previousFeature)) SaveDataGridTemplate(previousFeature);

                    string s = Path.Combine(AIP.AssemblyDirectory(), "Templates", "AIPDataSet", $@"{featureName}.xml");
                    if (File.Exists(s)) rgv_DataSet.LoadLayout(s);
                    else
                    {
                        //rgv_DataSet.XmlSerializationInfo.DisregardOriginalSerializationVisibility = true;
                        //rgv_DataSet.XmlSerializationInfo.SerializationMetadata.Clear();
                        rgv_DataSet.DataSource = null;
                        rgv_DataSet.Rows.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void DataSetViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFeature)) SaveDataGridTemplate(currentFeature);
        }

        private void rgv_DataSet_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {
                GridViewRowInfo currentRow = rgv_DataSet.CurrentRow;
                GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;
                /*if (current?.DataBoundItem is Feature)*/
                if ((current?.DataBoundItem as DataRowView)?.Row["Identifier"] != null)
                {
                    RadMenuItem TossmRedirect = new RadMenuItem();
                    TossmRedirect.Text = @"Open selected in the TOSSM";
                    TossmRedirect.Click += (o, args) =>
                    {
                        OpenCurrentRow();
                    };
                    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
                    e.ContextMenu.Items.Add(separator);
                    e.ContextMenu.Items.Add(TossmRedirect);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void OpenCurrentRow()
        {
            try
            {
                GridViewRowInfo currentRow = rgv_DataSet.CurrentRow;
                GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;
                // if (current?.DataBoundItem is Feature)
                if ((current?.DataBoundItem as DataRowView)?.Row["Identifier"] != null)
                {
                    //string id = ((Feature) current?.DataBoundItem).Identifier.ToString();
                    //string featureType = ((Feature) current?.DataBoundItem).FeatureType.ToString();
                    string id = (current?.DataBoundItem as DataRowView)?.Row["Identifier"]?.ToString();
                    string featureType = (current?.DataBoundItem as DataRowView)?.Row["FeatureType"]?.ToString();
                    if (Directory.Exists(Common.CurrentDirFunc()))
                    {
                        string eaip_url = $@"eaip://OpenDocument/id={id}&featureType={featureType}";
                        Process TOSSM = new Process();
                        TOSSM.StartInfo.FileName = Path.Combine(Common.CurrentDirFunc(), "TOSSM.exe");
                        TOSSM.StartInfo.Arguments = eaip_url;
                        TOSSM.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void rgv_DataSet_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            //GridViewRowInfo currentRow = rgv_DataSet.CurrentRow;
            //GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;
            //if (current?.DataBoundItem is Feature)
            //{
            //    DataSetFeatureViewer featureViewer = new DataSetFeatureViewer();
            //    featureViewer.feature = (Feature)current?.DataBoundItem;
            //    featureViewer.ShowDialog();
            //}
            //GridViewRowInfo currentRow = rgv_DataSet.CurrentRow;
            //GridViewDataRowInfo current = currentRow as GridViewDataRowInfo;
            //if (current?.DataBoundItem is Feature)
            //{
            //    DataSetTreeViewer featureViewer = new DataSetTreeViewer();
            //    featureViewer.feature = (Feature)current?.DataBoundItem;
            //    featureViewer.ShowDialog();
            //}
        }


        private void btn_HideComplex_Click(object sender, EventArgs e)
        {
            try
            {
                var _list = rgv_DataSet.Columns.ToDictionary(x => x.Name, x => x.DataType);
                foreach (GridViewDataColumn col in rgv_DataSet.Columns)
                {
                    if (col.DataType.ToString().Contains("List"))
                    {
                        col.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private void rgv_DataSet_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
        {


        }
    }


}

