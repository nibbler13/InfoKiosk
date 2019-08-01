using System;
using System.Data;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;

namespace InfoKiosk {
    class SystemFirebirdClient {
        private FbConnection connection;

		public SystemFirebirdClient(string ipAddress, string baseName, string user, string pass) {
			Logging.ToLog("Создание подключения к базе FB: " + 
				ipAddress + ":" + baseName);

			FbConnectionStringBuilder cs = new FbConnectionStringBuilder();

			try {
				cs.DataSource = ipAddress;
				cs.Database = baseName;
				cs.UserID = user;
				cs.Password = pass;
				cs.Charset = "NONE";
				cs.Pooling = false;

				connection = new FbConnection(cs.ToString());
			} catch (Exception e) {
				Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
			}
		}

		public DataTable GetDataTable(string query, Dictionary<string, object> parameters = null) {
			DataTable dataTable = new DataTable();

			try {
				connection.Open();
				FbCommand command = new FbCommand(query, connection);

				if (parameters != null && parameters.Count > 0)
					foreach (KeyValuePair<string, object> parameter in parameters)
						command.Parameters.AddWithValue(parameter.Key, parameter.Value);

				FbDataAdapter fbDataAdapter = new FbDataAdapter(command);
				fbDataAdapter.Fill(dataTable);
			} catch (Exception e) {
				Logging.ToLog("GetDataTable exception: " + query + 
					Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
			} finally {
				connection.Close();
			}

			return dataTable;
		}

		public bool ExecuteUpdateQuery(string query, Dictionary<string, object> parameters) {
			bool updated = false;

			try {
				connection.Open();
				FbCommand update = new FbCommand(query, connection);

				if (parameters.Count > 0) 
					foreach (KeyValuePair<string, object> parameter in parameters)
						update.Parameters.AddWithValue(parameter.Key, parameter.Value);
				

				updated = update.ExecuteNonQuery() > 0 ? true : false;
			} catch (Exception e) {
				Logging.ToLog("ExecuteUpdateQuery exception: " + query +
					Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
			} finally {
				connection.Close();
			}

			return updated;
		}
	}
}
