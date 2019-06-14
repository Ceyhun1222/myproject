using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Aran.Package;

namespace Aran.AranEnvironment
{
	public class Connection : AranObject, IAranCloneable, IPackable
	{
		public Connection()
        {
            ConnectionType = ConnectionType.Aran;
			Port = 0;

			Server =
                UserName =
                Password =
                Database =
                XmlFileName = string.Empty;

			UseCache = false;
		}

		public ConnectionType ConnectionType
		{
			get;
			set;
		}
		public string Server
		{
			get;
			set;
		}
		public int Port
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
		public string Password
		{
			get;
			set;
		}
		public string Database
		{
			get;
			set;
		}
		public string XmlFileName
		{
			get;
			set;
		}
		public bool UseCache
		{
			get;
			set;
		}

		public AranObject Clone()
		{
			Connection conn = new Connection ();
			conn.Assign (this);
			return conn;
		}

		public void Assign(AranObject source)
		{
			Connection src = (Connection) source;

			ConnectionType = src.ConnectionType;
			Server = src.Server;
			Port = src.Port;
			UserName = src.UserName;
			Password = src.Password;
			Database = src.Database;
			XmlFileName = src.XmlFileName;
			UseCache = src.UseCache;
		}

		public void Pack(PackageWriter writer)
		{
			writer.PutInt32 ((int) ConnectionType);
			writer.PutString (Server);
			writer.PutInt32 (Port);
			writer.PutString (UserName);
			writer.PutString (Database);
			writer.PutString (XmlFileName);
			writer.PutBool (UseCache);

			if (DataVersion > 3000)
			{
				writer.PutString (Password);
			}
			else
			{
				if (ConnectionType == ConnectionType.ComSoft)
					writer.PutString (Password);
			}
		}

		public void Unpack(PackageReader reader)
		{
			ConnectionType = (ConnectionType) reader.GetInt32 ();
			Server = reader.GetString ();
			Port = reader.GetInt32 ();
			UserName = reader.GetString ();
			Database = reader.GetString ();
			XmlFileName = reader.GetString ();
			UseCache = reader.GetBool ();

			if (DataVersion > 3000)
			{
				Password = reader.GetString ();
			}
			else
			{
				if (ConnectionType == ConnectionType.ComSoft)
					Password = reader.GetString ();
			}
		}

		public int DataVersion { get; set; }

		public string GetConnectionString()
		{
			switch (this.ConnectionType)
			{
				case ConnectionType.Aran:
					{
						string userName, password;
						GetUserName (out userName, out password);

						return string.Format (
							"Server={0};Port={1};User Id={3};Password={4};Database={2};CommandTimeout=340",
							Server, Port, Database, userName, password);

					}
				case ConnectionType.ComSoft:
                    
                    return string.Format(
                        "Server={0}\nPort={1}\nUserName={2}\nPassword={3}\nUseCache={4}",
                        Server, Port, UserName, Password, UseCache);

				case ConnectionType.TDB:
                    // TODO: return config name
                    return null;
                case AranEnvironment.ConnectionType.AimLocal:
                    return Server;
				default:
					return XmlFileName;
			}
		}

		private void GetUserName(out string userName, out string password)
		{
			if (SysUserName != null)
			{
				userName = SysUserName;
				password = SysPassword;
			}

			var dir = typeof (Connection).Assembly.Location;
			dir = Path.GetDirectoryName (dir);
			var userInfoFileName = dir + "\\AranDB.config";

			if (!File.Exists (userInfoFileName))
			{
#if EST
                userName = "panda";
                password = "panda";
#else
				userName = "aran";
				password = "airnav2012";
#endif
			}
			else
			{
				var xmlDoc = new XmlDocument ();

				try
				{
					xmlDoc.Load (userInfoFileName);

					var xmlElem = xmlDoc.DocumentElement["UserInfo"];
					userName = xmlElem.Attributes["Name"].Value;
					password = xmlElem.Attributes["Password"].Value;
				}
				catch (Exception ex)
				{
					throw new Exception ("Error reading AranDB.config", ex);
				}
			}

			SysUserName = userName;
			SysPassword = password;
		}

		private static string SysUserName;
		private static string SysPassword;
	}

	public enum ConnectionType
	{
		Aran,
		ComSoft,
		XmlFile,
		TDB,
        AimLocal
	}
}
