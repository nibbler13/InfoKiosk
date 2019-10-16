using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoKiosk {
	public class ItemService : IComparable {
		public string Schid { get; private set; }
		public string Kodoper { get; private set; }
		public string Name { get; private set; }
		public double Cost { get; private set; }

		private string priority;
		public string Priority {
			get { return priority; }
			set { 
				if (value != priority) {
					if (!string.IsNullOrEmpty(value) &&
						!int.TryParse(value, out _))
						return;

					priority = value; 
					HasChanged = true;
				} 
			} 
		}

		public double PriorityToSort {
			get {
				return double.TryParse(Priority, out double priority) ? priority : 99999;
			}
		}

		public string PriorityToSortInTerminal {
			get {
				string priorityString = PriorityToSort.ToString();
				while (priorityString.Length < 5)
					priorityString = "0" + priorityString;

				return priorityString + "-" + Name;
			}
		}

		private string originalPriority;

		public void RestorePriority() {
			Priority = originalPriority;
			HasChanged = false;
		}

		public void ConfirmSave() {
			originalPriority = Priority;
			HasChanged = false;
		}

		public int CompareTo(object obj) {
			if (obj == null)
				return 0;

			return PriorityToSortInTerminal.CompareTo(((ItemService)obj).PriorityToSortInTerminal);
		}

		public bool HasChanged { get; private set; } = false;

		public ItemService(string name, int cost, string schid, string kodoper, string priority) {
			Name = name;
			Cost = cost;
			Kodoper = kodoper;
			this.priority = priority;
			originalPriority = priority;
			Schid = schid;
		}
	}
}
