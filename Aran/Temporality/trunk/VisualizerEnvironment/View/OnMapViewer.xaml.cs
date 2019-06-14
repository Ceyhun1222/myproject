using System.Windows.Controls;
using VisualizerEnvironment.ViewModel;

namespace VisualizerEnvironment.View
{
    /// <summary>
    /// Interaction logic for OnMapViewer.xaml
    /// </summary>
    public partial class OnMapViewer : UserControl
    {
        public OnMapViewer()
        {
            InitializeComponent();
          
            Loaded += (a, b) =>
            {
                DataContext = new OnMapViewerToolViewModel();
               (DataContext as OnMapViewerToolViewModel).OnLoaded();
            };
        }

    }
}
