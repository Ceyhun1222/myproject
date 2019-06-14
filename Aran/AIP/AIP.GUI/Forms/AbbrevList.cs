using AIP.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AIP.BaseLib;
using AIP.DB.Entities;
using AIP.XML;
using EntityFramework.Extensions;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;
using Abbreviation = AIP.DB.Abbreviation;

namespace AIP.GUI.Forms
{
    public partial class AbbrevList : Telerik.WinControls.UI.RadForm
    {
        public eAIPContext db;
        private bool FormLoaded = false;
        private Dictionary<string, GridViewOption> fileProp = null;
        private object lockObj = null;

        public AbbrevList()
        {
            InitializeComponent();
        }

        private void LanguageManager_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            fileProp = Lib.GridViewAttributes<Abbreviation>();
            FillCbx();
            UpdateDataGridView();
            FormLoaded = true;
            //btn_Import.Visible = !db.GetDBConfiguration<bool>(Cfg.IsAbbreviationAdded);
        }

        private void FillCbx()
        {
            try
            {
                cbx_Section.DataSource = " ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                cbx_Lang.DisplayMember = "Value";
                cbx_Lang.ValueMember = "Key";

                db?.LanguageReference?.Load();
                List<KeyValuePair<int?, string>> cnList = db?.LanguageReference?.Local?.ToBindingList()?.Select(x => new KeyValuePair<int?, string>(x.id, x.Name)).ToList();
                cnList?.Insert(0, new KeyValuePair<int?, string>(null, " "));
                cbx_Lang.DataSource = cnList;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }


        private void UpdateDataGridView()
        {
            try
            {
                string sectionName = cbx_Section.SelectedValue.ToString();
                int? lang = int.TryParse(cbx_Lang?.SelectedValue?.ToString(), out int temp) ? temp : default(int?);

                Expression<Func<Abbreviation, bool>> selectQuery = _ => true;
                if (lang != null) selectQuery = x => x.LanguageReferenceId == lang;
                if (!string.IsNullOrWhiteSpace(sectionName)) selectQuery = selectQuery.And(x => x.Ident.StartsWith(sectionName.ToString()));

                var query = db.Abbreviation
                    .Where(x => x.IsCanceled == false)?
                    .GroupBy(x => x.Identifier)
                    .Select(n => n.OrderByDescending(x => x.Version).FirstOrDefault())
                    .Where(selectQuery)
                    .ToList();


                fm_rgv.DataSource = query;

                fm_rgv.Refresh();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void fm_rgv_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            try
            {
                foreach (GridViewDataColumn col in fm_rgv.Columns)
                {
                    if (fileProp.ContainsKey(col.Name))
                    {
                        col.IsVisible = fileProp[col.Name].Visible;
                        col.ReadOnly = fileProp[col.Name].ReadOnly;
                        col.DisableHTMLRendering = !fileProp[col.Name].RenderHTML;
                        if (fileProp[col.Name].MaxWidth > 0) col.MaxWidth = fileProp[col.Name].MaxWidth;
                    }
                    else
                    {
                        col.ReadOnly = true;
                    }
                }
                GridViewDataColumn dataColumn = e.CellElement.ColumnInfo as GridViewDataColumn;
                if (dataColumn != null && dataColumn.DisableHTMLRendering == false)
                {
                    e.CellElement.Text = $@"<html>{e.CellElement.Value}</html>";
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            try
            {
                AbbrevForm frm = new AbbrevForm();
                frm.db = db;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AddAbbrev(frm.formSource, frm.isNewDocument);
                    UpdateDataGridView();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void AddAbbrev(Abbreviation abbrev, bool isNew)
        {
            try
            {
                
                if (isNew)
                {
                    abbrev.Identifier = Guid.NewGuid();
                    abbrev.IdKey = "ABBR-" + abbrev.Identifier;
                    db.Abbreviation.Add(abbrev);
                }
                else
                {
                    // Require to compute next version
                    // Current version number may be not last, if last one is Canceled
                    int? ver = db.Abbreviation?
                        .AsNoTracking()
                        .Where(x => x.Identifier == abbrev.Identifier)?
                        .OrderByDescending(x => x.Version)
                        .FirstOrDefault()
                        ?.Version;
                    abbrev.Version = ver + 1 ?? 1;

                    db.Entry(abbrev).State = EntityState.Modified;
                    db.Abbreviation.Add(abbrev);

                }
                abbrev.Created = Lib.GetServerDate() ?? DateTime.UtcNow;
                abbrev.UserId = Globals.CurrentUser.id;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void fm_rgv_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (fm_rgv.CurrentRow?.DataBoundItem == null) return;
                if (fm_rgv.CurrentRow.DataBoundItem is Abbreviation)
                {
                    var selectedItem = fm_rgv.CurrentRow;
                    int id = ((Abbreviation)fm_rgv.CurrentRow.DataBoundItem).id;
                    Abbreviation abbrev = db.Abbreviation.FirstOrDefault(x => x.id == id);
                    AbbrevForm frm = new AbbrevForm(abbrev) { db = db };
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        AddAbbrev(frm.formSource, frm.isNewDocument);
                        UpdateDataGridView();
                        foreach (GridViewRowInfo row in fm_rgv.ChildRows)
                        {
                            if (((Abbreviation)row.DataBoundItem).Identifier == frm.formSource.Identifier)
                            {
                                row.IsCurrent = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_History_Click(object sender, EventArgs e)
        {
            try
            {
                if (fm_rgv.CurrentRow?.DataBoundItem == null) return;
                if (fm_rgv.CurrentRow.DataBoundItem is Abbreviation)
                {
                    AbbrevVersion frm = new AbbrevVersion
                    {
                        db = db,
                        Identifier = ((Abbreviation)fm_rgv.CurrentRow.DataBoundItem).Identifier
                    };
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void btn_Filter_Click(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (FormLoaded) UpdateDataGridView();
        }

        private async void btn_Import_Click(object sender, EventArgs e)
        {
            try
            {
                btn_Import.Enabled = false;
                string abbrevXml = Properties.Resources.EV_GEN_2_2_en_GB;
                string abbrevXmlLv = Properties.Resources.EV_GEN_2_2_lv_LV;
                var lang = db?.LanguageReference?.Local?.ToList();
                var langEng = lang?.FirstOrDefault(x => x.Value == "en-GB")?.id;
                var langLv = lang?.FirstOrDefault(x => x.Value == "lv-LV")?.id;
                XmlSerializer serializer = new XmlSerializer(typeof(GEN22));
                using (TextReader reader = new StringReader(abbrevXml))
                {
                    GEN22 resultingMessage = (GEN22) serializer.Deserialize(reader);

                    List<Abbreviation> abrList = new List<Abbreviation>();
                    Abbreviation abr;

                    foreach (Abbreviationdescription item in resultingMessage.Abbreviationdescription)
                    {
                        string details = "";
                        GetStringRecursively(item.Abbreviationdetails.Items, ref details);
                        abrList.Add(
                            new Abbreviation
                            {
                                Identifier = Guid.NewGuid(),
                                Version = 1,
                                EffectivedateFrom = DateTime.Now.Date,
                                Created = DateTime.Now,
                                Details = details,
                                IdKey = item.id,
                                Ident = String.Join("", item.Abbreviationident.Text),
                                LanguageReferenceId = langEng,
                                UserId = Globals.CurrentUser.id
                            });
                    }

                    db.Abbreviation.AddRange(abrList);
                    await db.SaveChangesAsync();
                    db.SetDBConfiguration(Cfg.IsAbbreviationAdded, true);
                    btn_Import.Visible = false;
                    UpdateDataGridView();
                }
                using (TextReader reader = new StringReader(abbrevXmlLv))
                {
                    GEN22 resultingMessage = (GEN22)serializer.Deserialize(reader);

                    List<Abbreviation> abrList = new List<Abbreviation>();
                    Abbreviation abr;

                    foreach (Abbreviationdescription item in resultingMessage.Abbreviationdescription)
                    {
                        string details = "";
                        GetStringRecursively(item.Abbreviationdetails.Items, ref details);
                        abrList.Add(
                            new Abbreviation
                            {
                                Identifier = Guid.NewGuid(),
                                Version = 1,
                                EffectivedateFrom = DateTime.Now.Date,
                                Created = DateTime.Now,
                                Details = details,
                                IdKey = item.id,
                                Ident = String.Join("", item.Abbreviationident.Text),
                                LanguageReferenceId = langLv,
                                UserId = Globals.CurrentUser.id
                            });
                    }

                    db.Abbreviation.AddRange(abrList);
                    await db.SaveChangesAsync();
                    db.SetDBConfiguration(Cfg.IsAbbreviationAdded, true);
                    btn_Import.Visible = false;
                    UpdateDataGridView();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
            finally
            {
                btn_Import.Enabled = true;
            }
        }

        private void GetStringRecursively(object[] Items, ref string output, string tag = "")
        {
            try
            {
                foreach (var item in Items)
                {
                    if (item is string) output += (string.IsNullOrEmpty(tag)) ? (string)item : $@"<{tag}>{(string)item}</{tag}>";
                    else if (item is XML.br) output += "<br/>";
                    else if ((item as em)?.Items != null) GetStringRecursively(((em) item).Items, ref output,"em");
                    else if ((item as strong)?.Items != null) GetStringRecursively(((strong) item).Items, ref output, "strong");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }

}
