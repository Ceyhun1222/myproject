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
using ChartElementEditor.ViewModel;

namespace ChartElementEditor.View
{
    /// <summary>
    /// Interaction logic for ElementEditorView.xaml
    /// </summary>
    public partial class ElementEditorView : UserControl
    {
        public ElementEditorView()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                var model = DataContext as ElementEditorViewModel;
                if (model == null) return;
                model.Load();
            };
        }
    }
}
