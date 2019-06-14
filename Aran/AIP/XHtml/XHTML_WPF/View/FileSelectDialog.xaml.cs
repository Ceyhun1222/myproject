using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using AIP.DB;
using Telerik.Windows.Controls;
using XHTML_WPF.Classes;
using XHTML_WPF.ViewModel;

namespace XHTML_WPF.View
{
    /// <summary>
    /// Interaction logic for FileSelectDialog.xaml
    /// </summary>
    public partial class FileSelectDialog : Window
    {
        public AIP.DB.AIPFile SelectedFile = null;
        public FileSelectDialog()
        {
            InitializeComponent();
        }

        private void InitForm()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private List<AIP.DB.AIPFile> FileList()
        {
            try
            {
                using (eAIPContext db = new eAIPContext())
                {
                    var files = db.AIPFile
                        .Where(x => x.IsCanceled == false)?
                        .GroupBy(x => x.Identifier)
                        .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                        .Where(x => x.SectionName == SectionName.None)
                        .ToList();
                    return files;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        private void FileSelectDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                rgv_File.ItemsSource = FileList();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void ControlsEnabled(bool enable)
        {
            try
            {
                btn_Cancel.IsEnabled = enable;
                btn_Insert.IsEnabled = enable;
                btn_Preview.IsEnabled = enable;
                rgv_File.IsEnabled = enable;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is RadButton)
                {
                    string bName = (sender as RadButton).Name;
                    if (bName.Contains("Cancel"))
                    {
                        this.Close();
                        return;
                    }
                    if ((rgv_File.SelectedItem as AIPFile) != null)
                    {
                        if (bName.Contains("Preview"))
                        {
                            ControlsEnabled(false);
                            using (eAIPContext db = new eAIPContext())
                            {
                                var Identifier = ((AIPFile) rgv_File.SelectedItem).Identifier;
                                var file = db.AIPFile.Where(x => x.Identifier == Identifier)
                                    .Include(x => x.AIPFileData)
                                    .OrderByDescending(x=>x.Version)
                                    .FirstOrDefault();
                                var path = Lib.SaveAIPFile(file);
                                if (path != "" && File.Exists(path))
                                {
                                    System.Diagnostics.Process.Start(path);
                                }
                            }
                        }
                        else if (bName.Contains("Insert"))
                        {
                            SelectedFile = (rgv_File.SelectedItem as AIPFile);
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                ControlsEnabled(true);
            }
        }
    }
}
