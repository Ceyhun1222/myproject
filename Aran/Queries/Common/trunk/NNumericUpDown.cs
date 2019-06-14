using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.Queries.Common
{
    public class NNumericUpDown : NumericUpDown
    {
        private TextBox _textBox;
        private bool _isNull;

        public NNumericUpDown ()
        {
            _textBox = new TextBox ();
            _textBox.Visible = false;
            _textBox.Text = "";//<null>";
            _textBox.Font = new Font (this.Font, FontStyle.Italic);
            _textBox.BorderStyle = BorderStyle.None;
            _textBox.ReadOnly = true;
            _textBox.BackColor = SystemColors.Window;
            _textBox.Multiline = true;
            _textBox.GotFocus += new EventHandler (textBox_GotFocus);

            OnSizeChanged (null);
            IsNull = true;
        }

        protected override void OnEnabledChanged (EventArgs e)
        {
            base.OnEnabledChanged (e);

            _textBox.BackColor = (Enabled ? SystemColors.Window : SystemColors.Control);
        }

        public bool IsNull
        {
            get { return _isNull; }
            set
            {
                _isNull = value;

                _textBox.Visible = _isNull;
            }
        }

        public new int Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                _textBox.Width = value - 4;
            }
        }

        protected override void OnParentChanged (EventArgs e)
        {
            base.OnParentChanged (e);

            if (Parent == null)
                return;

            Parent.Controls.Add (_textBox);
            _textBox.BringToFront ();
            _textBox.Visible = false;
        }

        protected override void OnSizeChanged (EventArgs e)
        {
            base.OnSizeChanged (e);

            _textBox.Size = new Size (Width - 20, Height - 4);
        }

        protected override void OnLocationChanged (EventArgs e)
        {
            base.OnLocationChanged (e);
            _textBox.Location = new System.Drawing.Point (Location.X + 2, Location.Y + 2);
        }

        protected override void OnLeave (EventArgs e)
        {
            base.OnLeave (e);

            if (!Enabled)
                return;

            if (_isNull)
            {
                _textBox.Visible = true;
            }
        }

        protected override void OnValueChanged (EventArgs e)
        {
            base.OnValueChanged (e);

            _isNull = false;
        }

        private void textBox_GotFocus (object sender, EventArgs e)
        {
            if (!Enabled)
                return;

            _textBox.Visible = false;
            Focus ();
        }
    }
}
