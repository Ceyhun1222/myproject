using ChartCompare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart
{
    public partial class EnrouteUpdateForm : Form
    {
        public List<DetailedItem> _updateList;
        public EnrouteUpdateForm()
        {
            InitializeComponent();
        }

        public EnrouteUpdateForm(List<DetailedItem> updateList)
        {
            InitializeComponent();

            checkedListBox1.Items.Clear();
            _updateList = updateList;
            foreach (DetailedItem item in _updateList)
            {
                if (item.Name.Contains("permdelta")) continue;
                //checkedListBox1.Items.Add(item.Name + " (" + item.ChangedStatus.ToString() + ")", item.IsChecked);
                checkedListBox1.Items.Add(item, item.IsChecked);
            }

            if (checkedListBox1.Items.Count > 0) checkedListBox1.SelectedIndex = 0;
            this.Text = "Update list " + checkedListBox1.Items.Count.ToString();
        } 

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var di = (DetailedItem)checkedListBox1.Items[checkedListBox1.SelectedIndex]; //_updateList[checkedListBox1.SelectedIndex];

            listBox1.Items.Clear();

            propertyGrid1.SelectedObject = di.Feature;
            foreach (var item in di.FieldLogList)
            {
                listBox1.Items.Add("Property name: " +item.FieldName);
                listBox1.Items.Add("    old value: " +item.OldValueText);
                listBox1.Items.Add("    new value: " +item.NewValueText);
                
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {

            //if (checkedListBox1.SelectedIndex < 0) return;
            //UpdateDetaledItemsCheckState_(checkedListBox1.SelectedIndex);

        }

        private void UpdateDetaledItemsCheckState_(int indx)
        {
            var di = _updateList[indx];
            di.IsChecked = !di.IsChecked;
        }

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllCheckBoxes(true, false);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip1.Items[0].Enabled = checkedListBox1.Items != null && checkedListBox1.Items.Count > 0;
            contextMenuStrip1.Items[1].Enabled = checkedListBox1.Items != null && checkedListBox1.Items.Count > 0;
            contextMenuStrip1.Items[2].Enabled = checkedListBox1.Items != null && checkedListBox1.Items.Count > 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            int i = 0;
            foreach (var di in _updateList)
            {
                if (di.IsChecked)
                {
                    lst.Add("");
                    lst.Add(di.ToString());
                    lst.Add("============================");
                    foreach (var item in di.FieldLogList)
                    {
                        lst.Add("   Property name: " + item.FieldName);
                        lst.Add("       old value: " + item.OldValueText);
                        lst.Add("       new value: " + item.NewValueText);
                    }
                }
                i++;
            }

            var dlg = new SaveFileDialog();
            dlg.FileName = "Report " + DateTime.Now.ToLocalTime().ToShortDateString().Replace('/', ' '); // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Title = "Save report ";
            dlg.Filter = "Text file|*.txt";
            // Show save file dialog box

            var result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == DialogResult.OK)
            {

                using (TextWriter tw = new StreamWriter(dlg.FileName))
                {
                    foreach (String s in lst)
                        tw.WriteLine(s);
                }
            }
        }

        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllCheckBoxes(false, false);

        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAllCheckBoxes(true, true);

        }

        private void SelectAllCheckBoxes(bool CheckThem, bool revert)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                var di = checkedListBox1.Items[i];

                if (CheckThem && !revert)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                    //di.IsChecked = true;
                }
                else if (!CheckThem && !revert)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                   // di.IsChecked = false;

                }
                else if (revert)
                {
                    CheckState state = checkedListBox1.GetItemCheckState(i);
                    if (state == CheckState.Checked) checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                    if (state == CheckState.Unchecked) checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                    if (state == CheckState.Indeterminate) checkedListBox1.SetItemCheckState(i, CheckState.Checked);

                    //di.IsChecked = !di.IsChecked;
                }

                
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _updateList.Clear();
            for (int i = 0; i <= checkedListBox1.Items.Count -1; i++)
            {
                DetailedItem di = (DetailedItem)checkedListBox1.Items[i];
                di.IsChecked = checkedListBox1.GetItemChecked(i);
                if (di.IsChecked)
                    _updateList.Add(di);
            }
        }
    }
}
