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

namespace Aran.Delta.View
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow(List<string> Logs )
        {
            InitializeComponent();
            this.DataContext = this;

            foreach (var log in Logs)
            {
                LogText += log+Environment.NewLine;
            }
        }

        private void BtnDetailInfo_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public string LogText { get; set; }
    }
}
