using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.AranEnvironment;
using System.Threading;

namespace Aran.Queries.Common
{
    public partial class DbProviderControl2 : UserControl
    {
        private Connection _connection;
        private string _prevServer;
        private int _prevPort;
        private bool _loginView;

        public event DbProControlLoadDbListEventHandler LoadedDbList;


        public DbProviderControl2()
        {
            InitializeComponent();

            _connection = new Connection();
        }


        public void SetValue(Connection conn)
        {
            if (conn == null)
                throw new Exception("Connection Value is invalid!");

            _connection.Assign(conn);

            var panelContainer = tabControl1.Parent;

            switch (conn.ConnectionType) {
                case ConnectionType.Aran: {
                        ui_pgPanel.Parent = panelContainer;
                        ui_pgServerTB.Text = conn.Server;
                        ui_pgPortTB.Text = conn.Port.ToString();
                        ui_pgUserNameTB.Text = conn.UserName;
                        ui_pgPswTB.Text = conn.Password;
                        ui_pgDbCB.Text = conn.Database;
                    }
                    break;
                case ConnectionType.ComSoft: {
                        ui_comSoftPanel.Parent = panelContainer;
                        ui_csServerTB.Text = conn.Server;
                        ui_csPortTB.Text = conn.Port.ToString();
                        ui_csUserNameTB.Text = conn.UserName;
                        ui_csPasswordTB.Text = conn.Password;
                    }
                    break;
                case ConnectionType.AimLocal: {
                        ui_aimLocalPanel.Parent = panelContainer;
                        ui_aimLocalFileNameTB.Text = conn.Server;
                    }
                    break;
            }

            IsValueChanged = false;
        }

        public Connection GetValue()
        {
            return _connection.Clone() as Connection;
        }

        public bool IsValueChanged { get; private set; }

        public string ErrorMessage
        {
            get { return ui_errorLabel.Text; }
            set
            {
                ui_errorLabel.Visible = !string.IsNullOrEmpty(value);
                ui_errorLabel.Text = value;
                ui_errorLabel.ForeColor = Color.Red;
            }
        }

        public bool IsSupportLogin
        {
            get
            {
                switch (_connection.ConnectionType) {
                    case ConnectionType.Aran:
                    case ConnectionType.ComSoft:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool LoginView
        {
            get { return _loginView; }
            set
            {
                _loginView = value;

                switch (_connection.ConnectionType) {
                    case ConnectionType.Aran:
                        ui_pgServerTB.ReadOnly = value;
                        ui_pgPortTB.ReadOnly = value;
                        ui_pgDbCB.Enabled = !value;
                        break;
                    case ConnectionType.ComSoft:
                        ui_csServerTB.ReadOnly = value;
                        ui_csPortTB.ReadOnly = value;
                        break;
                    case ConnectionType.AimLocal:
                        ui_aimLocalFileNameTB.ReadOnly = value;
                        break;
                }
            }
        }

        public bool IsPasswordSaved
        {
            get { return ui_pgSavePswChB.Checked; }
            set { ui_pgSavePswChB.Checked = value; }
        }

        public bool IsVisibleSavePasswordBox
        {
            get { return ui_pgSavePswChB.Visible; }
            set { ui_pgSavePswChB.Visible = value; }
        }



        private void XTB_TextChanged(object sender, EventArgs e)
        {
            var conn = _connection;

            switch (conn.ConnectionType) {
                case ConnectionType.Aran: {
                        conn.Server = ui_pgServerTB.Text;

                        int port;
                        if (int.TryParse(ui_pgPortTB.Text, out port))
                            conn.Port = port;
                        else
                            ui_pgPortTB.Text = conn.Port.ToString();

                        conn.UserName = ui_pgUserNameTB.Text;
                        conn.Password = ui_pgPswTB.Text;
                        try {
                            conn.Database = ui_pgDbCB.Text;
                        }
                        catch { }
                    }
                    break;
                case ConnectionType.ComSoft: {
                        conn.Server = ui_csServerTB.Text;
                        int port;
                        
                        if (int.TryParse(ui_csPortTB.Text, out port))
                            conn.Port = port;
                        else
                            ui_csPortTB.Text = conn.Port.ToString();

                        conn.UserName = ui_csUserNameTB.Text;
                        conn.Password = ui_csPasswordTB.Text;
                    }
                    break;
                case ConnectionType.AimLocal: {
                        conn.Server = ui_aimLocalFileNameTB.Text;
                    }
                    break;
            }
            
            IsValueChanged = true;
        }

        private void ui_csShowPwdChB_CheckedChanged(object sender, EventArgs e)
        {
            ui_csPasswordTB.UseSystemPasswordChar = !ui_csShowPwdChB.Checked;
        }

        private void CreateAimLocalFile_Click(object sender, EventArgs e)
        {
            var fd = new SaveFileDialog();
            fd.Title = "Create AIM Data Source";
            fd.Filter = "AIM Data Source Files (*.ads)|*.ads";

            if (fd.ShowDialog() == DialogResult.OK)
                ui_aimLocalFileNameTB.Text = fd.FileName;
        }

        private void SelectAimLocalFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "Select AIM Data Source";
            ofd.Filter = "AIM Data Source Files (*.ads)|*.ads";

            if (ofd.ShowDialog() == DialogResult.OK) 
                ui_aimLocalFileNameTB.Text = ofd.FileName;
            
        }

        private void PgDbCB_DropDown(object sender, EventArgs e)
        {
            if (LoadedDbList == null || string.IsNullOrEmpty(ui_pgServerTB.Text) || string.IsNullOrEmpty(ui_pgPortTB.Text))
                return;

            int port;
            if (!int.TryParse(ui_pgPortTB.Text, out port))
                return;

            if (ui_pgServerTB.Text == _prevServer && port == _prevPort)
                return;

            _prevPort = int.Parse(ui_pgPortTB.Text);
            _prevServer = ui_pgServerTB.Text;

            ui_pgDbCB.Items.Clear();
            ui_pgDbCB.Items.Add("Loading...");

            var thread = new Thread(() => {
                var ee = new DbProControlLoadDbListEventArgs(_prevServer, _prevPort);
                LoadedDbList(this, ee);

                Invoke(new Action(() => {
                    ui_pgDbCB.Items.Clear();
                    if (ee.DbList.Count > 0)
                        ee.DbList.ForEach(dbText => ui_pgDbCB.Items.Add(dbText));
                }));
            });

            Application.DoEvents();

            thread.Start();
        }

        

    }

    public delegate void DbProControlLoadDbListEventHandler (object sender, DbProControlLoadDbListEventArgs e);

    public class DbProControlLoadDbListEventArgs : EventArgs
    {
        public DbProControlLoadDbListEventArgs(string server, int port)
        {
            Server = server;
            Port = port;
            DbList = new List<string>();
        }

        public string Server { get; private set; }

        public int Port { get; private set; }

        public List<string> DbList { get; private set; }
    }
}
