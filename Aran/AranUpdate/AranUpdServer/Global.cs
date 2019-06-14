using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AranUpdServer
{
    static class Global
    {
        public static void WriteLog(string message)
        {
            Console.WriteLine(message);
        }

        public static SQLiteCommand CreateCommand(string commandText = null)
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aran-upd.sdb");
            var isDbExists = File.Exists(fileName);
            var conn = new SQLiteConnection("Data Source=" + fileName);
            conn.Open();

            var cmd = conn.CreateCommand();

            if (!isDbExists)
                CreateTables(conn);
            
            if (commandText != null)
                cmd.CommandText = commandText;
            return cmd;
        }

        public static string GetVersionBinFilePath(long versionId)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "version-bin-files", "id-" + versionId + ".ver");
        }

        public static string MakeServerError(Exception ex)
        {
            Global.WriteLog("Error: " + ex.Message);
            return "Server Error: " + ex.Message;
        }

        private static void CreateTables(SQLiteConnection conn)
        {
            var text = AranUpdManager.Properties.Resources.AranUpd;

            var cmd = conn.CreateCommand();
            cmd.CommandText = text;
            cmd.ExecuteNonQuery();
        }
    }
}
