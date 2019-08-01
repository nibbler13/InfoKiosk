using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoKiosk {
	class Services {
		private static Services instance = null;
		private static readonly object padlock = new object();

		public static Services Instance {
			get {
				lock (padlock) {
					if (instance == null)
						instance = new Services();

					return instance;
				}
			}
		}

		public Dictionary<string, List<ItemService>> ServiceDict { get; private set; }

		private Services() {
			ServiceDict = new Dictionary<string, List<ItemService>>();
			DataTable dataTable = ExcelReader.ReadExcelFile("https---clinicalcenter.ru-uslugi-_20190722_185002.xlsx", "Data");

			foreach (DataRow dataRow in dataTable.Rows) {
				if (!int.TryParse(dataRow[3].ToString(), out int cost))
					continue;

				string department = dataRow[0].ToString();
				string name = dataRow[2].ToString();

				if (!ServiceDict.ContainsKey(department))
					ServiceDict.Add(department, new List<ItemService>());

				ServiceDict[department].Add(new ItemService(name, cost));
			}
		}
	}
}
