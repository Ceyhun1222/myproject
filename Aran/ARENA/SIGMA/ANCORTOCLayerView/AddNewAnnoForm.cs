using PDM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANCORTOCLayerView
{
    public partial class AddNewAnnoForm : Form
    {

        public List<PDMObject> PDM_List;
        public List<PDMObject> Selected_PDMList;
        public AddNewAnnoForm()
        {
            InitializeComponent();
        }

        public AddNewAnnoForm(List<PDMObject> _pdmList)
        {
            InitializeComponent();

            this.PDM_List = new List<PDMObject>();
            PDM_List.AddRange(_pdmList);
            checkedListBox1.Items.Clear();

            PDM_List = PDM_List.OrderBy(el => el.GetObjectLabel()).ToList();

            foreach (var _pdm in PDM_List)
            {
                    checkedListBox1.Items.Add(_pdm.GetObjectLabel());
            }

            if (PDM_List != null && checkedListBox1.Items != null && checkedListBox1.Items.Count > 0)
            {
                checkedListBox1.SelectedIndex = 0;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selobj = PDM_List[checkedListBox1.SelectedIndex];
            propertyGrid1.SelectedObject = selobj;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Selected_PDMList = new List<PDMObject>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked) this.Selected_PDMList.Add(PDM_List[i]);
            }
        }
    }
}
