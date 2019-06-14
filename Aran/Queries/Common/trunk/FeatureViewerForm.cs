using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Aran.Aim;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aixm;
using Aran.Queries.Common;
using Aran.Queries.Common.Properties;
using Aran.AranEnvironment;

namespace Aran.Queries.Viewer
{
    public partial class FeatureViewerForm : Form
    {
        private NodeTag _currentNodeTag;
        private List<Feature> _mainFeatures;
        private EventHandler _backEventHandler;
		private MetadataControl _metadataControl;
        private bool _isValidTimeSelectionVisible;

        public event EventHandler OKClicked;
        public event EventHandler ValidTimeSelected;

        public event GetFeatureHandler GetFeature;
        public event FeatureEventHandler FeatureSaved;
		public event FeatureListByDependEventHandler GetFeatsListByDepend;
        public event FeatureEventHandler OpenedFeature;
        public event ShowGeometryEventHandler ShowGeometryClicked;
        public FillDataGridColumnsHandler DataGridColumnsFilled;
        public SetDataGridRowHandler DataGridRowSetted;
        public FeatureEventHandler GoToFeatureClicked;
		public event FeatureEventHandler StartOfValidChanged;

        public FeatureViewerForm ()
        {
            InitializeComponent ();

            ui_validTimePanel.Visible = false;
            ui_mainContainerPanel.Dock = DockStyle.Fill;
            _mainFeatures = new List<Feature>();
        }

        public void ShowValidTimePanel()
        {
            ui_mainContainerPanel.Visible = false;
            ui_validTimePanel.Visible = true;
            ui_validTimePanel.Dock = DockStyle.Fill;
            IsTimeSliceReadOnly = true;
            _isValidTimeSelectionVisible = true;

            Size = new Size(263, 151);
            StartPosition = FormStartPosition.CenterParent;
        }

        public Feature GetEditingFeature ()
        {
            if (ui_featuresTreeView.Nodes.Count == 0)
                return null;
            NodeTag nodeTag = ui_featuresTreeView.Nodes [0].Tag as NodeTag;
            return nodeTag.Entity as Feature;
        }

        public void SetFeature(params Feature[] features)
        {
            _mainFeatures.Clear();

            foreach (var feat in features)
                _mainFeatures.Add(feat);
            if (!_isValidTimeSelectionVisible)
            {
                foreach (var feat in features)
                    CreateTreeNode(feat, feat.FeatureType.ToString(), false, ui_featuresTreeView.Nodes, null);
            }
        }


        public Feature GetMainFeature ()
        {
            return _mainFeatures[0];
        }

        public Panel MainContainer
        {
            get { return ui_mainContainerPanel; }
        }

        public void HideBottomButtons ()
        {
            bottomButtonContainer.Visible = false;
            ui_scrollableFlow.Top += 25;
            ui_mainSplitContainer.Height += 25;
        }

        public void HideLeftPanel ()
        {
            ui_mainSplitContainer.Panel1Collapsed = true;
        }

        public bool ShowToolbar
        {
            get { return ui_topToolStrip.Visible; }
            set
            {
                ui_topToolStrip.Visible = value;
                if (!value)
                {

                }
            }
        }

        public DateTime DefaultEffectiveDate
        {
            get { return Global.DefaultEffectiveDate; }
            set { Global.DefaultEffectiveDate = value; }
        }

        public bool AsCorrection { get; set; }

        
        public EventHandler BackEventHandler
        {
            get { return _backEventHandler; }
            set
            {
                _backEventHandler = value;
                ui_backTSB.Visible = (value != null);
            }
        }

		public GetFeatureListHandler GetFeatureListHandler
		{
			get { return Global.GetFeatureList; }
			set { Global.GetFeatureList = value; }
		}

        public AiracDateTime SelectedValidTime
        {
            get { return ui_airacCycleCont.AiracDateTime; }
            set { ui_airacCycleCont.AiracDateTime = value; }
        }

        public bool IsTimeSliceReadOnly { get; private set; }

        public void ApplyAllIsVisible() 
        {
            btnApplyAll.Visible = true;
        }

        
        private bool DeserializeFeature (Feature feature, string fileName)
        {
            XmlReader reader = XmlReader.Create (fileName);

            bool isEmpty = true;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    isEmpty = false;
                    break;
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                    break;
            }

            if (isEmpty)
                return false;

            string xmlText = reader.ReadOuterXml ();
            XmlDocument xmlDoc = new XmlDocument ();
            xmlDoc.LoadXml (xmlText);

            XmlElement xmlElement = xmlDoc.DocumentElement;// ["aixm-5.1:" + feature.FeatureType + "TimeSlice"];

            if (xmlElement == null)
                return false;

            XmlContext xmlContext = new XmlContext (xmlElement);
            return (feature as IAixmSerializable).AixmDeserialize (xmlContext);
        }

