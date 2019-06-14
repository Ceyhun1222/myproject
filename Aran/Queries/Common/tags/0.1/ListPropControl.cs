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
        private IList _valueList;
        private AimPropInfo _propInfo;
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

                for (int i = 0; i < _valueList.Count; i++)
                {
                    AddNewItemControl (i);
                }
            }
        }

        private void AddNewItemControl (int i)
        {
            Panel linkPanel = new Panel ();
            linkPanel.Location = new Point (0, 0);
            linkPanel.Size = new Size (22, 22);
            linkPanel.BorderStyle = BorderStyle.FixedSingle;

            LinkLabel linkLabel = new LinkLabel ();
            linkLabel.Text = (i + 1).ToString ();
            linkLabel.Location = new Point (2, 2);
            linkPanel.ForeColor = linkLabel.LinkColor;
            linkLabel.Tag = i;

            linkLabel.Click += new EventHandler (linkLabel_Click);

            linkPanel.Controls.Add (linkLabel);
            flowLayoutPanel1.Controls.Add (linkPanel);
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
            AObject aObject = _valueList [index] as AObject;

            EntityClicked (this, aObject, _propInfo.AixmName, true, index);
        }

        public AimPropInfo PropInfo
        {
            get { return _propInfo; }
            set
            {
                _propInfo = value;
                propNameLabel.Text = PropControl.MakeSentence (_propInfo.AixmName) + ":";
                flowLayoutPanel1.Left = propNameLabel.Width + 12;
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

            AddNewItemControl (_valueList.Count - 1);
        }

        private void flowLayoutPanel1_SizeChanged (object sender, EventArgs e)
        {
            Width = flowLayoutPanel1.Right + 60;
            newItemButton.Left = flowLayoutPanel1.Right + 16;
        }
    }
}
