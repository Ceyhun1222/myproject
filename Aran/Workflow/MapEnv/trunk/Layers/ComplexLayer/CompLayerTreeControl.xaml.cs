using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Aran.Aim;
using System.Collections.Specialized;
using MapEnv.Layers;

namespace MapEnv.ComplexLayer
{
    /// <summary>
    /// Interaction logic for CompLayerTreeControl.xaml
    /// </summary>
    public partial class CompLayerTreeControl : UserControl
    {
        private ObservableCollection<MyTreeItem> _items;

        internal event CompLayerTreeitemEventHandler AddRefClicked;
        internal event CompLayerTreeitemEventHandler SetSymbolClicked;
        internal event CompLayerTreeitemEventHandler SetFilterClicked;
        internal event CompLayerTreeitemEventHandler RemoveItemClicked;

        public CompLayerTreeControl ()
        {
            InitializeComponent ();

            _items = new ObservableCollection<MyTreeItem> ();
            ui_treeView.DataContext = _items;
        }

        public ObservableCollection<MyTreeItem> Items
        {
            get { return _items; }
        }

        public FeatureType SelectedFeatureType { get; private set; }

        
        private void AddRef_Click (object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var treeItem = fe.DataContext as MyTreeItem;
            if (treeItem == null)
                return;

            if (AddRefClicked != null)
                AddRefClicked (this, new CompLayerTreeItemEventArgs (treeItem));
        }

        private void TreeView_SelectedItemChanged (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void Test_Click (object sender, RoutedEventArgs e)
        {
            (ui_treeView.Items [0] as MyTreeItem).IsNodeExpanded = true;
        }

        private void SetSymbol_Click (object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var treeItem = fe.DataContext as MyTreeItem;
            if (treeItem == null)
                return;

            if (SetSymbolClicked != null)
                SetSymbolClicked (this, new CompLayerTreeItemEventArgs (treeItem));
        }

        private void SetFilter_Click (object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var treeItem = fe.DataContext as MyTreeItem;
            if (treeItem == null)
                return;

            if (SetFilterClicked != null)
                SetFilterClicked (this, new CompLayerTreeItemEventArgs (treeItem));
        }

        private void RemoveItem_Click (object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var treeItem = fe.DataContext as MyTreeItem;
            if (treeItem == null)
                return;

            if (RemoveItemClicked != null)
                RemoveItemClicked (this, new CompLayerTreeItemEventArgs (treeItem));
        }
    }

    public class MyTreeItem : NotifiableObject
    {
        public MyTreeItem ()
        {
            Items = new ObservableCollection<MyTreeItem> ();
            Type = MyTreeItemType.Root;
            Items.CollectionChanged += Childs_CollectionChanged;
            _isNodeExpanded = true;
        }

        public MyTreeItem (QueryInfo queryInfo) :
            this ()
        {
            QueryInfo = queryInfo;
            FeatureType = queryInfo.FeatureType;
        }

        public MyTreeItemType Type { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged ("Name");
            }
        }

        public FeatureType FeatureType
        {
            get { return _featureType; }
            set
            {
                _featureType = value;
                OnPropertyChanged ("FeatureType");

                if (_name == null)
                    Name = value.ToString ();
            }
        }

        public bool IsNodeExpanded
        {
            get { return _isNodeExpanded; }
            set
            {
                if (_isNodeExpanded == value)
                    return;
                _isNodeExpanded = value;
                OnPropertyChanged ("IsNodeExpanded");
            }
        }

        public ObservableCollection<MyTreeItem> Items { get; private set; }

        public QueryInfo QueryInfo { get; private set; }

        public ILinkQueryInfo LinkInfo
        {
            get { return _linkInfo; }
            set 
            {
                _linkInfo = value;
                QueryInfo = _linkInfo.QueryInfo;
            }
        }

        public MyTreeItem Parent { get; private set; }

        public string PropPath { get; set; }
        
        public bool HasSymbol
        {
            get
            {
                return (QueryInfo != null && QueryInfo.ShapeInfoList.Count > 0);
            }
        }

        public void HasSymbolChanged ()
        {
            OnPropertyChanged ("HasSymbol");
        }

		public bool HasFilter
		{
			get
			{
				return (QueryInfo != null && QueryInfo.Filter != null);
			}
		}

		public void HasFilterChanged ()
		{
			OnPropertyChanged ("HasFilter");
		}


        private void Childs_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    (item as MyTreeItem).Parent = this;
                }
            }
        }

        private string _name;
        private FeatureType _featureType;
        private bool _isNodeExpanded;
        private ILinkQueryInfo _linkInfo;
    }

    public enum MyTreeItemType { Root, Sub, Ref }

    public class MyOtherTItem : MyTreeItem
    {
        public MyOtherTItem () : base ()
        {
        }
    }

    public delegate void CompLayerTreeitemEventHandler (object sender, CompLayerTreeItemEventArgs e);

    public class CompLayerTreeItemEventArgs : EventArgs
    {
        public CompLayerTreeItemEventArgs ()
        {

        }

        public CompLayerTreeItemEventArgs (MyTreeItem treeItem)
        {
            TreeItem = treeItem;
        }

        public MyTreeItem TreeItem { get; set; }
    }
}
