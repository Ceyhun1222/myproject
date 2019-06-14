using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace ARENA
{
    public class ArenaTreeNode : TreeNode
    {
        //public delegate void OnVisibilitiFlagChanged();
        //public event OnVisibilitiFlagChanged VisibilitiFlagChanged;

        public ArenaTreeNode()
        {
            
        }

        public ArenaTreeNode(string _Text)
        {
            this.Text = _Text;
        }

        public ArenaTreeNode(string _Text, bool VisibilitiFlag)
        {
            this.Text = _Text;

            if (VisibilitiFlag)
            {
                this.NodeFont = new Font(new FontFamily("Times New Roman"), 9, FontStyle.Regular);
            }
            else
            {
                this.NodeFont = new Font(new FontFamily("Times New Roman"), 9, FontStyle.Italic | FontStyle.Strikeout);
            }

            
        }

        public void ArenaTreeNodeVisibilityChanged(bool VisibilitiFlag)
        {
            if (VisibilitiFlag)
            {
                this.NodeFont = new Font(new FontFamily("Times New Roman"), 9, FontStyle.Regular);
            }
            else
            {
                this.NodeFont = new Font(new FontFamily("Times New Roman"), 9, FontStyle.Italic | FontStyle.Strikeout);
            }
        }

        
    }

   
}
