using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Aran.Aim.Objects;

namespace Aran.Aim.InputFormLib
{
    public partial class DbEntityControl : UserControl, IAObjectControl
    {
        private DbEntityController _controller;
        private char _hideDetailsText = '►';
        private char _showDetialsText = '▼';
        private bool _isExpanded = false;
        private Font _arialFont;
        private bool _expandable = true;

        public DbEntityControl ()
        {
            InitializeComponent ();

            _arialFont = new Font ("Arial", 12);

            _controller = new DbEntityController ();
            _controller.CreateAimFieldControl += new CreatorAimFieldControlHandler (Controller_CreatorAimProperty);
            _controller.CreateAObjectControl += new CreateAObjectControlHandler (Controller_CreateAObject);

            Height -= (ui_tableLayoutPanel1.Height + 5); ;
            ui_tableLayoutPanel1.Height = 1;
        }

        public void LoadDbEntity (DBEntity dbEntity, AimClassInfo classInfo)
        {
            _controller.Load (dbEntity, classInfo);
        }

        public void LoadObject (AObject aObject, AimClassInfo classInfo)
        {
            LoadDbEntity (aObject, classInfo);
        }

        public bool Expandable
        {
            get { return _expandable; }
            set
            {
                _expandable = value;

                ui_tableLayoutPanel1.AutoSize = _expandable;
                ui_tableLayoutPanel1.AutoScroll = !_expandable;

                ui_tableLayoutPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

                if (!_expandable)
                    ui_tableLayoutPanel1.Anchor |= AnchorStyles.Bottom;
            }
        }

        private IAimFieldControl Controller_CreatorAimProperty ()
        {
            AimFieldControl control = new AimFieldControl ();
            ui_tableLayoutPanel1.RowCount = ui_tableLayoutPanel1.RowCount + 1;
            ui_tableLayoutPanel1.Controls.Add (control);
            return control;
        }

        private IAObjectControl Controller_CreateAObject ()
        {
            DbEntityControl dbEntityControl = new DbEntityControl ();
            ui_tableLayoutPanel1.RowCount = ui_tableLayoutPanel1.RowCount + 1;
            ui_tableLayoutPanel1.Controls.Add (dbEntityControl);
            dbEntityControl.Anchor |= AnchorStyles.Right;
            //dbEntityControl.Expandable = true;
            return dbEntityControl;
        }

        private void ui_titlePanel_Paint (object sender, PaintEventArgs e)
        {
            Control cont = (Control) sender;
            Graphics gr = e.Graphics;

            Color color1 = Color.FromArgb (235, 235, 235);
            Color color2 = Color.LightGray;

            LinearGradientBrush linearBr = new LinearGradientBrush (
                new Point (0, 0),
                new Point (0, cont.Height),
                color1,
                color2);

            gr.FillRectangle (linearBr, new Rectangle (0, 0, cont.Width, cont.Height));

            string title = _controller.Title;

            SizeF titleSize = gr.MeasureString (title, cont.Font);

            gr.DrawString (
                (_isExpanded ? _hideDetailsText : _showDetialsText).ToString (),
                _arialFont,
                new SolidBrush ((_expandable ? SystemColors.ControlText : SystemColors.ControlDark)),
                new PointF (10, 6));

            gr.DrawString (title, cont.Font, new SolidBrush (cont.ForeColor),
                new PointF (40, 10));

            gr.Dispose ();
        }

        private void ui_titlePanel_SizeChanged (object sender, EventArgs e)
        {
            ui_titlePanel.Refresh ();
        }

        private void ui_titlePanel_Click (object sender, EventArgs e)
        {
            if (!_expandable)
                return;

            _isExpanded = !_isExpanded;

            Height = (_isExpanded ? ui_titlePanel.Height : ui_titlePanel.Height + ui_tableLayoutPanel1.Height + 4);
            
            ui_titlePanel.Refresh ();
        }

        private void flowLayoutPanel1_ControlAdded (object sender, ControlEventArgs e)
        {
            if (_expandable)
            {
                Height += e.Control.Height;
            }
        }

        private void flowLayoutPanel1_ControlRemoved (object sender, ControlEventArgs e)
        {
            if (_expandable)
            {
                Height -= e.Control.Height;
            }
        }
    }
}
