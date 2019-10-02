using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InfoKioskServicesManager.Pages {
	/// <summary>
	/// Interaction logic for PageSelectSpeciality.xaml
	/// </summary>
	public partial class PageSelectSpeciality : Page {
		public PageSelectSpeciality() {
			InitializeComponent();

			foreach (KeyValuePair<string, List<InfoKiosk.ItemService>> item in InfoKiosk.Services.DataProvider.Services) 
				ListViewSpeciality.Items.Add(item.Key);
		}

		private void ListViewSpeciality_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ButtonNext.IsEnabled = ListViewSpeciality.SelectedItems.Count > 0;
		}

		private void ButtonNext_Click(object sender, RoutedEventArgs e) {
			NavigationService.Navigate(new PageServicesView(ListViewSpeciality.SelectedItem as string));
		}

		private void ListViewSpeciality_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
			ButtonNext_Click(null, null);
		}
	}
}
