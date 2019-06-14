using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aran.PANDA.Reports
{
    public class ExcellExporter
    {
        private Dictionary<String, IFont> _fonts = new Dictionary<string, IFont>();
        private int max = 500;

        private XSSFWorkbook _workbook;
        private ISheet _currentSheet;
        private IRow _currentRow;
        private ICell _currentCell;
        public int CurrentRowNum { get; set; }
        public int CurrentCellNum { get; set; }


        private ExcellExporter(string summary) : this()
        {
            //create a entry of SummaryInformation
            //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //si.Subject = summary;
            //_workbook.
        }

        private ExcellExporter()
        {
            try
            {

                _workbook = new XSSFWorkbook();
            }
            catch (Exception ex)
            {
                string m = ex.Message;
            }
        }

        public static ExcellExporter Get()
        {
            return new ExcellExporter();
        }

        public static ExcellExporter Get(string summary)
        {
            return new ExcellExporter(summary);
        }

        public ExcellExporter CreateSheet()
        {
            _currentSheet = _workbook.CreateSheet();
            CurrentRowNum = -1;
            return this;
        }

        public ExcellExporter CreateSheet(string name)
        {
            _currentSheet = _workbook.CreateSheet(name.Replace("/", "-"));
            CurrentRowNum = -1;
            return this;
        }

        public ExcellExporter CreateRow()
        {
            if (CurrentRowNum > max)
                return this;
            _currentRow = _currentSheet.CreateRow(++CurrentRowNum);
            CurrentCellNum = -1;
            return this;
        }

        public ExcellExporter CreateRow(int num)
        {
            if (CurrentRowNum > max)
                return this;
            CurrentRowNum = num;
            _currentRow = _currentSheet.CreateRow(num);
            CurrentCellNum = -1;
            return this;
        }

        public ExcellExporter CreateCell()
        {
            if (CurrentRowNum > max)
                return this;
            _currentCell = _currentRow.CreateCell(++CurrentCellNum);
            return this;
        }

        public ExcellExporter CreateCell(int num)
        {
            if (CurrentRowNum > max)
                return this;
            CurrentCellNum = num;
            _currentCell = _currentRow.CreateCell(num);
            return this;
        }



        public ExcellExporter SetText(String text, System.Drawing.Color color, bool bold = false, bool italic = false, int size = 12)
        {
            if (CurrentRowNum > max)
                return this;
            IFont font = createFont(color, bold, italic, size);
            XSSFRichTextString richtext = new XSSFRichTextString(text == null ? "" : text);
            richtext.ApplyFont(font);
            _currentCell.SetCellValue(richtext);
            return this;
        }

        public ExcellExporter Text(String text, System.Drawing.Color color, bool bold = false, bool italic = false, int size = 12)
        {
            if (CurrentRowNum > max)
                return this;
            CreateCell();
            return SetText(text, color, bold, italic, size);
        }

        public ExcellExporter Text(int cellNum, System.Drawing.Color color, String text, bool bold = false, bool italic = false, int size = 12)
        {
            if (CurrentRowNum > max)
                return this;
            CreateCell(cellNum);
            return SetText(text, color, bold, italic, size);
        }

        public ExcellExporter SetText(String text, bool bold = false, bool italic = false, int size = 12)
        {
            if (CurrentRowNum > max)
                return this;
            IFont font = createFont(bold, italic, size);
            XSSFRichTextString richtext = new XSSFRichTextString(text == null ? "" : text);
            richtext.ApplyFont(font);
            _currentCell.SetCellValue(richtext);
            return this;
        }

        public ExcellExporter Text(String text, bool bold = false, bool italic = false, int size = 12)
        {
            if (CurrentRowNum > max)
                return this;
            CreateCell();
            return SetText(text, bold, italic, size);
        }

        public ExcellExporter Text(int cellNum, String text, bool bold = false, bool italic = false, int size = 12)
        {
            if (CurrentRowNum > max)
                return this;
            CreateCell(cellNum);
            return SetText(text, bold, italic, size);
        }

        public ExcellExporter H1(string text)
        {
            if (CurrentRowNum > max)
                return this;
            CreateRow();
            CreateCell();
            return H(text, 20, HSSFColor.Blue.Index);
        }

        public ExcellExporter H2(string text)
        {
            if (CurrentRowNum > max)
                return this;
            CreateRow();
            CreateCell();
            return H(text, 16, HSSFColor.Aqua.Index);
        }

        private ExcellExporter H(string text, int size, short color)
        {
            if (CurrentRowNum > max)
                return this;
            CreateRow();
            CreateCell();

            IFont font = createFont(color, true, false, size);
            ICellStyle style = _workbook.CreateCellStyle();

            style.Alignment = HorizontalAlignment.Center;
            XSSFRichTextString richtext = new XSSFRichTextString(text == null ? "" : text);
            richtext.ApplyFont(font);
            _currentCell.SetCellValue(richtext);
            _currentSheet.AddMergedRegion(new CellRangeAddress(CurrentRowNum, CurrentRowNum, CurrentCellNum, CurrentCellNum + 10));
            return this;
        }

        public byte[] GetBytes()
        {

            using (MemoryStream stream = new MemoryStream())
            {
                _workbook.Write(stream);
                return stream.ToArray();
            }
        }

        public void WriteToFile(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Create))
            {
                _workbook.Write(file);
            }
        }

        private IFont createFont(bool bold = false, bool italic = false, int size = 12)
        {
            return createFont(System.Drawing.Color.Black, bold, italic, size);
        }

        private IFont createFont(System.Drawing.Color color, bool bold = false, bool italic = false, int size = 12)
        {
            return createFont(new XSSFColor(color).Indexed, bold, italic, size);
        }

        private IFont createFont(short color, bool bold = false, bool italic = false, int size = 12)
        {
            var key = simpleHash(color, bold, italic, size);
            if (!_fonts.ContainsKey(key))
            {
                IFont font = _workbook.CreateFont();
                font.IsItalic = italic;
                font.Boldweight = (short)(bold ? FontBoldWeight.Bold : FontBoldWeight.None);
                font.FontHeightInPoints = (short)size;
                font.Color = color;
                _fonts.Add(key, font);
            }

            return _fonts[key];
        }

        private string simpleHash(short color, bool bold = false, bool italic = false, int size = 12)
        {
            return color.ToString() + bold.ToString() + italic.ToString() + size.ToString();
        }
    }
}
