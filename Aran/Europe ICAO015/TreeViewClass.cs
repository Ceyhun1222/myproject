using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Europe_ICAO015;

namespace ICAO015
{
    public static class TreeViewClass
    {
        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam,
                                                 ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        public static void HideCheckBox(TreeView tvw, TreeNode node)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = node.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }
        public static void OnlyOneCheckTreeNode(TreeView Treeview, TreeNode selectednode, TreeNode selectparentnode, bool CheckTrueFalse)
        {
            if (selectednode.Parent.Text == "Markers" || selectednode.Parent.Text == "NDB")
            {
                string selectnode = selectednode.Text;
                string selectparent = selectednode.Parent.Text;

                foreach (TreeNode nodes1 in Treeview.Nodes)
                {
                    if (nodes1.Text == "Markers" || nodes1.Text == "NDB")
                    {
                        if (nodes1.Text == selectparent)
                        {
                            foreach (TreeNode nodes2 in nodes1.Nodes)
                            {
                                if (nodes2.Text != selectnode)
                                {
                                    nodes2.Checked = false;
                                }
                            }
                        }
                        else if (nodes1.Text != selectparent)
                        {
                            foreach (TreeNode nodes2 in nodes1.Nodes)
                            {
                                nodes2.Checked = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (TreeNode nodes2 in nodes1.Nodes)
                        {
                            foreach (TreeNode lastnode in nodes2.Nodes)
                            {
                                lastnode.Checked = false;
                            }
                        }
                    }
                }
            }
            else
            {
                string selectnode = selectednode.Text;
                string selectparent2 = selectednode.Parent.Text;
                string selectparentmain = selectparentnode.Parent.Text;

                foreach (TreeNode parentnodemain in Treeview.Nodes)
                {
                    if (parentnodemain.Text == selectparentmain)
                    {
                        foreach (TreeNode parent2 in parentnodemain.Nodes)
                        {
                            if (parent2.Text == selectparent2)
                            {
                                foreach (TreeNode lastnode in parent2.Nodes)
                                {
                                    if (lastnode.Text != selectnode)
                                    {
                                        lastnode.Checked = false;
                                    }
                                }
                            }
                            else if (parent2.Text != selectparent2)
                            {
                                foreach (TreeNode lastnode in parent2.Nodes)
                                {
                                    lastnode.Checked = false;
                                }
                            }
                        }
                    }
                    else if (parentnodemain.Text != selectparentmain)
                    {
                        foreach (TreeNode parent2 in parentnodemain.Nodes)
                        {
                            foreach (TreeNode lastnode in parent2.Nodes)
                            {
                                lastnode.Checked = false;
                            }
                        }
                        if (parentnodemain.Text == "Markers" || parentnodemain.Text == "NDB")
                        {
                            foreach (TreeNode lastnode in parentnodemain.Nodes)
                            {
                                lastnode.Checked = false;
                            }
                        }
                    }
                }
            }

            CheckTrueFalse = true;


        }//OnlyOneCheckTreeNode
        public static int checkedt = 0;
        public static int uncheckedt = 0;
        public static int stepystepnocecheckcount = 0;
        public static void CheckAllUnCheckkALLStebyStepCheck(TreeNode child, TreeNode parentnod, int nodecount)
        {
            TreeNode childnode = child;
            TreeNode parentnode = parentnod;
            //---------------------------------------------------------
            TreeNode echildnode = childnode;
            TreeNode eThirdParentnode = parentnode;

            //nodecount = 3;

            if (echildnode.Checked == true)
            {
                if (echildnode.Level == 2)
                {
                    checkedt = checkedt + 1;
                    if (checkedt == 1)
                    {
                        //nodecount = 0;
                        foreach (TreeNode nodelevel3 in echildnode.Nodes)
                        {
                            nodelevel3.Checked = true;
                            //nodecount = nodecount + 1;
                            stepystepnocecheckcount = nodecount;
                            uncheckedt = 0;

                        }
                    }
                }
                if (echildnode.Level == 3)
                {
                    if (checkedt == 0)
                    {
                        checkedt = 1;

                        stepystepnocecheckcount++;

                        if (nodecount == stepystepnocecheckcount)
                        {
                            echildnode.Parent.Checked = true;
                            nodecount = stepystepnocecheckcount;
                        }


                    }
                }
            }

            //false processing------------------------------------------------------------------------------false------false
            if (echildnode.Checked == false)
            {

                if (echildnode.Level == 2)
                {
                    uncheckedt = uncheckedt + 1;
                    if (uncheckedt == 1)
                    {
                        foreach (TreeNode nodelevel3 in echildnode.Nodes)
                        {
                            //nodecount = 0;
                            stepystepnocecheckcount = 1;
                            nodelevel3.Checked = false;
                            checkedt = 0;


                        }
                    }
                }
                if (echildnode.Level == 3)
                {
                    //checkedt = 0;

                    if (uncheckedt == 0)
                    {

                        stepystepnocecheckcount--;

                        uncheckedt = 1;

                        TreeNode parent = echildnode.Parent;
                        parent.Checked = false;
                    }
                }
            }


            uncheckedt = 0;
            checkedt = 0;
        }

    }
}
