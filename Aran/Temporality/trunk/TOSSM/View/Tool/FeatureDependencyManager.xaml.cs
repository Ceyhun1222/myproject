using System;
using System.Windows.Controls;
using TOSSM.ViewModel.Tool;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for FeatureDependencyManager.xaml
    /// </summary>
    public partial class FeatureDependencyManager : UserControl
    {
        public FeatureDependencyManager()
        {
            InitializeComponent();

            Loaded += (t, e) =>
            {
                var model = DataContext as FeatureDependencyManagerToolViewModel;
                if (model != null)
                {
                    model.Load();
                }
            };
        }

        private void ComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            var model = DataContext as FeatureDependencyManagerToolViewModel;
            if (model == null) return;

            model.SelectedConfigurationViewModel.FeatureTypeFilter = null;
        }
    }
}
