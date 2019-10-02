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

namespace InfoKioskServicesManager {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			Loaded += MainWindow_Loaded;
		}

		private async void MainWindow_Loaded(object sender, RoutedEventArgs e) {
			InfoKiosk.Services.Configuration configuration = InfoKiosk.Services.Configuration.Instance;
			if (!configuration.IsConfigReadedSuccessfull) {
				TextBlockMessage.Text = "Не удалось считать конфигурацию." + Environment.NewLine +
					"Убедитесь, что существует файл: " + configuration.ConfigFilePath + Environment.NewLine +
					Environment.NewLine + "Если это первый запуск ПО InfoKiosk, то воспользуйтесь сначала утилитой для управления настройками: InfoKioskConfigManager.exe";
				return;
			}

			TextBlockMessage.Text = "Получение данных из БД \"МИС Инфоклиника\"";

			bool result = false;
			string errorMessage = string.Empty;

			await Task.Run(() => {
				result = result = InfoKiosk.Services.DataProvider.LoadServicesInGui(out errorMessage);
			});

			if (!result) {
				TextBlockMessage.Text = "Не удалось получить список услуг." + Environment.NewLine +
					Environment.NewLine + " Текст ошибки:" + Environment.NewLine + errorMessage;
				return;
			};

			TextBlockMessage.Visibility = Visibility.Hidden;
			FrameMain.Visibility = Visibility.Visible;
			FrameMain.Navigate(new Pages.PageSelectSpeciality());
		}
	}
}
