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
    public partial class HoldingsListForm : Form
    {

        public List<PDMObject> HoldingsList;
        public List<PDMObject> SelectedHoldingsList;

        public HoldingsListForm()
        {
            InitializeComponent();
        }

        public HoldingsListForm(List<PDMObject> _lst)
        {
            InitializeComponent();

            this.HoldingsList = new List<PDMObject>();
            HoldingsList.AddRange(_lst);
            checkedListBox1.Items.Clear();


            HoldingsList.RemoveAll(hld => ((HoldingPattern)hld).HoldingPoint == null);

            HoldingsList = HoldingsList.OrderBy(hld =>  ((HoldingPattern)hld).HoldingPoint.SegmentPointDesignator).ToList();

            foreach (var hld in HoldingsList)
            {
                if (((HoldingPattern)hld).HoldingPoint != null)
                    checkedListBox1.Items.Add("Holding " + ((HoldingPattern)hld).Type.ToString() +" " + ((HoldingPattern)hld).HoldingPoint.SegmentPointDesignator + " (" + ((HoldingPattern)hld).TurnDirection.ToString() + " " + (((HoldingPattern)hld).InboundCourse.HasValue? Math.Round(((HoldingPattern)hld).InboundCourse.Value).ToString() : "")+")");
                else
                    checkedListBox1.Items.Add("Holding");
            }

            if (HoldingsList!=null && checkedListBox1.Items!=null && checkedListBox1.Items.Count > 0)
            {
                checkedListBox1.SelectedIndex = 0;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            this.SelectedHoldingsList = new List<PDMObject>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked) this.SelectedHoldingsList.Add(HoldingsList[i]);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selHldn = HoldingsList[checkedListBox1.SelectedIndex];
            propertyGrid1.SelectedObject = selHldn;
        }
    }
}
