using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Aran.AranEnvironment;

namespace MapEnv
{
    public partial class NewProRecentFilesPage : UserControl
    {
        private List<RowItem> _fileNames;


        public NewProRecentFilesPage()
        {
            InitializeComponent();

            _fileNames = new List<RowItem>();
            label1.BorderStyle = BorderStyle.None;
        }


        public event EventHandler SelectedChanged;


        public string SelectedFileName
        {
            get
            {
                if (ui_dgv.CurrentRow == null)
                    return null;
                return _fileNames[ui_dgv.CurrentRow.Index].FileName;
            }
        }

        public void AddFile(string recentFile)
        {
            try {
                Connection conn;
                byte[] thumbnailImageBytes;
                if (MapEnvData.GetConnectionAndThumbnail(recentFile, out conn, out thumbnailImageBytes)) {

                    using (var ms = new MemoryStream(thumbnailImageBytes)) {
                        var image = Image.FromStream(ms);
                        _fileNames.Add(new RowItem() { FileName = recentFile, Image = image, Connection = conn });
                    }
                }
            }
            catch { }
        }

        public void RefreshGrid()
        {
            ui_dgv.RowCount = _fileNames.Count;
            ui_dgv.Refresh();
        }

        private void NewProRecentFilesPage_Load(object sender, EventArgs e)
        {
            label1.Parent = ui_thumbPictureBox;
            label1.Location = new Point(2, 2);
            label1.BackColor = Color.Transparent;
        }


        private void DGV_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _fileNames.Count)
                return;

            var rowItem = _fileNames[e.RowIndex];

            switch (e.ColumnIndex) {
                case 0:
                    e.Value = e.RowIndex + 1;
                    break;
                case 1:
                    e.Value = Path.GetFileName(rowItem.FileName);
                    break;
                case 2:
                    e.Value = Path.GetDirectoryName(rowItem.FileName);
                    break;
            }
        }

        private void DGV_CurrentCellChanged(object sender, EventArgs e)
        {
            ui_thumbPictureBox.Image = null;

            if (SelectedChanged != null)
                SelectedChanged(this, e);

            if (ui_dgv.CurrentRow == null)
                return;

            var rowItem = _fileNames[ui_dgv.CurrentRow.Index];

            if (rowItem.Image != null)
                ui_thumbPictureBox.Image = rowItem.Image;

            if (rowItem.Connection != null)
                label1.Text = GetConnectionInfoText(rowItem.Connection);
        }

        private string GetConnectionInfoText(Connection connection)
        {
            var s = string.Format("Type:  {0}", Globals.GetConnectionTypeText((connection.ConnectionType)));

            switch (connection.ConnectionType) {
                case ConnectionType.AimLocal:
                case ConnectionType.XmlFile:
                    break;
                case ConnectionType.Aran:
                    s += string.Format("\nServer:  {0}\nPort:  {1}\nDatabase:  {2}",
                        connection.Server, connection.Port, connection.Database);
                    break;
                case ConnectionType.ComSoft:
                    s += string.Format("\nServer:  {0}\nPort:  {1}", connection.Server, connection.Port);
                    break;
                case ConnectionType.TDB:
                    s += string.Format("\nServer: {0}\nPort:  {1}", connection.Server, connection.Port);
                    break;
            }

            return s;
        }

        private void ClearRecentFilesContextMenuItem_Click(object sender, EventArgs e)
        {
            Globals.Settings.RecentProjectFiles.Clear();
            _fileNames.Clear();
            RefreshGrid();
        }

        private class RowItem
        {
            public string FileName { get; set; }

            public Image Image { get; set; }

            public Connection Connection { get; set; }
        }
    }
}
