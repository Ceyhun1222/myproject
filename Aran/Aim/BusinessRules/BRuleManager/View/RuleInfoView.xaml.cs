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
    /// Interaction logic for RuleInfoView.xaml
    /// </summary>
    public partial class RuleInfoView : UserControl
    {
        public RuleInfoView()
        {
            InitializeComponent();
        }

        public RelayCommand EditCommand
        {
            get { return (RelayCommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public RelayCommand DeleteCommand
        {
            get { return (RelayCommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        #region Dependency Register

        public static readonly DependencyProperty EditCommandProperty =
            DependencyProperty.Register("EditCommand", typeof(RelayCommand), typeof(RuleInfoView), new PropertyMetadata(null));

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(RelayCommand), typeof(RuleInfoView), new PropertyMetadata(null));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RuleInfoView), new PropertyMetadata(false));

        #endregion
    }
}
