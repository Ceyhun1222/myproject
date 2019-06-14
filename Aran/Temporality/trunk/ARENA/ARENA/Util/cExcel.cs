using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace ARENA.Util
{

    class cExcel
    {
        public static string sMessage { get; private set; }

        ///
        /// Преобразует Excel-файл в DataTable
        ///
        ///Таблица для загрузки данных
        ///Полный путь к Excel-файлу
        ///SQL-запрос. Используйте $SHEETS$ для выбоки по всем листам
        public static void ExcelFileToDataTable(out DataTable dtData, string sFile, string sRequest)
        {
            DataSet dsData = new DataSet();

            string sConnStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1};HDR=YES\";", sFile, sFile.EndsWith(".xlsx") ? "Excel 12.0 Xml" : "Excel 8.0");

            using (OleDbConnection odcConnection = new OleDbConnection(sConnStr))
            {
                odcConnection.Open();
                if (sRequest.IndexOf("$SHEETS$") != -1)
                {
                    using (DataTable dtMetadata = odcConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[4] { null, null, null, "TABLE" }))
                    {
                        for (int i = 0; i < dtMetadata.Rows.Count; i++)
                            if (dtMetadata.Rows[i]["TABLE_NAME"].ToString().IndexOf("$") == -1)
                                dtMetadata.Rows.Remove(dtMetadata.Rows[i]);

                        foreach (DataRow drRow in dtMetadata.Rows)
                        {
                            string sLocalRequest = sRequest.Replace("$SHEETS$", String.Format("[{0}]", drRow["TABLE_NAME"]));
                            OleDbCommand odcCommand = new OleDbCommand(sLocalRequest, odcConnection);
                            using (OleDbDataAdapter oddaAdapter = new OleDbDataAdapter(((OleDbCommand)odcCommand)))
                                oddaAdapter.Fill(dsData);
                        }
                    }
                }
                else
                {
                    OleDbCommand odcCommand = new OleDbCommand(sRequest, odcConnection);
                    using (OleDbDataAdapter oddaAdapter = new OleDbDataAdapter(odcCommand))
                        oddaAdapter.Fill(dsData);
                }
                odcConnection.Close();
            }

            dtData = dsData.Tables[0];
        }

        ///
        /// Преобразует dataTable в Excel-файл
        ///
        ///Данные
        ///Полный путь к файлу
        ///Excel 2007 или новее
        /// В случае успеха возвращает true
        public static bool DataTableToExcelFile(DataTable dtData, string sFilePath, bool b2007)
        {
            try
            {
                // Исправляем имя таблицы
                if (dtData.TableName.Length != 0 || dtData.TableName.Equals("Table", StringComparison.OrdinalIgnoreCase) == true)
                    dtData.TableName = "NONAME";

                string sConnStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"{1};HDR=YES\";", sFilePath, b2007 ? "Excel 12.0 Xml" : "Excel 8.0");

                using (OleDbConnection odcConnection = new OleDbConnection(sConnStr))
                {
                    odcConnection.Open();
                    using (OleDbCommand odcCommand = new OleDbCommand() { Connection = odcConnection })
                    {
                        // Создание таблицы
                        odcCommand.CommandText = GenerateSqlStatementCreateTable(dtData);
                        odcCommand.ExecuteNonQuery();

                        DataRow dr;
                        OleDbParameter odpParameter;

                        // Генерируем скрипт создания строк со значениями (в качестве параметров)
                        string sColumns, sParameters;
                        GenerateColumnsString(dtData, out sColumns, out sParameters);

                        for (int i = 0; i < dtData.Rows.Count; i++)
                        {
                            dr = dtData.Rows[i];
                            // Устанавливаем параметр для INSERT
                            odcCommand.Parameters.Clear();
                            for (int j = 0; j < dtData.Columns.Count; j++)
                            {
                                odpParameter = new OleDbParameter();
                                odpParameter.ParameterName = "@p" + j;

                                odpParameter.Value = dr.IsNull(j) ? DBNull.Value : dr[j];

                                odcCommand.Parameters.Add(odpParameter);
                            }
                            odcCommand.CommandText = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", dtData.TableName, sColumns, sParameters);
                            odcCommand.ExecuteNonQuery();
                        }
                    }
                    odcConnection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
                return false;
            }
        }

        ///
        /// Создает список столбцов ([columnname0],[columnname1],[columnname2])
        /// и соответствующих им параметров (@p0,@p1,@p2).
        /// В качестве разделителя используется запятая
        ///
        ///Данные
        ///Список столбцов
        ///Список параметров
        private static void GenerateColumnsString(DataTable dtData, out string sColumns, out string sParams)
        {
            StringBuilder sbColumns = new StringBuilder();
            StringBuilder sbParams = new StringBuilder();
            for (int i = 0; i < dtData.Columns.Count; i++)
            {
                if (i != 0)
                {
                    sbColumns.Append(',');
                    sbParams.Append(',');
                }
                sbColumns.AppendFormat("[{0}]", dtData.Columns[i].ColumnName);
                sbParams.AppendFormat("@p{0}", i);
            }

            sColumns = sbColumns.ToString();
            sParams = sbParams.ToString();
        }

        ///
        /// Создает SQL-скрипт для создания таблицы, в соответствии с DataTable
        ///
        ///Данные
        /// Возвращает запрос 'CREATE TABLE...'
        private static string GenerateSqlStatementCreateTable(DataTable dtData)
        {
            StringBuilder sbCreateTable = new StringBuilder();

            DataColumn dc;

            sbCreateTable.AppendFormat("CREATE TABLE {0} (", dtData.TableName);
            for (int i = 0; i < dtData.Columns.Count; i++)
            {
                dc = dtData.Columns[i];

                if (i != 0) sbCreateTable.Append(",");

                string dataType = dc.DataType.Equals(typeof(double)) ? "DOUBLE" : "VARCHAR";

                sbCreateTable.AppendFormat("[{0}] {1}", dc.ColumnName, dataType);
            }
            sbCreateTable.Append(")");

            return sbCreateTable.ToString();
        }

       

       //Примеры использования:

        //DataTable dtData;
 
        //using (OpenFileDialog ofdOpen = new OpenFileDialog() { Filter = "Книга Excel|*.xlsx|Книга Excel 97-2003|*.xls|Все файлы|*.*" })
        //{
        //    if (ofdOpen.ShowDialog() != DialogResult.OK)
        //        return;
        //    cExcel.ExcelFileToDataTable(out dtData, ofdOpen.FileName, "SELECT * FROM $SHEETS$");
        //}
 
        //// dtData у нас содержит содержимое Excel-файла. Обрабатываем как хотим
 
        //using (SaveFileDialog sfdSave = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx|Книга Excel 97-2003|*.xls" })
        //{
        //    if (sfdSave.ShowDialog() == DialogResult.OK) cExcel.DataTableToExcelFile(dtData, sfdSave.FileName, sfdSave.FileName.EndsWith(".xlsx"));
        //}

                ///////////


    }
}
