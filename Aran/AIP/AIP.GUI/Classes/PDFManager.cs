using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using System.IO;
using AIP.DB;
using System.Globalization;
using iTextSharp.text;
using System.Data.Entity;

namespace AIP.GUI.Classes
{
    internal static class PDFManager
    {
        private static BaseFont BaseFont;

        static PDFManager()
        {
            try
            {
                // TODO: Rewrite to local path Templates/AIP/ARIALUNI.TTF
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
                if (!File.Exists(fontPath)) return;
                BaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static Font GetFont(int? size = null, int? style = null, BaseColor baseColor = null)
        {
            try
            {
                return new Font(BaseFont, size ?? 8, style ?? Font.NORMAL, baseColor ?? BaseColor.BLACK);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static bool DivideMultiPagedPdf(string PDFPath)
        {
            try
            {
                PdfReader reader = null;
                Document sourceDocument = null;
                PdfCopy pdfCopyProvider = null;
                PdfImportedPage importedPage = null;

                reader = new PdfReader(PDFPath);
                sourceDocument = new Document(reader.GetPageSizeWithRotation(1));

                sourceDocument.Open();
                string newPath = "";
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    Document document = new Document();
                    newPath = "C:/Users/ShahinK/Desktop/LGSSoft_fop/2018-03-29-AIRAC/graphics/eAIP/ENR1142018020112554205290000_" + i + ".pdf";

                    pdfCopyProvider = new PdfCopy(document, new FileStream(newPath, FileMode.Create));

                    document.Open();
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                    document.Close();

                }
                reader.Close();

                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        public static bool isPageAlbum(PdfReader reader, int pageNum)
        {
            try
            {
                Rectangle currentPageRectangle = reader?.GetPageSizeWithRotation(pageNum);
                return currentPageRectangle?.Width > currentPageRectangle?.Height;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return false;
            }
        }

        /// <summary>
        /// Return list of the Graphic-file
        /// </summary>
        /// <param name="aipFile"></param>
        /// <param name="filePath"></param>
        /// <param name="embedToPDF"></param>
        /// <returns></returns>
        public static List<string> GetGraphicFiles(AIPFile aipFile, string filePath, bool embedToPDF)
        {
            try
            {
                // If not required to embed into PDF, just insert show attribute with HTML value
                string fileName = Lib.AIPFileName(aipFile);
                string fileType = Lib.GetFileType(aipFile.FileName);
                string fileTitle = aipFile.Title;
                string fileId = $@"{aipFile.id}-{aipFile.Identifier}-{aipFile.Version}";
                string show = "HTML";
                if (!embedToPDF)
                {
                    return
                        new List<string>()
                    {
                        $"<e:Figure><e:Graphic-file id=\"{fileId}\" show=\"{show}\" xlink:show=\"replace\" xlink:href=\"../graphics/eAIP/{fileName}\" Type=\"{fileType}\" Page-name=\"{fileTitle}\" /></e:Figure>"
                    };
                }
                else
                {
                    // Analize PDF, 
                    // if one page and portrait orientation - return it with show="All"
                    // if one page and album orientation - flip to 90 degrees, return: original file path with show="HTML" and flipped file path with show="PDF"
                    // If many pages, return original file path with show="HTML", other files (divide by pages) with show="PDF"
                    PdfReader reader = new PdfReader(filePath);
                    Document sourceDocument = new Document(reader.GetPageSizeWithRotation(1));
                    sourceDocument.Open();
                    int numberOfPages = reader.NumberOfPages;
                    show = "All";

                    if (numberOfPages == 1)
                    {
                        if (isPageAlbum(reader, 1))
                        {
                            return new List<string>()
                        {
                            $"<e:Figure><e:Graphic-file id=\"{fileId}\" show=\"{show}\" xlink:show=\"replace\" xlink:href=\"../graphics/eAIP/{fileName}\" Type=\"{fileType}\" Page-name=\"{fileTitle}\" /></e:Figure>"
                        };
                        }
                        else
                        {
                            return new List<string>()
                        {
                            $"<e:Figure><e:Graphic-file id=\"{fileId}\" show=\"{show}\" xlink:show=\"replace\" xlink:href=\"../graphics/eAIP/{fileName}\" Type=\"{fileType}\" Page-name=\"{fileTitle}\" /></e:Figure>"
                        };
                        }
                    }
                    else
                    {
                        var graphicFilesList = new List<string>
                    {
                        $"<e:Figure><e:Graphic-file id=\"{fileId}\" show=\"HTML\" xlink:show=\"replace\" xlink:href=\"../graphics/eAIP/{fileName}\" Type=\"{fileType}\" Page-name=\"{fileTitle}\" /></e:Figure>"
                    };
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            Document document = new Document();
                            string newPath = filePath.Replace(".pdf", "-" + i.ToString("D2") + ".pdf");
                            string newFileName = Path.GetFileName(newPath);
                            var pdfCopyProvider = new PdfCopy(document, new FileStream(newPath, FileMode.Create));
                            document.Open();
                            var importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                            pdfCopyProvider.AddPage(importedPage);
                            document.Close();
                            graphicFilesList.Add($"<e:Figure><e:Graphic-file id=\"{fileId}\" show=\"PDF\" xlink:show=\"replace\" xlink:href=\"../graphics/eAIP/{newFileName}\" Type=\"{fileType}\" Page-name=\"{fileTitle}\" /></e:Figure>");
                            // Copy file into the same path of the source folder
                            // Require for EC tool embedding
                            File.Copy(newPath, newPath.Replace("eAIP-output", "eAIP-source"), true);
                        }
                        reader.Close();
                        return graphicFilesList;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }



        public static int GetPdfPagesNumber(string PDFPath)
        {
            try
            {
                if (!File.Exists(PDFPath)) return 0;
                PdfReader reader = new PdfReader(PDFPath);
                if (!reader.IsNull())
                {
                    int numberOfPages = reader.NumberOfPages;
                    reader.Close();
                    return numberOfPages;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return 0;
            }
        }

        public static List<PDFPage> GetNewPagesList(eAIPContext db, List<PDFPage> list, string PDFPath, string PDFNewPath, int? airportHeliportId)
        {
            try
            {
                if (!File.Exists(PDFPath)) return null;
                PdfReader reader = new PdfReader(PDFPath);
                Rectangle size = reader.GetPageSizeWithRotation(1);
                Document document = new Document(size);

                FileStream fs = new FileStream(PDFNewPath, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                PdfContentByte cb = writer.DirectContent;

                if (list.Count > reader.NumberOfPages && reader.NumberOfPages != 0)
                {
                    //ErrorLog.Write("count: " + list.Count + ", pages: " + reader.NumberOfPages);
                    //list.RemoveRange(reader.NumberOfPages - 1, list.Count - 1);
                    list = list.Take(reader.NumberOfPages).ToList();
                }

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    if (i > list.Count)
                    {
                        PDFPage pdfPage = new PDFPage();
                        pdfPage.id = 0;
                        pdfPage.eAIPID = Lib.CurrentAIP.id;
                        pdfPage.Page = i;
                        pdfPage.Section = list[0].Section;
                        pdfPage.PageeAIPID = Lib.CurrentAIP.id;
                        pdfPage.AirportHeliportID = airportHeliportId;
                        list.Add(pdfPage);
                    }

                    if (list[i - 1] != null)
                    {
                        list[i - 1].id = 0;
                        list[i - 1].eAIPID = Lib.CurrentAIP.id;
                        list[i - 1].AirportHeliportID = airportHeliportId;
                        //list[i - 1].eAIP = null;
                    }

                    PdfImportedPage page = writer.GetImportedPage(reader, i);

                    StringWriter output = new StringWriter();
                    output.WriteLine(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i, new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy()));

                    string content = output.ToString();


                    // TODO: change to ~~~LAST_PAGE~~~
                    if ((i % 2 == 0 && content.Contains("~~~LAST_PAGE~~~")))
                    {
                        document.NewPage();
                        cb.AddTemplate(page, 0, 0);
                        continue;
                    }
                    else if ((content.Contains("\r\n") && content.Length < 10))
                    {
                        continue;
                    }
                    else if ((content.Contains("~~~LAST_PAGE~~~") && content.Length < 40))
                    {
                        continue;
                    }

                    document.NewPage();
                    cb.AddTemplate(page, 0, 0);


                    if (!content.Contains("~~~UPDATED~~~"))
                    {
                        CultureInfo ci = new CultureInfo("en-US");// For LV = lv-LV?
                        var date = list[i - 1].PageeAIP?.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant();
                        var amdt = Tpl.Text("AIRAC_AMDT") + " " + list[i - 1].PageeAIP?.Amendment?.Number + "/" + list[i - 1].PageeAIP?.Amendment?.Year;

                        var dateFont = GetFont(10);

                        Phrase myDateText = new Phrase(date, dateFont);
                        Phrase myAMDTText = new Phrase(amdt, dateFont);

                        Rectangle rectangle;
                        ColumnText ct = new ColumnText(cb);

                        Rectangle rectangleAMDT;
                        ColumnText ctAMDT = new ColumnText(cb);


                        if (i % 2 != 0)
                        {
                            rectangle = new Rectangle(490, 804, 560, 819);
                            ct.SetSimpleColumn(myDateText, 490, 804, 554, 819, 12, Element.ALIGN_RIGHT);

                            rectangleAMDT = new Rectangle(400, 10, 560, 25);
                            ctAMDT.SetSimpleColumn(myAMDTText, 396, 14, 554, 29, 12, Element.ALIGN_RIGHT);

                        }
                        else
                        {
                            rectangle = new Rectangle(40, 804, 110, 819);
                            ct.SetSimpleColumn(myDateText, 43, 804, 107, 819, 12, Element.ALIGN_LEFT);

                            rectangleAMDT = new Rectangle(40, 10, 200, 25);
                            ctAMDT.SetSimpleColumn(myAMDTText, 43, 14, 200, 29, 12, Element.ALIGN_LEFT);

                        }

                        rectangle.BackgroundColor = BaseColor.WHITE;
                        cb.Rectangle(rectangle);
                        rectangleAMDT.BackgroundColor = BaseColor.WHITE;
                        cb.Rectangle(rectangleAMDT);
                        cb.Stroke();

                        ct.Go();
                        ctAMDT.Go();

                        list[i - 1].eAIPID = Lib.CurrentAIP.id;
                        list[i - 1].eAIP = null;
                        list[i - 1].AirportHeliportID = airportHeliportId;
                        list[i - 1].PageeAIP = null;
                    }
                    else
                    {
                        list[i - 1].eAIPID = Lib.CurrentAIP.id;
                        list[i - 1].eAIP = null;
                        list[i - 1].AirportHeliportID = airportHeliportId;
                        list[i - 1].PageeAIPID = Lib.CurrentAIP.id;
                        list[i - 1].PageeAIP = null;
                    }
                }

                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();

                // Small fix, TODO: make code better
                foreach (var page in list)
                {
                    page.eAIP = Lib.CurrentAIP;
                    page.eAIPID = Lib.CurrentAIP.id;
                    page.AirportHeliportID = airportHeliportId;
                    page.PageeAIP = null;
                }
                return list;
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
                return null;
            }
        }

        public static void GenerateGEN04Section(eAIPContext db, string path)
        {
            try
            {
                Document document = new Document();

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                writer.PageEvent = new PDFManagerGEN04();
                document.Open();

                PdfContentByte cb = writer.DirectContent;

                var defaultFont = GetFont(); // default is 8
                var dateFontSubParts = GetFont(9);
                var dateFontFold = GetFont(10, Font.BOLD);
                var dateFontForParts = GetFont(12);

                const int pageHeight = 765;
                const int pageFooterHeight = 40;
                var docHeight = pageHeight;

                Phrase sectionTtitle = new Phrase(Tpl.MenuFullText("GEN04"), dateFontFold);
                
                ColumnText ctSectionTitle = new ColumnText(cb);
                ctSectionTitle.SetSimpleColumn(sectionTtitle, 73, docHeight, 520, docHeight + 15, 12, Element.ALIGN_LEFT);
                ctSectionTitle.Go();

                List<PDFPage> listGen04 = db.PDFPage
                    .AsNoTracking()
                    .Where(x => x.eAIPID == Lib.CurrentAIP.id)
                    .Include(x => x.PageeAIP)
                    .Include(x => x.AirportHeliport)
                    .ToList()
                    .OrderBy(x => x.Section, new SectionNameComparer())
                    .ThenBy(x => x.AirportHeliportID)
                    .ThenBy(x => x.Page)
                    .ToList();

                CultureInfo ci = new CultureInfo("en-US");

                string secText = null;
                string subSecText = null;
                int part = 1;

                Phrase sectionText;
                Phrase sectionDate;

                ColumnText ctLeft = new ColumnText(cb);
                ColumnText ctRight = new ColumnText(cb);

                docHeight -= 10;

                int pageCount = 1;
                var leftMargin = 73;

                foreach (var item in listGen04)
                {
                    var section = GetSectionNameForGEN04(item.Section.ToName());

                    if (!object.Equals(secText, section))
                    {
                        leftMargin = SetLeftMarginForPage(pageCount);

                        secText = section;
                        docHeight -= 25;
                        if (docHeight < pageFooterHeight)
                        {
                            document.NewPage();
                            docHeight = pageHeight;
                            pageCount++;

                            leftMargin = SetLeftMarginForPage(pageCount);
                        }

                        Phrase sectionParts = new Phrase(Tpl.Text("Part_" + part) + " - " + Tpl.MenuText(section + "_"), dateFontForParts);
                        part++;

                        ColumnText ctSectionParts = new ColumnText(cb);
                        ctSectionParts.SetSimpleColumn(sectionParts, leftMargin, docHeight, leftMargin + 447, docHeight + 15, 12, Element.ALIGN_LEFT);
                        ctSectionParts.Go();
                    }
                    
                    char sectionNumber = GetSectionNumber(section , item.Section.ToString());

                    if (!object.Equals(subSecText, section + " " + sectionNumber))
                    {
                        leftMargin = SetLeftMarginForPage(pageCount);

                        subSecText = section + " " + sectionNumber;

                        docHeight -= 25;
                        if (docHeight < pageFooterHeight)
                        {
                            document.NewPage();
                            docHeight = pageHeight;
                            pageCount++;

                            leftMargin = SetLeftMarginForPage(pageCount);
                        }
                        
                        Phrase sectionSubParts = new Phrase(Tpl.MenuFullText(section + sectionNumber), dateFontSubParts);

                        ColumnText ctSectionSubParts = new ColumnText(cb);
                        ctSectionSubParts.SetSimpleColumn(sectionSubParts, leftMargin, docHeight, leftMargin + 447, docHeight + 15, 12, Element.ALIGN_LEFT);
                        ctSectionSubParts.Go();

                        docHeight -= 10;

                        var subSectionList = listGen04.Where(x => x.Section.ToString().Contains(subSecText.Replace(" " , ""))).ToList();

                        var lastDocHeight = docHeight;
                        var numberOfRows = (docHeight - pageFooterHeight) / 10;

                        var subSectionListSize = subSectionList.Count;

                        var newPage = false;
                        if ((numberOfRows * 3) > subSectionListSize)
                        {
                            numberOfRows = (subSectionListSize % 3) == 0 ? subSectionListSize / 3 : (subSectionListSize / 3) + 1;

                            numberOfRows = numberOfRows == 0 ? 1 : numberOfRows;
                        }
                        else
                        {
                            newPage = true;
                        }
                        
                        var subSectionColumn = 1;
                        var columnRowsCount = 0;
                        var listCount = 0;
                        var lastRowHeight = 0;
                        foreach (var subSectionItem in subSectionList)
                        {
                            listCount++;
                            columnRowsCount++;

                            section = GetSectionNameForGEN04(subSectionItem.Section.ToName());

                            sectionNumber = GetSectionNumber(section, subSectionItem.Section.ToString());

                            sectionText = section.Equals("AD") && (sectionNumber == '2' || sectionNumber == '3') ?
                                new Phrase($@"{subSectionItem.AirportHeliport.LocationIndicatorICAO} {section} {sectionNumber} - {subSectionItem.Page}",
                                    defaultFont) :
                                new Phrase($@"{subSectionItem.Section.ToName().Replace("-", " ")} - {subSectionItem.Page}",
                                    defaultFont);
                            sectionDate = new Phrase(subSectionItem.PageeAIP?.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant(), defaultFont);

                            leftMargin = SetLeftMarginForPage(pageCount);

                            if (subSectionColumn == 2)
                            {
                                leftMargin += 160;
                            }
                            else if(subSectionColumn == 3)
                            {
                                leftMargin += 320;
                            }

                            ctLeft.SetSimpleColumn(sectionText, leftMargin, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_LEFT);
                            ctRight.SetSimpleColumn(sectionDate, leftMargin + 90, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_RIGHT);

                            cb.Stroke();

                            ctLeft.Go();
                            ctRight.Go();

                            docHeight -= 10;

                            if (columnRowsCount == numberOfRows)
                            {
                                lastRowHeight = docHeight;
                                if (subSectionColumn == 3)
                                {
                                    if (newPage)
                                    {
                                        newPage = false;
                                        subSectionColumn = 1;
                                        document.NewPage();
                                        docHeight = lastDocHeight = pageHeight;

                                        pageCount++;

                                        //leftMargin = SetLeftMarginForPage(pageCount);

                                        numberOfRows = (docHeight - pageFooterHeight) / 10;

                                        if ((numberOfRows * 3) > (subSectionListSize - (columnRowsCount * 3)))
                                        {
                                            numberOfRows = ((subSectionListSize - (columnRowsCount * 3)) % 3) == 0
                                                ? (subSectionListSize - (columnRowsCount * 3)) / 3
                                                : ((subSectionListSize - (columnRowsCount * 3)) / 3) + 1;

                                            numberOfRows = numberOfRows == 0 ? 1 : numberOfRows;
                                        }
                                        else
                                        {
                                            newPage = true;
                                        }
                                    }
                                }
                                else
                                {
                                    subSectionColumn++;
                                    if (listCount != subSectionListSize) docHeight = lastDocHeight;
                                }

                                columnRowsCount = 0;
                            }
                            else
                            {
                                if (listCount == subSectionListSize) docHeight = lastRowHeight;
                            }
                        }

                    }
                }

                document.Close();

                writer.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        public static int SetLeftMarginForPage(int pageCount)
        {
            int leftMargin;
            const int leftMarginRight = 73;
            const int leftMarginLeft = 43;

            if ((pageCount % 2) == 0) leftMargin = leftMarginLeft;
            else leftMargin = leftMarginRight;

            return leftMargin;
        }

        public static string GetSectionNameForGEN04(string sectionName)
        {
            string section = "";
            if (sectionName.Contains("GEN"))
            {
                section = "GEN";
            }
            else if (sectionName.Contains("ENR"))
            {
                section = "ENR";
            }
            else if (sectionName.Contains("AD"))
            {
                section = "AD";
            }
            return section;
        }

        public static char GetSectionNumber(string section , string sectionNumberTXT)
        {
            char sectionNumber;
            if (section.Equals("AD"))
            {
                sectionNumber = sectionNumberTXT[2];
            }
            else
            {
                sectionNumber = sectionNumberTXT[3];
            }
            return sectionNumber;
        }

        public static void GenerateGEN04SectionOld(eAIPContext db, string path)
        {
            try
            {
                //var path = @"D:\AirNav\bin\Debug\Data\eAIP-output\2018-10-11-AIRAC\pdf\EV-GEN-0.4-en-GB_new.pdf";

                Document document = new Document();

                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                writer.PageEvent = new PDFManagerGEN04();
                document.Open();

                PdfContentByte cb = writer.DirectContent;

                var dateFont = GetFont(); // default is 8
                var dateFontSubParts = GetFont(9);
                var dateFontFold = GetFont(10, Font.BOLD);
                var dateFontForParts = GetFont(12);

                Phrase sectionTtitle = new Phrase(Tpl.MenuFullText("GEN04"), dateFontFold);

                var docHeight = 765;
                ColumnText ctSectionTitle = new ColumnText(cb);
                ctSectionTitle.SetSimpleColumn(sectionTtitle, 73, docHeight, 520, docHeight + 15, 12, Element.ALIGN_LEFT);
                ctSectionTitle.Go();

                List<PDFPage> listGen04 = db.PDFPage
                    .AsNoTracking()
                    .Where(x => x.eAIPID == Lib.CurrentAIP.id)
                    .Include(x => x.PageeAIP)
                    .Include(x => x.AirportHeliport)
                    .ToList()
                    .OrderBy(x => x.Section, new SectionNameComparer())
                    .ThenBy(x => x.AirportHeliportID)
                    .ThenBy(x => x.Page)
                    .ToList();

                CultureInfo ci = new CultureInfo("en-US");

                string secText = null;
                string subSecText = null;
                int part = 1;

                Phrase sectionText;
                Phrase sectionDate;

                ColumnText ctLeft = new ColumnText(cb);
                ColumnText ctRight = new ColumnText(cb);

                docHeight -= 10;

                const int leftMarginRight = 73;
                const int leftMarginLeft = 43;

                int pageCount = 1;
                var leftMargin = leftMarginRight;
                int element = 1;
                foreach (var item in listGen04)
                {
                    string section = null;
                    if (item.Section.ToName().Contains("GEN"))
                    {
                        section = "GEN";
                    }
                    else if (item.Section.ToName().Contains("ENR"))
                    {
                        section = "ENR";
                    }
                    else if (item.Section.ToName().Contains("AD"))
                    {
                        section = "AD";
                    }

                    if (!object.Equals(secText, section))
                    {
                        if ((pageCount % 2) == 0) leftMargin = leftMarginLeft;
                        else leftMargin = leftMarginRight;
                        element = 1;
                        secText = section;
                        docHeight -= 25;
                        if (docHeight < 40)
                        {
                            document.NewPage();
                            docHeight = 765;

                            pageCount++;
                            if ((pageCount % 2) == 0) leftMargin = leftMarginLeft;
                            else leftMargin = leftMarginRight;
                        }

                        Phrase sectionParts = new Phrase(Tpl.Text("Part_" + part) + " - " + Tpl.MenuText(section + "_"), dateFontForParts);
                        part++;

                        ColumnText ctSectionParts = new ColumnText(cb);
                        ctSectionParts.SetSimpleColumn(sectionParts, leftMargin, docHeight, leftMargin + 447, docHeight + 15, 12, Element.ALIGN_LEFT);
                        ctSectionParts.Go();
                    }

                    var sectionNumberTXT = item.Section.ToString();
                    char sectionNumber;
                    if (section.Equals("AD"))
                    {
                        sectionNumber = sectionNumberTXT[2];
                    }
                    else
                    {
                        sectionNumber = sectionNumberTXT[3];
                    }

                    if (!object.Equals(subSecText, section + " " + sectionNumber))
                    {
                        if ((pageCount % 2) == 0) leftMargin = leftMarginLeft;
                        else leftMargin = leftMarginRight;

                        element = 1;

                        subSecText = section + " " + sectionNumber;

                        docHeight -= 25;
                        if (docHeight < 40)
                        {
                            document.NewPage();
                            docHeight = 765;

                            pageCount++;
                            if ((pageCount % 2) == 0) leftMargin = leftMarginLeft;
                            else leftMargin = leftMarginRight;
                        }
                        
                        Phrase sectionSubParts = new Phrase(Tpl.MenuFullText(section + sectionNumber), dateFontSubParts);

                        ColumnText ctSectionSubParts = new ColumnText(cb);
                        ctSectionSubParts.SetSimpleColumn(sectionSubParts, leftMargin, docHeight, leftMargin + 447, docHeight + 15, 12, Element.ALIGN_LEFT);
                        ctSectionSubParts.Go();

                        docHeight -= 10;
                    }
                    sectionText = section.Equals("AD") && (sectionNumber == '2' || sectionNumber == '3') ?
                        new Phrase($@"{item.AirportHeliport.LocationIndicatorICAO} {section} {sectionNumber} - {item.Page}",
                            dateFont) :
                        new Phrase($@"{item.Section.ToName().Replace("-", " ")} - {item.Page}",
                            dateFont);
                    sectionDate = new Phrase(item.PageeAIP?.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant(), dateFont);
                    if ((element % 3) == 1)
                    {
                        ctLeft.SetSimpleColumn(sectionText, leftMargin, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_LEFT);
                        ctRight.SetSimpleColumn(sectionDate, leftMargin + 90, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_RIGHT);

                        leftMargin += 160;

                    }
                    else if ((element % 3) == 2)
                    {
                        ctLeft.SetSimpleColumn(sectionText, leftMargin, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_LEFT);
                        ctRight.SetSimpleColumn(sectionDate, leftMargin + 90, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_RIGHT);

                        leftMargin += 160;
                    }
                    else
                    {
                        ctLeft.SetSimpleColumn(sectionText, leftMargin, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_LEFT);
                        ctRight.SetSimpleColumn(sectionDate, leftMargin + 90, docHeight, leftMargin + 147, docHeight + 15, 12, Element.ALIGN_RIGHT);

                        if ((pageCount % 2) == 0) leftMargin = leftMarginLeft;
                        else leftMargin = leftMarginRight;
                        //leftMargin -= 320;
                        docHeight -= 10;
                    }

                    cb.Stroke();

                    ctLeft.Go();
                    ctRight.Go();


                    if (docHeight < 40 && element % 3 == 0)
                    {
                        document.NewPage();
                        docHeight = 765;

                        pageCount++;
                        if ((pageCount % 2) == 0) leftMargin = leftMarginLeft;
                        else leftMargin = leftMarginRight;
                    }
                    element++;
                }



                document.Close();

                writer.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
        
        public static void GenerateCoverPagePdf(List<Dictionary<string, string>> items)
        {
            try
            {
                Document document = new Document();

                FileStream fs = new FileStream("C:\\Users\\ShahinK\\Desktop\\LGSSoft_fop\\cover.pdf", FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                //writer.PageEvent = new PDFManagerGEN04();
                document.Open();

                PdfContentByte cb = writer.DirectContent;

                var font = GetFont(); // default is 8

                var docHeight = 765;

                var leftInsMargin = 40;
                var rightInsMargin = 160;
                var leftDelMargin = 300;
                var rightDelMargin = 420;

                Phrase sectionInsText;
                Phrase sectionInsDate;
                Phrase sectionDelText;
                Phrase sectionDelDate;

                ColumnText ctInsLeft = new ColumnText(cb);
                ColumnText ctInsRight = new ColumnText(cb);
                ColumnText ctDelLeft = new ColumnText(cb);
                ColumnText ctDelRight = new ColumnText(cb);

                

                foreach (var item in items)
                {
                    
                    sectionInsText = new Phrase(item.ContainsKey("Inserted_1") ? item["Inserted_1"] : "" , font);
                    sectionInsDate = new Phrase(item.ContainsKey("Inserted_2") ? item["Inserted_2"] : "" , font);
                    sectionDelText = new Phrase(item.ContainsKey("Deleted_1") ? item["Deleted_1"] : "" , font);
                    sectionDelDate = new Phrase(item.ContainsKey("Deleted_2") ? item["Deleted_2"] : "" , font);

                    ctInsLeft.SetSimpleColumn(sectionInsText, leftInsMargin, docHeight, leftInsMargin + 100, docHeight + 15, 12, Element.ALIGN_LEFT);
                    ctInsRight.SetSimpleColumn(sectionInsDate, rightInsMargin, docHeight, rightInsMargin + 100, docHeight + 15, 12, Element.ALIGN_RIGHT);

                    ctDelLeft.SetSimpleColumn(sectionDelText, leftDelMargin, docHeight, leftDelMargin + 100, docHeight + 15, 12, Element.ALIGN_LEFT);
                    ctDelRight.SetSimpleColumn(sectionDelDate, rightDelMargin, docHeight, rightDelMargin + 100, docHeight + 15, 12, Element.ALIGN_RIGHT);

                    cb.Stroke();

                    ctInsLeft.Go();
                    ctInsRight.Go();
                    ctDelLeft.Go();
                    ctDelRight.Go();

                    docHeight -= 10;
                    if (docHeight < 40)
                    {
                        document.NewPage();
                        docHeight = 765;
                    }
                }

                document.Close();

                writer.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }

    public class PDFManagerGEN04 : PdfPageEventHelper
    {
        int pageSize = 0;
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            if ((++pageSize % 2) != 0)
            {
                addHeaderRight(writer, document);
                addFooterRight(writer, document);
            }
            else
            {
                addHeaderLeft(writer, document);
                addFooterLeft(writer, document);
            }
        }

        private void addHeaderRight(PdfWriter writer, Document document)
        {
            try
            {
                PdfContentByte cb = writer.DirectContent;
                ColumnText ctLeft = new ColumnText(cb);
                ColumnText ctRight = new ColumnText(cb);
                ColumnText ctRightDate = new ColumnText(cb);

                CultureInfo ci = new CultureInfo("en-US");

                var dateFontBold = PDFManager.GetFont(10, Font.BOLD);
                var dateFont = PDFManager.GetFont(10, null, BaseColor.BLACK);

                Phrase headerTitle = new Phrase(Tpl.Text("PublishingOrganization"), dateFontBold);
                Phrase headerSection = new Phrase("GEN 0.4 - " + pageSize, dateFontBold);
                Phrase headerDate = new Phrase(Lib.CurrentAIP.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant(), dateFont);


                ctLeft.SetSimpleColumn(headerTitle, 73, 816, 200, 831, 12, Element.ALIGN_LEFT);
                ctRight.SetSimpleColumn(headerSection, 490, 816, 553, 831, 12, Element.ALIGN_RIGHT);
                ctRightDate.SetSimpleColumn(headerDate, 490, 804, 554, 819, 12, Element.ALIGN_RIGHT);
                ctLeft.Go();
                ctRight.Go();
                ctRightDate.Go();

                cb.MoveTo(73, 801);
                cb.LineTo(553, 801);
                cb.Stroke();

                addColumnsTitle(writer, document, 73);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void addHeaderLeft(PdfWriter writer, Document document)
        {
            try
            {
                PdfContentByte cb = writer.DirectContent;
                ColumnText ctLeft = new ColumnText(cb);
                ColumnText ctRight = new ColumnText(cb);
                ColumnText ctLeftDate = new ColumnText(cb);

                CultureInfo ci = new CultureInfo("en-US");

                var dateFontBold = PDFManager.GetFont(10, Font.BOLD);
                var dateFont = PDFManager.GetFont(10, null, BaseColor.BLACK);

                Phrase headerTitle = new Phrase(Tpl.Text("PublishingOrganization"), dateFontBold);
                Phrase headerSection = new Phrase("GEN 0.4 - " + pageSize, dateFontBold);
                Phrase headerDate = new Phrase(Lib.CurrentAIP.Effectivedate.ToString("dd MMM yyyy", ci).ToUpperInvariant(), dateFont);


                ctLeft.SetSimpleColumn(headerSection, 43, 816, 170, 831, 12, Element.ALIGN_LEFT);
                ctRight.SetSimpleColumn(headerTitle, 460, 816, 523, 831, 12, Element.ALIGN_RIGHT);
                ctLeftDate.SetSimpleColumn(headerDate, 43, 804, 107, 819, 12, Element.ALIGN_LEFT);

                ctLeft.Go();
                ctRight.Go();
                ctLeftDate.Go();

                cb.MoveTo(43, 801);
                cb.LineTo(523, 801);
                cb.Stroke();

                addColumnsTitle(writer, document, 43);
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void addFooterRight(PdfWriter writer, Document document)
        {
            try
            {
                PdfContentByte cb = writer.DirectContent;
                ColumnText ctLeft = new ColumnText(cb);
                ColumnText ctRight = new ColumnText(cb);
                var dateFont = PDFManager.GetFont(10, null, BaseColor.BLACK);

                Phrase footerTitle = new Phrase(Lib.GetText("PDF_FooterOrgName"), dateFont);
                Phrase footerAMDT = new Phrase(Tpl.Text("AIRAC_AMDT") + " " + Lib.CurrentAIP.Amendment.Number + "/" + Lib.CurrentAIP.Amendment.Year, dateFont);


                ctLeft.SetSimpleColumn(footerTitle, 73, 14, 200, 29, 12, Element.ALIGN_LEFT);
                ctRight.SetSimpleColumn(footerAMDT, 396, 14, 554, 29, 12, Element.ALIGN_RIGHT);
                ctLeft.Go();
                ctRight.Go();

                cb.MoveTo(73, 30);
                cb.LineTo(553, 30);
                cb.Stroke();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void addFooterLeft(PdfWriter writer, Document document)
        {
            try
            {
                PdfContentByte cb = writer.DirectContent;
                ColumnText ctLeft = new ColumnText(cb);
                ColumnText ctRight = new ColumnText(cb);
                var dateFont = PDFManager.GetFont(10, null, BaseColor.BLACK);

                Phrase footerTitle = new Phrase(Lib.GetText("PDF_FooterOrgName"), dateFont);
                Phrase footerAMDT = new Phrase(Tpl.Text("AIRAC_AMDT") + " " + Lib.CurrentAIP.Amendment.Number + "/" + Lib.CurrentAIP.Amendment.Year, dateFont);


                ctLeft.SetSimpleColumn(footerAMDT, 40, 14, 200, 29, 12, Element.ALIGN_LEFT);
                ctRight.SetSimpleColumn(footerTitle, 366, 14, 524, 29, 12, Element.ALIGN_RIGHT);
                ctLeft.Go();
                ctRight.Go();

                cb.MoveTo(43, 30);
                cb.LineTo(523, 30);
                cb.Stroke();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }

        private void addColumnsTitle(PdfWriter writer, Document document, int leftMargin)
        {
            try
            {
                PdfContentByte cb = writer.DirectContent;

                var dateFont = PDFManager.GetFont(10, null, BaseColor.BLACK);

                Phrase columnTitle1 = new Phrase(Tpl.Text("Page"), dateFont);
                Phrase columnTitle2 = new Phrase(Tpl.Text("Date"), dateFont);

                ColumnText ctPage1 = new ColumnText(cb);
                ColumnText ctDate1 = new ColumnText(cb);
                ColumnText ctPage2 = new ColumnText(cb);
                ColumnText ctDate2 = new ColumnText(cb);
                ColumnText ctPage3 = new ColumnText(cb);
                ColumnText ctDate3 = new ColumnText(cb);

                var height = 780;
                ctPage1.SetSimpleColumn(columnTitle1, leftMargin, height, leftMargin + 147, height + 15, 12, Element.ALIGN_LEFT);
                ctDate1.SetSimpleColumn(columnTitle2, leftMargin + 90, height, leftMargin + 147, height + 15, 12, Element.ALIGN_RIGHT);

                leftMargin += 160;
                ctPage2.SetSimpleColumn(columnTitle1, leftMargin, height, leftMargin + 147, height + 15, 12, Element.ALIGN_LEFT);
                ctDate2.SetSimpleColumn(columnTitle2, leftMargin + 90, height, leftMargin + 147, height + 15, 12, Element.ALIGN_RIGHT);

                leftMargin += 160;
                ctPage3.SetSimpleColumn(columnTitle1, leftMargin, height, leftMargin + 147, height + 15, 12, Element.ALIGN_LEFT);
                ctDate3.SetSimpleColumn(columnTitle2, leftMargin + 90, height, leftMargin + 147, height + 15, 12, Element.ALIGN_RIGHT);

                ctPage1.Go();
                ctDate1.Go();
                ctPage2.Go();
                ctDate2.Go();
                ctPage3.Go();
                ctDate3.Go();
            }
            catch (Exception ex)
            {
                ErrorLog.ShowException($@"Error in the {ex.TargetSite?.Name}", ex, true);
            }
        }
    }
}
