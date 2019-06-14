using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEnv
{
    public interface ILeftWindow
    {
        Control AreaControl { get; }

        string Title { get; }

        object BaseOn { get; }
    }
}
