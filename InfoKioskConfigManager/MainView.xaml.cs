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

namespace InfoKioskConfigManager {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			DataContext = InfoKiosk.Services.Config.Instance;
			Closing += MainWindow_Closing;
			Loaded += (s, e) => {
				if (!InfoKiosk.Services.Config.Instance.IsConfigReadedSuccessfull)
					MessageBox.Show(this, "Не удалось считать файл конфигурации: " +
						InfoKiosk.Services.Config.Instance.ConfigFilePath + Environment.NewLine +
						"Создана новая конфигурация, заполненная стандартными значениями", "Ошибка конфигурации", MessageBoxButton.OK, MessageBoxImage.Information);
			};
		}

		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (!InfoKiosk.Services.Config.Instance.IsNeedToSave)
				return;

			MessageBoxResult result = MessageBox.Show(
				this, "Имеются несохраненные изменения, хотите сохранить их перед выходом?", "Сохранение изменений",
				MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
				InfoKiosk.Services.Config.SaveConfiguration();
			else if (result == MessageBoxResult.Cancel)
				e.Cancel = true;
		}

		private void ButtonSqlQuery_Click(object sender, RoutedEventArgs e) {
			try {
				string buttonTag = (sender as Button).Tag as string;
				string buttonContent = (sender as Button).Content as string;
				string query;
				string filialId = string.Empty;
				WindowSqlQueryView.Type type;

				if (buttonTag.Equals("ButtonSqlRateDoctor")) {
					type = WindowSqlQueryView.Type.DoctorRate;
					query = InfoKiosk.Services.Config.Instance.SqlGetSurveyInfo;
				} else if (buttonTag.Equals("ButtonSqlDoctorSchedule")) {
					type = WindowSqlQueryView.Type.Schedule;
					query = InfoKiosk.Services.Config.Instance.SqlGetScheduleInfo;
				} else if (buttonTag.Equals("ButtonSqlServices")) {
					type = WindowSqlQueryView.Type.Services;
					query = InfoKiosk.Services.Config.Instance.SqlGetPriceInfo;
					filialId = InfoKiosk.Services.Config.Instance.SqlGetPriceInfoFilialID;
				} else if (buttonTag.Equals("ButtonSqlInsertSurveyResult")) {
					type = WindowSqlQueryView.Type.InsertSurveyResult;
					query = InfoKiosk.Services.Config.Instance.SqlInsertLoyaltySurveyResult;
				} else if (buttonTag.Equals("ButtonSqlInsertServicePriority")) {
					type = WindowSqlQueryView.Type.InsertServicePriority;
					query = InfoKiosk.Services.Config.Instance.SqlInsertServicePriority;
				} else
					return;

				WindowSqlQueryView windowSqlQueryView = new WindowSqlQueryView(type, query, filialId) {
					Owner = this,
					Title = buttonContent
				};
				windowSqlQueryView.ShowDialog();
			} catch (Exception exc) {
				MessageBox.Show(this, exc.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ButtonEditRecipients(object sender, RoutedEventArgs e) {
			try {
				string buttonTag = (sender as Button).Tag as string;
				WindowRecipientsListView.Type type;

				string addresses;
				if (buttonTag.Equals("Получатели системных уведомлений:")) {
					type = WindowRecipientsListView.Type.Admin;
					addresses = InfoKiosk.Services.Config.Instance.MailAdminAddress;
				} else if (buttonTag.Equals("Получатели негативных оценок про врачей:")) {
					type = WindowRecipientsListView.Type.Doctor;
					addresses = InfoKiosk.Services.Config.Instance.MailRecipientsNegativeMarksDoctor;
				} else if (buttonTag.Equals("Получатели негативных оценок про клинику:")) {
					type = WindowRecipientsListView.Type.Clinic;
					addresses = InfoKiosk.Services.Config.Instance.MailRecipientsNegativeMarksClinic;
				} else if (buttonTag.Equals("Получатели негативных оценок про регистратуру:")) {
					type = WindowRecipientsListView.Type.Registry;
					addresses = InfoKiosk.Services.Config.Instance.MailRecipientsNegativeMarksRegistry;
				} else
					return;

				WindowRecipientsListView windowRecipientsListView = new WindowRecipientsListView(type, addresses) {
					Owner = this,
					Title = buttonTag
				};
				windowRecipientsListView.ShowDialog();
			} catch (Exception exc) {
				MessageBox.Show(this, exc.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
