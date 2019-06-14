using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AIM_Data_Validator
{
	public partial class FormValidationList : Form
	{
		public FormValidationList ( )
		{
			InitializeComponent ( );
			chckBxListFeats.SearchTextBoxVisible = true;
		}

		public FormValidationList ( ValidatorController validatorController )
			: this ( )
		{
			_validatorController = validatorController;
		}

		/// <summary>
		/// To fill appropriate controls gets feature names and its properties
		/// </summary>
		private void FillCheckBoxLists ( )
		{
			foreach ( var item in _validatorController.Features )
			{
				chckBxListFeats.AddRow ( item.Key );
			}
		}

		private void ValidateDatabase ( DatabaseAttributes dbAttributes )
		{
			tsLblConnect.Text = string.Format ( "Connected to {0} on {1} port on {2} server ", dbAttributes.Name, dbAttributes.Port, dbAttributes.Server );
		}

		private void chckBxListFeats_SelectedRowChanged ( int index, string value )
		{
			//MessageBox.Show ( "SelectedRow called !" );
			_validatorController.SelectedFeature = value;
			chckBxListProps.RemoveAll ( );
			foreach ( var property in _validatorController.Features [ value ] )
			{
				chckBxListProps.AddRow ( property.Key, property.Value );
			}
		}

		private void chckBxListFeats_RowCheckedChanged ( string value, bool isChecked )
		{
			//MessageBox.Show ( "SelectedRowCheckedChanged called !" );
			if ( isChecked )
			{
				if ( chckBxListProps.SelectedItemsCount == 0 )
				{
					chckBxListProps.SetAll ( isChecked );
				}
			}
			else
			{
				chckBxListProps.SetAll ( false );
			}
		}

		private void chckBxListProps_RowCheckedChanged ( string value, bool isChecked )
		{
			_validatorController.Features [ _validatorController.SelectedFeature ] [ value ] = isChecked;
			if ( isChecked )
				chckBxListFeats.CheckItem ( _validatorController.SelectedFeature, true );
			else if ( chckBxListProps.SelectedItemsCount == 0 )
				chckBxListFeats.CheckItem ( _validatorController.SelectedFeature, false );
		}

		private void tsBtnSettings_CheckedChanged ( object sender, EventArgs e )
		{
			if ( tsBtnSettings.Checked )
				tbCntrlMain.SelectTab ( 1 );
			else
				tbCntrlMain.SelectTab ( 0 );
		}

		private void btnSetupDbApply_Click ( object sender, EventArgs e )
		{
			int port;
			if ( int.TryParse ( rchTxBxPort.Text, out port ) )
			{
				if ( _validatorController.SetupDbConnection ( txtBxServer.Text, port, txtBxUserName.Text, txtBxPassword.Text, txtBxDbName.Text ) )
				{
					tbCntrlMain.SelectedTab = tbPageFeats;
					tsLblConnection.Text = string.Format ( " Connected to '{0}' on {1} port on '{2}' server", txtBxDbName.Text, port, txtBxServer.Text );
				}
			}
		}

		private void rchTxBxPort_KeyPress ( object sender, KeyPressEventArgs e )
		{
			if ( !char.IsDigit ( e.KeyChar ) )
			{
				e.Handled = true;
			}

		}

		private void btnCheck_Click ( object sender, EventArgs e )
		{
			_validatorController.Check ( );
		}

		ValidatorController _validatorController;

		private void tsBtnSettings_Click ( object sender, EventArgs e )
		{

		}
	}
}