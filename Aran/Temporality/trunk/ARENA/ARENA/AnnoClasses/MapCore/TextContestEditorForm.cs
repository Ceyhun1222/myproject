using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Accent.MapCore
{
    public partial class TextContestEditorForm : Form
    {
        public List<List<AcntChartElementWord>> TextContents;
        public List<List<AcntChartElementWord>> BottomTextContest;
        public List<List<AcntChartElementWord>> CaptionTextContest;

        private int CaptionLinesCount=0;
        private int InnerLinesCount=0;
        private int BottomLinesCount = 0;
        private List<AcntChartElementWord> line;

        public TextContestEditorForm()
        {
            InitializeComponent();
        }

        private void TextContestEditorForm_Load(object sender, EventArgs e)
        {
            ShowElementsLines();

            listBox1.SelectedIndex = 0;
        }

        private void ShowElementsLines()
        {
            listBox1.Items.Clear();

            if (CaptionTextContest != null)
            {
                CaptionLinesCount = CaptionTextContest.Count;
                for (int i = 0; i <= CaptionTextContest.Count - 1; i++)
                {
                    listBox1.Items.Add("Caption Text Line #     " + (i + 1).ToString());

                }
            }

            if (TextContents != null)
            {
                InnerLinesCount = TextContents.Count;
                for (int i = 0; i <= TextContents.Count - 1; i++)
                {
                    listBox1.Items.Add("    Inner Text Line #   " + (i + 1).ToString());

                }
            }

            if (BottomTextContest != null)
            {
                BottomLinesCount = BottomTextContest.Count;
                for (int i = 0; i <= BottomTextContest.Count - 1; i++)
                {
                    listBox1.Items.Add("Bottom Text Line #     " + (i + 1).ToString());

                }
            }
        }

        private void showPhrases(int LineNumber, string textType)
        {

            listBox2.Items.Clear();

            line = TextContents[0];

            if (textType.StartsWith("Inner"))
                line = TextContents[LineNumber - CaptionLinesCount];

            if (textType.StartsWith("Caption"))
                line = CaptionTextContest[LineNumber];

            if (textType.StartsWith("Bottom"))
                line = BottomTextContest[LineNumber - CaptionLinesCount - InnerLinesCount];


            foreach (AcntChartElementWord wrd in line)
            {
                listBox2.Items.Add(wrd.TextValue);
            }

            listBox2.SelectedIndex = 0;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = line[listBox2.SelectedIndex];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<AcntChartElementWord> txtLine = new List<AcntChartElementWord>(); // создаем строку
            AcntChartElementWord wrd = new AcntChartElementWord();//создаем слово
            wrd.TextValue = "Value";
            wrd.Font.Bold = true;
            wrd.StartSymbol = new AcntSymbol("");
            wrd.EndSymbol = new AcntSymbol("");
            //wrd.Morse = true;
            txtLine.Add(wrd);
            this.TextContents.Add(txtLine);  // добавим его в строку

            ShowElementsLines();
            if (!listBox1.Items[listBox1.Items.Count-1].ToString().StartsWith("Bottom")) listBox1.SelectedIndex = listBox1.Items.Count-1;
            else listBox1.SelectedIndex = listBox1.Items.Count-2;
        }


    }
}
