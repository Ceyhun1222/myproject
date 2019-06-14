using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using HtmlAgilityPack;

namespace PVT.Utils
{
    class PDFDocument
    {

        public static PDFDocument CreateDocument(string filename)
        {
            return new PDFDocument(filename);
        }

        private readonly Document _pdfDocument;
        private readonly PdfWriter _pdfWriter;
        private Paragraph _currentParagraph;
        private Font _font;
        private readonly BaseFont _baseFont;


        private PDFDocument(string fileName)
        {
            _pdfDocument = new Document(PageSize.A4);
            _pdfWriter = PdfWriter.GetInstance(_pdfDocument, new FileStream(fileName, FileMode.Create));
            _pdfDocument.Open();

            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            _font = new Font(_baseFont);

        }

        public void Save()
        {
            _pdfDocument.Close();
            // _pdfWriter.Close();
        }

        public PDFDocument AddPage()
        {
            flush();
            _pdfDocument.NewPage();
            return this;
        }


        private void flush()
        {
            if (_currentParagraph != null)
            {
                _pdfDocument.Add(_currentParagraph);
                _currentParagraph = null;
            }
        }

        public PDFDocument SetSize(int size)
        {
            _font.Size = size;
            return this;
        }
        public PDFDocument SetAlignment(ParagraphAlignment alignment)
        {
            switch (alignment)
            {
                case ParagraphAlignment.Left:
                    _currentParagraph.Alignment = Element.ALIGN_LEFT;
                    break;
                case ParagraphAlignment.Center:
                    _currentParagraph.Alignment = Element.ALIGN_CENTER;
                    break;
                case ParagraphAlignment.Right:
                    _currentParagraph.Alignment = Element.ALIGN_RIGHT;
                    break;
                case ParagraphAlignment.Justify:
                    _currentParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                    break;
            }

            return this;
        }


        public PDFDocument AddHtml(string html)
        {
            flush();

            try
            {
                HtmlNode.ElementsFlags["p"] = HtmlElementFlag.Closed;
                HtmlNode.ElementsFlags["meta"] = HtmlElementFlag.Closed;
                HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Closed;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                using (var srHtml = new StringReader(doc.DocumentNode.OuterHtml))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(_pdfWriter, _pdfDocument, srHtml);
                }
            }
            catch (Exception ex)
            {
                Engine.Environment.Current.Logger.Error(ex, "PDF generation error.");
            }
            return this;
        }


        public PDFDocument AddImage(System.Drawing.Image image)
        {
            flush();
            var pic = Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);
            if (pic.Height > pic.Width)
            {
                //Maximum height is 800 pixels.
                var percentage = 0.0f;
                percentage = 700 / pic.Height;
                pic.ScalePercent(percentage * 100);
            }
            else
            {
                //Maximum width is 600 pixels.
                var percentage = 0.0f;
                percentage = 540 / pic.Width;
                pic.ScalePercent(percentage * 100);
            }

            pic.Border = Rectangle.BOX;
            pic.BorderColor = BaseColor.BLACK;
            pic.BorderWidth = 3f;
            _pdfDocument.Add(pic);
            return this;
        }

        public PDFDocument AddText(string text)
        {
            if (_currentParagraph == null)
                AddParagraph();
            _currentParagraph.Add(new Phrase(text));
            return this;
        }

        public PDFDocument AddH1()
        {
            return AddParagraph(20, TextFormat.BOLD);
        }

        public PDFDocument AddH1(string text)
        {
            return AddH1().AddText(text);
        }

        public PDFDocument AddH2()
        {
            return AddParagraph(18, TextFormat.BOLD);
        }

        public PDFDocument AddH2(string text)
        {
            return AddH2().AddText(text);
        }

        public PDFDocument AddH3(string text)
        {
            return AddH3().AddText(text);
        }

        public PDFDocument AddH3()
        {
            return AddParagraph(16, TextFormat.BOLD);
        }


        public PDFDocument AddParagraph(System.Drawing.Color color, int size, int style)
        {
            AddParagraph(size, style);
            _font.Color = new BaseColor(color.ToArgb());
            return this;
        }

        public PDFDocument AddParagraph(int size, int style)
        {
            AddParagraph(style);
            _font.Size = size;
            return this;
        }

        public PDFDocument AddParagraph(int style = 0)
        {
            flush();
            _currentParagraph = new Paragraph();
            _font = new Font(_baseFont, Font.DEFAULTSIZE, style);
            return this;
        }




        public class TextFormat
        {
            public const int BOLD = 1;
            public const int BOLDITALIC = 3;
            public const int ITALIC = 2;
            public const int NORMAL = 0;
            public const int STRIKETHRU = 8;
            public const int UNDEFINED = -1;
            public const int UNDERLINE = 4;
        }

        public enum ParagraphAlignment
        {
            Left = 0,
            Center = 1,
            Right = 2,
            Justify = 3
        }

    }
}

