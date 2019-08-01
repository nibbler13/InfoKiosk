using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoKiosk {
	public class ItemService {
		public string Name { get; private set; }
		public int Cost { get; private set; }

		public ItemService(string name, int cost) {
			Name = name;
			Cost = cost;
		}
	}
}
