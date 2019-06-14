using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aran.AranEnvironment;
using MahApps.Metro.Controls.Dialogs;

namespace DataImporter.Models
{
    class CommonObject:ICommonObject
    {
        public CommonObject(ILogger logger, IDialogCoordinator dialogCoordinator)
        {
            Logger = logger;
            DialogCoordinator = dialogCoordinator;
        }
        public ILogger Logger { get; set; }
        public IDialogCoordinator DialogCoordinator { get; set; }
    }
}
