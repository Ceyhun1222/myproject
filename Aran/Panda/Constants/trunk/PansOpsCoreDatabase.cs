using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace Aran.PANDA.Constants
{
	public class PansOpsCoreDatabase
	{
		private const string FileName = "panscore";

		private OleDbConnection _conn;
		public PansOpsCoreDatabase()
		{
			DepWs = new PansOpsCoreData();
			ArVisualWs = new PansOpsCoreData();
			FAirportIsAtC = new PansOpsCoreData();
		}

		public PansOpsCoreData DepWs { get; set; }
		public PansOpsCoreData ArVisualWs { get; set; }
		public PansOpsCoreData FAirportIsAtC { get; set; }

		public void LoadFromFile(string installDir)
		{
			if (!OpenConnection(installDir))
				throw new Exception("Cannot find directory");

			OleDbCommand pansopsCommand = new OleDbCommand("Select * from " + FileName, _conn);

			try
			{
				OleDbDataReader reader = pansopsCommand.ExecuteReader();
				while (reader.Read())
				{
					string parName = (string)reader["NAME"];
					PansOpsCoreData tmpData;

					if (parName == "depWS")
						tmpData = DepWs;
					else if (parName == "arVisualWS")
						tmpData = ArVisualWs;
					else if (parName == "fAirportISAtC")
						tmpData = FAirportIsAtC;
					else
						continue;

					tmpData.Multiplier = (double)reader["MULTIPLIER"];
					tmpData.Value = (double)reader["VALUE"] * tmpData.Multiplier;
					tmpData.Unit = (string)reader["UNIT"];
				}
			}
			catch (Exception)
			{
				throw new Exception("Cannot find file!");
			}
		}

		private bool OpenConnection(string pathFolder)
		{
			try
			{
				_conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathFolder + ";Extended Properties=dBASE IV;");
				_conn.Open();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void CloseConnection()
		{
			_conn.Close();
		}
	}


	public class PansOpsCoreData
	{
		public string Name { get; set; }
		public double Value { get; set; }
		public double Multiplier { get; set; }
		public string Unit { get; set; }
	}
}

