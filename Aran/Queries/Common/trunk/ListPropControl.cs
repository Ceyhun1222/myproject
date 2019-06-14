using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Aran.Aim;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Queries.Common
{
    public partial class ListPropControl : UserControl
    {
        private const int perStageItemCount = 10;
        private IList _valueList;
        private AimPropInfo _propInfo;
        private int _curAddedIndex;
        public event EntityClickedHandler EntityClicked;
        public event FeatureSelectedEventHandner FeatureSelected;

        public ListPropControl ()
        {
            InitializeComponent ();
        }

        public IList Value
        {
            get { return _valueList; }
            set
            {
                _valueList = value;

                //var IsMoreButtonVisible = false;

                //if (count > 10)
                //{
                //	count = 5;
                //	IsMoreButtonVisible = true;
                //}

                _curAddedIndex = 0;
                UpdateList();

				//if (IsMoreButtonVisible)
					//AddNewItemControl ("More Features (" + (_valueList.Count - 5) + ")", -1);
            }
        }

        private void AddNewItemControl (string text, int i)
        {
            Panel linkPanel = new Panel ();
            linkPanel.Location = new Point (0, 0);
			linkPanel.Size = new Size ((i == -1) ? 140 : 22, 22);
            linkPanel.BorderStyle = BorderStyle.FixedSingle;

            LinkLabel linkLabel = new LinkLabel ();
			linkLabel.Text = text;
            linkLabel.Location = new Point (2, 2);
            linkPanel.ForeColor = linkLabel.LinkColor;
			linkLabel.AutoSize = true;
            linkLabel.Tag = i;

            linkLabel.Click += new EventHandler (linkLabel_Click);

            linkPanel.Controls.Add (linkLabel);

			var lastIsMoreButton = 
				(flowLayoutPanel1.Controls.Count > 0 && 
				(-1 == (int) flowLayoutPanel1.Controls [flowLayoutPanel1.Controls.Count - 1].Controls [0].Tag));

			flowLayoutPanel1.Controls.Add (linkPanel);
			if (lastIsMoreButton)
				flowLayoutPanel1.Controls.SetChildIndex (linkPanel, flowLayoutPanel1.Controls.Count - 2);
        }

        private void RemoveListItem (int listIndex)
        {
            for (int i = listIndex + 1; i < flowLayoutPanel1.Controls.Count; i++)
            {
                Panel linkPanel = (Panel) flowLayoutPanel1.Controls [i];
                LinkLabel linkLabel = (LinkLabel) linkPanel.Controls [0];

                int newIndex = i - 1;
                linkLabel.Text = (newIndex + 1).ToString ();
                linkLabel.Tag = newIndex;
            }

            _valueList.RemoveAt (listIndex);
            flowLayoutPanel1.Controls.RemoveAt (listIndex);
        }

        private void linkLabel_Click (object sender, EventArgs e)
        {
            if (EntityClicked == null)
                return;

            LinkLabel linkLabel = (LinkLabel) sender;
            int index = (int) linkLabel.Tag;

			if (index == -1)
			{
				if (_propInfo.ReferenceFeature != 0)
					Global.ShowFeatureRefs (_propInfo.ReferenceFeature, _valueList);
				else
					MessageBox.Show (string.Format ("Error on loading other {0} features", _valueList.Count - 5));
			}
			else
			{
				AObject aObject = _valueList [index] as AObject;
				EntityClicked (this, aObject, _propInfo.AixmName, true, index);
			}
        }

        public AimPropInfo PropInfo
        {
            get { return _propInfo; }
            set
            {
                _propInfo = value;
                propNameLabel.Text = PropControl.MakeSentence (_propInfo.AixmName) + ":";
                flowLayoutPanel1.Left =propNameLabel.Right;
            }
        }

        public void NodeTag_DBEntityCleared (object sender, EventArgs e)
        {
            NodeTag nodeTag = (NodeTag) sender;

            RemoveListItem (nodeTag.Index);

            NodeTag [] childs = nodeTag.Parent.GetChilds ();
            for (int i = nodeTag.Index; i < childs.Length; i++)
            {
                NodeTag childNodeTag = childs [i];
                childNodeTag.Index--;
            }

            if (_valueList.Count == 0)
            {
                Parent.Controls.Remove (this);
                this.Dispose ();
            }
            
        }

        private void newItemButton_Click (object sender, EventArgs e)
        {
            if (_valueList.Count == 0)
                return;

            FeatureRef featureRef = null;

            if (PropInfo.IsFeatureReference)
            {
                featureRef = FeatureSelected (PropInfo);
                if (featureRef == null)
                    return;

                foreach (FeatureRefObject fro in _valueList)
                {
                    if (fro.Feature.Identifier == featureRef.Identifier)
                    {
                        MessageBox.Show ("Feature already added to this list", 
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            IAimObject item = _valueList [0] as IAimObject;
            int aranTypeIndex = AimMetadata.GetAimTypeIndex (item);
            AimObject newItem = AimObjectFactory.Create (aranTypeIndex);
            
            if (featureRef != null)
            {
                ((FeatureRefObject) newItem).Feature = featureRef;
            }

            _valueList.Add (newItem);

			var n = _valueList.Count - 1;
			AddNewItemControl ((n + 1).ToString (), n);
        }

        private void flowLayoutPanel1_SizeChanged (object sender, EventArgs e)
        {
            Width = flowLayoutPanel1.Right + 65;
            
            btnNext.Left = flowLayoutPanel1.Right + 10;
            newItemButton.Left = btnNext.Right;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            UpdateList(true);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            UpdateList(false);
        }

        private void UpdateList(bool isNext)
        {
            int startIndex = _curAddedIndex;
            int endIndex = 0;

            if (isNext)
            {
                endIndex = Math.Min(_curAddedIndex+perStageItemCount, _valueList.Count);
                startIndex = endIndex - perStageItemCount;
            }
            else {
                startIndex = Math.Max(_curAddedIndex -2* perStageItemCount, 0);
                endIndex =Math.Min(startIndex + perStageItemCount,_valueList.Count);
            }

            _curAddedIndex = startIndex;
            UpdateList();
        }

        private void UpdateList() {

            int size = Math.Min(perStageItemCount, _valueList.Count);

            btnPrev.Enabled = (_curAddedIndex) > 0;
            btnNext.Enabled = (_curAddedIndex+perStageItemCount) < _valueList.Count;

            flowLayoutPanel1.Controls.Clear();
            for (int i = _curAddedIndex; i < _curAddedIndex+ size; i++) 
                AddNewItemControl((i + 1).ToString(), i);

            _curAddedIndex = _curAddedIndex + size;
        }

       
    }
}
