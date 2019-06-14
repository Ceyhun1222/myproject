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
using System.Windows.Shapes;

namespace HelperDialog
{
    /// <summary>
    /// Interaction logic for MessageScreen.xaml
    /// </summary>
    public partial class MessageScreen : Window
    {


        public string MessageText
        {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register("MessageText", typeof(string), typeof(MessageScreen), new PropertyMetadata(""));


        public MessageScreen()
        {
            InitializeComponent();
        }
    }
}
