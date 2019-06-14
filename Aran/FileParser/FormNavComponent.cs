using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.Data;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using Aran.Aim.Enums;
using System.IO;

namespace KFileParser
{
	public partial class FormNavComponent : Form
	{
		private Dictionary<CodeNavaidService, List<LineData>> _lnDataDict;
		private DbProvider _dbProvider;
		private string _arpName;
		private Navaid _selectedNavaid;
		private List<Label> _labels;
		private List<ComboBox> _comboBoxes;
		public Dictionary<NavaidComponent, LineData> NavComponentPath;
		private List<int> _cmbBxSelectedIndices;

		public FormNavComponent ( DbProvider dbProvider, string arpName )
		{
			InitializeComponent ( );
			_dbProvider = dbProvider;
			_arpName = arpName;

			_labels = new List<Label> ( );
			_labels.Add ( lbl1 );
			_labels.Add ( lbl2 );
			_labels.Add ( lbl3 );

			_comboBoxes = new List<ComboBox> ( );
			_comboBoxes.Add ( cmbBx1 );
			_comboBoxes.Add ( cmbBx2 );
			_comboBoxes.Add ( cmbBx3 );

			_cmbBxSelectedIndices = new List<int> ( );
			_cmbBxSelectedIndices.Add ( -1 );
			_cmbBxSelectedIndices.Add ( -1 );
			_cmbBxSelectedIndices.Add ( -1 );
		}

		internal DialogResult ShowDialog ( List<LineData> vor_dmeList, List<LineData> ils_dmeList, List<LineData> ndb_mrkList )
		{
			_lnDataDict = new Dictionary<CodeNavaidService, List<LineData>> ( );
			_lnDataDict.Add ( CodeNavaidService.VOR_DME, vor_dmeList );
			_lnDataDict.Add ( CodeNavaidService.ILS_DME, ils_dmeList );
			_lnDataDict.Add ( CodeNavaidService.NDB_MKR, ndb_mrkList );

			FillTreeView ( );
			SetDefaultValues ( );
			return ShowDialog ( );
		}

		private void SetDefaultValues ( )
		{
			foreach ( CodeNavaidService code in _lnDataDict.Keys )
			{
				if ( _lnDataDict[ code ].Count == 1 )
				{
					SetNodeValue ( _lnDataDict[ code ], code );
				}
			}
		}

		private void SetNodeValue ( List<LineData> list, CodeNavaidService code )
		{
			foreach ( TreeNode treeNode in treeView1.Nodes )
			{
				Navaid navaid = ( Navaid ) treeNode.Tag;
				if ( navaid.Type == code )
				{
					switch ( code )
					{
						case CodeNavaidService.ILS_DME:
							treeNode.Nodes[ 1 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "llz" ) ).FileName;
							treeNode.Nodes[ 2 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "gp" ) ).FileName;
							treeNode.Nodes[ 0 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "dme" ) ).FileName;
							break;

						case CodeNavaidService.ILS:
							treeNode.Nodes[ 1 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "llz" ) ).FileName;
							treeNode.Nodes[ 2 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "gp" ) ).FileName;
							break;

