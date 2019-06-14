using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace BRulesExcelToDB
{
    public class RulesExcel
    {
        private Excel.Application _exApp;
        private Excel.Range _sheetCells;
        private int _currentIndex;


        public RulesExcel()
        {
        }

        public Action<Rule> RuleParsed { get; set; }

        public Func<string, string> RuleReaded { get; set; }

        public void Open(string fileName, bool isSave = false)
        {
            if (RuleParsed == null && RuleReaded == null)
                return;

            _exApp = new Excel.Application { Visible = false, ScreenUpdating = false };

            var workbook = _exApp.Workbooks.Open(fileName);
            Excel.Worksheet sheet = workbook.Worksheets[1];

            var range = sheet.Cells;
            Excel.Range last = range.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
            var rowCount = last.Row;
            var columnCount = last.Column;

            _sheetCells = range;

            for (var i = 2; i <= rowCount; i++)
            {
                _currentIndex = i;

                var uid = GetCurrentString(1);

                if (RuleParsed != null)
                {
                    var rule = new Rule();

                    rule.UID = uid;
                    rule.Profile = GetCurrentString(2);
                    rule.Source = GetCurrentString(5);
                    rule.TextualDescription = GetCurrentString(7);
                    rule.TaggedDescription = GetCurrentString(9);
                    rule.Comments = GetCurrentString(10);
                    rule.AixmClass = GetCurrentString(12);
                    rule.AixmAttribute = GetCurrentString(13);
                    rule.AixmAssociation = GetCurrentString(14);
                    rule.TimesliceApplicability = GetCurrentString(15);
                    rule.Category = GetCurrentString(16);
                    rule.Name = GetCurrentString(17);

                    RuleParsed(rule);
                }

                else if (RuleReaded != null)
                {
                    var notImplementedReason = RuleReaded(uid);
                    if (!string.IsNullOrEmpty(notImplementedReason))
                        _sheetCells.Cells[i, 21].Value = notImplementedReason;
                }
            }

            if (isSave)
                workbook.Save();
        }

        private string GetCurrentString(int column, bool isTrim = true)
        {
            var x = _sheetCells.Cells[_currentIndex, column];
            if (x != null && x.Value != null)
                return (isTrim ? x.Value.ToString().Trim() : x.Value.ToString());
            return String.Empty;
        }

        public void Close()
        {
            
            _exApp.Quit();
        }
    }

    public class Rule
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Source { get; set; }
        public string TextualDescription { get; set; }
        public string TaggedDescription { get; set; }
        public string Comments { get; set; }
        public string AixmClass { get; set; }
        public string AixmAttribute { get; set; }
        public string AixmAssociation { get; set; }
        public string TimesliceApplicability { get; set; }
        public string Category { get; set; }
    }
}
