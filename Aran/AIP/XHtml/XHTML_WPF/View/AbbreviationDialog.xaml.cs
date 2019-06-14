using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
    public partial class AbbreviationDialog : Window
    {
        public AIP.DB.Abbreviation SelectedAbbreviation = null;
        public AbbreviationDialog()
        {
            InitializeComponent();
        }
        
        private void FillCbx()
        {
            try
            {
                cbx_Section.ItemsSource = " ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                using (eAIPContext db = new eAIPContext())
                {
                    db?.LanguageReference?.Load();
                    cbx_Lang.DisplayMemberPath = "Value";
                   // cbx_Lang.v = "Key";
                    List<KeyValuePair<int?, string>> cnList = db?.LanguageReference?.Local?.ToBindingList()
                        ?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                    cnList?.Insert(0, new KeyValuePair<int?, string>(null, " "));
                    cbx_Lang.ItemsSource = cnList;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        private List<AIP.DB.Abbreviation> AbbreviationList()
        {
            try
            {
                using (eAIPContext db = new eAIPContext())
                {

                    string sectionName = cbx_Section.SelectedValue?.ToString();
                    
                    Expression<Func<Abbreviation, bool>> selectQuery = _ => true;
                    if (!string.IsNullOrWhiteSpace(sectionName)) selectQuery = x => x.Ident.StartsWith(sectionName.ToString());

                    int? langId = ((KeyValuePair<int?, string>?) cbx_Lang.SelectedValue)?.Key;

                    Expression<Func<Abbreviation, bool>> langQuery = _ => true;
                    if (!langId.IsNull()) langQuery = x => x.LanguageReferenceId == langId;

                    var query = db.Abbreviation
                        .Where(x => x.IsCanceled == false)?
                        .GroupBy(x => x.Identifier)
                        .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                        .Where(selectQuery)
                        .Where(langQuery)
                        .ToList();
                    return query;
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
                FillCbx();
                //rgv_Abbreviation.ItemsSource = AbbreviationList();
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
                rgv_Abbreviation.IsEnabled = enable;
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
                    if ((rgv_Abbreviation.SelectedItem as Abbreviation) != null)
                    {
                        if (bName.Contains("Insert"))
                        {
                            SelectedAbbreviation = (rgv_Abbreviation.SelectedItem as Abbreviation);
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

        private void Cbx_Section_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rgv_Abbreviation.ItemsSource = AbbreviationList();
        }

        private void Cbx_Lang_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rgv_Abbreviation.ItemsSource = AbbreviationList();
        }
    }
}
