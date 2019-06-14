using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AIP.DB;
using EntityFramework.Extensions;
using Telerik.WinControls;
using Telerik.WinControls.Zip;
using Telerik.WinForms.Documents;
using Telerik.WinForms.Documents.FormatProviders.Html;
using Telerik.WinForms.Documents.FormatProviders.Pdf;
using Telerik.WinForms.Documents.FormatProviders.Rtf;
using Telerik.WinForms.Documents.Layout;
using Telerik.WinForms.Documents.Model;
using Telerik.WinForms.Documents.TextSearch;
using FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeight;
using ZipInputStream = Ionic.Zip.ZipInputStream;

namespace AIP.GUI.Classes
{
    public class RichTextLib
    {
        /// <summary>
        /// DocType enum defined in the DB Enums
        /// </summary>
        private static DocType docType = DocType.HTML;

        /// <summary>
        /// DocType enum defined in the DB Enums
        /// </summary>
        private static PageType pageType = PageType.Cover;

        /// <summary>
        /// Source file (if not db entry)
        /// </summary>
        private readonly string sourceFile = "";

        /// <summary>
        /// Target/Preview file path
        /// </summary>
        private readonly string targetFile = "";

        /// <summary>
        /// Intermediate file path
        /// </summary>
        private readonly string intermediateFile = "";

        /// <summary>
        /// DB object imported from Main form
        /// </summary>
        private eAIPContext db;

