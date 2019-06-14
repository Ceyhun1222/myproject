using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim.DataTypes;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Metadata.UI;

namespace Aran.Queries.Common
{
    public partial class FeatureSelectorForm : Form
    {
        private IEnumerable<Feature> _featureList;
        private AimPropInfo _propInfo;
        public FillDataGridColumnsHandler DataGridColumnsFilled;
        public SetDataGridRowHandler DataGridRowSetted;
        //public GetFeatureListHandler GetFeatureList;
		public GetFeatureListByDependHandler GetFeatListByDepend;
        public bool _isAbstractFeature;		
        
        public FeatureSelectorForm (AimPropInfo propInfo)
        {
            InitializeComponent ();
            _propInfo = propInfo;
        }

        public FeatureRef SelectedFeatureRef { get; private set; }

        public AimPropInfo PropInfo
        {
            get { return _propInfo; }
        }

		public Dictionary<AimClassInfo, Feature> Features
		{
			get;
			set;
		}

        public IEnumerable<Feature> FeatureList
        {
            get { return _featureList; }
            //set
            //{
            //    _featureList = value;

            //    if (Visible)
            //    {
            //        FillRows ();
            //    }
            //}
        }

        private void FillRows (FeatureType featureType)
        {
			//if (GetFeatureList == null || DataGridRowSetted == null)
			if ( GetFeatListByDepend == null || DataGridRowSetted == null )
				return;

            //_featureList = GetFeatureList (featureType);
			Feature previousFeat;
			if ( IndexCurrRoot == 0 )
			{
				previousFeat = null;
			}
			else
				previousFeat = Features.ElementAt ( IndexCurrRoot - 1 ).Value;
			_featureList = GetFeatListByDepend ( featureType, previousFeat );

            foreach (Feature feature in _featureList)
                DataGridRowSetted (ui_dgv, feature);
        }

        private DataGridViewColumn ToDataGridViewColumn (AimPropInfo propInfo)
        {
            AimObjectType aimObjectType = AimMetadata.GetAimObjectType (propInfo.TypeIndex);

            DataGridViewColumn col = null;

            if (aimObjectType == AimObjectType.Field)
            {
                AimFieldType aimFieldType = (AimFieldType) propInfo.TypeIndex;

                switch (aimFieldType)
                {
                    case AimFieldType.SysBool:
                        DataGridViewCheckBoxColumn chbCol = new DataGridViewCheckBoxColumn ();
                        col = chbCol;
                        break;
                    case AimFieldType.SysString:
                        DataGridViewTextBoxColumn tbCol = new DataGridViewTextBoxColumn ();
                        col = tbCol;
                        break;
                }
            }

            if (col != null)
            {
                col.Name = propInfo.Name;
                col.HeaderText = propInfo.Name;
                col.Tag = propInfo;
            }

            return col;

        }

		private Dictionary<AimClassInfo,Feature> GetFeatures ( AimClassInfo classInfo )
		{
			if ( _uiMetaData == null )
			{
				_uiMetaData = UIMetadata.Instance;
			}
			string dependFeature = classInfo.UiInfo().DependsFeature;
			Dictionary<AimClassInfo, Feature> rootFeats = new Dictionary<AimClassInfo, Feature> ( );
			//FeatureType featType;
			AimClassInfo aimClassInfo = classInfo;
			List<AimClassInfo> aimClassInfoList = new List<AimClassInfo> ( );
			while ( dependFeature != null )
			{
				//Enum.TryParse<FeatureType> ( dependFeature, out featType );
				aimClassInfo = _uiMetaData.ClassInfoList.Where ( cInfo => cInfo.Name == dependFeature ).First ( );
				aimClassInfoList.Add ( aimClassInfo );
				dependFeature = aimClassInfo.UiInfo ( ).DependsFeature;
			}
			aimClassInfoList.Reverse ( );
			foreach ( var item in aimClassInfoList )
			{
				rootFeats.Add ( item, AimObjectFactory.CreateFeature ( ( FeatureType ) item.Index ) );
			}
			rootFeats.Add ( classInfo, AimObjectFactory.CreateFeature ( ( FeatureType ) classInfo.Index ) );
			return rootFeats;
		}

