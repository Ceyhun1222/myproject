using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.InputFormLib;
using Aran.Aim.Data;
using Aran.Aim;
using System.Security.Cryptography;

namespace MapEnv.Forms
{
	public partial class FormUser : Form
	{
		private bool _confirmPasswordTyped;
		private bool _isEdit;
		private bool _callEvent;
		public User User
		{
			get;
			set;
		}
		public List<Aran.Aim.Data.User> UserList
		{
			get;
			set;
		}

		public FormUser ( )
		{
			InitializeComponent ( );
		}

		public FormUser ( Aran.Aim.Data.User user )
			: this ( )
		{
			// TODO: Complete member initialization
			this.User = user;
			_isEdit = true;
		}

		private void chckBxSelectAll_CheckedChanged ( object sender, EventArgs e )
		{
			if ( chckBxSelectAll.Checked )
				chckBxSelectAll.Text = "Deselect All";
			else
				chckBxSelectAll.Text = "Select All";

			foreach ( TreeNode item in treeViewFeatTypes.Nodes )
			{
				CheckTreeNode ( item, chckBxSelectAll.Checked );
			}
		}

		private void CheckTreeNode ( TreeNode node, bool isChecked )
		{
			node.Checked = isChecked;
			foreach ( TreeNode item in node.Nodes )
			{
				CheckTreeNode ( item, isChecked );
			}
		}

		private void FormUser_Load ( object sender, EventArgs e )
		{
			InputFormController inputFormController = new InputFormController ( );
			var rootFeatTypes = inputFormController.GetFeaturesByDepends ( null );
			foreach ( string featType in rootFeatTypes )
			{
				TreeNode treeNode = new TreeNode ( );
				treeNode.Name= featType;
				treeNode.Text = featType;
				treeViewFeatTypes.Nodes.Add ( treeNode );
				AddTreeNode ( treeNode, inputFormController, featType );
			}
			treeViewFeatTypes.EndUpdate ( );
			treeViewFeatTypes.Sort ( );
			txtBxUserName.Focus ( );

			if ( User != null )
			{
				txtBxUserName.Text = User.Name;
				if ( User.Privilege == Privilige.prReadOnly )
					radBtnReadOnly.Checked = true;
				else if ( User.Privilege == Privilige.prReadWrite )
					radBtnReadWrite.Checked = true;
				_callEvent = false;
				foreach ( FeatureType featType in User.FeatureTypes )
				{
					TreeNode[] nodes = treeViewFeatTypes.Nodes.Find ( featType.ToString ( ), true );
					if ( nodes.Length > 0 )
					{
						
						nodes[ 0 ].Checked = true;
					}
				}
			}
			_callEvent = true;
		}

		private void AddTreeNode ( TreeNode treeNode, InputFormController inputFormController, string featType )
		{
			var childFeatTypes = inputFormController.GetFeaturesByDepends ( featType );
			foreach ( string childFeatType in childFeatTypes )
			{
				TreeNode childTreeNode = new TreeNode ( );
				childTreeNode.Name= childFeatType;
				childTreeNode.Text = childFeatType;
				treeNode.Nodes.Add ( childTreeNode );
				AddTreeNode ( childTreeNode, inputFormController, childFeatType );
			}
		}

		private void treeViewFeatTypes_AfterCheck ( object sender, TreeViewEventArgs e )
		{
			if ( !_callEvent )
				return;
			_callEvent = false;
			foreach ( TreeNode node in e.Node.Nodes )
			{
				CheckTreeNode ( node, e.Node.Checked );
			}
			_callEvent = true;
		}

		private void txtBxUserName_TextChanged ( object sender, EventArgs e )
		{
			this.Text = "User -" + txtBxUserName.Text;
		}

		private void chckBxShowPassword_CheckedChanged ( object sender, EventArgs e )
		{
			txtBxPassword.UseSystemPasswordChar = !chckBxShowPassword.Checked;
			txtBxConfirmPassword.UseSystemPasswordChar = !chckBxShowPassword.Checked;
		}

