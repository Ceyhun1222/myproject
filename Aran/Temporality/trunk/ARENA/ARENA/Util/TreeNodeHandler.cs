using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARENA.Util
{
    public class TreeNodeHandler
    {
        public TreeNode Node;
        public Action<TreeNode> OnChecked;
        public Object NodeData;
    }
}