        private TreeNode CreateTreeNode (
                            DBEntity entity,
                            string nodeText,
                            bool showType,
                            TreeNodeCollection nodes,
                            TreeNode parentNode,
                            int listIndex = -1)
        {
            IAimObject aimObject = entity;

            AimPropInfo [] propInfoArr = null;

            if (AimMetadata.IsChoice (entity))
            {
                #region Choice Class

                propInfoArr = AimMetadata.GetAimPropInfos (aimObject);

                IEditChoiceClass choiceClass = (IEditChoiceClass) entity;
                AimObjectType aot = AimMetadata.GetAimObjectType (choiceClass.RefType);
                if (aot == AimObjectType.Feature)
                {
                    Feature feature = null;

                    AimPropInfo api = propInfoArr.Where (
                        pi => pi.IsFeatureReference && pi.ReferenceFeature == (FeatureType) choiceClass.RefType).First ();

                    string newNodeText = nodeText;
                    if (!nodeText.StartsWith ("item - "))
                        newNodeText += "_" + api.AixmName;

                    FeatureRef featureRef = choiceClass.RefValue as FeatureRef;
                    feature = OnGetFeature ((FeatureType) choiceClass.RefType, featureRef.Identifier);

                    if (feature != null)
                    {
                        TreeNode choiceNode = CreateTreeNode (feature, newNodeText, true, nodes, parentNode);
                        ((NodeTag) choiceNode.Tag).ChoiceClass = (ChoiceClass) entity;
                        return choiceNode;
                    }
                }
                else if (aot == AimObjectType.Object)
                {
                    AimPropInfo api = propInfoArr.Where (pi => pi.TypeIndex == choiceClass.RefType).First ();

                    string newNodeText = nodeText;
                    if (!nodeText.StartsWith ("item - "))
                        newNodeText += "_" + api.AixmName;

                    AObject aObj = (AObject) choiceClass.RefValue;
                    return CreateTreeNode (aObj, newNodeText, true, nodes, parentNode);
                }
                else if (aot == AimObjectType.DataType) //--- Is AbstractFeatureRef
                {
                    AimPropInfo api = propInfoArr.Where (pi => pi.TypeIndex == choiceClass.RefType).First ();
                    string newNodeText = nodeText;
                    if (!nodeText.StartsWith ("item - "))
                        newNodeText += "_" + api.AixmName;
                    
                    IAbstractFeatureRef afr = choiceClass.RefValue as IAbstractFeatureRef;
                    Feature feature = OnGetFeature ((FeatureType) afr.FeatureTypeIndex, afr.Identifier);

                    if (feature != null)
                    {
                        TreeNode choiceNode = CreateTreeNode (feature, newNodeText, true, nodes, parentNode);
                        ((NodeTag) choiceNode.Tag).ChoiceClass = (ChoiceClass) entity;
                        return choiceNode;
                    }
                }
                else
                    throw new Exception ("Aim Metadata is not correct");

                #endregion
            }

            NodeTag nodeTag = new NodeTag ();
            nodeTag.Entity = entity;
            nodeTag.Name = nodeText;
            nodeTag.Index = listIndex;
            nodeTag.ShowType = showType;
            nodeTag.TreeNode = new TreeNode ();

            if (aimObject.AimObjectType == AimObjectType.Feature)
            {
                Feature feature = (Feature) entity;

                if (FeatureAddedToParentNode (parentNode, feature))
                {
                    TreeNode shortcutNode = new TreeNode ();
                    shortcutNode.Text = "Shortcut to " + feature.FeatureType;
                    
                    NodeTag nt = new NodeTag ();
                    nt.ShortcutFeature = feature;
                    nt.TreeNode = new TreeNode ();

                    shortcutNode.Tag = nt;
                    shortcutNode.ImageIndex = 6;
                    shortcutNode.SelectedImageIndex = shortcutNode.ImageIndex;
                    nodes.Add (shortcutNode);
                    
                    return shortcutNode;
                }

                nodeTag.TreeNode.ImageIndex = (nodeTag.IsNewFeature ? 0 : 1);

            }
            else if (aimObject.AimObjectType == AimObjectType.Object)
            {
                nodeTag.TreeNode.ImageIndex = 2;
            }

            nodes.Add (nodeTag.TreeNode);
            ui_featuresTreeView.SelectedNode = nodeTag.TreeNode;

            nodeTag.TreeNode.SelectedImageIndex = nodeTag.TreeNode.ImageIndex;
            return nodeTag.TreeNode;
        }

        private bool FeatureAddedToParentNode (TreeNode treeNode, Feature feature)
        {
            if (treeNode == null)
                return false;

            NodeTag nodeTag = (NodeTag) treeNode.Tag;
            if (nodeTag.Entity != null && nodeTag.Entity.Equals (feature))
                return true;

            return FeatureAddedToParentNode (treeNode.Parent, feature);

            //foreach (TreeNode tn in nodes)
            //{
            //    NodeTag nodeTag = (NodeTag) tn.Tag;
            //    if (nodeTag != null && nodeTag.Entity != null && nodeTag.Entity.Equals (feature))
            //        return true;

            //    if (FeatureAddedNode (tn.Nodes, feature))
            //        return true;
            //}
            //return false;
        }

        private Feature OnGetFeature (FeatureType featureType, Guid identifier)
        {
            if (GetFeature == null)
                return null;

            return GetFeature (featureType, identifier);
        }

        private void featuresTreeView_AfterSelect (object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                foreach (Control detailsPanel in featureDetailsPanel.Controls)
                {
                    detailsPanel.Visible = false;
                }

                ui_scrollableFlow.Flow.Controls.Clear ();

                return;
            }

            ShowHeararchy (e.Node);

            NodeTag nodeTag = e.Node.Tag as NodeTag;

            if (nodeTag.IsShortcut)
            {
                return;
            }

            if (_currentNodeTag != null)
            {
                if (_currentNodeTag == nodeTag)
                    return;

                if (_currentNodeTag.Panel != null)
                    _currentNodeTag.Panel.Visible = false;
            }

