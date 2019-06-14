using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Panda.Conventional.Settings
{
	public class MDBConnectionInfo
	{
		public string FileName;

		public MDBConnectionInfo ( )
		{
			FileName = "";
		}

		public void Assign ( MDBConnectionInfo value )
		{
			FileName = value.FileName;
		}
	}
}
