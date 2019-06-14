using System.Collections.Generic;
using Aran.Temporality.CommonUtil.Util;
using MvvmCore;

namespace Aran.Temporality.CommonUtil.Control.TreeViewDragDrop
{
    public class TreeViewItemViewModel : ViewModelBase, ITreeViewItemViewModel
    {
        private static readonly TreeViewItemViewModel DummyChild=new TreeViewItemViewModel();


        public ISelectedItemHolder SelectedItemHolder { get; set; }

        public string Name { get; set; }

        #region Implementation of ITreeViewItemViewModel

        private MtObservableCollection<ITreeViewItemViewModel> _children;
        public MtObservableCollection<ITreeViewItemViewModel> Children
        {
            get { return _children ?? (_children = new MtObservableCollection<ITreeViewItemViewModel>()); }
        }

      

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;

                // Lazy load the child items, if necessary.
                if (HasDummyChild)
                {
                    Children.Remove(DummyChild);
                    LoadChildren();
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");

                    if (_isSelected)
                    {
                        //IsExpanded = true;
                        if (SelectedItemHolder != null)
                        {
                            SelectedItemHolder.SelectedObject = this;
                        }
                    }
                }
            }
        }

        private ITreeViewItemViewModel _parent;
        public ITreeViewItemViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion
    }
}