						case CodeNavaidService.VOR_DME:
							treeNode.Nodes[ 0 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "vor" )  ).FileName;
							treeNode.Nodes[ 1 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "dme" ) ).FileName;
							break;

						case CodeNavaidService.NDB_MKR:
							treeNode.Nodes[ 0 ].Nodes[ 0 ].Text = list.Find ( lnData => lnData.Id.ToLower ( ).Contains ( "ndb" ) ).FileName;
							break;

						default:
							throw new NotImplementedException ( code + " is not implemented" );
					}
				}
			}
		}

		private void FillTreeView ( )
		{
			ComparisonOps compOperDsg = new ComparisonOps ( ComparisonOpType.EqualTo, "Name", _arpName );
			OperationChoice operChoiceDsg = new OperationChoice ( compOperDsg );
			Filter filter = new Filter ( operChoiceDsg );
            GettingResult getResult = _dbProvider.GetVersionsOf ( Aran.Aim.FeatureType.Navaid, Aran.Aim.Enums.TimeSliceInterpretationType.BASELINE, default ( Guid ), false, null, null, filter );
			if ( !getResult.IsSucceed )
				throw new Exception ( "Error on reading Navaids.\r\n" + getResult.Message );
			List<Navaid> navaidList = getResult.GetListAs<Navaid> ( );
			NavComponentPath = new Dictionary<NavaidComponent, LineData> ( );
			foreach ( Navaid navaid in navaidList )
			{
				TreeNode treeNode = new TreeNode ( navaid.Designator );
				treeNode.Tag = navaid;
				TreeNode dmeNode;
				NavaidComponent dmeNavComp, gpNavComp, llzNavComp;
				switch ( navaid.Type )
				{
					case CodeNavaidService.ILS_DME:
						llzNavComp = (NavaidComponent) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.Localizer );
						if ( llzNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no LLZ component" );
						TreeNode llzNode = new TreeNode ( "LLZ" );
						NavComponentPath.Add ( llzNavComp, null );
						llzNode.Tag = llzNavComp;
						llzNode.Nodes.Add ( "-" );


						gpNavComp = (NavaidComponent) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.Glidepath );
						if ( gpNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no GP component" );
						TreeNode gpNode = new TreeNode ( "GP" );
						NavComponentPath.Add ( gpNavComp, null );
						gpNode.Tag = gpNavComp;
						gpNode.Nodes.Add ( "-" );

						dmeNavComp = (NavaidComponent) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.DME );
						if ( dmeNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no DME component" );
						dmeNode = new TreeNode ( "DME" );
						NavComponentPath.Add ( dmeNavComp,null );
						dmeNode.Tag = dmeNavComp;
						dmeNode.Nodes.Add ( "-" );

						treeNode.Nodes.Add ( dmeNode );
						treeNode.Nodes.Add ( llzNode );
						treeNode.Nodes.Add ( gpNode );
						break;

					case CodeNavaidService.ILS:
						llzNavComp = ( NavaidComponent ) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.Localizer );
						if ( llzNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no LLZ component" );
						TreeNode llzNode2 = new TreeNode ( "LLZ" );
						NavComponentPath.Add ( llzNavComp, null );
						llzNode2.Tag = llzNavComp;
						llzNode2.Nodes.Add ( "-" );

						gpNavComp = ( NavaidComponent ) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.Glidepath );
						if ( gpNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no GP component" );
						TreeNode gpNode2 = new TreeNode ( "GP" );
						NavComponentPath.Add ( gpNavComp, null );
						gpNode2.Tag = gpNavComp;
						gpNode2.Nodes.Add ( "-" );

						treeNode.Nodes.Add ( llzNode2 );
						treeNode.Nodes.Add ( gpNode2 );

						if ( !_lnDataDict.ContainsKey ( CodeNavaidService.ILS ) )
							_lnDataDict.Add ( CodeNavaidService.ILS, _lnDataDict[ CodeNavaidService.ILS_DME ] );

						break;

					case CodeNavaidService.VOR_DME:
						NavaidComponent vorNavComp = (NavaidComponent) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.VOR );
						if ( vorNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no VOR component" );
						TreeNode vorNode = new TreeNode ( "VOR" );
						NavComponentPath.Add ( vorNavComp,null);
						vorNode.Tag = vorNavComp;
						vorNode.Nodes.Add ( "-" );

						dmeNavComp = (NavaidComponent) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.DME );
						if ( dmeNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no DME component" );
						dmeNode = new TreeNode ( "DME" );
						NavComponentPath.Add ( dmeNavComp,null );
						dmeNode.Tag = dmeNavComp;
						dmeNode.Nodes.Add ( "-" );

						treeNode.Nodes.Add ( dmeNode );
						treeNode.Nodes.Add ( vorNode );
						break;

					case CodeNavaidService.NDB_MKR:
						NavaidComponent ndbNavComp = (NavaidComponent) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.NDB );
						if ( ndbNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no NDB component" );
						TreeNode ndbNode = new TreeNode ( "NDB" );
						NavComponentPath.Add ( ndbNavComp,null );
						ndbNode.Tag = ndbNavComp;
						ndbNode.Nodes.Add ( "-" );

						NavaidComponent mrkNavComp = (NavaidComponent) navaid.NavaidEquipment.Find ( navComop => navComop.TheNavaidEquipment.Type == Aran.Aim.NavaidEquipmentType.MarkerBeacon );
						if ( mrkNavComp == null )
							throw new NotImplementedException ( navaid.Name + " has no Marker Beacon component" );
						TreeNode mrkNode = new TreeNode ( "MarkerBeacon" );
						NavComponentPath.Add ( mrkNavComp ,null);
						mrkNode.Tag = mrkNavComp;
						mrkNode.Nodes.Add ( "-" );

						treeNode.Nodes.Add ( ndbNode );
						treeNode.Nodes.Add ( mrkNode );
						break;

					default:
						throw new NotImplementedException ( navaid.Type + " is not implemented " );
				}
				treeView1.Nodes.Add ( treeNode );
			}
		}

		private void btnOk_Click ( object sender, EventArgs e )
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void btnCancel_Click ( object sender, EventArgs e )
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void treeView1_AfterSelect ( object sender, TreeViewEventArgs e )
		{
			if ( e.Node.Parent != null )
				return;
			int nodeCount = e.Node.Nodes.Count;
			_selectedNavaid = ( Navaid ) e.Node.Tag;
			string text;

			for ( int i = 0; i < _labels.Count; i++ )
			{
				if ( i < nodeCount )
				{
					text = e.Node.Nodes[ i ].Text;
					_labels[ i ].Text = text;
					_labels[ i ].Visible = true;

					SetComboBoxItems ( _comboBoxes[ i ], e.Node.Nodes[ i ], _lnDataDict[ _selectedNavaid.Type.Value ] );
					_cmbBxSelectedIndices[ i ] = _comboBoxes[ i ].SelectedIndex;
				}
				else
				{
					_labels[ i ].Visible = false;
					_comboBoxes[ i ].Visible = false;
				}
			}
			groupBox1.Visible = true;
			groupBox1.Text = _selectedNavaid.Designator;
		}

		private void SetComboBoxItems ( ComboBox comboBox, TreeNode treeNode, List<LineData> lnDataList )
		{
			comboBox.Items.Clear ( );
			NavaidComponent navComponent = ( NavaidComponent ) treeNode.Tag;
			foreach ( LineData lnData in lnDataList )
			{
				comboBox.Items.Add ( lnData.Id );
			}
			comboBox.Tag = navComponent;
			if ( NavComponentPath[ navComponent ] != null )
				comboBox.SelectedItem = _lnDataDict[ _selectedNavaid.Type.Value ].Find ( lnData => lnData.Id == NavComponentPath[ navComponent ].Id ).Id;
			//else
				//comboBox.SelectedIndex = 0;

			comboBox.Visible = true;
		}

		private void btnSave_Click ( object sender, EventArgs e )
		{
			string folderName;
			for ( int i = 0; i < treeView1.SelectedNode.Nodes.Count; i++ )
			{
				NavaidComponent navComponent = ( NavaidComponent ) _comboBoxes[ i ].Tag;
				NavComponentPath[ navComponent ] = _lnDataDict[ _selectedNavaid.Type.Value ].Find ( lnData => lnData.Id == _comboBoxes[ i ].SelectedItem.ToString ( ) );
				_cmbBxSelectedIndices[ i ] = _comboBoxes[ i ].SelectedIndex;
				if ( _selectedNavaid.Type == CodeNavaidService.NDB_MKR )
					folderName = "NDB";
				else
					folderName = _selectedNavaid.Type.Value.ToString ( );
				treeView1.SelectedNode.Nodes[ i ].Nodes[ 0 ].Text = folderName + @"\" + NavComponentPath[ navComponent ].FileName + ".txt";
			}
		}

		private void treeView1_BeforeSelect ( object sender, TreeViewCancelEventArgs e )
		{
			for ( int i = 0; i < e.Node.Nodes.Count; i++ )
			{
				if ( _comboBoxes[ i ].SelectedIndex != _cmbBxSelectedIndices[ i ] )
				{
					if ( MessageBox.Show ( this, "Do you want to save ?", "Save ", MessageBoxButtons.YesNo ) == System.Windows.Forms.DialogResult.Yes )
					{
						btnSave_Click ( null, null );
					}
					break;
				}
			}
		}
	}
}