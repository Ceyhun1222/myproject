using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AControls
{
	public partial class CheckBoxList : UserControl
	{
		public CheckBoxList ( )
		{
			InitializeComponent ( );
			//CheckedItems = new List<string> ( );
			SearchTextBoxVisible = true;
			_searchText = "Search";
			SelectedItemsCount = 0;
			_hasToBeChecked = true;
		}

		public string Title
		{
			get
			{
				return dataGridView.Columns [ 1 ].HeaderText;
			}

			set
			{
				dataGridView.Columns [ 1 ].HeaderText = value;
			}
		}

		public new Font Font
		{
			get
			{
				return dataGridView.Font;
			}
			set
			{
				dataGridView.Font = value;
			}
		}

		public int SelectedItemsCount
		{
			get;
			private set;
		}

		public void AddRow ( string value, bool isChecked = false )
		{
			int index = dataGridView.Rows.Add ( );
			DataGridViewRow row = dataGridView.Rows [ index ];
			row.Cells [ 0 ].Value = isChecked;
			row.Cells [ 1 ].Value = value;
			if ( dataGridView.Rows.Count == 1 && SelectedRowChanged != null )
				SelectedRowChanged ( 0, value );

			//dataGridView.Rows.Add(
			SetChckBxAll ( isChecked );
		}

		public bool SearchTextBoxVisible
		{
			get
			{
				return txtBxSearch.Visible;
			}

			set
			{
				txtBxSearch.Visible = value;
			}
		}

		public void RemoveAll ( )
		{
			dataGridView.Rows.Clear ( );
			SelectedItemsCount = 0;
			chckBxAll.Checked = false;
		}

		public void SetAll ( bool checkState )
		{
			if ( checkState == chckBxAll.Checked )
			{
				chckBxAll_CheckedChanged ( false, null );
			}
			chckBxAll.Checked = checkState;
		}

		public bool IsChecked ( int index )
		{
			return ( ( bool ) dataGridView.Rows [ index ].Cells [ 0 ].Value );
		}

		public bool IsChecked ( string value )
		{
			foreach ( DataGridViewRow row in dataGridView.Rows )
			{
				if ( row.Cells [ 1 ].Value.ToString() == value )
				{
					return IsChecked ( row.Index );
				}
			}
			return false;
		}

		public void CheckItem ( string value, bool isChecked )
		{
			foreach ( DataGridViewRow row in dataGridView.Rows )
			{
				if ( row.Cells [ 1 ].Value.ToString ( ) == value )
				{
					if ( !IsChecked ( row.Index ) )
						CheckItem ( row.Index, isChecked );
					return;
				}
			}
		}

		public void CheckItem ( int index, bool isChecked )
		{
			dataGridView.Rows [ index ].Cells [ 0 ].Value = isChecked;
		}

		private void chckBxAll_CheckedChanged ( object sender, EventArgs e )
		{

			if ( !_hasToBeChecked )
				return;
			bool isChecked = chckBxAll.Checked;
			_hasToBeChecked = false;
			CheckAll ( isChecked );
			_hasToBeChecked = true;
		}

		private void CheckAll ( bool isChecked )
		{
			foreach ( var item in dataGridView.Rows )
			{
				( ( DataGridViewRow ) item ).Cells [ 0 ].Value = isChecked;
				dataGridView_CellContentClick ( dataGridView, new DataGridViewCellEventArgs ( 0, ( ( DataGridViewRow ) item ).Index ) );
			}
			if ( isChecked )
				SelectedItemsCount = dataGridView.Rows.Count;
			else
				SelectedItemsCount = 0;
		}

		private void txtBxSearch_Enter ( object sender, EventArgs e )
		{
			txtBxSearch.Text = "";
			txtBxSearch.ForeColor = SystemColors.ControlText;
		}

		private void txtBxSearch_Leave ( object sender, EventArgs e )
		{
			txtBxSearch.Text = _searchText;
			txtBxSearch.ForeColor = SystemColors.InactiveCaptionText;
		}

		private void txtBxSearch_TextChanged ( object sender, EventArgs e )
		{
			SearchTextChanged ( txtBxSearch.Text );
		}

		private void dataGridView_RowEnter ( object sender, DataGridViewCellEventArgs e )
		{
			if ( SelectedRowChanged != null && dataGridView.Rows [ e.RowIndex ].Cells [ 1 ].Value != null )
				SelectedRowChanged ( e.RowIndex, dataGridView.Rows [ e.RowIndex ].Cells [ 1 ].Value.ToString ( ) );
		}

		private void dataGridView_CellContentClick ( object sender, DataGridViewCellEventArgs e )
		{
			if ( e.ColumnIndex != 0 )
				return;
			dataGridView.CommitEdit ( DataGridViewDataErrorContexts.Commit );
			if ( RowCheckedChanged != null )
			{
				bool isChecked = ( bool ) dataGridView [ 0, e.RowIndex ].Value;
				if ( _hasToBeChecked )
					SetChckBxAll ( isChecked );
				RowCheckedChanged ( dataGridView [ 1, e.RowIndex ].Value.ToString ( ), isChecked );
			}
		}

		private void SearchTextChanged ( string text )
		{
			//string text = ( (TextBoxBase ) sender ).Text;
			//int indexSearchText = text.IndexOf ( _searchText );
			//if ( indexSearchText > 0 )
			//{
			//    text = text.Remove ( indexSearchText );
			//    txtBxSearch.Text = text;
			//    return;
			//}

			if ( text == string.Empty || text == _searchText )
			{
				foreach ( var item in dataGridView.Rows )
				{
					( ( DataGridViewRow ) item ).Visible = true;
				}
				return;
			}
			foreach ( var row in dataGridView.Rows )
			{
				( ( DataGridViewRow ) row ).Visible = ( ( ( DataGridViewRow ) row ).Cells [ 1 ].Value ).ToString ( ).Contains ( text );
			}
		}

		private void SetChckBxAll ( bool isChecked )
		{
			if ( isChecked )
				SelectedItemsCount++;
			else if ( SelectedItemsCount > 0 )
				SelectedItemsCount--;
			_hasToBeChecked = false;
			chckBxAll.Checked = ( SelectedItemsCount == dataGridView.Rows.Count );
			_hasToBeChecked = true;
		}

		public event SelectedRowChanged SelectedRowChanged;
		public event RowCheckedChanged RowCheckedChanged;

		private bool _hasToBeChecked;
		private readonly string _searchText;
	}
}