		private void btnCancel_Click ( object sender, EventArgs e )
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close ( );
		}

		private void btnOk_Click ( object sender, EventArgs e )
		{
			if ( txtBxUserName.Text == "" )
			{
				MessageBox.Show ( "Please, type user name" );				
				txtBxUserName.Focus ( );
				return;
			}

			// First condition (Edit): Change user name to other existing username
			// Second condition (New User): Creates new user that user name exists in db
			if ( ( _isEdit && User.Name != txtBxUserName.Text && UserList.Find ( user => user.Name == txtBxUserName.Text ) != null ) || ( !_isEdit && UserList.Find ( user => user.Name == txtBxUserName.Text ) != null ) )
			{
				MessageBox.Show ( this, "User name you typed exists.\r\nPlease, type another user name", Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
				txtBxUserName.Focus ( );
				return;
			}
			if ( txtBxPassword.Text == "" )
			{
				MessageBox.Show ( this, "Please, type password", Text, MessageBoxButtons.OK, MessageBoxIcon.Information );
				txtBxPassword.Focus ( );
				return;
			}
			if ( lblPasswordMatch.Visible )
			{
				MessageBox.Show ( this, "Please, confirm password !", Text, MessageBoxButtons.OK, MessageBoxIcon.Information );
				lblPasswordMatch.Focus ( );
				return;
			}
			
			if ( !_isEdit )
				User = new User ( );
			else
				User.FeatureTypes.Clear ( );
			foreach ( TreeNode node in treeViewFeatTypes.Nodes )
			{
				if ( node.Checked )
				{
					User.AddFeatType(node.Text);
				}
				AddChildFeatTypes ( node );
			}
			if ( User.FeatureTypes.Count == 0 )
			{
				MessageBox.Show ( "Please, select at least one feature to give a selected privilige" );
				return;
			}
			
			User.Name = txtBxUserName.Text;
            User.Password = DbUtility.GetMd5Hash (txtBxPassword.Text);
			if ( radBtnReadOnly.Checked )
				User.Privilege = Privilige.prReadOnly;
			else if ( radBtnReadWrite.Checked )
				User.Privilege = Privilige.prReadWrite;
			
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			Close ( );
		}		

		private void AddChildFeatTypes ( TreeNode node )
		{
			foreach ( TreeNode childNode in node.Nodes )
			{
				if ( childNode.Checked )
				{
					User.AddFeatType ( childNode.Text );
				}
				AddChildFeatTypes ( childNode );
			}
		}

		private void txtBxConfirmPassword_TextChanged ( object sender, EventArgs e )
		{
			_confirmPasswordTyped = true;
			CheckVisibiltyPasswordMatch ( );			
		}

		private void CheckVisibiltyPasswordMatch ( )
		{
			if ( txtBxConfirmPassword.Text != txtBxPassword.Text )
			{
				lblPasswordMatch.Visible = true;
				lblPasswordMatch.Text = "Password doesn't match";
				lblPasswordMatch.ForeColor = System.Drawing.Color.Red;
			}
			else
			{
				lblPasswordMatch.Visible = false;
				lblPasswordMatch.Text = "Password matchs";
				lblPasswordMatch.ForeColor = System.Drawing.SystemColors.ControlText;
			}
		}

		private void txtBxConfirmPassword_Leave ( object sender, EventArgs e )
		{
			CheckVisibiltyPasswordMatch ( );
		}

		private void txtBxPassword_TextChanged ( object sender, EventArgs e )
		{
			if ( _confirmPasswordTyped )
				CheckVisibiltyPasswordMatch ( );
		}

		private void txtBxUserName_KeyPress ( object sender, KeyPressEventArgs e )
		{
			if ( e.KeyChar == ' ')
			{
				e.Handled = true;
			}
		}		
	}
}