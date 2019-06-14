using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiagnosticTool.ViewModel;

namespace DiagnosticTool.View
{
    /// <summary>
    /// Interaction logic for DiagView.xaml
    /// </summary>
    public partial class DiagView : UserControl
    {
        public DiagView()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                var model = DataContext as DiagWindowViewModel;
                if (model != null)
                {
                    model.ConnectCommand.Execute(null);
                }
            };
        }
    }
}
