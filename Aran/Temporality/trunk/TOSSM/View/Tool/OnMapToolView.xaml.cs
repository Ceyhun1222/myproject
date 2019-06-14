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
using System.Windows.Threading;
using Aran.Temporality.CommonUtil.ViewModel;
using TOSSM.ViewModel;

namespace TOSSM.View.Tool
{
    /// <summary>
    /// Interaction logic for OnMapToolView.xaml
    /// </summary>
    public partial class OnMapToolView : UserControl
    {
        public OnMapToolView()
        {
            InitializeComponent();

            
            Loaded += (a, b) => Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Background,
                (Action)(() =>
                             {
                                 MapContentControl.Content = MainManagerModel.Instance.OnMapViewer;
                                 MainManagerModel.Instance.OnMapViewer.DataContext = DataContext;
                             }));
        }
    }
}
