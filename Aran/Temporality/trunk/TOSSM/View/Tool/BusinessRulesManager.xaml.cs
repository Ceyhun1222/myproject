using System.Windows.Controls;
using TOSSM.ViewModel.Tool;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for BusinessRulesManager.xaml
    /// </summary>
    public partial class BusinessRulesManager : UserControl
    {
        public BusinessRulesManager()
        {
            InitializeComponent();
            Loaded += (t,e) =>
                          {
                              var model = DataContext as BusinessRulesManagerToolViewModel;
                              if (model!=null)
                              {
                                  model.Load();
                              }
                          };
        }
    }
}
