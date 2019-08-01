using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoKiosk {
	class ExcelReader {
		public static DataTable ReadExcelFile(string fileName, string sheetName) {
			Logging.ToLog("Считывание файла: " + fileName + ", лист: " + sheetName);
			DataTable dataTable = new DataTable();

			if (!File.Exists(fileName))
				return dataTable;

			try {
				using (OleDbConnection conn = new OleDbConnection()) {
					conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Mode=Read;" +
						"Extended Properties='Excel 12.0 Xml;HDR=NO;'";

					using (OleDbCommand comm = new OleDbCommand()) {
						if (string.IsNullOrEmpty(sheetName)) {
							conn.Open();
							DataTable dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
								new object[] { null, null, null, "TABLE" });
							sheetName = dtSchema.Rows[0].Field<string>("TABLE_NAME");
							conn.Close();
						} else
							sheetName += "$";

						comm.CommandText = "Select * from [" + sheetName + "]";
						comm.Connection = conn;

						using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter()) {
							oleDbDataAdapter.SelectCommand = comm;
							oleDbDataAdapter.Fill(dataTable);
						}
					}
				}
			} catch (Exception e) {
				Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
			}

			return dataTable;
		}
	}
}
