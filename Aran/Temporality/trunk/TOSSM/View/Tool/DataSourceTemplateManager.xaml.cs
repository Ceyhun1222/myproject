using System.Windows.Controls;
using TOSSM.ViewModel.Tool;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for DataSourceTemplateManager.xaml
    /// </summary>
    public partial class DataSourceTemplateManager : UserControl
    {
        public DataSourceTemplateManager()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                var model = DataContext as DataSourceTemplateManagerViewModel;
                if (model == null) return;
                model.Load();
            };
        }
    }
}
