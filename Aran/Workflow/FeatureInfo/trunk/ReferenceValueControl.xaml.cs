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

namespace Aran.Aim.FeatureInfo
{
    /// <summary>
    /// Interaction logic for ReferenceValueControl.xaml
    /// </summary>
    public partial class ReferenceValueControl : UserControl
    {
        public event OpenFeatureEventHandler FeatureOpened;

        public ReferenceValueControl ()
        {
            InitializeComponent ();
        }

        private void FeatureRefOpen_Click (object sender, RoutedEventArgs e)
        {
            if (FeatureOpened == null)
                return;

            RefItem refItem = (sender as Hyperlink).DataContext as RefItem;

            FeatureOpened (this,
                new OpenFeatureEventArgs (refItem.FeatureType, refItem.Identifier));
        }

		private void MoreFeatureList_Click (object sender, RoutedEventArgs e)
		{

		}
    }
}
