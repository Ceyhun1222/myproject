using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Aran.AranEnvironment
{
    public interface ISettingsPage
    {
        string Title { get; }
        Control Page { get; }
        void OnLoad ();
        bool OnSave ();
    }
}
