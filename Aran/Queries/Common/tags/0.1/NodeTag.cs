using System;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using System.Collections.Generic;

namespace Aran.Queries.Common
{
    public class NodeTag
    {
        public NodeTag ()
        {
            IsChanged = false;
            _suspendChangeEvent = false;
            Index = -1;
            _showType = false;
        }

        public DBEntity Entity
        {
            get { return _entity; }
            set
            {
                if (value == null)
                    throw new Exception ("Entity is null");

                IAimObject aimObject = value as IAimObject;
                IsFeature = (aimObject.AimObjectType == AimObjectType.Feature);

                if (IsFeature)
                {
                    _orginalFeature = value as Feature;
                    _entity = _orginalFeature.Clone () as DBEntity;
                    IsNewFeature = (_orginalFeature.Id == -1);
                }
                else
                {
                    _entity = value;
                }

                _entity.AimPropertyChanged += new AimPropertyChangedEventHandler (Entity_AimPropertyChanged);

                SetTreeNodeText ();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                SetTreeNodeText ();
            }
        }

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                SetTreeNodeText ();
            }
        }

        public bool ShowType
        {
            get { return _showType; }
            set
            {
                _showType = value;
                SetTreeNodeText ();
            }
        }

        public Feature OrginalFeature
        {
            get { return _orginalFeature; }
        }

        public ChoiceClass ChoiceClass { get; set; }
        
        public Panel Panel { get; set; }

        public ADataType FeatureRef { get; set; }

        public Feature ShortcutFeature
        {
            get { return _shortcutFeature; }
            set
            {
                _shortcutFeature = value;
                SetTreeNodeText ();
            }
        }

        public FlowLayoutPanel ComplexFlowLayoutPanel { get; set; }

        public TreeNode TreeNode
        {
            get { return _treeNode; }
            set
            {
                _treeNode = value;
                _treeNode.Tag = this;
                SetTreeNodeText ();
            }
        }

        public NodeTag Parent
        {
            get
            {
                if (TreeNode.Parent == null)
                    return null;

                return (NodeTag) TreeNode.Parent.Tag;
            }
        }

        public NodeTag [] GetChilds ()
        {
            List<NodeTag> list = new List<NodeTag> ();

            foreach (TreeNode childNode in TreeNode.Nodes)
                list.Add ((NodeTag) childNode.Tag);

            return list.ToArray ();
        }
        
        public bool IsFeature { get; private set; }
        
        public bool IsNewFeature { get; private set; }
        
        public bool IsChanged { get; protected set; }

        public bool IsShortcut
        {
            get { return (ShortcutFeature != null); }
        }

        public void SuspendChangeEvent ()
        {
            _suspendChangeEvent = true;
        }

        public void ResumeChangeEvent ()
        {
            _suspendChangeEvent = false;
        }

        public bool IsBaseOn (DBEntity entity)
        {
            if (IsFeature)
            {
                return OrginalFeature.Equals (entity);
            }
            
            return Entity.Equals (entity) || (ChoiceClass != null && ChoiceClass.Equals (entity));
        }

        public void Save ()
        {
            _orginalFeature.Assign (_entity);
            IsChanged = false;
        }

        public bool ClearDBEntity ()
        {
            if (DBEntityClared == null)
                return false;

            DBEntityClared (this, new EventArgs ());
            return true;
        }

        public event DBEntityClaredHandler DBEntityClared;

        private void SetTreeNodeText ()
        {
            if (_shortcutFeature == null)
            {
                if (_name == null || TreeNode == null)
                    return;

                _treeNode.Text = _name;
                if (_index >= 0)
                    _treeNode.Text += " (" + (_index + 1) + ")";

                if (_showType)
                {
                    string typeName = AimMetadata.GetAimTypeName (_entity);
                    
                    if (typeName == "AixmPoint")
                        typeName = "Point";

                    _treeNode.Text += " (" + typeName + ")";
                }
            }
            else
            {
                _treeNode.Text = "Shortcut to " + _shortcutFeature.FeatureType;
            }
        }

        private void Entity_AimPropertyChanged (object sender, AimPropertyChangedEventArgs e)
        {
            if (_suspendChangeEvent)
                return;

            IsChanged = true;

            if (Parent != null)
            {
                Parent.IsChanged = true;
            }
        }

        private DBEntity _entity;
        private Feature _orginalFeature;
        private bool _suspendChangeEvent;
        private TreeNode _treeNode;
        private int _index;
        private string _name;
        private bool _showType;
        private Feature _shortcutFeature;
    }

    public delegate void DBEntityClaredHandler (object sender, EventArgs e);
}
