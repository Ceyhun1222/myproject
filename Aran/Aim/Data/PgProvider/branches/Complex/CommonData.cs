using System;
using System.Data;
using System.Diagnostics;
using Aran.Aim.Features;
using Aran.Aim.Objects;

namespace Aran.Aim.Data
{
    internal class CommonData
    {
		public static IDbConnection CreateConnection ()
		{
			var conn = new Npgsql.NpgsqlConnection ();
            return conn;
		}

        public static string GetErrorMessage ( Exception ex )
        {
            string message = ex.Message;
            if ( ex is Npgsql.NpgsqlException )
            {
                Npgsql.NpgsqlException pgExcept = ( Npgsql.NpgsqlException ) ex;
                message += ";\r\nDetail: " + pgExcept.Detail + ";\r\nSQL Command: " + pgExcept.ErrorSql;
            }
            return message;
        }

		public static string GetTableName ( IAimObject aimObj )
		{
			if ( aimObj is Feature )
			{
				return "bl_" + ( aimObj as Feature ).FeatureType.ToString ( );
			}

			if ( aimObj is ChoiceClass )
			{
				return ( aimObj as ChoiceClass ).ObjectType.ToString ( );
			}

			if ( aimObj is AObject )
			{
				return "obj_" + ( aimObj as AObject ).ObjectType.ToString ( );
			}


			throw new Exception ( "Not found type of " + aimObj.ToString ( ) );
		}

        public static bool GetConnectionInfo(IDbConnection conn, 
            out string host, out int port, out string db)
        {
            host = null;
            port = 0;
            db = null;

            var pgConn = conn as Npgsql.NpgsqlConnection;
            if (pgConn != null){
                host = pgConn.Host;
                port = pgConn.Port;
                db = pgConn.Database;
                return true;
            }

            return false;
        }

    }
}
