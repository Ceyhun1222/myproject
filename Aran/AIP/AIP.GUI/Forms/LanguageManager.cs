using AIP.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using AIP.GUI.Classes;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace AIP.GUI.Forms
{
    public partial class LanguageManager : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db;

        public LanguageManager()
        {
            InitializeComponent();
        }

        private void LanguageManager_Load(object sender, EventArgs e)
        {
            FillLangList();
            WindowState = FormWindowState.Maximized;
        }
        

        private void FillLangList()
        {
            try
            {
                cbx_Lang.DisplayMember = "Value";
                //cbx_Lang.ValueMember = "Key";
                cbx_Lang.DataSource = db.LanguageReference.AsEnumerable().Select(x => new KeyValuePair<int, string>(x.id, x.Name)).ToList();               
                
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void FillGridView(int? ReferenceId = null)
        {
            try
            {
                dynamic query;
                if (ReferenceId == null)
                    query = db.LanguageTexts.Where(x => x.LanguageReferenceId == 1).ToList();
                else
                    query = db.LanguageTexts.Where(x => x.LanguageReferenceId == ReferenceId).ToList();

                BindingSource bi = new BindingSource();
                bi.DataSource = query;
                lm_rgv.DataSource = bi;                
                lm_rgv.Refresh();
                lm_rgv.GroupDescriptors.Clear();
                lm_rgv.GroupDescriptors.Add(new GridGroupByExpression("LanguageCategory as LanguageCategory format \"{0}: {1}\" Group By LanguageCategory"));
                lm_rgv.MasterTemplate.ExpandAllGroups();
                lm_rgv.AutoSizeRows = true;
                var colName = lm_rgv.Columns.FirstOrDefault(x => x.Name == "Name");
                if (colName != null)
                {
                    colName.Width = 100;
                    colName.ReadOnly = true;
                }
                var colVal = lm_rgv.Columns.FirstOrDefault(x => x.Name == "Value");
                if (colVal != null)
                {
                    colVal.WrapText = true;
                }
                foreach (GridViewColumn col in lm_rgv.Columns)
                {
                    GridViewTextBoxColumn tbCol = col as GridViewTextBoxColumn;
                    if (tbCol != null)
                    {
                        tbCol.AcceptsReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        

        private async void SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (Permissions.CanManageLanguageTexts()) await db.SaveChangesAsync();
                else
                {
                    ErrorLog.ShowInfo("You haven`t permission to make save");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                List<LanguageReference> obj = db.LanguageReference.Include("LanguageText").ToList();
                //string txt = SerializeObject(obj);
                //XmlSerializer serializer =
                //new XmlSerializer(typeof(List<LanguageReference>));
                //TextWriter writer = new StreamWriter("MyFile.txt");
                //serializer.Serialize(writer, obj);
                //writer.Close();
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("MyFile.txt",
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, obj);
                stream.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        private void btn_Install_Click(object sender, EventArgs e)
        {
            try
            {
                if (Permissions.CanManageLanguageTexts())
                {
                    Lib.InstallTexts(true);
                    lm_rgv.Refresh();
                }
                else
                {
                    ErrorLog.ShowInfo("You haven`t permission to make Install");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void cbx_Lang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillGridView(((KeyValuePair<int,string>)cbx_Lang.SelectedValue).Key);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);                
            }   
        }

        private void btn_Upgrade_Click(object sender, EventArgs e)
        {
            if (Permissions.CanManageLanguageTexts())
            {
                Lib.InstallTexts();
                lm_rgv.Refresh();
            }
            else
            {
                ErrorLog.ShowInfo("You haven`t permission to make Upgrade");
            }
        }

        private void lm_rgv_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadTextBoxEditor tbEditor = e.ActiveEditor as RadTextBoxEditor;
            RadTextBoxEditorElement element = tbEditor?.EditorElement as RadTextBoxEditorElement;
            if (element != null) element.TextBoxItem.Multiline = true;
        }
    }

    public class LanguageTemplate
    {
        public int Id { get; set; }
        public LanguageCategory Category { get; set; }
        public string FieldName { get; set; }
        public string English { get; set; }

        [Category("Language"), DisplayName("Language"), Description("Language")]
        public string OtherLang { get; set; }

    }
}
