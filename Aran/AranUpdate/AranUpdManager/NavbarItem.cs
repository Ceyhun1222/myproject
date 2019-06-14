using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AranUpdateManager
{
    public class NavbarItem : RadioButton
    {
        public NavbarItem()
        {
            Appearance = Appearance.Button;
            FlatAppearance.CheckedBackColor = SystemColors.Highlight;
            FlatStyle = FlatStyle.Flat;

            Font = new Font(DefaultFont.Name, 12F, FontStyle.Regular);
            Location = new Point(13, 13);
            Size = new Size(129, 30);
            TextAlign = ContentAlignment.MiddleCenter;
            UseVisualStyleBackColor = true;
            BaseControlName = string.Empty;
        }

        public string BaseControlName { get; set; }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);

            Font = new Font(DefaultFont.Name, 12F, Checked ? FontStyle.Bold : FontStyle.Regular);
            ForeColor = Checked ? SystemColors.Window : SystemColors.ControlText;
        }
    }
}
