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
using ADM.ViewModel.Tool;

namespace ADM.View.Tool
{
    /// <summary>
    /// Interaction logic for RelationFinder.xaml
    /// </summary>
    public partial class RelationFinder : UserControl
    {
        public RelationFinder()
        {
            InitializeComponent();

            DataContext = new RelationFinderToolViewModel();
        }



        private void ComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var model = DataContext as RelationFinderToolViewModel;
            if (model == null) return;

            model.FeatureTypeFilter = null;
            model.FeatureTypeFilter2 = null;
        }
    }
}
