using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.Metadata.UI
{
    public class UIPropInfo
    {
        public UIPropInfo ()
        {
            ShowGridView = false;
        }

        public string Caption { get; set; }
        public bool ShowGridView { get; set; }

        public int Position { get; set; }
    }
}
