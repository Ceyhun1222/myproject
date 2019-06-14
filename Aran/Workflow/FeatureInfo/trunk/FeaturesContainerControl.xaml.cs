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
using Aran.Aim.Features;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Aran.Aim.FeatureInfo
{
    /// <summary>
    /// Interaction logic for FeatureInfoWPFControl.xaml
    /// </summary>
    public partial class FeatureContainerControl : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<FeatureLBInfo> _featureList;

        public event EventHandler CloseClicked;
        public event System.Windows.Forms.MouseEventHandler TopMouseDown;
        public event System.Windows.Forms.MouseEventHandler TopMouseUp;
        public event System.Windows.Forms.MouseEventHandler TopMouseMove;


        public FeatureContainerControl ()
        {
            InitializeComponent ();

            ui_mainBorder.Background = SystemColors.ControlBrush;

            _featureList = new ObservableCollection<FeatureLBInfo> ();
            ui_featuresLB.DataContext = _featureList;

			_featureList.CollectionChanged += FeatureList_CollectionChanged;
        }

        public void SetFeature (IEnumerable<Feature> featureList)
        {
            _featureList.Clear ();

            foreach (var feature in featureList)
            {
                var featLBInfo = new FeatureLBInfo (feature);
                featLBInfo.IsCloseVisible = false;
                _featureList.Add (featLBInfo);
            }

            if (_featureList.Count == 0)
                return;

            ui_featuresLB.SelectedIndex = 0;
        }

        public Feature CurrentFeaure
        {
            get { return _currentFeature; }
            set
            {
                if (_currentFeature != value)
                {
                    _currentFeature = value;
                    ui_titleTextBlock.Text = value.FeatureType.ToString ();

                    if (PropertyChanged != null)
                        PropertyChanged (this, new PropertyChangedEventArgs ("CurrentFeature"));
                }
            }
        }

        public void HideTopPanel ()
        {
            ui_mainGrid.RowDefinitions [0].Height = new GridLength (0);
        }


        private void FeatatureInfoControl_FeatureOpened (object sender, OpenFeatureEventArgs e)
        {
            foreach (var item in _featureList)
            {
                if (item.Feature.Identifier == e.Identifier)
                {
                    ui_featuresLB.SelectedItem = item;
                    return;
                }
            }

            var feature = Global.GetFeature (e.FeatureType, e.Identifier);

            if (feature == null)
            {
                MessageBox.Show (string.Format ("Feature not found!\nFeature Type: {0} , Identifier: {1}",
                    e.FeatureType, e.Identifier));
                return;
            }

            var fi = new FeatureLBInfo (feature);
            _featureList.Add (fi);
            ui_featuresLB.SelectedItem = fi;
        }

        private void FeaturesListBox_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                FeatureLBInfo selInfo = e.AddedItems [0] as FeatureLBInfo;
                FeatureInfoControl featInfoControl = GetFeatureInfoControl (selInfo.Feature);

                if (featInfoControl == null)
                {
                    featInfoControl = new FeatureInfoControl ();
                    featInfoControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    featInfoControl.VerticalAlignment = VerticalAlignment.Stretch;
                    featInfoControl.FeatureOpened += new OpenFeatureEventHandler (FeatatureInfoControl_FeatureOpened);
                    ui_featInfoContaner.Children.Add (featInfoControl);
                    featInfoControl.Feature = selInfo.Feature;
                }

                SetVisibleControl (featInfoControl);
                
            }
        }

        private void FeatureClose_Click (object sender, RoutedEventArgs e)
        {
            if (_featureList.Count < 2)
                return;

            FeatureLBInfo featInfo = (sender as Button).DataContext as FeatureLBInfo;
            bool removed = RemoveFeatureInfoControl (featInfo.Feature);
            if (removed)
            {
                _featureList.Remove (featInfo);
                ui_featuresLB.SelectedIndex = 0;
            }
        }

        private void SetVisibleControl (FeatureInfoControl fiControl)
        {
            CurrentFeaure = fiControl.Feature;

            foreach (FrameworkElement childCont in ui_featInfoContaner.Children)
            {
                childCont.Visibility = (fiControl.Equals (childCont) ?
                    Visibility.Visible : Visibility.Collapsed);
            }
        }

        private FeatureInfoControl GetFeatureInfoControl (Feature feature)
        {
            foreach (FeatureInfoControl childCont in ui_featInfoContaner.Children)
            {
                if (childCont.Feature.Equals (feature))
                    return childCont;
            }
            return null;
        }

        private bool RemoveFeatureInfoControl (Feature feature)
        {
            for (int i = 0; i < ui_featInfoContaner.Children.Count; i++)
            {
                Feature contFeat = (ui_featInfoContaner.Children [i] as FeatureInfoControl).Feature;
                if (contFeat.Equals (feature))
                {
                    ui_featInfoContaner.Children.RemoveAt (i);
                    return true;
                }
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Feature _currentFeature { get; set; }

        private void CloseButton_Click (object sender, RoutedEventArgs e)
        {
            if (CloseClicked != null)
                CloseClicked (this, e);
        }

        private void Top_MouseDown (object sender, MouseButtonEventArgs e)
        {
            if (TopMouseDown != null)
            {
                var pos = e.GetPosition (null);
                var mea = new System.Windows.Forms.MouseEventArgs 
                    (System.Windows.Forms.MouseButtons.Left, 1, (int) pos.X, (int) pos.Y, 1);
                TopMouseDown (this, mea);
            }
        }

        private void Top_MouseUp (object sender, MouseButtonEventArgs e)
        {
            if (TopMouseUp != null)
            {
                var pos = e.GetPosition (null);
                var mea = new System.Windows.Forms.MouseEventArgs
                    (System.Windows.Forms.MouseButtons.Left, 1, (int) pos.X, (int) pos.Y, 1);
                TopMouseUp (this, mea);
            }
        }
        
        private void Top_MouseMove (object sender, MouseEventArgs e)
        {
            if (TopMouseMove != null)
            {
                var pos = e.GetPosition (null);
                var mea = new System.Windows.Forms.MouseEventArgs
                    (System.Windows.Forms.MouseButtons.Left, 1, (int) pos.X, (int) pos.Y, 1);
                TopMouseMove (this, mea);
            }
        }

		private void FeatureList_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			ui_featuresLB.Visibility = (_featureList.Count > 1 ? Visibility.Visible : Visibility.Collapsed);
		}
    }

    internal class FeatureLBInfo
    {
        public FeatureLBInfo (Feature feature)
        {
            Feature = feature;
			Description = string.Empty;
            IsCloseVisible = true;

			bool hasDesc;
			var s = Aran.Aim.Metadata.UI.UIUtilities.GetFeatureDescription (feature, out hasDesc);
			if (hasDesc)
				Description = s;
        }

        public Feature Feature { get; set; }

        public string Name
        {
            get { return Feature.FeatureType.ToString (); }
        }

		public string Description { get; private set; }

        public bool IsCloseVisible { get; set; }
    }
}