        private bool regenerate = false;
        public RichTextLib(PageType _pageType, DocType _docType, eAIPContext _db)
        {
            try
            {
                docType = _docType;
                pageType = _pageType;
                db = _db;
                sourceFile = $@"Templates/AIP/Page_{pageType.ToString()}_{docType.ToString()}_{Lib.CurrentAIP.lang}.rtf";
                //targetFile = Path.Combine(Lib.CurrentDataDir, $@"page_{pageType.ToString()}.{docType.ToString().ToLowerInvariant()}");
                string txt_airac = Lib.IsAIRAC(Lib.CurrentAIP.Effectivedate) ? "-AIRAC" : "";
                string dateAip = Lib.CurrentAIP.Effectivedate.ToString("yyyy-MM-dd") + txt_airac;
                string country = Lib.CurrentAIP.ICAOcountrycode.ToUpperInvariant();
                string lang = (docType == DocType.HTML) ? Lib.CurrentAIP.lang : Lib.CurrentAIP.lang.Substring(0, 2); // en-GB or en
                targetFile = (docType == DocType.HTML) ?
                    Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", "eAIP", $@"{country}-cover-{lang}.html") :
                    Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "pdf", $@"{country}_cover_{lang}.pdf")
                    ;
                intermediateFile = (docType == DocType.HTML) ?
                        Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "html", "eAIP", $@"{country}-cover-{lang}1.rtf") :
                        Path.Combine(Lib.OutputDirTemplate.Replace("{DATE}", dateAip), "pdf", $@"{country}_cover_{lang}1.rtf")
                    ;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public void ResetTemplate()
        {
            try
            {
                var page = db.AIPPage.Where(x => x.eAIPID == Lib.CurrentAIP.id &&
                                      x.PageType == pageType &&
                                      x.DocType == docType)
                    .Include(x => x.AIPPageData)
                    .FirstOrDefault();

                if (page != null) Save(GetSource(), page);

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public Dictionary<string, string> GetReplacementList()
        {
            try
            {
                CultureInfo ci = new CultureInfo("en-US");
                // Preparing to collect
                string txt_airac = Lib.IsAIRAC(Lib.CurrentAIP.Effectivedate) ? "AIRAC" : "";


                return new Dictionary<string, string>()
                {
                    {"{EFFECTIVE_DATE}", Lib.CurrentAIP.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant() ?? ""},
                    {"{PUBLICATION_DATE}", Lib.CurrentAIP.Publicationdate?.ToString("dd MMM yyyy", ci).ToUpperInvariant() ?? ""},
                    {"{AMMENDMENTS}", Lib.CurrentAIP.Amendment?.Number +"/"+Lib.CurrentAIP.Amendment?.Year},
                    {"{AIRAC}", txt_airac},
                    {"{GEN}", ""},
                    {"{ENR}", ""},
                    {"{AD}", ""},
                    {"{HAND}", "Nil"},
                    {"{NOTAM}", ""},
                    {"{AIC}", ""},
                    {"{SUP}", ""},
                    {"{AIC_CANCEL}", ""},
                    {"{SUP_CANCEL}", ""}
                };
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public RadDocument ReplaceAllMatches(RadDocument document, string toSearch, string toReplaceWith)
        {
            try
            {
                DocumentTextSearch search = new DocumentTextSearch(document);
                List<TextRange> rangesTrackingDocumentChanges = new List<TextRange>();

                foreach (var textRange in search.FindAll(toSearch))
                {
                    TextRange newRange = new TextRange(new DocumentPosition(textRange.StartPosition, true), new DocumentPosition(textRange.EndPosition, true));
                    rangesTrackingDocumentChanges.Add(newRange);
                }

                foreach (var textRange in rangesTrackingDocumentChanges)
                {
                    document.CaretPosition.MoveToPosition(textRange.StartPosition);
                    document.Selection.AddSelectionStart(textRange.StartPosition);
                    document.Selection.AddSelectionEnd(textRange.EndPosition);
                    var docEdit = new RadDocumentEditor(document);
                    docEdit.Delete(false);
                    document.Selection.Clear();
                    if (toReplaceWith != null) docEdit.Insert(toReplaceWith);
                    textRange.StartPosition.Dispose();
                    textRange.EndPosition.Dispose();
                }
                return document;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public RadDocument ReplaceTable(RadDocument document, string toSearch, Table table)
        {
            try
            {
                DocumentTextSearch search = new DocumentTextSearch(document);
                List<TextRange> rangesTrackingDocumentChanges = new List<TextRange>();

                foreach (var textRange in search.FindAll(toSearch))
                {
                    TextRange newRange = new TextRange(new DocumentPosition(textRange.StartPosition, true), new DocumentPosition(textRange.EndPosition, true));
                    rangesTrackingDocumentChanges.Add(newRange);
                }

                foreach (var textRange in rangesTrackingDocumentChanges)
                {
                    document.CaretPosition.MoveToPosition(textRange.StartPosition);
                    document.Selection.AddSelectionStart(textRange.StartPosition);
                    document.Selection.AddSelectionEnd(textRange.EndPosition);
                    var docEdit = new RadDocumentEditor(document);
                    docEdit.Delete(false);
                    document.Selection.Clear();
                    if (table != null) docEdit.InsertTable(table);
                    textRange.StartPosition.Dispose();
                    textRange.EndPosition.Dispose();
                }
                return document;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public RadDocument InitializeSource(RadDocument document, ref AIPPage page)
        {
            try
            {
                page = db.AIPPage
                    .Where(x => x.eAIPID == Lib.CurrentAIP.id &&
                                x.PageType == pageType &&
                                x.DocType == docType)
                    .Include(x => x.AIPPageData)
                    .FirstOrDefault();

                if (regenerate)
                {
                    return GetSource();
                }



                if (page == null) return GetSource();
                else
                {
                    string hash = Lib.SHA1(page.AIPPageData.Data);
                    if (!hash.Equals(page.AIPPageData.Hash))
                    {
                        MessageBox.Show($@"Incorrect file hash received, file corrupted!{Environment.NewLine}Stored hash: {page.AIPPageData.Hash}{Environment.NewLine}Calculated hash: {hash}");
                        return new RadDocument();
                    }
                    byte[] file = Decompress(page.AIPPageData.Data);
                    Stream stream = new MemoryStream(file);
                    RtfFormatProvider provider = new RtfFormatProvider();
                    return provider.Import(stream);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public Table GetInsertedDeleted()
        {

            try
            {
                if (Lib.PrevousAIP == null) return null;
                List<PDFPage> currentAipPages = db.PDFPage
                    .AsNoTracking()
                    .Where(x => x.eAIPID == Lib.CurrentAIP.id)
                    .Include(x => x.PageeAIP)
                    .Include(x => x.AirportHeliport)
                    .OrderBy(x => x.Section)
                    .ThenBy(x => x.AirportHeliportID)
                    .ThenBy(x => x.Page)
                    .ToList();
                List<PDFPage> previousAipPages = db.PDFPage
                    .AsNoTracking()
                    .Where(x => x.eAIPID == Lib.PrevousAIP.id)
                    .Include(x => x.PageeAIP)
                    .Include(x => x.AirportHeliport)
                    .OrderBy(x => x.Section)
                    .ThenBy(x => x.AirportHeliportID)
                    .ThenBy(x => x.Page)
                    .ToList();

                var sections = Lib.SectionByAttribute(SectionParameter.TextSection);

                List<Dictionary<string, string>> changedPagesList = new List<Dictionary<string, string>>();
                var airportHeliportSection = false;

                if (sections.Count != 0)
                {
                    foreach (var section in sections)
                    {
                        var pdfPagesCurrentAip = currentAipPages.Where(x => x.Section == section).ToList();
                        var pdfPagesPreviousAip = previousAipPages?.Where(x => x.Section == section).ToList();

                        var length = pdfPagesCurrentAip.Count >= pdfPagesPreviousAip.Count
                            ? pdfPagesCurrentAip.Count
                            : pdfPagesPreviousAip.Count;


                        for (var i = 0; i < length; i++)
                        {
                            Dictionary<string, string> items = new Dictionary<string, string>();

                            if (pdfPagesCurrentAip.Count > 0 && pdfPagesPreviousAip.Count > 0)
                            {
                                if (pdfPagesCurrentAip.ElementAtOrDefault(i) != null && pdfPagesPreviousAip.ElementAtOrDefault(i) != null)
                                {
                                    if (pdfPagesCurrentAip[i].PageeAIP.Effectivedate ==
                                        pdfPagesPreviousAip[i].PageeAIP.Effectivedate) continue;
                                    BuildCollection(ref items, i, pdfPagesCurrentAip, pdfPagesPreviousAip, airportHeliportSection);
                                    changedPagesList.Add(items);
                                }
                                else if ((pdfPagesCurrentAip.ElementAtOrDefault(i) != null && pdfPagesPreviousAip.ElementAtOrDefault(i) == null) ||
                                            (pdfPagesCurrentAip.ElementAtOrDefault(i) == null && pdfPagesPreviousAip.ElementAtOrDefault(i) != null))
                                {
                                    BuildCollection(ref items, i, pdfPagesCurrentAip, pdfPagesPreviousAip, airportHeliportSection);
                                    changedPagesList.Add(items);
                                }
                            }
                            else if ((pdfPagesCurrentAip.Count > 0 && pdfPagesPreviousAip.Count == 0) ||
                                        (pdfPagesCurrentAip.Count == 0 && pdfPagesPreviousAip.Count > 0))
                            {
                                BuildCollection(ref items, i, pdfPagesCurrentAip, pdfPagesPreviousAip, airportHeliportSection);
                                changedPagesList.Add(items);
                            }

                        }

                    }
                }

                var currentAirportHeliportList = currentAipPages.Where(x => x.AirportHeliportID != null).ToList();
                var previousAirportHeliportList = previousAipPages.Where(x => x.AirportHeliportID != null).ToList();

                var airportHeliportList = currentAirportHeliportList.Concat(previousAirportHeliportList)
                    .Distinct(new DistinctItemComparer()).ToList();

                if (airportHeliportList.Count > 0)
                {
                    airportHeliportSection = true;
                    foreach (var item in airportHeliportList)
                    {
                        var pdfPagesCurrentAip = currentAirportHeliportList
                            .Where(x => x.AirportHeliport?.Name.Equals(item.AirportHeliport.Name) == true).ToList();
                        var pdfPagesPreviousAip = previousAirportHeliportList
                            .Where(x => x.AirportHeliport?.Name.Equals(item.AirportHeliport.Name) == true).ToList();

                        var length = pdfPagesCurrentAip.Count >= pdfPagesPreviousAip.Count
                            ? pdfPagesCurrentAip.Count
                            : pdfPagesPreviousAip.Count;

                        for (var i = 0; i < length; i++)
                        {
                            Dictionary<string, string> items = new Dictionary<string, string>();

                            if (pdfPagesCurrentAip.Count > 0 && pdfPagesPreviousAip.Count > 0)
                            {
                                if (pdfPagesCurrentAip.ElementAtOrDefault(i) != null && pdfPagesPreviousAip.ElementAtOrDefault(i) != null)
                                {
                                    if (pdfPagesCurrentAip[i].PageeAIP.Effectivedate ==
                                        pdfPagesPreviousAip[i].PageeAIP.Effectivedate) continue;
                                    BuildCollection(ref items, i, pdfPagesCurrentAip, pdfPagesPreviousAip, airportHeliportSection);
                                    changedPagesList.Add(items);
                                }
                                else if ((pdfPagesCurrentAip.ElementAtOrDefault(i) != null && pdfPagesPreviousAip.ElementAtOrDefault(i) == null) ||
                                            (pdfPagesCurrentAip.ElementAtOrDefault(i) == null && pdfPagesPreviousAip.ElementAtOrDefault(i) != null))
                                {
                                    BuildCollection(ref items, i, pdfPagesCurrentAip, pdfPagesPreviousAip, airportHeliportSection);
                                    changedPagesList.Add(items);
                                }
                            }
                            else if ((pdfPagesCurrentAip.Count > 0 && pdfPagesPreviousAip.Count == 0) ||
                                        (pdfPagesCurrentAip.Count == 0 && pdfPagesPreviousAip.Count > 0))
                            {
                                BuildCollection(ref items, i, pdfPagesCurrentAip, pdfPagesPreviousAip, airportHeliportSection);
                                changedPagesList.Add(items);
                            }

                        }
                    }

                }
                //PDFManager.GenerateCoverPagePdf(changedPagesList);
                Table table = BuildCoverPageTable(changedPagesList);

                return table;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }


        }

        private void BuildCollection(ref Dictionary<string, string> items, int i, List<PDFPage> pdfPagesCurrentAip, List<PDFPage> pdfPagesPreviousAip, bool airportHeliportSection)
        {
            try
            {
                if (pdfPagesCurrentAip.Count != 0)
                {
                    if (pdfPagesCurrentAip.ElementAtOrDefault(i) != null)
                    {
                        string sectionName = airportHeliportSection ? $@"" + pdfPagesCurrentAip[i].AirportHeliport.LocationIndicatorICAO + " " + pdfPagesCurrentAip[i].Section.ToName().Substring(0, 3).Insert(2, " ") + "" : pdfPagesCurrentAip[i].Section.ToName();

                        if ((pdfPagesCurrentAip[i].Page % 2) == 0)
                        {
                            items.Add("Inserted_1",
                                $@"{sectionName} - {pdfPagesCurrentAip[i - 1].Page}/{pdfPagesCurrentAip[i].Page}");
                            items.Add("Inserted_2",
                                $@"{pdfPagesCurrentAip[i - 1].PageeAIP.Effectivedate:dd MMM yyyy} / {
                                        pdfPagesCurrentAip[i].PageeAIP.Effectivedate:dd MMM yyyy}");
                        }
                        else
                        {
                            items.Add("Inserted_1",
                                $@"{sectionName} - {pdfPagesCurrentAip[i].Page}/{pdfPagesCurrentAip[i + 1].Page}");
                            items.Add("Inserted_2",
                                $@"{pdfPagesCurrentAip[i].PageeAIP.Effectivedate:dd MMM yyyy} / {
                                        pdfPagesCurrentAip[i + 1].PageeAIP.Effectivedate:dd MMM yyyy}");
                        }
                    }
                    else
                    {
                        items.Add("Inserted_1", " ");
                        items.Add("Inserted_2", " ");
                    }
                }

                if (pdfPagesPreviousAip.Count != 0)
                {
                    if (pdfPagesPreviousAip.ElementAtOrDefault(i) != null)
                    {
                        string sectionName = airportHeliportSection ? $@"" + pdfPagesPreviousAip[i].AirportHeliport.LocationIndicatorICAO + " " + pdfPagesPreviousAip[i].Section.ToName().Substring(0, 3).Insert(2, " ") + "" : pdfPagesPreviousAip[i].Section.ToName();

                        if ((pdfPagesPreviousAip[i].Page % 2) == 0)
                        {
                            items.Add("Deleted_1",
                                $@"{sectionName} - {pdfPagesPreviousAip[i - 1].Page}/{pdfPagesPreviousAip[i].Page}");
                            items.Add("Deleted_2",
                                $@"{pdfPagesPreviousAip[i - 1].PageeAIP.Effectivedate:dd MMM yyyy} / {
                                    pdfPagesPreviousAip[i].PageeAIP.Effectivedate:dd MMM yyyy}");
                        }
                        else
                        {
                            items.Add("Deleted_1",
                                $@"{sectionName} - {pdfPagesPreviousAip[i].Page}/{pdfPagesPreviousAip[i + 1].Page}");
                            items.Add("Deleted_2",
                                $@"{pdfPagesPreviousAip[i].PageeAIP.Effectivedate:dd MMM yyyy} / {
                                    pdfPagesPreviousAip[i + 1].PageeAIP.Effectivedate:dd MMM yyyy}");
                        }
                    }
                    else
                    {
                        items.Add("Deleted_1", " ");
                        items.Add("Deleted_2", " ");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }

        }

        private Table BuildCoverPageTable(List<Dictionary<string, string>> items)
        {
            try
            {

                Table table = new Table();
                table.HorizontalAlignment = RadHorizontalAlignment.Right;

                TableRow row1 = new TableRow();
                row1.Height = Unit.PointToDip(11);

                TableCell cell1 = new TableCell();
                Paragraph p1 = new Paragraph();
                Span s1 = new Span();
                s1.Text = !String.IsNullOrEmpty(Tpl.Text("Cover_17")) ? Tpl.Text("Cover_17") : " ";
                s1.FontFamily = new Telerik.WinControls.RichTextEditor.UI.FontFamily("Arial");
                s1.FontSize = Unit.PointToDip(11);
                s1.FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold;
                p1.Inlines.Add(s1);
                cell1.ColumnSpan = 2;
                cell1.Blocks.Add(p1);
                cell1.TextAlignment = RadTextAlignment.Left;
                cell1.VerticalAlignment = RadVerticalAlignment.Center;

                TableCell cell2 = new TableCell();
                Paragraph p2 = new Paragraph();
                Span s2 = new Span();
                s2.Text = !String.IsNullOrEmpty(Tpl.Text("Cover_18")) ? Tpl.Text("Cover_18") : " ";
                s2.FontFamily = new Telerik.WinControls.RichTextEditor.UI.FontFamily("Arial");
                s2.FontSize = Unit.PointToDip(11);
                s2.FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold;
                p2.Inlines.Add(s2);
                cell2.ColumnSpan = 2;
                cell2.Blocks.Add(p2);
                cell2.TextAlignment = RadTextAlignment.Left;
                cell2.VerticalAlignment = RadVerticalAlignment.Center;

                row1.Cells.Add(cell1);
                row1.Cells.Add(cell2);

                TableRow row2 = new TableRow();
                row2.Height = Unit.PointToDip(11);

                TableCell cellEmpty1 = new TableCell();
                Paragraph pEmpty1 = new Paragraph();
                Span sEmpty1 = new Span();
                sEmpty1.Text = " ";
                pEmpty1.Inlines.Add(sEmpty1);
                cellEmpty1.ColumnSpan = 4;
                cellEmpty1.Blocks.Add(pEmpty1);

                row2.Cells.Add(cellEmpty1);

                table.Rows.Add(row1);
                table.Rows.Add(row2);

                foreach (var item in items)
                {
                    TableRow row = new TableRow();
                    row.Height = Unit.PointToDip(11);

                    TableCell cellInsSection = new TableCell();
                    Paragraph pInsSection = new Paragraph();
                    Span sInsSection = new Span();
                    sInsSection.FontSize = Unit.PointToDip(8);
                    sInsSection.FontFamily = new Telerik.WinControls.RichTextEditor.UI.FontFamily("Arial");
                    sInsSection.Text = item.ContainsKey("Inserted_1") ? item["Inserted_1"] : " ";
                    pInsSection.Inlines.Add(sInsSection);
                    cellInsSection.Blocks.Add(pInsSection);
                    cellInsSection.TextAlignment = RadTextAlignment.Left;
                    cellInsSection.VerticalAlignment = RadVerticalAlignment.Center;

                    TableCell cellInsDate = new TableCell();
                    Paragraph pInsDate = new Paragraph();
                    Span sInsDate = new Span();
                    sInsDate.FontSize = Unit.PointToDip(8);
                    sInsDate.FontFamily = new Telerik.WinControls.RichTextEditor.UI.FontFamily("Arial");
                    sInsDate.Text = item.ContainsKey("Inserted_2") ? item["Inserted_2"] : " ";
                    pInsDate.Inlines.Add(sInsDate);
                    cellInsDate.Blocks.Add(pInsDate);
                    cellInsDate.TextAlignment = RadTextAlignment.Right;
                    cellInsDate.VerticalAlignment = RadVerticalAlignment.Center;

                    TableCell cellDelSection = new TableCell();
                    Paragraph pDelSection = new Paragraph();
                    Span sDelSection = new Span();
                    sDelSection.FontSize = Unit.PointToDip(8);
                    sDelSection.FontFamily = new Telerik.WinControls.RichTextEditor.UI.FontFamily("Arial");
                    sDelSection.Text = item.ContainsKey("Deleted_1") ? item["Deleted_1"] : " ";
                    pDelSection.Inlines.Add(sDelSection);
                    cellDelSection.Blocks.Add(pDelSection);
                    cellDelSection.TextAlignment = RadTextAlignment.Left;
                    cellDelSection.VerticalAlignment = RadVerticalAlignment.Center;

                    TableCell cellDelDate = new TableCell();
                    Paragraph pDelDate = new Paragraph();
                    Span sDelDate = new Span();
                    sDelDate.FontSize = Unit.PointToDip(8);
                    sDelDate.FontFamily = new Telerik.WinControls.RichTextEditor.UI.FontFamily("Arial");
                    sDelDate.Text = item.ContainsKey("Deleted_2") ? item["Deleted_2"] : " ";
                    pDelDate.Inlines.Add(sDelDate);
                    cellDelDate.Blocks.Add(pDelDate);
                    cellDelDate.TextAlignment = RadTextAlignment.Right;
                    cellDelDate.VerticalAlignment = RadVerticalAlignment.Center;

                    row.Cells.Add(cellInsSection);
                    row.Cells.Add(cellInsDate);
                    row.Cells.Add(cellDelSection);
                    row.Cells.Add(cellDelDate);

                    table.Rows.Add(row);
                }
                return table;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }

        }

        public RadDocument GetSource()
        {
            try
            {
                RadDocument document;
                RtfFormatProvider provider = new RtfFormatProvider();
                using (FileStream inputStream = File.OpenRead(sourceFile))
                {
                    document = provider.Import(inputStream);
                }
                return document;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public RadDocument GetSourceFromIntermediate()
        {
            try
            {
                RadDocument document = new RadDocument();
                RtfFormatProvider provider = new RtfFormatProvider();
                using (FileStream inputStream = File.OpenRead(intermediateFile))
                {
                    document = provider.Import(inputStream);
                }

                return document;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public RadDocument GetSourceWithReplace(RadDocument document)
        {
            try
            {
                var list = GetReplacementList();
                foreach (var item in list)
                {
                    ReplaceAllMatches(document, item.Key, item.Value);
                }
                ReplaceTable(document, "{INSERTED_DELETED_TABLE}", GetInsertedDeleted());

                return document;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public RadDocument GetSourceWithReplace()
        {
            try
            {
                RadDocument document;
                var list = GetReplacementList();
                RtfFormatProvider provider = new RtfFormatProvider();
                using (FileStream inputStream = File.OpenRead(sourceFile))
                {
                    document = provider.Import(inputStream);
                    foreach (var item in list)
                    {
                        ReplaceAllMatches(document, item.Key, item.Value);
                    }
                    ReplaceTable(document, "{INSERTED_DELETED_TABLE}", GetInsertedDeleted());
                }
                return document;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public bool SaveFromSource()
        {
            try
            {
                return Save(GetSourceWithReplace());
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }



        public bool Generate(AIPPage page, out string errorText)
        {
            errorText = String.Empty;
            try
            {
                string hash = Lib.SHA1(page.AIPPageData.Data);
                if (!hash.Equals(page.AIPPageData.Hash))
                {
                    errorText = $@"Incorrect file hash received for {page.PageType} with type {page.DocType}, file corrupted!{Environment.NewLine}Stored hash: {page.AIPPageData.Hash}{Environment.NewLine}Calculated hash: {hash}";
                }
                byte[] file = Decompress(page.AIPPageData.Data);
                Stream stream = new MemoryStream(file);
                RtfFormatProvider importProvider = new RtfFormatProvider();
                RadDocument document = importProvider.Import(stream);
                dynamic provider;
                if (docType.HasFlag(DocType.HTML)) provider = new HtmlFormatProvider();
                else provider = new PdfFormatProvider();
                using (Stream output = File.OpenWrite(targetFile))
                {
                    provider.Export(document, output);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public bool Generate(RadDocument document)
        {
            try
            {
                dynamic provider;
                if (docType.HasFlag(DocType.HTML)) provider = new HtmlFormatProvider();
                else provider = new PdfFormatProvider();
                using (Stream output = File.OpenWrite(targetFile))
                {
                    provider.Export(document, output);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public bool GenerateIntermediateFile(RadDocument document)
        {
            try
            {
                RtfFormatProvider provider = new RtfFormatProvider();
                using (Stream output = File.OpenWrite(intermediateFile))
                {
                    provider.Export(document, output);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }
        public void Preview(RadDocument document)
        {
            try
            {
                if (!DeleteFile(intermediateFile)) return;
                if (!DeleteFile(targetFile)) return;

                GenerateIntermediateFile(document);
                var docNew = GetSourceFromIntermediate();
                RadDocument doc = GetSourceWithReplace(docNew);
                if (doc != null)
                {
                    if (Generate(doc))
                    {
                        System.Diagnostics.Process.Start(targetFile);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    if (new FileInfo(filePath).IsFileLocked())
                    {
                        ErrorLog.ShowWarning($@"File {Path.GetFileName(filePath)} is already opened, locked and can`t be rewritten! Please close opened file and try again.");
                        return false;
                    }
                    else
                    {
                        File.Delete(filePath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Save(RadDocument document, AIPPage page = null)
        {
            try
            {
                var provider = new RtfFormatProvider();
                MemoryStream ms = new MemoryStream();
                provider.Export(document, ms);
                byte[] file = Compress(ms.ToArray());
                string hash = Lib.SHA1(file);
                if (page == null)
                {
                    AIPPage newPage = new AIPPage();
                    newPage.DocType = docType;
                    newPage.PageType = pageType;
                    newPage.eAIP = Lib.CurrentAIP;
                    newPage.AIPPageData = new AIPPageData();
                    newPage.AIPPageData.Data = file;
                    newPage.AIPPageData.Hash = hash;
                    newPage.CreatedUserId = Globals.CurrentUser.id;
                    newPage.CreatedDate = Lib.GetServerDate() ?? DateTime.UtcNow;
                    newPage.ChangedUserId = Globals.CurrentUser.id;
                    newPage.ChangedDate = Lib.GetServerDate() ?? DateTime.UtcNow;
                    db.AIPPageData.Add(newPage.AIPPageData);
                    db.AIPPage.Add(newPage);
                }
                else
                {
                    page.ChangedUserId = Globals.CurrentUser.id;
                    page.ChangedDate = Lib.GetServerDate() ?? DateTime.UtcNow;
                    page.AIPPageData.Data = file;
                    page.AIPPageData.Hash = hash;
                    db.Entry(page).State = EntityState.Modified;
                }
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        static byte[] Compress(byte[] data)
        {
            try
            {
                using (var compressedStream = new MemoryStream())
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Close();
                    return compressedStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        static byte[] Decompress(byte[] data)
        {
            try
            {
                using (var compressedStream = new MemoryStream(data))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }
    }

    class DistinctItemComparer : IEqualityComparer<PDFPage>
    {

        public bool Equals(PDFPage x, PDFPage y)
        {
            return x.AirportHeliport.Name.Equals(y.AirportHeliport.Name);
        }

        public int GetHashCode(PDFPage obj)
        {
            return obj.AirportHeliport.Name.GetHashCode();
        }
    }
}