            if (nodeTag.Panel == null)
            {
                IAimObject aimObject = nodeTag.Entity as IAimObject;
                if (aimObject.AimObjectType == AimObjectType.Feature)
                {
                    nodeTag.Panel = FindFeaturePanelInTree (ui_featuresTreeView.Nodes, nodeTag.Entity);
                }

                if (nodeTag.Panel == null)
                {
                    LoadAttrubutesPanel (nodeTag, (e.Node.Parent == null));
                }
            }

            nodeTag.Panel.Visible = true;
            _currentNodeTag = nodeTag;

            ui_gotoFeatureTSB.Visible = (e.Node.Parent != null &&
                ((nodeTag.Entity as IAimObject).AimObjectType == AimObjectType.Feature) &&
                (GoToFeatureClicked != null));

            ui_applyTSB.Enabled = ((nodeTag.Entity is AObject) || (nodeTag.Entity.Id == -1));
        }

        private void ShowHeararchy (TreeNode treeNode)
        {
            ui_scrollableFlow.Flow.Controls.Clear ();
            List<NodeTag> nodeTagList = new List<NodeTag> ();

            TreeNode tn = treeNode;

            while (tn != null)
            {
                nodeTagList.Add ((NodeTag) tn.Tag);
                tn = tn.Parent;
            }

            Graphics gr = ui_scrollableFlow.Flow.CreateGraphics ();

            for (int i = nodeTagList.Count - 1; i >= 0; i--)
            {
                NodeTag nodeTag = nodeTagList [i];

                Button button = new Button ();
                button.FlatStyle = FlatStyle.Popup;
                button.Text = AimMetadata.GetAimTypeName (nodeTag.Entity);
                
                if (button.Text == "AimxPoint")
                    button.Text = "Point";

                button.Tag = nodeTag;

				if (i < nodeTagList.Count - 1)
				{
					var cms = new ContextMenuStrip ();
					var tsi = new ToolStripMenuItem ();
					tsi.Text = "Set <NULL>";
					tsi.Click += HeararchyButtonSetNull_Click;
					tsi.Tag = button;
					cms.Items.Add (tsi);

					button.ContextMenuStrip = cms;
				}

                button.Click += HeararchyButton_Click;

                SizeF sizeF = gr.MeasureString (button.Text, button.Font);
                button.Width = (int) sizeF.Width + 15;

                ui_scrollableFlow.Flow.Controls.Add (button);

                if (i > 0)
                {
                    PictureBox pb = new PictureBox ();
                    pb.Size = new Size (22, 22);
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    ui_scrollableFlow.Flow.Controls.Add (pb);

                    pb.Image = ((nodeTagList [i - 1].Entity as IAimObject).AimObjectType == AimObjectType.Feature ?
                        Resources.arrow_right_feature : Resources.arrow_right);
                }
            }

            gr.Dispose ();
        }

		private void HeararchyButtonSetNull_Click (object sender, EventArgs e)
		{
			clearToolStripMenuItem_Click (null, null);
			return;
		}

        private void HeararchyButton_Click (object sender, EventArgs e)
        {
            NodeTag nodeTag = (NodeTag) ((Control) sender).Tag;

            TreeNode tn = ui_featuresTreeView.SelectedNode;

            while (tn != null)
            {
                if (tn.Tag.Equals (nodeTag))
                {
                    ui_featuresTreeView.SelectedNode = tn;
                    break;
                }
                tn = tn.Parent;
            }

        }

        private void LoadAttrubutesPanel (NodeTag nodeTag, bool isRoot)
        {
            IAimObject aimObject = nodeTag.Entity as IAimObject;

            Panel detailsPanel = new Panel ();
            detailsPanel.Parent = featureDetailsPanel;
            detailsPanel.Dock = DockStyle.Fill;
            
            bool isCRCExtension = (nodeTag.Entity is ICRCExtension);
            bool isReadOnly = ((!isRoot && nodeTag.IsFeature && !nodeTag.IsNewFeature)|| isCRCExtension);

            #region Create TabControl

            TabControl tabControl = new TabControl ();
            tabControl.Dock = DockStyle.Fill;

            #region TimeSlice Page

            TabPage timeSliceTabPage = null;
            Panel timeSlicePanel = null;

            if (nodeTag.IsFeature && ((Feature) nodeTag.Entity).TimeSlice != null)
            {
                timeSliceTabPage = new TabPage ();
                timeSliceTabPage.Text = "TimeSlice";
                tabControl.TabPages.Add (timeSliceTabPage);

                timeSlicePanel = new Panel ();
                timeSlicePanel.BorderStyle = BorderStyle.Fixed3D;
                timeSlicePanel.AutoScroll = true;
                timeSlicePanel.Parent = timeSliceTabPage;
                timeSlicePanel.Dock = DockStyle.Fill;
            }

            #endregion

            #region Attributes Page

            TabPage attributesTabPage = new TabPage ();
            attributesTabPage.Text = "Attributes";
            tabControl.TabPages.Add (attributesTabPage);

            Panel attributesPanel = new Panel ();
            attributesPanel.BorderStyle = BorderStyle.Fixed3D;
            attributesPanel.AutoScroll = true;
            attributesPanel.Parent = attributesTabPage;
            attributesPanel.Dock = DockStyle.Fill;

            FlowLayoutPanel attributesFlow = new FlowLayoutPanel ();
            attributesFlow.Location = new Point (3, 10);
            attributesFlow.AutoSize = true;
            attributesFlow.WrapContents = false;
            attributesFlow.FlowDirection = FlowDirection.TopDown;

            #endregion

            #region Complex Page

            //bool showComplexPage = (!nodeTag.IsFeature || nodeTag.Parent == null);
            bool showComplexPage = true;
            FlowLayoutPanel complexFlow = null;
            Panel complexPropPanel = null;

            //if (showComplexPage)
            //{
            //    TabPage complexPropsTabPage = new TabPage ();
            //    complexPropsTabPage.Text = "Complex Properties";
            //    tabControl.TabPages.Add (complexPropsTabPage);

            complexPropPanel = new Panel ();
            complexPropPanel.AutoSize = true;
            //    complexPropPanel.BorderStyle = BorderStyle.Fixed3D;
            //    complexPropsTabPage.Controls.Add (complexPropPanel);
            //    complexPropPanel.Dock = DockStyle.Fill;
            //    complexPropPanel.AutoScroll = true;

            complexFlow = new FlowLayoutPanel ();
            complexFlow.WrapContents = false;
            complexFlow.FlowDirection = FlowDirection.TopDown;
            complexFlow.AutoSize = true;
            //}

            #endregion

            #region Metadata Page
			if (nodeTag.Entity is Feature)
			//if (nodeTag.IsFeature && ((Feature) nodeTag.Entity).TimeSliceMetadata != null)
			{
                TabPage metadataTabPage = new TabPage ();
                metadataTabPage.Text = "Metadata";
                tabControl.TabPages.Add (metadataTabPage);

                MetadataControl mdControl = new MetadataControl ();
                mdControl.Dock = DockStyle.Fill;
                mdControl.Metadata = ((Feature) nodeTag.Entity).TimeSliceMetadata;
                metadataTabPage.Controls.Add (mdControl);

				_metadataControl = mdControl;
			}
            #endregion

			tabControl.SelectedTab = timeSliceTabPage;

            #endregion

            #region Attributes

            AimPropInfo [] aimPropInfoArr = AimMetadata.GetAimPropInfos (nodeTag.Entity);
            foreach (AimPropInfo aimPropInfo in aimPropInfoArr)
            {
				// test 
                if (aimPropInfo.Name == "Id" || aimPropInfo.Name == "Identifier")
                    continue;

                IAimProperty aimPropValue = aimObject.GetValue (aimPropInfo.Index);

                PropControlTag propContTag = new PropControlTag
                {
                    AimObject = aimObject,
                    PropInfo = aimPropInfo,
                    PropValue = aimPropValue
                };

                if (aimPropInfo.TypeIndex == (int) DataType.TimeSlice)
                {
                    TimeSlice ts = (TimeSlice) propContTag.PropValue;
                    if (ts != null)
                    {
                        

                        var tsc = new TimeSliceControl();
                        tsc.Location = new Point (5, 20);
                        tsc.IsNewFeature = (aimObject is DBEntity && (aimObject as DBEntity).Id < 1);
                        tsc.AsCorrection = AsCorrection;
                        tsc.TimeSlice = ts;
                        tsc.Tag = propContTag;
                        tsc.ReadOnly = isReadOnly || IsTimeSliceReadOnly;
                        timeSlicePanel.Controls.Add (tsc);
                    }
                }
                else
                {
                    if (PropControl.IsSupported (aimPropInfo))
                    {
                        PropControl propControl = new PropControl ();
                        propControl.PropertyTag = propContTag;
                        propControl.ReadOnly = isReadOnly;
                        propControl.Location = new Point (2, 2);
                        propControl.ShowGeometryClicked += PropControl_ShowGeometryClicked;
                        attributesFlow.Controls.Add (propControl);
                    }
                    else if (aimPropValue != null && showComplexPage)
                    {
                        switch (aimPropValue.PropertyType)
                        {
                            case AimPropertyType.Object:
                            case AimPropertyType.DataType:
                                {
                                    if (aimPropValue.PropertyType == AimPropertyType.DataType &&
                                        !aimPropInfo.IsFeatureReference)
                                    {
                                        break;
                                    }

                                    ComplexPropControl cpc = new ComplexPropControl ();
                                    cpc.Value = aimPropValue;
                                    cpc.PropInfo = aimPropInfo;
                                    cpc.EntityClicked += new EntityClickedHandler (PropCont_EntityClicked);
                                    cpc.GetFeature += new GetFeatureHandler (OnGetFeature);
                                    complexFlow.Controls.Add (cpc);
                                }
                                break;
                            case AimPropertyType.List:
                                {
                                    #region List

                                    IList objList = aimPropValue as IList;

                                    if (objList.Count > 0)
                                    {
                                        ListPropControl lpc = new ListPropControl ();
                                        lpc.FeatureSelected += new FeatureSelectedEventHandner (SelectFeatureRef);
                                        lpc.PropInfo = aimPropInfo;
                                        lpc.Value = objList;
                                        lpc.EntityClicked += new EntityClickedHandler (PropCont_EntityClicked);
                                        complexFlow.Controls.Add (lpc);
                                    }

                                    #endregion
                                }
                                break;
                        }
                    }
                }
            }

			#endregion

			//if (attributesFlow.Controls.Count > 0)
			//{
			attributesFlow.Parent = attributesPanel;
			//}
			//else
			//{
			//    tabControl.TabPages.Remove (attributesTabPage);
			//}

			if (showComplexPage)
            {
                complexFlow.Parent = complexPropPanel;
                complexFlow.Location = new Point (4, 4);
                complexFlow.Size = new Size (2, 2);
            }

            #region Add new "AddNewItem" button
            Button addNewItemButton = new Button ();
            addNewItemButton.Size = new Size (32, 32);
            addNewItemButton.FlatStyle = FlatStyle.Popup;
            addNewItemButton.Image = Resources.add_new;
            addNewItemButton.Click += new EventHandler (addNewItemButton_Click);
            
            if (showComplexPage)
                complexPropPanel.Controls.Add (addNewItemButton);

            toolTip1.SetToolTip (addNewItemButton, "Add new complex property");
            #endregion

            nodeTag.Panel = detailsPanel;
            nodeTag.ComplexFlowLayoutPanel = complexFlow;

            if (showComplexPage)
            {
                complexFlow.Tag = addNewItemButton;
                complexFlow.SizeChanged += new EventHandler (complexFlp_SizeChanged);
                complexFlp_SizeChanged (complexFlow, null);
            }

            attributesFlow.Controls.Add (complexPropPanel);

            detailsPanel.Controls.Add (tabControl);
        }

        private void PropControl_ShowGeometryClicked (object sender, EventArgs e)
        {
            if (ShowGeometryClicked != null)
            {
                var s = string.Empty;
                foreach (Control cont in ui_scrollableFlow.Flow.Controls)
                {
                    if (cont is Button)
                        s += cont.Text;
                    else if (cont is PictureBox)
                        s += " -> ";
                }

                var propCont = sender as PropControl;

                var ge = new ShowGeometryEventArgs ();
				ge.FeatureType = _mainFeatures[0].FeatureType;
                ge.ReplacedControl = ui_mainContainerPanel;
                ge.PathText = s;

                var eaf = propCont.PropertyTag.PropValue as IEditAimField;
                ge.GeomValue = eaf.FieldValue as Geometries.Geometry;

                ShowGeometryClicked (this, ge);
            }
        }

        private void complexFlp_SizeChanged (object sender, EventArgs e)
        {
            Control addNewItemControl = ((Control) sender).Tag as Control;
            addNewItemControl.Location = new Point (8, ((Control) sender).Bottom + 4);
        }

        private void addNewItemButton_Click (object sender, EventArgs e)
        {
            addNewCompPropsContextMenu.Items.Clear ();

            TreeNode treeNode = ui_featuresTreeView.SelectedNode;
            if (treeNode == null)
                return;

            NodeTag nodeTag = (NodeTag) treeNode.Tag;
            IAimObject aimObject = nodeTag.Entity;
            FlowLayoutPanel complexFlp = nodeTag.ComplexFlowLayoutPanel;

            AimPropInfo [] aimPropInfoArr = AimMetadata.GetAimPropInfos (aimObject);
            foreach (AimPropInfo aimPropInfo in aimPropInfoArr)
            {
                IAimProperty aimPropVal = aimObject.GetValue (aimPropInfo.Index);
                if (aimPropVal != null)
                {
                    if (aimPropVal.PropertyType == AimPropertyType.List)
                    {
                        IList list = aimPropVal as IList;
                        if (list.Count > 0)
                            continue;
                    }
                    else
                        continue;
                }


                AimObjectType propAimObjectType = AimMetadata.GetAimObjectType (aimPropInfo.TypeIndex);

                ToolStripMenuItem newToolStripItem = null;

                if ((aimPropInfo.TypeCharacter & PropertyTypeCharacter.List) == PropertyTypeCharacter.List)
                {
                    newToolStripItem = new ToolStripMenuItem ();
                    newToolStripItem.Text = aimPropInfo.AixmName + "  (List)";
                }
                else
                {
                    switch (propAimObjectType)
                    {
                        case AimObjectType.Object:
                        case AimObjectType.DataType:
                            {
                                if (propAimObjectType == AimObjectType.DataType && !aimPropInfo.IsFeatureReference)
                                    break;

                                newToolStripItem = new ToolStripMenuItem ();
                                newToolStripItem.Text = aimPropInfo.AixmName;

                                if (aimPropInfo.PropType.SubClassType == AimSubClassType.Choice)
                                {
                                    foreach (AimPropInfo subPropInfo in aimPropInfo.PropType.Properties)
                                    {
                                        if (subPropInfo.AixmName != string.Empty)
                                        {
                                            var subNewToolStripItem = new ToolStripMenuItem ();
                                            subNewToolStripItem.Text = subPropInfo.AixmName;
                                            subNewToolStripItem.Click += newToolStripItem_Click;
                                            newToolStripItem.DropDownItems.Add(subNewToolStripItem);

                                            var tsTag = new ToolStripTag ();
                                            tsTag.IsChoice = true;
                                            tsTag.NodeTag = nodeTag;
                                            tsTag.PropInfo = subPropInfo;
                                            subNewToolStripItem.Tag = tsTag;
                                        }
                                    }
                                }
                                else if (aimPropInfo.PropType.IsAbstract) {
                                    var childClassInfoList = Aim.Utilities.AimMetadataUtility.GetAbstractChilds(aimPropInfo.PropType);
                                    foreach (var childClassInfo in childClassInfoList) {
                                        var subNewToolStripItem = new ToolStripMenuItem();
                                        subNewToolStripItem.Text = "By " + childClassInfo.AixmName;
                                        subNewToolStripItem.Click += newToolStripItem_Click;
                                        newToolStripItem.DropDownItems.Add(subNewToolStripItem);

                                        var tsTag = new ToolStripTag();
                                        tsTag.NodeTag = nodeTag;
                                        tsTag.PropInfo = aimPropInfo.Clone();
                                        tsTag.PropInfo.PropType = childClassInfo;
                                        tsTag.PropInfo.TypeIndex = childClassInfo.Index;
                                        subNewToolStripItem.Tag = tsTag;
                                    }
                                }
                            }
                            break;
                    }
                }

                if (newToolStripItem != null)
                {
                    var tsTag = new ToolStripTag ();
                    tsTag.NodeTag = nodeTag;
                    tsTag.PropInfo = aimPropInfo;
                    newToolStripItem.Tag = tsTag;
                    addNewCompPropsContextMenu.Items.Add (newToolStripItem);

                    if (!newToolStripItem.HasDropDownItems)
                        newToolStripItem.Click += new EventHandler (newToolStripItem_Click);
                }
            }

            addNewCompPropsContextMenu.Show ((Control) sender, 5, 5);
        }

        private void newToolStripItem_Click (object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem) sender;
            ToolStripTag tsTag = (ToolStripTag) tsmi.Tag;
            NodeTag nodeTag = tsTag.NodeTag;
            IAimObject aimObject = nodeTag.Entity;

            if (tsTag.PropInfo.IsList)
            {
                FeatureRef featureRef = null;

                if (tsTag.PropInfo.IsFeatureReference)
                {
                    featureRef = SelectFeatureRef (tsTag.PropInfo);
                    if (featureRef == null)
                        return;
                }

                IList listValue = aimObject.GetValue (tsTag.PropInfo.Index) as IList;
                if (listValue == null)
                {
                    listValue = AimObjectFactory.CreateList (tsTag.PropInfo.TypeIndex);
                    aimObject.SetValue (tsTag.PropInfo.Index, listValue as IAimProperty);
                }
                AimObject newItem = AimObjectFactory.Create (tsTag.PropInfo.TypeIndex);
                listValue.Add (newItem);

                if (featureRef != null)
                {
                    ((FeatureRefObject) newItem).Feature = featureRef;
                }

                ListPropControl lpc = new ListPropControl ();
                lpc.FeatureSelected += new FeatureSelectedEventHandner (SelectFeatureRef);
                lpc.PropInfo = tsTag.PropInfo;
                lpc.Value = listValue;
                lpc.EntityClicked += new EntityClickedHandler (PropCont_EntityClicked);
                nodeTag.ComplexFlowLayoutPanel.Controls.Add (lpc);
            }
            else
            {
                AimObject newObjectValue;

                if (tsTag.PropInfo.IsFeatureReference)
                {
                    newObjectValue = SelectFeatureRef (tsTag.PropInfo);
                    if (newObjectValue == null)
                        return;
                }
                else
                {
                    newObjectValue = AimObjectFactory.Create(tsTag.PropInfo.TypeIndex);
                }

                AimPropInfo propControlPropInfo = tsTag.PropInfo;

                if (tsTag.IsChoice) //--- Is Choice
                {
                    ToolStripTag choiceTsTag = (ToolStripTag) tsmi.OwnerItem.Tag;
                    propControlPropInfo = choiceTsTag.PropInfo;

                    IEditChoiceClass newChoiceObject = (IEditChoiceClass) AimObjectFactory.CreateAObject (
                        (ObjectType) choiceTsTag.PropInfo.TypeIndex);

                    newChoiceObject.RefValue = newObjectValue as IAimProperty;

                    if (tsTag.PropInfo.IsFeatureReference)
                    {
                        if (tsTag.PropInfo.ReferenceFeature != 0)
                            newChoiceObject.RefType = (int) tsTag.PropInfo.ReferenceFeature;
                        else
                            newChoiceObject.RefType = (int) tsTag.PropInfo.TypeIndex;
                    }
                    else
                    {
                        newChoiceObject.RefType = (int) ((AObject) newObjectValue).ObjectType;
                    }
                    newObjectValue = newChoiceObject as AimObject;
                }

                aimObject.SetValue (propControlPropInfo.Index, newObjectValue as IAimProperty);

                ComplexPropControl cpc = new ComplexPropControl ();
                cpc.Value = newObjectValue as IAimProperty;
                cpc.PropInfo = propControlPropInfo;
                cpc.EntityClicked += new EntityClickedHandler (PropCont_EntityClicked);
                cpc.GetFeature += new GetFeatureHandler (OnGetFeature);
                nodeTag.ComplexFlowLayoutPanel.Controls.Add (cpc);
            }
        }

        private FeatureRef SelectFeatureRef (AimPropInfo propInfo)
        {
            FeatureSelectorForm fsf = new FeatureSelectorForm (propInfo);
            //fsf.DataGridColumnsFilled = DataGridColumnsFilled;
            fsf.DataGridRowSetted = DataGridRowSetted;
			fsf.GetFeatListByDepend = GetFeatsListByDepend;
            fsf.EffectiveDate = GetEditedValidTime();

            if (fsf.ShowDialog (this) == DialogResult.OK)
                return fsf.SelectedFeatureRef;

            return null;
        }

        private void PropCont_EntityClicked (object sender, DBEntity entity, string nodeText, bool showType, int listIndex = -1)
        {
            TreeNode selNode = ui_featuresTreeView.SelectedNode;
            if (selNode == null)
                return;

            ADataType featureRef = null;
            DBEntityClaredHandler dbEntityClearedHandle;

            if (sender is ComplexPropControl)
            {
                ComplexPropControl cpc = (ComplexPropControl) sender;
                if (cpc.Value.PropertyType == AimPropertyType.DataType)
                {
                    featureRef = (ADataType) cpc.Value;
                }
                dbEntityClearedHandle = new DBEntityClaredHandler (cpc.NodeTag_DBEntityCleared);
            }
            else
            {
                if (entity is FeatureRefObject)
                {
                    FeatureRef featRef = ((FeatureRefObject) entity).Feature;
                    featureRef = featRef;

                    Feature feature = GetFeature (((ListPropControl) sender).PropInfo.ReferenceFeature, featRef.Identifier);
                    entity = feature;
                }

                dbEntityClearedHandle = new DBEntityClaredHandler ((sender as ListPropControl).NodeTag_DBEntityCleared);
            }

            foreach (TreeNode tn in selNode.Nodes)
            {
                NodeTag tag = (NodeTag) tn.Tag;
                if (tag.IsBaseOn (entity) || (featureRef != null && featureRef.Equals (tag.FeatureRef)))
                {
                    ui_featuresTreeView.SelectedNode = tn;
                    return;
                }
            }

            TreeNode tNode = selNode.Parent;
            while (tNode != null)
            {
                NodeTag tag = (NodeTag) tNode.Tag;
                if (tag.IsBaseOn (entity) || (featureRef != null && featureRef.Equals (tag.FeatureRef)))
                {
                    ui_featuresTreeView.SelectedNode = tNode;
                    return;
                }

                tNode = tNode.Parent;
            }

            TreeNode newTreeNode = CreateTreeNode (entity, nodeText, showType, selNode.Nodes, selNode, listIndex);
            NodeTag newNodeTag = (NodeTag) newTreeNode.Tag;
            newNodeTag.FeatureRef = featureRef;
            newNodeTag.DBEntityClared += dbEntityClearedHandle;
        }

        private Panel FindFeaturePanelInTree (TreeNodeCollection treeNodeColl, DBEntity entity)
        {
            foreach (TreeNode tn in treeNodeColl)
            {
                NodeTag nodeTag = tn.Tag as NodeTag;
                if (nodeTag.Entity == entity)
                {
                    return nodeTag.Panel;
                }

                Panel p = FindFeaturePanelInTree (tn.Nodes, entity);
                if (p != null)
                    return p;
            }
            return null;
        }

        private Control CreatePropertyControl (PropControlTag propContTag, bool readOnly)
        {
            if (propContTag.PropInfo.Name == "Id" ||
                propContTag.PropInfo.Name == "Identifier")
                return null;

            try
            {
                //if (propContTag.PropInfo.TypeIndex == (int) DataType.TimeSlice)
                //{
                //    var ts = (TimeSlice) propContTag.PropValue;
                //    var tsc = new TimeSliceControlV2 ();
                //    tsc.TimeSlice = ts;
                //    tsc.Tag = propContTag;
                //    tsc.ReadOnly = readOnly;
                //    tsc.StartOfValidChanged += TimeSliceControl_StartOfValidChanged;
                //    return tsc;
                //}

                PropControl propControl = new PropControl ();
                propControl.PropertyTag = propContTag;
                propControl.ReadOnly = readOnly;
                return propControl;
            }
            catch
            {
                return null;
            }
        }

		//private void SetStartOfValideNodeColl(TreeNodeCollection tnColl, DateTime startOfValidTime)
		//{
		//    foreach (TreeNode tn in tnColl)
		//    {
		//        var nodeTag = tn.Tag as NodeTag;
		//        (nodeTag.Entity as Feature).TimeSlice.ValidTime.BeginPosition = startOfValidTime;
				

		//        SetStartOfValideNodeColl (tn.Nodes, startOfValidTime);
		//    }
		//}

        private void saveButton_Click (object sender, EventArgs e)
        {
            TreeNode treeNode = ui_featuresTreeView.SelectedNode;
            if (treeNode == null)
                return;
            NodeTag nodeTag = (NodeTag)treeNode.Tag;
			
			// Just to activate TextBoxs to validate themself
			ui_featuresTreeView.Select ( );

            if (ui_featuresTreeView.Nodes.Count == 0)
                return;
            
            //NodeTag nodeTag = (NodeTag) ui_featuresTreeView.Nodes [0].Tag;

            if (nodeTag.Entity.Id == -1)
            {
                nodeTag.Save();

                if (nodeTag.IsFeature && FeatureSaved != null)
                    FeatureSaved(this, new FeatureEventArgs((Feature)nodeTag.Entity));
            }
        }

        private void cancel2Button_Click (object sender, EventArgs e)
        {
            TreeNode selNode = ui_featuresTreeView.SelectedNode;
            if (selNode == null)
                return;

            NodeTag nodeTag = (NodeTag) selNode.Tag;

            if (selNode.Parent == null)
            {
                if (nodeTag.IsChanged)
                {
                    if (MessageBox.Show ("Feature has changed!\nDo you want to save?", "", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        nodeTag.Save ();
                    }
                }

                if (!Visible)
                {
                    base.OnFormClosed (new FormClosedEventArgs (CloseReason.None));
                }
                else
                    Close ();
                return;
            }
            
            ui_scrollableFlow.Flow.Controls.RemoveAt (ui_scrollableFlow.Flow.Controls.Count - 1);
            selNode.Parent.Nodes.Remove (selNode);
        }

        private void OK_Cancel_Button_Click (object sender, EventArgs e)
        {
            if (sender.Equals (cancelButton))
            {
                DialogResult = DialogResult.Cancel;

                if (OKClicked != null)
                    Close ();

                return;
            }

            if (ui_featuresTreeView.Nodes.Count == 0)
                return;

            foreach (var node in ui_featuresTreeView.Nodes)
            {
                NodeTag nodeTag = (NodeTag) ((node as TreeNode).Tag);

                if (nodeTag.Entity is Feature && _metadataControl != null)
                {
                    var feat = nodeTag.Entity as Feature;
                    feat.TimeSliceMetadata = _metadataControl.Metadata;
                }

                nodeTag.Save();
            }
            

            if (OKClicked == null)
                DialogResult = DialogResult.OK;
            else
                OKClicked (this, e);
        }

        private DateTime? GetEditedValidTime()
        {
            foreach (var node in ui_featuresTreeView.Nodes)
            {
                NodeTag nodeTag = (NodeTag) ((node as TreeNode).Tag);

                if (nodeTag.Entity is Feature editedRootFeat)
                    return editedRootFeat.TimeSlice.ValidTime.BeginPosition;
            }
            return null;
        }

        private void featuresTreeView_NodeMouseClick (object sender, TreeNodeMouseClickEventArgs e)
        {
            ui_featuresTreeView.ContextMenuStrip = setDBEntityNullContextMenu;
        }

        private void featuresTreeView_MouseDown (object sender, MouseEventArgs e)
        {
            ui_featuresTreeView.ContextMenuStrip = null;
        }

        private void clearToolStripMenuItem_Click (object sender, EventArgs e)
        {
            TreeNode treeNode = ui_featuresTreeView.SelectedNode;
            if (treeNode == null)
                return;

            NodeTag nodeTag = (NodeTag) treeNode.Tag;
            if (!nodeTag.ClearDBEntity ())
            {
                return;
            }

            treeNode.Remove ();
        }

        private void ui_backTSB_Click (object sender, EventArgs e)
        {
            if (_backEventHandler != null)
                _backEventHandler (sender, e);
        }

        private void openFeatureToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (OpenedFeature != null)
            {
                NodeTag nodeTag = ui_featuresTreeView.SelectedNode.Tag as NodeTag;
                OpenedFeature (this, new FeatureEventArgs (nodeTag.Entity as Feature));
            }
        }

        private void setDBEntityNullContextMenu_Opening (object sender, CancelEventArgs e)
        {
            openFeatureToolStripMenuItem.Visible = false;

            if (OpenedFeature == null ||
                ui_featuresTreeView.SelectedNode == null ||
                (ui_featuresTreeView.Nodes.Count > 0) && ui_featuresTreeView.Nodes [0] == ui_featuresTreeView.SelectedNode)
            {
                return;
            }
            NodeTag nodeTag = ui_featuresTreeView.SelectedNode.Tag as NodeTag;
            if (nodeTag.IsFeature)
                openFeatureToolStripMenuItem.Visible = true;
        }

        private void ui_gotoFeatureTSB_Click (object sender, EventArgs e)
        {
            if (GoToFeatureClicked == null ||
                ui_featuresTreeView.SelectedNode == null ||
                !(ui_featuresTreeView.SelectedNode.Tag is NodeTag))
            {
                return;
            }

            NodeTag nodeTag = ui_featuresTreeView.SelectedNode.Tag as NodeTag;
            if (!(nodeTag.Entity is Feature))
                return;

            GoToFeatureClicked (this, new FeatureEventArgs (nodeTag.Entity as Feature));
        }

        private void FeatureViewerForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && OKClicked != null)
            {
                if (ui_featuresTreeView.Nodes.Count > 0)
                {
                    var nodeTag = ui_featuresTreeView.Nodes [0].Tag as NodeTag;
                    if (nodeTag.IsChanged)
                    {
                        var dr = MessageBox.Show ("Save " + _mainFeatures[0].FeatureType + " Changes?", Text,
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {
                            nodeTag.Save ();
                            OKClicked (this, null);
                        }
                        else if (dr == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        private void ValidTimeCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ValidTimeOk_Click(object sender, EventArgs e)
        {
            if (ValidTimeSelected != null)
                ValidTimeSelected(this, e);

            ui_validTimePanel.Visible = false;
            ui_mainContainerPanel.Visible = true;
            
            Size = new Size(820, 632);
            CenterToScreen();

            _isValidTimeSelectionVisible = false;
            SetFeature(_mainFeatures.ToArray());
        }

        private void btnApplyAll_Click(object sender, EventArgs e)
        {
            if (ui_featuresTreeView.Nodes.Count == 0)
                return;

            NodeTag nodeTag = (NodeTag)ui_featuresTreeView.Nodes[0].Tag;

            if (nodeTag.Entity is Feature && _metadataControl != null)
            {
                var feat = nodeTag.Entity as Feature;
                feat.TimeSliceMetadata = _metadataControl.Metadata;
            }

            nodeTag.Save();

            if (OKClicked == null)
                DialogResult = DialogResult.Yes;
            else
                OKClicked(this, e);
        }
    }

    public delegate void ShowGeometryEventHandler (object sender, ShowGeometryEventArgs e);

    public class ShowGeometryEventArgs : EventArgs
    {
        public Control ReplacedControl { get; set; }

        public string PathText { get; set; }

        public Geometries.Geometry GeomValue { get; set; }

		public FeatureType FeatureType { get; set; }
	}
}
