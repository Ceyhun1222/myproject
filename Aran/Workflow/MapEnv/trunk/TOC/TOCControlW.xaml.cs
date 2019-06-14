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
using MapEnv.Toc;
using System.Diagnostics;
using System.Windows.Forms.Integration;
using MapEnv.Layers;

namespace MapEnv.Controls
{
    /// <summary>
    /// Interaction logic for TOCControl.xaml
    /// </summary>
    public partial class TOCControlW : UserControl
    {
        private ObservableCollection<TocItem> _itemList;
        private readonly string _dataFormat;

        public event TocItemMenuEventHandler ItemMenuClicked;
        public TocItemReplacedEventHandler ItemReplaced;


        public TOCControlW ()
        {
            InitializeComponent ();

            _itemList = new ObservableCollection<TocItem> ();

            ui_tocLB.DataContext = _itemList;
            _dataFormat = "toc_item";
        }

        public void Add (TocItem tocItem)
        {
            _itemList.Add (tocItem);
        }

		public void Insert (int index, TocItem tocItem)
		{
			_itemList.Insert (index, tocItem);
		}

		public void Remove (TocItem tocItem)
		{
			_itemList.Remove (tocItem);
		}

        public void ClearItems ()
        {
            _itemList.Clear ();
        }


        private void ShowMenu_MouseUp (object sender, MouseButtonEventArgs e)
        {
            var elem = sender as DependencyObject;
            DependencyObject parentObj = null;

            while ((parentObj = VisualTreeHelper.GetParent (elem)) != null)
            {
                if (parentObj is Expander)
                {
                    var exp = parentObj as Expander;
                    if (exp.ContextMenu != null)
                    {
                        exp.ContextMenu.PlacementTarget = exp;
                        exp.ContextMenu.IsOpen = true;
                    }
                    break;
                }

                elem = parentObj;
            }
        }

        private void Button_Click (object sender, RoutedEventArgs e)
        {
            ShowMenu_MouseUp (sender, null);
        }

        private void Rectangle_MouseDown (object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ShowMenu_MouseUp (sender, null);
        }

        private void TocMenuItem_Click (object sender, RoutedEventArgs e)
        {
            Debug.Assert (ItemMenuClicked != null);

            if (ItemMenuClicked == null)
                return;

            var mi = sender as MenuItem;

            Debug.Assert (mi.DataContext != null);

            var cm = mi.Parent as ContextMenu;
            var elem = cm.PlacementTarget;

            if (elem != null && elem is FrameworkElement)
            {
                var fe = elem as FrameworkElement;

                TocItemMenuType menuItem;
                if (Enum.TryParse<TocItemMenuType> (mi.DataContext.ToString (), true, out menuItem))
                {
                    var tocItem = fe.DataContext as TocItem;
                    var ea = new TocItemMenuEventArg (menuItem, tocItem);
                    ItemMenuClicked (this, ea);

                    if (true.Equals(ea.Result)) {

                        if (ea.MenuType == TocItemMenuType.Property) {
                            List<TableShapeInfo> shapeInfos = null;
                            var simpleTocItem = tocItem as AimSimpleTocItem;

                            if (ea.AimLayer.Layer is AimFeatureLayer) {
                                var aimFL = ea.AimLayer.Layer as AimFeatureLayer;
                                if (aimFL.IsComplex) {

                                }
                                else {
                                    shapeInfos = aimFL.ShapeInfoList;
                                }

                                tocItem.Name = aimFL.Name;
                            }


                            if (shapeInfos != null) {
                                simpleTocItem.SymbolItems.Clear();
                                var gr = Globals.MainForm.CreateGraphics();
                                foreach (var item in shapeInfos) {
                                    var symbol = item.CategorySymbol.DefaultSymbol;
                                    var bitmap = StyleFunctions.SymbolToBitmap(symbol, new System.Drawing.Size(24, 24), gr, Globals.ToRGBColor(System.Drawing.Color.White).RGB);
                                    simpleTocItem.SymbolItems.Add(
                                        new TocSymbolItem {
                                            SymbolImage = TocGlobal.ToWpfImage(bitmap),
                                            PropertyName = item.GeoProperty
                                        });
                                }
                                gr.Dispose();
                            }

                        }
                        else if (ea.MenuType == TocItemMenuType.ExportXml) {

                        }
                    }
                }
                else
                {
                    Debug.Assert (false);
                }
            }
        }


        private void FE_MouseMove (object sender, MouseEventArgs e)
        {
            var fe =sender as FrameworkElement;
            if (fe != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject dataObj = new DataObject (_dataFormat, fe.DataContext);
                DragDrop.DoDragDrop (this, dataObj, DragDropEffects.Move);
            }
        }

        private void FE_DragEnter (object sender, DragEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var destItem = fe.DataContext as TocItem;
            if (e.Data.GetDataPresent (_dataFormat) && destItem != e.Data.GetData (_dataFormat))
            {
                destItem.IsDragDropLineVisible = true;
            }
        }

        private void FE_DragLeave (object sender, DragEventArgs e)
        {
            var fe = sender as FrameworkElement;
            (fe.DataContext as TocItem).IsDragDropLineVisible = false;
        }

        private void FE_Drop (object sender, DragEventArgs e)
        {
            var fe =sender as FrameworkElement;
            var destItem = fe.DataContext as TocItem;
            var srcItem = e.Data.GetData (_dataFormat) as TocItem;

            destItem.IsDragDropLineVisible = false;

            int index = _itemList.IndexOf (destItem);
            if (index == -1 || ItemReplaced == null)
                return;

            var ea = new TocItemReplacedEventArg (srcItem, destItem);
            ItemReplaced (this, ea);
            if (ea.Replaced)
            {
                _itemList.Remove (srcItem);
                _itemList.Insert (index, srcItem);    
            }
        }

        private void PictureBoxHost_Loaded (object sender, RoutedEventArgs e)
        {
            var wfh = e.OriginalSource as WindowsFormsHost;
            var pb =  wfh.Child as System.Windows.Forms.PictureBox;
            

            pb.Image = TocGlobal.LoadingImage;
        }

        private void AimSimpleLayerExpander_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
        }
    }

    public delegate void TocItemMenuEventHandler (object sender, TocItemMenuEventArg e);

    public class TocItemMenuEventArg : EventArgs
    {
        public TocItemMenuEventArg (TocItemMenuType menuType, TocItem tocItem)
        {
            MenuType = menuType;
            TocItem = tocItem;
        }

        public TocItemMenuType MenuType { get; private set;}

        public TocItem TocItem { get; private set; }

        public AimLayer AimLayer { get; set; }

        public object Result { get; set; }
    }

    public enum TocItemMenuType { Open, Refresh, Property, ZoomToLayer, Remove, ExportXml }

    public delegate void TocItemReplacedEventHandler (object sender, TocItemReplacedEventArg e);

    public class TocItemReplacedEventArg : EventArgs
    {
        public TocItemReplacedEventArg (TocItem srcItem, TocItem destItem)
        {
            SrcItem = srcItem;
            DestItem = destItem;
            Replaced = false;
        }

        public TocItem DestItem { get; private set; }

        public TocItem SrcItem { get; private set; }

        public bool Replaced { get; set; }
    }
}
