using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRulesExcelToDB
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenExcelFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "Excel file|*.xlsx" };
            
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var dbPro = new DbPro
            {
                DeleteExistedTable = () =>
                {
                    return MessageBox.Show(
                        "Rules table exists, do you want to recreate?", 
                        Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes;
                }
            };

            dbPro.Open();
            dbPro.BeginInsertRule();

            var rulesEx = new RulesExcel
            {
                RuleParsed = (rule) => { dbPro.SetRule(rule); }
            };

            rulesEx.Open(ofd.FileName);
            rulesEx.Close();

            dbPro.Close();

            MessageBox.Show("Done!");
        }

        private void Test_Click(object sender, EventArgs e)
        {
            var dbPro = new DbPro();
            dbPro.Open();
            dbPro.Test();
            dbPro.Close();
        }

        private void UpdateExcel_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "Excel file|*.xlsx" };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            var rulesEx = new RulesExcel();

            var dbPro = new DbPro();
            dbPro.Open();

            rulesEx.RuleReaded = (uid) => { return dbPro.GetNotImplementedReason(uid); };

            rulesEx.Open(ofd.FileName, true);
            rulesEx.Close();

            dbPro.Close();

            MessageBox.Show("Done");
        }

        private void SetCommandInfo_Click(object sender, EventArgs e)
        {
            var dbPro = new DbPro();
            dbPro.Open();
            dbPro.SetCustomCommandInfo("isComposedOf");
            dbPro.Close();

            MessageBox.Show("Done");
        }
    }
}
