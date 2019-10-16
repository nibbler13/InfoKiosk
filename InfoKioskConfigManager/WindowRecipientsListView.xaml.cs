using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace InfoKioskConfigManager {
	/// <summary>
	/// Interaction logic for WindowRecipientsListView.xaml
	/// </summary>
	public partial class WindowRecipientsListView : Window {
		public enum Type {
			Admin,
			Doctor,
			Clinic,
			Registry
		}

		public ObservableCollection<MailAddress> Addresses { get; set; } = new ObservableCollection<MailAddress>();

		public WindowRecipientsListView(Type type, string addresses) {
			InitializeComponent();

			string[] splitted = InfoKiosk.Services.Config.GetSplittedAddresses(addresses);
			foreach (string address in splitted)
				Addresses.Add(new MailAddress(address));

			DataGridAddresses.DataContext = this;

			Closed += (s, e) => {
				List<MailAddress> emptyOrWrong = new List<MailAddress>();
				foreach (MailAddress item in Addresses) {
					if (string.IsNullOrEmpty(item.Address)) {
						emptyOrWrong.Add(item);
						continue;
					}

					try {
						System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(item.Address);
					} catch (Exception) {
						emptyOrWrong.Add(item);
					}
				}

				foreach (MailAddress item in emptyOrWrong)
					Addresses.Remove(item);

				string addressesEdited = string.Join(" | ", Addresses);

				switch (type) {
					case Type.Admin:
						InfoKiosk.Services.Config.Instance.MailAdminAddress = addressesEdited;
						break;
					case Type.Doctor:
						InfoKiosk.Services.Config.Instance.MailRecipientsNegativeMarksDoctor = addressesEdited;
						break;
					case Type.Clinic:
						InfoKiosk.Services.Config.Instance.MailRecipientsNegativeMarksClinic = addressesEdited;
						break;
					case Type.Registry:
						InfoKiosk.Services.Config.Instance.MailRecipientsNegativeMarksRegistry = addressesEdited;
						break;
					default:
						break;
				}
			};
		}

#pragma warning disable CA1034 // Nested types should not be visible
		public class MailAddress {
#pragma warning restore CA1034 // Nested types should not be visible
			public string Address { get; set; }
			public MailAddress(string address) {
				Address = address;
			}

			override
			public string ToString() {
				return Address;
			}
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e) {
			Addresses.Add(new MailAddress(string.Empty));

			DataGridAddresses.SelectedIndex = Addresses.Count - 1;
			DataGridAddresses.Focus();
		}

		private void DataGridAddresses_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ButtonRemove.IsEnabled = DataGridAddresses.SelectedItems.Count > 0;
		}

		private void ButtonRemove_Click(object sender, RoutedEventArgs e) {
			List<MailAddress> mailAddresses = new List<MailAddress>();
			foreach (MailAddress mailAddress in DataGridAddresses.SelectedItems)
				mailAddresses.Add(mailAddress);

			foreach (MailAddress mailAddressToRemove in mailAddresses)
				Addresses.Remove(mailAddressToRemove);
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
