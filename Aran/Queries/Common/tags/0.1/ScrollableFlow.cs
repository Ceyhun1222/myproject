using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Aran.Queries.Common
{
    public partial class ScrollableFlow : UserControl
    {
        public ScrollableFlow ()
        {
            InitializeComponent ();
        }

        private void leftLabel_Click (object sender, EventArgs e)
        {
            try
            {
                Flow.HorizontalScroll.Value += Flow.HorizontalScroll.LargeChange;
            }
            catch { }
        }
    }
}
