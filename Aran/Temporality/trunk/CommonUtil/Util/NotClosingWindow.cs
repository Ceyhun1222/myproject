using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Aran.Temporality.CommonUtil.Util
{
    public class NotClosingWindow : Window
    {
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            Hide();
        }
    }
}
