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
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;

namespace Aran.Aim.FeatureInfo
{
    public partial class FeatureInfoControl : UserControl
    {
        private ObservableCollection<NavInfo> _navInfoList;
        private NavInfo _rootNavInfo;
        private NavInfo _currentNavInfo;
        public event OpenFeatureEventHandler FeatureOpened;

        public FeatureInfoControl ()
        {
            InitializeComponent ();

            _navInfoList = new ObservableCollection<NavInfo> ();

            ui_featNavCont.NavItemClicked += new EventHandler (NavigatorItem_Clicked);
            ui_featNavCont.DataContext = _navInfoList;
        }

        public Feature Feature
        {
            get
            {
                return _rootNavInfo.AimObject as Feature;
            }
            set
            {
                SetRootAimObject (value);
            }
        }

        private void SetRootAimObject (IAimObject aimObject)
        {
            _navInfoList.Clear ();
            SetItemsVisibility (null);

            _rootNavInfo = new NavInfo (string.Empty, aimObject);
            _rootNavInfo.BindingInfoList = Global.ToBindingInfoList (aimObject);
            _navInfoList.Add (_rootNavInfo);

            DataContext = _rootNavInfo.BindingInfoList;

            _currentNavInfo = _rootNavInfo;
        }

        private void SetItemsVisibility (IAimProperty aimProp)
        {
            if (aimProp == null || aimProp.PropertyType != AimPropertyType.List)
            {
                ui_listPanel.Visibility = Visibility.Collapsed;
                ui_aimItemsLB.DataContext = null;
            }
            else
            {
                ui_listPanel.Visibility = Visibility.Visible;
                IList list = aimProp as IList;
                ui_aimItemsLB.DataContext = ToItemsList (list);
                if (list.Count > 0)
                    ui_aimItemsLB.SelectedIndex = 0;
            }
        }

        private void OpenListItem (int index)
        {
            IList list = _currentNavInfo.AimProperty as IList;
            IAimObject aimObj = list [index] as IAimObject;

            _currentNavInfo.BindingInfoList = Global.ToBindingInfoList (aimObj as IAimObject);

            DataContext = _currentNavInfo.BindingInfoList;
        }

        private void NavigatorItem_Clicked (object sender, EventArgs e)
        {
            CloseAimProperty (sender as NavInfo);
        }

        private void CloseAimProperty (NavInfo navInfo)
        {
            List<BindingInfo> bindInfoList = navInfo.BindingInfoList;
            if (bindInfoList == null)
                bindInfoList = Global.ToBindingInfoList (navInfo.AimObject);

            DataContext = bindInfoList;

            for (int i = 0; i < _navInfoList.Count; i++)
            {
                if (navInfo.Equals (_navInfoList [i]))
                {
                    int n = i + 1;
                    while (n < _navInfoList.Count)
                        _navInfoList.RemoveAt (n);
                    break;
                }
            }

            _currentNavInfo = navInfo;

            SetItemsVisibility (navInfo.AimProperty);
        }

        private IEnumerable<int> ToItemsList (IList list)
        {
            int [] intArr = new int [list.Count];
            for (int i = 0; i < intArr.Length; i++)
                intArr [i] = i + 1;
            return intArr;
        }

        private void AimItemsListBox_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                int selIndex = (int) e.AddedItems [0];
                OpenListItem (selIndex - 1);
            }
        }

        private void PropItemControl_ComplexPropClicked (object sender, EventArgs e)
        {
            var bindInfo = sender as BindingInfo;

			if (bindInfo == Global.MetadataBindingInfo)
			{
				var feat = _currentNavInfo.AimObject as Feature;
				var mdViewer = new MetadataViewerForm ();
				mdViewer.SetValue (feat.TimeSliceMetadata);
				mdViewer.ShowDialog ();
				return;
			}

            NavInfo ni = new NavInfo (bindInfo.Name, bindInfo.AimProperty);
            ni.PrevNavInfo = _currentNavInfo;
            ni.BindingInfoList = Global.ToBindingInfoList (ni.AimObject as IAimObject);

            _navInfoList.Add (ni);
            _currentNavInfo = ni;

            DataContext = ni.BindingInfoList;

            SetItemsVisibility (bindInfo.AimProperty);
        }

        private void PropItemControl_ReferenceOpened (object sender, OpenFeatureEventArgs e)
        {
            if (FeatureOpened == null)
                return;

            FeatureOpened (this, e);
        }
    }

    public class OpenFeatureEventArgs : EventArgs
    {
        public OpenFeatureEventArgs (FeatureType featureType, Guid identifier)
        {
            FeatureType = featureType;
            Identifier = identifier;
        }

        public FeatureType FeatureType { get; private set; }

        public Guid Identifier { get; private set; }
    }

    public delegate void OpenFeatureEventHandler (object sender, OpenFeatureEventArgs e);
}
