using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Panda.Common;
using Aran.AranEnvironment;
using Aran.Aim.Data;
using Aran.Aim.Enums;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using System.Threading;

namespace Aran.Omega.TypeB.Settings
{
    public partial class SettingsForm : Form
    {
        #region Declaration

      
        #endregion

        public SettingsForm ()
        {
            InitializeComponent ();
        }

        public string Title
        {
            get { return "Omega"; }
        }

        public Control Page
        {
            get { return this; }
        }

        private void InitData()
        {
            try
            {
                if (!Globals.Settings.DbConnection.IsEmpty)
                {
                    txtDbName.Text = Globals.Settings.DbConnection.DbName;
                    txtHost.Text = Globals.Settings.DbConnection.Host;
                    txtPort.Text = Globals.Settings.DbConnection.Port.ToString();
                    txtUserName.Text = Globals.Settings.DbConnection.UserName;
                    txtPassword.Text = Globals.Settings.DbConnection.Password;
                    Connect();
                }
                else 
                {
                    Globals.Settings.DbConnection.DbName = txtDbName.Text;
                    Globals.Settings.DbConnection.Host =txtHost.Text;
                    Globals.Settings.DbConnection.Port =Convert.ToInt32(txtPort.Text);
                    Globals.Settings.DbConnection.UserName=txtUserName.Text;
                    Globals.Settings.DbConnection.Password=txtPassword.Text;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void OnLoad (TypeBSettings settings)
        {
            Globals.Settings = settings;
            _dbConnection = Globals.Settings.DbConnection;
            InitData();
            SettingsControl control = elementHost2.Child as SettingsControl;
            control.LoadAll();
        }

        public bool OnSave ()
        {
            SaveSettings ();
            return true;
        }

        public bool Connect() 
        {
            DbProvider dbProvider = DbProviderFactory.Create("Aran.Aim.Data.PgDbProviderComplex");
            //int transId = dbProvider.BeginTransaction();
            string userName, password;
            string connectionString = string.Format(
                "Server={0};Port={1};User Id={3};Password={4};Database={2};CommandTimeout=340",
                txtHost.Text, txtPort.Text, txtDbName.Text, "aran", "airnav2012");
            try
            {
                dbProvider.Open(connectionString);
                string pass = DbUtility.GetMd5Hash(txtPassword.Text);
                if (!dbProvider.Login(txtUserName.Text, pass))
                    MessageBox.Show("Cannot connect to Database");

                _dbConnection.DbProvider = dbProvider;
                _dbConnection.IsEmpty = false;

                SettingsControl control = elementHost2.Child as SettingsControl;
                control.LoadAll();
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot connect to database", "TypeB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        
        }

        private void SaveSettings ()
        {
           // Globals.Settings.Store (SettingsGlobals.PandaAranExtension);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (Connect())
                MessageBox.Show("Connection is successfull");
        }

        private void txtHost_TextChanged(object sender, EventArgs e)
        {
            _dbConnection.Host = txtHost.Text;
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            _dbConnection.Port = Convert.ToInt32(txtPort.Text);
        }

        private void txtDbName_TextChanged(object sender, EventArgs e)
        {
            _dbConnection.DbName = txtDbName.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Globals.Settings.DbConnection = _dbConnection;
            Close();
        }

        private DbConnectionModel _dbConnection;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
