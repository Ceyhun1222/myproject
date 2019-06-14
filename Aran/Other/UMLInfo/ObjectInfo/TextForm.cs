using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UMLInfo
{
    public partial class TextForm : Form
    {
        private IWin32Window _owner;

        public TextForm (IWin32Window owner)
        {
            InitializeComponent ();

            _owner = owner;
        }

        public void ShowForm (string text)
        {
            textBox1.Text = text;
            Show (_owner);
            Clipboard.SetText (textBox1.Text);
        }

        public void ShowForm (string [] lines)
        {
            textBox1.Lines = lines;
            Show (_owner);
            
            if (textBox1.Text.Length > 0)
                Clipboard.SetText (textBox1.Text);
        }

        private void closeButton_Click (object sender, EventArgs e)
        {
            Close ();
        }

        private void TextForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide ();
        }
    }
}
