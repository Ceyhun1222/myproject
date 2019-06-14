using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Aran.Queries.Common.Properties;

namespace Aran.Controls
{
    internal class PageButton : RadioButton
    {
        private ContextMenuStrip _contMenuStrip;
        private bool _inForm;
        private PictureBox _closeButton;
        private bool _hideCloseButton;

        public event EventHandler CloseClicked;
        public event EventHandler FloatClicked;
        public event EventHandler FloatWithClicked;
        public event EventHandler DockedClicked;

        public PageButton ()
        {
            InitComponents ();

            InForm = false;
            Image = (Checked ? Global.GetActiveImage (Height) : Global.GetImage (Height));
            _hideCloseButton = false;

            #region CloseButton
            _closeButton = new PictureBox ();
            _closeButton.Size = new Size (16, 16);
            _closeButton.Image = Resources.close_16;
            _closeButton.SizeMode = PictureBoxSizeMode.CenterImage;
            _closeButton.Location = new Point (Width - _closeButton.Width - 4, (Height - _closeButton.Height) / 2);
            _closeButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            _closeButton.MouseEnter += new EventHandler (CloseButton_MouseEnter);
            _closeButton.MouseLeave += new EventHandler (CloseButton_MouseLeave);
            _closeButton.Click += new EventHandler (CloseButton_Click);
            Controls.Add (_closeButton);
            #endregion
        }

        public bool VisibleContextStrip
        {
            get
            {
                return (ContextMenuStrip != null);
            }
            set
            {
                ContextMenuStrip = (value ? _contMenuStrip : null);
                _hideCloseButton = !value;
                _closeButton.Visible = value;
            }
        }

        public bool InForm
        {
            get
            {
                return _inForm;
            }
            set
            {
                _inForm = value;

                _contMenuStrip.Items ["ui_floatMenuItem"].Visible = !value;
                _contMenuStrip.Items ["ui_floatWithMenuItem"].Visible = !value;
                _contMenuStrip.Items ["ui_dockMenuItem"].Visible = value;
            }
        }


        private void InitComponents ()
        {
            Appearance = Appearance.Button;
            TextAlign = ContentAlignment.MiddleCenter;

            _contMenuStrip = new ContextMenuStrip ();

            ToolStripMenuItem tsmi = new ToolStripMenuItem ();
            tsmi.Text = "Close";
            tsmi.Name = "ui_closeMenuItem";
            tsmi.Click += new EventHandler (CloseMenuItem_Click);
            _contMenuStrip.Items.Add (tsmi);

            tsmi = new ToolStripMenuItem ();
            tsmi.Text = "Float";
            tsmi.Name = "ui_floatMenuItem";
            tsmi.Click += new EventHandler (FloatMenuItem_Click);
            _contMenuStrip.Items.Add (tsmi);

            tsmi = new ToolStripMenuItem ();
            tsmi.Text = "Float With";
            tsmi.Name = "ui_floatWithMenuItem";
            tsmi.Click += new EventHandler (FloatWithMenuItem_Click);
            _contMenuStrip.Items.Add (tsmi);

            tsmi = new ToolStripMenuItem ();
            tsmi.Text = "Dock";
            tsmi.Name = "ui_dockMenuItem";
            tsmi.Click += new EventHandler (DockMenuItem_Click);
            _contMenuStrip.Items.Add (tsmi);

            ContextMenuStrip = _contMenuStrip;
        }

        private void CloseMenuItem_Click (object sender, EventArgs e)
        {
            if (CloseClicked != null)
                CloseClicked (this, e);
        }

        private void FloatMenuItem_Click (object sender, EventArgs e)
        {
            if (FloatClicked != null)
                FloatClicked (this, e);
        }

        private void FloatWithMenuItem_Click (object sender, EventArgs e)
        {
            if (FloatWithClicked != null)
                FloatWithClicked (this, e);
        }

        private void DockMenuItem_Click (object sender, EventArgs e)
        {
            if (DockedClicked != null)
                DockedClicked (this, e);
        }

        protected override void OnMouseDown (MouseEventArgs mevent)
        {
            base.OnMouseDown (mevent);

            if (!Checked)
                Checked = true;
        }

        protected override void OnCheckedChanged (EventArgs e)
        {
            base.OnCheckedChanged (e);

            Image = (Checked ? Global.GetActiveImage (Height) : Global.GetImage (Height));

            if (!_hideCloseButton)
                _closeButton.Visible = Checked;
        }

        protected override void OnPaint (PaintEventArgs pevent)
        {
            base.OnPaint (pevent);

            Graphics gr = pevent.Graphics;
            gr.FillRectangle (Brushes.White, 0, 0, Width, Height);
            gr.DrawImage (Image, new Rectangle (0, 0, Width + 10, Height));
            gr.DrawRectangle (new Pen (Color.Gray, 1), new Rectangle (0, 0, Width - 1, Height - 1));

            StringFormat textFormat = new StringFormat ();
            textFormat.Alignment = StringAlignment.Near;
            textFormat.LineAlignment = StringAlignment.Center;
            textFormat.FormatFlags = StringFormatFlags.NoWrap;
            textFormat.Trimming = StringTrimming.EllipsisCharacter;

            gr.DrawString (Text, Font,
                SystemBrushes.ControlText,
                new RectangleF (20, 0, Width - _closeButton.Width - 24, Height), textFormat);
        }

        protected override void OnSizeChanged (EventArgs e)
        {
            Image = (Checked ? Global.GetActiveImage (Height) : Global.GetImage (Height));

            base.OnSizeChanged (e);
        }

        private void CloseButton_Click (object sender, EventArgs e)
        {
            if (CloseClicked != null)
                CloseClicked (this, e);
        }

        private void CloseButton_MouseLeave (object sender, EventArgs e)
        {
            _closeButton.Image = Resources.close_16;
        }

        private void CloseButton_MouseEnter (object sender, EventArgs e)
        {
            _closeButton.Image = Resources.hover_close_16;
        }
    }
}
