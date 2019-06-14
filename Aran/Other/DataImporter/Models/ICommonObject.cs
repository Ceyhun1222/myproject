using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using MahApps.Metro.Controls.Dialogs;

namespace DataImporter.Models
{
    public interface ICommonObject
    {
        ILogger Logger { get; set; }
        IDialogCoordinator DialogCoordinator { get; set; }
    }
}
