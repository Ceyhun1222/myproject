using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARENA
{
    public partial class AddNavaidComponentButton : UserControl
    {
        public delegate void BtnClicked();
        public event BtnClicked onClicked;
        private string _navSystem;

        public string NavSystem
        {
            get { return _navSystem; }
            set { _navSystem = value; }
        }

        public AddNavaidComponentButton()
        {
            InitializeComponent();
            this.NavSystem = "Add VOR";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            onClicked();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Text = comboBox1.SelectedItem.ToString();
            NavSystem = comboBox1.SelectedItem.ToString();
           // onClicked();
        }

        
    }


}
