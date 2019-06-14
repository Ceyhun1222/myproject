using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDM
{
    public partial class ApproachProfileEditorForm : Form
    {
        public ApproachProfileEditorForm()
        {
            InitializeComponent();

            dataGridView1.AllowUserToDeleteRows = Debugger.IsAttached;
            dataGridView2.AllowUserToDeleteRows = Debugger.IsAttached;
            dataGridView3.AllowUserToDeleteRows = Debugger.IsAttached;
            dataGridView4.AllowUserToDeleteRows = Debugger.IsAttached;

            dataGridView1.ReadOnly = !Debugger.IsAttached;
            dataGridView2.ReadOnly = !Debugger.IsAttached;
            dataGridView3.ReadOnly = !Debugger.IsAttached;
            dataGridView4.ReadOnly = !Debugger.IsAttached;

        }

        private void dataGridView1_AllowUserToAddRowsChanged(object sender, EventArgs e)
        {
        }
    }
}
