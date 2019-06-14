using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv.Controls
{
    public partial class LeftWindowTitle : UserControl
    {
        private bool _isActive;
        private string _title;
        public event EventHandler CloseClicked;

        public LeftWindowTitle ()
        {
            InitializeComponent ();

            _title = "<Name>";
        }

        public bool VisibleCloseButton
        {
            get { return ui_closePictureBox.Visible; }
            set { ui_closePictureBox.Visible = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                BackgroundImage = (value ?
                    Properties.Resources.left_win_title_active_backg :
                    Properties.Resources.left_win_title_backg);

                if (ui_closePictureBox.Visible)
                    ui_closePictureBox.BackgroundImage = BackgroundImage;
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                Refresh ();
            }
        }

        protected override void OnPaint (PaintEventArgs e)
        {
            base.OnPaint (e);

            e.Graphics.DrawString (_title, Font, Brushes.Black,
                new RectangleF (2, 2, ui_closePictureBox.Left, Height - 2));
        }


        private void Close_Click (object sender, EventArgs e)
        {
            if (CloseClicked != null)
                CloseClicked (this, e);
        }

        private void CosePictureBox_MouseEnter (object sender, EventArgs e)
        {
            ui_closePictureBox.Image = Properties.Resources.close_title_hover_24;
        }

        private void ClosePictureBox_MouseLeave (object sender, EventArgs e)
        {
            ui_closePictureBox.Image = Properties.Resources.close_title_24;
        }

        private void ClosePictureBox_MouseDown (object sender, MouseEventArgs e)
        {
            ui_closePictureBox.Image = Properties.Resources.close_title_click_24;
        }

        private void ClosePictureBox_MouseUp (object sender, MouseEventArgs e)
        {
            ui_closePictureBox.Image = Properties.Resources.close_title_hover_24;
        }
    }
}
