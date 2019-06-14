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
using Aran.Aim.FeatureInfo;

namespace Aran.Aim.FeatureInfo
{
    /// <summary>
    /// Interaction logic for ComplexValueControl.xaml
    /// </summary>
    public partial class ComplexValueControl : UserControl
    {
        public event EventHandler OpenAimPropClicked;

        public ComplexValueControl ()
        {
            InitializeComponent ();
        }

        private void ComplexProperty_Click (object sender, RoutedEventArgs e)
        {
            BindingInfo bindInfo = (sender as Hyperlink).DataContext as BindingInfo;

            if (bindInfo.InfoType == BindingInfoType.Complex)
                OpenAimProperty (bindInfo);
        }

        private void OpenAimProperty (BindingInfo bindInfo)
        {
            if (OpenAimPropClicked == null)
                return;

            OpenAimPropClicked (bindInfo, null);
        }
    }
}
