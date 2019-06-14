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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Aran.Aim.Features;
using Aran.Aim;
using System.Globalization;
using MapEnv.Layers;

namespace MapEnv.ComplexLayer
{
    public partial class ComplexLayerViewerW : UserControl
    {
        public ComplexLayerViewerW ()
        {
            InitializeComponent ();

            Root = new CLVRoot ();
            DataContext = Root;
            ui_infoGrid.DataContext = null;
        }

        public CLVRoot Root { get; private set; }


        public static bool IsControlKeyDown
        {
            get
            {
                return (Keyboard.IsKeyDown (Key.LeftCtrl) || Keyboard.IsKeyDown (Key.RightCtrl));
            }
        }


        private void TreeView_SelectedItemChanged (object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ui_infoGrid.DataContext = ui_treeView.SelectedItem;
        }

        private void Hyperlink_Click (object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            var treeItem = hyperlink.DataContext as CLVTreeItem;

            var roViewer = new Aran.Aim.FeatureInfo.ROFeatureViewer ();
            roViewer.GettedFeature = new Aran.Aim.FeatureInfo.GetFeatureHandler (Globals.FeatureViewer_GetFeature);
            
            var list = new List<Aran.Aim.Features.Feature> ();
            list.Add (treeItem.ComplexRow.Row.AimFeature);
            roViewer.SetOwner (Globals.MainForm);
            roViewer.ShowFeaturesForm (list, true);
        }
    }

    //**************************************************************************
    //**************************************************************************
    //**************************************************************************

    public class CLVRoot : NotifiableObject
    {
        public CLVRoot ()
        {
            Items = new ObservableCollection<CLVTreeItem> ();
        }

        public ObservableCollection<CLVTreeItem> Items { get; private set; }

        public string FeatureType
        {
            get { return _featureType; }
            set
            {
                if (_featureType == value)
                    return;
                _featureType = value;
                OnPropertyChanged ("FeatureType");
            }
        }

        private string _featureType;
    }

    public class CLVTreeItem : NotifiableObject
    {
        public CLVTreeItem ()
        {
            Items = new ObservableCollection<CLVTreeItem> ();
            IsVisible = true;
        }

        public CLVTreeItem (string name)
            : this ()
        {
            _name = name;
        }

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

        public string FeatureType
        {
            get { return _featureType; }
            set
            {
                if (_featureType == value)
                    return;
                _featureType = value;
                OnPropertyChanged ("FeatureType");
            }
        }

        public string PropName
        {
            get { return _propName; }
            set
            {
                if (_propName == value)
                    return;
                _propName = value;
                OnPropertyChanged ("PropName");
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

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                    return;
                _isVisible = value;
                OnPropertyChanged ("IsVisible");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                OnPropertyChanged ("IsSelected");
            }
        }

        public bool IsRoot
        {
            get { return string.IsNullOrEmpty (_propName); }
        }

        public ObservableCollection<CLVTreeItem> Items { get; private set; }

        public AimComplexRow ComplexRow { get; set; }

        private string _name;
        private string _featureType;
        private string _propName;
        private bool _isVisible;
        private bool _isSelected;
        private bool _isNodeExpanded;
    }

    //**************************************************************************
    //**************************************************************************
    //**************************************************************************

    public class PropNameConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            var s = value.ToString ();
            var sa = s.Split (new char [] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var res = string.Empty;

            for (int i = 0; i < sa.Length; i++)
            {
                res += Repeat ("   ", i) +  sa [i] + Environment.NewLine;
            }

            return res;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException ();
        }

        public static string Repeat (string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder (repeat * stringToRepeat.Length);

            for (int i = 0; i < repeat; i++)
                builder.Append (stringToRepeat);
            return builder.ToString ();
        }
    }

}