        private void FeatureSelectorForm_Load (object sender, EventArgs e)
        {
            if (DataGridColumnsFilled == null)
                return;

			if ( _propInfo.ReferenceFeature != 0 )
            {
                _isAbstractFeature = false;
                ui_topPanel.Visible = false;
                _mainClassInfo = AimMetadata.GetClassInfoByIndex ((int) _propInfo.ReferenceFeature);
				Features = GetFeatures ( _mainClassInfo );
				IndexCurrRoot = 0;
				AddFeatTypeToComboBox ( Features.ElementAt ( IndexCurrRoot ).Key );
            }
            else
            {
				IndexCurrRoot = 0;
				_isAbstractFeature = true;
                AimClassInfo classInfo = _propInfo.PropType;
                if (classInfo.SubClassType != AimSubClassType.AbstractFeatureRef)
                    return;

                string s = classInfo.Name.Substring ("Abstract".Length);
                s = s.Substring (0, s.Length - "Ref".Length);

                Text = "Select " + s;

                string absEnumTypeName = "Aran.Aim." + s + "Type";
                
                Type enumType = typeof (FeatureType).Assembly.GetType (absEnumTypeName);
                Array enumItemArr = Enum.GetValues (enumType);

                for (int i = 0; i < enumItemArr.Length; i++)
                {
                    int featureTypeIndex = (int) enumItemArr.GetValue (i);
                    AimClassInfo classInfoItem = AimMetadata.GetClassInfoByIndex (featureTypeIndex);
                    ui_featureTypeComboBox.Items.Add (new ComboBoxItem (classInfoItem));
                }
				ui_featureTypeComboBox.SelectedIndex = 0;
			}
		}

        private void dgv_CurrentCellChanged (object sender, EventArgs e)
        {
			if ( Features != null && IndexCurrRoot != Features.Count -1 )
				btnNext.Enabled = ( ui_dgv.CurrentRow != null );
			else
				ui_okButton.Enabled = ( ui_dgv.CurrentRow != null );
        }

        private void ui_featureTypeComboBox_SelectedIndexChanged (object sender, EventArgs e)
        {
            if (ui_featureTypeComboBox.SelectedItem == null)
                return;

            AimClassInfo classInfo = ((ComboBoxItem) ui_featureTypeComboBox.SelectedItem).ClassInfo;
            DataGridColumnsFilled (classInfo, ui_dgv);
            FillRows ((FeatureType) classInfo.Index);
        }

        internal class ComboBoxItem
        {
            public ComboBoxItem (AimClassInfo classInfo)
            {
                ClassInfo = classInfo;
            }

            public override string ToString ()
            {
                return ClassInfo.Name;
            }

            public AimClassInfo ClassInfo { get; private set; }
        }

		private void btnNext_Click ( object sender, EventArgs e )
		{
			Features [ Features.ElementAt ( IndexCurrRoot ).Key ] = ( ( Feature ) ui_dgv.CurrentRow.Tag );
			IndexCurrRoot++;
			AddFeatTypeToComboBox ( Features.ElementAt ( IndexCurrRoot ).Key );
		}

		private void okButton_Click ( object sender, EventArgs e )
		{
			if ( ui_dgv.CurrentRow == null || ui_featureTypeComboBox.SelectedItem == null )
				return;

			if ( _isAbstractFeature )
			{
				IAbstractFeatureRef absFeatRef = ( IAbstractFeatureRef ) AimObjectFactory.CreateADataType (
					( DataType ) PropInfo.TypeIndex );
				absFeatRef.FeatureTypeIndex = ( ( ComboBoxItem ) ui_featureTypeComboBox.SelectedItem ).ClassInfo.Index;
				SelectedFeatureRef = ( FeatureRef ) absFeatRef;
			}
			else
			{
				SelectedFeatureRef = new FeatureRef ( );
			}

			SelectedFeatureRef.Identifier = ( ( Feature ) ui_dgv.CurrentRow.Tag ).Identifier;

			DialogResult = DialogResult.OK;
		}

		private void btnBack_Click ( object sender, EventArgs e )
		{
			IndexCurrRoot--;
			AddFeatTypeToComboBox ( Features.ElementAt ( IndexCurrRoot ).Key );
			//if ( IndexCurrRoot != 0 )
		}

		private void AddFeatTypeToComboBox ( AimClassInfo classInfo )
		{
			ui_featureTypeComboBox.Items.Clear ( );
			//ui_featureTypeComboBox.Items.Add ( new ComboBoxItem ( RootFeatures.ElementAt ( _indexCurrRoot ).Key ) );
			//Text = "Select " + RootFeatures.ElementAt ( _indexCurrRoot ).Key.AixmName;
			ui_featureTypeComboBox.Items.Add ( new ComboBoxItem ( classInfo ) );
			Text = "Select " + classInfo.AixmName;			
			ui_featureTypeComboBox.SelectedIndex = 0;
		}

		private int IndexCurrRoot
		{
			get
			{
				return _indexCurrRoot;
			}
			set
			{
				_indexCurrRoot = value;
				btnBack.Visible = ( _indexCurrRoot > 0 );
				btnNext.Visible = ( Features != null && _indexCurrRoot != Features.Count - 1 );				
				ui_okButton.Visible = !btnNext.Visible;
			}
		}
		private UIMetadata _uiMetaData;
		private AimClassInfo _mainClassInfo;
		private int _indexCurrRoot;
		//private Feature _previousRootFeat;
		//private bool _hasRoot;
		//private Dictionary<FeatureType, Guid> _rootFeats;
	}
}