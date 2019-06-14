using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Data;

namespace Aran.Omega.TypeB.Settings
{
    public class DbConnectionModel
    {
        public DbConnectionModel()
        {
            IsEmpty = true;
        }
        public string Host { get; set; }
        public string DbName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public DbProvider DbProvider { get; set; }
        public bool IsEmpty { get; set; }

    }
}
