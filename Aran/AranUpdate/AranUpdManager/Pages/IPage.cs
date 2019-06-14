using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AranUpdateManager
{
    interface IPage
    {
        void OpenPage();

        void ClosePage();

        string StatusText { get; }

        event EventHandler StatusTextChanged;
    }
}
