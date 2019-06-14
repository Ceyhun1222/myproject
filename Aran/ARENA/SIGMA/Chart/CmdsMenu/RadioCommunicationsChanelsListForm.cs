using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDM;

namespace SigmaChart.CmdsMenu
{
    public partial class RadioCommunicationsChanelsListForm : Form
    {
        public List<RadioCommunicationChanel> ChanelsList;
        public List<string> SelectedChanelsList;

        public RadioCommunicationsChanelsListForm()
        {
            InitializeComponent();
        }

        public RadioCommunicationsChanelsListForm( List<RadioCommunicationChanel> _lst, List<string> _selected_lst)
        {
            InitializeComponent();

            this.ChanelsList = new List<RadioCommunicationChanel>();
            this.ChanelsList.AddRange(_lst);
            All_listBox.Items.Clear();

            List<string> lst = TerminalChartsUtil.getChanelsList(this.ChanelsList);

            if (lst != null)
            {
                int cntr = 0;
                foreach (var item in lst)
                {
                    All_listBox.Items.Add(item);
                    cntr++;
                }
            }

            if (_selected_lst !=null)
            {
                foreach (var item in _selected_lst)
                {
                    SelectedListBox.Items.Add(item);

                }
                
            }

            buttonRemove.Enabled = SelectedListBox.Items.Count > 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.SelectedChanelsList = new List<string>();
            foreach (var item in SelectedListBox.Items)
            {
                this.SelectedChanelsList.Add(item.ToString());
            }

        }

        private void MoveItem(int direction)
        {
            if (SelectedListBox.SelectedItem == null || SelectedListBox.SelectedIndex < 0)
                return; 

            int newIndex = SelectedListBox.SelectedIndex + direction;

            if (newIndex < 0 || newIndex >= SelectedListBox.Items.Count)
                return; 

            object selected = SelectedListBox.SelectedItem;

            SelectedListBox.Items.Remove(selected);
            SelectedListBox.Items.Insert(newIndex, selected);
            SelectedListBox.SetSelected(newIndex, true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
           if(SelectedListBox.SelectedIndex >= 0) SelectedListBox.Items.Remove(SelectedListBox.Items[SelectedListBox.SelectedIndex]);
            buttonRemove.Enabled = SelectedListBox.Items.Count > 0;

            if (SelectedListBox.Items.Count > 0) { SelectedListBox.SelectedIndex = SelectedListBox.Items.Count - 1;  SelectedListBox.Select(); }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            
            if (SelectedListBox.Items.IndexOf(All_listBox.Items[All_listBox.SelectedIndex]) >= 0)
            {
                MessageBox.Show("Already selected");
                return;
            }
            if (All_listBox.SelectedIndex >= 0) SelectedListBox.Items.Add(All_listBox.Items[All_listBox.SelectedIndex]);
            buttonRemove.Enabled = SelectedListBox.Items.Count > 0;
        }

        private void All_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void All_listBox_DoubleClick(object sender, EventArgs e)
        {
            buttonSelect_Click(sender, e);
        }

        private void SelectedListBox_DoubleClick(object sender, EventArgs e)
        {
            buttonRemove_Click(sender, e);
        }
    }
}
