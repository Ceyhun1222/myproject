using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AranUpdateManager.Data;
using System.IO;
using AranUpdManager;

namespace AranUpdateManager
{
    public partial class VersionsPageControl : UserControl, IPage
    {
        private List<AranVersion> _versionList;
        private string _statusText;
        private Comparison<AranVersion> _sorter;
        private int _sortedColumn;
        private SortOrder _sortedOrder;


        public VersionsPageControl()
        {
            InitializeComponent();

            _versionList = new List<AranVersion>();

            #region DGV Sorter

            _sorter = ((v1, v2) =>
            {
                int res = 0;

                switch (_sortedColumn)
                {
                    case 0:
                        res = string.Compare(v1.Key, v2.Key);
                        break;
                    case 1:
                        res = DateTime.Compare(v1.ReleasedDate, v2.ReleasedDate);
                        break;
                    case 2:
                        res = Convert.ToInt32(v1.IsDefault) - Convert.ToInt32(v2.IsDefault);
                        break;
                }

                if (_sortedOrder == SortOrder.Descending)
                    res = -1 * res;

                return res;
            });

            #endregion

            ui_versionUserGrDGV.AutoGenerateColumns = false;
        }

        public event EventHandler StatusTextChanged;

        public void OpenPage()
        {
            _versionList.Clear();

            var list = Global.DbPro.GetVersions();
            _versionList.AddRange(list);

            RefreshGrid();
        }

        public void ClosePage()
        {
            _versionList.Clear();
            RefreshGrid();
        }

        public string StatusText
        {
            get { return _statusText; }
        }

    
        private void RefreshGrid()
        {
            ui_versionDGV.RowCount = _versionList.Count;
            ui_versionDGV.Refresh();

            SetStatusText();
        }

        private void VersionDGV_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _versionList.Count)
                return;

            var item = _versionList[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = item.Key;
                    break;
                case 1:
                    e.Value = item.ReleasedDate.ToString("yyyy-MM-dd");
                    break;
                case 2:
                    e.Value = item.IsDefault ? "Actual" : "";
                    break;
            }
        }

        private void VersionDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _versionList.Count)
                return;

            var item = _versionList[e.RowIndex];

            var cellStyle = ui_versionDGV.DefaultCellStyle.Clone();
            cellStyle.Font = new Font(cellStyle.Font, item.IsDefault ? FontStyle.Bold : FontStyle.Regular);
        }

        private void VersionDGV_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_sorter == null)
                return;

            if (_sortedColumn != -1 && _sortedColumn != e.ColumnIndex)
                ui_versionDGV.Columns[_sortedColumn].HeaderCell.SortGlyphDirection = SortOrder.None;

            var hc = ui_versionDGV.Columns[e.ColumnIndex].HeaderCell;
            hc.SortGlyphDirection = (hc.SortGlyphDirection != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending);

            _sortedColumn = e.ColumnIndex;
            _sortedOrder = hc.SortGlyphDirection;

            _versionList.Sort(_sorter);

            RefreshGrid();
        }

        private void VersionDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            AranVersion version = null;
            if (ui_versionDGV.CurrentCell != null)
                version = _versionList[ui_versionDGV.CurrentCell.RowIndex];

            SetChangesInfo(version);
            ui_setUserGroupTSB.Enabled = (version != null);

            if (ui_versionUserFilterByVersionRB.Checked)
                FillVersionUserGroup(version == null ? null : (long?)version.Id);

            SetStatusText();
        }

        private void SetStatusText()
        {
            _statusText = string.Format("Versions: {0}, History: {1}", _versionList.Count, ui_versionUserGrDGV.Rows.Count);

            if (StatusTextChanged != null)
                StatusTextChanged(this, null);
        }

        private void SetChangesInfo(AranVersion aranVersion)
        {
            try
            {
                if (aranVersion == null)
                    ui_versionRTB.Rtf = string.Empty;
                else
                    ui_versionRTB.Rtf = aranVersion.ChangesRtf;
            }
            catch { }
        }

        private void FillVersionUserGroup(long? versionId)
        {
            ui_versionUserGrDGV.Rows.Clear();

            if (versionId == null)
                return;

            var list = Global.DbPro.GetVersionUserGroupDocs(versionId.Value);

            foreach (var item in list)
            {
                var rowIndex = ui_versionUserGrDGV.Rows.Add(new object[] { item.UserGroup, item.DateTime, item.Note });
                var row = ui_versionUserGrDGV.Rows[rowIndex];
                row.Tag = item;
            }
        }

        private void SetUserGroup_Click(object sender, EventArgs e)
        {
            if (ui_versionDGV.CurrentCell == null)
                return;
            
            var version = _versionList[ui_versionDGV.CurrentCell.RowIndex];

            var form = new AddUserGroupToVersionForm();
            form.AranVersion = version;

            if (form.ShowDialog() != DialogResult.OK || form.UserGroup == null)
                return;

            var userGropId = form.UserGroup.Id;

            var vugd = new VersionUserGroupDoc{Note=form.Note};
            vugd.Version.Id = version.Id;
            vugd.UserGroup.Id = userGropId;

            Global.DbPro.SetVersionUserGroup(vugd);

            FillVersionUserGroup(version.Id);
        }

        private void AddNewVersion_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Aran Update Source (*.aus)|*.aus";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var aus = new Aus();
            using (var fs = File.Open(ofd.FileName, FileMode.Open))
            {
                aus.Unpack(fs);
                fs.Close();
            }

            try
            {
                Global.DbPro.WriteNewVersion(new AranUpdManager.NewVersion
                {
                    Key = aus.VersionName,
                    ReleasedDate = aus.ReleaseDate,
                    Data = aus.BinFile,
                    ChangesRtf = aus.ChangesRTF
                });
            }
            catch (Exception ex)
            {
                Global.ShowException(ex);
                return;
            }

            OpenPage();

            var index = _versionList.FindIndex(av => av.Key == aus.VersionName);
            ui_versionDGV.CurrentCell = ui_versionDGV.Rows[index].Cells[0];
        }

        private void ParseInfoTxt(string fileName, out string versionName, out DateTime? releaseDate)
        {
            versionName = null;
            releaseDate = null;

            using (var sr = File.OpenText(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("version:"))
                        versionName = line.Substring(8).Trim();
                    else if (line.StartsWith("releaseDate:"))
                        releaseDate = DateTime.Parse(line.Substring(12).Trim());
                }
            }
        }

        private void VersionUserFilterByVersion_CheckedChanged(object sender, EventArgs e)
        {
            VersionDGV_CurrentCellChanged(null, null);
        }

        private void VersionUserNoFilter_CheckedChanged(object sender, EventArgs e)
        {
            FillVersionUserGroup(-1);
        }
    }
}
