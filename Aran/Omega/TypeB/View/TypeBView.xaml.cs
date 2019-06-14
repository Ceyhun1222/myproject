using System;
using System.Windows;

namespace Aran.Omega.TypeB.View
{
    /// <summary>
    /// Interaction logic for TypeB.xaml
    /// </summary>
    public partial class TypeBView : Window
    {
        public TypeBView()
        {
            InitializeComponent();
            var viewModel = new ViewModels.TypeBViewModel();
            viewModel.Init();
            GlobalParams.TypeBViewModel = viewModel;
            if (viewModel.CloseAction == null)
                viewModel.CloseAction += new Action(this.Close);
            Closing += viewModel.OnWindowClosing;
            this.DataContext = viewModel;
        }
    }
}
