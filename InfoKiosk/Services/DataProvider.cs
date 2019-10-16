using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfoKiosk.Services {
    public static class DataProvider {
		public static SortedDictionary<string, List<ItemDoctor>> Survey { get; private set; } =
			new SortedDictionary<string, List<ItemDoctor>>();
		public static SortedDictionary<string, SortedDictionary<string, SortedDictionary<string, string>>> Schedule { get; private set; } =
			new SortedDictionary<string, SortedDictionary<string, SortedDictionary<string, string>>>();
		public static SortedDictionary<string, List<ItemService>> Services { get; private set; } =
			new SortedDictionary<string, List<ItemService>>();

		public async static void LoadData() {
			Logging.ToLog("DataProvider - Запрос данных из БД");

			await Task.Run(() => {
				for(int i = 1; i <= 10; i++) {
					try {
						Logging.ToLog("Попытка: " + i);

						using (FirebirdClient firebirdClient = new FirebirdClient(
							Config.Instance.MisDbAddress,
							Config.Instance.MisDbPort,
							Config.Instance.MisDbName,
							Config.Instance.MisDbUserName,
							Config.Instance.MisDbUserPassword)) {
							LoadDepartmentsAndDoctors(firebirdClient);
							LoadSchedule(firebirdClient);
							LoadServices(firebirdClient);
						};
					} catch (Exception e) {
						Logging.ToLog(e.Message + Environment.NewLine + e.StackTrace);
						Logging.ToLog("Повторная попытка получения данных через: 60 секунд");
						Thread.Sleep(60 * 1000);
					}
				}
			}).ConfigureAwait(false);
		}

		public static bool LoadServicesInGui(out string errorMessage) {
			try {
				using (FirebirdClient firebirdClient = new FirebirdClient(
					Config.Instance.MisDbAddress,
					Config.Instance.MisDbPort,
					Config.Instance.MisDbName,
					Config.Instance.MisDbUserName,
					Config.Instance.MisDbUserPassword,
					true)) {
					LoadServices(firebirdClient, true);
				}

				errorMessage = string.Empty;
				return true;
			} catch (Exception e) {
				errorMessage = e.Message;
				return false;
			}
		}

		private static void LoadDepartmentsAndDoctors(FirebirdClient firebirdClient) {
			Logging.ToLog("DataProvider - Получение данных для оценки врачей");
			using (DataTable dataTableSurvey = firebirdClient.GetDataTable(InfoKiosk.Services.Config.Instance.SqlGetSurveyInfo)) {
				Logging.ToLog("DataProvider - Получено строк: " + dataTableSurvey.Rows.Count);
				foreach (DataRow dataRow in dataTableSurvey.Rows) {
					string dept = ControlsFactory.FirstCharToUpper(dataRow["DEPARTMENT"].ToString(), true);
					if (!Survey.ContainsKey(dept))
						Survey.Add(dept, new List<ItemDoctor>());

					string docname = dataRow["DOCNAME"].ToString();
					string depNum = dataRow["DEPNUM"].ToString();
					string dCode = dataRow["DCODE"].ToString();
					string docPosition = dataRow["DOCPOSITION"].ToString();

					ItemDoctor doc = new ItemDoctor(docname, docPosition, dept, dCode, depNum);
#pragma warning disable CA1307 // Specify StringComparison
					if (Survey[dept].FindAll(x => x.Code.Equals(dCode)).Count == 0)
#pragma warning restore CA1307 // Specify StringComparison
						Survey[dept].Add(doc);
				}
			}
		}

		private static void LoadSchedule(FirebirdClient firebirdClient) {
			Logging.ToLog("DataProvider - Получение расписания врачей");
			using (DataTable dataTableSchedule = firebirdClient.GetDataTable(InfoKiosk.Services.Config.Instance.SqlGetScheduleInfo)) {
				Logging.ToLog("DataProvider - Получено строк: " + dataTableSchedule.Rows.Count);
				foreach (DataRow dataRow in dataTableSchedule.Rows) {
					string depname = ControlsFactory.FirstCharToUpper(dataRow["DEPNAME"].ToString(), true);

					if (!Schedule.ContainsKey(depname))
						Schedule.Add(depname, new SortedDictionary<string, SortedDictionary<string, string>>());

					string doctor = dataRow["FULLNAME"].ToString();

					if (!Schedule[depname].ContainsKey(doctor))
						Schedule[depname].Add(doctor, new SortedDictionary<string, string>());

					for (int i = 0; i < 7; i++)
						Schedule[depname][doctor].Add("D" + i, dataRow["D" + i].ToString());
				}
			}
		}

		private static void LoadServices(FirebirdClient firebirdClient, bool isGui = false) {
			Logging.ToLog("DataProvider - Получение информации о ценах и услугах");
			using (DataTable dataTablePrice = firebirdClient.GetDataTable(
				InfoKiosk.Services.Config.Instance.SqlGetPriceInfo,
				new Dictionary<string, object> { { "@filialID", InfoKiosk.Services.Config.Instance.SqlGetPriceInfoFilialID } },
				isGui)) {
				Logging.ToLog("DataProvider - Получено строк: " + dataTablePrice.Rows.Count);
				Services = new SortedDictionary<string, List<ItemService>>();

				foreach (DataRow dataRow in dataTablePrice.Rows) {
					string cost = dataRow["COST"].ToString();
					if (string.IsNullOrEmpty(cost) ||
						!int.TryParse(cost, out int costValue))
						continue;

					string group = ControlsFactory.FirstCharToUpper(dataRow["GROUP"].ToString(), true);
					if (!Services.ContainsKey(group))
						Services.Add(group, new List<ItemService>());

					string serviceName = dataRow["SERVICE_NAME"].ToString();
					string serviceKodoper = dataRow["SERVICE_CODE"].ToString();
					string serviceSchid = dataRow["SCHID"].ToString();
					string priority = dataRow["PRIORITY"].ToString();

					Services[group].Add(new ItemService(serviceName, costValue, serviceSchid, serviceKodoper, priority));
				}
			}

			foreach (string key in Services.Keys)
				Services[key].Sort();
		}

		public static bool WriteItemServiceToDb(string department, out string message) {
			if (!Services.ContainsKey(department)) {
				message = "Не удается найти отделение: " + department;
				return false;
			}

			int i = 0;
			try {
				using (FirebirdClient firebirdClient = new FirebirdClient(
					Config.Instance.MisDbAddress,
					Config.Instance.MisDbPort,
					Config.Instance.MisDbName,
					Config.Instance.MisDbUserName,
					Config.Instance.MisDbUserPassword)) {
					foreach (ItemService item in Services[department]) {
						if (!item.HasChanged)
							continue;

						object priorityValue = null;
						if (int.TryParse(item.Priority, out int priority))
							priorityValue = priority;

						bool result = firebirdClient.ExecuteUpdateQuery(
							Config.Instance.SqlInsertServicePriority, 
							new Dictionary<string, object> {
								{ "@schid", item.Schid },
								{ "@priority", priorityValue }
							}, 
							true);

						if (result) {
							item.ConfirmSave();
							i++;
						}
					}
				}
			} catch (Exception e) {
				message = e.Message;
				return false;
			}

			message = "Записано в БД строк: " + i;
			return true;
		}
	}
}
