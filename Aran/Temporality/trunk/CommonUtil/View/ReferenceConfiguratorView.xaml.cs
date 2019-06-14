using System;
using System.Windows.Controls;
using Aran.Temporality.CommonUtil.ViewModel;

namespace Aran.Temporality.CommonUtil.View
{
    /// <summary>
    /// Interaction logic for ReferenceConfiguratorView.xaml
    /// </summary>
    public partial class ReferenceConfiguratorView : UserControl
    {
        public ReferenceConfiguratorView()
        {
            InitializeComponent();
            DataContext = new FeatureDependencyConfigurationViewModel();
        }

        private void ComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var model = DataContext as FeatureDependencyConfigurationViewModel;
            if (model == null) return;

            model.FeatureTypeFilter = null;
        }
    }
}
