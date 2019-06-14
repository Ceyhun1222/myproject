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
using Aran.Aim.AixmMessage;
using Aran.Aim.Features;
using Aran.Omega.VSImporter;
using Microsoft.Win32;

namespace Aran.Omega.VsImporter.IntergrationTests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileName = @"C:\Users\User\Desktop\etod export\AA_23RPAII_190112_2223\Annex14_Obstacles.mdb";
                var aixmConverter = new AixmConverterFromEtod(fileName);
                var vsList = aixmConverter.GetVerticalStructures();

                SaveToXml(vsList);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private static void SaveToXml(List<VerticalStructure> vsList)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog {Filter = "Xml file|*.xml",
                Title = "Save an Xml File"};
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                WriteToXml.WriteAllFeatureToXML(vsList.Cast<Feature>().ToList(),
                    saveFileDialog1.FileName, false, false, null,
                    SrsNameType.EPSG_4326);

                MessageBox.Show("Xml has saved");
            }
        }
    }
}
