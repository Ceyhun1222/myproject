using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Panda.Conventional.Settings
{
	public class ConnectionInfo
	{
		public ConnectionType ConnectedType;
		public PgConnectionInfo PgConnection;
		public MDBConnectionInfo MDBConnection;

		public ConnectionInfo ( )
		{
			PgConnection = new PgConnectionInfo ( );
			MDBConnection = new MDBConnectionInfo ( );
		}

		//Registry_Contract.PutInt32(handle, (int)ConnectedType);
		//if (ConnectedType == ConnectionType.connectionTypePostgres)
		//{
		//    Registry_Contract.PutString(handle, PgConnection.Host);
		//    Registry_Contract.PutString(handle, PgConnection.DbName);
		//    Registry_Contract.PutString(handle, PgConnection.User);
		//    Registry_Contract.PutString(handle, PgConnection.Password);
		//}
		//else
		//{
		//    Registry_Contract.PutString(handle, MDBConnection.FileName);
		//}

		public void Assign ( ConnectionInfo value )
		{
			ConnectedType = value.ConnectedType;
			PgConnection.Assign ( value.PgConnection );
			MDBConnection.Assign ( value.MDBConnection );
		}

		public ConnectionInfo Clone ( )
		{
			ConnectionInfo result = new ConnectionInfo ( );
			result.Assign ( this );
			return result;
		}
	}
}
