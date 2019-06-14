using MvvmCore;
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

namespace BRuleManager.View
{
    /// <summary>
    /// Interaction logic for BRuleControl.xaml
    /// </summary>
    public partial class RuleEditView : UserControl
    {
        public RuleEditView()
        {
            InitializeComponent();

            Loaded += RuleEditView_Loaded;
        }

        private void RuleEditView_Loaded(object sender, RoutedEventArgs e)
        {
            dynamic eo = DataContext;
            ui_sbvrEditor.SetTaggedDescription(eo.TaggedDescription);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var td = ui_sbvrEditor.GetTaggedDescription();

            dynamic eo = DataContext;
            eo.TaggedDescription = td;
            eo.SaveCancelCommand.Execute("save");
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            dynamic eo = DataContext;
            eo.ErrorText = string.Empty;
        }
    }
}
