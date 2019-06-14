using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ANCOR.MapCore
{
    public partial class TextContestEditorForm : Form
    {
        public List<List<AncorChartElementWord>> TextContents;
        public List<List<AncorChartElementWord>> BottomTextContest;
        public List<List<AncorChartElementWord>> CaptionTextContest;
        public string Objectname;
        public string ObjectID;
        public string relFeaturename;
        public string ResStr;

        private int CaptionLinesCount=0;
        private int InnerLinesCount=0;
        private int BottomLinesCount = 0;
        private List<AncorChartElementWord> line;

        public static Action objectTextChanged;
        //public static Action ObjectTextContensChanged;


        public TextContestEditorForm()
        {
            InitializeComponent();
            SigmaColorEdotor.FontColorChanged = textFontColorChanged;

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
            bool flag = true;

            line = TextContents[0];

            if (textType.StartsWith("Inner"))
            {
                line = TextContents[LineNumber - CaptionLinesCount];
                flag = true;
            }

            if (textType.StartsWith("Caption"))
            {
                line = CaptionTextContest[LineNumber];
                flag = false;
            }

            if (textType.StartsWith("Bottom"))
            {
                line = BottomTextContest[LineNumber - CaptionLinesCount - InnerLinesCount];
                flag = false;
            }

            foreach (AncorChartElementWord wrd in line)
            {
                listBox2.Items.Add(wrd.TextValue);
            }

            listBox2.SelectedIndex = 0;
            button5.Enabled = CaptionTextContest!=null ?  flag && listBox1.SelectedIndex >  CaptionTextContest.Count : listBox1.SelectedIndex > 0;
            button6.Enabled = flag && listBox1.SelectedIndex < TextContents.Count;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
            button2.Enabled = listBox1.SelectedItem.ToString().Trim().StartsWith("Inner");
            button5.Enabled = TextContents.Count > 1;
            button6.Enabled = TextContents.Count > 1;

            button5.Enabled = !listBox1.Items[listBox1.SelectedIndex].ToString().StartsWith("Caption");
            button6.Enabled = !listBox1.Items[listBox1.SelectedIndex].ToString().StartsWith("Caption");
            button5.Enabled = !listBox1.Items[listBox1.SelectedIndex].ToString().StartsWith("Bottom");
            button6.Enabled = !listBox1.Items[listBox1.SelectedIndex].ToString().StartsWith("Bottom");

            //foreach (var item in line)
            //{
            //    button2.Enabled = button2.Enabled && item.DataSource.Link.StartsWith("added");
            //}
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = line[listBox2.SelectedIndex];

            button8.Enabled = listBox2.SelectedIndex > 0;
            button7.Enabled = listBox2.SelectedIndex != listBox2.Items.Count -1;

            button3.Enabled = line[listBox2.SelectedIndex].DataSource.Link.StartsWith("added");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<AncorChartElementWord> txtLine = new List<AncorChartElementWord>(); // создаем строку
            
            txtLine.Add(AddWord());
            this.TextContents.Add(txtLine);  // добавим его в строку

            ShowElementsLines();

            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            if (listBox1.Items[listBox1.Items.Count-1].ToString().StartsWith("Bottom")) listBox1.SelectedIndex = listBox1.Items.Count-2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _removeLine();

            ShowElementsLines();
            listBox1.SelectedIndex = listBox1.Items.Count - 1;

            listBox1.Focus();
           
        }

        private void _removeLine()
        {
            this.TextContents.Remove(line);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

            //int selIndx = this.CaptionLinesCount == 0 ? listBox1.SelectedIndex : listBox1.SelectedIndex - 1;
            int selIndx = this.CaptionLinesCount > 0 ?  listBox1.SelectedIndex - this.CaptionLinesCount : listBox1.SelectedIndex;
            //if (this.CaptionLinesCount == 2) selIndx++;
            //selIndx = this.CaptionLinesCount == 0 ? selIndx : selIndx - 1;
            if (selIndx == 0) return;
            var _linePrev = this.TextContents[selIndx - 1] ;
            this.TextContents[selIndx - 1] = this.TextContents[selIndx];
            this.TextContents[selIndx] = _linePrev;

            listBox1.SelectedIndex = this.CaptionLinesCount > 0 ? selIndx + 1 : selIndx-1;
            //if (this.CaptionLinesCount == 2) listBox1.SelectedIndex--;
            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
            //listBox1.SelectedIndex = selIndx;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int selIndx = listBox1.SelectedIndex - this.CaptionLinesCount;
            //if (this.CaptionLinesCount == 2) selIndx--;
            if (selIndx == this.TextContents.Count - 1) return;
            var _lineNext = this.TextContents[selIndx + 1];
            this.TextContents[selIndx + 1] = this.TextContents[selIndx];
            this.TextContents[selIndx] = _lineNext;

            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
            listBox1.SelectedIndex++;
            //if (this.CaptionLinesCount == 2) listBox1.SelectedIndex++;
            
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int selIndx = listBox2.SelectedIndex;
            var _wordPrev = line[listBox2.SelectedIndex -1];
            line[listBox2.SelectedIndex - 1] = line[listBox2.SelectedIndex];
            line[listBox2.SelectedIndex] = _wordPrev;

            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
            listBox2.SelectedIndex = selIndx - 1;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int selIndx = listBox2.SelectedIndex;
            var _wordNext = line[listBox2.SelectedIndex + 1];
            line[listBox2.SelectedIndex + 1] = line[listBox2.SelectedIndex];
            line[listBox2.SelectedIndex] = _wordNext;

            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
            listBox2.SelectedIndex = selIndx+1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            line.Add( AddWord());
            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (line.Count == 1) _removeLine();
            else line.Remove(line[listBox2.SelectedIndex]);
            //listBox1.SelectedIndex = 0;
            ShowElementsLines();
            listBox1.SelectedIndex = 0;
            showPhrases(listBox1.SelectedIndex, listBox1.SelectedItem.ToString().Trim());
        }


        private AncorChartElementWord AddWord()
        {
            AncorChartElementWord wrd = (AncorChartElementWord)line[0].Clone();
            wrd.TextValue = "TEXT";
            wrd.DataSource.Condition = "";
            wrd.DataSource.Link = "added";
            wrd.DataSource.Value = "";

            wrd.StartSymbol.Text = "";
            wrd.EndSymbol.Text = "";
            return wrd;
        }

        private void button9_Click(object sender, EventArgs e)
        {

            if (objectTextChanged!=null) objectTextChanged();
            //if (ObjectTextContensChanged != null && propertyGrid1.SelectedGridItem.Label.Contains("TextValue"))
            if (propertyGrid1.SelectedGridItem.Label.Contains("TextValue"))
            {
                var sItm = propertyGrid1.SelectedGridItem;
                SelectPropertyGridItemByName(propertyGrid1, "Visible");
                propertyGrid1.SelectedGridItem = sItm;
            }
        }

        private void textFontColorChanged()
        {
            if (objectTextChanged!=null) objectTextChanged();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ResStr = "";

            if (objectTextChanged != null && e.ChangedItem.Label.Contains("TextValue"))
            {
                if (e.OldValue.ToString().CompareTo(e.ChangedItem.Value) != 0)
                {
                    ResStr = "Feature: " + relFeaturename + "\tobject: " + Objectname + "\tID: " + ObjectID + "\told value: " + e.OldValue.ToString() + "\tnew value: " + e.ChangedItem.Value;
                 }
                
            }
        }

        public static bool SelectPropertyGridItemByName(PropertyGrid propertyGrid, string propertyName)
        {
            MethodInfo getPropEntriesMethod = propertyGrid.GetType().GetMethod("GetPropEntries", BindingFlags.NonPublic | BindingFlags.Instance);

            Debug.Assert(getPropEntriesMethod != null, @"GetPropEntries by reflection is still valid in .NET 4.6.1 ");

            GridItemCollection gridItemCollection = (GridItemCollection)getPropEntriesMethod.Invoke(propertyGrid, null);

            GridItem gridItem = TraverseGridItems(gridItemCollection, propertyName);

            if (gridItem == null)
            {
                return false;
            }

            propertyGrid.SelectedGridItem = gridItem;

            return true;
        }

        private static GridItem TraverseGridItems(IEnumerable parentGridItemCollection, string propertyName)
        {
            foreach (GridItem gridItem in parentGridItemCollection)
            {
                if (gridItem.Label != null && gridItem.Label.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return gridItem;
                }

                if (gridItem.GridItems == null)
                {
                    continue;
                }

                GridItem childGridItem = TraverseGridItems(gridItem.GridItems, propertyName);

                if (childGridItem != null)
                {
                    return childGridItem;
                }
            }

            return null;
        }

        private void TextContestEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
