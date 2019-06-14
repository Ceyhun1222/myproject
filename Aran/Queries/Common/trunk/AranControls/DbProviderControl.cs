using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Package;
using System.IO;
using System.Xml;
using Aran.AranEnvironment;
using Aran.Aim.Data;

namespace Aran.Controls
{
    public partial class DbProviderControl : UserControl
    {
        private Connection _connection;
        private bool _userNameOrPasswordVisible;


        public DbProviderControl ()
        {
            InitializeComponent ();

            _connection = new Connection ();

            ui_aranDbRadioButton.Tag = ConnectionType.Aran;
            ui_comsoftDbRadioButton.Tag = ConnectionType.ComSoft;
            ui_xmlDbRadioButton.Tag = ConnectionType.XmlFile;
			ui_tdbRadioButton.Tag = ConnectionType.TDB;
        }


		public void SetValue(Connection conn)
		{
			if (conn != null)
				_connection.Assign (conn);

			foreach (Control cont in ui_dbsPanel.Controls)
			{
				if (_connection.ConnectionType == (ConnectionType) cont.Tag)
				{
					var rb = cont as RadioButton;
					rb.Checked = true;
					DbTypeRadioButton_CheckedChanged (rb, null);
					break;
				}
			}

			ui_serverTB.Text = _connection.Server;
			ui_portTB.Text = _connection.Port.ToString ();
			ui_userNameTB.Text = _connection.UserName;
			ui_passwordTB.Text = string.Empty;// _connection.Password;
			ui_dbNameTB.Text = _connection.Database;
			ui_xmlFileNameTB.Text = _connection.XmlFileName;
			ui_useCacheCheckBox.Checked = _connection.UseCache;

			IsValueChanged = false;
		}

		public Connection GetValue()
		{
			return _connection.Clone () as Connection;
		}

        public bool IsValueChanged { get; private set; }

        public ConnectionType ConnectionType
        {
            get
            {
                return _connection.ConnectionType;
            }
            set
            {
                _connection.ConnectionType = value;

                ui_serverPanel.Visible = true;
                ui_userNamePswPanel.Visible = _userNameOrPasswordVisible;
                ui_xmlFileNamePanel.Visible = false;
                ui_useCachePanel.Visible = false;

                if (value == ConnectionType.Aran || value == ConnectionType.TDB) {
                    ui_portPanel.Visible = true;
                    ui_dbNamePanel.Visible = true;
                }
                else {
                    ui_portPanel.Visible = false;
                    ui_dbNamePanel.Visible = false;

                    if (value == ConnectionType.ComSoft) {
                        ui_useCachePanel.Visible = true;
                    }
                    else {
                        ui_serverPanel.Visible = false;
                        ui_userNamePswPanel.Visible = false;
                        ui_xmlFileNamePanel.Visible = true;
                    }
                }
            }
        }

        public bool VisibleDbTypePanel
        {
            get { return ui_dbsPanel.Visible; }
            set { ui_dbsPanel.Visible = value; }
        }

        public bool ReadOnly
        {
            get { return ui_serverTB.ReadOnly; }
            set
            {
                ui_serverTB.ReadOnly = value;
                ui_portTB.ReadOnly = value;
                ui_userNameTB.ReadOnly = value;
                ui_passwordTB.ReadOnly = value;
                ui_dbNameTB.ReadOnly = value;
                ui_xmlFileNameTB.ReadOnly = value;

                ui_useCacheCheckBox.Enabled = !value;
                chckBxShowPassword.Enabled = !value;
            }
        }

        public bool UserNameOrPasswordVisible
        {
            get
            {
                return _userNameOrPasswordVisible;
            }
            set
            {
                _userNameOrPasswordVisible = value;
                ui_userNamePswPanel.Visible = value;
            }
        }


        private void DbTypeRadioButton_CheckedChanged (object sender, EventArgs e)
        {
            ConnectionType = (ConnectionType) ((Control) sender).Tag;
        }

        private void ConnectionInfoProps_TextChanged (object sender, EventArgs e)
        {
            try
            {
                TextBox tb = (TextBox) sender;
                string s = tb.Text;

                if (ui_serverTB.Equals(tb))
                    _connection.Server = s;
                else if (ui_portTB.Equals(tb))
                    _connection.Port = int.Parse(s);
                else if (ui_userNameTB.Equals(tb))
                    _connection.UserName = s;
                else if (ui_passwordTB.Equals(tb))
                    _connection.Password = s;
                else if (ui_dbNameTB.Equals(tb))
                    _connection.Database = s;
                else if (ui_xmlFileNameTB.Equals(tb))
                    _connection.XmlFileName = s;

                IsValueChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UseCache_CheckedChanged (object sender, EventArgs e)
        {
            _connection.UseCache = ((CheckBox) sender).Checked;
            IsValueChanged = true;
        }

		private void chckBxShowPassword_CheckedChanged ( object sender, EventArgs e )
		{
			ui_passwordTB.UseSystemPasswordChar = !chckBxShowPassword.Checked;
		}

		private void SelectXmlFile_Click (object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog ();
			ofd.Multiselect = false;
			ofd.Title = "Select AIX Message File";
			ofd.Filter = "XML Files (*.xml)|*.xml|GML Files (*.gml)|*.gml|All Files (*.*)|*.*";

			if (ofd.ShowDialog () == DialogResult.OK)
			{
				ui_xmlFileNameTB.Text = ofd.FileName;
			}
		}
    }
}
