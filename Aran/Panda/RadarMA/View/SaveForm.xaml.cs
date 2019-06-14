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

namespace Aran.Panda.RadarMA.View
{
    /// <summary>
    /// Interaction logic for SaveForm.xaml
    /// </summary>
    public partial class SaveForm : Window
    {
        public SaveForm()
        {
            InitializeComponent();
            ProjectName = "Project 1";
        }

        public string ProjectName { get; internal set; }
    }
}
