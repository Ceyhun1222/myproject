using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Panda.Conventional.Settings
{
	public class PgConnectionInfo
	{

		public string Host;
		public string DbName;
		public string User;
		public string Password;

		public PgConnectionInfo ( )
		{
			Host = "";
			DbName = "";
			User = "";
			Password = "";
		}

		public void Assign ( PgConnectionInfo value )
		{
			Host = value.Host;
			DbName = value.DbName;
			User = value.User;
			Password = value.Password;
		}
	}
}
