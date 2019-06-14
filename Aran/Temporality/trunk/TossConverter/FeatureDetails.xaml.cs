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
using TossConverter.Model;
using TossConverter.ViewModel;

namespace TossConverter
{
    /// <summary>
    /// Interaction logic for FeatureDetails.xaml
    /// </summary>
    public partial class FeatureDetails : Window
    {
        public FeatureDetails(LogInfo logInfo)
        {
            InitializeComponent();
            MessageTextBox.Text = logInfo.Title;
            FeatureTextBox.Text = logInfo.Description;
        }
    }
}
