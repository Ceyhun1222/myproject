using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Aran.Queries.Common
{
    public class NButton : Button
    {
        public NButton ()
        {
            OnSizeChanged (null);
        }

        protected override void OnSizeChanged (EventArgs e)
        {
            base.OnSizeChanged (e);

            GraphicsPath grPath = new GraphicsPath ();
            grPath.AddEllipse (5, 5, this.Width - 10, this.Height - 10);
            this.Region = new Region (grPath);
        }
    }
}
